'Added by Vikrant

Public Class wfTrainingOrg_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTrainingOrgList As TrainingOrgList
    Public mTrainingOrg As TrainingOrg
    Public mCityInvList As CityInvList
    Public BackPage As String
    Public IsFromRenewal As String = ""
    'Added by Vikrant on 20-July-2011
    Dim EventLogID As Guid

    Public mTrainingList As TrainingList
    Public mTrainingDetailList As TrainingDetailList
    Public mTrainingDetail As TrainingDetail
    Public mTrainingDetailID As Guid
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mTrainingOrg = Session("mTrainingOrg")
        mTrainingOrgList = Session("mTrainingOrgList")
        mCityInvList = Session("mCityInvList")
        IsFromRenewal = Request.QueryString("IsFromRenewal")
        mTrainingDetail = Session("mTrainingDetail")
        mTrainingDetailList = Session("mTrainingDetailList")
        mTrainingList = Session("mTrainingList")
    End Sub
    Private Sub SetSession()
        Session("mTrainingOrg") = mTrainingOrg
        Session("mTrainingOrgList") = mTrainingOrgList
        Session("mCityInvList") = mCityInvList
    End Sub
    Private Sub SetSessionForTrainingDetail()
        Session("mTrainingDetail") = mTrainingDetail
        Session("mTrainingDetailList") = mTrainingDetailList
        Session("mTrainingList") = mTrainingList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfTrainingOrg_Ajax.aspx" Then
            Session.Remove("mTrainingOrg")
            Session.Remove("mTrainingOrgList")
            Session.Remove("mCityInvList")
            Session.Remove("NewTrainingOrg")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'AJAX
        str = "try{document.getElementById('" + cntrl.ClientID + "').focus();} catch (Error) {}"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True) 'AJAX
    End Sub
    Private Sub NewRecord()
        mTrainingOrg = TrainingOrg.NewTrainingOrg()
        mCityInvList = CityInvList.GetCityList(0, "", "")
        Session("mTrainingOrg") = mTrainingOrg
        Session("mCityInvList") = mCityInvList
        txtName.Enabled = True
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mTrainingOrg = TrainingOrg.GetTrainingOrg(mID)
        Session("mTrainingOrg") = mTrainingOrg
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        MSGBoxCntrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mTrainingOrg = TrainingOrg.GetTrainingOrg(mID)
        Session("mTrainingOrg") = mTrainingOrg
    End Sub
    Private Sub setTitle()
        If mTrainingOrg.IsNew Then
            lbltitle.Text = "Training Organization [NEW]"
        Else
            If Len(mTrainingOrg.Name) > 15 Then
                lbltitle.Text = "Training Organization [" & mTrainingOrg.Name.Substring(0, 15) & "... ]"
            Else
                lbltitle.Text = "Training Organization [" & mTrainingOrg.Name & " ]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub SetObject()
        mTrainingOrg.Name = txtName.Text
        mTrainingOrg.Address1 = Trim(txtAddress1.Text)
        mTrainingOrg.Address2 = Trim(txtAddress2.Text)
        mTrainingOrg.Address3 = Trim(txtAddress3.Text)
        mTrainingOrg.PhoneNo1 = Trim(txtPhone1.Text)
        mTrainingOrg.PhoneNo2 = Trim(txtPhone2.Text)
        mTrainingOrg.Fax = Trim(txtFax.Text)
        mTrainingOrg.Email = Trim(txtEmail.Text)
        mTrainingOrg.Website = Trim(txtWebsite.Text)

        Try
            mTrainingOrg.CityID = New Guid(cmbCityList.SelectedValue)
        Catch ex As Exception
            mTrainingOrg.CityID = Guid.Empty
        End Try

    End Sub
    Private Sub setObjectForTrainingDetail()
        Dim chkValue As New CheckBox

        For i As Integer = 0 To dgTrainingDetailList.Rows.Count - 1
            chkValue = CType(Me.dgTrainingDetailList.Rows(i).FindControl("chkTrainingOrg"), CheckBox)
            Dim index As Integer = i + dgTrainingDetailList.PageIndex * dgTrainingDetailList.PageSize
            mTrainingList(index).IsSelect = chkValue.Checked

            If chkValue.Checked = True Then
                If Not mTrainingDetailList.Contains(mTrainingList(index).ID, TrainingDetailList.SearchWith.TrainingID) Then
                    mTrainingDetail = TrainingDetail.NewTrainingDetail(mTrainingDetailID)
                    mTrainingDetail.TrainingID = mTrainingList(index).ID
                    mTrainingDetail.TrainingOrgID = mTrainingOrg.ID
                    If mTrainingDetail.IsValid Then
                        mTrainingDetail = CType(mTrainingDetail.Save, TrainingDetail)
                        'MarkLog(Flypal.Util.Action.Save, "TrainingOrganization", mTrainingOrg.Name, Flypal.Util.ErrorType.HandledError, mTrainingDetail.ID)
                        MarkLog(Flypal.Util.Action.Save, "Training Detail", "Training : " & mTrainingList(index).Name & " Training Org: " & mTrainingOrg.Name, Flypal.Util.ErrorType.HandledError, mTrainingDetail.ID, EventLogID)
                    End If
                End If
            Else
                If mTrainingDetailList.Contains(mTrainingList(index).ID, TrainingDetailList.SearchWith.TrainingID) Then
                    TrainingDetail.DeleteTrainingDetail(mTrainingDetailList.Item(mTrainingList(index).ID, TrainingDetailList.SearchWith.TrainingID).ID)
                    'MarkLog(Flypal.Util.Action.Delete, "Training Organization", "Training : " & mTrainingList(i).Name & " Training Org: " & mTrainingOrg.Name, Flypal.Util.ErrorType.HandledError, mTrainingDetail.ID, EventLogID)
                End If
            End If
        Next

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCntrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCntrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            Session("NewTrainingOrg") = "False"
                            mTrainingOrg = Session("mTrainingOrg")
                            TrainingOrg.DeleteTrainingOrg(mTrainingOrg.ID)
                            NewRecord()
                            DataFieldBind()
                            setTitle()
                            ControlVisibility1()
                            upnlTrainingOrgDetails.Update()
                            upnlGridView.Update()
                            'Response.Redirect("wfTrainingOrg_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "TrainingOrg", "Can't delete :" & mTrainingOrg.Name & " is Currently in use", Flypal.Util.ErrorType.NoError, mTrainingOrg.ID, EventLogID)
                                MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind() ''Rajnish
                            setTitle()
                            ControlVisibility1()
                            upnlTrainingOrgDetails.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "TrainingOrg", mTrainingOrg.Name, Flypal.Util.ErrorType.NoError, mTrainingOrg.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    NewRecord()
                    setTitle()
                    ControlVisibility1()
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'CHK DataFieldBind()
                    'Response.Redirect("wfTrainingOrg_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
                Case MsgBoxResult.Ok And MSGBoxCntrl.Sender = "Authorization"  'Code Added
                    Session("sender") = ""
                    'CHK DataFieldBind()
                    'Response.Redirect("wfTrainingOrg_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'CHK DataFieldBind()
            'Response.Redirect("wfTrainingOrg_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
        ElseIf Result1 = 0 And MSGBoxCntrl.Sender = "Authorization" Then   'Code Added
            Session("sender") = ""
            'CHK DataFieldBind()
        End If
    End Sub
    Public Sub ControlVisibility(ByVal index As Integer)
        txtFor.Visible = IIf(index > 0, True, False)
        lblFor.Visible = IIf(index > 0, True, False)
    End Sub
    Public Sub ControlVisibility1()
        If mTrainingOrg.IsNew Then
            lnkTrainingDetail.Visible = False
        Else
            lnkTrainingDetail.Visible = True
        End If
        upnlSave.Update()
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerTrainingOrg(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind()
        mCityInvList = CityInvList.GetCityList(0, , , True)
        cmbCityList.DataSource = mCityInvList
        Session("mCityInvList") = mCityInvList

        'Added Code
        If Not mCityInvList.Contains(mTrainingOrg.CityID) Then
            mTrainingOrg.CityID = Guid.Empty
        End If
        'End of Added Code
        'cmbCityList.DataBind()

        mTrainingOrgList = TrainingOrgList.GetTrainingOrgList()
        dgTrainingOrgList.DataSource = mTrainingOrgList
        'dgTrainingOrgList.DataBind()
        DataBind()
        Session("mTrainingOrgList") = mTrainingOrgList

        lblResult.Text = "Training Organization List: " & mTrainingOrgList.Count & " Record(s) Found."
    End Sub
    Private Sub DataFieldBindForTrainingDetail()
        mTrainingList = TrainingList.GetTrainingList()
        dgTrainingDetailList.DataSource = mTrainingList
        Session("mTrainingList") = mTrainingList

        mTrainingDetailList = TrainingDetailList.GetTrainingDetailList(, mTrainingOrg.ID.ToString, True)
        Session("mTrainingDetailList") = mTrainingDetailList
        Dim Child As TrainingDetail
        For Each Child In mTrainingDetailList
            If mTrainingList.Contains(Child.TrainingID) Then
                mTrainingList.Item(Child.TrainingID).IsSelect = True
            End If
        Next
        txtTrainingOrgName.Text = mTrainingOrg.Name
        upnlTrainingDetails.DataBind()
        dgTrainingDetailList.PageIndex = 0
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        'Added by Vikrant on 20-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If

            If Session("MiddleFrame") <> "wfTrainingOrg_Ajax.aspx" Then
                Session("MiddleFrame") = "wfTrainingOrg_Ajax.aspx"
            End If

            BackPage = Request.QueryString("Backpage")
            Session("BackPage") = BackPage

            If Session("NewTrainingOrg") <> "True" Then
                NewRecord()
            Else
                Session("NewTrainingOrg") = "True"
            End If
            Session("mTrainingOrg") = mTrainingOrg
            DataFieldBind()
        Else
            dgTrainingOrgList.DataSource = mTrainingOrgList
            dgTrainingOrgList.DataBind()
        End If
        'MessageBoxResult()
        setTitle()
        ControlVisibility1()
        SetSession()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("TrainingOrganizationNew") And mTrainingOrg.IsNew) Or (Not User.IsInRole("TrainingOrganizationEdit") And Not mTrainingOrg.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "TrainingOrg", User.Identity.Name & " is not Authorized User to save" & mTrainingOrg.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                mTrainingOrg.Save()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                MarkLog(Flypal.Util.Action.Save, "TrainingOrg", mTrainingOrg.Name, Flypal.Util.ErrorType.HandledError, mTrainingOrg.ID, EventLogID)
                NewRecord()
                ControlVisibility1()
                txtState.Text = ""
                txtCountry.Text = ""
                DataFieldBind()
                SetSession()
                setTitle()
                upnlTrainingOrgDetails.Update()
                upnlGridView.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 547 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                End If
            End Try
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        MarkLog(Flypal.Util.Action.[New], "TrainingOrg", "", Flypal.Util.ErrorType.NoError, mTrainingOrg.ID, EventLogID)
        NewRecord()
        ControlVisibility1()
        DataFieldBind()
        setTitle()
        upnlTrainingOrgDetails.Update()
    End Sub
    Private Sub imgCity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgCity.Click
        If Not (User.IsInRole("TrainingOrganizationNew") And User.IsInRole("TrainingOrganizationEdit") And User.IsInRole("TrainingOrganizationDelete")) Then
            SetObject()
            SetSession()
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        SetObject()
        Session("NewTrainingOrg") = "True"
        Response.Redirect("wfCityInv_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=wfTrainingOrg_Ajax.aspx" & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Flypal.Util.Action.Close, "TrainingOrg", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("mTrainingOrgList") = mTrainingOrgList
        Session.Remove("NewTrainingOrg")

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        If Request.QueryString("ChildPage2") = "wfEmployeeTraining_Ajax.aspx" And IsFromRenewal = "True" Then
            Session("MiddleFrame") = "wfEmployeeDueForRenewal_Ajax.aspx"
            IsFromRenewal = "False"
            Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
        ElseIf Request.QueryString("ChildPage2") = "wfEmployeeTraining_Ajax.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
        Else
            Session("MiddleFrame") = ""
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub dgTrainingOrgList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingOrgList.RowCommand
        Dim index As Integer
        Dim mID As Guid
        Dim mName As String
        Select Case e.CommandName
            Case "EditRec"
                index = CInt(e.CommandArgument) + dgTrainingOrgList.PageIndex * dgTrainingOrgList.PageSize
                mID = mTrainingOrgList(index).ID
                mName = mTrainingOrgList(index).Name

                If (Not User.IsInRole("TrainingOrganizationView") And Not User.IsInRole("TrainingOrganizationEdit")) Then
                    SetObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Edit, "TrainingOrg", User.Identity.Name & " is not Authorized User to edit" & mName, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecord(mID)
                DataBind()
                ControlVisibility1()
                setTitle()
                DisableName(mID) 'Added by : Shital 19-Jun-2020, ALL16062020
                MarkLog(Flypal.Util.Action.Edit, "TrainingOrg", mTrainingOrg.Name, Flypal.Util.ErrorType.NoError, mTrainingOrg.ID, EventLogID)

                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                upnlTrainingOrgDetails.Update()
            Case "DeleteRec"
                index = CInt(e.CommandArgument) + dgTrainingOrgList.PageIndex * dgTrainingOrgList.PageSize
                mID = mTrainingOrgList(index).ID
                mName = mTrainingOrgList(index).Name

                If (Not User.IsInRole("TrainingOrganizationDelete")) Then
                    SetObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Delete, "TrainingOrg", User.Identity.Name & " is not Authorized User to delete" & mName, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(mID)
                'Newly Added by Vikrant 20-July-2011
                MarkLog(Flypal.Util.Action.Delete, "TrainingOrg", mTrainingOrg.Name, Flypal.Util.ErrorType.NoError, mTrainingOrg.ID, EventLogID)
        End Select
    End Sub
    Private Sub cmbCityList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCityList.SelectedIndexChanged
        txtState.Text = IIf(cmbCityList.SelectedIndex > 0, mCityInvList(cmbCityList.SelectedIndex).State, "")
        txtCountry.Text = IIf(cmbCityList.SelectedIndex > 0, mCityInvList(cmbCityList.SelectedIndex).Country, "")
        If cmbCityList.Enabled = True Then
            setFocus(cmbCityList)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If cmbSearchType.SelectedValue = 0 Then
            mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, , , )
        ElseIf cmbSearchType.SelectedValue = 1 Then
            mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, Trim(txtFor.Text), , )
        ElseIf cmbSearchType.SelectedValue = 2 Then
            mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, , txtFor.Text, )
        End If
        dgTrainingOrgList.DataSource = mTrainingOrgList
        dgTrainingOrgList.DataBind()
        Session("mTrainingOrgList") = mTrainingOrgList

        lblResult.Text = "Training Organization List: " & mTrainingOrgList.Count & " Record(s) Found."
        upnlGridView.Update()
    End Sub
    Private Sub cmbSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchType.SelectedIndexChanged
        Dim index As Integer
        txtFor.Text = ""
        index = cmbSearchType.SelectedIndex
        ControlVisibility(index)
    End Sub
    Private Sub lnkTrainingDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkTrainingDetail.Click
        lnkTrainingDetail_ModalPopupExtender.Show()
        DataFieldBindForTrainingDetail()
        upnlTrainingDetails.Update()
    End Sub
    Private Sub dgTrainingOrgList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingOrgList.PageIndexChanging
        dgTrainingOrgList.PageIndex = e.NewPageIndex
        dgTrainingOrgList.DataSource = mTrainingOrgList
        Session("mTrainingOrgList") = mTrainingOrgList
        dgTrainingOrgList.DataBind()
    End Sub
    'Added By Prashant 23-June-2009 for grid sorting
    Private Sub dgTrainingOrgList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTrainingOrgList.Sorting
        mTrainingOrgList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mTrainingOrgList") = mTrainingOrgList
        dgTrainingOrgList.DataSource = mTrainingOrgList
        dgTrainingOrgList.DataBind()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCntrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCity.Click
        mCityInvList = CityInvList.GetCityList(0, , , True)
        cmbCityList.DataSource = mCityInvList
        cmbCityList.DataBind()
        Session("mCityInvList") = mCityInvList
        upnlTrainingOrgDetails.Update()
    End Sub
