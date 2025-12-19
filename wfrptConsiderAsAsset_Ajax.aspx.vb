Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfrptConsiderAsAsset_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public PartNo As String = ""
    Public Description As String = ""
    Public EventLogDetails As String = String.Empty
    Dim email As Thread
    'Added By Abhishek on 6-OCT-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteria
    Dim ds As New dsConsiderAsAsset
    Dim rpt As rptConsiderAsAsset
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub RemoveSession()
       Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Sub Display()
        lblDateRange.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        upnlSerachCriteria.Update()
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
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "")
        lblDateRange.Text = "Date Range : " + txtFromDate.Text + " To  " + txtToDate.Text
        EventLogDetails = lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False, Optional ByVal ByMail As Boolean = False)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptConsiderAsAsset
        GetSession()
        Dim ds As New dsConsiderAsAsset
        myReport = New crptConsiderAsAsset
        SetValues()
        rpt = rptConsiderAsAsset.GetConsiderAsAsset(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, PartName:=PartNo, Description:=Description, IsValuedStore:=chkIsValued.Checked)
        If ByMail = False Then
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1340)
                EventLogDetails = EventLogDetails + ", Date Range : " + txtFromDate.Text + " " + txtToDate.Text
                MarkLog(Util.Action.Print, "AssetItems", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        End If
        If (ByMail = True And rpt.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Asset Items", "Asset Items", "There is no record for this search criteria.", "", _
                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                ReportGeneratedBy:=Session("ReportGenratedBy"), _
                SmtpHost:=mModuleList.Item("AssetItems").SmtpHost, SmtpPort:=mModuleList.Item("AssetItems").SmtpPort, SmtpUser:=mModuleList.Item("AssetItems").SmtpUser, SmtpPassword:=mModuleList.Item("AssetItems").SmtpPassword)

            Exit Sub
        End If
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, PartNo:=PartNo, SupplierName:="", BranchName:=AppSettings("Logo"), Category:="", Nomenclature:="", store:="", Aircraft:="", KitName:="", Description:=Description, RelNoteNo:="")

        ds.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
        End If
        da.Fill(ds, rpt)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        If ByMail = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Else
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Asset Items", "Asset Items", "", "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, _
                                      Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                SmtpHost:=mModuleList.Item("AssetItems").SmtpHost, SmtpPort:=mModuleList.Item("AssetItems").SmtpPort, SmtpUser:=mModuleList.Item("AssetItems").SmtpUser, SmtpPassword:=mModuleList.Item("AssetItems").SmtpPassword)

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
    Private Sub SetPage()
        upnlTitle.Update()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptConsiderAsAsset_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptConsiderAsAsset_Ajax.aspx"
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
            SetPage()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport(False, False)
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session.Remove("PartID")
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(False, True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        If Page.IsValid Then
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

            Session("UserEmailID") = mModuleList.Item("AssetItems").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("AssetItems").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlTitle.Update()
        End If
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
    'Added By Abhishek on 6-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then

            GetSession()
            SetValues()
            rpt = rptConsiderAsAsset.GetConsiderAsAsset(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, PartName:=PartNo, Description:=Description, IsValuedStore:=chkIsValued.Checked)
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, PartNo:=PartNo, SupplierName:="", BranchName:=AppSettings("Logo"), Category:="", Nomenclature:="", store:="", Aircraft:="", KitName:="", Description:=Description, RelNoteNo:="")

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            da.Fill(ds, "rptSearchingCriteria", objsearch)
            da.Fill(ds, "ExcelrptConsiderAsAsset", rpt)

            Dim columnToRemove1 As String() = {"PartID", "Remark", "Type", "CureDate", "ExpiryDate", "OrdQty", "RecQty", "IssQty", "StartDate", "VendorInvoiceNo", "VendorInvoiceDate", "VendorInvoiceDetail", "CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "CureQtrYear", "ExpQtrYear", "BatchNo", "TransTypeID", "Location", "TransType", "AlternateParts", "EffRate", "TransTypeName", "Applicability", "ATACode", "ATANomenclature", "ATAChapter", "PartStatus", "OrderText", "OrderNo", "Amend"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptConsiderAsAsset").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptConsiderAsAsset").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"CompanyName", "SupplierName", "BranchName", "Category", "Nomenclature", "Store", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "Search1", "Search2"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next



            If ds.Tables("ExcelrptConsiderAsAsset").Columns.Contains("ReceiptDate") Then
                ds.Tables("ExcelrptConsiderAsAsset").Columns("ReceiptDate").ColumnName = "Date"
            End If


            If ds.Tables("ExcelrptConsiderAsAsset").Columns.Contains("ReceiptNo") Then
                ds.Tables("ExcelrptConsiderAsAsset").Columns("ReceiptNo").ColumnName = "Receipt No."
            End If
            If ds.Tables("ExcelrptConsiderAsAsset").Columns.Contains("ReceiveFrom") Then
                ds.Tables("ExcelrptConsiderAsAsset").Columns("ReceiveFrom").ColumnName = "Received From"
            End If

            If ds.Tables("ExcelrptConsiderAsAsset").Columns.Contains("SerialNo") Then
                ds.Tables("ExcelrptConsiderAsAsset").Columns("SerialNo").ColumnName = "Serial No."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelrptConsiderAsAsset"))

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptConsiderAsAsset").TableName = "Asset  Items"
			Session("ExcelFileName") = "Asset  Items"
			Session("dsNew") = dsNew
			'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
			'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
			'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
			'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Shital on 18-Jan-2021
            EventLogDetails = EventLogDetails + ", Date Range : " + txtFromDate.Text + " " + txtToDate.Text
            MarkLog(Util.Action.Print, "AssetItems", "Export To excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            '--------
        End If
    End Sub
End Class