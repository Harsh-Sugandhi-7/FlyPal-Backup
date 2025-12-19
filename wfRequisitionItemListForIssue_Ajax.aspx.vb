'AJAX Conversion By Vikrant

Public Class wfRequisitionItemListForIssue_Ajax
    Inherits Web.UI.Page

#Region "Variable Declaration"
    Public mIssue As Issue
    Public mPendingRequisitionListNew As PendingRequisitionListNew
    Public RequisitionID As Guid
    Public mPendingRequisitionItemListForRequisition As PendingRequisitionItemListForRequisition
    Public mPendingRequisitionItemListForRequisitionInfo As PendingRequisitionItemListForRequisition.PendingRequisitionItemListForRequisitionInfo
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mIssue = Session("mIssue")
        mPendingRequisitionListNew = CType(Session("mPendingRequisitionListNew"), PendingRequisitionListNew)
        mPendingRequisitionItemListForRequisition = Session("mPendingRequisitionItemListForRequisition")
    End Sub
    Private Sub setSession()
        Session("mIssue") = mIssue
        Session("mPendingRequisitionListNew") = mPendingRequisitionListNew
        Session("mPendingRequisitionItemListForRequisition") = mPendingRequisitionItemListForRequisition
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mIssue")
    End Sub
    Public Sub setObject(Index As Integer)
        mPendingRequisitionItemListForRequisition = Session("mPendingRequisitionItemListForRequisition")
        mPendingRequisitionItemListForRequisitionInfo = mPendingRequisitionItemListForRequisition.Item(Index)
        'mIssue.IssueItems.CurrentItem.DisplayQty = mPendingRequisitionItemListForRequisitionInfo.IssueBalQty  'mPendingRequisitionItemListForRequisitionInfo.RequestedQty
        mIssue.IssueItems.CurrentItem.ItemID = mPendingRequisitionItemListForRequisitionInfo.ItemID
        mIssue.IssueItems.CurrentItem.RequisitionItemID = mPendingRequisitionItemListForRequisitionInfo.ID
        mIssue.IssueItems.CurrentItem.RequisitionItemTypeID = mPendingRequisitionItemListForRequisitionInfo.RequisitionItemTypeID
        mIssue.IssueItems.CurrentItem.RequisitionItemTypeName = mPendingRequisitionItemListForRequisitionInfo.RequisitionItemTypeName


        mIssue.ToTypeID = 18
        mIssue.RequisitionID = mPendingRequisitionItemListForRequisitionInfo.ReqID

        mIssue.ReqEmployeeID = mPendingRequisitionItemListForRequisitionInfo.ReqEmployeeID          'Added By Prashant 2-Apr-2019 ALL02042019
        ''mIssue.ReqEmployeeName = mPendingRequisitionItemListForRequisitionInfo.ReqEmployeeName      'Added By Prashant 2-Apr-2019 ALL02042019
        mIssue.ReqEmployeeName = mPendingRequisitionItemListForRequisitionInfo.EmpNoName      'Added By Prashant 2-Apr-2019 ALL02042019

        If mPendingRequisitionItemListForRequisitionInfo.WOID.Equals(Guid.Empty) Then 'Added By Prashant 24-Jun-2019 'Spare against CAMO work order
            'Do nothing 
        Else
            mIssue.nWOID = mPendingRequisitionItemListForRequisitionInfo.WOID
            mIssue.IssueTo = mPendingRequisitionItemListForRequisitionInfo.WONo
        End If                                                                        'End of added By Prashant 24-Jun-2019 'Spare against CAMO work order

        If mPendingRequisitionItemListForRequisitionInfo.WOToolsID.Equals(Guid.Empty) Then 'Added By Prashant On 25-Jul-2023
            'Do nothing 
        Else
            mIssue.IssueItems.CurrentItem.WOReqPartID = mPendingRequisitionItemListForRequisitionInfo.WOToolsID
        End If


        If Not mPendingRequisitionItemListForRequisitionInfo.MachineID.Equals(Guid.Empty) Then
            mIssue.MachineID = mPendingRequisitionItemListForRequisitionInfo.MachineID
            mIssue.IssueTo = mPendingRequisitionItemListForRequisitionInfo.RegNo
        End If
        If Not mPendingRequisitionItemListForRequisitionInfo.WorkShopID.Equals(Guid.Empty) Then
            mIssue.WorkShopID = mPendingRequisitionItemListForRequisitionInfo.WorkShopID
            mIssue.IssueTo = mPendingRequisitionItemListForRequisitionInfo.WorkShopName
        End If
        'Added By Vikrant On 08-May-2019 For BA07052019
        'mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingRequisitionItemListForRequisitionInfo.ReqItemUnitID
        'mIssue.IssueItems.CurrentItem.DisplayUnitName = mPendingRequisitionItemListForRequisitionInfo.ReqItemUnit
        'End
        'Added By Prashant on 17-May-2021 ALL17052021
        mIssue.ToolsIssuedToEmployeeID = mPendingRequisitionItemListForRequisitionInfo.ReqEmployeeID
        mIssue.ToolsIssuedToEmployeeName = mPendingRequisitionItemListForRequisitionInfo.EmpNoName
        'End of Added By Prashant on 17-May-2021 ALL17052021

        'Added By Vikrant On 08-Nov-2021 For IRM08112021-1
        mIssue.ReqTextNo = mPendingRequisitionItemListForRequisitionInfo.RequisitionNo
        mIssue.ReqDate = mPendingRequisitionItemListForRequisitionInfo.ReqDateFormatted.ToString
        'End

        Session("PartNo") = mPendingRequisitionItemListForRequisitionInfo.PartNo
        Session("RequiredQty") = mPendingRequisitionItemListForRequisitionInfo.RequestedQty
        Session("PendingIssuedQty") = mPendingRequisitionItemListForRequisitionInfo.IssueBalQty 'check
        Session("PendingIssuedQtyUnit") = mPendingRequisitionItemListForRequisitionInfo.ReqItemUnitID.ToString  'Added By Vikrant On 08-May-2019 For BA07052019
        Session("mIssue") = mIssue

    End Sub
    Private Sub SetTitle()
        ' ''If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
        ' ''    lblResult.Text = "List of Engineering Order as per criteria : " & mnPendingWOListForIssueSpares.Count & " Record(s) found."
        ' ''    dgRequisitionList.Columns(1).HeaderText = "E.O. No."
        ' ''    dgRequisitionList.Columns(2).HeaderText = "E.O.Date"
        ' ''    '' dgRequisitionList.DataBind()
        ' ''Else
        ' ''    lblResult.Text = "List of W.O. as per criteria :" & mnPendingWOListForIssueSpares.Count & " Record(s) found."
        ' ''    dgRequisitionList.Columns(1).HeaderText = "W.O. No."
        ' ''    dgRequisitionList.Columns(2).HeaderText = "W.O.Date"
        ' ''    '' dgRequisitionList.DataBind()
        ' ''End If
        lblResult.Text = "As per criteria : " & mPendingRequisitionListNew.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        dgItemsList.Columns(4).Visible = IIf(mIssue.TransTypeID = 14, True, False)
        dgItemsList.Columns(5).Visible = IIf(mIssue.TransTypeID = 44, True, False)
    End Sub
    'Added By Vikrant On 07-Oct-2014 For 
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'Added By Vikrant On 07-Oct-2014 For ALL07102014
                    If MSGBoxCtrl.Sender = "SaveItemMaster" Then
                        Try
                            Session("Sender") = ""
                            Dim index As Integer = Session("Index")
                            setObject(index)
                            Session("mIssue") = mIssue
                            Session("PartInfo") = "True"
                            Dim URL As Stack = New Stack
                            URL.Push("wfPartStockStatus_Ajax.aspx?ChildPage=wfIssueItem_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage1=wfRequisitionItemListForIssue_Ajax.aspx" & "&Name=" & Server.UrlEncode(Session("PartNo")))
                            Session("URL") = URL
                            Session("RequisitionItemID") = mIssue.IssueItems.CurrentItem.RequisitionItemID
                            Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfRequisitionItemListForIssue_Ajax.aspx")
                        Catch ex As SqlException

                        End Try
                    End If
                    'End
                Case MsgBoxResult.No
                    'Added By Vikrant On 07-Oct-2014 For ALL07102014
                    If CType(Session("sender"), String) = "SaveItemMaster" Then
                        Session("sender") = ""
                        'DataFieldBind()
                        'Response.Redirect("wfRequisitionItemListForIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                    'End
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfRequisitionItemListForIssue_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub GridBind(Optional ReqList As Boolean = False, Optional ReqItemList As Boolean = False)
        If ReqList Then
            dgRequisitionList.DataSource = mPendingRequisitionListNew
            dgRequisitionList.DataBind()
        End If
        If ReqItemList Then
            dgItemsList.DataSource = mPendingRequisitionItemListForRequisition
            dgItemsList.DataBind()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'txtDate.Text = mIssue.IDate
        mPendingRequisitionListNew = PendingRequisitionListNew.GetPendingRequisitionListNew(, , txtDate.Text, mIssue.MachineID.ToString,
            mIssue.TransTypeID, mIssue.WorkShopID.ToString, mIssue.RequisitionID.ToString, ClientCode:=AppSettings("ClientCode")) 'MachineID 'Added By Prashant 20-Aug-2014 ALL20082014
        Session("mPendingRequisitionListNew") = mPendingRequisitionListNew
        dgRequisitionList.DataSource = mPendingRequisitionListNew
        DataBind()
    End Sub
