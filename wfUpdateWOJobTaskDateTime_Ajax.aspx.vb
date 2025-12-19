'Added By Vikrant On 17-Dec-2019 For Gantt Chart
Imports System.Text
Public Class wfUpdateWOJobTaskDateTime_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Protected mnWO As nWO
	Private grdTaskCards As nWOJobs
	Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
	Private Sub GetSession()
		mnWO = Session("mnWO")
		grdTaskCards = Session("grdTaskCards")
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		Dim msgCount As Integer = 0
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes


				Case MsgBoxResult.No

				Case MsgBoxResult.Ok '

				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added

			End Select
		ElseIf Result1 = -1 Then

		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added

		End If
	End Sub
	Private Sub setData()
		Dim JobRow, TaskRow As GridViewRow
		For i As Integer = 0 To dgWOJobsWithTaskCard.Rows.Count - 1
			JobRow = dgWOJobsWithTaskCard.Rows(i)

			DirectCast(JobRow.FindControl("txtJobPlanStartDate"), TextBox).Text = mnWO.WOJobs(i).WOJobPlanStartDateFormatted.ToString
			DirectCast(JobRow.FindControl("txtJobPlanEndDate"), TextBox).Text = mnWO.WOJobs(i).WOJobPlanEndDateFormatted.ToString
			DirectCast(JobRow.FindControl("txtJobNoOfPersons"), TextBox).Text = mnWO.WOJobs(i).NoOfPersons.ToString
			Dim grdTasks As GridView = DirectCast(JobRow.FindControl("grdTaskCards"), GridView)
			For j As Integer = 0 To grdTasks.Rows.Count - 1
				TaskRow = grdTasks.Rows(j)
				DirectCast(TaskRow.FindControl("txtTaskPlanStartDate"), TextBox).Text = mnWO.WOJobs(i).WOJobTasks(j).PlanStartDateFormatted.ToString
				DirectCast(TaskRow.FindControl("txtTaskPlanEndDate"), TextBox).Text = mnWO.WOJobs(i).WOJobTasks(j).PlanEndDateFormatted.ToString
				DirectCast(TaskRow.FindControl("txtTaskNoOfPersons"), TextBox).Text = mnWO.WOJobs(i).WOJobTasks(j).NoOfPersons.ToString
			Next
		Next
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		grdTaskCards = nWOJobs.GetWOJobs(mnWO.ID, 0, False)
		Session("grdTaskCards") = grdTaskCards
		dgWOJobsWithTaskCard.DataSource = grdTaskCards
		DataBind()
	End Sub
	Protected Sub AddAttributesForGridControls()
		Dim txtJobNoOfPersons, txtTaskNoOfPersons As TextBox
		For i As Integer = 0 To dgWOJobsWithTaskCard.Rows.Count - 1
			Try
				txtJobNoOfPersons = CType(Me.dgWOJobsWithTaskCard.Rows(i).FindControl("txtJobNoOfPersons"), TextBox)
				txtJobNoOfPersons.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtJobNoOfPersons.ClientID + "').value,event)")

				Dim grdTasks As GridView = DirectCast(dgWOJobsWithTaskCard.Rows(i).FindControl("grdTaskCards"), GridView)
				For k As Integer = 0 To grdTasks.Rows.Count - 1
					txtTaskNoOfPersons = CType(grdTasks.Rows(k).FindControl("txtTaskNoOfPersons"), TextBox)
					txtTaskNoOfPersons.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtTaskNoOfPersons.ClientID + "').value,event)")
				Next
			Catch ex As Exception
			End Try
		Next
	End Sub
	Private Function CustomValidateJobDateTimes() As Boolean
		Dim strError As New StringBuilder

		Dim txtJobPlanStartDate, txtJobPlanEndDate, txtTaskPlanStartDate, txtTaskPlanEndDate As TextBox
		Dim cvJobStartDate, cvJobEndDate, cvTaskStartDate, cvTaskEndDate As CustomValidator
		Dim upnlValidationSummary1, upnlValidationSummary2, upnlValidationSummary3, upnlValidationSummary4 As UpdatePanel
		Dim validationcontrol1, validationcontrol2 As ValidationSummary

		For j As Integer = 0 To dgWOJobsWithTaskCard.Rows.Count - 1
			Dim SkipTaskDatesComparisononBasedOnJobDates As Boolean = False

			cvJobStartDate = CType(Me.dgWOJobsWithTaskCard.Rows(j).FindControl("cvJobStartDate"), CustomValidator)
			cvJobEndDate = CType(Me.dgWOJobsWithTaskCard.Rows(j).FindControl("cvJobEndDate"), CustomValidator)
			upnlValidationSummary3 = CType(Me.dgWOJobsWithTaskCard.Rows(j).FindControl("upnlValidationSummary3"), UpdatePanel)
			upnlValidationSummary4 = CType(Me.dgWOJobsWithTaskCard.Rows(j).FindControl("upnlValidationSummary4"), UpdatePanel)
			txtJobPlanStartDate = CType(Me.dgWOJobsWithTaskCard.Rows(j).FindControl("txtJobPlanStartDate"), TextBox)
			txtJobPlanEndDate = CType(Me.dgWOJobsWithTaskCard.Rows(j).FindControl("txtJobPlanEndDate"), TextBox)

			If txtJobPlanStartDate.Text = "" Then
				cvJobStartDate.IsValid = False
				cvJobStartDate.ErrorMessage = "Please Enter Job Plan Start Date Time"
				strError.Append("Please Enter Job Plan Start Date Time")
				upnlValidationSummary3.Update()
			Else
				cvJobStartDate.IsValid = True
				cvJobStartDate.ErrorMessage = ""
				upnlValidationSummary3.Update()
			End If
			If txtJobPlanEndDate.Text = "" Then
				cvJobEndDate.IsValid = False
				cvJobEndDate.ErrorMessage = "Please Enter Job Plan End Date Time"
				strError.Append("Please Enter Job Plan End Date Time")
				upnlValidationSummary4.Update()
			Else
				cvJobEndDate.IsValid = True
				cvJobEndDate.ErrorMessage = ""
				upnlValidationSummary4.Update()
			End If

			If txtJobPlanStartDate.Text <> "" AndAlso txtJobPlanEndDate.Text <> "" Then
				If DateTime.Compare(txtJobPlanStartDate.Text, txtJobPlanEndDate.Text) > 0 Then
					cvJobStartDate.IsValid = False
					cvJobStartDate.ErrorMessage = "Job Plan Start Date Time can not be greater than Job Plan End Date Time"
					strError.Append("Job Plan Start Date Time can not be greater than Job Plan End Date Time")
					SkipTaskDatesComparisononBasedOnJobDates = True
					upnlValidationSummary3.Update()
				Else
					cvJobStartDate.IsValid = True
					cvJobStartDate.ErrorMessage = ""
					upnlValidationSummary3.Update()
				End If
			End If


			Dim grdTasks As GridView = DirectCast(dgWOJobsWithTaskCard.Rows(j).FindControl("grdTaskCards"), GridView)
			For k As Integer = 0 To grdTasks.Rows.Count - 1
				cvTaskStartDate = CType(grdTasks.Rows(k).FindControl("cvTaskStartDate"), CustomValidator)
				validationcontrol1 = CType(grdTasks.Rows(k).FindControl("Validationsummary1"), ValidationSummary)
				upnlValidationSummary1 = CType(grdTasks.Rows(k).FindControl("upnlValidationSummary1"), UpdatePanel)
				txtTaskPlanStartDate = CType(grdTasks.Rows(k).FindControl("txtTaskPlanStartDate"), TextBox)

				cvTaskEndDate = CType(grdTasks.Rows(k).FindControl("cvTaskEndDate"), CustomValidator)
				validationcontrol2 = CType(grdTasks.Rows(k).FindControl("Validationsummary2"), ValidationSummary)
				upnlValidationSummary2 = CType(grdTasks.Rows(k).FindControl("upnlValidationSummary2"), UpdatePanel)
				txtTaskPlanEndDate = CType(grdTasks.Rows(k).FindControl("txtTaskPlanEndDate"), TextBox)

				If txtTaskPlanStartDate.Text = "" Then
					cvTaskStartDate.IsValid = False
					cvTaskStartDate.ErrorMessage = "Please Enter Task Plan Start Date Time"
					strError.Append("Please Enter Task Plan Start Date Time")
					upnlValidationSummary1.Update()
				Else
					cvTaskStartDate.IsValid = True
					cvTaskStartDate.ErrorMessage = ""
					upnlValidationSummary1.Update()
				End If
				If txtTaskPlanEndDate.Text = "" Then
					cvTaskEndDate.IsValid = False
					cvTaskEndDate.ErrorMessage = "Please Enter Task Plan End Date Time"
					strError.Append("Please Enter Task Plan End Date Time")
					upnlValidationSummary2.Update()
				Else
					cvTaskEndDate.IsValid = True
					cvTaskEndDate.ErrorMessage = ""
					upnlValidationSummary2.Update()
				End If

				If txtTaskPlanStartDate.Text <> "" AndAlso txtTaskPlanEndDate.Text <> "" Then
					If DateTime.Compare(txtTaskPlanStartDate.Text, txtTaskPlanEndDate.Text) > 0 Then
						cvTaskStartDate.IsValid = False
						cvTaskStartDate.ErrorMessage = "Task Plan Start Date Time can not be greater than Task Plan End Date Time"
						strError.Append("Task Plan Start Date Time can not be greater than Task Plan End Date Time")
						upnlValidationSummary1.Update()
					Else
						If txtJobPlanStartDate.Text <> "" AndAlso txtJobPlanEndDate.Text <> "" AndAlso Not SkipTaskDatesComparisononBasedOnJobDates Then
							If DateTime.Compare(txtJobPlanStartDate.Text, txtTaskPlanStartDate.Text) > 0 Or DateTime.Compare(txtJobPlanEndDate.Text, txtTaskPlanStartDate.Text) < 0 Then
								cvTaskStartDate.IsValid = False
								cvTaskStartDate.ErrorMessage = "Task Plan Start Date Time should fall between Job Plan Date Time"
								strError.Append("Task Plan Start Date Time should be between respective Job Plan Date Time")
								upnlValidationSummary1.Update()
							Else
								cvTaskStartDate.IsValid = True
								cvTaskStartDate.ErrorMessage = ""
								upnlValidationSummary1.Update()
							End If

							If DateTime.Compare(txtJobPlanStartDate.Text, txtTaskPlanEndDate.Text) > 0 Or DateTime.Compare(txtJobPlanEndDate.Text, txtTaskPlanEndDate.Text) < 0 Then
								cvTaskEndDate.IsValid = False
								cvTaskEndDate.ErrorMessage = "Task Plan End Date Time should fall between Job Plan Date Time"
								strError.Append("Task Plan End Date Time should be between respective Job Plan Date Time")
								upnlValidationSummary2.Update()
							Else
								cvTaskEndDate.IsValid = True
								cvTaskEndDate.ErrorMessage = ""
								upnlValidationSummary2.Update()
							End If
						End If
					End If
				End If
			Next
		Next
		'upnlJobPlanStartDateValidate.Update()
		'upnlJobPlanEndDateValidate.Update()
		'upnlTaskPlanStartDateValidate.Update()
		'upnlTaskPlanEndDateValidate.Update()

		If strError.ToString <> "" Then
			Return False
		Else

		End If

		Return True
	End Function
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)          'Added by Vikrant on 25-July-2011
		If Not Page.IsPostBack Then
			DataFieldBind()
			setData()
		End If
	End Sub
	Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Try
			If CustomValidateJobDateTimes() = False Then
				upnlValidation.Update()
				Exit Sub
			End If
			Dim mEventLogDetail As New StringBuilder
			If IsValid Then
				Dim JobRow, TaskRow As GridViewRow
				For i As Integer = 0 To dgWOJobsWithTaskCard.Rows.Count - 1
					JobRow = dgWOJobsWithTaskCard.Rows(i)
					mnWO.WOJobs(i).WOJobPlanStartDate = DirectCast(JobRow.FindControl("txtJobPlanStartDate"), TextBox).Text
					mnWO.WOJobs(i).WOJobPlanEndDate = DirectCast(JobRow.FindControl("txtJobPlanEndDate"), TextBox).Text
					mnWO.WOJobs(i).NoOfPersons = CInt(Val(DirectCast(JobRow.FindControl("txtJobNoOfPersons"), TextBox).Text))
					mnWO.WOJobs(i).WOJobEstimatedTime = New Period(1, DateDiff(DateInterval.Minute, mnWO.WOJobs(i).WOJobPlanStartDate, mnWO.WOJobs(i).WOJobPlanEndDate), 1).ValueFormatted

					mEventLogDetail.Append("JOB " + (i + 1).ToString + ": " + mnWO.WOJobs(i).WOJobDescription + " ")
					mEventLogDetail.Append("Start Date: " + mnWO.WOJobs(i).WOJobPlanStartDateFormatted.ToString + " ")
					mEventLogDetail.Append("End Date: " + mnWO.WOJobs(i).WOJobPlanEndDateFormatted.ToString + " ")
					mEventLogDetail.Append("No of Persons: " + mnWO.WOJobs(i).NoOfPersons.ToString + " ")
					mEventLogDetail.Append(Chr(13))
					Dim grdTasks As GridView = DirectCast(JobRow.FindControl("grdTaskCards"), GridView)
					For j As Integer = 0 To grdTasks.Rows.Count - 1
						TaskRow = grdTasks.Rows(j)
						mnWO.WOJobs(i).WOJobTasks(j).PlanStartDate = DirectCast(TaskRow.FindControl("txtTaskPlanStartDate"), TextBox).Text
						mnWO.WOJobs(i).WOJobTasks(j).PlanEndDate = DirectCast(TaskRow.FindControl("txtTaskPlanEndDate"), TextBox).Text
						mnWO.WOJobs(i).WOJobTasks(j).NoOfPersons = CInt(Val(DirectCast(TaskRow.FindControl("txtTaskNoOfPersons"), TextBox).Text))
						If AppSettings("nWOShowHrsInDecimal") = "True" Then
							mnWO.WOJobs(i).WOJobTasks(j).EstimatedHours = New Period(1, DateDiff(DateInterval.Minute, mnWO.WOJobs(i).WOJobTasks(j).PlanStartDate, mnWO.WOJobs(i).WOJobTasks(j).PlanEndDate), 1).DbValueDec
						Else
							mnWO.WOJobs(i).WOJobTasks(j).EstimatedHours = New Period(1, DateDiff(DateInterval.Minute, mnWO.WOJobs(i).WOJobTasks(j).PlanStartDate, mnWO.WOJobs(i).WOJobTasks(j).PlanEndDate), 1).ValueFormatted
						End If


						mEventLogDetail.Append("TASK : " + mnWO.WOJobs(i).WOJobTasks(j).TaskCardNo + " ")
						mEventLogDetail.Append("Start Date: " + mnWO.WOJobs(i).WOJobTasks(j).PlanStartDateFormatted.ToString + " ")
						mEventLogDetail.Append("End Date: " + mnWO.WOJobs(i).WOJobTasks(j).PlanEndDateFormatted.ToString + " ")
						mEventLogDetail.Append("No of Persons: " + mnWO.WOJobs(i).WOJobTasks(j).NoOfPersons.ToString + " ")
					Next
				Next
				mnWO.Save()
				MSGBoxCtrl.show("Success!", "Record updated successfully", "", MsgBoxStyle.OkOnly, "Success")
				MarkLog(Util.Action.Save, "WOJobGanttChart", mEventLogDetail.ToString.TrimEnd(" "), Util.ErrorType.NoError, Guid.Empty, EventLogID)
				Exit Sub
			End If
		Catch ex As Exception
			MSGBoxCtrl.show("Alert!", ex.GetBaseException.ToString, ex.Message, MsgBoxStyle.OkOnly, "Success")
			Exit Sub
		End Try
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
	End Sub
	Private Sub dgWOJobsWithTaskCard_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgWOJobsWithTaskCard.RowDataBound
		If e.Row.RowType <> DataControlRowType.DataRow Then
			Return
		End If

		If (e.Row.RowType = DataControlRowType.DataRow) Then
			Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
			Dim grdTasks As GridView = DirectCast(e.Row.FindControl("grdTaskCards"), GridView)
			Dim lblTasks As Label = DirectCast(e.Row.FindControl("lblTaskCards"), Label)

			If grdTaskCards(e.Row.RowIndex).WOJobTasks.Count > 0 Then
				e.Row.Cells(0).BackColor = Color.Yellow
				lblTasks.Text = "Task Card(s) : " & grdTaskCards(e.Row.RowIndex).WOJobTasks.Count & " Record(s)."
			Else
				lblTasks.Text = "Task Card(s) : 0 Record(s)."
			End If
			grdTasks.DataSource = grdTaskCards(e.Row.RowIndex).WOJobTasks
			grdTasks.DataBind()
		End If
	End Sub

#End Region

End Class