Public Class wfItemServiceInspections_Ajax
    Inherits System.Web.UI.Page
#Region " Variables and Declarations "
    Dim mMaintTypeID As Integer
    Dim mItemID As Guid
    Public mItem As Item
    Public mServiceInspectionsList As ItemServiceInspectionsList
    Public mServiceInspections As ItemServiceInspections
    Dim Description As String
    Dim Frequency, FrequencyIn As Integer
    Public mCalibrationPeriodInList As CalibrationPeriodInList
    Public mServiceInspectionNameList As ServiceInspectionNameList
#End Region

#Region " Methods "
    Private Sub GetSession()
        'mMaintenanceID = CType(Session("mMaintenanceID"), Guid)
        'LicenseNo = Session("LicenseNo")
        'DoneByID = Session("EmployeeID")

        mItem = Session("mItem")
        mServiceInspectionsList = CType(Session("mServiceInspectionsList"), ItemServiceInspectionsList)
        mMaintTypeID = Session("mMaintTypeID")
        mItemID = Session("mItemID")
        Description = Session("Description")
        Frequency = Session("Frequency")
        FrequencyIn = Session("FrequencyIn")
        mCalibrationPeriodInList = Session("mCalibrationPeriodInList")
        mServiceInspections = Session("mServiceInspections")
        mServiceInspectionNameList = Session("mServiceInspectionNameList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemID")
        Session.Remove("Description")
        Session.Remove("Frequency")
        Session.Remove("FrequencyIn")
        Session.Remove("mCalibrationPeriodInList")
        Session.Remove("mServiceInspections")
        Session.Remove("mServiceInspectionNameList")
    End Sub
    '    Private Sub setControls(ID As Guid)
    '        'txtLicenceNo.Text = mMaintenanceDoneByEmployees.CurrentItem.LicenceNo + " [" + mMaintenanceDoneByEmployees.CurrentItem.EmployeeName + "]"
    '        'txtRequiredManHours.Text = mMaintenanceDoneByEmployees.CurrentItem.RequiredManHours

    '        txtLicenceNo.Text = mMaintenanceDoneByEmployees(ID).LicenceNo + " [" + mMaintenanceDoneByEmployees(ID).EmployeeName + "]"
    '        txtRequiredManHours.Text = mMaintenanceDoneByEmployees(ID).RequiredManHours
    '    End Sub
    Private Sub ClearControls()
        'txtDescription.Text = String.Empty
        cmbServiceInspectionName.SelectedIndex = 0
        txtFrequency.Text = String.Empty
        cmbServiceInspectionIntervalIn.SelectedIndex = 0
    End Sub

    Private Sub SetTitle()
        lblResult.Text = "List of Service Inspections Nos. : " + mServiceInspectionsList.Count.ToString + " Record(s) found."
        'btnAddTop.Visible = mServiceInspectionsList.Count > 8
        'btnCloseTop.Visible = mServiceInspectionsList.Count > 8
    End Sub
    Private Sub addattributes1()
        txtFrequency.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtFrequency.ClientID + "').value,event)")

    End Sub
