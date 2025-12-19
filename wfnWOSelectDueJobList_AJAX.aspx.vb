'CREATED By : Saylee
'Dated      : 30-May-2014

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class wfnWOSelectDueJobList_AJAX
	Inherits Page

#Region " Variable Declaration "

	Public mnWO As nWO
	Public mDueLimits As DueLimits
	Public mSelectDueJob As SelectDueJob
	Public mSelectDueJobs As SelectDueJobs
	Public mMaintenanceKit As MaintenanceKit
	Public mMaintenanceTask As MaintenanceTask
	Public mSortedDueJobList As New List(Of SelectDueJob)
	Public mrptDueReport As rptDueReportForOnlyDueReport
	Public mMultiComplianceLinkList As New MultiComplianceList
	Public FetchLastWOJobDescription As FetchLastnWOJobDescription     ' Added by Shital on 15-May-2019
	Public mSpareListByMaintenanceActivity As SpareListByMaintenanceActivity

	'Added by Vikrant on 19-May-2021 
	Dim mIsNewDueReportObjectBindingRequired As String
	Dim mSortedDueJobs As Object = Nothing
	Dim mIsSelected As Boolean = False
	Private checkedIds As New List(Of String)()
	Private Flag As Int16
	Private checkedMaintLinkIds As New List(Of String)()

#End Region

