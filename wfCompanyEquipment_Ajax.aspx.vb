'AJAX Conversion By Vikrant

Partial Class wfCompanyEquipment_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeDepartmentInfoList As EmployeeDepartmentInfoList
    Public mCompanyEquipment As CompanyEquipment
    Public mEquipmentList As EquipmentList
    Public BackPage As String
    Public DesignationID As Guid
    Dim EventLogID As Guid
    Public mEquipment As Equipment
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mCompanyEquipment = Session("mCompanyEquipment")
        mEquipmentList = Session("mEquipmentList")
        mEquipment = Session("mEquipment")
    End Sub
    Private Sub SetSession()
        Session("mCompanyEquipment") = mCompanyEquipment
        Session("mEquipmentList") = mEquipmentList
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub RemoveSessionEquipmentMaster()
        Session.Remove("mEquipment")
        'Session.Remove("mCompanyEquipment")
        'Session.Remove("mEquipmentList")
    End Sub
    Private Sub DataFieldBind()
        mEquipmentList = EquipmentList.GetEquipmentList("(SELECT)")
        cmbEquipment.DataSource = mEquipmentList
        Session("mEquipmentList") = mEquipmentList
        calEquipmentIssuedDate.Text = mCompanyEquipment.EquipmentIssuedDateFormatted.ToString
        calEquipmentReturnDate.Text = mCompanyEquipment.EquipmentReturnedDateFormatted.ToString

        mCompanyEquipment = Session("mCompanyEquipment")

        upnlEquipmentDetails.DataBind()
    End Sub
    Private Sub DataFieldBindEquipmentMaster()
        mEquipmentList = EquipmentList.GetEquipmentList()
        dgEquipmentList.DataSource = mEquipmentList
        Session("mEquipmentList") = mEquipmentList
        upnlEquipmentMaster.DataBind()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteEquipmentMaster" Then
                        Try
                            Session("sender") = ""
                            mEquipment = Session("mEquipment")
                            Equipment.DeleteEquipment(mEquipment.ID)
                            NewRecordEquipmentMaster()
                            DataFieldBindEquipmentMaster()
                            txtEquipment.Text = ""
                            lblTitleEquipmentMaster.Text = "Equipment [New]"
                            upnlEquipmentMaster.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Flypal.Util.Action.Delete, "Equipment", "Can't delete : " + mEquipment.Name + "  is Currently in use", Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
                            End If
                            NewRecordEquipmentMaster()
                            DataFieldBindEquipmentMaster()
                            txtEquipment.Text = ""
                            lblTitleEquipmentMaster.Text = "Equipment [New]"
                            upnlEquipmentMaster.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Equipment", mEquipment.Name, Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteEquipmentMaster" Then
                        NewRecordEquipmentMaster()
                        txtEquipment.Text = ""
                        txtEquipment.DataBind()
                        lblTitleEquipmentMaster.Text = "Equipment [New]"
                        upnlEquipmentMaster.Update()
                    End If
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetTitle()
        If mCompanyEquipment.IsNew Then
            lblTitle.Text = "Company Equipment Information [New]"
        Else
            If Len(mCompanyEquipment.EquipmentName) > 15 Then
                lblTitle.Text = "Company Equipment Information [" & mCompanyEquipment.EquipmentName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Company Equipment Department Information [" & mCompanyEquipment.EquipmentName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub SetObject()
        mCompanyEquipment.EmployeeID = mEmployee.ID
        mCompanyEquipment.EquipmentID = New Guid(cmbEquipment.SelectedValue)
        mCompanyEquipment.EquipmentDetails = Trim(txtEquipmentDetails.Text)
        mCompanyEquipment.EquipmentIssuedDate = CType(calEquipmentIssuedDate.Text, Object)

        If calEquipmentReturnDate.Text = "" Then
            mCompanyEquipment.EquipmentReturnedDate = System.DBNull.Value
        Else
            mCompanyEquipment.EquipmentReturnedDate = calEquipmentReturnDate.Text
        End If
        mCompanyEquipment.Remark = Trim(txtRemark.Text)

        Session("mCompanyEquipment") = mCompanyEquipment
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "calEquipmentReturnDate" Then 'CHK
            If IsDate(calEquipmentIssuedDate.Text) And IsDate(calEquipmentReturnDate.Text) Then
                If ((calEquipmentReturnDate.Text <> "" And calEquipmentIssuedDate.Text <> "") And (New SmartDate(calEquipmentReturnDate.Text.ToString).Date < New SmartDate(calEquipmentIssuedDate.Text.ToString).Date)) Then
                    custValidator.ErrorMessage = "Return date should be greater or equal to issue date"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        End If
    End Sub
    Private Sub NewRecordEquipmentMaster()
        mEquipment = Equipment.NewEquipment
        Session("mEquipment") = mEquipment
    End Sub
    Private Sub EditRecordEquipmentMaster(ByVal mID As Guid)
        mEquipment = Equipment.GetEquipment(mID)
        Session("mEquipment") = mEquipment
    End Sub
    Private Sub DeleteRecordEquipmentMaster(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteEquipmentMaster")
        mEquipment = Equipment.GetEquipment(mID)
        Session("mEquipment") = mEquipment
    End Sub
    Private Sub SetObjectEquipmentMaster()
        mEquipment.Name = Trim(txtEquipment.Text)
    End Sub
    Private Sub SetSessionEquipmentMaster()
        Session("mEquipment") = mEquipment
        Session("mEquipmentList") = mEquipmentList
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeView") And mCompanyEquipment.IsNew) Or (Not User.IsInRole("EmployeeEdit") And Not mCompanyEquipment.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "Employee Equipment", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + "Equipment : " + mCompanyEquipment.EquipmentName, Flypal.Util.ErrorType.HandledError, mCompanyEquipment.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim clnEmployeeDepartmentInfo As CompanyEquipment

        If IsValid Then
            Try

                clnEmployeeDepartmentInfo = CType(mCompanyEquipment.Clone, CompanyEquipment)

                SetObject()

                'mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentTop1Info(mCompanyEquipment.EmployeeID, "", "1/1/1900", "1/1/2200", "", "", False, True)
                'If mEmployeeDepartmentInfoList.Count > 0 Then
                'If mCompanyEquipment.IsNew = True Then   'New record
                'If CDate(mCompanyEquipment.Date) < CDate(mEmployeeDepartmentInfoList(0).Date) Then
                '     ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", MessageBox.Show("Date should be greater than previous records date"))
                '    Exit Sub
                'Else
                mCompanyEquipment.Save()
                'End If
                'Else                                           'Edit record
                '    'If CDate(mCompanyEquipment.Date) > CDate(mEmployeeDepartmentInfoList(0).Date) Then
                '    '     ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", MessageBox.Show("Date should not be greater than previous records date"))
                '    '    Exit Sub
                '    'Else
                '    mCompanyEquipment.Save()
                '    'End If
                'End If
                'Else
                '    mCompanyEquipment.Save()
                'End If

                mEmployee = Employee.GetEmployee(mCompanyEquipment.EmployeeID)
                Session("mEmployee") = mEmployee
                MarkLog(Flypal.Util.Action.Save, "Employee Department", "Emp : " + mEmployee.EmpNoName + " Equipment : " + mCompanyEquipment.EquipmentName, Flypal.Util.ErrorType.NoError, mCompanyEquipment.ID, EventLogID)
                SetSession()
                lblTitle.Text = "Employee Department Information [New]"
                'Added by Vikrant on 20-nov-2013 for popup
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                'End
                'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 50000 Then
                    mCompanyEquipment = clnEmployeeDepartmentInfo
                    Session("mCompanyEquipment") = mCompanyEquipment
                    MSGBoxCtrl.show("Save Alert!", ex.Message, "Because its FDTL/Log Entry has been done.", MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgEquipment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgEquipment.Click
        SetObject()
        NewRecordEquipmentMaster()
        DataFieldBindEquipmentMaster()
        mdlPopUpEquipmentMaster.Show()
        upnlEquipmentMaster.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        If Not mCompanyEquipment.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Equipment", "Emp : " + mEmployee.EmpNoName + " Equipment : " + mCompanyEquipment.EquipmentName, Flypal.Util.ErrorType.NoError, mCompanyEquipment.ID, EventLogID)
        End If
        'Added by Vikrant on 20-nov-2013 for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub calEquipmentIssuedDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calEquipmentIssuedDate.TextChanged
        If IsDate(calEquipmentIssuedDate.Text) Or (calEquipmentIssuedDate.Text = "") Then
            If calEquipmentIssuedDate.Text = "" Then
                mCompanyEquipment.EquipmentIssuedDate = System.DBNull.Value
                calEquipmentIssuedDate.Text = mCompanyEquipment.EquipmentIssuedDateFormatted.ToString
            Else
                mCompanyEquipment.EquipmentIssuedDate = calEquipmentIssuedDate.Text
                calEquipmentIssuedDate.Text = mCompanyEquipment.EquipmentIssuedDateFormatted.ToString
            End If

        Else
            calEquipmentIssuedDate.Text = ""
        End If
    End Sub
    Private Sub calEquipmentReturnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calEquipmentReturnDate.TextChanged
        If IsDate(calEquipmentReturnDate.Text) Or (calEquipmentReturnDate.Text = "") Then
            If calEquipmentReturnDate.Text = "" Then
                mCompanyEquipment.EquipmentReturnedDate = System.DBNull.Value
                calEquipmentReturnDate.Text = mCompanyEquipment.EquipmentReturnedDateFormatted.ToString
            Else
                mCompanyEquipment.EquipmentReturnedDate = calEquipmentReturnDate.Text
                calEquipmentReturnDate.Text = mCompanyEquipment.EquipmentReturnedDateFormatted.ToString
            End If

        Else
            calEquipmentReturnDate.Text = ""
        End If
    End Sub
    Private Sub btnNewEquipmentMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewEquipmentMaster.Click
        If txtEquipment.Enabled = True Then
            setFocus(txtEquipment)
        End If
        NewRecordEquipmentMaster()
        MarkLog(Flypal.Util.Action.[New], "Equipment", "", Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
        txtEquipment.DataBind()
        lblTitleEquipmentMaster.Text = "Equipment [New]"
    End Sub
    Private Sub btnCloseEquipmentMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseEquipmentMaster.Click
        RemoveSessionEquipmentMaster()
        DataFieldBind()
        upnlEquipmentDetails.Update()
        mdlPopUpEquipmentMaster.Hide()
    End Sub
    Private Sub btnSaveEquipmentMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveEquipmentMaster.Click
        If (Not User.IsInRole("EmployeeNew") And mEquipment.IsNew) Or (Not User.IsInRole("EmployeeEdit") And Not mEquipment.IsNew) Then
            SetObjectEquipmentMaster()
            SetSessionEquipmentMaster()
            MarkLog(Flypal.Util.Action.Save, "Equipment", User.Identity.Name & " is not Authorized User to save " + mEquipment.Name, Flypal.Util.ErrorType.HandledError, mEquipment.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObjectEquipmentMaster()
                mEquipment.Save()
                If txtEquipment.Enabled = True Then
                    setFocus(txtEquipment)
                End If
                MarkLog(Flypal.Util.Action.Save, "Equipment", mEquipment.Name, Flypal.Util.ErrorType.HandledError, mEquipment.ID, EventLogID)
                NewRecordEquipmentMaster()
                txtEquipment.Text = ""
                DataFieldBindEquipmentMaster()
                'SetSession()
                lblTitleEquipmentMaster.Text = "Equipment [New]"
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    Private Sub dgEquipmentList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEquipmentList.RowCommand
        Dim mID As Guid
        Dim Index As String
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgEquipmentList.PageIndex * dgEquipmentList.PageSize
                mID = dgEquipmentList.DataKeys(CInt(e.CommandArgument)).Value
                If (Not User.IsInRole("EmployeeView") And Not User.IsInRole("EmployeeEdit")) Then
                    SetObjectEquipmentMaster()
                    SetSessionEquipmentMaster()
                    MarkLog(Flypal.Util.Action.Edit, "Equipment", User.Identity.Name & " is not Authorized User to edit " + mEquipment.Name, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecordEquipmentMaster(mID)
                txtEquipment.DataBind()
                MarkLog(Flypal.Util.Action.Edit, "Equipment", mEquipment.Name, Flypal.Util.ErrorType.NoError, mEquipment.ID, EventLogID)
                If Len(mEquipment.Name) > 15 Then
                    lblTitleEquipmentMaster.Text = "Equipment [" & mEquipment.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleEquipmentMaster.Text = "Equipment [" & mEquipment.Name & " ]"
                End If
                If txtEquipment.Enabled = True Then
                    setFocus(txtEquipment)
                End If
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgEquipmentList.PageIndex * dgEquipmentList.PageSize
                mID = dgEquipmentList.DataKeys(CInt(e.CommandArgument)).Value

                If (Not User.IsInRole("EmployeeDelete")) Then
                    SetObjectEquipmentMaster()
                    SetSessionEquipmentMaster()
                    MarkLog(Flypal.Util.Action.Delete, "Equipment", User.Identity.Name & " is not Authorized User to delete " + mEquipment.Name, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecordEquipmentMaster(mID)
        End Select
    End Sub
#End Region

End Class
