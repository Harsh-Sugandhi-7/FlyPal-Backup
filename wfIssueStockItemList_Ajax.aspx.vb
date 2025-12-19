'AJAX Conversion By Vikrant On 04-Nov-2014

Public Class wfIssueStockItemList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mIssue As Issue
    Private mPartStockListForERO As PartStockListForERO
    Private mPartStockInfo As PartStockListForERO.PartStockInfo
    Public mPendingToReturnForExchangeRepairInfo As PendingToReturnForExchangeRepairList.PendingToReturnForExchangeRepairInfo
    Dim mFileAttach As FileAttach
    Public mUserHasNoStoreRights As UserHasNoStoreRights
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssue = Session("mIssue")
        mPartStockListForERO = Session("mPartStockListForERO")
        mPendingToReturnForExchangeRepairInfo = Session("mPendingToReturnForExchangeRepairInfo")
    End Sub
    Private Sub SetSession()
        Session("mIssue") = mIssue
        Session("mPendingToReturnForExchangeRepairInfo") = mPendingToReturnForExchangeRepairInfo
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mIssue")
        Session.Remove("mPendingToReturnForExchangeRepairInfo")
    End Sub
    Public Sub setObject(ByVal Index As Integer)
        mPartStockListForERO = Session("mPartStockListForERO")
        mPartStockInfo = mPartStockListForERO.Item(Index)
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPartStockInfo.ReceiptItemID
        'mIssue.IssueItems.CurrentItem.Qty = mPartStockInfo.IssueAbleQty       'Commented By Prashant 4-Jun-010
        mIssue.IssueItems.CurrentItem.DisplayQty = mPartStockInfo.IssueAbleQty 'Added By Prashant 4-Jun-010 
        mIssue.StoreID = mPartStockInfo.StoreID

        'Changes by Kalpesh Shah as on 23-01-2008
        mIssue.IssueItems.CurrentItem.OrderItemID = mPendingToReturnForExchangeRepairInfo.OrderItemID
        mIssue.IssueItems.CurrentItem.ItemTagID = mPartStockInfo.ItemTagID
        mIssue.IssueItems.CurrentItem.ItemTagName = mPartStockInfo.ItemTagName
        mIssue.IssueItems.CurrentItem.StatusKit = mPartStockInfo.StatusKit
        Session("mIssue") = mIssue
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "SrNo" Then
                        Try
                            Session("Sender") = ""
                            Response.Redirect("wfIssueItem_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfIssueStockItemList_Ajax.aspx")
                        Catch ex As SqlException

                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub ReceiptItemAttachment(Optional ByVal ReceiptItemID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal Visibility As Integer = 0)
        mFileAttach = FileAttach.GetAttachment(New Guid(ReceiptItemID))
        If (mFileAttach.Size > 0) Then
            Dim No As New Random
            Dim StrName As String = "abc" & No.Next.ToString
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Changes by Kalpesh Shah as on 23-01-2008
        'mPartStockListForERO = PartStockListForERO.GetPartStockListForERO(mPendingToReturnForExchangeRepairInfo.ItemName, mPendingToReturnForExchangeRepairInfo.SerialNo, mIssue.IDate.ToString, mIssue.StoreID, mPendingToReturnForExchangeRepairInfo.ItemID.ToString, mPendingToReturnForExchangeRepairInfo.ReceiptItemID.ToString)
        mPartStockListForERO = PartStockListForERO.GetPartStockListForERO(mPendingToReturnForExchangeRepairInfo.ItemName, mPendingToReturnForExchangeRepairInfo.SerialNo, mIssue.IDate.ToString, mIssue.StoreID, mPendingToReturnForExchangeRepairInfo.ItemID.ToString, mPendingToReturnForExchangeRepairInfo.ReceiptItemID.ToString, mPendingToReturnForExchangeRepairInfo.OrderItemID.ToString)
        'Set DataSource of the Grid
        dgIssueStockItemList.DataSource = mPartStockListForERO
        'dgIssueStockItemList.DataBind()
        lblResult.Text = "Part Stock List : " & mPartStockListForERO.Count & " Record(s) found."
        Session("mPartStockListForERO") = mPartStockListForERO
        DataBind()
    End Sub
   
#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
    End Sub
    Private Sub dgIssueStockItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssueStockItemList.RowCommand

        Select Case e.CommandName
            Case "SelectPart"
                dgIssueStockItemList.DataSource = mPartStockListForERO
                dgIssueStockItemList.DataBind()

                Dim Index As Integer = CInt(e.CommandArgument) + dgIssueStockItemList.PageIndex * dgIssueStockItemList.PageSize
                mPartStockListForERO = Session("mPartStockListForERO")
                mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mPartStockListForERO(Index).StoreID.ToString) 'Added By Prashant 31-Oct-2018 ALL30102018
                If mUserHasNoStoreRights.Count > 0 Then
                    MSGBoxCtrl.show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mPendingToReturnForExchangeRepairInfo = Session("mPendingToReturnForExchangeRepairInfo")
                setObject(Index)
                If UCase(mPartStockListForERO(Index).SerialNo) = UCase(mPendingToReturnForExchangeRepairInfo.SerialNo) Then
                    Session("Edit") = False
                    Response.Redirect("wfIssueItem_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfIssueStockItemList_Ajax.aspx")
                Else
                    MSGBoxCtrl.show("Issue Selection !", "You are trying to Issue different SerialNo. Item <BR><BR>Do you want to Continue? ", "Click Yes to Select Item, No to Go Back", MsgBoxStyle.YesNo, "SrNo")
                End If
            Case "ViewRec"
                dgIssueStockItemList.DataSource = mPartStockListForERO
                dgIssueStockItemList.DataBind()
                Dim Index As Integer = CInt(e.CommandArgument) + dgIssueStockItemList.PageIndex * dgIssueStockItemList.PageSize
                ReceiptItemAttachment(ReceiptItemID:=mPartStockListForERO(Index).ReceiptItemID.ToString)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Response.Redirect(Request.QueryString("ChildPage1"))
        Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    'Added By Prashant 18-June-2009 for grid sorting
    Private Sub dgIssueStockItemList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgIssueStockItemList.Sorting
        mPartStockListForERO.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartStockListForERO") = mPartStockListForERO
        dgIssueStockItemList.DataSource = mPartStockListForERO
        dgIssueStockItemList.DataBind()
    End Sub
    '-----------------------------------------------
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgIssueStockItemList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgIssueStockItemList.PageIndexChanging
        dgIssueStockItemList.PageIndex = e.NewPageIndex
        dgIssueStockItemList.DataSource = mPartStockListForERO
        Session("mPartStockListForERO") = mPartStockListForERO
        dgIssueStockItemList.DataBind()
    End Sub
#End Region

   
End Class