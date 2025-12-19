'Added by Vikrant
Partial Class wfCurrency_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents valError As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txt As System.Web.UI.WebControls.TextBox
    'Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents reset As System.Web.UI.WebControls.Button

    Protected WithEvents print As System.Web.UI.WebControls.ImageButton

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.

        InitializeComponent()



    End Sub

#End Region

#Region " Variable Declartation "
    Public mCurrency As Currency
    Public mCurrencyList As CurrencyList
    Dim Type As Int32

    'Added by Vikrant on 19-July-2011
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCurrency = Session("mCurrency")
        mCurrencyList = Session("mCurrencyList")
        Type = CType(Session("Type"), Int32)
    End Sub
    Private Sub SetSession()
        Session("mCurrency") = mCurrency
        Session("mCurrencyList") = mCurrencyList
        Session("Type") = Type
    End Sub
    Private Sub NewRecord()
        mCurrency = Currency.NewCurrency
        Session("mCurrency") = mCurrency
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mCurrency = Currency.GetCurrency(mId)
        Session("mCurrency") = mCurrency
        'New Addition By Yogita on 10-Dec-2007 to solve Bug No.CT3 given by pramod 
        setFocus(txtCurrencyName)
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mCurrency = Currency.GetCurrency(mId)
        Session("mCurrency") = mCurrency
    End Sub
    'Private Sub ClearControls()
    '    txtCurrencyName.Text = ""
    '    txtNameAfterDecimal.Text = ""
    '    txtSymbol.Text = ""
    '    txtConvFactor.Text = "0.00"
    '    'upnl()
    'End Sub
    Private Sub setObject()
        mCurrency.Name = txtCurrencyName.Text
        mCurrency.Symbol = txtSymbol.Text
        mCurrency.ConversionFactor = CDec(txtConvFactor.Text)
        mCurrency.NameAfterDecimal = txtNameAfterDecimal.Text
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
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
                            mCurrency = Session("mCurrency")
                            Currency.DeleteCurrency(mCurrency.ID)
                            NewRecord()
                            DataFieldBind()
                            upnlCurrencyDetails.Update()
                            SetTitle()
                            'Response.Redirect("wfCurresncy_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfCurrency_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfCurrency_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                'msg1.ReplacePage = "wfCurrency_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                                'msg1.Show()
                                MarkLog(Util.Action.Delete, "Currency", "Can't delete :" & mCurrency.Name & " is Currently in use", Util.ErrorType.NoError, mCurrency.ID, EventLogID)     'NA
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlCurrencyDetails.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Currency", mCurrency.Name, Util.ErrorType.NoError, mCurrency.ID, EventLogID) 'NA
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    NewRecord()
                    DataFieldBind()
                    upnlCurrencyDetails.Update()
                    'Response.Redirect("wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type)
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type)
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type)
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type)
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then    'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub EnabledDisableButtons()
        ' btnAdd.Enabled = User.IsInRole("CurrencyNew") Or User.IsInRole("CurrencyEdit") 'Add/Edit 
    End Sub
    Private Sub addAttributes()
        txtConvFactor.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtConvFactor').value,event)")
    End Sub
    Private Sub SetTitle()
        If mCurrency.IsNew Then
            lblTitle.Text = "Currency Information [New]"
        Else
            If Len(mCurrency.Name) > 15 Then
                lblTitle.Text = "Currency Information [" & mCurrency.Name.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Currency Information [" & mCurrency.Name & "]"
            End If
        End If
        'New Addition By Yogita on 10-Dec-2007
        lblResult.Text = "Currency List : " & mCurrencyList.Count & " Record(s) Found."
        upnlTitle.Update()
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Saylee 17-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerCurrency(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtNameAfterDecimal.Enabled = mTransCountAsPerMasters.Count = 0
            txtCurrencyName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCurrencyList = CurrencyList.GetCurrencyList("", "")
        dgCurrency.DataSource = mCurrencyList
        Session("mCurrencyList") = mCurrencyList
        DataBind()
        upnlGridView.Update()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "txtConvFactor" Then
            If IsNumeric(txtConvFactor.Text) Then
                If txtConvFactor.Text <= 0 Then
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            Else
                e.IsValid = False
            End If
        ElseIf CustValidator.ControlToValidate = "txtCurrencyName" Then
            If txtCurrencyName.Text.Trim.Length > 50 Then
                txtCurrencyName.Text = txtCurrencyName.Text.Trim.Substring(0, 46) + "..."
                e.IsValid = False
            End If
        End If

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        GetSession()
        addAttributes()

        ' new Added by Vikrant on 19-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then

            If txtCurrencyName.Enabled = True Then
                setFocus(txtCurrencyName)
            End If

            Type = CType(Val(Request.QueryString("Type")), Int32)
            Session("Type") = Type
            NewRecord()
            DataFieldBind()
            EnabledDisableButtons()

            'Added by Harsh on 15th July 2024 for FLYPAL 1745
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Currency") Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Mark As Favourite",
                                                    "MarkAsFavourite();",
                                                    True)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Remove From Favourite",
                                                    "RemoveFromFavourite();",
                                                    True)

            End If

        End If

        'New Addition By Yogita on 10-Dec-2007
        lblResult.Text = "Currency List : " & mCurrencyList.Count & " Record(s) Found."
        upnlGridView.Update()

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Currency", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'NA
        If Session("Type") = 1 Then
            Response.Redirect(Request.QueryString("BackPage1"))
        End If
        Session("MiddleFrame") = ""
        Session("sender") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'if Not Authenticated User and New User Then Message OR if Not Authenticated User for Edit and Not New User then Message  
        If (Not User.IsInRole("CurrencyNew") And mCurrency.IsNew) Or (Not User.IsInRole("CurrencyEdit") And Not mCurrency.IsNew) Then
            setObject()
            SetSession()
            MarkLog(Util.Action.Save, "Currency", User.Identity.Name & " is not Authorized User to save " & mCurrency.Name, Util.ErrorType.HandledError, mCurrency.ID, EventLogID)   'NA
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfCurrency_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                setObject()
                'Added By Utkarsh ON 07-Dec-2012 FOR ALL07122012
                If mCurrency.ConversionFactor.Equals(CDec(1)) Then
                    If mCurrency.IsNew Then
                        If mCurrencyList.Contains(mCurrency.ConversionFactor) Then
                            Dim str As String = "alert('Record with conversion factor 1 already present.');"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", str, True)
                            dgCurrency.DataSource = mCurrencyList
                            dgCurrency.DataBind()
                            upnlGridView.Update()
                            Exit Sub
                        End If
                    Else
                        If mCurrencyList.Contains(mCurrency.ConversionFactor, mCurrency.ID.ToString) Then
                            Dim str As String = "alert('Record with conversion factor 1 already present.');"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", str, True)
                            dgCurrency.DataSource = mCurrencyList
                            dgCurrency.DataBind()
                            upnlGridView.Update()
                            Exit Sub
                        End If
                    End If
                End If
                'End
                mCurrency.Save()
                If txtCurrencyName.Enabled = True Then
                    setFocus(txtCurrencyName)
                End If
                MarkLog(Util.Action.Save, "Currency", mCurrency.Name, Util.ErrorType.NoError, mCurrency.ID, EventLogID)     'NA
                mCurrency = Currency.NewCurrency
                DataFieldBind()
                SetSession()
                lblTitle.Text = "Currency Information [New]"
                'New Addition By Yogita on 10-Dec-2007
                lblResult.Text = "Currency List : " & mCurrencyList.Count & " Record(s) Found."
                upnlGridView.Update()
                upnlTitle.Update()

            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                End If
                NewRecord()
                DataFieldBind()
                upnlCurrencyDetails.Update()
            End Try
        Else
            upnlValidationSummary.Update()
            dgCurrency.DataSource = mCurrencyList
            dgCurrency.DataBind()
            upnlGridView.Update()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtCurrencyName.Enabled = True Then
            setFocus(txtCurrencyName)
        End If
        MarkLog(Util.Action.[New], "Currency", "", Util.ErrorType.NoError, mCurrency.ID, EventLogID)      'NA
        NewRecord()
        DataFieldBind()
        SetTitle()
    End Sub
    Private Sub dgCurrency_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCurrency.RowCommand
        Dim Idx As Int32
        Dim mId As Guid
        Dim mName As String
        dgCurrency.DataSource = mCurrencyList
        dgCurrency.DataBind()

        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("CurrencyView") And Not User.IsInRole("CurrencyEdit")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Edit, "Currency", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)  'NA
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Idx = CInt(e.CommandArgument) + dgCurrency.PageIndex * dgCurrency.PageSize
                mId = mCurrencyList(Idx).ID
                mName = mCurrencyList(Idx).Name

                EditRecord(mId)
                txtCurrencyName.DataBind()
                txtSymbol.DataBind()
                txtConvFactor.DataBind()
                txtNameAfterDecimal.DataBind()
                DisableName(mId) 'Added by : Saylee 17-Jun-2020, ALL16062020
                upnlCurrencyDetails.Update()
                MarkLog(Util.Action.Edit, "Currency", mCurrency.Name, Util.ErrorType.NoError, mCurrency.ID, EventLogID) 'NA
                SetTitle()
            Case "DeleteRec"
                If (Not User.IsInRole("CurrencyDelete")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Delete, "Currency", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)  'NA
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfCurrency_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Type
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Idx = CInt(e.CommandArgument) + dgCurrency.PageIndex * dgCurrency.PageSize
                mId = mCurrencyList(Idx).ID
                mName = mCurrencyList(Idx).Name
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgCurrency_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCurrency.PageIndexChanging
        dgCurrency.PageIndex = e.NewPageIndex
        dgCurrency.DataSource = mCurrencyList
        Session("mVendorList") = mCurrencyList
        dgCurrency.DataBind()
    End Sub
    Private Sub dgCurrency_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCurrency.Sorting
        mCurrencyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCurrencyList") = mCurrencyList
        dgCurrency.DataSource = mCurrencyList
        dgCurrency.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1745
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "Currency")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "Currency")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    'End

#End Region

End Class
