Partial Class wfCommonPartList
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

#Region " Variables and Declarations "
    Dim mItems As Items
    Dim mLookInTypeID As Int16
    Dim mName As String
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If cmblookin.Enabled = True Then
                setFocus(cmblookin)
            End If
            mName = CType(Request.QueryString("Name"), String)
            mLookInTypeID = Val(Request.QueryString("LookinTypeID"))
            txtSearch.Text = mName
            FindNow(mLookInTypeID)
        End If
        'ControlVisibility()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SetObject()
        Session("AddParts") = "True"
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mItems")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub cmblookin_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmblookin.SelectedIndexChanged
        Dim Index As Int32 = cmblookin.SelectedIndex
        txtSearch.Text = ""
        lblFor.Visible = IIf(Index <> 0, True, False)
        txtSearch.Visible = IIf(Index <> 0, True, False)
        If cmblookin.Enabled = True Then
            setFocus(cmblookin)
        End If
    End Sub
    Private Sub dgPartList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPartList.PageIndexChanged
        SetObject()
        dgPartList.CurrentPageIndex = e.NewPageIndex
        dgPartList.DataSource = mItems
        dgPartList.DataBind()
    End Sub
    Private Sub dgPartList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPartList.SortCommand
        mItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItems") = mItems
        dgPartList.DataSource = mItems
        dgPartList.DataBind()
    End Sub
#End Region

#Region " FindNow "
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.CurrentPageIndex = 0
        Dim Index As Int32 = Val(cmblookin.SelectedIndex)
        FindNow(Index)
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
        DataBind()
        lblResult.Text = "List of Parts : " & mItems.Count & " Record(s) found."
    End Sub
#End Region

#Region " DataBind "
    Private Sub GetSession()
        mItems = Session("mItems")
        mLookInTypeID = Session("mLookInTypeID")
        mName = Session("mName")
    End Sub
    Private Sub SetSession()
        Session("mItems") = mItems
        Session("mLookInTypeID") = mLookInTypeID
        Session("mName") = mName
    End Sub
#End Region

#Region " DataBind "
    Private Sub SetObject()
        Dim chkSelect As CheckBox
        Dim item As DataGridItem
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgPartList.Items.Count - 1
        ' Set Selected Notes value  
        For i = 0 To PageItems
            Recordno = i + dgPartList.PageSize * dgPartList.CurrentPageIndex
            item = dgPartList.Items(i)
            chkSelect = CType(item.FindControl("chkSelect"), CheckBox)
            mItems(Recordno).IsSelected = chkSelect.Checked
            mItems(Recordno).MarkClean()
        Next
        Session("mItems") = mItems
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

End Class
