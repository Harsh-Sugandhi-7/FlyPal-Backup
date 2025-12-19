'AJAX COnversion By Vikrant On 16-May-2014

Public Class wfItemType_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPartType As PartType
    Public mPartTypeList As PartTypeList
    Public OpenFrom As Integer
    Dim EventLogID As Guid
    Public mPartStatusList As PartStatusList 'Added By Vikrant On 22-Oct-2012 For ALL22102012-1
    'public 
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mPartType = Session("mPartType")
        mPartTypeList = Session("mPartTypeList")
        mPartStatusList = Session("mPartStatusList") 'Added By Vikrant On 22-Oct-2012 For ALL22102012-1  
    End Sub
    Private Sub SetSession()
        Session("mPartType") = mPartType
        Session("mPartTypeList") = mPartTypeList
        Session("mPartStatusList") = mPartStatusList 'Added By Vikrant On 22-Oct-2012 For ALL22102012-1  
    End Sub
    Private Sub NewRecord()
        mPartType = PartType.NewPartType
        Session("mPartType") = mPartType
    End Sub
    Private Sub EditRecord(ByVal mId As Integer)
        mPartType = PartType.GetPartType(mId)
        Session("mPartType") = mPartType
    End Sub
    Private Sub DeleteRecord(ByVal mId As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartType = PartType.GetPartType(mId)
        Session("mPartType") = mPartType
    End Sub
    Private Sub setObject()
        mPartType.Name = txtName.Text.Trim
        mPartType.Code = txtGLCode.Text.Trim
        mPartType.Color = txtColor.Text.Trim
        mPartType.PartStatusID = CInt(cmbPartStatusList.SelectedValue) 'Added By Vikrant On 22-Oct-2012 For ALL22102012-1 
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mPartType = Session("mPartType")
                            PartType.DeletePartType(mPartType.ID)
                            NewRecord()
                            DataFieldBind()
                            lblTitle.Text = "Part Type [New]"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "ParentCallBackFunction();", True)
                            upnlItemTypeDetails.Update()
                            upnlGridView.Update()
                            upnlTitle.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            'SetColorLable()
                            lblTitle.Text = "Part Type [New]"
                            upnlItemTypeDetails.Update()
                            upnlGridView.Update()
                            upnlTitle.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "PartType", mPartType.Name, Util.ErrorType.NoError, mPartType.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "ParentCallBackFunction();", True)
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
    'Private Sub SetColorLable()
    '    Dim labelColor As Label
    '    For i As Integer = 0 To dgCategory.Rows.Count - 1
    '        labelColor = dgCategory.Rows(i).FindControl("Label1")
    '        If mPartTypeList.Item(i).Color = "ffffff" Then
    '            labelColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
    '        Else
    '            labelColor.BackColor = c
    '        End If
    '    Next
    'End Sub
    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(3).BackColor = System.Drawing.ColorTranslator.FromHtml("#" & e.Row.Cells(6).Text) ''Ajay 21-02-2023  7 => 6
        End If
    End Sub
    Private Sub ControlVisibility(Optional ByVal id As Integer = 0)
        If id = 0 Then
            txtName.Enabled = True
            txtGLCode.Enabled = True
        Else
            If mPartTypeList.Item(id, "").ItemTypeCount > 0 Then
                txtName.Enabled = False
                txtGLCode.Enabled = False
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPartTypeList = PartTypeList.GetPartTypeList()
        dgCategory.DataSource = mPartTypeList
        Session("mPartTypeList") = mPartTypeList

        'Added By Vikrant On 22-Oct-2012 For ALL22102012-1  
        mPartStatusList = PartStatusList.GetPartStatusList(True)
        cmbPartStatusList.DataSource = mPartStatusList
        Session("mPartStatusList") = mPartStatusList
        'End
        lblResult.Text = "Part Type List: " & mPartTypeList.Count & " Record(s) Found."
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        OpenFrom = CType(Request.QueryString("OpenFrom"), Integer)

        EventLogID = CType(Session("EventLogID"), Guid)
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        If Not IsPostBack And Session("sender") = "" Then
            NewRecord()
            DataFieldBind()
            'SetColorLable()
            ControlVisibility()
        End If
        'SetColorLable()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "PartType", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Session.Remove("PartTypeEdit")
        'If OpenFrom = 1 Then
        '    Response.Redirect("wfReceiptItem.aspx?BackPage=" & "wfReceipt.aspx" & "&ChildPage1=" & Request.QueryString("ChildPage1"))  '"wfReceiptPendingOrderList.aspx")
        'Else
        '    Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice.aspx" & "&ChildPage1=" Request.QueryString("ChildPage1")) '& "wfReceiptPendingOrderList.aspx")
        'End If
        Select Case OpenFrom
            Case 1
                Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=" & "wfReceipt_Ajax.aspx" & "&ChildPage1=" & Request.QueryString("ChildPage1"))  '"wfReceiptPendingOrderList.aspx")
            Case 2
                Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx" & "&ChildPage1=" & Request.QueryString("ChildPage1")) '& "wfReceiptPendingOrderList.aspx")
            Case 3
                Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type"))
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("PartNew") And mPartType.IsNew) Or (Not User.IsInRole("PartEdit") And Not mPartType.IsNew) Then
            setObject()
            SetSession()
            MarkLog(Util.Action.Save, "PartType", User.Identity.Name & "is not Authorized User to Save " & mPartType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                If Session("PartTypeEdit") = False Then
                    If mPartTypeList.Contains(txtName.Text.Trim) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                    setObject()
                Else
                    setObject()
                    If mPartType.NameDirty Then
                        If mPartTypeList.Contains(txtName.Text.Trim) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If
                    End If
                    Session("PartTypeEdit") = False
                End If
                mPartType.Save()
                MarkLog(Util.Action.Save, "PartType", "Part Type : " + mPartType.Name + " Status : " + cmbPartStatusList.SelectedItem.ToString, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                NewRecord()
                DataFieldBind()
                ' SetColorLable()
                lblTitle.Text = "Part Type [New]"
                upnlTitle.Update()
                upnlItemTypeDetails.Update()
                upnlGridView.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                NewRecord()
                'SetColorLable()
                lblTitle.Text = "Part Type [New]"
                upnlTitle.Update()
                upnlItemTypeDetails.Update()
            End Try
            'Else
            '    upnlValidationSummary.Update()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "ParentCallBackFunction();", True)
    End Sub
    Private Sub dgCategory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCategory.Sorting
        mPartStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartTypeList") = mPartTypeList
        dgCategory.DataSource = mPartTypeList
        dgCategory.DataBind()
    End Sub
    Private Sub dgCategory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCategory.PageIndexChanging
        dgCategory.PageIndex = e.NewPageIndex
        dgCategory.DataSource = mPartTypeList
        Session("mPartTypeList") = mPartTypeList
        dgCategory.DataBind()
    End Sub
    Private Sub dgCategory_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCategory.RowCommand
        Dim Idx As Int32
        Dim mId As Integer

        Select Case e.CommandName
            Case "EditRec"
                'Idx = CInt(e.CommandArgument) + dgCategory.PageIndex * dgCategory.PageSize
                'mId = mPartTypeList(Idx).ID

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 21-feb-2023
                Idx = gvr.RowIndex
                mId = mPartTypeList(Idx).ID

               ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "ParentCallBackFunction();", True)
                If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Edit, "PartType", User.Identity.Name & "is not authorized user to edit " & mPartType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                ControlVisibility(mId)
                EditRecord(mId)
                Session("PartTypeEdit") = True
                txtName.DataBind()
                txtGLCode.DataBind()
                txtColor.DataBind()
                cmbPartStatusList.DataBind() 'Added By Vikrant On 22-Oct-2012 For ALL22102012-1 
                'MarkLog(Util.Action.Edit, "PartType", mPartType.Name, Util.ErrorType.NoError, mPartType.ID, EventLogID)
                If Len(mPartType.Name) > 15 Then
                    lblTitle.Text = "Part Type  [" & mPartType.Name.Substring(0, 15) & "...]"
                Else
                    lblTitle.Text = "PartType [" & mPartType.Name & "]"
                End If
                'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
                lblResult.Text = "Part Type List: " & mPartTypeList.Count & " Record(s) Found."
                upnlTitle.Update()
                upnlItemTypeDetails.Update()
            Case "DeleteRec"
                'Idx = CInt(e.CommandArgument) + dgCategory.PageIndex * dgCategory.PageSize
                'mId = mPartTypeList(Idx).ID

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 21-feb-2023
                Idx = gvr.RowIndex
                mId = mPartTypeList(Idx).ID

                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "ParentCallBackFunction();", True)
                If (Not User.IsInRole("PartDelete")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Delete, "PartType", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(mId)
                upnlItemTypeDetails.Update()
                ControlVisibility()
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "ParentCallBackFunction();", True)
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        NewRecord()
        DataFieldBind()
        lblTitle.Text = "Part Type [New]"
        ControlVisibility()
        upnlTitle.Update()
        upnlItemTypeDetails.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
        upnlItemTypeDetails.Update()
    End Sub
#End Region

End Class