Public Class wfrptSearchMinLevelItem_Ajax
	Inherits System.Web.UI.Page

	Dim EventLogDetail As String

#Region " Variable Declaration "
	Public mCategoryList As CategoryList
	Public mNomenclatureList As NomenclatureList
	Public PartNo As String
	Public Description = ""
	Public strNomenclature As String = ""
	Public strCategory As String = ""
	Public strStore As String = ""
	Public strCustomer As String
#End Region

#Region " Helper Methods "
	Private Sub GetSession()
		mCategoryList = CType(Session("mCategoryList"), CategoryList)
		PartNo = IIf(IsNothing(PartNo), "", PartNo)
		Description = IIf(IsNothing(Description), "", Description)
		mNomenclatureList = CType(Session("mNomenclatureList"), NomenclatureList)
	End Sub
	Private Sub SetSession()
		Session("mCategoryList") = mCategoryList
		Session("PartNo") = PartNo
		Session("Description") = Description
		Session("mNomenclatureList") = mNomenclatureList
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mCategoryList")
		Session.Remove("PartNo")
		Session.Remove("Description")
		Session.Remove("mNomenclatureList")
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
	Private Sub Display()
		lblCategoryName.Visible = True
		lblNomenclatureName.Visible = True
		lblPartNo.Visible = True
		lblDesc.Visible = True
		'lblStoreName.Visible = True
		upnlCriteria.Update()
	End Sub
	Private Sub SetValues()
		strCategory = ""
		strNomenclature = ""
		'Added By Vikrant On 28-Nov-2012 For ALL28112012
		If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
			PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
			Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
		Else
			PartNo = Trim(txtPartDescription.Text)
			Description = Trim(txtPartDescription.Text)
		End If
		'End
		strCategory = IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "")
		'strNomenclature = IIf(cmbNomenclature.SelectedIndex > 0, cmbNomenclature.SelectedItem.Text, "")
		Dim NomenID As Guid = New Guid(Request.Form("cmbNomenclature").ToString)
		strNomenclature = IIf(NomenID.Equals(Guid.Empty), "", mNomenclatureList(NomenID).Name)
		strStore = ""
		lblCategoryName.Text = "Category : " & IIf(strCategory <> "", strCategory, "All")
		lblNomenclatureName.Text = "Nomenclature : " & IIf(strNomenclature <> "", strNomenclature, "All")
		'lblStoreName.Text = "Store : " & IIf(strStore <> "", strStore, "All")
		lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
		lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
		strCustomer = ""
		'If cmbCustomer.SelectedIndex = 0 Then
		'    lblCustomerName.Text = "Customer : All"
		'    strCustomer = ""
		'Else
		'    strCustomer = Vendor.GetVendor(New Guid(cmbCustomer.SelectedValue)).Name
		'    lblCustomerName.Text = "Customer :" & strCustomer
		'End If
		EventLogDetail = lblCategoryName.Text + ", " + lblNomenclatureName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
	End Sub
	Private Sub ResetValues()
		strCategory = ""
		strNomenclature = ""
		PartNo = ""
		Description = ""
		strStore = ""
	End Sub
	Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
		Dim da As New CSLA.Data.ObjectAdapter
		Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		Dim objsearch As rptSearchingCriteria
		Dim MinMax As Integer = 0 'Added By Prashant 25-Feb-2013 All25022013
		Dim rpt As rptMinLevelItem
		SetValues()
		Dim ds As New dsMinStockLevel
		Dim ExcelFileName As String

		If AppSettings("ClientCode") = "APFT" Or
		   AppSettings("ClientCode") = "AAP" Then 'ClientCode Added by Vikrant On 09-May-2019 For APFT09052019
			myReport = New crptMinMaxLevelItemAPFT
			MinMax = CInt(IIf(rbMinimum.Checked, 0, 1))
		Else
			If rbMinimum.Checked = True Then 'Added By Prashant 25-Feb-2013 All25022013
				myReport = New crptMinLevelItem
				MinMax = 0
			ElseIf rbMaximum.Checked = True Then
				myReport = New crptMaxLevelItem
				MinMax = 1
			End If '-----------------------------------------
		End If

		rpt = rptMinLevelItem.GetMinLevelItem(PartNo, Description, strCategory, strNomenclature, strStore, Guid.Empty, , MinMax, WithAlternatePatrs:=chkCheckForAlternatePart.Checked)
		objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", PartNo, strCustomer, "", strCategory, strNomenclature, strStore, "", "", Description, "", 0, "", IIf(chkCheckForAlternatePart.Checked = True, "Considered Alternate Patrs Stock", ""), "", AppSettings("Logo"), Search8:=IIf(rbMinimum.Checked, "Minimum Level", "Maximum Level"), Search9:=Today.Date.ToString(AppSettings("DateFormat")), Search10:=IIf(rbMinimum.Checked, 0, 1).ToString, Search1:=IIf(rbMinimum.Checked, "Parts on Minimum Level", "Parts on Maximum Level"))

		If rpt.Count <= 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		ElseIf (rpt.Count > 0 And IsExcel = False) Then
			RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 512)
		End If

		If IsExcel = False Then
			ds.Clear()
			Dim mrptImage As rptImage = rptImage.GetImage(ds)
			da.Fill(ds, rpt)
			da.Fill(ds, mrptImage)
			da.Fill(ds, objsearch)
			myReport.SetDataSource(ds)
			Session("CrystalReport") = myReport
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
			MarkLog(Util.Action.Print, "MinLevelItems", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
		Else
			rpt.Sort("LinkID", ComponentModel.ListSortDirection.Ascending) 'Added by Vikrant On 09-May-2019 For APFT09052019

			ds.Clear()
			da.Fill(ds, "ExcelrptMinLevelItem", rpt)
			da.Fill(ds, "rptSearchingCriteria", objsearch)

			Dim columnToRemove As String() = {"CompanyName", "FromDate", "ToDate", "SupplierName", "BranchName", "Store", "KitName", "Aircraft", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search10"}

			For i As Integer = 0 To columnToRemove.Length - 1
				If ds.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove(i)) Then
					ds.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove(i))
				End If
			Next

			If Not AppSettings("ClientCode") = "APFT" Or
			   AppSettings("ClientCode") = "AAP" Then
				If rbMinimum.Checked Then
					ds.Tables("ExcelrptMinLevelItem").Columns.Remove("MaxStockLevel")
				ElseIf rbMaximum.Checked Then
					ds.Tables("ExcelrptMinLevelItem").Columns.Remove("MinStockLevel")
				End If
			End If


			'Added by Vikrant On 09-May-2019 For APFT09052019
			ds.Tables("ExcelrptMinLevelItem").Columns.Remove("LinkID")
			If Not AppSettings("ClientCode") = "APFT" Or
			   AppSettings("ClientCode") = "AAP" Then
				ds.Tables("ExcelrptMinLevelItem").Columns.Remove("ItemMasterLocation")
			Else
				ds.Tables("ExcelrptMinLevelItem").Columns.Remove("NomenclatureName")
			End If
			'End

			Dim dsNew As New DataSet
			dsNew.Clear()

			dsNew.Merge(ds.Tables("rptSearchingCriteria"))
			dsNew.Merge(ds.Tables("ExcelrptMinLevelItem"))

			dsNew.Tables("rptSearchingCriteria").Columns("Search9").ColumnName = "Report Date"
			dsNew.Tables("rptSearchingCriteria").Columns("Search8").ColumnName = "Minimum/Maximum Stock Level"

			If AppSettings("ClientCode") = "APFT" Or
			   AppSettings("ClientCode") = "AAP" Then
				dsNew.Tables("ExcelrptMinLevelItem").Columns("MinStockLevel").ColumnName = "Min. Level"
				dsNew.Tables("ExcelrptMinLevelItem").Columns("MaxStockLevel").ColumnName = "Max. Level"
			Else
				If rbMinimum.Checked Then
					dsNew.Tables("ExcelrptMinLevelItem").Columns("MinStockLevel").ColumnName = "Min. Level"
				ElseIf rbMaximum.Checked Then
					dsNew.Tables("ExcelrptMinLevelItem").Columns("MaxStockLevel").ColumnName = "Max. Level"
				End If
			End If


			'Added by Vikrant On 09-May-2019 For APFT09052019
			If AppSettings("ClientCode") = "APFT" Or
			   AppSettings("ClientCode") = "AAP" Then
				ds.Tables("ExcelrptMinLevelItem").Columns.Remove("ItemMasterLocation")
				dsNew.Tables("ExcelrptMinLevelItem").Columns("ItemMasterLocation").ColumnName = "Location"
			Else
				dsNew.Tables("ExcelrptMinLevelItem").Columns("NomenclatureName").ColumnName = "Nomenclature"
			End If
			'End

			dsNew.Tables("ExcelrptMinLevelItem").Columns("QtyOrder").ColumnName = "Order Qty."
			dsNew.Tables("ExcelrptMinLevelItem").Columns("QtyReturnable").ColumnName = "Return Qty."
			dsNew.Tables("ExcelrptMinLevelItem").Columns("QtyStock").ColumnName = "Stock Qty."
			dsNew.Tables("ExcelrptMinLevelItem").Columns("PartName").ColumnName = "PartNo"
			dsNew.Tables("ExcelrptMinLevelItem").Columns("CategoryName").ColumnName = "Category"


			dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
			If rbMinimum.Checked Then
				dsNew.Tables("ExcelrptMinLevelItem").TableName = "Min. Level Items"
				ExcelFileName = "Min. Level Items"
			ElseIf rbMaximum.Checked Then
				dsNew.Tables("ExcelrptMinLevelItem").TableName = "Max. Level Items"
				ExcelFileName = "Max. Level Items"
			End If
			Session("ExcelFileName") = ExcelFileName
			Session("dsNew") = dsNew
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
			'Added by Prashant on 19-Jan-2021
			MarkLog(Util.Action.Print, "MinLevelItems", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
		End If
	End Sub
	Private Sub Invisiable()
		lblCategoryName.Visible = False
		lblNomenclatureName.Visible = False
		lblPartNo.Visible = False
		lblDesc.Visible = False
		'lblStoreName.Visible = False
		lblCustomerName.Visible = False
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mCategoryList = CategoryList.GetCategoryList("(All)")
		cmbCategory.DataSource = mCategoryList
		Session("mCategoryList") = mCategoryList

		mNomenclatureList = NomenclatureList.GetNomenclatureList("(All)")
		cmbNomenclature.DataSource = mNomenclatureList
		Session("mNomenclatureList") = mNomenclatureList


		DataBind()
	End Sub
#End Region

	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack Then
			RemoveSession()
			If cmbCategory.Enabled = True Then
				SetFocus(cmbCategory)
			End If
			DataFieldBind()
		End If
	End Sub
	Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
		Display()
		SetValues()
	End Sub
	Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
		SetReport()
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
		SetReport(True)
	End Sub
End Class