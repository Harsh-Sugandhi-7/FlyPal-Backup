Public Class wfMachineZoneConfiguration_Ajax
    Inherits System.Web.UI.Page


#Region " Variable Declaration "

    Public mMachine As Machine
    Public mMachineZoneConfigurationList As MachineZoneConfigurationList
    Public mMachineZoneConfigurationID As Guid
#End Region

#Region " Business Methods "

    Private Sub addAttributes()

        txtMaxWeight.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtMaxWeight').value,event)")
        txtReferenceArm.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReferenceArm').value,event)")
        txtMoments.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtMoments').value,event)")

    End Sub

    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineZoneConfigurationEdit")
        Session.Remove("mMachineZoneConfigurationID")
    End Sub

    Private Sub SetObject()

        mMachine.MachineZoneConfigurations.Item(mMachine.MachineZoneConfigurations.CurrentIndex).ZoneConfigurationName = txtName.Text
        mMachine.MachineZoneConfigurations.Item(mMachine.MachineZoneConfigurations.CurrentIndex).MaxWeight = Val(txtMaxWeight.Text)
        mMachine.MachineZoneConfigurations.Item(mMachine.MachineZoneConfigurations.CurrentIndex).ReferenceArm = Val(txtReferenceArm.Text)
        mMachine.MachineZoneConfigurations.Item(mMachine.MachineZoneConfigurations.CurrentIndex).Moments = Val(txtMoments.Text)

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try

                            mMachine.MachineZoneConfigurations.Remove(mMachine.MachineZoneConfigurations(mMachine.MachineZoneConfigurations.CurrentIndex))
                            For i As Integer = 0 To mMachine.MachineCertificates.Count - 1
                                mMachine.MachineZoneConfigurations(i).SerialNo = i + 1
                            Next
                            Session("mMachine") = mMachine
                            Session("mMachineZoneConfigurationEdit") = False
                            SetControlsToBlank()
                            setFocus(txtName)
                            DataFieldBind()
                            SetPage()


                            lblAircraftZoneConfigurationDetails.InnerText = "Aircraft Zone Configuration Details [NEW]"
                            UpdatePanel()
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)

                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.DatabaseException, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        SetGridView()
                        upnlGridView.Update()
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub

    Private Sub EditRecord(ByVal ID As Guid)

        txtName.Text = mMachine.MachineZoneConfigurations.Item(ID).ZoneConfigurationName
        txtMaxWeight.Text = mMachine.MachineZoneConfigurations.Item(ID).MaxWeight
        txtReferenceArm.Text = mMachine.MachineZoneConfigurations.Item(ID).ReferenceArm
        txtMoments.Text = mMachine.MachineZoneConfigurations.Item(ID).Moments

        lblAircraftZoneConfigurationDetails.InnerText = "Aircraft Zone Configuration Details" & " [" & mMachine.MachineZoneConfigurations.Item(ID).ZoneConfigurationName & "]"

        upnlAircraftZoneConfigurationDetails.Update()

    End Sub
    Private Sub SetPage()
        lblResult.Text = "List of Zone Configuration: " & mMachine.MachineZoneConfigurations.Count & " Record(s) found"
    End Sub

    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        SetObject()
        If Not mMachine.IsValid Then
            For i As Integer = 0 To mMachine.MachineZoneConfigurations.CurrentItem.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mMachine.MachineZoneConfigurations.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Return True
    End Function
    Private Sub UpdatePanel()
        upnlAircraftZoneConfigurationDetails.Update()
        upnlAdd.Update()
        upnlGridView.Update()
        upnlBack.Update()
    End Sub
    Private Sub SetControlsToBlank()
        txtName.Text = ""
        txtMaxWeight.Text = ""
        txtReferenceArm.Text = ""
        txtMoments.Text = ""
        upnlAircraftZoneConfigurationDetails.Update()
    End Sub
    Private Sub SetGridView()
        dgZoneConfigurationList.DataSource = mMachine.MachineZoneConfigurations
        dgZoneConfigurationList.DataBind()
    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        If Session("mMachineZoneConfigurationEdit") = True Then
            mMachineZoneConfigurationID = Session("mMachineZoneConfigurationID")
            EditRecord(mMachineZoneConfigurationID)
        End If
        dgZoneConfigurationList.DataSource = mMachine.MachineZoneConfigurations
        DataBind()
    End Sub


#End Region

#Region " Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GetSession()
        addAttributes()
        If Not IsPostBack Then

            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            DataFieldBind()
            SetControlsToBlank()
            lblAircraftZoneConfigurationDetails.InnerText = "Aircraft Zone Configuration Details [NEW]"
            SetPage()

        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If

        If Not IsValid Then upnlValidation.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True) : Exit Sub

         
        If Session("mMachineZoneConfigurationEdit") = False Then

            If mMachine.MachineZoneConfigurations.Contains((txtName.Text)) Then

                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Zone Name already exists, can not be added.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            mMachine.MachineZoneConfigurations.Add(mMachine.ID, (txtName.Text), Val(txtMaxWeight.Text), Val(txtReferenceArm.Text), Val(txtMoments.Text))
            If Not CustomValidate1() Then
                mMachine.MachineZoneConfigurations.Remove(mMachine.MachineZoneConfigurations.CurrentItem)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                upnlValidation.Update()
                Exit Sub
            End If
            For i As Integer = 0 To mMachine.MachineZoneConfigurations.Count - 1
                mMachine.MachineZoneConfigurations(i).SerialNo = i + 1
            Next
        Else
            SetObject()
            If Not CustomValidate1() Then
                upnlValidation.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                Exit Sub
            End If

            Session("mMachineZoneConfigurationEdit") = False
        End If

        Session("mMachine") = mMachine
        SetControlsToBlank()
        lblAircraftZoneConfigurationDetails.InnerText = "Aircraft Zone Configuration Details [NEW]"
        setFocus(txtName)
        DataFieldBind()
        SetPage()
        UpdatePanel()

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)

    End Sub

    Private Sub dgZoneConfigurationList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgZoneConfigurationList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgZoneConfigurationList.PageSize * dgZoneConfigurationList.PageIndex
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                SetGridView()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                mMachine.MachineZoneConfigurations.CurrentIndex = Index
                Session("mMachine") = mMachine
            Case "EditRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgZoneConfigurationList.PageSize * dgZoneConfigurationList.PageIndex
                mMachine.MachineZoneConfigurations.CurrentIndex = Index
                Dim mID As Guid = mMachine.MachineZoneConfigurations(Index).ID
                mMachineZoneConfigurationID = mID
                EditRecord(mID)
                setFocus(txtName)
                SetGridView()
                Session("mMachineZoneConfigurationEdit") = True
                Session("mMachineZoneConfigurationID") = mMachineZoneConfigurationID
                Session("mMachine") = mMachine
        End Select
        upnlValidation.Update()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        RemoveSession()
        ' Response.Redirect("wfMachine.aspx?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub

    Private Sub dgZoneConfigurationList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgZoneConfigurationList.PageIndexChanging
        dgZoneConfigurationList.PageIndex = e.NewPageIndex

        mMachine = Session("mMachine")
        SetGridView()
     
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
            
    End Sub


    Private Sub dgZoneConfigurationList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgZoneConfigurationList.Sorting
        mMachine.MachineZoneConfigurations.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)

        mMachine = Session("mMachine")
        SetGridView()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region



End Class