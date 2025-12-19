Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfrptAdvanceCoreReturns_Ajax
    Inherits System.Web.UI.Page

#Region " Variables "
    Public mItem As Item
    Public PartNo As String = ""
    Public Description As String = ""
    Public SerialNo As String = ""
    Public EventLogDetails As String = String.Empty
    Dim mCompanyDetail As New CompanyDetail
    Dim FromDate As String = ""
    Dim ToDate As String = ""
    Dim mVendorList As VendorList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorList = Session("mVendorList")
    End Sub
    Private Sub SetSession()
        Session("mVendorList") = mVendorList
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        End If
    End Sub
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblSerialNo1.Visible = True
        upnlSerachCriteria.Update()
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
                txtFromDate.Text = CDate(Today.AddDays(1).AddYears(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = New SmartDate("01-01-1900").FormattedText
            ToDate = New SmartDate("01-01-2200").FormattedText
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range     : " & FromDate & " To " & ToDate & " ( " & cmbDateRange.SelectedItem.Text & ")"
        End If

        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        If txtSerialNo.Text.Trim <> "" Then
            SerialNo = txtSerialNo.Text.Trim
        Else
            SerialNo = ""
        End If
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblSerialNo1.Text = "Serial No. : " + IIf(SerialNo <> "", SerialNo, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "")
        EventLogDetails = lblPartNo.Text + ", " + lblDesc.Text + ", " + ", " + lblSerialNo1.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As AdvanceCoreReturns
        Dim ds As New dsAdvanceCoreReturns
        If txtSerialNo.Text.Trim <> "" Then
            SerialNo = txtSerialNo.Text.Trim
        Else
            SerialNo = ""
        End If

        myReport = New crptAdvanceCoreReturns

        rpt = AdvanceCoreReturns.GetAdvanceCoreReturnsList(ItemName:=PartNo, SerialNo:=SerialNo, Description:=Description, FromDate:=FromDate, ToDate:=ToDate, VendorID:=cmbSupplier.SelectedValue.ToString)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1394)

            'MarkLog(Util.Action.Print, "AdvanceCoreReturns", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If IsExcel = False Then  'Added by Shital on 18-Jan-2021
                MarkLog(Util.Action.Print, "AdvanceCoreReturns", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        End If

        Dim SearchStr5 As String
        If cmbDateRange.SelectedIndex = 0 Then
            SearchStr5 = ""
        Else
            SearchStr5 = cmbDateRange.SelectedItem.Text + " : " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
       mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
       mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
      "Advance Core Return Part(s)", PartNo, Description, SerialNo, SearchStr4:=cmbSupplier.SelectedItem.ToString, SearchStr5:=SearchStr5, _
        ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", _
        SearchStr10:=AppSettings("Logo"), SearchStr11:="")

        If IsExcel = False Then     'PDF format
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        ElseIf IsExcel = True Then  'Excel format
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "AdvanceCoreReturns", rpt)

            Dim columnToRemove2 As String() = {"SearchStr4", "ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"IssueText", "IssueNo", "IssueDate", "OrderText", "OrderNo", "OrderAmend", "OrderDate"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("AdvanceCoreReturns").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("AdvanceCoreReturns").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("AdvanceCoreReturns").Columns.Contains("ItemName") Then
                ds.Tables("AdvanceCoreReturns").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("AdvanceCoreReturns").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("AdvanceCoreReturns").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("AdvanceCoreReturns").Columns.Contains("IssueDateFormatted") Then
                ds.Tables("AdvanceCoreReturns").Columns("IssueDateFormatted").ColumnName = "Issue Date"
            End If
            If ds.Tables("AdvanceCoreReturns").Columns.Contains("IssueItemRemark") Then
                ds.Tables("AdvanceCoreReturns").Columns("IssueItemRemark").ColumnName = "Remark"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Part No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Description"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Serial No."
            End If


            Dim dsNew As New DataSet

            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("AdvanceCoreReturns"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("AdvanceCoreReturns").TableName = "Advance Core Return Part(s)"
			Session("ExcelFileName") = "Advance Core Return Part"
			Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "AdvanceCoreReturns", "Export To excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        'Supplire  List
        mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", , IsSupplier:=True)
        cmbSupplier.DataSource = mVendorList
        Session("mVendorList") = mVendorList

      
        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        GetSession()
        If Not IsPostBack Then

            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetValues()
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetValues()
        SetReport(True)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
#End Region

#Region " Service Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As ItemListAutoComplete
        itemlist = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetSerialNo(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        'Dim partID As String = contextKey.Split("=")(1)
        'Dim mItem As Item = Item.GetItem(New Guid(partID))
        Dim mSerialNoListAutoComplete As SerialNoListAutoComplete = SerialNoListAutoComplete.GetSerialNoList(prefixText)
        If count = 0 Then
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete Select c.SerialNo).ToArray
        Else
            Return (From c As SerialNoListAutoComplete.SerialNoListAutoCompleteInfo In mSerialNoListAutoComplete
               Select c.SerialNo).Take(count).ToArray
        End If
    End Function
#End Region

End Class