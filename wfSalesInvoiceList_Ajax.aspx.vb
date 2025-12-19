Public Class wfSalesInvoiceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mSalesInvoice As SalesInvoice
    Public mSalesInvoiceList As SalesInvoiceList
    Public mDistinctTextListForInvoice As DistinctTextListForSalesInvoice
    Public mDistinctTextListForOrder As DistinctTextListForSalesInvoice
    Public mDistinctTextListForIssue As DistinctTextListForSalesInvoice
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptSalesInvoiceRegister
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, SalesOrderText, IssueText, SalesInvoiceText, PartNoSearchForSalesInvoice, No, CustomerSearchForSalesInvoice,
        SalesOrderNoSearchForSalesInvoice, IssueNoSearchForSalesInvoice As String
    Public mTransTypeID As Trans
    Dim SearchStr1 As String
    Dim SearchStr2 As String
    Dim mCompanyDetail As New CompanyDetail
    Dim EventLogID As Guid      'Added by Vikrant on 21-July-2011
    Dim mTransactionListCount As TransactionListCount 'Added By Shweta On 19-August-2013 for ALL16082013-1
    Dim mSalesInvoiceTypeList As SalesInvoiceTypeList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mSalesInvoice = Session("mSalesInvoice")
        mSalesInvoiceList = Session("mSalesInvoiceList")
        mDistinctTextListForOrder = Session("mDistinctTextListForOrder")
        mDistinctTextListForIssue = Session("mDistinctTextListForIssue")
        mDistinctTextListForInvoice = Session("mDistinctTextListForInvoice")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        SalesOrderText = Session("SalesOrderText")
        IssueText = Session("IssueText")
        SalesInvoiceText = Session("SalesInvoiceText")
        PartNoSearchForSalesInvoice = Session("PartNoSearchForSalesInvoice")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        lblTotal.Text = Session("lblTotalText") '------Added by Vikrant on 23-Dec-2011 FOR-ALL23122011-----
        CustomerSearchForSalesInvoice = Session("CustomerSearchForSalesInvoice")
        SalesOrderNoSearchForSalesInvoice = IIf(IsNothing(Session("SalesOrderNoSearchForSalesInvoice")), 0, Session("SalesOrderNoSearchForSalesInvoice"))
        IssueNoSearchForSalesInvoice = IIf(IsNothing(Session("IssueNoSearchForSalesInvoice")), 0, Session("IssueNoSearchForSalesInvoice"))
    End Sub
    Private Sub SetSession()
        Session("mSalesInvoice") = mSalesInvoice
        Session("mSalesInvoiceList") = mSalesInvoiceList
        Session("mDistinctTextListForOrder") = mDistinctTextListForOrder
        Session("mDistinctTextListForIssue") = mDistinctTextListForIssue
        Session("mDistinctTextListForInvoice") = mDistinctTextListForInvoice
        Session("mTransTypeId") = mTransTypeID
        Session("lblTotalText") = lblTotal.Text '------Added by Vikrant on 23-Dec-2011 FOR-ALL23122011-----
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSalesInvoiceList")
        Session.Remove("mDistinctTextListForOrder")
        Session.Remove("mDistinctTextListForIssue")
        Session.Remove("mDistinctTextListForInvoice")
        Session.Remove("mTransTypeId")
        Session.Remove("CustomerSearchForSalesInvoice")
        Session.Remove("SalesOrderNoSearchForSalesInvoice")
        Session.Remove("IssueNoSearchForSalesInvoice")
    End Sub
    Private Sub ClearAll()
        mTransTypeID = Session("mTransTypeId")
        If Session("MiddleFrame") <> "wfSalesInvoiceList_Ajax.aspx?TransTypeId=" & mTransTypeID Then
            Session.Remove("mSalesInvoiceList")
            Session.Remove("mInvoiceList")
            Session.Remove("mDistinctTextListForOrder")
            Session.Remove("mDistinctTextListForIssue")
            Session.Remove("mDistinctTextListForInvoice")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("SalesOrderText")
            Session.Remove("SalesInvoiceText")
            Session.Remove("IssueText")
            Session.Remove("PartNoSearchForSalesInvoice")
            Session.Remove("No")
            Session.Remove("CustomerSearchForSalesInvoice")
            Session.Remove("SalesOrderNoSearchForSalesInvoice")
            Session.Remove("IssueNoSearchForSalesInvoice")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        ''cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId

        If cmbSalesOrderText.Items.Contains(New System.Web.UI.WebControls.ListItem(SalesOrderText)) Then ''Added By Rajnish On 07-01-2008=========
            cmbSalesOrderText.SelectedValue = SalesOrderText
        Else
            cmbSalesOrderText.SelectedValue = "(All)"
        End If
        If cmbIssueText.Items.Contains(New System.Web.UI.WebControls.ListItem(IssueText)) Then
            cmbIssueText.SelectedValue = IssueText
        Else
            cmbIssueText.SelectedValue = "(All)"
        End If
        If cmbSalesInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(SalesInvoiceText)) Then
            cmbSalesInvoiceText.SelectedValue = SalesInvoiceText
        Else
            cmbSalesInvoiceText.SelectedValue = "(All)"
        End If
        txtPartNoSearch.Text = PartNoSearchForSalesInvoice
        txtSalesInvoiceNo.Text = No
        txtSalesOrderNo.Text = SalesOrderNoSearchForSalesInvoice
        txtIssueNo.Text = IssueNoSearchForSalesInvoice
        ControlVisibility(SearchIndex, DateIndex)
    End Sub
    Private Sub NewRecord()
        mSalesInvoice = SalesInvoice.NewSalesInvoice(mTransTypeID)
        mSalesInvoice.SalesInvoiceDate = Today.Date
        Session("mSalesInvoice") = mSalesInvoice
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mSalesInvoice = SalesInvoice.GetSalesInvoice(mId)
        mSalesInvoice.MarkClean()
        Session("mSalesInvoice") = mSalesInvoice
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mSalesInvoice = SalesInvoice.GetSalesInvoice(mId)
        Session("mSalesInvoice") = mSalesInvoice
        Session("mTransTypeId") = mTransTypeID
    End Sub
    Private Sub ClearControl()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim ErrorsCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mSalesInvoice = CType(Session("mSalesInvoice"), SalesInvoice)
                            mSalesInvoice.Delete()
                            mSalesInvoice.Save()
                            DataFieldBind()
                            SetControl()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                Dim mInvoiceDetail As String = mSalesInvoice.SalesInvoiceNo + " Dated : " + mSalesInvoice.SalesInvoiceDateFormatted + " to " + mSalesInvoiceList(mSalesInvoice.ID).VendorName
                                MarkLog(Util.Action.Delete, "SalesInvoice", "Can't delete :" & mInvoiceDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            ErrorsCount = ex.Errors.Count
                        Finally
                            SetTitle()
                            If ErrorsCount = 0 Then
                                Dim mInvoiceDetail As String = mSalesInvoice.SalesInvoiceNo + " Dated : " + mSalesInvoice.SalesInvoiceDateFormatted + " to " + mSalesInvoiceList(mSalesInvoice.ID).VendorName
                                MarkLog(Util.Action.Delete, "SalesInvoice", mInvoiceDetail, Util.ErrorType.NoError, mSalesInvoice.ID, EventLogID)
                            End If
                            'Session("ForEventLog") = "For Event Log"
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal SalesInvoiceText As String = "", Optional ByVal InvoiceNo As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal ReceiptText As String = "", Optional ByVal ReceiptNo As Integer = 0, Optional ByVal SalesOrderText As String = "", Optional ByVal SalesOrderNo As Integer = 0, Optional ByVal VendorName As String = "", Optional ByVal ItemName As String = "", Optional ByVal StatusID As Integer = 0)
        mSalesInvoiceList = Nothing
        dgSalesInvoiceList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mSalesInvoiceList = SalesInvoiceList.GetSalesInvoiceList(SalesInvoiceText, InvoiceNo, FromDate, ToDate, ReceiptText, ReceiptNo, SalesOrderText, SalesOrderNo, VendorName,
                                                                 ItemName, StatusID, mTransTypeID)
        'Set DataSource of the Grid
        Session("mSalesInvoiceList") = mSalesInvoiceList
        dgSalesInvoiceList.DataSource = mSalesInvoiceList
        dgSalesInvoiceList.DataBind()
        lblResult.Text = "List of Sales Invoice as per criteria : " & mSalesInvoiceList.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow(SalesInvoiceText:=Trim(SalesInvoiceText), InvoiceNo:=CInt(Val(No)), FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, ReceiptText:=Trim(IssueText),
                ReceiptNo:=CInt(Val(IssueNoSearchForSalesInvoice)), SalesOrderText:=Trim(SalesOrderText), SalesOrderNo:=CInt(Val(SalesOrderNoSearchForSalesInvoice)),
                VendorName:=Trim(CustomerSearchForSalesInvoice), ItemName:=Trim(PartNoSearchForSalesInvoice), StatusID:=CInt(StatusId))
        'Select Case Index
        '    Case -1
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", 0) 'for all records
        '    Case 0  'all
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", 0) 'for all records
        '    Case 1 'Order date
        '        Call FindNow("", 0, txtFromDate.Text.ToString, txtToDate.Text.ToString, "", 0, "", 0, "", "", 0)
        '    Case 2  'Invoice Text 
        '        Call FindNow(SalesInvoiceText, CInt(Val(No)), FromDate, ToDate, "", 0, "", 0, "", "", 0)
        '    Case 3 ' Part No 
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", PartNoSearchForSalesInvoice, 0)
        '    Case 4 ' Vendor Name
        'Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, Name, "", 0)
        '    Case 5  'Sales Order Text 
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, SalesOrderText, CInt(Val(txtSalesInvoiceNo.Text)), "", "", 0)
        '    Case 6  'Receipt Text 
        '        Call FindNow("", 0, FromDate, ToDate, IssueText, CInt(Val(txtSalesInvoiceNo.Text)), "", 0, "", "", 0)
        '    Case 7  'Status Text 
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, "", "", CInt(StatusId))
        'End Select
        dgSalesInvoiceList.PageIndex = 0    'Added Code on May,25,2007
    End Sub

    Private Sub SetPeriod(Index As Int32)

        Try

            Select Case Index
                Case 0 ' All
                    ' 
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
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)   '31-Mar-2006
                    End If

                    txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)

                Case 6 'Between Dates

                    FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                    ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
                    txtFromDate.Text = FromDate
                    txtToDate.Text = ToDate

            End Select

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        ''cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFrom.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblTo.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        ''cmbSalesInvoiceText.Visible = IIf(SearchIndex = 2, True, False)
        ''cmbSalesOrderText.Visible = IIf(SearchIndex = 5, True, False)
        ''cmbIssueText.Visible = IIf(SearchIndex = 6, True, False)

        ''txtSalesInvoiceNo.Visible = IIf(SearchIndex = 2 And cmbSalesInvoiceText.SelectedIndex <> 0 Or SearchIndex = 5 And cmbSalesOrderText.SelectedIndex <> 0 Or SearchIndex = 6 And cmbIssueText.SelectedIndex <> 0, True, False)

        ''txtPartNoSearch.Visible = IIf(SearchIndex = 3 Or SearchIndex = 4, True, False)
        ''cmbStatus.Visible = IIf(SearchIndex = 7, True, False)
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
    Private Sub CallFindNowReport(ByVal Index As Integer)
        'If txtSalesInvoiceNo.Text = "" Or IsNumeric(txtSalesInvoiceNo.Text) = False Then txtSalesInvoiceNo.Text = "0"
        Dim SalesInvoiceText As String = ""
        SalesInvoiceText = IIf(cmbSalesInvoiceText.SelectedIndex <= 0, "", cmbSalesInvoiceText.SelectedItem.Text)
        Dim SalesOrderText As String = ""
        SalesOrderText = IIf(cmbSalesOrderText.SelectedIndex <= 0, "", cmbSalesOrderText.SelectedItem.Text)
        Dim IssueText As String = ""
        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedItem.Text)
        Select Case Index
            Case -1
                Call FindNow("", 0, "1-Jan-1900", "1-Jan-2200", "", 0, "", 0, "", "", 0) 'for all records
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList(, , "1/1/1900", "1/1/2200", , , , , , , , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
            Case 0  'all
                Call FindNow("", 0, "1-Jan-1900", "1-Jan-2200", "", 0, "", 0, "", "", 0) 'for all records
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList(, , "1/1/1900", "1/1/2200", , , , , , , , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
            Case 1 'Order date
                Call FindNow("", 0, txtFromDate.Text.ToString, txtToDate.Text.ToString, "", 0, "", 0, "", "", 0)
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList(, , txtFromDate.Text.ToString, txtToDate.Text.ToString, , , , , , , , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), txtFromDate.Text.ToString, txtToDate.Text.ToString, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
            Case 2  'Invoice Text 
                Call FindNow(SalesInvoiceText, CInt(Val(txtSalesInvoiceNo.Text)), "1-Jan-1900", "1-Jan-2200", "", 0, "", 0, "", "", 0)
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList(SalesInvoiceText, Trim(txtSalesInvoiceNo.Text), "1/1/1900", "1/1/2200", , , , , , , , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", SalesInvoiceText, Trim(txtSalesInvoiceNo.Text), "", "", "", "", "", "", "", "", "")
            Case 3 ' Part No 
                Call FindNow("", 0, "1-Jan-1900", "1-Jan-2200", "", 0, "", 0, "", txtPartNoSearch.Text, 0)
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList("", "", "1/1/1900", "1/1/2200", , , , , , Trim(txtPartNoSearch.Text), , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", Trim(txtPartNoSearch.Text), "", "", "", "", "", "", "", "", "", "", "", "")
            Case 4 ' Vendor Name
                Call FindNow("", 0, "1-Jan-1900", "1-Jan-2200", "", 0, "", 0, txtPartNoSearch.Text, "", 0)
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList("", "", "1/1/1900", "1/1/2200", , , , , Trim(txtPartNoSearch.Text), , , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", Trim(txtPartNoSearch.Text), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
            Case 5  'Order Text 
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList("", "", "1/1/1900", "1/1/2200", , , SalesOrderText, Trim(txtSalesInvoiceNo.Text), , , , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", SalesOrderText, "", Trim(txtSalesInvoiceNo.Text), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
            Case 6  'Receipt Text 
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList("", "", "1/1/1900", "1/1/2200", IssueText, Trim(txtSalesInvoiceNo.Text), , , , , , )
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", IssueText, "", "", Trim(txtSalesInvoiceNo.Text), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
            Case 7  'Status Text 
                objReg = rptSalesInvoiceRegister.GetSalesInvoiceList("", "", "1/1/1900", "1/1/2200", , , , , , , , CInt(cmbStatus.SelectedIndex))
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", Trim(cmbStatus.SelectedItem.Text), "", "", "", "", "", "", "", "", "")
        End Select
    End Sub
    Private Sub setVariables()
        PartNoSearchForSalesInvoice = txtPartNoSearch.Text.Trim
        No = txtSalesInvoiceNo.Text.Trim
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        SalesOrderText = IIf(cmbSalesOrderText.SelectedIndex <= 0, "", cmbSalesOrderText.SelectedValue)
        IssueText = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedValue)
        SalesInvoiceText = IIf(cmbSalesInvoiceText.SelectedIndex <= 0, "", cmbSalesInvoiceText.SelectedValue)
        CustomerSearchForSalesInvoice = txtCustomer.Text.Trim
        SalesOrderNoSearchForSalesInvoice = txtSalesOrderNo.Text.Trim
        IssueNoSearchForSalesInvoice = txtIssueNo.Text.Trim

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("SalesOrderText") = SalesOrderText
        Session("IssueText") = IssueText
        Session("SalesInvoiceText") = SalesInvoiceText
        Session("No") = No
        Session("PartNoSearchForSalesInvoice") = PartNoSearchForSalesInvoice
        Session("CustomerSearchForSalesInvoice") = CustomerSearchForSalesInvoice
        Session("SalesOrderNoSearchForSalesInvoice") = SalesOrderNoSearchForSalesInvoice
        Session("IssueNoSearchForSalesInvoice") = IssueNoSearchForSalesInvoice
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub addAttributes()
        txtSalesInvoiceNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtSalesInvoiceNo').value,event)")
    End Sub
    Private Sub SetTitle() 'Added By Utkarsh On 21-Jul-2011 For All19072011
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(23)
        Dim mTotal = mTransactionListCount(0).Count
        lblSalesIncvoiceList.Text = "List of Sales Invoice "
        Session("lblTotalText") = lblTotal.Text
        upnlTitle.Update()
    End Sub
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        'DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)'Commented and added by Shweta on 19-August-2013 for ALL16082013-1
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex) 'end
        StatusId = Session("StatusId")
        SalesOrderText = Session("SalesOrderText")
        IssueText = Session("ReceiptText")
        SalesInvoiceText = Session("SalesInvoiceText")
        PartNoSearchForSalesInvoice = Session("PartNoSearchForSalesInvoice")
        'No = Session("No")
        mDistinctTextListForOrder = DistinctTextListForSalesInvoice.GetDistinctTextListForSalesInvoice("9", , True, "(All)")
        cmbSalesOrderText.DataSource = mDistinctTextListForOrder
        mDistinctTextListForIssue = DistinctTextListForSalesInvoice.GetDistinctTextListForSalesInvoice("3", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue
        mDistinctTextListForInvoice = DistinctTextListForSalesInvoice.GetDistinctTextListForSalesInvoice("11", , True, "(All)")
        cmbSalesInvoiceText.DataSource = mDistinctTextListForInvoice
        mSalesInvoiceTypeList = SalesInvoiceTypeList.GetSalesInvoiceTypeList()
        cmbSalesInvoiceType.DataSource = mSalesInvoiceTypeList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Saylee on 19-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbDate.Enabled = True Then
                setFocus(cmbDate)
            End If
            'mTransTypeID = Request.QueryString("TransTypeId")
            mTransTypeID = 0
            Session("mTransTypeId") = mTransTypeID
            Session("MiddleFrame") = "wfSalesInvoiceList_Ajax.aspx?TransTypeId=" & mTransTypeID
            DataFieldBind()
            SetControl()
            SetTitle()
        End If
    End Sub
    Private Sub dgSalesInvoiceList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSalesInvoiceList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                If (Not User.IsInRole("SalesInvoiceView") And Not User.IsInRole("SalesInvoiceEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mID)
                'Changed by Vikrant on 21-july-2011
                Dim mInvoiceDetail As String = mSalesInvoice.SalesInvoiceNo + " Dated : " + mSalesInvoice.SalesInvoiceDateFormatted + " to " + mSalesInvoiceList(mSalesInvoice.ID).VendorName
                MarkLog(Util.Action.Edit, "SalesInvoice", mInvoiceDetail, Util.ErrorType.NoError, mSalesInvoice.ID, EventLogID, "SalesInvoice")
                'End
                Dim str As String
                str = "openledgersame('wfSalesInvoice_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("SalesInvoiceDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub dgSalesInvoiceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSalesInvoiceList.PageIndexChanging
        dgSalesInvoiceList.PageIndex = e.NewPageIndex
        dgSalesInvoiceList.DataSource = mSalesInvoiceList
        Session("mSalesInvoiceList") = mSalesInvoiceList
        dgSalesInvoiceList.DataBind()
    End Sub
    Private Sub dgSalesInvoiceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSalesInvoiceList.Sorting
        mSalesInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mSalesInvoiceList") = mSalesInvoiceList
        dgSalesInvoiceList.DataSource = mSalesInvoiceList
        dgSalesInvoiceList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbSalesOrderText.SelectedIndexChanged, cmbSalesInvoiceText.SelectedIndexChanged, cmbIssueText.SelectedIndexChanged
        If sender.ID = "cmbDate" Then
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            setPeriod(DateIndex)
            ControlVisibility(1, DateIndex:=DateIndex)
            If cmbDate.Enabled = True Then
                setFocus(cmbDate)
            End If
        ElseIf sender.ID = "cmbSalesOrderText" Then
            txtSalesOrderNo.Text = "0"
            If cmbSalesOrderText.Enabled = True Then
                setFocus(cmbSalesOrderText)
            End If
        ElseIf sender.ID = "cmbSalesInvoiceText" Then
            txtSalesInvoiceNo.Text = "0"
            If cmbSalesInvoiceText.Enabled = True Then
                setFocus(cmbSalesInvoiceText)
            End If
        ElseIf sender.ID = "cmbIssueText" Then
            txtIssueNo.Text = "0"
            If cmbIssueText.Enabled = True Then
                setFocus(cmbIssueText)
            End If
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(1)
        btnPrintTop.Enabled = IIf(mSalesInvoiceList.Count = 0, False, True)
        btnBottomPrint.Enabled = IIf(mSalesInvoiceList.Count = 0, False, True)
        upnBottomButtons.Update()
        upnTopButtons.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomAddNew.Click, btnAddNewTop.Click
        NewRecord()
        If (Not User.IsInRole("SalesInvoiceNew")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        mTransTypeID = cmbSalesInvoiceType.SelectedValue
        Session("mTransTypeId") = mTransTypeID
        NewRecord()
        setVariables()
        MarkLog(Util.Action.[New], "Sales Invoice", "", Util.ErrorType.NoError, mSalesInvoice.ID, EventLogID) 'Changed by Vikrant on 21-july-2011
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfSalesInvoice_Ajax.aspx?BackPage=index.aspx');", True)
    End Sub
    Private Sub btnBottomClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region " Reports "
    Private Sub btnBottomPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomPrint.Click, btnPrintTop.Click
        If Not User.IsInRole("SalesInvoicePrint") Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        Dim Rpt As New crSalesInvoiceList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
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
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Value.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Value.ToString).FormattedText
        '    Else
        '        'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Value.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Value.ToString).FormattedText
        '    End If
        'ElseIf cmbSearch.SelectedIndex = 2 And cmbSalesInvoiceText.SelectedIndex > 0 Then
        '    'Sales Invoice 
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbSalesInvoiceText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 2 Then
        '    'Sales Invoice 
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbSalesInvoiceText.SelectedItem.Text '' + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 3 Then
        '    'Part Number
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtPartNoSearch.Text
        'ElseIf cmbSearch.SelectedIndex = 4 Then
        '    'Customer
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtPartNoSearch.Text
        'ElseIf cmbSearch.SelectedIndex = 5 And cmbSalesOrderText.SelectedIndex > 0 Then
        '    'Order
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtPartNoSearch.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 5 Then
        '    'Order
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtPartNoSearch.Text '' + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 6 And cmbIssueText.SelectedIndex > 0 Then
        '    'Issue
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 6 Then
        '    'Issue
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbIssueText.SelectedItem.Text ''+ " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 7 Then
        '    'Status
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        'End If
        ReportDetails.Add(New rptStatus(, 0, ,
              dgSalesInvoiceList.Columns.Item(1).HeaderText, dgSalesInvoiceList.Columns.Item(2).HeaderText, dgSalesInvoiceList.Columns.Item(3).HeaderText,
              dgSalesInvoiceList.Columns.Item(4).HeaderText, dgSalesInvoiceList.Columns.Item(5).HeaderText, dgSalesInvoiceList.Columns.Item(6).HeaderText,
              dgSalesInvoiceList.Columns.Item(7).HeaderText, dgSalesInvoiceList.Columns.Item(8).HeaderText, dgSalesInvoiceList.Columns.Item(9).HeaderText))

        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgSalesInvoiceList.PageIndex
        TotalCount = Me.dgSalesInvoiceList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(8) As String

        For j = 0 To TotalCount - 1

            Me.dgSalesInvoiceList.PageIndex = j
            Me.dgSalesInvoiceList.DataSource = mSalesInvoiceList
            Session("mSalesInvoiceList") = mSalesInvoiceList
            dgSalesInvoiceList.DataBind()
            For I = 0 To Me.dgSalesInvoiceList.PageSize - 1
                If I <= Me.dgSalesInvoiceList.Rows.Count - 1 Then

                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""
                    str(7) = ""
                    str(8) = ""
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(1).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(2).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(3).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(4).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(5).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(6).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(7).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(8).Text
                    If Me.dgSalesInvoiceList.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgSalesInvoiceList.Rows(I).Cells.Item(9).Text

                    ReportDetails.Add(New rptStatus(, 1, , str(0),
                        str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8)))
                End If
            Next
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Sales Invoice List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mSalesInvoiceList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)

        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "Sales Invoice", "Sales Invoice List Report", Util.ErrorType.NoError, Guid.Empty)
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        Me.dgSalesInvoiceList.DataSource = mSalesInvoiceList
        Session("mSalesInvoiceList") = mSalesInvoiceList
        dgSalesInvoiceList.DataBind()
    End Sub
#End Region


End Class