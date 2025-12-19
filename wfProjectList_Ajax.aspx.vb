'*************************************************
'Created By Prashant
'Modified by Harsh Sugandhi on 14th Jan 2025 for FLYPAL-2077
'Modified by Harsh Sugandhi on 10th Oct 2025 for FLYPAL-2698
'*************************************************


Public Class ProjectListPage
	Inherits Page


#Region " Variable Declaration "

	Public Project As Project
	Public FileAttach As FileAttach
	Public ProjectList As ProjectList
	Public AttachmentHelper As New AttachmentHelper
	Public AuthorizationHelper As New AuthorizationHelper
	Public ProjectDistinctTextList As ProjectDistinctTextList

	Dim Prefix As String
	Dim EventLogID As Guid
	Dim ProjectDetails As String
	Public TransTypeID As Trans 'Added by Saylee on 14-Nov-2024
	Dim SearchIndex, DateIndex, FromDate, ToDate, ProjectText, ProjectNo, SearchText As String

#End Region

#Region " Helper Method(s) "

	Private Sub GetSession()

		Project = Session("mProject")
		ProjectList = Session("mProjectList")
		ProjectDistinctTextList = Session("mDistinctTextListForProject")
		SearchIndex = Session("SearchIndex")
		DateIndex = Session("DateIndex")
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		ProjectText = Session("ProjectText")
		ProjectNo = IIf(IsNothing(Session("ProjectNo")), 0, Session("ProjectNo"))
		SearchText = Session("SearchText")
		TransTypeID = Session("TransTypeID") 'Added by Saylee on 14-Nov-2024

	End Sub

	Private Sub SetSession()

		Session("mProject") = Project
		Session("mProjectList") = ProjectList
		Session("mDistinctTextListForProject") = ProjectDistinctTextList
		SearchText = Session("SearchText")
		Session("TransTypeID") = TransTypeID 'Added by Saylee on 14-Nov-2024

	End Sub

	Private Sub RemoveSession()

		Session.Remove("mProject")
		Session.Remove("mProjectList")
		Session.Remove("mDistinctTextListForProject")
		Session.Remove("SearchText")
		Session.Remove("SearchIndex")
		Session.Remove("DateIndex")
		Session.Remove("FromDate")
		Session.Remove("ToDate")
		Session.Remove("ProjectText")
		Session.Remove("ProjectNo")
		Session.Remove("BackPage")
		Session.Remove("TransTypeID")

	End Sub

	Private Sub ClearAll()

		If Session("TransTypeID") Is Nothing Then
			TransTypeID = Request.QueryString("TransTypeId")
		Else
			TransTypeID = Session("TransTypeID")
		End If

		If (InStr(Session("MiddleFrame"), "wfProjectList_Ajax.aspx?TransTypeID=" & Request.QueryString("TransTypeId")) <= 0) Then

			Session.Remove("mProject")
			Session.Remove("mProjectList")
			Session.Remove("mDistinctTextListForProject")
			Session.Remove("SearchIndex")
			Session.Remove("DateIndex")
			Session.Remove("FromDate")
			Session.Remove("ToDate")
			Session.Remove("ProjectText")
			Session.Remove("ProjectNo")
			Session.Remove("BackPage")
			Session.Remove("IsProjectForRenew")
			Session.Remove("TransTypeID")

		End If

	End Sub

	Private Sub SetControl()

		Try

			SetPeriod(DateIndex)
			CallFindNow()

			dgProjectList.DataBind()
			If cmbProjectText.Items.Contains(New ListItem(ProjectText)) Then
				cmbProjectText.SelectedValue = ProjectText
			Else
				cmbProjectText.SelectedValue = "(ALL)"
			End If

			txtNo.Text = ProjectNo
			lblResult.Text = "As per criteria : " & ProjectList.Count & " Record(s) found."

			If SearchText IsNot Nothing Then
				SearchText = IIf(SearchText = "", "", SearchText)
			Else
				SearchText = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub NewRecord()

		Try

			Project = Project.NewProject(TransTypeID:=TransTypeID)
			Project.ProjectDate = Today.Date
			Session("mProject") = Project
			Session("TransTypeID") = TransTypeID

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub EditRecord(ID As Guid)

		Try

			Project = Project.GetProject(ID:=ID)

			If Project.IsAttachmentAdded Then
				FileAttach = FileAttach.GetAttachment(ReferenceID:=ID)
				Session("mFileAttachProject") = FileAttach
			End If

			Project.MarkClean()
			Session("mProject") = Project

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteRecord(ID As Guid)

		Try

			MSGBoxCtrl.Show(MSGBox.Message_Title.Delete,
							MSGBox.Message_Text.Delete,
							"",
							MsgBoxStyle.YesNo,
							"Delete")

			Project = Project.GetProject(ID:=ID)
			Session("mProject") = Project

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		Dim ErrorMsgCount As Integer = 0
		MsgBoxResult = MSGBoxCtrl.Result

		Try

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Try

								Dim Project As Project
								Session("Sender") = ""
								Project = CType(Session("mProject"), Project)
								Project.Delete()
								Project.Save()

								DataFieldBind()
								SetVariables()
								CallFindNow()

								upnlTitle.Update()
								upnlGrid.Update()

							Catch ex As SqlException

								Dim stringInfo As String = ""
								If ex.Number = 8145 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Message, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 2627 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Message, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 547 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDeleting, MSGBox.Message_Text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
								End If

								ErrorMsgCount = ex.Errors.Count

							Finally

								If ErrorMsgCount = 0 Then

									ProjectDetails = $"{Project.ProjectNumber} Dated : {Project.ProjectDateFormatted} Customer : {Project.CustomerName}"
									MarkLog(Action.Delete, "Project", ProjectDetails, ErrorType.NoError, Project.ID, EventLogID)

								End If

							End Try

						End If

					Case MsgBoxResult.No
						Session("Sender") = ""
					Case MsgBoxResult.Ok
						Session("sender") = ""
				End Select

			ElseIf MsgBoxResult = -1 Then
				Session("sender") = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FindNow(Optional FromDate As String = "1/1/1900",
						Optional ToDate As String = "1/1/2200",
						Optional Text As String = "",
						Optional No As Integer = 0,
						Optional SearchText As String = "")

		Try

			ProjectList = Nothing
			dgProjectList.DataSource = Nothing

			'Get List From the Database as per Criteria
			ProjectList = ProjectList.GetProjectList(FromDate:=FromDate,
													 ToDate:=ToDate,
													 Text:=Text,
													 No:=No,
													 SearchText:=SearchText,
													 TransTypeID:=TransTypeID)

			'Set DataSource of the Grid
			Session("mProjectList") = ProjectList
			dgProjectList.DataSource = ProjectList
			lblResult.Text = $"As per criteria : {ProjectList.Count} Record(s) found."
			dgProjectList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
			dgProjectList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CallFindNow()

		Try

			FindNow(FromDate:=FromDate.Trim,
					ToDate:=ToDate.Trim,
					Text:=Trim(ProjectText),
					No:=CInt(Val(ProjectNo)),
					SearchText:=txtSearchBox.Text.Trim)

			dgProjectList.PageIndex = 0

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetPeriod(Index As Int32)

		Try

			FromDate = Today.AddDays(1).AddMonths(-1).ToString(AppSettings("DateFormat"))
			ToDate = Today.Date.ToString(AppSettings("DateFormat"))
			txtFromDate.Text = FromDate
			txtToDate.Text = ToDate

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ClearControls()
		txtNo.Text = ""
	End Sub

	Private Sub SetVariables()

		Try

			FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
			ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
			ProjectText = IIf(cmbProjectText.SelectedIndex <= 0, "", cmbProjectText.SelectedValue)
			ProjectNo = txtNo.Text.Trim
			SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text)
			Session("FromDate") = FromDate
			Session("ToDate") = ToDate
			Session("SearchIndex") = SearchIndex
			Session("DateIndex") = DateIndex
			Session("ProjectText") = ProjectText
			Session("ProjectNo") = ProjectNo
			Session("SearchText") = SearchText

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub ControlVisibility()

		Try

			txtSearchBox.Visible = True

			dgProjectList.Columns(3).Visible = (TransTypeID = 104) 'Customer Name
			dgProjectList.Columns(5).Visible = (TransTypeID = 104) 'Receiving Date
			dgProjectList.Columns(6).Visible = (TransTypeID = 104) 'Inspection Date
			dgProjectList.Columns(10).Visible = (TransTypeID = 104) 'Customer Contract-No

			If TransTypeID = 101 Then
				lblTitle.Text = "Work-Pack List"
			Else
				lblTitle.Text = "AMO Project List"
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function MarkRemoveFavorite()

		Try

			If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Project") Then
				ScriptManager.RegisterStartupScript(Me, [GetType], "MarkFav", "MarkFav();", True)
			Else
				ScriptManager.RegisterStartupScript(Me, [GetType], "RemoveFav", "RemoveFav();", True)
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
			ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
			SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
			DateIndex = IIf(IsNothing(DateIndex), 0, DateIndex)

			ProjectText = Session("ProjectText")
			ProjectDistinctTextList = ProjectDistinctTextList.GetDistinctTextList([Of]:="37",
																				  AddTopItem:="(ALL)",
																				  TransTypeID:=TransTypeID) '37 is for tabProject Text
			cmbProjectText.DataSource = ProjectDistinctTextList

			DataBind()

			dgProjectList.Columns(2).HeaderText = $"{Prefix} No"
			dgProjectList.Columns(14).HeaderText = $"{Prefix} Completion"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			ClearAll()
			AddAttributes()
			GetSession()

			EventLogID = CType(Session("EventLogID"), Guid)

			If Not IsPostBack Then

				cmbShowE.SelectedIndex = 4

				MarkRemoveFavorite()

				If Session("TransTypeID") Is Nothing Then

					TransTypeID = Request.QueryString("TransTypeId")
					Session("TransTypeID") = TransTypeID
				Else
					TransTypeID = Session("TransTypeID")

				End If

				Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & TransTypeID

				DataFieldBind()
				SetControl()
				ControlVisibility()

			End If

			Prefix = $"{IIf(TransTypeID = 101, "Work-Pack", "Project")}"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ProjectList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgProjectList.RowCommand

		Dim ID
		Dim str As String
		Dim Index As Integer
		Dim FileAttach As FileAttach

		Try

			Dim GridViewRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
			Index = GridViewRow.RowIndex
			ID = New Guid(dgProjectList.DataKeys(Index).Value.ToString)
			ProjectDetails = $"{ProjectList(Index).ProjectNumber} Dated : {ProjectList(Index).ProjectDateFormatted} Customer : {ProjectList(Index).CustomerName}"
			Select Case e.CommandName
				Case "EditRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.Edit},
																	MarkLogDetail:=ProjectDetails) Then

						Exit Sub

					End If

					MarkRemoveFavorite()
					EditRecord(ID:=ID)

					str = "openledgersame('wfProject_Ajax.aspx?BackPage=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, [GetType], "Open Script", str, True)

				Case "DeleteRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.Delete},
																	MarkLogDetail:=ProjectDetails) Then

						Exit Sub

					End If

					DeleteRecord(ID:=ID)

				Case "AttachRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.View},
																	MarkLogDetail:=ProjectDetails) Then

						Exit Sub

					End If

					FileAttach = FileAttach.GetAttachment(ReferenceID:=ID)
					Session("mFileAttach") = FileAttach

					AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

					ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ProjectList_PageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles dgProjectList.PageIndexChanging

		Try

			dgProjectList.PageIndex = e.NewPageIndex
			dgProjectList.DataSource = ProjectList
			Session("mProjectList") = ProjectList
			dgProjectList.DataBind()
			dgProjectList.PageSize = CInt(cmbShowE.SelectedItem.ToString)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ProjectList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgProjectList.Sorting

		Try

			ProjectList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("mProjectList") = ProjectList
			dgProjectList.DataSource = ProjectList
			dgProjectList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ProjectList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgProjectList.RowDataBound

		Try

			If e.Row.RowType <> DataControlRowType.DataRow Then
				Exit Sub
			End If

			If (e.Row.RowType = DataControlRowType.DataRow) Then

				Dim TaskCompletionPercentage As Integer = (DataBinder.Eval(e.Row.DataItem, "TaskCompletionPercentage"))
				Dim tmpDiv As HtmlGenericControl = CType(e.Row.FindControl("prgbar"), HtmlGenericControl)
				Dim lblPercentage As HtmlGenericControl = CType(e.Row.FindControl("lblPercentage"), HtmlGenericControl)

				tmpDiv.Attributes.Add("style", "width:" + TaskCompletionPercentage.ToString + "%")
				tmpDiv.Attributes.Add("aria-valuenow", TaskCompletionPercentage.ToString)
				lblPercentage.InnerText = TaskCompletionPercentage.ToString + "%"

				If TaskCompletionPercentage = 0 Then
					lblPercentage.Attributes.Add("style", "color:black;")
				Else
					lblPercentage.Attributes.Add("style", "color:white;")
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SearchRecords(sender As Object, e As EventArgs) Handles btnSearchRecords.Click

		Try

			MarkRemoveFavorite()

			SetVariables()
			CallFindNow()
			dgProjectList.DataBind()
			upnlGrid.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddNewRecord(sender As Object, e As EventArgs) Handles btnAddNewTop.Click

		Dim str As String
		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:=Prefix,
															TransTypeID:=TransTypeID,
															Action:={Action.New}) Then

				Exit Sub

			End If


			MarkRemoveFavorite()
			NewRecord()

			str = "openledgersame('wfProject_Ajax.aspx?BackPage=index.aspx');"
			ScriptManager.RegisterStartupScript(Me, [GetType], "Open Script", str, True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnCloseTop.Click

		Try

			RemoveSession()
			Session("MiddleFrame") = ""
			Response.Redirect("Dashboard.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub ShowNumberOfRecords(sender As Object, e As EventArgs)

		Try

			dgProjectList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
			dgProjectList.DataSource = ProjectList
			dgProjectList.DataBind()

			SetVariables()

			upnlGrid.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Search(sender As Object, e As EventArgs) Handles txtSearchBox.TextChanged

		Try

			SetVariables()
			CallFindNow()
			dgProjectList.DataBind()
			upnlGrid.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ProjectChanged(sender As Object, e As EventArgs) Handles cmbProjectText.SelectedIndexChanged

		Try

			txtNo.Text = "0"
			If cmbProjectText.Enabled = True Then
				cmbProjectText.Focus()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFav.Click
		MarkFavourite(HttpContext.Current.User.Identity.Name, "Project")
	End Sub

	Private Sub HdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFav.Click
		RemoveFavourite(HttpContext.Current.User.Identity.Name, "Project")
	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

#End Region

End Class