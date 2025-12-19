Public Class wfMachineFeatureList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mFeatureList As FeatureList
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mFeatureList = CType(Session("mFeatureList"), FeatureList)
     End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mFeatureList") = mFeatureList
     End Sub
    Private Sub RemoveSession()
        Session.Remove("mFeatureList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        Dim mFeature As Feature
        mFeature = Feature.NewFeature()
        txtValue.Text = ""
        Session("mFeature") = mFeature
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mMachine.MachineFeatures.Remove(mMachine.MachineFeatures(mMachine.MachineFeatures.CurrentIndex))
                            For i As Integer = 0 To mMachine.MachineFeatures.Count - 1
                                mMachine.MachineFeatures(i).SerialNo = i + 1
                            Next
                            Session("mMachine") = mMachine
                            NewRecord()
                            DataFieldBind()
                            upnlAircraftFeatureDetails.Update()
                            GridBind()
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.DatabaseException, "", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                upnlAircraftFeatureDetails.Update()
                                Exit Sub
                            End If
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        'DataFieldBind()
                        GridBind()
                        upnlAircraftFeatureDetails.Update()
                    End If
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mFeatureList = FeatureList.GetFeatureList(True, "(SELECT)")
        cmbFeatureList.DataSource = mFeatureList
        Session("mFeatureList") = mFeatureList
        cmbFeatureList.DataBind()
        dgFeatureList.DataSource = mMachine.MachineFeatures
        dgFeatureList.DataBind()
        lblResult.Text = "List of Features: " & mMachine.MachineFeatures.Count & " Record(s)found"
    End Sub
    Private Sub GridBind()
        dgFeatureList.DataSource = mMachine.MachineFeatures
        dgFeatureList.DataBind()
        lblResult.Text = "List of Features: " & mMachine.MachineFeatures.Count & " Record(s)found"
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            If cmbFeatureList.Enabled = True Then
                setFocus(cmbFeatureList)
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If Not IsValid Then upnlValidation.Update() : Exit Sub

        Dim FeatureID As New Guid(cmbFeatureList.SelectedValue)
        If mMachine.MachineFeatures.Contains(FeatureID, "") = False Then
            mMachine.MachineFeatures.Add(mMachine.ID, FeatureID, txtValue.Text)
            For i As Integer = 0 To mMachine.MachineFeatures.Count - 1
                mMachine.MachineFeatures(i).SerialNo = i + 1
            Next
            Session("mMachine") = mMachine
            NewRecord()
            DataFieldBind()
            upnlAircraftFeatureDetails.Update()
            GridBind()
        Else
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Machine Feature already exists, can not be added.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub dgFeatureList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFeatureList.RowCommand
        Select Case e.CommandName
            Case "Remove"
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgFeatureList.PageIndex * dgFeatureList.PageSize
                GridBind()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                mMachine.MachineFeatures.CurrentIndex = index
                Session("mMachine") = mMachine
        End Select
        upnlValidation.Update()
    End Sub
    Protected Sub imgbtnFeature_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnFeature.Click
        ' Response.Redirect("wfFeature_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMachineFeatureList_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFeatureFunction", "CallParentFeatureFunction();", True)

    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        RemoveSession()
        'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class