#Region " Methods "

	Private Sub GetSession()

		mnWO = Session("mnWO")
		mSelectDueJob = Session("mSelectDueJob")
		mSelectDueJobs = Session("mSelectDueJobs")
		mDueLimits = Session("mDueLimits")
		'Added by Vikrant on 19-May-2021
		mIsNewDueReportObjectBindingRequired = Session("mIsNewDueReportObjectBindingRequired")
		mrptDueReport = Session("mrptDueReportForOnlyDueReport")
		'End
		mMultiComplianceLinkList = Session("mMultiComplianceLinkList")

	End Sub

	Private Sub SetSession()

		Session("mnWO") = mnWO
		Session("mSelectDueJob") = mSelectDueJob
		Session("mSelectDueJobs") = mSelectDueJobs
		Session("mDueLimits") = mDueLimits

	End Sub

	Private Sub SetTitle()

		lblResult.Text = "List of Due Jobs as per criteria : " & dgDueJob.Rows.Count & " Record(s) found."

	End Sub

	Private Sub AddJobs(isForSingleJob As Boolean)

		Try

			Dim builder = New StringBuilder()
			builder.Append("You have selected the following checks :<br/>")
			' get the selected checkboxes from the form data
			Dim checkString = Request.Form("chkSelect")

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

				If (isForSingleJob = True And
					values.Length > 1) Then '' SPZ Code added by Saylee on 13-Jun-2022  Deccan Code added by Vikrant On 16-Feb-2021

					MSGBoxCtrl.Show("Selection Alert!",
									"Multiple Jobs can not be added in Single W.O.",
									"",
									MsgBoxStyle.OkOnly,
									"RestrictMultipleJobs")

					Exit Sub

				End If

				For Each value As String In values

					builder.Append("<br/>")
					builder.Append(value)
					checkedIds.Add(value)

					If Not mIsNewDueReportObjectBindingRequired = "True" Then 'if condition Added by Vikrant on 19-May-2021 

						mSelectDueJobs(New Guid(value)).IsSelected = True

					End If

				Next

				If Not mIsNewDueReportObjectBindingRequired = "True" Then 'if condition Added by Vikrant on 19-May-2021 

					For i As Integer = 0 To mSelectDueJobs.Count - 1

						If mSelectDueJobs(i).IsSelected = True And Array.IndexOf(values, mSelectDueJobs(i).ID.ToString) = -1 Then

							mSelectDueJobs(i).IsSelected = False

						End If

					Next

				End If

				checkString = Nothing

			End If

			If mIsNewDueReportObjectBindingRequired = "True" Then 'if condition Added by Vikrant on 19-May-2021 

				For i As Integer = mnWO.WOJobs.Count - 1 To 0 Step -1

					If Not checkedIds.Contains(mnWO.WOJobs(i).PreviousTransID.ToString) And mnWO.WOJobs(i).WOJobTypeID = 2 Then

						mnWO.WOJobs.Remove(mnWO.WOJobs(i).PreviousTransID, "")

					End If

				Next

			Else 'End

				For i As Integer = 0 To mSelectDueJobs.Count - 1

					If mSelectDueJobs(i).IsSelected = False Then

						If mnWO.WOJobs.Contains(mSelectDueJobs.Item(i).ID, "") Then

							mnWO.WOJobs.Remove(mSelectDueJobs.Item(i).ID, "")

						End If

					End If

				Next

			End If

			Session("mnWO") = mnWO
			Session("mSelectDueJobs") = mSelectDueJobs

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	Private Sub setWOObject(mDueObject As Object, Index As Integer, mIsNewDueReportObjectBindingRequired As String)
		If mIsNewDueReportObjectBindingRequired = "True" Then
			mDueObject = CType(mDueObject, rptDueReportForOnlyDueReport)
		Else
			mDueObject = CType(mDueObject, SelectDueJobs)
		End If
		If mnWO.WOJobs.Contains(mDueObject(Index).ID, "") = False Then

			Dim Description As String = ""
			Dim LastWOJobDesc As String = ""  'Added by Shital on 15-May-2019
			Dim AssemblyTypeWithPosition As String = ""
			Dim CurrentItem As Object

			If mIsNewDueReportObjectBindingRequired = "True" Then
				CurrentItem = CType(CurrentItem, rptDueReportForOnlyDueReport.rptDueReportForOnlyDueReportInfo)
			Else
				CurrentItem = CType(CurrentItem, SelectDueJob)
			End If

			If mDueObject(Index).OnAssemblyOrComponent = "Assembly" Then

				CurrentItem = mDueObject(Index)

				With CurrentItem

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then

						Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString & IIf(.Position = "", " " & .DataType, " Position: " & .Position & " " & .DataType) & " on Assembly- " & .MaintenanceEvent & CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.: " & .Reference.ToString, ""))    '' & CStr(IIf(.DueAsof2.ToString <> "",  & " Due As Of:" & .DueAsof2.ToString, ""))                            
						'Added By Vikrant On 05-June-2013 For FGA05062013

					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then

						Description = .DataType & " on Assembly-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
						'End
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
							AppSettings("ClientCode") = "APFT" Or
							AppSettings("ClientCode") = "AAP" Then 'Added by Saylee on 9-May-2019

						Dim AssemblyType As String = CStr(IIf(.AssemblyTypeName.ToString = "Airframe", "Aircraft: ", .AssemblyTypeName.ToString & ": "))

						Description = CStr(IIf(.AssemblyModel <> "", vbCrLf & AssemblyType & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString + " ", "")) & IIf(.Position = "", "", " Position: " & .Position) & vbCrLf & .DataType & " on Assembly- " & .MaintenanceEvent & CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & " Ref.: " & .Reference.ToString, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then

						Description = CStr(IIf(mDueObject(Index).Zone.ToString <> "", "System: " & mDueObject(Index).Zone.ToString & vbCrLf, "")) & CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, "")) & CStr(IIf(mDueObject(Index).Remark <> "", vbCrLf & "Remark: " & mDueObject(Index).Remark, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "MEL" Then 'As Model and Serial no not required

						Description = .CodeType & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
					ElseIf (AppSettings("ClientCode") IsNot Nothing And AppSettings("ClientCode") = "PTW") Then 'As only Description,Directive No are required
						Description = CStr(IIf(mDueObject(Index).JobDescription <> "", mDueObject(Index).JobDescription, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & " Directive No.: " & .Number.ToString, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing And AppSettings("ClientCode") = "SAP") Then
						Description = CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & vbCrLf & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, ""))
						'Sankalp 26-11-25
					ElseIf (AppSettings("ClientCode") IsNot Nothing And AppSettings("ClientCode") = "CVA") Then
						Description = .DataType & " on Assembly - " & .CodeType & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

					Else

						If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or
						   AppSettings("ShowAMOOnlyForNewClients") = "True" Then

							Description = .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

						Else

							Description = .DataType & " on Assembly - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

						End If

					End If

				End With

				If AppSettings("setWOJobDescriptionFromPreviousSimilarWO") = "True" Then 'AppSettings code added by Vikrant On 09-Dec-2021 

					'-----------Added by Shital on 15-May-2019-----------
					FetchLastWOJobDescription = FetchLastnWOJobDescription.GetLastnWOJobDescription(PreviousTransID:=mDueObject(Index).ID,
																									ModelMonitorID:=mDueObject(Index).StatusMasterID,
																									mDueObject(Index).AssemblyCompID,
																									mDueObject(Index).OnAssemblyOrComponent,
																									mDueObject(Index).DataType,
																									DoneOndate:=mnWO.WODate)
					LastWOJobDesc = FetchLastWOJobDescription.WOJobDescription
					'-----------

				End If

			ElseIf mDueObject(Index).OnAssemblyOrComponent = "Component" Then

				CurrentItem = mDueObject(Index)

				With CurrentItem

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
						Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & " Position:" & .AssemblyPositionInComp & "<br/>" & .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.PartName <> "", "Part:" & .PartName & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))     '' & CStr(IIf(.DueAsof2.ToString <> "", "<BR>" & " Due As Of:" & .DueAsof2.ToString, ""))
						'Added By Vikrant On 05-June-2013 For FGA05062013
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
						Description = .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.PartName <> "", "Part:" & .PartName & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
						'End
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
						   AppSettings("ClientCode") = "APFT" Or
						   AppSettings("ClientCode") = "AAP" Then 'Added by Saylee on 9-May-2019

						Dim AssemblyType As String = CStr(IIf(.AssemblyTypeName.ToString = "Airframe", "Aircraft: ", .AssemblyTypeName.ToString & ": "))
						Description = CStr(IIf(.AssemblyModel <> "", vbCrLf & AssemblyType & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString + " ", "")) & vbCrLf & .DataType & " on Component- " & .MaintenanceEvent & CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "P/N: " & .PartName, "")) & CStr(IIf(.CompSerialNo <> "", " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & " Ref.: " & .Reference.ToString, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then

						Description = CStr(IIf(mDueObject(Index).Zone.ToString <> "", "System: " & mDueObject(Index).Zone.ToString & vbCrLf, "")) & CStr(IIf(mDueObject(Index).PartDescription.ToString <> "", "Nomenclature: " & mDueObject(Index).PartDescription.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "P/N: " & .PartName, "")) & CStr(IIf(.CompSerialNo <> "", " S/N: " & .CompSerialNo, "")) & CStr(IIf(.Position <> "", " Position: " & .Position, "")) & CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, "")) & CStr(IIf(mDueObject(Index).Remark <> "", vbCrLf & "Remark: " & mDueObject(Index).Remark, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "SAA" Then ' Added By Prashant on 2-Jan-2023

						Description = .DataType & " on Component - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Position <> "", vbCrLf & "Pos.: " & .Position, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "MEL" Then 'As Model and Serial no not required

						Description = .CodeType & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "AFC" Then 'As Model and Serial no not required

						Description = .CodeType & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
					ElseIf (AppSettings("ClientCode") IsNot Nothing And AppSettings("ClientCode") = "PTW") Then 'As only Description,Directive No and Part Details are required

						Description = CStr(IIf(mDueObject(Index).JobDescription <> "", mDueObject(Index).JobDescription, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part:" & .PartName & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Position.ToString <> "", vbCrLf & "Position: " & .Position.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

					ElseIf (AppSettings("ClientCode") IsNot Nothing And AppSettings("ClientCode") = "SAP") Then 'As only Description,Directive No and Part Details are required

						Description = CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, ""))
						'Sankalp 26-11-25
					ElseIf (AppSettings("ClientCode") IsNot Nothing And AppSettings("ClientCode") = "CVA") Then
						Description = .DataType & " on Component - " & .CodeType & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

					Else

						If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then

							Description = .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

						Else

							Description = .DataType & " on Component - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

						End If

					End If

				End With

				If AppSettings("setWOJobDescriptionFromPreviousSimilarWO") = "True" Then 'AppSettings code added by Vikrant On 09-Dec-2021 

					'-----------Added by Shital on 15-May-2019-----------
					FetchLastWOJobDescription = FetchLastnWOJobDescription.GetLastnWOJobDescription(PreviousTransID:=mDueObject(Index).ID,
																									ModelMonitorID:=mDueObject(Index).StatusMasterID,
																									mDueObject(Index).AssemblyCompID,
																									mDueObject(Index).OnAssemblyOrComponent,
																									mDueObject(Index).DataType,
																									DoneOndate:=mnWO.WODate)
					LastWOJobDesc = FetchLastWOJobDescription.WOJobDescription
					'-----------

				End If

			End If

			If AppSettings("ClientCode") IsNot Nothing AndAlso AppSettings("ClientCode") = "TBI" Then
				If mIsNewDueReportObjectBindingRequired = "True" Then
					Description = Description +
						  CStr(IIf(mDueObject(Index).MasterCode <> "",
								   vbCrLf & "MPD Task Reference : " & mDueObject(Index).MasterCode,
								   ""))
				Else
					Description = Description +
										  CStr(IIf(mDueObject(Index).Code <> "",
												   vbCrLf & "MPD Task Reference : " & mDueObject(Index).Code,
												   ""))


				End If



			End If

			'Commented and Added By Saylee On 05-June-2013 For BA07082013
			'Only Job Description is Required
			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA") Then

				Description = CStr(IIf(mDueObject(Index).JobDescription <> "", mDueObject(Index).JobDescription, ""))

			ElseIf AppSettings("ClientCode") = "PAS" Then

				Description = CStr(IIf(mDueObject(Index).JobDescription <> "", mDueObject(Index).JobDescription, "")) & CStr(IIf(mDueObject(Index).Reference.ToString <> "", vbCrLf & "Ref.: " & mDueObject(Index).Reference.ToString, "")) & IIf(mDueObject(Index).Code = "", "", " Code :" & mDueObject(Index).Code) & CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, ""))

			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
				   AppSettings("ClientCode") = "APFT" Or
				   AppSettings("ClientCode") = "AAP" Then

				Description = Description & CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, ""))

			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "MEL" Then 'As Note not required

				Description = Description & CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, ""))
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "PTW" Or AppSettings("ClientCode") = "SAP") Then
				'************************************************************
				'Do Nothing, plz Do Not remove this line as its required to avoid this duplicate mapping of Description..
				'as above already code has need data concatenated
				'************************************************************
				'Sankalp 02-12-25
			ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "CVA") Then

				Description = CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) &
					Description &
					CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, ""))
			Else

				Description = Description & CStr(IIf(mDueObject(Index).JobDescription.ToString <> "", vbCrLf & "Description: " & mDueObject(Index).JobDescription.ToString, "")) & CStr(IIf(mDueObject(Index).Note <> "", vbCrLf & "Note: " & mDueObject(Index).Note, ""))

				If LastWOJobDesc <> "" Then Description = LastWOJobDesc '-----Added by Shital on 15-May-2019

			End If

			'WOJOB:
			mnWO.WOJobs.Add(mnWO.ID, Val(Session("WOJobTypeID")))
			mnWO.WOJobs.CurrentItem.PreviousTransID = mDueObject(Index).ID
			mnWO.WOJobs.CurrentItem.WOJobDescription = Description
			mnWO.WOJobs.CurrentItem.DueAsOf = mDueObject(Index).DueAsof2.Replace("</BR>", vbCrLf)

			If mDueObject(Index).StartDate IsNot DBNull.Value Then mnWO.WOJobs.CurrentItem.WOJobStartDate = mDueObject(Index).StartDate

			mnWO.WOJobs.CurrentItem.TSNCSN = mDueObject(Index).SinceNewTSNCSN
			mnWO.WOJobs.CurrentItem.SBADNO = mDueObject(Index).Number
			mnWO.WOJobs.CurrentItem.ATAChapterID = mDueObject(Index).ATAID
			mnWO.WOJobs.CurrentItem.InspCode = mDueObject(Index).MasterCode  'Added by Saylee on 18-Feb-2018 for ASH18022019 


			mnWO.WOJobs.CurrentItem.TaskSourceRef = mDueObject(Index).Reference 'Added by Saylee on 18-Feb-2018 for ASH18022019 

			If AppSettings("ShowCAMOOnlyForNewClients") = "True" And mDueObject(Index).DataType = "Servicing" Then

				mnWO.WOJobs.CurrentItem.TaskCardNo = mDueObject(Index).TaskNo
				mnWO.WOJobs.CurrentItem.TaskSourceRef = mDueObject(Index).SourceDoc
				mnWO.WOJobs.CurrentItem.Publication = mDueObject(Index).Reference
				mnWO.WOJobs.CurrentItem.Skill = mDueObject(Index).Skill
				mnWO.WOJobs.CurrentItem.SkillID = mDueObject(Index).SkillID

			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And mDueObject(Index).DataType = "Modification" Then

				mnWO.WOJobs.CurrentItem.TaskCardNo = mDueObject(Index).Number
				mnWO.WOJobs.CurrentItem.InspCode = mDueObject(Index).MasterCode

				mnWO.WOJobs.CurrentItem.TaskSourceRef = mDueObject(Index).Reference

			Else

				mnWO.WOJobs.CurrentItem.InspCode = mDueObject(Index).MasterCode 'Added by Saylee on 18-Feb-2018 for ASH18022019 

				mnWO.WOJobs.CurrentItem.TaskSourceRef = mDueObject(Index).Reference

			End If

			If AppSettings("ShowNewDiscrepancyFlow") = "True" And mDueObject(Index).DataType = "Inspection" Then
				mnWO.WOJobs.CurrentItem.TaskCardNo = mDueObject(Index).MasterCode
			End If

			If AppSettings("ClientCode") = "PTW" Or AppSettings("ClientCode") = "FIT" Then 'Pattaya, Added by Saylee on 13-Aug-2024
				If mIsNewDueReportObjectBindingRequired = "True" Then
					mnWO.WOJobs.CurrentItem.InspCode = mDueObject(Index).Code
				Else
					mnWO.WOJobs.CurrentItem.InspCode = mDueObject(Index).CodeType
				End If

			End If

			mnWO.WOJobs.CurrentItem.SkillCode = mDueObject(Index).SkillCode


			'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
			If mDueObject(Index).OnAssemblyOrComponent = "Assembly" Then

				mnWO.WOJobs.CurrentItem.OnTypeID = 1

			ElseIf mDueObject(Index).OnAssemblyOrComponent = "Component" Then

				mnWO.WOJobs.CurrentItem.OnTypeID = 2

			End If

			If mDueObject(Index).DataType = "Servicing" Then

				mnWO.WOJobs.CurrentItem.MonitorTypeID = 1

			ElseIf mDueObject(Index).DataType = "Inspection" Then

				mnWO.WOJobs.CurrentItem.MonitorTypeID = 2

			ElseIf mDueObject(Index).DataType = "Modification" Then

				mnWO.WOJobs.CurrentItem.MonitorTypeID = 3

			End If

			mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = mDueObject(Index).EstimatedHours
			mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = mDueObject(Index).JobDescription 'Added By Vikrant On 19-Dec-2012 For ALL19122012

			'Added by Saylee on 23-July-2013 for BA22072013 	
			mnWO.WOJobs.CurrentItem.Zone = mDueObject(Index).Zone
			mnWO.WOJobs.CurrentItem.AREA = mDueObject(Index).Area
			mnWO.WOJobs.CurrentItem.IsRII = mDueObject(Index).IsRII
			'End

			If mDueObject(Index).AssemblyTypeID = 1 Then
				mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mDueObject(Index).AssemblyTypeName
			Else
				mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mDueObject(Index).AssemblyTypeName + IIf(mDueObject(Index).Position = "", "", "(" + mDueObject(Index).Position + ")")
			End If

			If mnWO.WOJobs.CurrentItem.OnTypeID = 2 Then

				mnWO.WOJobs.CurrentItem.PartNo = mDueObject(Index).PartName
				mnWO.WOJobs.CurrentItem.PartSerialNo = mDueObject(Index).CompSerialNo
				mnWO.WOJobs.CurrentItem.PartDescription = mDueObject(Index).PartDescription
				mnWO.WOJobs.CurrentItem.CompPosition = mDueObject(Index).Position
			End If

			mnWO.WOJobs.CurrentItem.AssemblyModel = mDueObject(Index).AssemblyModel
			mnWO.WOJobs.CurrentItem.AssemblySerialNo = mDueObject(Index).AssemblySerialNo
			mnWO.WOJobs.CurrentItem.AssemblyPosition = mDueObject(Index).AssemblyPositionInComp
			mnWO.WOJobs.CurrentItem.MethodOfCompliance = mDueObject(Index).MethodOfCompliance 'Sankalp 29-08-25

			'' Attachment

			If mDueObject(Index).IsAttachmentAdded Then
				Dim tmpAttachments As FileAttachments
				tmpAttachments = FileAttachments.GetChildFileAttachments(mDueObject(Index).StatusMasterID)

				If tmpAttachments.Count > 0 Then
					For Each mFileAttach As FileAttach In tmpAttachments
						mnWO.WOJobs.CurrentItem.FileAttachments.Add(mnWO.WOJobs.CurrentItem.ID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension)
						mnWO.WOJobs.CurrentItem.FileAttachments.CurrentItem.FileName = mnWO.WOJobs.CurrentItem.TaskCardNo + "_" + mnWO.WOJobs.CurrentItem.MonitorInfoType + mFileAttach.Extension
						mnWO.WOJobs.CurrentItem.IsAttachmentAdded = True
					Next
				End If
			End If
			''*************************************

			With mnWO.WOJobs.CurrentItem

				'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
				Dim mMaintenanceTask As MaintenanceTask
				Dim mMaintenanceTaskDetail As MaintenanceTaskDetail

				If .OnTypeID = 1 Then        'Assembly

					mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID,
																			   .PreviousTransID,
																			   True)

				ElseIf .OnTypeID = 2 Then    'Component

					mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID,
																			   .PreviousTransID,
																			   False)

				End If

				For Each mMaintenanceTaskDetail In mMaintenanceTask.MaintenanceTaskDetails

					mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

					With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem

						.TaskAction = ""  'Added By Prashant 12-Mar-2010
						.ActualStartDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
						.ActualEndDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
						.IsDone = False
						.TaskCardID = mMaintenanceTaskDetail.TaskCardID  'Added By Prashant 29-Dec-2008

						'Added By Utkarsh On 27-Apr-2011
						Dim mTaskCard As TaskCard

						mTaskCard = TaskCard.GetTaskCard(mMaintenanceTaskDetail.TaskCardID)
						.TaskCardNo = mTaskCard.TaskCardNo
						.TaskDescription = mTaskCard.TaskDesc
						.RevNo = mTaskCard.RevNo
						.RevDate = mTaskCard.RevDate
						.IssueDate = mTaskCard.IssueDate
						.Reference = mTaskCard.Reference
						.Equipment = mTaskCard.Equipment
						.Material = mTaskCard.Material
						.EstimatedHours = mTaskCard.EstimatedHours
						.checks = mTaskCard.Check
						.RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo
						.ImageSize = mTaskCard.ImageSize
						.ImageFile = mTaskCard.ImageFile
						.FileExtension = mTaskCard.FileExtension

						'Added by Vikrant on 06-Sept-2013 For BA04092013
						Dim mTaskCardSpare As TaskCardSpare
						Dim mTaskCardStepsSpare As TaskCardSpare

						For Each mTaskCardSpare In mTaskCard.TaskCardSpares

							If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Contains(mTaskCardSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue

								mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares(mTaskCardSpare.ItemID, "").RequiredQty += mTaskCardSpare.RequiredQty

							Else 'existing condition

								mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)

								With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem

									.ItemID = mTaskCardSpare.ItemID
									.RequiredQty = mTaskCardSpare.RequiredQty
									.PartNo = mTaskCardSpare.PartNo
									.Description = mTaskCardSpare.Description
									.Remark = mTaskCardSpare.Remark
									.OnSerialNo = mTaskCardSpare.OnSerialNo
									.OffSerialNo = mTaskCardSpare.OffSerialNo
									.IsForSteps = False

								End With

							End If

						Next

						For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares

							If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Contains(mTaskCardStepsSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue

								mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares(mTaskCardStepsSpare.ItemID, "").RequiredQty += mTaskCardStepsSpare.RequiredQty

							Else 'existing condition

								mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)

								With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem

									.ItemID = mTaskCardStepsSpare.ItemID
									.RequiredQty = mTaskCardStepsSpare.RequiredQty
									.PartNo = mTaskCardStepsSpare.PartNo
									.Description = mTaskCardStepsSpare.Description
									.Remark = mTaskCardStepsSpare.Remark
									.OnSerialNo = mTaskCardStepsSpare.OnSerialNo
									.OffSerialNo = mTaskCardStepsSpare.OffSerialNo
									.IsForSteps = True

								End With

							End If

						Next

						'Added By Vikrant on 03-Mar-2020 For ALL03032020
						For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals

							If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Contains(mTaskCardSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue

								mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals(mTaskCardSpare.ItemID, "").RequiredQty += mTaskCardSpare.RequiredQty

							Else 'existing condition

								mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)

								With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem

									.ItemID = mTaskCardSpare.ItemID
									.RequiredQty = mTaskCardSpare.RequiredQty
									.PartNo = mTaskCardSpare.PartNo
									.Description = mTaskCardSpare.Description
									.Remark = mTaskCardSpare.Remark
									.OnSerialNo = mTaskCardSpare.OnSerialNo
									.OffSerialNo = mTaskCardSpare.OffSerialNo
									.IsForSteps = False
									.IsPartRemoval = True
									.Position = mTaskCardSpare.Position

								End With

							End If

						Next

					End With

				Next

				'KIT(s):
				Dim mMaintenanceKit As MaintenanceKit

				If .OnTypeID = 1 Then        'Assembly

					mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID,
																			.PreviousTransID,
																			True)

				ElseIf .OnTypeID = 2 Then    'Component

					mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID,
																			.PreviousTransID,
																			False)

				End If

				'Added by Saylee on 23-July-2013 for BA22072013 	
				Dim mMaintenanceSpares As MaintenanceKit
				Dim mMaintenanceSparesDetail As MaintenanceKitDetail

				Dim mMaintenanceTools As MaintenanceKit
				Dim mMaintenanceToolsDetail As MaintenanceKitDetail

				If .OnTypeID = 1 Then        'Assembly

					mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID,
																			   .PreviousTransID,
																			   True,
																			   False)

					mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID,
																			  .PreviousTransID,
																			  True,
																			  True)

				ElseIf .OnTypeID = 2 Then    'Component

					mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID,
																			   .PreviousTransID,
																			   False,
																			   False)

					mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID,
																			  .PreviousTransID,
																			  False,
																			  True)

				End If

				For Each mMaintenanceSparesDetail In mMaintenanceSpares.MaintenanceKitDetails

					If mnWO.WOJobs.CurrentItem.WOJobSpares.Contains(mMaintenanceSparesDetail.ItemID, "") Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue

						mnWO.WOJobs.CurrentItem.WOJobSpares(mMaintenanceSparesDetail.ItemID).RequiredQty += mMaintenanceSparesDetail.Qty

					Else 'existing condition

						mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

						With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem

							.ItemID = mMaintenanceSparesDetail.ItemID
							.RequiredQty = mMaintenanceSparesDetail.Qty
							Dim mItem As Item = Item.GetItem(mMaintenanceSparesDetail.ItemID)
							.PartNo = mItem.Name
							.Description = mItem.Description
							mItem = Nothing
							.Remark = mMaintenanceSparesDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014

						End With

					End If

				Next

				For Each mMaintenanceToolsDetail In mMaintenanceTools.MaintenanceKitDetails

					If Not mnWO.WOTools.Contains(mMaintenanceToolsDetail.ItemID) Then

						mnWO.WOTools.Add(mnWO.ID)

						With mnWO.WOTools.CurrentItem

							.ItemID = mMaintenanceToolsDetail.ItemID
							.RequiredQty = mMaintenanceToolsDetail.Qty
							Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
							.PartNo = mItem.Name
							.Description = mItem.Description
							mItem = Nothing
							.WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014

						End With

					Else

						mnWO.WOTools.CurrentIndex = mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").SrNo - 1

						If mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty = 0 Then

						Else

							If (mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty <= mMaintenanceToolsDetail.Qty) Or
							   (mMaintenanceToolsDetail.Qty = 0) Then


								With mnWO.WOTools.CurrentItem

									.ItemID = mMaintenanceToolsDetail.ItemID
									.RequiredQty = mMaintenanceToolsDetail.Qty
									Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
									.PartNo = mItem.Name
									.Description = mItem.Description
									mItem = Nothing
									.WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014

								End With

							End If

						End If

					End If

				Next

			End With

		End If

		Session("mnWO") = mnWO
	End Sub
	Private Sub SetObjectCommon(mDueObject As Object, isForSingleJob As Boolean, Optional IsForLinkMaintenance As Boolean = False)

		Try

			mMultiComplianceLinkList = Session("mMultiComplianceLinkList")

			If mIsNewDueReportObjectBindingRequired = "True" Then
				mDueObject = CType(mDueObject, rptDueReportForOnlyDueReport)
			Else
				mDueObject = CType(mDueObject, SelectDueJobs)
			End If

			Dim i As Integer = 0

			While i < mDueObject.Count

				If checkedIds.Contains(mDueObject(i).ID.ToString) Then

					'Set WO object
					setWOObject(mDueObject, i, mIsNewDueReportObjectBindingRequired)


					'Added by Saylee on 26-Feb-2020, IND26022020
					If AppSettings("ClientCode") = "IND" Then  'This Change is for Aircraft MRO

						If mnWO.WOJobs.Is_Job_IsRII = True Then
							mnWO.IsCriticalWO = True

							'Code to bydefault check "Independent Inspection" in Parameters list 
							Dim mtmpcsRequestsParameterList As ncsWOParametersList
							mtmpcsRequestsParameterList = ncsWOParametersList.GetWOParametersList("Requests")

							If mtmpcsRequestsParameterList IsNot Nothing Then

								If mtmpcsRequestsParameterList.Contains(16) Then '16:Ind. Inspection (Independent Inspection)

									Dim mnWORequestsParameterList As nWOParameterList
									mnWORequestsParameterList = nWOParameterList.GetWOParameterList(mnWO.ID, "Requests")

									If Not mnWORequestsParameterList.Contains(mtmpcsRequestsParameterList(16, "").Name) Then

										Dim mnWORequestsParameter As nWOParameter

										mnWORequestsParameter = nWOParameter.NewParameter(mnWO.ID)
										mnWORequestsParameter.SectionName = mtmpcsRequestsParameterList(16, "").SectionName
										mnWORequestsParameter.WOParameterID = 16
										mnWORequestsParameter.Save()

									End If

								End If

							End If

						End If

					End If

					'Added by Saylee on 12-Sep-2025, as now we have to considered each linked activity as individual WO job
					'check for Linked Maintenance
					If AppSettings("LinkMaintenance") = "True" And IsForLinkMaintenance = True Then

						Dim mLinkMaintenanceList As LinkMaintenanceList
						mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mDueObject(i).StatusMasterID.ToString)

						If mLinkMaintenanceList.Count > 0 Then

							For m As Integer = 0 To mLinkMaintenanceList.Count - 1

								If mnWO.WOJobs.Contains(mLinkMaintenanceList(m).MaintenaceActivityID, "") = False Then

									Dim mLinkMaintenanceMonitorStatus As LinkMaintenaceMonitorStatus
									Dim AssemblyID As Guid

									If mIsNewDueReportObjectBindingRequired = "True" Then
										AssemblyID = mDueObject(i).AssemblyCompID
									Else
										AssemblyID = mDueObject(i).AssemblyID
									End If

									mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mnWO.MachineID, mLinkMaintenanceList(m).LinkedMaintenaceActivityID, mLinkMaintenanceList(m).LinkedMaintenanceTypeID, AssemblyID)
									If Not mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then

										mMultiComplianceLinkList.Add(ID:=mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID,
																	 MaintenanceActivity:=mLinkMaintenanceList(m).LinkedMaintenanceTypeID,
																	 IsSelect:=True,
																	 MaintenanceActionID:=mLinkMaintenanceList(m).MaintenanceActionID,
																	 MaintenanceActionName:=mLinkMaintenanceList(m).MaintenanceActionName)
									End If

								End If

								Session("mMultiComplianceLinkList") = mMultiComplianceLinkList

							Next

						End If

					End If

				End If

				i = i + 1

			End While

			Session("mnWO") = mnWO


			'Added by Saylee on 12-Sep-2025, as now we have to considered each linked activity as individual WO job, 
			If IsForLinkMaintenance = True And isForSingleJob = False Then
				For Each mMulticompliance As MultiCompliance In mMultiComplianceLinkList
					If mMulticompliance.MaintenanceActionID = 4 Then 'if Action is Comply then only to be added in WO job
						If Not checkedIds.Contains(mMulticompliance.ID.ToString) Then
							checkedMaintLinkIds.Add(mMulticompliance.ID.ToString)
						End If
					End If
					i = 0

				Next

				While i < mDueObject.Count
					If checkedMaintLinkIds.Contains(mDueObject(i).ID.ToString) Then
						'Set WO object
						setWOObject(mDueObject, i, mIsNewDueReportObjectBindingRequired)
					End If
					i = i + 1
				End While
			End If
			Session("mnWO") = mnWO
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		If control.Enabled = False Or control.Visible = False Then Exit Sub

		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"

		ClientScript.RegisterStartupScript(Me.GetType(),
										   "focusscript",
										   str)

	End Sub

	Private Sub AddAttributes()

		''  txtAvgMonth.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtAvgMonth').value)")

	End Sub

	Private Sub SetGridObject()

		Dim txtLimit As TextBox
		Dim i As Int32

		For i = 0 To Me.dgDuePeriod.Rows.Count - 1

			txtLimit = CType(Me.dgDuePeriod.Rows(i).FindControl("txtLimit"), TextBox)
			mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text)
			mDueLimits.Item(i).UserName = User.Identity.Name
			mDueLimits.Item(i).PageNo = 2 'wfnWOSelectDueJobList_AJAX

		Next i
		Session("mDueLimits") = mDueLimits

	End Sub

