

'Created By Saylee : 11-Nov-2022


Imports System.Linq.Enumerable
Imports System
Imports System.IO


Public Class wfAssemblyRemoveInstallSwapping
    Inherits System.Web.UI.Page


#Region "Variable Declaration"
    Public mMachineNameValueList As MachineNameValueList
    Public AircraftId As String
    Public mRemovalReasonList As RemovalReasonList

    Dim mAssemblylist As AssemblyList
    Dim mAssemblylist2 As AssemblyList

    Dim mAssemblyStatusInstall1 As AssemblyStatus
    Dim mAssemblyStatusInstall2 As AssemblyStatus

    Dim mAssemblyStatusRemoval1 As AssemblyStatus
    Dim mAssemblyStatusRemoval2 As AssemblyStatus

    Dim mMachineMaintenanceInstall1 As MachineMaintenance
    Dim mMachineMaintenanceInstall2 As MachineMaintenance

    Dim mMachineMaintenanceRemoval1 As MachineMaintenance
    Dim mMachineMaintenanceRemoval2 As MachineMaintenance

    'Dim mAssemblyStatus1 As AssemblyStatus
    'Dim mAssemblyStatus2 As AssemblyStatus

    Private AssemblyId As String

    Dim IsReadOnly As Boolean
    Public mRegNo As String
    Public mAssemblyInfo As String
    Public mAssemblyType As String
    Dim mAssemblyDetail As String
#End Region

#Region " Methods "
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
        mAssemblylist = Session("mAssemblylist")
        mRemovalReasonList = Session("mRemovalReasonList")
        mAssemblylist2 = Session("mAssemblylist2")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfAssemblyRemoveInstallSwapping.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblylist")
            Session.Remove("mRemovalReasonList")
            Session.Remove("mAssemblylist2")
        End If
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True, SkipReadOnlyAircrafts:=True, TagText:="(SELECT)", IsTagRequired:=True)
        cmbAircraft.DataSource = mMachineNameValueList

        If (IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString) Then
            'Do nothing
        Else
            cmbAircraft.SelectedValue = AircraftId
        End If
        cmbAircraft.DataBind()

        Session("AircraftId") = cmbAircraft.SelectedValue

        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT)")
        cmbReason1.DataSource = mRemovalReasonList
        cmbReason2.DataSource = mRemovalReasonList
        Session("mRemovalReasonList") = mRemovalReasonList
        cmbReason1.DataBind()
        cmbReason2.DataBind()

        'mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, Today.Date.ToString, "(All)", True)
        'cmbAircraftAssembly1.DataSource = mAssemblylist

        'If (Session("AssemblyId") = Guid.Empty.ToString Or IsNothing(Session("AssemblyId"))) Then
        '    'Do nothing
        'Else
        '    cmbAircraftAssembly1.SelectedValue = CType(Session("AssemblyId"), String)
        'End If
        'cmbAircraftAssembly1.DataBind()
        'Session("AssemblyId") = cmbAircraftAssembly1.SelectedValue
        'Session("mAssemblyList") = mAssemblylist
    End Sub
#End Region

