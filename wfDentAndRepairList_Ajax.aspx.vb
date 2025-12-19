'***********************************
'Modified by Harsh Sugandhi On 05th November 2025 => To Retrieve Last Updated by & Created by
'***********************************


Public Class DentAndRepairListPage
	Inherits Page

#Region " Variable(s) "

	Private FileAttach As FileAttach
	Private DentBuckle As DentBuckle
	Private DentBuckleList As DentBuckleList
	Private AttachmentHelper As New AttachmentHelper
	Private AuthorizationHelper As New AuthorizationHelper
	Private DistinctDentBuckleText As DistinctDentBuckelText

	Dim EventLogID As Guid
	Dim TotalCount As Integer
	Dim DateIndex, FromDate, ToDate, Text, StatusID, No, RegNo As String

#End Region

#Region " Helper Method(s) "

	Private Sub GetSession()

		Text = Session("Text")
		RegNo = Session("RegNo")
		ToDate = Session("ToDate")
		FromDate = Session("FromDate")
		StatusID = Session("StatusID")
		DateIndex = Session("DateIndex")
		DentBuckle = Session("mDentBuckle")
		DentBuckleList = Session("mDentBuckleList")
		DistinctDentBuckleText = Session("DistinctDentBuckleText")
		No = IIf(IsNothing(Session("No")), 0, Session("No"))

	End Sub

	Private Sub RemoveSession()

		Session.Remove("No")
		Session.Remove("Text")
		Session.Remove("RegNo")
		Session.Remove("ToDate")
		Session.Remove("FromDate")
		Session.Remove("StatusID")
		Session.Remove("DateIndex")
		Session.Remove("mDentBuckle")
		Session.Remove("mDentBuckleList")

	End Sub

	Private Sub ClearAll()

		If InStr(Session("MiddleFrame"), "wfDentAndRepairList_Ajax.aspx") <= 0 Then
			RemoveSession()
		End If

	End Sub

	Private Sub AddAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub SetGrid()
		txtNo.Visible = IIf(cmbDentAndRepairNo.SelectedIndex > 0, True, False)
		lblNo.Visible = IIf(cmbDentAndRepairNo.SelectedIndex > 0, True, False)
	End Sub

	Private Sub SetPeriod(Index As Int32)

		Try

			Select Case Index

				Case 0 'All'
					txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
					txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
				Case 1 'Last 1 Week
					txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
				Case 2 'Last 1 Month
					txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
				Case 3 'Last 1 Quarter
					Select Case Today.Month
						Case 1, 2, 3
							txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
							txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
						Case 4, 5, 6
							txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
							txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						Case 7, 8, 9
							txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
							txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						Case 10, 11, 12
							txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
							txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
					End Select
				Case 4 'Last 1 Year
					txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
				Case 5 'Current Financial Year
					If Today.Month <= 3 Then  'Jan|Feb|Mar
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
					Else
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
					End If
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
				Case 6 'Between Dates
					FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
					ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
					txtFromDate.Text = FromDate
					txtToDate.Text = ToDate
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetVariables()

		Try

			DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
			FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
			ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
			Text = IIf(cmbDentAndRepairNo.SelectedIndex <= 0, "", cmbDentAndRepairNo.SelectedValue)
			RegNo = txtRegNo.Text.Trim
			No = IIf(cmbDentAndRepairNo.SelectedIndex <= 0, 0, txtNo.Text.Trim)
			StatusID = IIf(cmbDentAndRepairStatus.SelectedIndex <= 0, 0, cmbDentAndRepairStatus.SelectedValue.ToString)
			Session("FromDate") = FromDate
			Session("ToDate") = ToDate
			Session("DateIndex") = DateIndex
			Session("StatusID") = StatusID
			Session("No") = No
			Session("RegNo") = RegNo
			Session("Text") = Text

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetFromToDate()

		Try

			FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
			ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
			Session("FromDate") = FromDate
			Session("ToDate") = ToDate

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FindNow(Optional Text As String = "",
						Optional No As Integer = 0,
						Optional FromDate As String = "",
						Optional ToDate As String = "",
						Optional RegNo As String = "",
						Optional StatusID As Integer = 0)

		Try

			DentBuckleList = Nothing
			gvDentAndRepairList.DataSource = Nothing

			DentBuckleList = DentBuckleList.GetDentBuckleList(Text:=Text,
															   No:=No,
															   FromDate:=FromDate,
															   ToDate:=ToDate,
															   StatusID:=StatusID,
															   RegNo:=RegNo)
			gvDentAndRepairList.DataSource = DentBuckleList
			Session("mDentBuckleList") = DentBuckleList

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetControl()

		Try

			SetPeriod(DateIndex)
			SetFromToDate()

			FindNow(Text:=Text,
					No:=Val(No),
					FromDate:=txtFromDate.Text,
					ToDate:=txtToDate.Text,
					RegNo:=RegNo,
					StatusID:=Val(StatusID))

			gvDentAndRepairList.DataBind()

			cmbDate.SelectedIndex = DateIndex
			txtRegNo.Text = RegNo

			cmbDentAndRepairNo.SelectedValue = IIf(Text = "", "(ALL)", Text)
			txtNo.Text = No

			ControlVisibility(DateIndex)
			gvDentAndRepairList.DataBind()
			lblResult.Text = $"List of Dent & Repair Entry: {DentBuckleList.Count} Record(s) found."

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibility(Optional DateIndex As Int32 = 0)

		Try

			If DateIndex = 6 Then
				txtFromDate.Enabled = True
				txtToDate.Enabled = True
			ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
				txtFromDate.Enabled = False
				txtToDate.Enabled = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetTitle()

		Dim mDentBuckleList As DentBuckleList
		Try

			mDentBuckleList = DentBuckleList.GetDentBuckleList(FromDate:="01-Jan-1900",
															   ToDate:="31-Dec-2200")
			Session("TotalCount") = mDentBuckleList.Count
			TotalCount = Session("TotalCount")
			lblHeader.Text = $"List of Dent & Repair"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GetAttachment(ID As Guid, IsAttachmentsAdded As Boolean)

		Try

			If IsAttachmentsAdded = True Then
				FileAttach = FileAttach.GetAttachment(ID, 1) 'Sort = 1 - Removal
				Session("mFileAttach") = FileAttach
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Try

			Dim MsgBoxResult As MsgBoxResult
			MsgBoxResult = MSGBoxCtrl.Result
			Dim msgCount As Integer = 0

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult

					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Dim TextNo As String

							Try

								Dim mDentBuckle As DentBuckle
								Session("sender") = ""
								mDentBuckle = CType(Session("mDentBuckle"), DentBuckle)
								TextNo = mDentBuckle.TextNo
								DentBuckle.DeleteDentBuckle(mDentBuckle.ID)
								DataFieldBind()
								SetControl()
								SetGrid()
								upnlGrid.Update()
								upnlActionBtn.Update()

							Catch ex As SqlException

								If ex.Number = 8145 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 2627 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 547 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
									MarkLog(Action.Delete, "Dent&RepairChart", $"Cannot Delete ( {TextNo} ) as it is Currently in use.", ErrorType.NoError, DentBuckle.ID, EventLogID)
								End If
								DataFieldBind()
								SetControl()
								SetGrid()
								upnlGrid.Update()
								msgCount = ex.Errors.Count

							Finally

								If msgCount = 0 Then
									MarkLog(Action.Delete,
											"Dent&RepairChart",
											TextNo,
											ErrorType.NoError,
											DentBuckle.ID,
											EventLogID)

								End If

							End Try

						End If

					Case MsgBoxResult.No
						Session("sender") = ""
					Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
						Session("sender") = ""
					Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
						Session("sender") = ""
				End Select

			ElseIf MsgBoxResult = -1 Then
				Session("sender") = ""
			ElseIf MsgBoxResult = 0 And Session("sender") = "Authorization" Then   'Code Added
				Session("sender") = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub NewRecord()

		DentBuckle = DentBuckle.NewDentBuckle(Guid.NewGuid)
		Session("mDentBuckle") = DentBuckle

	End Sub

