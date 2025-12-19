'AJAX Conversion By Vikrant on 10-July-2014

Public Class wfTerm_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTerm As Term
    Public mTermList As TermList
    Public BackPage As String
    Public OpenFrom As Integer
    Public mTransTypeID As Trans
    Public mTransactionList As TransactionList
#End Region

#Region " Enumeration "
    Private Enum Rights
        [New] = 0
        Edit = 1
        Delete = 2
        View = 3
        Print = 4
    End Enum
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTerm = Session("mTerm")
        mTermList = Session("mTermList")
        mTransTypeID = Session("mTransTypeId")
    End Sub
    Private Sub SetSession()
        Session("mTerm") = mTerm
        Session("mTermList") = mTermList
    End Sub
    Private Sub NewRecord()
        ' mTerm = Term.NewTerm
        mTerm = Term.NewTerm(Guid.Empty, OpenFrom)
        Session("mTerm") = mTerm
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mTerm = Term.GetTerm(mId, OpenFrom)
        Session("mTerm") = mTerm
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mTerm = Term.GetTerm(mId, OpenFrom)
        Session("mTerm") = mTerm
    End Sub
    Private Sub setObject()
        mTerm.Terms = txtName.Text
        mTerm.Type = OpenFrom
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
                            mTerm = Session("mTerm")
                            Term.DeleteTerm(mTerm.ID, OpenFrom)
                            NewRecord()
                            DataFieldBind()
                            lblTitle.Text = "Term [New]"
                            upnlTermMasterDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            txtName.Text = ""
                            DataFieldBind()
                            lblTitle.Text = "Term [New]"
                            upnlTermMasterDetails.Update()
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok '' And Session("sender") = ""        'Code Added
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
    Private Sub SetPage()
        If OpenFrom = 1 Then
            If mTerm.Terms = "" Then
                lblTitle.Text = "Term [New]"
            Else
                If mTerm.Terms.Length > 30 Then
                    lblTitle.Text = "Term [" + mTerm.Terms.Substring(0, 26) + "..." + "]"
                Else
                    lblTitle.Text = "Term [" + mTerm.Terms + "]"
                End If
            End If
        ElseIf OpenFrom = 2 Then
            If mTerm.Terms = "" Then
                lblTitle.Text = "Term [New]"
            Else
                If mTerm.Terms.Length > 30 Then
                    lblTitle.Text = "Term [" + mTerm.Terms.Substring(0, 26) + "..." + "]"
                Else
                    lblTitle.Text = "Term [" + mTerm.Terms + "]"
                End If
            End If
        End If
    End Sub
    Private Function IsInRole(ByVal mRights As Rights) As Boolean
        mTransactionList = TransactionList.GetTransactionList
        ' Session("mTransactionList") = mTransactionList
        Dim strIssue As String = mTransactionList.GetModuleName(mTransTypeID)
        With User
            Select Case mRights
                Case Rights.[New]
                    Return ((.IsInRole("EnquiryNew") And OpenFrom = 3) Or (.IsInRole("QuotationNew") And OpenFrom = 4) Or (.IsInRole("SalesOrderNew") And OpenFrom = 5) Or (.IsInRole("OrderNew") And OpenFrom = 1) Or (.IsInRole(strIssue + "New") And OpenFrom = 2))
                Case Rights.Edit
                    Return ((.IsInRole("EnquiryEdit") And OpenFrom = 3) Or (.IsInRole("QuotationEdit") And OpenFrom = 4) Or (.IsInRole("SalesOrderEdit") And OpenFrom = 5) Or (.IsInRole("OrderEdit") And OpenFrom = 1) Or (.IsInRole(strIssue + "Edit") And OpenFrom = 2))
                Case Rights.Delete
                    Return ((.IsInRole("EnquiryDelete") And OpenFrom = 3) Or (.IsInRole("QuotationDelete") And OpenFrom = 4) Or (.IsInRole("SalesOrderDelete") And OpenFrom = 5) Or (.IsInRole("OrderDelete") And OpenFrom = 1) Or (.IsInRole(strIssue + "Delete") And OpenFrom = 2))
                Case Rights.View
                    Return ((.IsInRole("EnquiryView") And OpenFrom = 3) Or (.IsInRole("QuotationView") And OpenFrom = 4) Or (.IsInRole("SalesOrderView") And OpenFrom = 5) Or (.IsInRole("OrderView") And OpenFrom = 1) Or (.IsInRole(strIssue + "View") And OpenFrom = 2))
                Case Rights.Print
                    Return ((.IsInRole("EnquiryPrint") And OpenFrom = 3) Or (.IsInRole("QuotationPrint") And OpenFrom = 4) Or (.IsInRole("SalesOrderPrint") And OpenFrom = 5) Or (.IsInRole("OrderPrint") And OpenFrom = 1) Or (.IsInRole(strIssue + "Print") And OpenFrom = 2))
            End Select
        End With
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTermList = TermList.GetTermList("", OpenFrom)
        dgTerm.DataSource = mTermList
        Session("mTermList") = mTermList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        OpenFrom = Request.QueryString("OpenFrom")
        BackPage = Request.QueryString("BackPage")
        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                txtName.Focus()
            End If
            NewRecord()
            DataFieldBind()
            SetPage()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Commented by Prashant 01-July-2011
        'If (Not IsInRole(Rights.[New]) And mTerm.IsNew) Or (Not IsInRole(Rights.Edit) And Not mTerm.IsNew) Then
        '    setObject()
        '    SetSession()
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0"
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        Try
            If IsValid() Then
                setObject()
                mTerm.Save()
                mTerm = Term.NewTerm
                DataFieldBind()
                SetSession()
                lblTitle.Text = "Term [New]"
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
            NewRecord()
            lblTitle.Text = "Term [New]"
        End Try
    End Sub
    Private Sub dgTerm_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTerm.RowCommand
        Dim mId As Guid
        Dim Idx As Int32
        Select Case e.CommandName
            Case "EditRec"
                'Commented by Prashant 01-July-2011
                'If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                '    setObject()
                '    SetSession()
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0"
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                'mId = New Guid(dgTerm.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                'EditRecord(mId)
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Idx = gvr.RowIndex
                mId = New Guid(dgTerm.DataKeys(Idx).Value.ToString)
                EditRecord(mId)

                txtName.DataBind()
                txtName.Focus()
                If Len(mTerm.Terms.Trim) > 30 Then
                    lblTitle.Text = "Term [" & mTerm.Terms.Trim.Substring(0, 26) + "..." & "]"
                Else
                    lblTitle.Text = "Term [" & mTerm.Terms & "]"
                End If
                'If Type = 1 Then
                '    lblTitle.Text = "Term [" & IIf(Len(mTerm.Terms.Trim) > 30, mTerm.Terms.Trim.Substring(0, 26) + "...", mTerm.Terms) & "]"
                'ElseIf Type = 2 Then
                '    lblTitle.Text = "Term [" & IIf(Len(mTerm.Terms.Trim) > 30, mTerm.Terms.Trim.Substring(0, 26) + "...", mTerm.Terms) & "]"
                'End If
            Case "DeleteRec"
                'Commented by Prashant 01-July-2011
                'If (Not IsInRole(Rights.Delete)) Then
                '    setObject()
                '    SetSession()
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0"
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                'mId = New Guid(dgTerm.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                'DeleteRecord(mId)
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 13-Jan-2023
                Idx = gvr.RowIndex
                mId = New Guid(dgTerm.DataKeys(Idx).Value.ToString)
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        txtName.Focus()
        NewRecord()
        txtName.Text = ""
        lblTitle.Text = "Term [New]"
    End Sub
    Private Sub dgTerm_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTerm.Sorting
        mTermList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mTermList") = mTermList
        dgTerm.DataSource = mTermList
        dgTerm.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region


End Class