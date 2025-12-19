'AJAX Conversion By Vikrant

Partial Class wfEmployeeDepartmentInfo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeDepartmentInfoList As EmployeeDepartmentInfoList
    Public mEmployeeDepartmentInfo As EmployeeDepartmentInfo
    Public mEmployeeDepartmentList As EmployeeDepartmentList
    Public BackPage As String
    Public DesignationID As Guid
    Dim EventLogID As Guid
    Dim mEmployeeDepartment As EmployeeDepartment
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeDepartmentInfo = Session("mEmployeeDepartmentInfo")
        mEmployeeDepartmentList = Session("mEmployeeDepartmentList")
        mEmployeeDepartment = Session("mEmployeeDepartment")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDepartmentInfo") = mEmployeeDepartmentInfo
        Session("mEmployeeDepartmentList") = mEmployeeDepartmentList
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub SetSessionForDeptMaster()
        Session("mEmployeeDepartment") = mEmployeeDepartment
        Session("mEmployeeDepartmentList") = mEmployeeDepartmentList
    End Sub
    Private Sub DataFieldBind()
        mEmployeeDepartmentList = EmployeeDepartmentList.GetEmployeeDepartmentList("(SELECT)")
        cmbEmployeeDepartmentList.DataSource = mEmployeeDepartmentList
        Session("mEmployeeDepartmentList") = mEmployeeDepartmentList
        txtDate.Text = mEmployeeDepartmentInfo.DateFormatted.ToString

        mEmployeeDepartmentInfo = Session("mEmployeeDepartmentInfo")

        upnlEmpDeptDetails.DataBind()
    End Sub
    Private Sub DataFieldBindForDeptMaster()
        mEmployeeDepartmentList = EmployeeDepartmentList.GetEmployeeDepartmentList()
        dgEmployeeDepartmentList.DataSource = mEmployeeDepartmentList
        Session("mEmployeeDepartmentList") = mEmployeeDepartmentList
        upnlDeptMaster.DataBind()
    End Sub
    Private Sub EditRecordForDeptMaster(ByVal mID As Guid)
        mEmployeeDepartment = EmployeeDepartment.GetEmployeeDepartment(mID)
        Session("mEmployeeDepartment") = mEmployeeDepartment
    End Sub
    Private Sub DeleteRecordForDeptMaster(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDeptMaster")
        mEmployeeDepartment = EmployeeDepartment.GetEmployeeDepartment(mID)
        Session("mEmployeeDepartment") = mEmployeeDepartment
    End Sub
    Private Sub NewRecordForDeptMaster()
        mEmployeeDepartment = EmployeeDepartment.NewEmployeeDepartment
        Session("mEmployeeDepartment") = mEmployeeDepartment
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Try
                        If MSGBoxCtrl.Sender = "DeleteDeptMaster" Then
                            Session("sender") = ""
                            EmployeeDepartment.DeleteEmployeeDepartment(mEmployeeDepartment.ID)
                            NewRecordForDeptMaster()
                            txtDepartment.Text = ""
                            lblTitleDeptMaster.Text = "Employee Department [New]"
                            DataFieldBindForDeptMaster()
                            upnlDeptMaster.Update()
                            'mEmployeeDepartmentInfo = Session("mEmployeeDepartmentInfo")
                            'Response.Redirect("wfEmployeeDepartmentInfo_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                        End If
                    Catch ex As SqlException
                        Dim stringInfo As String = ""
                        If ex.Message.Contains("tabDocumentLockerDepartment") Then
                            stringInfo = "Document Locker Department."
                        ElseIf ex.Message.Contains("tabEmployeeDepartmentInfo") Then
                            stringInfo = "Employee Department Info."
                        Else
                            stringInfo = ""
                        End If
                        If ex.Number = 8145 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 2627 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 547 Then
                            MarkLog(Flypal.Util.Action.Delete, "Employee Department", "Can't delete : " + mEmployeeDepartment.Name + "  is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDepartment.ID, EventLogID)
                            ''MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, stringInfo, MsgBoxStyle.OkOnly, "")
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                        End If
                        NewRecordForDeptMaster()
                        txtDepartment.Text = ""
                        lblTitleDeptMaster.Text = "Employee Department [New]"
                        txtDepartment.DataBind()
                        upnlDeptMaster.Update()
                        msgCount = ex.Errors.Count
                    Finally
                        If msgCount = 0 Then
                            MarkLog(Flypal.Util.Action.Delete, "Employee Department", mEmployeeDepartment.Name, Flypal.Util.ErrorType.NoError, mEmployeeDepartment.ID, EventLogID)
                        End If
                    End Try
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteDeptMaster" Then
                        NewRecordForDeptMaster()
                        txtDepartment.Text = ""
                        lblTitleDeptMaster.Text = "Employee Department [New]"
                        txtDepartment.DataBind()
                        upnlDeptMaster.Update()
                    End If
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        
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
        If mEmployeeDepartmentInfo.IsNew Then
            lblTitle.Text = "Employee Department Information [New]"
        Else
            If Len(mEmployeeDepartmentInfo.EmployeeDepartmentName) > 15 Then
                lblTitle.Text = "Employee Department Information [" & mEmployeeDepartmentInfo.EmployeeDepartmentName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Department Information [" & mEmployeeDepartmentInfo.EmployeeDepartmentName & "]"
            End If
        End If
        upnlTitle.Update()
        If User.IsInRole("EmployeeDepartmentNew") = False Then
            imgDepartment.Enabled = False
            imgDepartment.ToolTip = "You are not authorized user"
        End If
        upnlEmpDeptDetails.Update()
    End Sub
    Private Sub SetObject()
        mEmployeeDepartmentInfo.EmployeeID = mEmployee.ID
        mEmployeeDepartmentInfo.EmployeeDepartmentID = New Guid(cmbEmployeeDepartmentList.SelectedValue)
        mEmployeeDepartmentInfo.Date = CType(txtDate.Text, Object)
        mEmployeeDepartmentInfo.Remark = Trim(txtRemark.Text)

        Session("mEmployeeDepartmentInfo") = mEmployeeDepartmentInfo
    End Sub
    Private Sub SetObjectForDeptMaster()
        mEmployeeDepartment.Name = Trim(txtDepartment.Text)
    End Sub
    Private Sub AttachMyFile()
        Try
            mEmployeeDepartmentInfo.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeDepartmentInfo.ImageSize = Session("FileUpload.FileSize")
            mEmployeeDepartmentInfo.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeDepartmentInfo") = mEmployeeDepartmentInfo
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
#End Region

#Region "Department Child Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If txtDate.Enabled = True Then
                setFocus(cmbEmployeeDepartmentList)
            End If
            DataFieldBind()
            ControlVisibilityForAttachment()
            SetTitle()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeDepartmentNew") And mEmployeeDepartmentInfo.IsNew) Or (Not User.IsInRole("EmployeeDepartmentEdit") And Not mEmployeeDepartmentInfo.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "Employee Department", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Department : " + mEmployeeDepartmentInfo.EmployeeDepartmentName, Flypal.Util.ErrorType.HandledError, mEmployeeDepartmentInfo.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim clnEmployeeDepartmentInfo As EmployeeDepartmentInfo
        If IsValid Then
            Try

                clnEmployeeDepartmentInfo = CType(mEmployeeDepartmentInfo.Clone, EmployeeDepartmentInfo)

                SetObject()


                mEmployeeDepartmentInfoList = EmployeeDepartmentInfoList.GetEmployeeDepartmentTop1Info(mEmployeeDepartmentInfo.EmployeeID, , , , , , , True)
                If mEmployeeDepartmentInfoList.Count > 0 Then
                    If mEmployeeDepartmentInfo.IsNew = True Then   'New record
                        If CDate(mEmployeeDepartmentInfo.Date) < CDate(mEmployeeDepartmentInfoList(0).Date) Then
                            MSGBoxCtrl.show("Save Alert!", "Date should be greater than previous records date", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        Else
                            mEmployeeDepartmentInfo.Save()
                        End If
                    Else                                           'Edit record
                        If Not mEmployeeDepartmentInfoList(0).ID.Equals(mEmployeeDepartmentInfo.ID) Then
                            If CDate(mEmployeeDepartmentInfo.Date) > CDate(mEmployeeDepartmentInfoList(0).Date) Then
                                MSGBoxCtrl.show("Save Alert!", "Date should be greater than previous records date", "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            Else
                                mEmployeeDepartmentInfo.Save()
                            End If
                        Else
                            mEmployeeDepartmentInfo.Save()
                        End If
                    End If
                Else
                    mEmployeeDepartmentInfo.Save()
                End If

                mEmployee = Employee.GetEmployee(mEmployeeDepartmentInfo.EmployeeID)
                Session("mEmployee") = mEmployee
                MarkLog(Flypal.Util.Action.Save, "Employee Department", "Emp : " + mEmployee.EmpNoName + " Department : " + mEmployeeDepartmentInfo.EmployeeDepartmentName, Flypal.Util.ErrorType.NoError, mEmployeeDepartmentInfo.ID, EventLogID)
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
                    mEmployeeDepartmentInfo = clnEmployeeDepartmentInfo
                    Session("mEmployeeDepartmentInfo") = mEmployeeDepartmentInfo
                    MSGBoxCtrl.show("Save Alert!", ex.Message, "Because its FDTL/Log Entry has been done.", MsgBoxStyle.OkOnly, "")
                End If
                'add code CHK
            End Try
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgDepartment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgDepartment.Click
        mdlPopUpDeptMaster.Show()
        NewRecordForDeptMaster()
        DataFieldBindForDeptMaster()
        upnlDeptMaster.Update()
        'Response.Redirect("wfEmployeeDepartment.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeDepartmentInfo_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        If Not mEmployeeDepartmentInfo.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Department", "Emp : " + mEmployee.EmpNoName + " Department : " + mEmployeeDepartmentInfo.EmployeeDepartmentName, Flypal.Util.ErrorType.NoError, mEmployeeDepartmentInfo.ID, EventLogID)
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
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mEmployeeDepartmentInfo.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeDepartmentInfo.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDepartmentInfo.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployeeDepartmentInfo.ImageFile, 0, mEmployeeDepartmentInfo.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mEmployeeDepartmentInfo.ImageFile = file1
        mEmployeeDepartmentInfo.ImageSize = 0
        mEmployeeDepartmentInfo.FileExtension = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
#End Region

#Region "Department Master Events"
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlEmpDeptDetails.Update()
    End Sub
    Private Sub dgEmployeeDepartmentList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmployeeDepartmentList.RowCommand
        Dim mID As Guid
        Dim mName As String
        Dim Index As Int16
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgEmployeeDepartmentList.PageIndex * dgEmployeeDepartmentList.PageSize  'CInt(e.CommandArgument)
                mID = mEmployeeDepartmentList(Index).ID
                mName = mEmployeeDepartmentList(Index).Name

                If (Not User.IsInRole("EmployeeDepartmentView") And Not User.IsInRole("EmployeeDepartmentEdit")) Then
                    SetObjectForDeptMaster()
                    SetSessionForDeptMaster()
                    MarkLog(Flypal.Util.Action.Edit, "Employee Department", User.Identity.Name & " is not Authorized User to edit " + mName, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecordForDeptMaster(mID)
                txtDepartment.Text = mEmployeeDepartment.Name
                txtDepartment.DataBind()
                MarkLog(Flypal.Util.Action.Edit, "Employee Department", mEmployeeDepartment.Name, Flypal.Util.ErrorType.NoError, mEmployeeDepartment.ID, EventLogID)
                If Len(mEmployeeDepartment.Name) > 15 Then
                    lblTitleDeptMaster.Text = "Employee Department [" & mEmployeeDepartment.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleDeptMaster.Text = "Employee Department [" & mEmployeeDepartment.Name & " ]"
                End If
                If txtDepartment.Enabled = True Then
                    setFocus(txtDepartment)
                End If
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgEmployeeDepartmentList.PageIndex * dgEmployeeDepartmentList.PageSize 'CInt(e.CommandArgument)
                mID = mEmployeeDepartmentList(Index).ID
                mName = mEmployeeDepartmentList(Index).Name
                If (Not User.IsInRole("EmployeeDepartmentDelete")) Then
                    SetObjectForDeptMaster()
                    SetSessionForDeptMaster()
                    MarkLog(Flypal.Util.Action.Delete, "Employee Department", User.Identity.Name & " is not Authorized User to delete " + mName, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecordForDeptMaster(mID)
        End Select
    End Sub
    Private Sub btnSaveDeptMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDeptMaster.Click
        If (Not User.IsInRole("EmployeeDepartmentNew") And mEmployeeDepartment.IsNew) Or (Not User.IsInRole("EmployeeDepartmentEdit") And Not mEmployeeDepartment.IsNew) Then
            SetObjectForDeptMaster()
            SetSessionForDeptMaster()
            MarkLog(Flypal.Util.Action.Save, "Employee Department", User.Identity.Name & " is not Authorized User to save " + mEmployeeDepartment.Name, Flypal.Util.ErrorType.HandledError, mEmployeeDepartment.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        'Page.Validate("valGroupChild") 'CHK
        If IsValid Then
            Try
                SetObjectForDeptMaster()
                mEmployeeDepartment.Save()
                If txtDepartment.Enabled = True Then
                    setFocus(txtDepartment)
                End If
                MarkLog(Flypal.Util.Action.Save, "Employee Department", mEmployeeDepartment.Name, Flypal.Util.ErrorType.HandledError, mEmployeeDepartment.ID, EventLogID)
                NewRecordForDeptMaster()
                txtDepartment.Text = ""
                txtDepartment.DataBind()
                DataFieldBindForDeptMaster()
                SetSession()
                lblTitleDeptMaster.Text = "Employee Department [New]"
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
                NewRecordForDeptMaster()
                txtDepartment.Text = ""
                txtDepartment.DataBind()
                lblTitleDeptMaster.Text = "Employee Department [New]"
            End Try
        End If
    End Sub
    Private Sub btnCloseDeptMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseDeptMaster.Click
        MarkLog(Flypal.Util.Action.Close, "Employee Department", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSessionForDeptMaster()
        DataFieldBind()
        upnlEmpDeptDetails.Update()
        mdlPopUpDeptMaster.Hide()
    End Sub
    Private Sub btnNewDeptMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewDeptMaster.Click
        If txtDepartment.Enabled = True Then
            setFocus(txtDepartment)
        End If
        NewRecordForDeptMaster()
        MarkLog(Flypal.Util.Action.[New], "Employee Department", "", Flypal.Util.ErrorType.NoError, mEmployeeDepartment.ID, EventLogID)
        txtDepartment.Text = ""
        txtDepartment.DataBind()
        lblTitleDeptMaster.Text = "Employee Department [New]"
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mEmployeeDepartmentInfo.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub RemoveSessionForDeptMaster()
        Session.Remove("mEmployeeDepartmentList")
    End Sub

    Private Sub dgEmployeeDepartmentList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgEmployeeDepartmentList.PageIndexChanging
        dgEmployeeDepartmentList.PageIndex = e.NewPageIndex
        dgEmployeeDepartmentList.DataSource = mEmployeeDepartmentList
        Session("mEmployeeDepartmentList") = mEmployeeDepartmentList
        dgEmployeeDepartmentList.DataBind()
        'Session("PageIndex") = dgWOList.PageIndex
    End Sub
#End Region

End Class