#End Region

#Region " DataFieldBind "

	Private Sub DataFieldBind()

		Try

			FromDate = IIf(IsNothing(FromDate), "1/1/1900", txtFromDate.Text.ToString)
			ToDate = IIf(IsNothing(ToDate), "1/1/2200", txtToDate.Text.ToString)
			DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

			DistinctDentBuckleText = DistinctDentBuckelText.GetDistinctText("(ALL)")
			cmbDentAndRepairNo.DataSource = DistinctDentBuckleText
			Session("DistinctDentBuckleText") = DistinctDentBuckleText

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Event(s) "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try

			ClearAll()
			AddAttributes()
			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)

			If Not IsPostBack Then

				Session("MiddleFrame") = "wfDentAndRepairList_Ajax.aspx"
				DataFieldBind()
				SetControl()

				'Added by Harsh on 3rd September 2024 for FLYPAL-1860 Resolving Issues related to Dent & Repair Module
				If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Dent&RepairChart") Then

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"MarkAsFavourite",
														"MarkAsFavourite();",
														True)

				Else

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"RemoveFromFavourite",
														"RemoveFromFavourite();",
														True)

				End If

			End If

			SetGrid()
			SetTitle()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Date_Changed(sender As Object, e As EventArgs) Handles cmbDate.SelectedIndexChanged

		Try

			Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
			Session("DateIndex") = DateIndex
			ControlVisibility(DateIndex)
			SetPeriod(DateIndex)

			If cmbDate.Enabled = True Then
				SetFocus(cmbDate)
			End If

			SetVariables()

			FindNow(Text,
					Val(No),
					FromDate,
					ToDate,
					RegNo,
					0)

			gvDentAndRepairList.DataBind()
			SetGrid()
			ControlVisibility()
			lblResult.Text = "List of Dent and Buckle entry as per criteria : " & DentBuckleList.Count & " Record(s) found"
			upnlGrid.Update()
			upnlActionBtn.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ToDate_Changed(sender As Object, e As EventArgs) Handles txtFromDate.TextChanged,
																		 txtToDate.TextChanged,
																		 txtRegNo.TextChanged,
																		 cmbDentAndRepairStatus.SelectedIndexChanged,
																		 cmbDentAndRepairNo.SelectedIndexChanged,
																		 txtNo.TextChanged

		Try

			SetVariables()

			FindNow(Text,
					Val(No),
					FromDate,
					ToDate,
					RegNo,
					Val(StatusID))

			gvDentAndRepairList.DataBind()
			SetGrid()
			ControlVisibility()
			lblResult.Text = "List of Dent and Buckle entry as per criteria :" & DentBuckleList.Count & " Record(s) found"
			upnlGrid.Update()
			upnlGrid.Update()
			upnlActionBtn.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_DentBuckleList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDentAndRepairList.PageIndexChanging

		Try

			gvDentAndRepairList.PageIndex = e.NewPageIndex
			gvDentAndRepairList.DataSource = DentBuckleList
			Session("mDentBuckleList") = DentBuckleList
			gvDentAndRepairList.DataBind()
			SetGrid()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_DentBuckleList__RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDentAndRepairList.RowCommand

		Try

			Dim Index As Integer = CInt(e.CommandArgument) + gvDentAndRepairList.PageSize * gvDentAndRepairList.PageIndex
			Dim ID As Guid = DentBuckleList(Index).ID

			DentBuckle = DentBuckle.GetDentBuckle(ID:=ID)
			Session("mDentBuckle") = DentBuckle
			Dim Detail As String = $"DentBuckle : {DentBuckle.TextNo} Dated : {DentBuckle.ReportDateFormatted}"

			Select Case e.CommandName

				Case "EditRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	Action:={Action.Edit},
																	MarkLogDetail:=Detail,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:="Dent&RepairChart") Then

						Exit Sub

					End If

					MarkLog(Action.Edit,
							"Dent&RepairChart",
							Detail,
							ErrorType.NoError,
							DentBuckle.ID,
							EventLogID)

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"OpenScript",
														"openledgersame('wfDentAndRepair_Ajax.aspx?BackPage=Index.aspx');",
														True)

				Case "DeleteRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MarkLogDetail:=Detail,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	Action:={Action.Delete},
																	ModuleName:="Dent&RepairChart") Then

						Exit Sub

					End If

					MSGBoxCtrl.Show(MSGBox.Message_Title.Delete,
									MSGBox.Message_Text.Delete,
									"",
									MsgBoxStyle.YesNo,
									"Delete")

					DentBuckle = DentBuckle.GetDentBuckle(ID:=ID)
					Session("mDentBuckle") = DentBuckle

				Case "ViewRec"

					FileAttach = FileAttach.GetAttachment(ReferenceID:=ID)

					AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

					ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_CWPList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvDentAndRepairList.Sorting

		Try

			DentBuckleList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			gvDentAndRepairList.DataSource = DentBuckleList
			Session("mDentBuckleList") = DentBuckleList
			gvDentAndRepairList.DataBind()
			SetGrid()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddNewRecord(sender As Object, e As EventArgs) Handles btnAddNew.Click

		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															Action:={Action.[New]},
															ModuleName:="Dent&RepairChart") Then

				Exit Sub

			End If

			NewRecord()

			MarkLog(Action.[New],
					"Dent&RepairChart",
					"",
					ErrorType.NoError,
					DentBuckle.ID,
					EventLogID)

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"OpenScript",
												"openledgersame('wfDentAndRepair_Ajax.aspx?BackPage=index.aspx');",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click

		Session("MiddleFrame") = ""
		RemoveSession()
		Response.Redirect("Dashboard.aspx")

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		Try

			PrintWithPDF()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub PrintWithPDF()

		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															Action:={Action.Print},
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:="Dent&RepairChart") Then

				Exit Sub

			End If

			Dim dataAdapter As New ObjectAdapter
			Dim crystalReport As Engine.ReportClass
			Dim CompanyDetail As New CompanyDetail
			Dim dataSet As New dsDentBuckleRptList
			Dim DentBuckleRptList As DentBuckleRptList

			If AppSettings("ClientCode") = "CMX" Then 'Adde by Ajay on 13-Oct-2022 CMX12102022
				crystalReport = New crptDentBuckleRegisterCMX
			Else
				crystalReport = New crptDentBuckleRegisterNOVO
			End If

			DentBuckleRptList = DentBuckleRptList.GetDentBuckleList(No:=No,
																	RegNo:=RegNo,
																	Text:=Text,
																	ToDate:=ToDate,
																	FromDate:=FromDate,
																	StatusID:=StatusID)

			If DentBuckleRptList.Count = 0 Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound,
								MSGBox.Message_Text.NoRecordFound,
								"There is no record for this search criteria",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			Dim Report As New ReportData(CompanyDetail.CompanyName,
										 CompanyDetail.Address,
										 CompanyDetail.Tel1,
										 CompanyDetail.Tel2,
										 CompanyDetail.Fax,
										 CompanyDetail.Email,
										 CompanyDetail.WebSite,
										 "",
										 SearchStr1:=IIf(cmbDate.SelectedIndex = 0, "", txtFromDate.Text),
										 SearchStr2:=IIf(cmbDate.SelectedIndex = 0, "", txtToDate.Text),
										 SearchStr3:=txtRegNo.Text,
										 SearchStr4:=AppSettings("ClientCode"),
										 SearchStr5:="",
										 ProductVersion:=AppSettings("Product Version"),
										 SINote:=AppSettings("SINote"),
										 SearchStr6:=Text + "-" + No.Trim,
										 SearchStr7:=cmbDentAndRepairStatus.SelectedItem.Text,
										 "",
										 AppSettings("Government Authority"),
										 AppSettings("Logo"))

			Dim companyLogo As rptImage = rptImage.GetImage(dataSet)

			dataAdapter.Fill(dataSet, DentBuckleRptList)
			dataAdapter.Fill(dataSet, Report)
			dataAdapter.Fill(dataSet, companyLogo)

			crystalReport.SetDataSource(dataSet)

			Session("CrystalReport") = crystalReport
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Display Report",
												"openTranDetail();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	'Added by Harsh on 3rd September 2024 for FLYPAL-1860 Resolving Issues related to Dent & Repair Module
	Private Sub MarkFavorite(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

		Try
			MarkFavourite(HttpContext.Current.User.Identity.Name, "Dent&RepairChart")
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub RemoveFavorite(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

		Try
			RemoveFavourite(HttpContext.Current.User.Identity.Name, "Dent&RepairChart")
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub
	'End

#End Region

End Class