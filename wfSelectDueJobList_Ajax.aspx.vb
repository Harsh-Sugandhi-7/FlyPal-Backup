Imports System.Collections.Generic
Imports System.Text
Imports AjaxControlToolkit
Imports Org.BouncyCastle.Crypto.Tls

Public Class wfSelectDueJobList_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mDueJobPlanning As DueJobPlanning

    Public mSelectDueJob As SelectDueJob
    Public mSelectDueJobs As SelectDueJobs
    Public mDueLimits As DueLimits
    Private Flag As Int16

    Dim mIsSelected As Boolean = False
    Private checkedIds As New List(Of String)()
    Dim mSortedDueJobList As List(Of SelectDueJob) = New List(Of SelectDueJob)
    Dim mSpareListByMaintenanceActivity As SpareListByMaintenanceActivity
    Dim mFetchLastnWOJobDescription As FetchLastnWOJobDescription
    Public mMaintenanceTask As MaintenanceTask

    Dim mIsNewDueReportObjectBindingRequired As String
    Public mrptDueReport As rptDueReportForOnlyDueReport

#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mDueJobPlanning = Session("mDueJobPlanning")
        mSelectDueJob = Session("mSelectDueJob")
        mSelectDueJobs = Session("mSelectDueJobs")
        mDueLimits = Session("mDueLimits")

        mIsNewDueReportObjectBindingRequired = Session("mIsNewDueReportObjectBindingRequired")
        mrptDueReport = Session("mrptDueReportForOnlyDueReport")

    End Sub
    Private Sub SetSession()
        Session("mDueJobPlanning") = mDueJobPlanning
        Session("mSelectDueJob") = mSelectDueJob
        Session("mSelectDueJobs") = mSelectDueJobs
        Session("mDueLimits") = mDueLimits
    End Sub
    Private Sub SetTitle()
        'If mIsNewDueReportObjectBindingRequired = "True" Then
        lblResult.Text = "List of Due Jobs as per criteria :" & mrptDueReport.Count & " Record(s) found."
        'Else
        '    lblResult.Text = "List of Due Jobs as per criteria :" & mSelectDueJobs.Count & " Record(s) found."
        'End If
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
            If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "SPZ") And values.Length > 1 Then '' SPZ Code added by Saylee on 13-Jun-2022  Deccan Code added by Vikrant On 16-Feb-2021
                MSGBoxCtrl.Show("Selection Alert!", "Multiple Jobs can not be added in single WO.", "", MsgBoxStyle.OkOnly, "RestrictMultJobs")
                Exit Sub
            End If
            For Each value As String In values



                builder.Append("<br/>")
                builder.Append(value)
                checkedIds.Add(value)
                'If Not mIsNewDueReportObjectBindingRequired = "True" Then 'if condition Added by vikrant on 19-May-2021 
                'mSelectDueJobs(New Guid(value)).IsSelected = True
                'End If


            Next
            'If Not mIsNewDueReportObjectBindingRequired = "True" Then 'if condition Added by vikrant on 19-May-2021 
            '    For i As Integer = 0 To mSelectDueJobs.Count - 1
            '        If mSelectDueJobs(i).IsSelected = True And Array.IndexOf(values, mSelectDueJobs(i).ID.ToString) = -1 Then
            '            mSelectDueJobs(i).IsSelected = False
            '        End If
            '    Next
            'End If

            'For Each value As String In values
            '    builder.Append("<br/>")
            '    builder.Append(value)
            '    checkedIds.Add(value)
            '    ' mMaintenanceTask.MaintenanceTaskDetails.Remove(New Guid(value), "")
            '    mSelectDueJobs(New Guid(value)).IsSelected = True
            '    'If mSelectDueJobs.Contains(New Guid(value)) Then
            '    '    mSelectDueJobs(New Guid(value)).IsSelected = True
            '     If
            'Next
            'values = ""
            checkString = Nothing
        End If

        'If mIsNewDueReportObjectBindingRequired = "True" Then 'if condition Added by vikrant on 19-May-2021 
        For i As Integer = mDueJobPlanning.DueJobPlanningItems.Count - 1 To 0 Step -1
            If Not checkedIds.Contains(mDueJobPlanning.DueJobPlanningItems(i).MaintenanceActivityID.ToString) Then
                mDueJobPlanning.DueJobPlanningItems.Remove(mDueJobPlanning.DueJobPlanningItems(i).MaintenanceActivityID, "")
            End If
        Next
        'Else
        '    For i As Integer = 0 To mSelectDueJobs.Count - 1
        '        If mSelectDueJobs(i).IsSelected = False Then
        '            If mDueJobPlanning.DueJobPlanningItems.Contains(mSelectDueJobs.Item(i).ID, "") Then
        '                mDueJobPlanning.DueJobPlanningItems.Remove(mSelectDueJobs.Item(i).ID, "")
        '            End If
        '        End If
        '    Next

        'End If

        Session("mDueJobPlanning") = mDueJobPlanning
        Session("mSelectDueJobs") = mSelectDueJobs
    End Sub
    Private Sub setObjectNew()
        Dim i As Integer = 0

        While i < mrptDueReport.Count
            If checkedIds.Contains(mrptDueReport(i).ID.ToString) Then
                If mDueJobPlanning.DueJobPlanningItems.Contains(mrptDueReport(i).ID, "") = False Then
                    Dim Description As String = ""
                    Dim LastWOJobDesc As String = ""
                    Dim AssemblyTypeWithPosition As String = ""
                    Dim CurrentItem As rptDueReportForOnlyDueReport.rptDueReportForOnlyDueReportInfo
                    If mrptDueReport(i).OnAssemblyOrComponent = "Assembly" Then
                        CurrentItem = mrptDueReport(i)
                        With CurrentItem
                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
                                Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString & IIf(.Position = "", " " & .DataType, " Position: " & .Position & " " & .DataType) & " on Assembly- " & .MaintenanceEvent & CStr(IIf(mrptDueReport(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mrptDueReport(i).JobDescription.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.: " & .Reference.ToString, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
                                Description = .DataType & " on Assembly-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                                   AppSettings("ClientCode") = "APFT" Or
                                   AppSettings("ClientCode") = "AAP" Then
                                Dim AssemblyType As String = CStr(IIf(.AssemblyTypeName.ToString = "Airframe", "Aircraft: ", .AssemblyTypeName.ToString & ": "))
                                Description = CStr(IIf(.AssemblyModel <> "", vbCrLf & AssemblyType & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString + " ", "")) & IIf(.Position = "", "", " Position: " & .Position) & vbCrLf & .DataType & " on Assembly- " & .MaintenanceEvent & CStr(IIf(mrptDueReport(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mrptDueReport(i).JobDescription.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & " Ref.: " & .Reference.ToString, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                                Description = CStr(IIf(mrptDueReport(i).Zone.ToString <> "", "System: " & mrptDueReport(i).Zone.ToString & vbCrLf, "")) & CStr(IIf(mrptDueReport(i).JobDescription.ToString <> "", "Description: " & mrptDueReport(i).JobDescription.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(mrptDueReport(i).Note <> "", vbCrLf & "Note: " & mrptDueReport(i).Note, "")) & CStr(IIf(mrptDueReport(i).Remark <> "", vbCrLf & "Remark: " & mrptDueReport(i).Remark, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "MEL" Then 'As Model and Serial no not required
                                Description = .CodeType & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))

                            Else

                                If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then
                                    Description = .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                                Else
                                    Description = .DataType & " on Assembly - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                                End If
                            End If
                        End With
                        If AppSettings("setWOJobDescriptionFromPreviousSimilarWO") = "True" Then
                            mFetchLastnWOJobDescription = FetchLastnWOJobDescription.GetLastnWOJobDescription(mrptDueReport(i).ID, mrptDueReport(i).StatusMasterID, mrptDueReport(i).AssemblyCompID, mrptDueReport(i).OnAssemblyOrComponent, mrptDueReport(i).DataType, mDueJobPlanning.DueJobPlanningDate)
                            LastWOJobDesc = mFetchLastnWOJobDescription.WOJobDescription
                        End If
                    ElseIf mrptDueReport(i).OnAssemblyOrComponent = "Component" Then
                        CurrentItem = mrptDueReport(i)
                        With CurrentItem
                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "RAL" Then
                                Description = "Maintenance On-" & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString & " Position:" & .AssemblyPositionInComp & "<br/>" & .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.PartName <> "", "Part:" & .PartName & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "FG" Then
                                Description = .DataType & " on Component-" & .MaintenanceEvent & CStr(IIf(.AssemblyModel <> "", "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.PartName <> "", "Part:" & .PartName & " S/N:" & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", "Directive No.:" & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", " Ref.:" & .Reference.ToString, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                                   AppSettings("ClientCode") = "APFT" Or
                                   AppSettings("ClientCode") = "AAP" Then
                                Dim AssemblyType As String = CStr(IIf(.AssemblyTypeName.ToString = "Airframe", "Aircraft: ", .AssemblyTypeName.ToString & ": "))
                                Description = CStr(IIf(.AssemblyModel <> "", vbCrLf & AssemblyType & .AssemblyModel & " S/N: " & .AssemblySerialNo.ToString + " ", "")) & vbCrLf & .DataType & " on Component- " & .MaintenanceEvent & CStr(IIf(mrptDueReport(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mrptDueReport(i).JobDescription.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "P/N: " & .PartName, "")) & CStr(IIf(.CompSerialNo <> "", " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & " Ref.: " & .Reference.ToString, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Heligo" Then
                                Description = CStr(IIf(mrptDueReport(i).Zone.ToString <> "", "System: " & mrptDueReport(i).Zone.ToString & vbCrLf, "")) & CStr(IIf(mrptDueReport(i).PartDescription.ToString <> "", "Nomenclature: " & mrptDueReport(i).PartDescription.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "P/N: " & .PartName, "")) & CStr(IIf(.CompSerialNo <> "", " S/N: " & .CompSerialNo, "")) & CStr(IIf(.Position <> "", " Position: " & .Position, "")) & CStr(IIf(mrptDueReport(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mrptDueReport(i).JobDescription.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(mrptDueReport(i).Note <> "", vbCrLf & "Note: " & mrptDueReport(i).Note, "")) & CStr(IIf(mrptDueReport(i).Remark <> "", vbCrLf & "Remark: " & mrptDueReport(i).Remark, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "SAA" Then
                                Description = .DataType & " on Component - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Position <> "", vbCrLf & "Pos.: " & .Position, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "MEL" Then 'As Model and Serial no not required
                                Description = .CodeType & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                            Else
                                If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then
                                    Description = .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                                Else
                                    Description = .DataType & " on Component - " & .CodeType & CStr(IIf(.AssemblyModel <> "", vbCrLf & "Model:" & .AssemblyModel & " S/N:" & .AssemblySerialNo.ToString, "")) & CStr(IIf(.Reference.ToString <> "", vbCrLf & "Ref.: " & .Reference.ToString, "")) & CStr(IIf(.PartName <> "", vbCrLf & "Part: " & .PartName & " S/N: " & .CompSerialNo.ToString, "")) & CStr(IIf(.Number.ToString <> "", vbCrLf & "Directive No.: " & .Number.ToString, ""))
                                End If
                            End If
                        End With
                        If AppSettings("setWOJobDescriptionFromPreviousSimilarWO") = "True" Then
                            mFetchLastnWOJobDescription = FetchLastnWOJobDescription.GetLastnWOJobDescription(mrptDueReport(i).ID, mrptDueReport(i).StatusMasterID, mrptDueReport(i).AssemblyCompID, mrptDueReport(i).OnAssemblyOrComponent, mrptDueReport(i).DataType, mDueJobPlanning.DueJobPlanningDate)
                            LastWOJobDesc = mFetchLastnWOJobDescription.WOJobDescription
                        End If
                    End If
                    Description = Description & CStr(IIf(mrptDueReport(i).JobDescription.ToString <> "", vbCrLf & "Description: " & mrptDueReport(i).JobDescription.ToString, "")) & CStr(IIf(mrptDueReport(i).Note <> "", vbCrLf & "Note: " & mrptDueReport(i).Note, ""))
                    If LastWOJobDesc <> "" Then Description = LastWOJobDesc

                    mDueJobPlanning.DueJobPlanningItems.Add(mDueJobPlanning.ID)
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.MaintenanceActivityID = mrptDueReport(i).ID
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.ActivityTypeID = mrptDueReport.Item(i).ActivityTypeID
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.Description = Description
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.TaskNo = mrptDueReport.Item(i).TaskNo
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.EstimatedHours = mrptDueReport(i).EstimatedHours
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.Number = mrptDueReport.Item(i).Number
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.MasterCode = mrptDueReport.Item(i).MasterCode
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.FrequencyValue = mrptDueReport.Item(i).FrequencyValue
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.DueAsOf = mrptDueReport.Item(i).DueAsof2
                    mDueJobPlanning.DueJobPlanningItems.CurrentItem.PeriodIDWithDecValue = mrptDueReport.Item(i).PeriodIDWithDecValue

                    If mrptDueReport(i).OnAssemblyOrComponent = "Assembly" Then
                        mDueJobPlanning.DueJobPlanningItems.CurrentItem.OnTypeID = 1
                    ElseIf mrptDueReport(i).OnAssemblyOrComponent = "Component" Then
                        mDueJobPlanning.DueJobPlanningItems.CurrentItem.OnTypeID = 2
                    End If
                    If mrptDueReport.Item(i).DataType = "Servicing" Then
                        mDueJobPlanning.DueJobPlanningItems.CurrentItem.MonitorTypeID = 1
                    ElseIf mrptDueReport(i).DataType = "Inspection" Then
                        mDueJobPlanning.DueJobPlanningItems.CurrentItem.MonitorTypeID = 2
                    ElseIf mrptDueReport(i).DataType = "Modification" Then
                        mDueJobPlanning.DueJobPlanningItems.CurrentItem.MonitorTypeID = 3
                    End If
                End If
            End If

            i = i + 1
        End While
        Session("mDueJobPlanning") = mDueJobPlanning

        'SetEstimatedValues()
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub addAttributes()

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
        mDueLimits = DueLimits.GetDueLimits(mDueJobPlanning.MachineID)
        dgDuePeriod.DataSource = mDueLimits
        'If mIsNewDueReportObjectBindingRequired = "True" Then
        mrptDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString, mDueJobPlanning.RegNo, IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked, Note:=txtNote.Text.Trim)
        mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
        If Not mrptDueReport Is Nothing Then
            For Each Child As rptDueReportForOnlyDueReport.rptDueReportForOnlyDueReportInfo In mrptDueReport
                If mDueJobPlanning.DueJobPlanningItems.Contains(Child.ID, "") Then
                    checkedIds.Add(Child.ID.ToString)
                End If
            Next
        End If
        Session("mrptDueReportForOnlyDueReport") = mrptDueReport
        dgDueJob.DataSource = mrptDueReport
        'Else
        '    mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(txtAsOnDate.Text.ToString, mDueLimits, mDueJobPlanning.MachineID.ToString, 0)
        '    If Not mSelectDueJobs Is Nothing Then
        '        For Each Child As SelectDueJob In mSelectDueJobs
        '            Child.IsSelected = mDueJobPlanning.DueJobPlanningItems.Contains(Child.ID, "")
        '            'this.Request.Form[this.txtName.UniqueID]
        '            '= Request.Form("chkSelect")
        '            If mDueJobPlanning.DueJobPlanningItems.Contains(Child.ID, "") Then
        '                checkedIds.Add(Child.ID.ToString)
        '            End If
        '        Next
        '    End If
        '    mSortedDueJobList = (From c As SelectDueJob In mSelectDueJobs
        '                         Order By c.MinimumRemainingValue
        '                         Select c).ToList
        '    dgDueJob.DataSource = mSortedDueJobList
        'End If

        Session("mDueLimits") = mDueLimits
        'Session("mSelectDueJobs") = mSelectDueJobs
        If (ConfigurationManager.AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or
          ConfigurationManager.AppSettings("ClientCode") = "YA" Or ConfigurationManager.AppSettings("ClientCode") = "TA" Or
          ConfigurationManager.AppSettings("ClientCode") = "UHPL" Or ConfigurationManager.AppSettings("ClientCode") = "Novo" Or
          ConfigurationManager.AppSettings("ClientCode") = "ADeccan" Or ConfigurationManager.AppSettings("ClientCode") = "Heligo") Then  'Added By Prashant 24-Jun-2013 BA24062013
            dgDueJob.Columns(12).HeaderText = "Due As Of Airframe"
        Else
            dgDueJob.Columns(12).HeaderText = "Due As Of Assembly"
        End If

        If (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowMaintenanceForNewClients") = "True") Then
            dgDueJob.Columns(2).HeaderText = "Task No./Directive No."
        Else
            dgDueJob.Columns(2).HeaderText = "Code"
        End If


        DataBind()



        'If mIsNewDueReportObjectBindingRequired = "True" Then
        'If mrptDueReport.Count > 10 Then btnDoneTop.Visible = True
        'If mrptDueReport.Count > 10 Then btnBackTop.Visible = True
        ' btnDone.Enabled = IIf(mrptDueReport.Count > 0, True, False)
        btnDoneTop.Enabled = IIf(mrptDueReport.Count > 0, True, False)
        'Else
        'If mSelectDueJobs.Count > 10 Then btnDoneTop.Visible = True
        'If mSelectDueJobs.Count > 10 Then btnBackTop.Visible = True
        ' btnDone.Enabled = IIf(mSelectDueJobs.Count > 0, True, False)
        ' btnDoneTop.Enabled = IIf(mSelectDueJobs.Count > 0, True, False)
        'End If

        'For i As Integer = 0 To dgDueJob.Rows.Count - 1
        '    Dim checbox As CheckBox = CType(Me.dgDueJob.Rows(i).FindControl("chkSelect"), CheckBox) 'dgDueJob.Rows(i).FindControl("chkSelect")
        '    '  Dim WONumber As String = (DataBinder.Eval(dgDueJob.Rows(i).DataItem, "WONumber"))
        '    ' Dim PlannedDetails As String = (DataBinder.Eval(dgDueJob.Rows(i).DataItem, "PlannedDetails"))
        '    If mrptDueReport(i).WONumber <> "" Or mrptDueReport(i).PlannedDetails <> "" Then
        '        checbox.Enabled = False
        '    Else
        '        checbox.Enabled = True
        '    End If

        'Next
    End Sub
    ''Private Function CustomValidate1() As Boolean
    ''    AddJobs()
    ''    Dim strMSG As String = ""
    ''    Dim i As Integer = 0
    ''    While i < mSelectDueJobs.Count
    ''        If mSelectDueJobs.Item(i).IsDirty = True Then
    ''            If mSelectDueJobs.Item(i).IsSelected = True Then
    ''                mIsSelected = True
    ''                If mDueJobPlanning.DueJobPlanningItems.Contains(mSelectDueJobs.Item(i).ID, "") = True Then
    ''                    strMSG = strMSG + " Duplicate Scheduled Job " + mSelectDueJobs.Item(i).LogBook + " " + mSelectDueJobs.Item(i).DataType + "<BR>"
    ''                End If
    ''            End If
    ''        End If
    ''        i = i + 1
    ''    End While
    ''    Session("mDueJobPlanning") = mDueJobPlanning
    ''    If strMSG.Trim <> "" Then
    ''        cvControlValidator.ErrorMessage = strMSG
    ''        cvControlValidator.IsValid = False
    ''        Return False
    ''    End If
    ''    Return True
    ' Function
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

        If Not mDueJobPlanning.IsValid Then
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
            'txtAsOnDate.Text = mDueJobPlanning.WODateFormatted
            txtAsOnDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
        txtAsOnDate.Enabled = False
        If Not IsPostBack Then
            DataFieldBind()
            SetTitle()
            UpnlGrid.Update()

        End If
    End Sub
    Private Sub imgFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgFindNow.Click, chkZeroFrequency.CheckedChanged
        If IsValid Then
            SetGridObject()
            If mDueLimits.IsDirty Then
                mDueLimits.Save()
            End If

            dgDueJob.PageIndex = 0
            'If mIsNewDueReportObjectBindingRequired = "True" Then
            mrptDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString, mDueJobPlanning.RegNo, IsZeroFreqRecordsToBeShown:=chkZeroFrequency.Checked, Note:=txtNote.Text.Trim)
            mrptDueReport.Sort("RemainingValueForSorting", ComponentModel.ListSortDirection.Ascending)
            Session("mrptDueReportForOnlyDueReport") = mrptDueReport
            'Else
            'mSelectDueJobs = SelectDueJobs.GetSelectDueJobs(txtAsOnDate.Text.ToString, mDueLimits, mDueJobPlanning.MachineID.ToString, 0, chkZeroFrequency.Checked)
            'End If


            'If Not mSelectDueJobs Is Nothing Then
            '    For Each Child As SelectDueJob In mSelectDueJobs
            '        Child.IsSelected = mDueJobPlanning.DueJobPlanningItems.Contains(Child.ID, "")
            '        If mDueJobPlanning.DueJobPlanningItems.Contains(Child.ID, "") Then
            '            checkedIds.Add(Child.ID.ToString)
            '        End If
            '    Next
            'End If

            'Added By Vikrant On 17-Nov-2014 For 
            Dim mJobs
            'If mIsNewDueReportObjectBindingRequired = "True" Then
            'mJobs = (From c As rptDueReport.rptDueReportInfo In mrptDueReport
            '         Where (c.Note.ToUpper().Contains(txtNote.Text.ToUpper))
            '         Order By c.MinimumRemainingValue
            '         Select c).ToList
            mJobs = mrptDueReport
            'Else
            '    mJobs = (From c As SelectDueJob In mSelectDueJobs
            '             Where (c.Note.ToUpper().Contains(txtNote.Text.ToUpper))
            '             Order By c.MinimumRemainingValue
            '             Select c).ToList
            'End If
            '   mSelectDueJobs(0).ATACode.ToString()

            dgDueJob.DataSource = mJobs
            'Session("mSelectDueJobs") = mSelectDueJobs
            mDueLimits = Session("mDueLimits")
            dgDuePeriod.DataSource = mDueLimits
            If (ConfigurationManager.AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or
          ConfigurationManager.AppSettings("ClientCode") = "YA" Or ConfigurationManager.AppSettings("ClientCode") = "TA" Or
          ConfigurationManager.AppSettings("ClientCode") = "UHPL" Or ConfigurationManager.AppSettings("ClientCode") = "Novo" Or
          ConfigurationManager.AppSettings("ClientCode") = "ADeccan" Or ConfigurationManager.AppSettings("ClientCode") = "Heligo") Then  'Added By Prashant 24-Jun-2013 BA24062013
                dgDueJob.Columns(12).HeaderText = "Due As Of Airframe"
            Else
                dgDueJob.Columns(12).HeaderText = "Due As Of Assembly"
            End If

            If (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowMaintenanceForNewClients") = "True") Then
                dgDueJob.Columns(2).HeaderText = "Task No./Directive No."
            Else
                dgDueJob.Columns(2).HeaderText = "Code"
            End If

            DataBind()
            lblResult.Text = "List of Due Jobs as per criteria :" & mJobs.Count & " Record(s) found."
            UpnlResult.Update()
            UpnlGrid.Update()
        End If
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoneTop.Click

        Dim checkString = Request.Form("chkSelect")
        If Not checkString Is Nothing Then
            Dim values As String() = checkString.Split(","c)
            For Each value As String In values
                If mrptDueReport(New Guid(value)).WONumber <> "" Then
                    MSGBoxCtrl.Show("Alert..!!", IIf(mrptDueReport(New Guid(value)).TaskNo <> "", dgDueJob.Columns(2).HeaderText + ":  " + mrptDueReport(New Guid(value)).TaskNo, "") + " Work Order already created. Please select other due record for planning", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mrptDueReport(New Guid(value)).RemainingValueForSorting < 0 Then
                    MSGBoxCtrl.show("Alert..!!", IIf(mrptDueReport(New Guid(value)).TaskNo <> "", dgDueJob.Columns(2).HeaderText + ":  " + mrptDueReport(New Guid(value)).TaskNo, "") + " Overdue Record cannot be planned. Please select other due record for planning", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                If mrptDueReport(New Guid(value)).PlannedWODetails <> "" Then
                    MSGBoxCtrl.Show("Alert..!!", IIf(mrptDueReport(New Guid(value)).TaskNo <> "", dgDueJob.Columns(2).HeaderText + ":  " + mrptDueReport(New Guid(value)).TaskNo, "") + " Due record already planned. Please select other due record for planning", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Next
        End If



        AddJobs()
        'If mIsNewDueReportObjectBindingRequired = "True" Then  
        setObjectNew()
        'Else  
        '    setObject()


        checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select atleast one scheduled job.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            Dim values As String() = checkString.Split(","c)
            If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") And values.Length > 1 Then
                Exit Sub
            End If
            'Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If

            Response.Redirect("wfDueJobPlanning_Ajax.aspx?BackPage=index.aspx")
        End If

    End Sub
    Private Sub dgDueJob_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDueJob.PageIndexChanged
        dgDueJob.PageIndex = e.NewPageIndex
        'If mIsNewDueReportObjectBindingRequired = "True" Then
        dgDueJob.DataSource = mrptDueReport
        Session("mrptDueReportForOnlyDueReport") = mrptDueReport
        'Else
        '    dgDueJob.DataSource = mSelectDueJobs
        '    Session("mDueJobPlanningDefferedJobs") = mSelectDueJobs
        'End If
        dgDueJob.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            If mDueJobPlanning.DueJobPlanningItems.CurrentItem.IsNew And Session("EditDueJobPlanningItem") = False Then mDueJobPlanning.DueJobPlanningItems.Remove(mDueJobPlanning.DueJobPlanningItems.CurrentItem)
            Session.Remove("EditDueJobPlanningItem")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

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
    'Private Sub dgDueJob_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDueJob.RowDataBound
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        Dim WONumber As String = (DataBinder.Eval(e.Row.DataItem, "WONumber"))
    '        Dim PlannedDetails As String = (DataBinder.Eval(e.Row.DataItem, "PlannedDetails"))
    '        Dim chkslect As CheckBox = DirectCast(e.Row.FindControl("chkSelect"), CheckBox) 'CType(e.Row.FindControl("chkSelect"), CheckBox)
    '        'Dim chkslect As CheckBox = CType(sender, CheckBox)

    '        If WONumber <> "" Or PlannedDetails <> "" Then

    '            e.Row.BackColor = Color.Silver
    '        Else
    '            ' chkslect.Enabled = True
    '        End If
    '    End If
    'End Sub

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