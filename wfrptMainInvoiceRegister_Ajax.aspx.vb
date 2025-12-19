
Public Class wfrptMainInvoiceRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variables Declaration "
    Private mItemList As ItemList
    Private mVendor As Vendor
    Private mVendorList As VendorList
    Private FromDate As String
    Private ToDate As String
    Private PartNo As String
    Private Description As String
    Private strChargesFor As String
    Private strVendor As String
    Private VendorInvFDate As String
    Private VendorInvTDate As String
    Public InvoiceText As String
    Public InvoiceNo As Integer
    Public mDistinctTextListForMaintenanceInvoice As DistinctTextListForMaintenanceInvoice
    Dim Supplier As String = ""

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "

    Private Sub GetSession()
        ''mVendorList = CType(Session("mVendorList"), VendorList)
        ''mItemList = Session("mItemList")
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mDistinctTextListForMaintenanceInvoice = Session("mDistinctTextListForMaintenanceInvoice")
    End Sub
    Private Sub SetSession()
        ''Session("mVendorList") = mVendorList
        ''Session("mItemList") = mItemList
        ''Session("PartNo") = PartNo
        ''Session("Description") = Description
        ''Session("mDistinctTextListForMaintenanceInvoice") = mDistinctTextListForMaintenanceInvoice
    End Sub
    Private Sub RemoveSession()
        ''Session.Remove("mVendorList")
        ''Session.Remove("mItemList")
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mDistinctTextListForMaintenanceInvoice")
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
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)
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
        ''txtSearchFor.Text = ""
    End Sub
    Private Sub ControlVisibility1(ByVal Index As Int16)
        ''lblFor.Visible = (Index <> 0)
        ''txtSearchFor.Visible = (Index <> 0)
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblInvoiceDateRange.Visible = True
        lblVendor.Visible = True
        lblChargesForDisp.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblMainInvNo.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblInvoiceDateRange.Visible = False
        lblVendor.Visible = False
        lblChargesForDisp.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblMainInvNo.Visible = False
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
        If cmbDateRange.SelectedIndex = 0 Then          'Date Range
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            If txtFromDate.Text.ToString = "" And txtToDate.Text.ToString <> "" Then
                txtFromDate.Text = txtToDate.Text
            ElseIf txtFromDate.Text.ToString <> "" And txtToDate.Text.ToString = "" Then
                txtToDate.Text = txtFromDate.Text
            End If
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        If txtFromInvDate.Text.ToString = "" And txtToInvDate.Text.ToString = "" Then     'Invoice Date Range
            VendorInvFDate = "1-1-1900"
            VendorInvTDate = "1-1-2200"
            lblInvoiceDateRange.Text = "Invoice Date Range : All"
        Else
            If txtFromInvDate.Text.ToString = "" And txtToInvDate.Text.ToString <> "" Then
                txtFromInvDate.Text = txtToInvDate.Text
            ElseIf txtFromInvDate.Text.ToString <> "" And txtToInvDate.Text.ToString = "" Then
                txtToInvDate.Text = txtFromInvDate.Text
            End If
            VendorInvFDate = txtFromInvDate.Text.ToString
            VendorInvTDate = txtToInvDate.Text.ToString
            lblInvoiceDateRange.Text = "Invoice Date From : " & New SmartDate(VendorInvFDate).FormattedText & " To " & New SmartDate(VendorInvTDate).FormattedText
        End If


        strVendor = txtSupplier.Text
        lblVendor.Text = "Supplier : " & strVendor



        ' ''If cmbBy.SelectedItem.Text = "Supplier" Then
        ' ''    strVendor = txtSupplier.Text.Trim
        ' ''    lblVendor.Text = "Supplier : " & strVendor
        ' ''Else
        ' ''    strVendor = ""
        ' ''    lblVendor.Text = "Supplier : " & "(ALL)"
        ' ''End If

        InvoiceText = IIf(cmbMaintenanceInvoiceText.SelectedIndex > 0, Trim(cmbMaintenanceInvoiceText.SelectedItem.Text), "")
        InvoiceNo = Val(txtNo.Text)

        strChargesFor = txtChargesFor.Text.Trim         'Charges for
        lblChargesForDisp.Text = "Charges For : " & IIf(strChargesFor <> "", strChargesFor, "All")

        ''PartNo = IIf(IsNothing(PartNo), "", PartNo)
        ''Description = IIf(IsNothing(Description), "", Description)
        ''lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        ''lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

        'NEWLY ADDED----------------
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        lblPartNo.Text = "Part No. : " & PartNo
        lblDesc.Text = "Description : " & Description
        '---------------------------

        lblMainInvNo.Text = "Main. Inv. No.  : " & IIf(InvoiceText <> "", InvoiceText + "-" + InvoiceNo.ToString, "All")


        mCompleteSearchingCriteria = lblDateRangeFrom.Text + ", " + lblInvoiceDateRange.Text + ", " + lblMainInvNo.Text + ", " + lblInvoiceDateRange.Text + ", " + lblVendor.Text + ", " + lblChargesForDisp.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub

    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim rpt As rptMainInvReg
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsMainInvReg
        myReport = New crptMainInvReg
        Dim str As String
        SetValues()
        rpt = rptMainInvReg.GetMainInvRegList(FromDate, ToDate, strVendor, PartNo, Description, strChargesFor, VendorInvFDate, VendorInvTDate, InvoiceText, InvoiceNo)
        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, "", "", "", InvoiceText, "", "", InvoiceNo.ToString, "", "", strVendor, "", "", "", PartNo, Description, "", "", "", "", "", "", "", strChargesFor, "", VendorInvFDate, VendorInvTDate, 0, "", "", AppSettings("Logo"))
        If rpt.Count <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfrptMainInvoiceRegister.aspx?Backpage="
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011

        ElseIf rpt.Count > 0 Then

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 622)

            '*******************************
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, rpt)
        da.Fill(ds, objSearch)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        'str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", str)
        str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)

        MarkLog(Util.Action.Print, "MaintenanceInvoiceReg", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) '622

    End Sub
    Private Sub FindNow(ByVal LookInType As Integer, ByVal ItemName As String, ByVal Description As String)
        ''mItemList = Nothing
        ''dgPartSearch.DataSource = Nothing
        ''mItemList = ItemList.GetItemList(LookInType, ItemName, Description, "", "", "", "", False)
        ''dgPartSearch.DataSource = mItemList
        ''dgPartSearch.DataBind()
        ''Session("mItemList") = mItemList
        ''lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
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
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub


