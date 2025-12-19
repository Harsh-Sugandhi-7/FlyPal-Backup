'********************************************
'Modified by Harsh Sugandhi on 6th May 2025 for FLYPAL-2383 Employee Drop-down change in User Manager.
'********************************************


Public Class wfUserList_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mUser As User
    Public mUserList As UserList
    Public BackPage As String
    Dim Idx As Int32
    Dim UserName As String
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid
    Public mRoleList As RoleList 'Added By Vikrant On 28-Dec-2012 For ALL28122012
    Dim ModuleName As String = "UserManager"  'Added By Utkarsh ON 19-Apr-2013 FOR ALL18042013-1

#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mUserList = CType(Session("mUserList"), UserList)
        mUser = CType(Session("mUser"), User)
        UserName = Session("UserName")
        mRoleList = Session("mRoleList") 'Added By Vikrant On 28-Dec-2012 For ALL28122012
    End Sub

    Private Sub SetSession()
        Session("mUserList") = mUserList
        Session("mUser") = mUser
        Session("UserName") = UserName
        Session("mRoleList") = mRoleList 'Added By Vikrant On 28-Dec-2012 For ALL28122012
    End Sub

    Private Overloads Sub SetFocus(Control As WebControl)

        Dim script As String
        If Control.Enabled = False Or Control.Visible = False Then Exit Sub

        Try

            script = "document.getElementById('" + Control.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "focusscript",
                                                script,
                                                True)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub NewRecord()
        mUser = SI.UTILITY.User.NewUser()
        Session("mUser") = mUser
    End Sub

    'Added Code
    Private Sub CreateCopy(mID As Guid)

        Dim tempUser As User = SI.UTILITY.User.GetUser(mID)
        mUser = SI.UTILITY.User.NewUser()
        mUser.ExpiryPeriod = tempUser.ExpiryPeriod

        'User Roles
        For i As Integer = 0 To tempUser.UserRoles.Count - 1
            mUser.UserRoles(i).RoleID = tempUser.UserRoles(i).RoleID
            mUser.UserRoles(i).RoleName = tempUser.UserRoles(i).RoleName
            mUser.UserRoles(i).IsSelected = tempUser.UserRoles(i).IsSelected
        Next

        'User Machines
        For i As Integer = 0 To tempUser.UserMachines.Count - 1
            mUser.UserMachines(i).MachineID = tempUser.UserMachines(i).MachineID
            mUser.UserMachines(i).RegNo = tempUser.UserMachines(i).RegNo
            mUser.UserMachines(i).IsSelected = tempUser.UserMachines(i).IsSelected
        Next

        'User Departments
        For i As Integer = 0 To tempUser.UserEmployeeDepartments.Count - 1
            mUser.UserEmployeeDepartments(i).EmployeeDepartmentID = tempUser.UserEmployeeDepartments(i).EmployeeDepartmentID
            mUser.UserEmployeeDepartments(i).EmployeeDepartmentName = tempUser.UserEmployeeDepartments(i).EmployeeDepartmentName
            mUser.UserEmployeeDepartments(i).IsSelected = tempUser.UserEmployeeDepartments(i).IsSelected
        Next

        Session("CreateCopy") = True
        Session("CopiedUserName") = tempUser.Name
        tempUser = Nothing
        Session("mUser") = mUser

        MarkLog(Action.[New],
                ModuleName,
                "By Create Copy",
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)
        'End

    End Sub
    'End of Added Code

    Private Sub EditRecord(mID As Guid)
        mUser = SI.UTILITY.User.GetUser(mID)
        Session("mUser") = mUser
    End Sub

    Private Sub DeleteRecord(mID As Guid, Optional Name As String = "")

        MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                        MSGBox.Message_text.Delete,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

        mUser = SI.UTILITY.User.GetUser(mID)

        Session("mUser") = mUser

    End Sub

    Private Sub FindNow(Optional Name As String = "",
                        Optional RoleID As String = "{00000000-0000-0000-0000-000000000000}")

        mUser = Nothing
        dgUser.DataSource = Nothing
        dgUser.DataBind()

        'Get List From the Database as per Criteria  
        mUserList = UserList.GetUserList(Name,
                                         RoleID,
                                         HttpContext.Current.User.Identity.Name)
        Session("mUserList") = mUserList

        'Set DataSource of the Grid
        dgUser.DataSource = mUserList
        dgUser.DataBind()

    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult

        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Session("sender") = ""
                            mUser = Session("mUser")
                            SI.UTILITY.User.DeleteUser(mUser.UserID)

                            'Added by Vikrant on 4-AUG-2011
                            If mUser.Name.ToLower <> "admin" Then

                                MarkLog(Action.Delete,
                                        ModuleName,
                                        "User : " + mUser.Name,
                                        ErrorType.NoError,
                                        mUserList.Item(mUserList.CurrentIndex).UserID,
                                        EventLogID)

                            End If

                            DataFieldBind()

                        Catch ex As SqlException

                            Dim stringInfo As String = ""

                            If ex.Message.Contains("tabDocumentLockerUser") Then
                                stringInfo = "Document Locker User"
                            Else
                                stringInfo = ""
                            End If

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then


                                MarkLog(Action.Delete,
                                        ModuleName,
                                        "Can't Delete : " & mUserList.Item(mUserList.CurrentIndex).Name & " is currently in use",
                                        ErrorType.NoError,
                                        mUserList.Item(mUserList.CurrentIndex).UserID,
                                        EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting,
                                                MSGBox.Message_text.ReferenceDeleting,
                                                stringInfo,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            DataFieldBind()

                        End Try

                    End If

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
            End Select

        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If

    End Sub

    Public Sub SetControl()
        UserName = Session("UserName")
        txtSearch.Text = UserName
        FindNow(UserName)
        lblResult.Text = "List of Users as per criteria: " & mUserList.Count & " Record(s) found."
    End Sub

    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUserList_Ajax.aspx" Then
            Session.Remove("mUser")
            Session.Remove("UserName")
            Session.Remove("mUserList")
            Session.Remove("mRoleList") 'Added By Vikrant On 28-Dec-2012 For ALL28122012
        End If
    End Sub

    Private Function AllowNewAircraft() As Boolean
        Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
        Dim tmpUserList As UserList = UserList.GetUserList()
        If mCheck.Number("User") > 0 And mCheck.Number("User") <> -1 Then 'Added by Saylee on 17-Jan-2011 to chack no of User rights
            If tmpUserList.Count >= mCheck.Number("User") And mCheck.Number("User") <> -1 Then
                Return False
            Else
                Return True
            End If
        Else
            If tmpUserList.Count >= mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
                'MessageBox.Show("This version does not supports more than " & mCheck.Number("Aircraft").ToString & " Aircrafts", "Version 1.0", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                Return True
            End If
        End If
    End Function

#End Region

#Region " DataBinding "

    Public Sub DataFieldBind()

        mUserList = UserList.GetUserList(, , HttpContext.Current.User.Identity.Name)
        Session("mUserList") = mUserList

        dgUser.DataSource = mUserList
        dgUser.DataBind()

        lblResult.Text = "List of Users as per criteria: " & mUserList.Count & " Record(s) found."

        mRoleList = RoleList.GetRoleList("", "All")
        Session("mRoleList") = mRoleList

        cmbRoleList.DataSource = mRoleList
        cmbRoleList.DataBind()

        UserName = Session("UserName")

        upnlUserList.Update()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011

        If Not IsPostBack And Session("Sender") = "" Then
            If txtSearch.Enabled = True Then
                SetFocus(txtSearch)
            End If
            Session("MiddleFrame") = "wfUserList_Ajax.aspx"
            DataFieldBind()
            SetControl()
        End If
        'set the label

        lblResult.Text = "List of Users as per criteria: " & mUserList.Count & " Record(s) found."

        SetSession()

        Dim IsNewUserAllow As Boolean = False
        IsNewUserAllow = AllowNewAircraft()

        btnAdd.Enabled = IsNewUserAllow

        If IsNewUserAllow = False Then
            dgUser.Columns(3).Visible = False
        Else
            dgUser.Columns(3).Visible = True
        End If
    End Sub

    Private Sub SearchRecord(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click

        UserName = txtSearch.Text
        Session("UserName") = UserName
        FindNow(Trim(txtSearch.Text), cmbRoleList.SelectedValue.ToString)
        dgUser.DataBind()
        lblResult.Text = "List of Users as per criteria: " & mUserList.Count & " Record(s) found."

    End Sub

    Private Sub GV_User_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgUser.RowCommand

        Idx = CInt(e.CommandArgument) + dgUser.PageIndex * dgUser.PageSize
        Dim mId As Guid = mUserList.Item(Idx).UserID
        Dim mUName As String = mUserList.Item(Idx).Name

        If Not User.IsInRole("UserManagerView") Then 'Added by Saylee on 1-Feb-2013 for ALL01022013

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "")
            Exit Sub

        End If

        Select Case e.CommandName
            Case "EditView"

                EditRecord(mId)
                'Added by Vikrant on 4-AUG-2011
                MarkLog(Action.Edit,
                        ModuleName,
                        "User : " + mUName,
                        ErrorType.NoError,
                        mUserList.Item(mUserList.CurrentIndex).UserID,
                        EventLogID)

                Dim Str As String
                Str = "openledgersame('wfUser_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", Str, True)

            Case "Remove"

                If mUName.ToUpper = "BTPLADMIN" Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert,
                                    MSGBox.Message_text.DeleteAlert,
                                    "You can not delete this entry",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Exit Sub

                Else
                    DeleteRecord(mId, mUName)
                End If

                'Code Added 
            Case "CreateCopy"

                If mUName.ToUpper = "BTPLADMIN" Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Alert,
                                    MSGBox.Message_text.Alert,
                                    "You can not create copy of this user",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Exit Sub

                Else
                    CreateCopy(mId)
                End If

                Dim Str As String
                Str = "openledgersame('wfUser_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", Str, True)

        End Select

    End Sub

    Private Sub GV_MachineMaintenanceList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgUser.PageIndexChanging
        dgUser.PageIndex = e.NewPageIndex

        DataFieldBind()
    End Sub

    Private Sub AddUser(sender As Object, e As EventArgs) Handles btnAdd.Click

        If Not User.IsInRole("UserManagerView") Then 'Added by Saylee on 1-Feb-2013 for ALL01022013

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "")

            Exit Sub

        End If

        MarkLog(Action.[New],
                ModuleName,
                "",
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)

        NewRecord()

        Dim Str As String
        Str = "openledgersame('wfUser_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "OpenScript",
                                            Str,
                                            True)

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click ', btnTopClose.Click

        MarkLog(Action.Close,
                ModuleName,
                "",
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)

        Session("MiddleFrame") = ""
        Session.Remove("mUser")
        Session.Remove("UserName")

        mUserList = Nothing

        Response.Redirect("DashBoard.aspx")

    End Sub

    Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrintBottom.Click

        Dim da As New ObjectAdapter
        Dim myReport As Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsUserList
        Dim mrptUserList As rptUserList

        mrptUserList = rptUserList.GetrptUserList(txtSearch.Text.ToString)
        Session("mrptUserList") = "mrptUserList"

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                     mCompanyDetail.Address,
                                     mCompanyDetail.Tel1,
                                     mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax,
                                     mCompanyDetail.Email,
                                     mCompanyDetail.WebSite,
                                     "User List Report",
                                     "",
                                     "",
                                     "",
                                     "",
                                     "",
                                     AppSettings("Product Version"),
                                     AppSettings("SINote"))

        myReport = New crUserList

        da.Fill(ds, mrptUserList)
        da.Fill(ds, Report)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "openTranDetail",
                                            Str,
                                            True)


    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

#End Region

End Class