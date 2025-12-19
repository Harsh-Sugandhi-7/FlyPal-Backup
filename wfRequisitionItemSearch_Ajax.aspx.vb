'AJAX Conversion By Vikrant On 25-Aug-2014

Public Class wfRequisitionItemSearch_Ajax
    Inherits System.Web.UI.Page

#Region "Variables and Declarations"
    Dim mRequisitionItemListNew As RequisitionItemListNew
    Dim mName As String
    Dim PartNo As String
    Public mRequisitionNew As RequisitionNew
    Dim ItemName As String
    Dim ItemDesc As String
    Dim mTransTypeID As Integer
    Dim mFetchItemByName As FetchItemByName
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mRequisitionItemListNew = Session("mRequisitionItemListNew")
        PartNo = Session("PartNo")
        mRequisitionNew = Session("mRequisitionNew")
        ItemName = Session("ItemName")
        ItemDesc = Session("Description")
        mTransTypeID = Session("TransTypeID")
    End Sub
    Private Sub SetSession()
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub FindNow()
        dgPartList.PageIndex = 0
        mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList(txtPartNo.Text.Trim, Description:=txtDescriptionSearch.Text.Trim)
        dgPartList.DataSource = mRequisitionItemListNew
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        DataBind()
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Then
            dgPartList.Columns(7).Visible = True
        Else
            dgPartList.Columns(7).Visible = False
        End If
        lblResult.Text = "List of Parts : " & mRequisitionItemListNew.Count & " Record(s) found."
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    End Sub
    Private Sub setObject(ByVal Index As Integer)

        If Index <> -1 Then
            Session("AddSingleParts") = "True"
            Session("SelectedRequisitionItem") = mRequisitionItemListNew(Index)

            Session("ItemName") = ""
            Session("Description") = ""
        Else
            Session("ItemName") = txtPartCreate.Text
            Session("Description") = txtDescription.Text
        End If
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    Private Sub ControlVisibility()
        If AppSettings("ShowFirstPriorityParts") = "True" Then
            dgPartList.Columns(4).Visible = True
        Else
            dgPartList.Columns(4).Visible = False
        End If
    End Sub
    'End	
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If Session("AddMultipleParts") = "False" Then
                txtPartNo.Text = mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo
                txtPartNo.Text = PartNo
                ''txtPartCreate.Text = mRequisitionNew.RequisitionItemsNew.CurrentItem.ReqPartNo 'ItemName
                txtPartCreate.Text = mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo 'ItemName
                txtPartCreate.Text = ItemName
                ' ''txtDescription.Text = mRequisitionNew.RequisitionItemsNew.CurrentItem.ReqDescription '.ItemDescription
                txtDescription.Text = mRequisitionNew.RequisitionItemsNew.CurrentItem.Description '.ItemDescription
                txtDescription.Text = ItemDesc
            End If
            If txtPartNo.Enabled = True Then
                txtPartNo.Focus()
            End If
            FindNow()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("ItemID") = "True"
        Session.Remove("mRequisitionItemListNew")
        'Added by vikrant for popup
        'Dim mopenas As String = Request.QueryString("Type")
        'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
        '    Exit Sub
        'End If
        'End
        Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub dgPartList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartList.RowCommand
        Dim Index As Integer
        Select Case e.CommandName
            Case "Select"
                dgPartList.DataSource = mRequisitionItemListNew
                dgPartList.DataBind()
                ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
                Dim ItemId As Guid = New Guid(dgPartList.DataKeys(CInt(e.CommandArgument)).Values(0).ToString)
                Dim NameOfItem As String = dgPartList.DataKeys(CInt(e.CommandArgument)).Values(1).ToString

                mFetchItemByName = FetchItemByName.GetItemByName(NameOfItem)
                If mFetchItemByName(0).NotInUse = True Then
                    If CDate(mFetchItemByName(0).NotInUseDate) <= CDate(mRequisitionNew.ReqDate) Then
                        MSGBoxCtrl.show("Alert!", "Part " + NameOfItem.Trim + " is not applicable since " + mFetchItemByName(0).NotInUseDateFormatted + " <br><br> Select another Part from list & try again.", "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If

                Index = CInt(e.CommandArgument) + dgPartList.PageIndex * dgPartList.PageSize
                Session("ItemID") = "True"
                setObject(Index)
                Session.Remove("mRequisitionItemListNew")
                'Added by vikrant for popup
                'Dim mopenas As String = Request.QueryString("Type")
                'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                '    Exit Sub
                'End If
                'End
                Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage") & "&ItemId=" & ItemId.ToString)
                'setObject(Index)
                'Added By Vikrant On 30-Aug-2016 For ALL30082016
            Case "ShowPartStatus"
                dgPartList.DataSource = mRequisitionItemListNew
                dgPartList.DataBind()
                ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
                Index = CInt(e.CommandArgument)
                Dim PartNoStatus As String = dgPartList.Rows(CInt(e.CommandArgument)).Cells(1).Text
                Dim DescriptionStatus As String = dgPartList.Rows(CInt(e.CommandArgument)).Cells(2).Text
                Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(PartNoStatus)
                Dim ItemIDStatus As Guid
                If mFetchItemByName.Count > 0 Then
                    ItemIDStatus = mFetchItemByName(0).ID
                Else
                    ItemIDStatus = Guid.Empty
                End If

                If Not ItemIDStatus.Equals(Guid.Empty) Then
                    Dim mItemStatus As Item = Item.GetItem(ItemIDStatus)
                    Dim LinkID As Guid = mItemStatus.LinkID
                    Dim Unit As String = mItemStatus.UnitName


                    Dim mStockPartStatus As rptStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
                    Dim mOnOrderPartStatus As rptOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
                    Dim mReturnablePartStatus As rptReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
                    Dim mTransitPartList As rptTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
                    Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode"))

                    Session("PartNoStatus") = PartNoStatus
                    Session("DescriptionStatus") = DescriptionStatus
                    Session("Unit") = Unit

                    Session("mStockPartStatus") = mStockPartStatus
                    Session("mOnOrderPartStatus") = mOnOrderPartStatus
                    Session("mReturnablePartStatus") = mReturnablePartStatus
                    Session("mTransitPartList") = mTransitPartList
                    Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
                    Session("LinkID") = LinkID
                    'Added By Vikrant On 30-Aug-2016 For ALL30082016
                    Dim URL As Stack = New Stack
                    URL.Push(Request.Url)
                    Session("URL") = URL
                    'End
                    Response.Redirect("wfrptShowPartNoStatus_Ajax.aspx?BackPage=wfRequisitionItemSearch_Ajax.aspx")
                Else
                    'Alert Messege-Part Needs To Be Added In Part Master
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Part Needs To Be Added In Part Master.", False), True)
                End If
                'End
        End Select
    End Sub
    Private Sub dgPartList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
        dgPartList.PageIndex = e.NewPageIndex
        dgPartList.DataSource = mRequisitionItemListNew
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        dgPartList.DataBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    End Sub
    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        'If IsValid Then
        Dim mItem As Item
        mItem = Item.NewItem(txtPartCreate.Text, txtDescription.Text, "")
        Session("mItem") = mItem
        Session("mRequisitionNew") = mRequisitionNew
        Session("PartInfo") = "True"

        Dim URL As Stack = New Stack    'STACK to store url of current page
        URL.Push(Request.Url)           'Inserting URL in STACK
        Session("URL") = URL
        Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfRequisitionItemSearch_Ajax.aspx")
        'End If
    End Sub
    Private Sub dgPartList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartList.Sorting
        mRequisitionItemListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        dgPartList.DataSource = mRequisitionItemListNew
        dgPartList.DataBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    End Sub
    Private Sub dgPartList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgPartList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If (e.Row.Cells(8).Text <> "" And e.Row.Cells(8).Text <> "&nbsp;") Then 'Contracted With Supplier
                e.Row.Cells(8).Font.Bold = True 'Contracted With Supplier
                e.Row.Cells(8).BackColor = Color.Olive   'Contracted With Supplier
            End If
            'Added by Vikrant On 11-Jul-2019 For ALL11072019	
            If AppSettings("ShowFirstPriorityParts") = "True" AndAlso (e.Row.Cells(3).Text <> "" And e.Row.Cells(3).Text <> "&nbsp;") And (e.Row.Cells(1).Text <> e.Row.Cells(4).Text) Then
                e.Row.Cells(4).Font.Bold = True
            End If
            'End
        End If
    End Sub

#End Region

End Class