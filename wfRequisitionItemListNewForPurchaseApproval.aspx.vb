Partial Class wfRequisitionItemListNewForPurchaseApproval
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar
    Protected WithEvents lblTotal As System.Web.UI.WebControls.Label
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
    Public mRequisitionItemListNewForPurchaseApproval As RequisitionItemListNewForPurchaseApproval
    Public mRequisition As Requisition
    Dim SearchIndex, DateIndex, FromDate, ToDate, Name As String
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mRequisitionItemListNewForPurchaseApproval = Session("mRequisitionItemListNewForPurchaseApproval")
        mRequisition = Session("mRequisition")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        Name = Session("Name")
    End Sub
    Private Sub SetSession()
        Session("mRequisitionItemListNewForPurchaseApproval") = mRequisitionItemListNewForPurchaseApproval
        Session("mRequisition") = mRequisition
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRequisitionItemListNewForPurchaseApproval")
        Session.Remove("mRequisition")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfRequisitionItemListNewForPurchaseApproval.aspx?" Then
            Session.Remove("mRequisition")
            Session.Remove("mRequisitionItemListNewForPurchaseApproval")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("Name")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgApprovalList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        txtName.Text = Name
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Requisition as per criteria :" & mRequisitionItemListNewForPurchaseApproval.Count & " Record(s) found."
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", str)
    End Sub
    Private Sub findNow(ByVal PartNo As String, ByVal Description As String, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal RegNo As String = "")
        dgApprovalList.CurrentPageIndex = 0
        mRequisitionItemListNewForPurchaseApproval = RequisitionItemListNewForPurchaseApproval.GetRequisitionItemListNewForPurchaseApproval(PartNo, FromDate, ToDate, RegNo)
        Session("mRequisitionItemListNewForPurchaseApproval") = mRequisitionItemListNewForPurchaseApproval
        dgApprovalList.DataSource = mRequisitionItemListNewForPurchaseApproval
        lblResult.Text = "List of Logistic Approval as per criteria :" & mRequisitionItemListNewForPurchaseApproval.Count & " Record(s) found."
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        Select Case Index
            Case -1
                Call findNow("", "")
            Case 0  'all
                Call findNow("", "")
            Case 1 'date
                Call findNow("", "", txtFromDate.Value.ToString, txtToDate.Value.ToString)
            Case 2  'PartNo 
                Call findNow(Name, "", FromDate, ToDate)
        End Select
        dgApprovalList.CurrentPageIndex = 0  'Added Code on May,25,2007
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Value = CDate("01-01-1900")
                txtToDate.Value = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Value = CDate(Today.AddDays(-6))
                txtToDate.Value = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Value = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Value = Today.Date
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
                txtFromDate.Value = Today.AddDays(1).AddYears(-1)
                txtToDate.Value = Today.Date
            Case 5 'Current Financial Year
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Value = Today.Date
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Value = FromDate
                txtToDate.Value = ToDate
        End Select
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        txtName.Visible = IIf(SearchIndex = 2 Or SearchIndex = 3 Or SearchIndex = 4, True, False)
    End Sub
    Private Sub ClearControls()
        txtName.Text = ""
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Value.ToString <> "", txtFromDate.Value.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Value.ToString <> "", txtToDate.Value.ToString, "1/1/2200")
        Name = txtName.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("Name") = Name
    End Sub
    Private Sub setTitle()
        btnClose.ToolTip = "Click to Close the Requisition For Logistic Purchase Approval List screen"
        btnCloseTop.ToolTip = "Click to Close the Requisition For Logistic Purchase Approval List screen"
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        'Commented and added by Shweta on 19-August-2013 for ALL16082013-1
        'DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        'End
        Name = Session("Name")
        mRequisitionItemListNewForPurchaseApproval = RequisitionItemListNewForPurchaseApproval.GetRequisitionItemListNewForPurchaseApproval("", "1/1/1900", "1/1/2200")
        Session("mRequisitionItemListNewForPurchaseApproval") = mRequisitionItemListNewForPurchaseApproval
        dgApprovalList.DataSource = mRequisitionItemListNewForPurchaseApproval
        DataBind()
        Session("TotalEngApproval") = "   [Total No of Record(s):-" & mRequisitionItemListNewForPurchaseApproval.Count & "]"
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbSearch.Enabled = True Then
                SetFocus(cmbSearch)
            End If
            Session("MiddleFrame") = "wfRequisitionItemListNewForPurchaseApproval.aspx?"
            DataFieldBind()
            SetControl()
        End If
        setTitle()
        LblTitle.Text = "Requisition Item List For Purchase Approval " + Session("TotalEngApproval").ToString
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgApprovalList.DataBind()
        BtnPrint.Enabled = IIf(mRequisitionItemListNewForPurchaseApproval.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mRequisitionItemListNewForPurchaseApproval.Count = 0, False, True)
        lblResult.Text = "List of Requisition as per criteria :" & mRequisitionItemListNewForPurchaseApproval.Count & " Record(s) found."
    End Sub
    Private Sub dgApprovalList_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgApprovalList.PageIndexChanged
        dgApprovalList.CurrentPageIndex = e.NewPageIndex
        dgApprovalList.DataSource = mRequisitionItemListNewForPurchaseApproval
        Session("mRequisitionItemListNewForPurchaseApproval") = mRequisitionItemListNewForPurchaseApproval
        dgApprovalList.DataBind()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDate.SelectedIndex = 0
        ClearControls()
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        SetFocus(cmbSearch)
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        SetFocus(cmbDate)
    End Sub
    Private Sub dgApprovalList_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgApprovalList.ItemCommand
        Select Case e.CommandName
            Case "Detail"
                Dim Index As Int32 = e.Item.ItemIndex + dgApprovalList.CurrentPageIndex * dgApprovalList.PageSize
                Dim mID As New Guid(e.Item.Cells(0).Text)
                If User.IsInRole("RequisitionNewForPurchaseApprovalEdit") = False Then
                    MarkLog(Util.Action.Edit, "Requisition Approval List", User.Identity.Name & " is not Authorized User to edit " & mRequisitionItemListNewForPurchaseApproval(Index).PartNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                mRequisitionItemListNewForPurchaseApproval.CurrentIndex = Index
                Session("Index") = Index
                Session("mRequisitionItemListNewForPurchaseApproval") = mRequisitionItemListNewForPurchaseApproval
                Dim Detail As String = " Requisition No. : " + e.Item.Cells(2).Text + " Dated : " + e.Item.Cells(1).Text + " Part No. : " + e.Item.Cells(3).Text
                MarkLog(Util.Action.Edit, "Requisition Approval List", Detail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                Dim str As String
                str = "<script language='javascript'>openledgersame('wfRequisitionNewForPurchaseApproval.aspx?BackPage=index.aspx');</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        End Select
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim str As String
        str = "<script language='javascript'>  openledgersame('wfRequisitionHistoryList.aspx?" & "&BackPage=Index.aspx'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
    End Sub
    Private Sub dgApprovalList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgApprovalList.SortCommand
        mRequisitionItemListNewForPurchaseApproval.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItemListNewForPurchaseApproval") = mRequisitionItemListNewForPurchaseApproval
        dgApprovalList.DataSource = mRequisitionItemListNewForPurchaseApproval
        dgApprovalList.DataBind()
    End Sub
#End Region

End Class
