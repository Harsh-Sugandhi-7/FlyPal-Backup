Public Class wfEnquiriesForQuotation_Ajax
    Inherits System.Web.UI.Page

#Region " Variables and Declarations "
    Public mQuotation As Quotation
    Public mDistinctTextListForEnquiry As DistinctTextListForEnquiry
    Public mPendingEnquiryList As PendingEnquiryList
    Public mEnquiry As Enquiry
    Dim SearchIndex, DateIndex, FromDate, ToDate, EnquiryText, Name, No As String
    Dim StatusId As String = "2"
#End Region

#Region "Business Methods"
    Private Sub SetControl()
        'setPeroid(DateIndex)
        ToDate = IIf(txtTransactionDate.Text.ToString <> "", txtTransactionDate.Text.ToString, "01/01/2050")
        Session("ToDate") = ToDate
        CallFindNow(SearchIndex)
        dgEnquiryList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        'cmbDate.SelectedIndex = DateIndex
        cmbEnquiryText.SelectedValue = IIf(EnquiryText = "", "(All)", EnquiryText)
        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Enquiry as per criteria :" & mPendingEnquiryList.Count & " Record(s) found."
    End Sub
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "01/01/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "01/01/2050", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
        StatusId = 2 'Session("StatusId")
        EnquiryText = Session("EnquiryText")
        Name = IIf(Session("Name") Is Nothing, "", Session("Name"))
        No = Session("No")
        mDistinctTextListForEnquiry = DistinctTextListForEnquiry.GetDistinctTextList("7", , True, "(All)")
        cmbEnquiryText.DataSource = mDistinctTextListForEnquiry
        'mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(PendingFromList.PendingListOf.Enquiry, "", "", 0, "01/01/1900", ToDate, 2 , "")
        If mQuotation.TransTypeID = Util.Trans.Quotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList("", "", 0, "01/01/1900", ToDate, 2, Name, Util.Trans.Enquiry, mQuotation.VendorID.ToString)
        ElseIf mQuotation.TransTypeID = Util.Trans.PurchaseQuotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList("", "", 0, "01/01/1900", ToDate, 2, Name, Util.Trans.RequestingForQuotation, mQuotation.VendorID.ToString)
        ElseIf mQuotation.TransTypeID = Util.Trans.RentialLeaseQuotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList("", "", 0, "01/01/1900", ToDate, 2, Name, Util.Trans.RentialLeaseEnquiry, mQuotation.VendorID.ToString)
        ElseIf mQuotation.TransTypeID = Util.Trans.OverHaulRepairQuotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList("", "", 0, "01/01/1900", ToDate, 2, Name, Util.Trans.OverHaulRepairEnquiry, mQuotation.VendorID.ToString)
        End If
        dgEnquiryList.DataSource = mPendingEnquiryList
        Session("mPendingEnquiryList") = mPendingEnquiryList
        txtTransactionDate.Text = mQuotation.DateFormatted.ToString
        DataBind()
        lblResult.Text = "List of Enquiry as per criteria :" & mPendingEnquiryList.Count & " Record(s) found."
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mQuotation = Session("mQuotation")
        mEnquiry = Session("mEnquiry")
        mPendingEnquiryList = Session("mPendingEnquiryList")
        mDistinctTextListForEnquiry = Session("mDistinctTextListForEnquiry")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = 2 'Session("StatusId")
        EnquiryText = Session("EnquiryText")
        Name = IIf(Session("Name") Is Nothing, "", Session("Name"))
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
    End Sub
    Private Sub SetSession()
        Session("mQuotation") = mQuotation
        Session("mEnquiry") = mEnquiry
        Session("mPendingEnquiryList") = mPendingEnquiryList
        Session("mDistinctTextListForEnquiry") = mDistinctTextListForEnquiry
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEnquiry")
        Session.Remove("mPendingEnquiryList")
        Session.Remove("mDistinctTextListForEnquiry")
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        'DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        'FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "01/01/1900")
        ToDate = IIf(txtTransactionDate.Text.ToString <> "", txtTransactionDate.Text.ToString, "01/01/2050")
        StatusId = 2 'Authorized
        EnquiryText = IIf(cmbEnquiryText.SelectedIndex <= 0, "", cmbEnquiryText.SelectedValue)
        No = txtNo.Text.Trim
        'Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        'Session("SearchIndex") = SearchIndex
        'Session("DateIndex") = DateIndex
        'Session("StatusId") = StatusId
        'Session("EnquiryText") = EnquiryText
        'Session("No") = No
        'Session("Name") = Name
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        'cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        'lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        'lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        'If SearchIndex = 1 And DateIndex = 6 Then
        '    txtFromDate.Visible = True
        '    txtToDate.Visible = True
        '    txtFromDate.Enabled = True
        '    txtToDate.Enabled = False
        'ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
        '    txtFromDate.Visible = True
        '    txtToDate.Visible = True
        '    txtFromDate.Enabled = False
        '    txtToDate.Enabled = False
        'Else
        '    txtFromDate.Visible = False
        '    txtToDate.Visible = False
        'End If
        ''txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        ''calFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)

        ''txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        ''If txtToDate.Visible Then txtToDate.Enabled = False

        ''calToDate.Visible = False 'IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)

        cmbEnquiryText.Visible = IIf(SearchIndex = 1, True, False)
        lblNo.Visible = IIf(SearchIndex = 1 And cmbEnquiryText.SelectedIndex <> 0, True, False)
        txtNo.Visible = IIf(SearchIndex = 1 And cmbEnquiryText.SelectedIndex <> 0, True, False)
        txtName.Visible = IIf(SearchIndex = 2 Or SearchIndex = 4, True, False)
        txtTransactionDate.Enabled = IIf(mQuotation.QuotationItems.Count > 0, False, True)
    End Sub
    'Private Sub setPeroid(ByVal Index As Int32)
    '     Select Case Index
    '        Case 0 'All'
    '            txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
    '            txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
    '        Case 1 'Last 1 Week
    '            txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
    '            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
    '        Case 2 'Last 1 Month
    '            txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
    '            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
    '        Case 3 'Last 1 Quater
    '            Select Case Today.Month
    '                Case 1, 2, 3
    '                    txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
    '                    txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
    '                Case 4, 5, 6
    '                    txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
    '                    txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
    '                Case 7, 8, 9
    '                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
    '                    txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
    '                Case 10, 11, 12
    '                    txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
    '                    txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
    '            End Select
    '        Case 4 'Last 1 Year
    '            txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
    '            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
    '        Case 5 'Current Financial Year
    '            If Today.Month <= 3 Then  'Jan|Feb|Mar
    '                txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
    '            Else
    '                txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
    '            End If
    '            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
    '        Case 6 'Between Dates
    '            FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
    '            ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
    '            txtFromDate.Text = FromDate
    '            txtToDate.Text = ToDate
    '    End Select
    'End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        'If txtNo.Text = "" Or IsNumeric(txtNo.Text) = False Then txtNo.Text = "0"
        Dim EnquiryText As String = ""
        EnquiryText = IIf(cmbEnquiryText.SelectedIndex <= 0, "", cmbEnquiryText.SelectedItem.Text)
        Select Case Index
            Case -1
                Call FindNow(, , , "1-Jan-1900", ToDate, 2, Name)    'for all records
            Case 0  'all
                Call FindNow(, , , "1-Jan-1900", ToDate, 2, Name) 'for all records
                'Case 1 'Enquiry date
                '    Call FindNow("", "", 0, txtFromDate.Text, ToDate, 2, Name)
            Case 1 'Enquiry Text , No 
                Call FindNow("", EnquiryText, CInt(Val(txtNo.Text)), "1-Jan-1900", ToDate, 2, Name)
            Case 2  'ItemName
                Call FindNow(txtName.Text, "", 0, "1/1/1900", ToDate, 2, Name)
        End Select
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1800", Optional ByVal ToDate As String = "1/1/3050", Optional ByVal StatusID As Integer = 0, Optional ByVal VendorName As String = "")
        mPendingEnquiryList = Nothing
        dgEnquiryList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        ''mPendingEnquiryList = EnquiryList.GetEnquiryList(ItemName, Text, No, FromDate, ToDate, StatusID, RequestingLocation)
        'mPendingEnquiryList = PendingFromList.GetPendingFromList(PendingFromList.PendingListOf.Enquiry, ItemName, Text, No, FromDate, ToDate, StatusID, , , , , , RequestingLocation)
        If mQuotation.TransTypeID = Util.Trans.Quotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(ItemName, Text, No, FromDate, ToDate, 2, Name, Util.Trans.Enquiry, mQuotation.VendorID.ToString)
        ElseIf mQuotation.TransTypeID = Util.Trans.PurchaseQuotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(ItemName, Text, No, FromDate, ToDate, 2, Name, Util.Trans.RequestingForQuotation, mQuotation.VendorID.ToString)
        ElseIf mQuotation.TransTypeID = Util.Trans.RentialLeaseQuotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(ItemName, Text, No, FromDate, ToDate, 2, Name, Util.Trans.RentialLeaseEnquiry, mQuotation.VendorID.ToString)
        ElseIf mQuotation.TransTypeID = Util.Trans.OverHaulRepairQuotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(ItemName, Text, No, FromDate, ToDate, 2, Name, Util.Trans.OverHaulRepairEnquiry, mQuotation.VendorID.ToString)
        End If
        'Set DataSource of the Grid
        Session("mPendingEnquiryList") = mPendingEnquiryList
        dgEnquiryList.DataSource = mPendingEnquiryList
        lblResult.Text = "List of Enquiry as per criteria :" & mPendingEnquiryList.Count & " Record(s) found."

    End Sub
    Private Sub ShowItems(ByVal ID As Guid)
        mEnquiry = Enquiry.GetEnquiryForQuotation(ID)
        dgEnquiryItems.DataSource = mEnquiry.EnquiryItems
        dgEnquiryItems.DataBind()
        Session("mEnquiry") = mEnquiry
        Session("mQuotation") = mQuotation
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            ToDate = (New SmartDate(Request.QueryString("Date"))).FormattedText
            Session("ToDate") = ToDate
            If Not mQuotation.VendorID.Equals(Guid.Empty) Then
                Name = VendorList.GetVendortList(0).Item(mQuotation.VendorID).Name
                Session("Name") = Name
            End If
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            DataFieldBind()
            SetControl()
        End If
    End Sub
    Protected Sub txtTransactionDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        setVariables()
        CallFindNow(SearchIndex)
        dgEnquiryList.DataBind()
        upnlEnquiryList.Update()
        lblResult.Text = "List of Enquiry as per criteria :" & mPendingEnquiryList.Count & " Record(s) found."
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        'cmbDate.SelectedIndex = 0
        cmbEnquiryText.SelectedIndex = 0
        ClearControls()
        'Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex)
        'setPeroid(DateIndex)
        ToDate = IIf(txtTransactionDate.Text.ToString <> "", txtTransactionDate.Text.ToString, "01/01/2050")
        Session("ToDate") = ToDate
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    Private Sub cmbEnquiryText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEnquiryText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        'Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        If cmbEnquiryText.Enabled = True Then
            setFocus(cmbEnquiryText)
        End If
    End Sub
    'Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
    '    ClearControls()
    '    Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
    '    'Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
    '    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
    '    setPeroid(DateIndex)
    '    If cmbDate.Enabled = True Then
    '        setFocus(cmbDate)
    '    End If
    'End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgEnquiryList.DataBind()
        upnlEnquiryList.Update()
        lblResult.Text = "List of Enquiry as per criteria :" & mPendingEnquiryList.Count & " Record(s) found."
    End Sub
    'Private Sub dgEnquiryList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEnquiryList.ItemCommand
    '    If e.Item.Cells(0).Text = "ID" Or e.Item.Cells(0).Text = "" Then Exit Sub
    '    Dim mID As New Guid(e.Item.Cells(0).Text)
    '    Select Case e.CommandName
    '        Case "Select"
    '            lblCallOutJobs.Visible = True
    '            ShowItems(mID)
    'If mEnquiry.EnquiryItems.Count >= 0 Then
    '     btnDone.Enabled = True
    ' Else
    '     btnDone.Enabled = False
    ' End If
    '    End Select
    'End Sub
    Private Sub dgEnquiryList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEnquiryList.RowCommand
        Select Case e.CommandName
            Case "SelectRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgEnquiryList.PageIndex * dgEnquiryList.PageSize
                Dim mID As New Guid '(e.Item.Cells(0).Text)
                mID = mPendingEnquiryList(index).EnquiryID
                ShowItems(mID)
               
                If mEnquiry.EnquiryItems.Count >= 0 Then
                    btnDone.Enabled = True
                    mQuotation.VendorID = mPendingEnquiryList(index).VendorID
                Else
                    btnDone.Enabled = False
                End If
                upnlButtons.Update()
                upnlEnquiryItems.Update()
        End Select
    End Sub
    Private Sub dgEnquiryList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEnquiryList.PageIndexChanging
        dgEnquiryList.PageIndex = e.NewPageIndex
        dgEnquiryList.DataSource = mPendingEnquiryList
        Session("mPendingEnquiryList") = mPendingEnquiryList
        dgEnquiryList.DataBind()
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        Dim chkSelect As CheckBox
        For I As Integer = 0 To dgEnquiryItems.Rows.Count - 1
            'mQuotation.VendorID = mPendingEnquiryList(mEnquiry.ID, "").VendorID
            chkSelect = CType(dgEnquiryItems.Rows(I).FindControl("chkSelect"), CheckBox)
            mEnquiry.EnquiryItems.Item(I).IsSelect = chkSelect.Checked
            mEnquiry.EnquiryItems.Item(I).MarkClean()
        Next
        Session("AddEnquiryParts") = "True"
        Session("mEnquiry") = mEnquiry
        Session.Remove("Name")
        Session("TransactionDate") = txtTransactionDate.Text
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("IsBackFromPendingList") = "True"
        Session.Remove("Name")
        Response.Redirect(Request.QueryString("BackPage1"))
    End Sub
    Private Sub dgEnquiryList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEnquiryList.Sorting
        mPendingEnquiryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingEnquiryList") = mPendingEnquiryList
        dgEnquiryList.DataSource = mPendingEnquiryList
        dgEnquiryList.DataBind()
        upnlEnquiryList.Update()
    End Sub
#End Region

End Class