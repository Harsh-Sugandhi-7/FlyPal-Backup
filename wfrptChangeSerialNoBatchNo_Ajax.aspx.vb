Public Class wfrptChangeSerialNoBatchNo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPartListForSerialNoBatchNoChange As PartListForSerialNoBatchNoChange
    Dim mReceiptListForSamePartAndSerialNo As ReceiptListForSamePartAndSerialNo
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptChangeSerialNoBatchNo_Ajax.aspx" Then
            Session.Remove ("mPartListForSerialNoBatchNoChange")
        End If
    End Sub
    Private Sub GetSession()
        mPartListForSerialNoBatchNoChange = CType(Session("mPartListForSerialNoBatchNoChange"), PartListForSerialNoBatchNoChange)
        mReceiptListForSamePartAndSerialNo = CType(Session("mReceiptListForSamePartAndSerialNo"), ReceiptListForSamePartAndSerialNo)
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub DataFieldBind(Optional ByVal PartNo As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal SerialNo As String = "", Optional ByVal BatchNo As String = "")
        mPartListForSerialNoBatchNoChange = PartListForSerialNoBatchNoChange.GetPartListForSerialNoBatchNoChange(PartNo:=PartNo, Text:=Text, No:=No, SerialNo:=SerialNo, BatchNo:=BatchNo)
        Session("mPartListForSerialNoBatchNoChange") = mPartListForSerialNoBatchNoChange
        dgPartSearch.DataSource = mPartListForSerialNoBatchNoChange
        dgPartSearch.DataBind()
        lblResult.Text = "List of Parts : " & mPartListForSerialNoBatchNoChange.Count & " Record(s) found "
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptChangeSerialNoBatchNo_Ajax.aspx"
            If txtPartNo.Enabled = True Then
                SetFocus(txtPartNo)
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click
        DataFieldBind(Trim(txtPartNo.Text), Trim(txtReceiptTextList.Text), Val(txtNo.Text), Trim(txtSerialNo.Text), Trim(txtBatchNo.Text))
        upnlGridView.Update()
    End Sub
    Private Sub dgPartSearch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
        Select Case e.CommandName
            Case "Change"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
                Dim mReceiptItemID As Guid = mPartListForSerialNoBatchNoChange(index).ID
                Dim mItemID As Guid = mPartListForSerialNoBatchNoChange(index).ItemID
                Dim TempPartNo As String = mPartListForSerialNoBatchNoChange(index).ItemName
                Dim Desc As String = mPartListForSerialNoBatchNoChange(index).ItemDesc
                Dim SerialNo As String = mPartListForSerialNoBatchNoChange(index).SerialNo
                Dim mReceiptDate As String = mPartListForSerialNoBatchNoChange(index).DateFormatted
                Dim ReceiptNo As String = mPartListForSerialNoBatchNoChange(index).ReceiptNo
                Dim BatchNo As String = mPartListForSerialNoBatchNoChange(index).BatchNo
                Dim mSerialisedStatus As String = mPartListForSerialNoBatchNoChange(index).SerializedStatus
                MarkLog(Util.Action.Edit, "ChangeSerialNoBatchNo", "Part : " + TempPartNo + " Receipt No : " + ReceiptNo + " Serial No. : " + IIf(SerialNo = "&nbsp;", "", SerialNo) + " Batch No : " + IIf(BatchNo = "&nbsp;", "", BatchNo), Util.ErrorType.NoError, Guid.Empty, EventLogID)
                dgPartSearch.DataSource = mPartListForSerialNoBatchNoChange
                dgPartSearch.DataBind()
                SerialNoBatchNo(mReceiptItemID.ToString, mItemID.ToString, TempPartNo, Desc, SerialNo, mReceiptDate, ReceiptNo, BatchNo, mSerialisedStatus)
                mdlSerialNoBatchNo.Show()
        End Select
    End Sub

    Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        dgPartSearch.DataSource = mPartListForSerialNoBatchNoChange
        Session("mPartListForSerialNoBatchNoChange") = mPartListForSerialNoBatchNoChange
        dgPartSearch.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "ChangeSerialNoBatchNo", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mPartListForSerialNoBatchNoChange = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
