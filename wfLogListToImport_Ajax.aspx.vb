Imports System.Linq
Public Class wfLogListToImport_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declarations "
	Public mLog As Log
	Public mLogListForImport As LogList
	Dim dsMain As New DataSet
	Public mMachine As Machine
	Public mSearchListPilot As SearchList
	Public mSearchListPlace As SearchList
	Public mFlightLogClassificationList As FlightLogClassificationList
	Public mMachineNameValueList As MachineNameValueList
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		'mPrevLog = Session("mPrevLog")
		mLogListForImport = CType(Session("mLogListForImport"), LogList)
		dsMain = Session("dsMain")
		mMachine = Session("mMachine")
		mSearchListPlace = Session("mSearchListPlace")
		mSearchListPilot = Session("mSearchListPilot")
		mFlightLogClassificationList = Session("mFlightLogClassificationList")
		mMachineNameValueList = Session("mMachineNameValueList")
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mSearchListPlace")
		Session.Remove("mSearchListPilot")
		Session.Remove("mFlightLogClassificationList")
		Session.Remove("MachineIDToSet")
	End Sub
	Private Sub DataFieldBind()
		mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "(SELECT)", , SkipIsForInventoryAircarft:=True)
		Session("mMachineNameValueList") = mMachineNameValueList
		cmbAircraft.DataSource = mMachineNameValueList
		cmbAircraft.DataBind()

		'cmbAircraft.SelectedValue = mMachineNameValueList(1).ID.ToString
		cmbAircraft.SelectedValue = Session("MachineIDToSet").ToString

		mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(SELECT)")
		Session("mFlightLogClassificationList") = mFlightLogClassificationList

		mSearchListPilot = SearchList.GetSearchList("Pilot", "", "")
		Session("mSearchListPilot") = mSearchListPilot

		mSearchListPlace = SearchList.GetSearchList("Place", "", "")
		Session("mSearchListPlace") = mSearchListPlace

		If cmbAircraft.SelectedIndex > 0 Then
			mMachine = Machine.GetMachine(New Guid(cmbAircraft.SelectedValue))
			Session("mMachine") = mMachine
		End If


		mLogListForImport = LogList.GetLogList(New Guid(Session("MachineIDToSet").ToString), "01-Jan1900", "31-Dec-2200", True)
		Session("mLogListForImport") = mLogListForImport
		dgLastLogDetails.DataSource = (From Info As LogList.LogInfo In mLogListForImport
									   Select Info).ToList.Take(1)

		'dgLastLogDetails.DataSource = Session("mPrevLog")
		dgLastLogDetails.DataBind()
		dgImportLogList.DataSource = dsMain.Tables(0)
		'Dim dataView As New DataView(dsMain.Tables(0))
		'dataView.RowFilter( = "AutoID DESC, Name DESC" & dsMain.Tables(0).Columns("Date").ColumnName
		'dgImportLogList.DataSource = dataView.ToTable

		dgImportLogList.DataBind()
		lblResult.Text = "No. of Logs entries : " & dsMain.Tables(0).Rows.Count.ToString & " Record(s) found."
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
				Case MsgBoxResult.No
				Case MsgBoxResult.Ok
					If MSGBoxCtrl.Sender = "Success" Then
						RemoveSession()
						Dim mopenas As String = Request.QueryString("Type")
						If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
							ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
							Exit Sub
						End If
					End If
			End Select
		ElseIf Result1 = -1 Then

		ElseIf Result1 = 0 Then   'Code Added
		End If
	End Sub
#End Region

