Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfrptTransitDamageItemList_Ajax
    Inherits System.Web.UI.Page

#Region " Variables "
    Public mItem As Item
    Public PartNo As String = ""
    Public Description As String = ""
    Public SerialNo As String = ""
    Public EventLogDetails As String = String.Empty
#End Region

#Region " Helper Methods "
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
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblSerialNo1.Text = "Serial No. : " + IIf(SerialNo <> "", SerialNo, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "")
        EventLogDetails = lblPartNo.Text + ", " + lblDesc.Text + ", " + lblSerialNo1.Text
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As TransitDamageItemList
        If txtSerialNo.Text.Trim <> "" Then
            SerialNo = txtSerialNo.Text.Trim
        Else
            SerialNo = ""
        End If
        myReport = New crptTransitDamageItemList
        rpt = TransitDamageItemList.GetTransitDamageItemList(ItemName:=PartNo, SerialNo:=SerialNo, Description:=Description)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1303)

        End If

        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, "", AppSettings("Logo"), "", "", "", "", "", Description, "", 0, "", "", "", "", "", "", SerialNo)
        If IsExcel = False Then     'PDF format
            Dim ds As New dsTransitDamageItemList
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
            MarkLog(Util.Action.Print, "TransitDamageItemList", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcelTransitDamageItemList
            ds.Clear()
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            da.Fill(ds, "TransitDamageItemList", rpt)

            Dim columnToRemove2 As String() = {"WorkOrderText", "FromStore", "WorkShop", "FromDate", "ToDate", "Category", "Status", "CompanyName", "SupplierName", "BranchName", "Aircraft", "Nomenclature", "Store", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkOrderNo", "Search1", "Search2", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next

            Dim columnToRemove As String() = {"ReceiptDate", "OrderDate"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("TransitDamageItemList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("TransitDamageItemList").Columns.Remove(columnToRemove(i))
                End If
            Next

            If ds.Tables("TransitDamageItemList").Columns.Contains("ItemName") Then
                ds.Tables("TransitDamageItemList").Columns("ItemName").ColumnName = "Part No."
            End If
            If ds.Tables("TransitDamageItemList").Columns.Contains("ItemDescription") Then
                ds.Tables("TransitDamageItemList").Columns("ItemDescription").ColumnName = "Description"
            End If
            If ds.Tables("TransitDamageItemList").Columns.Contains("OrderDateFormatted") Then
                ds.Tables("TransitDamageItemList").Columns("OrderDateFormatted").ColumnName = "Order Date"
            End If
            If ds.Tables("TransitDamageItemList").Columns.Contains("ReceiptDateFormatted") Then
                ds.Tables("TransitDamageItemList").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
            End If
            
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Search3") Then
                ds.Tables("rptSearchingCriteria").Columns("Search3").ColumnName = "SerialNo"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()
            ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
			ds.Tables("TransitDamageItemList").TableName = "Transit Damage Item List"
			Session("ExcelFileName") = "Transit Damage Item List"
			dsNew = ds
            Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "TransitDamageItemList", "Export To Excel " + EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
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