#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", True, False, True)
        ''mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        ''cmbVendor.DataSource = mVendorList

        mDistinctTextListForMaintenanceInvoice = DistinctTextListForMaintenanceInvoice.GetDistinctText("12", , True, "(All)")
        cmbMaintenanceInvoiceText.DataSource = mDistinctTextListForMaintenanceInvoice

        ''mItemList = ItemList.GetItemList(0, "", "", "", "", "", "", False)
        ''dgPartSearch.DataSource = mItemList
        ''Session("mVendorList") = mVendorList
        ''Session("mItemList") = mItemList
        Session("mDistinctTextListForMaintenanceInvoice") = mDistinctTextListForMaintenanceInvoice

        DataBind()
    End Sub
    Public Sub NewPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        ''dgPartSearch.CurrentPageIndex = e.NewPageIndex
        ''mItemList = ItemList.GetItemList(cmbSearch.SelectedIndex, txtSearchFor.Text.Trim, txtSearchFor.Text.Trim, "", "", "", "", False)
        ''dgPartSearch.DataSource = mItemList
        ''Session("mItemList") = mItemList
        ''dgPartSearch.DataBind()
        ''SetFocus(dgPartSearch)
        ''lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            RemoveSession()

            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If

            DataFieldBind()

            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6

            txtFromInvDate.Text = ""
            txtToInvDate.Text = ""
            upnlSupplierSelection.Update()
        End If
        'SetValues()
        MessageBoxResult()
        ''lblResult.Text = "List of Part No.s : " & mItemList.Count & " Record(s) found."
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ''Dim Index As Int16 = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
        ''ClearControls()
        ''ControlVisibility1(Index)
        ''If cmbSearch.Enabled = True Then
        ''    SetFocus(cmbSearch)
        ''End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgPartSearch_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        ''Dim Index As Int16 = e.Item.ItemIndex + dgPartSearch.CurrentPageIndex * dgPartSearch.PageSize
        ''Select Case e.CommandName
        ''    Case "Select"
        ''        ClearControls()
        ''        PartNo = mItemList(Index).Name
        ''        Description = mItemList(Index).Description
        ''        Session("PartNo") = PartNo
        ''        Session("Description") = Description
        ''        SetFocus(dgPartSearch)
        ''End Select
    End Sub
    Private Sub cmbBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBy.SelectedIndexChanged
        If cmbBy.SelectedIndex = 0 Then
            lblSupplier.Visible = False
            txtSupplier.Visible = False ''
            ''cmbVendor.SelectedIndex = 0
            lblVendor.Text = "Supplier : All"
        Else
            lblSupplier.Visible = True
            txtSupplier.Visible = True ''
            ''cmbVendor.SelectedIndex = 0
        End If
        If cmbBy.Enabled = True Then
            setFocus(cmbBy)
        End If
    End Sub
    Private Sub cmbMaintenanceInvoiceText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMaintenanceInvoiceText.SelectedIndexChanged
        txtNo.Text = ""
        txtNo.Visible = IIf(cmbMaintenanceInvoiceText.SelectedIndex > 0, True, False)
        If cmbMaintenanceInvoiceText.Enabled = True Then
            setFocus(cmbMaintenanceInvoiceText)
        End If
    End Sub
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    '    'Me.txtSupplier.Visible = Not CType(sender, Boolean) ''
    'End Sub
    'Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
    '    'Me.txtSupplier.Visible = Not CType(sender, Boolean) ''
    'End Sub
    'Private Sub txtFromInvDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromInvDate.CalendarVisibleChanged
    '    'Me.txtSupplier.Visible = Not CType(sender, Boolean) ''
    'End Sub
    'Private Sub txtToInvDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToInvDate.CalendarVisibleChanged
    '    'Me.txtSupplier.Visible = Not CType(sender, Boolean) ''
    'End Sub
    Private Sub dgPartSearch_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        ''Added By Rahul 18-June-2009 for grid sorting
        ''mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        ''Session("mItemList") = mItemList
        ''dgPartSearch.DataSource = mItemList
        ''dgPartSearch.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region
End Class