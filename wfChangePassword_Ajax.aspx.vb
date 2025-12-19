Imports System.Text
Imports System.Security.Cryptography
Public Class wfChangePassword_Ajax
    Inherits System.Web.UI.Page

#Region "Enumeration"
    Enum RequestedBy
        Administrator = 1
        User = 2
    End Enum
#End Region

#Region " Variables and Declarations "
    Public mUserID As Guid
    '' Public mRequestedBy As RequestedBy
    Public mUser As User
    Public mIsRequestedForSave As Boolean
    Public mOldPassword As String
    Dim mRequestBy As RequestedBy
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUser = Session("mUser")
        mUserID = Session("mUserID")
        mOldPassword = Session("mOldPassword")
        mRequestBy = Session("mRequestBy")
    End Sub
    Private Sub SetSession()
        Session("mUser") = mUser
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No
                    '''''Session("sender") = ""
                    '''''Response.Redirect("wfChangePassword.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    ''Response.Redirect("Login.aspx?")
                    If Session("MsgFor") = "OldNotCorrect" Then
                        Session("MsgFor") = ""
                        '''''Response.Redirect("wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString)
                    Else
                        Session("MsgFor") = ""
                        BusinessPrincipal.login(mUser.Name, mUser.Password)
                        If Session("mRequestBy") = 1 Then
                            'Commented & Added by Vikrant on 24-July-2012 For ALL11072012
                            ''Response.Redirect("Login.aspx?BackPage=" & Request.QueryString("BackPage"))
                            Web.Security.FormsAuthentication.SignOut()
                            Session.Remove("MenuID")
                            Session.Remove("MiddleFrame")
                            MarkLog(Util.Action.Logoff)
                            'Drop all the references to the Principal.
                            Thread.CurrentPrincipal = Nothing
                            Dim str As String
                            '''''str = "<script language=javascript>  window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
                            '''''ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
                            str = "window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenPageScript", str, True)

                            Session.Remove("ReminderFired")
                            'end
                        Else
                            Response.Redirect("Dashboard.aspx?BackPage=" & Request.QueryString("BackPage"))
                        End If
                    End If
            End Select
        ElseIf Result1 = -1 Then
            'Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString)
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            'Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub DataFieldBind()
        mOldPassword = mUser.DBPassword
        Session("mOldPassword") = mOldPassword
        txtUserName.Text = mUser.Name
        If mRequestBy = RequestedBy.Administrator Then
            txtOldPassword.Text = mUser.Password
        Else
            txtOldPassword.Text = ""
        End If
        'txtOldPassword.Enabled = (mRequestBy = RequestedBy.User)
        DataBind()
    End Sub
    Private Function GetDbPassword(ByVal UserName As String, ByVal Password As String) As String
        Dim HashValue() As Byte
        Dim MessageString As String
        MessageString = Password & "$$" & LCase(UserName)
        'Create a new instance of UnicodeEncoding to 
        'convert the string into an array of Unicode bytes.
        Dim UE As New UnicodeEncoding
        'Convert the string into an array of bytes.
        Dim MessageBytes As Byte() = UE.GetBytes(MessageString)
        'Create a new instance of SHA1Managed to create 
        'the hash value.
        Dim SHhash As New SHA1Managed
        'Create the hash value from the array of bytes.
        HashValue = SHhash.ComputeHash(MessageBytes)
        Dim Str1 As String
        Str1 = ""
        Dim b As Byte
        For Each b In HashValue
            'If Label1.Text = "" Then
            Str1 = Str1 & Hex(b).ToString
        Next
        Return Str1
    End Function
    Private Sub SetObject()
        mUser.Name = txtUserName.Text.Trim
        mUser.Password = txtNewPassword.Text.Trim
        mUser.ConfirmPassword = txtConfrimPassword.Text.Trim
        Session("mUser") = mUser
    End Sub

    'Added by Vikrant on 16-July-2012 For ALL11072012
    Public Sub customValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If AppSettings("PasswordSettings") = "True" Then
            If custValidator.ControlToValidate = "txtNewPassword" Then
                If String.Compare(txtOldPassword.Text, txtNewPassword.Text, True) = 0 Then
                    custValidator.ErrorMessage = "Old Password and New Password are Same.Please Enter Another Password."
                    e.IsValid = False
                End If
            End If
        End If
    End Sub
    'End
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If txtOldPassword.Enabled = True Then
            setFocus(txtOldPassword)
        End If
        If Not IsPostBack And Session("sender") = "" Then
            mRequestBy = Request.QueryString("RequestedBy")
            Session("mRequestBy") = mRequestBy

            If mRequestBy = RequestedBy.User Then
                Dim mUserList As UserList = UserList.GetUserList(User.Identity.Name, , User.Identity.Name)
                mUserID = mUserList.Item(User.Identity.Name).UserID()
                Session("mUserID") = mUserID
                'txtOldPassword.Enabled = True
                mUser = SI.UTILITY.User.GetUser(mUserID)
            ElseIf mRequestBy = RequestedBy.Administrator Then
                mUserID = New Guid(Request.QueryString("UserID").ToString)
                Session("mUserID") = mUserID
                mUser = SI.UTILITY.User.GetUser(mUserID)
                'txtOldPassword.Enabled = False
                txtOldPassword.Text = mUser.Password
            End If
            Session("mUser") = mUser
            DataFieldBind()
        End If
        'MessageBoxResult()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            If mUser.IsDirty Or Len(txtOldPassword.Text) > 0 Then
                SetObject()
                If Not mIsRequestedForSave Then
                    If mUser.IsValid Then
                        'Verifing old Password
                        'If mRequestBy = RequestedBy.User Then 'Commented by Vikrant on 23-July-2012 For ALL11072012
                        If Not (mOldPassword = GetDbPassword(mUser.Name, txtOldPassword.Text)) Then
                            Session("MsgFor") = "OldNotCorrect"
                            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Password, SIMsgBox.Message_text.Password, "<Strong>Old Password is not correct! </Strong>", MsgBoxStyle.OKOnly)
                            'msg.ReplacePage = "wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString
                            'Response.Redirect("wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString)
                            'msg.Show()
                            MSGBoxCtrl.show(MSGBox.Message_title.Password, MSGBox.Message_text.Password, "<Strong>Old Password is not correct! </Strong>", MsgBoxStyle.OkOnly, "OldNotCorrect")
                            mIsRequestedForSave = False
                            Exit Sub
                        End If
                        'End If
                        mUser.ChangePassword = False
                        'Added by Vikrant on 23-July-2012 For ALL11072012
                        If AppSettings("PasswordSettings") = "True" Then
                            mUser.StartDate = Today.Date.ToShortDateString
                            If mUser.RemainingDays <= 0 Then
                                mUser.ChangePassword = True
                            End If
                        End If
                        'End

                        mUser.ApplyEdit()
                        mUser = CType(mUser.Save, User)
                        Session("MsgFor") = "Success"
                        Session("mRequestBy") = mRequestBy
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Password, SIMsgBox.Message_text.Password, "<Strong>Your password has been changed ! </Strong>", MsgBoxStyle.OKOnly)
                        'msg1.ReplacePage = "wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString                       
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.Password, MSGBox.Message_text.Password, "<Strong>Your password has been changed ! </Strong>", MsgBoxStyle.OkOnly, "Success")
                    Else
                        mIsRequestedForSave = False
                        If mUser.GetBrokenRulesCollection.Count > 0 Then
                            Dim i As Integer
                            Dim str As String = ""
                            For i = 0 To mUser.GetBrokenRulesCollection.Count - 1
                                str = str + mUser.GetBrokenRulesCollection.Item(i).Description + "<br>"
                            Next
                            Session("MsgFor") = "OldNotCorrect"
                            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Password, SIMsgBox.Message_text.Password, "<Strong>" & str & "</Strong>", MsgBoxStyle.OKOnly)
                            'msg1.ReplacePage = "wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString
                            'msg1.Show()
                            MSGBoxCtrl.show(MSGBox.Message_title.Password, MSGBox.Message_text.Password, "<Strong>" & str & "</Strong>", MsgBoxStyle.OkOnly, "OldNotCorrect")
                        End If
                    End If
                Else
                    If mUser.IsValid Then
                        'Verifing old Password
                        If mRequestBy = RequestedBy.User Then
                            If Not (mOldPassword = GetDbPassword(mUser.Name, txtOldPassword.Text)) Then
                                Session("MsgFor") = "OldNotCorrect"
                                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Password, SIMsgBox.Message_text.Password, "<Strong>Old Password is not correct! </Strong>", MsgBoxStyle.OKOnly)
                                'msg.ReplacePage = "wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString                               
                                'msg.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.Password, MSGBox.Message_text.Password, "<Strong>Old Password is not correct! </Strong>", MsgBoxStyle.OkOnly, "OldNotCorrect")
                                mIsRequestedForSave = False
                                Exit Sub
                            End If
                        End If
                        mUser.ChangePassword = False
                        'Added by Vikrant on 27-July-2012 For ALL11072012
                        If AppSettings("PasswordSettings") = "True" Then
                            If mUser.RemainingDays <= 0 Then
                                mUser.ChangePassword = True
                            End If
                        End If
                        'End
                        mUser.ApplyEdit()
                        mUser = CType(mUser.Save, User)
                        Session("MsgFor") = "Success"
                        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Password, SIMsgBox.Message_text.Password, "<Strong>Your password has been changed ! </Strong>", MsgBoxStyle.OKOnly)
                        'msg1.ReplacePage = "wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString                       
                        'msg1.Show()
                        MSGBoxCtrl.show(MSGBox.Message_title.Password, MSGBox.Message_text.Password, "<Strong>Your password has been changed ! </Strong>", MsgBoxStyle.OkOnly, "Success")
                    Else
                        mIsRequestedForSave = False
                        If mUser.GetBrokenRulesCollection.Count > 0 Then
                            Dim i As Integer
                            Dim str As String = ""
                            For i = 0 To mUser.GetBrokenRulesCollection.Count - 1
                                str = str + mUser.GetBrokenRulesCollection.Item(i).Description + "<br>"
                            Next
                            Session("MsgFor") = "OldNotCorrect"
                            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Password, SIMsgBox.Message_text.Password, "<Strong>" & str & "</Strong>", MsgBoxStyle.OKOnly)
                            'msg1.ReplacePage = "wfChangePassword.aspx?RequestedBy=" & Session("mRequestBy") & "&UserID=" & mUserID.ToString                           
                            'msg1.Show()
                            MSGBoxCtrl.show(MSGBox.Message_title.Password, MSGBox.Message_text.Password, "<Strong>" & str & "</Strong>", MsgBoxStyle.OkOnly, "OldNotCorrect")
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Session("mRequestBy") = 1 Then
            'Commented and Added by Vikrant on 27-July-2012 For ALL11072012
            ' ''Response.Redirect("Login.aspx?BackPage=" & Request.QueryString("BackPage"))
            Web.Security.FormsAuthentication.SignOut()
            Session.Remove("MenuID")
            Session.Remove("MiddleFrame")
            MarkLog(Util.Action.Logoff)
            'Drop all the references to the Principal.
            Thread.CurrentPrincipal = Nothing
            Session.Remove("ReminderFired")
            Dim str As String
            'str = "<script language=javascript>  window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
            str = "window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenPageScript", str, True)
        Else
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region
End Class