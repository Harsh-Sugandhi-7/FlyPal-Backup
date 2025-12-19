'CREATED By : Saylee
'Dated      : 3-July-2014


Imports System.Collections.Generic
Imports System.Text

Public Class wfnWOSelectDeferredJobList_AJAX
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mnWO As nWO
	Public mnWODefferedJobs As nWODefferedJobs
	Public mnWODefferedJob As nWODefferedJob

	Dim mIsSelected As Boolean = False
	Private checkedIds As New List(Of String)()
#End Region

#Region " Business Methods"
	Private Sub GetSession()
		mnWO = Session("mnWO")
		mnWODefferedJobs = Session("mnWODefferedJobs")
		mnWODefferedJob = Session("mnWODefferedJob")
	End Sub
	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("mnWODefferedJobs") = mnWODefferedJobs
		Session("mnWODefferedJob") = mnWODefferedJob
	End Sub
	Private Overloads Sub SetFocus(Control As WebControl)
		If Control.Enabled = False Or Control.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + Control.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub SetTitle()
		lblResult.Text = "List of Deferred Jobs as per criteria :" & mnWODefferedJobs.Count & " Record(s) found."
	End Sub
	Private Sub AddDeferredJobs()
		Dim builder = New StringBuilder()
		builder.Append("You have selected the following checks :<br/>")
		' get the selected checkboxes from the form data
		Dim checkString = Request.Form("chkSelect")
		If checkString Is Nothing Then
			MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			' we'll need a split to get the individual ids
			Dim values As String() = checkString.Split(","c)
			If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") And values.Length > 1 Then
				MSGBoxCtrl.Show("Selection Alert!", "Multiple Jobs can not be added in single WO.", "", MsgBoxStyle.OkOnly, "RestrictMultJobs")
				Exit Sub
			End If
			For Each value As String In values
				builder.Append("<br/>")
				builder.Append(value)
				checkedIds.Add(value)
				' mMaintenanceTask.MaintenanceTaskDetails.Remove(New Guid(value), "")
				If mnWODefferedJobs.Contains(New Guid(value)) Then
					mnWODefferedJobs(New Guid(value)).IsSelected = True
				End If
			Next
			'values = ""
			checkString = Nothing
		End If

		For i As Integer = 0 To mnWODefferedJobs.Count - 1
			If mnWODefferedJobs(i).IsSelected = False Then
				If mnWO.WOJobs.Contains(mnWODefferedJobs.Item(i).ID, "") Then
					mnWO.WOJobs.Remove(mnWODefferedJobs.Item(i).ID, "")
				End If
			End If
		Next
		Session("mnWO") = mnWO
		Session("mnWODefferedJobs") = mnWODefferedJobs
	End Sub
	Private Sub setObject()
		Dim i As Integer = 0
		While i < mnWODefferedJobs.Count
			If mnWODefferedJobs.Item(i).IsDirty = True Then
				If mnWODefferedJobs.Item(i).IsSelected = True Then
					mIsSelected = True
					''If mnWO.WOJobs.Contains(mnWODefferedJobs.Item(i).ID, "") = False Then
					''If mnWO.WOJobs.Contains(mnWODefferedJobs.Item(i).PreviousTransID, "") = False Then
					If (mnWO.WOJobs.Contains(mnWODefferedJobs.Item(i).PreviousTransID, "") = False And mnWODefferedJobs.Item(i).WOJobTypeID = 2) Or (mnWO.WOJobs.Contains(mnWODefferedJobs.Item(i).ID, "") = False) Then

						Dim Description As String = ""
						Description = Description & "<BR>" & mnWODefferedJobs.Item(i).JobDescriptionDetailWeb
						'WOJOB:
						' mnWO.WOJobs.Add(nWOJob.NewWOJob(mnWO.ID))

						'Commented by Saylee on 14-Jul-2015
						'mnWO.WOJobs.Add(mnWO.ID, Val(Session("WOJobTypeID")))
						'Added By Saylee on 14-Jul-2015 as to set its JobType same as previous 
						mnWO.WOJobs.Add(mnWO.ID, Val(mnWODefferedJobs.Item(i).WOJobTypeID))
						'****************************


						''mnWO.WOJobs.CurrentItem.PreviousTransID = mnWODefferedJobs.Item(i).ID Çommented by Saylee on 5-Sep-2019 , as wrong ID was being set
						If mnWODefferedJobs.Item(i).WOJobTypeID = 2 Then
							mnWO.WOJobs.CurrentItem.PreviousTransID = mnWODefferedJobs.Item(i).PreviousTransID
						Else
							mnWO.WOJobs.CurrentItem.PreviousTransID = mnWODefferedJobs.Item(i).ID
						End If

						mnWO.WOJobs.CurrentItem.OnTypeID = mnWODefferedJobs.Item(i).OnTypeID
						mnWO.WOJobs.CurrentItem.MonitorTypeID = mnWODefferedJobs.Item(i).MonitorTypeID

						mnWO.WOJobs.CurrentItem.WOJobDescription = Description

						mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = mnWODefferedJobs.Item(i).WOJobEstimatedTime
						mnWO.WOJobs.CurrentItem.WOJobActualTime = mnWODefferedJobs.Item(i).WOJobActualTime
						''mnWO.WOJobs.CurrentItem.WOJobStatusID = mnWODefferedJobs.Item(i).WOJobStatusID
						mnWO.WOJobs.CurrentItem.IsForBilling = mnWODefferedJobs.Item(i).IsForBilling

						mnWO.WOJobs.CurrentItem.WOJobAction = mnWODefferedJobs.Item(i).WOJobAction
						mnWO.WOJobs.CurrentItem.WOJobRemark = mnWODefferedJobs.Item(i).WOJobRemark
						mnWO.WOJobs.CurrentItem.IsUnderMEL = mnWODefferedJobs.Item(i).IsUnderMEL
						mnWO.WOJobs.CurrentItem.DateOfOccurrence = mnWODefferedJobs.Item(i).DateOfOccurence
						mnWO.WOJobs.CurrentItem.ATAChapterID = mnWODefferedJobs.Item(i).ATAChapterID
						mnWO.WOJobs.CurrentItem.CompID = mnWODefferedJobs.Item(i).CompID
						mnWO.WOJobs.CurrentItem.MELCategoryID = mnWODefferedJobs.Item(i).MELCategoryID
						mnWO.WOJobs.CurrentItem.IsMajor = mnWODefferedJobs.Item(i).IsMajor
						mnWO.WOJobs.CurrentItem.IsRepetitive = mnWODefferedJobs.Item(i).IsRepetitive
						mnWO.WOJobs.CurrentItem.IsHours = mnWODefferedJobs.Item(i).IsHours
						mnWO.WOJobs.CurrentItem.FrequencyInDays = mnWODefferedJobs.Item(i).FrequencyInDays
						mnWO.WOJobs.CurrentItem.FrequencyInHours = mnWODefferedJobs.Item(i).FrequencyInHours
						mnWO.WOJobs.CurrentItem.TempWOID = mnWODefferedJobs.Item(i).WOID
						mnWO.WOJobs.CurrentItem.TempWOJobID = mnWODefferedJobs.Item(i).ID
						' mnWO.WOJobs.CurrentItem.WOJobTypeID = Val(Session("WOJobTypeID")) 'mnWODefferedJobs.Item(i).WOJobTypeID
						mnWO.WOJobs.CurrentItem.DueAsOf = mnWODefferedJobs.Item(i).DueAsOf
						mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = Trim(Description.Replace("<BR>", vbCrLf)) 'Added By Vikrant On 19-Dec-2012 For ALL19122012

					End If
				Else
					'' mnWO.WOJobs.Remove(mnWODefferedJobs.Item(i).ID)
				End If
			End If
			i = i + 1
		End While
		Session("mnWO") = mnWO
	End Sub
