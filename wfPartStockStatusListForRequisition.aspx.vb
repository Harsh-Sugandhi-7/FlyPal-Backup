Partial Class wfPartStockStatusListForRequisition
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

#Region " Variable Declaration"
    Dim mItemStockStatusList As ItemStockStatusList
    Public mRequisition As Requisition
    Dim PartNo As String
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mItemStockStatusList = Session("mItemStockStatusList")
        mRequisition = Session("mRequisition")
        PartNo = Session("PartNo")
    End Sub
    Private Sub setSession()
        Session("mItemStockStatusList") = mItemStockStatusList
        Session("mRequisition") = mRequisition
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub setObject(ByVal ItemId As Guid)
        If Not mRequisition.RequisitionItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
            ' ' mRequisition.RequisitionItems.CurrentItem.Qty = 0
        End If
        mRequisition.RequisitionItems.CurrentItem.ItemID = ItemId
        mRequisition.RequisitionItems.CurrentItem.Remark = ""
        mRequisition.RequisitionItems.CurrentItem.Note = ""
        Session("mRequisition") = mRequisition
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            'Session("Sender") = ""
                            'Dim mRequisition As Requisition
                            'mRequisition = CType(Session("mRequisition"), Requisition)
                            'mRequisition.RequisitionItems.RemoveAt(mRequisition.RequisitionItems.CurrentIndex)
                            'Session("mRequisition") = mRequisition
                            'Response.Redirect("wfRequisition.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfPartStockStatusList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfPartStockStatusList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfPartStockStatusList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    Response.Redirect("wfPartStockStatusList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    Session("Sender") = ""
                    Response.Redirect("wfPartStockStatusList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfPartStockStatusList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgPartStockStatusList.DataSource = mItemStockStatusList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        If Not IsPostBack Then
            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
            txtSearch.Text = mRequisition.RequisitionItems.CurrentItem.ItemName
            txtSearch.Text = PartNo
            mItemStockStatusList = ItemStockStatusList.GetItemStockStatusList(txtSearch.Text)
            Session("mItemStockStatusList") = mItemStockStatusList
            DataFieldBind()
        End If
        'MessageBoxResult()
        lblResult.Text = "Part Stock Status List :" & mItemStockStatusList.Count & " No.of Record Found(s)."
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mItemStockStatusList = ItemStockStatusList.GetItemStockStatusList(txtSearch.Text.Trim)
        Session("mItemStockStatusList") = mItemStockStatusList
        DataFieldBind()
        lblResult.Text = "Part Stock Status List :" & mItemStockStatusList.Count & " No.of Record Found(s)."
    End Sub
    Private Sub dgPartStockStatusList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartStockStatusList.ItemCommand
        Dim Index As Integer = e.Item.ItemIndex + dgPartStockStatusList.CurrentPageIndex * dgPartStockStatusList.PageSize
        Dim ItemId As Guid = New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "Select"
                setObject(ItemId)
                Session.Remove("mItemStockStatusList")
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & ItemId.ToString)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session.Remove("mItemStockStatusList")
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
#End Region

End Class
