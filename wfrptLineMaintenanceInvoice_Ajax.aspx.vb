Public Class wfrptLineMaintenanceInvoice_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public FromDate As String = String.Empty
    Public ToDate As String = String.Empty
    Public Supplier As String = String.Empty
    Public InvText As String = String.Empty
    Public InvNo As String = String.Empty
#End Region

#Region " Business Properties and Methods "
    Private Sub ControlVisibility(ByVal Index As Int16)
        If Index = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        Else
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblInvoiceNo.Visible = True
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
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
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))  '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Status     : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If
        Supplier = txtSupplier.Text.Trim
        lblVendor.Text = "Supplier :  " & IIf(Supplier = "", "All", Supplier)
        InvText = IIf(txtOrderTextList.Text <> "", Trim(txtOrderTextList.Text), "")
        InvNo = txtOrderNo.Text.Trim

        lblInvoiceNo.Text = "Invoice No.: " & IIf(InvText + InvNo <> "", InvText + "-" + InvNo, "All")
    End Sub
    Public Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForReceipt
        Dim rpt As rptLineMaintenanceInvoiceRegister
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsLineMaintInvoice As New dsLineMaintenanceInvoice
        SetValues()
        myReport = New crptLineMaintenanceInvoiceRegister

        rpt = rptLineMaintenanceInvoiceRegister.GetLineMaintenanceInvoiceList(FromDate, ToDate, Supplier, InvText, InvNo)
        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), IIf(FromDate = "1-1-1900", "", FromDate), IIf(ToDate = "1-1-2200", "", ToDate), "", "", "", "", "", "", "", "", "", Supplier, "", "", "", "", "", InvText, InvNo, "", "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf rpt.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1256)
        End If
        dsLineMaintInvoice.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(dsLineMaintInvoice)
            da.Fill(dsLineMaintInvoice, mrptImage)
        End If

        da.Fill(dsLineMaintInvoice, rpt)
        da.Fill(dsLineMaintInvoice, objsearch)
        myReport.SetDataSource(dsLineMaintInvoice)

        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub addAttributes()
        txtOrderNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOrderNo').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        If Not IsPostBack Then
            'DataFieldBind()
            ControlVisibility(1)
            setDatePeroid(1)
            cmbDateRange.SelectedIndex = 1
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            cmbDateRange.Focus()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid Then
            ControlVisibility2()
            SetValues()
            upnlSearchingCriteria.Update()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid() Then
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
#End Region

End Class