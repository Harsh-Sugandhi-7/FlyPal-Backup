'AJAX Conversion By Vikrant

Public Class wfContractor_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mContractor As Contractor
    Public mContractorList As ContractorList
    Public mCityInvList As CityInvList
    Public BackPage As String
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mContractorDetail As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mContractor = CType(Session("mContractor"), Contractor)
        mContractorList = CType(Session("mContractorList"), ContractorList)

        mCityInvList = CType(Session("mCityInvList"), CityInvList)
    End Sub
    Private Sub SetSession()
        Session("mContractor") = mContractor
        Session("mContractorList") = mContractorList

        Session("mCityInvList") = mCityInvList
    End Sub
    Private Sub SetTitle()
        If mContractor.IsNew Then
            lblTitle.Text = "Contractor Information [NEW]"
        Else
            If Len(mContractor.Name) > 15 Then
                lblTitle.Text = "Contractor Information [" & mContractor.Name.Substring(0, 15) & "... ]"
            Else
                lblTitle.Text = "Contractor Information [" & mContractor.Name & " ]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        mContractor = Contractor.NewContractor(Guid.NewGuid)
        Session("mContractor") = mContractor
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        mContractor = Contractor.GetContractor(ID)
        Session("mContractor") = mContractor
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mContractor = Contractor.GetContractor(ID)
        Session("mContractor") = mContractor
    End Sub
    Private Sub setObject()
        With mContractor
            .Name = Trim(txtName.Text)
            .Code = Trim(txtCode.Text)
            .Address1 = Trim(txtAddress1.Text)
            .Address2 = Trim(txtAddress2.Text)
            .Address3 = Trim(txtAddress3.Text)
            .CityID = New Guid(cmbCityInvList.SelectedValue)
            .PhoneNo1 = Trim(txtPhone1.Text)
            .PhoneNo2 = Trim(txtPhone2.Text)
            .Fax = Trim(txtFax.Text)
            .Email = Trim(txtEmail.Text)
            .Website = Trim(txtWebsite.Text)
        End With
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
                            Dim mContractor As Contractor
                            Session("sender") = ""
                            mContractor = CType(Session("mContractor"), Contractor)
                            Contractor.DeleteContractor(mContractor.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlContractorDetails.Update()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                mContractorDetail = mContractor.Name + " Code : " + mContractor.Code
                                MarkLog(Flypal.Util.Action.Delete, "Contractor", "Can't delete : " & mContractorDetail & " This is Currently in use", Flypal.Util.ErrorType.NoError, mContractor.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            'DataFieldBind()
                            SetTitle()
                            upnlContractorDetails.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mContractorDetail = mContractor.Name + " Code : " + mContractor.Code
                                MarkLog(Flypal.Util.Action.Delete, "Contractor", mContractorDetail, Flypal.Util.ErrorType.NoError, mContractor.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        SetTitle()
                        upnlContractorDetails.Update()
                    End If
                    Session("sender") = ""

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
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

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCityInvList = CityInvList.GetCityList(0, , , True)
        cmbCityInvList.DataSource = mCityInvList
        Session("mCityInvList") = mCityInvList
        cmbCityInvList.DataBind()

        mContractorList = ContractorList.GetContractorList()
        dgContractor.DataSource = mContractorList
        Session("mContractorList") = mContractorList

        lblResult.Text = "Contractor List: " & mContractorList.Count & " Record(s) Found."
        'dgContractor.DataBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtName.Enabled = True Then
                setFocus(txtCode)
            End If
            BackPage = Request.QueryString("Backpage")
            Session("BakPage") = BackPage

            If Session("NewContractor") <> "True" Then
                NewRecord()
            Else
                Session("NewContractor") = ""
            End If

            Session("mContractor") = mContractor
            DataFieldBind()
            'Else
            '    dgContractor.DataSource = mContractorList
            '    dgContractor.DataBind()
            '    lblResult.Text = "Contractor List: " & mContractorList.Count & " Record(s) Found."
            SetTitle()
            'SetSession()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeNew") And mContractor.IsNew) Or (Not User.IsInRole("EmployeeEdit") And Not mContractor.IsNew) Then
            setObject()
            SetSession()
            'MarkLog(Flypal.Util.Action.Save, "Contractor", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
            mContractorDetail = mContractor.Name + " Code : " + mContractor.Code
            MarkLog(Util.Action.Save, "Contractor", User.Identity.Name & " is not Authorized User to save " & mContractorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                setObject()
                mContractor.Save()
                If txtName.Enabled = True Then
                    setFocus(txtCode)
                End If
                'MarkLog(Flypal.Util.Action.Save, "Contractor", mContractor.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty)
                mContractorDetail = mContractor.Name + " Code : " + mContractor.Code
                MarkLog(Util.Action.Save, "Contractor", mContractorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                NewRecord()
                'txtState.Text = ""
                'txtCountry.Text = ""
                DataFieldBind()
                SetTitle()
                upnlContractorDetails.Update()
                upnlGrid.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                NewRecord()
                'DataFieldBind()
                SetTitle()
                upnlContractorDetails.Update()
            End Try
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If txtName.Enabled = True Then
            setFocus(txtCode)
        End If
        NewRecord()
        upnlContractorDetails.DataBind()
        MarkLog(Flypal.Util.Action.[New], "Contractor", "", Flypal.Util.ErrorType.NoError, mContractor.ID, EventLogID)
        'txtCode.Text = ""
        'txtName.Text = ""
        'txtAddress1.Text = ""
        'txtAddress2.Text = ""
        'txtAddress3.Text = ""
        'cmbCityInvList.SelectedIndex = 0
        'txtPhone1.Text = ""
        'txtPhone2.Text = ""
        'txtFax.Text = ""
        'txtEmail.Text = ""
        'txtWebsite.Text = ""
        'DataFieldBind()
        'lblTitle.Text = "Contractor Information [New]"
        SetTitle()
        upnlContractorDetails.Update()
    End Sub
    Private Sub btnCityInvList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCityInvList.Click
        setObject() 'Added Code
        Session("NewContractor") = "True"

        'Response.Redirect("wfCityInv_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage3=wfContractor.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Flypal.Util.Action.Close, "Contractor", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'Session("mContractorList") = mContractorList
        Session.Remove("NewContractor")
        Session.Remove("mContractor")
        Session.Remove("mContractorList")
        Session.Remove("mCityInvList")
        'Added by vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub dgContractorList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgContractor.RowCommand
        Dim Index As Integer
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgContractor.PageIndex * dgContractor.PageSize
                mID = New Guid(dgContractor.DataKeys(Index).Value.ToString)
                If (Not User.IsInRole("EmployeeView") And Not User.IsInRole("EmployeeEdit")) Then
                    setObject()
                    SetSession()
                    'MarkLog(Flypal.Util.Action.Edit, "Contractor", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                    mContractorDetail = mContractor.Name + " Code : " + mContractor.Code
                    MarkLog(Util.Action.Edit, "Contractor", User.Identity.Name & " is not Authorized User to edit " & mContractorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecord(mID)
                upnlContractorDetails.DataBind()
                'txtName.Text = mContractor.Name
                'txtCode.Text = mContractor.Code
                'txtAddress1.Text = mContractor.Address1
                'txtAddress2.Text = mContractor.Address2
                'txtAddress3.Text = mContractor.Address3
                'cmbCityInvList.SelectedValue = mContractor.CityID.ToString
                'txtState.Text = mContractor.StateName
                'txtCountry.Text = mContractor.CountryName
                'txtPhone1.Text = mContractor.PhoneNo1
                'txtPhone2.Text = mContractor.PhoneNo2
                'txtFax.Text = mContractor.Fax
                'txtEmail.Text = mContractor.Email
                'txtWebsite.Text = mContractor.Website

                mContractorDetail = mContractor.Name + " Code : " + mContractor.Code
                MarkLog(Util.Action.Edit, "Contractor", mContractorDetail, Util.ErrorType.HandledError, mID, EventLogID)

                If txtName.Enabled = True Then
                    setFocus(txtCode)
                End If
                SetTitle()
                upnlContractorDetails.Update()

            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgContractor.PageIndex * dgContractor.PageSize
                mID = New Guid(dgContractor.DataKeys(Index).Value.ToString)
                If (Not User.IsInRole("EmployeeDelete")) Then
                    setObject()
                    SetSession()
                    'MarkLog(Flypal.Util.Action.Delete, "Contractor", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                    mContractorDetail = mContractor.Name + " Code : " + mContractor.Code
                    MarkLog(Util.Action.Delete, "Contractor", User.Identity.Name & " is not Authorized User to delete " & mContractorDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If cmbSearchType.SelectedValue = 0 Then     'All
            mContractorList = ContractorList.GetContractorList(, , , )
        ElseIf cmbSearchType.SelectedValue = 1 Then     'Code
            mContractorList = ContractorList.GetContractorList(, Trim(txtFor.Text), , )
        ElseIf cmbSearchType.SelectedValue = 2 Then     'Name
            mContractorList = ContractorList.GetContractorList(, , txtFor.Text, )
        ElseIf cmbSearchType.SelectedValue = 3 Then     'City
            mContractorList = ContractorList.GetContractorList(, , , txtFor.Text)

        End If
        dgContractor.DataSource = mContractorList
        dgContractor.DataBind()
        Session("mContractorList") = mContractorList

        lblResult.Text = "Contractor List: " & mContractorList.Count & " Record(s) Found."
        upnlGrid.Update()
    End Sub
    Private Sub cmbCityInvList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCityInvList.SelectedIndexChanged
        txtState.Text = IIf(cmbCityInvList.SelectedIndex > 0, mCityInvList(cmbCityInvList.SelectedIndex).State, "")
        txtCountry.Text = IIf(cmbCityInvList.SelectedIndex > 0, mCityInvList(cmbCityInvList.SelectedIndex).Country, "")

        If cmbCityInvList.Enabled = True Then
            setFocus(cmbCityInvList)
        End If
    End Sub
    Private Sub cmbSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchType.SelectedIndexChanged
        txtFor.Text = ""
        If cmbSearchType.SelectedIndex = 0 Then
            txtFor.Visible = False
            lblFor.Visible = False
        Else
            txtFor.Visible = True
            lblFor.Visible = True
        End If

        setFocus(cmbSearchType)
    End Sub
    'Added By Prashant 23-June-2009 for grid sorting
    Private Sub dgContractor_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgContractor.Sorting
        mContractorList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mContractorList") = mContractorList
        dgContractor.DataSource = mContractorList
        dgContractor.DataBind()
    End Sub
    '------------------------------------------------
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub dgContractor_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgContractor.PageIndexChanging
        dgContractor.PageIndex = e.NewPageIndex
        dgContractor.DataSource = mContractorList
        Session("mContractorList") = mContractorList
        dgContractor.DataBind()
    End Sub
    Private Sub hdnimgBtnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCity.Click
        mCityInvList = CityInvList.GetCityList(0, , , True)
        cmbCityInvList.DataSource = mCityInvList
        Session("mCityInvList") = mCityInvList
        cmbCityInvList.DataBind()
        upnlContractorDetails.Update()
    End Sub
#End Region

   
    
    
End Class