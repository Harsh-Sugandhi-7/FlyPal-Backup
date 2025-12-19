'Added By Vikrant On 27-Jul-2020 For ALL27072020
Public Class wfSpareAssemblyListForInstallation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mSpareAssemblyList As SpareAssemblyList
    Public mAssemblyStatus As AssemblyStatus
    Dim EventLogID As Guid
    Public mAssemblyType As String
    Dim mFileAttach As FileAttach
    Dim mMachine As Machine
    Dim mInstalledAssemblyStatusList As tmpInstalledAssemblyList
    Dim mMachineNameValueList As MachineNameValueList
    Dim IsReadOnly As Boolean
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mSpareAssemblyList = CType(Session("mSpareAssemblyList"), SpareAssemblyList)
        mMachineNameValueList = Session("mMachineNameValueList")
        IsReadOnly = Session("IsReadOnly")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSpareAssemblyList")
        Session.Remove("mMachineNameValueList")
        Session("IsReadOnly") = IsReadOnly
    End Sub
    Private Sub ControlVisibility()
        IsReadOnly = mMachineNameValueList(New Guid(cmbInstallOnMachine.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly
        If IsReadOnly = True Then
            lblReadOnly.Visible = True
        Else
            lblReadOnly.Visible = False
        End If

        For j As Integer = 0 To dgBuiltSpareList.Rows.Count - 1
            If IsReadOnly Then
                dgBuiltSpareList.Rows(j).Cells(5).Enabled = False
            Else
                dgBuiltSpareList.Rows(j).Cells(5).Enabled = True
            End If
            '*************************
        Next

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes


                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '   DataFieldBind()
        End If
    End Sub
    Public Function CheckPeriodsForRemovedAssemblyStatus(ByVal RemovedAssemblyStatus As AssemblyStatus, ByVal Mac As Machine) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        If RemovedAssemblyStatus.AssemblyTypeID = 2 Or RemovedAssemblyStatus.AssemblyTypeID = 4 Then Return True
        While i <= RemovedAssemblyStatus.AssemblyStatusPeriods.Count - 1
            If Mac.AssemblyStatus.AssemblyStatusPeriods.Contains(RemovedAssemblyStatus.AssemblyStatusPeriods(i).PeriodID) Then
                tmpIsPeriodExists = True
            Else
                tmpIsPeriodExists = False
                Exit While
            End If
            i = i + 1
        End While
        Return tmpIsPeriodExists
    End Function
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        txtInstallationDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True)
        cmbInstallOnMachine.DataSource = mMachineNameValueList

        mSpareAssemblyList = SpareAssemblyList.GetSparedAssemblyList(IsPeriodValuesRequired:=True)
        Session("mSpareAssemblyList") = mSpareAssemblyList
        dgBuiltSpareList.DataSource = mSpareAssemblyList

        DataBind()
        lblBuiltSpareAssembly.Text = "List of Built assembly " & " : " & mSpareAssemblyList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgBuiltSpareList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgBuiltSpareList.PageIndexChanging
        dgBuiltSpareList.PageIndex = e.NewPageIndex
        dgBuiltSpareList.DataSource = mSpareAssemblyList
        Session("mSpareAssemblyList") = mSpareAssemblyList
        dgBuiltSpareList.DataBind()
    End Sub
    Private Sub dgBuiltSpareList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgBuiltSpareList.RowCommand
        Dim Index As Int32
        Dim mAssemblyDetail As String
        Dim mRemovedAssemblyStatus As AssemblyStatus

        Select Case e.CommandName
            Case "InstallSelected"
                Index = CInt(e.CommandArgument) + dgBuiltSpareList.PageSize * dgBuiltSpareList.PageIndex
                Dim Id As Guid = New Guid(dgBuiltSpareList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                If (Not User.IsInRole("AssemblyInstallationNew")) Then
                    mAssemblyDetail = " Assembly Type : " + mSpareAssemblyList(Index).AssemblyType + " Assembly Info : " + mSpareAssemblyList(Index).ModelSerialNo
                    MarkLog(Util.Action.Install, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to install " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mInstalledAssemblyStatusList = tmpInstalledAssemblyList.GetInstalledAssemblyList("1/1/2099", cmbInstallOnMachine.SelectedValue, "", "")
                If mInstalledAssemblyStatusList.Contains(mSpareAssemblyList.Item(Index).ModelID, mSpareAssemblyList.Item(Index).SerialNo) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.AssemblyAlreadyInstalled, MSGBox.Message_text.AssemblyAlreadyInstalled, "Selected " & mSpareAssemblyList.Item(Index).AssemblyType & " already installed. Can not be installed again.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                mRemovedAssemblyStatus = AssemblyStatus.GetAssemblyStatus(Id)
                mMachine = Machine.GetMachine(New Guid(cmbInstallOnMachine.SelectedValue))
                Session("mMachine") = mMachine

                'Added by Saylee on 19-Mar-2013 for ALL14032013-1
                If CheckPeriodsForRemovedAssemblyStatus(mRemovedAssemblyStatus, mMachine) = False Then
                    MSGBoxCtrl.show("Assembly Status Installation Alert!", "Periods for selected " & mSpareAssemblyList.Item(Index).AssemblyType & " are mismatching with selected Installed On " & cmbInstallOnMachine.SelectedItem.Text & " Aircraft.Can not be installed.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '******************************************

                Session("FromType") = 1 'NewInstall
                Session("IsExistingAssembly") = CType(True, Boolean)
                ''Installed Aseembly Status
                mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbInstallOnMachine.SelectedValue), txtInstallationDate.Text, mRemovedAssemblyStatus.AssemblyTypeID, True, Id.ToString)
                Session("mRemovedAssemblyStatus") = mRemovedAssemblyStatus
                Session("mAssemblyStatus") = mAssemblyStatus
                'Added By Vikrant On 01-Dec-2014
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach
                'End
                RemoveSession()
                ' '' NewMachineMaintenance() 'Added by Saylee on 6th-Oct-2009
                mAssemblyDetail = " Assembly Type : " + mSpareAssemblyList(Index).AssemblyType + " Assembly Info : " + mSpareAssemblyList(Index).ModelSerialNo
                MarkLog(Util.Action.Install, "AssemblyInstallation", mAssemblyDetail, Util.ErrorType.NoError, mSpareAssemblyList(Index).AssemblyStatusID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfInstallAssembly_Ajax.aspx?BackPage=Index.aspx');", True)
        End Select
    End Sub
    Private Sub cmbInstallOnMachine_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbInstallOnMachine.SelectedIndexChanged 'Added By Prahsnat 15-Jun-2015 
        ControlVisibility()
        upnlBuiltSpareAssembly.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgBuiltSpareList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgBuiltSpareList.Sorting
        mSpareAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mSpareAssemblyList") = mSpareAssemblyList
        dgBuiltSpareList.DataSource = mSpareAssemblyList
        dgBuiltSpareList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub dgBuiltSpareList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgBuiltSpareList.Columns(i).HeaderText
            Next
        End If
    End Sub
#End Region

End Class