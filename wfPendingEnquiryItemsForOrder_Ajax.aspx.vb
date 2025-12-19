'Added By Vikrant On 04-Jan-2017 For ALL04012017
Imports System.Linq
Public Class wfPendingEnquiryItemsForOrder_Ajax
	Inherits System.Web.UI.Page

#Region " Variables and Declarations "
	Public mPendingEnquiryItemsForOrder As PendingEnquiryItemsForOrder
	Public TransDate, EnquiryText, PartName, VendorName, No As String
	Public mOrder As Order
	Public mQuotationItemsAsPerEnqItem As QuotationItems
	Public mInvoiceItemListForFinalApproval As InvoiceItemListForFinanceApproval
	'Public mQuotation As Quotation
	'Public mDistinctTextList As DistinctTextListForEnquiry
	'Public mPendingEnquiryList As PendingEnquiryList
	'Public mEnquiry As Enquiry

#End Region

#Region "Properties"
	Shared mTransDate As String
	Public Shared ReadOnly Property EnqDate As String
		Get
			Return mTransDate
		End Get
	End Property
#End Region

#Region "Business Methods"
	'Private Sub SetControl()
	'    'setPeroid(DateIndex)
	'    ToDate = IIf(txtTransactionDate.Text.ToString <> "", txtTransactionDate.Text.ToString, "01/01/2050")
	'    Session("ToDate") = ToDate
	'    CallFindNow(SearchIndex)
	'    dgEnquiryList.DataBind()
	'    cmbSearch.SelectedIndex = SearchIndex
	'    'cmbDate.SelectedIndex = DateIndex
	'    cmbEnquiryText.SelectedValue = IIf(EnquiryText = "", "(All)", EnquiryText)
	'    txtName.Text = Name
	'    txtNo.Text = No
	'    ControlVisibility(SearchIndex, DateIndex)
	'    lblResult.Text = "List of Enquiry as per criteria :" & mPendingEnquiryList.Count & " Record(s) found."
	'End Sub
	Private Sub SetProperties()
		mTransDate = txtTransactionDate.Text
	End Sub

	Private Sub DataFieldBind()
		'FromDate = IIf(IsNothing(FromDate), "01/01/1900", FromDate)
		'ToDate = IIf(IsNothing(ToDate), "01/01/2050", ToDate)
		'SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		'DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
		'StatusId = 2 'Session("StatusId")
		'EnquiryText = Session("EnquiryText")
		'VendorName = IIf(Session("VendorName") Is Nothing, "", Session("VendorName"))

		'No = Session("No")


		txtTransactionDate.Text = mOrder.OrderDateFormatted.ToString
		txtVendorName.Text = mOrder.VendorName
		mPendingEnquiryItemsForOrder = PendingEnquiryItemsForOrder.GetPendingEnquiryItemsForOrder(Trim(txtPartName.Text), Trim(txtText.Text), CInt(Val(IIf(txtNo.Text = "", 0, txtNo.Text))), txtTransactionDate.Text, txtVendorName.Text)
		dgEnquiryItemList.DataSource = mPendingEnquiryItemsForOrder
		Session("mPendingEnquiryItemsForOrder") = mPendingEnquiryItemsForOrder

		dgQuoteItems.DataSource = Nothing
		dgInvoiceItemList.DataSource = Nothing
		DataBind()
		lblEnqItemResult.Text = "As per criteria :" & mPendingEnquiryItemsForOrder.Count & " Record(s) found."
	End Sub
	Private Sub GetSession()
		mPendingEnquiryItemsForOrder = Session("mPendingEnquiryItemsForOrder")
		mOrder = Session("mOrder")
		mQuotationItemsAsPerEnqItem = Session("mQuotationItemsAsPerEnqItem")
		'mQuotation = Session("mQuotation")
		'mEnquiry = Session("mEnquiry")
		'mPendingEnquiryList = Session("mPendingEnquiryList")
		'mDistinctTextList = Session("mDistinctTextList")
		'SearchIndex = Session("SearchIndex")
		'DateIndex = Session("DateIndex")
		'FromDate = Session("FromDate")
		'ToDate = Session("ToDate")
		'StatusId = 2 'Session("StatusId")
		'EnquiryText = Session("EnquiryText")
		'Name = IIf(Session("Name") Is Nothing, "", Session("Name"))
		'No = IIf(IsNothing(Session("No")), 0, Session("No"))
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mPendingEnquiryItemsForOrder")
		Session.Remove("mQuotationItemsAsPerEnqItem")

		'Session.Remove("mEnquiry")
		'Session.Remove("mPendingEnquiryList")
		'Session.Remove("mDistinctTextList")
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
		'txtName.Text = ""
	End Sub
	Private Sub ControlVisibility()
		txtTransactionDate.Enabled = IIf(mOrder.OrderItems.Count > 1, False, True)
		txtVendorName.Enabled = IIf(mOrder.OrderItems.Count > 1, False, True)
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes

				Case MsgBoxResult.No

				Case MsgBoxResult.Ok
			End Select
		End If
	End Sub
	Private Sub CallFindNow()
		mPendingEnquiryItemsForOrder = Nothing
		dgEnquiryItemList.DataSource = Nothing
		'Get List From the Database as per Criteria    
		mPendingEnquiryItemsForOrder = PendingEnquiryItemsForOrder.GetPendingEnquiryItemsForOrder(txtPartName.Text, txtText.Text, CInt(Val(IIf(txtNo.Text = "", 0, txtNo.Text))), txtTransactionDate.Text, txtVendorName.Text)
		'Set DataSource of the Grid
		Session("mPendingEnquiryItemsForOrder") = mPendingEnquiryItemsForOrder
		dgEnquiryItemList.DataSource = mPendingEnquiryItemsForOrder
		lblEnqItemResult.Text = "As per criteria :" & mPendingEnquiryItemsForOrder.Count & " Record(s) found."
	End Sub
	Private Sub setObject(ByVal QuoteItem As QuotationItem)
		mOrder.OrderDate = txtTransactionDate.Text
		mOrder.VendorID = QuoteItem.VendorID
		mOrder.CurrencyID = QuoteItem.CurrencyID
		mOrder.ConversionFactor = QuoteItem.ConversionFactor
		mOrder.OrderItems.CurrentItem.ItemID = QuoteItem.ItemID
		mOrder.OrderItems.CurrentItem.Qty = QuoteItem.Qty
		mOrder.OrderItems.CurrentItem.CRate = QuoteItem.CRate
		mOrder.OrderItems.CurrentItem.DeliveryInDays = QuoteItem.DeliveryInDays
		mOrder.OrderItems.CurrentItem.PriorityID = QuoteItem.PriorityID

		mOrder.OrderItems.CurrentItem.UnitID = QuoteItem.UnitID     'Added By Prashant 5-Feb-2019 ALL04022019
		mOrder.OrderItems.CurrentItem.UnitName = QuoteItem.UnitName 'Added By Prashant 5-Feb-2019 ALL04022019
		mOrder.OrderItems.CurrentItem.RequisitionTextNo = QuoteItem.RequisitionTextNo
		mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.Add(mOrder.OrderItems.CurrentItem.ID, QuoteItem.ID, QuoteItem.Qty, QuoteItem.QuotationNo, QuoteItem.QuotationDateFormatted.ToString, QuoteItem.QuotationID)
		Dim mVendor As Vendor
		Dim mGSTPercentage As GSTPercentage
		Dim mtmpItem As Item = Item.GetItem(mOrder.OrderItems.CurrentItem.ItemID)
		With mOrder.OrderItems.CurrentItem
			If AppSettings("IsGSTApplicable") = "True" And Not mOrder.VendorID.Equals(Guid.Empty) Then
				mVendor = Vendor.GetVendor(mOrder.VendorID)
				If mVendor.CountryName.ToUpper = "INDIA" And CDate(mOrder.OrderDateFormatted.ToString) >= CDate("01-Jul-2017") And mVendor.ClientCountryName.ToUpper.Equals("INDIA") Then
					mGSTPercentage = GSTPercentage.GetPercentage(mOrder.OrderDateFormatted.ToString, 1, .ItemID.ToString)
					If mGSTPercentage IsNot Nothing Then

						If Len(mVendor.StateCode) > 0 Then
							If mVendor.StateCode = mVendor.ClientStateCode Then
								.CGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
								.SGSTPercentage = (mGSTPercentage.GSTPercentage / 2)
								.CGSTCAmount = ((.CGSTPercentage * .CAmount) / 100)
								.SGSTCAmount = ((.SGSTPercentage * .CAmount) / 100)
								.IGSTPercentage = 0
								.IGSTCAmount = 0
								.TotalCAmount = .CAmount + .CGSTCAmount + .SGSTCAmount
								mOrder.StateCode = mVendor.StateCode
								mOrder.ClientStateCode = mVendor.ClientStateCode
								mOrder.VendorCountry = mVendor.CountryName
								mOrder.Visibility = 1
							Else
								.IGSTPercentage = (mGSTPercentage.GSTPercentage)
								.IGSTCAmount = ((.IGSTPercentage * .CAmount) / 100)
								.CGSTPercentage = 0
								.SGSTPercentage = 0
								.CGSTCAmount = 0
								.SGSTCAmount = 0
								.TotalCAmount = .CAmount + .IGSTCAmount
								mOrder.StateCode = mVendor.StateCode
								mOrder.ClientStateCode = mVendor.ClientStateCode
								mOrder.VendorCountry = mVendor.CountryName
								mOrder.Visibility = 2
							End If
							.HSNACSCode = mtmpItem.HSNACSCode
						Else
							.CGSTPercentage = 0
							.SGSTPercentage = 0
							.CGSTCAmount = 0
							.SGSTCAmount = 0
							.IGSTPercentage = 0
							.IGSTCAmount = 0
							.HSNACSCode = ""
							mOrder.StateCode = mVendor.StateCode
							mOrder.ClientStateCode = mVendor.ClientStateCode
							mOrder.VendorCountry = mVendor.CountryName
							mOrder.Visibility = 3
						End If
					End If
				Else
					.CGSTPercentage = 0
					.SGSTPercentage = 0
					.CGSTCAmount = 0
					.SGSTCAmount = 0
					.IGSTPercentage = 0
					.IGSTCAmount = 0
					.HSNACSCode = ""
					mOrder.StateCode = mVendor.StateCode
					mOrder.ClientStateCode = mVendor.ClientStateCode
					mOrder.VendorCountry = mVendor.CountryName
					mOrder.Visibility = 3
				End If
			Else
				.CGSTPercentage = 0
				.SGSTPercentage = 0
				.CGSTCAmount = 0
				.SGSTCAmount = 0
				.IGSTPercentage = 0
				.IGSTCAmount = 0
				.HSNACSCode = ""
				mOrder.Visibility = 3
			End If
			.HSNACSCode = mtmpItem.HSNACSCode  'Added By Prashant on 28-Sep-2021 For STR27092021
		End With

		Session("mOrder") = mOrder
	End Sub
	Private Sub RefreshAllGrids()
		CallFindNow()
		dgEnquiryItemList.DataBind()
		lblEnqItemResult.Text = "As per criteria :" & mPendingEnquiryItemsForOrder.Count & " Record(s) found."
		lblResultQuoteItem.Visible = False
		lblResultInvItem.Visible = False
		dgQuoteItems.DataSource = Nothing
		dgInvoiceItemList.DataSource = Nothing
		dgQuoteItems.DataBind()
		dgInvoiceItemList.DataBind()
		upnlEnquiryItemList.Update()
		upnlInvoiceItemList.Update()
		upnlQuoteItems.Update()
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		addAttributes()
		GetSession()
		If Not IsPostBack Then
			DataFieldBind()
			'SetControl()
			SetProperties()
			ControlVisibility()
		End If
	End Sub
	Protected Sub txtTransactionDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
		CallFindNow()
		dgEnquiryItemList.DataBind()
		upnlEnquiryItemList.Update()
		lblEnqItemResult.Text = "As per criteria :" & mPendingEnquiryItemsForOrder.Count & " Record(s) found."
	End Sub
	Private Sub dgEnquiryItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEnquiryItemList.RowCommand
		Select Case e.CommandName
			Case "SelectRecord"
				Dim index As Integer = CInt(e.CommandArgument) + dgEnquiryItemList.PageIndex * dgEnquiryItemList.PageSize
				Dim mEnquiryItemID, mItemID As New Guid '(e.Item.Cells(0).Text)
				mEnquiryItemID = mPendingEnquiryItemsForOrder(index).EnquiryItemID
				mItemID = mPendingEnquiryItemsForOrder(index).ItemID

				mQuotationItemsAsPerEnqItem = QuotationItems.GetQuotationItemsForOrderAgainstEnqItems(mEnquiryItemID, mOrder.VendorID.ToString)
				Session("mQuotationItemsAsPerEnqItem") = mQuotationItemsAsPerEnqItem
				dgQuoteItems.DataSource = mQuotationItemsAsPerEnqItem
				lblResultQuoteItem.Visible = True
				dgQuoteItems.DataBind()
				upnlQuoteItems.Update()

				mInvoiceItemListForFinalApproval = InvoiceItemListForFinanceApproval.GetInvoiceItemListForFinalApprovalList(mItemID)
				dgInvoiceItemList.DataSource = mInvoiceItemListForFinalApproval
				Session("mInvoiceItemListForFinalApproval") = mInvoiceItemListForFinalApproval
				lblResultInvItem.Visible = True
				dgInvoiceItemList.DataBind()
				upnlInvoiceItemList.Update()
		End Select
	End Sub
	Private Sub dgEnquiryItemList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEnquiryItemList.PageIndexChanging
		dgEnquiryItemList.PageIndex = e.NewPageIndex
		dgEnquiryItemList.DataSource = mPendingEnquiryItemsForOrder
		Session("mPendingEnquiryItemsForOrder") = mPendingEnquiryItemsForOrder
		dgEnquiryItemList.DataBind()
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		If mOrder.OrderItems.CurrentItem.IsNew And Not Session("Edit") = True Then mOrder.OrderItems.Remove(mOrder.OrderItems.CurrentItem)
		RemoveSession()
		Session.Remove("Edit")
		Response.Redirect(Request.QueryString("BackPage"))
	End Sub
	Private Sub dgEnquiryItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEnquiryItemList.Sorting
		mPendingEnquiryItemsForOrder.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mPendingEnquiryItemsForOrder") = mPendingEnquiryItemsForOrder
		dgEnquiryItemList.DataSource = mPendingEnquiryItemsForOrder
		dgEnquiryItemList.DataBind()
	End Sub
	Protected Sub txtText_TextChanged(sender As Object, e As System.EventArgs)
		RefreshAllGrids()
	End Sub
	Private Sub txtPartName_TextChanged(sender As Object, e As System.EventArgs) Handles txtPartName.TextChanged
		RefreshAllGrids()
	End Sub
	Private Sub txtVendorName_TextChanged(sender As Object, e As System.EventArgs) Handles txtVendorName.TextChanged
		RefreshAllGrids()
	End Sub
	Private Sub txtNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtNo.TextChanged
		RefreshAllGrids()
	End Sub
	Private Sub dgQuoteItems_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgQuoteItems.RowCommand
		Select Case e.CommandName
			Case "SelectRecord"
				Dim index As Integer = CInt(e.CommandArgument) + dgQuoteItems.PageIndex * dgQuoteItems.PageSize
				Dim QuoteItem As QuotationItem
				QuoteItem = mQuotationItemsAsPerEnqItem(index)

				If Not mOrder.VendorID.Equals(QuoteItem.VendorID) And Not mOrder.VendorID.Equals(Guid.Empty) Then
					MSGBoxCtrl.show("Alert", "Selected Quoation vendor is diffrent from current Order Vendor", "Please select same vendor", MsgBoxStyle.OkOnly, "")
					mOrder.CancelEdit()
					Exit Sub
				End If

				If mOrder.OrderItems.Contains(ItemID:=QuoteItem.ItemID) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Order Item", MsgBoxStyle.OkOnly, "")
					mOrder.CancelEdit()
					Exit Sub
				End If

				setObject(QuoteItem)
				RemoveSession()
				'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & mItemId.ToString)
				Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")
			Case "ViewRec"
				Dim mQuotation As Quotation
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'Dim index As Integer = CInt(e.CommandArgument) + dgQuotationList.PageIndex * dgQuotationList.PageSize
				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				mQuotation = Quotation.GetQuotation(mID)
				If mQuotation.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mQuotation.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mQuotation.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mQuotation.ImageFile, 0, mQuotation.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
					End If
				End If
		End Select
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
#End Region

#Region " Service Methods "
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetTextList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim DistinctTextList As DistinctTextListAutoComplete

		DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, , True, Util.Trans.RequestingForQuotation, mTransDate)
		If count = 0 Then
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
		Else
			Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
		End If
	End Function
#End Region



End Class