#End Region
#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addattributes1()

        If Not IsPostBack Then
            mMaintTypeID = CType(Request.QueryString("MaintTypeID"), Integer)
            Session("mMaintTypeID") = mMaintTypeID
            DataFieldBind()
            BindGrid()
            SetTitle()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click ', btnCloseTop.Click
        'RemoveSession()
        Session.Remove("EditItem")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub imgServiceInspections_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgServiceInspections.Click
        If IsValid Then
            Try
                ' setObject()
                Session("mItem") = mItem
                Session("mItemID") = mItem.ID
                mServiceInspectionsList = mItem.ItemServiceInspectionsList
                Session("mServiceInspectionsList") = mServiceInspectionsList
                Session("mServiceInspections") = mServiceInspections
                Session("mServiceInspectionNameList") = mServiceInspectionNameList
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddServiceInspections", "AddServiceInspections();", True)

            Catch ex As Exception
                MSGBoxCtrl.show("Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
            End Try

        End If
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click ', btnAddTop.Click

        If IsValid Then
            If (Not User.IsInRole("PartNew") And mItem.IsNew) Or (Not User.IsInRole("PartEdit") And Not mItem.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
                DataFieldBind()
                Exit Sub
            End If

            NewRecord()
            BindGrid()
            'DataFieldBind()
        Else
            upnlValidationSummary.Update()

        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbServiceInspectionName" Then
            If cmbServiceInspectionName.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Service Inspection Required."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtFrequency" Then
            If txtFrequency.Text = "" Then
                custValidator.ErrorMessage = "Frequency Required."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "cmbServiceInspectionIntervalIn" Then
            If cmbServiceInspectionIntervalIn.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Frequency Interval Required."
                e.IsValid = False
            End If
        End If
    End Sub

    Private Sub NewRecord()



        If Session("EditItem") = False Then
            If Not mItem.ItemServiceInspectionsList.Contains(cmbServiceInspectionName.SelectedItem.ToString, mItem.ID) Then
                mItem.ItemServiceInspectionsList.Add(mItem.ID)
                mItem.ItemServiceInspectionsList.CurrentItem.Description = cmbServiceInspectionName.SelectedItem.ToString 'txtDescription.Text
                mItem.ItemServiceInspectionsList.CurrentItem.ServiceInspectionNameID = New Guid(cmbServiceInspectionName.SelectedValue)
                mItem.ItemServiceInspectionsList.CurrentItem.Frequency = txtFrequency.Text
                mItem.ItemServiceInspectionsList.CurrentItem.FrequencyPeriod = cmbServiceInspectionIntervalIn.SelectedValue

            Else
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.Information, "")
            End If
        Else
            mItem.ItemServiceInspectionsList.Item(mItem.ItemServiceInspectionsList.CurrentIndex).Description = cmbServiceInspectionName.SelectedItem.ToString 'txtDescription.Text
            mItem.ItemServiceInspectionsList.Item(mItem.ItemServiceInspectionsList.CurrentIndex).ServiceInspectionNameID = New Guid(cmbServiceInspectionName.SelectedValue)
            mItem.ItemServiceInspectionsList.Item(mItem.ItemServiceInspectionsList.CurrentIndex).Frequency = txtFrequency.Text
            mItem.ItemServiceInspectionsList.Item(mItem.ItemServiceInspectionsList.CurrentIndex).FrequencyPeriod = cmbServiceInspectionIntervalIn.SelectedValue

        End If


        'mServiceInspections.Description = cmbServiceInspectionName.SelectedItem.ToString
        'mServiceInspections.ServiceInspectionNameID = New Guid(cmbServiceInspectionName.SelectedValue)
        'mServiceInspections.Frequency = txtFrequency.Text
        'mServiceInspections.FrequencyPeriod = cmbServiceInspectionIntervalIn.SelectedValue

        'mItem.ItemServiceInspectionsList.Add(mServiceInspections)

        Session("mItem") = mItem
        Session.Remove("EditItem")
        ClearControls()
    End Sub
    Private Function setObject() As Boolean

        mItem.ItemServiceInspectionsList.CurrentItem.Description = cmbServiceInspectionName.SelectedItem.ToString
        mItem.ItemServiceInspectionsList.CurrentItem.ServiceInspectionNameID = New Guid(cmbServiceInspectionName.SelectedValue)
        mItem.ItemServiceInspectionsList.CurrentItem.Frequency = txtFrequency.Text
        mItem.ItemServiceInspectionsList.CurrentItem.FrequencyPeriod = cmbServiceInspectionIntervalIn.SelectedValue

        If mItem.ItemServiceInspectionsList.Contains(mItem.ItemServiceInspectionsList.CurrentItem) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "service Inspection", MsgBoxStyle.Information, "")
            Return False
        End If
        Return True
    End Function

#End Region

