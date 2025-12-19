Public Class wfQuotationList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Public Enum UserRightsFor
        urfNew = 1
        urfEdit = 2
        urfDelete = 3
        urfView = 4
        urfPrint = 5
        urfSave = 6
    End Enum
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
    Public mQuotationList As QuotationList
    Public mQuotation As Quotation
    Public mQuotationTextList As DistinctTextListForQuotation
    Public mDistinctTextListForEnquiry As DistinctTextListForEnquiry
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, QuotationText, EnquiryText, Name, PriorityID, ReportTitle, QuotationNo, EnquiryNo, VendorName As String
    Dim mModuleName As String
    Public mTransTypeID As Trans
    Dim EventLogID As Guid                              'Added by Prashant on 20-July-2011
    Dim mTransactionListCount As TransactionListCount   'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
    Dim mPendingTransactionCount As PendingTransactionCount
    Dim Amend As String 'Added By Vikrant On 22-Nov-2019 For ALL22112019
    Public mShowTopAmendedOrderNo As ShowTopAmendedOrderNo 'Added By Vikrant On 22-Nov-2019 For ALL22112019
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mQuotation = Session("mQuotation")
        mQuotationList = Session("mQuotationList")
        mQuotationTextList = Session("mQuotationTextList")
        mDistinctTextListForEnquiry = Session("mDistinctTextListForEnquiry")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        PriorityID = Session("PriorityID")
        QuotationText = Session("QuotationText")
        EnquiryText = Session("EnquiryText")
        Name = Session("Name")
        QuotationNo = IIf(IsNothing(Session("QuotationNo")), 0, Session("QuotationNo"))
        EnquiryNo = IIf(IsNothing(Session("EnquiryNo")), 0, Session("EnquiryNo"))
        mTransTypeID = Session("mTransTypeID")
        mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
        Amend = Session("Amend") 'Added By Vikrant On 22-Nov-2019 For ALL22112019
        VendorName = Session("VendorName")
    End Sub
    Private Sub SetSession()
        Session("mQuotation") = mQuotation
        Session("mQuotationList") = mQuotationList
        Session("mQuotationTextList") = mQuotationTextList
        Session("mDistinctTextListForEnquiry") = mDistinctTextListForEnquiry
        Session("mTransTypeID") = mTransTypeID
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mQuotation")
        Session.Remove("mQuotationList")
        Session.Remove("mQuotationTextList")
        Session.Remove("mDistinctTextListForEnquiry")
        Session.Remove("mTransTypeId")
        Session.Remove("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
        Session.Remove("EnquiryNo")
        Session.Remove("VendorName")
    End Sub
    Private Sub ClearAll()
        mTransTypeID = Session("mTransTypeId")
        If Session("MiddleFrame") <> "wfQuotationList_Ajax.aspx?TransTypeId=" & mTransTypeID Then
            Session.Remove("mQuotation")
            Session.Remove("mQuotationList")
            Session.Remove("mQuotationTextList")
            Session.Remove("mDistinctTextListForEnquiry")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("PriorityID")
            Session.Remove("QuotationText")
            Session.Remove("EnquiryText")
            Session.Remove("Name")
            Session.Remove("QuotationNo")
            Session.Remove("mItemList")
            Session.Remove("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
            Session.Remove("Amend") 'Added By Vikrant On 22-Nov-2019 For 
            Session.Remove("EnquiryNo")
            Session.Remove("VendorName")
        End If
    End Sub
    Private Sub NewRecord()
        mQuotation = Quotation.NewQuotation(mTransTypeID)
        mQuotation.Date = Today.Date.ToString(AppSettings("DateFormat").ToString)
        mQuotation.MarkClean()
        Session("mQuotation") = mQuotation
        Session("mTransTypeId") = mTransTypeID
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mQuotation = Quotation.GetQuotation(mId)
        mQuotation.MarkClean()
        Session("mQuotation") = mQuotation
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mQuotation = Quotation.GetQuotation(mId)
        Session("mQuotation") = mQuotation
        Session("mTransTypeId") = mTransTypeID
        GridBind()
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgQuotationList.DataBind()
        'cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
        cmbPriority.SelectedValue = PriorityID

        If cmbEnquiryText.Items.Contains(New System.Web.UI.WebControls.ListItem(EnquiryText)) Then
            cmbEnquiryText.SelectedValue = EnquiryText
        Else
            cmbEnquiryText.SelectedValue = "(All)"
        End If
        If cmbQuotationText.Items.Contains(New System.Web.UI.WebControls.ListItem(QuotationText)) Then
            cmbQuotationText.SelectedValue = QuotationText
        Else
            cmbQuotationText.SelectedValue = "(All)"
        End If

        txtPartNoSearch.Text = Name
        txtQuotationNo.Text = QuotationNo
        txtAmend.Text = Amend 'Added By Vikrant On 22-Nov-2019 For ALL22112019
        txtEnquiryNo.Text = EnquiryNo
        txtVendorName.Text = VendorName
        ControlVisibility(SearchIndex, DateIndex)
        SetTitle()  'Added by shweta on 22-12-11

        cmbAdd.Items.Clear()
        cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Multiple Parts", 1))
        'cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Add Parts From Enquiry (" + mPendingTransactionCount.EnquiryCountForQuotation.ToString + ")", 2))
        cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Multiple Parts From Enquiry", 2))
        If (AppSettings("NewRequisition") = "True" And mTransTypeID = Util.Trans.PurchaseQuotation) Then
            cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Add Requisition Items (" + mPendingTransactionCount.ReqItemCountForQuotation.ToString + ")", 3))
        ElseIf AppSettings("NewRequisition") = "False" Then 'End
            If (mTransTypeID = Util.Trans.PurchaseQuotation Or mTransTypeID = Util.Trans.Quotation) Then
                cmbAdd.Items.Add(New System.Web.UI.WebControls.ListItem("Store Approved part List", 3))
            End If '======================================================================
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim ErrorsCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mVendorName As String
                        Dim QuotationDetail As String
                        Try
                            Session("sender") = ""
                            mQuotation = CType(Session("mQuotation"), Quotation)

                            mVendorName = mQuotationList(mQuotation.ID).VendorName
                            If mQuotation.TransTypeID = 33 Then                  'Added By Prashant 20-July-2011
                                mModuleName = "Outright Quotation"
                            ElseIf mQuotation.TransTypeID = 36 Then
                                mModuleName = "Repair / Overhaul Quotation"
                            ElseIf mQuotation.TransTypeID = 37 Then
                                mModuleName = "Rental / Lease Quotation"
                            ElseIf mQuotation.TransTypeID = 2 Then
                                mModuleName = "Sales Quotation"
                            End If
                            'Added By Vikrant On 22-Nov-2019 For ALL22112019
                            mShowTopAmendedOrderNo = ShowTopAmendedOrderNo.GetTopAmendedOrderNo(mQuotation.Text, mQuotation.No, 1)
                            If (mQuotation.StatusID = 3) And (Not (mQuotation.ID.Equals(mShowTopAmendedOrderNo.ID))) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "You cannot delete this record as it is already amended.", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            'End
                            mQuotation.Delete()
                            mQuotation.Save()
                            DataFieldBind()
                            PendingTransCount()
                            SetControl()
                            'SetGrid()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                QuotationDetail = mQuotation.QuotationNo + "," + " Dated : " + mQuotation.DateFormatted + "," + " from : " + mVendorName
                                MarkLog(Util.Action.Delete, TransactionList.GetTransactionList().GetTransactionTypeName(mQuotation.TransTypeID).ToString, "Can't delete : " & QuotationDetail & " is Currently in use", Util.ErrorType.HandledError, mQuotation.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mQuotation.TransTypeID).ToString)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            ErrorsCount = ex.Errors.Count
                        Finally
                            TotalCount()
                            If ErrorsCount = 0 Then
                                mModuleName = TransactionList.GetTransactionList().GetTransactionTypeName(mQuotation.TransTypeID).ToString
                                Session("mModuleName") = mModuleName
                                QuotationDetail = mQuotation.QuotationNo + "," + " Dated : " + mQuotation.DateFormatted + "," + " from : " + mVendorName
                                MarkLog(Util.Action.Delete, mModuleName, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mQuotation.TransTypeID).ToString)
                                MSGBoxCtrl.show(MSGBox.Message_title.DeletedSuccessFully, MSGBox.Message_text.DeletedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            End If
                            'Session("ForEventLog") = "For Event Log"
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        PendingTransCount()
                        SetControl()
                        'SetGrid()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    PendingTransCount()
                    SetControl()
                    'SetGrid()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal QuotationNo As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal StatusID As Integer = 0, Optional ByVal VendorName As String = "", Optional ByVal EnquiryText As String = "", Optional ByVal EnquiryNo As Int16 = 0, Optional ByVal PriorityID As Integer = 0, Optional ByVal Amend As String = "")
        mQuotationList = Nothing
        dgQuotationList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mQuotationList = QuotationList.GetQuotationList(ItemName, Text, QuotationNo, FromDate, ToDate, StatusID, VendorName, EnquiryText, EnquiryNo, mTransTypeID, , PriorityID, Amend)
        'Set DataSource of the Grid
        Session("mQuotationList") = mQuotationList
        dgQuotationList.DataSource = mQuotationList
        dgQuotationList.DataBind()
        SetTitle() 'For lblResult
        upnlGridView.Update()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow(ItemName:=Trim(Name), Text:=Trim(QuotationText), QuotationNo:=CInt(Val(QuotationNo)), FromDate:=txtFromDate.Text.Trim, _
                ToDate:=txtToDate.Text.Trim, StatusID:=CInt(StatusId), VendorName:=Trim(VendorName), EnquiryText:=Trim(EnquiryText), _
                EnquiryNo:=CInt(Val(EnquiryNo)), PriorityID:=CInt(PriorityID), Amend:=Trim(Amend))

        'Select Case Index
        '    Case -1
        '        Call FindNow("", "", , FromDate, ToDate, 0, "", )   'for all records
        '    Case 0  'all
        '        Call FindNow("", "", , FromDate, ToDate, 0, "", )   'for all records
        '    Case 1 'date
        '        Call FindNow("", "", , txtFromDate.Text, txtToDate.Text, 0, "", , )    'for all records
        '    Case 2  'Quootation Text ,QuotationNo
        '        Call FindNow("", QuotationText, Val(QuotationNo), FromDate, ToDate, 0, "", , Amend:=Amend)  'for all records
        '    Case 3  'ItemName
        '        Call FindNow(Name, "", , FromDate, ToDate, 0, "", , )
        '    Case 4 ' Vendor Name
        '        Call FindNow(, "", , FromDate, ToDate, 0, Name, , )
        '    Case 5 ' EnquiryText 
        '        Call FindNow(, "", , FromDate, ToDate, 0, , EnquiryText, Val(QuotationNo))
        '    Case 6 ' Status
        '        Call FindNow(, "", , FromDate, ToDate, CInt(StatusId), , )
        '    Case 7 ' Priority
        '        Call FindNow(, "", , FromDate, ToDate, 0, , , , CInt(PriorityID))
        'End Select
        dgQuotationList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFrom.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblTo.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)

        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5 Or DateIndex = 7) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If

        'cmbQuotationText.Visible = IIf(SearchIndex = 2, True, False)
        'cmbEnquiryText.Visible = IIf(SearchIndex = 5, True, False)
        'lblNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbQuotationText.SelectedIndex <> 0 Or cmbEnquiryText.SelectedIndex <> 0), True, False)
        'txtNo.Visible = IIf((SearchIndex = 2 Or SearchIndex = 5) And (cmbQuotationText.SelectedIndex <> 0 Or cmbEnquiryText.SelectedIndex <> 0), True, False)
        'txtAmend.Visible = IIf(SearchIndex = 2 And cmbQuotationText.SelectedIndex <> 0, True, False) 'Added By Vikrant On 22-Nov-2019 For ALL22112019
        'txtName.Visible = IIf(SearchIndex >= 3 And SearchIndex <= 4, True, False)
        'cmbStatus.Visible = IIf(SearchIndex = 6, True, False)
        'cmbPriority.Visible = IIf(SearchIndex = 7, True, False)
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        'txtNo.Text = ""
        'txtName.Text = ""
        'txtAmend.Text = "" 'Added By Vikrant On 22-Nov-2019 For ALL22112019
    End Sub
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        EnquiryText = IIf(cmbEnquiryText.SelectedIndex <= 0, "", cmbEnquiryText.SelectedValue)
        QuotationText = IIf(cmbQuotationText.SelectedIndex <= 0, "", cmbQuotationText.SelectedValue)
        PriorityID = IIf(cmbPriority.SelectedIndex <= 0, 0, cmbPriority.SelectedValue)
        Name = txtPartNoSearch.Text.Trim
        QuotationNo = txtQuotationNo.Text.Trim
        EnquiryNo = txtEnquiryNo.Text.Trim
        VendorName = txtVendorName.Text.Trim
        'Added By Vikrant On 22-Nov-2019 For ALL22112019
        Amend = txtAmend.Text.Trim
        Session("Amend") = Amend
        'End
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("PriorityID") = PriorityID
        Session("EnquiryText") = EnquiryText
        Session("QuotationText") = QuotationText
        Session("QuotationNo") = QuotationNo
        Session("Name") = Name
        Session("EnquiryNo") = EnquiryNo
        Session("VendorName") = VendorName
    End Sub
    Private Sub addAttributes()
        txtQuotationNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetTitle()
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        lblQuotationList.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
        btnBottomClose.ToolTip = "Click to close List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " screen"
        btnCloseTop.ToolTip = "Click to close List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " screen"
        lblResult.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " as per criteria :" & mQuotationList.Count & " Record(s) found."
        ReportTitle = mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + " List Report" 'Added By Prashant 27/12/2007
        If mTransTypeID = 33 Then                                                                   'Added By Prashant 20-July-2011
            mModuleName = "Outright Quotation"
        ElseIf mTransTypeID = 36 Then
            mModuleName = "Repair / Overhaul Quotation"
        ElseIf mTransTypeID = 37 Then
            mModuleName = "Rental / Lease Quotation"
        ElseIf mTransTypeID = 2 Then
            mModuleName = "Sales Quotation"
        End If
        Session("mModuleName") = mModuleName
        'blQuotationList.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString + "    [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"
        lblQuotationList.Text = "List of " + mTransTypeList.GetTransactionTypeName(mTransTypeID).ToString
    End Sub
    Private Sub FillCombo()
        If mTransTypeID = 2 Then
            'cmbSearch.Items.Add(New ListItem("All", 0))
            'cmbSearch.Items.Add(New ListItem("Date", 1))
            'cmbSearch.Items.Add(New ListItem("Quotation", 2))
            'cmbSearch.Items.Add(New ListItem("Part QuotationNo.", 3))
            'cmbSearch.Items.Add(New ListItem("Customer", 4))
            'cmbSearch.Items.Add(New ListItem("Enquiry", 5))
            'cmbSearch.Items.Add(New ListItem("Status", 6))
            'cmbSearch.Items.Add(New ListItem("Priority", 7))
            dgQuotationList.Columns(3).HeaderText = "Customer"
        Else
            'cmbSearch.Items.Add(New ListItem("All", 0))
            'cmbSearch.Items.Add(New ListItem("Date", 1))
            'cmbSearch.Items.Add(New ListItem("Quotation", 2))
            'cmbSearch.Items.Add(New ListItem("Part QuotationNo.", 3))
            'cmbSearch.Items.Add(New ListItem("Supplier", 4))
            'cmbSearch.Items.Add(New ListItem("Enquiry", 5))
            'cmbSearch.Items.Add(New ListItem("Status", 6))
            'cmbSearch.Items.Add(New ListItem("Priority", 7))
            dgQuotationList.Columns(3).HeaderText = "Supplier"
        End If
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        Select Case mTransTypeID
            Case Util.Trans.Quotation
                IsInRoleString = "Quotation"
            Case Util.Trans.PurchaseQuotation
                IsInRoleString = "PurchaseQuotation"
            Case Util.Trans.OverHaulRepairQuotation
                IsInRoleString = "PurchaseQuotationRepairOverHaul"
            Case Util.Trans.RentialLeaseQuotation
                IsInRoleString = "PurchaseQuotationRentalLease"
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
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        PriorityID = Session("PriorityID")
        QuotationText = Session("QuotationText")
        EnquiryText = Session("EnquiryText")
        Name = Session("Name")
        Amend = Session("Amend") 'Added By Vikrant On 22-Nov-2019 For ALL22112019
        mQuotationTextList = DistinctTextListForQuotation.GetDistinctTextList("8", 0, True, "(All)")
        cmbQuotationText.DataSource = mQuotationTextList
        mDistinctTextListForEnquiry = DistinctTextListForEnquiry.GetDistinctTextList("7", 0, True, "(All)")  '7 Enquiry
        cmbEnquiryText.DataSource = mDistinctTextListForEnquiry
        mQuotationList = QuotationList.GetQuotationList(, , , "1/1/1900", "1/1/2200", 0, , , , mTransTypeID)
        dgQuotationList.DataSource = mQuotationList
        Session("mQuotationList") = mQuotationList
        DataBind()
    End Sub
    Public Sub PendingTransCount()
        mPendingTransactionCount = PendingTransactionCount.GetCount(Today.Date.ToString, IIf(mTransTypeID = 33, 32, IIf(mTransTypeID = 36, 34, 35)))
    End Sub
    Public Sub TotalCount()
        'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(mTransTypeID)
        Session("mTransactionListCount") = mTransactionListCount
        'End
        SetTitle()
        upnlTitle.Update()
    End Sub
    Public Sub GridBind()
        dgQuotationList.DataSource = mQuotationList
        dgQuotationList.DataBind()
        upnlGridView.Update()
    End Sub
    'Private Sub SetGrid()
    '    Dim P As Integer
    '    For j As Integer = 0 To dgQuotationList.Rows.Count - 1
    '        P = CType(Me.dgQuotationList.Rows.Item(j).Cells(12).Text, Integer)
    '        If P <= 0 Then
    '            dgQuotationList.Rows.Item(j).Cells(11).Enabled = False
    '        End If
    '    Next
    'End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Saylee on 19-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbDate.Enabled = True Then
                setFocus(cmbDate)
            End If
            If Session("IsBackFromPendingList") = "True" Then
            Else 'First Time Set Value
                mTransTypeID = Request.QueryString("TransTypeId")
            End If
            Session.Remove("IsBackFromPendingList")

            Session("mTransTypeId") = mTransTypeID
            Session("MiddleFrame") = "wfQuotationList_Ajax.aspx?TransTypeId=" & mTransTypeID
            FillCombo()
            DataFieldBind()
            TotalCount()
            PendingTransCount()
            SetControl()
        End If
        'SetGrid()
        SetTitle()
    End Sub
    Private Sub dgQuotationList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgQuotationList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                'Dim index As Integer = CInt(e.CommandArgument) + dgQuotationList.PageIndex * dgQuotationList.PageSize
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mID)
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                GridBind()
                SetTitle()
                Dim QuotationDetail As String = mQuotation.QuotationNo + " Dated : " + mQuotation.DateFormatted + " to " + mQuotationList(mQuotation.ID).VendorName & " Created By : " & mQuotation.UserName
                MarkLog(Util.Action.Edit, TransactionList.GetTransactionList().GetTransactionTypeName(mQuotation.TransTypeID).ToString, QuotationDetail, Util.ErrorType.NoError, mQuotation.ID, EventLogID, TransactionList.GetTransactionList().GetModuleName(mQuotation.TransTypeID).ToString)
                Dim str As String
                str = "openledgersame('wfQuotation_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                'Dim index As Integer = CInt(e.CommandArgument) + dgQuotationList.PageIndex * dgQuotationList.PageSize
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not IsInRole(Rights.Delete)) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
            Case "ViewRec"
                Dim QuotationNo As New Random
                Dim StrName As String = "abc" & QuotationNo.Next.ToString
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
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                    GridBind()
                End If
        End Select
        'SetGrid()
    End Sub
    'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
    '    cmbDate.SelectedIndex = 0
    '    cmbQuotationText.SelectedIndex = 0
    '    cmbEnquiryText.SelectedIndex = 0
    '    ClearControls()
    '    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
    '    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
    '    setPeriod(DateIndex)
    '    If cmbSearch.Enabled = True Then
    '        setFocus(cmbSearch)
    '    End If
    'End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbQuotationText.SelectedIndexChanged, cmbEnquiryText.SelectedIndexChanged
        If sender.id = "cmbDate" Then
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            setPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                setFocus(cmbDate)
            End If
        ElseIf sender.id = "cmbQuotationText" Then
            txtQuotationNo.Text = "0"
            txtAmend.Text = ""
            If cmbQuotationText.Enabled = True Then
                setFocus(cmbQuotationText)
            End If
        ElseIf sender.id = "cmbEnquiryText" Then
            txtEnquiryNo.Text = "0"
            If cmbEnquiryText.Enabled = True Then
                setFocus(cmbEnquiryText)
            End If
        End If

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        'SetGrid()
        btnBottomPrint.Enabled = IIf(mQuotationList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mQuotationList.Count = 0, False, True)
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomAddNew.Click, btnAddNewTop.Click
        NewRecord()
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        MarkLog(Util.Action.[New], TransactionList.GetTransactionList().GetModuleName(mQuotation.TransTypeID).ToString, "", Util.ErrorType.NoError, mQuotation.ID, EventLogID)
        ' Dim str As String
        'str = "openledgersame('wfQuotation_Ajax.aspx?BackPage=index.aspx');"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        If cmbAdd.SelectedValue = "1" Then 'Add Parts
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfCommonPartList_Ajax.aspx?BackPage1=index.aspx&BackPage=wfQuotation_Ajax.aspx&LookinTypeID=1&Name=&OpenFrom=Quotation&TransDate=" + mQuotation.DateFormatted.ToString + "&ItemsCount=" + mQuotation.QuotationItems.Count.ToString + "');", True)
        ElseIf cmbAdd.SelectedValue = "2" Then 'Add Enquiry Parts
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfEnquiriesForQuotation_Ajax.aspx?BackPage1=index.aspx&BackPage=wfQuotation_Ajax.aspx&Date=" & mQuotation.DateFormatted.ToString & "&VendorID = " & Guid.Empty.ToString & "');", True)
        ElseIf cmbAdd.SelectedValue = "3" Then 'Add Requisition Parts
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRequisitionPartList_Ajax.aspx?BackPage1=index.aspx&BackPage=wfQuotation_Ajax.aspx&ListFor=1&TransDate=" & mQuotation.DateFormatted.ToString + "&ItemsCount=" + mQuotation.QuotationItems.Count.ToString + "');", True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgQuotationList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgQuotationList.PageIndexChanging
        dgQuotationList.PageIndex = e.NewPageIndex
        dgQuotationList.DataSource = mQuotationList
        Session("mQuotationList") = mQuotationList
        GridBind()
        'SetGrid()
    End Sub
    Private Sub dgQuotationList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgQuotationList.Sorting
        mQuotationList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mQuotationList") = mQuotationList
        dgQuotationList.DataSource = mQuotationList
        GridBind()
        'SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

