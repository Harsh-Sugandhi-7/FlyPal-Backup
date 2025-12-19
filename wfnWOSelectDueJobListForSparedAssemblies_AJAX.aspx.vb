'ALL27072020

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System
Imports System.IO

Public Class wfnWOSelectDueJobListForSparedAssemblies_AJAX
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mnWO As nWO

    Public mSelectDueJob As SelectDueJob
    Public mSelectDueJobs As SelectDueJobs
    Public mDueLimits As DueLimits
    Private Flag As Int16

    Dim mIsSelected As Boolean = False
    Private checkedIds As New List(Of String)()
    Dim mSortedDueJobList As List(Of SelectDueJob) = New List(Of SelectDueJob)
    Dim mSpareListByMaintenanceActivity As SpareListByMaintenanceActivity
    Dim mFetchLastnWOJobDescription As FetchLastnWOJobDescription     ' Added by Shital on 15-May-2019
    Public mMaintenanceKit As MaintenanceKit
    Public mMaintenanceTask As MaintenanceTask
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mnWO = Session("mnWO")
        mSelectDueJob = Session("mSelectDueJob")
        mSelectDueJobs = Session("mSelectDueJobs")
        mDueLimits = Session("mDueLimits")
    End Sub
    Private Sub SetSession()
        Session("mnWO") = mnWO
        Session("mSelectDueJob") = mSelectDueJob
        Session("mSelectDueJobs") = mSelectDueJobs
        Session("mDueLimits") = mDueLimits
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "List of Due Jobs as per criteria :" & mSelectDueJobs.Count & " Record(s) found."
    End Sub
    Private Sub AddJobs()
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
                MSGBoxCtrl.show("Selection Alert!", "Multiple Jobs can not be added in single WO.", "", MsgBoxStyle.OkOnly, "RestrictMultJobs")
                Exit Sub
            End If
            For Each value As String In values
                builder.Append("<br/>")
                builder.Append(value)
                checkedIds.Add(value)
                mSelectDueJobs(New Guid(value)).IsSelected = True
            Next

            For i As Integer = 0 To mSelectDueJobs.Count - 1
                If mSelectDueJobs(i).IsSelected = True And Array.IndexOf(values, mSelectDueJobs(i).ID.ToString) = -1 Then
                    mSelectDueJobs(i).IsSelected = False
                End If
            Next
            'For Each value As String In values
            '    builder.Append("<br/>")
            '    builder.Append(value)
            '    checkedIds.Add(value)
            '    ' mMaintenanceTask.MaintenanceTaskDetails.Remove(New Guid(value), "")
            '    mSelectDueJobs(New Guid(value)).IsSelected = True
            '    'If mSelectDueJobs.Contains(New Guid(value)) Then
            '    '    mSelectDueJobs(New Guid(value)).IsSelected = True
            '    'End If
            'Next
            'values = ""
            checkString = Nothing
        End If

        For i As Integer = 0 To mSelectDueJobs.Count - 1
            If mSelectDueJobs(i).IsSelected = False Then
                If mnWO.WOJobs.Contains(mSelectDueJobs.Item(i).ID, "") Then
                    mnWO.WOJobs.Remove(mSelectDueJobs.Item(i).ID, "")
                End If
            End If
        Next
        Session("mnWO") = mnWO
        Session("mSelectDueJobs") = mSelectDueJobs
    End Sub
    Private Sub setObject()
        Dim i As Integer = 0
        While i < mSelectDueJobs.Count
            If mSelectDueJobs.Item(i).IsDirty = True Then
                If mSelectDueJobs.Item(i).IsSelected = True Then
                    mIsSelected = True
                    If mnWO.WOJobs.Contains(mSelectDueJobs.Item(i).ID, "") = False Then

                        Dim Description As String = ""

                        Dim LastWOJobDesc As String = ""  'Added by Shital on 15-May-2019
                        Dim AssemblyTypeWithPosition As String = ""

                        If mSelectDueJobs.Item(i).OnAssemblyOrComponent = "Assembly" Then
                            With mSelectDueJobs.Item(i)
                                'Description = .DataType & " on Assembly-" & .MaintenanceEvent & "<BR>" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & "<BR>" & "Directive No.:" & .Number.ToString & " Ref.:" & .Reference.ToString
                                If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
                                    'Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString & " Position: " & .Position & "<br/>" & .DataType & " on Assembly- " & .MaintenanceEvent & CStr(IIf(mSelectDueJobs.Item(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mSelectDueJobs.Item(i).JobDescription.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.: " & .Reference.ToString, ""))    '' & CStr(IIf(.DueAsof2.ToString <> "",  & " Due As Of:" & .DueAsof2.ToString, ""))                            
                                    Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString & IIf(.Position = "", " " & .DataType, " Position: " & .Position & " " & .DataType) & " on Assembly- " & .MaintenanceEvent & CStr(IIf(mSelectDueJobs.Item(i).JobDescription.ToString <> "", vbCrLf & " Description: " & mSelectDueJobs.Item(i).JobDescription.ToString, "")) & CStr(IIf(.Number.ToString <> "", " Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.: " & .Reference.ToString, ""))
                                    'Added By VIkrant On 05-June-2013 For FGA05062013
                                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
                                    Description = .DataType & " on Assembly-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                                    'End
                                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                                       AppSettings("ClientCode") = "APFT" Or
                                       AppSettings("ClientCode") = "AAP" Then 'Added by Saylee on 9-May-2019
                                    Dim AssemblyType As String = CStr(IIf(.AssemblyType.ToString = "Airframe", "Aircraft: ", .AssemblyType.ToString & ": "))
                                    Description = CStr(IIf(.AssemblyModel <> "", vbCrLf & AssemblyType & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString + " ", "")) & vbCrLf & .DataType & " on Assembly- " & .MaintenanceEvent & CStr(IIf(mSelectDueJobs.Item(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mSelectDueJobs.Item(i).JobDescription.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & " Ref.: " & .Reference.ToString, ""))
                                Else
                                    'Description = .DataType & " on Assembly-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, "")) & CStr(IIf(.SinceNew.ToString <> "", " Current Values:" & .SinceNew.ToString, ""))        '' & CStr(IIf(.DueAsof2.ToString <> "",  & " Due As Of:" & .DueAsof2.ToString, ""))
                                    Description = .DataType & " on Assembly - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                                End If
                            End With
                            If AppSettings("setWOJobDescriptionFromPreviousSimilarWO") = "True" Then 'Appsetting code added by Vikrant On 09-Dec-2021 
                                '-----------Added by Shital on 15-May-2019-----------
                                mFetchLastnWOJobDescription = FetchLastnWOJobDescription.GetLastnWOJobDescription(mSelectDueJobs.Item(i).ID, mSelectDueJobs.Item(i).StatusMasterID, mSelectDueJobs.Item(i).AssemblyID, mSelectDueJobs(i).OnAssemblyOrComponent, mSelectDueJobs.Item(i).DataType, mnWO.WODate)
                                LastWOJobDesc = mFetchLastnWOJobDescription.WOJobDescription
                                '-----------
                            End If
                            
                        ElseIf mSelectDueJobs.Item(i).OnAssemblyOrComponent = "Component" Then
                            With mSelectDueJobs.Item(i)
                                'Description = .DataType & " on Component-" & .MaintenanceEvent & "<BR>" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & "<BR>" & "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString & "<BR>" & "Directive No.:" & .Number.ToString & " Ref.:" & .Reference.ToString
                                If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
                                    'Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & " Position:" & .AssemblyPositionInComp & "<br/>" & .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.PartNo <> "", "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))     '' & CStr(IIf(.DueAsof2.ToString <> "", "<BR>" & " Due As Of:" & .DueAsof2.ToString, ""))
                                    Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & IIf(.AssemblyPositionInComp = "", " " & .DataType, " Position: " & .AssemblyPositionInComp & " " & .DataType) & " on Component-" & .MaintenanceEvent & CStr(IIf(.PartNo <> "", " Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", " Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                                    'Added By VIkrant On 05-June-2013 For FGA05062013
                                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
                                    Description = .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.PartNo <> "", "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                                    'End
                                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                                       AppSettings("ClientCode") = "APFT" Or
                                       AppSettings("ClientCode") = "AAP" Then 'Added by Saylee on 9-May-2019
                                    Dim AssemblyType As String = CStr(IIf(.AssemblyType.ToString = "Airframe", "Aircraft: ", .AssemblyType.ToString & ": "))
                                    Description = CStr(IIf(.AssemblyModel <> "", vbCrLf & AssemblyType & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString + " ", "")) & vbCrLf & .DataType & " on Component- " & .MaintenanceEvent & CStr(IIf(mSelectDueJobs.Item(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mSelectDueJobs.Item(i).JobDescription.ToString, "")) & CStr(IIf(.PartNo <> "", vbCrLf & "P/N: " & .PartNo, "")) & CStr(IIf(.CompSerialNo <> "", " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & " Ref.: " & .Reference.ToString, ""))
                                Else
                                    'Description = .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.PartNo <> "", "Part:" & .PartNo & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, "")) & CStr(IIf(.SinceNew.ToString <> "", " Current Values:" & .SinceNew.ToString, ""))          '' & CStr(IIf(.DueAsof2.ToString <> "",  & " Due As Of:" & .DueAsof2.ToString, ""))
                                    Description = .DataType & " on Component - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartNo <> "", vbCrLf & "Part: " & .PartNo & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                                End If
                            End With
                            If AppSettings("setWOJobDescriptionFromPreviousSimilarWO") = "True" Then 'Appsetting code added by Vikrant On 09-Dec-2021 
                                '-----------Added by Shital on 15-May-2019-----------
                                mFetchLastnWOJobDescription = FetchLastnWOJobDescription.GetLastnWOJobDescription(mSelectDueJobs.Item(i).ID, mSelectDueJobs.Item(i).StatusMasterID, mSelectDueJobs.Item(i).CompID, mSelectDueJobs(i).OnAssemblyOrComponent, mSelectDueJobs.Item(i).DataType, mnWO.WODate)
                                LastWOJobDesc = mFetchLastnWOJobDescription.WOJobDescription
                                '-----------
                            End If
                        End If
                        'Commented and Added By Saylee On 05-June-2013 For BA07082013
                        '''''''''Description = Description & CStr(IIf(mSelectDueJobs.Item(i).JobDescription <> "", mSelectDueJobs.Item(i).JobDescription, "")) & CStr(IIf(mSelectDueJobs.Item(i).Note <> "", mSelectDueJobs.Item(i).Note, ""))
                        'Here BA needs only description fro Master so directly assigned JobDescription
                        ''If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then ' Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "YA") Then
                        If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo") Then
                            Description = CStr(IIf(mSelectDueJobs.Item(i).JobDescription <> "", mSelectDueJobs.Item(i).JobDescription, ""))
                        ElseIf AppSettings("ClientCode") = "PAS" Then
                            Description = CStr(IIf(mSelectDueJobs.Item(i).JobDescription <> "", mSelectDueJobs.Item(i).JobDescription, "")) & CStr(IIf(mSelectDueJobs.Item(i).Reference.ToString <> "", vbCrLf & "Ref.: " & mSelectDueJobs.Item(i).Reference.ToString, "")) & IIf(mSelectDueJobs.Item(i).Code = "", "", " Code :" & mSelectDueJobs.Item(i).Code) & CStr(IIf(mSelectDueJobs.Item(i).Note <> "", vbCrLf & "Note: " & mSelectDueJobs.Item(i).Note, ""))
                        ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                               AppSettings("ClientCode") = "APFT" Or
                               AppSettings("ClientCode") = "AAP" Then
                            Description = Description & CStr(IIf(mSelectDueJobs.Item(i).Note <> "", vbCrLf & "Note: " & mSelectDueJobs.Item(i).Note, ""))
                        Else
                            Description = Description & CStr(IIf(mSelectDueJobs.Item(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mSelectDueJobs.Item(i).JobDescription.ToString, "")) & CStr(IIf(mSelectDueJobs.Item(i).Note <> "", vbCrLf & "Note: " & mSelectDueJobs.Item(i).Note, ""))
                            If LastWOJobDesc <> "" Then Description = LastWOJobDesc '-----Added by Shital on 15-May-2019
                        End If
                        '------------------------------

                        'WOJOB:
                        ' mnWO.WOJobs.Add(nWOJob.NewWOJob(mnWO.ID))
                        mnWO.WOJobs.Add(mnWO.ID, Val(Session("WOJobTypeID")))
                        mnWO.WOJobs.CurrentItem.PreviousTransID = mSelectDueJobs.Item(i).ID
                        mnWO.WOJobs.CurrentItem.WOJobDescription = Description
                        mnWO.WOJobs.CurrentItem.DueAsOf = mSelectDueJobs.Item(i).DueAsof2

                        If Not mSelectDueJobs.Item(i).StartDate Is DBNull.Value Then mnWO.WOJobs.CurrentItem.WOJobStartDate = mSelectDueJobs.Item(i).StartDate
                        mnWO.WOJobs.CurrentItem.TSNCSN = mSelectDueJobs.Item(i).SinceNewTSNCSN
                        mnWO.WOJobs.CurrentItem.SBADNO = mSelectDueJobs.Item(i).Number
                        mnWO.WOJobs.CurrentItem.ATAChapterID = mSelectDueJobs.Item(i).ATAID

						If AppSettings("ShowCAMOOnlyForNewClients") = "True" And mSelectDueJobs.Item(i).DataType = "Servicing" Then

							mnWO.WOJobs.CurrentItem.TaskCardNo = mSelectDueJobs.Item(i).TaskCardNo
							mnWO.WOJobs.CurrentItem.TaskSourceRef = mSelectDueJobs.Item(i).SourceDoc
							mnWO.WOJobs.CurrentItem.Publication = mSelectDueJobs.Item(i).Reference
							mnWO.WOJobs.CurrentItem.Skill = mSelectDueJobs.Item(i).Skill
							mnWO.WOJobs.CurrentItem.SkillID = mSelectDueJobs.Item(i).SkillID

						ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And mSelectDueJobs.Item(i).DataType = "Modification" Then

							mnWO.WOJobs.CurrentItem.TaskCardNo = mSelectDueJobs.Item(i).Number
							mnWO.WOJobs.CurrentItem.InspCode = mSelectDueJobs.Item(i).Code
							mnWO.WOJobs.CurrentItem.TaskSourceRef = mSelectDueJobs.Item(i).Reference

						Else
							mnWO.WOJobs.CurrentItem.InspCode = mSelectDueJobs.Item(i).Code 'Added by Saylee on 18-Feb-2018 for ASH18022019 
							mnWO.WOJobs.CurrentItem.TaskSourceRef = mSelectDueJobs.Item(i).Reference
						End If
						'mnWO.WOJobs.CurrentItem.InspCode = mSelectDueJobs.Item(i).Code 'Added by Saylee on 18-Feb-2018 for ASH18022019 
						'mnWO.WOJobs.CurrentItem.TaskSourceRef = mSelectDueJobs.Item(i).Reference 'Added by Saylee on 18-Feb-2018 for ASH18022019 

						'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
						If mSelectDueJobs.Item(i).OnAssemblyOrComponent = "Assembly" Then
                            mnWO.WOJobs.CurrentItem.OnTypeID = 1
                        ElseIf mSelectDueJobs.Item(i).OnAssemblyOrComponent = "Component" Then
                            mnWO.WOJobs.CurrentItem.OnTypeID = 2
                        End If
                        If mSelectDueJobs.Item(i).DataType = "Servicing" Then
                            mnWO.WOJobs.CurrentItem.MonitorTypeID = 1
                        ElseIf mSelectDueJobs.Item(i).DataType = "Inspection" Then
                            mnWO.WOJobs.CurrentItem.MonitorTypeID = 2
                        ElseIf mSelectDueJobs.Item(i).DataType = "Modification" Then
                            mnWO.WOJobs.CurrentItem.MonitorTypeID = 3
                        End If
                        '-----------------------------------------------------------------------
                        mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = mSelectDueJobs.Item(i).EstimatedHours                         ''mnWO.StatusID = mnWO.WOJobs.StatusChangeOfWO

                        mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = mSelectDueJobs.Item(i).JobDescription 'Added By Vikrant On 19-Dec-2012 For ALL19122012

                        'Added by Saylee on 23-July-2013 for BA22072013 	
                        mnWO.WOJobs.CurrentItem.Zone = mSelectDueJobs.Item(i).Zone
                        mnWO.WOJobs.CurrentItem.AREA = mSelectDueJobs.Item(i).Area
                        mnWO.WOJobs.CurrentItem.IsRII = mSelectDueJobs.Item(i).IsRII
                        'End
                        If mSelectDueJobs.Item(i).AssemblyTypeID = 1 Then
                            mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mSelectDueJobs.Item(i).AssemblyType
                        Else
                            mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mSelectDueJobs.Item(i).AssemblyType + IIf(mSelectDueJobs.Item(i).Position = "", "", "(" + mSelectDueJobs.Item(i).Position + ")")
                        End If



                        With mnWO.WOJobs.CurrentItem
                            'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
                            'TASK(s):
                            Dim mMaintenanceTask As MaintenanceTask
                            Dim mMaintenanceTaskDetail As MaintenanceTaskDetail

                            If .OnTypeID = 1 Then        'Assembly
                                mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, True)
                            ElseIf .OnTypeID = 2 Then    'Componant
                                mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, False)
                            End If

                            For Each mMaintenanceTaskDetail In mMaintenanceTask.MaintenanceTaskDetails
                                mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

                                With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem
                                    '.TaskAction = "No action taken." 'mMaintenanceTaskDetail.Task 'Commented By Prashant 12-Mar-2010
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

                                    ''Added by Saylee on 4-Feb-2013
                                    ''If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
                                    ''    .Reference = mSelectDueJobs.Item(i).Reference
                                    ''Else
                                    ''    .Reference = mTaskCard.Reference
                                    ''End If
                                    '***************************
                                    ''Commentedby Saylee on 15-Feb-2013
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

                                    Next

                                    For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
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
                                    Next
                                    'End
                                    'Added By Vikrant on 03-Mar-2020 For ALL03032020
                                    For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals
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

                                    Next
                                    'End
                                End With
                            Next

                            'KIT(s):
                            Dim mMaintenanceKit As MaintenanceKit

                            If .OnTypeID = 1 Then        'Assembly
                                mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True)
                            ElseIf .OnTypeID = 2 Then    'Componant
                                mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False)
                            End If
                            'Commented and Added by Saylee on 23-July-2013 for BA22072013 	
                            ''''For Each mMaintenanceKitDetail In mMaintenanceKit.MaintenanceKitDetails
                            ''''    mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

                            ''''    With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
                            ''''        .ItemID = mMaintenanceKitDetail.ItemID
                            ''''        .RequiredQty = mMaintenanceKitDetail.Qty
                            ''''        Dim mItem As Item = Item.GetItem(mMaintenanceKitDetail.ItemID)
                            ''''        .PartNo = mItem.Name
                            ''''        .Description = mItem.Description
                            ''''        mItem = Nothing
                            ''''    End With
                            ''''Next
                            '''''-----------------------------------------------------------------------
                            'Added by Saylee on 23-July-2013 for BA22072013 	
                            Dim mMaintenanceSpares As MaintenanceKit
                            Dim mMaintenanceSparesDetail As MaintenanceKitDetail

                            Dim mMaintenanceTools As MaintenanceKit
                            Dim mMaintenanceToolsDetail As MaintenanceKitDetail

                            If .OnTypeID = 1 Then        'Assembly
                                mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, False)
                                mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, True)
                            ElseIf .OnTypeID = 2 Then    'Componant
                                mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, False)
                                mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, True)
                            End If

                            For Each mMaintenanceSparesDetail In mMaintenanceSpares.MaintenanceKitDetails
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
                                        If (mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty <= mMaintenanceToolsDetail.Qty) Or (mMaintenanceToolsDetail.Qty = 0) Then
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
                            '-----------------------------------------------------------------------
                        End With
                    End If


                    'Added by Saylee on 26-Feb-2020, IND26022020
                    If AppSettings("ClientCode") = "IND" Then  'This Change is for Aircraft MRO
                        If mnWO.WOJobs.Is_Job_IsRII = True Then
                            mnWO.IsCriticalWO = True


                            'Code to bydefault check "Independent Inspection" in Parameters list 
                            Dim mtmpcsRequestsParameterList As ncsWOParametersList
                            mtmpcsRequestsParameterList = ncsWOParametersList.GetWOParametersList("Requests")
                            If Not mtmpcsRequestsParameterList Is Nothing Then
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
                    '*****************************************************
                Else
                    ''If mnWO.WOJobs.Contains(mSelectDueJobs.Item(i).ID, "") Then
                    ''    mnWO.WOJobs.Remove(mSelectDueJobs.Item(i).ID, "")
                    ''End If
                End If
            End If
            i = i + 1
        End While
        Session("mnWO") = mnWO
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub addAttributes()
        ''  txtAvgMonth.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtAvgMonth').value)")
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriod.Rows.Count - 1
            txtLimit = CType(Me.dgDuePeriod.Rows(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text)
        Next i
        Session("mDueLimits") = mDueLimits
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        Dim IsForSpareAssembly As Boolean = False
        Dim IsForSpareComp As Boolean = False
        If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
            IsForSpareAssembly = CBool(Session("IsWOForRemovedOrSpareAssembly"))
        ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
            IsForSpareComp = CBool(Session("IsWOForRemovedOrSpareComp"))

        End If
        mDueLimits = DueLimits.GetDueLimits(mnWO.MachineID)
        dgDuePeriod.DataSource = mDueLimits
        If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
            mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(txtAsOnDate.Text.ToString, mDueLimits, mnWO.MachineID.ToString, 0, IsForSpareAssembly:=IsForSpareAssembly, AssemblyID:=Session("AssemblyID").ToString, IsRemovedAssembly:=CBool(Session("IsRemovedAssembly")))
        Else
            mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(txtAsOnDate.Text.ToString, mDueLimits, mnWO.MachineID.ToString, 0, IsForSpareComponent:=IsForSpareComp, CompStatusID:=Session("CompStatusID").ToString, IsSpareOrRemoveComp:=CInt(Session("IsSpareOrRemovedComp")))
        End If

        If Not mSelectDueJobs Is Nothing Then
            For Each Child As SelectDueJob In mSelectDueJobs
                Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")
                'this.Request.Form[this.txtName.UniqueID]
                '= Request.Form("chkSelect")
                If mnWO.WOJobs.Contains(Child.ID, "") Then
                    checkedIds.Add(Child.ID.ToString)
                End If
            Next
        End If
        mSortedDueJobList = (From c As SelectDueJob In mSelectDueJobs
               Order By c.MinimumRemainingValue
               Select c).ToList
        dgDueJob.DataSource = mSortedDueJobList
        Session("mDueLimits") = mDueLimits
        Session("mSelectDueJobs") = mSelectDueJobs
        DataBind()

        If ConfigurationManager.AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or ConfigurationManager.AppSettings("ClientCode") = "YA" Or ConfigurationManager.AppSettings("ClientCode") = "TA" Or ConfigurationManager.AppSettings("ClientCode") = "UHPL" Or ConfigurationManager.AppSettings("ClientCode") = "Novo" Then  'Added By Prashant 24-Jun-2013 BA24062013
            dgDueJob.Columns(12).HeaderText = "Airframe Due As Of"
        Else
            dgDueJob.Columns(12).HeaderText = "Due As Of"
        End If


        If mSelectDueJobs.Count > 10 Then btnDoneTop.Visible = True
        If mSelectDueJobs.Count > 10 Then btnBackTop.Visible = True
    End Sub
    ''Private Function CustomValidate1() As Boolean
    ''    AddJobs()
    ''    Dim strMSG As String = ""
    ''    Dim i As Integer = 0
    ''    While i < mSelectDueJobs.Count
    ''        If mSelectDueJobs.Item(i).IsDirty = True Then
    ''            If mSelectDueJobs.Item(i).IsSelected = True Then
    ''                mIsSelected = True
    ''                If mnWO.WOJobs.Contains(mSelectDueJobs.Item(i).ID, "") = True Then
    ''                    strMSG = strMSG + " Duplicate Scheduled Job " + mSelectDueJobs.Item(i).LogBook + " " + mSelectDueJobs.Item(i).DataType + "<BR>"
    ''                End If
    ''            End If
    ''        End If
    ''        i = i + 1
    ''    End While
    ''    Session("mnWO") = mnWO
    ''    If strMSG.Trim <> "" Then
    ''        cvControlValidator.ErrorMessage = strMSG
    ''        cvControlValidator.IsValid = False
    ''        Return False
    ''    End If
    ''    Return True
    ''End Function
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
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

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        If txtAsOnDate.Text.ToString = "" Then
            txtAsOnDate.Text = mnWO.WODateFormatted
        End If
        txtAsOnDate.Enabled = False
        If Not IsPostBack Then
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, chkZeroFrequency.CheckedChanged
        If IsValid Then
            SetGridObject()
            dgDueJob.PageIndex = 0


            Dim IsForSpareAssembly As Boolean = False
            Dim IsForSpareComp As Boolean = False
            If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
                IsForSpareAssembly = CBool(Session("IsWOForRemovedOrSpareAssembly"))
            ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
                IsForSpareComp = CBool(Session("IsWOForRemovedOrSpareComp"))
            End If

            If mnWO.TransTypeID = Trans.SpareAssemblyWO Then
                mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(txtAsOnDate.Text.ToString, mDueLimits, mnWO.MachineID.ToString, 0, chkZeroFrequency.Checked, IsForSpareAssembly:=IsForSpareAssembly, AssemblyID:=Session("AssemblyID").ToString, IsRemovedAssembly:=CBool(Session("IsRemovedAssembly")))
            Else
                mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(txtAsOnDate.Text.ToString, mDueLimits, mnWO.MachineID.ToString, 0, chkZeroFrequency.Checked, IsForSpareComponent:=IsForSpareComp, CompStatusID:=Session("CompStatusID").ToString, IsSpareOrRemoveComp:=CInt(Session("IsSpareOrRemovedComp")))
            End If

            If Not mSelectDueJobs Is Nothing Then
                For Each Child As SelectDueJob In mSelectDueJobs
                    Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")
                    If mnWO.WOJobs.Contains(Child.ID, "") Then
                        checkedIds.Add(Child.ID.ToString)
                    End If
                Next
            End If

            'Added By Vikrant On 17-Nov-2014 For 
            Dim mJobs = (From c As SelectDueJob In mSelectDueJobs
                         Where (c.Note.ToUpper().Contains(txtNote.Text.ToUpper))
                         Order By c.MinimumRemainingValue
                         Select c).ToList
            'End
            dgDueJob.DataSource = mJobs
            Session("mSelectDueJobs") = mSelectDueJobs
            mDueLimits = Session("mDueLimits")
            dgDuePeriod.DataSource = mDueLimits
            DataBind()
            lblResult.Text = "List of Due Jobs as per criteria :" & mJobs.Count & " Record(s) found."
            UpnlResult.Update()
            UpnlGrid.Update()
        End If
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoneTop.Click, btnDone.Click
        '' If Not CustomValidate1() Then Exit Sub
        AddJobs()
        setObject()
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select atleast one scheduled job.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            Dim values As String() = checkString.Split(","c)
            If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") And values.Length > 1 Then
                Exit Sub
            End If
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        End If

    End Sub
    Private Sub dgDueJob_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDueJob.PageIndexChanged
        dgDueJob.PageIndex = e.NewPageIndex
        dgDueJob.DataSource = mSelectDueJobs
        Session("mnWODefferedJobs") = mSelectDueJobs
        dgDueJob.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub dgDueJob_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueJob.RowCommand
        Select Case e.CommandName
            Case "ViewSpareList" 'Added By Prashant 20-Dec-2018 
                Dim mStatusMasterID As Guid
                mStatusMasterID = New Guid(e.CommandArgument.ToString)
                Session("StatusMasterID") = mStatusMasterID
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSpareListWindow", "OpenSpareListWindow()", True)
        End Select
    End Sub
    Private Sub dgDueJob_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDueJob.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim StatusMasterID As Guid = (DataBinder.Eval(e.Row.DataItem, "StatusMasterID"))
            mSpareListByMaintenanceActivity = SpareListByMaintenanceActivity.GetList(Today.Date.ToString, StatusMasterID.ToString)
            Dim grdDueJob As GridView = DirectCast(e.Row.FindControl("dgDueJob"), GridView)

            mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(StatusMasterID, True)
            mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(StatusMasterID)
            If mSpareListByMaintenanceActivity.Count = 0 And mMaintenanceKit.MaintenanceKitDetails.Count = 0 And mMaintenanceTask.MaintenanceTaskDetails.Count = 0 Then
                Dim btnImageButton As ImageButton = CType(e.Row.FindControl("btnImageButton"), ImageButton)
                btnImageButton.Visible = False
            End If
        End If
    End Sub
#End Region

#Region "Checked Selection"

    Public Function NumeroChequeInclus(ByVal numero As String) As String

        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
#End Region

End Class