#Region " Data Binding "
    Private Sub BindGrid()
        dgServiceInspectionsList.DataSource = mItem.ItemServiceInspectionsList
        dgServiceInspectionsList.DataBind()
        SetTitle()
        upnlDetails.Update()
    End Sub
    Private Sub DataFieldBind()

        mServiceInspectionNameList = ServiceInspectionNameList.GetServiceInspectionList("", "SELECT")
        Session("mServiceInspectionNameList") = mServiceInspectionNameList
        cmbServiceInspectionName.DataSource = mServiceInspectionNameList
        cmbServiceInspectionName.DataBind()

        mCalibrationPeriodInList = CalibrationPeriodInList.GetCalibrationPeriodInList("(SELECT)")
        Session("mCalibrationPeriodInList") = mCalibrationPeriodInList
        cmbServiceInspectionIntervalIn.DataSource = mCalibrationPeriodInList
        cmbServiceInspectionIntervalIn.DataBind()
        BindGrid()
        upnlDetails.Update()

    End Sub
    Private Sub DeleteRecord(ByVal Idx As Integer)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mItem.ItemServiceInspectionsList.CurrentIndex = Idx
        Session("mItem") = mItem
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mItem = Session("mItem")
                            mItem.ItemServiceInspectionsList.Remove(mItem.ItemServiceInspectionsList.CurrentItem)
                            mItem.ItemServiceInspectionsList.CurrentIndex = mItem.ItemServiceInspectionsList.Count - 1
                            Session("mItem") = mItem
                            BindGrid()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.Information, "")
                            End If
                            BindGrid()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        mItem.ItemServiceInspectionsList.CurrentIndex = mItem.ItemServiceInspectionsList.Count - 1
                        Session("mItem") = mItem
                        Session("sender") = ""
                        BindGrid()
                    End If
                Case MsgBoxResult.Ok ' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""

        End If
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mServiceInspections = ItemServiceInspections.GetItemServiceInspections(mId)
        Session("mServiceInspections") = mServiceInspections
        cmbServiceInspectionName.SelectedValue = mItem.ItemServiceInspectionsList.Item(mId).ServiceInspectionNameID.ToString
        txtFrequency.Text = mItem.ItemServiceInspectionsList.Item(mId).Frequency.ToString
        cmbServiceInspectionIntervalIn.SelectedValue = mItem.ItemServiceInspectionsList.Item(mId).FrequencyPeriod.ToString
        Session("EditItem") = True
    End Sub
    Private Sub dgServiceInspectionsList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgServiceInspectionsList.RowCommand
        Dim Idx As Int32
        Dim mId As Guid

        Select Case e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgServiceInspectionsList.PageIndex * dgServiceInspectionsList.PageSize
                mItem.ItemServiceInspectionsList.CurrentIndex = Idx
                mId = mServiceInspectionsList(Idx).ID

                EditRecord(mId)
                'txtDescription.Text = mServiceInspectionsList.CurrentItem.Description
                cmbServiceInspectionName.SelectedValue = mServiceInspectionsList(mId).ServiceInspectionNameID.ToString
                cmbServiceInspectionName.DataBind()
                txtFrequency.Text = mServiceInspectionsList(mId).Frequency
                cmbServiceInspectionIntervalIn.SelectedValue = mServiceInspectionsList(mId).FrequencyPeriod
                cmbServiceInspectionIntervalIn.DataBind()

                MarkLog(Util.Action.Edit, "Service Inspections", mServiceInspectionsList.CurrentItem.Description, Util.ErrorType.NoError, mServiceInspectionsList.CurrentItem.ID, EventLogID)

            Case "DeleteRec"
                Dim index As Integer = CInt(e.CommandArgument) + dgServiceInspectionsList.PageIndex * dgServiceInspectionsList.PageSize
                If (Not User.IsInRole("PartDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.Information, "")
                    DataFieldBind()
                    Exit Sub
                End If
                DeleteRecord(index)
                txtFrequency.Text = ""
                DataFieldBind()
        End Select
    End Sub
    Private Sub hdnBtnServiceInspactionsName_Click(sender As Object, e As System.EventArgs) Handles hdnBtnServiceInspactionsName.Click
        DataFieldBind()
    End Sub
#End Region

End Class