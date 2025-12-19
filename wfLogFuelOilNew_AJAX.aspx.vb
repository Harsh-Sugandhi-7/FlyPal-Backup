'AJAX Conversion By Saylee On 24-Sep-2014

Public Class wfLogFuelOilNew_AJAX
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Dim mLogFuelAndOilList As LogFuelAndOilList
	Dim mMachineNameValueList As MachineNameValueList
	Dim FromDate As String
	Dim ToDate As String
	Dim Engine As String
	Dim MachineName As String
	Dim MachineID As String
	Public AircraftId As String

	Dim EventLogID As Guid
	Dim mLogDetail As String

	Dim StartDate As String
	Dim EndDate As String
	Dim Aircraft As String
	Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
	Dim objFuelOil As ReportFuelandOilRegister
	Dim mCompanyDetail As New CompanyDetail
	Dim da As New CSLA.Data.ObjectAdapter
	Dim dsFuelOil As New dsFuelOilRegister
#End Region

#Region " Helper Methods "
	Private Sub GetSession()
		mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
		mLogFuelAndOilList = CType(Session("mLogFuelAndOilList"), LogFuelAndOilList)
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		AircraftId = CType(Session("AircraftId"), String)
	End Sub
	Private Sub SetSession()
		Session("mMachineNameValueList") = mMachineNameValueList
		Session("mLogFuelAndOilList") = mLogFuelAndOilList
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("AircraftId") = AircraftId
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mMachineNameValueList")
		Session.Remove("mLogFuelAndOilList")
		Session.Remove("FromDate")
		Session.Remove("ToDate")
		Session.Remove("AircraftId")
	End Sub
	Private Sub ClearAll()
		If Session("MiddleFrame") <> "wfLogFuelOilNew_Ajax.aspx?" Then
			Session.Remove("mMachineNameValueList")
			Session.Remove("mLogFuelAndOilList")
			Session.Remove("FromDate")
			Session.Remove("ToDate")
			Session.Remove("AircraftId")
		End If
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub FindNow(Optional ByVal FromDate As String = "1-1-1900", Optional ByVal ToDate As String = "1-1-3300", Optional ByVal MachineID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal Show_100_Records As Boolean = False)

		mLogFuelAndOilList = LogFuelAndOilList.GetLogFuelAndOilList(MachineID, FromDate, ToDate, , Show_100_Records)
		'Set DataSource of the Grid
		dgLogFuelOilList.DataSource = mLogFuelAndOilList
		Session("mLogFuelAndOilList") = mLogFuelAndOilList
		dgLogFuelOilList.DataBind()
		If mLogFuelAndOilList.Count > 0 Then
			dgLogFuelOilList.Columns(15).HeaderText = mLogFuelAndOilList.Item(0).OilUpliftHeaderText.ToString 'added by shital on 11-Feb-2022
		End If

		DataBind()
		lblResult.Text = "As per criteria :" & mLogFuelAndOilList.Count & " Record(s) found."
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
				Case MsgBoxResult.No
					Session("sender") = ""
				Case MsgBoxResult.Ok
					Session("sender") = ""
					GetSession()
					DataFieldBind()
				Case MsgBoxResult.OK And Session("sender") = "Authorization"
					DataFieldBind()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 Then
			Session("sender") = ""
		End If
	End Sub
	Private Sub SetControl()
		dgLogFuelOilList.DataBind()
	End Sub
	Private Sub EditRecord(ByVal mID As Guid)
		Dim mLog As Log
		Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
		Dim mMachine As Machine = Machine.GetMachine(mMachineID)
		''   Session("mLogList") = mLogList
		Session("mLogList") = Nothing
		Session("mMachine") = mMachine
		mLog = Log.GetLog(mID)
		mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
		Session("mLog") = mLog
		mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
		MarkLog(Util.Action.Edit, "Log Fuel Oil", mLogDetail, Util.ErrorType.NoError, mLog.ID, EventLogID)

		AircraftId = Session("MachineID")
		Session("mOpenFromLogFuelNew") = True
		Session("OpenFromWO") = False

		'Dim str As String
		'str = "openledgersame('wfLogFuelOil_Ajax.aspx?ChildPage=index.aspx');"
		'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLogFuelOilWindow", "OpenLogFuelOilWindow()", True)
	End Sub
	Private Sub DataFieldBind()

		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)

		If (Not IsDate(FromDate) Or Not IsDate(ToDate)) Or (FromDate = "1/1/1900" Or ToDate = "1/1/2200") Then
			txtFromDate.Text = ""
			txtToDate.Text = ""
		Else
			txtFromDate.Text = FromDate
			txtToDate.Text = ToDate
		End If

		txtFromDate.DataBind()
		txtToDate.DataBind()

		Session("FromDate") = FromDate
		Session("ToDate") = ToDate

		mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , , , , True)
		cmbAircraft.DataSource = mMachineNameValueList
		Session("mMachineNameValueList") = mMachineNameValueList

		If mMachineNameValueList.Count <> 0 Then
			If IsNothing(AircraftId) Then AircraftId = mMachineNameValueList(0).ID.ToString Else AircraftId = AircraftId
		Else
			AircraftId = "00000000-0000-0000-0000-000000000000"
		End If
		Session("AircraftId") = AircraftId
		cmbAircraft.DataBind()


		mLogFuelAndOilList = LogFuelAndOilList.GetLogFuelAndOilList(AircraftId, FromDate, ToDate, Guid.Empty.ToString, True)
		dgLogFuelOilList.DataSource = mLogFuelAndOilList
		dgLogFuelOilList.DataBind()
		Session("mLogFuelAndOilList") = mLogFuelAndOilList
		If mLogFuelAndOilList.Count > 0 Then
			dgLogFuelOilList.Columns(15).HeaderText = mLogFuelAndOilList.Item(0).OilUpliftHeaderText.ToString 'added by shital on 11-Feb-2022
		End If

		DataBind()

		If mMachineNameValueList.Count > 1 And IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 1 Else cmbAircraft.SelectedValue = AircraftId
		AircraftId = cmbAircraft.SelectedValue
		Session("AircraftId") = AircraftId
		lblResult.Text = "As per criteria :" & mLogFuelAndOilList.Count & " Record(s) found."
		'DataBind()
	End Sub
	Private Sub SetReport()
		Dim OperatorName As String = ""
		MyReport = New crFuelAndOilSummery 'crFuelOil

		objFuelOil = ReportFuelandOilRegister.GetFuelOilRegisterList(cmbAircraft.SelectedValue.ToString, FromDate, ToDate, Guid.Empty.ToString, chkShowAll.Checked)   'cmbEngine.SelectedValue.ToString)

		'Added by Prashant on 11-Aug-2011
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
			Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
			If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
		End If

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
	mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
	mCompanyDetail.WebSite, "Fuel And Oil Register", New SmartDate(txtFromDate.Text.ToString).FormattedText, New SmartDate(txtToDate.Text.ToString).FormattedText, cmbAircraft.SelectedItem.Text, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"))

		If objFuelOil.Count = 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else

			RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 913)
		End If
		Dim mrptImage As rptImage = rptImage.GetImage(dsFuelOil)
		da.Fill(dsFuelOil, objFuelOil)
		da.Fill(dsFuelOil, mrptImage)
		da.Fill(dsFuelOil, Report)
		MyReport.SetDataSource(dsFuelOil)
		Session("CrystalReport") = MyReport

		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)

		ResetValues()
	End Sub
	Private Sub ResetValues()
		StartDate = txtFromDate.Text.ToString
		EndDate = txtToDate.Text.ToString
		MachineID = "{00000000-0000-0000-0000-000000000000}"
		Engine = ""
		Aircraft = ""
	End Sub
	Public Sub ControlVisibility()
		For j As Integer = 0 To dgLogFuelOilList.Rows.Count - 1
			Dim P As New Integer
			If mLogFuelAndOilList(j).LogTypeID = 3 Then
				dgLogFuelOilList.Rows(j).Cells(18).Enabled = False
			End If
		Next
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		ClearAll()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack Then
			Session("MiddleFrame") = "wfLogFuelOilNew_Ajax.aspx?"

			'Ajay 28-03-2022
			If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "LogFuelOil") Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
			Else
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
			End If
			'--------------------------
			DataFieldBind()
			ControlVisibility()


		End If
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		Session("AircraftId") = cmbAircraft.SelectedValue
		Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

		Session("FromDate") = FromDate
		Session("ToDate") = ToDate

		dgLogFuelOilList.PageIndex = 0

		If chkShowAll.Checked = True Then
			FindNow(FromDate, ToDate, mMachineID.ToString)
		Else
			FindNow(FromDate, ToDate, mMachineID.ToString, True)
		End If
		ControlVisibility()
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
	End Sub
	Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
		Session("AircraftId") = cmbAircraft.SelectedValue
		Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

		Session("FromDate") = FromDate
		Session("ToDate") = ToDate

		dgLogFuelOilList.PageIndex = 0

		If chkShowAll.Checked = True Then
			FindNow(FromDate, ToDate, mMachineID.ToString)
		Else
			FindNow(FromDate, ToDate, mMachineID.ToString, True)
		End If
		ControlVisibility()
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
	End Sub
	Private Sub dgLogFuelOilList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLogFuelOilList.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgLogFuelOilList.PageSize * dgLogFuelOilList.PageIndex
				Dim mId As Guid = mLogFuelAndOilList(Index).ID
				Dim mLog As Log
				mLog = Log.GetLog(mId)
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				'Added by Saylee on 8-Apr-2014 for ALL08042014
				If (Not User.IsInRole("LogFuelOilNew") And mLog.IsNew) Or (Not User.IsInRole("LogFuelOilEdit") And Not mLog.IsNew) Then
					'setObject()
					SetSession()
					MarkLog(Util.Action.Save, "LogFuelOil", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

					Exit Sub
				End If
				DataFieldBind()
				SetControl()
				EditRecord(mId)
				ControlVisibility()
				upnlGridView.Update()
				upnlActionBtnTop.Update()
				upnlActionBtnBottom.Update()
				upnlResult.Update()
		End Select
	End Sub
	Private Sub dgLogFuelOilList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLogFuelOilList.PageIndexChanging
		dgLogFuelOilList.PageIndex = e.NewPageIndex
		dgLogFuelOilList.DataSource = mLogFuelAndOilList
		Session("mLogFuelAndOilList") = mLogFuelAndOilList
		dgLogFuelOilList.DataBind()
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
		RemoveSession()
		Session("sender") = ""
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
		'Added by Saylee on 8-Apr-2014 for ALL08042014
		If (Not User.IsInRole("LogFuelOilPrint")) Then
			'setObject()
			SetSession()
			MarkLog(Util.Action.Print, "LogFuelOil", User.Identity.Name & " is not Authorized User to Print " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If IsValid = True Then
			SetReport()
		End If
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub hdnBtnLogFuelOil_Click(sender As Object, e As System.EventArgs) Handles hdnBtnLogFuelOil.Click
		DataFieldBind()
		SetControl()
		ControlVisibility()
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
	End Sub
	'Ajay 28-03-2022
	Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 07-Nov-2022
		MarkFavourite(HttpContext.Current.User.Identity.Name, "LogFuelOil")
	End Sub

	Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 07-Nov-2022
		RemoveFavourite(HttpContext.Current.User.Identity.Name, "LogFuelOil")
	End Sub
	'-----
#End Region

End Class