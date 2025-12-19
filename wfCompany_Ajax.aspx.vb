'Added by Prashant

Public Class wfCompany_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCompany As Company
    Public mCompanyList As CompanyList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCompany = CType(Session("mCompany"), Company)
        mCompanyList = CType(Session("mCompanyList"), CompanyList)
    End Sub
    Private Sub SetSession()
        Session("mCompany") = mCompany
        Session("mCompanyList") = mCompanyList
    End Sub
    Private Sub NewRecord()
        mCompany = Company.NewCompany(Guid.NewGuid)
        Session("mCompany") = mCompany
        lbltitle.Text = "Company [New]"
        pnlValidationSummary.Update()
        txtCompName.Enabled = True
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mCompany = Company.GetCompany(mId)
        Session("mCompany") = mCompany
        If Len(mCompany.Name) > 15 Then
            lbltitle.Text = "Company [" & mCompany.Name.Substring(0, 15) & "...]"
        Else
            lbltitle.Text = "Company [" & mCompany.Name & "]"
        End If
        pnlValidationSummary.Update()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        CompanyGridBind()
        mCompany = Company.GetCompany(mId)
        Session("mCompany") = mCompany
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub setObject()
        mCompany.Name = Trim(txtCompName.Text)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
    '            Case MsgBoxResult.Yes
    '                If CType(Session("sender"), String) = "Delete" Then
    '                    Try
    '                        Session("sender") = ""
    '                        mCompany = CType(Session("mCompany"), Company)
    '                        Company.DeleteCompany(mCompany.ID)
    '                        Response.Redirect("wfCompany.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2"))
    '                    Catch ex As SqlException
    '                        If ex.Number = 8145 Then
    '                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
    '                            msg1.ReplacePage = "wfCompany.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2")
    '                            msg1.Show()
    '                        ElseIf ex.Number = 2627 Then
    '                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
    '                            msg1.ReplacePage = "wfCompany.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2")
    '                            msg1.Show()
    '                        ElseIf ex.Number = 547 Then
    '                            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
    '                            msg1.ReplacePage = "wfCompany.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2")
    '                            'Changed By Utkarsh On 19-Jul-2011 For All19072011
    '                            MarkLog(Util.Action.Delete, "Company", "Can't delete : " & mCompany.Name & " is Currently in use", Util.ErrorType.NoError, mCompany.ID, EventLogID)
    '                            'End

    '                            msg1.Show()
    '                        End If
    '                        DataFieldBind()
    '                        msgCount = ex.Errors.Count
    '                    Finally
    '                        If msgCount = 0 Then
    '                            'Changed By Utkarsh On 19-Jul-2011 For All19072011
    '                            MarkLog(Util.Action.Delete, "Company", mCompany.Name, Util.ErrorType.NoError, mCompany.ID, EventLogID)
    '                            'End

    '                        End If
    '                    End Try
    '                End If
    '            Case MsgBoxResult.No
    '                Session("sender") = ""
    '                Response.Redirect("wfCompany.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2"))
    '            Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
    '                Session("sender") = ""
    '                DataFieldBind()
    '                Response.Redirect("wfCompany.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2"))
    '            Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
    '                Session("sender") = ""
    '                DataFieldBind()
    '                Response.Redirect("wfCompany.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2"))
    '        End Select
    '    ElseIf Result1 = -1 Then
    '        Session("sender") = ""
    '        DataFieldBind()
    '        Response.Redirect("wfCompany.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2"))
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
                            Company.DeleteCompany(mCompany.ID)
                            NewRecord()
                            txtCompName.DataBind()
                            DataFieldBind()
                            upnlCompanyDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                Exit Sub
                            End If
                        Finally
                            MarkLog(Util.Action.Delete, "Company", mCompany.Name, Util.ErrorType.NoError, mCompany.ID, EventLogID)
                            NewRecord()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        txtCompName.Text = ""
                        NewRecord()
                        DataFieldBind()
                        upnlCompanyDetails.Update()
                    End If
                    CompanyGridBind()
                Case MsgBoxResult.Ok
                    'DataFieldBind()
                    CompanyGridBind()
             
            End Select
        End If
    End Sub
    'Private Sub SetTitle()
    '    If mCompany.IsNew Then
    '        lbltitle.Text = "Company [New]"
    '    Else
    '        If Len(mCompany.Name) > 15 Then
    '            lbltitle.Text = "Company [" & mCompany.Name.Substring(0, 15) & "...]"
    '        Else
    '            lbltitle.Text = "Company [" & mCompany.Name & "]"
    '        End If
    '    End If
    '    pnlValidationSummary.Update()
    'End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerCompany(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtCompName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCompanyList = CompanyList.GetCompanyList("", False, "")
        dgCompanyList.DataSource = mCompanyList
        Session("mCompanyList") = mCompanyList
        txtCompName.DataBind()
        upnlCompanyDetails.Update()
        CompanyGridBind()
    End Sub
    Private Sub CompanyGridBind()
        dgCompanyList.DataSource = mCompanyList
        dgCompanyList.DataBind()
        lblResult.Text = "Company List: " & mCompanyList.Count & " Record(s) Found."
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If txtCompName.Enabled = True Then
            setFocus(txtCompName)
        End If
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If IsNothing(Request.QueryString("BackPage2")) Or Request.QueryString("BackPage2") = "" Then
                Session("MiddleFrame") = "wfCompany_Ajax.aspx?"
            End If
            NewRecord()
            DataFieldBind()
        End If
        'SetTitle()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("CompanyNew") And mCompany.IsNew) Or (Not User.IsInRole("CompanyEdit") And Not mCompany.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Try
            If IsValid Then
                setObject()
                mCompany.Save()
                MarkLog(Util.Action.Save, "Company", mCompany.Name, Util.ErrorType.NoError, mCompany.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
                'SetTitle()
            Else
                CompanyGridBind()
                pnlValidationSummary.Update()
                Exit Sub
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            DataFieldBind()
        End Try
    End Sub
    Private Sub dgCompanyList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCompanyList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument) + dgCompanyList.PageIndex * dgCompanyList.PageSize
                Dim mID As Guid = mCompanyList(index).ID
                Dim mName As String = mCompanyList(index).Name
                If (Not User.IsInRole("CompanyView") And Not User.IsInRole("CompanyEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecord(mID)
                setFocus(txtCompName)
                txtCompName.DataBind()
                upnlCompanyDetails.Update()
                CompanyGridBind()
                DisableName(mID) 'Added by : Shital 19-Jun-2020, ALL16062020
                MarkLog(Util.Action.Edit, "Company", mCompany.Name, Util.ErrorType.NoError, mCompany.ID, EventLogID)
                'SetTitle()
            Case "Remove"
                Dim index As Integer = CInt(e.CommandArgument) + dgCompanyList.PageIndex * dgCompanyList.PageSize
                Dim mID As Guid = mCompanyList(index).ID
                Dim mName As String = mCompanyList(index).Name
                If (Not User.IsInRole("CompanyDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub dgCompanyList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCompanyList.PageIndexChanging
        dgCompanyList.PageIndex = e.NewPageIndex
        dgCompanyList.DataSource = mCompanyList
        dgCompanyList.DataBind()
        Session("mCompanyList") = mCompanyList
        upnlGridView.Update()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Util.Action.[New], "Company", "", Util.ErrorType.NoError, mCompany.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        If txtCompName.Enabled = True Then
            setFocus(txtCompName)
        End If
        'SetTitle()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Company", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        If IsNothing(Request.QueryString("BackPage2")) Or Request.QueryString("BackPage2") = "" Then
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Else
            Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class