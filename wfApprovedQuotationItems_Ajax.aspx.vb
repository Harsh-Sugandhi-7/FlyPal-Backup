Public Class wfApprovedQuotationItems_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration"
    Public mOrderDate As String = "01-01-3050"
    Public mVendor As String = ""
    Public mCurrencyName As String = ""
    Public mRate As Decimal
    Public mVendorID As Guid = Guid.Empty
    Public mCurrencyID As Guid = Guid.Empty
    Public mItemID As Guid = Guid.Empty
    Public mOrder As Order
    Public PartNo As String
    Dim ShowMsg As Integer = 0
    Dim chkSelect As CheckBox
    Public mApprovedQuotationItemList As ApprovedQuotationItemList
    Public mQuotationItems As QuotationItems
#End Region

#Region " Helper Method"
    Private Sub FindNowItems()
        mApprovedQuotationItemList = ApprovedQuotationItemList.GetApprovedQuotationItemList(mOrder.OrderDate, txtSearch.Text)
        dgPartList.DataSource = mApprovedQuotationItemList
        Session("mApprovedQuotationItemList") = mApprovedQuotationItemList
        lblResult.Text = "List of Finance approved Part List as per selected criteria : " + mApprovedQuotationItemList.Count.ToString + " Record(s)  found."
        lblResult1.Visible = False
        DataBind()
        upnlPartList.Update()
    End Sub
    Private Sub FindNowQuotationItems()
        GetSession()
        mApprovedQuotationItemList = ApprovedQuotationItemList.GetApprovedQuotationItemList(mOrder.OrderDate, txtSearch.Text)
        dgPartList.DataSource = mApprovedQuotationItemList
        Session("mApprovedQuotationItemList") = mApprovedQuotationItemList
        lblResult.Text = "List of approved Part List as per selected criteria : " + mApprovedQuotationItemList.Count.ToString + " Record(s)  found."
        mQuotationItems = QuotationItems.GetQuotationItems(mOrder.OrderDate, "", mItemID)
        dgQuotationItems.DataSource = mQuotationItems
        If mQuotationItems.Count >= 0 Then
            btnOk.Enabled = True
        Else
            btnOk.Enabled = False
        End If
        Session("mQuotationItems") = mQuotationItems
        lblResult1.Text = "List of approved Quotation List as per selected Part :" + mQuotationItems.Count.ToString + " Record(s)  found."
        lblResult1.Visible = True
        DataBind()
        upnlPartList.Update()
        upnlQuotationItems.Update()
        upnlButtons.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mOrder = Session("mOrder")
        mVendor = Session("VendorName")
        mQuotationItems = Session("mQuotationItems")
        mApprovedQuotationItemList = Session("mApprovedQuotationItemList")
        PartNo = Session("PartNo")

        mCurrencyName = Session("mCurrencyName")
        mVendorID = Session("mVendorID")
        mCurrencyID = Session("mCurrencyID")
        mRate = Session("mRate")
    End Sub

    Private Sub SetSession()
        Session("mOrder") = mOrder
        Session("VendorName") = mVendor
        Session("mQuotationItems") = mQuotationItems
        Session("mApprovedQuotationItemList") = mApprovedQuotationItemList

    End Sub
    Private Function SetObject() As Boolean
        GetSession()
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgQuotationItems.Rows.Count - 1
        For i = 0 To PageItems
            Recordno = i + dgQuotationItems.PageSize * dgQuotationItems.PageIndex
            chkSelect = CType(dgQuotationItems.Rows(i).FindControl("chkSelect"), CheckBox)
            mQuotationItems(Recordno).IsSelect = chkSelect.Checked
            mQuotationItems(Recordno).MarkClean()
        Next
        Session("mQuotationItems") = mQuotationItems
    End Function
    Private Sub AddQuotationParts(ByVal mQuotationItems As QuotationItems)
        SetObject()
        Dim mQuotationItem As QuotationItem
        Dim mQuotationID As Guid = Guid.Empty

        Dim Str As String = ""
      
        If mQuotationItems Is Nothing Then
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select the Part from list.", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfApprovedQuotationItems_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            msg.Show()
            Exit Sub
        End If

        For Each mQuotationItem In mQuotationItems
            If mQuotationItem.IsSelect Then
                If mVendor = "" Then
                    mQuotationID = mQuotationItem.QuotationID
                    mVendor = mQuotationItem.VendorName
                    mCurrencyName = mQuotationItem.Currency
                    mRate = mQuotationItem.CRate
                End If
                Session("VendorName") = mVendor
                Session("mCurrencyName") = mCurrencyName
                Session("mRate") = mRate
                'If (mVendor = mQuotationItem.VendorName And mCurrencyName = mQuotationItem.Currency And mRate = mQuotationItem.CRate) Or ((mRate = mQuotationItem.CRate And mOrder.OrderItems.Count = 0) And mOrder.VendorID.Equals(mVendorID) And mOrder.CurrencyID.Equals(mCurrencyID)) Then
                If (mVendor = mQuotationItem.VendorName And mCurrencyName = mQuotationItem.Currency) Or (mOrder.OrderItems.Count = 0 And mOrder.VendorID.Equals(mVendorID) And mOrder.CurrencyID.Equals(mCurrencyID)) Then
                    If mOrder.OrderItems.Contains(mQuotationItem.ItemID) Then
                        mRate = mOrder.OrderItems(mQuotationItem.ItemID, "").CRate 'Added By Deven On 09-01-2008
                        If (mRate = mQuotationItem.CRate) Then
                            With mOrder.OrderItems.Item(mQuotationItem.ItemID, "")
                                'Check if Quotation Part is present ?
                                If Not .OrderItemQuotationItems.Contains(mQuotationItem.ID) Then
                                    'if NOT then add
                                    mOrder.BeginEdit()

                                    .ItemID = mQuotationItem.ItemID
                                    .PriorityID = mQuotationItem.PriorityID
                                    .CRate = mQuotationItem.CRate
                                    .DeliveryInDays = mQuotationItem.DeliveryInDays
                                    .CBillBackRate = mQuotationItem.CBillBackRate

                                    .OrderItemQuotationItems.Add(.ID, mQuotationItem.ID, mQuotationItem.OrderQty, mQuotationItem.QuotationNo, mQuotationItem.QuotationDate.ToString, mQuotationItem.QuotationID)
                                    mOrder.ApplyEdit()
                                    Session("mOrder") = mOrder
                                    'New Addition By Yogita on 15-Dec-2007 to solve Bug No:-PO_O_20_A suggested By Deven & Kalpesh Sir
                                ElseIf .OrderItemQuotationItems.Contains(mQuotationItem.ID) Then
                                    .OrderItemQuotationItems.Item(mQuotationItem.ID, "").Qty = .OrderItemQuotationItems.Item(mQuotationItem.ID, "").Qty + mQuotationItem.OrderQty
                                Else
                                    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
                                    msg.ReplacePage = "wfApprovedQuotationItems_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                    msg.Show()
                                    Exit Sub

                                End If
                            End With
                        Else
                            Str = Str & "Quotation No. : " & mQuotationItem.QuotationNo.ToString & "<BR>"
                        End If
                    Else
                        'If NOT
                        mOrder.BeginEdit()
                        'Dim mQuotation As Quotation = Quotation.GetQuotation(mQuotationItem.QuotationID)

                        mOrder.OrderItems.Add(mOrder.ID)
                        With mOrder.OrderItems.CurrentItem
                            .ItemID = mQuotationItem.ItemID
                            .PriorityID = mQuotationItem.PriorityID
                            .CRate = mQuotationItem.CRate

                            .DeliveryInDays = mQuotationItem.DeliveryInDays
                            .CBillBackRate = mQuotationItem.CBillBackRate

                            '.QuotationItemID = mQuotationItem.ID
                            '.Qty = mQuotationItem.PurchaseQty
                            'Check if Quotation Part is present?
                            If Not .OrderItemQuotationItems.Contains(mQuotationItem.ID) Then
                                'if NOT then add
                                .OrderItemQuotationItems.Add(.ID, mQuotationItem.ID, mQuotationItem.OrderQty, mQuotationItem.QuotationNo, mQuotationItem.QuotationDate.ToString, mQuotationItem.QuotationID)
                                mOrder.ApplyEdit()
                                Session("mOrder") = mOrder
                            Else
                                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
                                msg.ReplacePage = "wfApprovedQuotationItems_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                                msg.Show()
                                Exit Sub

                            End If
                        End With
                    End If
                Else
                    Str = Str & "Quotation No. : " & mQuotationItem.QuotationNo.ToString & "<BR>"
                End If
            End If

        Next

        If Not mQuotationID.Equals(Guid.Empty) Then
            Dim mQuotation As Quotation = Quotation.GetQuotation(mQuotationID)

            If mOrder.VendorID.Equals(Guid.Empty) Then
                mOrder.VendorID = mQuotation.VendorID

                mVendorID = mOrder.VendorID

                Session("mVendorID") = mVendorID
            End If
            If mOrder.CurrencyID.Equals(Guid.Empty) Then
                mOrder.CurrencyID = mQuotation.CurrencyID
                mOrder.ConversionFactor = mQuotation.ConversionFactor
                mOrder.QuotationDate = mQuotation.VendorQuoteDate
                mOrder.QuotationNo = mQuotation.VendorQuoteNo

                mCurrencyID = Session("mCurrencyID")

                Session("mCurrencyID") = mOrder.CurrencyID
            End If
        End If
    End Sub
