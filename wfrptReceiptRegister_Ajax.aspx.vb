Public Class wfrptReceiptRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim Fromdate As String = ""
    Dim ToDate As String = ""
    Dim RecText As String = ""
    Dim RecNo As String = ""
    Dim InternalReceiptNo As String = ""
    Dim Supplier As String = ""
    Dim Aircraft As String = ""
    Dim Store As String = ""
    Dim DCNo As String = ""
    Dim Status As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim OrdNo As String = ""
    Dim OrdText As String = ""
    Dim IssNo As String = ""
    Dim IssText As String = ""
    Dim ReleaseNoteNo As String = ""
    Public mVendor As Vendor
    Dim mItemList As ItemList
    Dim mVendorList As VendorList
    Dim mStoreList As StoreList
    Dim mOrderTextList As DistinctTextListForOrder
    Dim mReceiptTextList As DistinctTextListForReceipt
    Dim mIssueTextList As DistinctTextListForIssue
    Public Tital As String
    Dim mTransTypeID As Int16
    Dim mReceivingStoreList As StoreList
    Dim ReceivingStoreID As String
    Dim ReceivingStore As String
    Dim CustomBillofEntry As String = ""
    Public mPartTypeList As PartTypeList
    Dim mPartType As Integer
    Dim mPartTypeName As String = ""
    'Added By Utkarsh FOR ALL13122011
    Public Shared Type As String = ""
    Public Shared TextType As String = ""
    'End

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")

        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)

        mTransTypeID = CType(Session("mTransTypeID"), Int16)
        mPartTypeList = Session("mPartTypeList")
        Type = Session("Type")  'Added By Utkarsh On 21-Dec-2011 For ALL13122011
        TextType = Session("TextType") 'Added By Utkarsh On 21-Dec-2011 For ALL13122011
    End Sub

    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
        Session("mPartTypeList") = mPartTypeList

    End Sub

    Private Sub RemoveSession()

        Session.Remove("PartNo")
        Session.Remove("Description")

        Session.Remove("mPartTypeList")
        Session.Remove("Type")  'Added By Utkarsh On 21-Dec-2011 For ALL13122011
        Session.Remove("TextType") 'Added By Utkarsh On 21-Dec-2011 For ALL13122011
    End Sub

    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)

        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        If cmbFormat.Visible = True Then
            If cmbFormat.SelectedIndex = 0 Then
                chkDetail.Visible = True
                optPortrait.Visible = True
                optLandscape.Visible = True
            Else
                chkDetail.Visible = False
                optPortrait.Visible = False
                optLandscape.Visible = False
            End If
        End If
    End Sub

    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblOrderNo.Visible = True
        lblIntReceiptNo.Visible = True
        lblReleaseNoteno.Visible = True
        lblStatus1.Visible = True
        lblDCNo.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblReceivingStoreName.Visible = True
        lblTransactionType.Visible = True
        lblCustomBillofEntries.Visible = True
        lblPartType1.Visible = True

        upnlDisplaySearchCriteria.Update()
    End Sub

    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            Fromdate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & ")"
        End If

        'Commented and Added By Utkarsh On 14-Dec-2011
        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If

        Supplier = IIf(cmbType.SelectedIndex = 1, txtSupplier.Text.Trim, "")
        Aircraft = IIf(cmbType.SelectedIndex = 2, txtAircraft.Text.Trim, "")
        Store = IIf(cmbType.SelectedIndex = 3, cmbFromStore.SelectedItem.Text, "")
        RecText = IIf(cmbDocType.SelectedIndex = 1, txtReceiptText.Text.Trim, "")
        RecNo = IIf(cmbDocType.SelectedIndex = 1, txtNo.Text.Trim, "")
        OrdText = IIf(cmbDocType.SelectedIndex = 3, txtOrderText.Text.Trim, "")
        OrdNo = IIf(cmbDocType.SelectedIndex = 3, txtNo.Text.Trim, "")
        IssText = IIf(cmbDocType.SelectedIndex = 2, txtIssueText.Text.Trim, "")
        IssNo = IIf(cmbDocType.SelectedIndex = 2, txtNo.Text.Trim, "")
        'End

        ReleaseNoteNo = txtReleaseNoteNo.Text.Trim
        InternalReceiptNo = txtIntReceiptNo.Text.Trim
        DCNo = txtDCNo.Text.Trim
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")

        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        Session("PartNo") = PartNo
        Session("Description") = Description

        CustomBillofEntry = txtCustomBillofEntry.Text.Trim

        lblIntReceiptNo.Text = "Int. Receipt No.: " & IIf(InternalReceiptNo <> "", InternalReceiptNo, "All")
        lblReleaseNoteno.Text = "Release Note No.: " & IIf(ReleaseNoteNo <> "", ReleaseNoteNo, "All")
        lblStatus1.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblDCNo.Text = "D.C. No. : " & IIf(DCNo <> "", DCNo, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblCustomBillofEntries.Text = "Custom Bill of Entry : " & IIf(CustomBillofEntry <> "", CustomBillofEntry, "All")

        Select Case cmbType.SelectedIndex
            Case 0
                lblVendor.Text = "From Type  : All "
            Case 1
                lblVendor.Text = "Supplier : " & IIf(Supplier <> "", Supplier, "All")
            Case 2
                lblVendor.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
            Case 3
                lblVendor.Text = "Store : " & IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, "All")
        End Select

        Select Case cmbDocType.SelectedIndex
            Case 0
                lblOrderNo.Text = "Document Type : All "
            Case 1
                If RecText = "" Then
                    lblOrderNo.Text = "Receipt No : All "
                Else
                    lblOrderNo.Text = "Receipt No. : " + RecText + "-" + RecNo
                End If
            Case 2
                If IssText = "" Then
                    lblOrderNo.Text = "Issue No. : All "
                Else
                    lblOrderNo.Text = "Issue No. : " + IssText + "-" + IssNo
                End If
            Case 3
                If OrdText = "" Then
                    lblOrderNo.Text = "Order No. : All "
                Else
                    lblOrderNo.Text = "Order No. : " + OrdText + "-" + OrdNo
                End If
        End Select
        ReceivingStoreID = cmbReceivingStore.SelectedValue.ToString
        ReceivingStore = IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, "All")
        lblReceivingStoreName.Text = "Receiving Store : " & ReceivingStore
        lblTransactionType.Text = "Transaction Type :" + " " + cmbReceiptType.SelectedItem.Text
        mPartType = IIf(cmbPartType.SelectedIndex > 0, cmbPartType.SelectedValue, 0)
        mPartTypeName = IIf(cmbPartType.SelectedIndex > 0, cmbPartType.SelectedItem.Text, "")
        lblPartType1.Text = "Part Type : " + IIf(cmbPartType.SelectedIndex > 0, cmbPartType.SelectedItem.Text, "All")

        mCompleteSearchingCriteria = lblTransactionType.Text + ", " + lblDateRange.Text + ", " + _
            IIf(cmbDocType.SelectedIndex = 0, "All", cmbDocType.SelectedItem.Text) + ", " + lblOrderNo.Text + ", " + IIf(cmbType.SelectedIndex = 0, "All", cmbType.SelectedItem.Text) _
            + ", " + lblVendor.Text + ", " + lblIntReceiptNo.Text + ", " + lblReleaseNoteno.Text + ", " + lblDCNo.Text + ", " + lblCustomBillofEntries.Text + ", " + _
            lblReceivingStoreName.Text + ", " + lblStatus1.Text + ", " + " Format " + IIf(optLandscape.Checked, "LandScape", "Portrait") + ", " + lblPartType1.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub

    Public Sub SetReport(ByVal IsExcel As Boolean)
        'Session("IsExcel") = IsExcel

        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim objReg As rptReceiptReg
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsReceipt As New dsReceipt

        SetValues()
        Tital = GetTitle()
        If AppSettings("ClientCode") = "Taj" Then
           
        End If
        If cmbFormat.SelectedIndex = 0 Then ''Format 1
            If chkOnlyReceivedinSelectedStore.Checked = False Then
                If chkDetail.Checked Then
                    If optPortrait.Checked Then
                        myReport = New crptReceiptRegister
                    Else
                        myReport = New crptReceiptRegisterLandScape
                    End If
                    objReg = rptReceiptReg.GetRecepitList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Supplier, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, 1, CInt(cmbReceiptType.SelectedValue), ReceivingStoreID, CustomBillofEntry, mPartType)

                    objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, InternalReceiptNo, ReleaseNoteNo, RecText, IssText, OrdText, RecNo, IssNo, OrdNo, Aircraft, Supplier, IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, ""), Status, DCNo, PartNo, Description, mPartTypeName, "", "", Tital, IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), CustomBillofEntry, "", "", "", "", "", 0, "", "", AppSettings("Logo"))
                    If objReg.Count <= 0 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                        'msg1.ReplacePage = "wfrptReceiptRegister.aspx?Backpage="
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

                        Exit Sub
                    ElseIf objReg.Count > 0 Then
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 606)
                    End If
                    dsReceipt.Clear()

                    Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt)

                    da.Fill(dsReceipt, objReg)
                    da.Fill(dsReceipt, mrptImage)
                    da.Fill(dsReceipt, objSearch)

                    myReport.SetDataSource(dsReceipt)
                Else
                    If optPortrait.Checked Then
                        myReport = New crptReceiptRegSummary
                    Else
                        myReport = New crptReceiptRegSummaryLandscape
                    End If

                    objReg = rptReceiptReg.GetRecepitList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Supplier, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedIndex, CInt(cmbReceiptType.SelectedValue), ReceivingStoreID, CustomBillofEntry, mPartType)
                    objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, InternalReceiptNo, ReleaseNoteNo, RecText, IssText, OrdText, RecNo, IssNo, OrdNo, Aircraft, Supplier, IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, ""), Status, DCNo, PartNo, Description, mPartTypeName, "", "", Tital, IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), CustomBillofEntry, "", "", "", "", "", 0, "", "", AppSettings("Logo"))

                    If objReg.Count <= 0 Then
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                        'msg1.ReplacePage = "wfrptReceiptRegister.aspx?Backpage="
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                        Exit Sub

                    ElseIf objReg.Count > 0 Then
                        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 606)
                    End If

                    dsReceipt.Clear()

                    Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt)

                    da.Fill(dsReceipt, objReg)
                    da.Fill(dsReceipt, mrptImage)
                    da.Fill(dsReceipt, objSearch)

                    myReport.SetDataSource(dsReceipt)
                End If
            Else
                'myReport = New crptReceiptRegister
                myReport = New crptReceiptRegisterLandScape  'Added By Prashant 13-Aug-2010

                objReg = rptReceiptReg.GetRecepitList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Supplier, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, 1, CInt(cmbReceiptType.SelectedValue), ReceivingStoreID, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, InternalReceiptNo, ReleaseNoteNo, RecText, IssText, OrdText, RecNo, IssNo, OrdNo, Aircraft, Supplier, IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, ""), Status, DCNo, PartNo, Description, mPartTypeName, "", "", Tital, IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), CustomBillofEntry, "", "", "", "", "", 0, "", "", AppSettings("Logo"))

                If objReg.Count <= 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfrptReceiptRegister.aspx?Backpage="
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                dsReceipt.Clear()

                Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt)

                da.Fill(dsReceipt, objReg)
                da.Fill(dsReceipt, mrptImage)
                da.Fill(dsReceipt, objSearch)

                myReport.SetDataSource(dsReceipt)
            End If
        ElseIf cmbFormat.SelectedIndex = 1 Then ''Format 2
            myReport = New crptReceiptRegisterLandScapeFormat2
            objReg = rptReceiptReg.GetRecepitList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Supplier, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, _
                                                  OrdText, IssNo, IssText, ReleaseNoteNo, 1, CInt(cmbReceiptType.SelectedValue), ReceivingStoreID, CustomBillofEntry, mPartType, _
                                                  Format2:=2)
            objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, InternalReceiptNo, _
                                                                                      ReleaseNoteNo, RecText, IssText, OrdText, RecNo, IssNo, OrdNo, Aircraft, Supplier, _
                                                                                      IIf(cmbFromStore.SelectedIndex > 0, cmbFromStore.SelectedItem.Text, ""), Status, DCNo, _
                                                                                      PartNo, Description, mPartTypeName, "", "", Tital, IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), _
                                                                                      CustomBillofEntry, "", "", "", "", "", 0, "", "", AppSettings("Logo"))
            If objReg.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf objReg.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 606)
            End If
            dsReceipt.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt)
            da.Fill(dsReceipt, objReg)
            da.Fill(dsReceipt, mrptImage)
            da.Fill(dsReceipt, objSearch)
            myReport.SetDataSource(dsReceipt)
        End If
        Session("CrystalReport") = myReport
        If IsExcel Then
            Dim mExcelrptReceiptReg As ExcelrptReceiptReg
            mExcelrptReceiptReg = ExcelrptReceiptReg.GetExcelrptReceiptReg(FromDate:=Fromdate, ToDate:=ToDate, Text:=RecText, _
                                                                                             No:=RecNo, IntReceiptNo:=InternalReceiptNo, Name:=Supplier, _
                                                                                              DCNO:=DCNo, StatusID:=cmbStatus.SelectedValue, ItemName:=PartNo, _
                                                                                              Description:=Description, OrderNo:=OrdNo, OrderText:=OrdText, _
                                                                                              IssueNo:=IssNo, IssueText:=IssText, ReleaseNoteNo:=ReleaseNoteNo, Type:=CInt(cmbType.SelectedValue), _
                                                                                              TransTypeID:=(cmbReceiptType.SelectedValue), StoreID:=ReceivingStoreID, _
                                                                                              AWBNo:=CustomBillofEntry, ItemTypeID:=mPartType, _
                                                                                              OnlyReceivedinSelectedStore:=chkOnlyReceivedinSelectedStore.Checked)
            da.Fill(dsReceipt, mExcelrptReceiptReg)

            Dim columnToRemove2 As String() = {"CompanyName", "SupplierName", "BranchName", "Category", "Nomenclature", "Aircraft", "KitName", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "RelNoteNo"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If dsReceipt.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove2(i)) Then
                    dsReceipt.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"ReceiptText", "ReceiptNo", "OrderText", "OrderNo", "ReceiptItemID", "FromType", "OrderAmend"}
            For i As Integer = 0 To columnToRemove.Length - 1
                If dsReceipt.Tables("ExcelrptReceiptReg").Columns.Contains(columnToRemove(i)) Then
                    dsReceipt.Tables("ExcelrptReceiptReg").Columns.Remove(columnToRemove(i))
                End If
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(dsReceipt.Tables("rptSearchingCriteriaForReceipt"))
            dsNew.Merge(dsReceipt.Tables("ExcelrptReceiptReg"))

            dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("QuotationNo").ColumnName = "Receiving Store"

            dsNew.Tables("ExcelrptReceiptReg").Columns("DisplayQty").ColumnName = "Qty"
            dsNew.Tables("ExcelrptReceiptReg").Columns("DisplayUnitName").ColumnName = "Unit"
            dsNew.Tables("ExcelrptReceiptReg").Columns("ToStoreName").ColumnName = "Store"
            dsNew.Tables("ExcelrptReceiptReg").Columns("PartTypeName").ColumnName = "Part Type"

            dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptReceiptReg").TableName = "Receipt Register"
			Session("ExcelFileName") = "Receipt Register"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "ReceiptPOReg", "Export To Excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "ReceiptPOReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub

    Private Sub SetDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select

        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))

    End Sub

    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub

    Private Function GetTitle() As String

        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        Dim mTitle As String

        mTransTypeID = CInt(cmbReceiptType.SelectedValue)
        If cmbReceiptType.SelectedIndex <> 0 Then
            mTitle = mTransTypeList.GetTransactionTypeName(cmbReceiptType.SelectedValue).ToString + " Register"
        End If

        If chkDetail.Checked Then
            If mTitle = "" Then
                Return "Receipt Register (Detail Report)"
            Else
                Return mTitle + " (Detail Report)" '"Receipt-Cum-Invoice Register (Detail Report)"
            End If
        Else
            If mTitle = "" Then
                Return "Receipt Register (Summary Report)"
            Else
                Return mTitle + " (Summary Report)" '"Receipt-Cum-Invoice Register (Summary Report)"
            End If
        End If
        Return mTitle
    End Function

    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        mPartTypeList = PartTypeList.GetItemTypeList(True, "", "(All)")
        Session("mPartTypeList") = mPartTypeList

        cmbPartType.DataSource = mPartTypeList
        ''cmbPartType.DataBind()

        'Added By Prashant On 30-Apr-2013 For ALL29042013-4
        mStoreList = StoreList.GetStoreList(0, "", True, True)
        cmbFromStore.DataSource = mStoreList
        ''cmbFromStore.DataBind()

        cmbReceivingStore.DataSource = mStoreList
        ''cmbReceivingStore.DataBind()
        'End

        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"
        DataBind()
    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()

            If cmbReceiptType.Enabled = True Then
                SetFocus(cmbReceiptType)
            End If

            mTransTypeID = Request.QueryString("TransTypeId")
            Session("mTransTypeId") = mTransTypeID

            DataFieldBind()

            ControlVisibility(6)
            SetDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            chkOnlyReceivedinSelectedStore.Enabled = False
        End If
        lbltitle.Text = "Receipt Register - Receipt against Purchase Order"
    End Sub

    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        SetDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub

    'Private Sub txtFromDate_textChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
    '    Me.cmbDocType.Enabled = Not CType(sender, Boolean)
    '    Me.txtToDate.Enabled = Not CType(sender, Boolean)

    '    If cmbType.SelectedIndex = 1 Then         'Vendor
    '        txtSupplier.Visible = Not CType(sender, Boolean)
    '    End If

    '    If cmbDocType.SelectedIndex = 1 Then      'Receipt
    '        txtReceiptText.Visible = Not CType(sender, Boolean)
    '    ElseIf cmbDocType.SelectedIndex = 2 Then  'Issue
    '        txtIssueText.Visible = Not CType(sender, Boolean)
    '    ElseIf cmbDocType.SelectedIndex = 3 Then  'Order
    '        txtOrderText.Visible = Not CType(sender, Boolean)
    '    End If
    'End Sub

    Private Sub cmbDocType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDocType.SelectedIndexChanged

        txtNo.Text = ""
        txtReceiptText.Text = ""
        txtIssueText.Text = ""
        txtOrderText.Text = ""

        Dim Index As Int16 = IIf(cmbDocType.SelectedIndex > 0, cmbDocType.SelectedIndex, 0)
        lblDocTypeNo.Visible = (Index > 0)
        txtNo.Visible = (Index > 0)
        lblDocTypeNo.Text = IIf(Index = 0, "", IIf(Index = 1, "Receipt No.", IIf(Index = 2, "Issue No.", IIf(Index = 3, "Order No.", ""))))

        'Added By Utkarsh ON 14-Dec-2011 For ALL13122011
        txtReceiptText.Visible = (Index = 1)
        txtIssueText.Visible = (Index = 2)
        txtOrderText.Visible = (Index = 3)

        Type = IIf(Index > 0, "Text", Type)
        TextType = IIf(Index = 0, "0", IIf(Index = 1, "2", IIf(Index = 2, "3", IIf(Index = 3, "1", "0"))))

        Session("Type") = Type
        Session("TextType") = TextType

        hidden_DocType.Value = Type
        hidden_DocTextType.Value = TextType
        upnlHiddenFields.Update()

        txtNo.Visible = (Index > 0)
        'End

        If cmbDocType.Enabled = True Then
            SetFocus(cmbDocType)
        End If
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged

        txtNo.Text = ""
        txtSupplier.Text = ""
        txtAircraft.Text = ""
        cmbFromStore.SelectedIndex = 0 'Added By Prashant 29-Apr-2013 'ALL29042013-4

        Dim Index As Int16 = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedIndex, 0)
        lblType1.Visible = (Index > 0)
        lblType1.Text = IIf(Index = 0, "", IIf(Index = 1, "Supplier  ", IIf(Index = 2, "Aircraft  ", IIf(Index = 3, "Store  ", ""))))

        'Added By Utkarsh On 14-Dec-2011
        txtSupplier.Visible = (Index = 1)
        txtAircraft.Visible = (Index = 2)
        cmbFromStore.Visible = (Index = 3) 'Added By Prashant 29-Apr-2013 'ALL29042013-4

        Type = IIf(Index = 0, "", IIf(Index = 1, "Supplier", IIf(Index = 2, "Aircraft", IIf(Index = 3, "Store", ""))))
        Session("Type") = Type
        'End

        hidden_FromType.Value = Type
        upnlHiddenFields.Update()

        If cmbType.Enabled = True Then
            SetFocus(cmbType)
        End If
    End Sub

    Private Sub chkOnlyReceivedinSelectedStore_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOnlyReceivedinSelectedStore.CheckedChanged
        If chkOnlyReceivedinSelectedStore.Checked = True Then
            chkDetail.Enabled = False
            optPortrait.Enabled = False
            optLandscape.Enabled = False
        Else
            chkDetail.Enabled = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True
        End If
    End Sub

    'Added By Prashant 29-Apr-2013 'ALL29042013-4
    Private Sub cmbReceivingStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceivingStore.SelectedIndexChanged
        If cmbReceivingStore.SelectedIndex <= 0 And chkOnlyReceivedinSelectedStore.Checked = True Then
            chkOnlyReceivedinSelectedStore.Checked = False
            chkOnlyReceivedinSelectedStore.Enabled = False
            chkDetail.Enabled = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True
        ElseIf cmbReceivingStore.SelectedIndex <= 0 Then
            chkOnlyReceivedinSelectedStore.Enabled = False
        Else
            chkOnlyReceivedinSelectedStore.Enabled = True
        End If
        SetFocus(cmbStatus)
    End Sub
    'End

    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()

        upnlDisplaySearchCriteria.Update()
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub cmbFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedIndex = 0 Then
            chkDetail.Visible = True
            optPortrait.Visible = True
            optLandscape.Visible = True
        Else
            chkDetail.Visible = False
            optPortrait.Visible = False
            optLandscape.Visible = False
         End If
    End Sub
#End Region


End Class