#Region " Events "
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack Then
			DataFieldBind()
		End If
	End Sub
	Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
		RemoveSession()
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub dgImportLogList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgImportLogList.PageIndexChanging
		dgImportLogList.PageIndex = e.NewPageIndex
		dgImportLogList.DataSource = dsMain.Tables(0)
		dgImportLogList.DataBind()
	End Sub
	Private Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
		Try
			Dim Count As Integer = 0
			If IsValid Then
				For j As Integer = 0 To dgImportLogList.Rows.Count - 1
					Dim chkBox As HtmlInputCheckBox
					chkBox = CType(dgImportLogList.Rows(j).FindControl("chkSelect"), HtmlInputCheckBox)
					If chkBox.Checked Then
						'mMachine = Machine.GetMachine(mMachine.ID)
						If mMachine.IsUTC Then
							mLog = Log.NewLog(mMachine, dgImportLogList.Rows(j).Cells(2).Text, "", dgImportLogList.Rows(j).Cells(10).Text, 1)
						Else
							mLog = Log.NewLog(mMachine, dgImportLogList.Rows(j).Cells(2).Text, dgImportLogList.Rows(j).Cells(10).Text, "", 1)
						End If

						mLog.IsUTC = mMachine.IsUTC
						mLog.IsTakeoffTouchDown = CType(AppSettings("TakeOffTouchDown"), Boolean)
						mLog.LogPageNo = dgImportLogList.Rows(j).Cells(3).Text
						mLog.FlightNo = dgImportLogList.Rows(j).Cells(4).Text
						mLog.PilotID1 = mSearchListPilot(dgImportLogList.Rows(j).Cells(5).Text).GId
						mLog.PilotID2 = mSearchListPilot(dgImportLogList.Rows(j).Cells(6).Text).GId
						mLog.FlightLogClassificationID = mFlightLogClassificationList(1).ID
						mLog.SourceID = mSearchListPlace.Item(dgImportLogList.Rows(j).Cells(8).Text).GId
						mLog.DestinationID = mSearchListPlace.Item(dgImportLogList.Rows(j).Cells(9).Text).GId
						'mLog.SouUniverseDateTime = dsMain.Tables(0).Rows(j).ItemArray(9).ToString
						'mLog.TakeOffUniverseDateTime = dsMain.Tables(0).Rows(j).ItemArray(10).ToString
						'mLog.DesUniverseDateTime = dsMain.Tables(0).Rows(j).ItemArray(12).ToString
						'mLog.TouchDownUniverseDateTime = dsMain.Tables(0).Rows(j).ItemArray(11).ToString
						If mMachine.IsUTC Then
							mLog.DesUniverseDateTime = dgImportLogList.Rows(j).Cells(13).Text
							mLog.TouchDownUniverseDateTime = dgImportLogList.Rows(j).Cells(12).Text
							mLog.SouUniverseDateTime = dgImportLogList.Rows(j).Cells(10).Text
							mLog.TakeOffUniverseDateTime = dgImportLogList.Rows(j).Cells(11).Text
						Else
							mLog.DesLocalDateTime = dgImportLogList.Rows(j).Cells(13).Text
							mLog.TouchDownLocalDateTime = dgImportLogList.Rows(j).Cells(12).Text
							mLog.SouLocalDateTime = dgImportLogList.Rows(j).Cells(10).Text
							mLog.TakeOffLocalDateTime = dgImportLogList.Rows(j).Cells(11).Text
						End If

						mLog.Remark = dgImportLogList.Rows(j).Cells(17).Text
						'mLog.TimeInAir = mLog.BlockTime
						mLog.TimeInAir = dgImportLogList.Rows(j).Cells(14).Text
						For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1
							mLog.LogAFAssemblies(i).Hours = dgImportLogList.Rows(j).Cells(14).Text
							'mLog.LogAFAssemblies(i).Landings = dsMain.Tables(0).Rows(j).ItemArray(14).ToString
							mLog.LogAFAssemblies(i).Cycles = dgImportLogList.Rows(j).Cells(16).Text
						Next
						For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
							mLog.LogAPUAssemblies(i).Hours = dgImportLogList.Rows(j).Cells(14).Text
							'mLog.LogAPUAssemblies(i).Landings = dsMain.Tables(0).Rows(j).ItemArray(14).ToString
							If mLog.LogAPUAssemblies(i).LogPeriods.Contains(3) Then
								mLog.LogAPUAssemblies(i).Cycles = dgImportLogList.Rows(j).Cells(16).Text
							End If
						Next
						For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1
							mLog.LogCGBAssemblies(i).Hours = dgImportLogList.Rows(j).Cells(14).Text
							'mLog.LogCGBAssemblies(i).Landings = dsMain.Tables(0).Rows(j).ItemArray(14).ToString
							If mLog.LogCGBAssemblies(i).LogPeriods.Contains(3) Then
								mLog.LogCGBAssemblies(i).Cycles = dgImportLogList.Rows(j).Cells(16).Text
							End If
						Next
						For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
							mLog.LogEngAssemblies(i).Hours = dgImportLogList.Rows(j).Cells(14).Text
							'mLog.LogEngAssemblies(i).Landings = dsMain.Tables(0).Rows(j).ItemArray(14).ToString
							If mLog.LogEngAssemblies(i).LogPeriods.Contains(3) Then
								mLog.LogEngAssemblies(i).Cycles = dgImportLogList.Rows(j).Cells(16).Text
							End If

						Next
						mLog.Save()
						Count += 1
					ElseIf Count > 0 Then
						Exit For
					End If
				Next
				If Count = 0 Then
					MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select atleast one Log", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				MSGBoxCtrl.show("Success", "Log(s) imported successfully", "", MsgBoxStyle.OkOnly, "Success")
				'Else
				'upnlValidationSummary.Update()
			End If
		Catch ex As Exception

		End Try
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
		If cmbAircraft.SelectedIndex > 0 Then
			mLogListForImport = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue), "01-Jan1900", "31-Dec-2200", True)
			Session("mLogListForImport") = mLogListForImport
			dgLastLogDetails.DataSource = (From Info As LogList.LogInfo In mLogListForImport
										   Select Info).ToList.Take(1)
			dgLastLogDetails.DataBind()

			mMachine = Machine.GetMachine(New Guid(cmbAircraft.SelectedValue))
			Session("mMachine") = mMachine
		Else
			dgLastLogDetails.DataSource = Nothing
			dgLastLogDetails.DataBind()
		End If

	End Sub
#End Region




	Private Sub dgImportLogList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgImportLogList.RowDataBound
		If (e.Row.RowType = DataControlRowType.DataRow) Then
			e.Row.Cells(2).Text = DateTime.Parse(e.Row.Cells(2).Text).ToString(AppSettings("DateFormat"))
			e.Row.Cells(2).Wrap = False
			e.Row.Cells(10).Text = DateTime.Parse(e.Row.Cells(10).Text).ToString(AppSettings("DateTimeFormatForImport"))
			e.Row.Cells(11).Text = DateTime.Parse(e.Row.Cells(11).Text).ToString(AppSettings("DateTimeFormatForImport"))
			e.Row.Cells(12).Text = DateTime.Parse(e.Row.Cells(12).Text).ToString(AppSettings("DateTimeFormatForImport"))
			e.Row.Cells(13).Text = DateTime.Parse(e.Row.Cells(13).Text).ToString(AppSettings("DateTimeFormatForImport"))
			e.Row.Cells(14).Text = DateTime.Parse(e.Row.Cells(14).Text).ToString("HH:mm")
		End If
	End Sub
End Class