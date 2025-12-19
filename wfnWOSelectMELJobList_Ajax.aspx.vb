'******************************************
'CREATED By : Saylee
'Dated      : 1-Jul-2014
'Modified by Harsh on 14th May 2024 for FLYPAL 1632 -- W.O Job List page changes 
'******************************************

Imports System.Collections.Generic
Imports System.Text

Public Class SelectMELJobListPage
	Inherits Page

#Region "Variable Declaration"

	Public mnWO As nWO
	Public WODiscrepancyDueList As WODiscrepancyDueList
	Public WODiscrepancyDueDetails As WODiscrepancyDueDetails
	Public mnWOMELSnagCorrectiveActionListForDue As nWOMELSnagCorrectiveActionListForDue
	Public mnWOMELSnagCorrectiveActionListForDues As nWOMELSnagCorrectiveActionListForDues

	Dim mIsSelected As Boolean = False
	Private checkedIds As New List(Of String)()
	Dim ShowNewDiscrepancyFlow As Boolean
	Dim CommonObject

#End Region

#Region "Business Methods"

	Private Sub GetSession()

		mnWO = Session("mnWO")
		mnWOMELSnagCorrectiveActionListForDue = Session("mnWOMELSnagCorrectiveActionListForDue")
		mnWOMELSnagCorrectiveActionListForDues = Session("mnWOMELSnagCorrectiveActionListForDues")
		WODiscrepancyDueDetails = Session("DeferredDueDiscrepancyDetails")
		WODiscrepancyDueList = Session("DeferredDueDiscrepancyList")

	End Sub

	Private Sub SetSession()

		Session("mnWO") = mnWO
		Session("mnWOMELSnagCorrectiveActionListForDue") = mnWOMELSnagCorrectiveActionListForDue
		Session("mnWOMELSnagCorrectiveActionListForDues") = mnWOMELSnagCorrectiveActionListForDues
		Session("DeferredDueDiscrepancyDetails") = WODiscrepancyDueDetails
		Session("DeferredDueDiscrepancyList") = WODiscrepancyDueList

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		If control.Enabled = False Or control.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript([GetType], "focusScript", str)

	End Sub

	'Modified by Harsh on 14th May 2024 for FLYPAL 1632 -- W.O Job List page changes
	Private Sub AddMELJobs()

		Dim builder = New StringBuilder()
		CommonObject = Session("CommonObject")
		Try

			builder.Append("You have selected the following checks : <br/>")

			' get the selected checkboxes from the form data
			Dim checkString = IIf(ShowNewDiscrepancyFlow,
								  Request.Form("chkSelectDiscrepancies"),
								  Request.Form("chkSelect"))

			If checkString Is Nothing Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne,
								MSGBox.Message_Text.SelectAtleastOne,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			Else

				' we'll need a split to get the individual ids
				Dim values As String() = checkString.Split(","c)

				If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") And values.Length > 1 Then

					MSGBoxCtrl.Show("Selection Alert !",
									"Multiple Jobs can not be added in single WO.",
									"",
									MsgBoxStyle.OkOnly,
									"RestrictMultiJobs")
					Exit Sub

				End If

				For Each value As String In values

					builder.Append("<br/>")
					builder.Append(value)
					checkedIds.Add(value)

					If CommonObject.Contains(New Guid(value)) Then
						CommonObject(New Guid(value)).IsSelected = True
					End If

				Next

				checkString = Nothing

			End If


			For i As Integer = 0 To CommonObject.Count - 1

				If CommonObject(i).IsSelected = False Then

					If mnWO.WOJobs.Contains(CommonObject.Item(i).ID, "") Then
						mnWO.WOJobs.Remove(CommonObject.Item(i).ID, "")
					End If

				End If

			Next

			Session("mnWO") = mnWO
			Session("mnWOMELSnagCorrectiveActionListForDues") = mnWOMELSnagCorrectiveActionListForDues
			Session("DeferredDueDiscrepancyList") = WODiscrepancyDueList

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Modified by Harsh on 14th May 2024 for FLYPAL 1632 -- W.O Job List page changes
	Private Sub SetObject()

		Dim i As Integer = 0
		Try

			While i < CommonObject.Count

				If CommonObject.Item(i).IsDirty = True AndAlso CommonObject.Item(i).IsSelected = True Then

					mIsSelected = True

					If Not mnWO.WOJobs.Contains(PreviousTransID:=CommonObject.Item(i).ID, "") Then

						Dim Description As String = ""

						mnWO.WOJobs.Add(WOID:=mnWO.ID, WOJobTypeID:=Val(Session("WOJobTypeID")))
						mnWO.WOJobs.CurrentItem.PreviousTransID = CommonObject.Item(i).ID
						mnWO.WOJobs.CurrentItem.DateOfOccurrence = If(ShowNewDiscrepancyFlow, CommonObject.Item(i).DateOfOccurrence, CommonObject.Item(i).DateOfOccurence)
						mnWO.WOJobs.CurrentItem.MELCategoryID = CommonObject.Item(i).MELCategoryID
						mnWO.WOJobs.CurrentItem.ATAChapterID = CommonObject.Item(i).ATAChapterID
						mnWO.WOJobs.CurrentItem.IsUnderMEL = CommonObject.Item(i).IsMEL
						mnWO.WOJobs.CurrentItem.CompID = CommonObject.Item(i).PartID
						mnWO.WOJobs.CurrentItem.IsMajor = CommonObject.Item(i).IsMajor
						mnWO.WOJobs.CurrentItem.IsHours = CommonObject.Item(i).IsHours
						mnWO.WOJobs.CurrentItem.FrequencyInDays = CommonObject.Item(i).FrequencyInDays
						mnWO.WOJobs.CurrentItem.FrequencyInHours = CommonObject.Item(i).FrequencyInHours.ToString.Split(" ")(0)
						mnWO.WOJobs.CurrentItem.IsRepetitive = CommonObject.Item(i).IsRepetitive

						Description += $"{Environment.NewLine} {If(ShowNewDiscrepancyFlow, CommonObject.Item(i).JobDescriptionDetail, CommonObject.Item(i).JobDescriptionDetailWeb)}"
						Description += $"{Environment.NewLine} Date Of Occurrence : {mnWO.WOJobs.CurrentItem.DateOfOccurrence}"

						If CommonObject.Item(i).PartName <> "" Then Description += $"{Environment.NewLine} On Part : {CommonObject.Item(i).PartName}"

						If CommonObject.Item(i).MELCategoryName <> "" Then

							Description += $"{Environment.NewLine} {IIf(CBool(AppSettings("MELSnagNomenclature")), "ADD Category : ", "MEL Category :")} {CommonObject.Item(i).MELCategoryName} with"

							If CommonObject.Item(i).FrequencyInDays <> 0 Then
								Description += $"{CommonObject.Item(i).FrequencyInDays} Days"
							Else
								Description += $"{ CommonObject.Item(i).FrequencyInHours} Hours"
							End If

						End If

						mnWO.WOJobs.CurrentItem.WOJobDescription = Description
						mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = Trim(Description)
						mnWO.WOJobs.CurrentItem.DueAsOf = If(ShowNewDiscrepancyFlow, CommonObject.Item(i).DueAsOfWithoutHTML, CommonObject.Item(i).DateTimeOfDue.ToString)

						If CBool(AppSettings("ShowCAMOOnlyForNewClients")) Then
							mnWO.WOJobs.CurrentItem.TaskCardNo = CommonObject.Item(i).DefectNo
						End If

					End If

				Else
					mnWO.WOJobs.Remove(CommonObject.Item(i).ID)
				End If

				i += 1

			End While

			Session("mnWO") = mnWO

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SetTitleAndVisibility()

		Try

			If ShowNewDiscrepancyFlow Then

				Page.Header.Title = "Discrepancy Jobs"
				lblTitle.Text = "Discrepancy Job(s)"
				phDiscrepancyList.Visible = True
				lblResult.Text = "List of Due Discrepancy Jobs as per criteria : " & WODiscrepancyDueList.Count & " Record(s) found."

			Else

				Page.Header.Title = "MEL Jobs"
				lblTitle.Text = IIf(CBool(AppSettings("MELSnagNomenclature")),
									"ADD Job(s)",
									"MEL Job(s)")
				phMELJobGrid.Visible = True
				lblResult.Text = $"List of Due {IIf(CBool(AppSettings("MELSnagNomenclature")),
													"ADD / Defect",
													"MEL / Snag")} Jobs as per criteria :
																  {mnWOMELSnagCorrectiveActionListForDues.Count} Record(s) found."

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Data Binding "

	'Modified by Harsh on 14th May 2024 for FLYPAL 1632 -- W.O Job List page changes
	Private Sub DataFieldBind()

		Try

			mnWOMELSnagCorrectiveActionListForDues = nWOMELSnagCorrectiveActionListForDues.GetMELSnagCorrectiveActionListForDues(txtAsOnDate.Text.ToString, mnWO.MachineID, 0)
			WODiscrepancyDueList = WODiscrepancyDueList.GetDueDiscrepancyList(AsOnDate:=txtAsOnDate.Text.ToString(),
																			  MachineID:=mnWO.MachineID.ToString())

			CommonObject = If(ShowNewDiscrepancyFlow, WODiscrepancyDueList, mnWOMELSnagCorrectiveActionListForDues)
			Session("CommonObject") = CommonObject

			If CommonObject IsNot Nothing Then

				For Each Record As Object In CommonObject

					Record.IsSelected = mnWO.WOJobs.Contains(PreviousTransID:=Record.ID, "")

					If mnWO.WOJobs.Contains(PreviousTransID:=Record.ID, "") Then
						checkedIds.Add(Record.ID.ToString)
					End If

				Next

			End If

			dgDiscrepancyJobs.DataSource = WODiscrepancyDueList
			dgMELJob.DataSource = mnWOMELSnagCorrectiveActionListForDues

			dgMELJob.Columns(19).HeaderText = IIf(CBool(AppSettings("MELSnagNomenclature")),
														"Is ADD",
														"Is MEL") 'Added By Vikrant On 07-Sep-2020 For ALL07092020

			Session("DeferredDueDiscrepancyList") = WODiscrepancyDueList
			Session("mnWOMELSnagCorrectiveActionListForDues") = mnWOMELSnagCorrectiveActionListForDues

			DataBind()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	'Modified by Harsh on 14th May 2024 for FLYPAL 1632 -- W.O Job List page changes
	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()

			If txtAsOnDate.Text.ToString = "" Then
				txtAsOnDate.Text = mnWO.WODateFormatted
			End If

			txtAsOnDate.Enabled = False

			ShowNewDiscrepancyFlow = CBool(AppSettings("ShowNewDiscrepancyFlow"))

			If Not IsPostBack Then
				DataFieldBind()
			End If

			SetTitleAndVisibility()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub FindNow(sender As Object, e As EventArgs)

		Try

			If IsValid Then

				mnWOMELSnagCorrectiveActionListForDues = nWOMELSnagCorrectiveActionListForDues.GetMELSnagCorrectiveActionListForDues(AsOnDate:=txtAsOnDate.Text,
																																	 MachineID:=mnWO.MachineID,
																																	 TimeFormat:=0)

				WODiscrepancyDueList = WODiscrepancyDueList.GetDueDiscrepancyList(AsOnDate:=txtAsOnDate.Text.ToString(),
																				  MachineID:=mnWO.MachineID.ToString())

				If mnWOMELSnagCorrectiveActionListForDues IsNot Nothing Then

					For Each Child As nWOMELSnagCorrectiveActionListForDue In mnWOMELSnagCorrectiveActionListForDues
						Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")
					Next

				End If

				If WODiscrepancyDueList IsNot Nothing Then

					For Each Discrepancy As WODiscrepancyDueDetails In WODiscrepancyDueList
						Discrepancy.IsSelected = mnWO.WOJobs.Contains(Discrepancy.ID, "")
					Next

				End If

				Session("mnWOMELSnagCorrectiveActionListForDues") = mnWOMELSnagCorrectiveActionListForDues
				Session("DeferredDueDiscrepancyList") = WODiscrepancyDueList

				dgMELJob.Columns(19).HeaderText = IIf(CBool(AppSettings("MELSnagNomenclature")),
															"Is ADD",
															"Is MEL") 'Added By Vikrant On 07-Sep-2020 For ALL07092020

				DataBind()

				upnlDiscrepancy.Update()
				UpnlGrid.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Modified by Harsh on 14th May 2024 for FLYPAL 1632 -- W.O Job List page changes
	Private Sub DoneSelecting(sender As Object, e As EventArgs) Handles btnDoneTop.Click

		Try

			AddMELJobs()
			SetObject()

			Dim selectedCheckbox = IIf(ShowNewDiscrepancyFlow,
									   Request.Form("chkSelectDiscrepancies"),
									   Request.Form("chkSelect"))

			If selectedCheckbox Is Nothing Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne,
								MSGBox.Message_Text.SelectAtleastOne,
								$"Please select at least one {IIf(ShowNewDiscrepancyFlow,
																			 "Discrepancy",
																			 {IIf(CBool(AppSettings("MELSnagNomenclature")),
																						   "Defect / ADD",
																						   "Snag / MEL")})} Job",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			Else

				Dim values As String() = selectedCheckbox.Split(","c)

				If (AppSettings("ClientCode") = "IND" Or
					AppSettings("ClientCode") = "STR") And values.Length > 1 Then

					Exit Sub

				End If

				Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Back(sender As Object, e As EventArgs) Handles btnBackTop.Click

		If Session("wfProject_Ajax") = "wfProject_Ajax" Then
			Session("OpenFromProject") = Nothing
			Session("MiddleFrame") = $"wfProjectList_Ajax.aspx?TransTypeID={Session("mTransTypeID")}"
		End If

		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

	End Sub

#End Region

#Region " Checked Selection "

	Public Function CheckBoxSelection(ID As String) As String

		Try

			If (checkedIds.Contains(ID)) Then
				Return "checked"
			Else
				Return String.Empty
			End If

		Catch ex As Exception

			Return Nothing
			ex.GetBaseException()

		End Try

	End Function

#End Region

End Class