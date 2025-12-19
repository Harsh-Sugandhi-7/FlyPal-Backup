Public Class wfPendingPurchaseQuotations_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mPendingPurchaseQuotationList As PendingPurchaseQuotationList
    Public mPendingPurchaseQuotationItems As PendingPurchaseQuotationItems
    Public mOrder As Order
    Public mSelectList() As Boolean
    Public mPrevTransID As Guid
    Private mIsAll As Boolean = False
    Private mOrderDate As String
    Private mVendorID As Guid
    Private mSelectedQuotationIndex As Integer = -1
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mSelectedQuotationIndex = Session("mSelectedQuotationIndex")
        mOrder = Session("mOrder")
        mPendingPurchaseQuotationList = Session("mPendingPurchaseQuotationList")
        mPendingPurchaseQuotationItems = Session("mPendingPurchaseQuotationItems")
        mPrevTransID = Session("mPrevTransID")
    End Sub
    Private Sub SetMultipleObject()
        Dim chkSelect As CheckBox
        Dim Recordno, PageItems As Integer
        PageItems = dgTransItemList.Rows.Count - 1
        For I As Integer = 0 To PageItems
            Recordno = I + dgTransItemList.PageSize * dgTransItemList.PageIndex
            chkSelect = CType(dgTransItemList.Rows(I).FindControl("chkSelect"), CheckBox)
            mPendingPurchaseQuotationItems(Recordno).IsSelected = chkSelect.Checked
            mPendingPurchaseQuotationItems(Recordno).MarkClean()
        Next
        Session("mPendingPurchaseQuotationItems") = mPendingPurchaseQuotationItems
    End Sub
    '----ADded by Shital on 04-Feb-2021
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Confirmation" Then
                        Try
                            Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")
                            MarkLog(Util.Action.Save, "Pending Purchase Quotation Item list", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If

            End Select
        End If
    End Sub
    '--------
#End Region

#Region "Data Binding"
    Public Sub DataFieldBind()
        If mIsAll Then
            mPendingPurchaseQuotationList = PendingPurchaseQuotationList.GetPendingPurchaseQuotationList(txtDate.Text, Guid.Empty, Guid.Empty, _
                                                                                                         OrderTransTypeID:=mOrder.TransTypeID)
        Else
            mPendingPurchaseQuotationList = PendingPurchaseQuotationList.GetPendingPurchaseQuotationList(txtDate.Text, mOrder.VendorID, mPrevTransID, _
                                                                                                         OrderTransTypeID:=mOrder.TransTypeID)
        End If
        dgTransList.DataSource = mPendingPurchaseQuotationList
        Session("mPendingPurchaseQuotationList") = mPendingPurchaseQuotationList
        dgTransList.DataBind()
        lblResult.Text = "List of Quotations : " + mPendingPurchaseQuotationList.Count.ToString + " Record (s) found"
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If txtDate.Text = "" Then
                txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            End If
            If mOrder.IsNew Then
                txtDate.Enabled = True
                txtDate.Text = mOrder.OrderDateFormatted
                If mOrder.OrderItems.Count - 1 = -1 Then
                    txtDate.Enabled = True
                    rdbFromLastQuotation.Checked = False
                    rdbFromAllPendingQuotation.Checked = True
                    mIsAll = True
                Else
                    txtDate.Enabled = False
                    rdbFromAllPendingQuotation.Checked = False
                    rdbFromLastQuotation.Checked = True
                    mIsAll = False
                End If
            Else
                txtDate.Enabled = False
                rdbFromLastQuotation.Checked = True
                rdbFromAllPendingQuotation.Checked = False
                txtDate.Text = mOrder.OrderDateFormatted
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub rdbFromLastQuotation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbFromLastQuotation.CheckedChanged
        mIsAll = False
        Session("mIsAll") = mIsAll
    End Sub
    Private Sub rdbFromAllPendingQuotation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbFromAllPendingQuotation.CheckedChanged
        mIsAll = True
        Session("mIsAll") = mIsAll
    End Sub
    Private Sub dgTransList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTransList.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim index As Integer = CInt(e.CommandArgument) + dgTransList.PageIndex * dgTransList.PageSize
                mSelectedQuotationIndex = index
                Session("mSelectedQuotationIndex") = mSelectedQuotationIndex
                mPendingPurchaseQuotationItems = PendingPurchaseQuotationItems.GetPendingQuotationList(mPendingPurchaseQuotationList.Item(index).ID, _
                                                                                                       OrderTransTypeID:=mOrder.TransTypeID)

                dgTransItemList.DataSource = mPendingPurchaseQuotationItems
                Session("mPendingPurchaseQuotationItems") = mPendingPurchaseQuotationItems
                dgTransItemList.DataBind()

                lblResult1.Text = "List of Quotation Item (s): " + mPendingPurchaseQuotationItems.Count.ToString + " Record (s) found"

                If mPendingPurchaseQuotationItems.Count >= 0 Then
                    btnDone.Enabled = True
                Else
                    btnDone.Enabled = False
                End If
                upnlButtons.Update()
                upnlTransItemList.Update()
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, txtDate.TextChanged
        mIsAll = Session("mIsAll")
        DataFieldBind()
        upnlTransList.Update()
        upnlTransItemList.Update()
        Session.Remove("mIsAll")
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        If mSelectedQuotationIndex > -1 Then
            'If mOrder.VendorID.Equals(Guid.Empty) Then
            With mPendingPurchaseQuotationList(mSelectedQuotationIndex)
                mOrder.OrderDate = txtDate.Text
                mOrder.VendorID = .VendorID
                mOrder.CurrencyID = .CurrencyID
                mOrder.ConversionFactor = .ConversionFactor
                'Added By Vikrant On 06-Apr-2020
                mOrder.QuotationNo = .QuotationTextNo
                mOrder.QuotationDate = .DateFormatted.ToString
                'End
            End With
            'End If
        End If
        SetMultipleObject()
        Session("PendingQuotationItems") = "True"
        ' Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")  'Commented  by Shital on 15-Feb-2021

        'Added by Shital on 15-Feb-2021
        Dim chkSelect As CheckBox
        Dim ItemNames As String = ""
        Dim Recordno, PageItems As Integer
        PageItems = dgTransItemList.Rows.Count - 1
        For I As Integer = 0 To PageItems
            Recordno = I + dgTransItemList.PageSize * dgTransItemList.PageIndex
            chkSelect = CType(dgTransItemList.Rows(I).FindControl("chkSelect"), CheckBox)
            If chkSelect.Checked And mPendingPurchaseQuotationItems(Recordno).orderItemReceiptBalanceQuantity > 0.0 Then
                ItemNames = ItemNames + mPendingPurchaseQuotationItems(Recordno).ItemName + ","
            End If
        Next
        If ItemNames <> "" Then
			MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, "There are " + ItemNames.ToString.TrimEnd(",") + "An Order already exists for this Part or its Alternate Part. Do you still want to create another Order ?", MsgBoxStyle.YesNo, "Confirmation")
		Else
            Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")
        End If
        '--------

    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("PendingQuotationItems") = "True"
        Session("mPendingPurchaseQuotationItems") = Nothing
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub dgTransList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTransList.Sorting
        mPendingPurchaseQuotationList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingPurchaseQuotationList") = mPendingPurchaseQuotationList
        dgTransList.DataSource = mPendingPurchaseQuotationList
        dgTransList.DataBind()
        upnlTransList.Update()
    End Sub
    Private Sub dgTransList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTransList.PageIndexChanging
        dgTransList.PageIndex = e.NewPageIndex
        dgTransList.DataSource = mPendingPurchaseQuotationList
        Session("mPendingPurchaseQuotationList") = mPendingPurchaseQuotationList
        dgTransList.DataBind()
        upnlTransList.Update()
    End Sub
    Private Sub dgTransItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTransItemList.PageIndexChanging
        dgTransItemList.PageIndex = e.NewPageIndex
        dgTransItemList.DataSource = mPendingPurchaseQuotationItems
        Session("mPendingPurchaseQuotationItems") = mPendingPurchaseQuotationItems
        dgTransItemList.DataBind()
        upnlTransItemList.Update()
    End Sub
#End Region

End Class