'AJAX Conversion By Vikrant On 03-Jul-2015

Public Class wfMachineMaintenancePolicies_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMaintenanceProgramList As MaintenanceProgramList
    Public mProgramTypeList As ProgramTypeList

    Public mMachine As Machine
    Public mProgramTypeID As Guid
    Public mMaintProgramID As Guid
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mProgramTypeList = Session("mProgramTypeList")
        mMaintenanceProgramList = Session("mMaintenanceProgramList")
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mProgramTypeList") = mProgramTypeList
        Session("mMaintenanceProgramList") = mMaintenanceProgramList
        Session("mMachine") = mMachine
    End Sub
    'Added By Vikrant On 26-Jun-2014
    Private Sub RemoveSession()
        Session.Remove("mProgramTypeList")
        Session.Remove("mMaintenanceProgramList")
    End Sub
    'End
    Private Sub SetObjectForMaintPolicy()
        mMachine.MachineMaintenancePolicies.Item(mMachine.MachineMaintenancePolicies.CurrentIndex).MaintProgramID = New Guid(cmbMaintProgram.SelectedValue)
        mMachine.MachineMaintenancePolicies.Item(mMachine.MachineMaintenancePolicies.CurrentIndex).IsApplicable = chkApplicable.Checked
        mMachine.MachineMaintenancePolicies.Item(mMachine.MachineMaintenancePolicies.CurrentIndex).Remark = Trim(txtRemark.Text)
    End Sub
    Private Sub SetObjectForMachineInsp()
        mMachine.MachineStructuralInspectionList.Item(mMachine.MachineStructuralInspectionList.CurrentIndex).ProgramTypeID = New Guid(cmbProgramType.SelectedValue)
        mMachine.MachineStructuralInspectionList.Item(mMachine.MachineStructuralInspectionList.CurrentIndex).Description = Trim(txtDescription.Text)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteMaintPolicy" Then
                        Try
                            Session("sender") = ""
                            mMachine.MachineMaintenancePolicies.Remove(mMachine.MachineMaintenancePolicies(mMachine.MachineMaintenancePolicies.CurrentIndex))
                            Session("mMachine") = mMachine
                            DataFieldBind()
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                            SetPage()
                            upnlMaintPolicyDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If

                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Machine", "Aircraft Reg.No. -> " + mMachine.RegNo + " Certificate No.-> " + mMachineCertificateNo + " Certificate Name ->" + mMachineCertificateName, Util.ErrorType.NoError, mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).ID)
                            End If
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "DeleteMachineInsp" Then
                        Try
                            Session("sender") = ""
                            mMachine.MachineStructuralInspectionList.Remove(mMachine.MachineStructuralInspectionList(mMachine.MachineStructuralInspectionList.CurrentIndex))
                            Session("mMachine") = mMachine
                            DataFieldBind()
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                            SetPage()
                            upnlStructInspDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Machine", "Aircraft Reg.No. -> " + mMachine.RegNo + " Certificate No.-> " + mMachineCertificateNo + " Certificate Name ->" + mMachineCertificateName, Util.ErrorType.NoError, mMachine.MachineCertificates.Item(mMachine.MachineCertificates.CurrentIndex).ID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""       
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub EditRecordForMaintPolicy(ByVal ID As Guid)
        cmbMaintProgram.SelectedValue = mMachine.MachineMaintenancePolicies.Item(ID).MaintProgramID.ToString
        chkApplicable.Checked = mMachine.MachineMaintenancePolicies.Item(ID).IsApplicable
        txtRemark.Text = mMachine.MachineMaintenancePolicies.Item(ID).Remark
    End Sub
    Private Sub EditRecordForMachineInsp(ByVal ID As Guid)
        cmbProgramType.SelectedValue = mMachine.MachineStructuralInspectionList.Item(ID).ProgramTypeID.ToString
        txtDescription.Text = mMachine.MachineStructuralInspectionList.Item(ID).Description
    End Sub
    Private Sub SetPage()
        lblResult.Text = "List of Maintenance Policy: " & mMachine.MachineMaintenancePolicies.Count & " Record(s) found"
        Label4.Text = "List of Structural Inspection: " & mMachine.MachineStructuralInspectionList.Count & " Record(s) found"
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Session("mMachineMaintenancePolicyEdit") = True Then
            mMaintProgramID = Session("mMaintProgramID")
            EditRecordForMaintPolicy(mMaintProgramID)
        ElseIf Session("mMachineStructuralInspEdit") = True Then
            mProgramTypeID = Session("mProgramTypeID")
            EditRecordForMachineInsp(mProgramTypeID)
        End If
        mMaintenanceProgramList = MaintenanceProgramList.GetMaintenanceProgramList("", "(SELECT)")
        cmbMaintProgram.DataSource = mMaintenanceProgramList
        Session("mMaintenanceProgramList") = mMaintenanceProgramList

        mProgramTypeList = ProgramTypeList.GetProgramTypeList("", "(SELECT)")
        cmbProgramType.DataSource = mProgramTypeList
        Session("mProgramTypeList") = mProgramTypeList

        dgMaintenancePolicyList.DataSource = mMachine.MachineMaintenancePolicies
        dgMachineStructuralInspList.DataSource = mMachine.MachineStructuralInspectionList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            SetPage()
        End If
    End Sub
    Private Sub btnAddMaintPolicy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMaintPolicy.Click
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not IsValid Then upnlValidation1.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True) : Exit Sub

        If Session("mMachineMaintenancePolicyEdit") = False Then
            'MarkLog(Util.Action.[New], "Machine", " Aircraft Name ->" & mMachine.RegNo & " Certificate No. -> " & Trim(txtNo.Text) & "  Certificate Name -> " & txtName.Text, Util.ErrorType.NoError, Guid.Empty)
            If Not mMachine.MachineMaintenancePolicies.Contains(New Guid(cmbMaintProgram.SelectedValue), "") Then
                mMachine.MachineMaintenancePolicies.Add(mMachine.ID, New Guid(cmbMaintProgram.SelectedValue), chkApplicable.Checked, txtRemark.Text)
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Please Select Different Maintenance Program.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            'If Not CustomValidate1() Then
            '    mMachine.MachineMaintenancePolicies.Remove(mMachine.MachineMaintenancePolicies.CurrentItem)
            '    Exit Sub
            'End If
            
            Session("mMachine") = mMachine
            dgMaintenancePolicyList.DataSource = mMachine.MachineMaintenancePolicies
            dgMaintenancePolicyList.DataBind()
            SetPage()
            cmbMaintProgram.ClearSelection()
            txtRemark.Text = ""
            chkApplicable.Checked = False
        Else
            SetObjectForMaintPolicy()
            'If Not CustomValidate1() Then
            '    Exit Sub
            'End If
            Session("mMachine") = mMachine
            Session("mMachineMaintenancePolicyEdit") = False
            dgMaintenancePolicyList.DataSource = mMachine.MachineMaintenancePolicies
            dgMaintenancePolicyList.DataBind()
            cmbMaintProgram.ClearSelection()
            chkApplicable.Checked = False
            txtRemark.Text = ""
            SetPage()
        End If
    End Sub
    Private Sub btnAddMachineInsp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMachineInsp.Click
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not IsValid Then upnlValidation2.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True) : Exit Sub

        If Session("mMachineStructuralInspEdit") = False Then
            'MarkLog(Util.Action.[New], "Machine", " Aircraft Name ->" & mMachine.RegNo & " Certificate No. -> " & Trim(txtNo.Text) & "  Certificate Name -> " & txtName.Text, Util.ErrorType.NoError, Guid.Empty)
            If Not mMachine.MachineStructuralInspectionList.Contains(New Guid(cmbProgramType.SelectedValue), "") Then
                mMachine.MachineStructuralInspectionList.Add(mMachine.ID, New Guid(cmbProgramType.SelectedValue), txtDescription.Text)
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Please Select Different Program Type.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            'If Not CustomValidate1() Then
            '    mMachine.MachineStructuralInspectionList.Remove(mMachine.MachineStructuralInspectionList.CurrentItem)
            '    Exit Sub
            'End If
            ''For i As Integer = 0 To mMachine.MachineCertificates.Count - 1
            ''    mMachine.MachineMaintenancePolicies(i).SerialNo = i + 1
            ''Next
            Session("mMachine") = mMachine
            dgMachineStructuralInspList.DataSource = mMachine.MachineStructuralInspectionList
            dgMachineStructuralInspList.DataBind()
            SetPage()
            cmbProgramType.ClearSelection()
            txtDescription.Text = ""
        Else
            SetObjectForMachineInsp()
            'If Not CustomValidate1() Then
            '    Exit Sub
            'End If
            Session("mMachine") = mMachine
            Session("mMachineStructuralInspEdit") = False
            dgMachineStructuralInspList.DataSource = mMachine.MachineStructuralInspectionList
            dgMachineStructuralInspList.DataBind()
            SetPage()
            cmbProgramType.ClearSelection()
            txtDescription.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub dgMaintenancePolicyList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMaintenancePolicyList.RowCommand
        Dim Index As Int32

        Select Case e.CommandName
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMaintenancePolicyList.PageSize * dgMaintenancePolicyList.PageIndex
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteMaintPolicy")
                mMachine.MachineMaintenancePolicies.CurrentIndex = Index
                Session("mMachine") = mMachine
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMaintenancePolicyList.PageSize * dgMaintenancePolicyList.PageIndex
                mMachine.MachineMaintenancePolicies.CurrentIndex = Index
                Dim mID As Guid = New Guid(dgMaintenancePolicyList.DataKeys(Index).Value.ToString)
                mMaintProgramID = mID
                EditRecordForMaintPolicy(mID)
                dgMaintenancePolicyList.DataSource = mMachine.MachineMaintenancePolicies
                dgMachineStructuralInspList.DataSource = mMachine.MachineStructuralInspectionList
                upnlMaintPolicyDetails.DataBind()
                Session("mMachineMaintenancePolicyEdit") = True
                Session("mMaintProgramID") = mMaintProgramID
                Session("mMachine") = mMachine
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        RemoveSession()
        Session.Remove("mMachineMaintenancePolicyEdit")
        Session.Remove("mMachineStructuralInspEdit")
        '  Response.Redirect("wfMachine.aspx?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub dgMachineStructuralInspList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMachineStructuralInspList.PageIndexChanging, dgMaintenancePolicyList.PageIndexChanging
        If CType(sender, System.Web.UI.WebControls.GridView).ClientID = "dgMaintenancePolicyList" Then

        ElseIf CType(sender, System.Web.UI.WebControls.GridView).ClientID = "dgMachineStructuralInspList" Then
        End If
    End Sub
    Private Sub dgMaintenancePolicyList_Sorting(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMaintenancePolicyList.Sorting, dgMachineStructuralInspList.Sorting
        If CType(source, System.Web.UI.WebControls.GridView).ClientID = "dgMaintenancePolicyList" Then
            mMachine.MachineMaintenancePolicies.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
            dgMaintenancePolicyList.DataSource = mMachine.MachineMaintenancePolicies
            dgMaintenancePolicyList.DataBind()
        ElseIf CType(source, System.Web.UI.WebControls.GridView).ClientID = "dgMachineStructuralInspList" Then
            mMachine.MachineStructuralInspectionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
            dgMachineStructuralInspList.DataSource = mMachine.MachineStructuralInspectionList
            dgMachineStructuralInspList.DataBind()
        End If
    End Sub
    Private Sub imgbtnMaintProgram_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnMaintProgram.Click, imgbtnProgramType.Click
        If CType(sender, System.Web.UI.Control).ClientID = "imgbtnMaintProgram" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentMaintProgramMasterFunction", "CallParentMaintProgramMasterFunction();", True)
            'Response.Redirect("wfMaintenanceProgram_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMachineMaintenancePolicies_Ajax.aspx")
        ElseIf CType(sender, System.Web.UI.Control).ClientID = "imgbtnProgramType" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentProgramTypeMasterFunction", "CallParentProgramTypeMasterFunction();", True)
            'Response.Redirect("wfProgramType_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMachineMaintenancePolicies_Ajax.aspx")
        End If
    End Sub
    Private Sub dgMachineStructuralInspList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMachineStructuralInspList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgMachineStructuralInspList.PageSize * dgMachineStructuralInspList.PageIndex
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteMachineInsp")
                mMachine.MachineStructuralInspectionList.CurrentIndex = Index
                Session("mMachine") = mMachine
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgMachineStructuralInspList.PageSize * dgMachineStructuralInspList.PageIndex
                mMachine.MachineStructuralInspectionList.CurrentIndex = Index
                Dim mID As Guid = New Guid(dgMachineStructuralInspList.DataKeys(Index).Value.ToString)
                mProgramTypeID = mID
                EditRecordForMachineInsp(mID)
                dgMachineStructuralInspList.DataSource = mMachine.MachineStructuralInspectionList
                dgMaintenancePolicyList.DataSource = mMachine.MachineMaintenancePolicies
                upnlStructInspDetails.DataBind()
                Session("mMachineStructuralInspEdit") = True
                Session("mProgramTypeID") = mProgramTypeID
                Session("mMachine") = mMachine
        End Select
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class