'AJAX Conversion By Vikrant On 09-July-2014
Public Class wfCommonPartList_Ajax
    Inherits System.Web.UI.Page

#Region " Variables and Declarations "
    Dim mItems As Items
    Dim mLookInTypeID As Int16
    Dim mName As String
    Dim OpenFrom As String
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If cmblookin.Enabled = True Then
                cmblookin.Focus()
            End If
            mName = CType(Request.QueryString("Name"), String)
            mLookInTypeID = Val(Request.QueryString("LookinTypeID"))
            txtSearch.Text = mName
            OpenFrom = CType(Request.QueryString("OpenFrom"), String)
            Session("OpenFrom") = OpenFrom
            FindNow(mLookInTypeID)
            ControlVisibility()
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SetObject()
        Session("AddParts") = "True"
        Session("TransactionDate") = txtTransactionDate.Text
        Session.Remove("OpenFrom")
        Session.Remove("ItemsCount")
        'Added by vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mItems")
        Session.Remove("OpenFrom")
        Session.Remove("ItemsCount")
        'Added by vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        Session("IsBackFromPendingList") = "True"
        Response.Redirect(Request.QueryString("BackPage1"))
    End Sub
    Private Sub dgPartList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
        SetObject()
        dgPartList.PageIndex = e.NewPageIndex
        dgPartList.DataSource = mItems
        dgPartList.DataBind()
    End Sub
    Private Sub dgPartList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartList.Sorting
        mItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItems") = mItems
        dgPartList.DataSource = mItems
        dgPartList.DataBind()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.PageIndex = 0
        Dim Index As Int32 = Val(cmblookin.SelectedIndex)
        FindNow(Index)
    End Sub
    Private Sub btnAddNewPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewPart.Click
        If (Not User.IsInRole("PartNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Dim mItem As Item
            mItem = Item.NewItem()
            Session("mItem") = mItem
            Session("Create") = "False"
            Session("PartInfo") = "True"

            Dim URL As Stack = New Stack    'STACK to store url of current page
            URL.Push(Request.Url)           'Inserting URL in STACK
            Session("URL") = URL
            Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfCommonPartList_Ajax.aspx")
        End If
    End Sub
#End Region

#Region " Methods "
    Private Sub GetSession()
        mItems = Session("mItems")
        mLookInTypeID = Session("mLookInTypeID")
        mName = Session("mName")
        OpenFrom = Session("OpenFrom")
    End Sub
    Private Sub ControlVisibility()
        If OpenFrom = "Quotation" Then
            lblDate.Visible = True
            txtTransactionDate.Visible = True
            txtTransactionDate.Text = Request.QueryString("TransDate")
            txtTransactionDate.Enabled = IIf(CType(IIf(Request.QueryString("ItemsCount") Is Nothing, 0, Request.QueryString("ItemsCount")), Integer) > 0, False, True)
        Else
            lblDate.Visible = False
            txtTransactionDate.Visible = False
        End If
    End Sub
    Private Sub SetSession()
        Session("mItems") = mItems
        Session("mLookInTypeID") = mLookInTypeID
        Session("mName") = mName
    End Sub
    Private Sub SetObject()
        Dim checkString = Request.Form("chkSelect")
        ' Set Selectedvalue  
        If Not checkString Is Nothing Then
            Dim values = checkString.Split(","c)
            For Each value As String In values
                If mItems.Contains(New Guid(value)) Then
                    mItems(New Guid(value)).IsSelected = True
                End If
                Session("mItems") = mItems
            Next
            'For i As Integer = 0 To dgPartList.Rows.Count - 1
            '    Recordno = i + dgPartList.PageSize * dgPartList.PageIndex
            '    chkSelect = CType(dgPartList.Rows(i).FindControl("chkSelect"), CheckBox)
            '    mItems(Recordno).IsSelected = chkSelect.Checked
            '    mItems(Recordno).MarkClean()
            'Next
            'Session("mItems") = mItems
        End If
    End Sub
    Private Sub FindNow(ByVal Index As Int32)
        Select Case Index
            Case 0 'All
                mItems = Flypal.Items.GetItems(Index, "", "", "", "", "")
            Case 1 'PartNo
                mItems = Flypal.Items.GetItems(Index, txtSearch.Text.Trim, "", "", "", "")
            Case 2 'Desc
                mItems = Flypal.Items.GetItems(Index, , txtSearch.Text.Trim, "", "", "")
            Case 3 'Nomenclature
                mItems = Flypal.Items.GetItems(Index, , , txtSearch.Text.Trim, "", "")
            Case 4 'Category
                mItems = Flypal.Items.GetItems(Index, , , , txtSearch.Text.Trim, "")
                'Case 5 'Unit
                '    mItems = Items.GetItems(Index, "", "", "", "", txtSearch.Text.Trim, "")
                'Case 6 'Location
                '    mItems = Items.GetItems(Index, "", "", "", "", "", txtSearch.Text.Trim)
        End Select

        dgPartList.DataSource = mItems
        Session("mItems") = mItems
        dgPartList.DataBind()
        lblResult.Text = "List of Parts : " & mItems.Count & " Record(s) found."
        upnlPartDetails.Update()
    End Sub
#End Region
End Class