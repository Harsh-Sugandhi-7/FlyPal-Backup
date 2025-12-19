'Ajax Conversion By Vikrant On 31-Jan-2014
Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Public Class wfrptSearchPartList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCategoryList As CategoryList
    Public mNomenclatureList As NomenclatureList
    Public PartNo As String
    Public Description As String
    Public strNomenclature = "", strCategory As String = ""
    Dim EventLogDetail As String = String.Empty
    Public mATAList As ATAList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        mNomenclatureList = CType(Session("mNomenclatureList"), NomenclatureList)
        mATAList = CType(Session("mATAListwfrptSearchPartList_Ajax"), ATAList)
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
        Session("mNomenclatureList") = mNomenclatureList
        Session("mATAListwfrptSearchPartList_Ajax") = mATAList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mNomenclatureList")
        Session.Remove("mCategoryList")
        Session.Remove("mATAListwfrptSearchPartList_Ajax")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetValues()
        strCategory = IIf(hdnCategoryName.Value.Length > 0, hdnCategoryName.Value, "")
        strNomenclature = IIf(hdnNomenclatureName.Value.Length > 0, hdnNomenclatureName.Value, "")

        'Added By Vikrant On 28-Nov-2012 For ALL28112012
        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
        'End

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblCategoryName.Text = "Category : " & IIf(strCategory <> "", strCategory, "All")
        lblNomenclatureName.Text = "Nomenclature : " & IIf(strNomenclature <> "", strNomenclature, "All")
        lblATAChapter.Text = "ATA : " & IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "All")
        lblModelCurrentCriteria.Text = "Model : " + IIf(txtModelList.Text.Trim <> "", txtModelList.Text.Trim, "All") 'Added by Vikrant on 04-Oct-2018 For ALL04102018
        EventLogDetail = lblPartNo.Text + "," + lblDesc.Text + "," + lblCategoryName.Text + "," + lblNomenclatureName.Text + "," + "Format : " + cmbFormat.SelectedItem.ToString + "," + "Sort By : " + cmbSortBy.SelectedItem.ToString + ", " + lblATAChapter.Text + ", " + lblModelCurrentCriteria.Text
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptParts
        SetValues()

        If cmbFormat.SelectedValue = 0 Then                      'Format 1
            If cmbSortBy.SelectedValue = 1 Then
                'myReport = New crptItemListDescription
                myReport = New crptItemListDescriptionNew
            Else
                'myReport = New crptItemList
                myReport = New crptItemListNew
            End If
        ElseIf cmbFormat.SelectedValue = 1 Then                 'Format 2
            If cmbSortBy.SelectedValue = 1 Then
                'myReport = New crptItemListDescription
                myReport = New crptItemListDescriptionWithSerialisedStatus
            Else
                'myReport = New crptItemList
                myReport = New crptItemListWithSerialisedStatus
            End If
        Else                                                    'Format 3
            myReport = New crptAlternet
        End If

        rpt = rptParts.GetParts(PartNo, Description, strCategory, strNomenclature, cmbFormat.SelectedValue, _
                                ATAID:=cmbATAChapter.SelectedValue.ToString, ModelName:=txtModelList.Text.Trim, _
                                EssentialCatagoryID:=cmbEssentialCatagory.SelectedValue, _
                                IsOneTimePurchase:=IIf(chkIsOTP.Checked = True, 1, 0))
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, _
                                                              IIf(txtModelList.Text.Trim <> "", txtModelList.Text.Trim, ""), "", strCategory, _
                                                              strNomenclature, AppSettings("ClientCode"), "", "", Description, _
                                                              IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, ""), 0, "", "", _
                                                              WorkOrderText:=IIf(chkIsOTP.Checked = True, "One Time Purchase", ""), _
                                                              WorkOrderNo:=AppSettings("Logo"), Search1:="")
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf rpt.Count > 0 Then 'Added By Utkarsh On 7-Jun-2011 For All07062011
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 721)
        End If
        If IsExcel = False Then 'If PDF format  
            Dim ds As New dsParts
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 20-Feb-2012
            da.Fill(ds, mrptImage) 'Added by Shweta on 20-Feb-2012
            da.Fill(ds, rpt)
            da.Fill(ds, objsearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "PartList", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ElseIf IsExcel = True Then  'Excel format
            Dim ds As New dsExcelParts
            ds.Clear()
            da.Fill(ds, "rptSearchingCriteria", objsearch)
            da.Fill(ds, "rptParts", rpt)

            Dim columnToRemove2 As String() = {"CompanyName", "BranchName", "Store", "KitName", "CurrencySymbol", "currencyName", "ProductVersion", _
                                               "SINote", "TransTypeID", "FromStore", "WorkOrderText", "WorkOrderNo", "Search2", "Search3", _
                                               "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10", "FromDate", "ToDate", _
                                               "Aircraft", "WorkShop"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
                End If
            Next
            Dim columnToRemove As String()
            If AppSettings("ClientCode") = "BA" Then
                columnToRemove = {"PartID", "BecchMarkMonths", "Note", "Folio", "LinkID", "SerialisedStatus", "AlternetCountForItem", _
                                                             "ATANomenclature", "ATAChapter", "EssentialCatagoryID", "IsOneTimePurchase"}
            Else
                columnToRemove = {"PartID", "BecchMarkMonths", "Note", "Folio", "LinkID", "SerialisedStatus", "AlternetCountForItem", _
                                             "ATANomenclature", "ATAChapter", "EssentialCatagoryID", "EssentialCatagoryName", "IsOneTimePurchase"}
            End If


            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("rptParts").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("rptParts").Columns.Remove(columnToRemove(i))
                End If
            Next
            If AppSettings("ClientCode") = "BA" Then
                If ds.Tables("rptParts").Columns.Contains("EssentialCatagoryName") Then
                    ds.Tables("rptParts").Columns("EssentialCatagoryName").ColumnName = "Essential Catagory"
                End If
            End If
            If ds.Tables("rptParts").Columns.Contains("PartName") Then
                ds.Tables("rptParts").Columns("PartName").ColumnName = "Part Number"
            End If
            If ds.Tables("rptParts").Columns.Contains("NomenclatureName") Then
                ds.Tables("rptParts").Columns("NomenclatureName").ColumnName = "Nomenclature"
            End If
            If ds.Tables("rptParts").Columns.Contains("SerialisedStatusText") Then
                ds.Tables("rptParts").Columns("SerialisedStatusText").ColumnName = "Serialised Status"
            End If

            If ds.Tables("rptParts").Columns.Contains("ATACode") Then
                ds.Tables("rptParts").Columns("ATACode").ColumnName = "ATA"
            End If
            If ds.Tables("rptParts").Columns.Contains("MaxStockLevel") Then
                ds.Tables("rptParts").Columns("MaxStockLevel").ColumnName = "Max. Stock Level"
            End If
            If ds.Tables("rptParts").Columns.Contains("OneTimePurchase") Then
                ds.Tables("rptParts").Columns("OneTimePurchase").ColumnName = "One Time"
            End If

            If ds.Tables("rptSearchingCriteria").Columns.Contains("PartNo") Then
                ds.Tables("rptSearchingCriteria").Columns("PartNo").ColumnName = "Part No."
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Category") Then
                ds.Tables("rptSearchingCriteria").Columns("Category").ColumnName = "Category"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Nomenclature") Then
                ds.Tables("rptSearchingCriteria").Columns("Nomenclature").ColumnName = "Nomenclature"
            End If
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Description") Then
                ds.Tables("rptSearchingCriteria").Columns("Description").ColumnName = "Description"
            End If

            If ds.Tables("rptSearchingCriteria").Columns.Contains("RelNoteNo") Then
                ds.Tables("rptSearchingCriteria").Columns("RelNoteNo").ColumnName = "ATA"
            End If
            'Added by Vikrant on 04-Oct-2018 For ALL04102018
            If ds.Tables("rptSearchingCriteria").Columns.Contains("SupplierName") Then
                ds.Tables("rptSearchingCriteria").Columns("SupplierName").ColumnName = "Applicability"
            End If
            'End
            If ds.Tables("rptSearchingCriteria").Columns.Contains("Search1") Then
                ds.Tables("rptSearchingCriteria").Columns("Search1").ColumnName = "One Time Purchase"
            End If

            Dim dtCloned As DataSet = ds.Clone()
            dtCloned.Tables("rptParts").Columns("Max. Stock Level").DataType = GetType(Integer)

            For Each row As DataRow In ds.Tables("rptParts").Rows
                dtCloned.Tables("rptParts").ImportRow(row)
            Next

            Dim dsNew As New DataSet
            dsNew.Clear()
            'ds.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            'ds.Tables("rptParts").TableName = "Part List Report"
            'dsNew = ds
            dsNew.Merge(ds.Tables("rptSearchingCriteria"))
            dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
            dsNew.Merge(dtCloned.Tables("rptParts"))
			dsNew.Tables("rptParts").TableName = "Part List Report"
			Session("ExcelFileName") = "Part List Report"
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "PartList", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub Display()
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblCategoryName.Visible = True
        lblNomenclatureName.Visible = True
        lblATAChapter.Visible = True
        lblModelCurrentCriteria.Visible = True 'Added by Vikrant on 04-Oct-2018 For ALL04102018
        upnlCurrentCriteria.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList
        'mNomenclatureList = NomenclatureList.GetNomenclatureList("(All)")
        'cmbNomenclature.DataSource = mNomenclatureList
        'Session("mNomenclatureList") = mNomenclatureList

        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAListwfrptSearchPartList_Ajax") = mATAList
        cmbATAChapter.DataSource = mATAList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbCategory.Enabled = True Then
                setFocus(cmbCategory)
            End If
            DataFieldBind()
        End If
        cmbSortBy.Enabled = (cmbFormat.SelectedValue = 0 Or cmbFormat.SelectedValue = 1)
        Label1.Visible = IIf(AppSettings("ClientCode") = "BA", True, False)
        cmbEssentialCatagory.Visible = IIf(AppSettings("ClientCode") = "BA", True, False)
        Label2.Visible = IIf(AppSettings("ClientCode") = "BA", True, False)
        If AppSettings("ClientCode") = "BA" Then
            lblIsOneTimePurchase.Text = "Step VIII. Selection For Is One Time Purchase"
            lblStep7.Text = "Step IX. Display Report"
        Else
            lblIsOneTimePurchase.Text = "Step VII. Selection For Is One Time Purchase"
            lblStep7.Text = "Step VIII. Display Report"
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        SetReport(True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedIndex = 2 Then
            lblAlternetPartListOnly.Visible = True
            lblSortBy1.Visible = False
            lblStep6.Visible = False
            cmbSortBy.Visible = False
            If AppSettings("ClientCode") = "BA" Then
                Label2.Text = "Step VI. Essential Category"
                lblIsOneTimePurchase.Text = "Step VII. Selection For Is One Time Purchase"
                lblStep7.Text = "Step VIII. Display Report"
            Else
                lblIsOneTimePurchase.Text = "Step VI. Selection For Is One Time Purchase"
                lblStep7.Text = "Step VII. Display Report"
            End If
        Else
            lblAlternetPartListOnly.Visible = False
            lblSortBy1.Visible = True
            lblStep6.Visible = True
            cmbSortBy.Visible = True
            If AppSettings("ClientCode") = "BA" Then
                Label2.Text = "Step VII. Essential Category"
                lblIsOneTimePurchase.Text = "Step VIII. Selection For Is One Time Purchase"
                lblStep7.Text = "Step IX. Display Report"
            Else
                lblIsOneTimePurchase.Text = "Step VII. Selection For Is One Time Purchase"
                lblStep7.Text = "Step VIII. Display Report"
            End If
        End If
        upnlSortBySpan.Update()
        upnlSortBy.Update()
        upnllblStep6.Update()
        upnllblStep5.Update()
        UpdatePanel1.Update()
        upnllblStep6.Update()
        UpdatePanel2.Update()
    End Sub
#End Region

#Region " Helper Methods "
    'Added by Vikrant on 04-Oct-2018 For ALL04102018
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
               Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                   Select c.Name).Take(count).ToList
        End If
    End Function
    'End
#End Region

End Class