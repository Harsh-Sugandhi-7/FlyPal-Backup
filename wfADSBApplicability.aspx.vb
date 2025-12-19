

'Created by : Saylee
'Dated      : 5-Sep-2022

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class wfADSBApplicability
    Inherits System.Web.UI.Page


#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum

#End Region


#Region "Variables and Declarations"
    Public mADSBTechRecording As ADSBTechRecording
    Dim mUser As User
    Public mNatureList As ADSBNatureList
    Public mAssemblyTypeList As AssemblyTypeList

    Public mPartListAutoComplete As PartListAutoComplete
    Public mModelListAutoComplete As ModelListAutoComplete
#End Region

#Region "Helper Methods"

    Private Sub addAttributes()
        txtADSBTechRecordingText.Attributes.Add("onblur", "WaterMark(this, event);")
        txtADSBTechRecordingText.Attributes.Add("onfocus", "WaterMark(this, event);")
    End Sub
    Private Sub GetSession()
        mADSBTechRecording = Session("mADSBTechRecording")
        mNatureList = Session("mNatureList")
        mAssemblyTypeList = Session("mAssemblyTypeList")

        mPartListAutoComplete = Session("mPartListAutoComplete")
        mModelListAutoComplete = Session("mModelListAutoComplete")
    End Sub

    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "ADSBTechRecording"
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Sub setObject()
        mADSBTechRecording.Applicability = True
        mADSBTechRecording.ADSBNatureID = Val(cmbNatureList.SelectedValue)
        mADSBTechRecording.AssemblyTypeID = Val(cmbAssemblyType.SelectedValue)

        mADSBTechRecording.ApplicableToPart = rdbPart.Checked
        mADSBTechRecording.ApplicableToModel = rdbModel.Checked
        mADSBTechRecording.IsReviewBoardMeetingRequired = chkPreviewReq.Checked
        mADSBTechRecording.IsMeetingPriority = rdbPriority.Checked

        'ADSBTechRecordingApplicableOns
        Dim mADSBTechRecordingApplicableOn As ADSBTechRecordingApplicableOn
        Dim i As Integer = 0

        If mADSBTechRecording.ADSBTechRecordingApplicableOns.Count > 0 Then
            For Each mADSBTechRecordingApplicableOn In mADSBTechRecording.ADSBTechRecordingApplicableOns
                '  Dim cmbSerialNoList, cmbAssemblySerialNo As DropDownList
                Dim txtCompliancePeriod, txtRemark, txtEffectiveDate As TextBox

                txtEffectiveDate = dgEffectivityDetails.Rows(i).FindControl("txtEffectiveDate")
                txtCompliancePeriod = dgEffectivityDetails.Rows(i).FindControl("txtCompliance")
                txtRemark = dgEffectivityDetails.Rows(i).FindControl("txtRemark")




                With mADSBTechRecordingApplicableOn
                    .CompliancePeriod = txtCompliancePeriod.Text
                    If txtEffectiveDate.Text.ToString <> "" Then
                        .EffectiveDate = CDate(txtEffectiveDate.Text)
                    Else
                        .EffectiveDate = System.DBNull.Value
                    End If
                    .Remark = txtRemark.Text
                End With
                i = i + 1
            Next

        End If




        Session("mADSBTechRecording") = mADSBTechRecording
    End Sub
    Private Function Save() As Boolean
        Try
            setObject()

            If mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0 Then
                MSGBoxCtrl.show("Alert..!!", "Applicable to either Model/Part is Required.", "", MsgBoxStyle.OkOnly, "")
                Exit Function
            End If

            mADSBTechRecording.ApplyEdit()


            If mADSBTechRecording.IsValid Then
                mADSBTechRecording.Save()
            Else
                upnlValidationsummary.Update()
            End If
            DataFieldBind()
            ControlVisibility()
            SetPage()

            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
            MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)

            Return True
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
    Private Sub SetPage()

        If mADSBTechRecording.IsNew = True Then
            lblTitle.InnerText = "AD/SB For " + mADSBTechRecording.ADSBRecordingText.ToString + " [ NEW ]"
        Else
            lblTitle.InnerText = "AD/SB For " + mADSBTechRecording.ADSBRecordingText.ToString + " [" + mADSBTechRecording.ADSBNo + "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result


        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mADSBTechRecording.IsValid = True Then
                            mADSBTechRecording.StatusID = 2
                            DataFieldBind()
                            Save()

                            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                            MarkLog(Util.Action.Authorize, "ADSBTechRecording", User.Identity.Name + " Authorized AD/SB : " + ADSBDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                            MSGBoxCtrl.show("Authorized!", "Authorized SuccessFully", "", MsgBoxStyle.OkOnly, "")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        If mADSBTechRecording.IsValid = True Then
                            mADSBTechRecording.StatusID = 4
                            DataFieldBind()
                            Save()

                            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                            MarkLog(Util.Action.Cancel, "ADSBTechRecording", User.Identity.Name + " Canceled Invoice : " + ADSBDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                            MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")

                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                 
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If Not CustomValidate1() Then
                            upnlValidationsummary.Update()
                        End If

                      
                        If Save() Then
                            SetPage()
                            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                            MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved Invoice : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                            Response.Redirect("Index.aspx")
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session("mADSBTechRecording") = mADSBTechRecording
                        DataFieldBind()

                    End If
                Case MsgBoxResult.Ok

            End Select

        End If
    End Sub
   
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()


        If Not mADSBTechRecording.ADSBDateFormatted Is System.DBNull.Value Then
            txtADSBTechRecordingDate.Text = Format(CDate(mADSBTechRecording.ADSBDateFormatted), AppSettings("DateFormat"))
        Else
            txtADSBTechRecordingDate.Text = ""
        End If

        mNatureList = ADSBNatureList.GetADSBNatureList("(SELECT)")
        cmbNatureList.DataSource = mNatureList
        Session("mNatureList") = mNatureList

        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList("(SELECT)")
        cmbAssemblyType.DataSource = mAssemblyTypeList
        Session("mAssemblyTypeList") = mAssemblyTypeList

        mPartListAutoComplete = PartListAutoComplete.GetPartList()
        cmbPartList.DataSource = mPartListAutoComplete
        Session("mPartListAutoComplete") = mPartListAutoComplete

        mModelListAutoComplete = ModelListAutoComplete.GetModelList(AssemblyTypeID:=mADSBTechRecording.AssemblyTypeID)
        cmbModelList.DataSource = mModelListAutoComplete
        Session("mModelListAutoComplete") = mModelListAutoComplete


        dgEffectivityDetails.DataSource = mADSBTechRecording.ADSBTechRecordingApplicableOns
      

        DataBind()




    End Sub
    Private Sub SetPartList()
        For i As Integer = 0 To cmbPartList.Items.Count - 1
            If cmbPartList.Items(i).Selected Then
                Dim mPartListForSerialNos As PartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(PartName:=cmbPartList.Items(i).Text, SerialNo:="", CurrentDate:=Today.Date.ToString)

                For Each info As PartListForSerialNos.PartListForSerialNosInfo In mPartListForSerialNos

                    If Not mADSBTechRecording.ADSBTechRecordingApplicableOns.Contains(mPartListAutoComplete(cmbPartList.Items(i).Text).ID, info.SerialNo, "") Then


                        mADSBTechRecording.ADSBTechRecordingApplicableOns.Add(mADSBTechRecording.ID)

                        With mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentItem
                            .PartID = mPartListAutoComplete(cmbPartList.Items(i).Text).ID
                            .PartName = cmbPartList.Items(i).Text
                            .SerialNo = info.SerialNo
                        End With
                    End If
                Next
            End If
        Next
        dgEffectivityDetails.DataSource = mADSBTechRecording.ADSBTechRecordingApplicableOns
        dgEffectivityDetails.DataBind()
        Session("mADSBTechRecording") = mADSBTechRecording
        upnlPartList.Update()
        '' PartIDs.Append("</Part>")
    End Sub
    Private Sub SetModelList()
        For i As Integer = 0 To cmbModelList.Items.Count - 1
            If cmbModelList.Items(i).Selected Then
                Dim mModelListForSerialNos As nWOModelListForSerialNos = nWOModelListForSerialNos.GetModelListForSerialNosList(ModelName:=cmbModelList.Items(i).Text, AssemblyTypeID:=Val(cmbAssemblyType.SelectedValue), SerialNo:="", CurrentDate:=Today.Date.ToString, ShowOnlyInstalledSerialNos:=True)

                For Each info As nWOModelListForSerialNos.ModelListForSerialNosInfo In mModelListForSerialNos

                    If Not mADSBTechRecording.ADSBTechRecordingApplicableOns.Contains(mModelListAutoComplete(cmbModelList.Items(i).Text).ID, info.SerialNo) Then
                        mADSBTechRecording.ADSBTechRecordingApplicableOns.Add(mADSBTechRecording.ID)

                        With mADSBTechRecording.ADSBTechRecordingApplicableOns.CurrentItem
                            .ModelID = mModelListAutoComplete(cmbModelList.Items(i).Text).ID
                            .ModelName = cmbModelList.Items(i).Text
                            .SerialNo = info.SerialNo
                        End With
                    End If
                
                Next
            End If
        Next
        dgEffectivityDetails.DataSource = mADSBTechRecording.ADSBTechRecordingApplicableOns
        dgEffectivityDetails.DataBind()
        Session("mADSBTechRecording") = mADSBTechRecording

        '' PartIDs.Append("</Part>")
    End Sub

    'Public Sub GridBind()
    '    mADSBTechRecording = Session("mADSBTechRecording")
    '    For i As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1

    '        If rdbPart.Checked Then
    '            Dim cmbSerialNoList As DropDownList
    '            Dim txt As TextBox
    '            txt = dgEffectivityDetails.Rows(i).FindControl("txtPartNo")
    '            cmbSerialNoList = dgEffectivityDetails.Rows(i).FindControl("cmbCompSerialNo")

    '            If txt.Text <> "" Then
    '                If mADSBTechRecording.ADSBTechRecordingApplicableOns(i).PartName <> "" Then
    '                    txt.Text = mADSBTechRecording.ADSBTechRecordingApplicableOns(i).PartName
    '                End If

    '                Dim mPartListForSerialNos As PartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(PartName:=txt.Text, SerialNo:="", AddTopItem:="(SELECT)", CurrentDate:=Today.Date.ToString)
    '                cmbSerialNoList.DataSource = mPartListForSerialNos
    '                cmbSerialNoList.DataBind()
    '                If mADSBTechRecording.ADSBTechRecordingApplicableOns(i).SerialNo.ToString.Trim <> "" Then
    '                    cmbSerialNoList.SelectedItem.Text = mADSBTechRecording.ADSBTechRecordingApplicableOns(i).SerialNo.ToString.Trim
    '                End If
    '            End If

    '        ElseIf rdbModel.Checked Then
    '            Dim cmbSerialNoList As DropDownList
    '            Dim txt As TextBox
    '            txt = dgEffectivityDetails.Rows(i).FindControl("txtModel")
    '            cmbSerialNoList = dgEffectivityDetails.Rows(i).FindControl("cmbAssemblySerialNo")

    '            If mADSBTechRecording.ADSBTechRecordingApplicableOns(i).ModelName <> "" Then
    '                txt.Text = mADSBTechRecording.ADSBTechRecordingApplicableOns(i).ModelName
    '            End If

    '            If txt.Text <> "" Then
    '                Dim mModelListForSerialNos As nWOModelListForSerialNos = nWOModelListForSerialNos.GetModelListForSerialNosList(ModelName:=txt.Text, AssemblyTypeID:=Val(cmbAssemblyType.SelectedValue), SerialNo:="", AddTopItem:="(SELECT)", CurrentDate:=Today.Date.ToString)
    '                cmbSerialNoList.DataSource = mModelListForSerialNos
    '                cmbSerialNoList.DataBind()
    '                If mADSBTechRecording.ADSBTechRecordingApplicableOns(i).SerialNo.ToString.Trim <> "" Then
    '                    cmbSerialNoList.SelectedItem.Text = mADSBTechRecording.ADSBTechRecordingApplicableOns(i).SerialNo.ToString.Trim
    '                End If
    '            End If
    '        End If


    '    Next
    'End Sub
    Public Function FetchItemByNameCount(ByVal PartNo As String) As Object
        If rdbModel.Checked Then
            Dim ModelList As ModelListAutoComplete = ModelListAutoComplete.GetModelList(PartNo)
            If ModelList.Count > 0 And PartNo = ModelList(0).Name Then
                Return ModelList(0).ID
            Else
                Return Guid.Empty.ToString
            End If
        Else
            Dim Partlist As PartListAutoComplete = PartListAutoComplete.GetPartList(PartNo)
            If Partlist.Count > 0 And PartNo = Partlist(0).Name Then
                Return Partlist(0).ID
            Else
                Return Guid.Empty.ToString
            End If
        End If
    End Function
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mADSBTechRecording.IsValid = False Then
            For i As Integer = 0 To mADSBTechRecording.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mADSBTechRecording.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
       
        'For currentRow As Integer = 0 To dgEffectivityDetails.Rows.Count - 1
        '    Dim rowToCompare As GridViewRow = dgEffectivityDetails.Rows(currentRow)
        '    Dim cmbAssemblySerialNoOfCurrentRow As DropDownList
        '    If rdbModel.Checked Then
        '        cmbAssemblySerialNoOfCurrentRow = CType(dgEffectivityDetails.Rows(currentRow).FindControl("cmbAssemblySerialNo"), DropDownList)
        '    Else
        '        cmbAssemblySerialNoOfCurrentRow = CType(dgEffectivityDetails.Rows(currentRow).FindControl("cmbCompSerialNo"), DropDownList)
        '    End If

        '    For otherRow As Integer = currentRow + 1 To dgEffectivityDetails.Rows.Count - 1
        '        Dim row As GridViewRow = dgEffectivityDetails.Rows(otherRow)
        '        Dim cmbAssemblySerialNoOfOtherRow As DropDownList

        '        If rdbModel.Checked Then
        '            cmbAssemblySerialNoOfOtherRow = CType(dgEffectivityDetails.Rows(otherRow).FindControl("cmbAssemblySerialNo"), DropDownList)
        '        Else
        '            cmbAssemblySerialNoOfOtherRow = CType(dgEffectivityDetails.Rows(otherRow).FindControl("cmbCompSerialNo"), DropDownList)
        '        End If


        '        Dim duplicateRow As Boolean = True
        '        If cmbAssemblySerialNoOfCurrentRow.SelectedValue.ToString <> "(SELECT)" And cmbAssemblySerialNoOfOtherRow.SelectedValue.ToString <> "(SELECT)" And cmbAssemblySerialNoOfCurrentRow.SelectedValue.ToString = cmbAssemblySerialNoOfOtherRow.SelectedValue.ToString Then
        '            duplicateRow = False
        '            'Exit For
        '        End If
        '        If duplicateRow = False Then
        '            strMsg = strMsg + "Same Serial No. cannot be selected with same model." + "<Br>"
        '            GoTo 1
        '        End If
        '    Next
        'Next

1:
        'Dim txtPartNo As TextBox
        'Dim cvValidator As CustomValidator
        'Dim upnlPartNoValidate As UpdatePanel
        'Dim strError As String = String.Empty

        'For j As Integer = 0 To dgEffectivityDetails.Rows.Count - 1
        '    If rdbModel.Checked Then
        '        cvValidator = CType(Me.dgEffectivityDetails.Rows(j).FindControl("cvModelNo"), CustomValidator)
        '        upnlPartNoValidate = CType(Me.dgEffectivityDetails.Rows(j).FindControl("upnlModelValidate"), UpdatePanel)
        '        txtPartNo = CType(Me.dgEffectivityDetails.Rows(j).FindControl("txtModel"), TextBox)

        '    Else
        '        cvValidator = CType(Me.dgEffectivityDetails.Rows(j).FindControl("cvPartNo"), CustomValidator)
        '        upnlPartNoValidate = CType(Me.dgEffectivityDetails.Rows(j).FindControl("upnlPartNoValidate"), UpdatePanel)
        '        txtPartNo = CType(Me.dgEffectivityDetails.Rows(j).FindControl("txtPartNo"), TextBox)
        '    End If

        '    If txtPartNo.Text = "" Then
        '        cvValidator.IsValid = False
        '        If rdbPart.Checked Then cvValidator.Text = "* Part No. Required"
        '        If rdbPart.Checked Then strError = "* Part No. Required"
        '        If rdbModel.Checked Then cvValidator.Text = "* Model Required"
        '        If rdbModel.Checked Then strError = "* Model Required"
        '        cvValidator.Visible = True
        '        upnlPartNoValidate.Update()
        '    ElseIf FetchItemByNameCount(PartNo:=txtPartNo.Text.Trim).Equals(Guid.Empty.ToString) Then
        '        cvValidator.IsValid = False
        '        If rdbPart.Checked Then cvValidator.Text = "* Enter whole Part No."
        '        If rdbModel.Checked Then cvValidator.Text = "* Enter whole Model"
        '        If rdbPart.Checked Then strError = "* Enter whole Part No."
        '        If rdbModel.Checked Then strError = "* Enter whole Model"
        '        upnlPartNoValidate.Update()
        '    End If

        'Next
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
       
        End If
        Return True
    End Function

    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtADSBTechRecordingDate" Then
            If txtADSBTechRecordingDate.Text = "" Then
                custValidator.ErrorMessage = "Select Date."
                e.IsValid = False
            End If

        End If
    End Sub
#End Region

#Region "Business Methods"
    Private Sub ControlVisibility()
        txtADSBTechRecordingDate.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)
        txtADSBTechRecordingText.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)
        txtADSBTechRecordingNo.Enabled = IIf(Not mADSBTechRecording.IsNew, False, True)

        txtADSBNO.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)
        txtSubject.Enabled = IIf(mADSBTechRecording.StatusID >= 2, False, True)


        rdbPart.Enabled = mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0 And mADSBTechRecording.ADSBStepsID <= 2
        rdbModel.Enabled = mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0 And mADSBTechRecording.ADSBStepsID <= 2
        cmbAssemblyType.Enabled = mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0 And mADSBTechRecording.ADSBStepsID <= 2
        cmbAssemblyType.Visible = rdbModel.Checked
        cmbModelList.Visible = rdbModel.Checked And cmbAssemblyType.SelectedIndex > 0

        dgEffectivityDetails.Columns(1).Visible = rdbPart.Checked
        dgEffectivityDetails.Columns(2).Visible = rdbModel.Checked


        'btnCancel.Visible = (Not mADSBTechRecording.IsNew) And (mADSBTechRecording.StatusID = 2)
        'btnAuthorized.Visible = (Not mADSBTechRecording.IsNew) And (mADSBTechRecording.StatusID = 1)
        'btnSave.Visible = (Not mADSBTechRecording.StatusID >= 2)
        'btnPrint.Visible = (Not mADSBTechRecording.IsNew)
        UpdatePanel()
    End Sub
    Private Sub UpdatePanel()
        upnlADSBTechRecordingDetails.Update()
        upnlStatusName.Update()
        upnlTitle.Update()
    End Sub
  
  
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack Then
            DataFieldBind()
            SetPage()
            ControlVisibility()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateModel", "CheckDuplicateModel();", True)

        If CustomValidate1() Then

            If (Not IsInRole(Rights.[New]) And mADSBTechRecording.IsNew) Or (Not IsInRole(Rights.Edit) And Not mADSBTechRecording.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            'If mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0 Then
            '    MSGBoxCtrl.show("Alert..!!", "Applicable to either Model/Part is Required.", "", MsgBoxStyle.OkOnly, "")
            'End If

            If Save() Then
                SetPage()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
                MarkLog(Util.Action.Save, "ADSBTechRecording", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)
                'Added on 21-May-2018 by Shital
                mADSBTechRecording = ADSBTechRecording.GetADSBTechRecording(mADSBTechRecording.ID)
                Session("mADSBTechRecording") = mADSBTechRecording
                '----------
            End If
        Else
            upnlValidationsummary.Update()
        End If

    End Sub
    Private Sub ImgAddEffectivityDetails_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgAddEffectivityDetails.Click
        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CheckDuplicateModel", "CheckDuplicateModel();", True)

       
        If rdbModel.Checked And cmbAssemblyType.SelectedIndex = 0 Then
            MSGBoxCtrl.show("Alert..!!", "Assembly Type Required.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        If CustomValidate1() Then
            ''   setObject()


            If rdbPart.Checked Then
                SetPartList()
            Else
                SetModelList()
            End If
         
            '   upnlADSBApplicability.Update()
            DataFieldBind()

            rdbPart.Enabled = mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0
            rdbModel.Enabled = mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0
            cmbAssemblyType.Enabled = mADSBTechRecording.ADSBTechRecordingApplicableOns.Count = 0

        Else
            upnlValidationsummary.Update()
        End If


    End Sub
    'Protected Sub txtPart_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    If CustomValidate1() = False Then
    '        upnlValidationsummary.Update()
    '        Exit Sub
    '    End If

    '    Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
    '    Dim j As Integer = currentRow.DataItemIndex
    '    For i As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
    '        If i = j Then
    '            Dim cmbSerialNoList As DropDownList
    '            Dim txt As TextBox
    '            txt = dgEffectivityDetails.Rows(j).FindControl("txtPartNo")
    '            cmbSerialNoList = dgEffectivityDetails.Rows(j).FindControl("cmbCompSerialNo")
    '            Dim mPartListForSerialNos As PartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(PartName:=txt.Text, SerialNo:="", AddTopItem:="(SELECT)", CurrentDate:=Today.Date.ToString)
    '            cmbSerialNoList.DataSource = mPartListForSerialNos
    '            cmbSerialNoList.DataBind()

    '        End If
    '    Next
    'End Sub
    'Protected Sub txtModel_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

    '    If CustomValidate1() = False Then
    '        upnlValidationsummary.Update()
    '        Exit Sub
    '    End If

    '    Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
    '    Dim j As Integer = currentRow.DataItemIndex
    '    For i As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
    '        If i = j Then
    '            Dim cmbSerialNoList As DropDownList
    '            Dim txt As TextBox
    '            Dim mAutoCompleteExtender As AjaxControlToolkit.AutoCompleteExtender
    '            mAutoCompleteExtender = dgEffectivityDetails.Rows(j).FindControl("txtModel_Autocomplete")
    '            mAutoCompleteExtender.ContextKey = Val(cmbAssemblyType.SelectedValue)
    '            mAutoCompleteExtender.UseContextKey = True

    '            txt = dgEffectivityDetails.Rows(j).FindControl("txtModel")

    '            cmbSerialNoList = dgEffectivityDetails.Rows(j).FindControl("cmbAssemblySerialNo")
    '            'Dim mAssemblyList As AssemblyList = AssemblyList.GetAssemblyListForComboBox(Val(cmbAssemblyType.SelectedValue))
    '            Dim mModelListForSerialNos As nWOModelListForSerialNos = nWOModelListForSerialNos.GetModelListForSerialNosList(ModelName:=txt.Text, AssemblyTypeID:=Val(cmbAssemblyType.SelectedValue), SerialNo:="", AddTopItem:="(SELECT)", CurrentDate:=mADSBTechRecording.ADSBDateFormatted.ToString)
    '            cmbSerialNoList.DataSource = mModelListForSerialNos
    '            cmbSerialNoList.DataBind()


    '        End If
    '    Next
    'End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click

        Dim ADSBDetail = mADSBTechRecording.ADSBRecordingText + " Dated : " + mADSBTechRecording.ADSBDateFormatted + " for " + mADSBTechRecording.ADSBNo
        MarkLog(Util.Action.Close, "WOInvoice", ADSBDetail, Util.ErrorType.NoError, mADSBTechRecording.ID, EventLogID)

        If mADSBTechRecording.ADSBStepsID > 2 Then
            Response.Redirect("index.aspx")
            Exit Sub
        End If

        setObject()
        If mADSBTechRecording.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub dgEffectivityDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEffectivityDetails.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"

                setObject()
                mADSBTechRecording.ADSBTechRecordingApplicableOns.Remove(CInt(e.CommandArgument) - 1)
                dgEffectivityDetails.DataSource = mADSBTechRecording.ADSBTechRecordingApplicableOns
                dgEffectivityDetails.DataBind()
                Session("mADSBTechRecording") = mADSBTechRecording

                ControlVisibility()

                UpdatePanel()
        End Select
    End Sub
    'Private Sub dgEffectivityDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgEffectivityDetails.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        If rdbModel.Checked Then
    '            For i As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
    '                Dim mAutoCompleteExtender As AjaxControlToolkit.AutoCompleteExtender
    '                '  mAutoCompleteExtender = dgEffectivityDetails.Rows(i).FindControl("txtModel_Autocomplete")
    '                mAutoCompleteExtender = e.Row.FindControl("txtModel_Autocomplete")
    '                mAutoCompleteExtender.ContextKey = Val(cmbAssemblyType.SelectedValue)
    '                mAutoCompleteExtender.UseContextKey = True
    '            Next
    '        End If
    '    End If

    'End Sub
    Private Sub rdbModel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbModel.CheckedChanged, rdbPart.CheckedChanged
        If rdbModel.Checked Then
            cmbAssemblyType.Visible = True
            lblAssemblyType.Visible = True
            cmbModelList.Visible = True

            cmbPartList.Visible = False
        ElseIf rdbPart.Checked Then
            cmbAssemblyType.Visible = False
            lblAssemblyType.Visible = False
            cmbModelList.Visible = False
            cmbPartList.Visible = True
        End If
        cmbModelList.Visible = rdbModel.Checked And cmbAssemblyType.SelectedIndex > 0
        dgEffectivityDetails.Columns(1).Visible = rdbPart.Checked
        dgEffectivityDetails.Columns(2).Visible = rdbModel.Checked
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        mModelListAutoComplete = ModelListAutoComplete.GetModelList(AssemblyTypeID:=Val(cmbAssemblyType.SelectedValue.ToString))
        cmbModelList.DataSource = mModelListAutoComplete
        Session("mModelListAutoComplete") = mModelListAutoComplete
        cmbModelList.DataBind()

        cmbModelList.Visible = rdbModel.Checked And cmbAssemblyType.SelectedIndex > 0
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub chkPreviewReq_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPreviewReq.CheckedChanged
        If chkPreviewReq.Checked Then
            rdbNormal.Visible = True
            rdbPriority.Visible = True
        Else
            rdbNormal.Visible = False
            rdbPriority.Visible = False
        End If
        upnlPreviewReq.Update()

    End Sub
    
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mDistinctADSBText As DistinctADSBText
        mDistinctADSBText = DistinctADSBText.GetDistinctTextList(prefixText:=prefixText)

        If count = 0 Then
            Return (From c As DistinctADSBText.TextInfo In mDistinctADSBText
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctADSBText.TextInfo In mDistinctADSBText
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
    Public Shared Function GetPartNoList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim itemlist As PartListAutoComplete
        itemlist = PartListAutoComplete.GetPartList(prefixText)
       
        Return (From c As PartListAutoComplete.PartListAutoCompleteInfo In itemlist
                Select c.Name).Take(count).ToList
    End Function
    <System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
    Public Shared Function GetModelList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim itemlist As ModelListAutoComplete
        itemlist = ModelListAutoComplete.GetModelList(prefixText, Val(contextKey))

        Return (From c As ModelListAutoComplete.ModelListAutoCompleteInfo In itemlist
                Select c.Name).Take(count).ToList()
    End Function

    <System.Web.Services.WebMethod(), System.Web.Script.Services.ScriptMethod()>
    Public Shared Function GetCompSerialNoList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim itemlist As PartListForSerialNos
        itemlist = PartListForSerialNos.GetPartListForSerialNosList(contextKey, prefixText)

        Return (From c As PartListForSerialNos.PartListForSerialNosInfo In itemlist
                Select c.SerialNo).Take(count).ToList()
    End Function
#End Region

    
   
   
End Class