#End Region

#Region "Data Binding"
	Private Sub DataFieldBind()
		' mnWODefferedJobs = nWODefferedJobs.GetWODefferedJobs(mnWO.ID.ToString, txtAsOnDate.Value.ToString)  'GetMELSnagCorrectiveActionListForDues(txtAsOnDate.Value.ToString, mnWO.MachineID, 0)
		mnWODefferedJobs = nWODefferedJobs.GetWODefferedJobs(mnWO.MachineID, , txtAsOnDate.Text.ToString)
		dgDeferredJob.DataSource = mnWODefferedJobs
		If mnWODefferedJobs IsNot Nothing Then
			For Each Child As nWODefferedJob In mnWODefferedJobs
				Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")
				If mnWO.WOJobs.Contains(Child.ID, "") Then
					checkedIds.Add(Child.ID.ToString)
				End If
			Next
		End If
		dgDeferredJob.DataSource = mnWODefferedJobs
		Session("mnWODefferedJobs") = mnWODefferedJobs
		DataBind()
	End Sub
#End Region

#Region "Events"

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		Page.Header.DataBind()
		GetSession()
		If txtAsOnDate.Text.ToString = "" Then
			txtAsOnDate.Text = mnWO.WODateFormatted
		End If
		txtAsOnDate.Enabled = False
		If Not IsPostBack Then
			DataFieldBind()
			SetTitle()
		End If
	End Sub

	Private Sub DoneSelecting(sender As Object, e As EventArgs) Handles btnDone.Click

		AddDeferredJobs()
		setObject()
		Dim checkString = Request.Form("chkSelect")

		If checkString Is Nothing Then

			MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne,
							MSGBox.Message_text.SelectAtleastOne,
							"Please select atleast one Deferred Job.",
							MsgBoxStyle.OkOnly, "")
			Exit Sub

		Else

			Dim values As String() = checkString.Split(","c)

			If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") And values.Length > 1 Then
				Exit Sub
			End If

			Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

		End If

	End Sub

	Private Sub btnFindNow_Click(sender As Object, e As EventArgs) Handles btnFindNow.Click
		If IsValid Then
			mnWODefferedJobs = nWODefferedJobs.GetWODefferedJobs(mnWO.ID, , txtAsOnDate.Text.ToString)
			dgDeferredJob.PageIndex = 0   'Added Code on May,25,2007
			If mnWODefferedJobs IsNot Nothing Then
				For Each Child As nWODefferedJob In mnWODefferedJobs
					Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")
				Next
			End If
			Session("mnWODefferedJobs") = mnWODefferedJobs
			DataBind()
			SetTitle()
		End If
	End Sub
	Private Sub dgDeferredJob_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDeferredJob.PageIndexChanged
		dgDeferredJob.PageIndex = e.NewPageIndex
		dgDeferredJob.DataSource = mnWODefferedJobs
		Session("mnWODefferedJobs") = mnWODefferedJobs
		dgDeferredJob.DataBind()
	End Sub
	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click, btnBack.Click
		If Session("wfProject_Ajax") = "wfProject_Ajax" Then
			Session("OpenFromProject") = Nothing
			Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("mTransTypeID").ToString
		End If
		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
#End Region

#Region "Checked Selection"

	Public Function NumeroChequeInclus(numero As String) As String

		If (checkedIds.Contains(numero)) Then
			Return "checked"
		Else
			Return String.Empty
		End If
	End Function
#End Region
End Class