Imports System.Text
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfFaultFoundRecordList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public EventLogDetails As String = String.Empty
    Dim mCompanyDetail As New CompanyDetail
    Dim mVendorList As VendorList
    Dim PartNo, Description, SerialNo As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mVendorList = Session("mVendorList")
    End Sub
    Private Sub SetSession()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorList")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfFaultFoundRecordList_Ajax.aspx?") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                    End If
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "ResetIssuedToEmployee" Then

                    End If
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetValues()
        PartNo = IIf(PartNo <> "", PartNo, "")
        Description = IIf(Description <> "", Description, "")
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As FaultFoundRecordList
        Dim ds As New dsFaultFoundRecordList

        myReport = New crptFaultFoundRecordList

        rpt = FaultFoundRecordList.GetFaultFoundRecordListList(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, PartNo:=PartNo, _
                                                               Description:=Description, SupplierID:=cmbSupplierList.SelectedValue.ToString, _
                                                               FaultFound:=Val(cmbFaultFound.SelectedValue))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1436)
            MarkLog(Util.Action.Print, "NoFaultFound", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, _
                                     mCompanyDetail.Email, mCompanyDetail.WebSite, IIf(cmbFaultFound.SelectedIndex > 0, "Fault Found" + " (" + cmbFaultFound.SelectedItem.ToString + ")", "Fault Found"), _
                                     txtFromDate.Text.Trim, txtToDate.Text.Trim, "", SearchStr4:=PartNo, SearchStr5:=Description, _
                                     ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
                                     SearchStr6:=IIf(cmbSupplierList.SelectedIndex > 0, cmbSupplierList.SelectedItem.ToString, ""), _
                                     SearchStr7:=IIf(cmbFaultFound.SelectedIndex > 0, cmbFaultFound.SelectedItem.ToString, ""), _
                                     SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), SearchStr11:="")

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
            da.Fill(ds, "FaultFoundRecordList", rpt)

            Dim columnToRemove2 As String() = {"SearchStr3", "ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"OrderText", "OrderNo", "Amend", "OrderDate", "ReceiptText", "ReceiptNo", "ReceiptDate", "FaultFound"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("FaultFoundRecordList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("FaultFoundRecordList").Columns.Remove(columnToRemove(i))
                End If
            Next
            If ds.Tables("FaultFoundRecordList").Columns.Contains("OrderNumber") Then
                ds.Tables("FaultFoundRecordList").Columns("OrderNumber").ColumnName = "Order No."
            End If
            If ds.Tables("FaultFoundRecordList").Columns.Contains("PartNo") Then
                ds.Tables("FaultFoundRecordList").Columns("PartNo").ColumnName = "Part No."
            End If
            If ds.Tables("FaultFoundRecordList").Columns.Contains("SerialNo") Then
                ds.Tables("FaultFoundRecordList").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("FaultFoundRecordList").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("FaultFoundRecordList").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("FaultFoundRecordList").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("FaultFoundRecordList").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
            End If
            If ds.Tables("FaultFoundRecordList").Columns.Contains("ReceiptNumber") Then
                ds.Tables("FaultFoundRecordList").Columns("ReceiptNumber").ColumnName = "Receipt No."
            End If
            If ds.Tables("FaultFoundRecordList").Columns.Contains("FaultFoundYesNo") Then
                ds.Tables("FaultFoundRecordList").Columns("FaultFoundYesNo").ColumnName = "Y/N"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Part No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Description"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Supplier"
            End If

            Dim dsNew As New DataSet

            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("FaultFoundRecordList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("FaultFoundRecordList").TableName = IIf(cmbFaultFound.SelectedIndex > 0, "Fault Found" + " (" + cmbFaultFound.SelectedItem.ToString + ")", "Fault Found")
			Session("ExcelFileName") = "Fault Found"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            MarkLog(Util.Action.Print, "NoFaultFound", IIf(IsExcel = True, "Export To excel ", "") + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If
    End Sub
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", False, True)
        Session("mVendorList") = mVendorList
        cmbSupplierList.DataSource = mVendorList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfFaultFoundRecordList_Ajax.aspx?"
            txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-2)).ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
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
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
End Class