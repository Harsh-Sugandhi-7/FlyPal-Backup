Imports System.Web.UI.WebControls
Partial Class wfWebConfigSettings
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
    Public mConfigurationKey As ConfiguartionKey
    Public mConfigurationKeys As ConfiguartionKeys
    Public BackPage As String
    Public Flag As Integer = 0
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mConfigurationKey = Session("mConfigurationKey")
        mConfigurationKeys = Session("mConfigurationKeys")
    End Sub
    Private Sub SetSession()
        Session("mConfigurationKeys") = mConfigurationKeys
        Session("mConfigurationKey") = mConfigurationKey
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub NewRecord()
        mConfigurationKey = ConfiguartionKey.NewConfigurationKey
        Session("mConfigurationKey") = mConfigurationKey
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mConfigurationKey = ConfiguartionKey.GetConfigurationKey(mID)
        Session("mConfigurationKey") = mConfigurationKey
    End Sub

    Private Sub SetObject()
        mConfigurationKey.Name = Trim(txtName.Text)
        mConfigurationKey.Value_Options = Trim(txtOption.Text)
        mConfigurationKey.Value = Trim(txtValue.Text)
    End Sub
    Private Sub setobject1()
        Dim i As Integer
       Dim txtValue1 As TextBox
        mConfigurationKey.Name = Trim(txtName.Text)
        mConfigurationKey.Value_Options = Trim(txtOption.Text)
        For i = 0 To dgConfigurationKeys.Items.Count - 1
            txtValue1 = CType(Me.dgConfigurationKeys.Items(i).FindControl("txtKeyValue"), TextBox)
            'txtNameValue1 = CType(Me.dgConfigurationKeys.Items(i).Cells(1).Text, TextBox)
            'dgConfigurationKeys.Items(i).Cells(1).Text()

            mConfigurationKey.Value = txtValue1.Text
        Next

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Session("sender") = ""
                            mConfigurationKey = Session("mConfigurationKey")
                            ConfiguartionKey.DeleteConfiguarationKey(mConfigurationKey.ID)
                            Response.Redirect("wfWebConfigSettings.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                                MarkLog(Flypal.Util.Action.Delete, "City", "Can't delete :" & mConfigurationKey.Name & " is Currently in use", Flypal.Util.ErrorType.NoError, mConfigurationKey.ID, EventLogID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then

                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            Response.Redirect("wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub

    Public Sub EnableDisableButton()

        If Flag = 1 Then
            lblName.Visible = True
            lblValue.Visible = True
            lblOption.Visible = True
            txtName.Visible = True
            txtValue.Visible = True
            txtOption.Visible = True
            lblKeyDetails.Visible = True
            btnAdd.Visible = True
            lblSave.Visible = True

        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub DataFieldBind()
        mConfigurationKeys = ConfiguartionKeys.GetConfiguartionKeys()
        dgConfigurationKeys.DataSource = mConfigurationKeys
        dgConfigurationKeys.DataBind()
        Session("mConfigurationKeys") = mConfigurationKeys
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtName" Then
            If Len(Trim(txtName.Text)) > 50 Then
                CustValid.ErrorMessage = " Key Name too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If CustValid.ControlToValidate = "txtValue" Then
            Dim arr() As String
            Dim str As String = txtOption.Text
            arr = str.Split(",")
            If Len(Trim(txtValue.Text)) > 50 Then
                CustValid.ErrorMessage = "Value too long "
                e.IsValid = False
            ElseIf (Array.IndexOf(arr, txtValue.Text) = -1) Then
                CustValid.ErrorMessage = "Key value must be amongst options."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If

        If CustValid.ControlToValidate = "txtOption" Then
            If Len(Trim(txtName.Text)) > 500 Then
                CustValid.ErrorMessage = " Options too long "
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
        If Not IsPostBack And Session("sender") = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            NewRecord()
            DataFieldBind()
            'Else
            '    dgConfigurationKeys.DataSource = mConfigurationKeyList
            '    dgConfigurationKeys.DataBind()
        End If
        If mConfigurationKeys.Count > 25 Then
            btnBackTop.Visible = True
        Else
            btnBackTop.Visible = False
        End If
        EnableDisableButton()
        MessageBoxResult()
        SetSession()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If IsValid Then
            Try
                SetObject()
                mConfigurationKey.Save()

                Flag = 1
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                NewRecord()
                txtName.DataBind()
                txtOption.DataBind()
                txtValue.DataBind()
                DataFieldBind()
                SetSession()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    Session("sender") = "Delete"
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    Session("sender") = "Delete"
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfWebConfigSettings.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage3=" & Request.QueryString("ChildPage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    Session("sender") = "Delete"
                    msg1.Show()
                End If
            End Try
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Flag = 1
        EnableDisableButton()
        setFocus(txtName)
        NewRecord()
        txtName.Text = ""
        txtOption.Text = ""
        txtValue.Text = ""
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Session("sender") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgConfigurationKeys_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgConfigurationKeys.SortCommand
        mConfigurationKeys.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgConfigurationKeys.DataSource = mConfigurationKeys
        dgConfigurationKeys.DataBind()
        Session("mConfigurationKeys") = mConfigurationKeys
    End Sub

    Private Sub dgConfigurationKeys_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgConfigurationKeys.ItemCommand
        Select Case e.CommandName
            Case "Edit"
                Flag = 1
                EnableDisableButton()
                Dim mID As New Guid(e.Item.Cells(0).Text)
                EditRecord(mID)
                txtName.DataBind()
                txtOption.DataBind()
                txtValue.DataBind()
                setFocus(txtName)
        End Select
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Page.Validate()
        If IsValid Then
            Dim TextBox As TextBox
            Dim i As Integer
            Dim Count As Integer = 0
            For i = 0 To dgConfigurationKeys.Items.Count - 1
                TextBox = CType(dgConfigurationKeys.Items.Item(i).Cells(2).FindControl("txtKeyValue"), TextBox)
                If TextBox.Text <> "" Then
                    Dim ID As New Guid(dgConfigurationKeys.Items.Item(i).Cells(0).Text)
                    Dim Name As String = dgConfigurationKeys.Items.Item(i).Cells(1).Text
                    Dim Value As String = CType(dgConfigurationKeys.Items.Item(i).Cells(2).FindControl("txtKeyValue"), TextBox).Text
                    Dim ValueOptions As String = dgConfigurationKeys.Items.Item(i).Cells(3).Text
                    ConfiguartionKeys.UpdateItem(ID, Name, Value, ValueOptions)
                End If
            Next

            Dim path As String = Server.MapPath("WebConfig.txt")
            Dim sw As New StreamWriter(path, False)
            'mConfigurationKeyList = Session("mConfigurationKeyList")
            mConfigurationKeys = ConfiguartionKeys.GetConfiguartionKeys()
            Try
                Dim j As Integer
                For j = 0 To mConfigurationKeys.Count - 1
                    Dim key As String = mConfigurationKeys(j).Name
                    Dim value As String = mConfigurationKeys(j).Value
                    Dim appsetting As String = "<add key=""TempDir""" + " value=" + """" + value + """>" + "</add>"
                    appsetting = "<add key=" + """" + key + """" + " value=" + """" + value + """>" + "</add>"
                    sw.WriteLine(appsetting)
                Next
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("File Written Successfully!!"))
            Catch ex As Exception
                Throw ex.GetBaseException
            Finally
                sw.Close()

            End Try
        End If

    End Sub


#End Region

End Class
