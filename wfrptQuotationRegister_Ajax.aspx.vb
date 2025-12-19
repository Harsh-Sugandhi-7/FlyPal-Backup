Public Class wfrptQuotationRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mVendor As Vendor
    Public mItemList As ItemList
    Public mVendorList As VendorList
    Public mQuotationTextList As DistinctTextListForQuotation
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public Supplier As String = ""
    Public QuotationText As String = ""
    Public QuotationNo As String = ""
    Public Status As String = ""
    Public QuotationType As String = ""
    Public mType As Integer

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        mItemList = CType(Session("mItemList"), ItemList)
        PartNo = CType(Session("PartNo"), String)
        Description = CType(Session("Description"), String)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mType = CType(Session("mType"), Integer)
    End Sub
    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
        Session("mItemList") = mItemList
        Session("mType") = mType
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mType")
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
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        'Added By Saylee on 18-June 2007
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
    Private Sub ClearControls()
        txtSearch.Text = ""
    End Sub
    Private Sub ControlVisibility1(ByVal Index As Int16)
        'lblFor.Visible = (Index <> 0)
        txtSearch.Visible = (Index <> 0)
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        ' lblToDate.Visible = True
        lblVendor.Visible = True
        lblQuotNo1.Visible = True
        lblStatus1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblQuotationType1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblVendor.Visible = False
        lblQuotNo1.Visible = False
        lblStatus1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
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
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If
        If txtSupplier.Text = "" Then
            Supplier = ""
            'lblVendor.Text = "Vendor : All"
            lblVendor.Text = IIf(mType = 1, "Customer : All", "Supplier : All")
        Else
            Supplier = txtSupplier.Text
            lblVendor.Text = "Vendor :  " & Supplier
            lblVendor.Text = IIf(mType = 1, "Customer :  " & Supplier, "Supplier :  " & Supplier)
        End If
        QuotationText = IIf(txtQuotationText.Text <> "", txtQuotationText.Text.Trim, "")
        QuotationNo = txtNo.Text
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        QuotationType = IIf(cmbQuotationType.SelectedIndex > 0, cmbQuotationType.SelectedItem.Text, "")
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblQuotNo1.Text = "Quotation No.  : " & IIf(QuotationText <> "", QuotationText + "-" + QuotationNo, "All")
        lblStatus1.Text = "Status  : " & IIf(Status <> "", Status, "All")
        lblQuotationType1.Text = "Quotation Type :" & IIf(QuotationType <> "", QuotationType, "All")
        Session("mType") = mType
        If mType = 1 Then
            lblQuotationType1.Text = "Quotation Type :" & IIf(QuotationType <> "", QuotationType, "Sales Quotation")
        End If


        mCompleteSearchingCriteria = lblQuotationType1.Text + ", " + lblDateRangeFrom.Text + ", " + lblQuotNo1.Text + ", " + lblVendor.Text + ", " + lblStatus1.Text + ", " + _
                        IIf(chkDetail.Checked, "Detailed Report", "") + " Format : " + IIf(optLandscape.Checked, "LandScape", "Portrait") + lblPartNo.Text + lblDesc.Text

    End Sub
    
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForQuotation
        Dim objReg As rptQuotationRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsQuotation As New dsQuotation
        Dim ReportDetails As New rptStatusList
        Dim QuotationText1 As String = ""

        SetValues()
        QuotationNo = IIf(QuotationNo <> "", QuotationNo, "0")
        If chkDetail.Checked Then
            If optPortrait.Checked Then
                myReport = New crptQuotationRegister
            Else
                myReport = New crptQuotationRegisterLandscape
            End If
            'If Type = 0 Then
            objReg = rptQuotationRegister.GetQuotationList(QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue), CInt(cmbQuotationType.SelectedValue))
            objSearch = rptSearchingCriteriaForQuotation.GetSearchingCriteriaForQuotation(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, Status, CInt(cmbQuotationType.SelectedValue), AppSettings("Logo"))
            'Else
            '    objReg = rptQuotationRegister.GetQuotationList(QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue), 2)
            '    objSearch = rptSearchingCriteriaForQuotation.GetSearchingCriteriaForQuotation(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, Status, 2)
            'End If

            If txtQuotationText.Text <> "" And txtNo.Text <> "" Then
                QuotationText1 = Trim(txtQuotationText.Text) + "-" + txtNo.Text
            ElseIf txtQuotationText.Text <> "" And txtNo.Text = "" Then
                QuotationText1 = Trim(txtQuotationText.Text)
            ElseIf txtQuotationText.Text = "" And txtNo.Text = "" Then
                QuotationText1 = ""
            End If




            ReportDetails.Add(New rptStatus(, , QuotationText1, AppSettings("Logo")))



            If objReg.Count <= 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfrptQuotationRegister.aspx?Type=" & Type
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
                'Added By Utkarsh On 7-Jun-2011 For All07062011

            ElseIf objReg.Count > 0 Then

                If mType = 0 Then
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 602)
                ElseIf mType = 1 Then
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 635)
                End If

                '*******************************

            End If
            dsQuotation.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dsQuotation) 'Added by Shweta on 17-Feb-2012
            da.Fill(dsQuotation, objReg)
            da.Fill(dsQuotation, objSearch)
            da.Fill(dsQuotation, ReportDetails)
            da.Fill(dsQuotation, mrptImage) 'Added by Shweta on 17-Feb-2012
            myReport.SetDataSource(dsQuotation)
        Else
            If optPortrait.Checked Then
                myReport = New crptQuotationRegSummary
            Else
                myReport = New crptQuotationRegSummaryLandscape
            End If

            'If Type = 0 Then
            objReg = rptQuotationRegister.GetQuotationList(QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue), CInt(cmbQuotationType.SelectedValue))
            objSearch = rptSearchingCriteriaForQuotation.GetSearchingCriteriaForQuotation(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, Status, CInt(cmbQuotationType.SelectedValue), AppSettings("Logo"))
            'Else
            '    objReg = rptQuotationRegister.GetQuotationList(QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue), 2)
            '    objSearch = rptSearchingCriteriaForQuotation.GetSearchingCriteriaForQuotation(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, Status, 2)
            'End If

            If objReg.Count <= 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                'msg1.ReplacePage = "wfrptQuotationRegister.aspx?Type=" & Type
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub

                'Added By Utkarsh On 7-Jun-2011 For All07062011

            ElseIf objReg.Count > 0 Then

                If mType = 0 Then
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 602)
                ElseIf mType = 1 Then
                    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 635)
                End If


                '*******************************
            End If
            dsQuotation.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dsQuotation) 'Added by Shweta on 17-Feb-2012
            da.Fill(dsQuotation, mrptImage) 'Added by Shweta on 17-Feb-2012
            da.Fill(dsQuotation, objReg)
            da.Fill(dsQuotation, objSearch)
            myReport.SetDataSource(dsQuotation)
        End If
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        If mType = 0 Then
            MarkLog(Util.Action.Print, "QuotationReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)   '602
        ElseIf mType = 1 Then
            MarkLog(Util.Action.Print, "SalesQuotationReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '635
        End If

    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchQuotationRegister"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Text", QuotationText)
        cmd.Parameters.AddWithValue("@No", QuotationNo)
        cmd.Parameters.AddWithValue("@FromDate", FromDate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@ItemDescription", Description)
        cmd.Parameters.AddWithValue("@StatusID", CInt(cmbStatus.SelectedValue))
        cmd.Parameters.AddWithValue("@TransTypeID", CInt(cmbQuotationType.SelectedValue))

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        dataTable.Columns.Remove("Rem1")
        dataTable.Columns.Remove("Rem2")
        dataTable.Columns.Remove("Rem3")
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsQuotation

        Dim objSearch As rptSearchingCriteriaForQuotation
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        objSearch = rptSearchingCriteriaForQuotation.GetSearchingCriteriaForQuotation(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), QuotationText, QuotationNo, FromDate, ToDate, Supplier, PartNo, Description, Status, CInt(cmbQuotationType.SelectedValue), AppSettings("Logo"))


        ds.Clear()
        da.Fill(ds, objSearch)

        Dim columnToRemove As String() = {"ID", "CompanyName", "InternalReceiptNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ShowLogo", "BaseCurrencySymbol"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptSearchingCriteriaForQuotation").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptSearchingCriteriaForQuotation").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("rptSearchingCriteriaForQuotation"))
        dsNew.Merge(tbl)

        dsNew.Tables("rptSearchingCriteriaForQuotation").Columns("ItemName").ColumnName = "Part No."
        dsNew.Tables("rptSearchingCriteriaForQuotation").Columns("ItemDescription").ColumnName = "Part Description"
        dsNew.Tables("rptSearchingCriteriaForQuotation").Columns("VendorName").ColumnName = IIf(mType = 0, "Supplier", "Customer")
        dsNew.Tables("rptSearchingCriteriaForQuotation").Columns("QuotationText").ColumnName = "Quotation Text"
        dsNew.Tables("rptSearchingCriteriaForQuotation").Columns("QuotationNo").ColumnName = "Quotation No."
        dsNew.Tables("rptSearchingCriteriaForQuotation").Columns("FromDate").ColumnName = "From Date"
        dsNew.Tables("rptSearchingCriteriaForQuotation").Columns("ToDate").ColumnName = "To Date"

        dsNew.Tables("TMainReport").Columns("SuppName").ColumnName = IIf(mType = 0, "To Supplier", "To Customer")
        dsNew.Tables("TMainReport").Columns("AmountBaseCurr").ColumnName = "Amount (in " + objSearch(0).BaseCurrencySymbol + ")"

        dsNew.Tables("rptSearchingCriteriaForQuotation").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = IIf(mType = 0, "Purchase Quotation Register", "Sales Quotation Register")
		Session("ExcelFileName") = dsNew.Tables("TMainReport").TableName
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "QuotationReg", "Export To Excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub FindNow(ByVal LookInType As Integer, ByVal ItemName As String, ByVal Description As String)
        mItemList = Nothing
        'dgPartSearch.DataSource = Nothing
        mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
        'dgPartSearch.DataSource = mItemList
        'dgPartSearch.DataBind()
        Session("mItemList") = mItemList
        'lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub cmbQuotationTypeFill()
        Me.cmbQuotationType.Items.Remove(New ListItem("(All)", "00"))
        Me.cmbQuotationType.Items.Remove(New ListItem("Outright", "33"))
        Me.cmbQuotationType.Items.Remove(New ListItem("Overhaul / Repair", "36"))
        Me.cmbQuotationType.Items.Remove(New ListItem("Rental / Lease", "37"))
        Me.cmbQuotationType.Items.Add(New ListItem("Sales Quotation", "2"))
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mQuotationTextList = DistinctTextListForQuotation.GetDistinctTextList("8", 0, True, "(All)")
        'cmbQuoNo.DataSource = mQuotationTextList

        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        'dgPartSearch.DataSource = mItemList

        Session("mVendorList") = mVendorList
        Session("mQuotationTextList") = mQuotationTextList
        Session("mItemList") = mItemList
        DataBind()
    End Sub
    Public Sub NewPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        'dgPartSearch.CurrentPageIndex = e.NewPageIndex
        ''mItemList = ItemList.GetItemList(cmbSearch.SelectedIndex, txtSearchFor.Text.Trim, txtSearchFor.Text.Trim, "", "", "", "", False)
        'dgPartSearch.DataSource = mItemList
        Session("mItemList") = mItemList
        'dgPartSearch.DataBind()
        'lblResult.Text = "List of Part No.s: " & mItemList.Count & " Record(s) found."
        'SetFocus(dgPartSearch)
    End Sub
