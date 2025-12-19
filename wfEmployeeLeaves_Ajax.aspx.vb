
'****************************************************************************************
'Class name : EmployeeLeaves
'Developed By : Saylee
'Date : 20-Jan-10
'****************************************************************************************

'AJAX Conversion By Vikrant

Partial Class wfEmployeeLeaves_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeLeave As EmployeeLeave

    Public mClassificationList As ClassificationList

    Public BackPage As String
    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
    Public mClassification As Classification
#End Region

#Region " Helper Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeLeave = Session("mEmployeeLeave")
        'mEmployeeLeaveList = Session("mEmployeeLeaveList")
        mClassificationList = Session("mClassificationList")
        mClassification = Session("mClassification")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeLeave") = mEmployeeLeave
        'Session("mEmployeeLeaveList") = mEmployeeLeaveList
        Session("mClassificationList") = mClassificationList
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub DataFieldBind()
        mClassificationList = ClassificationList.GetClassificationList("(SELECT)")
        cmbClassificationList.DataSource = mClassificationList
        Session("mClassificationList") = mClassificationList

        calFromDate.Text = mEmployeeLeave.FromDateFormatted.ToString
        calToDate.Text = mEmployeeLeave.ToDate.ToString
        calReJoiningDate.Text = mEmployeeLeave.ReJoiningDateFormatted.ToString

        upnlLeaveDetails.DataBind()
    End Sub
    Private Sub addAttributes()
        txtNoOfDays.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNoOfDays').value,event)")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteLeaveMaster" Then
                        Try
                            Session("sender") = ""
                            mClassification = Session("mClassification")
                            Classification.DeleteClassification(mClassification.ID)
                            NewRecordLeaveMaster()
                            DataFieldBindLeaveMaster()
                            lblTitleLeaveMaster.Text = "Classification Information [New]"
                            upnlLeaveMaster.Update()
                            'Response.Redirect("wfClassification.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                               MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Changed by Vikrant on 22-July-2011
                                MarkLog(Flypal.Util.Action.Delete, "Classification", "Can't delete :" & mClassification.Name & " is Currently in use", Flypal.Util.ErrorType.NoError, mClassification.ID, EventLogID)
                            End If
                            NewRecordLeaveMaster()
                            DataFieldBindLeaveMaster()
                            lblTitleLeaveMaster.Text = "Classification Information [New]"
                            upnlLeaveMaster.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed by Vikrant on 22-July-2011
                                MarkLog(Flypal.Util.Action.Delete, "Classification", mClassification.Name, Flypal.Util.ErrorType.NoError, mClassification.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteLeaveMaster" Then
                        NewRecordLeaveMaster()
                        txtName.DataBind()
                        lblTitleLeaveMaster.Text = "Classification Information [New]"
                        upnlLeaveMaster.Update()
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
    Private Sub SetTitle()
        If mEmployeeLeave.IsNew Then
            lblTitle.Text = "Employee Leave Information [New]"
        Else
            If Len(mEmployeeLeave.ClassificationName) > 15 Then
                lblTitle.Text = "Employee Leave Information [" & mEmployeeLeave.ClassificationName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Leave Information [" & mEmployeeLeave.ClassificationName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub SetObject()
        mEmployeeLeave.EmployeeID = mEmployee.ID
        mEmployeeLeave.FromDate = CType(calFromDate.Text, Object)
        mEmployeeLeave.ToDate = CType(calToDate.Text, Object)
        mEmployeeLeave.NoOfDays = Trim(txtNoOfDays.Text)
        mEmployeeLeave.ClassificationID = New Guid(cmbClassificationList.SelectedValue)
        mEmployeeLeave.Note = Trim(txtNote.Text)
        mEmployeeLeave.ReJoiningDate = CType(calReJoiningDate.Text, Object)
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mEmployeeLeave.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub AttachMyFile()
        Try
            mEmployeeLeave.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeLeave.ImageSize = Session("FileUpload.FileSize")
            mEmployeeLeave.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeLeave") = mEmployeeLeave
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        'Dim custValidator As CustomValidator
        'custValidator = CType(s, CustomValidator)

        '''If custValidator.ControlToValidate = "cmbClassificationList" Then
        '''    If cmbClassificationList.SelectedIndex <= 0 Then
        '''        custValidator.ErrorMessage = "Please Select the Classification."
        '''        e.IsValid = False
        '''    Else
        '''        e.IsValid = True
        '''    End If
        '''Else
        ''If custValidator.ControlToValidate = "calFromDate" Then
        ''    If calFromDate.Value.ToString = "" Then
        ''        custValidator.ErrorMessage = "From Date should not be blank."
        ''        e.IsValid = False
        ''        ' ''ElseIf CDate(calToDate.Value.ToString) < CDate(calFromDate.Value.ToString) Then
        ''        ' ''    custValidator.ErrorMessage = "From Date should not greater than To Date."
        ''        ' ''    e.IsValid = False
        ''    End If
        ''Else
        'If custValidator.ControlToValidate = "calToDate" Then
        '    If calToDate.Text.ToString = "" Then
        '        custValidator.ErrorMessage = "To Date should not be blank."
        '        e.IsValid = False
        '        ' ''ElseIf CDate(calToDate.Value.ToString) < CDate(calFromDate.Value.ToString) Then
        '        ' ''    custValidator.ErrorMessage = "To Date should not less than From Date."
        '        ' ''    e.IsValid = False
        '    End If
        'ElseIf custValidator.ControlToValidate = "txtNote" Then
        '    If Len(txtNote.Text) > 500 Then
        '        custValidator.ErrorMessage = "Note cannot be greater than 500 characters."
        '        e.IsValid = False
        '    End If
        'ElseIf custValidator.ControlToValidate = "txtNoOfDays" Then
        '    If Val(txtNoOfDays.Text) = 0 Then
        '        custValidator.ErrorMessage = "No Of Days Required."
        '        e.IsValid = False
        '    End If
        'End If
    End Sub
    Private Sub NewRecordLeaveMaster()
        mClassification = Classification.NewClassification()
        Session("mClassification") = mClassification
    End Sub
    Private Sub EditRecordLeaveMaster(ByVal mID As Guid)
        mClassification = Classification.GetChildClassification(mID)
        Session("mClassification") = mClassification
        setFocus(txtName)
    End Sub
    Private Sub DeleteRecordLeaveMaster(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteLeaveMaster")
        mClassification = Classification.GetChildClassification(mID)
        Session("mClassification") = mClassification
    End Sub
    Private Sub SetObjectLeaveMaster()
        mClassification.Name = Trim(txtName.Text)
    End Sub
    Private Sub DataFieldBindLeaveMaster()
        mClassificationList = ClassificationList.GetClassificationList()
        dgClassification.DataSource = mClassificationList
        Session("mClassificationList") = mClassificationList
        upnlLeaveMaster.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        'calToDate.ShowClearButton = False
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            setFocus(cmbClassificationList)
            calFromDate.Text = Today.Date.ToString
            calToDate.Text = Today.Date.ToString
            txtNoOfDays.Text = "0"
            DataFieldBind()
            ControlVisibilityForAttachment()
            SetTitle()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeLeaveNew") And mEmployeeLeave.IsNew) Or (Not User.IsInRole("EmployeeLeaveEdit") And Not mEmployeeLeave.IsNew) Then
            SetObject()
            SetSession()
            'MarkLog(Flypal.Util.Action.Save, "EmployeeService", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
            MarkLog(Flypal.Util.Action.Save, "Employee Leave Records", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Leave Records : " + mEmployeeLeave.ClassificationName, Flypal.Util.ErrorType.HandledError, mEmployeeLeave.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                mEmployeeLeave.Save()
                SetSession()
                lblTitle.Text = "Employee Leave Information [New]"
                setFocus(cmbClassificationList)
                MarkLog(Flypal.Util.Action.Save, "Employee Leave Records", "Emp : " + mEmployee.EmpNoName + " Leave Records : " + mClassificationList(mEmployeeLeave.ClassificationID).Name, Flypal.Util.ErrorType.NoError, mEmployeeLeave.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
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
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgClassification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgClassification.Click
        SetObject() 'Added Code
        NewRecordLeaveMaster()
        DataFieldBindLeaveMaster()
        mdlPopUpLeaveMaster.Show()
        upnlLeaveMaster.Update()
        'Response.Redirect("wfClassification.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeLeaves.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        'Response.Redirect("wfEmployeeDetails.aspx")
        If Not mEmployeeLeave.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Leave Records", "Emp : " + mEmployee.EmpNoName + " Leave Records : " + mEmployeeLeave.ClassificationName, Flypal.Util.ErrorType.NoError, mEmployeeLeave.ID, EventLogID)
        End If
        'Added by Vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployeeLeave.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeLeave.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeLeave.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployeeLeave.ImageFile, 0, mEmployeeLeave.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mEmployeeLeave.ImageFile = file1
        mEmployeeLeave.ImageSize = 0
        mEmployeeLeave.FileExtension = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    Private Sub calToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calToDate.TextChanged
        '''txtNoOfDays.Text = DateDiff(DateInterval.Day, CDate(calFromDate.Value.ToString), CDate(calToDate.Value.ToString))
    End Sub
    Private Sub calFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calFromDate.TextChanged
        ' ''txtNoOfDays.Text = DateDiff(DateInterval.Day, CDate(calFromDate.Value.ToString), CDate(calToDate.Value.ToString))
        calToDate.Text = DateAdd(DateInterval.Day, Val(txtNoOfDays.Text) - 1, CDate(calFromDate.Text.ToString)).ToString
    End Sub
    Private Sub txtNoOfDays_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNoOfDays.TextChanged
        calToDate.Text = DateAdd(DateInterval.Day, Val(txtNoOfDays.Text) - 1, CDate(calFromDate.Text.ToString)).ToString
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlLeaveDetails.Update()
    End Sub
    
#End Region

#Region "Classification Master"
    Private Sub btnNewLeaveMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewLeaveMaster.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        NewRecordLeaveMaster()
        'Changed by Vikrant on 22-July-2011
        MarkLog(Flypal.Util.Action.[New], "Classification", "", Flypal.Util.ErrorType.NoError, mClassification.ID, EventLogID)
        DataFieldBindLeaveMaster()
        lblTitleLeaveMaster.Text = "Classification Information [New]"
    End Sub
    Private Sub dgClassification_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgClassification.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgClassification.PageIndex * dgClassification.PageSize
                mID = dgClassification.DataKeys(CInt(e.CommandArgument)).Value

                EditRecordLeaveMaster(mID)
                txtName.DataBind()
                'Changed by Vikrant on 22-July-2011
                MarkLog(Flypal.Util.Action.Edit, "Classification", mClassification.Name, Flypal.Util.ErrorType.NoError, mClassification.ID, EventLogID)
                If Len(mClassification.Name) > 15 Then
                    lblTitleLeaveMaster.Text = "Classification Information [" & mClassification.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleLeaveMaster.Text = "Classification Information [" & mClassification.Name & " ]"
                End If
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgClassification.PageIndex * dgClassification.PageSize
                mID = dgClassification.DataKeys(CInt(e.CommandArgument)).Value
                DeleteRecordLeaveMaster(mID)
        End Select
    End Sub
    Private Sub btnCloseLeaveMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseLeaveMaster.Click
        Session.Remove("mClassification")
        Session.Remove("mClassificationList")
        DataFieldBind()
        mdlPopUpLeaveMaster.Hide()
        upnlLeaveDetails.Update()
    End Sub
    Private Sub btnSaveLeaveMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveLeaveMaster.Click
        If (Not User.IsInRole("ClassificationNew") And mClassification.IsNew) Or (Not User.IsInRole("ClassificationEdit") And Not mClassification.IsNew) Then
            SetObjectLeaveMaster()
            Session("mClassification") = mClassification
        End If
        If IsValid Then
            SetObjectLeaveMaster()
            If Not mClassification.IsValid Then Exit Sub

            Try
                mClassification.Save()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                'Changed by Vikrant on 22-July-2011
                MarkLog(Flypal.Util.Action.Save, "Classification", mClassification.Name, Flypal.Util.ErrorType.HandledError, mClassification.ID, EventLogID)
                NewRecordLeaveMaster()
                DataFieldBindLeaveMaster()
                lblTitleLeaveMaster.Text = "Classification Information [New]"
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
#End Region

End Class
