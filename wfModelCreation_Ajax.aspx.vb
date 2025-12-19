Public Class wfModelCreation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mModel As Model
    Public mManufacturerList As ManufacturerList
    Public mAssemblyTypeList As AssemblyTypeList
    Public mMachineNameValueList As MachineNameValueList
    Public mMachine As Machine

    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyStatusList As tmpAssemblyStatusList

    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList

    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList

    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList

    Public mPrimaryModelList As PrimaryModelList
    Public mAssemblyTypeId As Integer
    Public Type As Boolean = False

#End Region

#Region " Helper Methods "

    Private Sub GetSession()

        mModel = Session("mModel")
        mAssemblyTypeList = Session("mAssemblyTypeList")
        mManufacturerList = Session("mManufacturerList")
        mAssemblyTypeId = CType(Session("AssemblyTypeId"), Integer)
        Type = CType(Session("Type"), Boolean)

    End Sub

    Private Sub SetSession()

        Session("mModel") = mModel
        Session("mAssemblyTypeList") = mAssemblyTypeList
        Session("mManufacturerList") = mManufacturerList
        Session("mAssemblyTypeId") = mAssemblyTypeId
        Session("Type") = Type

    End Sub

    Private Sub SetObject()
        mModel.ManufacturerID = New Guid(cmbManufacturerList.SelectedValue)
        mModel.Name = txtName.Text
        mModel.AssemblyTypeID = Val(cmbForAssemblyList.SelectedValue)

        If cmbPrimaryModelList.SelectedIndex <> 0 Then

            mModel.PrimaryModelID = New Guid(cmbPrimaryModelList.SelectedValue)
            mModel.PrimaryModelName = cmbPrimaryModelList.SelectedItem.Text

        End If

        Session("mModel") = mModel
    End Sub
    Private Sub DisableName() 'Added by : Saylee 17-Jun-2020, ALL16062020
        If Not mModel.IsNew Then
            Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerModel(mModel.ID)
            If Not mTransCountAsPerMasters Is Nothing Then
                txtName.Enabled = mTransCountAsPerMasters.Count = 0
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "AircraftNotConfigured" Then
                        Session("ActiveTabIndex") = 0
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallZerothActiveTabIndex", "CallZerothActiveTabIndex();", True)
                        Exit Sub
                    End If
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetTitle()
        If mModel.IsNew Then
            lblTitle.Text = "Model Information [New]"
        Else
            lblTitle.Text = "Model Information [" & mModel.Name & "]"
        End If
        upnlTitle.Update()
    End Sub

    Private Sub ControlVisibility()

        'If mModel.AssemblyTypeID = 1 And mCompanyDetail.IsSyncApplication = True Then
        If mModel.AssemblyTypeID = 1 Then
            PrimaryModelPlaceHolder.Visible = True
        Else
            PrimaryModelPlaceHolder.Visible = False
        End If

    End Sub

#End Region

#Region " Data Binding "

    Public Sub DataFieldBind()

        mManufacturerList = ManufacturerList.GetManufacturerList(, "(SELECT)")
        cmbManufacturerList.DataSource = mManufacturerList
        Session("mManufacturerList") = mManufacturerList
        Session("mAssemblyTypeList") = mAssemblyTypeList
        mAssemblyTypeList = AssemblyTypeList.GetAssemblyTypeList()
        cmbForAssemblyList.DataSource = mAssemblyTypeList
        mPrimaryModelList = PrimaryModelList.GetPrimaryModelList(AddTopItem:="(SELECT)")
        cmbPrimaryModelList.DataSource = mPrimaryModelList

        DataBind()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("Sender") = "" Then

            Type = CType(Session("Type"), Boolean)
            mAssemblyTypeId = CType(Session("AssemblyTypeId"), Integer)
            Session("Type") = Type
            Session("mAssemblyTypeId") = mAssemblyTypeId

            SetFocus(cmbManufacturerList)
            DataFieldBind()
            SetTitle()
            ControlVisibility()

        End If

        SetSession()
        DisableName()

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("OpenFromModelCreation")
        Session.Remove("ModelIDFromModelCreation")
        Session.Remove("ModelNameFromModelCreation")
        Session.Remove("ActiveTabIndex")
        Response.Redirect("index.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If IsValid = True Then
            Try
                SetObject()
                mModel.Save()
                If cmbManufacturerList.Enabled = True Then
                    SetFocus(cmbManufacturerList)
                End If
                MarkLog(Util.Action.Save, "Model", mModel.Name, Util.ErrorType.HandledError, mModel.ID, EventLogID)
                DataFieldBind()
                SetSession()
                SetTitle()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                End If
            End Try
        Else
            upnlValidationSummary.Update()
            Exit Sub
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub imgbtnManufacturer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnManufacturer.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenManufacturerWindow", "OpenManufacturerWindow()", True)
    End Sub
    Private Sub htnBtnManufacturer_Click(sender As Object, e As System.EventArgs) Handles htnBtnManufacturer.Click
        DataFieldBind()
        upnlModelInformation.Update()
    End Sub
    Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As System.EventArgs) Handles TabContainer1.ActiveTabChanged
        Session("OpenFromModelCreation") = "True"
        Session("ModelIDFromModelCreation") = mModel.ID
        Session("ModelNameFromModelCreation") = mModel.Name

        Select Case TabContainer1.ActiveTabIndex
            Case 0
            Case 1
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModelMonitorServiceListWindow", "OpenModelMonitorServiceListWindow();", True)
            Case 2
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModelMonitorInspListWindow", "OpenModelMonitorInspListWindow();", True)
            Case 3
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModelMonitorModListWindow", "OpenModelMonitorModListWindow();", True)
        End Select

    End Sub

    Private Sub AddPrimaryModel1(sender As Object, e As EventArgs) Handles imgbtnPrimaryModel.Click

        Try

            Session("mAssemblyTypeId") = mAssemblyTypeId
            Session("Type") = Type
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenPrimaryModelWindow",
                                                "OpenPrimaryModelWindow()",
                                                True)

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub AssemblyList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbForAssemblyList.SelectedIndexChanged

        Try

            If cmbForAssemblyList.SelectedIndex = 0 Then
                PrimaryModelPlaceHolder.Visible = True
            Else
                PrimaryModelPlaceHolder.Visible = False
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

End Class