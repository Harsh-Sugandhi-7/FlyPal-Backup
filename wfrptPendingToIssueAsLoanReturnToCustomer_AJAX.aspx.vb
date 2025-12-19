
'Created : Saylee
'Dates   : 3-Feb-2014

Imports System.Collections.Generic
Imports Flypal.ItemListAutoComplete
Imports System.Linq

Public Class wfrptPendingToIssueAsLoanReturnToCustomer_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mToStoreList As StoreList
    Public mVendorList As VendorList
    Public mrptPendingToIssueAsLoanTakenFromCustomer As rptPendingToIssueAsLoanTakenFromCustomer
    Public mSupplier As Vendor
    Dim FromDate As String
    Dim ToDate As String
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim FromCustomer As String = ""
    Dim ToStoreName As String = ""
    Dim NameOfToStore As String = ""  '

    Dim mSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mToStoreList = CType(Session("mToStoreList"), StoreList)
        mVendorList = CType(Session("mVendorList"), VendorList)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mToStoreList") = mToStoreList
        Session("mVendorList") = mVendorList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mToStoreList")
        Session.Remove("mVendorList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
        lblVendor1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblFromStore1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then      'Date Range
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If

        If cmbSupplier.SelectedIndex = 0 Then       'From Supplier
            FromCustomer = ""
            lblFromStore1.Text = "From Customer Name : All"
        Else
            mSupplier = Vendor.GetVendor(New Guid(cmbSupplier.SelectedValue))
            FromCustomer = mSupplier.Name
            lblFromStore1.Text = "From Customer Name : " & cmbSupplier.SelectedItem.Text
        End If
        'Commented and Added by Prashant 5-Apr-2013 'ALL05042013
        'ToStoreName = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "")
        If cmbStore.SelectedIndex > 0 Then
            ToStoreName = Store.GetStore(New Guid(cmbStore.SelectedValue.ToString)).Name
        Else
            ToStoreName = ""
        End If
        NameOfToStore = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "")
        '-----------------------------

        'Added By Vikrant On 28-Nov-2012 For ALL28112012
        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description
        'End
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblVendor1.Text = "To Store Name : " & IIf(NameOfToStore <> "", NameOfToStore, "All")

        mSearchingCriteria = lblDateRangeFrom.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblVendor1.Text + ", " + lblFromStore1.Text
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        FromCustomer = ""
        PartNo = ""
        Description = ""
        Session("PartNo") = ""
        Session("Description") = ""
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForReceipt
        SetValues()
        Dim ds As New dsPendingToIssueAsLoanTakenFromSupplier
        myReport = New crptPendingToIssueAsLoanTakenFromCustomer
        mrptPendingToIssueAsLoanTakenFromCustomer = rptPendingToIssueAsLoanTakenFromCustomer.GetPendingToIssueAsLoanTakenFromCustomer(FromDate, ToDate, FromCustomer, ToStoreName, PartNo, Description)
        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), FromDate, ToDate, "", "", "", "", "", "", "", "", "", "", NameOfToStore, "", "", PartNo, Description, "", "", FromCustomer, "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        If mrptPendingToIssueAsLoanTakenFromCustomer.Count <= 0 Then
             MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1115)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptPendingToIssueAsLoanTakenFromCustomer)
        da.Fill(ds, objsearch)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "PendingIssueLoanReturnToCustomer", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

        ResetValues()
    End Sub
 
    Private Sub SetTitle()
        cmbStore.SelectedIndex = 0
        cmbSupplier.SelectedIndex = 0
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'To Store
        mToStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mToStoreList
        Session("mToStoreList") = mToStoreList
        lblStoreCount.Text = "You have " + (mToStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mToStoreList.TotalStorelistCount.ToString + " Store(s)"

        'Supplier
        mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", True)
        cmbSupplier.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
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
            SetTitle()
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
        upnlDateRange.Update()
        upnlValidationsummary.Update()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
        upnlCriteria.Update()
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetItemList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim partlist As ItemListAutoComplete
        partlist = ItemListAutoComplete.GetItemList(prefixText)
        If count = 0 Then
            Return (From c As ItemListAutoCompleteInfo In partlist
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoCompleteInfo In partlist
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region
End Class