Imports System.Collections.Generic
Imports System.Linq
Public Class wfIssuetoSupplierasReturn_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mIssue As Issue
    Public mStockItemList As PendingToIssueItemList
    Public mPendingItemList As PendingToIssueList
    Public mOrder As Order
    Public PartNo As String
    Public mItemName As String
    Dim mIndex2 As Int32
    Dim LinkID As String
    Public mAlternateStockList As AlternateStockItemList
    Public mName As String = String.Empty
    Dim ReceiptItemStoreCollection As Dictionary(Of Guid, Guid)
    Dim mFileAttach As FileAttach
    Dim ItemPrimaryCategory As Integer = 0
    Public mUserHasNoStoreRights As UserHasNoStoreRights
    Public mCategoryList As CategoryList
    Dim EventLogID As Guid
    Dim MsgText As String
    Dim IssueDetail As String
    Dim str1 As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssue = CType(Session("mIssue"), Issue)
        mStockItemList = CType(Session("mStockItemList"), PendingToIssueItemList)
        mPendingItemList = CType(Session("mPendingItemList"), PendingToIssueList)
        mAlternateStockList = Session("mAlternateStockList")
        mOrder = CType(Session("mOrder"), Order)
        PartNo = Session("PartNo")
        If mIssue Is Nothing Then
            'do nothing
        Else
            If mIssue.TransTypeID = 18 Then
                LinkID = Session("mLinkID").ToString
            End If
        End If
        ReceiptItemStoreCollection = Session("ReceiptItemStoreCollection")
        ItemPrimaryCategory = IIf(Session("ItemPrimaryCategory") Is Nothing, 0, Session("ItemPrimaryCategory")) 'Added By Vikrant For Issue Tools Transaction
        mCategoryList = Session("mCategoryList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub setLandingRate()
        Dim Report As New ReportData("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", )
        If mPendingItemList.Count > 0 Then
            dgPendingItemList.Columns(11).HeaderText = "Landing Rate(" + Report.CurrencySymbol + ")"
        Else
            dgPendingItemList.Columns(11).HeaderText = "Landing Rate"
        End If
    End Sub
    Private Sub Method()
        Session("CheckQty") = "False"
        Session.Remove("mStockItemList")
        Session.Remove("mPendingItemList")
        Session.Remove("mAlternateStockList")
        Session.Remove("PartNo")
        Session("Edit") = False
        Session("IsRemovedAsReturnableFromAircraft") = False
        Session.Remove("ItemPrimaryCategory")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Expired" Or MSGBoxCtrl.Sender = "Continue1" Then
                        Try
                            Session("Sender") = ""
                            mIndex2 = Session("Index2")
                            SetObject(mIndex2)
                            Method()
                            Session.Remove("Index2")
                            Session.Remove("ItemName")
                            Session.Remove("IsAlternatePart")
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Expired" Or MSGBoxCtrl.Sender = "Continue1" Then
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "IssueCreated" Then
                        SetControl()
                    End If
            End Select
        End If
    End Sub
    Private Sub SetObject(ByVal Index As Int32, Optional ByVal IsAlternatePart As Boolean = False, Optional ByVal AsPerAllocation As String = "") 'Changed By Utkarsh On 02-May-2012 FOR ALLIssue30042012
        mIssue = Issue.NewIssue(TransTypeID:=103)
        mIssue.IDate = Today.Date.ToString
        mIssue.VendorID = mPendingItemList(Index).VendorID
        mIssue.StoreID = mPendingItemList(Index).StoreID
        mIssue.MachineID = Guid.Empty
        mIssue.ToStoreID = Guid.Empty
        mIssue.WorkShopID = Guid.Empty
        mIssue.nWOID = Guid.Empty
        mIssue.UserName = User.Identity.Name
        mIssue.StatusID = 2
        If Not mIssue Is Nothing Then
            mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
            mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingItemList(Index).ReceiptItemID
            mIssue.IssueItems.CurrentItem.IsAsPerAllocation = False
            If mPendingItemList(Index).CalibrationDueDateFormatted.ToString <> "" Then
                mIssue.IssueItems.CurrentItem.CalibrationDueDate = mPendingItemList(Index).CalibrationDueDateFormatted.ToString
            Else
                mIssue.IssueItems.CurrentItem.CalibrationDueDate = System.DBNull.Value
            End If

            If mPendingItemList(Index).ManufacturingDateFormatted.ToString <> "" Then
                mIssue.IssueItems.CurrentItem.ManufacturingDate = mPendingItemList(Index).ManufacturingDateFormatted.ToString
            Else
                mIssue.IssueItems.CurrentItem.ManufacturingDate = System.DBNull.Value
            End If
            mIssue.IssueItems.CurrentItem.DisplayQty = mPendingItemList(Index).AvailableQuantity
            mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingItemList(Index).DisplayUnitID
            mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingItemList(Index).DisplayUnitName

            mIssue.IssueItems.CurrentItem.ItemTagID = mPendingItemList(index:=Index).ItemTagID
            mIssue.IssueItems.CurrentItem.ItemTagName = mPendingItemList(index:=Index).ItemTagName
            mIssue.IssueItems.CurrentItem.StatusKit = mPendingItemList(index:=Index).StatusKit
            mIssue.IssueItems.CurrentItem.CodeNo = mPendingItemList(index:=Index).CodeNo
            mIssue.IssueItems.CurrentItem.Location = mPendingItemList(index:=Index).ReceiptItemBinLocation
            mIssue.IssueItems.CurrentItem.Note = "Auto Issue to Supplier as Return has been created on date " + Today.Date.ToString(AppSettings("DateFormat"))
            mIssue.Save()

            IssueDetail = IssueDetail + "Issue : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted
            MarkLog(Action.Save, "IssuetoSupplierasReturn", IssueDetail, Util.ErrorType.NoError, mIssue.ID, EventLogID)
            Session("mIssue") = mIssue
            str1 = str1 + ("<span class=""clsLabelAuto"">Issue(s) Created Successfully! <BR>" + IssueDetail + "</BR></span>")
            MSGBoxCtrl.Show("Alert!", str1, "", MsgBoxStyle.OkOnly, "IssueCreated")
        End If
    End Sub
    Private Sub ControlVisibility()

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryLists") = mCategoryList
        DataBind()
    End Sub
    Private Sub SetControl()
        mPendingItemList = PendingToIssueList.GetPendingToIssueList(StoreID:=Guid.Empty,
                                                                    ItemName:=txtSearch.Text.Trim,
                                                                    IssueDate:=Today.Date.ToString,
                                                                    CategoryID:=cmbCategory.SelectedValue,
                                                                    IsAllPartsRequired:=False,
                                                                    IsBERPart:=False,
                                                                    IsForIssuetoSupplierasReturn:=True,
                                                                    SearchStr:=txtSupplier.Text.Trim,
                                                                    ClientCode:=AppSettings("ClientCode"))
        dgPendingItemList.DataSource = mPendingItemList
        dgPendingItemList.DataBind()
        Session("mPendingItemList") = mPendingItemList
        setLandingRate()
        lblResult1.Text = "As per criteria: " & mPendingItemList.Count & " Record(s) found."
        upnlPendingItemList.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            SetControl()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        SetControl()
    End Sub
    Private Sub dgPendingItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingItemList.RowCommand
        DueAtMessage.Visible = False
        Select Case e.CommandName
            Case "SelectRecord"
                Dim Index2 As Int32 = CInt(e.CommandArgument) + dgPendingItemList.PageIndex * dgPendingItemList.PageSize

                mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mPendingItemList(Index2).StoreID.ToString) 'Added By Prashant 31-Oct-2018 ALL30102018
                If mUserHasNoStoreRights.Count > 0 Then
                    MSGBoxCtrl.Show("Alert!", "Sorry you do not have rights to select this store. Please contact with admin.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("Index2") = Index2

                MsgText = "You Are Going To Return Stock To Supplier " & mPendingItemList(Index2).Vendor & "." & "<BR> <BR> Do you want to continue? "
                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, MsgText, MsgBoxStyle.YesNo, "Continue1")
        End Select
    End Sub
    Private Sub dgPendingItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingItemList.PageIndexChanging
        DueAtMessage.Visible = False
        dgPendingItemList.PageIndex = e.NewPageIndex
        dgPendingItemList.DataSource = mPendingItemList
        dgPendingItemList.DataBind()
        upnlPendingItemList.Update()
        Session("mPendingItemList") = mPendingItemList
    End Sub
    Private Sub dgPendingItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingItemList.Sorting
        DueAtMessage.Visible = False
        mPendingItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingItemList") = mPendingItemList
        dgPendingItemList.DataSource = mPendingItemList
        dgPendingItemList.DataBind()
        upnlPendingItemList.Update()
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session.Remove("mStockItemList")
        Session.Remove("mPendingItemList")
        Session.Remove("mAlternateStockList")
        Session.Remove("PartNo")
        Session.Remove("PendingIssuedQty")
        Session.Remove("Index2")
        Session.Remove("ItemName")
        Session("Edit") = False
        Session.Remove("mLinkID")
        Session("IsRemovedAsReturnableFromAircraft") = False
        Session.Remove("ItemPrimaryCategory")
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class