#End Region

#Region " Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
            txtSearch.Text = PartNo
             If mOrder.OrderDate = "" Then
                Me.calOrderDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Else
                Me.calOrderDate.Text = mOrder.OrderDateFormatted
            End If
             FindNowItems()
        End If
        Me.calOrderDate.Enabled = mOrder.OrderItems.Count = 0
    End Sub
    Private Sub dgPartList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartList.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartList.PageIndex * dgPartList.PageSize
                mItemID = mApprovedQuotationItemList(index).ItemID
                Session("mItemID") = mItemID
                FindNowQuotationItems()
        End Select
    End Sub
  Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNowItems()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        AddQuotationParts(mQuotationItems)
        Session("mOrder") = mOrder

        GetSession()
        Session.Remove("PartNo")
        Session.Remove("VendorName")
        Session.Remove("mCurrencyName")
        Session.Remove("mVendorID")
        Session.Remove("mCurrencyID")
        Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub calOrderDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calOrderDate.TextChanged
        If Not IsDate(calOrderDate.Text) Then
            calOrderDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
        mOrder.OrderDate = calOrderDate.Text
        FindNowItems()
    End Sub
    Private Sub dgPartList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPartList.PageIndexChanged
        dgPartList.PageIndex = e.NewPageIndex
        dgPartList.DataSource = mApprovedQuotationItemList
        Session("mApprovedQuotationItemList") = mApprovedQuotationItemList
        dgPartList.DataBind()
        upnlPartList.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        GetSession()
        Session.Remove("PartNo")
        Session.Remove("VendorName")
        Session.Remove("mCurrencyName")
        Session.Remove("mVendorID")
        Session.Remove("mCurrencyID")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region

End Class