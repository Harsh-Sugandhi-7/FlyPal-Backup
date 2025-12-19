'Created By: Prashant
'Dated:  22-Feb-2024


Public Class wfDeferredDetail_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mDeviationCategoryList As DeviationCategoryList
    Public mDeviationList As DeviationList
    Public strMsg As String = ""
    Public mMachine As Machine
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub SetSession()
        Session("mDeviationList") = mDeviationList
    End Sub
    Private Sub GetSession()
        mDeviationList = Session("mDeviationList")
        mMachine = Session("mMachine")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDeviationList")
    End Sub
    Private Sub NewRecord()
        mDeviationList = DeviationList.NewDeviationList(Guid.NewGuid)
        Session("mDeviationList") = mDeviationList
    End Sub
    Private Sub ControlVisibility(Optional ByVal ForMELCategory As Boolean = False, Optional ByVal IsHours As Boolean = False)

    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDeviationList = DeviationList.GetDeviationList(mId)
        Session("mDeviationList") = mDeviationList
    End Sub
    Private Sub SetObject()
        mDeviationList.ModelID = New Guid(cmbModel.SelectedValue)
        mDeviationList.ModelName = (cmbModel.SelectedItem.Text)
        mDeviationList.ATAID = New Guid(cmbATAChapter.SelectedValue)
        mDeviationList.SubATAID = New Guid(cmbSubATAList.SelectedValue)
        'mDeviationList.ATACode = 0
        mDeviationList.ATANomenclature = cmbATAChapter.SelectedItem.Text
        'mDeviationList.SubATACode = cmbSubATAList.SelectedItem.Text
        mDeviationList.SubATANomenclature = cmbSubATAList.SelectedItem.Text
        mDeviationList.DeviationCategoryID = Val(cmbDeviationCategoryList.SelectedValue)
        mDeviationList.DeviationCategoryName = cmbDeviationCategoryList.SelectedItem.Text
        mDeviationList.Description = txtDescription.Text.Trim
        mDeviationList.QtyInstalled = Val(txtQtyInstalled.Text)
        mDeviationList.HoursLimit = txtFrequencyInHours.Text.Trim
        mDeviationList.CyclesLimit = Val(txtCyclesLimit.Text)
        mDeviationList.DaysLimit = Val(txtFrequencyInDay.Text)
        mDeviationList.Condition = txtCondition.Text.Trim
        mDeviationList.Limitation = txtLimitation.Text.Trim
        mDeviationList.Procedures = txtProcedures.Text.Trim
        mDeviationList.Note = txtNote.Text.Trim

        'Added By Saylee on 11-Sep-2024
        mDeviationList.ItemNo = Trim(txtItemSequenceNo.Text.Trim)
        mDeviationList.PageNo = txtPageNo.Text.Trim
        mDeviationList.RevisionNo = txtRevNo.Text.Trim
        If txtRevisionDate.Text <> "" Then
            mDeviationList.RevisionDate = txtRevisionDate.Text
        Else
            mDeviationList.RevisionDate = System.DBNull.Value
        End If
        '****************************************
    End Sub
    Private Sub addAttributes()
        txtQtyInstalled.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtQtyInstalled').value,event)")
        txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
        txtCyclesLimit.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtCyclesLimit').value,event)")
        txtFrequencyInHours.Attributes.Add("onkeypress", "var key; if(window.event){ key = event.keyCode;}else if(event.which){ key = event.which;} return (key == 13 || key == 8 || key == 9 || (key >= 48 && key <= 58) )")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim ErrorCount As Integer = 0
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Dim mtmpID As Guid
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mDeviationList = Session("mDeviationList")
                            mtmpID = mDeviationList.ID
                            DeviationList.DeleteDeviationList(mDeviationList.ID)
                            lbltitle.InnerText = "Minimum Equipment Details [New]"
                            NewRecord()
                            ClearValues()
                            DataFieldBind()
                            ControlVisibility(True, False)
                            upnlActionBtn.Update()
                            upnlDeviationListDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "DeviationList", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "DeviationList", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "DeviationList", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "DeferredListMaster", "", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                            End If
                            ErrorCount = ex.Errors.Count
                            NewRecord()
                            DataFieldBind()
                            upnlDeviationListDetails.Update()
                        Finally
                            If ErrorCount = 0 Then
                                MarkLog(Util.Action.Delete, "DeferredListMaster", "", Util.ErrorType.NoError, mtmpID, EventLogID)
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
    Private Sub ClearValues()
        txtQtyInstalled.Text = ""
        txtNote.Text = ""
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        cmbModel.DataSource = ModelList.GetAirframeModelList("(SELECT)")

        cmbDeviationCategoryList.DataSource = DeviationCategoryList.GetDeviationCategoryList("(SELECT)")

        cmbATAChapter.DataSource = ATAList.GetATAList("", "(SELECT)")

        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        cmbSubATAList.DataSource = SubATAList.GetSubATAList(mDeviationList.ATAID, "", "(SELECT)")

        DataBind()

        If IsDate(mDeviationList.RevisionDate) Then
            txtRevisionDate.Text = CDate(mDeviationList.RevisionDate).ToString(AppSettings("DateFormat"))
        End If

    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtFrequencyInDay" Then
            'If txtFrequencyInHours.Text = "" And txtCyclesLimit.Text = 0 And txtFrequencyInDay.Text = 0 Then
            '    custValidator.ErrorMessage = "Interval either in hours, cycles or days required."
            '    e.IsValid = False
            'Else
            '    e.IsValid = True
            'End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            cmbModel.Focus()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "DeferredListMaster", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("Sender") = ""
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("index.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SetObject()
        If mDeviationList.IsValid Then
            Try
                SetObject()
                mDeviationList.Save()
                cmbModel.Focus()
                MarkLog(Util.Action.Save, "DeferredListMaster", "", Util.ErrorType.NoError, mDeviationList.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                lbltitle.InnerText = "Details [New]"
                txtQtyInstalled.Text = ""
                txtNote.Text = ""
                txtRevisionDate.Text = ""
                Session("Sender") = ""
                MarkLog(Util.Action.Save, "DeferredListMaster", "", Util.ErrorType.HandledError, mDeviationList.ID, EventLogID)
                upnlDeviationListDetails.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "MEL", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "MEL", MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            If Not mDeviationList.IsValid Then
                For j As Integer = 0 To mDeviationList.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mDeviationList.GetBrokenRulesCollection(j).Description + "</BR>"
                Next
            End If

            If strMsg.Trim <> "" Then
                cvFrequency.ErrorMessage = strMsg.TrimEnd("</BR>")
                cvFrequency.IsValid = mDeviationList.IsValid
            End If
            upnlValidationSummary.Update()
        End If
    End Sub
    'Private Sub cmbDeviationCategoryList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDeviationCategoryList.SelectedIndexChanged
    '    mDeviationList.DeviationCategoryID = CInt(cmbDeviationCategoryList.SelectedValue)
    '    txtFrequencyInDay.Text = mDeviationList.DaysLimit

    '    ControlVisibility(True, False)
    '    If cmbDeviationCategoryList.SelectedIndex = 1 Then
    '        txtFrequencyInDay.Text = "0"
    '    End If

    '    cmbDeviationCategoryList.Focus()
    '    upnlFreq.Update()
    'End Sub
    'Private Sub chkIsInHours_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsInHours.CheckedChanged
    '    ControlVisibility(False, True)
    'End Sub
    Private Sub ImgBtnATAChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnATAChapter.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenATAWindow", "OpenATAWindow()", True)
    End Sub
    Private Sub cmbATAChapter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
        mDeviationList.SubATAID = Guid.Empty
        Session("mDeviationList") = mDeviationList

        cmbSubATAList.DataSource = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "(SELECT)")
        cmbSubATAList.DataBind()
        cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
        upnlSubATA.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Private Sub chkIsApplicable_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIsApplicable.CheckedChanged
    '    txtApplicabilityNote.Text = ""
    '    If chkIsApplicable.Checked Then
    '        txtApplicabilityNote.Enabled = False
    '    Else
    '        txtApplicabilityNote.Enabled = True
    '    End If
    '    upnlApplicability.Update()
    'End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        Dim mATAList As ATAList
        mATAList = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
#End Region



End Class