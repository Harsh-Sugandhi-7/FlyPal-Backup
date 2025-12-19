'Added By Vikrant on 22-Oct-2012 For ALL22102012-1

Partial Class wfSelectPartStatusForPartType
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPartTypeList As PartTypeList
    Public mPartStatusList As PartStatusList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mPartTypeList = Session("mPartTypeList")
        mPartStatusList = Session("mPartStatusList")
    End Sub
    Private Sub SetSession()
        Session("mPartTypeList") = mPartTypeList
        Session("mPartStatusList") = mPartStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartTypeList")
        Session.Remove("mPartStatusList")
    End Sub
    Private Sub SetColorLabel()
        Dim labelColor As Label
        For i As Integer = 0 To dgItemTypeList.Items.Count - 1
            labelColor = dgItemTypeList.Items(i).FindControl("lblColor")
            If mPartTypeList.Item(i).Color = "" Then
                labelColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
            Else
                labelColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#" & mPartTypeList.Item(i).Color)
            End If
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
                    If CType(Session("sender"), String) = "Continue1" Then
                        Try
                            Session("sender") = ""
                            Dim msg1 As New SIMsgBox(Page, "Alert!", "You are going to update ATA Chapter(s) .<BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
                            msg1.ReplacePage = "wfSelectPartStatusForPartType.aspx?BackPage=" & Request.QueryString("BackPage")
                            Session("sender") = "Continue2"
                            'ControlVisibility(SearchIndex)
                            msg1.Show()
                        Catch ex As SqlException

                        Finally

                        End Try
                    ElseIf CType(Session("sender"), String) = "Continue2" Then
                        Try
                            Session("sender") = ""
                            ''ControlVisibility(cmbSearch.SelectedIndex, cmbDate.SelectedIndex)
                            Response.Redirect("wfSelectPartStatusForPartType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException

                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfSelectPartStatusForPartType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    Response.Redirect("wfSelectPartStatusForPartType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Response.Redirect("wfSelectPartStatusForPartType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfSelectPartStatusForPartType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub ControlVisibility()
        For i As Integer = 0 To dgItemTypeList.Items.Count - 1
            Dim cmbValue As DropDownList

            cmbValue = CType(Me.dgItemTypeList.Items(i).FindControl("cmbPartStatusList"), DropDownList)
            If cmbValue.SelectedIndex <= 0 Then
                btnUpdate.Enabled = False
                btnUpdateBottom.Enabled = False
                Exit Sub
            Else
                btnUpdate.Enabled = True
                btnUpdateBottom.Enabled = True
            End If
        Next
    End Sub
    Private Function IsSelectedIndex() As Boolean
        Dim i As Integer = 0
        Dim cmbValue As DropDownList
        For i = 0 To dgItemTypeList.Items.Count - 1
            cmbValue = CType(dgItemTypeList.Items(i).FindControl("cmbPartStatusList"), DropDownList)
            If cmbValue.SelectedIndex = 0 Then
                Return True
                'Exit Function
            Else
                Return False
            End If
        Next
    End Function
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If IsSelectedIndex() = True Then
            e.IsValid = False
            custValidator.ErrorMessage = "Select part status"
        Else
            e.IsValid = True
        End If
        'End If
    End Sub
#End Region

#Region " DataFieldBind "
    Public Sub GridBind()
        mPartTypeList = PartTypeList.GetPartTypeList()
        dgItemTypeList.DataSource = mPartTypeList
        Session("mPartTypeList") = mPartTypeList

        mPartStatusList = PartStatusList.GetPartStatusList(True)
        Session("mPartStatusList") = mPartStatusList

        DataBind()
        lblResult.Text = "Part Type List :" & mPartTypeList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfSelectPartStatusForPartType.aspx?"
            GridBind()
        End If
        SetColorLabel()
        ControlVisibility()
        MessageBoxResult()
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click, btnUpdateBottom.Click
        If IsValid Then
            Dim cnt As Integer = 0
            Dim mPartType As PartType
            For i As Integer = 0 To dgItemTypeList.Items.Count - 1
                Dim cmbValue As DropDownList
                cmbValue = CType(Me.dgItemTypeList.Items(i).FindControl("cmbPartStatusList"), DropDownList)
                mPartType = PartType.GetPartType(CInt(dgItemTypeList.Items(i).Cells(0).Text))
                mPartType.ID = CInt(dgItemTypeList.Items(i).Cells(0).Text)
                mPartType.PartStatusID = cmbValue.SelectedValue
                Try
                    If mPartType.IsDirty Then
                        MarkLog(Util.Action.Save, "PartStatus", "Part Type : " + mPartType.Name + " Changed By : " + User.Identity.Name + " Status : " + cmbValue.SelectedItem.ToString, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    End If
                    mPartType.Save()
                    cnt += 1
                Catch ex As Exception
                    Throw ex.GetBaseException
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Error In Updating Part Status."))
                End Try
            Next
            If cnt > 0 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Part Status Updated Successfully."))
                Session("MiddleFrame") = ""
                Response.Redirect("index.aspx")
            End If
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        'MarkLog(Util.Action.Close, "Change Part ATA", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Web.Security.FormsAuthentication.SignOut()
        Session.Remove("MenuID")
        Session.Remove("MiddleFrame")
        MarkLog(Util.Action.Logoff)
        'Drop all the references to the Principal.
        Thread.CurrentPrincipal = Nothing
        Dim str As String
        str = "<script language=javascript>  window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
    End Sub
    Private Sub dgItemTypeList_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgItemTypeList.SortCommand
        mPartTypeList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartTypeList") = mPartTypeList
        dgItemTypeList.DataSource = mPartTypeList
        dgItemTypeList.DataBind()
    End Sub
    Private Sub dgItemTypeList_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgItemTypeList.PageIndexChanged
        dgItemTypeList.CurrentPageIndex = e.NewPageIndex
        dgItemTypeList.DataSource = mPartTypeList
        Session("mPartTypeList") = mPartTypeList
        dgItemTypeList.DataBind()
    End Sub
    Protected Sub cmbPartStatusList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ControlVisibility()
    End Sub
#End Region

End Class