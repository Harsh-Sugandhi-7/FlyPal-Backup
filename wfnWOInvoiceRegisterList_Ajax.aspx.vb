'Created By: Ajay
'Dated     : 02-Aug-2022

Imports System.Text
Public Class wfnWOInvoiceRegisterList_Ajax
	Inherits Page

#Region " Variable Declaration "

	Protected mnWO As nWO
	Protected mnWOList As nWOList
	Dim EventLogDetail As String
	Dim StartDate As String
	Dim EndDate As String
	Dim WOText, WONo As String
	Dim Name, Id As String
	Dim EventLogID As Guid
	Public mInvoiceRegList As WOInvoiceRegisterList
	Dim mDistinctWOText As nDistinctWOText
	Dim mCustomerList As VendorList
	Private mCTotalJobCharges As Decimal
	Private mCTotalSpareAmount As Decimal
	Private mCTotalSpareCharges As Decimal
	Public mEventLog As EventLog
	Public mUser As User

	Dim DateIndex, FromDate, ToDate As String

#End Region

#Region " Methods "

	Private Sub GetSession()
		mUser = Session("mUser")
		mDistinctWOText = Session("mDistinctWOText")
		WOText = Session("WOText")
		WONo = IIf(IsNothing(Session("WONo")), 0, Session("WONo"))
	End Sub
	Private Sub SetSession()
		Session("mDistinctWOText") = mDistinctWOText
		Session("WOText") = WOText
		Session("WONo") = WONo
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mUser")
		Session.Remove("mDistinctWOText")
		Session.Remove("WOText")
		Session.Remove("WONo")
	End Sub
	Private Sub ControlVisibility2()
		lblDateRangeFrom.Visible = True
		lblWONo.Visible = True
		lblCust.Visible = True
		lblStatus.Visible = True
	End Sub
	Private Sub ClearAll()
		If Session("MiddleFrame") <> "wfnWOInvoiceRegisterList_Ajax.aspx?" Then
			Session.Remove("mUser")
		End If
	End Sub
	Private Sub SetValues()
		If Not IsDate(txtFromDate.Text) Then
			StartDate = ""
		Else
			StartDate = txtFromDate.Text
		End If
		If Not IsDate(txtToDate.Text) Then
			EndDate = ""
		Else
			EndDate = txtToDate.Text
		End If

		WOText = IIf(cmbCWP.SelectedItem.Text = "" Or cmbCWP.SelectedItem.Text = "(ALL)", "", cmbCWP.SelectedItem.Text)
		WONo = txtCWPNo.Text.Trim
		Name = IIf(cmbCustomerList.SelectedItem.Text = "" Or cmbCustomerList.SelectedItem.Text = "(ALL)", "", cmbCustomerList.SelectedItem.Text)

		Dim RegIDForLogo As Guid
		Dim tmpRegIDs As New StringBuilder
		Session("tmpRegIDs") = tmpRegIDs.ToString.TrimEnd(",")
		Session("RegIDForLogo") = RegIDForLogo
		EventLogDetail = StartDate + ", " + EndDate + ",    Status : " + cmbStatus.SelectedItem.Text
		FromDate = txtFromDate.Text.ToString
		ToDate = txtToDate.Text.ToString
		lblDateRangeFrom.Text = "Date Range : " + New SmartDate(FromDate).FormattedText + " To " + New SmartDate(ToDate).FormattedText
		lblWONo.Text = "Work Order No : " & IIf(WOText = "", "(ALL)", WOText + IIf(txtCWPNo.Text.Trim = "0" Or txtCWPNo.Text = "", "", "-" + txtCWPNo.Text.Trim))
		lblCust.Text = "Customer : " & cmbCustomerList.SelectedItem.Text
		lblStatus.Text = "Status  : " & cmbStatus.SelectedItem.Text
	End Sub

	Public Sub DataFieldBind()
		txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
		txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
		mUser = CType(Session("mUser"), User)
		mEventLog = Session("mEventLog")
		If mUser Is Nothing Then mUser = mUser.GetUser(mEventLog.UserID)

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(ALL)")
		cmbCWP.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText
		mCustomerList = VendorList.GetVendorstList(0, , , , , , "(ALL)", True)
		cmbCustomerList.DataSource = mCustomerList
		DataBind()
	End Sub

