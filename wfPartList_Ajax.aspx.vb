'Created by Utkarsh on 10-Oct-2013

'New Style Conversion By Saylee 10-Jul-2024



Public Class wfPartList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mItemList As ItemList

    Public mDocumentTypeForID As Integer
    Public mAttachToID As Guid
    Public mName, StatusID, ShowNoE As String
    Public Text, Index, IsSerialized As String

    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0

    Public mPartNo, mDescrption, mCategory, mUnit, mLocation
    Public mSerializedStatus As Boolean
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItem = Session("mItem")
        mItemList = Session("mItemList")
        Index = Session("Index")
        Text = Session("Text")
        'New Addition By Yogita on 20-Dec-2007 to solve Bug No:-PL3
        StatusID = Session("StatusId")
        IsSerialized = Session("IsSerialized")

        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")



        mPartNo = Session("mPartNo")
        mDescrption = Session("mDescrption")
        mCategory = Session("mCategory")
        mUnit = Session("mUnit")
        mLocation = Session("mLocation")
        mSerializedStatus = Session("mSerializedStatus")
    End Sub
    Private Sub SetSession()
        Session("mItem") = mItem
        Session("mItemList") = mItemList
        Session("Index") = Index
        Session("Text") = Text
        Session("StatusId") = StatusID
        Session("IsSerialized") = IsSerialized

        Session("mPartNo") = mPartNo
        Session("mDescrption") = mDescrption
        Session("mCategory") = mCategory
        Session("mUnit") = mUnit
        Session("mLocation") = mLocation
        Session("mSerializedStatus") = mSerializedStatus
    End Sub
    'Added by Utkarsh on 10-Oct-2013
    Private Sub RemoveSession()
        Session.Remove("mItem")
        Session.Remove("mItemList")
        Session.Remove("Index")
        Session.Remove("Text")
        Session.Remove("StatusId")
        Session.Remove("MiddleFrame")
        Session.Remove("IsSerialized")

        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")

        Session.Remove("mPartNo")
        Session.Remove("mDescrption")
        Session.Remove("mCategory")
        Session.Remove("mUnit")
        Session.Remove("mLocation")
        Session.Remove("mSerializedStatus")
    End Sub
    'End
    Private Sub NewRecord()
        mItem = Item.NewItem
        Session("mItem") = mItem
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mItem = Item.GetItem(mId)
        mItem.MarkClean()
        Session("mItem") = mItem
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mItem = Item.GetItem(mId)
        Session("mItem") = mItem
    End Sub
    Private Sub DataFieldBind()
        Session("mItemList") = mItemList
        Index = IIf(IsNothing(Index), 0, Index)
        Text = Session("Text")
        StatusID = Session("StatusId")
        Session("Text") = Text
        Session("Index") = Index
        'New Addition By Yogita on 20-Dec-2007 to solve Bug No:-PL3
        Session("StatusId") = StatusID
        Session("IsSerialized") = IsSerialized
        UpdateItemGridView()


        mPartNo = Session("mPartNo")
        mDescrption = Session("mDescrption")
        mCategory = Session("mCategory")
        mUnit = Session("mUnit")
        mLocation = Session("mLocation")
        mSerializedStatus = Session("mSerializedStatus")

        Session("mPartNo") = mPartNo
        Session("mDescrption") = mDescrption
        Session("mCategory") = mCategory
        Session("mUnit") = mUnit
        Session("mLocation") = mLocation
        Session("mSerializedStatus") = mSerializedStatus
    End Sub
    Private Sub ControlVisibility()
        'Changed By Yogita on 20-Dec-2007 to solve Bug No:-PL3
        'lblFor.Visible = IIf(Index <> 0, True, False) And IIf(Index <> 7, True, False)
        'txtSearch.Visible = IIf(Index <> 0, True, False) And IIf(Index <> 7, True, False)
        '  cmbSerialisedStatus.Visible = IIf(Index = 7, True, False)
        upnlSearchCriteria.Update()
    End Sub
    Private Sub FindNow(ByVal Index As Int32, Optional mPartNo As String = "", Optional mDescrption As String = "", Optional mCategory As String = "", Optional mUnit As String = "", Optional mLocation As String = "", Optional StatusID As Integer = -1)
        ''Select Case Index
        ''    Case 0 'All
        ''        mItemList = ItemList.GetItemList(Index, "", "", "", "", "", IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''    Case 1 'PartNo
        ''        mItemList = ItemList.GetItemList(Index, Text, "", "", "", "", IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''    Case 2 'Desc
        ''        mItemList = ItemList.GetItemList(Index, , Text, "", "", "", IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''        'Case 3 'Nomenclature
        ''        '    mItemList = ItemList.GetItemList(Index, , , Text, "", "", IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''    Case 3 'Category
        ''        mItemList = ItemList.GetItemList(Index + 1, , , , Text, "", IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''    Case 4 'Unit
        ''        mItemList = ItemList.GetItemList(Index + 1, "", "", "", "", Text, "", IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''    Case 5 'Location
        ''        mItemList = ItemList.GetItemList(Index + 1, "", "", "", "", "", Text, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''        'New Addition By Yogita on 20-Dec-2007 to solve Bug No:-PL3
        ''    Case 6 'SerializedStatus
        ''mItemList = ItemList.GetItemsList(Index + 3, "", "", "", "", "", Text, False, CInt(StatusID), IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize)
        ''End Select

        mItemList = ItemList.GetItemListOnListPage(ItemName:=mPartNo, ItemDescription:=mDescrption, UnitName:=mUnit, CategoryName:=mCategory, Location:=mLocation, SerializedStatus:=StatusID)
        totalCount = mItemList.TotalRecords
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount

        Session("mItemList") = mItemList
        Session("IsSerialized") = IsSerialized
        gdvItem.DataSource = mItemList
        gdvItem.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        'lblResult.Text = "List of Part as per criteria : " & mItemList.Count & " Record(s) found."
        UpdateItemGridView()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mItem = Session("mItem")
                            Item.DeleteItem(mItem.ID)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                Dim stringInfo As String = ""
                                If ex.Message.Contains("PartOnOfftabItem") Then
                                    If AppSettings("ClientCode") = "IND" Then
                                        stringInfo = "OJS."
                                    Else
                                        stringInfo = "NRC."
                                    End If

                                ElseIf ex.Message.Contains("tabReqItem") Then
                                    stringInfo = "Req. Item."
                                ElseIf ex.Message.Contains("tabCalibrationItem") Then
                                    stringInfo = "Calibration."
                                ElseIf ex.Message.Contains("tabEnquiryItem") Then
                                    stringInfo = "Enquiry."
                                ElseIf ex.Message.Contains("tabExportInvoiceItem") Then
                                    stringInfo = "Export Invoice."
                                ElseIf ex.Message.Contains("tabInvoiceItem") Then
                                    stringInfo = "Invoice."
                                ElseIf ex.Message.Contains("tabIssueItem") Then
                                    stringInfo = "Issue."
                                ElseIf ex.Message.Contains("tabItemApplicable") Then
                                    stringInfo = "Item Applicable."
                                ElseIf ex.Message.Contains("tabKitItem") Then
                                    stringInfo = "Kit Item."
                                ElseIf ex.Message.Contains("tabKit") Then
                                    stringInfo = "Kit."
                                ElseIf ex.Message.Contains("tabMaintenanceInvoice") Then
                                    stringInfo = "Maintenance Invoice."
                                ElseIf ex.Message.Contains("tabMaintenanceKitDetails") Then
                                    stringInfo = "Maintenance Kit Details."
                                ElseIf ex.Message.Contains("tabnWOJobComp") Then
                                    stringInfo = "WO. Job Comp."
                                ElseIf ex.Message.Contains("tabnWOJobSpare") Then
                                    stringInfo = "WO. Job Spare."
                                ElseIf ex.Message.Contains("tabnWOSpareFromWOJobSpare") Then
                                    stringInfo = "WO. Spare From WO.Job Spare."
                                ElseIf ex.Message.Contains("tabnWOTools") Then
                                    stringInfo = "WO. Tools."
                                ElseIf ex.Message.Contains("tabOrderItem") Then
                                    stringInfo = "Order."
                                ElseIf ex.Message.Contains("tabQuotationItem") Then
                                    stringInfo = "Quotation."
                                ElseIf ex.Message.Contains("tabReceiptItem") Then
                                    stringInfo = "Receipt."
                                ElseIf ex.Message.Contains("tabSalesInvoiceItem") Then
                                    stringInfo = "Sales Invoice."
                                ElseIf ex.Message.Contains("tabSalesOrderItem") Then
                                    stringInfo = "Sales Order."
                                ElseIf ex.Message.Contains("tabTaskCardSpares") Then
                                    stringInfo = "Task Card Spares."
                                ElseIf ex.Message.Contains("tabTaskCardTools") Then
                                    stringInfo = "Task Card Tools."
                                ElseIf ex.Message.Contains("tabConditionCheckItem") Then
                                    stringInfo = "Condition Check."
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Part", "Can't delete : " & mItem.Name & " is Currently in use", Util.ErrorType.NoError, mItem.ID, EventLogID)
                                'End
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Part", mItem.Name, Util.ErrorType.NoError, mItem.ID, EventLogID)
                                'End
                            End If
                            FindNow(Index)
                        End Try
                    End If
                Case MsgBoxResult.No
                    FindNow(Index)
                Case MsgBoxResult.Ok ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    FindNow(Index)

                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
                    Session("sender") = ""
                    FindNow(Index)

            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            Response.Redirect("wfPartList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            '    DataFieldBind()
        End If
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfPartList_Ajax.aspx?" Then
            Session.Remove("mItem")
            Session.Remove("mItemList")
            Session.Remove("Text")
            Session.Remove("Index")
            'New Addition By Yogita on 20-Dec-2007 to solve Bug No:-PL3
            Session.Remove("StatusId")
            'Rajnish On 31-03-2008
            'Below code is for Removing the TYPE value From Session which come from Log
            Session.Remove("Type") '
        End If
    End Sub
    Private Sub SetControl()
        Index = Session("Index")
        Text = Session("Text")
        IsSerialized = Session("IsSerialized")
        StatusID = Session("StatuaID")
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, gdvItem.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        If ShowNoE Is Nothing Then
            cmbShowE.SelectedValue = "4"
        Else
            cmbShowE.SelectedValue = ShowNoE
        End If

        If Session("mpageindex") = 0 Then
            mpageindex = gdvItem.PageIndex
            mCurrentpage = mpageindex + 1
        Else
            mpageindex = CInt(Session("mpageindex"))
            mCurrentpage = CInt(Session("mCurrentpage"))
        End If


        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        mPartNo = IIf(Session("mPartNo") Is Nothing, "", Session("mPartNo"))
        mDescrption = IIf(Session("mDescrption") Is Nothing, "", Session("mDescrption"))
        mCategory = IIf(Session("mCategory") Is Nothing, "", Session("mCategory"))
        mUnit = IIf(Session("mUnit") Is Nothing, "", Session("mUnit"))
        mLocation = IIf(Session("mLocation") Is Nothing, "", Session("mLocation"))
        StatusID = IIf(Session("StatusID") Is Nothing, "-1", Session("StatusID"))
        ShowNoE = IIf(cmbShowE.SelectedIndex <= 0, 0, cmbShowE.SelectedValue)

        FindNow(Index, mPartNo, mDescrption, mCategory, mUnit, mLocation, CInt(StatusID))
        'txtSearch.Text = Text
        'cmblookin.SelectedIndex = Index
        cmbSerialisedStatus.SelectedValue = IsSerialized


        txtPartNo.Text = mPartNo
        txtDescription.Text = mDescrption
        txtCategory.Text = mCategory
        txtUnit.Text = mUnit
        txtLocation.Text = mLocation

        ControlVisibility()
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If mItemList.Count = 0 Then
            lblResult.Text = "List of Part as per criteria : " & "0 Record(s) found."
        Else
            '  lblResult.Text = "List of Part as per criteria : " & currentrow + 1 & " to " & currentrow + mItemList.Count & " of " & totalCount & " Record(s) found."
            lblResult.Text = "List of Part as per criteria : " & mItemList.Count & " Record(s) found."

        End If

        'SliderExtender1.Minimum = 1
        'SliderExtender1.Maximum = pagecount
        'Slidercontrol.Text = mCurrentpage
        'txtPageDisplay.Text = mCurrentpage
        'lblpagecount.Text = pagecount
        'If pagecount > 1 Then
        '    PnlPaging.Visible = True
        'Else
        '    PnlPaging.Visible = False
        'End If

        gdvItem.DataBind()
        upnlgrid.Update()
        upnlResult.Update()
    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011
        If Not IsPostBack Then
            'If cmblookin.Enabled = True Then
            '    setFocus(cmblookin)
            'End If
            Session("MiddleFrame") = "wfPartList_Ajax.aspx?"

            If Session("ShowNoE") Is Nothing Then
                cmbShowE.SelectedValue = "4"
                Session("ShowNoE") = cmbShowE.SelectedValue 'Ajay 24-07-2023
                ShowNoE = cmbShowE.SelectedValue
            End If

            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Part") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If

            'DataFieldBind()
            SetControl()
        End If
    End Sub

    Private Sub gdvItem_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvItem.PageIndexChanging
        gdvItem.PageIndex = e.NewPageIndex
        gdvItem.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        mCurrentpage = e.NewPageIndex
        gdvItem.DataSource = mItemList
        Session("mCurrentpage") = mCurrentpage
        gdvItem.DataBind()
        ' FindNow(Index)
    End Sub
    Private Sub gdvItem_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvItem.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument) + gdvItem.PageIndex * gdvItem.PageSize
                Dim mId As Guid = mItemList(index).ID
                If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
                    SetSession()
                    'Changed By Utkarsh On 19-Jul-2011 For All19072011
                    mName = mItemList(index).Name
                    MarkLog(Util.Action.Edit, "Part", User.Identity.Name & " is not Authorized User to Edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    gdvItem.DataSource = mItemList
                    UpdateItemGridView()
                    Exit Sub
                End If
                EditRecord(mId)
                'Changed By Utkarsh On 19-Jul-2011 For All19072011
                MarkLog(Util.Action.Edit, "Part", mItem.Name, Util.ErrorType.NoError, mItem.ID, EventLogID)
                'End
                gdvItem.DataSource = mItemList
                UpdateItemGridView()
                Dim str As String
                str = "openledgersame('wfPartInformation_Ajax.aspx?BackPage=Index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "Del"
                Dim index As Integer = CInt(e.CommandArgument) '+ gdvItem.PageIndex * gdvItem.PageSize
                Dim mId As Guid = mItemList(index).ID
                If (Not User.IsInRole("PartDelete")) Then
                    SetSession()
                    'Changed By Utkarsh On 19-Jul-2011 For All19072011
                    mName = mItemList(index).Name
                    MarkLog(Util.Action.Delete, "Part", User.Identity.Name & " is not Authorized User to Delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    gdvItem.DataSource = mItemList
                    UpdateItemGridView()
                    Exit Sub
                End If
                DeleteRecord(mId)
                gdvItem.DataSource = mItemList
                UpdateItemGridView()
            Case "ViewRec"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim mFileAttachments As New FileAttachments
                Dim mID As Guid
                mID = mItemList(Index).ID 'New Guid(e.CommandArgument.ToString)
                mFileAttachments = FileAttachments.GetChildFileAttachments(mID)
                gdvItem.DataSource = mItemList
                UpdateItemGridView()
                Session("mFileAttachments") = mFileAttachments
                'Session("TransactionNameMarkLog") = "Work Order" 'used for marklog
                'Session("TransactionName") = "Work Order No. & Date"
                'Session("TransactionDetails") = mnWO.WONumber + " & " + mnWO.WODateFormatted.ToString
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)
        End Select
    End Sub
    Private Sub gdvItem_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvItem.Sorting
        mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItemList") = mItemList
        gdvItem.DataSource = mItemList
        UpdateItemGridView()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ' Dim Index As Int32 = Val(cmblookin.SelectedIndex)
        'Index = IIf(cmblookin.SelectedIndex < 0, 0, cmblookin.SelectedIndex)
        'Text = txtSearch.Text.Trim
        IsSerialized = cmbSerialisedStatus.SelectedValue.ToString

        mPartNo = txtPartNo.Text.Trim
        mDescrption = txtDescription.Text.Trim
        mCategory = txtCategory.Text.Trim
        mUnit = txtUnit.Text.Trim
        mLocation = txtLocation.Text.Trim
        ShowNoE = IIf(cmbShowE.SelectedIndex <= 0, 0, cmbShowE.SelectedValue)

        Session("Text") = Text
        Session("Index") = Index

        Session("mPartNo") = mPartNo
        Session("mDescrption") = mDescrption
        Session("mCategory") = mCategory
        Session("mUnit") = mUnit
        Session("mLocation") = mLocation
        Session("ShowNoE") = ShowNoE

        'New Addition By Yogita on 20-Dec-2007 to solve Bug No:-PL3
        If cmbSerialisedStatus.SelectedValue = 1 Then
            StatusID = "1"   'Part is Serialized
        ElseIf cmbSerialisedStatus.SelectedValue = 2 Then
            StatusID = "0"    'Part is Not Serialized
        Else
            StatusID = "-1"    'ALL
        End If
        gdvItem.PageIndex = 0    'Added Code May,23,2007  
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage

        Session("StatusId") = StatusID
        FindNow(Index, mPartNo, mDescrption, mCategory, mUnit, mLocation, StatusID)
        ControlVisibility()
    End Sub
    'Private Sub cmblookin_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmblookin.SelectedIndexChanged
    '    Dim Index As Int32 = cmblookin.SelectedIndex
    '    txtSearch.Text = ""
    '    ControlVisibility(Index)
    '    If cmblookin.Enabled = True Then
    '        setFocus(cmblookin)
    '    End If
    'End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTop.Click 'btnAdd.Click,
        NewRecord()
        If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
            'Changed By Utkarsh On 19-Jul-2011 For All19072011
            MarkLog(Util.Action.[New], "Part", User.Identity.Name & " Is Not Authorized User To add New Part ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        'Changed By Utkarsh On 19-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Part", "", Util.ErrorType.NoError, mItem.ID, EventLogID)
        'End
        Dim str As String
        str = "openledgersame('wfPartInformation_Ajax.aspx?BackPage=wfPartList_Ajax.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click  'btnClose.Click,
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFav.Click
        MarkFavourite(HttpContext.Current.User.Identity.Name, "Part")

    End Sub

    Private Sub hdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFav.Click
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "Part")

    End Sub

    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim ExpiryDateList = ((From res In mWOList).ToList.Take(CInt(DropDownList1.SelectedItem.ToString))).ToList
        gdvItem.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        gdvItem.DataSource = mItemList
        gdvItem.DataBind()

        ControlVisibility()

        upnlgrid.Update()
    End Sub
    'Private Sub btnGridPaging_Click(sender As Object, e As System.EventArgs) Handles btnGridPaging.Click
    '    mCurrentpage = CInt(Slidercontrol.Text.Trim)
    '    mpageindex = mCurrentpage - 1
    '    gdvItem.PageIndex = mpageindex
    '    Session("mpageindex") = mpageindex
    '    Session("mCurrentpage") = mCurrentpage
    '    FindNow(Index)
    'End Sub
#End Region
End Class