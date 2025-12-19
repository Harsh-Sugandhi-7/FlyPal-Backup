Imports System.IO
Imports OfficeOpenXml
Imports System.Data.SqlClient
Public Class wfrptRCIRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ToDate As String = ""
    Dim RecText As String = ""
    Dim RecNo As String = ""
    Dim InternalReceiptNo As String = ""
    Dim Supplier As String = ""
    Dim Aircraft As String = ""
    Dim Store As String = ""
    Dim DCNo As String = ""
    Dim Status As String = ""
    Dim ReceiptCumInvoice As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim OrdNo As String = ""
    Dim OrdText As String = ""
    Dim IssNo As String = ""
    Dim IssText As String = ""
    Dim InvNo As String = ""
    Dim InvText As String = ""
    Dim ReleaseNoteNo As String = ""
    Dim Fromdate As String
    Dim mTransTypeID As Int16
    Dim Tital As String
    Dim WorkShop As String = ""
    Dim CustomBillofEntry As String = ""
    Public mPartTypeList As PartTypeList
    Dim mPartType As Integer
    Dim mPartTypeName As String = ""
    Dim WorkOrderText As String = "" 'Added By Prashant 28-Dec-2010
    Dim WorkOrderNo As String = ""
    Dim mDistinctWOText As nDistinctWOText '-----------------------------
    Public Type As String = "" 'Added By Utkarsh ON 21-Dec-2011 FOR ALL13122011
    Public TextType As String = "" 'End
    Dim mStoreList As StoreList     'Added By Prashant 30-Apr-2013 'ALL29042013
    Dim mReceivingStoreList As StoreList 'Added By Prashant 30-Apr-2013 'ALL29042013
    Dim mRCIRegisterSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Dim mText As String = ""
    Dim mName As String = ""
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTransTypeID = CType(Session("mTransTypeID"), Int16)
        mPartTypeList = Session("mPartTypeList")
        Type = Session("Type") 'Added By Utkarsh On 21-Dec-2011 FOR ALL13122011
        TextType = Session("TextType") 'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartTypeList")
        Session.Remove("Type") 'Added By Utkarsh On 21-Dec-2011 FOR ALL13122011
        Session.Remove("TextType") 'End
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
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
        lblReceiptCumInvoice1.Visible = True
        lblReceivingStoreName.Visible = True
        lblCustomBillofEntries.Visible = True
        lblPartType1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor.Visible = False
        lblOrderNo.Visible = False
        lblIntReceiptNo.Visible = False
        lblReleaseNoteno.Visible = False
        lblStatus1.Visible = False
        lblDCNo.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblCustomBillofEntries.Visible = False
        lblPartType1.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
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
    Private Sub SetValues()
        Dim mTransTypeID As Integer
        mTransTypeID = CType(cmbReceiptCumInvoice.SelectedValue, Int16)
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1/1/1900"
            ToDate = "1/1/2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            Fromdate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " (" & cmbDateRange.SelectedItem.Text & ")"
        End If
        Supplier = IIf(cmbType.SelectedIndex = 1, txtSupplier.Text.Trim, "")
        If Supplier <> "" Then
            lblVendor.Text = IIf(mTransTypeID = 28, "Customer : " & Supplier, "Supplier : " & Supplier)
        Else
            lblVendor.Text = IIf(mTransTypeID = 28, "Customer : All", "Supplier : All")
        End If
        Aircraft = IIf(cmbType.SelectedIndex = 2, txtAircraft.Text.Trim, "")
        If Supplier <> "" Then
            mName = Supplier
        ElseIf Aircraft <> "" Then
            mName = Aircraft
        Else
            mName = ""
        End If
        WorkShop = IIf(cmbType.SelectedIndex = 4, txtWorkShop.Text.Trim, "")
        WorkOrderNo = IIf(cmbType.SelectedIndex = 5, txtWONo.Text.Trim, "") 'Added By Prashant 28-Dec-2010
        WorkOrderText = IIf(cmbType.SelectedIndex = 5, txtWorkOrderText.Text.Trim, "") '-----------------------------
        If cmbDocType.SelectedIndex = 1 Then
            IssText = ""
            IssNo = "0"
            OrdText = ""
            OrdNo = "0"
            InvText = ""
            InvNo = "0"
            RecText = Trim(txtReceiptText.Text)
            RecNo = Trim(txtNo.Text)
        End If
        If cmbDocType.SelectedIndex = 2 Then
            RecText = ""
            RecNo = "0"
            OrdText = ""
            OrdNo = "0"
            InvText = ""
            InvNo = "0"
            IssText = Trim(txtIssueText.Text)
            IssNo = Trim(txtNo.Text)
        End If
        If cmbDocType.SelectedIndex = 3 Then
            IssText = ""
            IssNo = "0"
            RecText = ""
            RecNo = "0"
            InvText = ""
            InvNo = "0"
            OrdText = Trim(txtOrderText.Text)
            OrdNo = Trim(txtNo.Text)
        End If
        If cmbDocType.SelectedIndex = 4 Then
            IssText = ""
            IssNo = "0"
            RecText = ""
            RecNo = "0"
            OrdText = ""
            OrdNo = "0"
            InvText = Trim(txtInvoiceText.Text)
            InvNo = Trim(txtNo.Text)
        End If
        If cmbDocType.SelectedIndex = 0 Then
            RecText = ""
            RecNo = "0"
            IssText = ""
            IssNo = "0"
            OrdText = ""
            OrdNo = "0"
            InvText = ""
            InvNo = "0"
        End If

        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If

        ReleaseNoteNo = txtReleaseNoteNo.Text.Trim
        InternalReceiptNo = txtIntReceiptNo.Text.Trim
        DCNo = txtDCNo.Text.Trim
        CustomBillofEntry = txtCustomBillofEntry.Text.Trim
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblIntReceiptNo.Text = "Int. Receipt No. : " & IIf(InternalReceiptNo <> "", InternalReceiptNo, "All")
        lblReleaseNoteno.Text = "Release Note No. : " & IIf(ReleaseNoteNo <> "", ReleaseNoteNo, "All")
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        lblStatus1.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblDCNo.Text = "D.C. No. : " & IIf(DCNo <> "", DCNo, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        ReceiptCumInvoice = IIf(cmbReceiptCumInvoice.SelectedIndex > 0, cmbReceiptCumInvoice.SelectedItem.Text, "")
        lblReceiptCumInvoice1.Text = "Goods Receipt : " & IIf(ReceiptCumInvoice <> "", ReceiptCumInvoice, "All")
        lblCustomBillofEntries.Text = "Custom Bill of Entry : " & IIf(CustomBillofEntry <> "", CustomBillofEntry, "All")
        Select Case cmbType.SelectedIndex
            Case 0
                lblVendor.Text = "From Type  : All "
            Case 1
                lblVendor.Text = IIf(mTransTypeID = 28, "Customer : " & IIf(Supplier <> "", Supplier, "All"), "Supplier : " & IIf(Supplier <> "", Supplier, "All"))
            Case 2
                lblVendor.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
            Case 3
                lblVendor.Text = "Store : " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text.Trim, "All")
            Case 4                                                                      'Added By Saylee on 11th-Apr-2008
                lblVendor.Text = "WorkShop : " & IIf(WorkShop <> "", WorkShop, "All")
            Case 5  'WorkOrder                                                          'Added By Prashant 28-Dec-2010
                lblVendor.Text = "WorkOrder : " & IIf(WorkOrderText <> "", WorkOrderText, "All")
        End Select

        Select Case cmbDocType.SelectedIndex
            Case 0
                lblOrderNo.Text = "Document Type : All "
            Case 1
                If RecText = "" Then
                    lblOrderNo.Text = "Receipt No. : All "
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
            Case 4
                If InvText = "" Then
                    lblOrderNo.Text = "Invoice No. : All "
                Else
                    lblOrderNo.Text = "Invoice No. : " + InvText + "-" + InvNo
                End If
        End Select
        lblReceivingStoreName.Text = "Receiving Store : " & IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, "All")
        mPartType = IIf(cmbPartType.SelectedIndex > 0, cmbPartType.SelectedValue, 0)
        mPartTypeName = IIf(cmbPartType.SelectedIndex > 0, cmbPartType.SelectedItem.Text, "")
        lblPartType1.Text = "Part Type : " + IIf(cmbPartType.SelectedIndex > 0, cmbPartType.SelectedItem.Text, "All")

        If chkHighValue.Checked And txtCEffectiveRate.Text <> "" Then 'Added By Prashant 14-Aug-2014 For ALL14082014
            mText = "Report shows valued parts with landing rate greater than  " + txtCEffectiveRate.Text
        Else
            mText = ""
        End If

        mRCIRegisterSearchingCriteria = lblDateRange.Text.ToString + ", " + lblReceiptCumInvoice1.Text + ", " + lblOrderNo.Text + ", " + lblVendor.Text + ", " + lblIntReceiptNo.Text + ", " + lblReleaseNoteno.Text + ", " + lblDCNo.Text + ", " + lblCustomBillofEntries.Text + ", " + _
           lblReceivingStoreName.Text + ", " + lblStatus1.Text + ", " + lblPartType1.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + _
           "format : " + IIf((chkDetail.Checked), IIf(optPortrait.Checked, "Detaail Portrait", " Detail landScape"), IIf(optPortrait.Checked, "Portrait", "landScape"))

    End Sub
    Private Function GetTitle() As String
        Dim mTransTypeList As TransactionList
        Dim mTitle As String
        mTransTypeList = TransactionList.GetTransactionList()
        mTransTypeID = CType(cmbReceiptCumInvoice.SelectedValue, Int16)

        mTransTypeList = TransactionList.GetTransactionList("Receipt-Cum-Invoice")
        mTitle = mTransTypeList.GetTransactionTypeName(cmbReceiptCumInvoice.SelectedValue).ToString + " Register"

        If chkDetail.Checked Or chkWithDocketCharges.Checked Then
            If mTitle = "" Then
                Return "Goods Receipt Register (Detail Report)"
            Else
                Return mTitle + " (Detail Report)"
            End If
        Else
            If mTitle = "" Then
                Return "Goods Receipt Register (Summary Report)"
            Else
                Return mTitle + " (Summary Report)"
            End If
        End If
        Return mTitle
    End Function
    Public Sub SetReport(ByVal IsExcel As Boolean)
        'Session("IsExcel") = IsExcel
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim objReg As rptReceiptCumInvReg
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsReceipt As New dsRecCumInvReg
        SetValues()
        Tital = GetTitle()
        If cmbFormat.SelectedIndex = 0 Then ''Format 1
            If chkOnlyReceivedinSelectedStore.Checked = False Then
                If chkDetail.Checked Or chkWithDocketCharges.Checked Then
                    If optPortrait.Checked Then
                        If chkWithoutinvoicingDetail.Checked Then
                            myReport = New crptReceiptCumInvoiceReciptRegister
                        Else
                            myReport = New crptReceiptCumInvoiceRegDetail
                        End If
                    Else
                        If chkWithoutinvoicingDetail.Checked Then
                            myReport = New crptReceiptCumInvoiceReciptRegisterLandScape
                        Else
                            If chkWithDocketCharges.Checked = True Then            'Added By Prashant 17-Aug-2012
                                myReport = New crptRecCumInvWithDocketChargeLand
                            Else
                                myReport = New crptReceiptCumInvoiceRegDetailLandscape
                            End If
                        End If
                    End If
                Else
                    If optPortrait.Checked Then
                        myReport = New crptReceiptCumInvoiceRegSummary
                    Else
                        myReport = New crptReceiptCumInvoiceRegSummaryLandscape
                    End If
                End If

                If cmbType.SelectedIndex = 0 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, , DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, , WorkOrderText, WorkOrderNo, , , chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 1 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Supplier, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, , WorkOrderText, WorkOrderNo, , chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 2 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Aircraft, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, , WorkOrderText, WorkOrderNo, , , chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 3 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, , WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, , chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 4 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, , WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, , chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 5 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, , WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, , chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                End If
            Else   'Else of chkOnlyReceivedinSelectedStore.Checked = True
                If optWithRate.Checked Then
                    myReport = New crptReceiptCumInvoiceRegDetailLandscapeWithRate    'Added By Prashant 13-Aug-2010
                ElseIf optWithEffRate.Checked Then
                    myReport = New crptReceiptCumInvoiceRegDetailLandscapeWithEffRate 'Added By Prashant 13-Aug-2010
                End If

                If cmbType.SelectedIndex = 0 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, , DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, , chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 1 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Supplier, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, , chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 2 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Aircraft, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, , chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 3 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 4 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                ElseIf cmbType.SelectedIndex = 5 Then
                    objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"))
                End If
            End If
        ElseIf cmbFormat.SelectedIndex = 1 Then ''Format 2
            myReport = New crptReceiptCumInvoiceRegDetailLandscapeFormat2
            If cmbType.SelectedIndex = 0 Then
                objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, , DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, , chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), Format2:=2)
            ElseIf cmbType.SelectedIndex = 1 Then
                objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Supplier, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, , chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), Format2:=2)
            ElseIf cmbType.SelectedIndex = 2 Then
                objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, Aircraft, DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, , chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), Format2:=2)
            ElseIf cmbType.SelectedIndex = 3 Then
                objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), Format2:=2)
            ElseIf cmbType.SelectedIndex = 4 Then
                objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), Format2:=2)
            ElseIf cmbType.SelectedIndex = 5 Then
                objReg = rptReceiptCumInvReg.GetRecCumInvList(Fromdate, ToDate, RecText, RecNo, InternalReceiptNo, "", DCNo, cmbStatus.SelectedValue, PartNo, Description, OrdNo, OrdText, IssNo, IssText, ReleaseNoteNo, cmbType.SelectedValue, InvText, InvNo, CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop, cmbReceivingStore.SelectedValue.ToString, CustomBillofEntry, mPartType, chkOnlyReceivedinSelectedStore.Checked, WorkOrderText, WorkOrderNo, cmbStore.SelectedValue.ToString, chkIsOHRepairRecords.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IsValued:=Val(cmbStoreType.SelectedValue), ClientCode:=AppSettings("ClientCode"), Format2:=2)
            End If
        End If
        If objReg.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf objReg.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 632)
        End If
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, InternalReceiptNo, ReleaseNoteNo, RecText, IssText, OrdText, RecNo, IssNo, OrdNo, Aircraft, Supplier, IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), Status, DCNo, PartNo, Description, InvText, InvNo, mText, Tital, IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), CustomBillofEntry, SerialNo:=mPartTypeName, Charge:=txtBottomLine.Text.Trim, SuppInvNo:="", FromInvDate:="", ToInvDate:="", TransTypeID:=CInt(cmbReceiptCumInvoice.SelectedValue), WorkShop:=WorkShop, WorkOrderText:=IIf(chkIsOHRepairRecords.Checked, "Return from OH/Repair records.", ""), WorkOrderNo:=AppSettings("Logo"))
        dsReceipt.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt)
            da.Fill(dsReceipt, mrptImage)
        End If
        da.Fill(dsReceipt, objReg)
        da.Fill(dsReceipt, objSearch)
        myReport.SetDataSource(dsReceipt)

        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "RCIReg", mRCIRegisterSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetTitle()
        Dim mTransTypeID As Integer
        mTransTypeID = CType(cmbReceiptCumInvoice.SelectedValue, Int16)

        cmbType.Enabled = False
        txtNo.Text = ""
        txtWONo.Text = ""
        Dim Index As Int16 = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedIndex, 0)
        lblType1.Visible = (Index > 0)
        If Index = 1 Then
            If mTransTypeID = 28 Or mTransTypeID = 50 Or mTransTypeID = 53 Or mTransTypeID = 57 Then
                lblType1.Text = "Customer "
                Type = "Customer"
            Else
                lblType1.Text = "Supplier "
                Type = "Supplier"
            End If
        ElseIf Index = 2 Then
            lblType1.Text = "Aircraft  "
            Type = "Aircraft"
        ElseIf Index = 3 Then
            lblType1.Text = "Store  "
            Type = "Store"
        ElseIf Index = 4 Then
            lblType1.Text = "WorkShop  "
            Type = "WorkShop"
        ElseIf Index = 5 Then
            lblType1.Text = "WorkOrder "
            Type = "Text"
        ElseIf Index = 0 Then
            Type = ""
        End If
        'Added By Utkarsh On 21-Dec-2011 FOR ALL13122011
        txtSupplier.Text = ""
        txtAircraft.Text = ""
        cmbStore.SelectedIndex = 0
        txtWorkShop.Text = ""
        txtWorkOrderText.Text = ""
        txtSupplier.Visible = (Index = 1)
        txtAircraft.Visible = (Index = 2)
        cmbStore.Visible = (Index = 3)
        txtWorkShop.Visible = (Index = 4)
        txtWorkOrderText.Visible = (Index = 5)
        txtWONo.Visible = (Index = 5)

        'Type = IIf(Index = 0, "", IIf(Index = 1, "Supplier", IIf(Index = 2, "Aircraft", IIf(Index = 3, "Store", IIf(Index = 4, "WorkShop", IIf(Index = 5, "Text", ""))))))

        TextType = IIf(Index = 5, "16", "0")

        Session("Type") = Type
        Session("TextType") = TextType

        hidden_DocTextType.Value = TextType
        hidden_DocType.Value = Type
        upnlHiddenField.Update()

        upnlDocType.Update()
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
        txtWONo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtWONo').value,event)") 'Added By Utkarsh On 22-Dec-2011 FOR ALL13122011
        txtCEffectiveRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCEffectiveRate').value,event)")
    End Sub
    Public Sub SetCustomer()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Customer", "14"))
        cmbType.Items.Add(New ListItem("Aircraft", "2"))
        cmbType.Items.Add(New ListItem("Store", "8"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
        cmbType.Items.Add(New ListItem("WorkOrder", "17"))
    End Sub
    Public Sub SetVendor()
        Me.cmbType.Items.Clear()
        cmbType.Items.Add(New ListItem("(All)", "0"))
        cmbType.Items.Add(New ListItem("Supplier", "14"))
        cmbType.Items.Add(New ListItem("Aircraft", "2"))
        cmbType.Items.Add(New ListItem("Store", "8"))
        cmbType.Items.Add(New ListItem("WorkShop", "16"))
        cmbType.Items.Add(New ListItem("WorkOrder", "17"))
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("DT")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchRecCumInvList"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@FromDate", Fromdate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@Text", RecText)
        cmd.Parameters.AddWithValue("@No", RecNo)
        cmd.Parameters.AddWithValue("@IntReceiptNo", InternalReceiptNo)
        cmd.Parameters.AddWithValue("@Name ", mName)
        cmd.Parameters.AddWithValue("@DCNO", DCNo)
        cmd.Parameters.AddWithValue("@StatusID ", cmbStatus.SelectedValue)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@Description", Description)
        cmd.Parameters.AddWithValue("@OrderNo", OrdNo)
        cmd.Parameters.AddWithValue("@OrderText", OrdText)
        cmd.Parameters.AddWithValue("@IssueNo", IssNo)
        cmd.Parameters.AddWithValue("@IssueText", IssText)
        cmd.Parameters.AddWithValue("@Type", cmbType.SelectedValue)
        cmd.Parameters.AddWithValue("@ReleaseNoteNo", ReleaseNoteNo)
        cmd.Parameters.AddWithValue("@InvoiceText", InvText)
        cmd.Parameters.AddWithValue("@InvoiceNo", InvNo)
        cmd.Parameters.AddWithValue("@TransTypeID", cmbReceiptCumInvoice.SelectedValue)
        cmd.Parameters.AddWithValue("@WorkShopName", WorkShop)
        cmd.Parameters.AddWithValue("@StoreID", cmbReceivingStore.SelectedValue.ToString)
        cmd.Parameters.AddWithValue("@AWBNo", CustomBillofEntry)
        cmd.Parameters.AddWithValue("@ItemTypeID", mPartType)
        cmd.Parameters.AddWithValue("@WorkOrderText", WorkOrderText)
        cmd.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo)
        cmd.Parameters.AddWithValue("@ReceiptStoreID", cmbStore.SelectedValue.ToString)
        cmd.Parameters.AddWithValue("@IsReturnFromOHRepair", chkIsOHRepairRecords.Checked)
        cmd.Parameters.AddWithValue("@HighValue", chkHighValue.Checked)
        cmd.Parameters.AddWithValue("@RateValue", CDec(Val(txtCEffectiveRate.Text)))
        cmd.Parameters.AddWithValue("@IsValued", Val(cmbStoreType.SelectedValue))                   'Added By Prashant 15-Jan-2018 For Deccan15012018
        cmd.Parameters.AddWithValue("@ClientCode", AppSettings("ClientCode"))           'Added By Prashant 15-Jan-2018 For Deccan15012018
        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        dataTable.Columns.Remove("Date")
        dataTable.Columns.Remove("Text")
        dataTable.Columns.Remove("No")
        Return dataTable
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mStoreList

        mReceivingStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbReceivingStore.DataSource = mReceivingStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        mPartTypeList = PartTypeList.GetItemTypeList(True, "", "(All)")
        cmbPartType.DataSource = mPartTypeList
        DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            cmbReceiptCumInvoice.Items.Clear()
            cmbReceiptCumInvoice.Items.Add(New ListItem("(All)", "00"))

            If User.IsInRole("RCIFromPORegView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Against Purchase Order", "07"))
            If User.IsInRole("RCIFromStoreRegView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("From other Store", "08"))
            If User.IsInRole("RCIFromAircraftRegView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("From Aircraft", "09"))
            If User.IsInRole("RCIFromVendorRegView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("As Exchange/Repair", "10"))
            If User.IsInRole("RCIFromStoreForLoanReturnRegView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Against loan issued to Store", "11"))
            If User.IsInRole("RCIFromStoreForLoanRegView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("As loan taken from another Store", "12"))
            If User.IsInRole("RCIFromAircraftForLoanReturnRegView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Against loan issued to Aircraft", "13"))
            If User.IsInRole("RCIAgainstLoanToVendorView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Against loan issued to Supplier", "27"))
            If User.IsInRole("RCIAgainstLoanToCustomerView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Against loan issued to Customer", "28"))
            If User.IsInRole("AssembledFromWorkShopView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("From WorkShop", "46"))
            If User.IsInRole("RCIFromWorkShopForLoanReturnView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Against loan issued to WorkShop", "47"))
            If User.IsInRole("ReceiptasLoanFromSupplierView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Receipt as Loan From Supplier", "48"))
            If User.IsInRole("ReceiptasLoanFromCustomerView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Receipt as Loan From Customer", "50"))
            If User.IsInRole("RCIFromCustomerView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Receipt From Customer", "53"))
            If User.IsInRole("ReceivedfromSupplierRentalLeaseView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Received from Supplier Rental/Lease", "54"))
            If User.IsInRole("DisassembledFromWorkShopView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Disassembled From WorkShop", "56"))
            If User.IsInRole("ReceivedFromCustomerAsForRepairView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Received From Customer As For Repair", "57"))
            If User.IsInRole("RCIFromWorkOrderView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Received From Work Order As Removed", "61"))
            If User.IsInRole("RCIFromWorkOrderReturnView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("Received From Work Order As Return", "62"))
            'Added By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1
            If User.IsInRole("RCIFromAircraftAsCoreUnitReturnView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("RCI From Aircraft As Core Unit Return", "66"))
            If User.IsInRole("RCIFromSupplierAsNoneView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("RCI From Supplier As None", "67"))
            'End
            If User.IsInRole("ReceivedFromWorkShopAsServiceablReturnedView") Then cmbReceiptCumInvoice.Items.Add(New ListItem("WorkShop-Serviceable Returned", "73"))
            RemoveSession()

            If cmbReceiptCumInvoice.Enabled = True Then
                setFocus(cmbReceiptCumInvoice)
            End If

            mTransTypeID = Request.QueryString("TransTypeId")
            Session("mTransTypeID") = mTransTypeID

            DataFieldBind()

            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            SetTitle()

            chkOnlyReceivedinSelectedStore.Enabled = False
            optWithEffRate.Enabled = False
            optWithRate.Enabled = False
        End If
        'Added By Vikrant On 23-July-2013 For ALL23072013
        If cmbReceiptCumInvoice.SelectedIndex = 21 Then 'Receipt From Supplier As None
            chkIsOHRepairRecords.Visible = True
            upnlIsOHRepairRecords.Update()
            lblOHRepairRecords.Visible = True
            If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                lblValuedStores.Text = "Step XIV. Selection For Valued, Non-Valued Stores"
                lblStep11.Text = "Step XV. Display Report"
            Else
                lblStep11.Text = "Step XIV. Display Report"
            End If
            upnlValuedStores.Update()
            upnlDisplaySearchCriteria.Update()
        Else
            chkIsOHRepairRecords.Visible = False
            chkIsOHRepairRecords.Checked = False
            upnlIsOHRepairRecords.Update()
            lblOHRepairRecords.Visible = False
            If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                lblValuedStores.Text = "Step XIII. Selection For Valued, Non-Valued Stores"
                lblStep11.Text = "Step XIV. Display Report"
            Else
                lblStep11.Text = "Step XIII. Display Report"
            End If
            upnlValuedStores.Update()
        End If
        'End
        If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            lblValuedStores.Visible = True
            cmbStore.Visible = True
            lblCase.Visible = True
            Label1.Visible = True
        Else
            lblValuedStores.Visible = False
            cmbStore.Visible = False
            lblCase.Visible = False
            Label1.Visible = False
        End If
        If cmbReceiptCumInvoice.SelectedValue = 0 Then
            cmbType.SelectedIndex = 0
            lblStep4.Text = "Step IV. All"
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub cmbDocType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDocType.SelectedIndexChanged
        txtNo.Text = ""
        Dim Index As Int16 = IIf(cmbDocType.SelectedIndex > 0, cmbDocType.SelectedIndex, 0)
        lblDocTypeNo.Visible = (Index > 0)
        lblDocTypeNo.Text = IIf(Index = 0, "", IIf(Index = 1, "Receipt No.  ", IIf(Index = 2, "Issue No.  ", IIf(Index = 3, "Order No.  ", IIf(Index = 4, "Invoice No.  ", "")))))
        'Added By Utkarsh ON 21-Dec-2011 For ALL13122011

        txtReceiptText.Visible = (Index = 1)
        txtIssueText.Visible = (Index = 2)
        txtOrderText.Visible = (Index = 3)
        txtInvoiceText.Visible = (Index = 4)
        txtNo.Visible = (Index > 0)

        Type = IIf(Index > 0, "Text", Type)
        TextType = IIf(Index = 0, "0", IIf(Index = 1, "2", IIf(Index = 2, "3", IIf(Index = 3, "1", IIf(Index = 4, "4", "0")))))

        Session("Type") = Type
        Session("TextType") = TextType

        hidden_DocType.Value = Type
        hidden_DocTextType.Value = TextType

        upnlHiddenField.Update()

        'End
        If cmbDocType.Enabled = True Then
            setFocus(cmbDocType)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetValues()
        GenerateXLSXFile(CreateDataTable())
    End Sub
    Private Sub GenerateXLSXFile(tbl As DataTable)
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsReceipt As New dsRecCumInvReg
        Dim objSearch As rptSearchingCriteriaForReceipt
        SetValues()
        Tital = GetTitle()
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:=Fromdate, ToDate:=ToDate, _
                                                                                  InternalReceiptNo:=InternalReceiptNo, ReleaseNoteNo:=ReleaseNoteNo, _
                                                                                  RecText:=RecText, IssText:=IssText, OrdText:=OrdText, RecNo:=RecNo, _
                                                                                  IssNo:=IssNo, OrdNo:=OrdNo, Aircraft:=Aircraft, Supplier:=Supplier, _
                                                                                  Store:=IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, ""), _
                                                                                  Status:=Status, DCNo:=DCNo, PartNo:=PartNo, Description:=Description, _
                                                                                  InvText:=InvText, InvNo:=InvNo, FromStore:=mText, Amend:=Tital, _
                                                                                  QuotationNo:=IIf(cmbReceivingStore.SelectedIndex > 0, cmbReceivingStore.SelectedItem.Text, ""), _
                                                                                  IntOrderNo:=CustomBillofEntry, SerialNo:=mPartTypeName, Charge:=txtBottomLine.Text.Trim, SuppInvNo:="", _
                                                                                  FromInvDate:="", ToInvDate:="", TransTypeID:=CInt(cmbReceiptCumInvoice.SelectedValue), _
                                                                                  WorkShop:=WorkShop, WorkOrderText:=IIf(chkIsOHRepairRecords.Checked, "Return from OH/Repair records.", ""), _
                                                                                  WorkOrderNo:=AppSettings("Logo"))

        dsReceipt.Clear()
        da.Fill(dsReceipt, objSearch)

        Dim columnToRemove As String() = {"ID", "CompanyName", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", _
                                         "TransTypeID", "ShowLogo", "ClientCode", "WorkOrderNo", "FromStore"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If dsReceipt.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove(i)) Then
                dsReceipt.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(dsReceipt.Tables("rptSearchingCriteriaForReceipt"))
        dsNew.Merge(tbl)

        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InternalReceiptNo").ColumnName = "Int. Rece. No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("ReleaseNoteNo").ColumnName = "Rel. Note No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("RecText").ColumnName = "Receipt Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("RecNo").ColumnName = "Receipt No"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("IssText").ColumnName = "Issue Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("IssNo").ColumnName = "Issue No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdText").ColumnName = "Order Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdNo").ColumnName = "Order No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InvText").ColumnName = "Inv. Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InvNo").ColumnName = "Inv. No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Amend").ColumnName = "Receipt Type"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("QuotationNo").ColumnName = "Receiving Store"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("IntOrderNo").ColumnName = "Custom Bill of Entry"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("SerialNo").ColumnName = "Part Type"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Charge").ColumnName = "Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("WorkShop").ColumnName = "Work Shop"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("WorkOrderText").ColumnName = "Return from OH/Repair records."

        dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("DT").TableName = "RCI Register"
		Session("ExcelFileName") = "RCI Register"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "RCIReg", "Export To Excel " + mRCIRegisterSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub cmbReceiptCumInvoice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptCumInvoice.SelectedIndexChanged

        Dim mTransTypeID As Integer
        mTransTypeID = CType(cmbReceiptCumInvoice.SelectedValue, Int16)
        Select Case (mTransTypeID)
            Case 7                                                         'Receipt cum Invoice against Purchase Order Register
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 8                                                          'Received from Store Register
                cmbType.SelectedIndex = 3
                lblStep4.Text = "Step IV. Selection of Store"
            Case 9, 66   'Added(66) By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1                                                       'Received from Aircraft Register
                cmbType.SelectedIndex = 2
                lblStep4.Text = "Step IV. Selection of Aircraft"
            Case 10, 67  'Added(67) By Utkarsh ON 17-Oct-2012 FOR ALL12102012-1                                                      'Received as Exchange / Repair from Vendor Register
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 11                                                         'Receipt against loan issued to Store Register
                cmbType.SelectedIndex = 3
                lblStep4.Text = "Step IV. Selection of Store"
            Case 12                                                         'Received as loan taken from another Store Register
                cmbType.SelectedIndex = 3
                lblStep4.Text = "Step IV. Selection of Store"
            Case 13                                                         'Receipt against loan issued to Aircraft Register  
                cmbType.SelectedIndex = 2
                lblStep4.Text = "Step IV. Selection of Aircraft"
            Case 27                                                         'Receipt Against Loan Issued To Vendor 
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 28                                                         'Receipt Against issued to Customer
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 46                                                        'Received from WorkShop Register
                lblStep4.Text = "Step IV. Selection of WorkShop"
                cmbType.SelectedIndex = 4
            Case 47
                lblStep4.Text = "Step IV. Selection of WorkShop"            'Receipt against loan issued to WorkShop Register  
                cmbType.SelectedIndex = 4
            Case 48                                                         'Receipt as Loan From Supplier*
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 50                                                         'Receipt as Loan From Customer*
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 53                                                         'Receipt From Customer*
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 54                                                         'Received from Supplier Rental/Lease
                lblStep4.Text = "Step IV. Selection of Supplier"
                SetVendor()
                cmbType.SelectedIndex = 1
            Case 56, 73                                                        'Received from WorkShop Register
                lblStep4.Text = "Step IV. Selection of WorkShop"
                cmbType.SelectedIndex = 4
            Case 57                                                         'Received From Customer As For Repair
                lblStep4.Text = "Step IV. Selection of Customer"
                SetCustomer()
                cmbType.SelectedIndex = 1
            Case 61                                                         'RCIFromWorkOrder
                lblStep4.Text = "Step IV. Selection of WorkOrder"
                cmbType.SelectedIndex = 5
            Case 62                                                         'RCIFromWorkOrderAs Return
                lblStep4.Text = "Step IV. Selection of WorkOrder"
                cmbType.SelectedIndex = 5
        End Select
        SetTitle()
        If cmbReceiptCumInvoice.Enabled = True Then
            setFocus(cmbReceiptCumInvoice)
        End If
        hidden_FromType.Value = Type
        upnlHiddenField.Update()
        upnlFromType.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub chkDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDetail.CheckedChanged
        If chkDetail.Checked = True And optPortrait.Checked = True Then
            chkWithoutinvoicingDetail.Visible = True
        ElseIf chkDetail.Checked = False And optPortrait.Checked = True Then
            chkWithoutinvoicingDetail.Visible = False
        ElseIf chkDetail.Checked = True And optLandscape.Checked = True Then
            chkWithoutinvoicingDetail.Visible = True
        ElseIf chkDetail.Checked = False And optLandscape.Checked = True Then
            chkWithoutinvoicingDetail.Visible = False
        End If
    End Sub
    Private Sub optPortrait_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPortrait.CheckedChanged
        If chkDetail.Checked = True And optPortrait.Checked = True Then
            chkWithoutinvoicingDetail.Visible = True
        ElseIf optPortrait.Checked = True Then
            chkWithoutinvoicingDetail.Visible = False
        End If
    End Sub
    Private Sub optLandscape_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optLandscape.CheckedChanged
        If chkDetail.Checked = True And optLandscape.Checked = True Then
            chkWithoutinvoicingDetail.Visible = True
        ElseIf optLandscape.Checked = True Then
            chkWithoutinvoicingDetail.Visible = False
        End If
    End Sub
    Private Sub chkOnlyReceivedinSelectedStore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOnlyReceivedinSelectedStore.CheckedChanged
        If chkOnlyReceivedinSelectedStore.Checked = True Then
            chkDetail.Enabled = False
            optPortrait.Enabled = False
            optLandscape.Enabled = False
            chkWithoutinvoicingDetail.Enabled = False
            chkWithDocketCharges.Enabled = False
            chkWithDocketCharges.Checked = False
            optWithEffRate.Enabled = True
            optWithRate.Enabled = True
        Else
            chkDetail.Enabled = True
            chkDetail.Checked = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True
            chkWithoutinvoicingDetail.Enabled = True
            chkWithDocketCharges.Enabled = True
            optWithEffRate.Enabled = False
            optWithRate.Enabled = False
        End If
        upnlStatus.Update()
        upnlpartType.Update()
    End Sub
    Private Sub cmbReceivingStore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReceivingStore.SelectedIndexChanged
        'If cmbReceivingStore.SelectedIndex = 0 And chkOnlyReceivedinSelectedStore.Checked = True Then
        '    chkOnlyReceivedinSelectedStore.Checked = False
        '    optWithEffRate.Enabled = False
        '    chkOnlyReceivedinSelectedStore.Enabled = False
        '    optWithRate.Enabled = False
        '    chkDetail.Enabled = True
        '    optPortrait.Enabled = True
        '    optLandscape.Enabled = True
        '    chkWithoutinvoicingDetail.Enabled = True
        'Else
        If cmbReceivingStore.SelectedIndex = 0 Then 'Added By Prashant 30-Apr-2013 'ALL29042013
            chkOnlyReceivedinSelectedStore.Enabled = False
            chkOnlyReceivedinSelectedStore.Checked = False
            optWithEffRate.Enabled = False
            optWithRate.Enabled = False

            chkDetail.Enabled = True
            chkDetail.Checked = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True

            chkWithoutinvoicingDetail.Enabled = True
            chkWithDocketCharges.Enabled = True

            upnlStatus.Update()
            upnlpartType.Update()
        Else
            chkOnlyReceivedinSelectedStore.Enabled = True
            chkOnlyReceivedinSelectedStore.Checked = False
        End If
        setFocus(cmbStatus)
    End Sub
    Private Sub chkWithDocketCharges_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWithDocketCharges.CheckedChanged
        If chkWithDocketCharges.Checked = True Then
            chkDetail.Checked = False
            optPortrait.Checked = False
            optLandscape.Checked = False
            chkWithoutinvoicingDetail.Checked = False
            chkOnlyReceivedinSelectedStore.Checked = False
            optWithRate.Checked = False
            chkDetail.Enabled = False
            optPortrait.Enabled = False
            optLandscape.Enabled = False
            chkWithoutinvoicingDetail.Enabled = False
            chkOnlyReceivedinSelectedStore.Enabled = False
            optWithEffRate.Enabled = False
            optWithRate.Enabled = False
        Else
            chkDetail.Checked = True
            optPortrait.Checked = True
            chkDetail.Enabled = True
            optPortrait.Enabled = True
            optLandscape.Enabled = True
            chkWithoutinvoicingDetail.Enabled = True
            If cmbReceivingStore.SelectedIndex > 0 Then 'Added By Prashant 30-Apr-2013 'ALL29042013
                chkOnlyReceivedinSelectedStore.Enabled = True
            Else
                chkOnlyReceivedinSelectedStore.Enabled = False
            End If
        End If
        upnlReceivingStore.Update()
        upnlStatus.Update()
    End Sub
    Private Sub chkHighValue_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkHighValue.CheckedChanged 'Added By Prashant 14-Aug-2014 For ALL14082014
        If chkHighValue.Checked = True Then
            txtCEffectiveRate.Enabled = True
        Else
            txtCEffectiveRate.Enabled = False
            txtCEffectiveRate.Text = ""
        End If
        upnlHighValue.Update()
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
            chkWithDocketCharges.Visible = True
            chkWithoutinvoicingDetail.Visible = True
            chkOnlyReceivedinSelectedStore.Visible = True
            optWithEffRate.Visible = True
            optWithRate.Visible = True
        Else
            chkDetail.Visible = False
            optPortrait.Visible = False
            optLandscape.Visible = False
            chkWithDocketCharges.Visible = False
            chkWithoutinvoicingDetail.Visible = False
            chkOnlyReceivedinSelectedStore.Visible = False
            optWithEffRate.Visible = False
            optWithRate.Visible = False
        End If
        upnlReceivingStore.Update()
        upnlpartType.Update()
        upnlReceivingStore.Update()
    End Sub
#End Region
End Class