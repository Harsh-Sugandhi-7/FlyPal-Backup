'Added by Utkarsh on 03-Feb-2014
Imports System.Collections.Generic
Public Class wfrptSearchPendingOrder_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mVendorList As VendorList
	Public mVendor As Vendor
	Dim FromDate As String
	Dim ToDate As String
	Dim PartNo As String
	Dim Description As String
	Dim Supplier As String
	Dim EventLogDetail As String
	Dim rpt As rptPendingOrder
	Dim objsearch As rptSearchingCriteria
	Dim dsPenOrd As New dsOrder
	Dim da As New CSLA.Data.ObjectAdapter
	Public Aircraft As String = String.Empty 'Added By Vikrant On 16-Nov-2015 For All16112015
#End Region

#Region " Helper Methods "
	Private Sub GetSession()
		mVendorList = CType(Session("mVendorlist"), VendorList)
		PartNo = Session("PartNo")
		Description = Session("Description")
		PartNo = IIf(IsNothing(PartNo), "", PartNo)
		Description = IIf(IsNothing(Description), "", Description)
	End Sub
	Private Sub SetSession()
		Session("mVendorlist") = mVendorList
		Session("PartNo") = PartNo
		Session("Description") = Description
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mVendorlist")
		Session.Remove("PartNo")
		Session.Remove("Description")
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
	Private Sub ControlVisibility(ByVal Index As Int16)
		If Index = 6 Then
			lblFromDate.Visible = True
			lblToDate.Visible = True
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
		ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
			lblFromDate.Visible = True
			lblToDate.Visible = True
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = False
			txtToDate.Enabled = False
		End If
		upnlDateRange.Update()
	End Sub
	Private Sub ControlVisibilityForCriteria()
		''txtFromDate.Visible = IIf(Index <> 0, True, False)
		''txtToDate.Visible = IIf(Index <> 0, True, False)
		''calFromDate.Visible = IIf(Index = 6, True, False)
		''calToDate.Visible = IIf(Index = 6, True, False)
		lblTransType.Visible = True
		lblDateRangeFrom.Visible = True
		lblToDate1.Visible = True
		lblPartNo.Visible = True
		lblDesc.Visible = True
		lblVendorName.Visible = True
		lblAircraft1.Visible = True 'Added By Vikrant On 16-Nov-2015 For All16112015
		upnlCriteria.Update()
	End Sub
	Private Sub setDatePeroid(ByVal Index As Int32)
		Select Case Index
			Case 0 ' All   
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
				txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
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
			FromDate = "1-1-1900"
			ToDate = "1-1-2200"
			lblDateRangeFrom.Text = "Date Range : All"
		Else
			FromDate = txtFromDate.Text.ToString
			ToDate = txtToDate.Text.ToString
			lblDateRangeFrom.Text = "Date Range : " & FromDate & " To " & ToDate & " ( " & cmbDateRange.SelectedItem.Text & " ) "
		End If
		If cmbSupplier.SelectedIndex = 0 Then
			Supplier = ""
			lblVendorName.Text = "Supplier : All"
		Else
			'    mVendor = Vendor.GetVendor(New Guid(cmbSupplier.SelectedValue))
			Supplier = cmbSupplier.SelectedItem.Text
			lblVendorName.Text = "Supplier :  " & Supplier
		End If
		'Added By Utkarsh ON 28-Nov-2012 FOR ALL28112012
		If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
			PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
			Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
		Else
			PartNo = Trim(txtSearch.Text)
			Description = Trim(txtSearch.Text)
		End If
		'End

		'Added By Utkarsh On 05-Feb-2013 FOR Heligo054022013 
		Aircraft = txtAircraft.Text.Trim
		lblAircraft1.Text = "Aircraft :  " & IIf(Aircraft <> "", Aircraft, "All")
		'End

		lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
		lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
		lblTransType.Text = "Order Type : " & IIf(cmbOrderType.SelectedIndex > 0, cmbOrderType.SelectedItem.Text, "All")

		EventLogDetail = lblTransType.Text + ", " + lblDateRangeFrom.Text + ", " + lblVendorName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", " + lblAircraft1.text
	End Sub
	Private Sub ResetValues()
		FromDate = "1-1-1900"
		ToDate = "1-1-2200"
		Supplier = ""
		PartNo = ""
		Description = ""
	End Sub
	Private Sub SetReport()
		Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
		myReport = New crptPendingOrder
		GetSession()
		SetValues()

		rpt = rptPendingOrder.GetPendingOrder(FromDate, ToDate, PartNo, Supplier, Description, cmbOrderType.SelectedValue, Aircraft, _
											  IsPartialOrdersExcluded:=chkExcludePartialOrder.Checked, IntOrderNo:=txtIntOrderNo.Text.ToString, _
											  IsPBHPurchase:=chkIsPBHPurchase.Checked)
		objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, Supplier, "", "", "", "", Aircraft, "", Description, "", cmbOrderType.SelectedValue, "", "", txtIntOrderNo.Text.ToString, AppSettings("Logo"))

		If rpt.Count <= 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
			'Added By Utkarsh On 7-Jun-2011 For All07062011
		ElseIf rpt.Count > 0 Then
			RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 501)
			'*******************************
		End If
		dsPenOrd.Clear()
		Dim mrptImage As rptImage = rptImage.GetImage(dsPenOrd) 'Added by Shweta on 20-Feb-2012
		da.Fill(dsPenOrd, rpt)
		da.Fill(dsPenOrd, objsearch)
		da.Fill(dsPenOrd, mrptImage) 'Added by Shweta on 20-Feb-2012
		myReport.SetDataSource(dsPenOrd)
		Session("CrystalReport") = myReport
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
		MarkLog(Util.Action.Print, "PendingOrder", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
		'ResetValues()
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
				Case MsgBoxResult.OK
					Session("Sender") = ""
					'Response.Redirect("wfrptSearchPendingOrder.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
				Case Else
					'
			End Select
		ElseIf Result1 = -1 Then
			Session("Sender") = ""
			' Response.Redirect("wfrptSearchPendingOrder.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
		cmbSupplier.DataSource = mVendorList
		Session("mVendorList") = mVendorList
		DataBind()
	End Sub
#End Region

#Region "Events"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		EventLogID = CType(Session("EventLogID"), Guid)
		GetSession()
		If Not IsPostBack Then
			RemoveSession()
			If cmbDateRange.Enabled = True Then
				setFocus(cmbOrderType)
			End If
			DataFieldBind()
			ControlVisibility(6)
			setDatePeroid(6)
			cmbDateRange.SelectedIndex = 6
		End If
	End Sub
	Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
		Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
		ControlVisibility(Index)
		setDatePeroid(Index)

		If cmbDateRange.Enabled = True Then
			SetFocus(cmbDateRange)
		End If
	End Sub
	Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
		ControlVisibilityForCriteria()
		SetValues()
	End Sub
	Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
		If IsValid Then
			SetReport()
		End If
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		mVendorList = Nothing
		Session("MiddleFrame") = ""
		RemoveSession()
		Response.Redirect("Dashboard.aspx")
	End Sub
	'Added By Vikrant On 16-Nov-2015 For All16112015
	Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
		If IsValid Then
			Dim PeriodColumnsForExportToExcel As New List(Of String)
			SetValues()
			rpt = rptPendingOrder.GetPendingOrder(FromDate, ToDate, PartNo, Supplier, Description, cmbOrderType.SelectedValue, Aircraft, , IntOrderNo:=txtIntOrderNo.Text.ToString, IsPBHPurchase:=chkIsPBHPurchase.Checked)
			objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, Supplier, "", "", "", "", Aircraft, "", Description, "", cmbOrderType.SelectedValue, "", "", "", AppSettings("Logo"))

			If rpt.Count <= 0 Then
				MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
			dsPenOrd.Clear()
			da.Fill(dsPenOrd, objsearch)
			da.Fill(dsPenOrd, "ExcelrptPendingOrder", rpt)

			Dim columnToRemove1 As String() = {"CAmount", "IntOrderNo", "Amend", "CurrencySymbol", "TransTypeName", "TransTypeID", "Heading", "GroupBy", "Remark", "OrderNo", "OrderText", "OrderID"}
			For i As Integer = 0 To columnToRemove1.Length - 1
				If dsPenOrd.Tables("ExcelrptPendingOrder").Columns.Contains(columnToRemove1(i)) Then
					dsPenOrd.Tables("ExcelrptPendingOrder").Columns.Remove(columnToRemove1(i))
				End If
			Next

			Dim columnToRemove2 As String() = {"CompanyID", "Category", "WorkShop", "BranchName", "Nomenclature", "Store", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "FromStore", "WorkOrderText", "WorkOrderNo", "Search1", "Search2", "Search3", "Search4", "Search5", "Search6", "Search7", "Search8", "Search9", "Search10"}
			For i As Integer = 0 To columnToRemove2.Length - 1
				If dsPenOrd.Tables("rptSearchingCriteria").Columns.Contains(columnToRemove2(i)) Then
					dsPenOrd.Tables("rptSearchingCriteria").Columns.Remove(columnToRemove2(i))
				End If
			Next

			If dsPenOrd.Tables("ExcelrptPendingOrder").Columns.Contains("OrderTextNo") Then
				dsPenOrd.Tables("ExcelrptPendingOrder").Columns("OrderTextNo").ColumnName = "OrderNo"
			End If
			If dsPenOrd.Tables("ExcelrptPendingOrder").Columns.Contains("CAmountWithCurrencySymbol") Then
				dsPenOrd.Tables("ExcelrptPendingOrder").Columns("CAmountWithCurrencySymbol").ColumnName = "Bal. Amount"
			End If
			If dsPenOrd.Tables("ExcelrptPendingOrder").Columns.Contains("Amount") Then
				dsPenOrd.Tables("ExcelrptPendingOrder").Columns("Amount").ColumnName = "Bal. Amount(" + CType(objsearch.CurrentItem, Flypal.rptSearchingCriteria.Search).CurrencySymbol + ")"
			End If

			Dim dsNew As New DataSet
			dsNew.Clear()

			dsNew.Merge(dsPenOrd.Tables("rptSearchingCriteria"))
			dsNew.Merge(dsPenOrd.Tables("ExcelrptPendingOrder"))

			dsNew.Tables("rptSearchingCriteria").TableName = "Searching Criteria"
			dsNew.Tables("ExcelrptPendingOrder").TableName = "Pending Order" + IIf(cmbOrderType.SelectedIndex > 0, "(" + cmbOrderType.SelectedItem.Text + ")", "")
			Session("ExcelFileName") = "Pending Order" + IIf(cmbOrderType.SelectedIndex > 0, "-" + cmbOrderType.SelectedItem.Text, "")
			Session("dsNew") = dsNew
			Session("DataTableToBeFormattedForExportToExcel") = "Pending Order" + IIf(cmbOrderType.SelectedIndex > 0, "(" + cmbOrderType.SelectedItem.Text + ")", "")
			PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
			Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
			'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
			'Added by Prashant on 19-Jan-2021
			MarkLog(Util.Action.Print, "PendingOrder", "Export To Excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
		End If
	End Sub
	'End
#End Region
End Class