#Region " Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ClearAll()
            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)

            If Not IsPostBack Then
                Session("MiddleFrame") = "wfAssemblyRemoveInstallSwapping.aspx?"
                DataFieldBind()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, Today.Date.ToString, "(SELECT)", True)
        cmbAircraftAssembly1.DataSource = mAssemblylist
        cmbAircraftAssembly1.DataBind()

        cmbAircraftAssembly2.DataSource = mAssemblylist
        cmbAircraftAssembly2.DataBind()

        Session("mAssemblyList") = mAssemblylist

        If cmbAircraft.SelectedIndex > 0 Then
            pl1.Visible = True
            pl2.Visible = True
        Else
            pl1.Visible = False
            pl2.Visible = False
        End If
    End Sub
    Private Sub txtRemovedOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRemovedOnDate1.TextChanged
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtRemovedOnDate1.Text.ToString, "(SELECT)", True)
        cmbAircraftAssembly1.DataSource = mAssemblylist
        cmbAircraftAssembly1.DataBind()
        Session("mAssemblyList") = mAssemblylist

        cmbAircraftAssembly2.DataSource = mAssemblylist
        cmbAircraftAssembly2.DataBind()

        If cmbAircraft.SelectedIndex > 0 Then
            pl1.Visible = True
            pl2.Visible = True
        Else
            pl1.Visible = False
            pl2.Visible = False
        End If
    End Sub
    Private Sub cmbAircraftAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraftAssembly1.SelectedIndexChanged

        mAssemblylist = Session("mAssemblylist")
        If cmbAircraft.SelectedIndex = 0 Then
            MSGBoxCtrlNEW.Show("Aircraft Alert!", "Please select Aircraft first", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        If txtRemovedOnDate1.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #1.", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly1.SelectedIndex = 0
            Exit Sub
        End If
        If txtRemovedOnDate2.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #2.", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly1.SelectedIndex = 0
            Exit Sub
        End If

        If New Guid(cmbAircraftAssembly1.SelectedValue).Equals(New Guid(cmbAircraftAssembly2.SelectedValue)) Then
            MSGBoxCtrlNEW.Show("Assembly Alert!", "Both Assemblies cannot be same. Please select other Assembly.", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly1.DataSource = mAssemblylist2
            cmbAircraftAssembly1.DataBind()
            Exit Sub
        End If


        If mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyTypeID = 1 Then
            MSGBoxCtrlNEW.show("Airframe Swap Alert!", "Airframe swapping not possible. Please select other assembly", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly1.SelectedIndex = 0
            Exit Sub
        Else
            mAssemblylist2 = AssemblyList.GetAssemblyListForComboBox(mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyTypeID, cmbAircraft.SelectedValue, Today.Date.ToString, "(SELECT)", True)
            cmbAircraftAssembly2.DataSource = mAssemblylist2
            cmbAircraftAssembly2.DataBind()
            Session("mAssemblyList2") = mAssemblylist2
        End If

    End Sub

    Private Sub cmbAircraftAssembly2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraftAssembly2.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            MSGBoxCtrlNEW.show("Aircraft Alert!", "Please select Aircraft first", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly2.SelectedIndex = 0
            Exit Sub
        End If


        If txtRemovedOnDate1.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #1.", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly2.SelectedIndex = 0
            Exit Sub
        End If
        If txtRemovedOnDate2.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #2.", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly2.SelectedIndex = 0
            Exit Sub
        End If


        If New Guid(cmbAircraftAssembly1.SelectedValue).Equals(New Guid(cmbAircraftAssembly2.SelectedValue)) Then
            MSGBoxCtrlNEW.Show("Assembly Alert!", "Both Assemblies cannot be same. Please select other Assembly.", "", MsgBoxStyle.OkOnly, "")
            cmbAircraftAssembly2.DataSource = mAssemblylist2
            cmbAircraftAssembly2.DataBind()

            Exit Sub
        End If
    End Sub
    Private Sub txtInstalledOnDate1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInstalledOnDate1.TextChanged

        If txtRemovedOnDate1.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #1.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If txtRemovedOnDate2.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #2.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If CDate(txtInstalledOnDate1.Text) < CDate(txtRemovedOnDate1.Text) Or CDate(txtInstalledOnDate1.Text) < CDate(txtRemovedOnDate2.Text) Then
            MSGBoxCtrlNEW.show("Alert!", "Installed Date of Assembly #1 should be greater than its Removal date.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

    End Sub
    Private Sub txtInstalledOnDate2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInstalledOnDate2.TextChanged

        If txtRemovedOnDate1.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #1.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If txtRemovedOnDate2.Text = "" Then
            MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #2.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If CDate(txtInstalledOnDate2.Text) < CDate(txtRemovedOnDate2.Text) Or CDate(txtInstalledOnDate2.Text) < CDate(txtRemovedOnDate1.Text) Then
            MSGBoxCtrlNEW.show("Alert!", "Installed Date of Assembly #2 should be greater than its Removal date.", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub
    Private Sub btnSwap_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSwap.ServerClick

        Try
            If cmbAircraft.SelectedIndex = 0 Then
                MSGBoxCtrlNEW.show("Aircraft Alert!", "Please select Aircraft first", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If cmbAircraftAssembly1.SelectedIndex = 0 Or cmbAircraftAssembly2.SelectedIndex = 0 Then
                MSGBoxCtrlNEW.show("Assembly Alert!", "Please select both Assemblies", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If txtRemovedOnDate1.Text = "" Then
                MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #1.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If txtRemovedOnDate2.Text = "" Then
                MSGBoxCtrlNEW.show("Removal Date Alert!", "Please Enter Removal Date of Assembly #2.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If txtInstalledOnDate1.Text = "" Then
                MSGBoxCtrlNEW.show("Installed Date Alert!", "Please Enter Installation Date of Assembly #1.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If txtInstalledOnDate2.Text = "" Then
                MSGBoxCtrlNEW.show("Installed Date Alert!", "Please Enter Installation Date of Assembly #2.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyTypeID = 1 Then
                MSGBoxCtrlNEW.show("Airframe Swap Alert!", "Airframe swapping not possible. Please select other assembly", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If New Guid(cmbAircraftAssembly1.SelectedValue).Equals(New Guid(cmbAircraftAssembly2.SelectedValue)) Then
                MSGBoxCtrlNEW.show("Assembly Alert!", "Both Assemblies cannot be same. Please select other Assembly.", "", MsgBoxStyle.OkOnly, "")
                cmbAircraftAssembly1.DataSource = mAssemblylist
                cmbAircraftAssembly1.DataBind()
                Exit Sub
            End If

            If CDate(txtInstalledOnDate1.Text) < CDate(txtRemovedOnDate1.Text) Or CDate(txtInstalledOnDate1.Text) < CDate(txtRemovedOnDate2.Text) Then
                MSGBoxCtrlNEW.show("Alert!", "Installed Date of Assembly #1 should be greater than Removal date.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If CDate(txtInstalledOnDate2.Text) < CDate(txtRemovedOnDate2.Text) Or CDate(txtInstalledOnDate2.Text) < CDate(txtRemovedOnDate1.Text) Then
                MSGBoxCtrlNEW.show("Alert!", "Installed Date of Assembly #2 should be greater than Removal date.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            If cmbReason1.SelectedIndex = 0 Then
                MSGBoxCtrlNEW.show("Reason Alert!", "Please select Reason for Removal of Assembly #1.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If cmbReason2.SelectedIndex = 0 Then
                MSGBoxCtrlNEW.show("Reason Alert!", "Please select Reason for Removal of Assembly #2.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If


            ''Swap code




            'Removal Alert(s)
            If (Not User.IsInRole("AssemblyRemovalNew")) Then

                mRegNo = cmbAircraft.SelectedItem.Text
                mAssemblyInfo = mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).ModelSerialNoPostion
                mAssemblyType = mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyType
                mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                MarkLog(Util.Action.Delete, "AssemblyRemoval", User.Identity.Name & " is not Authorized User to delete " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                'End
                MSGBoxCtrlNEW.show(MSGBoxNew.Message_title.Authorization, MSGBoxNew.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Dim checkRemovedAssemblyList As tmpRemovedAssemblyList
            checkRemovedAssemblyList = tmpRemovedAssemblyList.GetRemovedAssemblyList(Today.ToShortDateString, cmbAircraft.SelectedValue, IIf(cmbAircraftAssembly1.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly1.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly1.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly1.SelectedIndex).SerialNo, ""))
            If checkRemovedAssemblyList.Contains(mAssemblylist.Item(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyStatusID) Then
                MSGBoxCtrlNEW.show(MSGBoxNew.Message_title.SelectRestriction, MSGBoxNew.Message_text.SelectRestriction, "other Assembly. Selected " & mAssemblylist.Item(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyType & ",  Already removed, cannot remove again", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            'Installation Alert(s)
            If (Not User.IsInRole("AssemblyInstallationNew")) Then
                'Added by Vikrant on 28-July-2011
                mRegNo = cmbAircraft.SelectedItem.Text
                mAssemblyInfo = mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).ModelSerialNoPostion
                mAssemblyType = mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyType
                mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                MarkLog(Util.Action.Install, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to install " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                'End
                MSGBoxCtrlNEW.show(MSGBoxNew.Message_title.Authorization, MSGBoxNew.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            'Dim mInstalledAssemblyStatusList As tmpInstalledAssemblyList
            'mInstalledAssemblyStatusList = tmpInstalledAssemblyList.GetInstalledAssemblyList("1/1/2099", cmbAircraft.SelectedValue, "", "")
            'If mInstalledAssemblyStatusList.Contains(mAssemblylist.Item(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyStatusID) Then
            '    MSGBoxCtrlNEW.Show(MSGBoxNew.Message_title.AssemblyAlreadyInstalled, MSGBoxNew.Message_text.AssemblyAlreadyInstalled, "Selected " & cmbAircraftAssembly1.Item(index).AssemblyType & " already installed. Can not be installed again.", MsgBoxStyle.OkOnly, "")
            '    Exit Sub
            'End If



            '###############################################################################################
            'REMOVAL Assembly #1    

            SetAssemblyRemoval1()

            '###############################################################################################
            'REMOVAL Assembly #2    

            SetAssemblyRemoval2()


            '###############################################################################################
            'INSTALLATION Assembly #1    

            SetAssemblyInstall1()


            '###############################################################################################
            'INSTALLATION Assembly #2   
            SetAssemblyInstall2()

            If Not CustomValidate() Then
                upnlValidationSummary.Update()

                Exit Sub
            End If

            Try
                Session("MiddleFrame") = ""
                If Save() Then
                    Dim RemValues1 As String
                    Dim RemValues2 As String
                    For i As Integer = 0 To mAssemblyStatusRemoval1.AssemblyStatusPeriods.Count - 1
                        If mAssemblyStatusRemoval1.AssemblyStatusPeriods(i).PeriodID <> 2 Then
                            RemValues1 = RemValues1 + mAssemblyStatusRemoval1.AssemblyStatusPeriods(i).AssemblyRemovalValueFormatted + " " + mAssemblyStatusRemoval1.AssemblyStatusPeriods(i).PeriodCode + " "
                        End If

                    Next
                    For i As Integer = 0 To mAssemblyStatusRemoval2.AssemblyStatusPeriods.Count - 1
                        If mAssemblyStatusRemoval2.AssemblyStatusPeriods(i).PeriodID <> 2 Then
                            RemValues2 = RemValues2 + mAssemblyStatusRemoval2.AssemblyStatusPeriods(i).AssemblyRemovalValueFormatted + " " + mAssemblyStatusRemoval1.AssemblyStatusPeriods(i).PeriodCode + " "
                        End If
                    Next

                    Dim det As String = ""
                    det = "Swapping of Assemblies " + mAssemblyStatusRemoval1.Assembly.ModelName + "-" + mAssemblyStatusRemoval1.Assembly.SerialNo + _
                        " and " + mAssemblyStatusRemoval2.Assembly.ModelName + "-" + mAssemblyStatusRemoval2.Assembly.SerialNo _
                        + " from " + cmbAircraft.SelectedItem.Text + "<br>" + "<b>Removal Details of Assembly #1 :</b> " + mAssemblyStatusRemoval1.Assembly.ModelName _
                        + "-" + mAssemblyStatusRemoval1.Assembly.SerialNo + " removed on " + mAssemblyStatusRemoval1.RemovedOnFormatted + " are " + RemValues1 + "<br>" _
                        + "<b>Installation Details : </b> installed on " + txtInstalledOnDate1.Text + " are " + RemValues1 + "<br> <br>" _
                        + "<b>Removal Details of Assembly #2 : </b>" + mAssemblyStatusRemoval2.Assembly.ModelName + "-" + mAssemblyStatusRemoval2.Assembly.SerialNo _
                        + " removed on " + mAssemblyStatusRemoval2.RemovedOnFormatted + " are " + RemValues2 + "<br>" + "<b>Installation Details : </b> installed on " + _
                        txtInstalledOnDate2.Text + " are " + RemValues2


                    MarkLog(Action.New, "AssemblySwapping", det, ErrorType.NoError, mAssemblyStatusRemoval2.ID, EventLogID)
                    MSGBoxCtrlNEW.show("Swapped Successfully..!!", "Congratulations..!! You have swapped assemblies successfully..!!", "You can check onto individual links.", MsgBoxStyle.OkOnly, "")
                    Session("MiddleFrame") = ""
                    btnSwap.Disabled = True
                End If
            Catch ex As Exception
                '
            End Try

        Catch ex As Exception

        End Try
    End Sub

    Public Function CustomValidate() As Boolean

        mAssemblyStatusRemoval1 = Session("mAssemblyStatusRemoval1")
        mMachineMaintenanceRemoval1 = Session("mMachineMaintenanceRemoval1")

        mAssemblyStatusRemoval2 = Session("mAssemblyStatusRemoval2")
        mMachineMaintenanceRemoval2 = Session("mMachineMaintenanceRemoval2")

        mMachineMaintenanceInstall1 = Session("mMachineMaintenanceInstall1")
        mAssemblyStatusInstall1 = Session("mAssemblyStatusInstall1")

        mMachineMaintenanceInstall2 = Session("mMachineMaintenanceInstall2")
        mAssemblyStatusInstall2 = Session("mAssemblyStatusInstall2")

        Dim str As String = ""
        If Not mAssemblyStatusRemoval1.IsValid Then
            For i As Integer = 0 To mAssemblyStatusRemoval1.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatusRemoval1.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If

        If Not mAssemblyStatusRemoval2.IsValid Then
            For i As Integer = 0 To mAssemblyStatusRemoval2.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatusRemoval2.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If

        If Not mAssemblyStatusInstall1.IsValid Then
            For i As Integer = 0 To mAssemblyStatusInstall1.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatusInstall1.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If

        If Not mAssemblyStatusInstall2.IsValid Then
            For i As Integer = 0 To mAssemblyStatusInstall2.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatusInstall2.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If

        If str <> "" Then
            custValidator.ErrorMessage = str
            custValidator.IsValid = False
            Return False
        End If

        Return True

    End Function
#End Region

#Region " Methods "
    Public Function Save() As Boolean
        mAssemblyStatusRemoval1 = Session("mAssemblyStatusRemoval1")
        mMachineMaintenanceRemoval1 = Session("mMachineMaintenanceRemoval1")

        mAssemblyStatusRemoval2 = Session("mAssemblyStatusRemoval2")
        mMachineMaintenanceRemoval2 = Session("mMachineMaintenanceRemoval2")

        mMachineMaintenanceInstall1 = Session("mMachineMaintenanceInstall1")
        mAssemblyStatusInstall1 = Session("mAssemblyStatusInstall1")

        mMachineMaintenanceInstall2 = Session("mMachineMaintenanceInstall2")
        mAssemblyStatusInstall2 = Session("mAssemblyStatusInstall2")

        Try
            mAssemblyStatusRemoval1.Save()
            mMachineMaintenanceRemoval1.Save()

            mAssemblyStatusRemoval2.Save()
            mMachineMaintenanceRemoval2.Save()

            mAssemblyStatusInstall1.Save()
            mMachineMaintenanceInstall1.Save()

            mAssemblyStatusInstall2.Save()
            mMachineMaintenanceInstall2.Save()

            Return True
        Catch ex As SqlException

            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrlNEW.Show(MSGBoxNew.Message_title.NumericOverFlow, MSGBoxNew.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrlNEW.Show(MSGBoxNew.Message_title.DataBaseError, MSGBoxNew.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrlNEW.Show(MSGBoxNew.Message_title.DataBaseError, MSGBoxNew.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrlNEW.Show(MSGBoxNew.Message_title.ReferenceDelete, MSGBoxNew.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrlNEW.Show(MSGBoxNew.Message_title.DatabaseException, MSGBoxNew.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try

        Return False
    End Function

    Public Sub SetAssemblyRemoval1()

        Dim mPrevAssemblyStatus1 As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblylist(New Guid(cmbAircraftAssembly1.SelectedValue)).AssemblyStatusID)
        mAssemblyStatusRemoval1 = AssemblyStatus.NewRemovalAssemblyStatus(mPrevAssemblyStatus1.ID, txtRemovedOnDate1.Text)


        mAssemblyStatusRemoval1.RemovalReasonID = New Guid(cmbReason1.SelectedValue)
        mAssemblyStatusRemoval1.RemovalReasonName = cmbReason1.SelectedItem.Text
        If txtRemovedOnDate1.Text <> "" Then
            mAssemblyStatusRemoval1.RemovedOn = txtRemovedOnDate1.Text
        Else
            mAssemblyStatusRemoval1.RemovedOn = System.DBNull.Value
        End If
        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mPrevAssemblyStatus1.MaintenanceDoneByEmployees
            mAssemblyStatusRemoval1.MaintenanceDoneByEmployees.Add(mAssemblyStatusRemoval1.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
        Next
        'End

        mMachineMaintenanceRemoval1 = MachineMaintenance.NewMachineMaintenance(mAssemblyStatusRemoval1.MachineID, 2, txtRemovedOnDate1.Text, mAssemblyStatusRemoval1.ID, Guid.Empty, 0, 0, mAssemblyStatusRemoval1.ID)
        With mMachineMaintenanceRemoval1
            .MachineID = mAssemblyStatusRemoval1.MachineID
            ''.MaintenanceActivityTypeID = 2
            .MaintenanceID = mAssemblyStatusRemoval1.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatusRemoval1.ID

            .Date = txtRemovedOnDate1.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo(txtRemovedOnDate1.Text, mAssemblyStatusRemoval1.MachineID, mAssemblyStatusRemoval1.AssemblyID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
            End If
        End With

        Session("mAssemblyStatusRemoval1") = mAssemblyStatusRemoval1
        Session("mMachineMaintenanceRemoval1") = mMachineMaintenanceRemoval1
    End Sub
    Public Sub SetAssemblyRemoval2()

        Dim mPrevAssemblyStatus2 As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblylist2(New Guid(cmbAircraftAssembly2.SelectedValue)).AssemblyStatusID)
        mAssemblyStatusRemoval2 = AssemblyStatus.NewRemovalAssemblyStatus(mPrevAssemblyStatus2.ID, txtRemovedOnDate2.Text)


        mAssemblyStatusRemoval2.RemovalReasonID = New Guid(cmbReason2.SelectedValue)
        mAssemblyStatusRemoval2.RemovalReasonName = cmbReason2.SelectedItem.Text
        If txtRemovedOnDate2.Text <> "" Then
            mAssemblyStatusRemoval2.RemovedOn = txtRemovedOnDate2.Text
        Else
            mAssemblyStatusRemoval2.RemovedOn = System.DBNull.Value
        End If

        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mPrevAssemblyStatus2.MaintenanceDoneByEmployees
            mAssemblyStatusRemoval2.MaintenanceDoneByEmployees.Add(mAssemblyStatusRemoval2.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
        Next
        'End

        mMachineMaintenanceRemoval2 = MachineMaintenance.NewMachineMaintenance(mAssemblyStatusRemoval2.MachineID, 2, txtRemovedOnDate2.Text, mAssemblyStatusRemoval2.ID, Guid.Empty, 0, 0, mAssemblyStatusRemoval2.ID)
        With mMachineMaintenanceRemoval1
            .MachineID = mAssemblyStatusRemoval2.MachineID
            ''.MaintenanceActivityTypeID = 2
            .MaintenanceID = mAssemblyStatusRemoval2.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatusRemoval2.ID

            .Date = txtRemovedOnDate1.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo(txtRemovedOnDate2.Text, mAssemblyStatusRemoval2.MachineID, mAssemblyStatusRemoval2.AssemblyID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
            End If
        End With

        Session("mAssemblyStatusRemoval2") = mAssemblyStatusRemoval2
        Session("mMachineMaintenanceRemoval2") = mMachineMaintenanceRemoval2
    End Sub
    Public Sub SetAssemblyInstall1()

        mAssemblyStatusInstall1 = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbAircraft.SelectedValue), txtInstalledOnDate1.Text, mAssemblyStatusRemoval2.AssemblyTypeID, True, mAssemblyStatusRemoval2.ID.ToString)

        ''mAssemblyStatusRemoval2 : This is Removed Assembly #2
        'Assembly #2 to install here in place of #1

        mAssemblyStatusInstall1.Assembly.ModelID = mAssemblyStatusRemoval2.Assembly.ModelID
        mAssemblyStatusInstall1.ATAID = mAssemblyStatusRemoval2.ATAID
        mAssemblyStatusInstall1.Assembly.SerialNo = mAssemblyStatusRemoval2.Assembly.SerialNo


        If txtInstalledOnDate1.Text = "" Then
            mAssemblyStatusInstall1.InstalledOn = DBNull.Value
        Else
            mAssemblyStatusInstall1.InstalledOn = txtInstalledOnDate1.Text
        End If

        'Periods
        For Each Tmperiod As AssemblyStatusPeriod In mAssemblyStatusRemoval2.AssemblyStatusPeriods
            If Tmperiod.PeriodID <> 2 Then
                mAssemblyStatusInstall1.AssemblyStatusPeriods(Tmperiod.PeriodID, "").AssemblyInstallationValueFormatted = Tmperiod.AssemblyRemovalValueFormatted
            End If
        Next

        mAssemblyStatusInstall1.Position = mAssemblyStatusRemoval1.Position

        mMachineMaintenanceInstall1 = MachineMaintenance.NewMachineMaintenance(New Guid(cmbAircraft.SelectedValue), 1, txtInstalledOnDate1.Text, mAssemblyStatusInstall1.ID, Guid.Empty, 0, 0, mAssemblyStatusInstall1.ID)
        With mMachineMaintenanceInstall1
            .MachineID = New Guid(cmbAircraft.SelectedValue)
            ''.MaintenanceActivityTypeID = 1
            .MaintenanceID = mAssemblyStatusInstall1.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatusInstall1.ID

            .Date = txtInstalledOnDate1.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(txtInstalledOnDate1.Text, mAssemblyStatusInstall1.MachineID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
            End If
        End With
        Session("mMachineMaintenanceInstall1") = mMachineMaintenanceInstall1
        Session("mAssemblyStatusInstall1") = mAssemblyStatusInstall1
    End Sub
    Public Sub SetAssemblyInstall2()

        mAssemblyStatusInstall2 = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbAircraft.SelectedValue), txtInstalledOnDate2.Text, mAssemblyStatusRemoval1.AssemblyTypeID, True, mAssemblyStatusRemoval1.ID.ToString)

        ''mAssemblyStatusRemoval1 : This is Removed Assembly #1
        '' Assembly #1 to install here in place of #2

        mAssemblyStatusInstall2.Assembly.ModelID = mAssemblyStatusRemoval1.Assembly.ModelID
        mAssemblyStatusInstall2.ATAID = mAssemblyStatusRemoval1.ATAID
        mAssemblyStatusInstall2.Assembly.SerialNo = mAssemblyStatusRemoval1.Assembly.SerialNo

        If txtInstalledOnDate2.Text = "" Then
            mAssemblyStatusInstall2.InstalledOn = DBNull.Value
        Else
            mAssemblyStatusInstall2.InstalledOn = txtInstalledOnDate2.Text
        End If

        mAssemblyStatusInstall2.Position = mAssemblyStatusRemoval2.Position

        'Periods
        For Each Tmperiod As AssemblyStatusPeriod In mAssemblyStatusRemoval1.AssemblyStatusPeriods
            If Tmperiod.PeriodID <> 2 Then
                mAssemblyStatusInstall2.AssemblyStatusPeriods(Tmperiod.PeriodID, "").AssemblyInstallationValueFormatted = Tmperiod.AssemblyRemovalValueFormatted
            End If


        Next



        mMachineMaintenanceInstall2 = MachineMaintenance.NewMachineMaintenance(New Guid(cmbAircraft.SelectedValue), 1, txtInstalledOnDate2.Text, mAssemblyStatusInstall2.ID, Guid.Empty, 0, 0, mAssemblyStatusInstall2.ID)
        With mMachineMaintenanceInstall2
            .MachineID = New Guid(cmbAircraft.SelectedValue)
            ''.MaintenanceActivityTypeID = 1
            .MaintenanceID = mAssemblyStatusInstall2.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatusInstall2.ID

            .Date = txtInstalledOnDate2.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(txtInstalledOnDate2.Text, mAssemblyStatusInstall2.MachineID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
            End If
        End With
        Session("mMachineMaintenanceInstall2") = mMachineMaintenanceInstall2
        Session("mAssemblyStatusInstall2") = mAssemblyStatusInstall2
    End Sub
#End Region
 
End Class