'Created by Utkarsh on 04-Dec-2013
Public Class wfStockCard_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ParameterValues As Hashtable
    Dim mPartHistoryBinCardServiceable As rptPartHistoryBinCardServiceableUnserviceableList
    Dim mPartHistoryBinCardUnServiceable As rptPartHistoryBinCardServiceableUnserviceableList
    Dim mAlternatePartNumbers As AlternatePartNumbers
    'Added By Shweta on 7 Nov-2012 For ALL07112012
    Dim mPartName As String = ""
    Dim mDescription As String = ""
    Dim mStorageLife As String = ""
    Dim mCategoryname As String = ""
    Dim mLocation As String = ""
    Dim mTotQty As String = ""
    Dim mAlternatePartNo As String = ""
    Dim mATAChapter As String = ""
    Dim mStoreId As String = ""
    Dim mIsCustomerStore As Boolean
    Dim mIsValuedStore As Boolean
    Dim mPartStatusID As String
    ''
    Dim mApplicability As String = "" 'Added By Utkarsh ON 22-Apr-2013 FOR BA-22042013-1

    Dim mIssue As Issue
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Dim mReceipt As Receipt
    Dim mScrapLife As String = ""
    Public mSerialNo As String = ""
    Public mBinCardNumber, mCalibrationInterval, mCalibrationStandard, mWorkingRange, mEssentialcategory As String
    Public mCalibrationPeriodInList As CalibrationPeriodInList

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
        mPartHistoryBinCardServiceable = Session("mPartHistoryBinCardServiceable")
        mPartHistoryBinCardUnServiceable = Session("mPartHistoryBinCardUnServiceable")
        mAlternatePartNumbers = Session("mAlternatePartNumbers")
        mItem = Session("mItemFromStockCard")
    End Sub
    Private Sub ShowPartInfo(Optional ByVal FromAltPartGrid As Boolean = False)
        mItem = Item.GetItem(New Guid(ParameterValues("PartID").ToString))
        Session("mItemFromStockCard") = mItem
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
        lblItemNotInUseNote.Text = mItem.Note
        lbltextUnit.Text = mItem.UnitName
        mPartName = lbltextPartNo.Text
        lbltextDescription.Text = mItem.Description
        mDescription = lbltextDescription.Text
        lbltextStorageLife.Text = IIf(mItem.StorageLife = 0, "N/A", mItem.StorageLife & " Months") 'Added By Prashant 27-Oct-2014 ALL27102014-3
        mStorageLife = lbltextStorageLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3
        lbltextItemCategory.Text = Category.GetCategory(mItem.CategoryID).Name
        mCategoryname = lbltextItemCategory.Text
        lbltextLocation.Text = mItem.Location
        mLocation = lbltextLocation.Text
        lbltextScrapLife.Text = IIf(mItem.ExpiryMonths = 0, "N/A", mItem.ExpiryMonths & " Months")  'Added By Prashant 27-Oct-2014 ALL27102014-3
        mScrapLife = lbltextScrapLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3
        lblSerialNoList.Text = CStr(ParameterValues("SerialNo")) 'Added By Prashant 24-Dec-2014 ALL23122014-1
        mSerialNo = lblSerialNoList.Text
        lblBinCardNumber.Text = mItem.BinCardNumber
        mBinCardNumber = lblBinCardNumber.Text

        mCalibrationPeriodInList = CalibrationPeriodInList.GetCalibrationPeriodInList()
        lblCalibrationInterval.Text = Str(mItem.BenchmarkMonths) + " " + mCalibrationPeriodInList.Item(mItem.CalibrationPeriodInID, "").Name
        mCalibrationInterval = lblCalibrationInterval.Text
        lblCalibrationStandard.Text = mItem.CalibrationStandard
        mCalibrationStandard = mItem.CalibrationStandard
        lblWorkingRange.Text = mItem.Specification
        mWorkingRange = mItem.Specification
        If mItem.EssentialcategoryID = 0 Then 'Added By Prashant 20-Apr-2021 BA20042021
            lbltextEssentialcategory.Text = "GO"
        ElseIf mItem.EssentialcategoryID = 1 Then
            lbltextEssentialcategory.Text = "No-GO"
        ElseIf mItem.EssentialcategoryID = 2 Then
            lbltextEssentialcategory.Text = "GO-IF"
        Else
            lbltextEssentialcategory.Text = ""
        End If
        If Not mItem.ATAID.Equals(Guid.Empty) Then
            Dim mATA As ATA = ATA.GetATA(mItem.ATAID)
            lbltextATA.Text = mATA.ATACode & " - " & mATA.ATANomenclature
            mATAChapter = lbltextATA.Text
        End If
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

        Dim recieptqty As Decimal = 0.0
        Dim issueqty As Decimal = 0.0
        Dim RCIItemID1 As Guid = Guid.Empty
        Dim RCIItemID As Guid = Guid.Empty

        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
            'Added By Utkarsh ON 06-Jun-2013 FOR BA06062013 (TransTypeID=67)
            If (mItem.PrimaryCategoryID = 1 Or mItem.PrimaryCategoryID = 2) Then
                For i As Integer = 0 To mPartHistoryBinCardServiceable.Count - 1
                    ' If mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 6 Or _
                    ' mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 7 Or _
                    ' mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 22 Or _
                    '((mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 10) And mPartHistoryBinCardServiceable(i).IsConsiderAsAsset = True) Or _
                    '(mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 9 And mPartHistoryBinCardServiceable(i).IsConsiderAsAsset = True And mItem.PrimaryCategoryID = 1) Then
                    If mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 6 Or
                    mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 7 Or
                    mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 22 Or
                   ((mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 10) And mPartHistoryBinCardServiceable(i).IsConsiderAsAsset = True) Then
                        If Not RCIItemID1.Equals(mPartHistoryBinCardServiceable(i).RCIItemID) Then
                            RCIItemID1 = mPartHistoryBinCardServiceable(i).RCIItemID
                            recieptqty = recieptqty + mPartHistoryBinCardServiceable(i).ReceiptQty
                        End If
                    End If
                    If (mPartHistoryBinCardServiceable(i).IssueTransTypeID = 19 Or mPartHistoryBinCardServiceable(i).IssueTransTypeID = 25 Or ((mPartHistoryBinCardServiceable(i).IssueTransTypeID = 14 Or mPartHistoryBinCardServiceable(i).IssueTransTypeID = 44) And mPartHistoryBinCardServiceable(i).IsCapitalize = True)) Then
                        issueqty = issueqty + mPartHistoryBinCardServiceable(i).IssueQty
                    End If
                Next
                'Added By Utkarsh ON 06-Jun-2013 FOR BA06062013 (TransTypeID=67)
                For i As Integer = 0 To mPartHistoryBinCardUnServiceable.Count - 1
                    'If mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 6 Or _
                    'mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 7 Or _
                    'mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 22 Or _
                    '((mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 10) And mPartHistoryBinCardUnServiceable(i).IsConsiderAsAsset = True) Or _
                    '(mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 9 And mPartHistoryBinCardUnServiceable(i).IsConsiderAsAsset = True And mItem.PrimaryCategoryID = 1) Then
                    If mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 6 Or
                    mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 7 Or
                    mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 22 Or
                    ((mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 10) And mPartHistoryBinCardUnServiceable(i).IsConsiderAsAsset = True) Then
                        If Not RCIItemID.Equals(mPartHistoryBinCardUnServiceable(i).RCIItemID) Then
                            RCIItemID = mPartHistoryBinCardUnServiceable(i).RCIItemID
                            recieptqty = recieptqty + mPartHistoryBinCardUnServiceable(i).ReceiptQty
                        End If
                    End If
                    If (mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 19 Or mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 25 Or ((mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 14 Or mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 44) And mPartHistoryBinCardUnServiceable(i).IsCapitalize = True)) Then
                        issueqty = issueqty + mPartHistoryBinCardUnServiceable(i).IssueQty
                    End If
                Next
            Else
                For i As Integer = 0 To mPartHistoryBinCardServiceable.Count - 1
                    ' If mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 6 Or _
                    ' mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 7 Or _
                    ' mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 22 Or _
                    '((mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 10 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 9 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 66) And mPartHistoryBinCardServiceable(i).IsConsiderAsAsset = True) Or _
                    ' mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 21 Then
                    If mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 6 Or
                    mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 7 Or
                    mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 22 Or
                   ((mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 10 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 66) And mPartHistoryBinCardServiceable(i).IsConsiderAsAsset = True) Or
                    (mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 21 Or mPartHistoryBinCardServiceable(i).ReceiptTransTypeID = 9) Then
                        If Not RCIItemID1.Equals(mPartHistoryBinCardServiceable(i).RCIItemID) Then
                            RCIItemID1 = mPartHistoryBinCardServiceable(i).RCIItemID
                            recieptqty = recieptqty + mPartHistoryBinCardServiceable(i).ReceiptQty
                        End If
                    End If
                    If (mPartHistoryBinCardServiceable(i).IssueTransTypeID = 19 Or mPartHistoryBinCardServiceable(i).IssueTransTypeID = 25 Or ((mPartHistoryBinCardServiceable(i).IssueTransTypeID = 14 Or mPartHistoryBinCardServiceable(i).IssueTransTypeID = 44) And mPartHistoryBinCardServiceable(i).IsCapitalize = True) Or ((mPartHistoryBinCardServiceable(i).IssueTransTypeID = 16 Or mPartHistoryBinCardServiceable(i).IssueTransTypeID = 14) And mPartHistoryBinCardServiceable(i).IsReduceFromAsset = True)) Then
                        issueqty = issueqty + mPartHistoryBinCardServiceable(i).IssueQty
                    End If
                Next
                'Added By Utkarsh ON 06-Jun-2013 FOR BA06062013 (TransTypeID=67)
                For i As Integer = 0 To mPartHistoryBinCardUnServiceable.Count - 1
                    'If mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 6 Or _
                    'mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 7 Or _
                    'mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 22 Or _
                    '((mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 10 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 9 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 66) And mPartHistoryBinCardUnServiceable(i).IsConsiderAsAsset = True) Or _
                    'mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 21 Then
                    If mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 6 Or
                    mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 7 Or
                    mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 22 Or
                    ((mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 67 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 10 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 66) And mPartHistoryBinCardUnServiceable(i).IsConsiderAsAsset = True) Or
                    (mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 21 Or mPartHistoryBinCardUnServiceable(i).ReceiptTransTypeID = 9) Then
                        If Not RCIItemID.Equals(mPartHistoryBinCardUnServiceable(i).RCIItemID) Then
                            RCIItemID = mPartHistoryBinCardUnServiceable(i).RCIItemID
                            recieptqty = recieptqty + mPartHistoryBinCardUnServiceable(i).ReceiptQty
                        End If
                    End If
                    If (mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 19 Or mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 25 Or ((mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 14 Or mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 44) And mPartHistoryBinCardUnServiceable(i).IsCapitalize = True) Or ((mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 16 Or mPartHistoryBinCardUnServiceable(i).IssueTransTypeID = 14) And mPartHistoryBinCardUnServiceable(i).IsReduceFromAsset = True)) Then
                        issueqty = issueqty + mPartHistoryBinCardUnServiceable(i).IssueQty
                    End If
                Next
            End If
        Else  'Added By Prashant 11-Mar-2016  'Texel Air   CE11032016  Show other clients StockQty
            For i As Integer = 0 To mPartHistoryBinCardServiceable.Count - 1
                If Not RCIItemID1.Equals(mPartHistoryBinCardServiceable(i).RCIItemID) Then
                    RCIItemID1 = mPartHistoryBinCardServiceable(i).RCIItemID
                    recieptqty = recieptqty + mPartHistoryBinCardServiceable(i).ReceiptQty
                End If
                If mPartHistoryBinCardServiceable(i).IssueStatusID = 2 Then
                    issueqty = issueqty + mPartHistoryBinCardServiceable(i).IssueQty
                End If
            Next
            'Added By Utkarsh ON 06-Jun-2013 FOR BA06062013 (TransTypeID=67)
            For i As Integer = 0 To mPartHistoryBinCardUnServiceable.Count - 1
                If Not RCIItemID.Equals(mPartHistoryBinCardUnServiceable(i).RCIItemID) Then
                    RCIItemID = mPartHistoryBinCardUnServiceable(i).RCIItemID
                    recieptqty = recieptqty + mPartHistoryBinCardUnServiceable(i).ReceiptQty
                End If
                If mPartHistoryBinCardUnServiceable(i).IssueStatusID = 2 Then
                    issueqty = issueqty + mPartHistoryBinCardUnServiceable(i).IssueQty
                End If
            Next
            lblTotalQty.Text = "Stock Qty. :"
        End If

        lbltextTotalQty.Text = IIf(recieptqty - issueqty > 0, Format(recieptqty - issueqty, "##0.##"), 0)
        mTotQty = lbltextTotalQty.Text
        If mItem.AttachmentCount > 0 Then
            ImageButton1.Visible = True
            lblView.Visible = True
        Else
            ImageButton1.Visible = False
            lblView.Visible = False
        End If
    End Sub
    'Added By Vikrant On 30-May-2016 For ALL30052016
    Private Sub GridBind(Optional ByVal IsServiceableGrid As Boolean = False, Optional ByVal IsUnServiceableGrid As Boolean = False)
        If IsServiceableGrid Then
            gdvPartHistoryServiceable.DataSource = mPartHistoryBinCardServiceable
            setGridHeaders()
            gdvPartHistoryServiceable.DataBind()
        End If
        If IsUnServiceableGrid Then
            gdvPartHistoryUnserviceable.DataSource = mPartHistoryBinCardUnServiceable
            setGridHeaders()
            gdvPartHistoryUnserviceable.DataBind()
        End If
    End Sub
    'End
    Private Sub SetReport(ByVal IsExcel As Boolean)  'Added By Shweta on 7-Nov-2012 for ALL07112012
        setValue()
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim myReport As Object
        Dim objsearch As rptSearchingCriteria
        Dim Value As String = ""
        Dim ReportName As String = ""
        mPartHistoryBinCardServiceable = rptPartHistoryBinCardServiceableUnserviceableList.GetPartHistoryBinCardServiceableUnserviceableList( _
        CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), ParameterValues("CustomerID"), _
        ParameterValues("StoreID"), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 0, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))
        Dim ds As New dsPartHistoryBinCardServiceableUnserviceableList

        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", mPartName, mAlternatePartNo, _
                                                              mLocation, mCategoryname, mATAChapter, mStorageLife, mTotQty, KitName:=lbltextEssentialcategory.Text, _
                                                              Description:=mDescription, RelNoteNo:=lbltextUnit.Text, TransTypeID:=0, _
                                                              FromStore:=mApplicability, WorkShop:=mScrapLife, _
                                                              WorkOrderText:="", WorkOrderNo:=AppSettings("Logo"), _
                                                              Search1:=IIf(lblOnetimepurchase.Visible = True, lblOnetimepurchase.Text, ""), _
                                                              Search2:=mItem.PrimaryCategoryID.ToString, _
                                                              Search3:=CStr(ParameterValues("SerialNo")), Search4:=mBinCardNumber, _
                                                              Search5:=AppSettings("ClientCode"), Search6:=lblItemNotInUseDate.Text, _
                                                              Search7:=lblItemNotInUseNote.Text, Search8:=lblCalibrationInterval.Text, _
                                                              Search9:=lblCalibrationStandard.Text, Search10:=lblWorkingRange.Text)

        mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt = rptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt.GetPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt( _
       CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), New Guid(ParameterValues("CustomerID").ToString), _
       New Guid(ParameterValues("StoreID").ToString), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 0, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))


        If mPartHistoryBinCardServiceable.Count <= 0 And mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt.Count <= 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If mPartHistoryBinCardServiceable.Count > 0 Then
            myReport = New crptPartHistoryBinCardServiceableUnserviceable  'crptPartHistoryBinCardServiceableUnserviceableList
        Else
            myReport = New crptPartHistoryBinCardServiceableUnserviceableForOpenReceipt
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
        With myReport
            If mrptPartHistoryOfForBinCardServiceableUnserviceableOrderReceipt.Count = 0 Then
                .Section4.SectionFormat.EnableSuppress = True
            End If
        End With
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
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
                        'str1 = "<div><span style=""background-color:#FFFF00;""><b>" & mAlternatePartNumbers(i).PartName & "</b></span></div>"
                        str1 = "<b>" & mAlternatePartNumbers(i).PartName & "</b>"
                        mAlternatePartNo = mAlternatePartNo + ", " + str1
                    End If
                End If
            Next
        End If
        mApplicability = lblApplicabilityList.Text
        mATAChapter = lbltextATA.Text
        mCategoryname = lbltextItemCategory.Text
        mStorageLife = lbltextStorageLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3
        mTotQty = lbltextTotalQty.Text
        mScrapLife = lbltextScrapLife.Text 'Added By Prashant 27-Oct-2014 ALL27102014-3
        mBinCardNumber = lblBinCardNumber.Text
    End Sub
    'Added By Vikrant On 01-Apr-2016
    Private Sub setGridHeaders()
        Dim Report As New ReportData("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", )
        Dim BaseCurrencySymbol As String = Report.CurrencySymbol

        gdvPartHistoryServiceable.Columns(8).HeaderText = "Purchase Price(" + BaseCurrencySymbol + ")"
        gdvPartHistoryServiceable.Columns(9).HeaderText = "OH/RPR Cost(" + BaseCurrencySymbol + ")"

        gdvPartHistoryUnserviceable.Columns(8).HeaderText = "Purchase Price(" + BaseCurrencySymbol + ")"
        gdvPartHistoryUnserviceable.Columns(9).HeaderText = "OH/RPR Cost(" + BaseCurrencySymbol + ")"
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
        mPartHistoryBinCardServiceable = rptPartHistoryBinCardServiceableUnserviceableList.GetPartHistoryBinCardServiceableUnserviceableList( _
        CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), New Guid(ParameterValues("CustomerID").ToString), _
        New Guid(ParameterValues("StoreID").ToString), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 1, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))

        mPartHistoryBinCardUnServiceable = rptPartHistoryBinCardServiceableUnserviceableList.GetPartHistoryBinCardServiceableUnserviceableList( _
       CStr(ParameterValues("PartNo")), CStr(ParameterValues("Description")), CStr(ParameterValues("ReleaseNoteNo")), ParameterValues("CustomerID"), _
       ParameterValues("StoreID"), CBool(ParameterValues("IsCustomerStore")), CBool(ParameterValues("IsValuedStore")), 2, CStr(ParameterValues("SerialNo")), CBool(ParameterValues("WithAlternateParts")))

        gdvPartHistoryServiceable.DataSource = mPartHistoryBinCardServiceable
        gdvPartHistoryUnserviceable.DataSource = mPartHistoryBinCardUnServiceable
        setGridHeaders()
        DataBind()

        upnlServiceableGrid.Update()
        upnlUnServiceableGrid.Update()
        Session("mPartHistoryBinCardServiceable") = mPartHistoryBinCardServiceable
        Session("mPartHistoryBinCardUnServiceable") = mPartHistoryBinCardUnServiceable
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
            ShowPartInfo()
            If CountOf() = True Then
                MSGBoxCtrl.Show("Alert!", "Some Of Calibration Items Serial number Overdue Or Expired.", "", MsgBoxStyle.OkOnly, "Status")
            End If
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mPartHistoryBinCardServiceable")
        Session.Remove("mPartHistoryBinCardUnServiceable")
        Session.Remove("ParameterValues")
        Session.Remove("mAlternatePartNumbers")
        Session("MiddleFrame") = "wfrptPartHistory_Ajax.aspx?Type=" & Session("mType").ToString
        Response.Redirect("index.aspx")
    End Sub
    Private Sub gdvPartHistoryServiceable_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvPartHistoryServiceable.RowCommand
        Select Case e.CommandName
            Case "IssueReference"
                Dim index As Integer = CInt(e.CommandArgument) + gdvPartHistoryServiceable.PageIndex * gdvPartHistoryServiceable.PageSize
                Dim mID As Guid = mPartHistoryBinCardServiceable(index).IssueID
                mIssue = Issue.GetIssue(mID)
                Session("mIssue") = mIssue
                GridBind(True, False)
                Session("IsOpenFromServiceableGrid") = "True"
                Dim Str As String
                Str = "OpenIssueWindow();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenIssueWindow", Str, True)
            Case "GRNReference"
                Dim index As Integer = CInt(e.CommandArgument) + gdvPartHistoryServiceable.PageIndex * gdvPartHistoryServiceable.PageSize
                Dim mReceiptID As Guid = mPartHistoryBinCardServiceable(index).ReceiptID
                Dim mInvoiceID As Guid = mPartHistoryBinCardServiceable(index).InvoiceID
                Dim Str As String
                If (mPartHistoryBinCardServiceable(index).ReceiptTransTypeID = 6 Or (mPartHistoryBinCardServiceable(index).ReceiptTransTypeID = 10 And mPartHistoryBinCardServiceable(index).FromTypeID = 1)) Then
                    mReceipt = Receipt.GetReceipt(mReceiptID)
                    Session("mReceipt") = mReceipt
                    Session("IsOpenFromServiceableGrid") = "True"
                    Str = "OpenReceipt1Window();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenReceipt1Window", Str, True)
                Else
                    mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptID, mInvoiceID)
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                    Session("IsOpenFromServiceableGrid") = "True"
                    Str = "OpenReceiptWindow();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenReceiptWindow", Str, True)
                End If
                GridBind(True, False)
                'Added By Vikrant On 30-May-2016 For ALL30052016
            Case "Show"
                Dim index As Integer = CInt(e.CommandArgument) + gdvPartHistoryServiceable.PageIndex * gdvPartHistoryServiceable.PageSize
                GridBind(True, True)

                Dim OrderItemIDFromIssueItem As Guid = mPartHistoryBinCardServiceable(index).OrderItemIDFromIssueItem
                Dim IssueItemIDFromIssueItem As Guid = mPartHistoryBinCardServiceable(index).IssueItemID

                For i As Integer = 0 To gdvPartHistoryServiceable.Rows.Count - 1
                    Dim Cond1 As Boolean = Not OrderItemIDFromIssueItem.Equals(Guid.Empty) And OrderItemIDFromIssueItem.Equals(mPartHistoryBinCardServiceable(i).OrderItemIDFromReceiptItem)
                    Dim Cond2 As Boolean = Not IssueItemIDFromIssueItem.Equals(Guid.Empty) And IssueItemIDFromIssueItem.Equals(mPartHistoryBinCardServiceable(i).IssueItemIDFromReceipt)
                    Dim Cond3 As Boolean = (index = i) 'Highlight Row which is clicked
                    If (Cond1 Or Cond2 Or Cond3) Then
                        gdvPartHistoryServiceable.Rows(i).BackColor = Color.FromArgb(255, 203, 96)
                    End If
                Next

                For i As Integer = 0 To gdvPartHistoryUnserviceable.Rows.Count - 1
                    Dim Cond1 As Boolean = Not OrderItemIDFromIssueItem.Equals(Guid.Empty) And OrderItemIDFromIssueItem.Equals(mPartHistoryBinCardUnServiceable(i).OrderItemIDFromReceiptItem)
                    Dim Cond2 As Boolean = Not IssueItemIDFromIssueItem.Equals(Guid.Empty) And IssueItemIDFromIssueItem.Equals(mPartHistoryBinCardUnServiceable(i).IssueItemIDFromReceipt)

                    If (Cond1 Or Cond2) Then
                        gdvPartHistoryUnserviceable.Rows(i).BackColor = Color.FromArgb(255, 203, 96)
                    End If
                Next
                upnlUnServiceableGrid.Update()
                'End
        End Select
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
    Private Sub gdvPartHistoryServiceable_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPartHistoryServiceable.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Cells(17).ToolTip = "Click to highlight related transactions"
            If (CDbl(e.Row.Cells(18).Text) > 0 And CInt(e.Row.Cells(19).Text) = 38) Then 'OrderItemReceiptBalanceQty ' OrderTransTypeID
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.Olive   'Issue Date
                e.Row.Cells(11).ForeColor = Color.White
            ElseIf (CDbl(e.Row.Cells(23).Text) > 0 And e.Row.Cells(24).Text = "True") Then 'tabIssueItemLoanQty 'IsReturnableFromAircraft
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(128, 128, 255)   'Issue Date
                e.Row.Cells(11).ForeColor = Color.White
            ElseIf (CInt(e.Row.Cells(25).Text) = 1) Then 'Issue Status
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(0, 92, 184)   'Issue Date
                e.Row.Cells(11).ForeColor = Color.White
            ElseIf (e.Row.Cells(33).Text = "19") Then   'Issue to discard
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(192, 192, 192)   'Issue Date 'Gray  Shade
                e.Row.Cells(11).ForeColor = Color.White
            ElseIf (e.Row.Cells(33).Text = "25") Then   'Issue To Customer as Sales
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.Yellow    'Issue Date 'Yellow
                e.Row.Cells(11).ForeColor = Color.Black
            ElseIf (e.Row.Cells(34).Text = "True") Then   'Capitalized Issue
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(0, 128, 0)   'Issue Date 'Dark Green  Shade
                e.Row.Cells(11).ForeColor = Color.White
            End If
            If (CDbl(e.Row.Cells(20).Text) > 0 And CInt(e.Row.Cells(21).Text) = 31) Then        'OrderItemEROQty EROQty > 0 Purchase order for Exchange TransTypeID=31
                e.Row.Cells(5).Font.Bold = True 'Pending To Issue For Exchange (Core Unit Return) (Order No. Column)
                e.Row.Cells(5).BackColor = Color.FromArgb(255, 192, 130) 'Order No  Orange shade
                e.Row.Cells(5).ForeColor = Color.White
            ElseIf (CDbl(e.Row.Cells(22).Text) > 0 And CInt(e.Row.Cells(21).Text) = 39) Then    'tabReceiptItemLoanQty LoanQty > 0 Rental lease order.
                e.Row.Cells(5).Font.Bold = True 'Order No
                e.Row.Cells(5).BackColor = Color.FromArgb(255, 170, 213) 'Order No
            ElseIf (CDbl(e.Row.Cells(26).Text) = 0 And e.Row.Cells(27).Text <> "" And e.Row.Cells(27).Text <> "&nbsp;" And e.Row.Cells(28).Text = "True" And CInt(e.Row.Cells(29).Text) = 10 And e.Row.Cells(30).Text = "True") Then    'EROQty=0 and Receipt item remark <>"" and IsConsiderAsAsset=True and IsConvertToOutright=True then magenta Colour. 
                e.Row.Cells(5).Font.Bold = True 'Order No
                e.Row.Cells(5).BackColor = Color.FromArgb(255, 0, 255) 'Order No
                e.Row.Cells(5).ForeColor = Color.White
            ElseIf (CDbl(e.Row.Cells(20).Text) = 0 And CDbl(e.Row.Cells(18).Text) > 0 And CInt(e.Row.Cells(19).Text) = 31) Then
                e.Row.Cells(11).Font.Bold = True 'Issue Date 'advance core-return orders OrderItemEROQty EROQty = 0 AND  OrderItemReceiptBalanceQty>0 
                e.Row.Cells(11).BackColor = Color.FromArgb(128, 0, 0) 'Issue Date                    'Added By Prashant BA26022018 26-Feb-2018 
                e.Row.Cells(11).ForeColor = Color.White
                e.Row.Cells(5).ForeColor = Color.White
            ElseIf (CDbl(e.Row.Cells(31).Text) > 0) Then 'AircraftRemovedQty > 0 TransTye ID 9
                e.Row.Cells(1).Font.Bold = True 'Pending to Issue to aircraft against receipt from aircraft (Receipt Date)
                e.Row.Cells(1).ForeColor = Color.White
                e.Row.Cells(1).BackColor = Color.FromArgb(0, 0, 153) '(Receipt Date Column)    Dark Blue
            ElseIf (e.Row.Cells(35).Text = "True") Then 'Receipt marked as asset
                e.Row.Cells(1).Font.Bold = True '(Receipt Date)
                e.Row.Cells(1).ForeColor = Color.Black
                e.Row.Cells(1).BackColor = Color.FromArgb(0, 255, 128) '(Receipt Date Column)    Parot colour i.e green
            End If
            If (e.Row.Cells(32).Text = "True") Then
                e.Row.Cells(0).Font.Bold = True 'Not In Use (Part No)
                e.Row.Cells(0).ForeColor = Color.White
                e.Row.Cells(0).BackColor = Color.FromArgb(255, 0, 0) '(Part No)    Red
            End If

        End If
    End Sub
    Private Sub gdvPartHistoryServiceable_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvPartHistoryServiceable.Sorting
        mPartHistoryBinCardServiceable.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartHistoryBinCardServiceable") = mPartHistoryBinCardServiceable
        GridBind(True, True)
        upnlUnServiceableGrid.Update()
    End Sub
    Private Sub gdvPartHistoryUnserviceable_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvPartHistoryUnserviceable.RowCommand
        Select Case e.CommandName
            Case "IssueReference"
                Dim index As Integer = CInt(e.CommandArgument) + gdvPartHistoryUnserviceable.PageIndex * gdvPartHistoryUnserviceable.PageSize
                Dim mID As Guid = mPartHistoryBinCardUnServiceable(index).IssueID
                mIssue = Issue.GetIssue(mID)
                Session("mIssue") = mIssue
                GridBind(False, True)
                Session("IsOpenFromServiceableGrid") = "False"
                Dim Str As String
                Str = "OpenIssueWindow();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenIssueWindow", Str, True)
            Case "GRNReference"
                Dim index As Integer = CInt(e.CommandArgument) + gdvPartHistoryUnserviceable.PageIndex * gdvPartHistoryUnserviceable.PageSize
                Dim mReceiptID As Guid = mPartHistoryBinCardUnServiceable(index).ReceiptID
                Dim mInvoiceID As Guid = mPartHistoryBinCardUnServiceable(index).InvoiceID
                Dim Str As String
                If (mPartHistoryBinCardUnServiceable(index).ReceiptTransTypeID = 6 Or (mPartHistoryBinCardUnServiceable(index).ReceiptTransTypeID = 10 And mPartHistoryBinCardUnServiceable(index).FromTypeID = 1)) Then
                    mReceipt = Receipt.GetReceipt(mReceiptID)
                    Session("mReceipt") = mReceipt
                    Session("IsOpenFromServiceableGrid") = "False"
                    Str = "OpenReceipt1Window();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenReceipt1Window", Str, True)
                Else
                    mReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(mReceiptID, mInvoiceID)
                    Session("mReceiptCumInvoice") = mReceiptCumInvoice
                    Session("IsOpenFromServiceableGrid") = "False"
                    Str = "OpenReceiptWindow();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenReceiptWindow", Str, True)
                End If
                GridBind(False, True)
                'Added By Vikrant On 30-May-2016 For ALL30052016
            Case "Show"
                Dim index As Integer = CInt(e.CommandArgument) + gdvPartHistoryUnserviceable.PageIndex * gdvPartHistoryUnserviceable.PageSize
                GridBind(True, True)

                Dim OrderItemIDFromIssueItem As Guid = mPartHistoryBinCardUnServiceable(index).OrderItemIDFromIssueItem
                Dim IssueItemIDFromIssueItem As Guid = mPartHistoryBinCardUnServiceable(index).IssueItemID

                For i As Integer = 0 To gdvPartHistoryServiceable.Rows.Count - 1
                    Dim Cond1 As Boolean = Not OrderItemIDFromIssueItem.Equals(Guid.Empty) And OrderItemIDFromIssueItem.Equals(mPartHistoryBinCardServiceable(i).OrderItemIDFromReceiptItem)
                    Dim Cond2 As Boolean = Not IssueItemIDFromIssueItem.Equals(Guid.Empty) And IssueItemIDFromIssueItem.Equals(mPartHistoryBinCardServiceable(i).IssueItemIDFromReceipt)

                    If (Cond1 Or Cond2) Then
                        gdvPartHistoryServiceable.Rows(i).BackColor = Color.FromArgb(255, 203, 96)
                    End If
                Next

                For i As Integer = 0 To gdvPartHistoryUnserviceable.Rows.Count - 1
                    Dim Cond1 As Boolean = Not OrderItemIDFromIssueItem.Equals(Guid.Empty) And OrderItemIDFromIssueItem.Equals(mPartHistoryBinCardUnServiceable(i).OrderItemIDFromReceiptItem)
                    Dim Cond2 As Boolean = Not IssueItemIDFromIssueItem.Equals(Guid.Empty) And IssueItemIDFromIssueItem.Equals(mPartHistoryBinCardUnServiceable(i).IssueItemIDFromReceipt)
                    Dim Cond3 As Boolean = (index = i) 'Highlight Row which is clicked

                    If (Cond1 Or Cond2 Or Cond3) Then
                        gdvPartHistoryUnserviceable.Rows(i).BackColor = Color.FromArgb(255, 203, 96)
                    End If
                Next
                upnlServiceableGrid.Update()
                'End
        End Select
    End Sub
    Private Sub gdvPartHistoryUnserviceable_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPartHistoryUnserviceable.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Cells(18).ToolTip = "Click to highlight related transactions"
            If (CDbl(e.Row.Cells(19).Text) > 0 And CInt(e.Row.Cells(20).Text) = 38) Then 'OrderItemReceiptBalanceQty   OrderTransTypeID
                e.Row.Cells(11).Font.Bold = True            'Issue Date
                e.Row.Cells(11).BackColor = Color.Olive     'Issue Date
            ElseIf (CDbl(e.Row.Cells(24).Text) > 0 And e.Row.Cells(25).Text = "True") Then 'tabIssueItemLoanQty 'IsReturnableFromAircraft
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(128, 128, 255)   'Issue Date
            ElseIf (CInt(e.Row.Cells(26).Text) = 1) Then 'Issue Status
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(0, 92, 184)   'Issue Date
            ElseIf (e.Row.Cells(34).Text = "19") Then   'Issue to discard
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(192, 192, 192)   'Issue Date 'Gray  Shade
                e.Row.Cells(11).ForeColor = Color.White
            ElseIf (e.Row.Cells(34).Text = "25") Then   'Issue To Customer as Sales
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.Yellow    'Issue Date 'Yellow
                e.Row.Cells(11).ForeColor = Color.Black
            ElseIf (e.Row.Cells(35).Text = "True") Then   'Capitalized Issue
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(0, 128, 0)   'Issue Date 'Dark Green  Shade
                e.Row.Cells(11).ForeColor = Color.White
            End If
            If (CDbl(e.Row.Cells(22).Text) > 0 And CInt(e.Row.Cells(23).Text) = 31) Then 'OrderItemEROQty 'TransTypeID
                e.Row.Cells(5).Font.Bold = True 'Order No
                e.Row.Cells(5).BackColor = Color.FromArgb(255, 192, 130)   'Order No
            ElseIf (CDbl(e.Row.Cells(23).Text) > 0 And CInt(e.Row.Cells(22).Text) = 39) Then 'tabReceiptItemLoanQty TransTypeID LoanQty > 0 Rental lease order.
                e.Row.Cells(5).Font.Bold = True 'Order No
                e.Row.Cells(5).BackColor = Color.FromArgb(255, 170, 213) 'Order No
            ElseIf (CDbl(e.Row.Cells(27).Text) = 0 And e.Row.Cells(28).Text <> "" And e.Row.Cells(28).Text <> "&nbsp;" And e.Row.Cells(29).Text = "True" And CInt(e.Row.Cells(30).Text) = 10 And e.Row.Cells(31).Text = "True") Then    'EROQty=0 and Receipt item remark <>"" and IsConsiderAsAsset=True and IsConvertToOutright=True then magenta Colour. 
                e.Row.Cells(5).Font.Bold = True 'Order No
                e.Row.Cells(5).BackColor = Color.FromArgb(255, 0, 255) 'Order No
            ElseIf (CDbl(e.Row.Cells(22).Text) = 0 And CDbl(e.Row.Cells(19).Text) > 0 And CInt(e.Row.Cells(20).Text) = 31) Then        'advance core-return orders OrderItemEROQty EROQty = 0 AND  OrderItemReceiptBalanceQty>0 
                e.Row.Cells(11).Font.Bold = True 'Issue Date
                e.Row.Cells(11).BackColor = Color.FromArgb(128, 0, 0) 'Issue Date                   'Added By Prashant BA26022018 26-Feb-2018 
            ElseIf (CDbl(e.Row.Cells(32).Text) > 0) Then 'AircraftRemovedQty > 0 TransTye ID 9
                e.Row.Cells(1).Font.Bold = True 'Pending to Issue to aircraft against receipt from aircraft (Receipt Date)
                e.Row.Cells(1).ForeColor = Color.White
                e.Row.Cells(1).BackColor = Color.FromArgb(0, 0, 153) '(Receipt Date)    Dark Blue
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
    Private Sub gdvPartHistoryUnserviceable_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvPartHistoryUnserviceable.Sorting
        mPartHistoryBinCardUnServiceable.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartHistoryBinCardUnServiceable") = mPartHistoryBinCardUnServiceable
        GridBind(True, True)
        upnlServiceableGrid.Update()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        SetReport(False) 'Added By Shweta on 7-Nov-2012 for ALL07112012
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
                DataFieldBindingForOpenReceipt()
                ShowPartInfo(True)
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
    'Added By Vikrant On 30-May-2016 For ALL30052016
    Private Sub hdnBtnIssue_Click(sender As Object, e As System.EventArgs) Handles hdnBtnIssue.Click
        If Session("IsOpenFromServiceableGrid") = "True" Then 'Bind UnServi. Grid
            GridBind(False, True)
            upnlUnServiceableGrid.Update()
        Else 'Bind Servi. Grid
            GridBind(True, False)
            upnlServiceableGrid.Update()
        End If
    End Sub
    Private Sub hdnBtnReceipt_Click(sender As Object, e As System.EventArgs) Handles hdnBtnReceipt.Click
        If Session("IsOpenFromServiceableGrid") = "True" Then 'Bind UnServi. Grid
            GridBind(False, True)
            upnlUnServiceableGrid.Update()
        Else 'Bind Servi. Grid
            GridBind(True, False)
            upnlServiceableGrid.Update()
        End If
    End Sub
    Private Sub hdnBtnReceipt1_Click(sender As Object, e As System.EventArgs) Handles hdnBtnReceipt1.Click
        If Session("IsOpenFromServiceableGrid") = "True" Then 'Bind UnServi. Grid
            GridBind(False, True)
            upnlUnServiceableGrid.Update()
        Else 'Bind Servi. Grid
            GridBind(True, False)
            upnlServiceableGrid.Update()
        End If
    End Sub
    'End
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim mFileAttachments As New FileAttachments
        mFileAttachments = FileAttachments.GetChildFileAttachments(New Guid(ParameterValues("PartID").ToString))
        Session("mFileAttachments") = mFileAttachments
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)
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

        mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = CType(Session("mItemFromStockCard"), Item).UnitID        'Added By Prashant On 07-May-2019 BA07052019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = CType(Session("mItemFromStockCard"), Item).UnitName        'Added By Prashant On 07-May-2019 BA07052019
        mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = CType(Session("mItemFromStockCard"), Item).IsOneTimePurchase
        If Not CType(Session("mItemFromStockCard"), Item).IsOneTimePurchase Then
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = CType(Session("mItemFromStockCard"), Item).MinStockLevel
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = CType(Session("mItemFromStockCard"), Item).MaxStockLevel
            mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = CType(Session("mItemFromStockCard"), Item).MinReOrderLevel
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
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region


End Class