Public Class wfrptPendingPaymentListForReceipt_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPendingInvoiceList As PendingPaymentInvoiceList
    Public mVendorList As VendorList
    Public mVendor As Vendor
    Dim msupplier As String
    Dim mCAmount As Decimal  'CGrandToatal Amount
    Dim mPendingAmount As Decimal
    Dim mInvoiceNo As Integer
    Dim mInvoiceText As String
    Dim mInvoiceID As Guid
    Dim mVendorID As Guid
    Dim mInvoiceDate As String
    Dim mMaxPendingAmount As Decimal
    Dim mPendingPaymentListForReceiptSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        mPendingInvoiceList = CType(Session("mPendingInvoiceList"), PendingPaymentInvoiceList)
        mInvoiceID = Session("mInvoiceID")
        mVendorID = Session("mVendorID")
        mInvoiceNo = Session("mInvoiceNo")
        mInvoiceText = Session("mInvoiceText")
        mCAmount = Session("mCAmount")
        mInvoiceDate = Session("mInvoiceDate")
        mPendingAmount = Session("mPendingAmount")
    End Sub
    Private Sub RemoveSession()
        mVendorList = Nothing
        mPendingInvoiceList = Nothing
        mInvoiceID = Nothing
        mInvoiceNo = Nothing
        mVendorID = Nothing
        mInvoiceNo = Nothing
        mInvoiceText = Nothing
        mCAmount = Nothing
        mInvoiceDate = Nothing
        mPendingAmount = Nothing
        Session.Remove("mVendorlist")
        Session.Remove("mPendingInvoiceList")
        Session.Remove("mInvoiceID")
        Session.Remove("mVendorID")
        Session.Remove("mInvoiceNo")
        Session.Remove("mInvoiceText")
        Session.Remove("mCAmount")
        Session.Remove("mInvoiceDate")
        Session.Remove("mPendingAmount")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetValues()
        If txtSupplier.Text.Trim = "" Then
            msupplier = ""
            lblSuppliebName.Text = "Supplier Name  : "
        Else
            msupplier = txtSupplier.Text.Trim
            lblSuppliebName.Text = "Supplier Name  :  " & txtSupplier.Text.Trim
        End If
        lblInvoiceDate.Text = ""
        lblInvoiceText.Text = ""
        lblInvoiceNo.Text = ""
        lblGrandAmount.Text = ""
        lblPendingAmount.Text = ""
    End Sub
   Private Sub FindNow(ByVal mVendorID As Guid)
        'Get List From the Database as per Criteria             
        mPendingInvoiceList = PendingPaymentInvoiceList.GetPendingPaymentInvoiceList(mVendorID)
        Session("mPendingInvoiceList") = mPendingInvoiceList
        'Set DataSource of the Grid
        dgPartSearch.DataSource = mPendingInvoiceList
        dgPartSearch.DataBind()
        lblResult.Text = "Pending Payments Invoice List : " & mPendingInvoiceList.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForInvoicePayment
        Dim ds As New dsInvoicePayment
        Dim obj As InvoicePaymentList
        SetValues()
        myReport = New crptPaymentagainstInvoice
        obj = InvoicePaymentList.GetInvoicePaymentList(mInvoiceID)
        objsearch = rptSearchingCriteriaForInvoicePayment.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), mInvoiceText, mInvoiceNo, mCAmount, mInvoiceDate, mPendingAmount, mCAmount - mPendingAmount, AppSettings("Logo"))
        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 715)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        mPendingPaymentListForReceiptSearchingCriteria = lblSuppliebName.Text + ", " + lblInvoiceDate.Text + ", " + lblInvoiceText.Text + ", " + lblInvoiceNo.Text + ", " + lblGrandAmount.Text + ", " + lblPendingAmount.Text
        MarkLog(Util.Action.Print, "PaymentDetail", mPendingPaymentListForReceiptSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", True, False, True)
       Session("mVendorList") = mVendorList
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidate As CustomValidator
        CustValidate = CType(s, CustomValidator)
        If CustValidate.ControlToValidate = "txtSupplier" Then
            If txtSupplier.Text.Trim = "" Then
                CustValidate.ErrorMessage = "Please Select the Supplier."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack Then
            RemoveSession()
            DataFieldBind()
            FindNow(mVendorID)
        End If
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        mVendorList = Nothing
        mVendorID = VendorList.GetVendortList(0).Item(txtSupplier.Text.Trim).ID
        Session("mVendorID") = mVendorID
        FindNow(mVendorID)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgPartSearch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartSearch.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartSearch.PageIndex * dgPartSearch.PageSize
                mInvoiceID = mPendingInvoiceList(index).InvoiceID
                mInvoiceNo = mPendingInvoiceList(index).InvoiceNo
                mInvoiceText = mPendingInvoiceList(index).InvoiceText
                mCAmount = mPendingInvoiceList(index).CGrandtotal
                mInvoiceDate = mPendingInvoiceList(index).InvoiceDate
                mPendingAmount = mPendingInvoiceList(index).PendingAmount     'that is Pending Amount
                mMaxPendingAmount = mPendingInvoiceList(index).PendingAmount  'that is for Check pending amount is not greater than the given Amount
                Session("mInvoiceID") = mInvoiceID
                Session("mInvoiceNo") = mInvoiceNo
                Session("mInvoiceText") = mInvoiceText
                Session("mCAmount") = mCAmount
                Session("mInvoiceDate") = mInvoiceDate
                Session("mPendingAmount") = mPendingAmount
                lblInvoiceDate.Text = "Invoice Date :" & New SmartDate(mInvoiceDate.ToString).FormattedText
                lblInvoiceText.Text = "Invoice Text :" & mInvoiceText
                lblInvoiceNo.Text = "Invoice No. :" & mInvoiceNo
                lblGrandAmount.Text = "Grand Amount :" & Format(mCAmount, "#.00")
                lblPendingAmount.Text = "Pending Amount :" & Format(mPendingAmount, "#.00")
                If txtSupplier.Text.Trim = "" Then
                    lblSuppliebName.Text = "Supplier Name  : "
                Else
                    lblSuppliebName.Text = "Supplier Name  :  " & txtSupplier.Text.Trim
                End If
                upnlSelection.Update()
                setFocus(dgPartSearch)
        End Select
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        dgPartSearch.DataSource = mPendingInvoiceList
        Session("mPendingInvoiceList") = mPendingInvoiceList
        dgPartSearch.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class