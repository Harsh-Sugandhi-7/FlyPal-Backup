'Ajax Conversion By Vikrant On 05-Jan-2015

Public Class wfOtherChargeList_Ajax
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
#End Region

#Region " Variable Declaration "
    Public mOtherChargeList As OtherChargeList
    Public mOtherCharge As OtherCharge
    Public mDistinctTextListForOtherCharge As DistinctTextListForOtherCharge
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptOtherChargeRegister
    Dim SearchIndex, DateIndex, FromDate, ToDate, OtherChargeText, OtherChargeNo, SupplierSearchForOtherChargeList As String
    Private SearchStr1, SearchStr2 As String

    Dim EventLogID As Guid 'Added By Utkarsh On 22-Jul-2011 For All19072011
    Dim OCDetail As String 'Added By Utkarsh On 22-Jul-2011 For All19072011
    Dim totcnt As Integer 'Added by shweta on 23-12-11
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer
    'Added By Vikrant on 08-Jun-2015 For ALL08062015
    Dim mDistinctTextListForInvoice As DistinctTextListForInvoice
    Dim mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim mDistinctTextListForOrder As DistinctTextListForOrder
    Dim ReceiptText, InvoiceText, OrderText, ReceiptNoSearchForOtherChargeList, InvoiceNoSearchForOtherChargeList, OrderNoSearchForOtherChargeList, _
        PartNoSearchForOtherChargeList As String
    'End
    Dim mFileAttach As FileAttach 'Added By Vikrant On 24-Sep-2020 For ALL24092020
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mOtherCharge = Session("mOtherCharge")
        mOtherChargeList = Session("mOtherChargeList")
        mDistinctTextListForOtherCharge = Session("mDistinctTextListForOtherCharge")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SupplierSearchForOtherChargeList = Session("SupplierSearchForOtherChargeList")
        OtherChargeText = Session("OtherChargeText")
        OtherChargeNo = IIf(IsNothing(Session("OtherChargeNo")), 0, Session("OtherChargeNo"))
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        'Added By Vikrant on 08-Jun-2015 For ALL08062015
        mDistinctTextListForReceipt = Session("mDistinctTextListForReceipt")
        mDistinctTextListForInvoice = Session("mDistinctTextListForInvoice")
        ReceiptText = Session("ReceiptText")
        InvoiceText = Session("InvoiceText")
        OrderText = Session("OrderText")
        'End
        mFileAttach = Session("mFileAttach") 'Added By Vikrant On 24-Sep-2020 For ALL24092020

        ReceiptNoSearchForOtherChargeList = IIf(IsNothing(Session("ReceiptNoSearchForOtherChargeList")), 0, Session("ReceiptNoSearchForOtherChargeList"))
        InvoiceNoSearchForOtherChargeList = IIf(IsNothing(Session("InvoiceNoSearchForOtherChargeList")), 0, Session("InvoiceNoSearchForOtherChargeList"))
        OrderNoSearchForOtherChargeList = IIf(IsNothing(Session("OrderNoSearchForOtherChargeList")), 0, Session("OrderNoSearchForOtherChargeList"))
        PartNoSearchForOtherChargeList = Session("PartNoSearchForOtherChargeList")
    End Sub
    Private Sub SetSession()
        Session("mOtherCharge") = mOtherCharge
        Session("mOtherChargeList") = mOtherChargeList
        Session("mDistinctTextListForOtherCharge") = mDistinctTextListForOtherCharge
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mOtherCharge")
        Session.Remove("mOtherChargeList")
        Session.Remove("mDistinctTextListForOtherCharge")
        Session.Remove("totcnt")
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        Session.Remove("mFileAttach") 'Added By Vikrant On 24-Sep-2020 For ALL24092020
        Session.Remove("ReceiptNoSearchForOtherChargeList")
        Session.Remove("InvoiceNoSearchForOtherChargeList")
        Session.Remove("OrderNoSearchForOtherChargeList")
    End Sub
    'Added By Vikrant On 24-Sep-2020 For ALL24092020
    Private Sub SetGrid()
        'Dim IsAttachAdded As Boolean
        'For j As Integer = 0 To dgOtherChargeList.Rows.Count - 1
        '    IsAttachAdded = CType(Me.dgOtherChargeList.Rows.Item(j).Cells(12).Text, Boolean)
        '    If IsAttachAdded = False Then
        '        dgOtherChargeList.Rows.Item(j).Cells(11).Enabled = False
        '    End If
        'Next
    End Sub
    'End
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfOtherChargeList_Ajax.aspx?" Then
            Session.Remove("mOtherCharge")
            Session.Remove("mOtherChargeList")
            Session.Remove("mDistinctTextListForOtherCharge")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("OtherChargeText")
            Session.Remove("OtherChargeNo")
            Session.Remove("SupplierSearchForOtherChargeList")
            Session.Remove("mCurrentpage")
            Session.Remove("mpageSize")
            Session.Remove("mpageindex")
            Session.Remove("pagecount")
            Session.Remove("totalCount")
            'Added By Vikrant on 08-Jun-2015 For ALL08062015
            Session.Remove("mDistinctTextListForReceipt")
            Session.Remove("mDistinctTextListForInvoice")
            Session.Remove("ReceiptText")
            Session.Remove("InvoiceText")
            Session.Remove("OrderText")
            'End
            Session.Remove("mFileAttach")
            Session.Remove("ReceiptNoSearchForOtherChargeList")
            Session.Remove("InvoiceNoSearchForOtherChargeList")
            Session.Remove("OrderNoSearchForOtherChargeList")
            Session.Remove("PartNoSearchForOtherChargeList")
        End If
    End Sub
    'Added By Vikrant On 24-Sep-2020 For ALL24092020
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachmentChild(ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    'End
    Private Sub SetControl()
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgOtherChargeList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgOtherChargeList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgOtherChargeList.DataBind()
        'cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        If cmbOtherChargeText.Items.Contains(New System.Web.UI.WebControls.ListItem(OtherChargeText)) Then ' Added By Rajnish On 03-01-2008
            cmbOtherChargeText.SelectedValue = OtherChargeText
        Else
            cmbOtherChargeText.SelectedValue = "(All)"
        End If
        If cmbInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(InvoiceText)) Then ' Added By Rajnish On 03-01-2008
            cmbInvoiceText.SelectedValue = InvoiceText
        Else
            cmbInvoiceText.SelectedValue = "(All)"
        End If
        If cmbReceiptText.Items.Contains(New System.Web.UI.WebControls.ListItem(ReceiptText)) Then ' Added By Rajnish On 03-01-2008
            cmbReceiptText.SelectedValue = ReceiptText
        Else
            cmbReceiptText.SelectedValue = "(All)"
        End If
        If cmbOrderText.Items.Contains(New System.Web.UI.WebControls.ListItem(OrderText)) Then ' Added By Rajnish On 03-01-2008
            cmbOrderText.SelectedValue = OrderText
        Else
            cmbOrderText.SelectedValue = "(All)"
        End If
        ''cmbOtherChargeText.SelectedValue = IIf(OtherChargeText = "", "(All)", OtherChargeText)
        txtSupplier.Text = SupplierSearchForOtherChargeList
        txtNo.Text = OtherChargeNo
        txtPartNoSearch.Text = PartNoSearchForOtherChargeList
        txtReceipNo.Text = ReceiptNoSearchForOtherChargeList
        txtInvoiceNo.Text = InvoiceNoSearchForOtherChargeList
        txtOrderNo.Text = OrderNoSearchForOtherChargeList
        ControlVisibility(SearchIndex, DateIndex)
        'lblResult.Text = "List of Other Charge as per criteria :" & mOtherChargeList.Count & " Record(s) found."
    End Sub
    Private Sub SetTitle()
        Dim mOtherChargeListCount As OtherChargeListCount = OtherChargeListCount.GetRecordsCount()
        totcnt = mOtherChargeListCount(0).Count
        lblotherchargelist.Text = "List Of Other Charges" + " [Total No of Record(s):-" + totcnt.ToString() + "]" 'Added by shweta on 22-12-11
    End Sub
    Private Sub NewRecord()
        mOtherCharge = OtherCharge.NewOtherCharge
        mOtherCharge.Date = Today.Date
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mOtherCharge = OtherCharge.GetOtherCharge(mId)
        mOtherCharge.MarkClean()
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub GridBind()
        dgOtherChargeList.DataSource = mOtherChargeList
        dgOtherChargeList.DataBind()
        SetGrid() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mOtherCharge = OtherCharge.GetOtherCharge(mId)
        Session("mOtherCharge") = mOtherCharge
    End Sub
    Private Sub DataFieldBind()
        Session("totcnt") = totcnt 'Added by shweta on 23-12-11
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        OtherChargeText = Session("OtherChargeText")
        OtherChargeNo = Session("OtherChargeNo")
        SupplierSearchForOtherChargeList = Session("SupplierSearchForOtherChargeList")
        mDistinctTextListForOtherCharge = DistinctTextListForOtherCharge.GetDistinctTextList("6", , True, "(All)")
        cmbOtherChargeText.DataSource = mDistinctTextListForOtherCharge
        'Added By Vikrant on 08-Jun-2015 For ALL08062015
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("2", , True, "(All)")
        cmbReceiptText.DataSource = mDistinctTextListForReceipt
        'mDistinctTextListForInvoice = DistinctTextListForInvoice.GetDistinctTextListForInvoice("15", , True, "(All)")
        mDistinctTextListForInvoice = DistinctTextListForInvoice.GetDistinctTextListForInvoice("31", , True, "(All)")
        cmbInvoiceText.DataSource = mDistinctTextListForInvoice
        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder
        'End
        DataBind()
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        'lblResult.Text = "List of Other Charge as per criteria :" & mOtherChargeList.Count & " Record(s) found."
    End Sub
    Private Function FormLevelRights(ByVal Type As UserRightsFor) As Boolean
        Select Case Type
            ''Case UserRightsFor.urfNew
            ''    Return mUserRights.Contains(271)
            ''Case UserRightsFor.urfEdit
            ''    Return mUserRights.Contains(272) And mOtherChargeList.Count > 0
            ''Case UserRightsFor.urfDelete
            ''    Return mUserRights.Contains(273) And mOtherChargeList.Count > 0
            ''Case UserRightsFor.urfPrint
            ''    Return mUserRights.Contains(275) And mOtherChargeList.Count > 0
        End Select
    End Function
    Private Sub EnableDisableButtons()
        'Enables Buttons as per User Rights
        btnAddNew.Enabled = FormLevelRights(UserRightsFor.urfNew)
        dgOtherChargeList.Columns(8).Visible = FormLevelRights(UserRightsFor.urfEdit)
        dgOtherChargeList.Columns(9).Visible = FormLevelRights(UserRightsFor.urfDelete)
        BtnPrint.Enabled = FormLevelRights(UserRightsFor.urfPrint)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mOtherCharge As OtherCharge
                            Session("sender") = ""

                            mOtherCharge = CType(Session("mOtherCharge"), OtherCharge)
                            'Added By Vikrant On 24-Sep-2020 For ALL24092020
                            If mOtherCharge.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachmentChild(mOtherCharge.ID)
                            End If
                            'End
                            'mOtherCharge.DeleteOtherCharge(mOtherCharge.ID)
                            mOtherCharge.Delete()
                            'Added By Vikrant On 24-Sep-2020 For ALL24092020
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'End
                            mOtherCharge.Save()
                            SetControl()
                            SetTitle()
                            ControlEnability()
                            SetGrid() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
                            upnlTitle.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Changed By Utkarsh On 22-Jul-2011 For All19072011
                                OCDetail = mOtherChargeList(mOtherCharge.ID).OtherChargeNumber + " Dated : " + mOtherChargeList(mOtherCharge.ID).DateFormatted
                                MarkLog(Util.Action.Delete, "Other Charge", "Can't delete : " & OCDetail & " is Currently in use", Util.ErrorType.NoError, mOtherCharge.ID, EventLogID)
                                'End                                
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            'SetControl()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 22-Jul-2011 For All19072011
                                'OCDetail = mOtherChargeList(mOtherCharge.ID).OtherChargeNumber + " Dated : " + mOtherChargeList(mOtherCharge.ID).DateFormatted
                                OCDetail = mOtherCharge.OtherChargeNo + " Dated : " + mOtherCharge.DateFormatted
                                MarkLog(Util.Action.Delete, "Other Charge", OCDetail, Util.ErrorType.NoError, mOtherCharge.ID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub UpdateGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = "List of Other Charge as per criteria : " & totalCount & " Record(s) found."
        Else
            lblResult.Text = "List of Other Charge as per criteria : " & currentrow + 1 & " to " & currentrow + mOtherChargeList.Count & " of " & totalCount & " Record(s) found."
        End If

        SliderExtender1.Minimum = 1
        SliderExtender1.Maximum = pagecount
        Slidercontrol.Text = mCurrentpage
        txtPageDisplay.Text = mCurrentpage
        lblpagecount.Text = pagecount
        If pagecount > 1 Then
            PnlPaging.Visible = True
        Else
            PnlPaging.Visible = False
        End If

        dgOtherChargeList.DataBind()
        SetGrid() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
        upnlGrid.Update()
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal InvoiceText As String = "", Optional ByVal InvoiceNo As Integer = 0, Optional ByVal ReceiptText As String = "", Optional ByVal ReceiptNo As Integer = 0, Optional ByVal ItemName As String = "", Optional ByVal OrderText As String = "", Optional ByVal OrderNo As Integer = 0, Optional ByVal VendorName As String = "")
        mOtherChargeList = Nothing
        dgOtherChargeList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mOtherChargeList = OtherChargeList.GetOtherChargeList(Text, No, FromDate, ToDate, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize, InvoiceText:=InvoiceText, InvoiceNo:=InvoiceNo, ReceiptText:=ReceiptText, ReceiptNo:=ReceiptNo, ItemName:=ItemName, OrderText:=OrderText, OrderNo:=OrderNo, VendorName:=VendorName)
        'Set DataSource of the Grid
        totalCount = mOtherChargeList.TotalRecords
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount

        Session("mOtherChargeList") = mOtherChargeList
        dgOtherChargeList.DataSource = mOtherChargeList
        UpdateGridView()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow(Text:=OtherChargeText, No:=CInt(Val(OtherChargeNo)), FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, _
                InvoiceText:=Trim(InvoiceText), InvoiceNo:=CInt(Val(InvoiceNoSearchForOtherChargeList)), ReceiptText:=Trim(ReceiptText), _
                ReceiptNo:=CInt(Val(ReceiptNoSearchForOtherChargeList)), ItemName:=Trim(PartNoSearchForOtherChargeList), _
                OrderText:=Trim(OrderText), OrderNo:=CInt(Val(OrderNoSearchForOtherChargeList)), VendorName:=Trim(SupplierSearchForOtherChargeList))
        'Select Case Index
        '    Case -1
        '        Call FindNow("", 0, FromDate, ToDate)    'for all records
        '    Case 0  'all
        '        Call FindNow("", 0, FromDate, ToDate)  'for all records
        '    Case 1 'OtherCharge date
        '        Call FindNow("", 0, txtFromDate.Text, txtToDate.Text)
        '    Case 2  'OtherCharge Text , No And Amend
        '        Call FindNow(OtherChargeText, CInt(Val(OtherChargeNo)), FromDate, ToDate)
        '        'Added By Vikrant on 08-Jun-2015 For ALL08062015
        '    Case 3  'Invoice
        '        Call FindNow("", 0, FromDate, ToDate, InvoiceText, CInt(Val(OtherChargeNo)))
        '    Case 4  'Receipt
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, ReceiptText, CInt(Val(OtherChargeNo)))
        '    Case 5  'Order
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, , , , OrderText, CInt(Val(OtherChargeNo)))
        '    Case 6  'PartNo
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, Name)
        '    Case 7  'Supplier
        '        Call FindNow("", 0, FromDate, ToDate, "", 0, "", 0, , , , Name)
        '        'End
        'End Select
        dgOtherChargeList.PageIndex = 0    ' Added Code on May,25,2007
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-Jan-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("1-Jan-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = Today.AddDays(-6).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = Today.AddDays(1).AddMonths(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
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
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End If
                txtToDate.Text = Today.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        OtherChargeText = IIf(cmbOtherChargeText.SelectedIndex <= 0, "", cmbOtherChargeText.SelectedValue)
        SupplierSearchForOtherChargeList = txtSupplier.Text.Trim
        'OtherChargeNo = txtNo.Text.Trim
        OtherChargeNo = IIf(txtNo.Text.Trim <> "", txtNo.Text.Trim, "0")
        'Added By Vikrant on 08-Jun-2015 For ALL08062015
        ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)
        InvoiceText = IIf(cmbInvoiceText.SelectedIndex <= 0, "", cmbInvoiceText.SelectedValue)
        OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue)
        PartNoSearchForOtherChargeList = txtPartNoSearch.Text.Trim

        ReceiptNoSearchForOtherChargeList = IIf(txtReceipNo.Text.Trim <> "", txtReceipNo.Text.Trim, "0")
        InvoiceNoSearchForOtherChargeList = IIf(txtInvoiceNo.Text.Trim <> "", txtInvoiceNo.Text.Trim, "0")
        OrderNoSearchForOtherChargeList = IIf(txtOrderNo.Text.Trim <> "", txtOrderNo.Text.Trim, "0")

        Session("ReceiptNoSearchForOtherChargeList") = ReceiptNoSearchForOtherChargeList
        Session("InvoiceNoSearchForOtherChargeList") = InvoiceNoSearchForOtherChargeList
        Session("OrderNoSearchForOtherChargeList") = OrderNoSearchForOtherChargeList

        Session("ReceiptText") = ReceiptText
        Session("InvoiceText") = InvoiceText
        Session("OrderText") = OrderText
        'End
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("OtherChargeText") = OtherChargeText
        Session("OtherChargeNo") = OtherChargeNo
        Session("SupplierSearchForOtherChargeList") = SupplierSearchForOtherChargeList
        Session("PartNoSearchForOtherChargeList") = PartNoSearchForOtherChargeList
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        'cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        ''calFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)
        ''calToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)
        lblFromDate.Visible = IIf(DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(DateIndex <> 0, True, False)
        'cmbOtherChargeText.Visible = IIf(SearchIndex = 2, True, False)
        'lblNo.Visible = IIf((SearchIndex = 2 And cmbOtherChargeText.SelectedIndex <> 0) Or (SearchIndex = 3 And cmbInvoiceText.SelectedIndex <> 0) Or (SearchIndex = 4 And cmbReceiptText.SelectedIndex <> 0) Or (SearchIndex = 5 And cmbOrderText.SelectedIndex <> 0), True, False)
        'txtNo.Visible = IIf((SearchIndex = 2 And cmbOtherChargeText.SelectedIndex <> 0) Or (SearchIndex = 3 And cmbInvoiceText.SelectedIndex <> 0) Or (SearchIndex = 4 And cmbReceiptText.SelectedIndex <> 0) Or (SearchIndex = 5 And cmbOrderText.SelectedIndex <> 0), True, False)

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
        'Added By Vikrant on 08-Jun-2015 For ALL08062015
        'cmbInvoiceText.Visible = IIf(SearchIndex = 3, True, False)
        'cmbReceiptText.Visible = IIf(SearchIndex = 4, True, False)
        'txtName.Visible = IIf(SearchIndex = 6 Or SearchIndex = 7, True, False)
        'cmbOrderText.Visible = IIf(SearchIndex = 5, True, False)
        'End
    End Sub
    Private Sub ControlEnability()
        BtnPrint.Enabled = IIf(dgOtherChargeList.Rows.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(dgOtherChargeList.Rows.Count = 0, False, True)
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        'txtName.Text = ""
    End Sub
    Private Sub CallFindNowReport(ByVal Index As Integer)
        Dim OtherChargeText As String = ""
        OtherChargeText = IIf(cmbOtherChargeText.SelectedIndex <= 0, "", cmbOtherChargeText.SelectedItem.Text)
        OtherChargeNo = IIf(txtNo.Text.Trim <> "", txtNo.Text.Trim, "0")
        SearchStr1 = ""
        SearchStr2 = ""
        objReg = rptOtherChargeRegister.GetOtherChargeRegister(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, Text:=OtherChargeText, No:=txtNo.Text.Trim, _
                                                               InvoiceText:=InvoiceText.Trim, InvoiceNo:=Val(txtInvoiceNo.Text.Trim), ReceiptText:=ReceiptText.Trim, _
                                                               ReceiptNo:=Val(txtReceipNo.Text), ItemName:=txtPartNoSearch.Text.Trim, OrderText:=OrderText, _
                                                               OrderNo:=Val(txtOrderNo.Text), VendorName:=txtSupplier.Text)
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), _
                                                                                  txtFromDate.Text, txtToDate.Text, "", "", "", "", "", "", "", "", _
                                                                                  SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", _
                                                                                  "", "", "", "", 0, "", "", AppSettings("Logo"))
         
        'Select Case Index
        '    Case -1
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", , )
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '    Case 0  'all
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", , )
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '    Case 1 'OtherCharge date
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister(txtFromDate.Text, txtToDate.Text, , )
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), txtFromDate.Text, txtToDate.Text, "", "", "", "", "", "", "", "", SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '    Case 2  'OtherCharge Text , No And Amend
        'objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", OtherChargeText, Trim(txtNo.Text))
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", OtherChargeText, "", "", OtherChargeNo, SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '        'Added By Vikrant on 08-Jun-2015 For ALL08062015
        '    Case 3  'Invoice Text , No 
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", , , InvoiceText, Trim(txtNo.Text))
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", OtherChargeText, "", "", OtherChargeNo, SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '    Case 4  'Receipt Text , No 
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", , , , , ReceiptText, Trim(txtNo.Text))
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", OtherChargeText, "", "", OtherChargeNo, SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '    Case 5  'Order Text , No 
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", , , , , , , , OrderText, Trim(txtNo.Text))
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", OtherChargeText, "", "", OtherChargeNo, SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '    Case 6  'Item Name
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", , , , , , , Trim(txtName.Text))
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", OtherChargeText, "", "", OtherChargeNo, SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '    Case 7  'Supplier
        '        objReg = rptOtherChargeRegister.GetOtherChargeRegister("1/1/1900", "1/1/2200", , , , , , , , , , Trim(txtName.Text))
        '        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", OtherChargeText, "", "", OtherChargeNo, SearchStr1, SearchStr2, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        '        'End
        'End Select
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 22-Jul-2011 For All19072011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
            Session("MiddleFrame") = "wfOtherChargeList_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            SetTitle()
            ControlEnability()
            SetGrid() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
        End If

        'EnableDisableButtons()
    End Sub
    Private Sub dgOtherChargeList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgOtherChargeList.RowCommand

        Dim mID As Guid
        Dim index As Integer
        Select Case e.CommandName
            Case "EditRec"
                index = CInt(e.CommandArgument) '+ dgOtherChargeList.PageIndex * dgOtherChargeList.PageSize
                mID = mOtherChargeList(index).ID
                If (Not User.IsInRole("OtherChargeView") And Not User.IsInRole("OtherChargeEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                'Response.Redirect("wfOtherCharge.aspx")
                Session("EditCharge") = True
                EditRecord(mID)
                'Changed By Utkarsh On 22-Jul-2011 For All19072011
                OCDetail = mOtherCharge.OtherChargeNo + " Dated : " + mOtherCharge.DateFormatted
                MarkLog(Util.Action.Edit, "Other Charge", OCDetail, Util.ErrorType.NoError, mOtherCharge.ID, EventLogID)
                'End
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfOtherCharge_Ajax.aspx?BackPage=index.aspx');", True)
            Case "DeleteRec"
                index = CInt(e.CommandArgument) '+ dgOtherChargeList.PageIndex * dgOtherChargeList.PageSize
                mID = mOtherChargeList(index).ID
                If (Not User.IsInRole("OtherChargeDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                GridBind()
                DeleteRecord(mID)
                'Added By Vikrant On 24-Sep-2020 For ALL24092020
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                index = CInt(e.CommandArgument) '+ dgOtherChargeList.PageIndex * dgOtherChargeList.PageSize
                mID = mOtherChargeList(index).ID
                GridBind()
                mOtherCharge = OtherCharge.GetOtherCharge(mID)
                GetAttachment(mOtherCharge.ID, mOtherCharge.IsAttachmentAdded)
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If

        End Select
    End Sub
    'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
    '    cmbDate.SelectedIndex = 0
    '    cmbOtherChargeText.SelectedIndex = 0
    '    'Added By Vikrant on 08-Jun-2015 For ALL08062015
    '    cmbInvoiceText.ClearSelection()
    '    cmbReceiptText.ClearSelection()
    '    cmbOrderText.ClearSelection()
    '    'End
    '    ClearControls()
    '    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
    '    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
    '    setPeriod(DateIndex)
    '    If cmbSearch.Enabled = True Then
    '        cmbSearch.Focus()
    '    End If
    'End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbOtherChargeText.SelectedIndexChanged, cmbReceiptText.SelectedIndexChanged, cmbInvoiceText.SelectedIndexChanged, cmbOrderText.SelectedIndexChanged
        If sender.ID = "cmbDate" Then
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            setPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
        ElseIf sender.ID = "cmbOtherChargeText" Then
            txtNo.Text = "0"
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            If cmbOtherChargeText.Enabled = True Then
                cmbOtherChargeText.Focus()
            End If
        ElseIf sender.ID = "cmbReceiptText" Then
            txtReceipNo.Text = "0"
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            If cmbReceiptText.Enabled = True Then
                cmbReceiptText.Focus()
            End If
        ElseIf sender.ID = "cmbInvoiceText" Then
            txtInvoiceNo.Text = "0"
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            If cmbInvoiceText.Enabled = True Then
                cmbInvoiceText.Focus()
            End If
        ElseIf sender.ID = "cmbOrderText" Then
            txtOrderNo.Text = "0"
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            If cmbOrderText.Enabled = True Then
                cmbOrderText.Focus()
            End If
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        dgOtherChargeList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex)
        dgOtherChargeList.DataBind()
        ControlEnability()
        SetGrid() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        'lblResult.Text = "List of Other Charge as per criteria :" & mOtherChargeList.Count & " Record(s) found."
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        NewRecord()
        If Not User.IsInRole("OtherChargeNew") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'Changed By Utkarsh On 22-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Other Charge", "", Util.ErrorType.NoError, mOtherCharge.ID, EventLogID)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfOtherCharge_Ajax.aspx?BackPage=index.aspx');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub BtnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrint.Click, btnPrintTop.Click
        If Not User.IsInRole("OtherChargePrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim Index As Int32 = 1
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsOtherCharge As New dsOtherCharge
        myReport = New crOtherChargeRegSummaryLandscape 'crptOtherChargeRegSummaryLandscape
        CallFindNowReport(Index)
        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        dsOtherCharge.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(dsOtherCharge)
        da.Fill(dsOtherCharge, objReg)
        da.Fill(dsOtherCharge, mrptImage)
        da.Fill(dsOtherCharge, objSearch)
        myReport.SetDataSource(dsOtherCharge)
        Session("CrystalReport") = myReport
        'Commented By Utkarsh On 22-Jul-2011 For All19072011
        'MarkLog(Util.Action.Print, "OtherCharge", "OtherCharge List Report", Util.ErrorType.NoError, Guid.Empty)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        objReg = Nothing
        objSearch = Nothing
    End Sub
    Private Sub dgOtherChargeList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgOtherChargeList.Sorting
        mOtherChargeList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mOtherChargeList") = mOtherChargeList
        dgOtherChargeList.DataSource = mOtherChargeList
        dgOtherChargeList.DataBind()
        SetGrid() 'Added By Vikrant On 24-Sep-2020 For ALL24092020
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnGridPaging_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        dgOtherChargeList.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(1)
    End Sub

#End Region

End Class