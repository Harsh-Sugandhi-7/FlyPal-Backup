
'Created By     :   Saylee
'Dated          :   20-Aug-2015

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic

Public Class wfTaskListForAuditSchedule_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditTaskList As AuditTaskList
    Public mAuditTask As AuditTask
    Protected mAudit As Audit
    Protected mAuditSchedule As AuditSchedule
    Protected mAuditExecution As AuditExecution

    Dim AuditStandardID As String
    Dim Type As Int16
    Public mAuditCategoryList As AuditCategoryList

    Private checkedIds As New List(Of String)()
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAudit = Session("mAudit")
        mAuditSchedule = Session("mAuditSchedule")
        mAuditExecution = Session("mAuditExecution")
        mAuditTaskList = Session("mAuditTaskList")
        Type = Session("Type")
    End Sub
    Private Sub SetSession()
        Session("mAudit") = mAudit
        Session("mAuditSchedule") = mAuditSchedule
        Session("mAuditExecution") = mAuditExecution
        Session("mAuditTaskList") = mAuditTaskList
        Session("Type") = Type
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "Task List : " & mAuditTaskList.Count & " Record(s) found."
        If Type = 1 Then
            lblTitle.Text = "Task List For Schedule"
        ElseIf Type = 2 Then
            lblTitle.Text = "Task List For Compliance"
        ElseIf Type = 3 Then
            lblTitle.Text = "Task List For Audit"
        End If

        'btnOKTop.Visible = mAuditTaskList.Count > 15
        'btnBackTop.Visible = mAuditTaskList.Count > 15
    End Sub
    Private Sub setAuditObject()
        Dim i As Integer = 0
        While i < mAuditTaskList.Count
            If mAuditTaskList.Item(i).IsSelected = True Then
                If Not mAudit.AuditMasterTasks.Contains(mAuditTaskList.Item(i).ID) Then
                    mAudit.AuditMasterTasks.Add(mAudit.ID)
                    mAudit.AuditMasterTasks.CurrentItem.SrNo = mAudit.AuditMasterTasks.CurrentIndex + 1
                    mAudit.AuditMasterTasks.CurrentItem.AuditTaskID = mAuditTaskList.Item(i).ID
                End If
            Else
                mAudit.AuditMasterTasks.Remove(mAuditTaskList.Item(i).ID, "")
            End If
            'End If
            i = i + 1
        End While
    End Sub
    Private Sub setScheduleObject()
        Dim i As Integer = 0
        While i < mAuditTaskList.Count
            'If mAuditTaskList.Item(i).IsDirty = True Then
            If mAuditTaskList.Item(i).IsSelected = True Then
                If Not mAuditSchedule.AuditScheduleTasks.Contains(mAuditTaskList.Item(i).ID) Then
                    mAuditSchedule.AuditScheduleTasks.Add(mAuditSchedule.ID)
                    mAuditSchedule.AuditScheduleTasks.CurrentItem.SrNo = mAuditSchedule.AuditScheduleTasks.CurrentIndex + 1
                    mAuditSchedule.AuditScheduleTasks.CurrentItem.AuditTaskID = mAuditTaskList.Item(i).ID
                End If
            Else
                mAuditSchedule.AuditScheduleTasks.Remove(mAuditTaskList.Item(i).ID, "")
            End If
            'End If
            i = i + 1
        End While
    End Sub
    Private Sub setExecutionObject()
        Dim i As Integer = 0
        While i < mAuditTaskList.Count
            'If mAuditTaskList.Item(i).IsDirty = True Then
            If mAuditTaskList.Item(i).IsSelected = True Then
                If Not mAuditExecution.AuditExecutionTasks.Contains(mAuditTaskList.Item(i).ID, "") Then
                    mAuditExecution.AuditExecutionTasks.Add(mAuditExecution.ID)
                    mAuditExecution.AuditExecutionTasks.CurrentItem.AuditTaskID = mAuditTaskList.Item(i).ID
                    mAuditExecution.AuditExecutionTasks.CurrentItem.SrNo = mAuditExecution.AuditExecutionTasks.CurrentIndex + 1
                    If AppSettings("ClientCode") = "SAA" Or AppSettings("ClientCode") = "ABD" Then 'Added By Prashant on 28-Jun-2022, ABD code addedby saylee on 28-Sep-2022 as they need satisfactory
                        mAuditExecution.AuditExecutionTasks.CurrentItem.TaskStatusID = 1 ''As Client need Satisfactory as defualt selected
                    End If
                End If
            Else
                mAuditExecution.AuditExecutionTasks.Remove(mAuditTaskList.Item(i).ID, "", "")
            End If
            'End If
            i = i + 1
        End While
    End Sub
    Private Sub setTasks()
        Dim i As Integer
        While i < mAuditTaskList.Count

            If Type = 1 Then 'AuditSchedule
                If mAuditSchedule.AuditScheduleTasks.Contains(mAuditTaskList.Item(i).ID) = True Then
                    mAuditTaskList.Item(i).IsSelected = True
                Else
                    mAuditTaskList.Item(i).IsSelected = False
                End If
                i = i + 1
            ElseIf Type = 2 Then 'AuditExecution
                If mAuditExecution.AuditExecutionTasks.Contains(mAuditTaskList.Item(i).ID, "") = True Then
                    mAuditTaskList.Item(i).IsSelected = True
                Else
                    mAuditTaskList.Item(i).IsSelected = False
                End If
                i = i + 1
            ElseIf Type = 3 Then 'Audit
                If mAudit.AuditMasterTasks.Contains(mAuditTaskList.Item(i).ID) = True Then
                    mAuditTaskList.Item(i).IsSelected = True
                Else
                    mAuditTaskList.Item(i).IsSelected = False
                End If
                i = i + 1
            End If
        End While
    End Sub
    Private Sub setSelectedTasks()
        'Dim item As GridViewRow
        'Dim chkBox As CheckBox
        'Dim Recordno, PageItems As Integer
        'Dim i As Integer
        'PageItems = dgTaskList.Rows.Count - 1
        '' Set Selected Notes value  
        'For i = 0 To PageItems
        '    Recordno = i + dgTaskList.PageSize * dgTaskList.PageIndex
        '    item = dgTaskList.Rows(i)
        '    chkBox = CType(item.FindControl("chkSelect"), CheckBox)
        '    mAuditTaskList(Recordno).IsSelected = chkBox.Checked
        'Next

        Dim builder = New StringBuilder()
        builder.Append("You have selected the following checks :<br/>")
        ' get the selected checkboxes from the form data
        Dim checkString = Request.Form("chkSelect")
        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            ' we'll need a split to get the individual ids
            Dim values = checkString.Split(","c)
            For Each value As String In values
                builder.Append("<br/>")
                builder.Append(value)
                checkedIds.Add(value)
                If mAuditTaskList.Contains(New Guid(value)) Then
                    mAuditTaskList(New Guid(value)).IsSelected = True
                End If
            Next
            values = ""
            checkString = Nothing
        End If

    
        Session("mAuditTaskList") = mAuditTaskList
        Session("mAuditSchedule") = mAuditSchedule
        Session("mAuditExecution") = mAuditExecution
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Type = 1 Then
            AuditStandardID = mAuditSchedule.AuditStandardID.ToString
        ElseIf Type = 2 Then
            AuditStandardID = mAuditExecution.AuditStandardID.ToString
        ElseIf Type = 3 Then
            AuditStandardID = mAudit.AuditStandardID.ToString
        End If

        mAuditTaskList = AuditTaskList.GetAuditTaskList(New Guid(AuditStandardID))
        Session("mAuditTaskList") = mAuditTaskList
        setTasks()
        dgTaskList.DataSource = mAuditTaskList

        mAuditCategoryList = AuditCategoryList.GetAuditCategoryList(New Guid(AuditStandardID), "(SELECT)")

        cmbTaskCategorySearch.DataSource = mAuditCategoryList
        cmbTaskCategorySearch.DataBind()
        DataBind()
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32)
        cmbTaskCategorySearch.Visible = IIf(SearchIndex = 1, True, False)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            Type = CType(Request.QueryString("AType"), Int16)
            Session("Type") = Type
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        dgTaskList.PageIndex = 0
        'mAudit  SetGrid()  'Added By Utkarsh On 4-May-2011
        'mTaskList = AuditTaskList.GetAuditTaskList("", cmbTaskCategorySearch.SelectedValue.ToString, cmbDepartmentListSearch.SelectedValue.ToString, AuditStandardID.ToString)
        If Type = 1 Then
            mAuditTaskList = AuditTaskList.GetAuditTaskList(, cmbTaskCategorySearch.SelectedValue.ToString, , mAuditSchedule.AuditStandardID.ToString)
        ElseIf Type = 2 Then
            mAuditTaskList = AuditTaskList.GetAuditTaskList(, cmbTaskCategorySearch.SelectedValue.ToString, , mAuditExecution.AuditStandardID.ToString)
        ElseIf Type = 3 Then
            mAuditTaskList = AuditTaskList.GetAuditTaskList(, cmbTaskCategorySearch.SelectedValue.ToString, , mAudit.AuditStandardID.ToString)
        End If
        Session("TaskCategoryID") = cmbTaskCategorySearch.SelectedValue.ToString
        setTasks()
        dgTaskList.DataSource = mAuditTaskList
        Session("mAuditTaskList") = mAuditTaskList
        dgTaskList.DataBind()
        SetTitle()
        lblResult.Text = "Task List: " & mAuditTaskList.Count & " Record(s) Found."

        upnlResult.Update()
        upnlGrid.Update()
        upnlButtonsTop.Update()
    End Sub
    Private Sub hdnimgBtnTaskMasterChapter_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnTaskMasterChapter.Click
        DataFieldBind()
        upnlGrid.Update()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbTaskCategorySearch.SelectedIndex = 0
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
            Session("Search") = "True"
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        ' Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
      
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click, btnOKTop.Click
        'If mAuditSchedule.AuditScheduleTasks.Contains(mAuditSchedule.AuditScheduleTasks.CurrentItem.TaskID) Then
        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, " AuditSchedule AuditCategory.", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfAuditScheduleTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
        '    msg1.Show()
        '    mAuditSchedule.CancelEdit()
        '    Exit Sub
        'Else
        setSelectedTasks()
        If Type = 1 Then
            setScheduleObject()
            'mAuditSchedule.ApplyEdit()
            Session("mAuditSchedule") = mAuditSchedule
            ' Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        ElseIf Type = 2 Then
            setExecutionObject()
            Session("mAuditExecution") = mAuditExecution
            ' Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        ElseIf Type = 3 Then
            setAuditObject()
            Session("mAudit") = mAudit
        End If
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub imgTask_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgTask.Click
        Dim AuditStandardName As String
        If Type = 1 Then
            AuditStandardID = mAuditSchedule.AuditStandardID.ToString
            AuditStandardName = mAuditSchedule.AuditStandardName
        ElseIf Type = 2 Then
            AuditStandardID = mAuditExecution.AuditStandardID.ToString
            AuditStandardName = mAuditExecution.AuditStandardName
        ElseIf Type = 3 Then
            AuditStandardID = mAudit.AuditStandardID.ToString
            AuditStandardName = mAudit.AuditStandardName
        End If
        Session("AuditStandardName") = AuditStandardName
        'Response.Redirect("wfTask.aspx?BackPage2=wfTaskListForAuditSchedule.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&AuditStandardID=" & AuditStandardID & "&Type=" & Type)
        Session("AuditStandardID") = AuditStandardID
        Session("AType") = Type
        Session.Remove("mFileAttach") 'Ajay 17-11-2023
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskMasterWindow", "OpenTaskMasterWindow()", True)
    End Sub
#End Region

#Region "Checked Selection"
    Public Function NumeroChequeInclus(ByVal numero As String) As String

        If Not mAuditTaskList Is Nothing Then
            For i As Integer = 0 To mAuditTaskList.Count - 1
                If mAuditTaskList.Item(i).IsSelected = True Then
                    checkedIds.Add(mAuditTaskList.Item(i).ID.ToString)
                End If
            Next
   
        End If
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
        Return String.Empty
    End Function
#End Region

End Class