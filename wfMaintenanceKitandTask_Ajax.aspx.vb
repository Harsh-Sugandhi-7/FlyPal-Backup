Imports System.Collections.Generic
Imports System.Text
Imports System.Web.Services
Public Class wfMaintenanceKitandTask_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mMaintenanceKit As MaintenanceKit
    Public mMaintenanceTask As MaintenanceTask
    Public mChild As Integer 'Added by Saylee on 23-July-2013 for BA22072013 
    Private checkedIds As New List(Of String)()
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mMaintenanceKit = CType(Session("mMaintenanceKit"), MaintenanceKit)
        mMaintenanceTask = CType(Session("mMaintenanceTask"), MaintenanceTask)
        mChild = CType(Session("mChild"), Integer) 'Added by Saylee on 23-July-2013 for BA22072013 	
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMaintenanceKit")
        Session.Remove("mMaintenanceTask")
        'Session.Remove("mChild") 'Added by Saylee on 23-July-2013 for BA22072013 
    End Sub
    Private Sub DeleteRecordKit(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteKit")
        mMaintenanceKit.MaintenanceKitDetails.CurrentIndex = Index
        Session("mMaintenanceKit") = mMaintenanceKit
    End Sub
    Private Sub DeleteRecordTask(ByVal Index As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteTask")
        mMaintenanceTask.MaintenanceTaskDetails.CurrentIndex = Index
        Session("mMaintenanceTask") = mMaintenanceTask
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteKit" Then
                        Try
                            mMaintenanceKit.MaintenanceKitDetails.Remove(mMaintenanceKit.MaintenanceKitDetails.CurrentItem)
                            mMaintenanceKit.ApplyEdit()
                            mMaintenanceKit.Save()
                            Session("mMaintenanceKit") = mMaintenanceKit
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                DataFieldBind()
                                upnlKitList.Update()
                                Exit Sub
                            End If
                        Finally
                            DataFieldBind()
                            upnlKitList.Update()
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteTask" Then
                        Try
                            mMaintenanceTask.MaintenanceTaskDetails.Remove(mMaintenanceTask.MaintenanceTaskDetails.CurrentItem)
                            mMaintenanceTask.ApplyEdit()
                            mMaintenanceTask.Save()
                            Session("mMaintenanceTask") = mMaintenanceTask
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                DataFieldBind()
                                upnlTaskList.Update()
                                Exit Sub
                            End If
                        Finally
                            DataFieldBind()
                            upnlTaskList.Update()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteKit" Then
                        DataFieldBind()
                        upnlKitList.Update()
                    End If
                    If MSGBoxCtrl.Sender = "DeleteTask" Then
                        DataFieldBind()
                        upnlTaskList.Update()
                    End If
            End Select
        End If
    End Sub
    Private Sub SetPage()
        If mMaintenanceTaskAndKit.MaintenanceTypeID = 1 Then
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                lgdInspectionDetails.InnerText = "MPD Details"
                lblCode.InnerText = "Task No."
            Else
                lgdInspectionDetails.InnerText = "Service Details"
                lblCode.InnerText = "Code"
            End If

        ElseIf mMaintenanceTaskAndKit.MaintenanceTypeID = 2 Then
            lgdInspectionDetails.InnerText = "Inspection Details"
        ElseIf mMaintenanceTaskAndKit.MaintenanceTypeID = 3 Then
            lgdInspectionDetails.InnerText = "Directives Details"
        End If
    End Sub
    Private Sub ControlVisibility()
        If mChild = 1 Then
            pnlTask.Visible = True
            lblTitle.Text = "Maintenance Task Card Details"
            btnDelete.Enabled = mMaintenanceTask.MaintenanceTaskDetails.Count > 0
        Else
            pnlSpareTools.Visible = True
            pnlSpareToolsButton.Visible = True
            If mChild = 2 Then
                lblTitle.Text = "Maintenance Spares Details"
                lgdKitDetails.InnerText = "Spares Detailss"
                btnAddKit.ToolTip = "Click to Add Spares"
            Else
                lblTitle.Text = "Maintenance Tools Details"
                lgdKitDetails.InnerText = "Tools Detailss"
                btnAddKit.ToolTip = "Click to Add Tools"
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If mChild = 2 Or mChild = 3 Then dgKitList.DataSource = mMaintenanceKit.MaintenanceKitDetails
        If mChild = 1 Then dgTaskList.DataSource = mMaintenanceTask.MaintenanceTaskDetails
        If mChild = 1 Then btnDelete.Visible = True
        Session("mMaintenanceTask") = mMaintenanceTask
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            'Added by Saylee on 23-July-2013 for BA22072013 
            If mChild = 1 Then 'Task Card 
                If mMaintenanceTaskAndKit.MaintenanceTaskID.Equals(Guid.Empty) Then
                    mMaintenanceTask = MaintenanceTask.NewMaintenanceTask(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly)
                Else '
                    mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mMaintenanceTaskAndKit.ID)
                End If
            ElseIf mChild = 2 Then  'Spares
                If mMaintenanceTaskAndKit.MaintenanceKitID.Equals(Guid.Empty) Then
                    mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly, False)
                Else
                    mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mMaintenanceTaskAndKit.ID, False)
                End If
            ElseIf mChild = 3 Then  'Tools
                If mMaintenanceTaskAndKit.MaintenanceToolID.Equals(Guid.Empty) Then
                    mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly, True)
                Else
                    mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mMaintenanceTaskAndKit.ID, True)
                End If
            End If
            Session("mMaintenanceTask") = mMaintenanceTask
            Session("mMaintenanceKit") = mMaintenanceKit
            DataFieldBind()
            SetPage()
            ControlVisibility()

            If CType(Session("AddTaskCards"), String) = "True" Then
                'Add selected part(s) to Enquiry Items
                AddMultipleTaskCards()
                'Added by Saylee on 23-July-2013 for BA22072013 
                mChild = 1
                Session("mChild") = mChild
                'End
                Session("AddTaskCards") = "False"
            Else
                Session("AddTaskCards") = "False"
            End If
        End If

    End Sub
