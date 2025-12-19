Public Class wfIssueStockItemListForDiscardExchangeCoreUnit_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mIssue As Issue
    Private mPartStockListForERO As PartStockListForERO
    Private mPartStockInfo As PartStockListForERO.PartStockInfo
    Public mPendingToReturnForExchangeRepairInfo As PendingToReturnForExchangeRepairList.PendingToReturnForExchangeRepairInfo
    Public IssueDetail As String
    Public Remark As String
    Dim mIssueStockItemListIndex As Integer
    Dim mIssueDate As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssue = Session("mIssue")
        mPartStockListForERO = Session("mPartStockListForERO")
        mPendingToReturnForExchangeRepairInfo = Session("mPendingToReturnForExchangeRepairInfo")
        mIssueStockItemListIndex = Session("mIssueStockItemListIndex")
        mIssueDate = Session("mIssueDate")
    End Sub
   Private Sub RemoveSession()
        Session.Remove("mIssue")
        Session.Remove("mPendingToReturnForExchangeRepairInfo")
        Session.Remove("mIssueStockItemListIndex")
        Session.Remove("mIssueDate")
    End Sub
    Public Sub setObject(ByVal Index As Integer)
        mPartStockListForERO = Session("mPartStockListForERO")
        mPartStockInfo = mPartStockListForERO.Item(Index)
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPartStockInfo.ReceiptItemID
        mIssue.IssueItems.CurrentItem.DisplayQty = mPartStockInfo.IssueAbleQty 'Added By Prashant 4-Jun-010 
        mIssue.StoreID = mPartStockInfo.StoreID
        mIssue.IssueItems.CurrentItem.OrderItemID = mPendingToReturnForExchangeRepairInfo.OrderItemID
        Session("mIssue") = mIssue
    End Sub
     Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SelectPartToDiscard" Then
                        Try
                            Session("Sender") = ""
                            MSGBoxCtrl.show("Discarding Core Unit!", "Exchange core unit will get discarded. And the ERO pending List gets updated accordingly<BR><BR>Do you want to Continue? ", "", MsgBoxStyle.YesNo, "Discard")
                            Exit Sub
                        Catch ex As SqlException

                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Discard" Then
                        Try
                            If CreateAutoIssue() = True Then
                                MSGBoxCtrl.show("Part Discarded Successfully!", "<BR><BR>" + IssueDetail, "", MsgBoxStyle.OkOnly, "DiscardedSuccessfully")
                                Exit Sub
                            End If
                        Catch ex As SqlException

                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "DiscardedSuccessfully" Then
                        Try
                            RemoveSession()
                            Response.Redirect("Index.aspx")
                        Catch ex As SqlException

                        End Try
                    End If
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        End If
    End Sub
    Private Function CheckDateForTransactionLock(ByVal TransDate As Date) As Boolean
        Dim FirstDayofLastMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)
        Dim FirstDayofMonth As Date = DateSerial(Year(Today.Date), Month(Today.Date), 1)
        If (TransDate >= FirstDayofLastMonth) Then
            If (TransDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If
    End Function
    Private Function CreateAutoIssue() As Boolean
        mPartStockListForERO = Session("mPartStockListForERO")
        mPartStockInfo = mPartStockListForERO.Item(mIssueStockItemListIndex)
        mIssue = Issue.NewIssue(Util.Trans.DisacrdPart)
        mIssue.IDate = mIssueDate.ToString
        mIssue.VendorID = mPendingToReturnForExchangeRepairInfo.VendorID
        mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPartStockInfo.ReceiptItemID
        mIssue.IssueItems.CurrentItem.DisplayQty = mPendingToReturnForExchangeRepairInfo.LoanQty 'LoanQty=ERO Qty in this object.
        mIssue.IssueItems.CurrentItem.DiscardAmt = mPartStockInfo.EffRate * mIssue.IssueItems.CurrentItem.DisplayQty '(mReceiptCumInvoiceItem.EffRate * mIssue.IssueItems.CurrentItem.DisplayQty)
        mIssue.IssueItems.CurrentItem.OrderItemID = mPendingToReturnForExchangeRepairInfo.OrderItemID
        mIssue.IssueItems.CurrentItem.Remark = "Discard exchange core unit."
        'AttachMyFile() 'Added By Vikrant On 02-Jun-2014
        mIssue.StoreID = mPartStockInfo.StoreID
        mIssue.UserName = User.Identity.Name
        mIssue.CalculateTotal()
        mIssue.MachineID = Guid.Empty
        mIssue.ToStoreID = Guid.Empty
        mIssue.WorkShopID = Guid.Empty
        mIssue.nWOID = Guid.Empty
        mIssue.UserName = User.Identity.Name
        Remark = "Discard exchange core unit."
        mIssue.StatusID = 2

        Try
            If mIssue.IsValid Then
                mIssue.Save()
                Session("mIssue") = mIssue
                IssueDetail = "Issue : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted
                MarkLog(Util.Action.Authorize, "Issue to Discard exchange core unit.", IssueDetail + vbCrLf + Remark, Util.ErrorType.NoError, mIssue.ID, EventLogID)
                Return True
            Else
                Dim strMSG As String = ""
                If Not mIssue.IsValid Then
                    For i As Integer = 0 To mIssue.GetBrokenRulesCollection.Count - 1
                        strMSG = strMSG + mIssue.GetBrokenRulesCollection(i).Description + "<Br>"
                    Next
                End If
                Dim mIssueItem As IssueItem
                If Not mIssue.IssueItems.IsValid Then
                    For Each mIssueItem In mIssue.IssueItems
                        For i As Integer = 0 To mIssueItem.GetBrokenRulesCollection.Count - 1
                            strMSG = strMSG + mIssueItem.ItemName + " : " + mIssueItem.GetBrokenRulesCollection(i).Description + "<Br>"
                        Next
                    Next
                End If
                If strMSG.Trim <> "" Then
                    Session("strMSG") = strMSG
                    Return False
                End If
            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "CCtabReceiptItemStockBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show("Stock Alert!", "", ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Issue Qty can not be greater than Stock Qty.", MsgBoxStyle.OkOnly, "")
                Session("sender") = "Status"
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabnWOSpareFromWOJobSparePendingIssuedQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabnWOToolsPendingIssuedQty", CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "", "Issue Qty can not be greater than Required Qty.", MsgBoxStyle.OkOnly, "")
                Session("sender") = "Status"
                Session("mIssue") = mIssue
                DataFieldBind()
            ElseIf InStr(ex1.Message, "CCtabReceiptItemLoanQty", CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "", ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Issue Qty can not be greater than Receipt Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabSalesOrderItemIssueBalQty", CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "", ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Issue Qty can not be greater than Sales Order Qty.", MsgBoxStyle.OkOnly, "")
            ElseIf InStr(ex1.Message, "CCtabOrderItemEROQty", CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "", ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Issue Qty can not be greater than Exchange/Repair/Overhaul Qty.", MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPartStockListForERO = PartStockListForERO.GetPartStockListForERO(mPendingToReturnForExchangeRepairInfo.ItemName, mPendingToReturnForExchangeRepairInfo.SerialNo, mIssueDate.ToString, Guid.Empty, mPendingToReturnForExchangeRepairInfo.ItemID.ToString, mPendingToReturnForExchangeRepairInfo.ReceiptItemID.ToString, mPendingToReturnForExchangeRepairInfo.OrderItemID.ToString)
        dgIssueStockItemList.DataSource = mPartStockListForERO
        lblResult.Text = "Part Stock List : " & mPartStockListForERO.Count & " Record(s) found."
        Session("mPartStockListForERO") = mPartStockListForERO
        DataBind()
    End Sub
#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            mIssueDate = Request.QueryString("IssueDate")
            Session("mIssueDate") = mIssueDate
            DataFieldBind()
        End If
    End Sub
    Private Sub dgIssueStockItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssueStockItemList.RowCommand
        Select Case e.CommandName
            Case "SelectPart"
                dgIssueStockItemList.DataSource = mPartStockListForERO
                dgIssueStockItemList.DataBind()

                Dim Index As Integer = CInt(e.CommandArgument) + dgIssueStockItemList.PageIndex * dgIssueStockItemList.PageSize

                If AppSettings("LockBackDatedTransaction") = "True" Then
                    If UCase(User.Identity.Name.Trim).Equals("BTPLADMIN") Then
                        'Do nothing
                    Else
                        If CheckDateForTransactionLock(CDate(mIssueDate)) Then
                            MSGBoxCtrl.Show("Save Alert!", "Previous Months transactions can only be saved until " & DateSerial(Year(CDate(mIssueDate).AddMonths(1)), Month(CDate(mIssueDate).AddMonths(1)), 10).ToString(AppSettings("DateFormat")) & ", as Accounts are closed for Previous Months.", "Kindly book this transaction in current month to reflect in Valuation.", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If
                End If
                mPartStockListForERO = Session("mPartStockListForERO")
                mPendingToReturnForExchangeRepairInfo = Session("mPendingToReturnForExchangeRepairInfo")
                Session("mIssueStockItemListIndex") = Index
               
                'setObject(Index)
                If UCase(mPartStockListForERO(Index).SerialNo) = UCase(mPendingToReturnForExchangeRepairInfo.SerialNo) Then
                    MSGBoxCtrl.show("Discarding Core Unit", "Exchange core unit will get discarded. And the ERO pending List gets updated accordingly<BR><BR>Do you want to Continue? ", "", MsgBoxStyle.YesNo, "Discard")
                Else
                    MSGBoxCtrl.show("Stock Selection !", "You are trying to Discard different Serial No. Item <BR><BR>Do you want to Continue? ", "", MsgBoxStyle.YesNo, "SelectPartToDiscard")
                End If
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Response.Redirect("Index.aspx")
    End Sub
    Private Sub dgIssueStockItemList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgIssueStockItemList.Sorting
        mPartStockListForERO.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartStockListForERO") = mPartStockListForERO
        dgIssueStockItemList.DataSource = mPartStockListForERO
        dgIssueStockItemList.DataBind()
    End Sub
    Private Sub dgIssueStockItemList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgIssueStockItemList.PageIndexChanging
        dgIssueStockItemList.PageIndex = e.NewPageIndex
        dgIssueStockItemList.DataSource = mPartStockListForERO
        Session("mPartStockListForERO") = mPartStockListForERO
        dgIssueStockItemList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class