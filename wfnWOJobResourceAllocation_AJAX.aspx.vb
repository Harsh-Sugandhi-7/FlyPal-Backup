'CREATED By : Saylee
'Dated      : 22-Nov-2013

Public Class wfnWOJobResourceAllocation_AJAX
    Inherits Page

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

#Region " Variable Declaration "

    Public mnWOJobDesignationAllocation As nWOJobDesignationAllocation
    Dim mEmployeeList As EmployeeList
    Dim mDesignationName As String = ""
    Dim mWOJobDesignationAllocationID As Guid
    Protected mnWO As nWO
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 05-Aug-2013 For ALL01082013
    Dim mEmployeeAircraftRightsCount As EmployeeAircraftRightsCount

#End Region

#Region " Helper Methods "

    Public Sub GetSession()
        mnWOJobDesignationAllocation = Session("mWOJobDesignationAllocations")
        mnWO = Session("mnWO")
        mEmployeeList = Session("mEmployeeList")
    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        Try

            If control.Enabled = False Or control.Visible = False Then Exit Sub
            Dim str As String
            str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
            ClientScript.RegisterStartupScript([GetType](), "focusscript", str)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Function IsInRole(CheckFor As Rights) As Boolean

        Dim IsInRoleString As String = ""
        Try

            If AppSettings("ShowNewWOFlow") = "True" Then

                If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then

                    If mnWO.TransTypeID = Trans.WO145 Then
                        IsInRoleString = "WOCreate"
                    Else
                        IsInRoleString = "CAMOWOCreate"
                    End If

                ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then
                    IsInRoleString = "WOPlanning"
                ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
                    IsInRoleString = "WOExecution"
                ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then
                    IsInRoleString = "WOCompletion"
                ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then
                    IsInRoleString = "WOQCApproval"
                ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then
                    IsInRoleString = "WOCAMOUpdate"
                ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then
                    IsInRoleString = "WOBilling"
                End If

            Else

                If mnWO.TransTypeID = Trans.WO145 Then
                    IsInRoleString = "WorkOrder"
                ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then
                    IsInRoleString = "SpareAssemblyWO"
                ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
                    IsInRoleString = "SpareComponentWO"
                ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then
                    IsInRoleString = "EngineeringOrder"
                Else
                    IsInRoleString = "CAMOWO"
                End If

            End If

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

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Private Sub SetObject()

        Try

            mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.ResourceID = New Guid(cmbResource.SelectedValue)
            mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.WOJobDesignationAllocationID = mnWOJobDesignationAllocation.ID
            mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.ResourceActualTime = mnWOJobDesignationAllocation.ActualTime
            'Added on 11-Feb-2020 By Shital
            mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.ResourceMailID = mEmployeeList.Item(cmbResource.SelectedItem.Text, "").Email.ToString

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ControlVisibility()
        dgResourceAllocation.Columns(5).Visible = mnWO.WOStatusID <> 3
    End Sub

    Private Sub EditRecord(Index As Int32)

        Try

            mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentIndex = Index
            Session("CurrentResourceAllocationID") = mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.ID
            txtEstimatedManHours.Text = mnWOJobDesignationAllocation.EstimatedTime
            txtActualTime.Text = mnWOJobDesignationAllocation.WOJobResourceAllocations.Item(Index).WOTotalResourceActualTime
            txtDesignation.Text = mnWOJobDesignationAllocation.DesignationName
            cmbResource.SelectedValue = mnWOJobDesignationAllocation.WOJobResourceAllocations.Item(Index).ResourceID.ToString
            dgResourceAllocation.DataSource = mnWOJobDesignationAllocation.WOJobResourceAllocations

            SetFocus(cmbResource)
            txtEstimatedManHours.DataBind()
            txtActualTime.DataBind()
            cmbResource.DataBind()
            '-- Added By Utkarsh On 21-Jan-2011
            If mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.WOJobResourceDetails.Count > 0 Then
                cmbResource.Enabled = False
            End If
            '-----------------------------------

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DeleteRecord(Index As Int32)

        Try

            MSGBoxCtrl.Show(MSGBox.Message_title.Delete,
                            MSGBox.Message_text.Delete,
                            "",
                            MsgBoxStyle.YesNo,
                            "Delete")

            mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentIndex = Index
            Session("mnWOJobDesignationAllocation") = mnWOJobDesignationAllocation

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        Try

            If Result1 > 0 Then

                Select Case Result1
                    Case MsgBoxResult.Yes

                        If MSGBoxCtrl.Sender = "Delete" Then

                            Try

                                Session("sender") = ""
                                mnWOJobDesignationAllocation = Session("mnWOJobDesignationAllocation")
                                mnWOJobDesignationAllocation.WOJobResourceAllocations.Remove(mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentIndex)

                                For i As Integer = 0 To mnWOJobDesignationAllocation.WOJobResourceAllocations.Count - 1
                                    mnWOJobDesignationAllocation.WOJobResourceAllocations(i).SrNo = i + 1
                                Next

                                Session("mnWOJobDesignationAllocation") = mnWOJobDesignationAllocation
                                Session("mResourceAllocationEdit") = False
                                DataFieldBind()
                                ControlVisibility()
                                updatePanels()

                            Catch ex As SqlException
                                Throw ex.GetBaseException
                            End Try

                        End If

                    Case MsgBoxResult.No

                        ControlVisibility()
                        If MSGBoxCtrl.Sender = "Delete" Then Session.Remove("mResourceAllocationEdit")
                        updatePanels()
                        DataFieldBind()
                        upnlResource.Update()

                    Case MsgBoxResult.Ok

                        DataFieldBind()
                        ControlVisibility()
                        updatePanels()

                    Case MsgBoxResult.Ok And Session("sender") = "Authorization"

                        DataFieldBind()
                        ControlVisibility()
                        updatePanels()

                End Select

            ElseIf Result1 = -1 Then

                DataFieldBind()
                ControlVisibility()
                updatePanels()

            ElseIf Result1 = 0 Then

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Function CustomValidate_Object() As Boolean

        Dim strMSG As String = ""
        Try

            If Not mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.IsValid Then

                For i As Integer = 0 To mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next

            End If

            If strMSG.Trim <> "" Then

                cvControlValidator.ErrorMessage = strMSG
                cvControlValidator.IsValid = False

                Return False

            End If

            Return True

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Private Sub UpdatePanels()

        upnlGridView.Update()
        upnlResource.Update()
        upnlTitle.Update()

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        Try

            If AppSettings("ClientCode") = "IND" Then
                mEmployeeList = EmployeeList.GetEmployeeList("", mnWOJobDesignationAllocation.DesignationName, "(SELECT)")
            Else
                mEmployeeList = EmployeeList.GetEmployeeList("", , "(SELECT)")
            End If

            cmbResource.DataSource = mEmployeeList
            Session("mEmployeeList") = mEmployeeList
            dgResourceAllocation.DataSource = mnWOJobDesignationAllocation.WOJobResourceAllocations
            DataBind()
            txtActualTime.Text = "0:00"

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here
        Try

            GetSession()

            If Not IsPostBack And Session("sender") = "" Then

                If cmbResource.Enabled = True Then
                    SetFocus(cmbResource)
                End If
                DataFieldBind()

            End If

            ControlVisibility()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAddTop.Click

        Try

            'Added by Saylee on 7-Mar-2014 for ALL07032014
            If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
           (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

                MSGBoxCtrl.Show(MSGBox.Message_title.Authorization,
                                MSGBox.Message_text.Authorization,
                                "",
                                MsgBoxStyle.OkOnly,
                                "Authorization")

                Exit Sub

            End If

            If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbResource.SelectedValue.ToString,
                                                                      mnWO.WODate)

            If (mEmployeeStatus(0).Information <> "") Then

                'Added By Vikrant On 05-Aug-2013 For ALL01082013
                Dim message As String = mEmployeeStatus(0).Information

                MSGBoxCtrl.Show("Save Alert !",
                                message,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            End If
            'End

            mEmployeeAircraftRightsCount = EmployeeAircraftRightsCount.GetEmployeeAircraftRightsCount(cmbResource.SelectedValue.ToString,
                                                                                                     mnWO.MachineID.ToString)
            If mEmployeeAircraftRightsCount(0).Count >= 1 Then 'Means Resource has not rights for aircrafts 

                MSGBoxCtrl.Show("Save Alert !",
                                "Resource has not rights for " + mnWO.RegNo + ". Please select another resource.",
                                "",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            End If

            If Page.IsValid Then

                If Session("mResourceAllocationEdit") = False Then

                    If Not mnWOJobDesignationAllocation.WOJobResourceAllocations.Contains(New Guid(cmbResource.SelectedValue)) Then

                        mnWOJobDesignationAllocation.WOJobResourceAllocations.Add(mnWOJobDesignationAllocation.ID, New Guid(cmbResource.SelectedValue))
                        SetObject()

                        If mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem.IsValid Then

                            dgResourceAllocation.DataSource = mnWOJobDesignationAllocation.WOJobResourceAllocations
                            dgResourceAllocation.DataBind()
                            Session("mnWOJobDesignationAllocation") = mnWOJobDesignationAllocation
                            upnlGridView.Update()
                            cmbResource.SelectedIndex = 0

                        Else

                            If Not CustomValidate_Object() Then

                                upnlValidationSummary.Update()
                                mnWOJobDesignationAllocation.WOJobResourceAllocations.Remove(mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem)
                                Exit Sub

                            End If

                        End If

                    Else

                        MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate,
                                        MSGBox.Message_text.Duplicate,
                                        "",
                                        MsgBoxStyle.OkOnly,
                                        "")
                        Exit Sub

                    End If

                Else

                    '--Added By Utkarsh On 20-jan-2011
                    For Each mJobResourceAllocation As nWOJobResourceAllocation In mnWOJobDesignationAllocation.WOJobResourceAllocations
                        If Not mJobResourceAllocation.ID.Equals(New Guid(Session("CurrentResourceAllocationID").ToString)) Then
                            If mnWOJobDesignationAllocation.WOJobResourceAllocations.Contains(New Guid(cmbResource.SelectedValue)) Then
                                Session("mResourceAllocationEdit") = False
                                Session.Remove("CurrentResourceAllocationID")
                                DataFieldBind()
                                MSGBoxCtrl.Show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        End If
                    Next

                    SetObject()
                    If Not CustomValidate_Object() Then
                        upnlValidationSummary.Update()
                        Exit Sub
                    End If
                    dgResourceAllocation.DataSource = mnWOJobDesignationAllocation.WOJobResourceAllocations
                    dgResourceAllocation.DataBind()
                    Session("mnWOJobDesignationAllocation") = mnWOJobDesignationAllocation
                    SetFocus(cmbResource)
                    Session("mResourceAllocationEdit") = False
                    upnlGridView.Update()
                    cmbResource.SelectedIndex = 0
                    '---------------------------------------------
                End If

                ControlVisibility()
                DataFieldBind()
                upnlResource.Update()
                upnlGridView.Update()

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GV_ResourceAllocation_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgResourceAllocation.RowCommand

        Dim Index As Int32 = dgResourceAllocation.PageIndex * dgResourceAllocation.PageSize + CInt(e.CommandArgument)
        Dim mID As Guid = mnWOJobDesignationAllocation.WOJobResourceAllocations(Index).ID

        Select Case e.CommandName
            Case "EditRecord"

                'Added by Saylee on 7-Mar-2014 for ALL07032014
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then

                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")
                    Exit Sub

                End If

                Session("mResourceAllocationEdit") = True
                EditRecord(Index)
                dgResourceAllocation.DataSource = mnWOJobDesignationAllocation.WOJobResourceAllocations
                DataBind()
                updatePanels()

            Case "DeleteRecord"

                'Added by Saylee on 7-Mar-2014 for ALL07032014
                If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
                   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

                    MSGBoxCtrl.Show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")
                    Exit Sub

                End If

                DeleteRecord(Index)

            Case "AddResourceDetail"

                mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentIndex = Index
                Session("mnWOJobResourceAllocation") = mnWOJobDesignationAllocation.WOJobResourceAllocations.CurrentItem
                Session("mDesignationName") = mnWOJobDesignationAllocation.DesignationName
                Session("mResourceDetailEdit") = False
                Session("mResourceAllocationEdit") = False

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenToAddResourceDetail",
                                                    "OpenToAddResourceDetail();",
                                                    True)

        End Select

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click, btnCloseTop.Click

        Try

            Session.Remove("mWOJobDesignationAllocations")
            Dim OpenAs As String = Request.QueryString("Type")

            If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "On Close",
                                                    "CallParentCallback();",
                                                    True)
                Exit Sub

            End If

            Response.Redirect(Request.QueryString("BackPage3") & "?CPage1=" &
                              Request.QueryString("CPage1") & "&BackPage2=" &
                              Request.QueryString("BackPage2") & "&BackPage1=" &
                              Request.QueryString("BackPage1") & "&BackPage=" &
                              Request.QueryString("BackPage"))

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
        MessageBoxResult()
    End Sub

    Private Sub AddResourceDetail_HdnBtn(sender As Object, e As EventArgs) Handles hdnBtnAddResourceDetail.Click

        dgResourceAllocation.DataSource = mnWOJobDesignationAllocation.WOJobResourceAllocations
        dgResourceAllocation.DataBind()
        upnlGridView.Update()

    End Sub

    Private Sub ResourceListDataBound(sender As Object, e As EventArgs) Handles cmbResource.DataBound

        Try

            HighlightNonWorkingResource(sender:=sender)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub Resource_Selected(sender As Object, e As EventArgs) Handles cmbResource.SelectedIndexChanged

        Try

            HighlightNonWorkingResource(sender:=sender)

            CheckResourceStatus(SelectedEmployeeIndex:=cmbResource.SelectedIndex,
                                SelectedEmployee:=cmbResource.SelectedValue)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Function HighlightNonWorkingResource(sender As Object)

        Try

            Dim _DropDownList As DropDownList = CType(sender, DropDownList)
            mEmployeeList = CType(Session("mEmployeeList"), EmployeeList)

            If sender IsNot Nothing Then

                For Each Item As ListItem In _DropDownList.Items

                    If Not Item.Value = (Guid.Empty.ToString) AndAlso
                       Not mEmployeeList(New Guid(Item.Value)).IsWorking Then

                        Item.Attributes.Add("style", "background-color: yellow;")

                    End If

                Next

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Public Function CheckResourceStatus(SelectedEmployeeIndex As Integer,
                                        SelectedEmployee As String)

        mEmployeeList = CType(Session("mEmployeeList"), EmployeeList)
        Try

            If Not SelectedEmployeeIndex = 0 AndAlso
               Not mEmployeeList(New Guid(SelectedEmployee)).IsWorking Then

                MSGBoxCtrl.Show("Alert..!!!",
                                "Selected Resource is not working with the Organization.",
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

                cmbResource.SelectedIndex = 0

            End If


        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

#End Region

End Class