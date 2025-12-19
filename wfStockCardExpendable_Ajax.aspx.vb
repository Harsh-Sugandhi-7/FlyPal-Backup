'Created by Utkarsh on 03-Dec-2013
Public Class wfStockCardExpendable_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ParameterValues As Hashtable
    Dim mStockCardExpendablelist As rptPartHistoryBinCardServiceableUnserviceableList
    Dim mAlternatePartNumbers As AlternatePartNumbers
    Dim mPartName As String = ""
    Dim mDescription As String = ""
    Dim mStorageLife As String = ""
    Dim mCategoryname As String = ""
    Dim mLocation As String = ""
    Dim mTotQty As String = ""
    Dim mRelNoteNo As String = ""
    Dim mAlternatePartNo As String = ""
    Dim mmaxstocklevel As String = ""
    Dim mminstocklevel As String = ""
    Dim mreorder As String = ""
    Dim mordqty As String = ""
    Dim mUnitName As String = ""
    ''
    Dim mApplicability As String = "" 'Added By Utkarsh ON 22-Apr-2013 FOR BA-22042013-1
    Dim mIssue As Issue
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Dim mReceipt As Receipt
    Dim mScrapLife As String = ""
    Public mSerialNo As String = ""
    Public mBinCardNumber As String = ""

    Dim mItem As Item
    Public mRequisitionNew As RequisitionNew
    Dim TransTypeID, ReqTypeID As Integer
    Dim mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt As rptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt
    Dim EventLogID As Guid
    Dim mrptDueCalibrationList As rptDueCalibrationList
    Dim mrptExpiryDate As rptExpiryDate
    Dim CountOfExpired As Integer
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        ParameterValues = CType(Session("ParameterValues"), Hashtable)
        mStockCardExpendablelist = Session("mStockCardExpendablelist")
        mAlternatePartNumbers = Session("mAlternatePartNumbers")
        mItem = Session("mItemFromStockCardExpendable")
    End Sub
    Private Sub ShowPartInfo(Optional ByVal FromAltPartGrid As Boolean = False)
        mItem = Item.GetItem(New Guid(ParameterValues("PartID").ToString))
        Session("mItemFromStockCardExpendable") = mItem
        lbltextPartNo.Text = mItem.Name
        If mItem.IPCReference = "" Then
            lbltextPartNo.BackColor = Nothing
        Else
            lbltextPartNo.BackColor = Color.Yellow
        End If
        If mItem.NotInUse = True Then
            lblItemNotInUse.Visible = True
            lblItemNotInUseDate.Visible = True
            lblItemNotInUseDate.Text = mItem.NotInUseDateFormatted
            ItemNotInUseImage.Visible = True
        Else
            lblItemNotInUse.Visible = False
            lblItemNotInUseDate.Visible = False
            lblItemNotInUseDate.Text = ""
            ItemNotInUseImage.Visible = False
        End If
        If mItem.IsOneTimePurchase = True Then
            lblOnetimepurchase.Visible = True
        Else
            lblOnetimepurchase.Visible = False
        End If
        mPartName = lbltextPartNo.Text
        lbltextDescription.Text = mItem.Description
        mDescription = lbltextDescription.Text
        lbltextLocation.Text = mItem.Location
        mLocation = lbltextLocation.Text
        mAlternatePartNumbers = mItem.AlternatePartNos
        Session("mAlternatePartNumbers") = mAlternatePartNumbers
        If mAlternatePartNumbers.Count <> 0 Then
            For i As Integer = 0 To mAlternatePartNumbers.Count - 1
                If i = 0 Then
                    mAlternatePartNo = mAlternatePartNo + mAlternatePartNumbers(i).PartName
                Else
                    mAlternatePartNo = mAlternatePartNo + ", " + mAlternatePartNumbers(i).PartName
                End If
            Next
        End If
        'Added By Prashant 6-Aug-2013  BA06082013
        If Not FromAltPartGrid Then
            If mAlternatePartNumbers.Count > 0 Then
                gdvAlternate.DataSource = mAlternatePartNumbers
                gdvAlternate.DataBind()
            End If
            lblAlternateParts.Text = mItem.AlternatePartNos.Count.ToString + " Alternate Parts : "
        End If
        'End
        'Added By Utkarsh ON 22-Apr-2013 FOR BA-22042013-1
        lblApplicabilityList.Text = ""
        If mItem.ItemApplicables.Count <> 0 Then
            For i As Integer = 0 To mItem.ItemApplicables.Count - 1
                If i = 0 Then
                    lblApplicabilityList.Text = lblApplicabilityList.Text + mItem.ItemApplicables(i).ModelName
                Else
                    lblApplicabilityList.Text = lblApplicabilityList.Text + ", " + mItem.ItemApplicables(i).ModelName
                End If
            Next
        End If
        mApplicability = lblApplicabilityList.Text
        'End
        lblItemNotInUseNote.Text = mItem.Note
        lbltextUnit.Text = mItem.UnitName
        mUnitName = mItem.UnitName
        lbltextMinStockLevel.Text = mItem.MinStockLevel
        mminstocklevel = lbltextMinStockLevel.Text
        lbltextMaxStockLevel.Text = mItem.MaxStockLevel
        mmaxstocklevel = lbltextMaxStockLevel.Text
        lbltextReOrderLevel.Text = mItem.MinReOrderLevel
        mreorder = lbltextReOrderLevel.Text
        lbltextScrapLife.Text = IIf(mItem.ExpiryMonths = 0, "N/A", mItem.ExpiryMonths & " Months")  'Added By Prashant 27-Oct-2014 ALL27102014-3
        mScrapLife = lbltextScrapLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3

        lbltextStorageLife.Text = IIf(mItem.StorageLife = 0, "N/A", mItem.StorageLife & " Months") 'Added By Prashant 27-Oct-2014 ALL27102014-3
        mStorageLife = lbltextStorageLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3
        lblSerialNoList.Text = CStr(ParameterValues("SerialNo")) 'Added By Prashant 24-Dec-2014 ALL23122014-1
        mSerialNo = lblSerialNoList.Text
        lblBinCardNumber.Text = mItem.BinCardNumber
        mBinCardNumber = lblBinCardNumber.Text
        If mItem.EssentialcategoryID = 0 Then 'Added By Prashant 20-Apr-2021 BA20042021
            lbltextEssentialcategory.Text = "GO"
        ElseIf mItem.EssentialcategoryID = 1 Then
            lbltextEssentialcategory.Text = "No-GO"
        ElseIf mItem.EssentialcategoryID = 2 Then
            lbltextEssentialcategory.Text = "GO-IF"
        Else
            lbltextEssentialcategory.Text = ""
        End If
        'Dim mrptOnOrderPartStatus As rptStoreBalance
        'mrptOnOrderPartStatus = rptStoreBalance.GetStoreBalance(mItem.Name, mItem.Description, "", False, Guid.Empty, Guid.Empty, False, 2, , , , , , , , , , , , , "Landing Value")
        'mrptOnOrderPartStatus = rptStoreBalance.GetStoreBalance(mItem.Name, mItem.Description, "", False, Guid.Empty, Guid.Empty, False, 0, Guid.Empty.ToString, 0, False, Today.Date.ToString, True, True, , , , , "", Guid.Empty.ToString, "Landing Value", Guid.Empty.ToString, , True)
        'If mrptOnOrderPartStatus.Count > 0 Then
        '    lbltextOrderQty.Text = Format(mrptOnOrderPartStatus.Item(0).OnOrder, "##0.##")
        'Else
        '    lbltextOrderQty.Text = ""
        'End If
        mordqty = "" 'lbltextOrderQty.Text

        'Added By Utkarsh FOR ALL26082013
        If Not mStockCardExpendablelist Is Nothing AndAlso mStockCardExpendablelist.Count > 0 Then
            Dim temprciqty As Decimal = 0
            Dim tempissueqty As Decimal = 0
            For i As Integer = 0 To mStockCardExpendablelist.Count - 1
                temprciqty = temprciqty + mStockCardExpendablelist(i).ReceiptQty
                tempissueqty = tempissueqty + mStockCardExpendablelist(i).IssueQty
            Next
            lbltextBalQty.Text = temprciqty - tempissueqty
        Else
            lbltextBalQty.Text = 0
        End If
        'End
        lblItemCategory.Text = mItem.CategoryName 'Added By Vikrant On 20-May-2014 For BA20052014
        If mItem.AttachmentCount > 0 Then
            ImageButton1.Visible = True
            lblView.Visible = True
        Else
            ImageButton1.Visible = False
            lblView.Visible = False
        End If
        upnlDetails.Update()
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean) 'Added By Shweta on 7-Nov-2012 for ALL07112012
        setValue()
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim mPartHistoryBinCardServiceable As rptPartHistoryBinCardServiceableUnserviceableList

        Dim Value As String = ""
        Dim ReportName As String = ""

        mPartHistoryBinCardServiceable = rptPartHistoryBinCardServiceableUnserviceableList.GetPartHistoryBinCardServiceableUnserviceableList( _
        CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), ParameterValues("CustomerID"), _
        ParameterValues("StoreID"), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 0, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))

        mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt = rptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt.GetPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt( _
       CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), New Guid(ParameterValues("CustomerID").ToString), _
       New Guid(ParameterValues("StoreID").ToString), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 0, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))


        Dim ds As New dsPartHistoryBinCardServiceableUnserviceableList
       
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", mPartName, mAlternatePartNo, _
                                                              mLocation, mmaxstocklevel, mminstocklevel, mreorder, mUnitName, lbltextEssentialcategory.Text, mDescription, _
                                                              mRelNoteNo, 0, mApplicability, lblItemCategory.Text, lbltextBalQty.Text, AppSettings("Logo"), _
                                                              mStorageLife, mScrapLife, CStr(ParameterValues("SerialNo")), mBinCardNumber, _
                                                              Search5:=IIf(lblOnetimepurchase.Visible = True, lblOnetimepurchase.Text, ""), _
                                                              Search6:=lblItemNotInUseDate.Text, Search7:=lblItemNotInUseNote.Text)

        If mPartHistoryBinCardServiceable.Count <= 0 And mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If mPartHistoryBinCardServiceable.Count > 0 Then
            myReport = New crptStockCardExpendable
        Else
            myReport = New crptStockCardExpendableForOpenReceipt
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        If IsExcel = False Then
            da.Fill(ds, mrptImage)
        End If
        da.Fill(ds, mPartHistoryBinCardServiceable)
        da.Fill(ds, mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        'ResetValues()
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = IIf(mStockCardExpendablelist.Count = 0 And mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt.Count = 0, False, True)
        upnlActionBtns.Update()
    End Sub
    Private Sub setValue()
        mPartName = lbltextPartNo.Text
        mDescription = lbltextDescription.Text
        mLocation = lbltextLocation.Text
        Dim str1 As String = ""
        If mAlternatePartNumbers.Count <> 0 Then
            For i As Integer = 0 To mAlternatePartNumbers.Count - 1
                If i = 0 Then
                    If mAlternatePartNumbers(i).IPCReference = "" Then
                        mAlternatePartNo = mAlternatePartNo + mAlternatePartNumbers(i).PartName
                    Else
                        mAlternatePartNo = "<b>" & mAlternatePartNo + mAlternatePartNumbers(i).PartName & "</b>"
                    End If
                Else
                    If mAlternatePartNumbers(i).IPCReference = "" Then
                        mAlternatePartNo = mAlternatePartNo + ", " + mAlternatePartNumbers(i).PartName
                    Else
                        'str1 = "<b><span style=""background-color:#FFFF00;"">" & mAlternatePartNumbers(i).PartName & "</span></b>"
                        str1 = "<b>" & mAlternatePartNumbers(i).PartName & "</b>"
                        mAlternatePartNo = mAlternatePartNo + ", " + str1
                    End If
                End If
            Next
        End If
        mApplicability = lblApplicabilityList.Text
        mUnitName = lbltextUnit.Text
        mminstocklevel = lbltextMinStockLevel.Text
        mmaxstocklevel = lbltextMaxStockLevel.Text
        mreorder = lbltextReOrderLevel.Text
        mordqty = "" 'lbltextOrderQty.Text
        mScrapLife = lbltextScrapLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3
        mStorageLife = lbltextStorageLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3
        mBinCardNumber = lblBinCardNumber.Text
    End Sub
    'Added By Vikrant On 01-Apr-2016
    Private Sub setGridHeaders()
        Dim Report As New ReportData("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", )
        Dim BaseCurrencySymbol As String = Report.CurrencySymbol

        gdvStockCard.Columns(10).HeaderText = "Purchase Price(" + BaseCurrencySymbol + ")"
        gdvStockCard.Columns(11).HeaderText = "OH/RPR Cost(" + BaseCurrencySymbol + ")"
    End Sub
    'End
    Private Function CountOf() As Boolean
        mrptDueCalibrationList = rptDueCalibrationList.GetrptDueCalibrationList(, Date.Today.ToString("dd-MMM-yyyy"),
                                                                                   CStr(ParameterValues("PartNo")),
                                                                                  CStr(ParameterValues("Description")), "",
           "{00000000-0000-0000-0000-000000000000}", New SmartDate("1/1/3300").FormattedText, "{00000000-0000-0000-0000-000000000000}",
            False, 0, 0)

        mrptExpiryDate = rptExpiryDate.GetExpiryDate(Today.Date.ToString, CStr(ParameterValues("PartNo")),
                                                     CStr(ParameterValues("Description")), "", "",
                                                     "", 2, Today.Date.ToString) '2 menas 0 Days - 3 Month 
        If mrptDueCalibrationList.Count > 0 Then
            For v As Integer = 0 To mrptDueCalibrationList.Count - 1
                If mrptDueCalibrationList(v).RemainingDays <= 0 Then
                    CountOfExpired = CountOfExpired + 1
                End If
            Next
        End If
        If mrptExpiryDate.Count > 0 Then
            For v As Integer = 0 To mrptExpiryDate.Count - 1
                If mrptExpiryDate(v).DateDifference <= 0 Then
                    CountOfExpired = CountOfExpired + 1
                End If
            Next
        End If
        If CountOfExpired > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then

                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then

                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Status" Then
                        MarkLog(Util.Action.View, "PartBinCardHistory", "Message For Over Due of Calibration Item and Expired Item Showed", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                    End If
            End Select
        End If
    End Sub
#End Region

#Region "DataBinding"
    Private Sub DataFieldBinding()
        mStockCardExpendablelist = rptPartHistoryBinCardServiceableUnserviceableList.GetPartHistoryBinCardServiceableUnserviceableList( _
        CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), New Guid(ParameterValues("CustomerID").ToString), _
          New Guid(ParameterValues("StoreID").ToString), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 0, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))
        gdvStockCard.DataSource = mStockCardExpendablelist
        setGridHeaders()

        DataBind()
        Session("mStockCardExpendablelist") = mStockCardExpendablelist
        upnlStockCardGrid.Update()
    End Sub
    Private Sub DataFieldBindingForOpenReceipt()
        mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt = rptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt.GetPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt( _
        CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), New Guid(ParameterValues("CustomerID").ToString), _
        New Guid(ParameterValues("StoreID").ToString), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 0, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))
        dgOpenReceipt.DataSource = mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt
        dgOpenReceipt.DataBind()
        upnlOpenReceipt.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            DataFieldBinding()
            DataFieldBindingForOpenReceipt()
            ControlVisibility()
            ShowPartInfo()
            If CountOf() = True Then
                MSGBoxCtrl.Show("Alert!", "Some Of Calibration Items Serial number Overdue Or Expired.", "", MsgBoxStyle.OkOnly, "Status")
            End If
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mStockCardExpendablelist")
        Session.Remove("ParameterValues")
        Session.Remove("mAlternatePartNumbers")
        Session.Remove("ModuleName")
        Session("MiddleFrame") = "wfrptPartHistory_Ajax.aspx?Type=" & Session("mType").ToString
        Response.Redirect("index.aspx")
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        SetReport(False) ''Added By Shweta on 7-Nov-2012 for ALL07112012
    End Sub
    Private Sub gdvStockCard_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvStockCard.RowCommand
        Select Case e.CommandName
            Case "IssueReference"
                Dim index As Integer = CInt(e.CommandArgument) + gdvStockCard.PageIndex * gdvStockCard.PageSize
                Dim mID As Guid = mStockCardExpendablelist(index).IssueID
                mIssue = Issue.GetIssue(mID)
                Session("mIssue") = mIssue
                If mIssue.TransTypeID = 14 Then
                    If mIssue.ToTypeID = 18 Then
                        Session("ModuleName") = "Issue To Aircraft As Requisition"
                    Else
                        Session("ModuleName") = "Issue To Aircraft"
                    End If
                ElseIf mIssue.TransTypeID = 44 Then
                    If mIssue.ToTypeID = 18 Then
                        Session("ModuleName") = "Issue To WorkShop As Requisition"
                    Else
                        Session("ModuleName") = "Issue To WorkShop"
                    End If
                Else
                    Session("ModuleName") = TransactionList.GetTransactionList().GetTransactionTypeName(mIssue.TransTypeID).ToString 'mTransTypeList.GetTransactionTypeName(mIssue.TransTypeID).ToString
                End If
                gdvStockCard.DataSource = mStockCardExpendablelist
                gdvStockCard.DataBind()
                Dim Str As String
                Str = "OpenIssueWindow();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenIssueWindow", Str, True)
            Case "GRNReference"
                Dim index As Integer = CInt(e.CommandArgument) + gdvStockCard.PageIndex * gdvStockCard.PageSize
                Dim mReceiptID As Guid = mStockCardExpendablelist(index).ReceiptID
                Dim mInvoiceID As Guid = mStockCardExpendablelist(index).InvoiceID
                Dim Str As String
                If (mStockCardExpendablelist(index).ReceiptTransTypeID = 6 Or (mStockCardExpendablelist(index).ReceiptTransTypeID = 10 And mStockCardExpendablelist(index).FromTypeID = 1)) Then
                    mReceipt = Receipt.GetReceipt(mReceiptID)
                    Session("mReceipt") = mReceipt
                    Str = "OpenReceipt1Window();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenReceipt1Window", Str, True)
                Else
                    mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptID, mInvoiceID)
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                    Str = "OpenReceiptWindow();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenReceiptWindow", Str, True)
                End If
                gdvStockCard.DataSource = mStockCardExpendablelist
                gdvStockCard.DataBind()
                'Added By Vikrant On 30-May-2016 For ALL30052016
            Case "Show"
                Dim index As Integer = CInt(e.CommandArgument) + gdvStockCard.PageIndex * gdvStockCard.PageSize
                gdvStockCard.DataSource = mStockCardExpendablelist
                gdvStockCard.DataBind()

                Dim OrderItemIDFromIssueItem As Guid = mStockCardExpendablelist(index).OrderItemIDFromIssueItem
                Dim IssueItemIDFromIssueItem As Guid = mStockCardExpendablelist(index).IssueItemID

                For i As Integer = 0 To gdvStockCard.Rows.Count - 1
                    Dim Cond1 As Boolean = Not OrderItemIDFromIssueItem.Equals(Guid.Empty) And OrderItemIDFromIssueItem.Equals(mStockCardExpendablelist(i).OrderItemIDFromReceiptItem)
                    Dim Cond2 As Boolean = Not IssueItemIDFromIssueItem.Equals(Guid.Empty) And IssueItemIDFromIssueItem.Equals(mStockCardExpendablelist(i).IssueItemIDFromReceipt)
                    Dim Cond3 As Boolean = (index = i) 'Highlight Row which is clicked
                    If (Cond1 Or Cond2 Or Cond3) Then
                        gdvStockCard.Rows(i).BackColor = Color.FromArgb(255, 203, 96)
                    End If
                Next
                'End
        End Select
    End Sub
    Private Sub gdvStockCard_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvStockCard.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Cells(19).ToolTip = "Click to highlight related transactions"
            If (CDbl(e.Row.Cells(20).Text) > 0 And e.Row.Cells(21).Text = "True") Then
                e.Row.Cells(13).Font.Bold = True 'Issue Date
                e.Row.Cells(13).BackColor = Color.FromArgb(128, 128, 255)   'Issue Date
            ElseIf (CInt(e.Row.Cells(22).Text) = 1) Then
                e.Row.Cells(13).Font.Bold = True 'Issue Date
                e.Row.Cells(13).BackColor = Color.FromArgb(0, 92, 184)   'Issue Date
            ElseIf (CDbl(e.Row.Cells(23).Text) = 0 And e.Row.Cells(24).Text <> "" And e.Row.Cells(24).Text <> "&nbsp;" And e.Row.Cells(25).Text = "True" And CInt(e.Row.Cells(26).Text) = 10 And e.Row.Cells(27).Text = "True") Then    'EROQty=0 and Receipt item remark <>"" and IsConsiderAsAsset=True and IsConvertToOutright=True then magenta Colour. 
                e.Row.Cells(8).Font.Bold = True 'Order No
                e.Row.Cells(8).BackColor = Color.FromArgb(255, 0, 255) 'Order No
            ElseIf (CDbl(e.Row.Cells(30).Text) = 0 And CDbl(e.Row.Cells(28).Text) > 0 And CInt(e.Row.Cells(29).Text) = 31) Then
                e.Row.Cells(13).Font.Bold = True 'Issue Date 'advance core-return orders OrderItemEROQty EROQty = 0 AND  OrderItemReceiptBalanceQty>0 
                e.Row.Cells(13).BackColor = Color.FromArgb(128, 0, 0) 'Issue Date     Maroon                   'Added By Prashant 7-Aug-2018 
            ElseIf (CDbl(e.Row.Cells(30).Text) > 0 And CInt(e.Row.Cells(31).Text) = 31) Then 'OrderItemEROQty > 0 Purchase order for Exchange TransTypeID=31
                e.Row.Cells(8).Font.Bold = True 'Pending To Issue For Exchange (Core Unit Return) (Order No. Column)
                e.Row.Cells(8).BackColor = Color.FromArgb(255, 192, 130) 'Order No    Orange
            ElseIf (CDbl(e.Row.Cells(32).Text) > 0) Then 'AircraftRemovedQty > 0 TransTye ID 9
                e.Row.Cells(1).Font.Bold = True 'Pending to Issue to aircraft against receipt from aircraft (Source Column)
                e.Row.Cells(1).ForeColor = Color.White
                e.Row.Cells(1).BackColor = Color.FromArgb(0, 0, 153) '(Receipt Date)    Dark Blue
            ElseIf (e.Row.Cells(34).Text = "19") Then   'Issue to discard
                e.Row.Cells(13).Font.Bold = True 'Issue Date
                e.Row.Cells(13).BackColor = Color.FromArgb(192, 192, 192)   'Issue Date 'Gray  Shade
                e.Row.Cells(13).ForeColor = Color.White
            ElseIf (e.Row.Cells(34).Text = "25") Then   'Issue To Customer as Sales
                e.Row.Cells(13).Font.Bold = True 'Issue Date
                e.Row.Cells(13).BackColor = Color.Yellow    'Issue Date 'Yellow
                e.Row.Cells(13).ForeColor = Color.Black
            ElseIf (e.Row.Cells(35).Text = "True") Then   'Capitalized Issue
                e.Row.Cells(13).Font.Bold = True 'Issue Date
                e.Row.Cells(13).BackColor = Color.FromArgb(0, 128, 0)   'Issue Date 'Dark Green  Shade
                e.Row.Cells(13).ForeColor = Color.White
            ElseIf (e.Row.Cells(36).Text = "True") Then 'Receipt marked as asset
                e.Row.Cells(1).Font.Bold = True '(Receipt Date)
                e.Row.Cells(1).ForeColor = Color.Black
                e.Row.Cells(1).BackColor = Color.FromArgb(0, 255, 128) '(Receipt Date Column)    Parot colour i.e green
            End If
            If (e.Row.Cells(33).Text = "True") Then
                e.Row.Cells(0).Font.Bold = True 'Not In Use (Part No)
                e.Row.Cells(0).ForeColor = Color.White
                e.Row.Cells(0).BackColor = Color.FromArgb(255, 0, 0) '(Part No)    Red
            End If
        End If
    End Sub
    Private Sub gdvStockCard_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvStockCard.Sorting
        mStockCardExpendablelist.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        gdvStockCard.DataSource = mStockCardExpendablelist
        Session("mStockCardExpendablelist") = mStockCardExpendablelist
        gdvStockCard.DataBind()
    End Sub
    Private Sub gdvAlternate_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvAlternate.PageIndexChanging
        gdvAlternate.PageIndex = e.NewPageIndex
        gdvAlternate.DataSource = mAlternatePartNumbers
        gdvAlternate.DataBind()
    End Sub
    Private Sub gdvAlternate_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvAlternate.RowCommand
        Select Case e.CommandName
            Case "Attach"
                Dim index As Integer = CInt(e.CommandArgument) + gdvAlternate.PageIndex * gdvAlternate.PageSize
                Dim mId As Guid = mAlternatePartNumbers(index).AlternatePartID
                Dim mPartNo As String = mAlternatePartNumbers(index).PartName
                Dim mDescription As String = mAlternatePartNumbers(index).PartDescription
                ParameterValues.Remove("PartNo")
                ParameterValues.Remove("PartID")
                ParameterValues.Remove("Description")
                ParameterValues.Add("PartNo", mPartNo)
                ParameterValues.Add("PartID", mId)
                ParameterValues.Add("Description", mDescription)
                Session("ParameterValues") = ParameterValues
                DataFieldBinding()
                ShowPartInfo(True)
                DataFieldBindingForOpenReceipt()
                ControlVisibility()
                gdvAlternate.DataSource = mAlternatePartNumbers
                gdvAlternate.DataBind()
                If CountOf() = True Then
                    MSGBoxCtrl.Show("Alert!", "Some Of Calibration Items Serial number Overdue Or Expired.", "", MsgBoxStyle.OkOnly, "Status")
                End If
        End Select
    End Sub
    Private Sub btnDummyIssue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDummyIssue.Click
        Session.Remove("mIssue")
        DataFieldBinding()
    End Sub
    Private Sub btnDummyReceipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDummyReceipt.Click
        Session.Remove("mReceiptCumInvoice")
        DataFieldBinding()
    End Sub
    Private Sub gdvAlternate_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvAlternate.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If (e.Row.Cells(3).Text <> "&nbsp;") Then
                e.Row.Cells(1).Font.Bold = True
                e.Row.Cells(1).BackColor = Color.Yellow
            End If
        End If
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
         Dim mFileAttachments As New FileAttachments
        mFileAttachments = FileAttachments.GetChildFileAttachments(New Guid(ParameterValues("PartID").ToString))
        Session("mFileAttachments") = mFileAttachments
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)
        DataFieldBinding()
        gdvAlternate.DataSource = mAlternatePartNumbers
        gdvAlternate.DataBind()
    End Sub
    Private Sub btnCreateRequisition_Click(sender As Object, e As System.EventArgs) Handles btnCreateRequisition.Click
        mRequisitionNew = RequisitionNew.NewRequisition(65)
        mRequisitionNew.ReqDate = Today.Date
        Session("TransTypeID") = 65
        TransTypeID = 65

        'ItemID = Session("ItemID")

        mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, mRequisitionNew.WorkShopID)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = New Guid(ParameterValues("PartID").ToString)
        mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = CStr(ParameterValues("PartNo"))
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = CStr(ParameterValues("Description"))

        mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = CType(Session("mItemFromStockCardExpendable"), Item).UnitID        'Added By Prashant On 07-May-2019 BA07052019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = CType(Session("mItemFromStockCardExpendable"), Item).UnitName        'Added By Prashant On 07-May-2019 BA07052019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = CType(Session("mItemFromStockCardExpendable"), Item).IsOneTimePurchase
        If Not CType(Session("mItemFromStockCardExpendable"), Item).IsOneTimePurchase Then
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = CType(Session("mItemFromStockCardExpendable"), Item).MinStockLevel
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = CType(Session("mItemFromStockCardExpendable"), Item).MaxStockLevel
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = CType(Session("mItemFromStockCardExpendable"), Item).MinReOrderLevel
        Else
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
        End If
        Session("OpenFromPartNoBinCard") = "OpenFromPartNoBinCard"
        Session("mRequisitionNew") = mRequisitionNew

        Session("MiddleFrame") = "wfRequisitionList_Ajax.aspx?TransTypeID=65"

        Dim str As String
        str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnShowPartStatus_Click(sender As Object, e As System.EventArgs) Handles btnShowPartStatus.Click

        gdvAlternate.DataSource = mAlternatePartNumbers
        gdvAlternate.DataBind()

        Dim mItemStatus As Item = Item.GetItem(New Guid(ParameterValues("PartID").ToString))
        Dim LinkID As Guid = mItemStatus.LinkID
        Dim Unit As String = mItemStatus.UnitName

        Dim mStockPartStatus As rptStockPartStatus = rptStockPartStatus.GetStockPartStatusList(LinkID)
        Dim mOnOrderPartStatus As rptOnOrderPartStatus = rptOnOrderPartStatus.GetrptOnOrderPartStatusList(LinkID)
        Dim mReturnablePartStatus As rptReturnablePartStatus = rptReturnablePartStatus.GetrptReturnnablePartStatusList(LinkID)
        Dim mTransitPartList As rptTransitPartList = rptTransitPartList.GetTransitPartList(LinkID, Today.Date.ToShortDateString)
        Dim mRequisitionItemsNew As RequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForPartNoStatus(LinkID, AppSettings("ClientCode"))

        Session("PartNo") = lbltextPartNo.Text
        Session("Description") = lbltextDescription.Text
        Session("Unit") = Unit

        Session("mStockPartStatus") = mStockPartStatus
        Session("mOnOrderPartStatus") = mOnOrderPartStatus
        Session("mReturnablePartStatus") = mReturnablePartStatus
        Session("mTransitPartList") = mTransitPartList
        Session("mRequisitionItemsNewForPartNoStatus") = mRequisitionItemsNew
        Session("LinkID") = LinkID
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenShowPartNoStatusWindow", "OpenShowPartNoStatusWindow();", True)
    End Sub
    Private Sub dgOpenReceipt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgOpenReceipt.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If (CDbl(e.Row.Cells(12).Text) > 0 And e.Row.Cells(13).Text = "True" And CInt(e.Row.Cells(14).Text) = 38) Then 'OrderItemEROQty>0 ' IsOverhaul=True   TransTypeID=38
                e.Row.Cells(4).Font.Bold = True 'OrderNo
                e.Row.Cells(4).BackColor = Color.FromArgb(58, 143, 140)
                e.Row.Cells(4).ForeColor = Color.White
            End If
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class