#End Region


#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()

            If cmbQuotationType.Enabled = True Then
                setFocus(cmbQuotationType)
            End If

            DataFieldBind()

            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            mType = CType(Request.QueryString("Type"), Integer)
            Session("mType") = mType

            Me.cmbQuotationType.Items.Clear()

            If mType = 0 Then             'Supplier
                cmbQuotationType.Items.Add(New ListItem("(All)", "00"))
                If User.IsInRole("PurchaseQuotationRegOutrightView") Then cmbQuotationType.Items.Add(New ListItem("Outright", "33"))
                If User.IsInRole("PurchaseQuotationRegRepairOverHaulView") Then cmbQuotationType.Items.Add(New ListItem("Overhaul / Repair", "36"))
                If User.IsInRole("PurchaseQuotationRegRentailLeaseView") Then cmbQuotationType.Items.Add(New ListItem("Rental / Lease", "37"))
                mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
                'cmbSupplier.DataSource = mVendorList
                Session("mVendorList") = mVendorList
                DataBind()
                lbltitle.Text = "Purchase Quotation Register"
                lblStep4.Text = "Step IV. Selection of Supplier"
                lblSupplier.Text = "Supplier"
                btnClose.ToolTip = "Click to close the Purchase Quotation Register screen"
            ElseIf mType = 1 Then          'Customer
                If User.IsInRole("SalesQuotationRegView") Then cmbQuotationType.Items.Add(New ListItem("Sales Quotation", "2"))
                mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", True, False, False)
                'cmbSupplier.DataSource = mVendorList
                Session("mVendorList") = mVendorList
                DataBind()
                lbltitle.Text = "Sales Quotation Register"
                lblStep4.Text = "Step IV. Selection of Customer"
                lblSupplier.Text = "Customer"
                btnClose.ToolTip = "Click to close the Sales Quotation Register screen"
            End If
        End If
        'lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Index As Int16 = IIf(txtSearch.Text = "", 0, txtSearch.Text)
        ClearControls()
        ControlVisibility1(Index)
        If txtSearch.Enabled = True Then
            setFocus(txtSearch)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    'Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    dgPartSearch.CurrentPageIndex = 0
    '    SetValues()
    '    PartNo = IIf(txtSearch.SelectedIndex = 1, Trim(txtSearchFor.Text), "")
    '    Description = IIf(cmbSearch.SelectedIndex = 2, Trim(txtSearchFor.Text), "")
    '    Session("PartNo") = PartNo
    '    Session("Description") = Description
    '    FindNow(cmbSearch.SelectedIndex, PartNo, Description)
    '    ControlVisibility3()
    'End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    'Private Sub dgPartSearch_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
    '    Dim Index As Int16 = e.Item.ItemIndex + dgPartSearch.CurrentPageIndex * dgPartSearch.PageSize
    '    Select Case e.CommandName
    '        Case "Select"
    '            ClearControls()
    '            PartNo = mItemList(Index).Name
    '            Description = mItemList(Index).Description
    '            Session("PartNo") = PartNo
    '            Session("Description") = Description
    '            SetFocus(dgPartSearch)
    '            ControlVisibility3()
    '    End Select
    'End Sub
    'Private Sub cmbQuoNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    txtNo.Text = ""
    '    txtNo.Visible = IIf(cmbQuoNo.SelectedIndex > 0, True, False)
    '    If cmbQuoNo.Enabled = True Then
    '        SetFocus(cmbQuoNo)
    '    End If
    'End Sub
    'Private Sub dgPartSearch_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
    '    'Added By Rahul 18-June-2009 for grid sorting
    '    mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    '    Session("mItemList") = mItemList
    '    dgPartSearch.DataSource = mItemList
    '    dgPartSearch.DataBind()
    'End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        SetValues()
        GenerateXLSXFile(CreateDataTable())
    End Sub

#End Region

End Class