#Region " Add Multiple Task Cards "
    Private Sub AddMultipleTaskCards()
        Dim mTaskCard As TaskCard
        Dim mTaskCardList As TaskCardList = Session("mSelectTaskCardList")
        For Each mTaskCard In mTaskCardList
            If mTaskCard.IsSelect Then
                If Not mMaintenanceTask.MaintenanceTaskDetails.Contains(mTaskCard.ID, "") Then
                    mMaintenanceTask.MaintenanceTaskDetails.Add(mMaintenanceTask.ID)
                    With mMaintenanceTask.MaintenanceTaskDetails.CurrentItem
                        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.SrNo = mMaintenanceTask.MaintenanceTaskDetails.CurrentIndex + 1
                        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardID = mTaskCard.ID
                        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardNo = mTaskCard.TaskCardNo
                        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task = mTaskCard.TaskDesc
                        mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Note = ""

                        Try
                            mMaintenanceTask.ApplyEdit()
                            mMaintenanceTask.Save()
                            mMaintenanceTaskAndKit.MaintenanceTaskID = mMaintenanceTask.ID
                        Catch ex As Exception
                            mMaintenanceTask.MaintenanceTaskDetails.Remove(mMaintenanceTask.MaintenanceTaskDetails.CurrentItem)
                        End Try
                    End With
                    'Added by Vikrant On 27-Mar-2020 to add Task Card Spares Tools into maint activity Tools spares
                    Dim TmpTaskCard As TaskCard
                    TmpTaskCard = TaskCard.GetTaskCard(mTaskCard.ID)
                    If mMaintenanceTaskAndKit.MaintenanceKitID.Equals(Guid.Empty) Then
                        mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly, False)
                    Else
                        mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mMaintenanceTaskAndKit.ID, False)
                    End If
                    For Each TaskSpare As TaskCardSpare In TmpTaskCard.TaskCardSpares
                        If Not TaskSpare.ItemID.Equals(Guid.Empty) Then
                            If mMaintenanceKit.MaintenanceKitDetails.Contains(TaskSpare.PartNo) Then
                                mMaintenanceKit.MaintenanceKitDetails(TaskSpare.ItemID, "").Qty += TaskSpare.RequiredQty
                            Else
                                mMaintenanceKit.MaintenanceKitDetails.Add(mMaintenanceKit.ID)
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = mMaintenanceKit.MaintenanceKitDetails.CurrentIndex + 1
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = TaskSpare.ItemID
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = TaskSpare.RequiredQty
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = TaskSpare.Remark
                            End If
                        End If
                    Next
                    mMaintenanceKit.Save()
                    'If mMaintenanceKit.IsForTool = True Then 'Added by Saylee on 23-July-2013 for BA22072013 
                    '    mMaintenanceTaskAndKit.MaintenanceToolID = mMaintenanceKit.ID
                    'Else
                    '    mMaintenanceTaskAndKit.MaintenanceKitID = mMaintenanceKit.ID
                    'End If
                    mMaintenanceTaskAndKit.MaintenanceKitID = mMaintenanceKit.ID 'Added by Vikrant On 07-Jul-2020 to solve duplicate items getting added issue

                    If mMaintenanceTaskAndKit.MaintenanceToolID.Equals(Guid.Empty) Then
                        mMaintenanceKit = MaintenanceKit.NewMaintenanceKit(mMaintenanceTaskAndKit.MaintenanceTypeID, mMaintenanceTaskAndKit.ID, mMaintenanceTaskAndKit.IsAssembly, True)
                    Else
                        mMaintenanceKit = MaintenanceKit.GetMaintenanceKitByParent(mMaintenanceTaskAndKit.ID, True)
                    End If
                    For Each TaskTool As TaskCardTool In TmpTaskCard.TaskCardTools
                        If Not TaskTool.ItemID.Equals(Guid.Empty) Then
                            If mMaintenanceKit.MaintenanceKitDetails.Contains(TaskTool.PartNo) Then
                                mMaintenanceKit.MaintenanceKitDetails(TaskTool.ItemID, "").Qty += TaskTool.RequiredQty
                            Else
                                mMaintenanceKit.MaintenanceKitDetails.Add(mMaintenanceKit.ID)
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo = mMaintenanceKit.MaintenanceKitDetails.CurrentIndex + 1
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID = TaskTool.ItemID
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty = TaskTool.RequiredQty
                                mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark = TaskTool.Remark
                            End If
                        End If

                    Next
                    mMaintenanceKit.Save() 'Need to save seperate as IsForTool is as saved differently
                    mMaintenanceTaskAndKit.MaintenanceToolID = mMaintenanceKit.ID 'Added by Vikrant On 07-Jul-2020 to solve duplicate items getting added issue
                    'If mMaintenanceKit.IsForTool = True Then 'Added by Saylee on 23-July-2013 for BA22072013 
                    '    mMaintenanceTaskAndKit.MaintenanceToolID = mMaintenanceKit.ID
                    'Else
                    '    mMaintenanceTaskAndKit.MaintenanceKitID = mMaintenanceKit.ID
                    'End If
                    'End
                Else
                End If
            End If
        Next
        DataFieldBind()
        Session("TaskCards") = "False"
        Session.Remove("mTaskCard")
        Session.Remove("mTaskCardList")
    End Sub
