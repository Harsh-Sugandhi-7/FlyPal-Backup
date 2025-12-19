Imports System.Collections.Generic
Imports System.Linq
Public Class wfrptUCRComponentReliability_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mUCRReportList As UCRReportList
    Dim ListPartNo As String
    Dim ListCompSerialNo As String
    Dim ListModel As String
    Dim ListSerialNo As String
    Dim StartDate As String
    Dim EndDate As String
    Dim mEventLogDetails As String = String.Empty
    Public mATAList As ATAList
    Public mCustomerID As Guid
#End Region

#Region " Helper Methods "
    Private Sub ResetValues()
        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text
        txtModelNo.Text = ""
        txtSerialNo.Text = ""
        txtCPartNo.Text = ""
        txtCSerialNo.Text = ""
        ListPartNo = ""
        ListCompSerialNo = ""
        ListModel = ""
        ListSerialNo = ""
    End Sub
    Private Sub GetSession()
        ListModel = CType(Session("ListModel"), String)
        ListSerialNo = CType(Session("ListSerialNo"), String)
        ListModel = IIf(IsNothing(ListModel), "", ListModel)
        ListSerialNo = IIf(IsNothing(ListSerialNo), "", ListSerialNo)

        ListPartNo = CType(Session("ListPartNo"), String)
        ListCompSerialNo = CType(Session("ListCompSerialNo"), String)
        ListPartNo = IIf(IsNothing(ListPartNo), "", ListPartNo)
        ListCompSerialNo = IIf(IsNothing(ListCompSerialNo), "", ListCompSerialNo)

        mATAList = CType(Session("mATAList"), ATAList)
    End Sub
    Public Sub SetSession()
        Session("ListModel") = ListModel
        Session("ListSerialNo") = ListSerialNo
        Session("ListPartNo") = ListPartNo
        Session("ListCompSerialNo") = ListCompSerialNo
        Session("mATAList") = mATAList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptUCRComponentReliability_Ajax.aspx?" Then
            Session.Remove("ListModel")
            Session.Remove("ListSerialNo")
            Session.Remove("ListPartNo")
            Session.Remove("ListCompSerialNo")
            Session.Remove("mATAList")
        End If
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblModelNo1.Visible = True
        lblSerialNo1.Visible = True
        lblCPartNo1.Visible = True
        lblCSerialNo1.Visible = True
        lblRemovalFrom.Visible = True
        lblRemovalof.Visible = True

        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        If txtCustomerList.Text.Trim <> "" Then
            If hdnCustomerID.Value <> String.Empty Then
                mCustomerID = New Guid(hdnCustomerID.Value.ToString)
            Else
                mCustomerID = Guid.Empty
            End If
        Else
            mCustomerID = Guid.Empty
        End If

        ListModel = txtModelNo.Text
        ListSerialNo = txtSerialNo.Text
        ListPartNo = txtCPartNo.Text
        ListCompSerialNo = txtCSerialNo.Text

        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = CDate(txtFromDate.Text).ToString(AppSettings("DateFormat"))
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = CDate(txtToDate.Text).ToString(AppSettings("DateFormat"))
        End If

        If (StartDate <> "") Then
            lblDateRangeFrom.Text = "From Date : " & CDate(txtFromDate.Text).ToString(AppSettings("DateFormat"))
        Else
            lblDateRangeFrom.Text = "From Date : "
        End If

        If (EndDate <> "") Then
            lblDateRangeTo.Text = "To Date : " & CDate(txtToDate.Text).ToString(AppSettings("DateFormat"))
        Else
            lblDateRangeTo.Text = "To Date : "
        End If
        lblModelNo1.Text = "Model : " & IIf(ListModel <> "", ListModel, "All")
        lblSerialNo1.Text = "Serial No. : " & IIf(ListSerialNo <> "", ListSerialNo, "All")
        lblCPartNo1.Text = "Part No. : " & IIf(ListPartNo <> "", ListPartNo, "All")
        lblCSerialNo1.Text = "Component Serial No. : " & IIf(ListCompSerialNo <> "", ListCompSerialNo, "All")
        mEventLogDetails = lblDateRangeFrom.Text + "; " + lblDateRangeTo.Text + "; " + "Removal From Info. : " + lblModelNo1.Text + ", " + lblSerialNo1.Text + "; " + "Removal of Info. : " + lblCPartNo1.Text + ", " + lblCSerialNo1.Text
    End Sub
    Public Sub ReportDetail()
        mUCRReportList = UCRReportList.GetList(StartDate, EndDate, FromOrToOrOnModel:=ListModel, FromOrToOrOnSerialNo:=ListSerialNo, OfPart:=ListPartNo, OfCompSerialNo:=ListCompSerialNo, ATAID:=cmbATAChapter.SelectedValue.ToString, SupplierID:=mCustomerID.ToString, IsWarrantyChecked:=chkWarranty.Checked, IsPercentageComparisonChecked:=chkPercentage.Checked)
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        'Session("IsExcel") = IsExcel
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        'mUCRReportList = New tmpHistoryList
        Dim da As New CSLA.Data.ObjectAdapter

        Dim mCompanyDetail As New CompanyDetail

        SetValues()

        RptCommonHistory = New crCommonHistory

        ReportDetail()

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                    mCompanyDetail.WebSite, "UCR Report", txtFromDate.Text, txtToDate.Text, "", txtModelNo.Text, txtSerialNo.Text, AppSettings("Product Version"), AppSettings("SINote"), txtCPartNo.Text, txtCSerialNo.Text, IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.ToString, ""), IIf(chkWarranty.Checked, "Yes", "No"), AppSettings("Logo"), IIf(chkPercentage.Checked, "Yes", "No")) 'Changed By Utkarsh For Report Logo.

        If mUCRReportList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf mUCRReportList.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1386)
        End If

        If IsExcel = True Then  'Excel format
            Dim ds As New dsCompHistory
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "UCRReportList", mUCRReportList)

            Dim columnToRemove As String()

            If chkPercentage.Checked Then
                columnToRemove = {"HistoryType", "ChildValue", "Date", "ParentValue", "mTSIValueFormatted", "mTSOValueFormatted", "StoreReceivedDate", "Type", "WorkOrderNo", "FromOrToOrOnModel", "Of", "ChildValueFormatted", "ParentValueFormatted", "Remark", "AssignedManHours", "RequiredManHours", "TSOValue", "TSOValueFormatted", "LogPageNo", "TSIValue", "TSOOfHours", "TSOOfLanding", "TSOOfDate", "TSIValueFormatted", "TSIOfLanding", "TSOOfCycle", "ATACode", "LastUpdateDateFormatted", "LastUpdateDate", "OrderText", "OrderNo", "ATANomenclature", "PeriodID", "DoneOnDateFormatted", "DoneOnDate", "Type1", "ID", "WarrantyExpiryDate", "WarrantyExpiryDateFormatted", "ReceiptText", "ReceiptNo", "LastWorkScope", "ReleaseNoteNo"}
            Else
                columnToRemove = {"PercentageBHA", "PercentageWorld", "HistoryType", "ChildValue", "Date", "ParentValue", "mTSIValueFormatted", "mTSOValueFormatted", "StoreReceivedDate", "Type", "WorkOrderNo", "FromOrToOrOnModel", "Of", "ChildValueFormatted", "ParentValueFormatted", "Remark", "AssignedManHours", "RequiredManHours", "TSOValue", "TSOValueFormatted", "LogPageNo", "TSIValue", "TSOOfHours", "TSOOfLanding", "TSOOfDate", "TSIValueFormatted", "TSIOfLanding", "TSOOfCycle", "ATACode", "LastUpdateDateFormatted", "LastUpdateDate", "OrderText", "OrderNo", "ATANomenclature", "PeriodID", "DoneOnDateFormatted", "DoneOnDate", "Type1", "ID", "WarrantyExpiryDate", "WarrantyExpiryDateFormatted", "ReceiptText", "ReceiptNo", "LastWorkScope", "ReleaseNoteNo"}
            End If


            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("UCRReportList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("UCRReportList").Columns.Remove(columnToRemove(i))
                End If
            Next

            ds.Tables("UCRReportList").Columns("DateFormatted").SetOrdinal(0)
            ds.Tables("UCRReportList").Columns("StoreReceivedDateFormatted").SetOrdinal(1)
            ds.Tables("UCRReportList").Columns("AssemblySerialNo").SetOrdinal(2)
            ds.Tables("UCRReportList").Columns("RegNo").SetOrdinal(3)
            ds.Tables("UCRReportList").Columns("Description").SetOrdinal(4)
            ds.Tables("UCRReportList").Columns("OfModelOrPart").SetOrdinal(5)
            ds.Tables("UCRReportList").Columns("SerialNo").SetOrdinal(6)
            ds.Tables("UCRReportList").Columns("ATA").SetOrdinal(7)
            ds.Tables("UCRReportList").Columns("Position").SetOrdinal(8)
            ds.Tables("UCRReportList").Columns("Supplier").SetOrdinal(9)
            ds.Tables("UCRReportList").Columns("MROName").SetOrdinal(10)
            ds.Tables("UCRReportList").Columns("PreviousWorkScope").SetOrdinal(11)
            ds.Tables("UCRReportList").Columns("ARCForm").SetOrdinal(12)
            ds.Tables("UCRReportList").Columns("TSIOfHours").SetOrdinal(13)
            ds.Tables("UCRReportList").Columns("WorldMTBUR").SetOrdinal(14)
            If chkPercentage.Checked Then
                ds.Tables("UCRReportList").Columns("PercentageWorld").SetOrdinal(15)
                ds.Tables("UCRReportList").Columns("SelfMTBUR").SetOrdinal(16)
                ds.Tables("UCRReportList").Columns("PercentageBHA").SetOrdinal(17)
                ds.Tables("UCRReportList").Columns("TSIOfCycle").SetOrdinal(18)
                ds.Tables("UCRReportList").Columns("TSIOfDate").SetOrdinal(19)
                ds.Tables("UCRReportList").Columns("Reason").SetOrdinal(20)
                ds.Tables("UCRReportList").Columns("Reference").SetOrdinal(21)
                ds.Tables("UCRReportList").Columns("WarrantyStatus").SetOrdinal(22)
            Else
                ds.Tables("UCRReportList").Columns("SelfMTBUR").SetOrdinal(15)
                ds.Tables("UCRReportList").Columns("TSIOfCycle").SetOrdinal(16)
                ds.Tables("UCRReportList").Columns("TSIOfDate").SetOrdinal(17)
                ds.Tables("UCRReportList").Columns("Reason").SetOrdinal(18)
                ds.Tables("UCRReportList").Columns("Reference").SetOrdinal(19)
                ds.Tables("UCRReportList").Columns("WarrantyStatus").SetOrdinal(20)
            End If


            If ds.Tables("UCRReportList").Columns.Contains("DateFormatted") Then
                ds.Tables("UCRReportList").Columns("DateFormatted").ColumnName = "Date"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("PercentageBHA") Then
                ds.Tables("UCRReportList").Columns("PercentageBHA").ColumnName = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", "% BHA", "% " + AppSettings("ClientCode").ToString)
            End If
            If ds.Tables("UCRReportList").Columns.Contains("PercentageWorld") Then
                ds.Tables("UCRReportList").Columns("PercentageWorld").ColumnName = "% World"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("StoreReceivedDateFormatted") Then
                ds.Tables("UCRReportList").Columns("StoreReceivedDateFormatted").ColumnName = "Store Received Date"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("AssemblySerialNo") Then
                ds.Tables("UCRReportList").Columns("AssemblySerialNo").ColumnName = "MSN"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("RegNo") Then
                ds.Tables("UCRReportList").Columns("RegNo").ColumnName = "Aircraft Reg. Tail Number"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("OfModelOrPart") Then
                ds.Tables("UCRReportList").Columns("OfModelOrPart").ColumnName = "Rotable Part Number"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("SerialNo") Then
                ds.Tables("UCRReportList").Columns("SerialNo").ColumnName = "Serial Number"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("MROName") Then
                ds.Tables("UCRReportList").Columns("MROName").ColumnName = "MRO"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("PreviousWorkScope") Then
                ds.Tables("UCRReportList").Columns("PreviousWorkScope").ColumnName = "Previous Work Scope"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("ARCForm") Then
                ds.Tables("UCRReportList").Columns("ARCForm").ColumnName = "Form"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("TSIOfHours") Then
                ds.Tables("UCRReportList").Columns("TSIOfHours").ColumnName = "TSI"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("WorldMTBUR") Then
                ds.Tables("UCRReportList").Columns("WorldMTBUR").ColumnName = "World MTBUR"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("SelfMTBUR") Then
                ds.Tables("UCRReportList").Columns("SelfMTBUR").ColumnName = IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS", "BHA MTBUR", AppSettings("ClientCode").ToString + " MTBUR")
            End If
            If ds.Tables("UCRReportList").Columns.Contains("TSIOfCycle") Then
                ds.Tables("UCRReportList").Columns("TSIOfCycle").ColumnName = "CSI"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("TSIOfDate") Then
                ds.Tables("UCRReportList").Columns("TSIOfDate").ColumnName = "Days Since Installation"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("Reason") Then
                ds.Tables("UCRReportList").Columns("Reason").ColumnName = "Text"
            End If
            If ds.Tables("UCRReportList").Columns.Contains("WarrantyStatus") Then
                ds.Tables("UCRReportList").Columns("WarrantyStatus").ColumnName = "Warranty Status"
            End If

           

            Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "SearchStr3", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Model"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Serial No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Part"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr7") Then
                ds.Tables("ReportData").Columns("SearchStr7").ColumnName = "Comp Serial No."
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
                ds.Tables("ReportData").Columns("SearchStr8").ColumnName = "ATA"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr9") Then
                ds.Tables("ReportData").Columns("SearchStr9").ColumnName = "Warranty Checked"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr11") Then
                ds.Tables("ReportData").Columns("SearchStr11").ColumnName = "Percentage Comparison Checked"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("UCRReportList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("UCRReportList").TableName = "UCR Report"
			Session("ExcelFileName") = "UCR Report"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "UCRReport", "Export To Excel " + mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList

        DataBind()
    End Sub
#End Region

#Region " EVENTS"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptUCRComponentReliability_Ajax.aspx?"
            DataFieldBind()
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            ResetValues()
            SetSession()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        If IsValid Then
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region " Service Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCustomerList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim type As String = ""
        Dim mVendorListAutoComplete As VendorListAutoComplete = VendorListAutoComplete.GetVendorListAutoComplete(prefixText, type)
        If count = 0 Then
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).ToArray
        Else
            Return (From c As VendorListAutoComplete.VendorListAutoCompleteInfo In mVendorListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.VendorID.ToString())).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetModelList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim list As ModelListAutoComplete
        list = ModelListAutoComplete.GetModelList(prefixText, 1)
        If count = 0 Then
            Return (From c As ModelListAutoComplete.ModelListAutoCompleteInfo In list
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.ToString())).ToArray
        Else
            Return (From c As ModelListAutoComplete.ModelListAutoCompleteInfo In list
                      Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region


End Class