
'Created by :- Kalpesh
'Date       :- 02-Jun-2008

Partial Class wfAlternateEnquiryPartList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblSearch1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblOptions1 As System.Web.UI.WebControls.Label
    Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
    Protected WithEvents lblAltType As System.Web.UI.WebControls.Label
    Protected WithEvents cmbAltType As System.Web.UI.WebControls.DropDownList
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mItem As Item
    Public mQuotation As Quotation
#End Region

#Region " Business Methods "
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mItem = Session("mItem")
        mQuotation = CType(Session("mQuotation"), Quotation)
    End Sub
    Private Sub SetSession()
        Session("mQuotation") = mQuotation
        Session("mItem") = mItem
    End Sub
    Private Sub SetPage()
        mItem = Session("mItem")
        lblResult.Text = "List of alternate parts For : " + mItem.Name
        If Not mItem.IsNew Then
            lblTitle.Text = "Alternate Part For [" + mItem.Name + "]"
        End If
    End Sub
    Private Sub SetObject(ByVal Index As Integer)
        mQuotation.QuotationItems.CurrentItem.AlternateItemID = mItem.AlternatePartNos(Index).AlternatePartID
        Session("mQuotation") = mQuotation
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgAlternatePartList.DataSource = mItem.AlternatePartNos
        Session("Item") = mItem
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
        SetPage()
    End Sub
    Private Sub dgAlternatePartList_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAlternatePartList.ItemCommand
        Dim Index As Integer = e.Item.ItemIndex + dgAlternatePartList.PageSize * dgAlternatePartList.CurrentPageIndex
        Select Case e.CommandName
            Case "Select"
                SetObject(Index)
                Response.Redirect("wfQuotationItem_Ajax.aspx?BackPage=" & "wfQuotation_Ajax.aspx" & "&ChildPage1=" & "wfQuotationPendingOrderList.aspx")
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mItem")
        Session("MiddleFrame") = ""
        Response.Redirect("wfQuotationItem_Ajax.aspx?BackPage=" & "wfQuotation_Ajax.aspx" & "&ChildPage1=" & "wfQuotationPendingOrderList.aspx")
    End Sub
#End Region

    Private Sub dgAlternatePartList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgAlternatePartList.SelectedIndexChanged

    End Sub
End Class
