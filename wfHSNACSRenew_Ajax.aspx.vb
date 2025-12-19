Public Class wfHSNACSRenew_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mHSNACS As HSNACS
    Protected mHSNACSChild As HSNACSChild
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mHSNACS = Session("mHSNACS")
        mHSNACSChild = Session("mHSNACSChild")
    End Sub
    Private Sub SaveFormToObject()
        Try
            mHSNACSChild.FromDate = txtFromDate.Text
            mHSNACSChild.ToDate = CDate(txtFromDate.Text).AddDays(-1)
            mHSNACSChild.GSTPercent = CDec(Val(txtPercentage.Text))
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DataFieldBind()
        'txtFromDate.Text = mHSNACSChild.FromDateFormatted
        DataBind()
    End Sub
    Private Sub SetPage()
        If mHSNACSChild.IsNew Then
            lblTitle.Text = "HSN/ACS  [" + CType(mHSNACS.Code, String) + "]"
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        'Dim custValidator As CustomValidator
        'custValidator = CType(s, CustomValidator)

        'If custValidator.ControlToValidate = "txtDoneOnDate" Then

        '    If Len(txtDoneOnDate.Text.ToString) = 0 Then
        '        custValidator.ErrorMessage = " Please Select Done On Date."
        '        e.IsValid = False
        '    Else
        '        'Added By Utkarsh On 24-May-2011

        '        If Not mHSNACSChild.PreviousHSNACSChildID.Equals(Guid.Empty) Then
        '            Dim moldHSNACSChild As HSNACSChild
        '            moldHSNACSChild = HSNACSChild.GetHSNACSChild(mHSNACSChild.PreviousHSNACSChildID)
        '            If Not mHSNACSChild.DoneOnDate > CDate(moldHSNACSChild.DoneOnDate) Then
        '                custValidator.ErrorMessage = "Done On Date should be greater than Last Done On Date."
        '                e.IsValid = False
        '            Else
        '                e.IsValid = True
        '            End If
        '        Else
        '            '***************************************
        '            e.IsValid = True
        '        End If
        '    End If

        'ElseIf custValidator.ControlToValidate = "txtNo" Then
        '    If Len(txtNo.Text) > 50 Then
        '        custValidator.ErrorMessage = "Maximun Length of Calibration No. should be 50."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'ElseIf custValidator.ControlToValidate = "txtDoneByAgency" Then
        '    If Len(txtDoneByAgency.Text) > 150 Then
        '        custValidator.ErrorMessage = "Maximun Length of Note should be 150."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'ElseIf custValidator.ControlToValidate = "txtCertRef" Then
        '    If Len(txtCertRef.Text) > 100 Then
        '        custValidator.ErrorMessage = "Maximun Length of Note should be 100."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'ElseIf custValidator.ControlToValidate = "txtRemark" Then
        '    If Len(txtRemark.Text) > 1000 Then
        '        custValidator.ErrorMessage = "Remark should not be greater than 1000 characters"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then
                        If mHSNACSChild.IsValid Then
                            Save()
                            mHSNACSChild.ApplyEdit()
                            Dim mopenas As String = Request.QueryString("Type")
                            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                Exit Sub
                            End If
                        Else
                            If CustomValidate1() = False Then
                                upnlValidations.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Sub Save()
        mHSNACSChild = Session("mHSNACSChild")
        Try
            If mHSNACSChild.IsValid Then
                mHSNACSChild.ApplyEdit()
                mHSNACSChild = mHSNACSChild.Save()
                Session("mHSNACSChild") = mHSNACSChild
                SetPage()
                DataFieldBind()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        If Not Page.IsPostBack Then
            setFocus(txtFromDate)
            DataFieldBind()
        End If
        SetPage()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        mHSNACSChild = Session("mHSNACSChild")
        SaveFormToObject()
        Try
            If mHSNACSChild.IsValid Then
                mHSNACSChild.ApplyEdit()
                mHSNACSChild = mHSNACSChild.Save()
                Session("mHSNACSChild") = mHSNACSChild
                Dim mHSNACSDetail As String = mHSNACSChild.GSTPercent.ToString + " From Date : " + mHSNACSChild.FromDateFormatted
                MarkLog(Util.Action.Save, "HSNACS", mHSNACSDetail, Util.ErrorType.NoError, mHSNACSChild.ID, EventLogID)
                SetPage()
                DataFieldBind()

            End If
        Catch ex As Exception
        End Try
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        mHSNACSChild = Session("mHSNACSChild")
        SaveFormToObject()
        If mHSNACSChild.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            Exit Sub
        Else
            SaveFormToObject()
            Session("mHSNACSChild") = mHSNACSChild
            Session.Remove("mHSNACSChild")
            Session.Remove("mOldHSNACSChild")
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        If mHSNACSChild.IsValid = False Then
            For i As Integer = 0 To mHSNACSChild.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mHSNACSChild.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMsg.Trim <> "" Then
            cvItem.ErrorMessage = strMsg
            cvItem.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

End Class