#End Region

#Region " Data Binding "

	'Modified by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
	Private Sub DataFieldBind()

		mDueLimits = DueLimits.GetDueLimits(mnWO.MachineID, UserName:=User.Identity.Name, PageNo:=2) 'DueLimits.GetDueLimits(mnWO.MachineID)
		dgDuePeriod.DataSource = mDueLimits

		If mIsNewDueReportObjectBindingRequired = "True" Then 'Added by Vikrant on 19-May-2021'

			''''''''''''Index for ActivityTypeID''''''''''''

			'1 Assembly Service
			'2 Assembly Inspection
			'3 Assembly Directive
			'4 Component Service
			'5 Component Insp
			'6 Component Mod

			If AppSettings("IsEngineeringWORequired").ToLower = "true" And mnWO.TransTypeID = 102 Then  '102 => Engineering WO

				'mSortedDueJobs = (From list In mrptDueReport
				'				  Where list.ActivityTypeID = 3 Or list.ActivityTypeID = 6   '3 and 6 => Assembly Directive & Component Mod
				'				  Select list).ToList

				'dgDueJob.DataSource = mSortedDueJobs
				mrptDueReport = rptDueReportForOnlyDueReport.
							 GetList(Today.Date.ToString,
									 mnWO.RegNo,
									 IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked,
									 Note:=txtNote.Text.Trim, ShowOnlyDirectives:=True)

			ElseIf AppSettings("IsEngineeringWORequired").ToLower = "true" And mnWO.TransTypeID = 89 Then  '89 => CAMO WO

				'mSortedDueJobs = (From list In mrptDueReport
				'				  Where list.ActivityTypeID = 1 Or list.ActivityTypeID = 2 Or list.ActivityTypeID = 4 Or list.ActivityTypeID = 5
				'				  Select list).ToList        '1, 2, 4 and 5 => Everything except Assembly Directive & Component Mod

				'dgDueJob.DataSource = mSortedDueJobs
				mrptDueReport = rptDueReportForOnlyDueReport.
							 GetList(Today.Date.ToString,
									 mnWO.RegNo,
									 IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked,
									 Note:=txtNote.Text.Trim, SkipOnlyDirectives:=True)

			Else
				mrptDueReport = rptDueReportForOnlyDueReport.
								GetList(Today.Date.ToString,
										mnWO.RegNo,
										IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked,
										Note:=txtNote.Text.Trim)



			End If



			dgDueJob.DataSource = mrptDueReport
			mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)

			If mrptDueReport IsNot Nothing Then

				For Each Child As rptDueReportForOnlyDueReport.rptDueReportForOnlyDueReportInfo In mrptDueReport

					If mnWO.WOJobs.Contains(Child.ID, "") Then

						checkedIds.Add(Child.ID.ToString)

					End If

				Next

			End If

			'Added by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order



			Session("mrptDueReportForOnlyDueReport") = mrptDueReport

		Else 'End

			mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(txtAsOnDate.Text.ToString,
															mDueLimits,
															mnWO.MachineID.ToString,
															0,
															IsEngineeringWORequired:=IIf(AppSettings("IsEngineeringWORequired").
																									ToLower = "true",
																						 True,
																						 False),
															TransTypeID:=mnWO.TransTypeID)

			If mSelectDueJobs IsNot Nothing Then

				For Each Child As SelectDueJob In mSelectDueJobs

					Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")

					If mnWO.WOJobs.Contains(Child.ID, "") Then

						checkedIds.Add(Child.ID.ToString)

					End If

				Next

			End If

			mSortedDueJobList = (From c As SelectDueJob In mSelectDueJobs
								 Order By c.MinimumRemainingValue
								 Select c).ToList

			dgDueJob.DataSource = mSortedDueJobList

		End If

		Session("mDueLimits") = mDueLimits
		Session("mSelectDueJobs") = mSelectDueJobs

		If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or
			AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or
			AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "Novo" Or
			AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "Heligo") Then 'Added By Prashant 24-Jun-2013 BA24062013

			dgDueJob.Columns(13).HeaderText = "Due As Of Airframe"

		Else

			dgDueJob.Columns(13).HeaderText = "Due As Of Assembly"

		End If

		If (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowMaintenanceForNewClients") = "True") Then

			dgDueJob.Columns(2).HeaderText = "Task No./Directive No."

		Else

			dgDueJob.Columns(2).HeaderText = "Code"

		End If

		DataBind()

		If mIsNewDueReportObjectBindingRequired = "True" Then 'Added by Vikrant on 19-May-2021 

			btnDoneTop.Enabled = IIf(mrptDueReport.Count > 0, True, False)

		Else 'End

			btnDoneTop.Enabled = IIf(mSelectDueJobs.Count > 0, True, False)

		End If

	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		If Flag = 1 Then Exit Sub

		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		'this is for grid validation
		Dim Childs As Integer
		Dim Child As DueLimit
		Dim str As String = ""
		Dim BR As Integer

		SetGridObject()

		If Not mnWO.IsValid Then

			For Childs = 0 To mDueLimits.Count - 1

				Child = mDueLimits(Childs)

				For BR = 0 To Child.GetBrokenRulesCollection.Count - 1

					str = str + mDueLimits.Item(Childs).GetBrokenRulesCollection(BR).Description + "<BR>"

				Next

			Next

		End If

		If str <> "" Then

			custValidator.ErrorMessage = str
			e.IsValid = False

		End If

		Flag = 1

	End Sub

#End Region

#Region " Event(s) "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		GetSession()
		AddAttributes()

		If txtAsOnDate.Text.ToString = "" Then

			txtAsOnDate.Text = mnWO.WODateFormatted

		End If

		txtAsOnDate.Enabled = False

		If Not IsPostBack Then

			DataFieldBind()
			SetTitle()
			UpnlGrid.Update()

		End If

	End Sub

	Private Sub FindNow_Click(sender As Object, e As EventArgs) Handles imgFindNow.Click, chkZeroFrequency.CheckedChanged

		Dim mJobs

		If IsValid Then

			SetGridObject()
			If mDueLimits.IsDirty Then
				mDueLimits.Save()
			End If

			dgDueJob.PageIndex = 0

			If mIsNewDueReportObjectBindingRequired = "True" Then 'Added by Vikrant on 19-May-2021 

				'mrptDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString, mnWO.RegNo, IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked, Note:=txtNote.Text.Trim)
				'mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
				'Session("mrptDueReportForOnlyDueReport") = mrptDueReport

			Else 'End

				mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(AsOnDate:=txtAsOnDate.Text.ToString,
																DueLimits:=mDueLimits,
																MachineID:=mnWO.MachineID.ToString,
																AverageMonths:=0,
																ShowZeroFrequency:=chkZeroFrequency.Checked,
																IsEngineeringWORequired:=IIf(AppSettings("IsEngineeringWORequired").
																										ToLower = "true",
																							 True,
																							 False),
																TransTypeID:=mnWO.TransTypeID)
			End If


			If mSelectDueJobs IsNot Nothing Then

				For Each Child As SelectDueJob In mSelectDueJobs

					Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")

					If mnWO.WOJobs.Contains(Child.ID, "") Then
						checkedIds.Add(Child.ID.ToString)
					End If

				Next

			End If

			''''''''''''Index for ActivityTypeID''''''''''''

			'1 Assembly Service
			'2 Assembly Inspection
			'3 Assembly Directive
			'4 Component Service
			'5 Component Insp
			'6 Component Mod

			'Added By Vikrant On 17-Nov-2014 For
			If mIsNewDueReportObjectBindingRequired = "True" Then 'Added by Vikrant on 19-May-2021 

				'Added by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
				If AppSettings("IsEngineeringWORequired").ToLower = "true" And mnWO.TransTypeID = 102 Then '102 => Engineering WO

					'mSortedDueJobs = (From list In mrptDueReport
					'                  Where list.ActivityTypeID = 3 Or list.ActivityTypeID = 6   '3 and 6 => Assembly Directive & Component Mod
					'                  Select list).ToList

					'mJobs = mSortedDueJobs
					mrptDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString,
																		 mnWO.RegNo,
																		 IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked,
																		 Note:=txtNote.Text.Trim,
																		 ShowOnlyDirectives:=True)

				ElseIf AppSettings("IsEngineeringWORequired") And mnWO.TransTypeID = 89 Then '89 => CAMO WO

					'mJobs = (From list In mrptDueReport
					'         Where list.ActivityTypeID = 1 Or list.ActivityTypeID = 2 Or list.ActivityTypeID = 4 Or list.ActivityTypeID = 5
					'         Select list).ToList        '1, 2, 4 and 5 => Everything except Assembly Directive & Component Mod
					mrptDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString,
																		 mnWO.RegNo,
																		 IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked,
																		 Note:=txtNote.Text.Trim,
																		 SkipOnlyDirectives:=True)

				Else


					mrptDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString,
																		 mnWO.RegNo,
																		 IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked,
																		 Note:=txtNote.Text.Trim)

				End If
				mJobs = mrptDueReport
				dgDueJob.DataSource = mrptDueReport
				mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
			Else

				mJobs = (From c As SelectDueJob In mSelectDueJobs
						 Where (c.Note.ToUpper().Contains(txtNote.Text.ToUpper))
						 Order By c.MinimumRemainingValue
						 Select c).ToList
				dgDueJob.DataSource = mJobs
			End If

			Session("mrptDueReportForOnlyDueReport") = mrptDueReport
			Session("mSelectDueJobs") = mSelectDueJobs
			mDueLimits = Session("mDueLimits")
			dgDuePeriod.DataSource = mDueLimits

			If (AppSettings("ClientCode") = "BA" Or
				AppSettings("ClientCode") = "PAS" Or
				AppSettings("ClientCode") = "YA" Or
				AppSettings("ClientCode") = "TA" Or
				AppSettings("ClientCode") = "UHPL" Or
				AppSettings("ClientCode") = "Novo" Or
				AppSettings("ClientCode") = "ADeccan" Or
				AppSettings("ClientCode") = "Heligo") Then  'Added By Prashant 24-Jun-2013 BA24062013

				dgDueJob.Columns(13).HeaderText = "Due As Of Airframe"

			Else

				dgDueJob.Columns(13).HeaderText = "Due As Of Assembly"

			End If

			If (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or
				AppSettings("ShowMaintenanceForNewClients") = "True") Then

				dgDueJob.Columns(2).HeaderText = "Task No./Directive No."

			Else

				dgDueJob.Columns(2).HeaderText = "Code"

			End If

			DataBind()
			lblResult.Text = "List of Due Jobs as per criteria : " & mJobs.Count & " Record(s) found."
			UpnlResult.Update()
			UpnlGrid.Update()

		End If

	End Sub

	Private Sub DoneSelecting(sender As Object, e As EventArgs) Handles btnDoneTop.Click

		Try

			mMultiComplianceLinkList = New MultiComplianceList
			Session("mMultiComplianceLinkList") = mMultiComplianceLinkList

			Dim checkString = Request.Form("chkSelect")



			Dim isForSingleJob As Boolean = False

			'following is for Single job Clients
			'If (
			'		AppSettings("ClientCode") = "STR" Or
			'		AppSettings("ClientCode") = "Deccan" Or
			'		AppSettings("ClientCode") = "IPA" Or
			'		AppSettings("ClientCode") = "FBW" Or
			'		AppSettings("ClientCode") = "IRM" Or
			'		AppSettings("ClientCode") = "SPZ" Or
			'		AppSettings("ClientCode") = "AFC" Or
			'		AppSettings("ClientCode") = "PTW" Or
			'		AppSettings("ClientCode") = "RAJ" Or
			'		AppSettings("ClientCode") = "ASH" Or
			'		AppSettings("ClientCode") = "SIT" Or
			'		AppSettings("ClientCode") = "SKY" Or
			'		AppSettings("ClientCode") = "RGP"
			'	) Then
			'Sankalp 20-11-25
			If AppSettings("SelectOnlySingleJob") = "True" Then

				isForSingleJob = True

			End If

			AddJobs(isForSingleJob)

			If mIsNewDueReportObjectBindingRequired = "True" Then 'Added by Vikrant on 19-May-2021 

				SetObjectCommon(mDueObject:=mrptDueReport, isForSingleJob, IsForLinkMaintenance:=(AppSettings("LinkMaintenance") = "True"))

			Else 'End

				SetObjectCommon(mDueObject:=mSelectDueJobs, isForSingleJob, IsForLinkMaintenance:=(AppSettings("LinkMaintenance") = "True"))

			End If

			If checkString Is Nothing Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.SelectAtleastOne,
								MSGBox.Message_Text.SelectAtleastOne,
								"Please select at-least One Scheduled Job.",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			Else

				Dim values As String() = checkString.Split(","c)

				'If (AppSettings("ClientCode") = "STR" Or
				'	AppSettings("ClientCode") = "Deccan" Or
				'	AppSettings("ClientCode") = "IPA" Or
				'	AppSettings("ClientCode") = "FBW" Or
				'	AppSettings("ClientCode") = "IRM" Or
				'	AppSettings("ClientCode") = "SPZ" Or
				'	AppSettings("ClientCode") = "AFC" Or
				'	AppSettings("ClientCode") = "PTW" Or
				'	AppSettings("ClientCode") = "RAJ" Or
				'	AppSettings("ClientCode") = "ASH" Or
				'	AppSettings("ClientCode") = "SIT") And
				'	values.Length > 1 Then
				'Sankalp 20-11-25
				If AppSettings("SelectOnlySingleJob") = "True" And
					values.Length > 1 Then
					MSGBoxCtrl.Show("Selection Alert!",
									"Multiple Jobs cannot be added in Single W.O.",
									"",
									MsgBoxStyle.OkOnly,
									"RestrictMultipleJobs")

					Exit Sub

				End If

				Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GridViewDueJob_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgDueJob.PageIndexChanged

		dgDueJob.PageIndex = e.NewPageIndex

		If mIsNewDueReportObjectBindingRequired = "True" Then 'Added by Vikrant on 19-May-2021 

			dgDueJob.DataSource = mrptDueReport
			Session("mrptDueReportForOnlyDueReport") = mrptDueReport

		Else

			dgDueJob.DataSource = mSelectDueJobs
			Session("mnWODefferedJobs") = mSelectDueJobs

		End If

		dgDueJob.DataBind()

	End Sub

	Private Sub Back_Click(sender As Object, e As EventArgs) Handles btnBackTop.Click
		If Session("wfProject_Ajax") = "wfProject_Ajax" Then
			Session("OpenFromProject") = Nothing
			Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("mTransTypeID").ToString
		End If

		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

	End Sub

	Private Sub GridViewDueJob_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgDueJob.RowCommand

		Select Case e.CommandName
			Case "ViewSpareList" 'Added By Prashant 20-Dec-2018 
				Dim mStatusMasterID As Guid
				mStatusMasterID = New Guid(e.CommandArgument.ToString)
				Session("StatusMasterID") = mStatusMasterID
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSpareListWindow", "OpenSpareListWindow()", True)
		End Select

	End Sub

	Private Sub GridViewDueJob_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgDueJob.RowDataBound

		If (e.Row.RowType = DataControlRowType.DataRow) Then

			Dim StatusMasterID As Guid = (DataBinder.Eval(e.Row.DataItem, "StatusMasterID"))
			Dim grdDueJob As GridView = DirectCast(e.Row.FindControl("dgDueJob"), GridView)

			mSpareListByMaintenanceActivity = SpareListByMaintenanceActivity.GetList(Today.Date.ToString, StatusMasterID.ToString)
			mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(StatusMasterID, True)
			mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(StatusMasterID)

			If mSpareListByMaintenanceActivity.Count = 0 And mMaintenanceKit.MaintenanceKitDetails.Count = 0 And
			   mMaintenanceTask.MaintenanceTaskDetails.Count = 0 Then

				Dim btnImageButton As ImageButton = CType(e.Row.FindControl("btnImageButton"), ImageButton)
				btnImageButton.Visible = False

			End If

		End If

	End Sub

#End Region

#Region " Checked Selection "

	Public Function NumeroChequeInclus(numero As String) As String

		If (checkedIds.Contains(numero)) Then

			Return "checked"

		Else

			Return String.Empty

		End If

	End Function

#End Region

End Class