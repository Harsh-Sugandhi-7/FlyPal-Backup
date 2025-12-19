Public Class wfHSNACSList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mItem As Item
    Protected mHSNACS As HSNACS
    Protected mHSNACSList As HSNACSList
    Protected mHSNACSChild As HSNACSChild
    Protected mHSNACSHistoryList As HSNACSHistoryList
    Dim mCurrentPageindex As Integer
    Dim UsedIn As String = ""
    Dim EventLogID As Guid
    Dim SearchIndex, DateIndex, FromDate, ToDate, ItemName, Description, SerialNo As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchIndex = Session("SearchIndex")
        ItemName = Session("ItemName")
        Description = Session("Description")
        SerialNo = Session("SerialNo")
        mHSNACSList = Session("mHSNACSList")
    End Sub
    Private Sub SetSession()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mHSNACSList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfHSNACSList_Ajax.aspx" Then
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("mHSNACSList")
            Session.Remove("ItemName")
            Session.Remove("Description")
            Session.Remove("SerialNo")
        End If
    End Sub
    Private Sub NewRecord()

    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        'mOrder = Order.GetOrder(mId)
        'mTransTypeID = mOrder.TransTypeID
        'mOrder.MarkClean()
        ''=======Added By Saylee on 2nd Nov 2007============ In Order to keep selected criteria as it is
        'POrderType = cmbOrderType.SelectedIndex
        'POAgainstType = cmbPOAgainstType.SelectedIndex
        'POFor = cmbFor.SelectedIndex
        'Session("mOrder") = mOrder
        'Session("POrderType") = POrderType
        'Session("POFor") = POFor
        'Session("POAgainstType") = POAgainstType
        ''================================================
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid, ByVal HSNACSChildID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mHSNACS = HSNACS.GetHSNACS(mId)
        mHSNACSChild = HSNACSChild.GetHSNACSChild(HSNACSChildID)
        Session("mHSNACS") = mHSNACS
        Session("mHSNACSChild") = mHSNACSChild
    End Sub
    Private Sub SetControl()
        CallFindNow(SearchIndex)
        dgGridView.DataBind()
        txtCode.Text = ItemName
        txtDescription.Text = Description
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of HSN/SAC as per criteria : " & mHSNACSList.Count & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mHSNACSChild = CType(Session("mHSNACSChild"), HSNACSChild)
                            mHSNACS = CType(Session("mHSNACS"), HSNACS)
                            mHSNACSHistoryList = HSNACSHistoryList.GetHSNACSHistoryList(mHSNACS.ID)
                            If mHSNACSHistoryList.Count = 0 Then
                                HSNACS.DeleteHSNACS(mHSNACS.ID)
                                mHSNACS.Save()
                            End If
                            HSNACSChild.DeleteHSNACSChild(mHSNACSChild.ID)
                            mHSNACSChild.Save()
                            DataFieldBind()
                            SetControl()
                            upnlGridView.Update()
                            upnTopButtons.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                If ex.Message.Contains("FK_tabMROAccountHeads_tabHSNACS") Then
                                    UsedIn = " Account Heads"
                                ElseIf ex.Message.Contains("FKtabItemtabHSNACS") Then
                                    UsedIn = " Part"
                                ElseIf ex.Message.Contains("FK_tabCapabilityTask_tabHSNACS") Then
                                    UsedIn = " Capability Task"
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, UsedIn, MsgBoxStyle.OkOnly, "")
                                UsedIn = ""
                                Exit Sub
                            End If
                        Finally
                            TotalCount()
                            Dim mHSNACSDetail As String = " From Date: " + mHSNACSChild.FromDateFormatted + " Code: " + mHSNACSChild.Code + " Percent: " + mHSNACSChild.GSTPercent.ToString + " User: " + User.Identity.Name
                            MarkLog(Util.Action.Delete, "HSNACS", mHSNACSDetail, Util.ErrorType.NoError, mHSNACSChild.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Code As String = "", Optional ByVal Description As String = "")
        mHSNACSList = Nothing
        dgGridView.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mHSNACSList = HSNACSList.GetHSNACSList(Code, Description)
        'Set DataSource of the Grid
        Session("mHSNACSList") = mHSNACSList
        dgGridView.DataSource = mHSNACSList
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow(txtCode.Text, txtDescription.Text)
        dgGridView.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        txtCode.Text = ""
        txtDescription.Text = ""
    End Sub
    Private Sub addAttributes()
    End Sub
    Private Sub SetTitle()

    End Sub
    Private Function IsInRole() As Boolean
        'Dim IsInRoleString As String = ""
        ''Deciding IsInRole String to check Rights
        'Select Case mTransTypeID
        '    Case Util.Trans.PurchaseOrder
        '        IsInRoleString = "Order"
        '    Case Util.Trans.PurchaseOrderForExchangeRepair
        '        IsInRoleString = "OrderForExchange"
        '    Case Util.Trans.OverHaulRepairOrder
        '        IsInRoleString = "PurchaseOrderRepairOverHaul"
        '    Case Util.Trans.RentialLeaseOtder
        '        IsInRoleString = "PurchaseOrderRentalLease"
        'End Select
        ''Depending upon decided IsInRole String; checkign Rights of the User
        'Select Case CheckFor
        '    Case Rights.[New]
        '        Return User.IsInRole(IsInRoleString + "New")
        '    Case Rights.Edit
        '        Return User.IsInRole(IsInRoleString + "Edit")
        '    Case Rights.Save
        '        Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
        '    Case Rights.Delete
        '        Return User.IsInRole(IsInRoleString + "Delete")
        '    Case Rights.View
        '        Return User.IsInRole(IsInRoleString + "View")
        '    Case Rights.Print
        '        Return User.IsInRole(IsInRoleString + "Print")
        '    Case Rights.FindNow
        '        Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        'End Select
    End Function
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        'FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        'ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)

        'SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        'DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

        'mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        'cmbOrderText.DataSource = mDistinctTextListForOrder

        'POAgainstType = IIf(IsNothing(POAgainstType), 0, POAgainstType)
        'POFor = IIf(IsNothing(POFor), 0, POFor)
        'POrderType = IIf(IsNothing(POrderType), 0, POrderType)

        'Session("mDistinctTextListForOrder") = mDistinctTextListForOrder

        'DataBind()
    End Sub
    Public Sub TotalCount()
        'mTransactionListCount = TransactionListCount.GetTransactionListCountt(, OrderType)
        'Session("mTransactionListCount") = mTransactionListCount
        'lblPurchaseHSNACSList.Text = "List of Purchase Orders" & " [Total No of Record(s):-" & mTransactionListCount(0).Count.ToString & "]"
        'upnlTitle.Update()
    End Sub
    Public Sub GridBind()
        dgGridView.DataBind()
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If txtCode.Enabled = True Then
                setFocus(txtCode)
            End If
            Session("MiddleFrame") = "wfHSNACSList_Ajax.aspx"
            DataFieldBind()
            TotalCount()
            SetControl()
        End If
        SetTitle()
    End Sub
    Private Sub dgGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgGridView.RowCommand
        Select Case e.CommandName
            Case "RenewRecord"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mOldHSNACSChild As HSNACSChild
                Dim mHSNACSChild As HSNACSChild

                mHSNACS = HSNACS.GetHSNACS(mID)
                mOldHSNACSChild = HSNACSChild.GetHSNACSChild(mHSNACSList(mID).HSNACSChildID)
                mHSNACSChild = HSNACSChild.NewComplyHSNACSChild(mHSNACS.ID, mOldHSNACSChild.SrNo + 1, 0.0, New SmartDate(True), New SmartDate(True))
                mHSNACSChild.FromDate = ""
                Session("mHSNACS") = mHSNACS
                Session("mHSNACSChild") = mHSNACSChild
                Dim mHSNACSDetail As String = " From Date : " + mOldHSNACSChild.FromDateFormatted + " Sr. No. " + mOldHSNACSChild.SrNo.ToString
                MarkLog(Util.Action.Comply, "HSNACS", mHSNACSDetail, Util.ErrorType.NoError, mID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHSNACSRenewWindow", "OpenHSNACSRenewWindow();", True)

            Case "DeleteRecord"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                DeleteRecord(mID, mHSNACSList(mID).HSNACSChildID)
            Case "History"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mHSNACSHistoryList = HSNACSHistoryList.GetHSNACSHistoryList(mID)

                Session("mHSNACSHistoryList") = mHSNACSHistoryList
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHSNACSHistoryWindow", "OpenHSNACSHistoryWindow();", True)
        End Select
    End Sub
    Private Sub txtCode_TextChanged(sender As Object, e As System.EventArgs) Handles txtCode.TextChanged, txtDescription.TextChanged
        CallFindNow(0)
        dgGridView.DataBind()
        btnPrintTop.Enabled = IIf(mHSNACSList.Count = 0, False, True)
        btnBottomPrint.Enabled = IIf(mHSNACSList.Count = 0, False, True)
        lblResult.Text = "List of HSN/SAC as per criteria : " & mHSNACSList.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomAddNew.Click, btnAddNewTop.Click
        Session("mHSNACSChild") = Nothing
        Session("mHSNACS") = Nothing
        Session("mHSNACSList") = mHSNACSList
        MarkLog(Util.Action.[New], "HSNACS", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHSNACSWindow", "OpenHSNACSWindow();", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgGridView.PageIndexChanging
        dgGridView.PageIndex = e.NewPageIndex
        dgGridView.DataSource = mHSNACSList
        Session("mHSNACSList") = mHSNACSList
        GridBind()
    End Sub
    Private Sub dgGridView_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgGridView.Sorting
        mHSNACSList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mHSNACSList") = mHSNACSList
        dgGridView.DataSource = mHSNACSList
        GridBind()
    End Sub
    Private Sub hdnBtnCompMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnHSNACS.Click, hdnBtnHSNACSRenew.Click
        mHSNACSList = HSNACSList.GetHSNACSList("", "")
        Session("mHSNACSList") = mHSNACSList
        dgGridView.DataSource = mHSNACSList
        dgGridView.DataBind()
        lblResult.Text = "List of HSN/SAC as per criteria : " & mHSNACSList.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class