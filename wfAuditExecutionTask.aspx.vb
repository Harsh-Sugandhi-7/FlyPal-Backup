
'Created By     :   Saylee
'Dated          :   5-Feb-2010
'Modified By    :   6-Apr-2010
Imports System.Text

Partial Class wfAuditExecutionTask
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
    Public mAuditExecution As AuditExecution
    Public mAuditExecutionTask As AuditExecutionTask
    Public mAuditCategoryList As AuditCategoryList
    Public mDepartmentList As AuditDepartmentList
    Public mTaskStatusList As TaskStatusList
#End Region

#Region " Buisness Method And Properties "
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        mAuditExecution = Session("mAuditExecution")
        mAuditCategoryList = Session("mAuditCategoryList")
    End Sub
    Private Sub SetSession()
        Session("mAuditExecution") = mAuditExecution
        Session("mAuditCategoryList") = mAuditCategoryList
    End Sub
    Private Function Setobject() As Boolean
        mAuditExecution.AuditExecutionTasks.CurrentItem.SrNo = mAuditExecution.AuditExecutionTasks.CurrentIndex + 1
        mAuditExecution.AuditExecutionTasks.CurrentItem.KindAttention = Trim(txtKindAttention.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.TaskStatusID = cmbTaskStatus.SelectedValue
    End Function
    Private Sub ControlVisibility()
        If Session("Edit") Then
            lblTitle.Text = "Audit Compliance Task [ " & mAuditExecution.AuditExecutionTasks.CurrentItem.AuditCategoryName & " ]"
        Else
            lblTitle.Text = "Audit Compliance Task [ New ]"
        End If
    End Sub
    Private Sub DeleteAuditExecutionTaskFinding(ByVal index As Int32)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Remove, SIMsgBox.Message_text.Remove, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
        Session("sender") = "DeleteAuditExecutionFindingTask"
        msg1.Show()
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentIndex = index
        Session("mAuditExecution") = mAuditExecution
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = 0
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "DeleteAuditExecutionFindingTask" Then
                        Try
                            Session("Sender") = ""
                            mAuditExecution = CType(Session("mAuditExecution"), AuditExecution)
                            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.Remove(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem)
                            Session("mAuditExecution") = mAuditExecution
                            Response.Redirect("wfAuditExecutionTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                                msg1.Show()
                            End If
                        End Try
                    ElseIf CType(Session("sender"), String) = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        If mAuditExecution.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If Save() Then
                                mAuditExecution = Session("mAuditExecution")
                                Setobject()
                                Session("mAuditExecution") = mAuditExecution
                                Session.Remove("Edit")
                                Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
                            End If
                        Else
                            Session.Remove("IsValid")
                            Response.Redirect("wfAuditExecution.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        End If
                    End If
                Case MsgBoxResult.No
                    If CType(Session("sender"), String) = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Session("mAuditExecution") = Session("AuditExecutionClone")
                        If mAuditExecution.AuditExecutionTasks.CurrentItem.IsNew And Not Session("Edit") = True Then mAuditExecution.AuditExecutionTasks.Remove(mAuditExecution.AuditExecutionTasks.CurrentItem)
                        Session.Remove("Edit")
                        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
                    Else
                        Session("Sender") = ""
                        Response.Redirect("wfAuditExecutionTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
                    End If
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfAuditExecutionTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
                Case Else
                    Session("Sender") = ""
                    Response.Redirect("wfAuditExecutionTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfAuditExecutionTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Function Save() As Boolean
        Setobject()
        If mAuditExecution.AuditExecutionTasks.Contains(mAuditExecution.AuditExecutionTasks.CurrentItem) Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Audit Compliance Task", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
            msg1.Show()
            mAuditExecution.CancelEdit()
            Exit Function
        Else
            'mAuditExecution.ApplyEdit()
            Session("mAuditExecution") = mAuditExecution
            Session.Remove("Edit")
            'Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
            Return True
        End If
    End Function
#End Region

#Region " DataBind Methods "

    Public Sub DataFieldBind()
        mAuditCategoryList = AuditCategoryList.GetAuditCategoryList("(SELECT)")
        cmbAuditCategory.DataSource = mAuditCategoryList
        Session("mAuditCategoryList") = mAuditCategoryList

        mDepartmentList = AuditDepartmentList.GetAuditDepartmentList("(SELECT)")
        cmbDepartment.DataSource = mDepartmentList
        Session("mDepartmentList") = mDepartmentList

        mTaskStatusList = TaskStatusList.GetTaskStatusList("(SELECT)")
        cmbTaskStatus.DataSource = mTaskStatusList
        Session("mTaskStatusList") = mTaskStatusList

        dgAuditExecutionTaskFinding.DataSource = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings

        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        Dim Index As Int32 = IIf(cmbAuditCategory.SelectedIndex <= 0, 0, cmbAuditCategory.SelectedIndex)
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "cmbAuditCategory" Then
            If cmbAuditCategory.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Please select the Task Category."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidator.ControlToValidate = "cmbDepartment" Then
            If cmbDepartment.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Please select the Department."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidator.ControlToValidate = "cmbTaskStatus" Then
            If cmbTaskStatus.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Please select the Task Status."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                CustValidator.ErrorMessage = "Note should not be greater than 1000 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 5000 Then
                CustValidator.ErrorMessage = "Description should not be greater than 5000 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub SetGrid()
        Dim P As Integer
        Dim lb As LinkButton 'ButtonColumn 
        For j As Integer = 0 To dgAuditExecutionTaskFinding.Items.Count - 1
            P = CType(Me.dgAuditExecutionTaskFinding.Items.Item(j).Cells(20).Text, Integer)
            If P <= 0 Then
                lb = CType(dgAuditExecutionTaskFinding.Items.Item(j).Cells(19).FindControl("LinkButton1"), LinkButton)
                lb.Enabled = False
            End If

            If (mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(j).IsNew) Or (mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(j).ToMailID.Trim = "" And mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(j).CCMailID.Trim = "") Then
                lb = CType(dgAuditExecutionTaskFinding.Items.Item(j).Cells(21).FindControl("lnkSendMail"), LinkButton)
                lb.Enabled = False
            End If
           
        Next
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            If txtKindAttention.Enabled = True Then
                setFocus(txtKindAttention)
            End If
            DataFieldBind()
        End If
        ControlVisibility()
        Session("mAuditExecution") = mAuditExecution
        SetGrid()
        MessageBoxResult()
    End Sub
    Private Sub imgbtnAuditCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnAuditCategory.Click
        Setobject()
        Response.Redirect("wfAuditCategory.aspx?BackPage2=wfAuditExecutionTask.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub imgbtnDepartment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgbtnDepartment.Click
        Setobject()
        Response.Redirect("wfAuditDepartment.aspx?BackPage2=wfAuditExecutionTask.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If (Not User.IsInRole("AuditExecutionNew") And Not User.IsInRole("AuditExecutionEdit")) Then
            ' ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If IsValid Then
            Setobject()
            If mAuditExecution.AuditExecutionTasks.Contains(mAuditExecution.AuditExecutionTasks.CurrentItem) Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Audit Compliance Task", MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
                mAuditExecution.CancelEdit()
                Exit Sub
            Else
                'mAuditExecution.ApplyEdit()
                Session("mAuditExecution") = mAuditExecution
                Session.Remove("Edit")
                If mAuditExecution.IsValid Then
                    ' mAuditExecution.ApplyEdit()

                    mAuditExecution = mAuditExecution.Save()
                    '' MarkLog(Util.Action.Save, "Audit Execution", "Audit Execution" + "-> " + mAuditExecution.AuditNo, Util.ErrorType.NoError, mAuditExecution.ID)
                    Session("mAuditExecution") = mAuditExecution
                    dgAuditExecutionTaskFinding.DataSource = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings
                    If txtKindAttention.Enabled = True Then
                        setFocus(txtKindAttention)
                    End If
                    GetSession()
                    DataFieldBind()
                    ControlVisibility()
                    SetGrid()
                End If
                'Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
            End If
        End If
    End Sub
    Private Sub btnAddExecutionTaskFinding_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddExecutionTaskFinding.Click
        'If (Not User.IsInRole("AuditExecutionNew") And mAuditExecution.IsNew) Or (Not User.IsInRole("AuditExecutionEdit") And Not mAuditExecution.IsNew) Then
        '    ' setObject()
        '    SetSession()
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        ''  If IsValid Then
        Setobject()
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.Add(mAuditExecution.AuditExecutionTasks.CurrentItem.ID)
        Session("mAuditExecution") = mAuditExecution
        Session("FindingEdit") = False
        Response.Redirect("wfAuditExecutionTaskFinding.aspx?BackPage2=wfAuditExecutionTask.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
        '' End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Setobject()
        If mAuditExecution.AuditExecutionTasks.CurrentItem.IsDirty Then 'Or mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.IsDirty Then
            Session("IsValid") = "True"
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
            msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
            Session("sender") = "Close"
            msg1.Show()
        Else
            If mAuditExecution.AuditExecutionTasks.CurrentItem.IsNew And Not Session("Edit") = True Then mAuditExecution.AuditExecutionTasks.Remove(mAuditExecution.AuditExecutionTasks.CurrentItem)
            Session.Remove("Edit")
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub

    Private Sub dgAuditExecutionTaskFinding_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAuditExecutionTaskFinding.ItemCommand
        Dim Index As Int32 = e.Item.ItemIndex + dgAuditExecutionTaskFinding.CurrentPageIndex * dgAuditExecutionTaskFinding.PageSize
        Select Case e.CommandName
            Case "Edit"
                'If (Not User.IsInRole("AuditExecutionView") And Not User.IsInRole("AuditExecutionEdit")) Then
                '    Setobject()
                '    SetSession()
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfAuditExecutionTask.aspx?BackPage2=wfAuditExecutionTask.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                Session("FindingEdit") = True
                ' mAuditExecution.BeginEdit()
                Setobject()
                mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentIndex = Index
                Session("FindingIndex") = Index
                Dim AuditExecutionClone As AuditExecution
                AuditExecutionClone = mAuditExecution.Clone
                Session("mAuditExecution") = mAuditExecution
                Session("AuditExecutionClone") = AuditExecutionClone
                Response.Redirect("wfAuditExecutionTaskFinding.aspx?BackPage2=wfAuditExecutionTask.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
            Case "Remove"
                ' If (Not User.IsInRole("AuditExecutionDelete")) Then
                'If (Not User.IsInRole("AuditExecutionNew") And mAuditExecution.IsNew) Or (Not User.IsInRole("AuditExecutionEdit") And Not mAuditExecution.IsNew) Then
                '    Setobject()
                '    SetSession()
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfAuditExecutionTask.aspx?BackPage2=wfAuditExecutionTask.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                DeleteAuditExecutionTaskFinding(Index)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentIndex = Index
                ''mAuditExecution = mAuditExecution.GetAuditExecution(New Guid(e.Item.Cells(0).Text))
                If mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(Index).ImageSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(Index).FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(Index).FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(Index).ImageFile, 0, mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(Index).ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        Dim Str As String
                        Str = "<script language=Javascript>openFile();</script>"
                        ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)
                    End If
                Else
                    'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfAuditExecutionTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                    'msg1.Show()
                End If
            Case "SendMail"
                GetSession()
                Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
                Dim mCompanyDetail As New CompanyDetail
                Dim da As New CSLA.Data.ObjectAdapter
                Dim mrptAuditFindings As rptAuditFindings
                Dim dsrptAuditFindings As New dsrptAuditFindings

                myReport = New crFindingReport
                Dim mUser As SI.UTILITY.User = SI.UTILITY.User.GetUser(HttpContext.Current.User.Identity.Name)
                mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                       mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                     mCompanyDetail.WebSite, "Audit Findings Report", "", "", "", mUser.EmployeeName, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode")) 'Changed By Utkarsh For Report Logo.


                '----------------------------------------------------------
                mrptAuditFindings = rptAuditFindings.GetrptAuditFindings("1/1/1900", "1/1/2100", mAuditExecution.AuditNo, , , mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings(Index).ID.ToString)

                If mrptAuditFindings.Count <= 0 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                    msg1.ReplacePage = "wfAuditExecutionTask.aspx?" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
                    msg1.Show()
                    Exit Sub
                End If
                '-----------Added by Utkarsh for Report Logo---------------
                Dim mrptImage As rptImage = rptImage.GetImage(dsrptAuditFindings)
                '----------------------------------------------------------
                da.Fill(dsrptAuditFindings, mrptAuditFindings)
                da.Fill(dsrptAuditFindings, Report)
                da.Fill(dsrptAuditFindings, mrptImage) 'Added by Utkarsh for Report Logo
                myReport.SetDataSource(dsrptAuditFindings)
                Session("CrystalReport") = myReport
                Dim str As New StringBuilder
                str.Append("Finding Details are as follows: ")
                str.Append("<p><b>Audit No.: </b> " & mrptAuditFindings(0).AuditNo & "</p>")
                str.Append("<p><b>Task Category: </b> " & mrptAuditFindings(0).AuditCategoryName & "</p>")
                str.Append("<p><b>Task Description: </b> " & mrptAuditFindings(0).Description & "</p>")

                Try
                    SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Finding Details", mrptAuditFindings(0).FindingNo, Info:=str.ToString, VendorEmailID:="", ToMailID:=mrptAuditFindings(0).ToMailID, CCMailID:=mrptAuditFindings(0).CCMailID, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)
                Catch ex As Exception
                    Dim Title As String = "Error Sending Mail"
                    Dim Message As String = ex.InnerException.ToString
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show(Title, Message))
                    Exit Sub
                End Try
        End Select
    End Sub
#End Region


End Class
