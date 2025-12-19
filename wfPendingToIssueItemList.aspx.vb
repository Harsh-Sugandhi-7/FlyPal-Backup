Partial Class wfPendingToIssueItemList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variables "
    Public mPendingToIssueItemList As PendingToIssueItemList
    Public mPendingToIssueList As PendingToIssueList
    Public mIssue As Issue

    Public mReceiptItemID As Guid
    Public mStoreID As Guid
    Public mItemName As String
    Public mAvailableItemQty As Decimal
    Public mTransTypeID As Trans
    Public mPartName As String
    Public mIssueDate As String = ""
    Public mFromSalesOrder As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mPendingToIssueItemList = Session("mPendingToIssueItemList")
        mPendingToIssueList = Session("mPendingToIssueList")
        mIssue = Session("mIssue")

        mReceiptItemID = Session("mReceiptItemID")
        mStoreID = Session("mStoreID")
        mAvailableItemQty = Session("mAvailableItemQty")
        mTransTypeID = Session("mTransTypeID")
        mPartName = Session("mPartName")
        mIssueDate = Session("mIssueDate")
    End Sub
    Private Sub SetSession()
        Session("mPendingToIssueItemList") = mPendingToIssueItemList
        Session("mPendingToIssueList") = mPendingToIssueList
        Session("mReceiptItemID") = mReceiptItemID
        Session("mStoreID") = mStoreID
        Session("mAvailableItemQty") = mAvailableItemQty
        Session("mTransTypeID") = mTransTypeID
        Session("mPartName") = mPartName
        Session("mIssueDate") = mIssueDate
        Session("mIssue") = mIssue
    End Sub

    Private Sub FindNow1(ByVal ItemName As String)
        mIssue = Session("mIssue")
        'Get List From the Database as per Criteria             
        'lintype  text  No storename from date todate  Vendor  Aircraft totype  0 vendor   1 aircraft
        mPendingToIssueItemList = PendingToIssueItemList.GetPendingItemList(mIssue.StoreID, ItemName, mIssue.IDate.ToString, mTransTypeID)
        dgItemList.DataSource = mPendingToIssueItemList
        dgItemList.DataBind()
        'FindNow2(ItemName)
        Session("mPendingToIssueItemList") = mPendingToIssueItemList
        'EnableDisableButtons1()
    End Sub
    Private Sub FindNow2(ByVal ItemName As String)  ''for second 
        mIssue = Session("mIssue")
        mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, ItemName, , , , , mIssue.IDate.ToString, mTransTypeID, ToTypeIDOfIssue:=mIssue.ToTypeID)
        'Set DataSource of the Grid
        dgPendingList.DataSource = mPendingToIssueList
        Session("mPendingToIssueList") = mPendingToIssueList
        dgPendingList.DataBind()
        lblResult2.Text = "Part Stock List : " & mPendingToIssueList.Count & " Record(s) found."
    End Sub
    Private Sub SetGrid(ByVal ItemName As String)  ''for second grid Only for heading and label
        If mFromSalesOrder = "1" Then
            mIssue = Session("mIssue")
            mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, ItemName, , , , , mIssue.IDate.ToString, mTransTypeID, ToTypeIDOfIssue:=mIssue.ToTypeID)
            'Set DataSource of the Grid
            dgPendingList.DataSource = mPendingToIssueList
            Session("mPendingToIssueList") = mPendingToIssueList
            dgPendingList.DataBind()
            lblResult2.Text = "Part Stock List : " & mPendingToIssueList.Count & " Record(s) found."
        Else
            mIssue = Session("mIssue")
            mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(Guid.Empty, , , , , , mIssue.IDate.ToString, mTransTypeID, ToTypeIDOfIssue:=mIssue.ToTypeID)
            'Set DataSource of the Grid
            dgPendingList.DataSource = mPendingToIssueList
            Session("mPendingToIssueList") = mPendingToIssueList
            dgPendingList.DataBind()
            lblResult2.Text = "Part Stock List : " & mPendingToIssueList.Count & " Record(s) found."
        End If

    End Sub
    Private Sub EnableDisableButtons()
        btnFindNow.Visible = False
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

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        GetSession()
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mReceiptItemID
        mIssue.IssueItems.CurrentItem.Qty = mAvailableItemQty

        Session("mIssue") = mIssue
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here]
        mPartName = Session("mItemName")
        mFromSalesOrder = Session("FromSalesOrder")
        If Not IsPostBack Then
            txtSearch.Text = mPartName
            FindNow1(Trim(mPartName))
            lblResult.Text = "Total Part Stock  List : " & mPendingToIssueItemList.Count & " Record(s) found."
            SetGrid(mPartName)
            If mFromSalesOrder = "1" Then
                lblResult.Visible = False
                dgItemList.Visible = False
                lblInfo.Visible = True
                lblInfo.Text = "You have to issue  " + Session("SalesOrderQty") + "  Qty."
            Else
                lblResult.Visible = True
                dgItemList.Visible = True
                lblInfo.Visible = False
            End If
        End If
        EnableDisableButtons()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mPartName = Session("mPartname")
        FindNow1(Trim(mPartName))
        lblResult.Text = "Total Part Stock  List : " & mPendingToIssueItemList.Count & " Record(s) found."
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("AddSalesOrderParts") = "False"
        mPendingToIssueItemList = Nothing
        mPendingToIssueList = Nothing
        Session.Remove("mPendingToIssueItemList")
        Session.Remove("mPendingToIssueList")
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        Response.Redirect("wfPendingSalesOrderList_Ajax.aspx?BackPage=wfIssue_Ajax.aspx&ChildPage=wfIssueItem_Ajax.aspx&Name=" & HttpUtility.UrlEncode(mPartName))
        'Response.Redirect("wfPendingToReturnForExchangeRepair.aspx?BackPage=wfIssue.aspx&ChildPage=wfIssueItem.aspx&Name=" & mPartName)
    End Sub
    Private Sub dgItemList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgItemList.ItemCommand
        Dim Index As Integer = e.Item.ItemIndex + dgItemList.CurrentPageIndex * dgItemList.PageSize
        Dim ItemName As String
        Select Case e.CommandName
            Case "Select"
                mPendingToIssueItemList = Session("mPendingToIssueItemList")
                ItemName = mPendingToIssueItemList.Item(Index).ItemName()
                FindNow2(ItemName)
                lblResult2.Text = "Part Stock List : " & mPendingToIssueList.Count & " Record(s) found."
        End Select
    End Sub
    Private Sub dgPendingList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPendingList.ItemCommand
        Dim Index As Integer = e.Item.ItemIndex + dgPendingList.CurrentPageIndex * dgPendingList.PageSize
        Select Case e.CommandName
            Case "Select"
                mPendingToIssueList = Session("mPendingToIssueList")
                mReceiptItemID = mPendingToIssueList.Item(Index).ReceiptItemID
                If mPendingToIssueList.Item(Index).IsSerialized = True Then
                    mAvailableItemQty = 1
                Else
                    mAvailableItemQty = mPendingToIssueList.Item(Index).AvailableQuantity
                End If
                Session("mAvailableItemQty") = mAvailableItemQty
                Session("mReceiptItemID") = mReceiptItemID
                'Added by Saylee on 12-Apr-2023
                mIssue = Session("mIssue")
                mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingToIssueList.Item(Index).DisplayUnitID
                Session("mIssue") = mIssue
                '*******************************************
                DataFieldBind()
                SetSession()
                Session("CheckQty") = "False"

                Session.Remove("mPendingToIssueItemList")
                Session.Remove("mPendingToIssueList")
                'Session("AddSalesOrderParts") = "False"
                ' Session("AddPendingToIssueItems") = "True"
                Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage"))
            Case "ViewRec"
                mPendingToIssueList = Session("mPendingToIssueList")
                ReceiptItemAttachment(ReceiptItemID:=mPendingToIssueList.Item(Index).ReceiptItemID.ToString)
        End Select
    End Sub
    Private Sub dgItemList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgItemList.PageIndexChanged
        dgItemList.CurrentPageIndex = e.NewPageIndex
        dgItemList.DataSource = mPendingToIssueItemList
        dgItemList.DataBind()
    End Sub
    Private Sub dgPendingList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPendingList.PageIndexChanged
        dgPendingList.CurrentPageIndex = e.NewPageIndex
        dgPendingList.DataSource = mPendingToIssueList
        dgPendingList.DataBind()
    End Sub
    'Added By Prashant 18-June-2009 for grid sort
    Private Sub dgItemList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgItemList.SortCommand
        mPendingToIssueItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingToIssueItemList") = mPendingToIssueItemList
        dgItemList.DataSource = mPendingToIssueItemList
        dgItemList.DataBind()
    End Sub
    Private Sub dgPendingList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPendingList.SortCommand
        mPendingToIssueList = Session("mPendingToIssueList")
        mPendingToIssueList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingToIssueList") = mPendingToIssueList
        dgPendingList.DataSource = mPendingToIssueList
        dgPendingList.DataBind()
    End Sub
    '------------------------------------------
#End Region

End Class
