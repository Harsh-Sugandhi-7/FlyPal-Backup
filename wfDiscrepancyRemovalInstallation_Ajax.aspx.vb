Public Class wfDiscrepancyRemovalInstallation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mPartListForCombo As PartListForCombo
    Protected mMELSnagCorrectiveAction As MELSnagCorrectiveAction
    Protected mnWO As nWO

    Public mRemovalReasonList As RemovalReasonList
    Public mPartListForSerialNos As PartListForSerialNos
    Public mnWOModelNameValueList As nWOModelNameValueList

    Public mnWOModelListForSerialNos As nWOModelListForSerialNos

    Dim ComponentIndex As Integer
    Dim ComponentName As String
#End Region

#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mMELSnagCorrectiveAction = Session("mDiscrepancyCorrectiveAction")
        mnWOModelNameValueList = Session("mnWOModelNameValueList")
        mRemovalReasonList = Session("mRemovalReasonList")
        mPartListForCombo = Session("mPartListForCombo")
        mnWOModelListForSerialNos = Session("mnWOModelListForSerialNos")
        mPartListForSerialNos = Session("mPartListForSerialNos")
    End Sub
    Private Sub SetSession()
        Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
        Session("mnWOModelNameValueList") = mnWOModelNameValueList
        Session("mPartListForCombo") = mPartListForCombo
        Session("mRemovalReasonList") = mRemovalReasonList
        Session("mnWOModelListForSerialNos") = mnWOModelListForSerialNos
        Session("mPartListForSerialNos") = mPartListForSerialNos
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "DiscrepancyAction"
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
        End Select
    End Function

    Private Sub DataFieldBind()
        ''Off Part List
        mPartListForCombo = PartListForCombo.GetPartListForCombo(Guid.Empty, "", , , "(SELECT)")
            cmbOffPartList.DataSource = mPartListForCombo
            Session("mPartListForCombo") = mPartListForCombo

            'On Part List
            cmbOnPartList.DataSource = mPartListForCombo


        SetLabels(False)
        'Removal Reason List
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList(, "(SELECT)")
        cmbRemovalReason.DataSource = mRemovalReasonList
        Session("mRemovalReasonList") = mRemovalReasonList

        'Removal/Installation Grid 
        dgRemovalInstallation.DataSource = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations

        If cmbOffPartList.SelectedIndex > 0 Then
            cmbOffSerialNo.Enabled = True
        Else
            cmbOffSerialNo.Enabled = False
        End If
        Call cmbOffPartList_SelectedIndexChanged(Nothing, Nothing)

        DataBind()
    End Sub
    Private Sub chkIsRemoval()
        If chkRemoval.Checked = True Then
            cmbOffPartList.Enabled = True
            txtOffPartNo.ReadOnly = False
            txtOffDescription.ReadOnly = False
            txtOffDescription.Enabled = True

            txtOffSerialNo.ReadOnly = False
            cmbOffSerialNo.Enabled = True

            txtOffRemark.ReadOnly = False
            txtOffRemark.Enabled = True

            cmbRemovalReason.Enabled = True
            txtOffTSN.ReadOnly = False
            txtOffTSN.Enabled = True

            txtOffCSN.ReadOnly = False
            txtOffCSN.Enabled = True

            txtOffPosition.ReadOnly = False
            txtOffPosition.Enabled = True


            txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffPosition.BackColor = Color.FromKnownColor(KnownColor.White)

            tblRem.BgColor = "#FFFFFF"
        Else
            cmbOffPartList.Enabled = False
            txtOffPartNo.ReadOnly = True

            txtOffDescription.ReadOnly = True
            txtOffDescription.Enabled = False

            txtOffSerialNo.ReadOnly = True
            cmbOffSerialNo.Enabled = False

            txtOffRemark.ReadOnly = True
            txtOffRemark.Enabled = False

            cmbRemovalReason.Enabled = False

            txtOffTSN.ReadOnly = True
            txtOffTSN.Enabled = False

            txtOffCSN.ReadOnly = True
            txtOffCSN.Enabled = False

            txtOffPosition.ReadOnly = True
            txtOffPosition.Enabled = False

            txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.Silver)

            txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOffPosition.BackColor = Color.FromKnownColor(KnownColor.Silver)
            cmbOffPartList.ClearSelection()
            txtOffPartNo.Text = ""
            txtOffDescription.Text = ""
            txtOffSerialNo.Text = ""
            txtOffRemark.Text = ""
            cmbRemovalReason.ClearSelection()
            txtOffTSN.Text = ""
            txtOffCSN.Text = ""
            cmbOffSerialNo.ClearSelection()

            tblRem.BgColor = "E0E0E0"
        End If
    End Sub
    Private Sub chkIsIntallation()
        If chkInstallation.Checked = True Then

            cmbOnPartList.Enabled = True
            txtOnPartNo.ReadOnly = False
            txtOnPartNo.Enabled = True
            txtOnDescription.ReadOnly = False
            txtOnDescription.Enabled = True

            txtOnSerialNo.ReadOnly = False
            txtOnSerialNo.Enabled = True

            txtOnRemark.ReadOnly = False
            txtOnRemark.Enabled = True

            txtOnTSN.ReadOnly = False
            txtOnTSN.Enabled = True

            txtOnCSN.ReadOnly = False
            txtOnCSN.Enabled = True

            txtOnPosition.ReadOnly = False
            txtOnPosition.Enabled = True
            txtGRN.ReadOnly = False
            txtGRN.Enabled = True

            txtFormNo.ReadOnly = False
            txtFormNo.Enabled = True

            'End

            txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnPosition.BackColor = Color.FromKnownColor(KnownColor.White)
            txtGRN.BackColor = Color.FromKnownColor(KnownColor.White)
            txtFormNo.BackColor = Color.FromKnownColor(KnownColor.White)
            tblInst.BgColor = "#FFFFFF"
        Else
            cmbOnPartList.Enabled = False
            txtOnPartNo.ReadOnly = True
            txtOnPartNo.Enabled = False
            txtOnDescription.ReadOnly = True
            txtOnDescription.Enabled = False

            txtOnSerialNo.ReadOnly = True
            txtOnSerialNo.Enabled = False

            txtOnRemark.ReadOnly = True
            txtOnRemark.Enabled = False

            txtOnTSN.ReadOnly = True
            txtOnTSN.Enabled = False

            txtOnCSN.ReadOnly = True
            txtOnCSN.Enabled = False

            txtOnPosition.ReadOnly = True
            txtOnPosition.Enabled = False
            txtGRN.ReadOnly = True
            txtGRN.Enabled = False

            txtFormNo.ReadOnly = True
            txtFormNo.Enabled = False


            txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.Silver)

            txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.Silver) '"E0E0E0"
            txtOnPosition.BackColor = Color.FromKnownColor(KnownColor.Silver)

            txtGRN.BackColor = Color.FromKnownColor(KnownColor.Silver)
            txtFormNo.BackColor = Color.FromKnownColor(KnownColor.Silver)
            cmbOnPartList.ClearSelection()
            txtOnPartNo.Text = ""
            txtOnDescription.Text = ""
            txtOnSerialNo.Text = ""
            txtOnRemark.Text = ""
            txtOnTSN.Text = ""
            txtOnCSN.Text = ""
            tblInst.BgColor = "E0E0E0"
            txtGRN.Text = ""
            txtFormNo.Text = ""
        End If
    End Sub
    Private Sub SetEnability(ByVal IsInstall As Boolean, ByVal IsRemoval As Boolean)
        If IsInstall = True Then
            cmbOnPartList.Enabled = True
            If cmbOnPartList.SelectedIndex <= 0 Then
                txtOnPartNo.ReadOnly = False
                txtOnPartNo.Enabled = True

                txtOnDescription.ReadOnly = False
                txtOnDescription.Enabled = True

            End If
            txtOnSerialNo.ReadOnly = False
            txtOnSerialNo.Enabled = True

            txtOnRemark.ReadOnly = False
            txtOnRemark.Enabled = True

            txtOnTSN.ReadOnly = False
            txtOnTSN.Enabled = True

            txtOnCSN.ReadOnly = False
            txtOnCSN.Enabled = True
        Else
            cmbOnPartList.Enabled = False

            txtOnPartNo.ReadOnly = True
            txtOnPartNo.Enabled = False
            txtOnDescription.ReadOnly = True
            txtOnDescription.Enabled = False

            txtOnSerialNo.ReadOnly = True
            txtOnSerialNo.Enabled = False

            txtOnRemark.ReadOnly = True
            txtOnRemark.Enabled = False

            txtOnTSN.ReadOnly = True
            txtOnTSN.Enabled = False

            txtOnCSN.ReadOnly = True
            txtOnCSN.Enabled = False

            txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.White)

            cmbOnPartList.ClearSelection()
            txtOnPartNo.Text = ""
            txtOnDescription.Text = ""
            txtOnSerialNo.Text = ""
            txtOnRemark.Text = ""
            txtOnTSN.Text = ""
            txtOnCSN.Text = ""
            txtGRN.Text = ""
            txtFormNo.Text = ""
        End If

        If IsRemoval = True Then

            cmbOffPartList.Enabled = True

            If cmbOffPartList.SelectedIndex <= 0 Then
                txtOffPartNo.ReadOnly = False
                txtOffDescription.ReadOnly = False
                txtOffDescription.Enabled = True

                txtOffSerialNo.ReadOnly = False
            End If

            txtOffRemark.ReadOnly = False
            txtOffRemark.Enabled = True

            cmbRemovalReason.Enabled = True
            txtOffTSN.ReadOnly = False
            txtOffTSN.Enabled = True

            txtOffCSN.ReadOnly = False
            txtOffCSN.Enabled = True
        Else
            cmbOffPartList.Enabled = False
            txtOffPartNo.ReadOnly = True

            txtOffDescription.ReadOnly = True
            txtOffDescription.Enabled = False

            txtOffSerialNo.ReadOnly = True
            cmbOffSerialNo.Enabled = False

            txtOffRemark.ReadOnly = True
            txtOffRemark.Enabled = False

            cmbRemovalReason.Enabled = False

            txtOffTSN.ReadOnly = True
            txtOffTSN.Enabled = False

            txtOffCSN.ReadOnly = True
            txtOffCSN.Enabled = False

            txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.White)
            cmbRemovalReason.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.White)
            txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.White)

            cmbOffPartList.ClearSelection()
            txtOffPartNo.Text = ""
            txtOffDescription.Text = ""
            txtOffSerialNo.Text = ""
            txtOffRemark.Text = ""
            cmbRemovalReason.ClearSelection()
            txtOffTSN.Text = ""
            txtOffCSN.Text = ""
            cmbOffSerialNo.ClearSelection()
        End If
    End Sub
    Private Sub SetLabels(ByVal IsAssembly As Boolean)
        If IsAssembly = False Then
            lblOffPartList.Text = "Part No."
            lblOffPartNo.Text = "Part Number"
            lblOffDescription.Text = "Part Description"

            lblOnPartList.Text = "Part No."
            lblOnPartNo.Text = "Part Number"
            lblOnDescription.Text = "Part Description"

            txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
            txtOnPartNo.ToolTip = "Enter Part Name for Installed Component"

            txtOffDescription.ToolTip = "Enter Description for Removed Component"
            txtOnDescription.ToolTip = "Enter Description for Installed Component"

            txtOffSerialNo.ToolTip = "Enter Serial Number for Removed Component"
            txtOnSerialNo.ToolTip = "Enter Serial Number for Installed Component"

            txtOffRemark.ToolTip = "Enter Remark for Removed Component"
            txtOnRemark.ToolTip = "Enter Remark for Installed Component"

            txtOffTSN.ToolTip = "Enter TSN for Removed Component"
            txtOnTSN.ToolTip = "Enter TSN for Installed Component"

            txtOffCSN.ToolTip = "Enter CSN for Removed Component"
            txtOnCSN.ToolTip = "Enter CSN for Installed Component"

        Else
            lblOffPartList.Text = "Model No."
            lblOffPartNo.Text = "Model Name"
            lblOffDescription.Text = "Assembly  Description"  'Added by shital on 30-Oct-2020'

            lblOnPartList.Text = "Model No."
            lblOnPartNo.Text = "Model Name"
            lblOnDescription.Text = "Assembly  Description"  'Added by shital on 30-Oct-2020'

            txtOffPartNo.ToolTip = "Enter Model Name for Removed Assembly"
            txtOnPartNo.ToolTip = "Enter Model Name for Installed Assembly"

            txtOffDescription.ToolTip = "Enter Description for Removed Assembly"
            txtOnDescription.ToolTip = "Enter Description for Installed Assembly"

            txtOffSerialNo.ToolTip = "Enter Serial Number for Removed Assembly"
            txtOnSerialNo.ToolTip = "Enter Serial Number for Installed Assembly"

            txtOffRemark.ToolTip = "Enter Remark for Removed Assembly"
            txtOnRemark.ToolTip = "Enter Remark for Installed Assembly"

            txtOffTSN.ToolTip = "Enter TSN for Removed Assembly"
            txtOnTSN.ToolTip = "Enter TSN for Installed Assembly"

            txtOffCSN.ToolTip = "Enter CSN for Removed Assembly"
            txtOnCSN.ToolTip = "Enter CSN for Installed Assembly"
        End If
    End Sub
    Private Sub OnPartSelection()
        If cmbOnPartList.SelectedIndex <= 0 Then

            txtOnSerialNo.Enabled = True


            txtOnPartNo.ReadOnly = False
            txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOnDescription.ReadOnly = False
            txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)

        Else
            txtOnPartNo.ReadOnly = True
            txtOnPartNo.BackColor = Color.Gainsboro

            txtOnDescription.ReadOnly = True
            txtOnDescription.BackColor = Color.Gainsboro

            'COMPONENT
            mPartListForCombo = Session("mPartListForCombo")
                txtOnPartNo.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, "")
                txtOnDescription.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Description, "")

            End If
    End Sub
    Private Sub OffPartSelection()
        If cmbOffPartList.SelectedIndex <= 0 Then
            cmbOffSerialNo.Enabled = False
            'cmbOffSerialNo.SelectedIndex = 0
            cmbOffSerialNo.ClearSelection()
            ComponentIndex = cmbOffPartList.SelectedIndex
            Session("ComponentIndex") = ComponentIndex

            txtOffSerialNo.Enabled = True
            txtOffPartNo.ReadOnly = False
            txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOffDescription.ReadOnly = False
            txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
            txtOffDescription.ToolTip = "Enter Description for Removed Component"
        Else
            txtOffPartNo.ReadOnly = True
            txtOffPartNo.BackColor = Color.Gainsboro

            txtOffDescription.ReadOnly = True
            txtOffDescription.BackColor = Color.Gainsboro

            'COMPONENT
            mPartListForCombo = Session("mPartListForCombo")

                cmbOffSerialNo.Enabled = True

                ComponentName = cmbOffPartList.SelectedValue.ToString

                mPartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(mPartListForCombo(New Guid(ComponentName)).Name, "", Today.Date.ToString, , "(SELECT)")
                Session("mPartListForSerialNos") = mPartListForSerialNos

                txtOffPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Name, "")
                txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Description, "")

                txtOffPartNo.ToolTip = "Part Name for Removed Component"
                txtOffDescription.ToolTip = "Description for Removed Component"


                If mPartListForSerialNos.Count > 1 Then
                    If Not mPartListForSerialNos(1).SerialNo = "" Then
                        cmbOffSerialNo.DataSource = mPartListForSerialNos
                        cmbOffSerialNo.DataBind()
                    Else
                        cmbOffSerialNo.Items.Clear()
                        cmbOffSerialNo.Items.Add("(SELECT)")
                        cmbOffSerialNo.DataBind()
                    End If
                Else
                    cmbOffSerialNo.Items.Clear()
                    cmbOffSerialNo.Items.Add("(SELECT)")
                    cmbOffSerialNo.DataBind()
                End If

                txtOffPartNo.DataBind()
            txtOffDescription.DataBind()
            Session("mnWOModelListForSerialNos") = mnWOModelListForSerialNos
            ComponentIndex = cmbOffPartList.SelectedIndex
            Session("ComponentIndex") = ComponentIndex
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Confirm" Then
                        Session("sender") = ""
                        If Session("mWOJobCompsEdit") = True Then
                            mMELSnagCorrectiveAction = Session("mDiscrepancyCorrectiveAction")
                            DataFieldBind()
                            SetControl(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex)
                            Session("mWOJobCompsEdit") = False
                        End If
                        If Not Save() Then
                            Exit Sub
                        End If
                        SetTitle()
                        If mMELSnagCorrectiveAction.InvestigationStatus = False Then
                            ControlVisibility()
                        End If

                        CallUpdatePanels()
                    ElseIf MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mMELSnagCorrectiveAction = Session("mDiscrepancyCorrectiveAction")
                            mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Remove(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex)
                            For i As Integer = 0 To mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Count - 1
                                mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations(i).SrNo = i + 1
                            Next
                            Session("mnWO") = mnWO
                            Session("mWOJobCompsEdit") = False
                            'Removal/Installation Grid 
                            dgRemovalInstallation.DataSource = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations
                            DataFieldBind()
                            SetTitle()
                            If mMELSnagCorrectiveAction.InvestigationStatus = False Then
                                ControlVisibility()
                            End If
                            ClearControls()
                            CallUpdatePanels()
                            'If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Count.ToString + "');", True)
                        Catch ex As SqlException
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        Session("mWOJobCompsEdit") = False
                        DataFieldBind()
                        SetTitle()
                        'mMELSnagCorrectiveAction.InvestigationStatus = True Then
                        '    ControlVisibility()
                        'End If
                        chkInstallation.Checked = True
                        chkRemoval.Checked = True
                        SetEnability(chkInstallation.Checked, chkRemoval.Checked)
                        chkIsRemoval()
                        chkIsIntallation()
                        ClearAllControls()
                        CallUpdatePanels()

                    ElseIf MSGBoxCtrl.Sender = "Confirm" Then
                        If Session("mWOJobCompsEdit") = False Then
                            mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Remove(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem)
                        Else
                            mMELSnagCorrectiveAction = Session("mnWOJobClone")
                        End If

                        Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
                        Session("sender") = ""
                        Session("mWOJobCompsEdit") = False
                        DataFieldBind()
                        SetTitle()
                        'mMELSnagCorrectiveAction.InvestigationStatus = True Then
                        '    ControlVisibility()
                        'End If
                        ClearControls()
                        CallUpdatePanels()
                        Session("mWOJobCompsEdit") = False
                    End If

                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                    SetTitle()
                    'mMELSnagCorrectiveAction.InvestigationStatus = True Then
                    '    ControlVisibility()
                    'End If
                    CallUpdatePanels()

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
                    SetTitle()
                    'mMELSnagCorrectiveAction.InvestigationStatus = True Then
                    '    ControlVisibility()
                    'End If
                    CallUpdatePanels()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            SetTitle()
            'mMELSnagCorrectiveAction.InvestigationStatus = True Then
            '    ControlVisibility()
            'End If
            CallUpdatePanels()
        ElseIf Result1 = 0 Then

        End If
    End Sub
    Private Sub ControlVisibility()
        If mMELSnagCorrectiveAction.InvestigationStatus = True Then
            chkRemoval.Visible = False
            chkInstallation.Visible = False
            cmbOffPartList.Visible = False
            cmbOnPartList.Visible = False
            txtOffPartNo.Visible = False
            txtOnPartNo.Visible = False
            txtOffDescription.Visible = False
            txtOnDescription.Visible = False
            txtOffSerialNo.Visible = False
            txtOnSerialNo.Visible = False
            txtOffRemark.Visible = False
            txtOnRemark.Visible = False
            cmbRemovalReason.Visible = False
            txtOffTSN.Visible = False
            txtOnTSN.Visible = False
            txtOffCSN.Visible = False
            txtOnCSN.Visible = False
        Else
            chkRemoval.Visible = True
            chkInstallation.Visible = True
            cmbOffPartList.Visible = True
            cmbOnPartList.Visible = True
            txtOffPartNo.Visible = True
            txtOnPartNo.Visible = True
            txtOffDescription.Visible = True
            txtOnDescription.Visible = True
            txtOffSerialNo.Visible = True
            txtOnSerialNo.Visible = True
            txtOffRemark.Visible = True
            txtOnRemark.Visible = True
            cmbRemovalReason.Visible = True
            txtOffTSN.Visible = True
            txtOnTSN.Visible = True
            txtOffCSN.Visible = True
            txtOnCSN.Visible = True
        End If
        If Session("mWOJobCompsEdit") = True Then
            chkRemoval.Enabled = False
            chkInstallation.Enabled = False
        Else
            chkRemoval.Enabled = True
            chkInstallation.Enabled = True
        End If
    End Sub
    Private Sub SetTitle()
        txtJobDescription.Text = mMELSnagCorrectiveAction.Defect

        If Session("mWOJobCompsEdit") = True Then
            chkRemoval.Enabled = False
            chkInstallation.Enabled = False
        Else
            chkRemoval.Enabled = True
            chkInstallation.Enabled = True
        End If

    End Sub
    Private Sub SetObject()
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForRemoval = chkRemoval.Checked
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffPartID = New Guid(cmbOffPartList.SelectedValue.ToString)

        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffRemark = Trim(txtOffRemark.Text)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.RemovalReasonID = New Guid(cmbRemovalReason.SelectedValue.ToString)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffTSN = Trim(txtOffTSN.Text)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffCSN = Trim(txtOffCSN.Text)

        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForInstall = chkInstallation.Checked
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnRemark = Trim(txtOnRemark.Text)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnTSN = Trim(txtOnTSN.Text)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnCSN = Trim(txtOnCSN.Text)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnSerialNo = Trim(txtOnSerialNo.Text)


        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffPartNo = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOffPartList.SelectedValue.ToString)).Name, Trim(txtOffPartNo.Text))
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffDescription = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOffPartList.SelectedValue.ToString)).Description, Trim(txtOffDescription.Text))
        If cmbOffPartList.SelectedIndex > 0 Then
            If cmbOffSerialNo.SelectedIndex > 0 Then
                mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffSerialNo = mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo
                Dim mCompStatusList As CompStatusList = CompStatusList.GetCompStatusList(Guid.Empty, CurrentDate:=Today.Date.ToString,
                                                                                             CompID:=mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).CompID.ToString,
                                                                                             PartName:=mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffPartNo,
                                                                                             CompSerialNo:=mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffSerialNo,
                                                                                             IsCompInstalled:=True, IsCompPeriodsRequired:=False)
                If mCompStatusList.Count = 1 Then
                    mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.CompStatusOffID = mCompStatusList(0).ID
                End If


            Else
                mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
            End If
        Else
            mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffSerialNo = Trim(txtOffSerialNo.Text)
        End If


        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnPartID = New Guid(cmbOnPartList.SelectedValue.ToString)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnPartNo = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, Trim(txtOnPartNo.Text))
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnDescription = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Description, Trim(txtOnDescription.Text))

        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OffPosition = Trim(txtOffPosition.Text)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.OnPosition = Trim(txtOnPosition.Text)

        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.GRN = Trim(txtGRN.Text)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.FormNo = Trim(txtFormNo.Text)

        If Session("IsFromMessageBox") = True Then
            Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
        End If
    End Sub
    Private Sub SetControl(ByVal Index As Int32)
        chkRemoval.Checked = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).IsForRemoval

        'OFF Part
        cmbOffPartList.SelectedValue = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffPartID.ToString
        txtOffPartNo.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffPartNo
        txtOffDescription.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffDescription
        txtOffSerialNo.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffSerialNo
        txtOffRemark.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffRemark
        cmbRemovalReason.SelectedValue = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).RemovalReasonID.ToString
        txtOffTSN.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffTSN
        txtOffCSN.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffCSN

        'ON Part
        chkInstallation.Checked = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).IsForInstall
        cmbOnPartList.SelectedValue = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnPartID.ToString
        txtOnPartNo.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnPartNo()
        txtOnDescription.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnDescription
        txtOnSerialNo.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnSerialNo
        txtOnRemark.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnRemark
        txtOnTSN.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnTSN
        txtOnCSN.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnCSN

        txtOffPosition.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OffPosition
        txtOnPosition.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).OnPosition
        txtGRN.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).GRN
        txtFormNo.Text = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).FormNo
        OffPartSelection()
        OnPartSelection()
        cmbOffSerialNo.DataBind()
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex = Index
        SetControl(Index)
        setFocus(cmbOffPartList)
        Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
        Session("JobCompEdit") = True
        dgRemovalInstallation.DataSource = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        MSGBoxCtrl.Show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex = Index
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        If mMELSnagCorrectiveAction.IsValid Then
            For i As Integer = 0 To mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        If ((mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Contains(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem, "") And mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForRemoval = True)) Then
            strMSG = strMSG + "This Removal Entry is already done" + "<Br>"
        End If

        If ((mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Contains(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem, "", "") And mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForInstall = True)) Then
            strMSG = strMSG + "This Installation Entry is already done" + "<Br>"
        End If

        If (cmbOffPartList.SelectedIndex <= 0 And txtOffPartNo.Text = "" And chkRemoval.Checked = True) Then
            strMSG = strMSG + "Select or enter the Part to be removed." + "<Br>"
        End If

        If cmbOnPartList.SelectedIndex <= 0 And txtOnPartNo.Text = "" And chkInstallation.Checked = True Then
            strMSG = strMSG + "Select or enter the Part to be installed." + "<Br>"
        End If

        If strMSG.Trim <> "" Then
            cvControlValidator.ErrorMessage = strMSG
            cvControlValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbOffPartList" Then
            If cmbOffPartList.SelectedIndex <= 0 And chkRemoval.Checked = True Then
                custValidator.ErrorMessage = "Select the Part to be removed."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbOnPartList" Then
            If cmbOnPartList.SelectedIndex <= 0 And chkInstallation.Checked = True Then
                custValidator.ErrorMessage = "Select the Part to be installed."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "chkRemoval" Or custValidator.ControlToValidate = "chkInstallation" Then
            If chkRemoval.Checked = False And chkInstallation.Checked = False Then
                custValidator.ErrorMessage = "Atleast select one Removal/Installation"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Function Save() As Boolean
        If Session("mWOJobCompsEdit") = False Then
            If Session("IsFromMessageBox") = False Then
                mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Add(mMELSnagCorrectiveAction.ID)
                SetObject()
            End If
            If Not CustomValidate1() Then
                upnlValidationSummary.Update()
                mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Remove(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem)
                Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
                Return False
            End If

            If (mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsValid) And ((Not mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Contains(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem, "") And mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForRemoval = True)) Or ((Not mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Contains(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem, "", "") And mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForInstall = True)) Then
                mMELSnagCorrectiveAction.ApplyEdit()
                dgRemovalInstallation.DataSource = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations
                dgRemovalInstallation.DataBind()
                Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
                SetTitle()
                'mMELSnagCorrectiveAction.InvestigationStatus = True Then
                '    ControlVisibility()
                'End If
                upnlGrid.Update()
                upnlTitle.Update()
                ClearControls()
                Return True
            Else
                If Not CustomValidate1() Then
                    upnlValidationSummary.Update()
                    mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Remove(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem)
                    Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
                    Return False
                End If
            End If
            SetEnability(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForInstall, mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForRemoval)
        Else
            If Session("IsFromMessageBox") = False Then
                SetObject()
            End If

            If Not CustomValidate1() Then upnlValidationSummary.Update() : Return False

            If (mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsValid) And ((Not mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Contains(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem, "") And mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForRemoval = True)) Or ((Not mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Contains(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem, "", "") And mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForInstall = True)) Then
                dgRemovalInstallation.DataSource = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations
                dgRemovalInstallation.DataBind()
                Session("mnWO") = mnWO
                setFocus(cmbOffPartList)
                Session("mWOJobCompsEdit") = False
                SetEnability(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForInstall, mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.IsForRemoval)
                ClearControls()
                Return True
            Else
                If Not CustomValidate1() Then upnlValidationSummary.Update() : Return False
            End If
        End If
        Return False
    End Function
    Private Sub CallUpdatePanels()
        upnlInst.Update()
        upnlGrid.Update()
        upnlRemoval.Update()
        upnlTitle.Update()
        upnlValidationSummary.Update()
    End Sub
    Private Sub ClearAllControls()
        cmbOnPartList.ClearSelection()
        txtOnPartNo.Text = ""
        txtOnDescription.Text = ""
        txtOnSerialNo.Text = ""
        txtOnRemark.Text = ""
        txtOnTSN.Text = ""
        txtOnCSN.Text = ""
        txtOnPosition.Text = ""
        txtGRN.Text = ""
        txtFormNo.Text = ""

        cmbOffPartList.ClearSelection()
        cmbRemovalReason.ClearSelection()
        cmbOffSerialNo.ClearSelection()
        txtOffPartNo.Text = ""
        txtOffDescription.Text = ""
        txtOffSerialNo.Text = ""
        txtOffRemark.Text = ""
        txtOffTSN.Text = ""
        txtOffCSN.Text = ""
        txtOffPosition.Text = ""
    End Sub
    Private Sub ClearControls()
        chkInstallation.Checked = True
        chkRemoval.Checked = True
        cmbOnPartList.Enabled = True
        txtOnPartNo.ReadOnly = False
        txtOnPartNo.Enabled = True
        txtOnDescription.ReadOnly = False
        txtOnDescription.Enabled = True

        txtOnSerialNo.ReadOnly = False
        txtOnSerialNo.Enabled = True

        txtOnRemark.ReadOnly = False
        txtOnRemark.Enabled = True

        txtOnTSN.ReadOnly = False
        txtOnTSN.Enabled = True

        txtOnCSN.ReadOnly = False
        txtOnCSN.Enabled = True

        txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOnSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOnRemark.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOnTSN.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOnCSN.BackColor = Color.FromKnownColor(KnownColor.White)

        cmbOnPartList.ClearSelection()
        txtOnPartNo.Text = ""
        txtOnDescription.Text = ""
        txtOnSerialNo.Text = ""
        txtOnRemark.Text = ""
        txtOnTSN.Text = ""
        txtOnCSN.Text = ""
        txtOnPosition.Text = ""
        txtGRN.Text = ""
        txtFormNo.Text = ""


        cmbOffPartList.Enabled = True
        txtOffPartNo.ReadOnly = False
        txtOffDescription.ReadOnly = False
        txtOffDescription.Enabled = True

        txtOffSerialNo.ReadOnly = False
        cmbOffSerialNo.Enabled = True

        txtOffRemark.ReadOnly = False
        txtOffRemark.Enabled = True

        cmbRemovalReason.Enabled = True
        txtOffTSN.ReadOnly = False
        txtOffTSN.Enabled = True

        txtOffCSN.ReadOnly = False
        txtOffCSN.Enabled = True

        txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

        txtOffRemark.BackColor = Color.FromKnownColor(KnownColor.White)
        cmbRemovalReason.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOffTSN.BackColor = Color.FromKnownColor(KnownColor.White)
        txtOffCSN.BackColor = Color.FromKnownColor(KnownColor.White)

        cmbOffPartList.ClearSelection()
        cmbRemovalReason.ClearSelection()
        cmbOffSerialNo.ClearSelection()
        txtOffPartNo.Text = ""
        txtOffDescription.Text = ""
        txtOffSerialNo.Text = ""
        txtOffRemark.Text = ""
        txtOffTSN.Text = ""
        txtOffCSN.Text = ""
        txtOffPosition.Text = ""
    End Sub
    Private Sub ControlVisibilityForStar()
        'If mMELSnagCorrectiveAction.WOJobTypeID = 1 Or mMELSnagCorrectiveAction.WOJobTypeID = 5 Then
        '    Label1.Visible = False
        '    Label3.Visible = False
        '    Label4.Visible = False
        '    Label5.Visible = False
        'Else
        '    Label1.Visible = True
        '    Label3.Visible = True
        '    Label4.Visible = True
        '    Label5.Visible = True
        'End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbOffPartList.Enabled = True Then
                setFocus(cmbOffPartList)
            End If
            DataFieldBind()
            chkRemoval.Checked = True
            chkInstallation.Checked = True

            If Session("mWOJobCompsEdit") = True Then
                EditRecord(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex)
                dgRemovalInstallation.DataBind()
                Session("IsInstall") = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex).IsForInstall
                Session("IsRemove") = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex).IsForRemoval
                SetEnability(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex).IsForInstall, mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentIndex).IsForRemoval)
            End If
        End If
        SetTitle()
        'If mMELSnagCorrectiveAction.InvestigationStatus = True Then
        '    ControlVisibility()
        'End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTop.Click
        If (Not IsInRole(Rights.[New]) And mMELSnagCorrectiveAction.IsNew) Or (Not IsInRole(Rights.Edit) And Not mMELSnagCorrectiveAction.IsNew) Then
            SetSession()
            MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        If Not Page.IsValid() Then upnlValidationSummary.Update() : Exit Sub
        If String.Compare(Trim(txtOffPosition.Text), Trim(txtOnPosition.Text), True) <> 0 And (chkInstallation.Checked = True And chkRemoval.Checked = True) Then
            Dim mMELSnagCorrectiveActionClone As MELSnagCorrectiveAction
            mMELSnagCorrectiveActionClone = CType(mMELSnagCorrectiveAction.Clone, MELSnagCorrectiveAction)
            Session("mMELSnagCorrectiveActionClone") = mMELSnagCorrectiveActionClone
            Session("IsFromMessageBox") = True
            If Session("mWOJobCompsEdit") = True Then
                SetObject()
            Else
                mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Add(mMELSnagCorrectiveAction.ID)
                SetObject()
            End If

            If Not CustomValidate1() Then
                upnlValidationSummary.Update()
                mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Remove(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem)
                Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction
                Exit Sub
            End If
            MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Off Component Position is not Same as On Component Position." & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "Confirm")
            Exit Sub
        Else 'End
            Session("IsFromMessageBox") = False
            If Not Save() Then
                upnlValidationSummary.Update()
                Exit Sub
            Else
                chkIsRemoval()
                chkIsIntallation()
            End If

        End If
        DataFieldBind()
        If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Count.ToString + "');", True)
        SetTitle()
        'mMELSnagCorrectiveAction.InvestigationStatus = True Then
        '    ControlVisibility()
        'End If
        CallUpdatePanels()
    End Sub
    Private Sub dgRemovalInstallation_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovalInstallation.RowCommand
        Dim Index As Int32 = dgRemovalInstallation.PageIndex * dgRemovalInstallation.PageSize + CInt(e.CommandArgument)
        Dim mID As Guid = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).ID
        Select Case e.CommandName
            Case "EditRecord"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    SetSession()
                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Session("mWOJobCompsEdit") = True
                EditRecord(Index)
                SetEnability(mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).IsForInstall, mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Item(Index).IsForRemoval)

                'mMELSnagCorrectiveAction.InvestigationStatus = True Then
                '    ControlVisibility()
                'End If

                If Session("mWOJobCompsEdit") = True Then
                    chkRemoval.Enabled = False
                    chkInstallation.Enabled = False
                Else
                    chkRemoval.Enabled = True
                    chkInstallation.Enabled = True
                End If
                upnlInst.Update()
                upnlRemoval.Update()
                dgRemovalInstallation.DataBind()
            Case "DeleteRecord"
                If (Not IsInRole(Rights.[New]) And mMELSnagCorrectiveAction.IsNew) Or (Not IsInRole(Rights.Edit) And Not mMELSnagCorrectiveAction.IsNew) Then
                    SetSession()
                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(Index)
        End Select
    End Sub
    Private Sub cmbOffPartList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOffPartList.SelectedIndexChanged
        If cmbOffPartList.SelectedIndex <= 0 Then
            cmbOffSerialNo.Enabled = False
            cmbOffSerialNo.ClearSelection()
            ComponentIndex = cmbOffPartList.SelectedIndex
            Session("ComponentIndex") = ComponentIndex


            txtOffPartNo.Text = ""
            txtOffDescription.Text = ""
            txtOffSerialNo.Enabled = True


            txtOffPartNo.ReadOnly = False
            txtOffPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOffDescription.ReadOnly = False
            txtOffDescription.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
            txtOffDescription.ToolTip = "Enter Description for Removed Component"
            'New
            If chkInstallation.Checked Then
                cmbOnPartList.ClearSelection()
                txtOnPartNo.Text = ""
                txtOnDescription.Text = ""

                txtOnPartNo.ReadOnly = False
                txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

                txtOnDescription.ReadOnly = False
                txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)

                txtOnPartNo.ToolTip = "Enter Part Name for Installed Component"
                txtOnDescription.ToolTip = "Enter Description for Installed Component"
                upnlInst.Update()
            End If
            'End
        Else
            txtOffPartNo.ReadOnly = True
            txtOffPartNo.BackColor = Color.Gainsboro

            txtOffDescription.ReadOnly = True
            txtOffDescription.BackColor = Color.Gainsboro

            'New
            If chkInstallation.Checked Then
                txtOnPartNo.ReadOnly = True
                txtOnPartNo.BackColor = Color.Gainsboro

                txtOnDescription.ReadOnly = True
                txtOnDescription.BackColor = Color.Gainsboro
            End If
            'End
            'COMPONENT
            mPartListForCombo = Session("mPartListForCombo")

            cmbOffSerialNo.Enabled = True

            ComponentName = cmbOffPartList.SelectedValue.ToString

            mPartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(mPartListForCombo(New Guid(ComponentName)).Name, "", Today.Date.ToString, , "(SELECT)")
            Session("mPartListForSerialNos") = mPartListForSerialNos

            txtOffPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Name, "")
            txtOffDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Description, "")

            txtOffPartNo.ToolTip = "Part Name for Removed Component"
            txtOffDescription.ToolTip = "Description for Removed Component"

            'New
            If chkInstallation.Checked Then
                cmbOnPartList.SelectedValue = cmbOffPartList.SelectedValue.ToString
                txtOnPartNo.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Name, "")
                txtOnDescription.Text = IIf(cmbOffPartList.SelectedIndex > 0, mPartListForCombo(New Guid(ComponentName)).Description, "")

                txtOnPartNo.ToolTip = "Part Name for Installed Component"
                txtOnDescription.ToolTip = "Description for Installed Component"
                cmbOnPartList.DataBind()
                txtOnPartNo.DataBind()
                txtOnDescription.DataBind()
                upnlInst.Update()
            End If

            'End
            If mPartListForSerialNos.Count > 1 Then
                If Not mPartListForSerialNos(1).SerialNo = "" Then
                    cmbOffSerialNo.DataSource = mPartListForSerialNos
                    cmbOffSerialNo.DataBind()
                Else
                    cmbOffSerialNo.Items.Clear()
                    cmbOffSerialNo.Items.Add("(SELECT)")
                    cmbOffSerialNo.DataBind()
                End If
            Else
                cmbOffSerialNo.Items.Clear()
                cmbOffSerialNo.Items.Add("(SELECT)")
                cmbOffSerialNo.DataBind()
            End If


            txtOffPartNo.DataBind()
            txtOffDescription.DataBind()
            Session("mnWOModelListForSerialNos") = mnWOModelListForSerialNos
            ComponentIndex = cmbOffPartList.SelectedIndex
            Session("ComponentIndex") = ComponentIndex
        End If
        cmbOffSerialNo_SelectedIndexChanged(sender, e)
    End Sub
    Private Sub cmbOffSerialNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOffSerialNo.SelectedIndexChanged
        If cmbOffSerialNo.SelectedIndex <= 0 Then
            txtOffSerialNo.Text = ""

            txtOffSerialNo.Enabled = True
            txtOffSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)



        Else
            txtOffSerialNo.Enabled = True
            txtOffSerialNo.BackColor = Color.Gainsboro
            'COMPONENT
            mPartListForSerialNos = Session("mPartListForSerialNos")
            txtOffSerialNo.Text = mPartListForSerialNos(New Guid(cmbOffSerialNo.SelectedValue.ToString)).SerialNo

            txtOffSerialNo.ToolTip = "Serial Number for Removed Component"
        End If

        txtOffSerialNo.DataBind()
    End Sub
    Private Sub cmbOnPartList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOnPartList.SelectedIndexChanged
        If cmbOnPartList.SelectedIndex <= 0 Then

            txtOnPartNo.Text = ""
            txtOnDescription.Text = ""
            txtOnSerialNo.Enabled = True


            txtOnPartNo.ReadOnly = False
            txtOnPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

            txtOnDescription.ReadOnly = False
            txtOnDescription.BackColor = Color.FromKnownColor(KnownColor.White)

        Else
            txtOnPartNo.ReadOnly = True
            txtOnPartNo.BackColor = Color.Gainsboro

            txtOnDescription.ReadOnly = True
            txtOnDescription.BackColor = Color.Gainsboro
        End If


        'COMPONENT
        mPartListForCombo = Session("mPartListForCombo")
        txtOnPartNo.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Name, "")
        txtOnDescription.Text = IIf(cmbOnPartList.SelectedIndex > 0, mPartListForCombo(New Guid(cmbOnPartList.SelectedValue.ToString)).Description, "")

        If cmbOnPartList.SelectedIndex <= 0 Then
            txtOffPartNo.ToolTip = "Enter Part Name for Removed Component"
            txtOffDescription.ToolTip = "Enter Description for Removed Component"
        Else
            txtOffPartNo.ToolTip = "Part Name for Removed Component"
            txtOffDescription.ToolTip = "Description for Removed Component"
        End If


    End Sub
    Private Sub chkRemoval_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRemoval.CheckedChanged
        chkIsRemoval()
    End Sub
    Private Sub chkInstallation_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInstallation.CheckedChanged
        chkIsIntallation()
    End Sub

    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        SetSession()
        Session.Remove("mWOJobCompsEdit")
        If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.Count.ToString + "');", True)
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        'Response.Redirect(Request.QueryString("BackPage2") & "?CPage1=" & Request.QueryString("CPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&Index=-1")
    End Sub
    Private Sub imgReason_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgReason.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemovalReasonWindow", "OpenRemovalReasonWindow()", True)
    End Sub
    Private Sub hdnBtnRemovalReason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemovalReason.Click
        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        cmbRemovalReason.DataSource = mRemovalReasonList
        cmbRemovalReason.DataBind()
        If Session("mWOJobCompsEdit") = True Then
            If Not mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.RemovalReasonID.Equals(Guid.Empty) Then
                cmbRemovalReason.SelectedValue = mMELSnagCorrectiveAction.DiscrepancyRemovalInstallations.CurrentItem.RemovalReasonID.ToString
            End If
        End If
        upnlRemoval.Update()
    End Sub
#End Region


End Class