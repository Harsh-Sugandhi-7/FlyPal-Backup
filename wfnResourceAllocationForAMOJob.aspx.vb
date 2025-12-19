Imports java.lang

Public Class wfnResourceAllocationForAMOJob
    Inherits Page

#Region " Variable Declaration "

    Public mnWO As nWO
    Public mnWOJob As nWOJob
    Public mnWOJobResourceAllocationAMO As nWOJobResourceAllocationAMO
    Public mnWOJobResourceAllocationAMOList As nWOJobResourceAllocationAMOList
    Public mEmployeeList As EmployeeList
    Dim mMPDSkillList As MPDSkillList 'Added by Saylee on 3-Jul-2023

    Protected mEmployeeDocumentDueList As EmployeeDocumentDueList

#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mnWO = Session("mnWO")
        mnWOJob = Session("mnWOJob")
        mnWOJobResourceAllocationAMO = Session("mnWOJobResourceAllocationAMO")
        mEmployeeList = Session("mEmployeeList")
        mMPDSkillList = Session("mMPDSkillList")
        mnWOJobResourceAllocationAMOList = Session("mnWOJobResourceAllocationAMOList")
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mnWO")
        Session.Remove("mnWOJob")
        Session.Remove("mnWOJobResourceAllocationAMO")
        Session.Remove("mEmployeeList")
        Session.Remove("mMPDSkillList")
        Session.Remove("mnWOJobResourceAllocationAMOList")
    End Sub

    Private Sub NewResourceAllocationRecord()
        mnWOJobResourceAllocationAMO = nWOJobResourceAllocationAMO.NewWOJobResourceAllocationAMO
        Session("mnWOJobResourceAllocationAMO") = mnWOJobResourceAllocationAMO
    End Sub

    Private Sub EditResourceAllocationRecord(mID As Guid)
        mnWOJobResourceAllocationAMO = nWOJobResourceAllocationAMO.GetWOJobResourceAllocationAMO(mID)
        Session("mnWOJobResourceAllocationAMO") = mnWOJobResourceAllocationAMO
    End Sub

    Private Sub SetObject(mnWOJobResourceAllocationAMO As nWOJobResourceAllocationAMO, EmpID As String)
        mnWOJobResourceAllocationAMO.EmployeeID = New Guid(EmpID)
        mnWOJobResourceAllocationAMO.WOJobID = mnWOJob.ID
        mnWOJobResourceAllocationAMO.SrNo = mnWOJobResourceAllocationAMOList.Count + 1
    End Sub

    Private Sub SetCheckBoxList()

        For i As Integer = 0 To chkEmployeeList.Items.Count - 1

            If mnWOJobResourceAllocationAMOList.Contains(New Guid(chkEmployeeList.Items(i).Value)) Then
                chkEmployeeList.Items(i).Selected = True
            End If

        Next

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Save" Then

                        Try

                            Dim strMSG As String = ""

                            For i As Integer = 0 To chkEmployeeList.Items.Count - 1

                                If chkEmployeeList.Items(i).Selected Then

                                    If mnWOJobResourceAllocationAMOList.Contains(New Guid(chkEmployeeList.Items(i).Value)) Then
                                        EditResourceAllocationRecord(mnWOJobResourceAllocationAMOList(New Guid(chkEmployeeList.Items(i).Value), "").ID)
                                    Else
                                        NewResourceAllocationRecord()
                                    End If

                                    SetObject(mnWOJobResourceAllocationAMO, chkEmployeeList.Items(i).Value)

                                    'Çheck  For Duplicate
                                    If mnWOJobResourceAllocationAMO.IsValid Then
                                        mnWOJobResourceAllocationAMO.Save()
                                    Else

                                        If Not mnWOJobResourceAllocationAMO.IsValid Then

                                            For j As Integer = 0 To mnWOJobResourceAllocationAMO.GetBrokenRulesCollection.Count - 1
                                                strMSG = strMSG + mnWOJobResourceAllocationAMO.GetBrokenRulesCollection(j).Description + "<Br>"
                                            Next

                                        End If

                                    End If

                                ElseIf mnWOJobResourceAllocationAMOList.Contains(New Guid(chkEmployeeList.Items(i).Value)) Then

                                    mnWOJobResourceAllocationAMO = nWOJobResourceAllocationAMO.GetWOJobResourceAllocationAMO(New Guid(chkEmployeeList.Items(i).Value), mnWOJob.ID)
                                    nWOJobResourceAllocationAMO.DeleteWOJobResourceAllocationAMO(mnWOJobResourceAllocationAMO.ID)
                                    MarkLog(Action.Delete,
                                            "WOJobResourceAllocationAMO",
                                            "Emp : " + chkEmployeeList.Items(i).Text +
                                            " Job Task No. : " + mnWOJob.TaskCardNo +
                                            "Deallocate or Delete for Job",
                                            ErrorType.NoError,
                                            Guid.Empty,
                                            EventLogID)

                                End If

                            Next

                            If strMSG <> "" Then

                                MSGBoxCtrl.Show("Error",
                                                strMSG,
                                                "",
                                                MsgBoxStyle.YesNo,
                                                "")

                                Exit Sub

                            End If

                            mnWOJobResourceAllocationAMOList = nWOJobResourceAllocationAMOList.GetWOJobResourceAllocationAMOList(mnWOJob.ID, "")
                            Session("mnWOJobResourceAllocationAMOList") = mnWOJobResourceAllocationAMOList
                            mEmployeeList = EmployeeList.GetEmployeeListAsPerSkill(SkillID:=mnWOJob.SkillID,
                                                                                   CheckForDocumentAndTrainingDue:=1)
                            Session("mEmployeeList") = mEmployeeList
                            chkEmployeeList.DataSource = mEmployeeList
                            chkEmployeeList.DataBind()
                            SetDueEmployees()
                            SetCheckBoxList()
                            upnlResourceAllocationInfo.Update()
                            MSGBoxCtrl.Show("Success!",
                                            "Job allocated to selected employees successfully",
                                            "", MsgBoxStyle.OkOnly,
                                            "OkMsg")

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure + "," + ex.Message,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            DataFieldBind()
                            msgCount = ex.Errors.Count

                        End Try

                    End If

                Case MsgBoxResult.No

                    Session("sender") = ""

                Case MsgBoxResult.Ok

                    Session("sender") = ""
                    If MSGBoxCtrl.Sender = "OkMsg" Then

                        RemoveSession()
                        Dim mOpenAs As String = Request.QueryString("Type")
                        If Not mOpenAs Is Nothing AndAlso mOpenAs = "pup" Then

                            ScriptManager.RegisterStartupScript(Me,
                                                                [GetType],
                                                                "onclose",
                                                                "CallParentCallback();",
                                                                True)
                            Exit Sub

                        End If

                        Response.Redirect("index.aspx")

                    End If

                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"

                    Session("sender") = ""
                    DataFieldBind()

            End Select

        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If

    End Sub

    Private Sub SetDueEmployees()

        Try

            For i As Integer = 0 To mEmployeeList.Count - 1

                Dim item As ListItem = chkEmployeeList.Items(i)
                Dim hasDue As Boolean = Convert.ToBoolean(mEmployeeList.Item(i).IsDocumentOrTrainingDue)

                item.Enabled = Not hasDue

                If hasDue Then
                    item.Attributes("style") = "color: Red;"
                End If

            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        mnWOJobResourceAllocationAMOList = nWOJobResourceAllocationAMOList.GetWOJobResourceAllocationAMOList(mnWOJob.ID, "")
        Session("mnWOJobResourceAllocationAMOList") = mnWOJobResourceAllocationAMOList

        mEmployeeList = EmployeeList.GetEmployeeListAsPerSkill(SkillID:=mnWOJob.SkillID,
                                                               CheckForDocumentAndTrainingDue:=1)
        Session("mEmployeeList") = mEmployeeList
        chkEmployeeList.DataSource = mEmployeeList
        mMPDSkillList = MPDSkillList.GetSkillList(True)
        cmbSkillcode.DataSource = mMPDSkillList
        Session("mMPDSkillList") = mMPDSkillList

        DataBind()

        SetDueEmployees()

    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            DataFieldBind()
            SetCheckBoxList()
        End If

        txtCustomer.Text = mnWO.CustomerName + vbCrLf + mnWO.CustomerWONo

    End Sub

    Private Sub SaveDetails(sender As Object, e As EventArgs) Handles btnSave.Click

        If IsValid Then

            Try
                MSGBoxCtrl.Show("Save Alert",
                                "You are about to Allocate Job for selected employees. Do you want to continue? ",
                                "",
                                MsgBoxStyle.YesNo,
                                "Save")
                Exit Sub

            Catch ex As Exception
                Throw ex
            End Try

        Else
            upnlValidationSummary.Update()
        End If

    End Sub

    Private Sub CloseModal(sender As Object, e As EventArgs) Handles btnClose.Click

        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then

            ScriptManager.RegisterStartupScript(Me, [GetType], "onclose", "CallParentCallback();", True)
            Exit Sub

        End If

        Response.Redirect("index.aspx")

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub ShowAllEmp_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowAllEmp.CheckedChanged

        If chkShowAllEmp.Checked Then

            mEmployeeList = EmployeeList.GetEmployeeListAsPerSkill(0,
                                                                   CheckForDocumentAndTrainingDue:=1)
            Session("mEmployeeList") = mEmployeeList
            chkEmployeeList.DataSource = mEmployeeList

        Else

            mEmployeeList = EmployeeList.GetEmployeeListAsPerSkill(SkillID:=mnWOJob.SkillID,
                                                                   CheckForDocumentAndTrainingDue:=1)
            Session("mEmployeeList") = mEmployeeList
            chkEmployeeList.DataSource = mEmployeeList

        End If

        chkEmployeeList.DataBind()
        SetDueEmployees()
        upnlResourceAllocationInfo.Update()
        SetCheckBoxList()

    End Sub

#End Region

End Class