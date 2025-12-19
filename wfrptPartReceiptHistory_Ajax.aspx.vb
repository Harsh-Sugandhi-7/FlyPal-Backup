Public Class wfrptPartReceiptHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public PartNo As String = ""
    Public Description As String = ""
    Dim EventLogID As Guid 'Added by Prashant
    Dim mPartReceiptHistorySearchingCriteria As String = String.Empty
    'Added by Abhishek on 28-SEP-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim objsearch As rptSearchingCriteria
    Dim rpt As rptPartHistory
    Dim ds As New dsPartHistory
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility()
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
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
        mPartReceiptHistorySearchingCriteria = lblPartNo.Text + ", " + lblDesc.Text.Trim
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptPartHistory
        GetSession()
        Dim ds As New dsPartHistory
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, "", AppSettings("Logo"), "", "", "", "", "", Description, "")
        myReport = New crptRecItemHistoryNew
        rpt = rptPartHistory.GetPartHistory(PartNo, Description, 2, "", chkIsValued.Checked)
        If PartNo = "" And Description = "" Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please Select the item from the List", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 704)
        End If
        ds.Clear()
        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
        End If
        da.Fill(ds, rpt)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "PartReceiptHistory", mPartReceiptHistorySearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        custValidator.ControlToValidate = "txtsearch"
        If txtSearch.Text = "" Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
            If PartNo = "" Or Description = "" Then
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            RemoveSession()
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetValues()
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    'Added by Abhishek on 28-SEP-2017
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            GetSession()
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, "", AppSettings("Logo"), "", "", "", "", "", Description, "")
            ' MyReport = New crptRecItemHistoryNew
            rpt = rptPartHistory.GetPartHistory(PartNo, Description, 2, "", chkIsValued.Checked)
            If PartNo = "" And Description = "" Then
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please Select the item from the List", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 704)
            End If
            ds.Clear()
            da.Fill(ds, "ExcelReceiptrptPartHistory", rpt)
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            Dim columnToRemove1 As String() = {"PartID", "PartName", "PartDescription", "OrderFor", "Type", "ReleaseNoteDate", "CureDate", "ExpiryDate", "IssQty", "IssueTo", "StartDate", "VendorInvoiceNo", "VendorInvoiceDate", "VendorInvoiceDetail", "CureQtrs", "CureYear", "ExpQtrs", "ExpYear", "CureQtrYear", "ExpQtrYear", "BatchNo", "Location", "TransType", "AlternateParts", "TransTypeName", "EffRate", "ATAChapter", "PartStatus", "OrderNumber", "OrderDateFormatted", "OrderType", "TransTypeID", "Applicability", "ATACode", "ATANomenclature", "OrderText", "OrderNo", "OrderAmend", "IsOverhaul", "OrderTransTypeID", "OrderDate", "Remark", "OrdQty"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelReceiptrptPartHistory").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelReceiptrptPartHistory").Columns.Remove(columnToRemove1(i))
                End If
            Next
            Dim columnToRemove2 As String() = {"CompanyName", "FromDate", "ToDate", "SupplierName", "BranchName", "Category", "Nomenclature", "Store", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            If ds.Tables("ExcelReceiptrptPartHistory").Columns.Contains("PHDate") Then
                ds.Tables("ExcelReceiptrptPartHistory").Columns("PHDate").ColumnName = "Date "
            End If

            If ds.Tables("ExcelReceiptrptPartHistory").Columns.Contains("IdentityNo") Then
                ds.Tables("ExcelReceiptrptPartHistory").Columns("IdentityNo").ColumnName = "Receipt No."
            End If
            If ds.Tables("ExcelReceiptrptPartHistory").Columns.Contains("ToFrom") Then
                ds.Tables("ExcelReceiptrptPartHistory").Columns("ToFrom").ColumnName = "Received From"
            End If
            If ds.Tables("ExcelReceiptrptPartHistory").Columns.Contains("ReleaseNoteNo") Then
                ds.Tables("ExcelReceiptrptPartHistory").Columns("ReleaseNoteNo").ColumnName = "Rel. Note No."
            End If

            If ds.Tables("ExcelReceiptrptPartHistory").Columns.Contains("SerialNo") Then
                ds.Tables("ExcelReceiptrptPartHistory").Columns("SerialNo").ColumnName = "Serial No."
            End If
            If ds.Tables("ExcelReceiptrptPartHistory").Columns.Contains("RecQty") Then
                ds.Tables("ExcelReceiptrptPartHistory").Columns("RecQty").ColumnName = "Rec Qty."
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Merge(ds.Tables("ExcelReceiptrptPartHistory"))

            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Tables("ExcelReceiptrptPartHistory").TableName = "Part Receipt History"
			Session("ExcelFileName") = "Part Receipt History"

			Session("dsNew") = dsNew
            'Session("DataTableToBeFormattedForExportToExcel") = "Pending Requisition"
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "PartReceiptHistory", "Export To excel " + mPartReceiptHistorySearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
            'If Page.IsValid Then
            '    SetValues()
            '    SetReport(True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class