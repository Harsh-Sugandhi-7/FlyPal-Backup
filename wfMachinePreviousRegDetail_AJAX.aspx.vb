Public Class wfMachinePreviousRegDetail_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mRegNo As String
    Public mAttachToID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mRegNo = CType(Session("mRegNo"), String)
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
    End Sub
    'Added By Vikrant On 26-Jun-2014
    Private Sub RemoveSession()
        Session.Remove("mRegNo")
        Session.Remove("mAttachToID")
        Session.Remove("mMachinePreviousRegDetailEdit")
    End Sub
    'End
    Private Sub NewRecord()
    End Sub
    Private Sub SetObject()
        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).RegNo = txtRegNo.Text
        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).MachineID = mMachine.ID

        If calStartDate.Text = "" Then
            mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).StartDate = System.DBNull.Value
        Else
            mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).StartDate = calStartDate.Text
        End If

        If calEndDate.Text = "" Then
            mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).EndDate = System.DBNull.Value
        Else
            mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).EndDate = calEndDate.Text
        End If

        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).StartTSN = txtStartTSN.Text
        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).EndTSN = txtEndTSN.Text
        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).StartCycle = Val(txtStartCycle.Text)
        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).EndCycle = Val(txtEndCycle.Text)
        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).[Operator] = txtOperator.Text.Trim
        mMachine.MachinePreviousRegDetails.Item(mMachine.MachinePreviousRegDetails.CurrentIndex).Country = txtCountry.Text.Trim
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
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
                            mMachine.MachinePreviousRegDetails.Remove(mMachine.MachinePreviousRegDetails(mMachine.MachinePreviousRegDetails.CurrentIndex))
                            For i As Integer = 0 To mMachine.MachinePreviousRegDetails.Count - 1
                                mMachine.MachinePreviousRegDetails(i).SerialNo = i + 1
                            Next
                            Session("mMachine") = mMachine
                            Session("mMachinePreviousRegDetailEdit") = False
                            dgPrevRegList.DataSource = mMachine.MachinePreviousRegDetails
                            dgPrevRegList.DataBind()
                            ClearControls()
                            lblAircraftPreviousRegistrationDetails.InnerText = "Aircraft Previous Registration Details [NEW]"
                            SetPage()
                            upnlGridPrevRegList.Update()
                            upnlPrevReg.Update()
                            ' upnlTitle.Update()
                            upnlResult.Update()
                        Catch ex As SqlException
                            If ex.Number = 8114 Or ex.Number = 8115 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
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
                    ' Response.Redirect("wfMachinePreviousRegDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok
                    Session("sender") = ""

                    DataFieldBind()
                    ' Response.Redirect("wfMachinePreviousRegDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    ' Response.Redirect("wfMachinePreviousRegDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            ' Response.Redirect("wfMachinePreviousRegDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        txtRegNo.Text = mMachine.MachinePreviousRegDetails.Item(ID).RegNo
        calStartDate.Text = mMachine.MachinePreviousRegDetails.Item(ID).StartDateFormatted.ToString
        calEndDate.Text = mMachine.MachinePreviousRegDetails.Item(ID).EndDateFormatted.ToString
        txtStartTSN.Text = mMachine.MachinePreviousRegDetails.Item(ID).StartTSN
        txtEndTSN.Text = mMachine.MachinePreviousRegDetails.Item(ID).EndTSN
        txtStartCycle.Text = mMachine.MachinePreviousRegDetails.Item(ID).StartCycle
        txtEndCycle.Text = mMachine.MachinePreviousRegDetails.Item(ID).EndCycle
        txtOperator.Text = mMachine.MachinePreviousRegDetails.Item(ID).[Operator]
        txtCountry.Text = mMachine.MachinePreviousRegDetails.Item(ID).Country
    End Sub
    Private Sub SetPage()
        'If mMachine.IsNew Then
        '    lblTitle.Text = "Aircraft [New]"
        'Else
        '    lblTitle.Text = "Aircraft [" & mMachine.RegNo & "]"
        'End If
        lblResult.Text = "List of Aircraft Previous Registration Details: " & mMachine.MachinePreviousRegDetails.Count & " Record(s) found"
    End Sub
    Private Sub ControlVisibility()
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        If Not mMachine.IsValid Then
            For i As Integer = 0 To mMachine.MachinePreviousRegDetails.CurrentItem.GetBrokenRulesCollection.Count - 1
                strMSG = strMSG + mMachine.MachinePreviousRegDetails.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMSG.Trim <> "" Then
            cvStartDate.ErrorMessage = strMSG
            cvStartDate.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub addAttributes()
        txtStartTSN.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtStartTSN').value,event)")
        txtEndTSN.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEndTSN').value,event)")
        txtStartCycle.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtStartCycle').value,event)")
        txtEndCycle.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEndCycle').value,event)")
    End Sub
    Private Sub ClearControls()
        txtRegNo.Text = ""
        txtCountry.Text = ""
        txtEndCycle.Text = ""
        txtEndTSN.Text = ""
        txtOperator.Text = ""
        txtStartCycle.Text = ""
        txtStartTSN.Text = ""
        calStartDate.Text = ""
        calEndDate.Text = ""
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        If Session("mMachinePreviousRegDetailEdit") = True Then
            mAttachToID = Session("mAttachToID")
            EditRecord(mAttachToID)
        End If
        dgPrevRegList.DataSource = mMachine.MachinePreviousRegDetails
        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtStartTSN" Then
            If calStartDate.Text = "" Then
                custValidator.ErrorMessage = "Start date required"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If custValidator.ControlToValidate = "txtStartCycle" Then
            If calEndDate.Text = "" Then
                custValidator.ErrorMessage = "End date required"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            If txtRegNo.Enabled = True Then
                setFocus(txtRegNo)
            End If
            DataFieldBind()
            SetPage()
            ControlVisibility()
            ClearControls()
            lblAircraftPreviousRegistrationDetails.InnerText = "Aircraft Previous Registration Details [NEW]"
        End If
    End Sub
   
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            'setObject()
            SetSession()
            'MarkLog(Util.Action.[New], "Machine", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If Not IsValid Then upnlValidationSummary.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True) : Exit Sub

        If Session("mMachinePreviousRegDetailEdit") = False Then

            'MarkLog(Util.Action.[New], "Machine", " Aircraft Name ->" & mMachine.RegNo & " Certificate No. -> " & Trim(txtNo.Text) & "  Certificate Name -> " & txtName.Text, Util.ErrorType.NoError, Guid.Empty)
            mMachine.MachinePreviousRegDetails.Add(txtRegNo.Text.Trim, mMachine.ID, calStartDate.Text, calEndDate.Text, Val(txtStartTSN.Text), Val(txtEndTSN.Text), Val(txtStartCycle.Text), Val(txtEndCycle.Text), txtOperator.Text.Trim, txtCountry.Text.Trim)
            If Not CustomValidate1() Then
                mMachine.MachinePreviousRegDetails.Remove(mMachine.MachinePreviousRegDetails.CurrentItem)
                upnlValidationSummary.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                Exit Sub
            End If

            For i As Integer = 0 To mMachine.MachinePreviousRegDetails.Count - 1
                mMachine.MachinePreviousRegDetails(i).SerialNo = i + 1
            Next
            Session("mMachine") = mMachine
            setFocus(txtRegNo)
            DataFieldBind()
            ClearControls()
            lblAircraftPreviousRegistrationDetails.InnerText = "Aircraft Previous Registration Details [NEW]"
            SetPage()

            ControlVisibility()
            upnlGridPrevRegList.Update()
            upnlPrevReg.Update()
            'upnlTitle.Update()
            upnlResult.Update()
            upnlPrevReg.Update()
            'Response.Redirect("wfMachinePreviousRegDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

        Else
            SetObject()
            If Not CustomValidate1() Then
                upnlValidationSummary.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
                Exit Sub
            End If
            Session("mMachine") = mMachine
            SetFocus(txtRegNo)
            Session("mMachinePreviousRegDetailEdit") = False
            'Response.Redirect("wfMachinePreviousRegDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            DataFieldBind()
            SetPage()
            ClearControls()
            lblAircraftPreviousRegistrationDetails.InnerText = "Aircraft Previous Registration Details [NEW]"
            ControlVisibility()
            upnlGridPrevRegList.Update()
            upnlPrevReg.Update()
            'upnlTitle.Update()
            upnlResult.Update()
            upnlPrevReg.Update()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub dgPrevRegList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPrevRegList.RowCommand
      
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPrevRegList.PageSize * dgPrevRegList.PageIndex
                mRegNo = mMachine.MachinePreviousRegDetails(Index).RegNo
                Session("mRegNo") = mRegNo
                'If (Not User.IsInRole("MachineDelete")) Then
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    'MarkLog(Util.Action.Delete, "Machine", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If


                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                'msg.ReplacePage = "wfMachinePreviousRegDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                'Session("sender") = "Delete"
                'msg.Show()
                mMachine.MachinePreviousRegDetails.CurrentIndex = Index
                Session("mMachine") = mMachine
                upnlValidationSummary.Update()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
              
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPrevRegList.PageSize * dgPrevRegList.PageIndex
                mRegNo = mMachine.MachinePreviousRegDetails(Index).RegNo
                mMachine.MachinePreviousRegDetails.CurrentIndex = index
                Dim mID As Guid = mMachine.MachinePreviousRegDetails(Index).ID
                mAttachToID = mID
                EditRecord(mID)
                setFocus(txtRegNo)
                dgPrevRegList.DataSource = mMachine.MachinePreviousRegDetails
                DataBind()
                Session("mMachinePreviousRegDetailEdit") = True
                Session("mAttachToID") = mAttachToID
                Session("mMachine") = mMachine

                lblAircraftPreviousRegistrationDetails.InnerText = "Aircraft Previous Registration Details" & " [" & mMachine.MachinePreviousRegDetails.Item(mID).RegNo & "]"

                upnlGridPrevRegList.Update()
                upnlPrevReg.Update()
                upnlValidationSummary.Update()
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        RemoveSession()
        ' Response.Redirect("wfMachine.aspx?BackPage=" & Request.QueryString("BackPage"))
         ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class