Public Class wfAlternatePartListForRCI_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItem As Item
    Public mReceipt As Receipt
    Public mReceiptCumInvoice As ReceiptCumInvoice
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
        mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
    End Sub
     Private Sub SetPage()
        mItem = Session("mItem")
        lblResult.Text = "List of alternate parts For : " + mItem.Name
        If Not mItem.IsNew Then
            lblTitle.Text = "Alternate Part For [" + mItem.Name + "]"
        End If
    End Sub
     Private Sub SetRCIObject(ByVal Index As Integer)
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.AlternateItemID = mItem.AlternatePartNos(Index).AlternatePartID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mItem.AlternatePartNos(Index).PartName
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mItem.AlternatePartNos(Index).PartDescription
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mItem.AlternatePartNos(Index).UnitID
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
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
        Session("UnitID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID
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
                If Not mUnitListForConverter.Contains(BaseUnitID:=UnitID, ConvertUnitID:=mtmpItem.UnitID) Then
                    'End If
                    'If Not UnitID.Equals(mtmpItem.UnitID) Then
                    MSGBoxCtrl.show("Alert!", "The Unit of the alternate part doesnot match. Please select the alternate part with same unit", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                SetRCIObject(Index)
                If Not mReceiptCumInvoice.IsNew Then
                    Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx")
                ElseIf mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 46 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 56 Or mReceiptCumInvoice.TransTypeID = 57 Then
                    Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx")
                Else
                    Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx" & "&ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx")
                End If
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
        Dim AlternateType As Integer = 3 ''
        Session("AlternateType") = AlternateType ''
        str = "openledgersame('wfAlternatePart_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&ChildPage1=wfAlternatePartListForRCI_Ajax.aspx&AlternateType=3');"
         ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    '--------------------------------------------------------------------------
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mItem")
        'Session("MiddleFrame") = ""
        If Not mReceiptCumInvoice.IsNew Then
            Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx")
        ElseIf mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 46 Or mReceiptCumInvoice.TransTypeID = 48 Or mReceiptCumInvoice.TransTypeID = 50 Or mReceiptCumInvoice.TransTypeID = 53 Or mReceiptCumInvoice.TransTypeID = 56 Or mReceiptCumInvoice.TransTypeID = 57 Then
            Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx")
        Else
            Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx" & "&ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx")
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class