#End Region

#Region "Serial No. Batch No."
    Private Sub SerialNoBatchNo(Optional ByVal ReceiptItemID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal ItemID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal TempPartNo As String = "", Optional ByVal Desc As String = "", Optional ByVal SerialNo As String = "", Optional ByVal ReceiptDate As String = "", Optional ByVal ReceiptNo As String = "", Optional ByVal BatchNo As String = "", Optional ByVal mSerialisedStatus As String = "")
        txtPart.Text = TempPartNo.ToString
        txtOldSerialNo.Text = Trim(IIf(SerialNo = "&nbsp;", "", SerialNo))
        txtNewSerialNo.Text = IIf(SerialNo = "&nbsp;", "", SerialNo)
        txtNewBatchNo.Text = IIf(BatchNo = "&nbsp;", "", BatchNo)
        txtOldBatchNo.Text = Trim(IIf(BatchNo = "&nbsp;", "", BatchNo))
        chkSerialized.Checked = mSerialisedStatus

        Session("mReceiptItemID") = ReceiptItemID
        Session("mItemID") = ItemID
        Session("tempSerialNo") = SerialNo
        Session("tempBatchNo") = BatchNo
        Session("mReceiptDate") = ReceiptDate
        Session("PartNo") = TempPartNo
        Session("ReceiptNo") = ReceiptNo
        Session("mSerialisedStatus") = mSerialisedStatus

        mReceiptListForSamePartAndSerialNo = ReceiptListForSamePartAndSerialNo.GetReceiptListForSamePartAndSerialNo(New Guid(ItemID), txtOldSerialNo.Text, New Guid(ReceiptItemID))
        Session("mReceiptListForSamePartAndSerialNo") = mReceiptListForSamePartAndSerialNo
        dgPartNoSrNo.DataSource = mReceiptListForSamePartAndSerialNo
        dgPartNoSrNo.DataBind()
        lblTitle.Text = "Change Serial No. / Batch No. " + "[ " + ReceiptNo + " ]"
        lblNote.Text = "List Of Receipts against the same Part No. and Same Serial No : " + mReceiptListForSamePartAndSerialNo.Count.ToString() + " Record(s) Found."
        upnlSerialNoBatchNo.Update()
    End Sub
    Private Sub GridBind()
        dgPartNoSrNo.DataSource = mReceiptListForSamePartAndSerialNo
        dgPartNoSrNo.DataBind()
    End Sub
    Private Sub Save()
        If chkSerialized.Checked = True Then
            If PartListForSerialNoBatchNoChange.CheckDuplicateSerialNo(New Guid(Session("mReceiptItemID").ToString), New Guid(Session("mItemID").ToString), txtNewSerialNo.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Serial number already exist. You can not add Duplicate.", MsgBoxStyle.OkOnly, "")
                GridBind()
                pnlSerialNoBatchNo.Enabled = True
                upnlSerialNoBatchNo.Update()
                Exit Sub
            End If
        End If
        PartListForSerialNoBatchNoChange.ChangeSerialNoBatchNo(New Guid(Session("mReceiptItemID").ToString), New Guid(Session("mItemID").ToString), txtOldSerialNo.Text, txtNewSerialNo.Text, txtOldBatchNo.Text, txtNewBatchNo.Text, CType(Session("mSerialisedStatus"), Boolean))
        Dim ReceiptInfo As String = ""
        Dim SerialNoInfo As String = ""
        Dim BatchNoInfo As String = ""
        ReceiptInfo = "Part No. : " + txtPart.Text + " Receipt No. : " + Session("ReceiptNo") + " Old Serial No. : " + txtOldSerialNo.Text + " New Serial No. : " + txtNewSerialNo.Text + " Old Batch no. : " + txtOldBatchNo.Text + " New Batch No. : " + txtNewBatchNo.Text
        MarkLog(Util.Action.Save, "ChangeSerialNoBatchNo", ReceiptInfo, Util.ErrorType.NoError, New Guid(Session("mReceiptItemID").ToString), EventLogID)
        mReceiptListForSamePartAndSerialNo = ReceiptListForSamePartAndSerialNo.GetReceiptListForSamePartAndSerialNo(New Guid(Session("mItemID").ToString), txtNewSerialNo.Text, New Guid(Session("mReceiptItemID").ToString))
        Session("mReceiptListForSamePartAndSerialNo") = mReceiptListForSamePartAndSerialNo
        GridBind()
        upnlSerialNoBatchNo.Update()
        DataFieldBind()
        upnlGridView.Update()
    End Sub
    Protected Sub btnSaveSerialNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveSerialNo.Click
        If (Len(txtNewSerialNo.Text) = 0 And chkSerialized.Checked = True) Then
            lblMessageTitle.Text = "Alert!"
            lblMessage.Text = "Serial No. Required Since Part is Serialized."
            btnYes.Visible = False
            btnNo.Visible = False
            btnOk.Visible = True
            pnlSerialNoBatchNo.Enabled = False
            upnlMessageBox.Update()
            GridBind()
            mdlPopupExit.Show()
            Exit Sub
        End If
        If (txtOldSerialNo.Text <> txtNewSerialNo.Text) And (mReceiptListForSamePartAndSerialNo.Count <> 0) And (((Len(txtNewSerialNo.Text.Trim) <> 0) And chkSerialized.Checked = True) Or (chkSerialized.Checked = False)) Then
            lblMessageTitle.Text = "Alert!"
            lblMessage.Text = "<strong> Following List of Receipts will also be affected when a Serial No. is changed. </strong> <p>Do you want to Continue? </p>"
            btnYes.Visible = True
            btnNo.Visible = True
            btnOk.Visible = False
            GridBind()
            pnlSerialNoBatchNo.Enabled = False
            upnlMessageBox.Update()
            mdlPopupExit.Show()
            Exit Sub
        End If
        Save() 
    End Sub
    Protected Sub BtnCloseModal_Click(sender As Object, e As EventArgs) Handles btnCloseModal.Click
        Session.Remove("mReceiptItemID")
        Session.Remove("mItemID")
        Session.Remove("tempSerialNo")
        Session.Remove("tempBatchNo")
        Session.Remove("mReceiptDate")
        Session.Remove("PartNo")
        Session.Remove("ReceiptNo")
        Session.Remove("mSerialisedStatus")
        Session.Remove("mReceiptListForSamePartAndSerialNo")
        DataFieldBind()
        upnlGridView.Update()
        mdlSerialNoBatchNo.Hide()
    End Sub
    Private Sub dgPartNoSrNo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartNoSrNo.PageIndexChanging
        dgPartNoSrNo.PageIndex = e.NewPageIndex
        dgPartNoSrNo.DataSource = mReceiptListForSamePartAndSerialNo
        Session("mReceiptListForSamePartAndSerialNo") = mReceiptListForSamePartAndSerialNo
        dgPartNoSrNo.DataBind()
        upnlSerialNoBatchNo.Update()
    End Sub
    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnYes.Click
        Try
            Save()
            pnlSerialNoBatchNo.Enabled = True
            mdlPopupExit.Hide()
        Catch ex As SqlException
            lblMessageTitle.Text = "Erro !"
            lblMessage.Text = "Error while saving record."
            btnYes.Visible = False
            btnNo.Visible = False
            btnOk.Visible = True
            upnlMessageBox.Update()
            mdlPopupExit.Show()
        End Try
    End Sub
    Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNo.Click
        GridBind()
        pnlSerialNoBatchNo.Enabled = True
        upnlSerialNoBatchNo.Update()
        mdlPopupExit.Hide()
    End Sub
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOk.Click
        GridBind()
        pnlSerialNoBatchNo.Enabled = True
        upnlSerialNoBatchNo.Update()
        mdlPopupExit.Hide()
    End Sub
#End Region

   
End Class