'Added by Prashant

Public Class wfCityInv_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCityInvList As CityInvList
    Public mCityInv As CityInv
    Public mStateList As StateList
    Public BackPage As String
    Public IsFromRenewal As String = ""
    Dim EventLogID As Guid

    Public mState As State
    Public mCountryList As CountryList

    Public mCountry As Country

#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mCityInv = Session("mCityInv")
        mCityInvList = Session("mCityInvList")
        mStateList = Session("mStateList")
        IsFromRenewal = Request.QueryString("IsFromRenewal")


        mCountryList = Session("mCountryList")
        mStateList = Session("mStateList")
        mState = Session("mState")

        mCountry = Session("mCountry")
    End Sub
    Private Sub SetSession()
        Session("mCityInv") = mCityInv
        Session("mCityInvList") = mCityInvList
        Session("mStateList") = mStateList
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub NewRecord()
        mCityInv = CityInv.NewCity
        mStateList = StateList.GetStateList(0, "", "")
        Session("mCityInv") = mCityInv
        Session("mStateList") = mStateList
        CityInvTitle()
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mCityInv = CityInv.GetCity(mID)
        Session("mCityInv") = mCityInv
        CityInvTitle()
        txtName.Enabled = True
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
        'Session("sender") = "Delete"
        'msg1.Show()
        GridBind()
        mCityInv = CityInv.GetCity(mID)
        Session("mCityInv") = mCityInv
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub SetObject()
        mCityInv.Name = txtName.Text
        Try
            mCityInv.stateID = New Guid(cmbStateName.SelectedValue)
        Catch ex As Exception
            mCityInv.stateID = Guid.Empty
        End Try
    End Sub
    'Private Sub MessageBoxResult()
    '    Dim Result1 As MsgBoxResult
    '    Dim msgCount As Integer = 0
    '    If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
    '        Result1 = -1
    '    Else
    '        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
    '    End If
    '    If Result1 > 0 Then
    '        Select Case Result1
    '            'code by manisha....5/5/06
    '            'Case MsgBoxResult.OK
    '            '        If CType(Session("sender"), String) = "Delete" Then
    '            '            Session("sender") = ""
    '            '            'mCityInv = Session("mCityInv")
    '            '            'mCityInv.DeleteCity(mCityInv.ID)
    '            '            Response.Redirect("wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type"))
    '            '        End If
    '            Case MsgBoxResult.Yes
    '                If CType(Session("sender"), String) = "Delete" Then
    '                    Try
    '                        Session("sender") = ""
    '                        mCityInv = Session("mCityInv")
    '                        CityInv.DeleteCity(mCityInv.ID)
    '                        Response.Redirect("wfCityInv.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    '                    Catch ex As SqlException
    '                        If ex.Number = 8145 Then
    '                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
    '                            msg1.ReplacePage = "wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
    '                            msg1.Show()
    '                        ElseIf ex.Number = 2627 Then
    '                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
    '                            msg1.ReplacePage = "wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
    '                            msg1.Show()
    '                        ElseIf ex.Number = 547 Then
    '                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
    '                            msg1.ReplacePage = "wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
    '                            MarkLog(Flypal.Util.Action.Delete, "City", "Can't delete :" & mCityInv.Name & " is Currently in use", Flypal.Util.ErrorType.NoError, mCityInv.ID, EventLogID)
    '                            msg1.Show()
    '                        End If
    '                        DataFieldBind() ''Rajnish
    '                        msgCount = ex.Errors.Count
    '                    Finally
    '                        If msgCount = 0 Then
    '                            MarkLog(Flypal.Util.Action.Delete, "City", mCityInv.Name, Flypal.Util.ErrorType.NoError, mCityInv.ID, EventLogID)
    '                        End If
    '                    End Try
    '                End If
    '            Case MsgBoxResult.No
    '                Session("sender") = ""
    '                Response.Redirect("wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    '            Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
    '                Session("sender") = ""
    '                DataFieldBind()
    '                Response.Redirect("wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    '            Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
    '                Session("sender") = ""
    '                DataFieldBind()
    '                Response.Redirect("wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    '        End Select
    '    ElseIf Result1 = -1 Then
    '        Session("sender") = ""
    '        DataFieldBind()
    '        Response.Redirect("wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    '    ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
    '        Session("sender") = ""
    '        DataFieldBind()
    '    End If
    'End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mCityInv = Session("mCityInv")
                            CityInv.DeleteCity(mCityInv.ID)
                            NewRecord()
                            txtCountry.Text = ""
                            DataFieldBind()
                            SetGrid()
                            upnlDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                txtCountry.Text = ""
                                Exit Sub
                            End If
                        Finally
                            SetGrid()
                            upnlGridView.Update()
                            MarkLog(Util.Action.Delete, "WorkShop", mCityInv.Name, Util.ErrorType.NoError, mCityInv.ID, EventLogID)
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteState" Then
                        Try
                            mState = Session("mState")
                            State.DeleteState(mState.ID)
                            NewState()
                            DataFieldBindOfState()
                            SetStateGrid()
                            upnlState.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "ReferenceStateDelete")
                                NewState()
                                SetStateGrid()
                                Exit Sub
                            End If
                        Finally
                            SetStateGrid()
                            MarkLog(Util.Action.Delete, "WorkShop", mState.Name, Util.ErrorType.NoError, mState.ID, EventLogID)
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCountry" Then
                        Try
                            mCountry = Session("mCountry")
                            Country.DeleteCountry(mCountry.ID)
                            NewCountry()
                            DataFieldBindOfCountry()
                            upnlCountry.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "ReferenceCountryDelete")
                                NewCountry()
                                SetCountryGrid()
                                Exit Sub
                            End If
                        Finally
                            SetCountryGrid()
                            MarkLog(Util.Action.Delete, "WorkShop", mCountry.Name, Util.ErrorType.NoError, mCountry.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        SetGrid()
                        upnlDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "DeleteState" Then
                        NewState()
                        DataFieldBindOfState()
                        SetStateGrid()
                        upnlState.Update()
                    Else
                        NewCountry()
                        DataFieldBindOfCountry()
                        SetCountryGrid()
                        upnlCountry.Update()
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "ReferenceStateDelete" Then
                        NewState()
                        DataFieldBindOfState()
                        SetStateGrid()
                        upnlState.Update()
                    ElseIf MSGBoxCtrl.Sender = "ReferenceCountryDelete" Then
                        NewCountry()
                        DataFieldBindOfCountry()
                        SetCountryGrid()
                        upnlCountry.Update()
                    Else
                        NewRecord()
                        DataFieldBind()
                        SetGrid()
                        upnlDetails.Update()
                    End If
            End Select
        End If
    End Sub
    Private Sub CityInvTitle()
        If mCityInv.IsNew Then
            lblTitle.Text = "City Information [New]"
        Else
            If Len(mCityInv.Name) > 15 Then
                lblTitle.Text = "City [" & mCityInv.Name.Substring(0, 15) & "... ]"
            Else
                lblTitle.Text = "City [" & mCityInv.Name & " ]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub GridBind()
        dgCity.DataSource = mCityInvList
        dgCity.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerCityInv(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
    Private Sub SetGrid()
        Dim IsSyncFromCRS As Boolean
        For j As Integer = 0 To dgCity.Rows.Count - 1
            IsSyncFromCRS = CType(Me.dgCity.Rows(j).Cells(6).Text, Boolean)

            If IsSyncFromCRS = True Then

                dgCity.Rows(j).Cells(3).Enabled = False
                dgCity.Rows(j).Cells(4).Enabled = False

            End If
        Next
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind()
        mStateList = StateList.GetStateList(0, "", "", True)
        cmbStateName.DataSource = mStateList
        Session("mStateList") = mStateList
        cmbStateName.DataBind()
        txtName.DataBind()

        mCityInvList = CityInvList.GetCityList(0, "", "", False)
        dgCity.DataSource = mCityInvList
        Session("mCityInvList") = mCityInvList
        GridBind()
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtName" Then
            If Len(Trim(txtName.Text)) > 25 Then
                CustValid.ErrorMessage = " City Name too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "cmbStateName" Then
            If cmbStateName.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Please select the State "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by Vikrant on 20-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            BackPage = Request.QueryString("Backpage")
            Session("BackPage") = BackPage

            If Session("NewCity") <> "True" Then
                NewRecord()
            Else
                Session("NewCity") = ""
            End If

            Session("mCityInv") = mCityInv
            DataFieldBind()
            SetGrid()
            'Else
            '    dgCity.DataSource = mCityInvList
            '    dgCity.DataBind()
        End If
        SetSession()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Rights for Employee are added by Amrita on 10-Apr-2009
        If (Not (User.IsInRole("VendorNew") Or User.IsInRole("EmployeeNew") Or User.IsInRole("WorkShopNew")) And mCityInv.IsNew) Or (Not (User.IsInRole("VendorEdit") Or User.IsInRole("EmployeeEdit") Or User.IsInRole("WorkShopEdit")) And Not mCityInv.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Page.Validate("a")
        If IsValid Then
            Try
                SetObject()
                mCityInv.Save()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                MarkLog(Flypal.Util.Action.Save, "City", mCityInv.Name, Flypal.Util.ErrorType.HandledError, mCityInv.ID, EventLogID)
                NewRecord()
                txtName.DataBind()
                txtCountry.DataBind()
                cmbStateName.DataBind()
                DataFieldBind()
                SetGrid()
                SetSession()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End Try
            txtName.Text = ""
            txtCountry.Text = ""
            cmbStateName.SelectedIndex = 0
            upnlDetails.Update()
        Else
            GridBind()
            SetGrid()
            upnlTitle.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        MarkLog(Flypal.Util.Action.[New], "City", "", Flypal.Util.ErrorType.NoError, mCityInv.ID, EventLogID)
        NewRecord()
        txtName.Text = ""
        txtCountry.Text = ""
        cmbStateName.SelectedIndex = 0
        DataFieldBind()
        SetGrid()
        upnlDetails.Update()
    End Sub
    Private Sub btnBackBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackBottom.Click
        MarkLog(Flypal.Util.Action.Close, "City", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("mCityInvList") = mCityInvList
        If Request.QueryString("BackPage3") = "wfStoreLocation_Ajax.aspx" Then
            Session("sender") = "CityList"
        Else
            Session("sender") = ""
        End If
        Session.Remove("NewCity")
        'Added by vikrant for City popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        If Request.QueryString("ChildPage3") = "wfTrainingOrg_Ajax.aspx" Or Request.QueryString("ChildPage3") = "wfEmployeeContactInfo_Ajax.aspx" Or Request.QueryString("ChildPage3") = "wfContractor.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
        Else
            Response.Redirect(Request.QueryString("BackPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&Type=" & Request.QueryString("Type") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
        End If
    End Sub
    Private Sub dgCity_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCity.RowCommand
        Select Case e.CommandName
            Case "EditView"
                If (Not (User.IsInRole("VendorView") Or User.IsInRole("EmployeeView") Or User.IsInRole("WorkShopView")) _
                    And Not (User.IsInRole("VendorEdit") Or User.IsInRole("EmployeeEdit") Or User.IsInRole("WorkShopEdit")) _
                    ) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgCity.PageIndex * dgCity.PageSize
                Dim mID As Guid = mCityInvList(index).ID
                'Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditRecord(mID)
                setFocus(txtName)
                txtName.DataBind()

                cmbStateName.SelectedValue = mCityInv.stateID.ToString
                txtCountry.Text = mStateList(cmbStateName.SelectedIndex).CountryName
                cmbStateName.DataBind()
                upnlDetails.Update()
                upnlTitle.Update()
                GridBind()
                DisableName(mID) 'Added by : Shital 19-Jun-2020, ALL16062020
                SetGrid()
                MarkLog(Util.Action.Edit, "WorkShop", mCityInv.Name, Util.ErrorType.NoError, mCityInv.ID, EventLogID)
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
            Case "Remove"
                If (Not (User.IsInRole("VendorDelete") Or User.IsInRole("EmployeeDelete") Or User.IsInRole("WorkShopDelete"))) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgCity.PageIndex * dgCity.PageSize
                Dim mID As Guid = mCityInvList(index).ID
                'Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                upnlTitle.Update()
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub cmbStateName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStateName.SelectedIndexChanged
        txtCountry.Text = IIf(cmbStateName.SelectedIndex > 0, mStateList(cmbStateName.SelectedIndex).CountryName, "")
        If cmbStateName.Enabled = True Then
            setFocus(cmbStateName)
        End If
    End Sub
    Private Sub dgCity_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCity.PageIndexChanging
        dgCity.PageIndex = e.NewPageIndex
        dgCity.DataSource = mCityInvList
        Session("mCityInvList") = mCityInvList
        GridBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

#Region " State "
    Private Sub NewState()
        mState = State.Newstate
        txtStateName.Text = ""
        mCountryList = CountryList.GetCountryList
        Session("mState") = mState
        Session("mCountryList") = mCountryList
        StateTitle()
    End Sub
    Private Sub EditStateRecord(ByVal mID As Guid)
        mState = State.GetState(mID)
        Session("mState") = mState
        StateTitle()
    End Sub
    Private Sub DeleteStateRecord(ByVal mID As Guid)
        EditStateRecord(mID)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteState")
    End Sub
    Private Sub SetStateGrid()
        Dim IsSyncFromCRS As Boolean
        For j As Integer = 0 To dgState.Rows.Count - 1
            IsSyncFromCRS = CType(Me.dgState.Rows(j).Cells(4).Text, Boolean)

            'If IsSyncFromCRS = True Then

            '    dgState.Rows(j).Cells(3).Enabled = False
            '    dgState.Rows(j).Cells(4).Enabled = False

            'End If
        Next
    End Sub
    Private Sub SetObjectOfState()
        mState.Name = txtStateName.Text
        Try
            mState.CountryID = New Guid(cmbCountry.SelectedValue)
        Catch ex As Exception
            mState.CountryID = Guid.Empty
        End Try
    End Sub
    Private Sub StateTitle()
        If mState.IsNew Then
            lblStateTitle.Text = "State Informaion [New]"
        Else
            If Len(mState.Name) > 15 Then
                lblStateTitle.Text = "State Information [" & mState.Name.Substring(0, 15) & "...]"
            Else
                lblStateTitle.Text = "State Information [" & mState.Name & "]"
            End If
        End If
    End Sub
    Private Sub DataFieldBindOfState()
        mCountryList = CountryList.GetCountryList(True)
        cmbCountry.DataSource = mCountryList
        Session("mCountryList") = mCountryList
        cmbCountry.DataBind()
        mStateList = StateList.GetStateList(0, "", "", False)
        Session("mStateList") = mStateList
        dgState.DataSource = mStateList
        dgState.DataBind()
        txtStateName.DataBind()
        upnlState.Update()
    End Sub
    Private Sub imgbtnState_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnState.Click
        NewState()
        DataFieldBindOfState()
        SetStateGrid()
        upnlState.Update()
        mdlState.Show()
    End Sub
    Protected Sub btnSaveState_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveState.Click
        If IsValid Then
            Try
                SetObjectOfState()
                mState.Save()
                txtStateName.DataBind()
                cmbCountry.DataBind()
                MarkLog(Flypal.Util.Action.Save, "State", mState.Name, Flypal.Util.ErrorType.HandledError, mState.ID, EventLogID)
                NewState()
                DataFieldBindOfState()
                SetStateGrid()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End Try
        End If
    End Sub
    Private Sub btnNewState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewState.Click
        If txtStateName.Enabled = True Then
            setFocus(txtStateName)
        End If
        MarkLog(Flypal.Util.Action.[New], "State", "", Flypal.Util.ErrorType.NoError, mState.ID, EventLogID)
        NewState()
        txtStateName.Text = ""
        cmbCountry.SelectedIndex = 0
        DataFieldBindOfState()
        SetStateGrid()
    End Sub
    Private Sub dgState_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgState.RowCommand
        Select Case e.CommandName
            Case "EditView"
                'If (Not (User.IsInRole("VendorView") Or User.IsInRole("EmployeeView") Or User.IsInRole("WorkShopView")) _
                '    And Not (User.IsInRole("VendorEdit") Or User.IsInRole("EmployeeEdit") Or User.IsInRole("WorkShopEdit")) _
                '    ) Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                Dim index As Integer = CInt(e.CommandArgument) + dgState.PageIndex * dgState.PageSize
                Dim mID As Guid = mStateList(index).ID
                'Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditStateRecord(mID)
                txtStateName.DataBind()

                cmbCountry.SelectedValue = mState.CountryID.ToString
                cmbCountry.DataBind()

                upnlState.Update()
                SetStateGrid()
                MarkLog(Flypal.Util.Action.Edit, "State", mState.Name, Flypal.Util.ErrorType.NoError, mState.ID, EventLogID)
            Case "Remove"
                'If (Not (User.IsInRole("VendorDelete") Or User.IsInRole("EmployeeDelete") Or User.IsInRole("WorkShopDelete"))) Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                Dim index As Integer = CInt(e.CommandArgument) + dgState.PageIndex * dgState.PageSize
                Dim mID As Guid = mStateList(index).ID
                'Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                DeleteStateRecord(mID)
        End Select
    End Sub
    Private Sub dgState_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgState.PageIndexChanging
        dgState.PageIndex = e.NewPageIndex
        dgState.DataSource = mStateList
        dgState.DataBind()
        Session("mStateList") = mStateList
        SetStateGrid()
        upnlState.Update()
    End Sub
    Private Sub btnCloseState_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseState.Click
        dgState.PageIndex = 0
        Session.Remove("mCountryList")
        Session.Remove("mStateList")
        DataFieldBind()
        SetGrid()
        upnlDetails.Update()
        mdlState.Hide()
    End Sub
#End Region

#Region " Country "
    Private Sub NewCountry()
        mCountry = Country.NewCountry
        Session("mCountry") = mCountry
        CountryTitle()
    End Sub
    Private Sub EditCountryRecord(ByVal mId As Guid)
        mCountry = Country.GetCountry(mId)
        Session("mCountry") = mCountry
        CountryTitle()
    End Sub
    Private Sub DeleteCountryRecord(ByVal mId As Guid)
        GridViewForCountry()
        EditCountryRecord(mId)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteCountry")
    End Sub
    Private Sub SetObjectOfCountry()
        mCountry.Name = txtCountryName.Text
    End Sub
    Private Sub SetCountryGrid()
        Dim IsSyncFromCRS As Boolean
        For j As Integer = 0 To dgCountry.Rows.Count - 1
            IsSyncFromCRS = CType(Me.dgCountry.Rows(j).Cells(3).Text, Boolean)

            'If IsSyncFromCRS = True Then

            '    dgCountry.Rows(j).Cells(2).Enabled = False
            '    dgCountry.Rows(j).Cells(3).Enabled = False

            'End If
        Next
    End Sub
    Private Sub CountryTitle()
        If mCountry.IsNew Then
            lblCountryTitle.Text = "Country Information [New]"
        Else
            If Len(mCountry.Name) > 15 Then
                lblCountryTitle.Text = "Country Information [" & mCountry.Name.Substring(0, 15) & "...]"
            Else
                lblCountryTitle.Text = "Country Information [" & mCountry.Name & "]"
            End If
        End If
    End Sub
    Private Sub DataFieldBindOfCountry()
        mCountryList = CountryList.GetCountryList(False)
        dgCountry.DataSource = mCountryList
        Session("mCountryList") = mCountryList
        dgCountry.DataBind()
        txtCountryName.DataBind()
        upnlCountry.Update()
    End Sub
    Private Sub GridViewForCountry()
        dgCountry.DataSource = mCountryList
        dgCountry.DataBind()
    End Sub
    '  Private Sub imgbtnCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgbtnCountry.Click
    Private Sub imgCountry_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCountry.Click
        NewCountry()
        DataFieldBindOfCountry()
        SetCountryGrid()
        upnlCountry.Update()
        mdlCountry.Show()
    End Sub
    Protected Sub btnSaveCountry_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveCountry.Click
        If IsValid Then
            Try
                SetObjectOfCountry()
                mCountry.Save()
                txtCountryName.DataBind()
                MarkLog(Flypal.Util.Action.Save, "Country", mCountry.Name, Flypal.Util.ErrorType.HandledError, mCountry.ID, EventLogID)
                NewCountry()
                DataFieldBindOfCountry()
                SetCountryGrid()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    GridViewForCountry()
                    Exit Sub
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    GridViewForCountry()
                    Exit Sub
                End If
            End Try
        End If
    End Sub
    Private Sub btnNewCountry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewCountry.Click
        If txtCountryName.Enabled = True Then
            setFocus(txtCountryName)
        End If
        MarkLog(Flypal.Util.Action.[New], "Country", "", Flypal.Util.ErrorType.NoError, mCountry.ID, EventLogID)
        NewCountry()
        txtCountryName.Text = ""
        DataFieldBindOfCountry()
        SetCountryGrid()
    End Sub
    Private Sub dgCountry_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCountry.RowCommand
        Select Case e.CommandName
            Case "EditView"
                'If (Not (User.IsInRole("VendorView") Or User.IsInRole("EmployeeView") Or User.IsInRole("WorkShopView")) _
                '    And Not (User.IsInRole("VendorEdit") Or User.IsInRole("EmployeeEdit") Or User.IsInRole("WorkShopEdit")) _
                '    ) Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                'Dim index As Integer = CInt(e.CommandArgument) + dgCountry.PageIndex * dgCountry.PageSize
                'Dim mID As Guid = mCountryList(index).ID
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                EditCountryRecord(mID)
                txtCountryName.DataBind()
                GridViewForCountry()
                SetCountryGrid()
                upnlCountry.Update()
                MarkLog(Flypal.Util.Action.Edit, "Country", mCountry.Name, Flypal.Util.ErrorType.NoError, mCountry.ID, EventLogID)
            Case "Remove"
                'If (Not (User.IsInRole("VendorDelete") Or User.IsInRole("EmployeeDelete") Or User.IsInRole("WorkShopDelete"))) Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                'Dim index As Integer = CInt(e.CommandArgument) + dgCountry.PageIndex * dgCountry.PageSize
                'Dim mID As Guid = mCountryList(index).ID
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                DeleteCountryRecord(mID)
        End Select
    End Sub
    Private Sub dgCountry_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCountry.PageIndexChanging
        dgCountry.PageIndex = e.NewPageIndex
        GridViewForCountry()
        Session("mCountryList") = mCountryList
        SetCountryGrid()
        upnlCountry.Update()
    End Sub
    Private Sub btnCloseCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseCountry.Click
        DataFieldBindOfState()
        SetStateGrid()
        dgCountry.PageIndex = 0
        mdlCountry.Hide()
    End Sub
#End Region

End Class
