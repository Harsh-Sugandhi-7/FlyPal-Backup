Public Class wfrptEnquiryRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mVendor As Vendor
    Public mItemList As ItemList
    Public mVendorList As VendorList
    Public mDistinctTextListForEnquiry As DistinctTextListForEnquiry
    Public EnqText As String = ""
    Public EnqNo As String = ""
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public PartNo As String = ""
    Public Description As String = ""
    Public Supplier As String = ""
    Public Status As String = ""
    Dim EnquiryType1 As String
    Dim TypeOfEnquiry As Integer
    'Private User As System.Security.Principal.IPrincipal 'Added By Prashant 14/09/07

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
        TypeOfEnquiry = CType(Session("TypeOfEnquiry"), Integer)
    End Sub
    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
        Session("mItemList") = mItemList
        Session("TypeOfEnquiry") = TypeOfEnquiry
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
        Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("TypeOfEnquiry")
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
        'txtSearchFor.Text = ""
    End Sub
    Private Sub ControlVisibility1(ByVal Index As Int16)
        'lblFor.Visible = (Index <> 0)
        txtSearch.Visible = (Index <> 0)
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        ' lblToDate.Visible = True
        lblEnquiryNo.Visible = True
        lblVendor.Visible = True
        lblStatus1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblEnquiryType.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblEnquiryNo.Visible = True
        lblVendor.Visible = False
        lblStatus1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
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
            lblVendor.Text = IIf(TypeOfEnquiry = 1, "Customer : All", "Supplier : All")
        Else
            'mVendor = Vendor.GetVendor(New Guid(txtSupplier.Text)) ''shweta 
            'Supplier = mVendor.Name
            Supplier = txtSupplier.Text
            lblVendor.Text = "Vendor :  " & Supplier
            lblVendor.Text = IIf(TypeOfEnquiry = 1, "Customer :  " & Supplier, "Supplier :  " & Supplier)
        End If
        EnqText = IIf(txtEnquiryText.Text <> "", Trim(txtEnquiryText.Text), "")
        EnqNo = txtOrderNo.Text
        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
        'PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        'Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        EnquiryType1 = IIf(cmbEnquiryType.SelectedIndex > 0, cmbEnquiryType.SelectedItem.Text, "")

        Session("PartNo") = PartNo
        Session("Description") = Description

        lblPartNo.Text = "Part No.       : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description    : " & IIf(Description <> "", Description, "All")
        lblEnquiryNo.Text = "Enquiry No. : " & IIf(EnqText + EnqNo <> "", EnqText + "-" + EnqNo, "All")
        lblStatus1.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblEnquiryType.Text = "EnquiryType : " & IIf(EnquiryType1 <> "", EnquiryType1, "All")

        TypeOfEnquiry = CType(Request.QueryString("Type"), Integer)
        Session("TypeOfEnquiry") = TypeOfEnquiry

        If TypeOfEnquiry = 1 Then
            lblEnquiryType.Text = "EnquiryType : " & IIf(EnquiryType1 <> "", EnquiryType1, "Sales Enquiry")
        End If

        mCompleteSearchingCriteria = lblEnquiryType.Text + ", " + lblDateRange.Text + ", " + lblEnquiryNo.Text + ", " + lblVendor.Text + ", " + lblStatus1.Text + ", " + _
                                 IIf(chkDetail.Checked, "Detailed Report", "") + ", " + " Format " + IIf(optLandscape.Checked, "LandScape", "Portrait") + ", " + lblPartNo.Text + ", " + lblDesc.Text

    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExcelrptfetchEnquiryRegister"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Text", EnqText)
        cmd.Parameters.AddWithValue("@No", EnqNo)
        cmd.Parameters.AddWithValue("@FromDate", FromDate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@ItemDescription", Description)
        cmd.Parameters.AddWithValue("@StatusID", CInt(cmbStatus.SelectedValue))
        cmd.Parameters.AddWithValue("@TransTypeID", CInt(cmbEnquiryType.SelectedValue))

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
        Dim ds As New dsEnquiry

        Dim objSearch As rptSearchingCriteriaForEnquiry
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        objSearch = rptSearchingCriteriaForEnquiry.GetSearchingCriteriaForEnquiry(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), EnqText, EnqNo, FromDate, ToDate, Supplier, PartNo, Description, Status, CInt(cmbEnquiryType.SelectedValue), AppSettings("Logo"))


        ds.Clear()
        da.Fill(ds, objSearch)

        Dim columnToRemove As String() = {"ID", "CompanyName", "InternalReceiptNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ShowLogo"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptSearchingCriteriaForEnquiry").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptSearchingCriteriaForEnquiry").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("rptSearchingCriteriaForEnquiry"))
        dsNew.Merge(tbl)

        dsNew.Tables("rptSearchingCriteriaForEnquiry").Columns("ItemName").ColumnName = "Part No."
        dsNew.Tables("rptSearchingCriteriaForEnquiry").Columns("ItemDescription").ColumnName = "Part Description"
        dsNew.Tables("rptSearchingCriteriaForEnquiry").Columns("VendorName").ColumnName = IIf(TypeOfEnquiry = 0, "Supplier", "Customer")
        dsNew.Tables("rptSearchingCriteriaForEnquiry").Columns("EnquiryText").ColumnName = "Enquiry Text"
        dsNew.Tables("rptSearchingCriteriaForEnquiry").Columns("EnquiryNo").ColumnName = "Enquiry No."
        dsNew.Tables("rptSearchingCriteriaForEnquiry").Columns("FromDate").ColumnName = "From Date"
        dsNew.Tables("rptSearchingCriteriaForEnquiry").Columns("ToDate").ColumnName = "To Date"

        dsNew.Tables("TMainReport").Columns("SuppName").ColumnName = IIf(TypeOfEnquiry = 0, "From Supplier", "From Customer")

        dsNew.Tables("rptSearchingCriteriaForEnquiry").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = IIf(TypeOfEnquiry = 0, "Purchase Enquiry Register", "Sales Enquiry Register")
		Session("ExcelFileName") = dsNew.Tables("TMainReport").TableName
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Shital on 18-Jan-2021
        If TypeOfEnquiry = 0 Then
            MarkLog(Util.Action.Print, "EnquiryReg", "Export To excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf TypeOfEnquiry = 1 Then
            MarkLog(Util.Action.Print, "SalesEnquiryReg", "Export To excel " + mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        '---
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForEnquiry
        Dim objReg As rptEnquiryRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsEnquiry As New dsEnquiry
        Dim ReportDetails As New rptStatusList
        Dim EnquiryText1 As String = ""

        SetValues()
        EnqNo = IIf(EnqNo <> "", EnqNo, "0")

        If chkDetail.Checked Then
            If optPortrait.Checked Then
                myReport = New crptEnquiryRegister
            Else
                myReport = New crptEnquiryRegisterLandscape
            End If
        Else
            If optPortrait.Checked Then
                myReport = New crptEnquiryRegSummary
            Else
                myReport = New crptEnquiryRegSummaryLandscape
            End If
        End If

        objReg = rptEnquiryRegister.GetEnquiryList(EnqText, EnqNo, FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbStatus.SelectedValue), CInt(cmbEnquiryType.SelectedValue))
        objSearch = rptSearchingCriteriaForEnquiry.GetSearchingCriteriaForEnquiry(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), EnqText, EnqNo, FromDate, ToDate, Supplier, PartNo, Description, Status, CInt(cmbEnquiryType.SelectedValue), AppSettings("Logo"))

        If txtEnquiryText.Text <> "" And txtOrderNo.Text <> "" Then
            EnquiryText1 = Trim(txtEnquiryText.Text) + "-" + txtOrderNo.Text
        ElseIf txtEnquiryText.Text <> "" And txtOrderNo.Text = "" Then
            EnquiryText1 = Trim(txtEnquiryText.Text)
        ElseIf txtEnquiryText.Text = "" And txtOrderNo.Text = "" Then
            EnquiryText1 = ""
        End If

        ReportDetails.Add(New rptStatus(, , EnquiryText1))

        If objReg.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfrptEnquiryRegister.aspx?Type=" & Type
            'msg1.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub

            'Added By Utkarsh On 7-Jun-2011 For All07062011

        ElseIf objReg.Count > 0 Then
            If TypeOfEnquiry = 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 601)
            ElseIf TypeOfEnquiry = 1 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 634)
            End If

            '*******************************
        End If


        dsEnquiry.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(dsEnquiry) 'Added by Shweta on 17-Feb-2012

        da.Fill(dsEnquiry, objReg)
        da.Fill(dsEnquiry, objSearch)
        da.Fill(dsEnquiry, ReportDetails)
        da.Fill(dsEnquiry, mrptImage) 'Added by Shweta on 17-Feb-2012

        myReport.SetDataSource(dsEnquiry)
        Session("CrystalReport") = myReport

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        If objReg.Count > 0 Then
            If TypeOfEnquiry = 0 Then
                MarkLog(Util.Action.Print, "EnquiryReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            ElseIf TypeOfEnquiry = 1 Then
                MarkLog(Util.Action.Print, "SalesEnquiryReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        End If


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

    Private Sub addAttributes()
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOrderNo').value,event)")
    End Sub
    Private Sub cmbEnquiryTypeFill()
        Me.cmbEnquiryType.Items.Remove(New ListItem("(All)", "00"))
        Me.cmbEnquiryType.Items.Remove(New ListItem("Outright", "32"))
        Me.cmbEnquiryType.Items.Remove(New ListItem("Overhaul / Repair", "34"))
        Me.cmbEnquiryType.Items.Remove(New ListItem("Rental / Lease", "35"))
        Me.cmbEnquiryType.Items.Add(New ListItem("Sales Enquiry", "1"))
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDistinctTextListForEnquiry = DistinctTextListForEnquiry.GetDistinctTextList("7", 0, True, "(All)") 'Enquiry
        'cmbOrderTextList.DataSource = mDistinctTextListForEnquiry
        mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        'dgPartSearch.DataSource = mItemList

        Session("mDistinctTextListForEnquiry") = mDistinctTextListForEnquiry
        Session("mItemList") = mItemList
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

            If cmbEnquiryType.Enabled = True Then
                setFocus(cmbEnquiryType)
            End If

            DataFieldBind()

            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            TypeOfEnquiry = CType(Request.QueryString("Type"), Integer)
            Session("TypeOfEnquiry") = TypeOfEnquiry

            Me.cmbEnquiryType.Items.Clear()

            If TypeOfEnquiry = 0 Then
                cmbEnquiryType.Items.Add(New ListItem("(All)", "00"))

                If User.IsInRole("PurchaseEnquiryRegOutrightView") Then cmbEnquiryType.Items.Add(New ListItem("Outright", "32"))
                If User.IsInRole("PurchaseEnquiryRegRepairOverHaulView") Then cmbEnquiryType.Items.Add(New ListItem("Overhaul / Repair", "34"))
                If User.IsInRole("PurchaseEnquiryRegRentailLeaseView") Then cmbEnquiryType.Items.Add(New ListItem("Rental / Lease", "35"))

                mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
                Session("mVendorList") = mVendorList

                DataBind()

                lbltitle.Text = "Purchase Enquiry Register"
                lblStep4.Text = "Step IV. Selection of Supplier"
                lblSupplier.Text = "Supplier"
                btnClose.ToolTip = "Click to close the Purchase Enquiry Register"

            ElseIf TypeOfEnquiry = 1 Then

                If User.IsInRole("SalesEnquiryRegView") Then cmbEnquiryType.Items.Add(New ListItem("Sales Enquiry", "1"))

                mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", True, False, False)
                Session("mVendorList") = mVendorList

                DataBind()

                lbltitle.Text = "Sales Enquiry Register"
                lblStep4.Text = "Step IV. Selection of Customer"
                lblSupplier.Text = "Customer"
                btnClose.ToolTip = "Click to close the Sales Enquiry Register"
            End If

            SetFocus(cmbEnquiryType)

        End If

    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
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
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        SetValues()
        GenerateXLSXFile(CreateDataTable())
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
  
    Private Sub cmbOrderTextList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtOrderNo.Text = ""
        txtOrderNo.Visible = IIf(txtEnquiryText.Text <> "", True, False)
        If txtEnquiryText.Enabled = True Then
            SetFocus(txtEnquiryText)
        End If
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    
End Class