Public Class wfCustomerTerm_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCustomerTerm As CustomerTerm
    Public mCustomerTermList As CustomerTermList
    Public mCapabilityList As CapabilityList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCustomerTerm = CType(Session("mCustomerTerm"), CustomerTerm)
        mCustomerTermList = CType(Session("mCustomerTermList"), CustomerTermList)
    End Sub
    Private Sub SetSession()
        Session("mCustomerTerm") = mCustomerTerm
        Session("mCustomerTermList") = mCustomerTermList
    End Sub
    Private Sub NewRecord()
        mCustomerTerm = CustomerTerm.NewCustomerTerm()
        Session("mCustomerTerm") = mCustomerTerm
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mCustomerTerm = CustomerTerm.GetCustomerTerm(mId)
        Session("mCustomerTerm") = mCustomerTerm
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mCustomerTerm = CustomerTerm.GetCustomerTerm(mId)
        Session("mCustomerTerm") = mCustomerTerm
    End Sub
    Private Sub setObject()
        mCustomerTerm.Terms = Trim(txtTerm.Text)
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
                        Dim CustomerTermDet As String = String.Empty
                        Try
                            Session("sender") = ""
                            mCustomerTerm = CType(Session("mCustomerTerm"), CustomerTerm)

                            CustomerTermDet = mCustomerTerm.Terms
                            CustomerTerm.DeleteCustomerTerm(mCustomerTerm.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                Dim stringInfo As String = ""
                                If ex.Message.Contains("tabCustomerContractTerm") Then
                                    stringInfo = "Customer Contract"
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "CustomerTerm", "Can't delete : " & mCustomerTerm.Terms & " is Currently in use", Util.ErrorType.NoError, mCustomerTerm.ID, EventLogID)
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "CustomerTerm", CustomerTermDet, Util.ErrorType.NoError, mCustomerTerm.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        SetTitle()
                    End If
                    Session("sender") = ""
                    SetTitle()
                Case MsgBoxResult.Ok
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            DataFieldBind()

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            DataFieldBind()
        End If
        upnlCustomerTerm.Update()
    End Sub
    Private Sub SetTitle()
        If mCustomerTerm.IsNew Then
            lbltitle.Text = "Customer Term [New]"
        Else
            If Len(mCustomerTerm.Terms) > 15 Then
                lbltitle.Text = "Customer Term [" & mCustomerTerm.Terms.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Customer Term [" & mCustomerTerm.Terms & "]"
            End If
        End If
        lblResult.Text = "Customer Term List: " & mCustomerTermList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCustomerTermList = CustomerTermList.GetCustomerTermList()
        Session("mCustomerTermList") = mCustomerTermList
        dgCustomerTerm.DataSource = mCustomerTermList
        dgCustomerTerm.DataBind()
        txtTerm.Text = mCustomerTerm.Terms
        upnlCustomerTerm.Update()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()
        If Not mCustomerTerm.IsValid Then
            For i As Integer = 0 To mCustomerTerm.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mCustomerTerm.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If Not mCustomerTerm.IsValid Then
            For i As Integer = 0 To mCustomerTerm.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mCustomerTerm.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        If strMsg.Trim <> "" Then
            cvTerm.ErrorMessage = strMsg
            cvTerm.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            NewRecord()
            DataFieldBind()
            SetTitle()
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "CustomerTerm", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mCustomerTerm")
            Session.Remove("mCustomerTermList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
       If Not IsValid Then Exit Sub

        If CustomValidate1() Then
            Try
                setObject()
                mCustomerTerm.Save()
                MarkLog(Util.Action.Save, "CustomerTerm", mCustomerTerm.Terms, Util.ErrorType.NoError, mCustomerTerm.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
                SetTitle()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    If InStr(ex.Message, "UK_tabHolidays", CompareMethod.Text) Then
                        MSGBoxCtrl.show("Save Error!", "Duplicate Record", "You are trying to add duplicate.", MsgBoxStyle.OkOnly, "")
                    End If
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgCustomerTerm_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCustomerTerm.RowCommand
        Dim mId As Guid
        Select Case e.CommandName
            Case "ViewRec"
                mId = New Guid(e.CommandArgument.ToString)
                EditRecord(mId)
                txtTerm.Text = mCustomerTerm.Terms
                SetTitle()
                MarkLog(Util.Action.Edit, "CustomerTerm", mCustomerTerm.Terms, Util.ErrorType.NoError, mCustomerTerm.ID, EventLogID)
                upnlCustomerTerm.Update()
            Case "DeleteRec"
                mId = New Guid(e.CommandArgument.ToString)
              DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgCustomerTerm_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCustomerTerm.PageIndexChanging
        dgCustomerTerm.PageIndex = e.NewPageIndex
        dgCustomerTerm.DataSource = mCustomerTermList
        Session("mCustomerTermList") = mCustomerTermList
        dgCustomerTerm.DataBind()
    End Sub
    Private Sub dgCustomerTerm_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCustomerTerm.Sorting
        mCustomerTermList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCustomerTermList") = mCustomerTermList
        dgCustomerTerm.DataSource = mCustomerTermList
        dgCustomerTerm.DataBind()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "AccountHead", "", Util.ErrorType.NoError, mCustomerTerm.ID, EventLogID)
        DataFieldBind()
        SetTitle()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class