#Region " Report "
    'Created By :- Jyoti
    'Dated On 9/5/2007
#Region "Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomPrint.Click, btnPrintTop.Click
        If Not IsInRole(Rights.Print) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'For Quotation List
        Dim Rpt As New crQuotationList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        SetTitle() 'For Report Titel
        SearchStr1 = ""
        SearchStr2 = ""
        'If cmbSearch.SelectedIndex = 0 Then
        '    'All
        '    SearchStr1 = "The report shows all records till date."
        '    SearchStr2 = ""
        'ElseIf cmbSearch.SelectedIndex = 1 Then
        '    'Date
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    If cmbDate.SelectedIndex = 0 Then
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
        '    ElseIf cmbDate.SelectedIndex = 6 Then
        '        'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
        '    Else
        '        'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
        '    End If
        'ElseIf cmbSearch.SelectedIndex = 2 And cmbQuotationText.SelectedIndex > 0 Then
        '    'Quotation QuotationNo.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbQuotationText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text + IIf(txtAmend.Text.Trim = "", "", "-" + txtAmend.Text)
        'ElseIf cmbSearch.SelectedIndex = 2 Then
        '    'Quotation QuotationNo.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbQuotationText.SelectedItem.Text ''+ " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 3 Then
        '    'Part Number
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        'ElseIf cmbSearch.SelectedIndex = 4 Then
        '    'Vendor
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        'ElseIf cmbSearch.SelectedIndex = 5 And cmbEnquiryText.SelectedIndex > 0 Then
        '    'Enquiry QuotationNo.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbEnquiryText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 5 Then
        '    'Enquiry QuotationNo.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbEnquiryText.SelectedItem.Text ''+ " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 6 Then
        '    'Status
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        'End If

        ReportDetails.Add(New rptStatus(, 0, , _
              dgQuotationList.Columns.Item(1).HeaderText, dgQuotationList.Columns.Item(2).HeaderText, dgQuotationList.Columns.Item(3).HeaderText, _
              dgQuotationList.Columns.Item(4).HeaderText, dgQuotationList.Columns.Item(5).HeaderText, dgQuotationList.Columns.Item(6).HeaderText, _
              dgQuotationList.Columns.Item(7).HeaderText, dgQuotationList.Columns.Item(8).HeaderText))

        'Added by Saylee on 16-June 2007
        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgQuotationList.PageIndex 'Code Added
        TotalCount = Me.dgQuotationList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(7) As String

        For j = 0 To TotalCount - 1

            Me.dgQuotationList.PageIndex = j
            Me.dgQuotationList.DataSource = mQuotationList
            Session("mQuotationList") = mQuotationList
            dgQuotationList.DataBind()
            For I = 0 To Me.dgQuotationList.PageSize - 1
                If I <= Me.dgQuotationList.Rows.Count - 1 Then
                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""
                    str(7) = ""

                    If Me.dgQuotationList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgQuotationList.Rows(I).Cells.Item(1).Text
                    If Me.dgQuotationList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgQuotationList.Rows(I).Cells.Item(2).Text
                    If Me.dgQuotationList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgQuotationList.Rows(I).Cells.Item(3).Text
                    If Me.dgQuotationList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgQuotationList.Rows(I).Cells.Item(4).Text
                    If Me.dgQuotationList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgQuotationList.Rows(I).Cells.Item(5).Text
                    If Me.dgQuotationList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgQuotationList.Rows(I).Cells.Item(6).Text
                    If Me.dgQuotationList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgQuotationList.Rows(I).Cells.Item(7).Text
                    If Me.dgQuotationList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgQuotationList.Rows(I).Cells.Item(8).Text


                    ReportDetails.Add(New rptStatus(, 1, , str(0), _
                        str(1), str(2), str(3), str(4), str(5), str(6), str(7)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, ReportTitle, SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mQuotationList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is No record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        Me.dgQuotationList.DataSource = mQuotationList
        Session("mQuotationList") = mQuotationList
        dgQuotationList.DataBind()
        'SetGrid()
    End Sub
#End Region

#End Region

End Class