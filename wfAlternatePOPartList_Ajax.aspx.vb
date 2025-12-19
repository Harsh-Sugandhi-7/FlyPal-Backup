Public Class wfAlternatePOPartList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mReceipt As Receipt
    Public OpenFrom As Integer
    Dim mUnitListForConverter As UnitListForConverter
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mItem = Session("mItem")
        mReceipt = CType(Session("mReceipt"), Receipt)
    End Sub
    Private Sub SetPage()
        mItem = Session("mItem")
        lblResult.Text = "List of alternate parts For : " + mItem.Name
        If Not mItem.IsNew Then
            lblTitle.Text = "Alternate Part For [" + mItem.Name + "]"
        End If
    End Sub
    Private Sub SetReceiptObject(ByVal Index As Integer)
        mReceipt.ReceiptItems.CurrentItem.AlternateItemID = mItem.AlternatePartNos(Index).AlternatePartID
        mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = mItem.AlternatePartNos(Index).UnitID
        Session("mReciept") = mReceipt
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
            End Select
        End If
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
        OpenFrom = CType(Request.QueryString("OpenFrom"), Integer)
        Session("UnitID") = mReceipt.ReceiptItems.CurrentItem.DisplayUnitID
        If Not IsPostBack Then
            DataFieldBind()
        End If
        SetPage()
        '------------Added BY Vikrant on 28-03-2012--------------------------------
        If User.IsInRole("AlternatePartInReceiptView") = True Then
            btnCreatealternatepart.Visible = True
        End If
        '--------------------------------------------------------------------------
    End Sub
    Private Sub dgAlternatePartList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAlternatePartList.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgAlternatePartList.PageIndex * dgAlternatePartList.PageSize
                Dim UnitID As Guid = CType(Session("UnitID"), Guid)
                Dim mtmpItem As Item = Item.GetItem(mItem.AlternatePartNos(Index).AlternatePartID)
                dgAlternatePartList.DataSource = mItem.AlternatePartNos
                dgAlternatePartList.DataBind()
                upnlAlternatePartList.Update()
                mUnitListForConverter = UnitListForConverter.GetUnitListForConverter()
                'If Not UnitID.Equals(mtmpItem.UnitID) Then
                If Not mUnitListForConverter.Contains(BaseUnitID:=UnitID, ConvertUnitID:=mtmpItem.UnitID) Then
                    MSGBoxCtrl.show("Alert!", "The Unit of the alternate part doesnot match. Please select the alternate part with same unit", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                SetReceiptObject(Index)
                Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=" & "wfReceipt_Ajax.aspx" & "&ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx")
        End Select
    End Sub
    Private Sub dgAlternatePartList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgAlternatePartList.PageIndexChanging
        dgAlternatePartList.PageIndex = e.NewPageIndex
        dgAlternatePartList.DataSource = mItem.AlternatePartNos
        dgAlternatePartList.DataBind()
        upnlAlternatePartList.Update()
    End Sub
    '------------Added BY Vikrant on 28-03-2012--------------------------------
    Private Sub btnCreatealternatepart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatealternatepart.Click
        Dim str As String
        ' Session("mItem") = mItem
        Session("mAltItem") = mItem
        Dim AlternateType As Integer = 4 ''
        Session("AlternateType") = AlternateType ''
        str = "openledgersame('wfAlternatePart_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx&ChildPage=wfReceiptItem_Ajax.aspx&ChildPage1=wfAlternatePOPartList_Ajax.aspx&mType=1&OpenFrom=1&AlternateType=4');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    '--------------------------------------------------------------------------
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mItem")
        Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=" & "wfReceipt_Ajax.aspx" & "&ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region


End Class