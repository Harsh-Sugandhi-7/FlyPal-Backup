'Created By     :   Saylee
'Dated          :   21-Aug-2015


Public Class wfAuditCategory_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditCategory As AuditCategory
    Public mAuditCategoryList As AuditCategoryList
    Public mAuditStandardList As AuditStandardList

    Dim AuditStandardID As Guid
    Dim EventlogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAuditCategory = CType(Session("mAuditCategory"), AuditCategory)
        mAuditCategoryList = CType(Session("mAuditCategoryList"), AuditCategoryList)

        AuditStandardID = New Guid(Session("AuditStandardID").ToString)
    End Sub
    Private Sub SetSession()
        Session("mAuditCategory") = mAuditCategory
        Session("mAuditCategoryList") = mAuditCategoryList

        Session("AuditStandardID") = AuditStandardID
    End Sub
    Private Sub NewRecord()
        mAuditCategory = AuditCategory.NewAuditCategory()
        Session("mAuditCategory") = mAuditCategory
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mAuditCategory = AuditCategory.GetChildAuditCategory(mId)
        Session("mAuditCategory") = mAuditCategory
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfAuditCategory.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAuditCategory = AuditCategory.GetChildAuditCategory(mId)
        Session("mAuditCategory") = mAuditCategory
    End Sub
    Private Sub setObject()
        mAuditCategory.Name = Trim(txtName.Text)
        mAuditCategory.IdentificationNo = Trim(txtIdentificationtxtNo.Text)
        mAuditCategory.AuditStandardID = New Guid(cmbStandard.SelectedValue.ToString)
        Session("mAuditCategory") = mAuditCategory
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
      Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mAuditCategory = CType(Session("mAuditCategory"), AuditCategory)

                            AuditCategory.DeleteAuditCategory(mAuditCategory.ID)
                            '   Response.Redirect("wfAuditCategory.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type"))

                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlTaskDet.Update()
                            upnlGrid.Update()
                            upnlResult.Update()
                            upnlTitle.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                                'Ajay 20-11-2023
                                NewRecord()
                                DataFieldBind()
                                SetTitle()
                                upnlTaskDet.Update()
                                upnlGrid.Update()
                                upnlResult.Update()
                                upnlTitle.Update()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Audit Category", mAuditCategory.Name, Flypal.Util.ErrorType.NoError, mAuditCategory.ID, EventlogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""

                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mAuditCategory.IsNew Then
            lbltitle.Text = "Task Category [New]"
        Else
            If Len(mAuditCategory.Name) > 15 Then
                lbltitle.Text = "Task Category [" & mAuditCategory.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Task Category [" & mAuditCategory.Name & "]"
            End If
        End If
        lblResult.Text = "Task Category List: " & mAuditCategoryList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAuditCategoryList = AuditCategoryList.GetAuditCategoryList(AuditStandardID)
        Session("mAuditCategoryList") = mAuditCategoryList
        dgAuditCategoryList.DataSource = mAuditCategoryList

        mAuditStandardList = AuditStandardList.GetAuditStandardList("(SELECT)")
        cmbStandard.DataSource = mAuditStandardList
        Session("mAuditStandardList") = mAuditStandardList

        DataBind()
        cmbStandard.SelectedValue = AuditStandardID.ToString
    End Sub

    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "txtName" Then
        '    If Len(txtName.Text) >= 100 Then
        '        custValidator.ErrorMessage = "Task Category Name should not be greater than 100 characters."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If

        If custValidator.ControlToValidate = "cmbStandard" Then
            If cmbStandard.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Standard Required."
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
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If

        EventlogID = CType(Session("EventlogID"), Guid)     'Added by Vikrant on 25-July-2011
        If Not IsPostBack Then
            AuditStandardID = New Guid(Session("AuditStandardID").ToString)
            Session("AuditStandardID") = AuditStandardID

            'If (Request.QueryString("BackPage2") Is Nothing Or (Request.QueryString("BackPage2") = "")) And (Request.QueryString("ChildPage2") Is Nothing Or (Request.QueryString("ChildPage2") = "")) Then Session("MiddleFrame") = "wfAuditCategory.aspx?"
            NewRecord()
            DataFieldBind()
            SetTitle()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ''If (Not User.IsInRole("AuditCategoryNew") And mAuditCategory.IsNew) Or (Not User.IsInRole("AuditCategoryEdit") And Not mAuditCategory.IsNew) Then
        ''    setObject()
        ''    SetSession()
        ''    MarkLog(Flypal.Util.Action.Save, "AuditCategory", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfAuditCategory.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&ChildPage2=" & Request.QueryString("ChildPage2")& "&AuditStandardID=" & AuditStandardID.ToString
        ''    Session("sender") = "Authorization"
        ''    msg.Show()
        ''    Exit Sub
        ''End If
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mAuditCategory.Save()
            'Changed by Vikrant on 25-July-2011
            MarkLog(Flypal.Util.Action.Save, "Audit Category", mAuditCategory.Name, Flypal.Util.ErrorType.HandledError, mAuditCategory.ID, EventlogID)
            mAuditCategory = AuditCategory.NewAuditCategory()
            NewRecord()
            DataFieldBind()
            SetSession()
            SetTitle()
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            upnlTitle.Update()
            upnlTaskDet.Update()
            upnlGrid.Update()
            upnlResult.Update()
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            DataFieldBind()
        End Try
    End Sub

    Private Sub dgAuditCategoryList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditCategoryList.RowCommand
        'If e.Item.Cells(0).Text = "ID" Or e.Item.Cells(0).Text = "" Then Exit Sub
        'Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "EditRec"
                ''If (Not User.IsInRole("AuditCategoryView") And Not User.IsInRole("AuditCategoryEdit")) Then
                ''    setObject()
                ''    SetSession()
                ''    MarkLog(Flypal.Util.Action.Edit, "AuditCategory", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfAuditCategory.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&ChildPage2=" & Request.QueryString("ChildPage2")& "&AuditStandardID=" & AuditStandardID.ToString
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                Dim Idx As Int32 = e.CommandArgument.ToString + dgAuditCategoryList.PageIndex * dgAuditCategoryList.PageSize
                Dim mID As Guid = mAuditCategoryList(Idx).ID
                EditRecord(mID)
                dgAuditCategoryList.DataSource = mAuditCategoryList
                DataBind()
                upnlTaskDet.Update()
                SetTitle()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                'Changed by Vikrant on 25-July-2011
                MarkLog(Flypal.Util.Action.Edit, "Audit Category", mAuditCategory.Name, Flypal.Util.ErrorType.NoError, mAuditCategory.ID, EventlogID)
                upnlTitle.Update()
            Case "DeleteRec"
                ''If (Not User.IsInRole("AuditCategoryDelete")) Then
                ''    setObject()
                ''    SetSession()
                ''    MarkLog(Flypal.Util.Action.Delete, "AuditCategory", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfAuditCategory.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&ChildPage2=" & Request.QueryString("ChildPage2")& "&AuditStandardID=" & AuditStandardID.ToString
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                Dim Idx As Int32 = e.CommandArgument.ToString + dgAuditCategoryList.PageIndex * dgAuditCategoryList.PageSize
                Dim mID As Guid = mAuditCategoryList(Idx).ID
                DeleteRecord(mID)
        End Select
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Changed by Vikrant on 25-July-2011
        MarkLog(Flypal.Util.Action.[New], "Audit Category", "", Flypal.Util.ErrorType.NoError, mAuditCategory.ID, EventlogID)
        NewRecord()
        DataFieldBind()
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        SetTitle()
        upnlTitle.Update()
        upnlTaskDet.Update()
        upnlGrid.Update()

    End Sub
    Private Sub dgAuditCategoryList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditCategoryList.Sorting
        mAuditCategoryList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAuditCategoryList") = mAuditCategoryList
        dgAuditCategoryList.DataSource = mAuditCategoryList
        dgAuditCategoryList.DataBind()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        'Changed by Vikrant on 25-July-2011
        MarkLog(Flypal.Util.Action.Close, "Audit Category", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventlogID)
        Session("sender") = ""

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        'If Request.QueryString("ChildPage2") <> "" Then
        '    Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type"))
        'ElseIf Request.QueryString("BackPage2") <> "" Then
        '    Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type"))
        'Else
        '    Session("MiddleFrame") = ""
        '    Response.Redirect("Dashboard.aspx")
        'End If
    End Sub
    Private Sub imgbtnStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnStandard.Click
        setObject()
        'Dim str As String
        'str = "<script language='javascript'>openledgersame('wfAuditStandard.aspx?ChildPage3=wfAuditCategory.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type") & "');</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenStandardWindow", "OpenStandardWindow()", True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnAuditStandard_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnAuditStandard.Click
        mAuditStandardList = AuditStandardList.GetAuditStandardList("(SELECT)")
        Session("mAuditStandardList") = mAuditStandardList
        cmbStandard.DataSource = mAuditStandardList
        cmbStandard.DataBind()
        upnlStandard.Update()
    End Sub
#End Region

End Class