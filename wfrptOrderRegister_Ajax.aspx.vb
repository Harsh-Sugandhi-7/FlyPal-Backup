Public Class wfrptOrderRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mVendor As Vendor
    Public mItemList As ItemList
    Public mVendorList As VendorList
    Public mOrderTextList As DistinctTextListForOrder
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public Supplier As String = ""
    Public OrdText As String = ""
    Public OrdNo As String = ""
    Public Amend As String = ""
    Public QuotationNo As String = ""
    Public Status As String = ""
    Public IntOrderNo As String = ""
    Public PriorityName As String = ""
    Public PriorityID As Integer
    Public mPriorityList As PriorityList
    Public Aircraft As String = "" 'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
    Public mPOTowards As POTowards
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        mPriorityList = CType(Session("mPriorityList"), PriorityList)
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = CType(Session("PartNo"), String)
        Description = CType(Session("Description"), String)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub

    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
        Session("mItemList") = mItemList
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mPriorityList")
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
        lblTransType.Visible = True
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblOrderNo.Visible = True
        lblQuotNo1.Visible = True
        lblIntOrderNo.Visible = True
        lblStatus1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblPriority1.Visible = True
        lblAircraft1.Visible = True 'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
        lblExpenses1.Visible = True
    End Sub

    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblVendor.Visible = False
        lblOrderNo.Visible = False
        lblQuotNo1.Visible = False
        lblIntOrderNo.Visible = False
        lblStatus1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblPriority1.Visible = False
        lblAircraft1.Visible = False 'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
        lblExpenses1.Visible = False
    End Sub

    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        Supplier = txtSupplier.Text.Trim
        lblVendor.Text = "Supplier :  " & Supplier

        'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
        Aircraft = txtAircraft.Text.Trim
        lblAircraft1.Text = "Aircraft :  " & Aircraft
        'End

        'Added by Utkarsh On 20-Dec-2011
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        'End

        QuotationNo = txtQuotNo.Text.Trim
        'OrdText = IIf(cmbOrderTextList.SelectedIndex > 0, Trim(cmbOrderTextList.SelectedItem.Text), "")
        OrdText = IIf(txtOrderTextList.Text <> "", Trim(txtOrderTextList.Text), "")
        OrdNo = txtOrderNo.Text.Trim
        Amend = txtAmend.Text.Trim
        Status = cmbStatus.SelectedItem.Text 'IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        PriorityName = IIf(cmbPriority.SelectedItem.Text = "(All)", "", cmbPriority.SelectedItem.Text)
        PriorityID = IIf(cmbPriority.SelectedItem.Text = "(All)", -1, cmbPriority.SelectedValue)
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")

        Session("PartNo") = PartNo
        Session("Description") = Description

        IntOrderNo = txtIntOrderNo.Text.Trim

        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblQuotNo1.Text = "Supp. Quot. No. : " & IIf(QuotationNo <> "", QuotationNo, "All")
        lblOrderNo.Text = "Order No.: " & IIf(OrdText + OrdNo + Amend <> "", OrdText + "-" + OrdNo + Amend, "All")
        lblStatus1.Text = "Status :" & cmbStatus.SelectedItem.Text 'IIf(Status <> "", Status, "All")
        lblIntOrderNo.Text = "Internal Order No.: " & IIf(IntOrderNo <> "", IntOrderNo, "All")
        lblTransType.Text = "Order Type     : " & IIf(cmbOrderType.SelectedIndex > 0, cmbOrderType.SelectedItem.Text, "All")
        lblPriority1.Text = "Priority     : " & IIf(cmbPriority.SelectedItem.Text = "(All)", "All", cmbPriority.SelectedItem.Text)
        lblExpenses1.Text = "Expenses : " & cmbExpenses.SelectedItem.Text

        mCompleteSearchingCriteria = lblTransType.Text + ", " + lblDateRange.Text + ", " + lblVendor.Text + ", " + lblQuotNo1.Text + ", " + lblIntOrderNo.Text + ", " + _
           lblOrderNo.Text + ", " + lblStatus1.Text + ", " + IIf(chkDetail.Checked, "Detailed Report", "") + ", " + " Format " + IIf(optLandscape.Checked, "LandScape", "Portrait") + ", " + _
           lblPriority1.Text + ", " + lblAircraft1.Text + ", " + lblExpenses1.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text

    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchOrderRegister"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@Description", Description)
        cmd.Parameters.AddWithValue("@Text", OrdText)
        cmd.Parameters.AddWithValue("@No", OrdNo)
        cmd.Parameters.AddWithValue("@Amend", Amend)
        cmd.Parameters.AddWithValue("@IntOrderNo", IntOrderNo)
        cmd.Parameters.AddWithValue("@FromDate", FromDate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@StatusID", cmbStatus.SelectedIndex)
        cmd.Parameters.AddWithValue("@QuotationNo", QuotationNo)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@TransTypeID", cmbOrderType.SelectedValue)
        cmd.Parameters.AddWithValue("@PriorityID", PriorityID)
        cmd.Parameters.AddWithValue("@OrderAircraftReg", Aircraft)
        cmd.Parameters.AddWithValue("@ScheduleExpenses", cmbExpenses.SelectedIndex)
        cmd.Parameters.AddWithValue("@IsCalibrationOrder", chkIsCalibrationOrder.Checked)
        cmd.Parameters.AddWithValue("@ClientCode", AppSettings("ClientCode"))
        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        dataTable.Columns.Remove("Rem1")
        dataTable.Columns.Remove("Rem2")
        dataTable.Columns.Remove("Rem3")
        If AppSettings("ClientCode") = "CE" Then
            dataTable.Columns.Remove("Quotation Date")
            dataTable.Columns.Remove("Quotation No.")
        Else
            dataTable.Columns.Remove("PO. Towards")
        End If
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsOrder As New dsOrder

        Dim objSearch As rptSearchingCriteriaForReceipt
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, _
                                                                                  IIf(cmbExpenses.SelectedIndex = 0, "", cmbExpenses.SelectedItem.Text), _
                                                                                  PriorityName, IIf(chkIsCalibrationOrder.Checked = True, "Calibration Order", ""), _
                                                                                  AppSettings("ClientCode"), OrdText, cmbPOTowards.SelectedItem.Text, "", OrdNo, Aircraft, Supplier, "", _
                                                                                  Status, "", PartNo, Description, "", "", "", Amend, QuotationNo, _
                                                                                  IntOrderNo, "", "", "", "", "", cmbOrderType.SelectedValue, "", "", _
                                                                                  AppSettings("Logo"))


        dsOrder.Clear()
        da.Fill(dsOrder, objSearch)
        Dim columnToRemove As String()
        If AppSettings("ClientCode") = "CE" Then
            columnToRemove = {"ID", "CompanyName", "IssText", "Store", "IssNo", "DCNo", "InvText", "InvNo", "FromStore", "SerialNo", _
                                         "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", _
                                         "TransTypeID", "ShowLogo", "WorkShop", "WorkOrderText", "WorkOrderNo", "ClientCode"}
        Else
            columnToRemove = {"ID", "CompanyName", "IssText", "Store", "IssNo", "RecNo", "DCNo", "InvText", "InvNo", "FromStore", "SerialNo", _
                                         "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", _
                                         "TransTypeID", "ShowLogo", "WorkShop", "WorkOrderText", "WorkOrderNo", "ClientCode"}
        End If
       
        For i As Integer = 0 To columnToRemove.Length - 1
            If dsOrder.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove(i)) Then
                dsOrder.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(dsOrder.Tables("rptSearchingCriteriaForReceipt"))
        dsNew.Merge(tbl)


        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("InternalReceiptNo").ColumnName = "Expenses"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("ReleaseNoteNo").ColumnName = "Priority"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdText").ColumnName = "Order Text"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("OrdNo").ColumnName = "Order No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("PartNo").ColumnName = "Part No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Description").ColumnName = "Part Description"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("QuotationNo").ColumnName = "Quotation No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("IntOrderNo").ColumnName = "Int. Order No."
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("FromDate").ColumnName = "From Date"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("ToDate").ColumnName = "To Date"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("RecText").ColumnName = "Calibration Order"
        If AppSettings("ClientCode") = "CE" Then
            dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("RecNo").ColumnName = "PO. Towards"
        End If
        dsNew.Tables("TMainReport").Columns("AmountInBaseCurrency").ColumnName = "Amount (in " + objSearch(0).CurrencySymbol + ")"
        dsNew.Tables("TMainReport").Columns("TotalOrderAmountInBaseCurrency").ColumnName = "Total Order Amount (in " + objSearch(0).CurrencySymbol + ")"

        dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = "Purchase Order Register"
		Session("ExcelFileName") = "Purchase Order Register"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "OrderReg", "Export To excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel

        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim objReg As rptOrderRegister
        Dim mrptOrderListSum As rptOrderRegisterSum
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsOrder As New dsOrder

        SetValues()

        If chkDetail.Checked Then
            If optPortrait.Checked Then
                myReport = New crptOrderRegister
            Else
                myReport = New crptOrderRegisterLandScape
            End If

            'Added 'Aircraft' parameter By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
            objReg = rptOrderRegister.GetOrderList(PartNo, Description, OrdText, OrdNo, Amend, IntOrderNo, FromDate, ToDate, cmbStatus.SelectedIndex, _
                                                   QuotationNo, Supplier, cmbOrderType.SelectedValue, PriorityID, Aircraft, cmbExpenses.SelectedIndex, _
                                                   chkIsCalibrationOrder.Checked, POTowardsID:=cmbPOTowards.SelectedValue, IsPBHPurchase:=chkIsPBHPurchase.Checked)
            mrptOrderListSum = rptOrderRegisterSum.GetOrderListSum(PartNo, Description, OrdText, OrdNo, Amend, IntOrderNo, FromDate, ToDate, _
                                                                   cmbStatus.SelectedIndex, QuotationNo, Supplier, cmbOrderType.SelectedValue, PriorityID)

            'Added 'Aircraft' parameter By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
            objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, _
                                                                                      IIf(cmbExpenses.SelectedIndex = 0, "", cmbExpenses.SelectedItem.Text), _
                                                                                      PriorityName, IIf(chkIsCalibrationOrder.Checked = True, "Calibration Order", ""), _
                                                                                      AppSettings("ClientCode"), OrdText, cmbPOTowards.SelectedItem.Text, "", OrdNo, Aircraft, Supplier, "", Status, "", _
                                                                                       PartNo, Description, "", "", "", Amend, QuotationNo, IntOrderNo, "", _
                                                                                       "", "", "", "", cmbOrderType.SelectedValue, "", "", _
                                                                                       AppSettings("Logo"))

            If objReg.Count <= 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfrptOrderRegister.aspx?Backpage="
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub

                'Added By Utkarsh On 7-Jun-2011 For All07062011
            ElseIf objReg.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 605)
                '*******************************
            End If

            dsOrder.Clear()

            If IsExcel = False Then
                Dim mrptImage As rptImage = rptImage.GetImage(dsOrder) 'Added by Shweta on 21-Feb-2012
                da.Fill(dsOrder, mrptImage) 'Added by Shweta on 21-Feb-2012
            End If

            da.Fill(dsOrder, objReg)
            da.Fill(dsOrder, objSearch)
            da.Fill(dsOrder, mrptOrderListSum)

            myReport.SetDataSource(dsOrder)
        Else
            If optPortrait.Checked Then
                myReport = New crptOrderRegSummary
            Else
                myReport = New crptOrderRegSummaryLandScape
            End If

            'Added 'Aircraft' parameter By Utkarsh On 05-Feb-2013 FOR Heligo054022013
            objReg = rptOrderRegister.GetOrderList(PartNo, Description, OrdText, OrdNo, Amend, IntOrderNo, FromDate, ToDate, cmbStatus.SelectedIndex, _
                                                   QuotationNo, Supplier, cmbOrderType.SelectedValue, PriorityID, Aircraft, cmbExpenses.SelectedIndex, _
                                                   chkIsCalibrationOrder.Checked, POTowardsID:=cmbPOTowards.SelectedValue, IsPBHPurchase:=chkIsPBHPurchase.Checked)

            'Added 'Aircraft' parameter By Utkarsh On 05-Feb-2013 FOR Heligo054022013
            objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, _
                                                                                      IIf(cmbExpenses.SelectedIndex = 0, "", cmbExpenses.SelectedItem.Text), _
                                                                                      PriorityName, IIf(chkIsCalibrationOrder.Checked = True, "Calibration Order", ""), _
                                                                                      AppSettings("ClientCode"), OrdText, cmbPOTowards.SelectedItem.Text, "", OrdNo, Aircraft, Supplier, "", Status, "", _
                                                                                      PartNo, Description, "", "", "", "", QuotationNo, IntOrderNo, "", "", "", "", "", _
                                                                                      cmbOrderType.SelectedValue, "", "", AppSettings("Logo"))
            mrptOrderListSum = rptOrderRegisterSum.GetOrderListSum(PartNo, Description, OrdText, OrdNo, Amend, IntOrderNo, FromDate, ToDate, _
                                                                   cmbStatus.SelectedIndex, QuotationNo, Supplier, cmbOrderType.SelectedValue, PriorityID)

            If objReg.Count <= 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfrptOrderRegister.aspx?Backpage="
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

                Exit Sub
                'Added By Utkarsh On 7-Jun-2011 For All07062011
            ElseIf objReg.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 605)
                '*******************************
            End If

            dsOrder.Clear()

            If IsExcel = False Then
                Dim mrptImage As rptImage = rptImage.GetImage(dsOrder) 'Added by Shweta on 21-Feb-2012
                da.Fill(dsOrder, mrptImage) 'Added by Shweta on 21-Feb-2012
            End If

            da.Fill(dsOrder, objReg)
            da.Fill(dsOrder, objSearch)
            da.Fill(dsOrder, mrptOrderListSum)

            myReport.SetDataSource(dsOrder)
        End If

        Session("CrystalReport") = myReport

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "OrderReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub

    Private Sub addAttributes()
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOrderNo').value,event)")
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

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        Session("mItemList") = mItemList

        mPriorityList = PriorityList.GetPriorityList(0, "", "(All)")
        Session("mPriorityList") = mPriorityList
        cmbPriority.DataSource = mPriorityList

        mPOTowards = POTowards.GetPOTowards("(All)")
        cmbPOTowards.DataSource = mPOTowards

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()

            If cmbOrderType.Enabled = True Then
                SetFocus(cmbOrderType)
            End If

            DataFieldBind()

            ControlVisibility(6)
            SetDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
    End Sub

    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        SetDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub

    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid Then
            ControlVisibility2()
            SetValues()

            upnlDisplaySearchCriteria.Update()
        End If
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(False)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid() Then
            SetValues()
            GenerateXLSXFile(CreateDataTable())
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub

    Protected Sub cmbOrderType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbOrderType.SelectedIndexChanged
        If cmbOrderType.SelectedIndex = 2 Or cmbOrderType.SelectedIndex = 4 Then
            cmbExpenses.Visible = True
            lblExpenses.Visible = True
            lblStep8.Visible = True
            'lblStep6.Text = "Step X. Selection of Part Number"
            If AppSettings("ClientCode") = "CE" Then
                'lblPOTowards.Text = "Step XI. Selection for PO. Towards"
                'lblStep7.Text = "Step XII. Display Report"
            Else
                'lblStep7.Text = "Step XI. Display Report"
            End If
            If cmbOrderType.SelectedIndex = 2 Then
                chkIsCalibrationOrder.Enabled = True
            Else
                chkIsCalibrationOrder.Enabled = False
                chkIsCalibrationOrder.Checked = False
            End If
        Else
            cmbExpenses.SelectedIndex = 0
            cmbExpenses.Visible = False
            lblExpenses.Visible = False
            lblStep8.Visible = False
            'lblStep6.Text = "Step IX. Selection of Part Number"
            If AppSettings("ClientCode") = "CE" Then
                'lblPOTowards.Text = "Step X. Selection for PO. Towards"
                'lblStep7.Text = "Step XI. Display Report"
            Else
                'lblStep7.Text = "Step X. Display Report"
            End If
            chkIsCalibrationOrder.Enabled = False
            chkIsCalibrationOrder.Checked = False
        End If
        upnlMain.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class