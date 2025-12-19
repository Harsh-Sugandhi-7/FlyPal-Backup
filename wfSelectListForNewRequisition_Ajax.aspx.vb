'AJAX Conversion By Vikrant on 26-Aug-2014
Public Class wfSelectListForNewRequisition_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private WOText As String
    Private WONo As Integer = 0
    Private RegNo As String
    Dim mNWOList As nWOList
    Dim mRequisitionNew As RequisitionNew
    Dim mDistinctWOText As nDistinctWOText
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        RegNo = Session("RegNo")
        WOText = Session("WOText")
        mNWOList = Session("mNWOList")
        mRequisitionNew = Session("mRequisitionNew")
    End Sub
    Private Sub DataFieldBind()

        mDistinctWOText = nDistinctWOText.GetDistinctWOText("(ALL)")
        cmbWO.DataSource = mDistinctWOText
        cmbWO.DataBind()

        mNWOList = nWOList.GetWOList("", WONo, , mRequisitionNew.ReqDateFormatted, RegNo, , 2, 1)
        dgWOList.DataSource = mNWOList
        Session("mNWOList") = mNWOList
        dgWOList.DataBind()
        lblResult.Text = "List Having " & mNWOList.Count.ToString & " Work Order(s)." & " (Aircraft : " & IIf(RegNo = "", "All", RegNo) & ")"
        If Request.QueryString("OpenFrom") = "RequisitionDetailPage" Then
            'Do nothing
        Else
            dgWOList.Columns(5).Visible = False
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub dgWOList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
        Dim Index As Integer
        Dim ID As Guid
        Dim MachineID As Guid
        Dim WONo As String
        Select Case e.CommandName
            Case "Select"
                dgWOList.DataSource = mNWOList
                dgWOList.DataBind()

                Index = CInt(e.CommandArgument)
                ID = New Guid(dgWOList.DataKeys(Index).Values(0).ToString)
                MachineID = New Guid(dgWOList.DataKeys(Index).Values(1).ToString)
                WONo = dgWOList.Rows(Index).Cells(3).Text

                Session("ID") = ID
                'Session("No") = WONo
                Session("WONo") = WONo ''Ajay 01-02-2023
                Session("WOMachineID") = MachineID
                Session.Remove("mNWOList")
                'Added By Prashant 17-Feb-2020
                If Request.QueryString("OpenFrom") = "RequisitionDetailPage" Then
                    Dim mnWO As nWO
                    mnWO = nWO.GetWO(ID)
                    For i As Integer = 0 To mnWO.WOJobs.Count - 1
                        For j As Integer = 0 To mnWO.WOJobs(i).WOJobSpares.Count - 1
                            'If Not ReqItemIds.ToString.TrimEnd(",").Contains(mnWO.WOJobs(i).WOJobSpares(j).ItemID.ToString) Then '12-Jun-2019
                            Dim mItemList As ItemList
                            mItemList = ItemList.GetItemList(1, ItemName:=mnWO.WOJobs(i).WOJobSpares(j).PartNo)
                            If mItemList.Count > 0 Then
                                If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then
                                    mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = mnWO.WOJobs(i).WOJobSpares(j).RequiredQty
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItemList(0).UnitID
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItemList(0).UnitName
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItemList(0).IsOneTimePurchase
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
                                    mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber

                                    If Not mItemList(0).IsOneTimePurchase Then
                                        mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItemList(0).MinStockLevel
                                        mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItemList(0).MaxStockLevel
                                        mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItemList(0).MinReOrderLevel
                                    Else
                                        mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
                                        mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
                                        mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
                                    End If
                                Else
                                    mRequisitionNew.RequisitionItemsNew(mItemList(0).ID, "").RequestedQty += mnWO.WOJobs(i).WOJobSpares(j).RequiredQty
                                End If
                            End If
                            'End If
                        Next
                    Next
                    Session("mRequisitionNew") = mRequisitionNew
                End If
                '-----------------------------
                'Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage"))
                'Added by vikrant for popup
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                'End
            Case "SparesView"
                dgWOList.DataSource = mNWOList
                dgWOList.DataBind()
                Index = CInt(e.CommandArgument)
                ID = New Guid(dgWOList.DataKeys(Index).Values(0).ToString)
                Dim mnWOJobSpares As nWOJobSpares
                mnWOJobSpares = nWOJobSpares.GetWOSpares(ID, "")
                Session("mnWOJobSpares") = mnWOJobSpares
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSpareListWindow", "OpenSpareListWindow();", True)
        End Select
    End Sub
    Private Sub dgWOList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
        dgWOList.PageIndex = e.NewPageIndex
        dgWOList.DataSource = mNWOList
        Session("mNWOList") = mNWOList
        dgWOList.DataBind()

        If Request.QueryString("OpenFrom") = "RequisitionDetailPage" Then
            'Do nothing
        Else
            dgWOList.Columns(5).Visible = False
        End If
    End Sub
    Private Sub dgWOList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting
        mNWOList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mNWOList") = mNWOList
        dgWOList.DataSource = mNWOList
        dgWOList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("RegNo")
        Session.Remove("WONo")
        Session.Remove("mNWOList")
        'Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage"))
        'Added by vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click
        mNWOList = nWOList.GetWOList(IIf(cmbWO.SelectedIndex = 0, "", cmbWO.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text.Trim)), , _
                                     mRequisitionNew.ReqDateFormatted, txtRegNo.Text.Trim, , 2, 1)
        dgWOList.DataSource = mNWOList
        Session("mNWOList") = mNWOList
        dgWOList.DataBind()
        lblResult.Text = "List Having " & mNWOList.Count.ToString & " Work Order(s)." & " (Aircraft : " & IIf(txtRegNo.Text.Trim = "", "All", txtRegNo.Text.Trim) & ")"
        If Request.QueryString("OpenFrom") = "RequisitionDetailPage" Then
            'Do nothing
        Else
            dgWOList.Columns(5).Visible = False
        End If
        upnlWODetails.Update()
    End Sub
#End Region


   
End Class