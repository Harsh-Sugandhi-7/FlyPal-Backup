' Rajnish - 19-09-2006 
Partial Class wfHobbsOffset_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents rfvDate As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents calDate As SIControls.SICalendar

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mLog As Log
    Public mMachine As Machine
    Public mHobbsOffset As HobbsOffset
    Public mHobbsOffsetList As HobbsOffsetList
    Public Flag As Integer
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mLog = CType(Session("mLog"), Log)
        mMachine = CType(Session("mMachine"), Machine)
        mHobbsOffset = CType(Session("mHobbsOffset"), HobbsOffset)
        mHobbsOffsetList = CType(Session("mHobbsOffsetList"), HobbsOffsetList)
    End Sub
    Private Sub SetSession()
        Session("mLog") = mLog
        Session("mMachine") = mMachine
        Session("mHobbsOffset") = mHobbsOffset
        Session("mHobbsOffsetList") = mHobbsOffsetList
    End Sub
    Private Sub RemoveSession()
        mHobbsOffsetList = Nothing
        Session.Remove("mHobbsOffsetList")
    End Sub
    Private Sub NewRecord()
        mHobbsOffset = HobbsOffset.NewHobbsOffset(Guid.NewGuid, mLog.MachineID)
        Session("mHobbsOffset") = mHobbsOffset
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mHobbsOffset = HobbsOffset.GetHobbsOffset(mId)
        Session("mHobbsOffset") = mHobbsOffset

        If mHobbsOffset.Date.ToString = "" Then
            calDate.Text = ""
        Else
            calDate.Text = mHobbsOffset.DateFormatted
        End If


        txtOffset.DataBind()
        If Not mHobbsOffset.IsLogExist Then
            calDate.BackColor = Color.Gainsboro
            txtOffset.BackColor = Color.Gainsboro
        End If
        calDate.Enabled = mHobbsOffset.IsLogExist
        'txtOffset.ReadOnly = Not mHobbsOffset.IsLogExist
    End Sub
    Private Sub DeleteRecord(ByVal Index As Integer)
        '' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        '' '' ''msg1.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
        '' '' ''Session("sender") = "Delete"
        '' '' ''msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mHobbsOffsetList.CurrentIndex = Index
        'mHobbsOffset = HobbsOffset.GetHobbsOffset(mId)
        If txtOffset.Enabled = True Then
            SetFocus(txtOffset)
        End If

        SetPage()

        Session("mHobbsOffset") = mHobbsOffset
    End Sub
    Private Sub addAttributes()
        txtOffset.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtOffset').value,event)")
    End Sub
    Private Sub setObject()
        mHobbsOffset.OffSet = Trim(txtOffset.Text)

        If (calDate.Text = "") Then  ' '' ''If Not (calDate.IsDateValue) Then
            mHobbsOffset.Date = System.DBNull.Value
        Else
            mHobbsOffset.Date = calDate.Text.ToString
        End If

        Session("mHobbsOffset") = mHobbsOffset
    End Sub
    'Private Sub setFocus(ByVal cntrl As SIControls.SICalendar)
    '    If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
    '    Dim str As String
    '    str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
    '     ClientScript.RegisterStartupScript(Me.GetType(),"focusscript", str)
    'End Sub
    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub


    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult

        ' '' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        ' '' ''    Result1 = -1
        ' '' ''Else
        ' '' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        ' '' ''End If

        ' '' ''Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    ' '' ''If CType(Session("sender"), String) = "Delete" Then  MSGBoxCtrl.Sende
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            ' '' ''Session("sender") = ""
                            HobbsOffset.DeleteHobbsOffset(mHobbsOffsetList.CurrentItem.Id)
                            ' '' ''Response.Redirect("wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                            NewRecord()
                            DataFieldBind()
                            SetPage()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                ' '' ''msg1.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                                ' '' ''msg1.Show()

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 2627 Then
                                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                ' '' ''msg1.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                                ' '' ''msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                ' '' ''msg1.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                                ' '' ''msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                        End Try
                    End If
                Case MsgBoxResult.No
                    ' '' ''Session("sender") = ""
                    ' '' ''Response.Redirect("wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))                   
                    mHobbsOffsetList = Session("mHobbsOffsetList")
                    dgHobbsOffsetList.DataSource = mHobbsOffsetList
                    txtOffset.Text = mHobbsOffset.OffSet
                    DataBind()
                    upnlDetails.Update()

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    ' '' ''Session("sender") = ""
                    ' '' ''DataFieldBind()
                    ' '' ''Response.Redirect("wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    '' '' ''Session("sender") = ""
                    DataFieldBind()
                    ' '' ''Response.Redirect("wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ' '' ''Response.Redirect("wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
            DataFieldBind()
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub

    Private Sub SetPage()
        If mHobbsOffset.IsNew Then
            lbltitle.Text = "Hobbs Offset for " & mLog.RegNo
        Else
            lbltitle.Text = "Hobbs Offset for " & mLog.RegNo
        End If
        lblResult.Text = "List of Hobbs Offset : " & mHobbsOffsetList.Count & " Record(s) Found"

        upnlTitle.Update()
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim CustValidator As CustomValidator = CType(s, CustomValidator)
        REM: this is for grid validation
        setObject()         REM: this is required to get the date updated.
        Dim str As String = ""
        If Not mHobbsOffset.IsValid Then
            For i As Integer = 0 To mHobbsOffset.GetBrokenRulesCollection.Count - 1
                str = str + mHobbsOffset.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        If str <> "" Then
            CustValidator.ErrorMessage = str
            e.IsValid = False
        Else
            e.IsValid = True
        End If
        Flag = 1
    End Sub
    Private Sub DataFieldBind()
        mHobbsOffsetList = HobbsOffsetList.GetHobbsOffsetList(mLog.MachineID)
        Session("mHobbsOffsetList") = mHobbsOffsetList
        dgHobbsOffsetList.DataSource = mHobbsOffsetList

        'Added on 28-05-2007 by Kalpesh Shah
        If mHobbsOffset.Date.ToString = "" Then
            calDate.Text = ""
        Else
            calDate.Text = mHobbsOffset.DateFormatted
        End If


        calDate.Enabled = mHobbsOffset.IsLogExist

        DataBind()

        upnlDetails.Update()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'addAttributes()
        If Not IsPostBack And MSGBoxCtrl.Sender = "" Then  '''''CType(Session("sender"), String)
            If txtOffset.Enabled = True Then
                SetFocus(txtOffset)
            End If
            ' '' ''If Not (calDate.IsDateValue) Then calDate.Text = Today.Date.ToShortDateString
            If (calDate.Text = "") Then calDate.Text = Today.Date.ToShortDateString

            'Code Commented and newly Added on 28-05-2007 by Kalpesh Shah -------- 
            ''calDate.TitleText = Today.Date.ToShortDateString
            ''calDate.DateToday = Today.Date
            ''calDate.SelectedDate = Today.Date
            calDate.Text = Today.Date.ToShortDateString
            '---------------------------------------------------------------------
            'mHobbsOffset.OffSet = mLog.PrevHobbsValue
            DataFieldBind()
        End If
        ' '' ''MessageBoxResult()
        SetPage()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
            setObject()
            SetSession()

            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "User is not Authorised", MsgBoxStyle.OkOnly)
            ' '' ''msg.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            ' '' ''Session("sender") = "Authorization"
            ' '' ''msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "User is not Authorised", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If Not IsValid Then upnlErrorList.Update() : Exit Sub
        Try
            setObject()
            mHobbsOffset = CType(mHobbsOffset.Save(), HobbsOffset)
            NewRecord()
            Session("mHobbsOffset") = mHobbsOffset
            ' '' ''Response.Redirect("wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))


            DataFieldBind()
            SetPage()
            upnlErrorList.Update()

        Catch ex As SqlException
            If ex.Number = 8145 Then
                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                ' '' ''msg1.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                ' '' ''Session("sender") = "Delete"
                ' '' ''msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")

            ElseIf ex.Number = 2627 Then
                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                ' '' ''msg1.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                ' '' ''Session("sender") = "Delete"
                ' '' ''msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 547 Then
                ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                ' '' ''msg1.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                ' '' ''Session("sender") = "Delete"
                ' '' ''msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            End If
        End Try

    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            ' '' ''msg.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            ' '' ''Session("sender") = "Authorization"
            ' '' ''msg.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        DataFieldBind()
        upnlErrorList.Update()
        ' '' ''Response.Redirect("wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub dgHobbsOffsetList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgHobbsOffsetList.ItemCommand
        Dim Index As Integer = dgHobbsOffsetList.CurrentPageIndex * dgHobbsOffsetList.PageSize + e.Item.ItemIndex
        Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "Edit"
                If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
                    ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    ' '' ''msg.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                    ' '' ''msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecord(mId)
                If txtOffset.Enabled = True Then
                    SetFocus(txtOffset)
                End If
                SetPage()
                upnlErrorList.Update()

            Case "Delete"
                If (Not User.IsInRole("LogDelete")) Then
                    ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    ' '' ''msg.ReplacePage = "wfHobbsOffset.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                    ' '' ''msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(Index)
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

    End Sub
#End Region

    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Protected Sub calDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calDate.TextChanged
        'Changed by Yogita for Wrong Date Format
        If IsDate(calDate.Text) Or (calDate.Text = "") Then
            '

            If calDate.Text = "" Then
                mHobbsOffset.Date = System.DBNull.Value
                calDate.Text = mHobbsOffset.Date.ToString
            Else
                mHobbsOffset.Date = calDate.Text
                calDate.Text = mHobbsOffset.DateFormatted
            End If


        Else
            calDate.Text = ""
        End If
    End Sub
End Class
