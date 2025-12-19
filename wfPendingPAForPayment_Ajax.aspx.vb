Public Class wfPendingPAForPayment_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mPaymentAdvice As PaymentAdvice
    Public mPendingPAForPayment As PendingPAForPayment
    Dim mTransactionListCount As TransactionListCount
    Dim mPaymentAdviceListForReminderReport As PaymentAdviceListForReminderReport
    Dim DateIndex As String = ""
    Dim OrderName As String = ""
    Dim VendorName As String = ""
    Dim SerialNo As String = ""

    Dim StatusID, PaymentAdviceText, No, OrderText, OrderNo As String
    Public FromDate As String = "1-1-1900"
    Public ToDate As String = "1-1-2200"
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mDistinctTextListForPaymentAdvice As DistinctTextListForOrder
    Protected mtmpVendorList As VendorList
#End Region

#Region " Enum "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum
#End Region

#Region "Methods"
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfPendingPAForPayment_Ajax.aspx?" Then
            Session.Remove("PAFromDate")
            Session.Remove("PAToDate")
            Session.Remove("PADateIndex")
            Session.Remove("PAOrderName")
            Session.Remove("PASerialNo")
            Session.Remove("PAOrderText")
            Session.Remove("PAOrderNo")
            Session.Remove("PANo")
            Session.Remove("PAVendorName")
            Session.Remove("mPendingPAForPayment")
        End If
    End Sub
    Public Sub GetSession()
        mPaymentAdvice = Session("mPaymentAdvice")
        mTransactionListCount = Session("mTransactionListCount")
        mPendingPAForPayment = Session("mPendingPAForPayment")
        OrderName = Session("PAOrderName")
        SerialNo = Session("PASerialNo")
        'StatusID = Session("Status")
        FromDate = Session("PAFromDate")
        ToDate = Session("PAToDate")
        DateIndex = Session("PADateIndex")
        OrderText = Session("PAOrderText")
        OrderNo = Session("PAOrderNo")
        PaymentAdviceText = Session("PaymentAdviceText")
        No = Session("PANo")
        VendorName = Session("PAVendorName")

    End Sub
    Public Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

        Session("PAFromDate") = FromDate
        Session("PAToDate") = ToDate
        Session("PADateIndex") = DateIndex

        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder

        mDistinctTextListForPaymentAdvice = DistinctTextListForOrder.GetDistinctTextList("29", , True, "(All)")
        cmbPaymentAdviceText.DataSource = mDistinctTextListForPaymentAdvice

        mtmpVendorList = VendorList.GetVendortList(0, , , , , , True, False, True)
        cmbSupplier.DataSource = mtmpVendorList
        Session("mtmpVendorList") = mtmpVendorList

        DataBind()

        mTransactionListCount = TransactionListCount.GetTransactionListCountt(Util.Trans.PaymentAdvice, 5)
        Session("mTransactionListCount") = mTransactionListCount
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        IsInRoleString = "PendingPA"

        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Sub SetControl()
        SetPeriod(DateIndex)

        OrderName = IIf(OrderName Is Nothing, txtOrderNo.Text.Trim, OrderName)
        OrderText = IIf(OrderText Is Nothing, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.ToString), OrderText)
        OrderNo = IIf(OrderNo Is Nothing, txtOrderNo.Text.Trim, OrderNo)
        SerialNo = IIf(SerialNo Is Nothing, txtNo.Text.Trim, SerialNo)
        FromDate = IIf(Not (txtFromDate.Text.ToString = ""), txtFromDate.Text.ToString, FromDate)
        ToDate = IIf(Not (txtToDate.Text.ToString = ""), txtToDate.Text.ToString, ToDate)
        PaymentAdviceText = IIf(PaymentAdviceText Is Nothing, IIf(cmbPaymentAdviceText.SelectedIndex <= 0, "", cmbPaymentAdviceText.SelectedValue), PaymentAdviceText)
        No = IIf(No Is Nothing, txtNo.Text.Trim, No)
        VendorName = IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.ToString)
        Session("PAOrderText") = OrderText
        Session("PAOrderNo") = OrderNo
        Session("PaymentAdviceText") = PaymentAdviceText
        Session("PANo") = No
        Session("PAVendorName") = VendorName

        cmbDate.SelectedIndex = DateIndex

        If mDistinctTextListForOrder.Contains(OrderText) Then
            cmbOrderText.SelectedValue = IIf(OrderText = "", "(All)", OrderText)
        Else
            cmbOrderText.SelectedValue = "(All)"
        End If
        txtOrderNo.Text = OrderNo

        If mDistinctTextListForPaymentAdvice.Contains(PaymentAdviceText) Then
            cmbPaymentAdviceText.SelectedValue = IIf(PaymentAdviceText = "", "(All)", PaymentAdviceText)
        Else
            cmbPaymentAdviceText.SelectedValue = "(All)"
        End If
        txtNo.Text = No.ToString
        ' cmbStatus.SelectedValue = StatusID

        If mtmpVendorList.Contains(VendorName) Then
            cmbSupplier.SelectedValue = IIf(VendorName = "", "(SELECT)", VendorName)
        Else
            cmbSupplier.SelectedIndex = 0
        End If


        mPendingPAForPayment = PendingPAForPayment.GetPenndingPAForPaymentList(FromDate, ToDate, VendorName.ToString, PaymentAdviceText.ToString, Val(No), txtSupplierInvoiceNo.Text.ToString, OrderText, OrderNo)
        Session("mPendingPAForPayment") = mPendingPAForPayment

        dgPaymentAdviceList.DataSource = mPendingPAForPayment
        dgPaymentAdviceList.DataBind()
        ControlVisibility(DateIndex)
        upnldgPaymentAdvice.Update()

        'SetTitle()

        lblResult.Text = "List of Pending PA for Payment as per criteria :" & mPendingPAForPayment.Count & " Record(s) found."
        upnlResult.Update()
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        ' StatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue)
        OrderNo = txtOrderNo.Text.Trim
        PaymentAdviceText = IIf(cmbPaymentAdviceText.SelectedIndex <= 0, "", cmbPaymentAdviceText.SelectedValue)
        No = txtNo.Text.Trim

        VendorName = IIf(cmbSupplier.SelectedIndex <= 0, "", cmbSupplier.SelectedItem.ToString)

        Session("PADateIndex") = DateIndex
        Session("PAFromDate") = FromDate
        Session("PAToDate") = ToDate
        ' Session("StatusId") = StatusID
        Session("PAOrderText") = OrderText
        Session("PAOrderNo") = OrderNo
        Session("PAPaymentAdviceText") = PaymentAdviceText
        Session("PANo") = No
        Session("PAVendorName") = VendorName

        mPendingPAForPayment = Nothing
        dgPaymentAdviceList.DataSource = Nothing

        mPendingPAForPayment = PendingPAForPayment.GetPenndingPAForPaymentList(FromDate, ToDate, VendorName.ToString, PaymentAdviceText.ToString, Val(No), txtSupplierInvoiceNo.Text.ToString, OrderText, OrderNo)
        Session("mPendingPAForPayment") = mPendingPAForPayment
        dgPaymentAdviceList.DataSource = mPendingPAForPayment
        dgPaymentAdviceList.DataBind()
        ControlVisibility(DateIndex)
        upnldgPaymentAdvice.Update()

        BottomActionButtonVisibility()

        lblResult.Text = "List of Pending PA for Payment as per criteria :" & mPendingPAForPayment.Count & " Record(s) found."
        upnlResult.Update()
    End Sub
    Private Sub SetPeriod(ByVal index As Int32)
        Select Case index
            Case 0 ' All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
        Session("PAFromDate") = txtFromDate.Text
        Session("PAToDate") = txtToDate.Text
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        upnlSearch.Update()

    End Sub
    Private Sub BottomActionButtonVisibility()

        btnBottomClose.Visible = IIf(mPendingPAForPayment.Count > 25, True, False)
        btnPrint.Enabled = IIf(mPendingPAForPayment.Count = 0, False, True)
        upnlActionBtnTop.Update()
        upnlBottomActionButton.Update()

    End Sub
    Private Sub SetTitle()

        If mTransactionListCount.Count = 0 Then
            lbltitle.Text = "List of Pending PA(s) [Total No of Record(s):- 0 ]"
        Else
            lbltitle.Text = "List of Pending PA(s) [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"
        End If

        upnltitle.Update()
    End Sub
    Private Sub SetGrid()

    End Sub
    Public Sub GridBind()
        dgPaymentAdviceList.DataBind()
        upnldgPaymentAdvice.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfPendingPAForPayment_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            BottomActionButtonVisibility()
            SetTitle()

        End If
    End Sub
    Private Sub dgPaymentAdviceList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPaymentAdviceList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mPaymentAdvice = PaymentAdvice.GetPaymentAdvice(ID)
                Session("mPaymentAdvice") = mPaymentAdvice

                Dim PaymentDetail As String = mPaymentAdvice.PaymentNo + " Dated : " + mPaymentAdvice.PaymentDateFormatted + " to " + mPaymentAdvice.VendorName & " Created By : " & mPaymentAdvice.CreatedBy
                MarkLog(Util.Action.Edit, "Payment Advice", PaymentDetail, Util.ErrorType.NoError, mPaymentAdvice.ID, EventLogID)

                Session("IsFromPendingPAPaymentPage") = True
                Dim str As String
                str = "openledgersame('wfPaymentAdvice_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End Select
    End Sub
    Private Sub dgPaymentAdviceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPaymentAdviceList.Sorting
        mPendingPAForPayment.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingPAForPayment") = mPendingPAForPayment
        dgPaymentAdviceList.DataSource = mPendingPAForPayment
        GridBind()
        SetGrid()
    End Sub
    Private Sub dgPaymentAdviceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPaymentAdviceList.PageIndexChanging
        dgPaymentAdviceList.PageIndex = e.NewPageIndex
        dgPaymentAdviceList.DataSource = mPendingPAForPayment
        Session("mPendingPAForPayment") = mPendingPAForPayment
        GridBind()
        SetGrid()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnBottomClose.Click
        ClearAll()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        SetPeriod(DateIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        Session("PAFromDate") = FromDate
        Session("PAToDate") = ToDate
        Session("PADateIndex") = DateIndex
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click

        Dim SearchStr1 As String = ""
        Dim SearchStr2 As String = ""
        Dim SearchStr3 As String = ""
        Dim SearchStr4 As String = ""
        Dim SearchStr5 As String = ""
        Dim SearchStr6 As String = ""

        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsPaymentAdviceListForReminderReport
        Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        rpt = New crptPaymentAdviceReminder

        Dim mCompanyDetail As CompanyDetail

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        If cmbDate.SelectedIndex = 0 Then
            'All
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbDate.SelectedIndex <> 0 Then
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = lblFromDate.Text + " : " + txtFromDate.Text + " " + lblToDate.Text + " : " + " " + txtToDate.Text
        End If

        If cmbPaymentAdviceText.SelectedIndex <> 0 Or txtNo.Text <> "0" Then
            SearchStr3 = "Payment Advice No : " + cmbPaymentAdviceText.SelectedItem.ToString + " " + txtNo.Text
        End If

        If cmbSupplier.SelectedIndex <> 0 Then
            SearchStr4 = "Supplier : " + cmbSupplier.SelectedItem.ToString
        End If

        If cmbOrderText.SelectedIndex <> 0 Or txtOrderNo.Text <> "0" Then
            SearchStr5 = "Order No : " + cmbOrderText.SelectedItem.ToString + " " + txtOrderNo.Text
        End If

        If txtSupplierInvoiceNo.Text <> "0" Then
            SearchStr6 = "Supplier Invoice No : " + txtSupplierInvoiceNo.Text
        End If


        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "Payment Advice", SearchStr1, SearchStr2, SearchStr3, SearchStr4, SearchStr5, AppSettings("Product Version"), AppSettings("SINote"), SearchStr6, "", "", "", AppSettings("Logo"))

        mPaymentAdviceListForReminderReport = PaymentAdviceListForReminderReport.GetPaymentAdviceListForReminderRport(FromDate, ToDate, VendorName.ToString, PaymentAdviceText, No, txtSupplierInvoiceNo.Text.ToString, OrderText, OrderNo)

        Session("mPaymentAdviceListForRegisterReport") = mPaymentAdviceListForReminderReport

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        da.Fill(ds, mPaymentAdviceListForReminderReport)
        rpt.SetDataSource(ds)

        Session("CrystalReport") = rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region


End Class