Partial Class wfSalesOrdersForPurchaseOrder
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variales and Declarations"

    Public mDistinctTextList As DistinctTextListForSalesOrder
    Public mQuotationTextList As DistinctTextListForQuotation
    Public mSalesOrderList As PendingFromList 'SalesOrderList
    Public mSalesOrder As SalesOrder
    Dim SearchIndex, DateIndex, FromDate, ToDate, SalesOrderText, QuotationText, Name, No As String
    Dim StatusId As String = "2"
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            ToDate = (New SmartDate(Request.QueryString("Date"))).Text
            Session("ToDate") = ToDate
            Dim mCustomerID As Guid = New Guid(Request.QueryString("CustomerID"))
            If Not mCustomerID.Equals(Guid.Empty) Then
                Name = VendorList.GetVendortList(0).Item(mCustomerID).Name
                Session("Name") = Name
            End If
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            'Session("MiddleFrame") = "wfSalesOrderList.aspx"
            DataFieldBind()
            SetControl()
        End If
        ''MessageBoxResult()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDate.SelectedIndex = 0
        cmbSalesOrderText.SelectedIndex = 0
        cmbQuotationText.SelectedIndex = 0
        ClearControls()
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeroid(DateIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeroid(DateIndex)
        If cmbDate.Enabled = True Then
            setFocus(cmbDate)
        End If
    End Sub
    Private Sub cmbQuotationText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQuotationText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        'setPeroid(DateIndex)
        If cmbQuotationText.Enabled = True Then
            setFocus(cmbQuotationText)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgSalesOrderList.DataBind()
        lblResult.Text = "List of SalesOrder as per criteria :" & mSalesOrderList.Count & " Record(s) found."
    End Sub

    Private Sub dgSalesOrderList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSalesOrderList.ItemCommand
        Dim mID As New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "Select"
                ShowItems(mID)
        End Select
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        Dim chkSelect As CheckBox
        For I As Integer = 0 To dgSalesOrderItems.Items.Count - 1
            chkSelect = CType(dgSalesOrderItems.Items(I).FindControl("chkSelect"), CheckBox)

            mSalesOrder.SalesOrderItems.Item(I).IsSelect = chkSelect.Checked
            mSalesOrder.SalesOrderItems.Item(I).MarkClean()
        Next

        Session("AddSalesOrderParts") = "True"
        Session("mSalesOrder") = mSalesOrder

        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
#End Region

#Region " Business Methods "
    Private Sub SetControl()
        setPeroid(DateIndex)
        CallFindNow(SearchIndex)
        dgSalesOrderList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbSalesOrderText.SelectedValue = IIf(SalesOrderText = "", "(All)", SalesOrderText)
        cmbQuotationText.SelectedValue = IIf(QuotationText = "", "(All)", QuotationText)
        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of SalesOrder as per criteria :" & mSalesOrderList.Count & " Record(s) found."
    End Sub
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "01/01/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "01/01/2050", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
        StatusId = 2 'Session("StatusId")
        SalesOrderText = Session("SalesOrderText")
        QuotationText = Session("QuotationText")
        Name = Session("Name")
        No = Session("No")
        mDistinctTextList = DistinctTextListForSalesOrder.GetDistinctTextList("10", , True, "(All)")
        cmbSalesOrderText.DataSource = mDistinctTextList
        mQuotationTextList = DistinctTextListForQuotation.GetDistinctTextList("8", 0, True, "(All)") '7 Quotation
        cmbQuotationText.DataSource = mQuotationTextList
        mSalesOrderList = PendingFromList.GetPendingFromList(PendingFromList.PendingListOf.SalesOrder, "", "", 0, "01/01/1900", ToDate, 2)
        dgSalesOrderList.DataSource = mSalesOrderList
        Session("mSalesOrderList") = mSalesOrderList
        DataBind()

        lblResult.Text = "List of SalesOrder as per criteria :" & mSalesOrderList.Count & " Record(s) found."
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mSalesOrder = Session("mSalesOrder")
        mSalesOrderList = Session("mSalesOrderList")
        mDistinctTextList = Session("mDistinctTextList")
        mQuotationTextList = Session("mQuotationTextList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = 2 'Session("StatusId")
        SalesOrderText = Session("SalesOrderText")
        QuotationText = Session("QuotationText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
    End Sub
    Private Sub SetSession()
        Session("mSalesOrder") = mSalesOrder
        Session("mSalesOrderList") = mSalesOrderList
        Session("mDistinctTextList") = mDistinctTextList
        Session("mQuotationTextList") = mQuotationTextList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSalesOrder")
        Session.Remove("mSalesOrderList")
        Session.Remove("mDistinctTextList")
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSalesOrderList.aspx" Then
            'Session.Remove("mSalesOrder")
            Session.Remove("mSalesOrderList")
            Session.Remove("mDistinctTextList")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            'Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("SalesOrderText")
            Session.Remove("mQuotationTextList")
            Session.Remove("QuotationText")
            'Session.Remove("Name")
            Session.Remove("No")
        End If
    End Sub

    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Value.ToString <> "", txtFromDate.Value.ToString, "01/01/1900")
        ToDate = IIf(txtToDate.Value.ToString <> "", txtToDate.Value.ToString, "01/01/2050")
        StatusId = 2 'Authorized
        SalesOrderText = IIf(cmbSalesOrderText.SelectedIndex <= 0, "", cmbSalesOrderText.SelectedValue)
        QuotationText = IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedValue)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("SalesOrderText") = SalesOrderText
        Session("QuotationText") = QuotationText
        Session("No") = No
        Session("Name") = Name
    End Sub

    Private Sub ClearControls()
        txtNo.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)

        lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)

        If SearchIndex = 1 And DateIndex = 6 And DateIndex <> 0 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = False
        ElseIf SearchIndex = 1 And DateIndex <> 0 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If

        ''txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        ''calFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)

        ''txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        ''If txtToDate.Visible Then txtToDate.Enabled = False

        ''calToDate.Visible = False 'IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)


        cmbSalesOrderText.Visible = IIf(SearchIndex = 2, True, False)
        cmbQuotationText.Visible = IIf(SearchIndex = 5, True, False)
        lblNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbSalesOrderText.SelectedIndex <> 0 Or cmbQuotationText.SelectedIndex <> 0), True, False)
        txtNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbSalesOrderText.SelectedIndex <> 0 Or cmbQuotationText.SelectedIndex <> 0), True, False)
        txtName.Visible = IIf(SearchIndex = 3 Or SearchIndex = 4, True, False)

    End Sub
    Private Sub setPeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                ToDate = Request.QueryString("Date")
                txtFromDate.Value = CDate("01-01-1900")
                txtToDate.Value = CDate(ToDate)  'CDate("01-01-2200")
            Case 1 'Last 1 Week
                ToDate = Request.QueryString("Date")
                txtFromDate.Value = CDate(Today.AddDays(-6))
                txtToDate.Value = CDate(ToDate)  'Today.Date
            Case 2 'Last 1 Month
                ToDate = Request.QueryString("Date")
                txtFromDate.Value = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Value = CDate(ToDate)  'Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Value = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Value = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Value = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Value = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Value = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                ToDate = Request.QueryString("Date")
                txtFromDate.Value = Today.AddDays(1).AddYears(-1)
                txtToDate.Value = CDate(ToDate)  'Today.Date
            Case 5 'Current Financial Year
                'Dim Month As Integer
                'Month = Today.Month
                ToDate = Request.QueryString("Date")
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))    '31-Mar-2006
                End If
                txtToDate.Value = CDate(ToDate)  'Today.Date
            Case 6 'Between Dates
                ToDate = Request.QueryString("Date")
                txtFromDate.Value = CDate(ToDate)  'Today.Date
                txtToDate.Value = CDate(ToDate)  'Today.Date
        End Select
    End Sub

    Private Sub CallFindNow(ByVal Index As Integer)
        'If txtNo.Text = "" Or IsNumeric(txtNo.Text) = False Then txtNo.Text = "0"
        Dim SalesOrderText = "", QuotationText As String = ""
        SalesOrderText = IIf(cmbSalesOrderText.SelectedIndex <= 0, "", cmbSalesOrderText.SelectedItem.Text)
        QuotationText = IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedItem.Text)
        Select Case Index
            Case -1
                Call FindNow("", "", , FromDate, ToDate, CInt(StatusId), Name, , )    'for all records
            Case 0  'all
                Call FindNow("", "", , FromDate, ToDate, CInt(StatusId), Name, , ) 'for all records
            Case 1 'date
                Call FindNow("", "", , txtFromDate.Value.ToString, txtToDate.Value.ToString, CInt(StatusId), Name, , )    'for all records
            Case 2  'Sales Order Teaxt ,No
                Call FindNow("", SalesOrderText, Val(No), FromDate, ToDate, CInt(StatusId), Name, , )   'for all records
            Case 3  'ItemName
                Call FindNow(Name, "", , FromDate, ToDate, CInt(StatusId), Name, , )
            Case 4 ' Vendor Name
                Call FindNow(, "", , FromDate, ToDate, CInt(StatusId), Name, , )
            Case 5 ' QuotationText 
                Call FindNow(, "", , FromDate, ToDate, CInt(StatusId), Name, QuotationText, Val(No))
            Case 6 ' Status
                Call FindNow(, "", , FromDate, ToDate, CInt(StatusId), Name, )
        End Select
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1800", Optional ByVal ToDate As String = "1/1/3050", Optional ByVal StatusID As Integer = 0, Optional ByVal VendorName As String = "", Optional ByVal QuotationText As String = "", Optional ByVal QuotationNo As Int16 = 0)
        mSalesOrderList = Nothing
        dgSalesOrderList.DataSource = Nothing
        'Get List From the Database as per Criteria             

        ''mSalesOrderList = SalesOrderList.GetSalesOrderList(ItemName, Text, No, FromDate, ToDate, StatusID, VendorName, QuotationText, QuotationNo)
        mSalesOrderList = PendingFromList.GetPendingFromList(PendingFromList.PendingListOf.SalesOrder, ItemName, Text, No, FromDate, ToDate, StatusID, VendorName, QuotationText, QuotationNo)

        'Set DataSource of the Grid
        Session("mSalesOrderList") = mSalesOrderList
        dgSalesOrderList.DataSource = mSalesOrderList
        lblResult.Text = "List of Sales Order as per criteria :" & mSalesOrderList.Count & " Record(s) found."
    End Sub

    Private Sub ShowItems(ByVal ID As Guid)
        mSalesOrder = SalesOrder.GetSalesOrder(ID)
        dgSalesOrderItems.DataSource = mSalesOrder.SalesOrderItems
        dgSalesOrderItems.DataBind()
        Session("mSalesOrder") = mSalesOrder
    End Sub
    Private Sub cmbSalesOrderText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSalesOrderText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        If cmbSalesOrderText.Enabled = True Then
            setFocus(cmbSalesOrderText)
        End If
    End Sub
#End Region

End Class
