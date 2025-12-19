Imports System.Linq
Public Class wfPartStatus
    Inherits System.Web.UI.Page

#Region "Variables"
    Private mInvoiceItemListForFinanceApproval As InvoiceItemListForFinanceApproval
    Private mListOfQuotationForComparison As ListOfQuotationForComparison
    Dim mAlternatePartNumbers As AlternatePartNumbers
    Dim mListOfRequisition As ListOfRequisition
    Dim mItemStockStatusList As ItemStockStatusList
    Dim mOrder As Order
    Dim mOpenFrom As String
    Dim mItem As Item
#End Region

#Region "Methods"
    Private Sub GetSession()
        mOrder = Session("mOrderForPartStatus")
        mOpenFrom = Session("FromDashboardForInventoryOrOrder")
    End Sub
    Private Sub ItemStockStatusListMethod(Optional ByVal PartNo As String = "")
        mItemStockStatusList = ItemStockStatusList.GetItemStockStatusList(PartNo, Today.Date.ToString)
        lblPartNumber.Text = mItemStockStatusList(PartNo).ItemName
        lblPartDescription.Text = mItemStockStatusList(PartNo).ItemDescription
        lblPartStockQty.Text = mItemStockStatusList(PartNo).StockQTY
        lblPartServiceableStockQty.Text = mItemStockStatusList(PartNo).ServiceablePartStockBalanceQty
        lblPartUnserviceableStockQty.Text = mItemStockStatusList(PartNo).UnServiceablePartStockBalanceQty
        lblPartOnOrderQty.Text = mItemStockStatusList(PartNo).PendingQTY
        If mOpenFrom = "FromDashboardForInventory" Then
            fdsAlternateParts.Visible = True
            mItem = Item.GetItem(mItemStockStatusList(PartNo).ItemID)
            mAlternatePartNumbers = mItem.AlternatePartNos
            gdvAlternate.DataSource = mAlternatePartNumbers
            gdvAlternate.DataBind()
            lblAlternateParts.Text = mItem.AlternatePartNos.Count.ToString + " Alternate Parts : "
            upnlAlternateParts.Update()
        Else
            If (mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7) Then  '2 Against Quotations , 7 Against Enqiry selection is for Quotations
                fdsAlternateParts.Visible = False
            Else
                fdsAlternateParts.Visible = True
                mItem = Item.GetItem(mItemStockStatusList(PartNo).ItemID)
                mAlternatePartNumbers = mItem.AlternatePartNos
                gdvAlternate.DataSource = mAlternatePartNumbers
                gdvAlternate.DataBind()
                lblAlternateParts.Text = mItem.AlternatePartNos.Count.ToString + " Alternate Parts : "
                upnlAlternateParts.Update()
            End If
        End If
    End Sub
    Private Sub InvoiceItemListForFinanceApprovalMethod(Optional ByVal PartNo As String = "")
        mInvoiceItemListForFinanceApproval = InvoiceItemListForFinanceApproval.GetInvoiceItemListForFinalApprovalList(mItemStockStatusList(PartNo).ItemID)
        If mInvoiceItemListForFinanceApproval.Count > 0 Then
            Dim PoInfo = (From Info As InvoiceItemListForFinanceApproval.InvoiceItemListForFinanceApprovalInfo In mInvoiceItemListForFinanceApproval
                                              Where Info.OrderTranstypeID = 5
                                              Select Info).ToList.Take(3)

            fdsLast3Purchasesdetails.Visible = True
            dgPurchaseOrderInfo.DataSource = PoInfo
            dgPurchaseOrderInfo.DataBind()
        Else
            fdsLast3Purchasesdetails.Visible = False
        End If
    End Sub
    Private Sub ReqDetailInformationMethod(Optional ByVal PartNo As String = "")
        mListOfRequisition = ListOfRequisition.GetRequisitionList(ItemName:=PartNo)
        If mListOfRequisition.Count > 0 Then
            fdsLast3Requisitiondetails.Visible = True
            dgRequisitionInfo.DataSource = mListOfRequisition
            dgRequisitionInfo.DataBind()
        Else
            fdsLast3Requisitiondetails.Visible = False
        End If
    End Sub
    Private Sub ListOfQuotationForComparisonMethod(Optional ByVal PartNo As String = "")
        If (mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7) Then  '2 Against Quotations , 7 Against Enqiry selection is for Quotations
            mListOfQuotationForComparison = ListOfQuotationForComparison.GetListOfQuotationForComparison(PartNo, Today.Date.ToString, _
                                                                                                         QuotationItemID:=mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.CurrentItem.QuotationItemID.ToString)
            If mListOfQuotationForComparison.Count > 0 Then
                'Dim QuotationInfo = (From Info As ListOfQuotationForComparison.ListOfQuotationForComparisonInfo In mListOfQuotationForComparison
                '              Select Info).ToList.Take(3)

                fdsLast3QuotationCompared.Visible = True
                dgQuotationList.DataSource = mListOfQuotationForComparison
                dgQuotationList.DataBind()
            Else
                fdsLast3QuotationCompared.Visible = False
            End If
        Else
            fdsLast3QuotationCompared.Visible = False
            upnlQuotationList.Update()
        End If
    End Sub
#End Region

#Region "Function"

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            mOpenFrom = Request.QueryString("Type")
            Session("FromDashboardForInventoryOrOrder") = mOpenFrom
            ItemStockStatusListMethod(Session("FromPOItemName"))
            InvoiceItemListForFinanceApprovalMethod(Session("FromPOItemName"))
            ReqDetailInformationMethod(Session("FromPOItemName"))
            If mOpenFrom = "FromDashboardForInventory" Then
                'do nothing
            Else
                If (mOrder.AgainstTypeID = 2 Or mOrder.AgainstTypeID = 7) Then  '2 Against Quotations , 7 Against Enqiry selection is for Quotations
                    ListOfQuotationForComparisonMethod(Session("FromPOItemName"))
                    fdsAlternateParts.Visible = False
                End If
            End If
        End If
    End Sub
    Private Sub gdvAlternate_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvAlternate.RowCommand
        Select Case e.CommandName
            Case "PartStatus"
                Dim index As Integer = CInt(e.CommandArgument) + gdvAlternate.PageIndex * gdvAlternate.PageSize
                Dim mPartNo As String = CType(gdvAlternate.DataKeys(CInt(e.CommandArgument)).Values("PartName"), String)
                ItemStockStatusListMethod(mPartNo)
                InvoiceItemListForFinanceApprovalMethod(mPartNo)
                ReqDetailInformationMethod(mPartNo)
                If mOpenFrom = "FromDashboardForInventory" Then
                    'Do nothing
                Else
                    ListOfQuotationForComparisonMethod(mPartNo)
                End If
                upnlPartInfo.Update()
                upnlPurchaseOrderInfo.Update()
                upnlRequisitionInfo.Update()
                upnlQuotationList.Update()
                upnlAlternateParts.Update()
        End Select
    End Sub
    Private Sub btnCloseBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseBottom.Click
        mOpenFrom = Request.QueryString("Type")
        If Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromPurchaseOrder" Or mOpenFrom = "FromDashboardForInventory") Then
            Session.Remove("mOrderForPartStatus")
            Session.Remove("FromDashboardForInventoryOrOrder")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
#End Region
End Class