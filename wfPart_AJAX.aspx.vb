'AJAX Created By Saylee On 27-Apr-2015

Public Class wfPart_AJAX
    Inherits System.Web.UI.Page
#Region " Variable Declarations "
    Public mPartList As PartList
    Public mPart As Part
    Dim Type As Int32
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub SetSession()
        Session("mPart") = mPart
        Session("mPartList") = mPartList
    End Sub
    Private Sub GetSession()
        mPart = Session("mPart")
        mPartList = Session("mPartList")
    End Sub
    Private Sub NewRecord()
        mPart = Part.NewPart(Guid.NewGuid)
        'txtPartNo.Text = ""
        'txtDescription.Text = ""
        'txtPartNoSearch.Text = ""
        'txtSearchDesc.Text = ""
        Session("mPart") = mPart
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mPart = Part.GetPart(mId)
        Session("mPart") = mPart
        If mPart.CountForPartUse > 0 Then 'Added By Prashant On 5-May-2021 ALL05052021
            txtPartNo.Enabled = False
        Else
            txtPartNo.Enabled = True
        End If
        setFocus(txtPartNo)   'by Saylee on 10thDec to solve bug-PT1 from Aircraft Master given by Pramod
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfPart.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPart = Part.GetPart(mId)
        Session("mPart") = mPart
    End Sub
    Private Sub SetObject()
        mPart.Name = txtPartNo.Text.Trim
        mPart.Description = txtDescription.Text.Trim
        Session("mPart") = mPart
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
                            mPart = Session("mPart")
                            Part.DeletePart(mPart.ID)
                            NewRecord()
                            DataFieldBind()
                            txtPartNoSearch.Text = ""
                            txtSearchDesc.Text = ""
                            txtPartNo.Text = ""
                            txtDescription.Text = ""
                            upnlPartOnfo.Update()
                            upnlResult.Update()
                            upnlGrid.Update()
                            upnlSearch.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    NewRecord()
                    DataFieldBind()
                    txtPartNoSearch.Text = ""
                    txtSearchDesc.Text = ""
                    txtPartNo.Text = ""
                    txtDescription.Text = ""
                    upnlPartOnfo.Update()

                    upnlResult.Update()
                    upnlGrid.Update()
                    upnlSearch.Update()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    NewRecord()
                    DataFieldBind()
                    txtPartNoSearch.Text = ""
                    txtSearchDesc.Text = ""
                    txtPartNo.Text = ""
                    txtDescription.Text = ""
                    upnlPartOnfo.Update()

                    upnlResult.Update()
                    upnlGrid.Update()
                    upnlSearch.Update()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            upnlResult.Update()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
            upnlResult.Update()
        End If
    End Sub
  
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        mPartList = PartList.GetPartList
        Session("mPartList") = mPartList
        dgPart.DataSource = mPartList
        DataBind()

        lblResult.Text = "Part List : " & mPartList.Count & " Record(s) Found."
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 200 Then
                custValidator.ErrorMessage = "Max Length of Description Should be 200 chars."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            If txtPartNo.Enabled = True Then
                setFocus(txtPartNo)
            End If
            NewRecord()
            DataFieldBind()
            lblResult.Text = "Part List : " & mPartList.Count & " Record(s) Found."

        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        MarkLog(Util.Action.Close, "Part", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("Sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

        Response.Redirect(Request.QueryString("GChildPage3") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("MachineNew") And mPart.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mPart.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Util.Action.Save, "Part", User.Identity.Name & " is not Authorized User to save " & mPart.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                mPart.Save()
                If txtPartNo.Enabled = True Then
                    setFocus(txtPartNo)
                End If
                MarkLog(Util.Action.Save, "Part", mPart.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                NewRecord()
                mPartList = Session("mPartList")
                DataFieldBind()
                SetSession()
                lbltitle.Text = "Part [New]"
                upnlCloseTop.Update()
                upnlResult.Update()
                txtPartNo.Text = ""
                txtDescription.Text = ""
                txtPartNoSearch.Text = ""
                txtSearchDesc.Text = ""
                txtPartNo.Enabled = True  'Added By Prashant On 5-May-2021 ALL05052021
                upnlPartOnfo.Update()
                upnltitle.Update()
                upnlGrid.Update()
                upnlSearch.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                End If
            End Try
        End If
    End Sub
    Private Sub dgPart_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPart.RowCommand


        Select Case e.CommandName
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPart.PageSize * dgPart.PageIndex
                Dim mId As Guid = mPartList(Index).ID
                Dim mName As String = mPartList(Index).Name

                If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                    SetObject()
                    SetSession()
                    'MarkLog(Util.Action.Edit, "Part", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecord(mId)
                txtPartNo.DataBind()
                txtDescription.DataBind()

                MarkLog(Util.Action.Edit, "Part", mPart.Name, Util.ErrorType.NoError, mPart.ID, EventLogID)
                If Len(mPart.Name) > 15 Then
                    lbltitle.Text = "Part[" & mPart.Name.Substring(0, 15) & "...]"
                Else
                    lbltitle.Text = "Part[" & mPart.Name & "]"
                End If
                upnlPartOnfo.Update()
                upnltitle.Update()
                upnlGrid.Update()
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPart.PageSize * dgPart.PageIndex
                Dim mId As Guid = mPartList(Index).ID
                Dim mName As String = mPartList(Index).Name
                If (Not User.IsInRole("MachineDelete")) Then
                    SetObject()
                    SetSession()
                    MarkLog(Util.Action.Delete, "Part", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

                    Exit Sub
                End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mPartList = PartList.GetPartList(txtPartNoSearch.Text, txtSearchDesc.Text.Trim)
        dgPart.DataSource = mPartList
        Session("mPartList") = mPartList
        DataBind()
        lblResult.Text = "Part List : " & mPartList.Count & " Record(s) Found."

        upnlGrid.Update()
        upnlCloseTop.Update()
        upnlResult.Update()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "Part", "", Util.ErrorType.NoError, mPart.ID, EventLogID)
        'Commented by Saylee to solve bug-PT2 of Aircraft Master given by Pramod
        'DataFieldBind()
        txtPartNo.DataBind()
        txtDescription.DataBind()
        txtPartNo.Enabled = True  'Added By Prashant On 5-May-2021 ALL05052021
        If txtPartNo.Enabled = True Then
            setFocus(txtPartNo)
        End If
        lbltitle.Text = "Part [New]"
        upnltitle.Update()
        upnlPartOnfo.Update()
        upnlValidationSummary.Update()
    End Sub
    'Added By Prashant 19-June-2009 for grid sorting
    Private Sub dgPart_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPart.Sorting
        mPartList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartList") = mPartList
        dgPart.DataSource = mPartList
        dgPart.DataBind()
        upnlGrid.Update()
    End Sub
    '------------------------------------------------
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region



End Class