'***********************************
'Modified by Harsh Sugandhi On 05th November 2025 => To Retrieve Last Updated by & Created by
'***********************************


Public Class DentAndRepairRectificationListPage
	Inherits Page


#Region " Variable Declaration "

	Private mDentBuckle As DentBuckle
	Private mDentBuckleItems As DentBuckleItems
	Private DistinctDentRepairText As DistinctDentBuckelText

	Dim EventLogID As Guid
	Dim DateIndex, FromDate, ToDate, Text, StatusID, No, RegNo, ItemNo, Description As String

#End Region

#Region " Helper Methods "

	Private Sub GetSession()
		mDentBuckleItems = Session("mDentBuckleItems")
		DistinctDentRepairText = Session("mDistinctDentBuckelText")
		Text = Session("Text")
		No = IIf(IsNothing(Session("No")), 0, Session("No"))
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		DateIndex = Session("DateIndex")
		StatusID = IIf(IsNothing(Session("StatusID")), 0, Session("No"))
		RegNo = Session("RegNo")
		ItemNo = Session("ItemNo")
		Description = Session("Description")
	End Sub

	Private Sub RemoveSession()
		Session.Remove("FromDate")
		Session.Remove("ToDate")
		Session.Remove("DateIndex")
		Session.Remove("StatusID")
		Session.Remove("No")
		Session.Remove("RegNo")
		Session.Remove("Text")
		Session.Remove("ItemNo")
		Session.Remove("Description")
	End Sub

	Private Sub ClearAll()

		If Session("MiddleFrame") <> "wfDentAndRepairRectificationList_Ajax.aspx?" Then

			Session.Remove("FromDate")
			Session.Remove("ToDate")
			Session.Remove("DateIndex")
			Session.Remove("StatusID")
			Session.Remove("No")
			Session.Remove("RegNo")
			Session.Remove("Text")
			Session.Remove("ItemNo")
			Session.Remove("Description")

		End If

	End Sub

	Private Sub ControlVisibility(Index As Int16)

		Try

			lblFromDate.Visible = IIf(Index <> 0, True, False)
			lblToDate.Visible = IIf(Index <> 0, True, False)

			If Index = 6 Then

				txtFromDate.Visible = True
				txtToDate.Visible = True
				txtFromDate.Enabled = True
				txtToDate.Enabled = True

			ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then

				txtFromDate.Visible = True
				txtToDate.Visible = True
				txtFromDate.Enabled = False
				txtToDate.Enabled = False

			Else
				txtFromDate.Visible = False
				txtToDate.Visible = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		MsgBoxResult = MSGBoxCtrl.Result

		Try

			If MsgBoxResult > 0 Then
				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

					Case MsgBoxResult.No

					Case MsgBoxResult.Ok

					Case MsgBoxResult.Ok

				End Select

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetPage()
		lblResult.Text = $"List of Item(s) as per selected criteria : {mDentBuckleItems.Count} Record(s) shown."
	End Sub

	Private Sub SetGrid()

		Dim B As Integer
		Try

			For j As Integer = 0 To dgItems.Rows.Count - 1
				B = CType(Me.dgItems.Rows(j).Cells(14).Text, Integer)
				If B = 3 Then
					dgItems.Rows(j).Cells(13).Enabled = False
				End If
			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetVariables()

		Try

			DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
			FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
			ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
			Text = IIf(cmbDentBuckelNo.SelectedIndex <= 0, "", cmbDentBuckelNo.SelectedValue)
			RegNo = txtRegNo.Text.Trim
			No = IIf(cmbDentBuckelNo.SelectedIndex <= 0 Or txtNo.Text.Trim = "", 0, txtNo.Text.Trim)
			StatusID = IIf(cmbDentBuckelStatus.SelectedIndex <= 0, 0, cmbDentBuckelStatus.SelectedValue.ToString)
			ItemNo = txtItemNo.Text.Trim
			Description = txtDescription.Text.Trim
			Session("FromDate") = FromDate
			Session("ToDate") = ToDate
			Session("DateIndex") = DateIndex
			Session("StatusID") = StatusID
			Session("No") = No
			Session("RegNo") = RegNo
			Session("Text") = Text
			Session("ItemNo") = ItemNo
			Session("Description") = Description

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub


	Private Sub FindNow(Optional Text As String = "",
						Optional No As Int32 = 0,
						Optional FromDate As String = "",
						Optional ToDate As String = "",
						Optional RegNo As String = "",
						Optional StatusID As Integer = 0,
						Optional ItemNo As String = "",
						Optional Description As String = "")

		Try

			mDentBuckleItems = Nothing
			dgItems.DataSource = Nothing

			mDentBuckleItems = DentBuckleItems.GeDentBuckleItems(Text:=Text,
																 No:=No,
																 FromDate:=FromDate,
																 ToDate:=ToDate,
																 StatusID:=StatusID,
																 RegNo:=RegNo,
																 ItemNo:=ItemNo,
																 Description:=Description)
			dgItems.DataSource = mDentBuckleItems
			Session("mDentBuckleItems") = mDentBuckleItems
			dgItems.DataBind()

			SetGrid()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetPeriod(Index As Int32)

		Try

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
				Case 3 'Last 1 Quarter

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

			Session("FromDate") = txtFromDate.Text
			Session("ToDate") = txtToDate.Text

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetControl()

		Try

			SetPeriod(Index:=DateIndex)

			FindNow(Text:=Text,
					No:=No,
					FromDate:=txtFromDate.Text,
					ToDate:=txtToDate.Text,
					RegNo:=RegNo,
					StatusID:=StatusID,
					ItemNo:=ItemNo,
					Description:=Description)

			cmbDate.SelectedIndex = DateIndex
			cmbDentBuckelStatus.SelectedValue = StatusID

			If cmbDentBuckelNo.Items.Contains(New ListItem(Text)) Then
				cmbDentBuckelNo.SelectedValue = Text
			Else
				cmbDentBuckelNo.SelectedValue = "(ALL)"
			End If

			txtItemNo.Text = ItemNo
			txtDescription.Text = Description
			txtNo.Text = No
			txtRegNo.Text = RegNo

			ControlVisibility(DateIndex)
			SetPage()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
			FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
			ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)

			ItemNo = Session("ItemNo")
			Description = Session("Description")
			StatusID = Session("StatusID")
			Text = Session("Text")
			No = Session("No")

			DistinctDentRepairText = DistinctDentBuckelText.GetDistinctText(AddTopItem:="(ALL)")
			cmbDentBuckelNo.DataSource = DistinctDentRepairText
			Session("mDistinctDentBuckelText") = DistinctDentRepairText

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			ClearAll()
			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)

			If Not IsPostBack Then

				Session("MiddleFrame") = "wfDentBuckleRectificationList_Ajax.aspx?"
				ControlVisibility(1)
				SetPeriod(1)
				cmbDate.SelectedIndex = 1
				DataFieldBind()
				SetControl()
				SetGrid()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnBack.Click

		Try

			RemoveSession()
			Session("MiddleFrame") = ""
			Response.Redirect("Dashboard.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DentAndRepairItems_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgItems.PageIndexChanging

		Try

			dgItems.PageIndex = e.NewPageIndex
			dgItems.DataSource = mDentBuckleItems
			Session("mDentBuckleItems") = mDentBuckleItems
			dgItems.DataBind()

			SetGrid()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DueMonitoringList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgItems.RowCommand

		Dim ID, ItemID As Guid
		Try

			ID = New Guid(dgItems.DataKeys(CInt(e.CommandArgument)).Values("DentbuckleID").ToString)
			ItemID = New Guid(dgItems.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

			Select Case e.CommandName
				Case "Rectify"

					If Not User.IsInRole("Dent&RepairRectificationView") Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
										MSGBox.Message_Text.Authorization,
										"",
										MsgBoxStyle.OkOnly,
										"")

						Exit Sub

					End If

					mDentBuckle = DentBuckle.GetDentBuckle(ID)
					mDentBuckle.DentBuckleItems.CurrentIndex = mDentBuckle.DentBuckleItems.IndexOfItem(ItemID)
					Session("mDentBuckle") = mDentBuckle
					Session("Edit") = True

					ScriptManager.RegisterStartupScript(Me, [GetType], "Open Script", "openPageInSameTab('wfDentBuckelItems_Ajax.aspx?BackPage=Index.aspx');", True)

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DentAndRepairItems_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgItems.Sorting

		Try

			mDentBuckleItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("mDentBuckleItems") = mDentBuckleItems
			dgItems.DataSource = mDentBuckleItems
			dgItems.DataBind()

			SetGrid()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Private Sub RegNo_Changed(sender As Object, e As EventArgs) Handles txtRegNo.TextChanged,
																		txtItemNo.TextChanged,
																		txtDescription.TextChanged,
																		cmbDentBuckelStatus.SelectedIndexChanged,
																		cmbDentBuckelNo.SelectedIndexChanged,
																		txtNo.TextChanged,
																		txtFromDate.TextChanged,
																		txtToDate.TextChanged

		Try

			SetVariables()

			FindNow(Text:=Text,
					No:=No,
					FromDate:=FromDate,
					ToDate:=ToDate,
					RegNo:=RegNo,
					StatusID:=StatusID,
					ItemNo:=ItemNo,
					Description:=Description)

			SetPage()
			upnlgrid.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Date_Changed(sender As Object, e As EventArgs) Handles cmbDate.SelectedIndexChanged

		Dim Index As Int16 = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)

		Try
			ControlVisibility(Index)
			SetPeriod(Index)

			If cmbDate.Enabled = True Then
				cmbDate.Focus()
			End If

			SetVariables()

			FindNow(Text:=Text,
					No:=No,
					FromDate:=FromDate,
					ToDate:=ToDate,
					RegNo:=RegNo,
					StatusID:=StatusID,
					ItemNo:=ItemNo,
					Description:=Description)

			SetPage()
			upnlgrid.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class