Public Class wfTransactionsOfInventory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItemEnquiryList As ItemEnquiryList
    Public mItemQuotationList As ItemQuotationList
    Public mItemReceiptList As ItemReceiptList
    Public mItemOrderList As ItemOrderList
    Public mItemIssueList As ItemIssueList
    Dim FromDate, ToDate As String
    Dim DateIndex As Integer
    Dim ItemName As String
    Dim SerialNo As String
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        ItemName = Session("ItemName")
        mItemEnquiryList = Session("mItemEnquiryList")
        mItemQuotationList = Session("mItemQuotationList")
        mItemOrderList = Session("mItemOrderList")
        mItemReceiptList = Session("mItemReceiptList")
        mItemIssueList = Session("mItemIssueList")
        SerialNo = Session("SerialNo")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemEnquiryList")
        Session.Remove("mItemQuotationList")
        Session.Remove("mItemOrderList")
        Session.Remove("mItemReceiptList")
        Session.Remove("mItemIssueList")
        Session.Remove("SerialNo")
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-Jan-1900")
                txtToDate.Text = CDate("1-Jan-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = Today.AddDays(-6).ToShortDateString
                txtToDate.Text = Today.ToShortDateString
            Case 2 'Last 1 Month
                txtFromDate.Text = Today.AddDays(1).AddMonths(-1).ToShortDateString
                txtToDate.Text = Today.ToShortDateString
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToShortDateString
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToShortDateString
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToShortDateString
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToShortDateString
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToShortDateString
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToShortDateString
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToShortDateString
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToShortDateString
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToShortDateString
                txtToDate.Text = Today.ToShortDateString
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToShortDateString
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToShortDateString
                End If
                txtToDate.Text = Today.ToShortDateString
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date)
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date)
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Sub ControlVisibility(Optional ByVal DateIndex As Int32 = 0)
        txtFromDate.Visible = IIf(DateIndex <> 0, True, False)
        txtToDate.Visible = IIf(DateIndex <> 0, True, False)
        lblFromDate.Visible = IIf(DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(DateIndex <> 0, True, False)
        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbDocType" Then
            If cmbDocType.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Select Transaction"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            cmbDate.SelectedIndex = 4
            setPeriod(cmbDate.SelectedIndex)
            ControlVisibility(cmbDate.SelectedIndex)
        End If
        lbltitle.Text = "Part " + ItemName + " Status"
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            If cmbDocType.SelectedIndex = 1 Then  'Enqiry

                dgEnquiryList.Visible = True
                dgQuotationList.Visible = False
                dgOrderList.Visible = False
                dgReceiptCumInvoiceList.Visible = False
                dgIssueList.Visible = False

                lblEnquiryList.Visible = True
                lblQuotationList.Visible = False
                lblPurchaseOrderList.Visible = False
                lblReceiptList.Visible = False
                lblIssueList.Visible = False

                mItemEnquiryList = ItemEnquiryList.GetItemEnquiryList(ItemName, txtFromDate.Text.ToString, txtToDate.Text.ToString)
                dgEnquiryList.DataSource = mItemEnquiryList
                dgEnquiryList.DataBind()
                Session("mItemEnquiryList") = mItemEnquiryList
                lblEnquiryList.Text = "As per criteria :" & mItemEnquiryList.Count & " Record(s) found."
                If mItemEnquiryList.Count = 0 Then
                    btnPrint.Enabled = False
                Else
                    btnPrint.Enabled = True
                End If
            ElseIf cmbDocType.SelectedIndex = 2 Then  'Quotation

                dgEnquiryList.Visible = False
                dgQuotationList.Visible = True
                dgOrderList.Visible = False
                dgReceiptCumInvoiceList.Visible = False
                dgIssueList.Visible = False

                lblEnquiryList.Visible = False
                lblQuotationList.Visible = True
                lblPurchaseOrderList.Visible = False
                lblReceiptList.Visible = False
                lblIssueList.Visible = False

                mItemQuotationList = ItemQuotationList.GetItemQuotationList(ItemName, txtFromDate.Text.ToString, txtToDate.Text.ToString)
                dgQuotationList.DataSource = mItemQuotationList
                dgQuotationList.DataBind()
                Session("mItemQuotationList") = mItemQuotationList
                lblQuotationList.Text = "As per criteria :" & mItemQuotationList.Count & " Record(s) found."
                If mItemQuotationList.Count = 0 Then
                    btnPrint.Enabled = False
                Else
                    btnPrint.Enabled = True
                End If
            ElseIf cmbDocType.SelectedIndex = 3 Then  'Purchase Order

                dgEnquiryList.Visible = False
                dgQuotationList.Visible = False
                dgOrderList.Visible = True
                dgReceiptCumInvoiceList.Visible = False
                dgIssueList.Visible = False

                lblEnquiryList.Visible = False
                lblQuotationList.Visible = False
                lblPurchaseOrderList.Visible = True
                lblReceiptList.Visible = False
                lblIssueList.Visible = False

                mItemOrderList = ItemOrderList.GetItemOrderList(ItemName, txtFromDate.Text.ToString, txtToDate.Text.ToString)
                dgOrderList.DataSource = mItemOrderList
                dgOrderList.DataBind()
                Session("mItemOrderList") = mItemOrderList
                lblPurchaseOrderList.Text = "As per criteria :" & mItemOrderList.Count & " Record(s) found."
                If mItemOrderList.Count = 0 Then
                    btnPrint.Enabled = False
                Else
                    btnPrint.Enabled = True
                End If
            ElseIf cmbDocType.SelectedIndex = 4 Then  'Receipt

                dgEnquiryList.Visible = False
                dgQuotationList.Visible = False
                dgOrderList.Visible = False
                dgReceiptCumInvoiceList.Visible = True
                dgIssueList.Visible = False

                lblEnquiryList.Visible = False
                lblQuotationList.Visible = False
                lblPurchaseOrderList.Visible = False
                lblReceiptList.Visible = True
                lblIssueList.Visible = False

                mItemReceiptList = ItemReceiptList.GetItemReceiptList(ItemName, txtFromDate.Text.ToString, txtToDate.Text.ToString, SerialNo)
                dgReceiptCumInvoiceList.DataSource = mItemReceiptList
                dgReceiptCumInvoiceList.DataBind()
                Session("mItemReceiptList") = mItemReceiptList
                lblReceiptList.Text = "As per criteria :" & mItemReceiptList.Count & " Record(s) found."
                If mItemReceiptList.Count = 0 Then
                    btnPrint.Enabled = False
                Else
                    btnPrint.Enabled = True
                End If
            ElseIf cmbDocType.SelectedIndex = 5 Then  'Issue

                dgEnquiryList.Visible = False
                dgQuotationList.Visible = False
                dgOrderList.Visible = False
                dgReceiptCumInvoiceList.Visible = False
                dgIssueList.Visible = True

                lblEnquiryList.Visible = False
                lblQuotationList.Visible = False
                lblPurchaseOrderList.Visible = False
                lblReceiptList.Visible = False
                lblIssueList.Visible = True

                mItemIssueList = ItemIssueList.GetItemIssueList(ItemName, txtFromDate.Text.ToString, txtToDate.Text.ToString)
                dgIssueList.DataSource = mItemIssueList
                dgIssueList.DataBind()
                Session("mItemIssueList") = mItemIssueList
                lblIssueList.Text = "As per criteria :" & mItemIssueList.Count & " Record(s) found."
                If mItemIssueList.Count = 0 Then
                    btnPrint.Enabled = False
                Else
                    btnPrint.Enabled = True
                End If
            End If
            upnlGrid.Update()
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub dgEnquiryList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEnquiryList.PageIndexChanging
        dgEnquiryList.PageIndex = e.NewPageIndex
        dgEnquiryList.DataSource = mItemEnquiryList
        Session("mItemEnquiryList") = mItemEnquiryList
        dgEnquiryList.DataBind()
    End Sub
    Private Sub dgQuotationList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgQuotationList.PageIndexChanging
        dgQuotationList.PageIndex = e.NewPageIndex
        dgQuotationList.DataSource = mItemQuotationList
        Session("mItemQuotationList") = mItemQuotationList
        dgQuotationList.DataBind()
    End Sub
    Private Sub dgOrderList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgOrderList.PageIndexChanging
        dgOrderList.PageIndex = e.NewPageIndex
        dgOrderList.DataSource = mItemOrderList
        Session("mItemOrderList") = mItemOrderList
        dgOrderList.DataBind()
    End Sub
    Private Sub dgReceiptCumInvoiceList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReceiptCumInvoiceList.PageIndexChanging
         dgReceiptCumInvoiceList.PageIndex = e.NewPageIndex
        dgReceiptCumInvoiceList.DataSource = mItemReceiptList
        Session("mItemReceiptList") = mItemReceiptList
        dgReceiptCumInvoiceList.DataBind()
    End Sub
    Private Sub dgIssueList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgIssueList.PageIndexChanging
         dgIssueList.PageIndex = e.NewPageIndex
        dgIssueList.DataSource = mItemIssueList
        Session("mItemIssueList") = mItemIssueList
        dgIssueList.DataBind()
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Response.Redirect("Index.aspx")
        RemoveSession()
        Response.Redirect("DashboardForInventory.aspx")
    End Sub

    'Added by Shital on 20-Dec-2019
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim Rpt As New crptDashboardforInventory
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsDashboardforInventory

        Dim DateCriteria As String = ""
        If cmbDate.SelectedIndex = 0 Then
            DateCriteria = cmbDate.SelectedItem.Text
        Else
            DateCriteria = cmbDate.SelectedItem.Text + "  From Date :" + CDate(txtFromDate.Text).ToString("dd/MMM/yyyy") + "  To Date :" + CDate(txtToDate.Text).ToString("dd/MMM/yyyy")
        End If

        SearchStr1 = "The report shows records filtered by the following criteria"
        SearchStr2 = " Date:" + " " + DateCriteria.ToString


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, cmbDocType.SelectedItem.Text + " Transaction List", SearchStr1, SearchStr2, cmbDocType.SelectedIndex.ToString, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If (Not mItemEnquiryList Is Nothing) And cmbDocType.SelectedIndex = 1 Then
            If mItemEnquiryList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        ElseIf Not mItemQuotationList Is Nothing And cmbDocType.SelectedIndex = 2 Then
            If mItemQuotationList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        ElseIf Not mItemOrderList Is Nothing And cmbDocType.SelectedIndex = 3 Then
            If mItemOrderList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        ElseIf Not mItemReceiptList Is Nothing And cmbDocType.SelectedIndex = 4 Then
            If mItemReceiptList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        ElseIf Not mItemIssueList Is Nothing And cmbDocType.SelectedIndex = 5 Then
            If mItemIssueList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        If cmbDocType.SelectedIndex = 1 Then
            da.Fill(ds, "ItemEnquiryList", mItemEnquiryList)
        ElseIf cmbDocType.SelectedIndex = 2 Then
            da.Fill(ds, "ItemQuotationList", mItemQuotationList)
        ElseIf cmbDocType.SelectedIndex = 3 Then
            da.Fill(ds, "ItemOrderList", mItemOrderList)
        ElseIf cmbDocType.SelectedIndex = 4 Then
            da.Fill(ds, "ItemReceiptList", mItemReceiptList)
        Else
            da.Fill(ds, "ItemIssueList", mItemIssueList)
        End If
        da.Fill(ds, Report)

        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub

#End Region

End Class