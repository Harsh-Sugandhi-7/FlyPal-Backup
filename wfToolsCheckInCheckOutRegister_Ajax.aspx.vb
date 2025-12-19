Imports System.Linq
Imports System.Linq.Enumerable
Public Class wfToolsCheckInCheckOutRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public EventLogDetails As String = String.Empty
    Dim mCompanyDetail As New CompanyDetail
    Dim mMachineNameValueList As MachineNameValueList
    Dim PartNo, Description, SerialNo As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
    Private Sub SetSession()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfToolsCheckInCheckOutRegister_Ajax.aspx?") <= 0 Then
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
        'Session("PartNo") = PartNo
        'Session("Description") = Description
        'lblSerialNo1.Text = "Serial No. : " + IIf(SerialNo <> "", SerialNo, "All")
        'lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "")
        'lblDesc.Text = "Description : " & IIf(Description <> "", Description, "")
        EventLogDetails = "Date Range: " + txtFromDate.Text.Trim + " " + txtToDate.Text.Trim + ", " + "Requested By: " + txtIssuedToEmployee.Text.Trim + ", " + "Part No.: " & IIf(PartNo <> "", PartNo, "") + ", " + "Description: " & IIf(Description <> "", Description, "") + ", " + "Serial No.: " + IIf(SerialNo <> "", SerialNo, "All")
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As ToolsCheckInCheckOutRegister
        Dim ds As New dsToolsCheckInCheckOutRegister

        myReport = New crptToolsCheckInCheckOutRegister

        rpt = ToolsCheckInCheckOutRegister.GetToolsCheckInCheckOutRegisterList(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, PartNo:=PartNo,
                                                                               Description:=Description, Employee:=txtIssuedToEmployee.Text.Trim,
                                                                               MachineID:=cmbAircraftList.SelectedValue.ToString, SerialNo:=SerialNo)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1401)
            'MarkLog(Util.Action.Print, "ToolsCheckInCheckOutRegister", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "TOOLS CHECK-IN CHECK-OUT REGISTER",
                                     txtFromDate.Text.Trim, txtToDate.Text.Trim, txtIssuedToEmployee.Text.Trim, SearchStr4:=PartNo, SearchStr5:=Description,
                                     ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                     SearchStr6:=IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.ToString, ""), SearchStr7:=txtSerialNo.Text.Trim,
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
            MarkLog(Util.Action.Print, "ToolsCheckInCheckOutRegister", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "ToolsCheckInCheckOutRegister", rpt)

            Dim columnToRemove2 As String() = {"SearchStr4", "ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"IssueDate", "ReceiptDate"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ToolsCheckInCheckOutRegister").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("PartNo") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("PartNo").ColumnName = "PART No."
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("SerialNo") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("SerialNo").ColumnName = "Ser. No."
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("RequestedBy") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("RequestedBy").ColumnName = "Requested By"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("IssueNo") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("IssueNo").ColumnName = "Issue No"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("IssueDateFormatted") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("IssueDateFormatted").ColumnName = "Issue Date"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("RequisitionRef") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("RequisitionRef").ColumnName = "Requisition Ref."
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("IssuedBy") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("IssuedBy").ColumnName = "Issued By"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("CollectedBy") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("CollectedBy").ColumnName = "Collected By"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("ReturnedBy") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("ReturnedBy").ColumnName = "Returned By"
            End If

            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("ReceiptNo") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("ReceiptNo").ColumnName = "Receipt No"
            End If

            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("ReceivedBy") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("ReceivedBy").ColumnName = "Received By"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("PhyCondition") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("PhyCondition").ColumnName = "Phy. Condition"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("IssueRemark") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("IssueRemark").ColumnName = "Issue Remark"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("ReceiptItemRemark") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("ReceiptItemRemark").ColumnName = "Receipt Item Remark"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("ReceiptStoreAndLocation") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("ReceiptStoreAndLocation").ColumnName = "Receive Store & Location"
            End If
            If ds.Tables("ToolsCheckInCheckOutRegister").Columns.Contains("IssueStoreAndLocation") Then
                ds.Tables("ToolsCheckInCheckOutRegister").Columns("IssueStoreAndLocation").ColumnName = "Issue Store & Location"
            End If



            ds.Tables("ToolsCheckInCheckOutRegister").Columns("PART No.").SetOrdinal(0)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Description").SetOrdinal(1)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Ser. No.").SetOrdinal(2)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Requested By").SetOrdinal(3)

            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Issue No").SetOrdinal(4)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Issue Date").SetOrdinal(5)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Issue Store & Location").SetOrdinal(6)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Requisition Ref.").SetOrdinal(7)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Issued By").SetOrdinal(8)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("RegNo").SetOrdinal(9)

            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Collected By").SetOrdinal(10)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Returned By").SetOrdinal(11)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Receipt No").SetOrdinal(12)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Receipt Date").SetOrdinal(13)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Receive Store & Location").SetOrdinal(14)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Received By").SetOrdinal(15)

            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Phy. Condition").SetOrdinal(16)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Remark").SetOrdinal(17)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Issue Remark").SetOrdinal(18)
            ds.Tables("ToolsCheckInCheckOutRegister").Columns("Receipt Item Remark").SetOrdinal(19)



            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Employee"
            End If

            Dim dsNew As New DataSet

            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ToolsCheckInCheckOutRegister"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ToolsCheckInCheckOutRegister").TableName = "TOOLS CHECK-IN CHECK-OUT REGISTER"
			Session("ExcelFileName") = "TOOLS CHECK-IN CHECK-OUT REGISTER"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "ToolsCheckInCheckOutRegister", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(ALL)", ForInventory:=True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraftList.DataSource = mMachineNameValueList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfToolsCheckInCheckOutRegister_Ajax.aspx?"
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

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmployeeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As EmpNoNameAutoComplete
        itemlist = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region



End Class