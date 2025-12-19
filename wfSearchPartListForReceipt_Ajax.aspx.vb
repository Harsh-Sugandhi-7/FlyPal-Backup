Public Class wfSearchPartListForReceipt_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItems As Items
    Public mReceipt As Receipt
    Dim mMachineID As Guid 'Added by Vikrant on 7.3.12 FORALL03052012
    Public mName, StatusID As String
    Public Text, Index, IsSerialized, DescriptionText As String

    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCountForCustomePaging As Integer = 0

    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReceipt = CType(Session("mReceipt"), Receipt)
        mItems = CType(Session("mItems"), Items)
        mMachineID = Session("mMachineID") 'Added by Vikrant on 7.3.12 FORALL03052012

        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCountForCustomePaging = Session("totalCountForCustomePaging")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCountForCustomePaging")

        Session.Remove("mItemList")
    End Sub
    Private Sub FindNow(Optional ByVal PartNo As String = "", Optional ByVal Description As String = "")
        'PartNo
        mItems = Flypal.Items.GetItems(1, PartNo, Description, "", "", "", "", Guid.Empty.ToString, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize) 'Newly Added by Vikrant on 13-Feb-2013 For All13022013
        totalCountForCustomePaging = mItems.TotalRecords
        pagecount = Math.Ceiling(totalCountForCustomePaging / mpageSize)

        Session("totalCountForCustomePaging") = totalCountForCustomePaging
        Session("pagecount") = pagecount

        Session("mItems") = mItems
        Session("IsSerialized") = IsSerialized
        gdvItem.DataSource = mItems
        gdvItem.DataBind()
        UpdateItemGridView()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub DataFieldBind(Optional ByVal PartNo As String = "", Optional ByVal Description As String = "")
        Index = Session("Index")
        Text = Session("Text")
        DescriptionText = Session("DescriptionText")
        IsSerialized = Session("IsSerialized")

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, gdvItem.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = gdvItem.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        FindNow(PartNo, Description)
        txtName.Text = Text
        txtDescription.Text = DescriptionText
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCountForCustomePaging = 0 Then
            lblResult.Text = "List of Part as per criteria : " & totalCountForCustomePaging & " Record(s) found."
        Else
            lblResult.Text = "List of Part as per criteria : " & currentrow + 1 & " to " & currentrow + mItems.Count & " of " & totalCountForCustomePaging & " Record(s) found."
        End If

        SliderExtender1.Minimum = 1
        SliderExtender1.Maximum = pagecount
        Slidercontrol.Text = mCurrentpage
        txtPageDisplay.Text = mCurrentpage
        lblpagecount.Text = pagecount
        If pagecount > 1 Then
            PnlPaging.Visible = True
        Else
            PnlPaging.Visible = False
        End If

        gdvItem.DataBind()
        upnlgrid.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            setFocus(txtName)
            txtName.Text = Session("ItemNo").ToString
            txtName.DataBind()
            DataFieldBind(txtName.Text.Trim, txtDescription.Text.Trim)
            Session.Remove("ItemNo")
        End If
    End Sub
    Private Sub gdvItem_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvItem.PageIndexChanging
        gdvItem.PageIndex = e.NewPageIndex
        mCurrentpage = e.NewPageIndex
        Session("mCurrentpage") = mCurrentpage
        FindNow(txtName.Text.Trim, txtDescription.Text.Trim)
    End Sub
    Private Sub gdvItem_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvItem.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim Index As Integer = CInt(e.CommandArgument) + gdvItem.PageIndex * gdvItem.PageSize
                Dim mId As Guid = mItems(Index).ID
                Dim mQtyRemovedFromAircraft As Decimal = mItems(Index).QtyRemovedFromAircraft 'CDbl(e.Item.Cells(5).Text) 'Added by Vikrant on 7.3.12 FORALL03052012
                Session("RCIItem") = False

                mReceipt.ReceiptItems.CurrentItem.FromItemTypeID = 12 'From Aircraft
                mReceipt.ReceiptItems.CurrentItem.FromPartList = True
                mReceipt.ReceiptItems.CurrentItem.ItemID = mId
                mReceipt.ReceiptItems.CurrentItem.Part = mItems.Item(Index).Name  'mItems.Item(Index).Name
                mReceipt.ReceiptItems.CurrentItem.PartDescription = mItems.Item(Index).Description 'mItems.Item(Index).Description
                'mReceipt.ReceiptItems.CurrentItem.IsPartFromListisSerialized = mItems.Item(Index).SerialisedStatus 'mItems.Item(Index).SerialisedStatus
                mReceipt.ReceiptItems.CurrentItem.BaseUnitID = mItems.Item(Index).UnitID   'Added By Prashant 13-May-2010
                mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = mItems.Item(Index).UnitID    'Added By Prashant 16-July-2010
                mReceipt.ReceiptItems.CurrentItem.Location = mItems.Item(Index).Location
                '-----------------Added by Vikrant on 7.3.12 FORALL03052012-------------------------------
                'mReceipt.ReceiptItems.CurrentItem.DisplayQty = CDec(IIf(mItems.Item(Index).SerialisedStatus, 1, 0))  'Added By Prashant 12-May-2010
                If mReceipt.TransTypeID = 9 Then
                    mReceipt.ReceiptItems.CurrentItem.DisplayQty = mQtyRemovedFromAircraft
                    mReceipt.ReceiptItems.CurrentItem.IssueItemID = mItems.Item(Index).IssueItemID
                    If (mItems.Item(Index).PrimaryCategoryID = 1 And AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then
                        mReceipt.ReceiptItems.CurrentItem.RemovedAsReturnableFromAircraft = True
                    End If
                Else
                    mReceipt.ReceiptItems.CurrentItem.DisplayQty = CDec(IIf(mItems.Item(Index).SerialisedStatus, 1, 0))  'Added By Prashant 12-May-2010
                End If
                '-----------------------------------------------------------------------------------------
                'Added By Saylee on 17/07/2008
                'If mItems(Index).ExpiryMonths > 0 Then
                '    mReceipt.ReceiptItems.CurrentItem.StartDate = mReceipt.Receipt.RecdDate
                '    If Not (mReceipt.ReceiptItems.CurrentItem.StartDate) Is System.DBNull.Value Then
                '        mReceipt.ReceiptItems.CurrentItem.ExpiryDate = CDate(mReceipt.ReceiptItems.CurrentItem.StartDate).AddMonths(mItems(Index).ExpiryMonths)
                '    End If
                'End If

                mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = mItems.Item(Index).PrimaryCategoryID 'Added By Prashant 4-Jun-2014 ALL03062014

                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" And mReceipt.TransTypeID = 67 And (mItems.Item(Index).PrimaryCategoryID = 1 Or mItems.Item(Index).PrimaryCategoryID = 2)) Then
                    mReceipt.ReceiptItems.CurrentItem.IsConsiderAsAsset = True
                End If

                mReceipt.ReceiptItems.CurrentItem.ItemTagID = mItems.Item(Index).ItemTagID
                mReceipt.ReceiptItems.CurrentItem.ItemTagName = mItems.Item(Index).ItemTagName
                'mReceipt.ReceiptItems.CurrentItem.IsAirworthinss = mItems.Item(Index).IsAirworthiCheck

                Session("mReceipt") = mReceipt
                Session("TotalCount") = CDec(IIf(mItems.Item(Index).SerialisedStatus, 1, 0)).ToString
                Session("mTotalPendingItemQty") = CDec(IIf(mItems.Item(Index).SerialisedStatus, 1, 0)).ToString

                DataFieldBind(txtName.Text.Trim, txtDescription.Text.Trim)
                Session.Remove("mItems")
                mItems = Nothing
                Session("Pending") = False
                Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=" & "wfReceipt_Ajax.aspx" & "&ChildPage1=" & "wfReceipt_Ajax.aspx")
        End Select
    End Sub
    Private Sub gdvItem_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvItem.Sorting
        mItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItems") = mItems
        gdvItem.DataSource = mItems
        UpdateItemGridView()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        gdvItem.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        FindNow(txtName.Text.Trim, txtDescription.Text.Trim)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If mReceipt.ReceiptItems.CurrentItem.IsNew Then mReceipt.ReceiptItems.Remove(mReceipt.ReceiptItems.CurrentItem)
        RemoveSession()
        mItems = Nothing
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnGridPaging_Click(sender As Object, e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        gdvItem.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        FindNow(txtName.Text.Trim, txtDescription.Text.Trim)
    End Sub
#End Region
End Class