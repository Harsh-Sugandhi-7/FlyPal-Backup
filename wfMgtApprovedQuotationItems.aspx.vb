Partial Class wfMgtApprovedQuotationItems
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents calOrderDate As SIControls.SICalendar

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
    Public mApprovedQuotationPartList As ApprovedQuotationPartList
    Public mQuotationItems As QuotationItems
#End Region

#Region " Helper Method"
    Private Sub FindNowItems()

        mApprovedQuotationPartList = ApprovedQuotationPartList.ApprovedQuotationPartList(mOrder.OrderDate, mVendor, txtSearch.Text)
        dgPartList.DataSource = mApprovedQuotationPartList
        Session("mApprovedQuotationPartList") = mApprovedQuotationPartList
        lblResult.Text = "List of Finance approved Part List as per selected criteria : " + mApprovedQuotationPartList.Count.ToString + " Record(s)  found."
        lblResult1.Visible = False
        DataBind()
    End Sub
    Private Sub FindNowQuotationItems()
        GetSession()
        dgPartList.CurrentPageIndex = 0
        dgQuotationItems.CurrentPageIndex = 0
        mApprovedQuotationPartList = ApprovedQuotationPartList.ApprovedQuotationPartList(mOrder.OrderDate, mVendor, txtSearch.Text)
        dgPartList.DataSource = mApprovedQuotationPartList
        Session("mApprovedQuotationPartList") = mApprovedQuotationPartList
        lblResult.Text = "List of Finance approved Part List as per selected criteria : " + mApprovedQuotationPartList.Count.ToString + " Record(s)  found."
        mQuotationItems = QuotationItems.GetQuotationItems(mOrderDate, mVendor, txtSearch.Text, mItemID)
        dgQuotationItems.DataSource = mQuotationItems
        Session("mQuotationItems") = mQuotationItems
        lblResult1.Text = "List of Finance approved Quotation List as per selected Part :" + mQuotationItems.Count.ToString + " Record(s)  found."
        lblResult1.Visible = True
        DataBind()
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mOrder = Session("mOrder")
        mVendor = Session("VendorName")
        mQuotationItems = Session("mQuotationItems")
        mApprovedQuotationPartList = Session("mApprovedQuotationPartList")
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
        Session("mApprovedQuotationPartList") = mApprovedQuotationPartList

    End Sub
    Private Function SetObject() As Boolean
        GetSession()
        Dim txtValue As TextBox
        Dim item As DataGridItem
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgQuotationItems.Items.Count - 1
        For i = 0 To PageItems
            Recordno = i + dgQuotationItems.PageSize * dgQuotationItems.CurrentPageIndex
            item = dgQuotationItems.Items(i)
            chkSelect = CType(item.FindControl("chkSelect"), CheckBox)
            mQuotationItems(Recordno).IsSelect = chkSelect.Checked
            mQuotationItems(Recordno).MarkClean()

            txtValue = CType(item.FindControl("txtOrderQty"), TextBox)
            mQuotationItems(Recordno).OrderQty = Val(txtValue.Text)
            mQuotationItems(Recordno).MarkClean()
            If mQuotationItems(Recordno).OrderQty = 0 And chkSelect.Checked Then
                ShowMsg = 1
            End If
        Next

        Session("mQuotationItems") = mQuotationItems

        If ShowMsg = 1 Then
            Return True
        Else
            Return False
        End If

    End Function
    Private Sub AddQuotationParts(ByVal mQuotationItems As QuotationItems)
        If SetObject() Then
            Session("sender") = "QtyNotSet"
            Dim msg As New SIMsgBox(Page, "Order Item Information", "Please enter Quantity for selected Items or Select Item", "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            msg.Show()
            Exit Sub
        End If
        Dim mQuotationItem As QuotationItem
        Dim mQuotationID As Guid = Guid.Empty

        Dim Str As String = ""
        'If mQuotationItems Is Nothing Then Exit Sub

        If mQuotationItems Is Nothing Then
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select the Part from list.", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
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
                                    msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
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
                                msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
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

        If Str <> "" Then
            Dim msg As New SIMsgBox(Page, "Order Item Information", "Out of selected Quotation Part(s) following are not added in the order!" & "<BR> <BR>" + Str, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            Session("sender") = "NotAdded"
            msg.Show()
        ElseIf ShowMsg = 0 Then
            Dim msg As New SIMsgBox(Page, "Order Item Information", "Item is added successfully. Do you want to add another item?", "", MsgBoxStyle.YesNo)
            msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            msg.Show()
            FindNowItems()
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Try
                        Session("Sender") = ""
                    Catch ex As SqlException
                        If ex.Number = 8145 Then
                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                            msg1.ReplacePage = "wfMgtApprovedQuotationItems.aspx?BackPage=" & Request.QueryString("BackPage")
                            msg1.Show()
                        ElseIf ex.Number = 2627 Then
                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                            msg1.ReplacePage = "wfMgtApprovedQuotationItems.aspx?BackPage=" & Request.QueryString("BackPage")
                            msg1.Show()
                        ElseIf ex.Number = 547 Then
                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                            msg1.ReplacePage = "wfMgtApprovedQuotationItems.aspx?BackPage=" & Request.QueryString("BackPage")
                            msg1.Show()
                        End If
                    End Try
                Case MsgBoxResult.No
                    GetSession()
                    Session.Remove("PartNo")
                    Session.Remove("VendorName")
                    Session.Remove("mCurrencyName")
                    Session.Remove("mVendorID")
                    Session.Remove("mCurrencyID")
                    Session("mOrder") = mOrder
                    Session("Sender") = ""
                    Response.Redirect("wfPurchaseOrder.aspx?BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK
                    If Session("sender") = "NotAdded" Then
                        Session("sender") = ""
                        Dim msg As New SIMsgBox(Page, "Order Item Information", "Item is added successfully. Do you want to add another item?", "", MsgBoxStyle.YesNo)
                        msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                        msg.Show()
                        FindNowItems()
                    ElseIf Session("sender") = "QtyNotSet" Then
                        Session("sender") = ""
                        mItemID = Session("mItemID")

                        dgQuotationItems.DataSource = mQuotationItems
                        Session("mQuotationItems") = mQuotationItems
                        lblResult1.Text = "List of Finance approved Quotation List as per selected Part :" + mQuotationItems.Count.ToString + " Record(s)  found."
                        lblResult1.Visible = True
                        DataBind()
                    End If
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Public Sub NewPageofQuotationItems(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgQuotationItems.CurrentPageIndex = e.NewPageIndex
        dgQuotationItems.DataSource = mQuotationItems
        Session("mQuotationItems") = mQuotationItems
        dgQuotationItems.DataBind()
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
            'Added By Saylee on 13th Dec 2007 to solve bug of Purchase Order from Inventory By Pramod
            If mOrder.OrderDate = "" Then
                Me.calOrderDate.Value = Today.Date
            Else
                Me.calOrderDate.Value = mOrder.OrderDate
            End If
            'If Me.calOrderDate.Text = "" Then
            '    Me.calOrderDate.Text = Today.Date.ToShortDateString
            'End If
            FindNowItems()
        End If
        Me.calOrderDate.Enabled = mOrder.OrderItems.Count = 0
        MessageBoxResult()
    End Sub

    Private Sub dgPartList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPartList.ItemCommand
        Select Case e.CommandName
            Case "Select"
                mItemID = New Guid(e.Item.Cells(0).Text)
                Session("mItemID") = mItemID
                dgQuotationItems.CurrentPageIndex = 0
                FindNowQuotationItems()
        End Select
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        GetSession()
        Session.Remove("PartNo")
        Session.Remove("VendorName")
        Session.Remove("mCurrencyName")
        Session.Remove("mVendorID")
        Session.Remove("mCurrencyID")
        Response.Redirect("wfPurchaseOrder.aspx?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub dgQuotationItems_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgQuotationItems.ItemCommand
        GetSession()
        Select Case e.CommandName
            Case "Select"
                Dim mQuotationItem As QuotationItem
                mQuotationItem = mQuotationItems.Item(e.Item.ItemIndex)

                Dim mQuotation As Quotation = Quotation.GetQuotation(mQuotationItem.QuotationID)

                If mOrder.VendorID.Equals(Guid.Empty) Then
                    mOrder.VendorID = mQuotation.VendorID
                End If
                If mOrder.CurrencyID.Equals(Guid.Empty) Then
                    mOrder.CurrencyID = mQuotation.CurrencyID
                    mOrder.ConversionFactor = mQuotation.ConversionFactor
                    mOrder.QuotationDate = mQuotation.VendorQuoteDate
                    mOrder.QuotationNo = mQuotation.VendorQuoteNo
                End If
                With mOrder.OrderItems.CurrentItem
                    If Not .OrderItemQuotationItems.Contains(mQuotationItem.ID) Then
                        'if NOT then add
                        mOrder.BeginEdit()
                        .ItemID = mQuotationItem.ItemID
                        .PriorityID = mQuotationItem.PriorityID
                        .CRate = mQuotationItem.CRate
                        .DeliveryInDays = mQuotationItem.DeliveryInDays
                        .CBillBackRate = mQuotationItem.CBillBackRate

                        .OrderItemQuotationItems.Add(.ID, mQuotationItem.ID, mQuotationItem.Qty, mQuotationItem.QuotationNo, mQuotationItem.QuotationDate.ToString, mQuotationItem.QuotationID)
                        mOrder.ApplyEdit()
                        SetSession()
                        Session.Remove("PartNo")
                        Response.Redirect("wfOrderItem.aspx?BackPage=" & Request.QueryString("BackPage"))
                    Else
                        Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
                        msg.ReplacePage = "wfMgtApprovedQuotationItems.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                        msg.Show()
                        Exit Sub
                    End If
                End With
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.CurrentPageIndex = 0
        FindNowItems()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        AddQuotationParts(mQuotationItems)
        Session("mOrder") = mOrder
    End Sub
    Private Sub calOrderDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calOrderDate.TextChanged
        If Not (calOrderDate.IsDateValue) Then
            calOrderDate.Value = Today.Date
        End If
        mOrder.OrderDate = calOrderDate.Value.ToString
        FindNowItems()
    End Sub
    Private Sub dgPartList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPartList.PageIndexChanged
        dgPartList.CurrentPageIndex = e.NewPageIndex
        ''mApprovedQuotationPartList = ApprovedQuotationPartList.ApprovedQuotationPartList(mOrder.OrderDate, mVendor, txtSearch.Text)
        dgPartList.DataSource = mApprovedQuotationPartList
        Session("mApprovedQuotationPartList") = mApprovedQuotationPartList
        dgPartList.DataBind()
    End Sub

#End Region

End Class
