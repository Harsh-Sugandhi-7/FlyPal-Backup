Imports System.Linq
Public Class wfDentBuckelItems_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mDentBuckle As DentBuckle
    Public mDentBuckleItem As DentBuckleItem
    Public mATAList As ATAList
    Public mEmployeeStatus As EmployeeStatus
    Public mDentBuckleItemStatusList As DentBuckleItemStatusList
    Dim mDentBuckleTypeList As DentBuckleTypeList
    Public mDamageTypeList As DamageTypeList 'Ajay 18-oct-2022
#End Region

#Region " Buisness Method And Properties "
    Private Sub GetSession()
        mDentBuckle = Session("mDentBuckle")
        mATAList = CType(Session("mATAList"), ATAList)
        mDamageTypeList = CType(Session("mDamageTypeList"), DamageTypeList)
        mDentBuckleItemStatusList = Session("mDentBuckleItemStatusList")
        mDentBuckleTypeList = Session("mDentBuckleTypeList") 'Ajay 18-oct-2022
    End Sub
    Private Sub RemoveSession()
        Session.Remove("Edit")
        Session.Remove("mATAList")
        Session.Remove("mDamageTypeList") 'Ajay 18-oct-2022
        Session.Remove("mDentBuckleTypeList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "ResetActionTakenByEmployee" Then
                        txtActionTakenByEmployee.Text = ""
                        If Not mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByID.Equals(Guid.Empty) Then
                            txtActionTakenByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByName
                        End If
                        upnlDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "ResetReportedByEmployee" Then
                        txtReportedByEmployee.Text = ""
                        If Not mDentBuckle.DentBuckleItems.CurrentItem.ReportedByID.Equals(Guid.Empty) Then
                            txtReportedByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.ReportedByName
                        End If
                        upnlDetails.Update()
                    ElseIf MSGBoxCtrl.Sender = "ResetAcceptanceByEmployee" Then
                        txtAcceptanceByEmployee.Text = ""
                        If Not mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByID.Equals(Guid.Empty) Then
                            txtAcceptanceByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByName
                        End If
                        upnlDetails.Update()
                    End If
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Close" Then
                        Setobject()
                        If Session("middleframe") = "wfDentBuckleRectificationList_Ajax.aspx?" Then
                            mDentBuckle.Save()
                        End If
                        RemoveSession()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        Response.Redirect("index.aspx")
                    End If
                Case MsgBoxResult.No


                    If MSGBoxCtrl.Sender = "Close" Then


                        If Session("Edit") = True Then
                            mDentBuckle = IIf(Session("mDentBuckleClone") Is Nothing, mDentBuckle, Session("mDentBuckleClone"))
                        End If
                        Session.Remove("Edit")
                        Session("mDentBuckle") = mDentBuckle
                        RemoveSession()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        Response.Redirect("index.aspx")
                    End If
            End Select
        End If
    End Sub

    Private Sub Setobject()

        Try

            mDentBuckle.DentBuckleItems.CurrentItem.SrNo = mDentBuckle.DentBuckleItems.CurrentIndex + 1
            mDentBuckle.DentBuckleItems.CurrentItem.ATAID = New Guid(cmbATA.SelectedValue)

            If cmbATA.SelectedIndex > 0 Then
                Dim mATA As ATA = ATA.GetATA(New Guid(cmbATA.SelectedValue))
                mDentBuckle.DentBuckleItems.CurrentItem.ATACode = mATA.ATACode
                mDentBuckle.DentBuckleItems.CurrentItem.ATANomenclature = mATA.ATANomenclature
            Else
                mDentBuckle.DentBuckleItems.CurrentItem.ATACode = 0
                mDentBuckle.DentBuckleItems.CurrentItem.ATANomenclature = ""
            End If

            mDentBuckle.DentBuckleItems.CurrentItem.ItemNo = Trim(txtItemNo.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.Description = Trim(txtDescription.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.Reference = Trim(txtReference.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.ApprovalDoc = Trim(txtApprovalDoc.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.AcceptableDescription = Trim(txtAcceptableDescription.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.TemporaryAction = Trim(txtTemporaryAction.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.Remark = Trim(txtRemark.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.CorrectiveActionRemark = Trim(txtCorrectiveActionRemark.Text)

            If txtDateofAcceptance.Text = "" Then
                mDentBuckle.DentBuckleItems.CurrentItem.DateofAcceptance = System.DBNull.Value
            Else
                mDentBuckle.DentBuckleItems.CurrentItem.DateofAcceptance = CDate(txtDateofAcceptance.Text)
            End If
            If txtCorrectiveActionDate.Text = "" Then
                mDentBuckle.DentBuckleItems.CurrentItem.DateofCorrctiveAction = System.DBNull.Value
            Else
                mDentBuckle.DentBuckleItems.CurrentItem.DateofCorrctiveAction = CDate(txtCorrectiveActionDate.Text)
            End If

            mDentBuckle.DentBuckleItems.CurrentItem.ItemStatusID = CInt(cmbItemStatus.SelectedValue)
            mDentBuckle.DentBuckleItems.CurrentItem.ItemStatusName = cmbItemStatus.SelectedItem.Text

            'Adde by Shital on 09-Apr-2018
            mDentBuckle.DentBuckleItems.CurrentItem.DamageLocation = txtDamageLocation.Text
            mDentBuckle.DentBuckleItems.CurrentItem.Dimensions = txtDimensions.Text
            mDentBuckle.DentBuckleItems.CurrentItem.WorkOrderNo = txtWoNo.Text
            mDentBuckle.DentBuckleItems.CurrentItem.DoneAtHrCycles = txtDoneAtHrCycle.Text
            mDentBuckle.DentBuckleItems.CurrentItem.NextDueRemark = txtNextDueRemark.Text
            mDentBuckle.DentBuckleItems.CurrentItem.DentBuckleTypeID = cmbDentBuckleType.SelectedValue
            '----------
            'Ajay 18-oct-2022
            mDentBuckle.DentBuckleItems.CurrentItem.DamageTypeID = New Guid(cmbDamageTypeID.SelectedValue)
            mDentBuckle.DentBuckleItems.CurrentItem.Interval = Trim(txtInterval.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.PerformanceDetails = Trim(txtPerformanceDetails.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.NextDue = Trim(txtNextDue.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.Remaining = Trim(txtRemaining.Text)
            '----------

            'Added by Saylee on 3-Jan-2025 FLYPAL-2105
            mDentBuckle.DentBuckleItems.CurrentItem.Threshold = Trim(txtThreshold.Text)
            mDentBuckle.DentBuckleItems.CurrentItem.StationAndStringer = Trim(txtStationAndStringer.Text)
            '--------------------------------


            mDentBuckle.ApplyEdit()
            Session("mDentBuckle") = mDentBuckle

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
#End Region

#Region " Binding Methods "
    Public Sub DataFieldBind()
        mATAList = ATAList.GetATAList("", "(SELECT)")
        cmbATA.DataSource = mATAList
        Session("mATAList") = mATAList

        'Added By Ajay On 17-Oct-2022
        mDamageTypeList = DamageTypeList.GetDamageTypeList("", "(SELECT)")
        cmbDamageTypeID.DataSource = mDamageTypeList
        Session("mDamageTypeList") = mDamageTypeList

        mDentBuckleItemStatusList = DentBuckleItemStatusList.GetStatusList(False)
        cmbItemStatus.DataSource = mDentBuckleItemStatusList
        Session("mDentBuckleItemStatusList") = mDentBuckleItemStatusList

        'Added By Shital On 09-Apr-2018
        mDentBuckleTypeList = DentBuckleTypeList.GetDentBuckleTypeList(True, "(SELECT)")
        cmbDentBuckleType.DataSource = mDentBuckleTypeList
        Session("mDentBuckleTypeList") = mDentBuckleTypeList

        txtDateofAcceptance.Text = mDentBuckle.DentBuckleItems.CurrentItem.DateofAcceptanceFormatted.ToString
        txtCorrectiveActionDate.Text = mDentBuckle.DentBuckleItems.CurrentItem.DateofCorrctiveActionFormatted.ToString
        txtReportedByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.ReportedByName
        txtActionTakenByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByName
        txtAcceptanceByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByName

        DataBind()

    End Sub
    Private Sub SetTitle()
        If Session("Edit") Then
            If Len(mDentBuckle.DentBuckleItems.CurrentItem.ItemNo) > 15 Then
                lblTitle.Text = "Report [ " & mDentBuckle.DentBuckleItems.CurrentItem.ItemNo.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Report [ " & mDentBuckle.DentBuckleItems.CurrentItem.ItemNo & " ]"
            End If
        Else
            lblTitle.Text = "Report [ New ]"
        End If
    End Sub
    Public Sub customvalidate(s As Object, e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "cmbItemStatus" Then
            If cmbItemStatus.SelectedValue = "2" And txtTemporaryAction.Text = "" Then
                CustValidator.ErrorMessage = "Enter Temporary Action"
                e.IsValid = False
            ElseIf cmbItemStatus.SelectedValue = "3" And txtCorrectiveActionRemark.Text = "" Then
                CustValidator.ErrorMessage = "Enter Corrective Action Remark"
                e.IsValid = False
            End If
        End If
        If CustValidator.ControlToValidate = "txtActionTakenByEmployee" Then
            If cmbItemStatus.SelectedValue = "3" And (txtActionTakenByEmployee.Text = "" Or mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByID.Equals(Guid.Empty)) Then
                e.IsValid = False
                CustValidator.ErrorMessage = "Select Action Taken By Employee"
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub ControlVisibility()
        If Session("middleframe") = "wfDentBuckleRectificationList_Ajax.aspx?" Then 'open from recification link
            btnOK.Text = "Save"
        Else
            btnOK.Text = "Add"
        End If
        If AppSettings("ClientCode") = "CMX" Then 'Ajay 17-oct-2022
            lblType.InnerText = "Repair Category"
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            txtItemNo.Focus()
            DataFieldBind()
            SetTitle()
            ControlVisibility()
        End If
    End Sub

    Private Sub btnOk_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        If IsValid Then
            If (Session("Edit") = True And mDentBuckle.DentBuckleItems.Contains(Trim(txtItemNo.Text)) And mDentBuckle.DentBuckleItems.CurrentItem.ItemNo <> Trim(txtItemNo.Text)) Or (Session("Edit") <> True And mDentBuckle.DentBuckleItems.Contains(Trim(txtItemNo.Text))) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Item No.", MsgBoxStyle.OkOnly, "")
                mDentBuckle.CancelEdit()
                Exit Sub
            End If
            Setobject()
            If Session("middleframe") = "wfDentBuckleRectificationList_Ajax.aspx?" Then
                mDentBuckle.Save()
            End If
            RemoveSession()
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
            Response.Redirect("index.aspx")
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnBack_Click(sender As System.Object, e As System.EventArgs) Handles btnBack.Click
        If mDentBuckle.DentBuckleItems.CurrentItem.IsNew And Not Session("Edit") = True Then mDentBuckle.DentBuckleItems.Remove(mDentBuckle.DentBuckleItems.CurrentItem)

        'Setobject()
        'If mDentBuckle.IsDirty Then
        '    Session("IsValid") = "True"
        '    MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
        'Else
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("index.aspx")
        'End If


    End Sub

    'Ajay 18-oct-2022
    'Private Sub btnAddDamageList_Click( sender As System.Object,  e As System.EventArgs) Handles btnAddDamageList.Click
    '    ' If IsValid = False Then upnlValidationSummary.Update() : Exit Sub
    '    'SaveFormtoObject()
    '    'Session("mManual") = mManual
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDamageTypeListWindow", "OpenDamageTypeListWindow()", True)
    'End Sub
    Private Sub btnAddDamageList_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddDamageList.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDamageTypeListWindow", "OpenDamageTypeListWindow()", True)
    End Sub




    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Protected Sub AcceptanceByEmployee_Changed(sender As Object, e As System.EventArgs)

        Dim message As String = ""
        If hdnAcceptanceByEmpId.Value <> "" Then

            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnAcceptanceByEmpId.Value.ToString,
                                                                      mDentBuckle.ReportDateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then

                If (mEmployeeStatus(0).Information <> "") Then

                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.Custom,
                                    message,
                                    MsgBoxStyle.OkOnly,
                                    "ResetAcceptanceByEmployee")
                    Exit Sub

                End If

                mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByID = New Guid(hdnAcceptanceByEmpId.Value)
                mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByName = txtAcceptanceByEmployee.Text

            Else

                txtAcceptanceByEmployee.Text = ""
                If Not mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByID.Equals(Guid.Empty) Then

                    hdnAcceptanceByEmpId.Value = mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByID.ToString
                    txtAcceptanceByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByName

                End If

            End If

        Else

            txtAcceptanceByEmployee.Text = ""
            mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByID = Guid.Empty
            mDentBuckle.DentBuckleItems.CurrentItem.AcceptanceByName = ""

        End If

    End Sub

    Protected Sub txtActionTakenByEmployee_TextChanged(sender As Object, e As System.EventArgs)
        Dim message As String = ""
        If hdnActionTakenByEmpId.Value <> "" Then
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnActionTakenByEmpId.Value.ToString, mDentBuckle.ReportDateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetActionTakenByEmployee")
                    Exit Sub
                End If
                mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByID = New Guid(hdnActionTakenByEmpId.Value)
                mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByName = txtActionTakenByEmployee.Text
            Else
                txtActionTakenByEmployee.Text = ""
                If Not mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByID.Equals(Guid.Empty) Then
                    hdnActionTakenByEmpId.Value = mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByID.ToString
                    'SetEmpID()
                    txtActionTakenByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByName
                End If
            End If
        Else
            txtActionTakenByEmployee.Text = ""
            mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByID = Guid.Empty
            mDentBuckle.DentBuckleItems.CurrentItem.ActionTakenByName = ""
        End If
    End Sub
    Protected Sub txtReportedByEmployee_TextChanged(sender As Object, e As System.EventArgs)
        Dim message As String = ""
        If hdnReportedByEmpId.Value <> "" Then
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(hdnReportedByEmpId.Value.ToString, mDentBuckle.ReportDateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetReportedByEmployee")
                    Exit Sub
                End If
                mDentBuckle.DentBuckleItems.CurrentItem.ReportedByID = New Guid(hdnReportedByEmpId.Value)
                mDentBuckle.DentBuckleItems.CurrentItem.ReportedByName = txtReportedByEmployee.Text
            Else
                txtReportedByEmployee.Text = ""
                If Not mDentBuckle.DentBuckleItems.CurrentItem.ReportedByID.Equals(Guid.Empty) Then
                    hdnReportedByEmpId.Value = mDentBuckle.DentBuckleItems.CurrentItem.ReportedByID.ToString
                    'SetEmpID()
                    txtReportedByEmployee.Text = mDentBuckle.DentBuckleItems.CurrentItem.ReportedByName
                End If
            End If
        Else
            txtReportedByEmployee.Text = ""
            mDentBuckle.DentBuckleItems.CurrentItem.ReportedByID = Guid.Empty
            mDentBuckle.DentBuckleItems.CurrentItem.ReportedByName = ""
        End If
    End Sub
    Private Sub cmbItemStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbItemStatus.SelectedIndexChanged
        lblTempAction.Visible = IIf(cmbItemStatus.SelectedValue = "2", True, False)
        lblCorrectiveActionRemark.Visible = IIf(cmbItemStatus.SelectedValue = "3", True, False)
        lblActionTakenBy.Visible = IIf(cmbItemStatus.SelectedValue = "3", True, False)
        upnlDetails.Update()
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmployeeList(prefixText As String, count As Integer, contextKey As String) As String()
        Dim itemlist As EmpNoNameAutoComplete
        itemlist = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

    Private Sub hdnBtnManualPropertyValue_Click(sender As Object, e As System.EventArgs) Handles hdnBtnDamageTypeList.Click 'Ajay 18-oct-2022
        cmbDamageTypeID.DataSource = DamageTypeList.GetDamageTypeList(, "(SELECT)")
        cmbDamageTypeID.DataBind()
        upnlDetails.Update()
    End Sub


End Class