#End Region

#Region "Training Detail Child"
    Private Sub btnSaveTrainingDet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrainingDet.Click
        If (Not User.IsInRole("TrainingOrganizationNew") And mTrainingOrg.IsNew) Or (Not User.IsInRole("TrainingOrganizationEdit") And Not mTrainingOrg.IsNew) Then
            SetObject()
            SetSession()
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "AuthorizationForModal")
            Exit Sub
        End If
        Dim chkValue As New CheckBox
        Dim mIsNotSelect As Boolean = True

        If IsValid Then
            Try
                setObjectForTrainingDetail()
                DataFieldBindForTrainingDetail()
                SetSessionForTrainingDetail()
                'lbltitle.Text = "Training Detail"
                lnkTrainingDetail_ModalPopupExtender.Show()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "DeleteForModal")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "DeleteForModal")
                ElseIf ex.Number = 547 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "DeleteForModal")
                End If
            End Try
        End If
    End Sub
    Private Sub btnBackTrainingDet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackTrainingDet.Click
        Session.Remove("mTrainingDetail")
        Session.Remove("mTrainingDetailList")
        Session.Remove("mTrainingList")
        lnkTrainingDetail_ModalPopupExtender.Hide()
    End Sub
    Private Sub dgTrainingDetailList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingDetailList.PageIndexChanging
        dgTrainingDetailList.PageIndex = e.NewPageIndex
        dgTrainingDetailList.DataSource = mTrainingList
        Session("mTrainingList") = mTrainingList
        dgTrainingDetailList.DataBind()
    End Sub
#End Region

    
End Class