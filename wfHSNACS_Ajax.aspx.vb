Public Class wfHSNACS_Ajax
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
            mHSNACS.Code = txtCode.Text
            mHSNACS.Description = txtDescription.Text
            mHSNACSChild.HSNACSID = mHSNACS.ID
            mHSNACSChild.FromDate = txtFromDate.Text
            mHSNACSChild.GSTPercent = CDec(Val(txtPercentage.Text))
            Session("mHSNACS") = mHSNACS
            Session("mHSNACSChild") = mHSNACSChild
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DataFieldBind()
        mHSNACS = Session("mHSNACS")
        mHSNACSChild = Session("mHSNACSChild")
        txtFromDate.Text = mHSNACSChild.FromDateFormatted
        DataBind()
    End Sub
    Private Sub SetPage()
        If Not mHSNACS.IsNew Then
            lblTitle.Text = Session("HSNACS") + "HSN/SAC Item  [" + CType(mHSNACSChild.GSTPercent, String) + "]"
        Else
            lblTitle.Text = Session("HSNACS") + "HSN/SAC [New]"
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtFromDate" Then
            If Len(txtFromDate.Text.ToString) = 0 Then
                custValidator.ErrorMessage = " Please Select Done On Date."
                e.IsValid = False
            End If
        End If
    End Sub
    Private Sub Save()
        Try
            mHSNACS = Session("mHSNACS")
            mHSNACSChild = Session("mHSNACSChild")
            'SaveFormToObject()
            If mHSNACS.IsValid Then
                mHSNACS.ApplyEdit()

                mHSNACS = mHSNACS.Save()
                If mHSNACSChild.IsValid Then
                    mHSNACSChild = mHSNACSChild.Save()
                End If
                Dim mHSNACSDetail As String = mHSNACSChild.GSTPercent.ToString + " From Date : " + mHSNACSChild.FromDateFormatted
                MarkLog(Util.Action.Save, "HSNACS", mHSNACSDetail, Util.ErrorType.NoError, mHSNACS.ID, EventLogID)

                Session("mHSNACS") = mHSNACS
                Session("mHSNACSChild") = mHSNACSChild
                SetPage()
                DataBind()
            End If
        Catch ex As SqlException
            If ex.Number = 2627 Then
                MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + "You can not add duplicate entry in HSN/SAC.", MsgBoxStyle.OkOnly, "")
                Session("ex.Number") = "ex.Number"
            End If
        End Try
        If Session("ex.Number") = "ex.Number" Then
            '
        Else
            Session.Remove("ex.Number")
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                        If mHSNACS.IsValid Then
                            Save()
                            If Session("ex.Number") = "ex.Number" Then
                                Session.Remove("ex.Number")
                            Else
                                Session.Remove("ex.Number")
                                Dim mopenas As String = Request.QueryString("Type")
                                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                    Exit Sub
                                End If
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
                        If mHSNACS.IsNew Then
                            lblTitle.Text = "HSN/SAC [New]"
                            Session.Remove("mHSNACS")
                        End If
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
    Private Sub addAttributes()
        txtPercentage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentage').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        If mHSNACS Is Nothing Or mHSNACSChild Is Nothing Then
            'Make new HSNACS
            mHSNACS = HSNACS.NewHSNACS()
            mHSNACSChild = HSNACSChild.NewHSNACSChild(mHSNACS.ID)
            mHSNACSChild.FromDate = Today.Date
            Session("mHSNACS") = mHSNACS
            Session("mHSNACSChild") = mHSNACSChild
        End If

        If Not Page.IsPostBack Then
            setFocus(txtCode)
            DataFieldBind()
        End If
        SetPage()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not IsValid Then Exit Sub
            mHSNACS = Session("mHSNACS")
            mHSNACSChild = Session("mHSNACSChild")
            SaveFormToObject()

            If mHSNACS.IsValid Then
                mHSNACS.ApplyEdit()

                mHSNACS = mHSNACS.Save()
                If mHSNACSChild.IsValid Then
                    mHSNACSChild = mHSNACSChild.Save()
                End If
                Dim mHSNACSDetail As String = mHSNACSChild.GSTPercent.ToString + " From Date : " + mHSNACSChild.FromDateFormatted
                MarkLog(Util.Action.Save, "HSNACS", mHSNACSDetail, Util.ErrorType.NoError, mHSNACS.ID, EventLogID)

                Session("mHSNACS") = mHSNACS
                Session("mHSNACSChild") = mHSNACSChild
                SetPage()
                DataBind()
            Else
                If CustomValidate1() = False Then
                    upnlValidations.Update()
                    Exit Sub
                End If
            End If
        Catch ex As SqlException
            If ex.Number = 2627 Then
                MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + "You can not add duplicate entry in HSN/SAC.", MsgBoxStyle.OkOnly, "")
                Session("Duplicate") = "Duplicate"
            End If
        End Try
        If Session("Duplicate") = "Duplicate" Then
            Session.Remove("Duplicate")
        Else
            Session.Remove("Duplicate")
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        mHSNACS = Session("mHSNACS")
        mHSNACSChild = Session("mHSNACSChild")
        SaveFormToObject()
        If mHSNACSChild.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            Exit Sub
        Else
            SaveFormToObject()
            Session("mHSNACS") = mHSNACS
            Session("mHSNACSChild") = mHSNACSChild
            Session.Remove("mHSNACS")
            Session.Remove("mHSNACSChild")
            Session.Remove("mOldHSNACSChild")
            Session.Remove("mHSNACSList")

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
        If mHSNACS.IsValid = False Then
            For i As Integer = 0 To mHSNACS.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mHSNACS.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
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