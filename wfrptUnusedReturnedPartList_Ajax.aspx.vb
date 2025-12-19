'Added By Vikrant on 31-Oct-2013 For All29102013
Public Class wfrptUnusedReturnedPartList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim BlankFromDate As String = ""
    Dim ToDate As String
    Dim BlankToDate As String = ""
    Dim PartNo As String
    Dim Description As String
    Dim IssueTo As String = ""
    Public EventLogDetails As String = String.Empty
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        IssueTo = IIf(IsNothing(IssueTo), "", IssueTo)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("IssueTo")
    End Sub
    Public Sub customValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtFromDate" Then
            If Trim(txtFromDate.Text) = "" Then
                custValidator.ErrorMessage = "Select From Date."
                e.IsValid = False
            ElseIf Not IsDate(txtFromDate.Text) Then
                custValidator.ErrorMessage = "Please Enter valid From date."
                e.IsValid = False
            End If
        End If

        If custValidator.ControlToValidate = "txtToDate" Then
            If Trim(txtToDate.Text) = "" Then
                custValidator.ErrorMessage = "Select To Date."
                e.IsValid = False
            ElseIf Not IsDate(txtToDate.Text) Then
                custValidator.ErrorMessage = "Please Enter valid To date."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub ControlVisibility()
        lblDateRangeFrom.Visible = False
        lblToDate1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
        lblVendorName.Visible = False
        lblIssueTo.Visible = False
    End Sub
    Private Sub setDatePeroid()
        'Last 1 Month
        txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
    End Sub
    Private Sub SetValues()

        FromDate = txtFromDate.Text
        BlankFromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        BlankToDate = txtToDate.Text
        lblDateRangeFrom.Text = "Date Range: " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText
        IssueTo = txtIssueTo.Text.Trim


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
        Session("IssueTo") = IssueTo
        lblPartNo.Text = "Part No.: " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description: " & IIf(Description <> "", Description, "All")
        lblIssueTo.Text = "Issue To: " & IIf(IssueTo <> "", IssueTo, "All")
        EventLogDetails = lblDateRangeFrom.Text + ", " + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblIssueTo.Text
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        PartNo = ""
        Description = ""
        IssueTo = ""
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As UnusedReturnedPartsList
        Dim dsPenOrd As New dsUnusedReturnedParts
        myReport = New crptUnusedReturnedPartList
        GetSession()
        SetValues()

        rpt = UnusedReturnedPartsList.GetUnusedReturnedPartsList(FromDate, ToDate, PartNo, Description, IssueTo)
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                       mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                       mCompanyDetail.WebSite, "Unused Return Part List", New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, PartNo, Description, BlankFromDate, AppSettings("Product Version"), AppSettings("SINote"), BlankToDate, "", "", IssueTo, AppSettings("Logo"))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf rpt.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1280)
        End If
        dsPenOrd.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(dsPenOrd)
        da.Fill(dsPenOrd, rpt)

        da.Fill(dsPenOrd, mrptImage)
        da.Fill(dsPenOrd, Report)
        myReport.SetDataSource(dsPenOrd)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "UnusedReturnPartList", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ResetValues()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    'Response.Redirect("wfrptUnusedReturnedPartList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfrptUnusedReturnedPartList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            RemoveSession()

            ControlVisibility()
            setDatePeroid()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblIssueTo.Visible = True
        lblDateRangeFrom.Visible = True
        lblToDate1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblVendorName.Visible = True
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid() Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Added By Vikrant On 10-Jun-2015
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As UnusedReturnedPartsList
        Dim ds As New dsExcelIssue

        SetValues()

        rpt = UnusedReturnedPartsList.GetUnusedReturnedPartsList(FromDate, ToDate, PartNo, Description, IssueTo)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                       mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                       mCompanyDetail.WebSite, "Unused Return Part List", New SmartDate(FromDate).FormattedText, New SmartDate(ToDate).FormattedText, PartNo, Description, BlankFromDate, AppSettings("Product Version"), AppSettings("SINote"), BlankToDate, "", "", IssueTo, AppSettings("Logo"))

        ds.Clear()
        da.Fill(ds, "ReportData", Report)
        da.Fill(ds, "UnusedReturnedPartsList", rpt)

        Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "SearchStr6", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr10", "SearchStr5", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
        For i As Integer = 0 To columnToRemove2.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
            End If
        Next

        Dim columnToRemove As String() = {"ID", "IssueDate", "ReturnDate", "FromStore", "Location", "TotalAmount", "IssueItemID", "CRate", "CAmount", "ReqDate"}

        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("UnusedReturnedPartsList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("UnusedReturnedPartsList").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ReportData"))
        dsNew.Merge(ds.Tables("UnusedReturnedPartsList"))

        dsNew.Tables("ReportData").Columns("ReportDate").ColumnName = "Report Date"
        dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
        dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
        dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Part No."
        dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Description"
        dsNew.Tables("ReportData").Columns("SearchStr9").ColumnName = "Issue To"

        dsNew.Tables("UnusedReturnedPartsList").Columns("ReturnDateFormatted").ColumnName = "Return Date"
        dsNew.Tables("UnusedReturnedPartsList").Columns("IssueDateFormatted").ColumnName = "Issue Date"
        dsNew.Tables("UnusedReturnedPartsList").Columns("StoreLocation").ColumnName = "Store(Location)"
        dsNew.Tables("UnusedReturnedPartsList").Columns("ReqDateFormatted").ColumnName = "Req. Date"
        dsNew.Tables("UnusedReturnedPartsList").Columns("ReqTextNo").ColumnName = "Req. No."
        dsNew.Tables("UnusedReturnedPartsList").Columns("IssueTo").ColumnName = "Issue To"
        dsNew.Tables("UnusedReturnedPartsList").Columns("IssuedQty").ColumnName = "Issued Qty."

        dsNew.Tables("ReportData").TableName = "Searching Criteria"
        dsNew.Tables("UnusedReturnedPartsList").TableName = "Unused Return Part List"
		Session("ExcelFileName") = "Unused Return Part List"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "UnusedReturnPartList", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    'End
#End Region
    
End Class