#End Region

    Private Sub btnAddKit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKit.Click
        '''mMaintenanceKit.MaintenanceKitDetails.Add(mMaintenanceKit.ID)
        Session("mMaintenanceKit") = mMaintenanceKit
        Session("EditKit") = False
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddKit", "OpenToAddKit(" + Server.UrlEncode("wfMaintenanceKitDetailMultipleItems_Ajax.aspx?Type=pup") + ");", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddKit", "OpenToAddKit(1);", True)
    End Sub
    Private Sub btnAddTask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTask.Click
        Dim ModelID As Guid
        Dim ModelName As String
        ModelID = mMaintenanceTaskAndKit.ModelID 'Model_OR_PartID
        ModelName = mMaintenanceTaskAndKit.ModelName 'Model_OR_PartName
        Session("mMaintenanceKit") = mMaintenanceKit
        Session("AddTaskCards") = "False"
        Session.Remove("mSelectTaskCardList")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddSelectTasks", "OpenToAddSelectTasks();", True)

        'Response.Redirect("wfSelectTaskCardList_Ajax.aspx?ModelID=" & ModelID.ToString & "&ModelName=" & ModelName & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=wfMaintenanceKitandTask_Ajax.aspx" & "&Type=pup")
    End Sub
    Private Sub btnCloseKit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseKit.Click
        RemoveSession()
        Session("EditKit") = False

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("BackPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6"))
    End Sub
    Private Sub btnCloseTask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTask.Click
        RemoveSession()
        Session("EditKit") = False

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("BackPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6"))
    End Sub
    Private Sub dgKitList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgKitList.RowCommand
        Select Case e.CommandName
            Case "EditRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgKitList.PageIndex * dgKitList.PageSize
                mMaintenanceKit.MaintenanceKitDetails.CurrentIndex = index
                Session("mMaintenanceKit") = mMaintenanceKit
                Session("EditKit") = True
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddKit", "OpenToAddKit(0);", True)
                'Response.Redirect("wfMaintenanceKitDetail_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=wfMaintenanceKitandTask_Ajax.aspx")
            Case "RemoveRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgKitList.PageIndex * dgKitList.PageSize
                DeleteRecordKit(index)
        End Select
    End Sub
    Private Sub dgTaskList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskList.RowCommand
        Select Case e.CommandName
            Case "EditRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgTaskList.PageIndex * dgTaskList.PageSize
                mMaintenanceTask.MaintenanceTaskDetails.CurrentIndex = index
                Session("mMaintenanceTask") = mMaintenanceTask
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddTasks", "OpenToAddTasks();", True)
                'Response.Redirect("wfMaintenanceTaskDetail_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&BackPage5=wfMaintenanceKitandTask_Ajax.aspx")
            Case "RemoveRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgTaskList.PageIndex * dgTaskList.PageSize
                DeleteRecordTask(index)
        End Select
    End Sub
    Private Sub hdnBtnAddTasksClick(sender As Object, e As System.EventArgs) Handles hdnBtnAddTasks.Click
        DataFieldBind()
        upnlTaskList.Update()
    End Sub
    Private Sub hdnBtnAddKit_Click(sender As Object, e As System.EventArgs) Handles hdnBtnAddKit.Click
        DataFieldBind()
        upnlKitList.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnAddSelectTasks_Click(sender As Object, e As System.EventArgs) Handles hdnBtnAddSelectTasks.Click
        If CType(Session("AddTaskCards"), String) = "True" Then
            'Add selected part(s) to Task's Items
            AddMultipleTaskCards()
            'Added by Saylee on 23-July-2013 for BA22072013 
            mChild = 1
            Session("mChild") = mChild
            'End
            Session("AddTaskCards") = "False"
        Else
            Session("AddTaskCards") = "False"
        End If

        ControlVisibility()

        upnlTaskList.DataBind()
        upnlTaskList.Update()

    End Sub
#End Region

#Region "Drag n Drop" 'Added by Saylee on 30-Sep-2013 for ALL03102013

    <WebMethod()>
    Public Shared Sub GetTableIDs(ByVal Ids As DragDrop)
        Dim mDragDrop As New DragDrop

        mDragDrop = Ids
        Dim mMaintenanceTask As MaintenanceTask

        mMaintenanceTask = HttpContext.Current.Session("mMaintenanceTask")
        For i As Integer = 0 To mDragDrop.SrNo.Length - 1
            If mMaintenanceTask.MaintenanceTaskDetails.Contains(mDragDrop.SrNo(i)) Then
                mMaintenanceTask.MaintenanceTaskDetails.Item(mDragDrop.SrNo(i), "").TempSrNo = CInt(mDragDrop.index(i)) + 1
            End If
        Next

        HttpContext.Current.Session("mMaintenanceTask") = mMaintenanceTask
    End Sub
    Protected Sub btnSaveTasks_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveTasks.Click
        Try
            mMaintenanceTask.MaintenanceTaskDetails.UpdateSrNo()
            mMaintenanceTask.ApplyEdit()
            mMaintenanceTask.Save()
            mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskByParent(mMaintenanceTaskAndKit.ID)
            dgTaskList.DataSource = mMaintenanceTask.MaintenanceTaskDetails
            dgTaskList.DataBind()
            HttpContext.Current.Session("mMaintenanceTask") = mMaintenanceTask
        Catch ex As Exception

        End Try
    End Sub
    Public Class DragDrop
        Private mIndex(10) As String
        Private mSrno(10) As String
        Public Property index() As String()
            Get
                Return mIndex
            End Get
            Set(ByVal value As String())
                ReDim mIndex(value.Length)
                mIndex = value
            End Set
        End Property
        Public Property SrNo() As String()
            Get
                Return mSrno
            End Get
            Set(ByVal value As String())
                ReDim mSrno(value.Length)
                mSrno = value
            End Set
        End Property
        Public Sub New()

        End Sub
    End Class
#End Region

#Region "Checked Selection" 'Added by Saylee on 11-Mar-2014 for ALL11032014
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If IsPostBack Then
            ' create a string builder to create the displayed string
            Dim builder = New StringBuilder()
            builder.Append("Vous have selected the following checks :<br/>")
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
                    mMaintenanceTask.MaintenanceTaskDetails.Remove(New Guid(value), "")
                Next
                values = ""
                checkString = Nothing
            End If
        End If

        If mMaintenanceTask.IsDirty Then
            mMaintenanceTask.ApplyEdit()
            mMaintenanceTask.Save()
            Session("mMaintenanceTask") = mMaintenanceTask
        End If
        dgTaskList.DataSource = mMaintenanceTask.MaintenanceTaskDetails
        dgTaskList.DataBind()
        Session("mMaintenanceKit") = mMaintenanceKit
        btnDelete.Enabled = mMaintenanceTask.MaintenanceTaskDetails.Count > 0
        upnlTaskList.Update()
    End Sub
#End Region



End Class