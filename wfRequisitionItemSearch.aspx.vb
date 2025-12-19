'Added by vikrant For New Requisition

Partial Class wfRequisitionItemSearch
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button3 As System.Web.UI.WebControls.Button
    Protected WithEvents Button4 As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variables and Declarations"
    Dim mRequisitionItemListNew As RequisitionItemListNew
    Dim mName As String
    Dim PartNo As String
    Public mRequisitionNew As RequisitionNew
    Dim ItemName As String
    Dim ItemDesc As String
    Dim OpeningFor As Integer
#End Region

#Region " Business Method "
    Private Sub GetSession()
        mRequisitionItemListNew = Session("mRequisitionItemListNew")
        PartNo = Session("PartNo")
        mRequisitionNew = Session("mRequisitionNew")
        ItemName = Session("ItemName")
        ItemDesc = Session("Description")
        OpeningFor = Session("OpeningFor")
    End Sub
    Private Sub SetSession()
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        Session("mRequisitionNew") = mRequisitionNew
    End Sub
    Private Sub FindNow()
        dgPartList.CurrentPageIndex = 0
        mRequisitionItemListNew = RequisitionItemListNew.GetRequisitionItemList(txtPartNo.Text.Trim)
        dgPartList.DataSource = mRequisitionItemListNew
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        DataBind()
        SetSessionForNewPart()
        lblResult.Text = "List of Parts : " & mRequisitionItemListNew.Count & " Record(s) found."
    End Sub
    Private Sub SetMultipleObject()
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
            mRequisitionItemListNew(Recordno).IsSelect = chkSelect.Checked
            mRequisitionItemListNew(Recordno).MarkClean()
        Next
        'For I As Integer = 0 To dgPartList.Items.Count - 1
        '    chkSelect = CType(dgPartList.Items(I).FindControl("chkSelect"), CheckBox)
        '    'If mRequisitionItemList(I).IsSelected And chkSelect.Checked Then
        '    '    mRequisitionItemList(I).IsSelected = chkSelect.Checked
        '    '    mRequisitionItemList(I).MarkClean()
        '    'ElseIf mRequisitionItemList(I).IsSelected And Not chkSelect.Checked Then
        '    '    mRequisitionItemList(I).IsSelected = Not chkSelect.Checked
        '    '    mRequisitionItemList(I).MarkClean()
        '    'Else
        '    '    mRequisitionItemList(I).IsSelected = chkSelect.Checked
        '    'End If
        '    mRequisitionItemList.Item(I).IsSelect = chkSelect.Checked
        '    mRequisitionItemList.Item(I).MarkClean()
        'Next
        Session("Description") = txtDescription.Text
        Session("ItemName") = txtPartCreate.Text
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        Session("AddMultipleParts") = "True"

    End Sub
    Private Sub setObject(ByVal Index As Integer)

        If Index <> -1 Then

            'With mRequisitionItemListNew(Index)
            '    If OpeningFor = 1 Then
            '        .ReqItemID = .ItemID
            '        .ReqPartNo = .ItemName
            '        .ReqDescription = .ItemDescription
            '        .JobDescription = .JobDescription
            '        .IPCReference = .IPCReference
            '        .RequestedQty = .RequestedQty
            '    ElseIf OpeningFor = 2 Then
            '        .ItemID = .ItemID
            '        .ItemName = .ItemName
            '        .ItemDescription = .ItemDescription
            '        .JobDescription = .JobDescription
            '        .IPCReference = .IPCReference
            '    End If
            'End With
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
    Private Sub ControlVisibility()
        If Session("AddMultipleParts") = "False" Then
            dgPartList.Columns(1).Visible = False
            dgPartList.Columns(5).Visible = True
            btnOk.Visible = False
            btnCreate.Visible = True
            Label1.Visible = True
            lblPartCreate.Visible = True
            txtPartCreate.Visible = True
            lblDescription.Visible = True
            txtDescription.Visible = True
        Else
            dgPartList.Columns(1).Visible = True
            dgPartList.Columns(5).Visible = False
            btnOk.Visible = True
            btnCreate.Visible = False
            Label1.Visible = False
            lblPartCreate.Visible = False
            txtPartCreate.Visible = False
            lblDescription.Visible = False
            txtDescription.Visible = False
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetSessionForNewPart()
        If dgPartList.Items.Count <= 0 And Session("AddMultipleParts") = "False" Then
            Session("Create") = "True"
        Else
            Session("Create") = "False"
        End If
    End Sub
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
                setFocus(txtPartNo)
            End If
            ' ''Datafieldbind()
            FindNow()
        End If
        ControlVisibility()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Session("ItemID") = "True"
        SetMultipleObject()
        Session("AddParts") = "True"
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'If Session("Add") = True Then
        '    If mRequisitionNew.RequisitionItemsNew.CurrentItem.IsNew Then
        '        mRequisitionNew.RequisitionItemsNew.Remove(mRequisitionNew.RequisitionItemsNew.CurrentItem)
        '    End If
        'End If
        Session("ItemID") = "True"
        Session.Remove("mRequisitionItemListNew")
        '' Session("Add") = False
        Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub dgPartList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartList.ItemCommand
        Dim Index As Integer = e.Item.ItemIndex + dgPartList.CurrentPageIndex * dgPartList.PageSize
        Select Case e.CommandName
            Case "Select"
                Session("ItemID") = "True"
                Dim ItemId As Guid = New Guid(e.Item.Cells(0).Text)
                setObject(Index)
                Session.Remove("mRequisitionItemListNew")
                Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage") & "&ItemId=" & ItemId.ToString)
                setObject(Index)
        End Select
    End Sub
    Private Sub dgPartList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPartList.PageIndexChanged
        If Session("AddMultipleParts") = "True" Then
            SetMultipleObject()
        End If
        dgPartList.CurrentPageIndex = e.NewPageIndex
        dgPartList.DataSource = mRequisitionItemListNew
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        dgPartList.DataBind()
        SetSessionForNewPart()
    End Sub
    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        ' ''If IsValid Then
        ' ''    If Session("Create") = "True" Then
        ' ''        If ItemName <> "" Then
        ' ''            txtPartCreate.Text = ItemName
        ' ''            txtDescription.Text = ItemDesc
        ' ''            Dim mItem As Item
        ' ''            mItem = Item.NewItem(ItemName, ItemDesc, "")
        ' ''            Session("mItem") = mItem
        ' ''            Session("mRequisitionNew") = mRequisitionNew
        ' ''            Session("Create") = "False"
        ' ''            Session("PartInfo") = "True"
        ' ''            Response.Redirect("wfPartInformation.aspx?BackPage=wfRequisitionItemSearch.aspx&Type=1")
        ' ''        End If
        ' ''    Else
        ' ''        setObject(-1)
        ' ''        Session("AddSingleParts") = "True"
        ' ''        Session.Remove("mRequisitionItemListNew")
        ' ''        Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("ChildPage") & "&ItemId=" & Guid.Empty.ToString)
        ' ''    End If
        ' ''End If
        If IsValid Then
            Dim mItem As Item
            mItem = Item.NewItem(txtPartCreate.Text, txtDescription.Text, "")
            Session("mItem") = mItem
            Session("mRequisitionNew") = mRequisitionNew
            Session("Create") = "False"
            Session("PartInfo") = "True"

            Dim URL As Stack = New Stack    'STACK to store url of current page
            URL.Push(Request.Url)           'Inserting URL in STACK
            Session("URL") = URL
            Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfRequisitionItemSearch.aspx")
        End If
    End Sub
    Private Sub dgPartList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPartList.SortCommand
        mRequisitionItemListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItemListNew") = mRequisitionItemListNew
        dgPartList.DataSource = mRequisitionItemListNew
        dgPartList.DataBind()
        SetSessionForNewPart()
    End Sub
#End Region

End Class
