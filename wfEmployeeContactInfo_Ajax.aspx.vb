'AJAX COnversion  By Vikrant

Partial Class wfEmployeeContactInfo_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeContactInfo As EmployeeContactInfo

    Public mCityInvList As CityInvList

    Public BackPage As String

    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
#End Region

#Region " Helper Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeContactInfo = Session("mEmployeeContactInfo")
        mCityInvList = Session("mCityInvList")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeContactInfo") = mEmployeeContactInfo
        Session("mCityInvList") = mCityInvList
        Session("mEmployee") = mEmployee
    End Sub
    'Private Sub RemoveSession() 'Static CH
    '    Session.Remove("mEmployeeContactInfo")
    '    Session.Remove("mCityInvList")
    'End Sub
    Private Sub DataFieldBind()
        mCityInvList = CityInvList.GetCityList(0, , , True)
        cmbCityInvList.DataSource = mCityInvList
        Session("mCityInvList") = mCityInvList
        upnlContactInfo.DataBind()
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No

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
        If mEmployeeContactInfo.IsNew Then
            lblTitle.Text = "Employee Next To Kin Information [New]"
        Else
            If Len(mEmployeeContactInfo.Name) > 15 Then
                lblTitle.Text = "Employee Next To Kin Information [" & mEmployeeContactInfo.Name.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Next To Kin Information [" & mEmployeeContactInfo.Name & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mEmployeeContactInfo.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub SetObject()
        mEmployeeContactInfo.EmployeeID = mEmployee.ID
        mEmployeeContactInfo.Name = Trim(txtName.Text)
        mEmployeeContactInfo.Relation = Trim(txtRelation.Text)
        mEmployeeContactInfo.Address1 = Trim(txtAddress1.Text)
        mEmployeeContactInfo.Address2 = Trim(txtAddress2.Text)
        mEmployeeContactInfo.Address3 = Trim(txtAddress3.Text)
        mEmployeeContactInfo.CityID = New Guid(cmbCityInvList.SelectedValue)
        mEmployeeContactInfo.PhoneNo1 = Trim(txtPhone1.Text)
        mEmployeeContactInfo.PhoneNo2 = Trim(txtPhone2.Text)
        mEmployeeContactInfo.Mobile = Trim(txtMobile.Text)
        mEmployeeContactInfo.Email = Trim(txtEmail.Text)
    End Sub
    Private Sub AttachMyFile()
        Try
            mEmployeeContactInfo.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeContactInfo.ImageSize = Session("FileUpload.FileSize")
            mEmployeeContactInfo.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeContactInfo") = mEmployeeContactInfo
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                txtName.Focus()
            End If
            DataFieldBind()
            SetTitle()
            ControlVisibilityForAttachment()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeNextToKinInfoNew") And mEmployeeContactInfo.IsNew) Or (Not User.IsInRole("EmployeeNextToKinInfoEdit") And Not mEmployeeContactInfo.IsNew) Then
            SetObject()
            SetSession()
            'MarkLog(Flypal.Util.Action.Save, "EmployeeNextToKinInfo", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MarkLog(Flypal.Util.Action.Save, "Employee Next To Kin Info", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Next To Kin Info : " + mEmployeeContactInfo.Name, Flypal.Util.ErrorType.HandledError, mEmployeeContactInfo.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                mEmployeeContactInfo.Save()
                MarkLog(Flypal.Util.Action.Save, "Employee Next To Kin Info", "Emp : " + mEmployee.EmpNoName + " Next To Kin Info : " + mEmployeeContactInfo.Name, Flypal.Util.ErrorType.HandledError, mEmployeeContactInfo.ID, EventLogID)
                SetSession()
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
                End If
            End Try
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgCity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgCity.Click
        'SetObject() 'Added Code
        'Code to open CityInv_Ajax 'Static CH
        'Response.Redirect("wfCityInv.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage3=wfEmployeeContactInfo_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        If Not mEmployeeContactInfo.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Next To Kin Info", "Emp : " + mEmployee.EmpNoName + " Next To Kin Info : " + mEmployeeContactInfo.Name, Flypal.Util.ErrorType.NoError, mEmployeeContactInfo.ID, EventLogID)
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
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployeeContactInfo.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeContactInfo.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeContactInfo.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployeeContactInfo.ImageFile, 0, mEmployeeContactInfo.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
            End If
        End If
    End Sub
    Private Sub cmbCityInvList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCityInvList.SelectedIndexChanged
        txtState.Text = IIf(cmbCityInvList.SelectedIndex > 0, mCityInvList(cmbCityInvList.SelectedIndex).State, "")
        txtCountry.Text = IIf(cmbCityInvList.SelectedIndex > 0, mCityInvList(cmbCityInvList.SelectedIndex).Country, "")
        If cmbCityInvList.Enabled = True Then
            setFocus(cmbCityInvList)
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mEmployeeContactInfo.ImageFile = file1
        mEmployeeContactInfo.ImageSize = 0
        mEmployeeContactInfo.FileExtension = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlContactInfo.Update()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCity.Click
        DataFieldBind()
        upnlCity.Update()
    End Sub
#End Region

End Class
