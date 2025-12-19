Public Class wfExchangeRepairOverhaulOrderRecordUpdate_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mExchangeRepairOverhaulOrderRecordsList As ExchangeRepairOverhaulOrderRecordsList
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim PartNo As String
    Dim ReceiptText, SearchIndex, ReceiptNo, mItemName, mReceiptItemSerialNo As String
    Public mOrderItemID As Guid
    Public mReceiptItemID As Guid
    Dim EventLogID As Guid
    'Added By Vikrant On 19-Dec-2018 For ALL19122018
    Private IsAttachmentAdded As Boolean = False
    'End
#End Region

#Region " Helper Methods "
    Private Sub GetSessionForLocation()
        mOrderItemID = Session("mOrderItemID")
        mReceiptItemID = Session("mReceiptItemID")
        mItemName = Session("mItemName")
        mReceiptItemSerialNo = Session("mReceiptItemSerialNo")
        mDistinctTextListForReceipt = CType(Session("mDistinctTextListForReceipt"), DistinctTextListForReceipt)
        'Added By Vikrant On 19-Dec-2018 For ALL19122018
        IsAttachmentAdded = Session("IsAttachmentAdded")
        'End
    End Sub
    Private Sub RemoveSessionForPartStore()
        Session.Remove("mItemTypeList")
        Session.Remove("ChangeItemTypeID")
        Session.Remove("ChangeItemTypeName")
        Session.Remove("ChangeStore")
        Session.Remove("IsStoreChangeble")
        Session.Remove("ChangeStoreID")
        Session.Remove("ChangeStoreList")
    End Sub
    Private Sub RemoveSessionForLocation()
        Session.Remove("mOrderItemID")
        Session.Remove("mReceiptItemID")
        Session.Remove("mItemName")
        Session.Remove("mReceiptItemSerialNo")
        
    End Sub
    Private Sub GetSession()
        mReceiptItemID = CType(Session("mReceiptItemID"), Guid)
        mExchangeRepairOverhaulOrderRecordsList = CType(Session("mExchangeRepairOverhaulOrderRecordsList"), ExchangeRepairOverhaulOrderRecordsList)
        PartNo = IIf(IsNothing(Session("PartNo")), "", Session("PartNo"))
        ReceiptText = IIf(IsNothing(Session("ReceiptText")), "", Session("ReceiptText"))
        ReceiptNo = IIf(IsNothing(Session("ReceiptNo")), "", Session("ReceiptNo"))
        SearchIndex = IIf(IsNothing(Session("SearchIndex")), "", Session("SearchIndex"))
     End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("ReceiptText")
        Session.Remove("ReceiptNo")
        Session.Remove("SearchIndex")
        Session.Remove("mExchangeRepairOverhaulOrderRecordsList")
        Session.Remove("mDistinctTextListForReceipt")
        'Added By Vikrant On 19-Dec-2018 For ALL19122018
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentAdded")
        'End
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfExchangeRepairOverhaulOrderRecordUpdate_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub UpdateChange(ByVal mReceiptItemID As Guid, ByVal mOrderItemID As Guid, ByVal mItemName As String, ByVal mReceiptItemSerialNo As String, ByVal IsAttachmentAdded As Boolean)
        Session("mReceiptItemID") = mReceiptItemID
        Session("mOrderItemID") = mOrderItemID
        Session("mItemName") = mItemName
        Session("mReceiptItemSerialNo") = mReceiptItemSerialNo
        'Added By Vikrant On 19-Dec-2018 For ALL19122018
        Session("mFileAttach") = Nothing
        Session("IsAttachmentAdded") = IsAttachmentAdded
        Session("IsLoanTransaction") = chkShowLoanTransactions.Checked
        'End
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility1(ByVal SearchIndex As Int32)
        If SearchIndex = 0 Then
            lblFor.Visible = False
            txtSearchFor.Visible = False
            cmbReceiptText.Visible = False
            cmbReceiptText.SelectedIndex = 0
            txtReceiptNo.Visible = False
            txtReceiptNo.Text = ""
        ElseIf SearchIndex = 1 Then
            lblFor.Visible = True
            txtSearchFor.Visible = True
            txtSearchFor.Text = PartNo
            cmbReceiptText.Visible = False
            cmbReceiptText.SelectedIndex = 0
            txtReceiptNo.Visible = False
            txtReceiptNo.Text = ""
        ElseIf SearchIndex = 2 Then
            lblFor.Visible = False
            txtSearchFor.Visible = False
            cmbReceiptText.Visible = True
            txtReceiptNo.Visible = True
        End If
    End Sub
    Private Sub ClearControls()
        txtSearchFor.Text = ""
        txtReceiptNo.Text = ""
    End Sub
    Private Sub ResetValues()
        PartNo = ""
        ReceiptText = ""
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal ItemName As String = "")
        gdPartSearch.DataSource = Nothing
        mExchangeRepairOverhaulOrderRecordsList = Nothing
        mExchangeRepairOverhaulOrderRecordsList = ExchangeRepairOverhaulOrderRecordsList.GetExchangeRepairOverhaulOrderRecordsList(Text, No, ItemName, chkShowLoanTransactions.Checked)
        gdPartSearch.DataSource = mExchangeRepairOverhaulOrderRecordsList
        Session("mExchangeRepairOverhaulOrderRecordsList") = mExchangeRepairOverhaulOrderRecordsList
        PartSearchGridBind()
    End Sub
    Public Sub SetControl()
        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        ReceiptText = Session("ReceiptText")
        ReceiptNo = Session("ReceiptNo")
        FindNow(ReceiptText, CInt(Val(ReceiptNo)), PartNo)
        PartSearchGridBind()
        cmbSearch.SelectedIndex = SearchIndex
        ControlVisibility1(SearchIndex)
        lblResult.Text = "List of Receipts : " & mExchangeRepairOverhaulOrderRecordsList.Count & " Record(s) found. "
    End Sub
    Private Sub PartSearchGridBind()
        gdPartSearch.DataBind()
        upnlgrid.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("13", , True, "(All)")
        cmbReceiptText.DataSource = mDistinctTextListForReceipt
        cmbReceiptText.DataBind()
        mExchangeRepairOverhaulOrderRecordsList = ExchangeRepairOverhaulOrderRecordsList.GetExchangeRepairOverhaulOrderRecordsList()
        gdPartSearch.DataSource = mExchangeRepairOverhaulOrderRecordsList
        Session("mExchangeRepairOverhaulOrderRecordsList") = mExchangeRepairOverhaulOrderRecordsList
        SearchIndex = Session("SearchIndex")
        PartNo = Session("PartNo")
        ReceiptText = Session("ReceiptText")
        ReceiptNo = Session("ReceiptNo")
        lblResult.Text = "List of Receipts : " & mExchangeRepairOverhaulOrderRecordsList.Count & " Record(s) found "
        btnClose1.DataBind()
        PartSearchGridBind()
        upnlSearch.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfExchangeRepairOverhaulOrderRecordUpdate_Ajax.aspx?"
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            DataFieldBind()
        End If
    End Sub
    Protected Sub gdPartSearch_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gdPartSearch.RowCommand
        Select Case e.CommandName
            Case "UpdateRec"
                Dim index As Integer = CInt(e.CommandArgument) + gdPartSearch.PageIndex * gdPartSearch.PageSize
                Dim mReceiptItemID As Guid = (mExchangeRepairOverhaulOrderRecordsList(index).ReceiptItemID)
                Dim mOrderItemID As Guid = (mExchangeRepairOverhaulOrderRecordsList(index).OrderItemID)
                Dim mItemName As String = (mExchangeRepairOverhaulOrderRecordsList(index).ItemName)
                Dim mReceiptItemSerialNo As String = (mExchangeRepairOverhaulOrderRecordsList(index).ReceiptItemSerialNo)
                UpdateChange(mReceiptItemID, mOrderItemID, mItemName, mReceiptItemSerialNo, mExchangeRepairOverhaulOrderRecordsList(index).IsAttachmentAdded)
                MarkLog(Util.Action.Edit, "ExchangeRepairOverhaulOrderRecordUpdate", "Part : " + mExchangeRepairOverhaulOrderRecordsList(index).ItemName + " Receipt : " + mExchangeRepairOverhaulOrderRecordsList(index).ReceiptNumber + " Receipt Date : " + mExchangeRepairOverhaulOrderRecordsList(index).ReceiptDateFormatted.ToString + " Supplier : " + mExchangeRepairOverhaulOrderRecordsList(index).VendorName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenGROOutrightConversionWindow", "OpenGROOutrightConversionWindow();", True)
        End Select
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        ClearControls()
        ControlVisibility1(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        gdPartSearch.PageIndex = 0
        SearchIndex = cmbSearch.SelectedIndex
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        ReceiptNo = IIf(cmbSearch.SelectedIndex = 2, Trim(txtReceiptNo.Text), "")

        Session("SearchIndex") = SearchIndex
        Session("PartNo") = PartNo
        Session("ReceiptText") = ReceiptText
        Session("ReceiptNo") = ReceiptNo

        FindNow(IIf(cmbReceiptText.SelectedIndex > 0, cmbReceiptText.SelectedItem.Text, ""), CInt(Val(ReceiptNo)), PartNo)
        lblResult.Text = "List of Receipts : " & mExchangeRepairOverhaulOrderRecordsList.Count & " Record(s) found "
        ControlVisibility1(cmbSearch.SelectedIndex)
        PartSearchGridBind()
    End Sub
   Private Sub cmbReceiptText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbReceiptText.SelectedIndexChanged
        ClearControls()
        If cmbReceiptText.SelectedIndex > 0 Then
            txtReceiptNo.Visible = True
        Else
            txtReceiptNo.Visible = False
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose1.Click, btnClose.Click
        MarkLog(Util.Action.Close, "ExchangeRepairOverhaulOrderRecordUpdate", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mExchangeRepairOverhaulOrderRecordsList = Nothing
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub gdPartSearch_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdPartSearch.Sorting
        mExchangeRepairOverhaulOrderRecordsList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mExchangeRepairOverhaulOrderRecordsList") = mExchangeRepairOverhaulOrderRecordsList
        gdPartSearch.DataSource = mExchangeRepairOverhaulOrderRecordsList
        PartSearchGridBind()
    End Sub
    Protected Sub gdPartSearch_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdPartSearch.PageIndexChanging
        gdPartSearch.PageIndex = e.NewPageIndex
        gdPartSearch.DataSource = mExchangeRepairOverhaulOrderRecordsList
        Session("mExchangeRepairOverhaulOrderRecordsList") = mExchangeRepairOverhaulOrderRecordsList
        PartSearchGridBind()
    End Sub
    Private Sub hdnimgBtnGROOutrightConversion_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnGROOutrightConversion.Click
        If Session("IsRecordSaved") = "True" Then
            chkShowLoanTransactions.Checked = False
            DataFieldBind()
            upnlClose.Update()
        End If
      
    End Sub
#End Region

#Region "Change ReceiptText"
    Private Sub chkShowLoanTransactions_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowLoanTransactions.CheckedChanged
        gdPartSearch.PageIndex = 0
        SearchIndex = cmbSearch.SelectedIndex
        PartNo = IIf(cmbSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
        ReceiptNo = IIf(cmbSearch.SelectedIndex = 2, Trim(txtReceiptNo.Text), "")

        Session("SearchIndex") = SearchIndex
        Session("PartNo") = PartNo
        Session("ReceiptText") = ReceiptText
        Session("ReceiptNo") = ReceiptNo

        FindNow(IIf(cmbReceiptText.SelectedIndex > 0, cmbReceiptText.SelectedItem.Text, ""), CInt(Val(ReceiptNo)), PartNo)
        lblResult.Text = "List of Receipts :" & mExchangeRepairOverhaulOrderRecordsList.Count & " Record(s) found "
        ControlVisibility1(cmbSearch.SelectedIndex)
        PartSearchGridBind()
    End Sub
   
#End Region

    
   
End Class