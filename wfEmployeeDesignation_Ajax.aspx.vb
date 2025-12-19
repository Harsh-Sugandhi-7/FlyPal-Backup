'AJAX Conversion By Vikrant

Partial Class wfEmployeeDesignation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeDesignation As EmployeeDesignation
    Public mDesignationList As DesignationList
    Public BackPage As String
    Public DesignationID As Guid

    'Added by Saylee on 13-Jan-2010
    ''  Public mEmployeeDesgSalaryList As EmployeeDesgSalaryList
    Public mEmployeeDesgSalary As EmployeeDesgSalary
    Public mSalaryHeadList As SalaryHeadList
    Public mEmployeeDesgSalarys As EmployeeDesgSalarys

    Public mEmployeeDesgAllowance As EmployeeDesgAllowance
    Public mAllowanceList As AllowanceList
    Public mEmployeeDesgAllowances As EmployeeDesgAllowances

    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
    Dim mAllowance As Allowance
    Dim mSalaryHeads As SalaryHead
    Dim mSalaryHeadsList As SalaryHeadList
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeDesignation = Session("mEmployeeDesignation")
        'mEmployeeDesignationList = Session("mEmployeeDesignationList")
        mDesignationList = Session("mDesignationList")

        'Added by Saylee on 13-Jan-2010
        '' mEmployeeDesgSalaryList = Session("mEmployeeDesgSalaryList")
        mSalaryHeadList = Session("mSalaryHeadList")
        mAllowanceList = Session("mAllowanceList")
        mAllowance = Session("mAllowance")
        mSalaryHeads = Session("mSalaryHeads")
        mSalaryHeadsList = Session("mSalaryHeadsList")
    End Sub
    Private Sub ControlVisibilityForDesgAttachment()
        If mEmployeeDesignation.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
        upnlDesgDetails.Update()
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDesignation") = mEmployeeDesignation
        'Session("mEmployeeSkillList") = mEmployeeSkillList
        Session("mDesignationList") = mDesignationList
        Session("mEmployee") = mEmployee

        'Added by Saylee on 13-Jan-2010
        '' Session("mEmployeeDesgSalaryList") = mEmployeeDesgSalaryList
        Session("mSalaryHeadList") = mSalaryHeadList
        Session("mAllowanceList") = mAllowanceList
    End Sub
    Private Sub SetSessionSalaryHeads()
        Session("mSalaryHeads") = mSalaryHeads
        Session("mSalaryHeadsList") = mSalaryHeadsList
    End Sub
    Private Sub SetObjectAllowance()
        mAllowance.Name = txtAllowanceName.Text
        mAllowance.Code = txtAllowanceCode.Text
    End Sub
    Private Sub DataFieldBind()
        mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
        cmbDesignationList.DataSource = mDesignationList
        Session("mDesignationList") = mDesignationList
        txtDate.Text = mEmployeeDesignation.DateFormatted.ToString

        'mEmployeeDesignation = Session("mEmployeeDesignation") CHK

        'Added by Saylee on 13-Jan-2010
        mSalaryHeadList = SalaryHeadList.GetSalaryHeadList("(SELECT)")
        cmbSalaryHeadList.DataSource = mSalaryHeadList
        Session("mSalaryHeadList") = mSalaryHeadList

        ''mEmployeeDesgSalaryList = EmployeeDesgSalaryList.GetEmployeeDesgSalaryList(mEmployeeDesignation.ID)
        ''dgEmployeeDesgSalaryList.DataSource = mEmployeeDesgSalaryList
        ''Session("mEmployeeDesgSalaryList") = mEmployeeDesgSalaryList
        dgEmployeeDesgSalaryList.DataSource = mEmployeeDesignation.EmployeeDesgSalarys

        If Not mEmployeeDesignation.EmployeeDesgSalarys Is Nothing Then
            txtTotalValue.Text = mEmployeeDesignation.EmployeeDesgSalarys.TotalValue
        End If

        mAllowanceList = AllowanceList.GetAllowanceList("(SELECT)")
        cmbAllowanceList.DataSource = mAllowanceList
        Session("mAllowanceList") = mAllowanceList
        dgEmployeeDesgAllowanceList.DataSource = mEmployeeDesignation.EmployeeDesgAllowances

        If Not mEmployeeDesignation.EmployeeDesgAllowances Is Nothing Then
            txtTotalAllowanceValue.Text = mEmployeeDesignation.EmployeeDesgAllowances.TotalValue
        End If
        '===============================================================
        'DataBind() CHK
        upnlDesgDetails.DataBind()
        upnlSalaryDetails.DataBind()
        upnlAllowanceDetails.DataBind()
        'End
    End Sub
    Private Sub DataFieldBindSalaryHeads()
        mSalaryHeadsList = SalaryHeadList.GetSalaryHeadList()
        dgSalaryHeads.DataSource = mSalaryHeadsList
        Session("mSalaryHeadsList") = mSalaryHeadsList
        upnlSalaryHeads.DataBind()
    End Sub
    Private Sub DataFieldBindAllowance()
        mAllowanceList = AllowanceList.GetAllowanceList()
        dgAllowance.DataSource = mAllowanceList
        Session("mAllowanceList") = mAllowanceList
        upnlAllowance.DataBind()
    End Sub
    Private Sub addAttributes()
        txtValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtValue').value,event)")
        txtAllowanceValue.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAllowanceValue').value,event)")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteSalaryHeads" Then
                        Try
                            Session("sender") = ""
                            Dim Index As Int16 = CType(Session("Index"), Integer)
                            mEmployeeDesignation.EmployeeDesgSalarys.Remove(Index)
                            dgEmployeeDesgSalaryList.DataSource = mEmployeeDesignation.EmployeeDesgSalarys
                            'dgEmployeeDesgSalaryList.DataBind()
                            'txtTotalValue.DataBind()
                            cmbSalaryHeadList.SelectedIndex = 0
                            txtValue.Text = "0.0"
                            Session("EmployeeDesgSalaryEdit") = "False"
                            upnlSalaryDetails.DataBind()
                            upnlSalaryDetails.Update()
                            'Response.Redirect("wfEmployeeDesignation_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                               MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then 'Added by Saylee on 22-Apr-2009
                                MSGBoxCtrl.show("Delete Alert!", ex.Message, "Because its FDTL/Log Entry has been done.", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Flypal.Util.Action.Delete, "EmployeeSkill", mEmployeeSkill.Name, Flypal.Util.ErrorType.NoError, mEmployeeSkill.ID, EventLogID)
                            End If
                        End Try
                    End If

                    If MSGBoxCtrl.Sender = "DeleteAllowance" Then
                        Try

                            Session("sender") = ""
                            Dim Index As Int16 = CType(Session("Index"), Integer)
                            mEmployeeDesignation.EmployeeDesgAllowances.Remove(Index)
                            dgEmployeeDesgAllowanceList.DataSource = mEmployeeDesignation.EmployeeDesgAllowances
                            'dgEmployeeDesgAllowanceList.DataBind()
                            'txtTotalAllowanceValue.DataBind()
                            cmbAllowanceList.SelectedIndex = 0
                            txtAllowanceValue.Text = "0.0"
                            Session("EmployeeDesgAllowanceEdit") = "False"
                            upnlAllowanceDetails.DataBind()
                            upnlAllowanceDetails.Update()
                            'Response.Redirect("wfEmployeeDesignation_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then 'Added by Saylee on 22-Apr-2009
                                MSGBoxCtrl.show("Delete Alert!", ex.Message, "Because its FDTL/Log Entry has been done.", MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Flypal.Util.Action.Delete, "EmployeeSkill", mEmployeeSkill.Name, Flypal.Util.ErrorType.NoError, mEmployeeSkill.ID, EventLogID)
                            End If
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteSalaryHeadsMaster" Then
                        Try
                            Session("sender") = ""
                            SalaryHead.DeleteSalaryHead(mSalaryHeads.ID)
                            NewRecordSalaryHeads()
                            txtSalaryHeadCode.Text = ""
                            txtSalaryHeadName.Text = ""
                            lblTitleSalaryHeads.Text = "Salary Head Information [New]"
                            DataFieldBindSalaryHeads()
                            upnlSalaryHeads.Update()
                            'Response.Redirect("wfAllowance.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordSalaryHeads()
                            txtSalaryHeadCode.Text = ""
                            txtSalaryHeadName.Text = ""
                            lblTitleSalaryHeads.Text = "Salary Head Information [New]"
                            DataFieldBindSalaryHeads()
                            upnlSalaryHeads.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Flypal.Util.Action.Delete, "Allowance", mAllowance.Name, Flypal.Util.ErrorType.NoError, mAllowance.ID)
                            End If
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteAllowanceMaster" Then
                        Try
                            Session("sender") = ""
                            'mAllowance = Session("mAllowance") CHK
                            Allowance.DeleteAllowance(mAllowance.ID)
                            NewRecordAllowance()
                            txtAllowanceName.Text = ""
                            txtAllowanceCode.Text = ""
                            lblTitleAllowance.Text = "Allowance Information [New]"
                            DataFieldBindAllowance()
                            upnlAllowance.Update()
                            'Response.Redirect("wfAllowance.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordAllowance()
                            txtAllowanceName.Text = ""
                            txtAllowanceCode.Text = ""
                            lblTitleAllowance.Text = "Allowance Information [New]"
                            DataFieldBindAllowance()
                            upnlAllowance.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Flypal.Util.Action.Delete, "Allowance", mAllowance.Name, Flypal.Util.ErrorType.NoError, mAllowance.ID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteAllowanceMaster" Then
                        'Session("sender") = ""
                        NewRecordAllowance()
                        txtAllowanceName.Text = ""
                        txtAllowanceCode.Text = ""
                        lblTitleAllowance.Text = "Allowance Information [New]"
                        dgAllowance.DataSource = mAllowanceList
                        upnlAllowance.DataBind()
                        upnlAllowance.Update()
                    End If
                    If MSGBoxCtrl.Sender = "DeleteSalaryHeadsMaster" Then
                        'Session("sender") = ""
                        NewRecordSalaryHeads()
                        txtSalaryHeadCode.Text = ""
                        txtSalaryHeadName.Text = ""
                        lblTitleSalaryHeads.Text = "Salary Head Information [New]"
                        dgSalaryHeads.DataSource = mSalaryHeadsList
                        upnlSalaryHeads.DataBind()
                        upnlSalaryHeads.Update()
                    End If
                    Session("sender") = ""
                    'Response.Redirect("wfEmployeeDesignation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeDesignation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind() CHK
                    'Response.Redirect("wfEmployeeDesignation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'DataFieldBind()
            'Response.Redirect("wfEmployeeDesignation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mEmployeeDesignation.IsNew Then
            lblTitle.Text = "Employee Designation Information [New]"
        Else
            If Len(mEmployeeDesignation.DesignationName) > 15 Then
                lblTitle.Text = "Employee Designation Information [" & mEmployeeDesignation.DesignationName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Designation Information [" & mEmployeeDesignation.DesignationName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub SetObject()
        mEmployeeDesignation.EmployeeID = mEmployee.ID
        mEmployeeDesignation.DesignationID = New Guid(cmbDesignationList.SelectedValue)
        mEmployeeDesignation.Date = CType(txtDate.Text, Object)
        mEmployeeDesignation.IsPromoted = chkPromoted.Checked
        mEmployeeDesignation.Remark = Trim(txtRemark.Text)

        Session("mEmployeeDesignation") = mEmployeeDesignation
    End Sub
    Private Sub SetObjectSalaryHeads()
        mSalaryHeads.Name = txtSalaryHeadName.Text
        mSalaryHeads.Code = txtSalaryHeadCode.Text
    End Sub
    Private Sub AttachMyFile()
        Try
            mEmployeeDesignation.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeDesignation.ImageSize = Session("FileUpload.FileSize")
            mEmployeeDesignation.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeDesignation") = mEmployeeDesignation
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForDesgAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecordAllowance()
        mAllowance = Allowance.NewAllowance()
        Session("mAllowance") = mAllowance
    End Sub
    Private Sub NewRecordSalaryHeads()
        mSalaryHeads = SalaryHead.NewSalaryHead()
        Session("mSalaryHeads") = mSalaryHeads
    End Sub
    Private Sub EditRecordAllowance(ByVal mID As Guid)
        mAllowance = Allowance.GetChildAllowance(mID)
        Session("mAllowance") = mAllowance
        setFocus(txtAllowanceCode)
    End Sub
    Private Sub EditRecordSalaryHeads(ByVal mID As Guid)
        mSalaryHeads = SalaryHead.GetChildSalaryHead(mID)
        Session("mSalaryHeads") = mSalaryHeads
        setFocus(txtSalaryHeadCode)
    End Sub
    Private Sub DeleteRecordAllowance(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteAllowanceMaster")
        mAllowance = Allowance.GetChildAllowance(mID)
        Session("mAllowance") = mAllowance
    End Sub
    Private Sub DeleteRecordSalaryHeads(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteSalaryHeadsMaster")
        mSalaryHeads = SalaryHead.GetChildSalaryHead(mID)
        Session("mSalaryHeads") = mSalaryHeads
    End Sub
    Private Sub SaveSalaryHeads()
        SetObjectSalaryHeads()
        If Not mSalaryHeads.IsValid Then Exit Sub

        Try
            mSalaryHeads.Save()
            If txtSalaryHeadCode.Enabled = True Then
                setFocus(txtSalaryHeadCode)
            End If
            'MarkLog(Flypal.Util.Action.Save, "SalaryHeads", mSalaryHeads.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty)
            NewRecordSalaryHeads()
            txtSalaryHeadName.Text = ""
            txtSalaryHeadCode.Text = ""
            DataFieldBindSalaryHeads()
            lblTitleSalaryHeads.Text = "Salary Head Information [New]"
            upnlSalaryHeads.Update()
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
            NewRecordSalaryHeads()
            txtSalaryHeadName.Text = ""
            txtSalaryHeadCode.Text = ""
            DataFieldBindSalaryHeads()
            lblTitleSalaryHeads.Text = "Salary Head Information [New]"
            upnlSalaryHeads.Update()
        End Try
    End Sub
    Private Sub SaveAllowance()
        SetObjectAllowance()
        If Not mAllowance.IsValid Then Exit Sub

        Try
            mAllowance.Save()
            If txtAllowanceCode.Enabled = True Then
                setFocus(txtAllowanceCode)
            End If
            'MarkLog(Flypal.Util.Action.Save, "Allowance", mAllowance.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty)
            NewRecordAllowance()
            txtAllowanceName.Text = ""
            txtAllowanceCode.Text = ""
            DataFieldBindAllowance()
            lblTitleAllowance.Text = "Allowance Information [New]"
            upnlAllowance.Update()
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
            NewRecordAllowance()
            txtAllowanceName.Text = ""
            txtAllowanceCode.Text = ""
            DataFieldBindAllowance()
            lblTitleAllowance.Text = "Allowance Information [New]"
            upnlAllowance.Update()
        End Try
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbDesignationList.Enabled = True Then
                setFocus(cmbDesignationList)
            End If
            DataFieldBind()
            SetTitle()
            ControlVisibilityForDesgAttachment()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeView") And mEmployeeDesignation.IsNew) Or (Not User.IsInRole("EmployeeEdit") And Not mEmployeeDesignation.IsNew) Then
            SetObject()
            SetSession()
            'MarkLog(Flypal.Util.Action.Save, "EmployeeDesignation", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MarkLog(Flypal.Util.Action.Save, "Employee Designation", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Designation : " + mEmployeeDesignation.DesignationName, Flypal.Util.ErrorType.HandledError, mEmployeeDesignation.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim clnEmployeeDesignation As EmployeeDesignation
        If IsValid Then
            Try

                clnEmployeeDesignation = CType(mEmployeeDesignation.Clone, EmployeeDesignation)

                SetObject()
                mEmployeeDesignation.Save()
                'Added by Amrita on 17-Amrita to set topmost designation
                mEmployee = Employee.GetEmployee(mEmployeeDesignation.EmployeeID)
                Session("mEmployee") = mEmployee
                '---------------

                'If calDate.Enabled = True Then
                '    setFocus(calDate)
                'End If
                'MarkLog(Flypal.Util.Action.Save, "EmployeeService", mEmployeeService.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty)
                MarkLog(Flypal.Util.Action.Save, "Employee Designation", "Emp : " + mEmployee.EmpNoName + " Designation : " + mEmployeeDesignation.DesignationName, Flypal.Util.ErrorType.HandledError, mEmployee.ID, EventLogID)
                'NewRecord()
                ''txtName.DataBind()
                'calDate.DataBind()
                'cmbServiceList.DataBind()
                'DataFieldBind()
                SetSession()
                lblTitle.Text = "Employee Designation Information [New]"

                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    'RemoveSession() CHK which values to remove
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 50000 Then
                    mEmployeeDesignation = clnEmployeeDesignation
                    Session("mEmployeeDesignation") = mEmployeeDesignation
                    MSGBoxCtrl.show("Save Alert!", ex.Message, "Because its FDTL/Log Entry has been done.", MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    Private Sub imgDesignation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgDesignation.Click
        'SetObject() 'Added Code
        'Response.Redirect("wfDesignation.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeDesignation_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()

        If Not mEmployeeDesignation.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Designation", "Emp : " + mEmployee.EmpNoName + " Designation : " + mEmployeeDesignation.DesignationName, Flypal.Util.ErrorType.NoError, mEmployeeDesignation.ID, EventLogID)
        End If
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            'RemoveSession() CHK which values to remove
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployeeDesignation.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeDesignation.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDesignation.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployeeDesignation.ImageFile, 0, mEmployeeDesignation.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mEmployeeDesignation.ImageFile = file1
        mEmployeeDesignation.ImageSize = 0
        mEmployeeDesignation.FileExtension = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    Private Sub btnAddSalaryHead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSalaryHead.Click
        Dim i As Integer
        Dim TotalValue As Decimal = 0

        If Not IsValid Then Exit Sub
        If cmbSalaryHeadList.SelectedIndex <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please Select Salary Head", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        SetObject()
        If Session("EmployeeDesgSalaryEdit") = "True" Then
            mEmployeeDesignation.ApplyEdit()
            Dim index As Int16 = Session("Index")
            If mEmployeeDesignation.EmployeeDesgSalarys.Contains(New Guid(cmbSalaryHeadList.SelectedValue), "") Then
                If mEmployeeDesignation.EmployeeDesgSalarys.Contains(mEmployeeDesignation.EmployeeDesgSalarys(index).ID, New Guid(cmbSalaryHeadList.SelectedValue)) Then
                    mEmployeeDesignation.EmployeeDesgSalarys(index).SalaryHeadID = New Guid(cmbSalaryHeadList.SelectedValue)
                    mEmployeeDesignation.EmployeeDesgSalarys(index).Value = txtValue.Text
                    Session("EmployeeDesgSalaryEdit") = "False"
                Else
                    Session("IsContains") = "True"
                End If
            Else
                mEmployeeDesignation.EmployeeDesgSalarys(index).SalaryHeadID = New Guid(cmbSalaryHeadList.SelectedValue)
                mEmployeeDesignation.EmployeeDesgSalarys(index).Value = txtValue.Text
                Session("EmployeeDesgSalaryEdit") = "False"
            End If
            Session("EmployeeDesgSalaryEdit") = "False"
        Else
            If mEmployeeDesignation.EmployeeDesgSalarys Is Nothing Then mEmployeeDesignation.EmployeeDesgSalarys = EmployeeDesgSalarys.NewEmployeeDesgSalarys
            If Not mEmployeeDesignation.EmployeeDesgSalarys.Contains(New Guid(cmbSalaryHeadList.SelectedValue), "") Then

                mEmployeeDesignation.EmployeeDesgSalarys.Add(mEmployeeDesignation.ID)
                ''mEmployeeDesignation.BeginEdit()
                mEmployeeDesignation.EmployeeDesgSalarys.CurrentItem.SalaryHeadID = New Guid(cmbSalaryHeadList.SelectedValue)
                mEmployeeDesignation.EmployeeDesgSalarys.CurrentItem.Value = Trim(txtValue.Text)
                '' mEmployeeDesignation.ApplyEdit()
            Else
                Session("IsContains") = "True"
            End If
        End If


        For i = 0 To mEmployeeDesignation.EmployeeDesgSalarys.Count - 1
            TotalValue = TotalValue + mEmployeeDesignation.EmployeeDesgSalarys(i).Value
        Next
        txtTotalValue.Text = TotalValue

        mEmployeeDesignation.EmployeeDesgSalarys.TotalValue = TotalValue
        cmbSalaryHeadList.DataSource = mSalaryHeadList
        Session("mSalaryHeadList") = mSalaryHeadList
        cmbSalaryHeadList.DataBind()
        Session("mEmployeeDesignation") = mEmployeeDesignation
        dgEmployeeDesgSalaryList.DataSource = mEmployeeDesignation.EmployeeDesgSalarys
        dgEmployeeDesgSalaryList.DataBind()
        txtValue.Text = "0.0"
        txtTotalValue.DataBind()

        If Session("IsContains") = "True" Then
            Session("IsContains") = "False"
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Salary Head List", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfEmployeeDesignation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1")
            'Session("sender") = ""
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Salary Head List", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub imgSalaryHead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgSalaryHead.Click
        mdlPopUpSalaryHeads.Show()
        NewRecordSalaryHeads()
        DataFieldBindSalaryHeads()
        upnlSalaryHeads.Update()
        'SetObject() 'Added Code
        'Response.Redirect("wfSalaryHeads.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeDesignation_Ajax.aspx")
    End Sub
    Private Sub dgEmployeeDesgSalaryList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmployeeDesgSalaryList.RowCommand
        Dim Index As Int16
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgEmployeeDesgSalaryList.PageSize * dgEmployeeDesgSalaryList.PageIndex
                mID = CType(dgEmployeeDesgSalaryList.DataKeys(Index).Value, Guid)

                Session("Index") = Index
                mEmployeeDesignation.EmployeeDesgSalarys.CurrentIndex = Index
                cmbSalaryHeadList.SelectedValue = mEmployeeDesignation.EmployeeDesgSalarys(Index).SalaryHeadID.ToString
                txtValue.Text = mEmployeeDesignation.EmployeeDesgSalarys(Index).Value
                cmbSalaryHeadList.DataBind()
                txtValue.DataBind()
                setFocus(cmbSalaryHeadList)
                Session("EmployeeDesgSalaryEdit") = "True"
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgEmployeeDesgSalaryList.PageSize * dgEmployeeDesgSalaryList.PageIndex
                mID = CType(dgEmployeeDesgSalaryList.DataKeys(Index).Value, Guid)

                Session("Index") = Index
                mEmployeeDesignation.EmployeeDesgSalarys.CurrentIndex = Index
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteSalaryHeads")
        End Select
    End Sub
    Private Sub btnAddAllowance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAllowance.Click
        Dim i As Integer
        Dim TotalValue As Decimal = 0

        If Not IsValid Then Exit Sub

        If cmbAllowanceList.SelectedIndex <= 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please Select Allowance Head", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfEmployeeDesignation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1")
            'Session("sender") = ""
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please Select Allowance Head", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        SetObject()
        If Session("EmployeeDesgAllowanceEdit") = "True" Then
            mEmployeeDesignation.ApplyEdit()
            Dim index As Int16 = Session("Index")
            If mEmployeeDesignation.EmployeeDesgAllowances.Contains(New Guid(cmbAllowanceList.SelectedValue), "") Then
                If mEmployeeDesignation.EmployeeDesgAllowances.Contains(mEmployeeDesignation.EmployeeDesgAllowances(index).ID, New Guid(cmbAllowanceList.SelectedValue)) Then
                    mEmployeeDesignation.EmployeeDesgAllowances(index).AllowanceID = New Guid(cmbAllowanceList.SelectedValue)
                    mEmployeeDesignation.EmployeeDesgAllowances(index).Value = txtAllowanceValue.Text
                    Session("EmployeeDesgAllowanceEdit") = "False"
                Else
                    Session("IsContains") = "True"
                End If
            Else
                mEmployeeDesignation.EmployeeDesgAllowances(index).AllowanceID = New Guid(cmbAllowanceList.SelectedValue)
                mEmployeeDesignation.EmployeeDesgAllowances(index).Value = txtAllowanceValue.Text
                Session("EmployeeDesgAllowanceEdit") = "False"
            End If
            Session("EmployeeDesgAllowanceEdit") = "False"
        Else
            If mEmployeeDesignation.EmployeeDesgAllowances Is Nothing Then mEmployeeDesignation.EmployeeDesgAllowances = EmployeeDesgAllowances.NewEmployeeDesgAllowances
            If Not mEmployeeDesignation.EmployeeDesgAllowances.Contains(New Guid(cmbAllowanceList.SelectedValue), "") Then

                mEmployeeDesignation.EmployeeDesgAllowances.Add(mEmployeeDesignation.ID)
                '' mEmployeeDesignation.BeginEdit()
                mEmployeeDesignation.EmployeeDesgAllowances.CurrentItem.AllowanceID = New Guid(cmbAllowanceList.SelectedValue)
                mEmployeeDesignation.EmployeeDesgAllowances.CurrentItem.Value = Trim(txtAllowanceValue.Text)
                '' mEmployeeDesignation.ApplyEdit()
            Else
                Session("IsContains") = "True"
            End If
        End If


        For i = 0 To mEmployeeDesignation.EmployeeDesgAllowances.Count - 1
            TotalValue = TotalValue + mEmployeeDesignation.EmployeeDesgAllowances(i).Value
        Next
        txtTotalAllowanceValue.Text = TotalValue

        mEmployeeDesignation.EmployeeDesgAllowances.TotalValue = TotalValue
        cmbAllowanceList.DataSource = mAllowanceList
        Session("mAllowanceList") = mAllowanceList
        cmbAllowanceList.DataBind()
        Session("mEmployeeDesignation") = mEmployeeDesignation
        dgEmployeeDesgAllowanceList.DataSource = mEmployeeDesignation.EmployeeDesgAllowances
        dgEmployeeDesgAllowanceList.DataBind()
        txtAllowanceValue.Text = "0.0"
        txtTotalAllowanceValue.DataBind()

        If Session("IsContains") = "True" Then
            Session("IsContains") = "False"
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Allowance List", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfEmployeeDesignation_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1")
            'Session("sender") = ""
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Allowance List", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub imgAllowance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgAllowance.Click
        SetObject() 'Added Code
        NewRecordAllowance()
        DataFieldBindAllowance()
        mdlPopUpAllowance.Show()
        upnlAllowance.Update()
        'Response.Redirect("wfAllowance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeDesignation_Ajax.aspx")
    End Sub
    Private Sub dgEmployeeDesgAllowanceList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmployeeDesgAllowanceList.RowCommand
        Dim Index As Int16
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgEmployeeDesgAllowanceList.PageSize * dgEmployeeDesgAllowanceList.PageIndex
                mID = New Guid(dgEmployeeDesgAllowanceList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                Session("Index") = Index
                mEmployeeDesignation.EmployeeDesgAllowances.CurrentIndex = Index
                cmbAllowanceList.SelectedValue = mEmployeeDesignation.EmployeeDesgAllowances(Index).AllowanceID.ToString
                txtAllowanceValue.Text = mEmployeeDesignation.EmployeeDesgAllowances(Index).Value
                cmbAllowanceList.DataBind()
                txtAllowanceValue.DataBind()
                setFocus(cmbAllowanceList)
                Session("EmployeeDesgAllowanceEdit") = "True"
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgEmployeeDesgAllowanceList.PageSize * dgEmployeeDesgAllowanceList.PageIndex
                mID = CType(dgEmployeeDesgAllowanceList.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                Session("Index") = Index
                mEmployeeDesignation.EmployeeDesgAllowances.CurrentIndex = Index
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteAllowance")
        End Select
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlDesgDetails.Update()
    End Sub
    Private Sub hdnimgBtnDesignation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnDesignation.Click
        mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
        cmbDesignationList.DataSource = mDesignationList
        cmbDesignationList.DataBind()
        Session("mDesignationList") = mDesignationList
        upnlDesgDetails.Update()
    End Sub
#End Region

#Region "Salary Heads"
    Private Sub btnSaveSalaryHeads_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSalaryHeads.Click
        If (Not User.IsInRole("SalaryHeadsNew") And mSalaryHeads.IsNew) Or (Not User.IsInRole("SalaryHeadsEdit") And Not mSalaryHeads.IsNew) Then
            SetObjectSalaryHeads()
            Session("mSalaryHeads") = mSalaryHeads
        End If
        If IsValid Then
            SaveSalaryHeads()
        End If
    End Sub
    Private Sub dgSalaryHeads_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSalaryHeads.RowCommand
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                mID = CType(dgSalaryHeads.DataKeys(CInt(e.CommandArgument)).Value, Guid)

                EditRecordSalaryHeads(mID)
                txtSalaryHeadName.Text = mSalaryHeads.Name
                txtSalaryHeadCode.Text = mSalaryHeads.Code

                'MarkLog(Flypal.Util.Action.Edit, "SalaryHeads", mSalaryHeads.Name, Flypal.Util.ErrorType.NoError, mSalaryHeads.ID)
                If Len(mSalaryHeads.Name) > 15 Then
                    lblTitleSalaryHeads.Text = "Salary Head Information [" & mSalaryHeads.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleSalaryHeads.Text = "Salary Head Information [" & mSalaryHeads.Name & " ]"
                End If
                If txtSalaryHeadName.Enabled = True Then
                    setFocus(txtSalaryHeadCode)
                End If
            Case "DeleteRec"
                mID = CType(dgSalaryHeads.DataKeys(CInt(e.CommandArgument)).Value, Guid)

                DeleteRecordSalaryHeads(mID)
        End Select
    End Sub
    Private Sub btnCloseSalaryHeads_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseSalaryHeads.Click
        Session.Remove("mSalaryHeads")
        Session.Remove("mSalaryHeadsList")

        txtSalaryHeadName.Text = ""
        txtSalaryHeadCode.Text = ""

        mSalaryHeadList = SalaryHeadList.GetSalaryHeadList("(SELECT)")
        cmbSalaryHeadList.DataSource = mSalaryHeadList
        cmbSalaryHeadList.DataBind()
        Session("mSalaryHeadList") = mSalaryHeadList

        mdlPopUpSalaryHeads.Hide()
        upnlSalaryDetails.Update()
    End Sub
    Private Sub btnNewSalaryHeads_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSalaryHeads.Click
        NewRecordSalaryHeads()
        txtSalaryHeadCode.Text = ""
        txtSalaryHeadName.Text = ""
        lblTitleSalaryHeads.Text = "Salary Head Information [New]"
    End Sub
#End Region

#Region "Allowance"
    Private Sub btnSaveAllowance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAllowance.Click
        If (Not User.IsInRole("AllowanceNew") And mAllowance.IsNew) Or (Not User.IsInRole("AllowanceEdit") And Not mAllowance.IsNew) Then
            SetObjectAllowance()
            Session("mAllowance") = mAllowance
        End If
        If IsValid Then
            SaveAllowance()
        End If
    End Sub
    Private Sub dgAllowance_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAllowance.RowCommand
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                mID = CType(dgAllowance.DataKeys(CInt(e.CommandArgument)).Value, Guid)

                EditRecordAllowance(mID)
                txtAllowanceCode.Text = mAllowance.Code
                txtAllowanceName.Text = mAllowance.Name

                If Len(mAllowance.Name) > 15 Then
                    lblTitleAllowance.Text = "Allowance Information [" & mAllowance.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleAllowance.Text = "Allowance Information [" & mAllowance.Name & " ]"
                End If
                If txtAllowanceName.Enabled = True Then
                    setFocus(txtAllowanceCode)
                End If
            Case "DeleteRec"
                mID = CType(dgAllowance.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                DeleteRecordAllowance(mID)
        End Select
    End Sub
    Private Sub btnCloseAllowance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseAllowance.Click
        Session.Remove("mAllowance")
        Session.Remove("mAllowanceList")

        txtAllowanceCode.Text = ""
        txtAllowanceName.Text = ""

        mAllowanceList = AllowanceList.GetAllowanceList("(SELECT)")
        cmbAllowanceList.DataSource = mAllowanceList
        cmbAllowanceList.DataBind()
        Session("mAllowanceList") = mAllowanceList

        mdlPopUpAllowance.Hide()
        upnlAllowanceDetails.Update()
    End Sub
    Private Sub btnNewAllowance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewAllowance.Click
        NewRecordAllowance()
        txtAllowanceName.Text = ""
        txtAllowanceCode.Text = ""
        lblTitleAllowance.Text = "Allowance Information [New]"
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub dgSalaryHeads_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgSalaryHeads.PageIndexChanging
        dgSalaryHeads.PageIndex = e.NewPageIndex
        dgSalaryHeads.DataSource = mSalaryHeadsList
        Session("mSalaryHeadsList") = mSalaryHeadsList
        dgSalaryHeads.DataBind()
    End Sub

    Private Sub dgAllowance_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgAllowance.PageIndexChanging
        dgAllowance.PageIndex = e.NewPageIndex
        dgAllowance.DataSource = mAllowanceList
        Session("mAllowanceList") = mAllowanceList
        dgAllowance.DataBind()
    End Sub
#End Region

End Class
