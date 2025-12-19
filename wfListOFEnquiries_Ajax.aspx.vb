Public Class wfListOFEnquiries_Ajax
	Inherits Page

#Region " Enumaration "
	Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum
#End Region

#Region " Variable Declaration "
    Public mEnquiryList As EnquiryList
    Public mEnquiry As Enquiry
    Public mDistinctTextListForEnquiry As DistinctTextListForEnquiry
    Dim objSearch As rptSearchingCriteriaForEnquiry
    Dim objReg As rptEnquiryRegister
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, EnquiryText, Name, No, VendorNo, RequisitionText As String
    Public mModuleName As String
    Public mTransTypeID As Trans

    Dim EventLogID As Guid
    Dim mEnquiryDetail As String
    Dim mTransactionListCount As TransactionListCount

    Public mRequisitionListNew As RequisitionListNew
    Public mRequisitionNew As RequisitionNew

    Public mDistinctTextListForRequisition As DistinctTextListForRequisition
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEnquiry = Session("mEnquiry")
        mEnquiryList = Session("mEnquiryList")
        mDistinctTextListForEnquiry = Session("mDistinctTextListForEnquiry")
        SearchIndex = Session("SearchIndexEnq")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        EnquiryText = Session("EnquiryText")
        RequisitionText = Session("RequisitionText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        VendorNo = Session("VendorNo")
        mTransactionListCount = Session("mTransactionListCount")
        mTransTypeID = Session("mTransTypeId")
        mModuleName = Session("mModuleName")

        mRequisitionListNew = Session("mRequisitionListNewwfListOFEnquiries")
        mRequisitionNew = Session("mRequisitionNewwfListOFEnquiries")
    End Sub
    Private Sub SetSession()
        Session("mEnquiry") = mEnquiry
        Session("mEnquiryList") = mEnquiryList
        Session("mDistinctTextListForEnquiry") = mDistinctTextListForEnquiry
        Session("mTransTypeId") = mTransTypeID
        Session("SearchIndexEnq") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("StatusId") = StatusId
        Session("EnquiryText") = EnquiryText
        Session("RequisitionText") = RequisitionText
        Session("Name") = Name
        Session("No") = No
        Session("VendorNo") = VendorNo
        Session("mTransactionListCount") = mTransactionListCount

        Session("mRequisitionListNewwfListOFEnquiries") = mRequisitionListNew
        Session("mRequisitionNewwfListOFEnquiries") = mRequisitionNew
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEnquiry")
        Session.Remove("mEnquiryList")
        Session.Remove("mDistinctTextListForEnquiry")
        Session.Remove("mTransTypeId")
    End Sub
    Private Sub ClearAll()
        mTransTypeID = Session("mTransTypeId")
        If Session("MiddleFrame") <> "wfListOFEnquiries_Ajax.aspx?TransTypeId=" & mTransTypeID Then
            Session.Remove("mEnquiry")
            Session.Remove("mEnquiryList")
            Session.Remove("mDistinctTextListForEnquiry")
            Session.Remove("SearchIndexEnq")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("EnquiryText")
            Session.Remove("RequisitionText")
            Session.Remove("Name")
            Session.Remove("No")
            Session.Remove("VendorNo")
            Session.Remove("mTransactionListCount")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgEnqList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex

        If cmbEnquiryText.Items.Contains(New System.Web.UI.WebControls.ListItem(EnquiryText)) Then
            cmbEnquiryText.SelectedValue = EnquiryText
        Else
            cmbEnquiryText.SelectedValue = "(All)"
        End If
        If cmbRequisitionText.Items.Contains(New System.Web.UI.WebControls.ListItem(RequisitionText)) Then
            cmbRequisitionText.SelectedValue = RequisitionText
        Else
            cmbRequisitionText.SelectedValue = "(All)"
        End If
        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
    End Sub
    Private Sub NewRecord()
        mEnquiry = Enquiry.NewEnquiry(mTransTypeID)
        mEnquiry.Date = Today.Date
        Session("mEnquiry") = mEnquiry
        Session("mTransTypeID") = mTransTypeID
    End Sub
	Private Sub EditRecord(mId As Guid)
		mEnquiry = Enquiry.GetEnquiry(mId)
		mEnquiry.MarkClean()
		Session("mEnquiry") = mEnquiry
	End Sub
	Private Sub EditReqRecord(mId As Guid)
		mRequisitionNew = RequisitionNew.GetRequisition(mId)
		mRequisitionNew.MarkClean()
		Session("mRequisitionNewwfListOFEnquiries") = mRequisitionNew
	End Sub
	Private Sub DeleteRecord(mId As Guid)
		MSGBoxCtrl.Show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
		mEnquiry = Enquiry.GetEnquiry(mId)
		Session("mEnquiry") = mEnquiry
		Session("mTransTypeId") = mTransTypeID
	End Sub
	Private Overloads Sub setFocus(cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub

	Private Sub FindNow(Optional ItemName As String = "",
						Optional Text As String = "",
						Optional No As Integer = 0,
						Optional Amend As String = "",
						Optional IntEnquiryNo As String = "",
						Optional FromDate As String = "1/1/1900",
						Optional ToDate As String = "1/1/2200",
						Optional StatusID As Integer = 0,
						Optional VendorName As String = "",
						Optional VendorNo As String = "",
						Optional ReqText As String = "",
						Optional ReqNo As Integer = 0)

		Try

			mEnquiryList = Nothing
			dgEnqList.DataSource = Nothing

			'Get List From the Database as per Criteria             
			mEnquiryList = EnquiryList.GetEnquiryList(ItemName:=ItemName,
													  Text:=Text,
													  No:=No,
													  FromDate:=FromDate,
													  ToDate:=ToDate,
													  StatusID:=StatusID,
													  VendorName:=VendorName,
													  TransTypeID:=mTransTypeID,
													  VendorNo:=VendorNo,
													  IsFromQuotationComparison:=1,
													  DoneOrder:=chkDoneOrder.Checked)
			'Set DataSource of the Grid
			Session("mEnquiryList") = mEnquiryList
			dgEnqList.DataSource = mEnquiryList
			lblResult.Text = "List of Enquires for Quotation Comparison as per criteria : " & mEnquiryList.Count & " Record(s) found."

			mRequisitionListNew = RequisitionListNew.GetRequisitionList(ItemName:=ItemName,
																		Text:=ReqText,
																		No:=ReqNo,
																		FromDate:=FromDate,
																		ToDate:=ToDate,
																		StatusID:=StatusID,
																		Location:="",
																		Employee:="",
																		LocationID:="{00000000-0000-0000-0000-000000000000}",
																		Aircraft:="",
																		ReqTypeID:=0,
																		TransTypeID:=0,
																		IsFromQuotationComparison:=1,
																		DoneOrder:=chkDoneOrder.Checked)

			lblReqResult.Text = "List of Requisitions for Quotation Comparison as per criteria : " & mRequisitionListNew.Count & " Record(s) found."

			dgRequisitionList.DataSource = mRequisitionListNew
			dgRequisitionList.DataBind()
			Session("mRequisitionListNewwfListOFEnquiries") = mRequisitionListNew

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CallFindNow(Index As Integer)
		Select Case Index
			Case -1
				Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "")  'for all records
			Case 0  'all
				Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "") 'for all records
			Case 1 'Enquiry date
				Call FindNow("", "", 0, "", "", txtFromDate.Text, txtToDate.Text, 0, "")
			Case 2  'Enquiry Text , No And Amend
				Call FindNow("", EnquiryText, CInt(Val(No)), "", "", FromDate, ToDate, 0, "", VendorNo)
			Case 3  'ItemName
				Call FindNow(Name, "", 0, "", "", FromDate, ToDate, 0, "")
			Case 4 ' Vendor Name
				Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, Name)
			Case 5 ' Requisition
				Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "", RequisitionText, CInt(Val(No)))
		End Select
		dgEnqList.PageIndex = 0
	End Sub
	Private Sub setPeriod(Index As Int32)
		Select Case Index
			Case 0 ' All   
				txtFromDate.Text = CDate("1-1-1900").ToString(AppSettings("DateFormat"))
				txtToDate.Text = CDate("1-1-2200").ToString(AppSettings("DateFormat"))
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
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
				End If
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			Case 6 'Between Dates
				FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date)
				ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date)
				txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
				txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
		End Select
	End Sub
	Private Sub ControlVisibility(SearchIndex As Int32, Optional DateIndex As Int32 = 0)
		cmbDate.Visible = IIf(SearchIndex = 1, True, False)
		If SearchIndex = 1 And DateIndex = 6 Then
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
		ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
			txtFromDate.Enabled = False
			txtToDate.Enabled = False
		End If
		cmbEnquiryText.Visible = IIf(SearchIndex = 2, True, False)
		cmbRequisitionText.Visible = IIf(SearchIndex = 5, True, False)
		lblNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbEnquiryText.SelectedIndex <> 0 Or cmbRequisitionText.SelectedIndex <> 0), True, False)
		txtNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbEnquiryText.SelectedIndex <> 0 Or cmbRequisitionText.SelectedIndex <> 0), True, False)
		txtName.Visible = IIf(SearchIndex = 3 Or SearchIndex = 4, True, False)
		If rAgainstEnquiry.Checked = True Then
			dgEnqList.Visible = True
			lblResult.Visible = True
			dgRequisitionList.Visible = False
			lblReqResult.Visible = False
		ElseIf rbAgainstRequisition.Checked = True Then
			dgEnqList.Visible = False
			lblResult.Visible = False
			dgRequisitionList.Visible = True
			lblReqResult.Visible = True
		End If
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
		txtName.Text = ""
	End Sub
	Private Sub setVariables()
		SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
		DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
		ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
		EnquiryText = IIf(cmbEnquiryText.SelectedIndex <= 0, "", cmbEnquiryText.SelectedValue)
		RequisitionText = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue)
		Name = txtName.Text.Trim
		No = txtNo.Text.Trim
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndexEnq") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("StatusId") = StatusId
		Session("EnquiryText") = EnquiryText
		Session("RequisitionText") = RequisitionText
		Session("No") = No
		Session("Name") = Name
		Session("VendorNo") = VendorNo
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
	Private Sub SetTitle()
		Dim mTransTypeList As TransactionList
		mTransTypeList = TransactionList.GetTransactionList()
		btnAddNew.ToolTip = "Click to Add New " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
		btnAddNewTop.ToolTip = "Click to Add New " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
		btnClose.ToolTip = "Click to Close list of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " screen"
		btnCloseTop.ToolTip =
			"Click to Close list of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " screen"
		mModuleName = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
		Session("mModuleName") = mModuleName
		lblEnquiryList.Text = "List of Enquires/Requisitions for Quotation Comparison " '"    [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"  'shweta
	End Sub
	Private Function IsInRole(CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		'Deciding IsInRole String to check Rights
		Select Case mTransTypeID
			Case Util.Trans.Enquiry
				IsInRoleString = "Enquiry"
			Case Util.Trans.RequestingForQuotation
				IsInRoleString = "RequestingForQuotation"
			Case Util.Trans.OverHaulRepairEnquiry
				IsInRoleString = "PurchaseEnquiryRepairOverHaul"
			Case Util.Trans.RentialLeaseEnquiry
				IsInRoleString = "PurchaseEnquiryRentalLease"
		End Select
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
		End Select
	End Function
	Private Sub FillCombo()
		'If mTransTypeID = 1 Then
		'    cmbSearch.Items.Add("All")
		'    cmbSearch.Items.Add("Date")
		'    cmbSearch.Items.Add("Enquiry")
		'    cmbSearch.Items.Add("Part No.")
		'    cmbSearch.Items.Add("Customer")
		'    dgEnqList.Columns(3).HeaderText = "Customer"
		'ElseIf (mTransTypeID = 32) Or (mTransTypeID = 34) Or (mTransTypeID = 35) Then
		cmbSearch.Items.Add("All")
		cmbSearch.Items.Add("Date")
		cmbSearch.Items.Add("Enquiry")
		cmbSearch.Items.Add("Part No.")
		cmbSearch.Items.Add("Supplier")
		cmbSearch.Items.Add("Requisition")
		dgEnqList.Columns(3).HeaderText = "Supplier"
		'End If
	End Sub
	Private Sub Visiblity()
		If chkDoneOrder.Checked = True Then
			dgEnqList.Columns(9).Visible = True 'View
		ElseIf chkDoneOrder.Checked = False Then
			dgEnqList.Columns(9).Visible = False 'View
		End If
		upnlGridView.Update()
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
		StatusId = Session("StatusId")
		EnquiryText = Session("EnquiryText")
		Name = Session("Name")
		No = Session("No")
		VendorNo = Session("VendorNo")
		mDistinctTextListForEnquiry = DistinctTextListForEnquiry.GetDistinctTextList("7", , True, "(All)")
		cmbEnquiryText.DataSource = mDistinctTextListForEnquiry

		mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)")
		cmbRequisitionText.DataSource = mDistinctTextListForRequisition

		mTransactionListCount = TransactionListCount.GetTransactionListCountt(mTransTypeID)
		Session("mTransactionListCount") = mTransactionListCount
		DataBind()
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
		ClearAll()
		GetSession()
		addAttributes()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack And Session("sender") = "" Then
			If cmbSearch.Enabled = True Then
				setFocus(cmbSearch)
			End If
			mTransTypeID = Request.QueryString("TransTypeId")
			Session("mTransTypeId") = mTransTypeID
			Session("MiddleFrame") = "wfListOFEnquiries_Ajax.aspx?TransTypeId=" & mTransTypeID
			FillCombo()
			DataFieldBind()
			SetControl()
			SetTitle()
			SetSession()
			Visiblity()
		End If
	End Sub
	Private Sub dgEnqList_RowCommand(source As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEnqList.RowCommand
		Select Case e.CommandName
			Case "CreateOrder", "View"
				dgEnqList.DataSource = mEnquiryList
				dgEnqList.DataBind()
				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				If (Not IsInRole(Rights.Edit) And Not IsInRole(Rights.View)) Then
					MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				EditRecord(mID)
				mEnquiryDetail = mEnquiry.EnquiryNo + " Dated : " + mEnquiry.DateFormatted + " from " + mEnquiryList(mEnquiry.ID).VendorName
				MarkLog(Util.Action.Edit, mModuleName, mEnquiryDetail, Util.ErrorType.NoError, mEnquiry.ID, EventLogID)
				'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWindow", "OpenPartsWindow('" + mEnquiry.ID.ToString + "','" + Guid.Empty.ToString + "','" + "True" + "','" + "False" + "');", True)'
				If e.CommandName = "CreateOrder" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWindow", "OpenPartsWindow('" + mEnquiry.ID.ToString + "','" + Guid.Empty.ToString + "','" + "True" + "','" + "False" + "','" + "False" + "');", True)
				ElseIf e.CommandName = "View" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWindow", "OpenPartsWindow('" + mEnquiry.ID.ToString + "','" + Guid.Empty.ToString + "','" + "True" + "','" + "False" + "','" + chkDoneOrder.Checked.ToString + "');", True)
				End If
		End Select
	End Sub
	Private Sub dgEnquiryList_PageIndexChanging(source As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEnqList.PageIndexChanging
		dgEnqList.PageIndex = e.NewPageIndex
		dgEnqList.DataSource = mEnquiryList
		Session("mEnquiryList") = mEnquiryList
		dgEnqList.DataBind()
	End Sub
	Private Sub cmbSearch_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
		cmbDate.SelectedIndex = 0
		cmbEnquiryText.SelectedIndex = 0
		cmbRequisitionText.SelectedIndex = 0
		ClearControls()
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbSearch.Enabled = True Then
			setFocus(cmbSearch)
		End If
	End Sub
	Private Sub cmbDate_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbDate.Enabled = True Then
			setFocus(cmbDate)
		End If
	End Sub
	Private Sub btnFindNow_Click(sender As System.Object, e As System.EventArgs) Handles btnFindNow.Click, chkDoneOrder.CheckedChanged
		setVariables()
		CallFindNow(SearchIndex)
		dgEnqList.DataBind()
		lblResult.Text = "List of Enquires for Quotation Comparison as per criteria :" & mEnquiryList.Count & " Record(s) found."
		lblReqResult.Text = "List of Requisitions for Quotation Comparison as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
		If chkDoneOrder.Checked = True Then
			dgEnqList.Columns(8).Visible = False 'Create Order
			dgEnqList.Columns(9).Visible = True  'View
			dgRequisitionList.Columns(9).Visible = False
			dgRequisitionList.Columns(10).Visible = True
		ElseIf chkDoneOrder.Checked = False Then
			dgEnqList.Columns(8).Visible = True  'Create Order
			dgEnqList.Columns(9).Visible = False 'View
			dgRequisitionList.Columns(9).Visible = True
			dgRequisitionList.Columns(10).Visible = False
		End If
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
	End Sub
	Private Sub cmbEnquiryText_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbEnquiryText.SelectedIndexChanged, cmbRequisitionText.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		If cmbEnquiryText.Enabled = True Then
			setFocus(cmbEnquiryText)
		End If
	End Sub
	Private Sub btnAddNew_Click(sender As System.Object, e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
		NewRecord()
		If Not IsInRole(Rights.[New]) Then
			MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		MarkLog(Util.Action.[New], mModuleName, "", Util.ErrorType.NoError, mEnquiry.ID, EventLogID)
		Dim str As String
		str = "openledgersame('wfEnquiry_Ajax.aspx?BackPage=wfListOFEnquiries_Ajax.aspx');"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
	End Sub
	Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
		Session("MiddleFrame") = ""
		RemoveSession()
		Session.Remove("mTransactionListCount")
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgEnqList_Sorting(source As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEnqList.Sorting
		mEnquiryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mEnquiryList") = mEnquiryList
		dgEnqList.DataSource = mEnquiryList
		dgEnqList.DataBind()
	End Sub
	Private Sub hdnimgBtnCommonPartList_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnCommonPartList.Click
		setVariables()
		CallFindNow(SearchIndex)
		dgEnqList.DataBind()
		lblResult.Text = "List of Enquires for Quotation Comparison as per criteria :" & mEnquiryList.Count & " Record(s) found."
		lblReqResult.Text = "List of Requisitions for Quotation Comparison as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
	End Sub
	Private Sub rAgainstEnquiry_CheckedChanged(sender As Object, e As System.EventArgs) Handles rAgainstEnquiry.CheckedChanged
		dgEnqList.Visible = True
		lblResult.Visible = True
		dgRequisitionList.Visible = False
		lblReqResult.Visible = False
		If chkDoneOrder.Checked = True Then
			dgEnqList.Columns(8).Visible = False 'Create Order
			dgEnqList.Columns(9).Visible = True  'View
			dgRequisitionList.Columns(9).Visible = False
			dgRequisitionList.Columns(10).Visible = True
		ElseIf chkDoneOrder.Checked = False Then
			dgEnqList.Columns(8).Visible = True  'Create Order
			dgEnqList.Columns(9).Visible = False 'View
			dgRequisitionList.Columns(9).Visible = True
			dgRequisitionList.Columns(10).Visible = False
		End If
		upnlGridView.Update()
	End Sub
	Private Sub rbAgainstRequisition_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbAgainstRequisition.CheckedChanged
		dgEnqList.Visible = False
		lblResult.Visible = False
		dgRequisitionList.Visible = True
		lblReqResult.Visible = True
		If chkDoneOrder.Checked = True Then
			dgEnqList.Columns(8).Visible = False 'Create order
			dgEnqList.Columns(9).Visible = True  'View
			dgRequisitionList.Columns(9).Visible = False
			dgRequisitionList.Columns(10).Visible = True
		ElseIf chkDoneOrder.Checked = False Then
			dgEnqList.Columns(8).Visible = True  'Create order
			dgEnqList.Columns(9).Visible = False 'View
			dgRequisitionList.Columns(9).Visible = True
			dgRequisitionList.Columns(10).Visible = False
		End If
		upnlGridView.Update()
	End Sub
	Private Sub dgRequisitionList_RowCommand(source As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionList.RowCommand
		Select Case e.CommandName
			Case "CreateOrder", "View"
				dgRequisitionList.DataSource = mRequisitionListNew
				dgRequisitionList.DataBind()
				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				If (Not IsInRole(Rights.Edit) And Not IsInRole(Rights.View)) Then
					MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				EditReqRecord(mID)
				'mEnquiryDetail = mEnquiry.EnquiryNo + " Dated : " + mEnquiry.DateFormatted + " from " + mEnquiryList(mEnquiry.ID).VendorName
				'MarkLog(Util.Action.Edit, mModuleName, mEnquiryDetail, Util.ErrorType.NoError, mEnquiry.ID, EventLogID)
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenWindow", "OpenPartsWindow('" + Guid.Empty.ToString + "','" + mRequisitionNew.ID.ToString + "','" + "False" + "','" + "True" + "','" + chkDoneOrder.Checked.ToString + "');", True)
		End Select
	End Sub
	Private Sub dgRequisitionList_PageIndexChanging(source As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRequisitionList.PageIndexChanging
		dgRequisitionList.PageIndex = e.NewPageIndex
		dgRequisitionList.DataSource = mRequisitionListNew
		Session("mRequisitionListNew") = mRequisitionListNew
		dgRequisitionList.DataBind()
	End Sub
	Private Sub dgRequisitionList_Sorting(source As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRequisitionList.Sorting
		mRequisitionListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mRequisitionListNew") = mRequisitionListNew
		dgRequisitionList.DataSource = mRequisitionListNew
		dgRequisitionList.DataBind()
	End Sub
#End Region

End Class