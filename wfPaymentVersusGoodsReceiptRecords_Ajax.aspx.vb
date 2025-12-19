Public Class wfPaymentVersusGoodsReceiptRecords_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mPaymentVersusGoodsReceiptRecords As PaymentVersusGoodsReceiptRecords
    Private mPaymentAdvice As PaymentAdvice
    Dim EventLogID As Guid
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mDistinctTextListForPaymentAdvice As DistinctTextListForOrder
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Protected mtmpVendorList As VendorList
#End Region

#Region "Methods"
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfPaymentVersusGoodsReceiptRecords_Ajax.aspx?" Then
            Session.Remove("mPaymentAdviceList")
            Session.Remove("OrderName")
            Session.Remove("Status")
        End If
    End Sub
    Public Sub GetSession()
    End Sub
    Public Sub DataFieldBind()

        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder

        mDistinctTextListForPaymentAdvice = DistinctTextListForOrder.GetDistinctTextList("29", , True, "(All)")
        cmbPaymentAdviceText.DataSource = mDistinctTextListForPaymentAdvice

        mtmpVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbSupplier.DataSource = mtmpVendorList
        Session("mtmpVendorList") = mtmpVendorList

        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("19", , True, "(All)")
        cmbReceiptText.DataSource = mDistinctTextListForReceipt

        DataBind()
    End Sub
    Private Sub MessageBoxResult()

    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfPaymentVersusGoodsReceiptRecords_Ajax.aspx?"
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ClearAll()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            mPaymentVersusGoodsReceiptRecords = PaymentVersusGoodsReceiptRecords.GetPaymentVersusGoodsReceiptRecords(IIf(cmbSupplier.SelectedIndex = 0, Guid.Empty.ToString, cmbSupplier.SelectedValue.ToString), _
                                                                                                                         IIf(cmbPaymentAdviceText.SelectedIndex = 0, "", cmbPaymentAdviceText.SelectedItem.Text), _
                                                                                                                         Val(txtNo.Text.Trim), IIf(cmbReceiptText.SelectedIndex = 0, "", cmbReceiptText.SelectedItem.Text), _
                                                                                                                         Val(txtReceiptNo.Text.Trim), IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), _
                                                                                                                         Val(txtOrderNo.Text.Trim), txtAmend.Text, txtProformaInv.Text.Trim, txtFromDate.Text, txtToDate.Text)
            If mPaymentVersusGoodsReceiptRecords.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1418)
            End If

            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsPaymentVersusGoodsReceiptRecords
            Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
            rpt = New crptPaymentVersusGoodsReceiptRecords

            Dim mCompanyDetail As CompanyDetail
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, _
            mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
            mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
            "PAYMENT VERSUS GOODS RECEIPT REGISTER", _
            SearchStr1:=IIf(cmbSupplier.SelectedIndex = 0, "", cmbSupplier.SelectedItem.ToString), _
            SearchStr2:=IIf(cmbPaymentAdviceText.SelectedIndex = 0, "", cmbPaymentAdviceText.SelectedItem.Text), _
            SearchStr3:=txtNo.Text.Trim, SearchStr4:=IIf(cmbReceiptText.SelectedIndex = 0, "", cmbReceiptText.SelectedItem.Text), _
            SearchStr5:=txtReceiptNo.Text.Trim, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
            SearchStr6:=IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), SearchStr7:=txtOrderNo.Text.Trim, _
            SearchStr8:=txtAmend.Text.Trim, SearchStr9:=txtProformaInv.Text, SearchStr10:=AppSettings("Logo"), _
            SearchStr11:=New SmartDate(txtFromDate.Text).FormattedText, _
            SearchStr12:=New SmartDate(txtToDate.Text).FormattedText)

            Session("mPaymentVersusGoodsReceiptRecords") = mPaymentVersusGoodsReceiptRecords

            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, Report)
            da.Fill(ds, mPaymentVersusGoodsReceiptRecords)
            rpt.SetDataSource(ds)

            Session("CrystalReport") = rpt
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Else
            upnlValidations.Update()
        End If
    End Sub
#End Region

End Class