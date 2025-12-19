'CREATED By : Saylee
'Dated      : 21-Nov-2013

Public Class wfDesignation_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mDesignation As Designation
    Protected mDesignationList As DesignationList
    'Added by Vikrant on 20-July-2011
    Dim EventLogID As Guid
#End Region

#Region " Page Load "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Get the existing Designation (if any)
        mDesignation = Session("wfDesignation.Designation")


        If mDesignation Is Nothing Then
            'Make new Designation
            mDesignation = Designation.NewDesignation()
            Session("wfDesignation.Designation") = mDesignation
        Else
            'Else do nothing
        End If
        'Added by Vikrant on 20-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not Page.IsPostBack Then
            If txtDesignation.Enabled = True Then
                setFocus(txtDesignation)
            End If
            GetDesignationList()
            DataBind()
            SetGrid()
        End If
        mDesignationList = Session("mDesignationList")
        If mDesignationList.Count > 25 Then
            'btnBackTop.Visible = True
        Else
            'btnBackTop.Visible = False
        End If

        'upnlBack.Update()
        'upnlBackTop.Update()
        '''upnlDesignation.Update()
        '''upnlGridView.Update()
        '''upnlTitle.Update()

    End Sub
#End Region

#Region " Helper Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub SaveFormToObject()
        With mDesignation
            .Name = Trim(txtDesignation.Text)
        End With
    End Sub
    Private Sub GetDesignationList()
        mDesignationList = DesignationList.GetDesignationList()
        dgDesignationList.DataSource = DesignationList.GetDesignationList()
        Session("mDesignationList") = mDesignationList
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDesignation = Designation.GetDesignation(mID)
        Session("wfDesignation.Designation") = mDesignation

    End Sub
    Private Sub SetGrid()
        Dim IsSyncFromCRS As Boolean
        For j As Integer = 0 To dgDesignationList.Rows.Count - 1
            IsSyncFromCRS = CType(Me.dgDesignationList.Rows(j).Cells(3).Text, Boolean)

            If IsSyncFromCRS = True Then

                dgDesignationList.Rows(j).Cells(2).Enabled = False
                'dgDesignationList.Rows(j).Cells(3).Enabled = False

            End If
        Next
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            Try
                                Designation.DeleteDesignation(mDesignation.ID)
                            Catch ex As Exception
                                If ex.Message.Contains("Record in use. Cannot delete record.") Then

                                    MarkLog(Flypal.Util.Action.Delete, "Designation", "Can't delete : " + "Designation : " + mDesignation.Name + " is Currently in use", Flypal.Util.ErrorType.NoError, mDesignation.ID, EventLogID)
                                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                    mDesignation = Designation.NewDesignation
                                    txtDesignation.Text = ""
                                    lbltitle.Text = "Designation [New]"
                                    Session("wfDesignation.Designation") = mDesignation
                                    upnlDesignation.Update()
                                    upnlTitle.Update()
                                End If
                            End Try
                            GetDesignationList()
                            DataBind()
                            MarkLog(Util.Action.Delete, "Designation", mDesignation.Name, Util.ErrorType.NoError, mDesignation.ID, EventLogID)
                            mDesignation = Designation.NewDesignation
                            txtDesignation.Text = ""
                            lbltitle.Text = "Designation [New]"
                            Session("wfDesignation.Designation") = mDesignation
                            SetGrid()
                            upnlDesignation.Update()
                            upnlTitle.Update()
                            upnlGridView.Update()

                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Designation", "Can't delete : " + "Designation : " + mDesignation.Name + " is Currently in use", Flypal.Util.ErrorType.NoError, mDesignation.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                mDesignation = Designation.NewDesignation
                                txtDesignation.Text = ""
                                lbltitle.Text = "Designation [New]"
                                Session("wfDesignation.Designation") = mDesignation
                                upnlDesignation.Update()
                                upnlTitle.Update()

                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count

                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Designation", mDesignation.Name, Flypal.Util.ErrorType.NoError, mDesignation.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        mDesignation = Designation.NewDesignation
                        txtDesignation.Text = ""
                        lbltitle.Text = "Designation [New]"
                        Session("wfDesignation.Designation") = mDesignation
                        SetGrid()
                        upnlDesignation.Update()
                        upnlTitle.Update()
                    End If

                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " Events "

    Private Sub dgDesignationList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDesignationList.PageIndexChanging
        dgDesignationList.PageIndex = e.NewPageIndex
        dgDesignationList.DataSource = mDesignationList
        Session("mDesignationList") = mDesignationList
        dgDesignationList.DataBind()
        'SetGrid()
        'upnlDesignation.Update()
    End Sub
    'Private Sub dgDesignationList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDesignationList.RowCreated
    '    If e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
    '        Dim button As LinkButton
    '        button = e.Row.Cells(3).Controls.Item(0)
    '        'button.Attributes.Add("onclick", MessageBox.Show("Do you want to delete selected records ?", MessageBox.MessageBoxButton.YesNo))
    '    End If
    'End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        mDesignation = Designation.NewDesignation()
        Session("wfDesignation.Designation") = mDesignation
        lbltitle.Text = "Designation [New]"
        txtDesignation.Text = ""
        upnlDesignation.Update()
        ' DataBind()
        'Added by Vikrant on 20-July-2011
        MarkLog(Util.Action.[New], "Designation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        SetGrid()
        If txtDesignation.Enabled = True Then
            setFocus(txtDesignation)
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            SaveFormToObject()
            If mDesignation.IsSavable And mDesignation.IsDirty Then
                mDesignation = mDesignation.Save
                'Added by Vikrant on 20-July-2011
                MarkLog(Util.Action.Save, "Designation", mDesignation.Name, Util.ErrorType.NoError, mDesignation.ID, EventLogID)

                Session("wfDesignation.Designation") = mDesignation
                GetDesignationList()
                dgDesignationList.DataBind()
                SetGrid()
                txtDesignation.Text = ""
                mDesignation = Designation.NewDesignation
                lbltitle.Text = "Designation [New]"
                If txtDesignation.Enabled = True Then
                    setFocus(txtDesignation)
                End If
                Session("wfDesignation.Designation") = mDesignation
            Else
                If Not mDesignation.IsSavable Then
                    cvControlValidator.ErrorMessage = mDesignation.GetBrokenRulesString
                    cvControlValidator.IsValid = mDesignation.IsSavable
                    If cvControlValidator.ErrorMessage = "" Then
                        txtDesignation.Text = ""
                        lbltitle.Text = "Designation [New]"
                        upnlDesignation.Update()
                    End If
                    SetGrid()
                    Exit Sub
                End If
                txtDesignation.Text = ""
                upnlDesignation.Update()
            End If
        Catch ex As Exception
            ' ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show(ex.Message))
            SetGrid()
            upnlDesignation.Update()
            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, ex.Message, MsgBoxStyle.OkOnly, "")
        End Try
        upnlGridView.Update()
        upnlDesignation.Update()
        upnlValidationSummary.Update()
        upnlTitle.Update()
    End Sub
    Private Sub dgDesignationList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDesignationList.RowCommand
        Dim Idx As Int32
        Dim mID As Guid

        Select Case e.CommandName
            Case "EditRec"

                'index = CInt(e.CommandArgument) + dgEmployeeDepartmentList.PageIndex * dgEmployeeDepartmentList.PageSize  'CInt(e.CommandArgument)
                'mID = mEmployeeDepartmentList(index).ID
                'mName = mEmployeeDepartmentList(index).Name
                'Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 21-04-2023
                'Idx = gvr.RowIndex
                Idx = CInt(e.CommandArgument) + dgDesignationList.PageIndex * dgDesignationList.PageSize  'CInt(e.CommandArgument)

                '  Idx = CInt(e.CommandArgument) + dgDesignationList.PageIndex * dgDesignationList.PageSize
                mID = mDesignationList(Idx).ID
                mDesignation = Designation.GetDesignation(mID)
                Session("wfDesignation.Designation") = mDesignation
                lbltitle.Text = "Designation [" & mDesignation.Name & " ]"
                txtDesignation.DataBind()
                ''End If
                SetGrid()
                'Added by Vikrant on 20-July-2011
                MarkLog(Util.Action.Edit, "Designation", mDesignation.Name, Util.ErrorType.NoError, mDesignation.ID, EventLogID)
                upnlDesignation.Update()
                upnlTitle.Update()
                upnlValidationSummary.Update()
                If txtDesignation.Enabled = True Then
                    setFocus(txtDesignation)
                End If
            Case "DeleteRec"
                Try
                    'Idx = CInt(e.CommandArgument) + dgDesignationList.PageIndex * dgDesignationList.PageSize
                    'mID = mDesignationList(Idx).ID
                    Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 21-04-2023
                    Idx = gvr.RowIndex
                    mID = mDesignationList(Idx).ID
                    DeleteRecord(mID)
                    SetGrid()
                Catch ex As Exception
                    'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show(ex.Message))
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript();", MessageBox.Show(ex.Message), True)

                    MarkLog(Util.Action.Delete, "Designation", "Can't delete :" & mDesignation.Name & " is Currently in use", Util.ErrorType.NoError, mDesignation.ID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Message, MsgBoxStyle.OkOnly, "")
                    mDesignation = Designation.NewDesignation
                    txtDesignation.Text = ""
                    lbltitle.Text = "Designation [New]"
                    Session("wfDesignation.Designation") = mDesignation
                    upnlDesignation.Update()
                    upnlTitle.Update()
                    upnlValidationSummary.Update()
                    SetGrid()
                End Try
        End Select
    End Sub

    'Private Sub dgDesignationList_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgDesignationList.RowEditing
    '    mDesignation = FlyPal22.Maintain.Designation.GetDesignation(mDesignationList(e.NewEditIndex).ID)
    '    Session("wfDesignation.Designation") = mDesignation
    '    lbltitle.Text = "Designation [" & mDesignation.Name & " ]"
    '    DataBind()
    '    ''End If

    '    'Added by Vikrant on 20-July-2011
    '    MarkLog(Util.Action.Edit, "Designation", mDesignation.Name, Util.ErrorType.NoError, mDesignation.ID, EventLogID)
    '    upnlDesignation.Update()
    '    upnlTitle.Update()
    '    If txtDesignation.Enabled = True Then
    '        setFocus(txtDesignation)
    '    End If
    'End Sub
    'Private Sub dgDesignationList_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles dgDesignationList.RowDeleting
    '    Try

    '        FlyPal22.Maintain.Designation.DeleteDesignation(mDesignationList(e.RowIndex).ID)
    '        GetDesignationList()
    '        DataBind()

    '        'Added by Vikrant on 20-July-2011
    '        MarkLog(Util.Action.Delete, "Designation", mDesignation.Name, Util.ErrorType.NoError, mDesignation.ID, EventLogID)
    '        upnlDesignation.Update()
    '        upnlTitle.Update()
    '        upnlGridView.Update()
    '    Catch ex As Exception
    '        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show(ex.Message))
    '        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript();", MessageBox.Show(ex.Message), True)
    '        MarkLog(Util.Action.Delete, "Designation", "Can't delete :" & mDesignation.Name & " is Currently in use", Util.ErrorType.NoError, mDesignation.ID, EventLogID)
    '        MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Message, MsgBoxStyle.OkOnly, "")
    '    End Try
    'End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by Vikrant on 20-July-2011
        MarkLog(Util.Action.Close, "Designation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)

        Session.Remove("wfDesignation.Designation")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mKit")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If


        If Request.QueryString("ChildPage2") = "wfEmployeeDesignation_Ajax.aspx" Or Request.QueryString("ChildPage2") = "wfPilot.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1"))
        ElseIf Request.QueryString("ChildPage2") = "wfnWOJobDesignationAllocation.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
        Else
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End If
        'upnlBackTop.Update()
        'upnlBack.Update()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region



End Class