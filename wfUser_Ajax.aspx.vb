'********************************************
'Modified by Harsh Sugandhi on 6th May 2025 for FLYPAL-2383 Employee Drop-down change in User Manager.
'********************************************


Imports System.CodeDom
Imports System.Text

Imports AjaxControlToolkit


Public Class wfUser_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mUser As User
    Public mUserID As Guid
    Public flag As Boolean = False
    'Added by Vikrant on 4-AUG-2011
    Dim EventLogID As Guid
    Dim ModuleName As String = "UserManager"  'Added By Utkarsh ON 19-Apr-2013 FOR ALL18042013-1
    Dim mErrorMessage As String
    Dim mErrorNo As Integer = 0
    Protected mEmployeeListAutoComplete As EmployeeListAutoComplete

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mUser = CType(Session("mUser"), User)
        mUserID = CType(Session("mUserID"), Guid)
        mEmployeeListAutoComplete = CType(Session("EmployeeListAutoComplete"), EmployeeListAutoComplete)

    End Sub

    Private Sub SetSession()

        Session("mUser") = mUser
        Session("mUserID") = mUserID
        Session("EmployeeListAutoComplete") = mEmployeeListAutoComplete

    End Sub

    Private Sub RemoveSession()
        Session.Remove("mUser")
        Session.Remove("mUserID")
        'Added By Utkarsh ON 19-Apr-2013 FOR ALL18042013-1
        Session.Remove("CreateCopy")
        Session.Remove("CopiedUserName")
        'End
    End Sub

    Private Function SetObject() As Boolean

        Try

            If mUser.IsNew Then

                mUser.Name = Trim(txtUserName.Text)
                mUser.Password = Trim(txtPassword.Text)
                mUser.ConfirmPassword = Trim(txtConPass.Text)
                mUser.StyleSheet = "Styles.css"

            End If

            mUser.ChangePassword = chkLogon.Checked
            mUser.IsAccessOutSideLAN = chkNetAccess.Checked 'Added by Kalpesh Shah

            'Added by Vikrant on 20-July-2012 For ALL11072012
            If AppSettings("PasswordSettings") = "True" Then
                mUser.StartDate = Today.Date.ToShortDateString
                mUser.ExpiryPeriod = IIf(txtExpiryPeriod.Text <> "", Val(txtExpiryPeriod.Text), 0)
            End If
            'End

            'Added by Vikrant on 31-Dec-2012 For ALL31122012
            mUser.UserEmail = Trim(txtUserEmail.Text)
            mUser.ManagerEmail = Trim(txtManagerEmail.Text)
            'End

            mUser.IsCurrencywisePOLimit = chkIsCurrencywisePOLimit.Checked
            mUser.EmployeeID = IIf(cmbEmployeeList.SelectedIndex > 0, New Guid(cmbEmployeeList.SelectedValue.ToString), Guid.Empty)

            Dim j As Integer = 0
            While j < mUser.UserRoles.Count
                Dim item As GridViewRow
                item = dgUser.Rows(j)
                mUser.UserRoles.Item(j).IsSelected = CType(item.FindControl("CheckBox1"), CheckBox).Checked
                j = j + 1
            End While

            Dim i As Integer = 0
            While i < mUser.UserMachines.Count
                Dim item As GridViewRow
                item = dgMachine.Rows(i)
                mUser.UserMachines.Item(i).IsSelected = CType(item.FindControl("chkSelect"), CheckBox).Checked
                i = i + 1
            End While

            Dim k As Integer = 0
            While k < mUser.UserEmployeeDepartments.Count
                Dim item As GridViewRow
                item = dgDepartment.Rows(k)
                mUser.UserEmployeeDepartments.Item(k).IsSelected = CType(item.FindControl("chkDepartmentSelect"), CheckBox).Checked
                k = k + 1
            End While

            Dim l As Integer = 0
            While l < mUser.UserCurrencywisePOLimits.Count
                Dim item As GridViewRow
                item = dgCurrency.Rows(l)
                mUser.UserCurrencywisePOLimits.Item(l).IsSelected = CType(item.FindControl("chkCurrencySelect"), CheckBox).Checked
                mUser.UserCurrencywisePOLimits.Item(l).Limit = CDec(Val(CType(item.FindControl("txtLimit"), TextBox).Text))
                mUser.UserCurrencywisePOLimits.Item(l).IsApplicable = CType(item.FindControl("chkCurrencySelect"), CheckBox).Checked
                l = l + 1
            End While

            Dim m As Integer = 0
            While m < mUser.UserStores.Count
                Dim item As GridViewRow
                item = dgStore.Rows(m)
                mUser.UserStores.Item(m).IsSelected = CType(item.FindControl("chkStoreSelect"), CheckBox).Checked
                m = m + 1
            End While

            Return True

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Public Sub NewRecord() 'Added By Prashant 29/12/2007
        Dim j As Integer = 0
        While j < mUser.UserRoles.Count
            Dim item As GridViewRow
            item = dgUser.Rows(j)
            mUser.UserRoles.Item(j).IsSelected = CType(item.FindControl("CheckBox1"), CheckBox).Checked
            CType(item.FindControl("CheckBox1"), CheckBox).Checked = False
            j = j + 1
        End While

        Dim k As Integer = 0
        While k < mUser.UserEmployeeDepartments.Count
            Dim item As GridViewRow
            item = dgDepartment.Rows(k)
            mUser.UserEmployeeDepartments.Item(k).IsSelected = CType(item.FindControl("chkDepartmentSelect"), CheckBox).Checked
            k = k + 1
        End While

        Dim l As Integer = 0
        While l < mUser.UserCurrencywisePOLimits.Count
            Dim item As GridViewRow
            item = dgCurrency.Rows(l)
            CType(item.FindControl("chkCurrencySelect"), CheckBox).Checked = False
            CType(item.FindControl("txtLimit"), TextBox).Text = 0
            CType(item.FindControl("chkCurrencySelect"), CheckBox).Checked = False
            l = l + 1
        End While

        Dim m As Integer = 0
        While m < mUser.UserStores.Count
            Dim item As GridViewRow
            item = dgStore.Rows(m)
            mUser.UserStores.Item(m).IsSelected = CType(item.FindControl("chkStoreSelect"), CheckBox).Checked
            m = m + 1
        End While

        txtUserName.Text = ""
        chkLogon.Checked = False
        chkIsCurrencywisePOLimit.Checked = False
        'Added by Vikrant on 20-July-2012 For ALL11072012
        If AppSettings("PasswordSettings") = "True" Then
            txtExpiryPeriod.Text = ""
        End If
        'End

        'Added by Vikrant on 31-Dec-2012 For ALL31122012
        txtUserEmail.Text = ""
        txtManagerEmail.Text = ""
        'End
        cmbEmployeeList.SelectedIndex = 0
        mUser = SI.UTILITY.User.NewUser()
        Session("mUser") = mUser
        If mUser.IsNew Then
            txtUserName.Enabled = True
            txtPassword.Enabled = True
            txtConPass.Enabled = True
            btnSave.Enabled = True
            txtPassword.TextMode = TextBoxMode.Password
            txtConPass.TextMode = TextBoxMode.Password
            lbltitle.Text = "User Information [New] "
            'Added by Vikrant on 20-July-2012 For ALL11072012
            If AppSettings("PasswordSettings") = "True" Then
                txtExpiryPeriod.Visible = True
                lblExpiryPeriod.Visible = True
                Label1.Visible = True
                txtExpiryPeriod.Enabled = True
            End If
        End If
    End Sub

    Public Sub UncheckCurrencyList()
        chkIsCurrencywisePOLimit.Checked = False
        Dim l As Integer = 0
        While l < mUser.UserCurrencywisePOLimits.Count
            Dim item As GridViewRow
            item = dgCurrency.Rows(l)
            CType(item.FindControl("chkCurrencySelect"), CheckBox).Checked = False
            CType(item.FindControl("txtLimit"), TextBox).Text = 0
            CType(item.FindControl("chkCurrencySelect"), CheckBox).Enabled = False
            CType(item.FindControl("txtLimit"), TextBox).Enabled = False
            l = l + 1
        End While
    End Sub

    Public Sub UnableDisableCurrencyList()
        Dim l As Integer = 0
        While l < mUser.UserCurrencywisePOLimits.Count
            Dim item As GridViewRow
            item = dgCurrency.Rows(l)
            If chkIsCurrencywisePOLimit.Checked = True Then
                CType(item.FindControl("chkCurrencySelect"), CheckBox).Enabled = True
                CType(item.FindControl("txtLimit"), TextBox).Enabled = True
            Else
                CType(item.FindControl("chkCurrencySelect"), CheckBox).Checked = False
                CType(item.FindControl("chkCurrencySelect"), CheckBox).Enabled = False
                CType(item.FindControl("txtLimit"), TextBox).Text = 0
                CType(item.FindControl("txtLimit"), TextBox).Enabled = False
            End If
            l = l + 1
        End While
        'upnldgCurrency.Update()
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

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mUser = CType(Session("mUser"), User)
                            Response.Redirect("wfUser.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'Added By Vikrant On 26-Feb-2021 for Heligo01032021
                    If MSGBoxCtrl.Sender = "ResetEmployee" Then
                        cmbEmployeeList.ClearSelection()
                        upnlUser.Update()
                    End If
                    'End
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"     'Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
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
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Public Sub TextChanged(sender As Object, e As EventArgs)

        Dim txtValue As TextBox
        Dim mUserCurrencyWisePOLimit As UserCurrencywisePOLimit
        Dim i As Integer = 0

        For Each mUserCurrencyWisePOLimit In mUser.UserCurrencywisePOLimits

            With mUserCurrencyWisePOLimit

                Try
                    txtValue = CType(Me.dgCurrency.Rows(i).FindControl("txtLimit"), TextBox)
                    txtValue.Attributes.Add("onKeyPress",
                                            "validateText(('D'),document.getElementById('" + txtValue.ClientID + "').value,event)")

                Catch ex As Exception
                    Throw ex.GetBaseException
                End Try

            End With

            i = i + 1

        Next

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        Try

            txtUserName.Text = mUser.Name
            txtPassword.Text = mUser.Password
            txtConPass.Text = mUser.ConfirmPassword
            txtExpiryPeriod.Text = mUser.ExpiryPeriod
            txtUserEmail.Text = mUser.UserEmail
            txtManagerEmail.Text = mUser.ManagerEmail
            chkLogon.Checked = mUser.ChangePassword
            chkNetAccess.Checked = mUser.IsAccessOutSideLAN
            chkIsCurrencywisePOLimit.Checked = mUser.IsCurrencywisePOLimit

            mEmployeeListAutoComplete = EmployeeListAutoComplete.GetEmployeeList(AddTopItem:="(SELECT)",
                                                                                 IsWorkingEmployeesOnly:=0)

            Session("EmployeeListAutoComplete") = mEmployeeListAutoComplete

            cmbEmployeeList.DataSource = mEmployeeListAutoComplete
            cmbEmployeeList.DataBind()

            dgUser.DataSource = mUser.UserRoles
            dgUser.DataBind()

            dgMachine.DataSource = mUser.UserMachines
            dgMachine.DataBind()

            dgDepartment.DataSource = mUser.UserEmployeeDepartments
            dgDepartment.DataBind()

            dgCurrency.DataSource = mUser.UserCurrencywisePOLimits
            dgCurrency.DataBind()

            dgStore.DataSource = mUser.UserStores 'Added By Prashant 31-Oct-2018 ALL30102018
            dgStore.DataBind()

            If mUser.UserMachines.Count > 0 Then
                lblAircraftList.Text = "List of Aircrafts : " & mUser.UserMachines.Count & " Record(s) found."
            End If

            If mUser.UserEmployeeDepartments.Count > 0 Then
                lblListofDepartment.Text = "List of Department : " & mUser.UserEmployeeDepartments.Count & " Record(s) found."
            End If

            If mUser.UserStores.Count > 0 Then
                lblListofStore.Text = "List of Store : " & mUser.UserStores.Count & " Record(s) found."
            End If

            upnlUser.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Function CheckChecked() As Boolean

        SetObject()

        Dim j As Integer = 0
        While j < mUser.UserRoles.Count

            If mUser.UserRoles.Item(j).IsSelected = True Then
                Return True
                Exit Function
            Else
                j = j + 1
            End If

        End While

        Return False

    End Function

    '---Added by Prashant 6-Sep-2013 ALL06092013---------------------------------
    Public Function CheckForAdmin() As Boolean
        If mUser.Name.ToLower = "admin" Then
            SetObject()
            Dim j As Integer = 0
            While j < mUser.UserRoles.Count
                If (mUser.UserRoles.Item(j).RoleName = "Administrator" And mUser.UserRoles.Item(j).IsSelected = False) Then
                    Return False
                    Exit Function
                Else
                    j = j + 1
                End If
            End While
        End If
        Return True
    End Function
    '--End----------------------------------

    Public Function CheckChecked1() As Boolean

        SetObject()

        Dim k As Integer = 0
        While k < mUser.UserEmployeeDepartments.Count
            If mUser.UserEmployeeDepartments.Item(k).IsSelected = True Then
                Return True
                Exit Function
            Else
                k = k + 1
            End If

        End While

        Return False
    End Function

    Public Function CheckCurrencyChecked() As Boolean
        SetObject()
        Dim l As Integer = 0
        While l < mUser.UserCurrencywisePOLimits.Count
            If (mUser.UserCurrencywisePOLimits.Item(l).IsSelected = False And mUser.UserCurrencywisePOLimits.Item(l).Limit > 0) Then
                mErrorMessage = "You have not selected currency, but entered limit > 0. "
                mErrorNo = 2
                Return False
                Exit Function
            Else
                l = l + 1
            End If
        End While
        Return True
    End Function

    Public Function CurrencyChecked() As Boolean
        SetObject()
        Dim l As Integer = 0
        While l < mUser.UserCurrencywisePOLimits.Count
            If mUser.UserCurrencywisePOLimits.Item(l).IsSelected = True Then
                Return True
                Exit Function
            Else
                l = l + 1
            End If
        End While
        Return False
    End Function

    Public Function CheckCheckedStore() As Boolean
        SetObject()
        Dim m As Integer = 0
        While m < mUser.UserStores.Count
            If mUser.UserStores.Item(m).IsSelected = True Then
                Return True
                Exit Function
            Else
                m = m + 1
            End If
        End While
        Return False
    End Function

    Public Sub Customvalidate(s As Object, e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "txtPassword" Then
            If Len(Trim(txtPassword.Text)) < 4 Or Len(Trim(txtPassword.Text)) > 15 Then
                CustValid.ErrorMessage = "Minimum Password should be of 4 characters and Maximum 15 characters."
                e.IsValid = False
            Else
                e.IsValid = True

                'Added By VIkrant On 10-Jan-2013 For ALL10012013-1
                If Not CheckChecked1() Then
                    CustValid.ErrorMessage = "Select at least one department for the User."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
                'End

                If CheckCurrencyChecked() = True Then
                    e.IsValid = True
                    'ElseIf CurrencyChecked() = True Then
                    '    e.IsValid = True
                Else
                    If mErrorNo = 1 Then
                        CustValid.ErrorMessage = mErrorMessage
                        e.IsValid = False
                    ElseIf mErrorNo = 2 Then
                        CustValid.ErrorMessage = mErrorMessage
                        e.IsValid = False
                    ElseIf mErrorNo = 3 Then
                        CustValid.ErrorMessage = mErrorMessage
                        e.IsValid = False
                        'ElseIf mErrorNo = 4 Then
                        '    CustValid.ErrorMessage = "You have set currencywise PO limit, but not selected currencies. "
                        '    e.IsValid = False
                    End If
                End If
            End If
        End If
        If CustValid.ControlToValidate = "txtConPass" Then
            If txtPassword.Text <> txtConPass.Text Then
                CustValid.ErrorMessage = "Confirm Password should be same as the Password."
                e.IsValid = False
            Else
                e.IsValid = True

                If flag = False Then
                    If Not CheckChecked() Then
                        CustValid.ErrorMessage = "Select at least one Role for the User."
                        e.IsValid = False
                    ElseIf Not CheckForAdmin() Then '''Added by Prashant 6-Sep-2013 ALL06092013
                        CustValid.ErrorMessage = "Select Administrator Role for the admin user."
                        e.IsValid = False
                    ElseIf chkIsCurrencywisePOLimit.Checked = True And CurrencyChecked() = False Then
                        CustValid.ErrorMessage = "You have set currency wise PO limit, but not selected currencies."
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                End If
                flag = True
            End If
        End If
        'Added by Vikrant on 20-July-2012 For ALL11072012
        If AppSettings("PasswordSettings") = "True" Then
            If CustValid.ControlToValidate = "txtExpiryPeriod" Then
                Dim No As Integer
                No = IIf(txtExpiryPeriod.Text <> "", Val(txtExpiryPeriod.Text), 0)
                If No <= 0 Or No > 365 Then
                    CustValid.ErrorMessage = "Expiry Period must be between 1 and 365 days."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        End If
        'End
    End Sub

    'Added by Vikrant on 20-July-2012 For ALL11072012
    Private Sub AddAttributes()
        txtExpiryPeriod.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtExpiryPeriod').value,event)")
    End Sub

    'Added By Utkarsh ON 25-Apr-2013 FOR All-25042013
    Private Sub EnableDisableButton()
        btnResetPassword.Enabled = IIf(mUser.IsNew, False, IIf(mUser.Name.ToUpper() = "BTPLADMIN", False, True)) 'changed by vikrant on 20-Sep-2019 For ALL20062019
        chkLogon.Enabled = IIf(mUser.Name.ToUpper() = "BTPLADMIN", False, True) 'Added by vikrant on 20-Sep-2019 For ALL20062019
    End Sub
    'End

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        AddAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Vikrant on 4-AUG-2011

        Try

            If mUser.IsNew Then

                txtUserName.Enabled = True
                txtPassword.Enabled = True
                txtConPass.Enabled = True
                btnSave.Enabled = True
                btnPrint.Enabled = False
                btnPrintbottom.Enabled = False

                'Added by Vikrant on 20-July-2012 For ALL11072012
                If AppSettings("PasswordSettings") = "True" Then

                    txtExpiryPeriod.Visible = True
                    lblExpiryPeriod.Visible = True
                    Label1.Visible = True
                    txtExpiryPeriod.Enabled = True

                End If
                'End

            Else

                txtUserName.Enabled = False
                txtPassword.Enabled = False
                txtConPass.Enabled = False
                btnSave.Enabled = True
                btnPrint.Enabled = True
                btnPrintbottom.Enabled = True

                'Added by Vikrant on 20-July-2012 For ALL11072012
                If AppSettings("PasswordSettings") = "True" Then

                    txtExpiryPeriod.Visible = True
                    lblExpiryPeriod.Visible = True
                    Label1.Visible = True
                    txtExpiryPeriod.Enabled = False

                End If
                'End

            End If

            If mUser.IsNew Then
                txtPassword.TextMode = TextBoxMode.Password
                txtConPass.TextMode = TextBoxMode.Password
            End If

            If Not IsPostBack And CType(Session("sender"), String) = "" Then

                If txtUserName.Enabled = True Then
                    SetFocus(txtUserName)
                End If

                DataFieldBind()

            End If

            If mUser.IsNew Then

                'Added By Utkarsh ON 19-Apr-2013 FOR ALL18042013-1
                If Session("CreateCopy") = "True" Then
                    lbltitle.Text = "User Information [ New as " + Session("CopiedUserName") + " ] "
                Else 'End
                    lbltitle.Text = "User Information [New]"
                End If

            Else
                lbltitle.Text = "User Information [ " & mUser.Name & " ]"
            End If

            btnNewUser.Enabled = AllowNewAircraft()
            EnableDisableButton() 'Added By Utkarsh ON 25-Apr-2013 FOR All-25042013            
            TextChanged(sender, e)
            upnlUser.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click, btnCloseTop.Click

        MarkLog(Action.Close,
                ModuleName,
                "",
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)

        RemoveSession()

        Response.Redirect(Request.QueryString("BackPage"))

    End Sub

    Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click, btnSaveTop.Click

        Try

            If Not User.IsInRole("UserManagerView") Then 'Added by Saylee on 1-Feb-2013 for ALL01022013

                MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                MSGBox.Message_text.Authorization,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            End If

            If IsValid Then

                SetObject()

                Session("mUser") = mUser
                mUser.Save()

                'Added By Utkarsh ON 19-Apr-2013 FOR ALL18042013-1
                If Session("CreateCopy") = "True" Then

                    MarkLog(Action.Save,
                            ModuleName,
                            "Copy of User Name : " +
                                Session("CopiedUserName") +
                                ", New User Name : " +
                                mUser.Name +
                                ", Created By : " +
                                User.Identity.Name,
                            ErrorType.NoError,
                            mUser.UserID,
                            EventLogID)

                Else

                    MarkLog(Action.Save,
                            ModuleName,
                            "User Name : " + txtUserName.Text,
                            ErrorType.NoError,
                            mUser.UserID,
                            EventLogID)

                End If
                'End

                DataFieldBind()

                'Added By Vikrant on 27-July-2012 For ALL11072012
                txtUserName.Text = ""
                txtExpiryPeriod.Text = ""
                chkLogon.Checked = False
                'End

                'Added by Vikrant on 31-Dec-2012 For ALL31122012
                txtUserEmail.Text = ""
                txtManagerEmail.Text = ""
                'End

                txtPassword.Text = ""
                txtConPass.Text = ""
                cmbEmployeeList.SelectedIndex = 0
                UncheckCurrencyList()
                SetSession()

                btnNewUser.Enabled = AllowNewAircraft()
                btnSave.Enabled = AllowNewAircraft()
                btnSaveTop.Enabled = AllowNewAircraft() 'Added By Utkarsh ON 19-Apr-2013 FOR ALL18042013-1

                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
                                MSGBox.Message_text.SavedSuccessFully,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        Catch ex As SqlException

            If ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.Alert,
                                MSGBox.Message_text.Duplicate,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        End Try

    End Sub

    Private Sub CreateNewUser(sender As Object, e As EventArgs) Handles btnNewUser.Click

        MarkLog(Action.[New],
                ModuleName,
                "",
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)

        NewRecord()

    End Sub

    Private Sub GV_User_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgUser.RowCommand
        Dim index As Int32 = CInt(e.CommandArgument) + dgUser.PageIndex * dgUser.PageSize
        dgUser.DataSource = mUser.UserRoles
        dgUser.DataBind()
    End Sub

    'Added By Vikrant On 28-Feb-2013 For All28022013
    Protected Sub chkSelectAllAircraft_CheckChanged(Sender As Object, e As EventArgs)
        Dim chk As CheckBox
        For i As Integer = 0 To mUser.UserMachines.Count - 1
            chk = CType(Me.dgMachine.Rows(i).FindControl("chkSelect"), CheckBox)
            chk.Checked = CType(Sender, CheckBox).Checked
        Next
    End Sub
    'End

    'Added By Shital On 19-Jun-2020 For All19062020
    Protected Sub chkSelectAllDept_CheckChanged(Sender As Object, e As EventArgs)
        Dim chk As CheckBox
        For i As Integer = 0 To mUser.UserEmployeeDepartments.Count - 1
            chk = CType(Me.dgDepartment.Rows(i).FindControl("chkDepartmentSelect"), CheckBox)
            chk.Checked = CType(Sender, CheckBox).Checked
        Next
    End Sub

    Protected Sub chkSelectAllStore_CheckChanged(Sender As Object, e As EventArgs)
        Dim chk As CheckBox
        For i As Integer = 0 To mUser.UserStores.Count - 1
            chk = CType(Me.dgStore.Rows(i).FindControl("chkStoreSelect"), CheckBox)
            chk.Checked = CType(Sender, CheckBox).Checked
        Next
    End Sub
    'End

    'Added By Utkarsh ON 25-Apr-2013 FOR All-25042013
    Private Sub ResetPassword(sender As Object, e As EventArgs) Handles btnResetPassword.Click

        If IsValid Then

            Dim random As Random = New Random
            Dim NewPassword As String = ""
            NewPassword = random.Next(1111, 9999)

            If Not mUser.IsNew Then

                mUser.Password = NewPassword
                mUser.ConfirmPassword = NewPassword
                btnNewUser.Enabled = AllowNewAircraft()
                btnSave.Enabled = AllowNewAircraft()
                btnSaveTop.Enabled = AllowNewAircraft()

                Try

                    If mUser.IsValid Then

                        mUser.Save()
                        MarkLog(Action.Save, ModuleName, "Reset Password , User Name : " + txtUserName.Text + ", Reset By : " + User.Identity.Name, ErrorType.NoError, mUser.UserID, EventLogID)

                        Dim str As String = " alert('Your New Password is " & mUser.Password & "');"
                        ScriptManager.RegisterStartupScript(Me, [GetType], "Reset Password", str, True)

                    Else

                        Dim str As String = ""
                        For i As Integer = 0 To mUser.GetBrokenRulesCollection.Count - 1
                            str = str + mUser.GetBrokenRulesCollection(i).Description + "<BR>"
                        Next
                        If str <> "" Then
                            cvcp.ErrorMessage = str
                            cvcp.IsValid = False
                        End If

                    End If

                Catch ex As Exception
                    Throw ex.GetBaseException
                End Try

            End If

        End If

    End Sub
    'End

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    'Added by Shital on 20-Aug-2020
    Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click, btnPrintbottom.Click

        Dim da As New ObjectAdapter
        Dim myReport As Engine.ReportDocument
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsUserRoleWiseRights
        Dim ReportTitle As String = "User Role Wise Rights Report"

        Try

            Dim ReportData As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             ReportTitle,
                                             "",
                                             "",
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Logo"),)

            Dim RolIDs As New StringBuilder
            RolIDs.Append("<RoleIDs>")
            For i As Integer = 0 To mUser.UserRoles.Count - 1

                If mUser.UserRoles(i).IsSelected = True Then
                    RolIDs.Append("<ID>")
                    RolIDs.Append(mUser.UserRoles(i).RoleID.ToString)
                    RolIDs.Append("</ID>,")
                End If

            Next

            RolIDs.Append("</RoleIDs>")


            Dim mUserRoleWiseRights As UserRoleWiseRights
            mUserRoleWiseRights = UserRoleWiseRights.GetUserRoleWiseRights(mUser.UserID, RolIDs.ToString)

            myReport = New crUserRoleWiseRights
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, ReportData)
            da.Fill(ds, mUser)
            da.Fill(ds, mUser.UserRoles)
            da.Fill(ds, mUserRoleWiseRights)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "openTranDetail",
                                                "openTranDetail();",
                                                True)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    ''Added By Vikrant On 26-Feb-2021 Heligo 01032021
    Private Sub EmployeeSelected(sender As Object, e As EventArgs) Handles cmbEmployeeList.SelectedIndexChanged

        Dim EmployeeName As String = String.Empty
        Try

            EmployeeName = IIf(cmbEmployeeList.SelectedIndex = 0, "", cmbEmployeeList.SelectedItem.ToString)

            HighlightNonWorkingEmployee(sender:=sender)

            CheckEmployeeStatus(SelectedEmployeeIndex:=cmbEmployeeList.SelectedIndex,
                                SelectedEmployee:=cmbEmployeeList.SelectedValue,
                                EmployeeName:=EmployeeName)

            If mUser.IsNew Then

                Dim message As String = ""
                Dim mEmployeeList As EmployeeList = EmployeeList.GetEmployeeList()
                Dim mEmployeeStatus As EmployeeStatus

                If mEmployeeList.Contains(cmbEmployeeList.SelectedItem.ToString) Then

                    mEmployeeStatus =
                        EmployeeStatus.
                            GetEmployeeWorkingStatus(mEmployeeList(cmbEmployeeList.SelectedItem.ToString, "").ID.ToString,
                                                     Today.Date.ToString)

                    If mEmployeeStatus.Count > 0 Then

                        If (mEmployeeStatus(0).Information <> "") Then

                            message = mEmployeeStatus(0).Information

                            MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert,
                                            MSGBox.Message_text.Custom,
                                            message,
                                            MsgBoxStyle.OkOnly,
                                            "ResetEmployee")

                            Exit Sub

                        End If

                    End If

                End If

                upnlEmp.Update()
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub
    'End

    Private Sub EmployeeListDataBound(sender As Object, e As EventArgs) Handles cmbEmployeeList.DataBound

        Try

            HighlightNonWorkingEmployee(sender:=sender)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Function HighlightNonWorkingEmployee(sender As Object)

        Try

            Dim _DropDownList As DropDownList = CType(sender, DropDownList)
            mEmployeeListAutoComplete = CType(Session("EmployeeListAutoComplete"), EmployeeListAutoComplete)

            If sender IsNot Nothing Then

                For Each Item As ListItem In _DropDownList.Items

                    If Not Item.Value = (Guid.Empty.ToString) AndAlso
                       Not mEmployeeListAutoComplete(New Guid(Item.Value)).IsEmployeeWorking Then

                        Item.Attributes.Add("style", "background-color: yellow;")

                    End If

                Next

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

    Public Function CheckEmployeeStatus(SelectedEmployeeIndex As Integer,
                                        SelectedEmployee As String,
                                        EmployeeName As String)

        mEmployeeListAutoComplete = CType(Session("EmployeeListAutoComplete"), EmployeeListAutoComplete)
        Try

            If Not SelectedEmployeeIndex = 0 AndAlso
               Not mEmployeeListAutoComplete(New Guid(SelectedEmployee)).IsEmployeeWorking Then

                MSGBoxCtrl.Show("Alert..!!!",
                                $" {EmployeeName} is not working with the Organization.",
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

                cmbEmployeeList.SelectedIndex = 0

            End If


        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Function

#End Region

End Class