#End Region

#Region "Events"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        getSession()

        If Not IsPostBack Then
            If txtDate.Text = "" Then
                txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            End If
            DataFieldBind()

            If mIssue.IssueItems.Count - 1 = 0 Then
                txtDate.Enabled = True
            Else
                txtDate.Enabled = False
            End If
            SetTitle()
            ControlVisibility()
        End If
    End Sub

    Private Sub dgRequisitionList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgRequisitionList.RowCommand

        Select Case e.CommandName
            Case "Select"
                GridBind(ReqList:=True)
                RequisitionID = New Guid(dgRequisitionList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                mPendingRequisitionItemListForRequisition = PendingRequisitionItemListForRequisition.GetnPendingRequisitionItemListForRequisition(RequisitionID, mIssue.MachineID.ToString, mIssue.TransTypeID, mIssue.WorkShopID.ToString, ClientCode:=AppSettings("ClientCode"))
                Session("mPendingRequisitionItemListForRequisition") = mPendingRequisitionItemListForRequisition
                dgItemsList.DataSource = mPendingRequisitionItemListForRequisition
                dgItemsList.DataBind()
                ControlVisibility()
                SetTitle()
                lblResult.Text = "As per criteria : " & mPendingRequisitionListNew.Count & " Record(s) found."

                lblResult1.Text = "As per criteria :" & mPendingRequisitionItemListForRequisition.Count & " Record(s) found."
                upnlReqItemListDetails.Update()
        End Select
    End Sub

    Private Sub Date_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged

        If mIssue.IsNew Then
            mIssue.IDate = CDate(txtDate.Text)
        End If

        dgRequisitionList.PageIndex = 0
        mPendingRequisitionListNew = PendingRequisitionListNew.GetPendingRequisitionListNew(, ,
                                                                                            txtDate.Text, ,
                                                                                            mIssue.TransTypeID,
                                                                                            ClientCode:=AppSettings("ClientCode"))

        Session("mPendingRequisitionListNew") = mPendingRequisitionListNew
        dgRequisitionList.DataSource = mPendingRequisitionListNew
        SetTitle()
        dgRequisitionList.DataBind() 'Added By Prashant 20-Aug-2014 ALL20082014

    End Sub

    Private Sub ItemsList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgItemsList.RowCommand

        Select Case e.CommandName

            Case "Select"

                Dim Index As Integer = CInt(e.CommandArgument) + dgItemsList.PageIndex * dgItemsList.PageSize

                GridBind(ReqItemList:=True)
                Session("Index") = Index

                'Added By Vikrant On 07-Oct-2014 For ALL07102014
                Dim ItemID As Guid = Guid.Empty
                Dim mFetchItemByName As FetchItemByName = FetchItemByName.
                                                            GetItemByName(mPendingRequisitionItemListForRequisition(Index).PartNo)
                If mFetchItemByName.Count > 0 Then

                    ItemID = mFetchItemByName(0).ID

                End If

                If ItemID.Equals(Guid.Empty) Then

                    Dim mItem As Item
                    mItem = Item.NewItem(mPendingRequisitionItemListForRequisition(Index).PartNo, mPendingRequisitionItemListForRequisition(Index).Description, mPendingRequisitionItemListForRequisition(Index).IPCReference)
                    Session("mItem") = mItem

                    MSGBoxCtrl.Show("Alert", "Part not added in Part Master", "Do you want to add it in Part Master", MsgBoxStyle.YesNo, "SaveItemMaster")
                    Exit Sub

                Else 'End

                    setObject(Index)
                    Session("mIssue") = mIssue
                    If mIssue.TransTypeID = 79 And mIssue.ToTypeID = 18 Then 'Added By Prashant on 17-May-2021 ALL17052021
                        Response.Redirect("wfPartStockStatus_Ajax.aspx?ChildPage=wfToolsCheckOut_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage1=wfRequisitionItemListForIssue_Ajax.aspx" & "&Name=" & Server.UrlEncode(Session("PartNo")))
                    Else
                        Response.Redirect("wfPartStockStatus_Ajax.aspx?ChildPage=wfIssueItem_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage1=wfRequisitionItemListForIssue_Ajax.aspx" & "&Name=" & Server.UrlEncode(Session("PartNo")))
                    End If

                End If

        End Select

    End Sub

    Private Sub Back(sender As Object, e As EventArgs) Handles btnBack.Click
        If Request.QueryString("BackPage") = "wfIssue_Ajax.aspx" Then
            mIssue.IssueItems.RemoveAt(mIssue.IssueItems.CurrentIndex)
            Session("Edit") = False
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            Response.Redirect("Index.aspx")
        End If
    End Sub

    Private Sub RequisitionList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgRequisitionList.Sorting
        mPendingRequisitionListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgRequisitionList.DataSource = mPendingRequisitionListNew
        Session("mPendingRequisitionListNew") = mPendingRequisitionListNew
        dgRequisitionList.DataBind()
    End Sub

    Private Sub ItemsList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgItemsList.Sorting
        mPendingRequisitionItemListForRequisition.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgItemsList.DataSource = mPendingRequisitionItemListForRequisition
        Session("mPendingRequisitionItemListForRequisition") = mPendingRequisitionItemListForRequisition
        dgItemsList.DataBind()
    End Sub

    Private Sub RequisitionList_PageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles dgRequisitionList.PageIndexChanging
        dgRequisitionList.PageIndex = e.NewPageIndex
        'lblResult1.Visible = True
        dgRequisitionList.DataSource = mPendingRequisitionListNew
        mPendingRequisitionListNew = Session("mPendingRequisitionListNew")
        dgRequisitionList.DataBind()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

#End Region


End Class