#End Region

#Region " Events "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
		EventLogID = CType(Session("EventLogID"), Guid)
		ClearAll()
		GetSession()
		If Not IsPostBack Then
			Session("MiddleFrame") = "wfnWOInvoiceRegisterList_Ajax.aspx?"
			mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))
			Session("mEventLog") = mEventLog
			DataFieldBind()
		End If
	End Sub

	Private Sub CurrentSearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click
		If IsValid Then
			ControlVisibility2()
			SetValues()
			upnlDisplaySearchCriteria.Update()
		End If
	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

		Dim da As New ObjectAdapter
		Dim myReport As Engine.ReportClass
		Dim mCompanyDetail As New CompanyDetail
		Dim rpt As WOInvoiceRegisterList
		Dim ds As New dsWOInvoice
		myReport = New crWOInvoiceRegister

		SetValues()
		Dim CWPNo As String
		If Trim(txtCWPNo.Text) = "0" Or txtCWPNo.Text = "" Then
			CWPNo = ""
		Else
			CWPNo = "-" + txtCWPNo.Text
		End If

		rpt = WOInvoiceRegisterList.GetInvoiceRegisterList(FromDate:=StartDate, ToDate:=EndDate, WOText:=WOText,
														   WONo:=WONo, StatusID:=CInt(cmbStatus.SelectedValue),
														   CustomerID:=cmbCustomerList.SelectedValue.ToString)
		If rpt.Count <= 0 Then
			MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound, MSGBox.Message_Text.NoRecordFound,
							"There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1366)
			MarkLog(Action.Print, "WOInvoiceRegister", EventLogDetail, ErrorType.NoError,
					Guid.Empty, EventLogID)
		End If
		Dim Report As New ReportData(CompanyName:=mCompanyDetail.CompanyName,
									 Address:=mCompanyDetail.Address,
									 Tel1:=mCompanyDetail.Tel1,
									 Tel2:=mCompanyDetail.Tel2,
									 Fax:=mCompanyDetail.Fax,
									 Email:=mCompanyDetail.Email,
									 WebSite:=mCompanyDetail.WebSite,
									 ReportName:="",
									 ProductVersion:="",
									 SINote:="",
									 SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText,
									 SearchStr2:=New SmartDate(txtToDate.Text).FormattedText,
									 SearchStr3:="",
									 SearchStr4:="",
									 SearchStr5:="",
									 SearchStr6:=cmbCustomerList.SelectedItem.ToString,
									 SearchStr7:=IIf(WOText = "", "(ALL)", cmbCWP.SelectedItem.ToString + CWPNo),
									 SearchStr8:="",
									 SearchStr9:="",
									 SearchStr10:=AppSettings("Logo"),
									 SearchStr11:=AppSettings("MROISONo"),
									 SearchStr12:="TELEFAX:" & mCompanyDetail.Fax & " " & mCompanyDetail.Email,
									 SearchStr13:=cmbStatus.SelectedItem.ToString,
									 SearchStr14:="",
									 SearchStr16:="",
									 SearchStr15:="",
									 SearchStr17:="",
									 SearchStr18:="",
									 SearchStr19:="",
									 SearchStr20:="",
									 SearchStr21:="",
									 SearchStr22:="",
									 SearchStr23:="",
									 SearchStr24:="",
									 SearchStr25:=""
									 )

		ds.Clear()
		Dim mRptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mRptImage)
		da.Fill(ds, rpt)
		da.Fill(ds, Report)
		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport
		Dim FunctionName As String
		FunctionName = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", FunctionName, True)
	End Sub

	Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub

#End Region

End Class