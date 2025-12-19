
'Created by : Saylee
'Date       : 21-Dec-2009

Partial Class wfDailyStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mDailyStatusList As DailyStatusList
    Private mDailyStatusCertificateList As DailyStatusList
    Private mDailyStatus As DailyStatus
    Private mBoardTypeList As AircraftInformationBoard.BoardTypeList
    Private mMaintenanceActivityTypeList As MaintenanceActivityTypeList

    Private mAssemblylist As AssemblyList
    Private mModelListForCombo As ModelListForCombo

    Dim MachineName As String
    Dim ModelName As String
    Dim Model As String
    Dim Assembly1 As String
    Dim Aircraft As String
    Dim MaintenanceActivityType As Integer = 0
    Dim RegNo As String
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDailyStatusList = Session("mDailyStatusList")
        mDailyStatus = Session("mDailyStatus")
        mAssemblylist = Session("mAssemblyList")
        mModelListForCombo = Session("mModelListForCombo")
        mDailyStatusCertificateList = Session("mDailyStatusCertificateList")

        MachineName = Session("AircraftId")
        Model = Session("ModelId")
        ModelName = Session("ModelName")
        Aircraft = Session("Aircraft")
        MaintenanceActivityType = Session("MaintenanceActivityType")
        RegNo = Session("RegNo")

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDailyStatus_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub SetSession()
        Session("mDailyStatusList") = mDailyStatusList
        Session("mDailyStatus") = mDailyStatus
        Session("mBoardTypeList") = mBoardTypeList
        Session("mAssemblyList") = mAssemblylist
        Session("mModelListForCombo") = mModelListForCombo
        Session("MaintenanceActivityType") = MaintenanceActivityType
        Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
    End Sub
    Private Sub RemoveSession()
        ' Session.Remove("mDailyStatusList")
        Session.Remove("mDailyStatus")
        Session.Remove("BoardType")
        Session.Remove("mAssemblyList")
        Session.Remove("mMachineList")
        Session.Remove("mAssemblyList")
        Session.Remove("mModelListForCombo")
        Session.Remove("MaintenanceActivityType")
        Session.Remove("ModelId")
        Session.Remove("ModelName")
        Session.Remove("Aircraft")
        Session.Remove("PartId")
        Session.Remove("PartName")
        Session.Remove("RegNo")
        'Session.Remove("mDailyStatusCertificateList")
    End Sub
    Private Sub DeleteDailyStatus(ByVal Index As Int32)
        ' mDailyStatus = AircraftDailyStatus.DailyStatus.GetChildDailyStatus(mDailyStatusList(Index).ID)
        'Session("mDailyStatus") = mDailyStatus
        Session("Index") = Index
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub DeleteDailyStatusCertificate(ByVal Index As Int32)
        Session("Index") = Index
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteCertificate")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetPage()
        If cmbMaintenanceActivityType.SelectedValue <> "7" And Not mDailyStatusList Is Nothing Then
            lblResult.Text = "List of Maintenance Activities: " & mDailyStatusList.Count & " Record(s) found"
        ElseIf cmbMaintenanceActivityType.SelectedValue = "7" And Not mDailyStatusCertificateList Is Nothing Then
            lblResult.Text = "List of Certificates: " & mDailyStatusCertificateList.Count & " Record(s) found"
        End If

        lbltitle.Text = "Daily Status [" + RegNo + "]"
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
                            'mDailyStatus = Session("mDailyStatus")
                            Dim Index As Integer = CType(Session("Index"), Integer)
                            ''If mDailyStatusList.Count = 1 Then
                            mDailyStatus = DailyStatus.GetChildDailyStatus(mDailyStatusList(Index).ID)
                            mDailyStatus.Delete()
                            mDailyStatus.Save()
                            '' End If
                            mDailyStatusList.Remove(Index)


                            'DataFieldBind()

                            dgDailyStatusList.DataSource = mDailyStatusList
                            dgDailyStatusList.DataBind()
                            Session("mDailyStatusList") = mDailyStatusList
                            Session("FromSelectInfo") = "FromSelectInfo"
                            upnlGrid.Update()

                            'Response.Redirect("wfDailyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx" & "&MachineID=" & MachineName.ToString & "&MaintenanceActivityTypeID=" & MaintenanceActivityType.ToString & "&ModelID=" & Model.ToString & "&RegNo=" & RegNo)
                            'Response.Redirect("wfDailyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx" & "&MachineID=" & MachineName.ToString & "&MaintenanceActivityTypeID=" & MaintenanceActivityType.ToString & "&RegNo=" & RegNo)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                 MarkLog(Util.Action.Delete, "Daily Status", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mDailyStatus.ID, EventLogID)
                                 MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Daily Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteCertificate" Then
                        Try
                            Session("sender") = ""
                            Dim Index As Integer = CType(Session("Index"), Integer)
                            ''  If mDailyStatusCertificateList.Count = 1 Then
                            mDailyStatus = DailyStatus.GetChildDailyStatus(mDailyStatusCertificateList(Index).ID)
                            mDailyStatus.Delete()
                            mDailyStatus.Save()
                            '' End If
                            mDailyStatusCertificateList.Remove(Index)
                            dgDailyStatusCertificateList.DataSource = mDailyStatusCertificateList
                            dgDailyStatusCertificateList.DataBind()
                            Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
                            Session("FromSelectInfo") = "FromSelectInfo"
                            upnlGrid.Update()
                            'Response.Redirect("wfDailyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx" & "&MachineID=" & MachineName.ToString & "&MaintenanceActivityTypeID=" & MaintenanceActivityType.ToString & "&ModelID=" & Model.ToString & "&RegNo=" & RegNo)
                            'Response.Redirect("wfDailyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx" & "&MachineID=" & MachineName.ToString & "&MaintenanceActivityTypeID=" & MaintenanceActivityType.ToString & "&RegNo=" & RegNo)
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                 MarkLog(Util.Action.Delete, "DailyStatus", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mDailyStatus.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "DailyStatus", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""
                    Session("sender") = ""
                    Session("FromSelectInfo") = "FromSelectInfo"
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfDailyStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx" & "&MachineID=" & MachineName.ToString & "&MaintenanceActivityTypeID=" & MaintenanceActivityType.ToString & "&ModelID=" & Model.ToString & "&RegNo=" & RegNo)
            'Response.Redirect("wfDailyStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx" & "&MachineID=" & MachineName.ToString & "&MaintenanceActivityTypeID=" & MaintenanceActivityType.ToString & "&RegNo=" & RegNo)

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetValues()
        'If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
        '    MachineName = "{00000000-0000-0000-0000-000000000000}"
        '    'AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        '    Model = Guid.Empty.ToString
        '    ModelName = ""
        'Else
        'MachineName = cmbAircraft.SelectedValue.ToString

        If cmbModel.SelectedItem.Text = "(All)" Or (cmbModel.SelectedItem.Text = "(SELECT)") Then
            Model = Guid.Empty.ToString
            ModelName = ""
        Else
            Model = cmbModel.SelectedValue.ToString
            ModelName = cmbModel.SelectedItem.Text
        End If
        '' End If

        If (cmbModel.SelectedItem.Text = "(All)") Or (cmbModel.SelectedItem.Text = "(SELECT)") Then
            Model = "{00000000-0000-0000-0000-000000000000}"
        Else
            Model = cmbModel.SelectedValue.ToString
        End If

        ''Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        Aircraft = RegNo
        MaintenanceActivityType = IIf(cmbMaintenanceActivityType.SelectedIndex > 0, CType(cmbMaintenanceActivityType.SelectedValue, Integer), 0)

        Session("AircraftId") = MachineName
        Session("ModelId") = Model
        Session("ModelName") = ModelName
        Session("Aircraft") = Aircraft
        Session("MaintenanceActivityType") = MaintenanceActivityType
    End Sub
    Private Sub ControlVisibility()
        If cmbMaintenanceActivityType.SelectedValue = "7" Then
            dgDailyStatusCertificateList.Visible = True
            dgDailyStatusList.Visible = False
            dgDailyStatusCertificateList.ShowHeaderWhenEmpty = True

            ''If Not mDailyStatusCertificateList Is Nothing Then
            ''    If mDailyStatusCertificateList.Count >= 1 Then
            ''        btnSave.Visible = True
            ''    Else
            ''        btnSave.Visible = False
            ''    End If
            ''End If
        Else
            dgDailyStatusList.Visible = True
            dgDailyStatusCertificateList.Visible = False
            dgDailyStatusCertificateList.ShowHeaderWhenEmpty = False

            ''If Not mDailyStatusList Is Nothing Then
            ''    If mDailyStatusList.Count >= 1 Then
            ''        btnSave.Visible = True
            ''    Else
            ''        btnSave.Visible = False
            ''    End If
            ''End If
        End If



    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        ''mBoardTypeList = AircraftInformationBoard.BoardTypeList.GetBoardTypeList()
        ''cmbBoardType.DataSource = mBoardTypeList
        ''cmbBoardType.DataBind()


        Dim mMaintenanceActivityTypeList As MaintenanceActivityTypeList = MaintenanceActivityTypeList.GetMaintenanceActivityTypeList(True)
        cmbMaintenanceActivityType.DataSource = mMaintenanceActivityTypeList
        cmbMaintenanceActivityType.DataBind()

        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, New Guid(MachineName).ToString, Today.Date.ToString, , True)
        mModelListForCombo = ModelListForCombo.GetModelListForCombo(mAssemblylist, "(SELECT)")
        Session("mModelListForCombo") = mModelListForCombo
        cmbModel.DataSource = mModelListForCombo
        cmbModel.DataBind()

        If Session("FromSelectInfo") = "FromSelectInfo" Then
            mDailyStatusList = Session("mDailyStatusList")
            mDailyStatusCertificateList = Session("mDailyStatusCertificateList")
            Session("FromSelectInfo") = ""
        Else
            If mDailyStatusList Is Nothing Then
                ''mDailyStatusList = DailyStatusList.GetDailyStatusList()
                ''Session("mDailyStatusList") = mDailyStatusList
            Else
                mDailyStatusList = Session("mDailyStatusList")
                mDailyStatusCertificateList = Session("mDailyStatusCertificateList")
            End If
        End If

        dgDailyStatusList.DataSource = mDailyStatusList
        Session("mDailyStatusList") = mDailyStatusList
        dgDailyStatusCertificateList.DataSource = mDailyStatusCertificateList
        Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
        DataBind()

        If Not MachineName Is Nothing And Not MachineName = Guid.Empty.ToString Then
            ''cmbAircraft.SelectedValue = MachineName
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, MachineName, Today.Date.ToString)
            mModelListForCombo = ModelListForCombo.GetModelListForCombo(mAssemblylist, "(SELECT)")

            cmbModel.DataSource = mModelListForCombo
            cmbModel.DataBind()
            If Not Model Is Nothing And Model <> Guid.Empty.ToString And MaintenanceActivityType <> 7 Then
                cmbModel.SelectedValue = Model
            End If
        End If

        cmbMaintenanceActivityType.SelectedValue = MaintenanceActivityType
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        '' ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            ''Session("MiddleFrame") = "wfDailyStatus_Ajax.aspx?"
            RegNo = Request.QueryString("RegNo")
            MachineName = Request.QueryString("MachineID")
            Session("RegNo") = RegNo
            Session("AircraftId") = MachineName
            DataFieldBind()
            setFocus(cmbMaintenanceActivityType)
            SetPage()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not IsValid Then Exit Sub

        SetValues()
        If mDailyStatusList Is Nothing Then
            mDailyStatusList = DailyStatusList.GetDailyStatusList(New Guid(MachineName.ToString), Model, Guid.Empty.ToString, cmbMaintenanceActivityType.SelectedValue)
        End If

        If mDailyStatusCertificateList Is Nothing Then
            mDailyStatusCertificateList = DailyStatusList.GetDailyStatusList(New Guid(MachineName.ToString), Model, Guid.Empty.ToString, cmbMaintenanceActivityType.SelectedValue, True)
        End If

        Session("mDailyStatusList") = mDailyStatusList
        Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
        Session("mAircraftDSCList") = mDailyStatusCertificateList
        Session("mAircraftDailyStatusList") = mDailyStatusCertificateList

        Dim str As String
        ''str = "<script language='javascript'>openledgersame('wfSelectDailyStatus.aspx?BackPage=Index.aspx" & "&ChildPage=" & Request.QueryString("ChildPage") & "&MachineID=" & cmbAircraft.SelectedValue.ToString & "&MaintenanceActivityTypeID=" & cmbMaintenanceActivityType.SelectedValue & "&ModelID=" & mAssemblylist(New Guid(cmbAssembly.SelectedValue.ToString)).ModelID.ToString & "'); </script>"
        str = "openledgersame('wfSelectDailyStatus_Ajax.aspx?ChildPage=wfDailyStatus_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & MachineName.ToString & "&MaintenanceActivityTypeID=" & cmbMaintenanceActivityType.SelectedValue & "&ModelID=" & New Guid(cmbModel.SelectedValue.ToString).ToString & "&RegNo=" & RegNo & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        If Not IsValid Then Exit Sub

        SetValues()
        mDailyStatusList = DailyStatusList.GetDailyStatusList(New Guid(MachineName.ToString), Model, Guid.Empty.ToString, cmbMaintenanceActivityType.SelectedValue)
        Session("mDailyStatusList") = mDailyStatusList

        mDailyStatusCertificateList = DailyStatusList.GetDailyStatusList(New Guid(MachineName.ToString), Guid.Empty.ToString, Guid.Empty.ToString, cmbMaintenanceActivityType.SelectedValue, True)
        Session("mDailyStatusCertificateList") = mDailyStatusCertificateList

        SetPage()
        dgDailyStatusList.DataSource = mDailyStatusList
        dgDailyStatusList.DataBind()

        dgDailyStatusCertificateList.DataSource = mDailyStatusCertificateList
        dgDailyStatusCertificateList.DataBind()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        mDailyStatusList = Session("mDailyStatusList")
        mDailyStatusCertificateList = Session("mDailyStatusCertificateList")
        Try
            ''If (Not User.IsInRole("DailyStatusNew")) Then 'Or (Not User.IsInRole("DailyStatusEdit")) Then
            ''    SetSession()
            ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            ''    msg.ReplacePage = "wfDailyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1")
            ''    Session("sender") = "Authorization"
            ''    msg.Show()
            ''    Exit Sub
            ''End If

            If mDailyStatusList.Count >= 1 Then
                mDailyStatusList.Save()
                Session("mDailyStatusList") = mDailyStatusList
                ''  Session("mAircraftDailyStatusList") = mDailyStatusList
            End If

            If mDailyStatusCertificateList.Count >= 1 Then
                mDailyStatusCertificateList.Save()
                Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
                ''  Session("mAircraftDSCList") = mDailyStatusCertificateList
            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub dgDailyStatusCertificateList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDailyStatusCertificateList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgDailyStatusCertificateList.PageSize * dgDailyStatusCertificateList.PageIndex
                ''If (Not User.IsInRole("DailyStatusDelete")) Then
                'SetSession()
                ' Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                'msg.ReplacePage = "wfDailyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx"
                'Session("sender") = "Authorization"
                'msg.Show()
                ''Exit Sub
                ''Else
                DeleteDailyStatusCertificate(Index)
                ''End If
        End Select
    End Sub
    Private Sub dgDailyStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDailyStatusList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgDailyStatusList.PageSize * dgDailyStatusList.PageIndex
                ''If (Not User.IsInRole("DailyStatusDelete")) Then
                'SetSession()
                ' Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                'msg.ReplacePage = "wfDailyStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfDailyStatus_Ajax.aspx"
                'Session("sender") = "Authorization"
                'msg.Show()
                ''Exit Sub
                ''Else
                DeleteDailyStatus(Index)
                ''End If
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        ''Session("MiddleFrame") = ""
        ''Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub dgDailyStatusCertificateList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDailyStatusCertificateList.PageIndexChanging
        dgDailyStatusCertificateList.PageIndex = e.NewPageIndex
        dgDailyStatusCertificateList.DataSource = mDailyStatusCertificateList
        Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
        dgDailyStatusCertificateList.DataBind()
    End Sub
    Private Sub dgDailyStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDailyStatusList.PageIndexChanging
        dgDailyStatusList.PageIndex = e.NewPageIndex
        dgDailyStatusList.DataSource = mDailyStatusList
        Session("mDailyStatusList") = mDailyStatusList
        dgDailyStatusList.DataBind()
    End Sub
    Private Sub dgDailyStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDailyStatusList.Sorting
        mDailyStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        mDailyStatusList = Session("mDailyStatusList")
        dgDailyStatusList.DataSource = mDailyStatusList
        dgDailyStatusList.DataBind()
    End Sub
    Private Sub dgDailyStatusCertificateList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDailyStatusCertificateList.Sorting
        mDailyStatusCertificateList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        mDailyStatusCertificateList = Session("mDailyStatusCertificateList")
        dgDailyStatusCertificateList.DataSource = mDailyStatusCertificateList
        dgDailyStatusCertificateList.DataBind()
    End Sub
    Private Sub cmbMaintenanceActivityType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMaintenanceActivityType.SelectedIndexChanged
        If cmbMaintenanceActivityType.SelectedValue <> "7" Then
            lblLabelStar.Visible = True
        Else
            lblLabelStar.Visible = False
        End If
        setFocus(cmbMaintenanceActivityType)
        SetPage()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

   
End Class
