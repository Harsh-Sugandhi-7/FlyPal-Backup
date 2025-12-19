'Created By: Saylee
'Dated     : 20-Sep-2016


Public Class wfMELDetail_Ajax
    Inherits Page

#Region "Variable Declarations"

    Public mMELList As MELList
    Public mMELCategoryList As MELCategoryList
    Public mMEL As MEL
    Public strMsg As String = ""
    Public mMachine As Machine
    Dim EventLogID As Guid
    Dim PartTypeId As Guid = Guid.Empty

#End Region

#Region " Business Methods "
    Private Sub SetSession()
        Session("mMEL") = mMEL
        Session("mMELList") = mMELList
    End Sub
    Private Sub GetSession()
        mMEL = Session("mMEL")
        mMELList = Session("mMELList")
        mMachine = Session("mMachine")
        PartTypeId = Session("PartTypeId")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMEL")
        Session.Remove("mMELList")
        Session.Remove("PartTypeId")
    End Sub
    Private Sub NewRecord()
        mMEL = MEL.NewMEL(Guid.NewGuid)
        Session("mMEL") = mMEL
    End Sub

    Private Sub ControlVisibility(Optional ForMELCategory As Boolean = False,
                                  Optional IsHours As Boolean = False)

        If ForMELCategory Then

            If cmbMELCategory.SelectedIndex = 1 Then

                chkIsInHours.Enabled = True
                txtFrequencyInDay.Enabled = True
                txtFrequencyInHours.Enabled = False

            Else

                chkIsInHours.Checked = False
                chkIsInHours.Enabled = False
                txtFrequencyInDay.Enabled = False
                txtFrequencyInHours.Enabled = False
                txtFrequencyInHours.Text = ""

            End If

        End If

        If IsHours Then

            If chkIsInHours.Checked = True Then

                txtFrequencyInDay.Text = "0"
                txtFrequencyInDay.Enabled = False
                txtFrequencyInHours.Enabled = True

            Else

                txtFrequencyInHours.Text = ""

                If cmbMELCategory.SelectedIndex = 1 Then
                    txtFrequencyInDay.Enabled = True
                End If

                txtFrequencyInHours.Enabled = False

                txtFrequencyInCycles.Text = ""

            End If

        End If

    End Sub
    Private Sub DeleteRecord(mId As Guid)

        MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                        MSGBox.Message_text.Delete,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

        mMEL = MEL.GetMEL(mId)
        Session("mMEL") = mMEL

    End Sub

    Private Sub SetObject()

        ' mMEL.MachineID = mMachine.ID
        mMEL.ModelID = New Guid(cmbModel.SelectedValue)
        mMEL.MakeMELQty = Val(txtMakeMELQty.Text)
        mMEL.FlyMELQty = Val(txtFlyMELQty.Text)
        mMEL.MELCategoryID = CInt(cmbMELCategory.SelectedValue)
        mMEL.Remark = txtRemark.Text.Trim
        mMEL.IsHours = chkIsInHours.Checked
        mMEL.FrequencyInDays = Val(txtFrequencyInDay.Text)
        mMEL.FrequencyInHours = txtFrequencyInHours.Text.Trim
        mMEL.FrequencyInCycles = txtFrequencyInCycles.Text.Trim

        mMEL.ATAID = New Guid(cmbATAChapter.SelectedValue)
        mMEL.SubATAID = New Guid(cmbSubATAList.SelectedValue)
        mMEL.ItemNo = Trim(txtItemSequenceNo.Text.Trim)
        mMEL.PageNo = txtPageNo.Text.Trim
        mMEL.RevisionNo = txtRevNo.Text.Trim
        If txtRevisionDate.Text <> "" Then
            mMEL.RevisionDate = txtRevisionDate.Text
        Else
            mMEL.RevisionDate = System.DBNull.Value
        End If

        mMEL.IsApplicable = chkIsApplicable.Checked
        mMEL.NotApplicableNote = txtApplicabilityNote.Text.Trim

        mMEL.MELDescription = txtDescription.Text.Trim

    End Sub
    Private Sub AddAttributes()
        txtMakeMELQty.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtMakeMELQty').value,event)")
        txtFlyMELQty.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFlyMELQty').value,event)")
        txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
        txtFrequencyInHours.Attributes.Add("onkeypress", "var key; if(window.event){ key = event.keyCode;}else if(event.which){ key = event.which;} return (key == 13 || key == 8 || key == 9 || (key >= 48 && key <= 58) )")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim ErrorCount As Integer = 0
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Dim tmpPartName As String
                    Dim mtmpID As Guid
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mMEL = Session("mMEL")
                            tmpPartName = mMEL.PartName
                            mtmpID = mMEL.ID
                            MEL.DeleteMEL(mMEL.ID)
                            lbltitle.InnerText = "Minimum Equipment Details [New]"
                            NewRecord()
                            ClearValues()
                            DataFieldBind()
                            ControlVisibility(True, False)
                            upnlActionBtn.Update()
                            upnlMELDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "MEl", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "MEl", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "MEl", MsgBoxStyle.OkOnly, "")
                                MarkLog(Action.Delete, "MEL", "Can't delete : " & tmpPartName & " is Currently in use", ErrorType.HandledError, Guid.Empty, EventLogID)
                            End If
                            ErrorCount = ex.Errors.Count
                            NewRecord()
                            DataFieldBind()
                            upnlMELDetails.Update()
                        Finally
                            If ErrorCount = 0 Then
                                MarkLog(Action.Delete, "MEL", tmpPartName, ErrorType.NoError, mtmpID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub

    Private Sub ClearComboBoxValues()
        PartTypeId = Guid.Empty
        Session("PartTypeId") = PartTypeId
    End Sub
    Private Sub ClearValues()
        txtMakeMELQty.Text = ""
        txtFlyMELQty.Text = ""
        txtRemark.Text = ""
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbModel.DataSource = ModelList.GetAirframeModelList("(SELECT)")

        cmbMELCategory.DataSource = MELCategoryList.GetMELCategoryList("(SELECT)")

        cmbATAChapter.DataSource = ATAList.GetATAList("", "(SELECT)")

        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        cmbSubATAList.DataSource = SubATAList.GetSubATAList(mMEL.ATAID, "", "(SELECT)")

        DataBind()
        If IsDate(mMEL.RevisionDate) Then
            txtRevisionDate.Text = CDate(mMEL.RevisionDate).ToString(AppSettings("DateFormat"))
        End If

    End Sub
    Public Sub customvalidate(s As Object, e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        AddAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            cmbModel.Focus()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        MarkLog(Action.Close, "MEL", "", ErrorType.NoError, Guid.Empty, EventLogID)
        Session("Sender") = ""
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("index.aspx")
    End Sub

    Private Sub Save_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        SetObject()

        If mMEL.IsValid Then

            Try

                SetObject()
                mMEL.Save()
                cmbModel.Focus()

                MarkLog(Action.Save,
                        "MEL",
                        mMEL.PartName,
                        ErrorType.NoError,
                        mMEL.ID,
                        EventLogID)

                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
                                MSGBox.Message_text.SavedSuccessFully,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")


                NewRecord()
                mMELList = Session("mMELList")
                DataFieldBind()
                lbltitle.InnerText = "Minimum Equipment Details [New]"
                txtMakeMELQty.Text = ""
                txtRemark.Text = ""
                txtFlyMELQty.Text = ""
                txtRevisionDate.Text = ""
                Session("Sender") = ""

                If cmbMELCategory.SelectedIndex = 1 Then
                    chkIsInHours.Enabled = True
                    txtFrequencyInDay.Enabled = True
                Else
                    If chkIsInHours.Checked = True Then
                        chkIsInHours.Checked = False
                    End If
                    chkIsInHours.Enabled = False
                    txtFrequencyInDay.Enabled = False
                End If

                MarkLog(Action.Save,
                        "MEL",
                        mMEL.PartName,
                        ErrorType.HandledError,
                        mMEL.ID,
                        EventLogID)

                upnlMELDetails.Update()

            Catch ex As SqlException

                If ex.Number = 8145 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.ProcedureError,
                                    "MEL",
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.Duplicate,
                                    "MEL",
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

            End Try

        Else

            If Not mMEL.IsValid Then

                For j As Integer = 0 To mMEL.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mMEL.GetBrokenRulesCollection(j).Description + "</BR>"
                Next

            End If

            If strMsg.Trim <> "" Then

                cvFrequency.ErrorMessage = strMsg.TrimEnd("</BR>")
                cvFrequency.IsValid = mMEL.IsValid

            End If

            upnlValidationSummary.Update()

        End If

    End Sub

    Private Sub cmbMELCategory_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbMELCategory.SelectedIndexChanged
        mMEL.MELCategoryID = CInt(cmbMELCategory.SelectedValue)
        txtFrequencyInDay.Text = mMEL.FrequencyInDays

        ControlVisibility(True, False)
        If cmbMELCategory.SelectedIndex = 1 Then
            txtFrequencyInDay.Text = "0"
        End If

        cmbMELCategory.Focus()
        upnlFreq.Update()
    End Sub
    Private Sub chkIsInHours_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkIsInHours.CheckedChanged
        ControlVisibility(False, True)
    End Sub
    Private Sub ImgBtnATAChapter_Click(sender As System.Object, e As System.EventArgs) Handles imgbtnATAChapter.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenATAWindow", "OpenATAWindow()", True)
    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        mMEL.SubATAID = Guid.Empty
        Session("mMEL") = mMEL

        cmbSubATAList.DataSource = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "(SELECT)")
        cmbSubATAList.DataBind()
        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        upnlSubATA.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub chkIsApplicable_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIsApplicable.CheckedChanged
        txtApplicabilityNote.Text = ""
        If chkIsApplicable.Checked Then
            txtApplicabilityNote.Enabled = False
        Else
            txtApplicabilityNote.Enabled = True
        End If
        upnlApplicability.Update()
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        cmbATAChapter.DataSource = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
#End Region



End Class