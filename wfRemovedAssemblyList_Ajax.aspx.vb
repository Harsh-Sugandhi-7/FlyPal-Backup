'Added By Vikrant On 17-Mar-2015

Public Class wfRemovedAssemblyList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachineNameValueList As MachineNameValueList
    Public mAssemblyStatus As AssemblyStatus
    Public mRemovedAssemblyStatusList As tmpRemovedAssemblyList
    Public mInstalledAssemblyStatusList As tmpInstalledAssemblyList
    Public mMachine As Machine
    Public InstallDate As String
    Public InstallOnId As String
    Public AircraftId As String
    Public mMachineMaintenance As MachineMaintenance      'Added by Saylee on 6th-Oct-2009
    Public mAssemblyTypeListForUI As AssemblyTypeListForUI 'Added by Saylee on 23-Oct-2009
    'Added by Vikrant on 26-July-2011
    Dim mAssemblyDetail As String
    Dim EventLogID As Guid
    Public mRegNo As String
    Public mAssemblyInfo As String
    Public mAssemblyType As String
    Dim mFileAttach As FileAttach 'Added By Vikrant On 01-Dec-2014
    Dim mAssemblylist As AssemblyList  'Added By Prahsnat 15-Jun-2015 
    Private AssemblyId As String

    Dim IsReadOnly As Boolean 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
    Dim IsReadOnlyInstalledOn As Boolean 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mRemovedAssemblyStatusList = CType(Session("mRemovedAssemblyStatusList"), tmpRemovedAssemblyList)
        mInstalledAssemblyStatusList = CType(Session("mInstalledAssemblyStatusList"), tmpInstalledAssemblyList)
        InstallDate = CType(Session("InstallDate"), String)
        InstallOnId = CType(Session("InstallOnId"), String)
        AircraftId = CType(Session("AircraftId"), String)
        Model = CType(Session("Model"), String) 'Added by Rahul on 29-Apr-2009
        SerialNo = CType(Session("SerialNo"), String) 'Added by Rahul on 29-Apr-2009
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 6th-Oct-2009
        mAssemblyTypeListForUI = Session("mAssemblyTypeListForUI")
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AssemblyId = CType(Session("AssemblyId"), String)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = Session("IsReadOnlyInstalledOn") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mRemovedAssemblyStatusList")
        Session.Remove("mInstalledAssemblyStatusList")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 6th-Oct-2009
        Session.Remove("mAssemblyTypeListForUI")
        Session.Remove("mAssemblyStatus")
        Session.Remove("InstallDate")
        Session.Remove("InstallOnId")
        Session.Remove("AircraftId")
        Session.Remove("mAssemblylist")
        Session.Remove("AssemblyId")
        Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        Session.Remove("IsReadOnlyInstalledOn")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfRemovedAssemblyList_Ajax.aspx?" Then
            Session.Remove("mMachine")
            Session.Remove("mAssemblyStatus")
            Session.Remove("mRemovedAssemblyStatusList")
            Session.Remove("mInstalledAssemblyStatusList")
            Session.Remove("mMachineNameValueList")
            Session.Remove("InstallDate")
            Session.Remove("InstallOnId")
            Session.Remove("AircraftId")
            Session.Remove("Model") 'Added by Rahul on 29-Apr-2009
            Session.Remove("SerialNo") 'Added by Rahul on 29-Apr-2009
            Session.Remove("mAssemblyTypeListForUI")
            Session.Remove("mMachineMaintenance") 'Added by Saylee on 6th-Oct-2009
            Session.Remove("mAssemblylist")
            Session.Remove("AssemblyId")
            Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            Session.Remove("IsReadOnlyInstalledOn")
        End If
    End Sub
    Private Sub FindNow()
        Session("InstallDate") = txtInstallationDate.Text
        Session("InstallOnId") = cmbInstallOnMachine.SelectedValue
        Session("AircraftId") = cmbAircraft.SelectedValue
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue

        mRemovedAssemblyStatusList = tmpRemovedAssemblyList.GetRemovedAssemblyList(txtInstallationDate.Text, cmbAircraft.SelectedValue, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList
        dgRemovedList.DataSource = mRemovedAssemblyStatusList
        dgRemovedList.DataBind()
        lblRemovedAssembly.Text = "List of Removed assembly as of " & txtInstallationDate.Text & "  : " & mRemovedAssemblyStatusList.Count & " Record(s) found."
        '***************
        REM:-Binds Installed AssemblyStatus List
        mInstalledAssemblyStatusList = tmpInstalledAssemblyList.GetInstalledAssemblyList(txtInstallationDate.Text, cmbAircraft.SelectedValue, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        Session("mInstalledAssemblyStatusList") = mInstalledAssemblyStatusList
        dgInstalledList.DataSource = mInstalledAssemblyStatusList
        dgInstalledList.DataBind()
        lblInstalledAssembly.Text = "List of Installed assembly as of " & txtInstallationDate.Text & " : " & mInstalledAssemblyStatusList.Count & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "revert" Then
                        Session("sender") = ""
                        Try
                            If mInstalledAssemblyStatusList.CurrentItem.IsRemoved = True Then
                                MSGBoxCtrl.show(MSGBox.Message_title.AssemblyRemoved, MSGBox.Message_text.AssemblyRemoved, mInstalledAssemblyStatusList.CurrentItem.AssemblyType & " is currently removed, first revert the removal and then revert the installation", MsgBoxStyle.OkOnly, "")
                                'MessageBox.Show(mInstalledAssemblyStatusList.Item(dgInstalledList.CurrentRowIndex).AssemblyType & " is currently removed, first revert the removal and then revert the installation", "Revert Installation", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                Exit Sub
                            End If

                            'Added by Saylee on 6th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mInstalledAssemblyStatusList.CurrentItem.AssemblyStatusID, 1)
                            '=============================

                            'Added By Vikrant On 01-Dec-2014
                            If mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).IsAttachmentAdded Then
                                Dim mAssemblyStatusID As Guid = mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).AssemblyStatusID
                                mFileAttach = FileAttach.GetAttachment(mAssemblyStatusID, 1) 'Sort=1, for Installation 
                            End If
                            'End
                            AssemblyStatus.RevertInstalledAssemblyStatus(mInstalledAssemblyStatusList.CurrentItem.AssemblyStatusID, mInstalledAssemblyStatusList.CurrentItem.MachineID, mInstalledAssemblyStatusList.CurrentItem.InstalledOnDBValue)

                            Try
                                MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                                Session("mMachineMaintenance") = mMachineMaintenance
                                'Added By Vikrant On 01-Dec-2014
                                If Not mFileAttach Is Nothing Then
                                    If mFileAttach.Size > 0 Then
                                        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID, 1)
                                    End If
                                End If
                                'End
                                'Added by Vikrant on 28-July-2011
                                mRegNo = mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).MachineInfo
                                mAssemblyType = mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).AssemblyType
                                mAssemblyInfo = mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).AssemblyInfo
                                mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                                MarkLog(Util.Action.RevertInstallation, "AssemblyInstallation", "Revert: " + mAssemblyDetail, Util.ErrorType.NoError, mInstalledAssemblyStatusList.CurrentItem.AssemblyStatusID, EventLogID)
                            Catch ex As Exception
                                '
                            End Try

                            'Added by Saylee on 14-July-2009
                            Session("mAircraftInformationBoardList") = Nothing
                            '***********************************
                            FindNow()
                            ControlVisibility()
                            SetPage()
                            SetGrid()
                            upnlInstalledAssembly.Update()
                            upnlRemovedAssemblyList.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.CannotRevert, MSGBox.Message_text.CannotRevert, "You are trying to revert the installation.Cannot revert installation as it is currently in use.", MsgBoxStyle.OkOnly, "")
                                'Added by Vikrant on 28-July-2011
                                mRegNo = mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).MachineInfo
                                mAssemblyType = mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).AssemblyType
                                mAssemblyInfo = mInstalledAssemblyStatusList.Item(mInstalledAssemblyStatusList.CurrentIndex).AssemblyInfo
                                mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                                MarkLog(Util.Action.RevertInstallation, "AssemblyInstallation", "Can't Revert: " + mAssemblyDetail + " is currently in use", Util.ErrorType.NoError, mInstalledAssemblyStatusList.CurrentItem.AssemblyStatusID, EventLogID)
                            Else
                                MSGBoxCtrl.show(MSGBox.Message_title.CannotRevert, MSGBox.Message_text.CannotRevert, ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                        End Try
                        'Response.Redirect("wfAssemblyStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If

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
    Private Sub SetPage()
        lblRemovedAssembly.Text = "List of Removed assembly as of " & txtInstallationDate.Text & "  : " & mRemovedAssemblyStatusList.Count & " Record(s) found."
        lblInstalledAssembly.Text = "List of Installed assembly as of " & txtInstallationDate.Text & " : " & mInstalledAssemblyStatusList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        btnPrintRemovedAssembly.Enabled = mRemovedAssemblyStatusList.Count > 0
        btnPrintInstalledAssembly.Enabled = mInstalledAssemblyStatusList.Count > 0
        'Added By Prashant 2-Dec-2020
        If (User.IsInRole("BuildSpareAssemblyNew") = True And User.IsInRole("BuildSpareAssemblyEdit") = True) Then
            lnkSpareAssembly.Visible = True
        End If
        'End of Added By Prashant 2-Dec-2020
        SetEnable()
    End Sub
    'Added by Saylee on 19-Mar-2013 for ALL14032013-1
    Public Function CheckPeriodsForRemovedAssemblyStatus(ByVal RemovedAssemblyStatus As AssemblyStatus) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        If RemovedAssemblyStatus.AssemblyTypeID = 2 Or RemovedAssemblyStatus.AssemblyTypeID = 4 Then Return True
        mMachine = Machine.GetMachine(New Guid(cmbInstallOnMachine.SelectedValue))
        While i <= RemovedAssemblyStatus.AssemblyStatusPeriods.Count - 1
            If mMachine.AssemblyStatus.AssemblyStatusPeriods.Contains(RemovedAssemblyStatus.AssemblyStatusPeriods(i).PeriodID) Then
                tmpIsPeriodExists = True
            Else
                tmpIsPeriodExists = False
                Exit While
            End If
            i = i + 1
        End While
        Return tmpIsPeriodExists
    End Function
    Private Sub SetGrid()

        Dim P As Integer
        Dim B As Boolean

        Dim B1 As Boolean

        For j As Integer = 0 To dgInstalledList.Rows.Count - 1
            P = CType(Me.dgInstalledList.Rows(j).Cells(12).Text, Integer)
            B = CType(Me.dgInstalledList.Rows(j).Cells(13).Text, Boolean)
            If (P = 1 And B = True) Then
                dgInstalledList.Rows(j).Cells(11).Enabled = False
            End If

            B1 = CType(Me.dgInstalledList.Rows(j).Cells(15).Text, Boolean)
            If B1 = False Then
                dgInstalledList.Rows(j).Cells(14).Enabled = False
            End If
        Next
    End Sub
    Private Sub SetEnable()
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = Session("IsReadOnlyInstalledOn")

        For j As Integer = 0 To dgRemovedList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnlyInstalledOn = True Or IsReadOnly = True Then
                dgRemovedList.Rows(j).Cells(9).Enabled = False
            Else
                dgRemovedList.Rows(j).Cells(9).Enabled = True
            End If
            '*************************
        Next

        For j As Integer = 0 To dgInstalledList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgInstalledList.Rows(j).Cells(9).Enabled = False
                dgInstalledList.Rows(j).Cells(10).Enabled = False
            Else
                dgInstalledList.Rows(j).Cells(9).Enabled = True
                dgInstalledList.Rows(j).Cells(10).Enabled = True
            End If
            '*************************
        Next

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        'Disable AddNew buttons if Aircraft is ReadOnly
        If IsReadOnly = True Then
            btnAdd.Enabled = False
            lblReadOnly.Visible = True
        Else
            btnAdd.Enabled = True
            lblReadOnly.Visible = False
        End If

        If IsReadOnlyInstalledOn = True Then
            btnAdd.Enabled = False
            lblReadOnlyInstalledOn.Visible = True
        Else
            btnAdd.Enabled = True
            lblReadOnlyInstalledOn.Visible = False
        End If

        '21-Jul-2022: Revert Installation Link disabled for all users as to avoid issues when reverting if already assembly was removed and installing after the date in this removal
        If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then
            dgInstalledList.Columns(9).Visible = True
        Else
            dgInstalledList.Columns(9).Visible = False
        End If
        '**************************************

        upnlRemovedAssemblyList.Update()
        upnlInstalledAssembly.Update()
        upnlInstallAssembly.Update()
        '*************************
    End Sub
    'Added By Vikrant On 01-Dec-2014
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 1) 'Sort = 1 - Installation
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded)
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    'End
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        REM: Machine List with "All"
        If IsNothing(Session("InstallDate")) Then
            txtInstallationDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            InstallDate = Today.Date.ToString(AppSettings("DateFormat")) 'Added By Rahul on 29-Apr-2009
        Else
            txtInstallationDate.Text = InstallDate
        End If
        Session("InstallDate") = InstallDate
        txtInstallationDate.DataBind()

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineNameValueList
        If (IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString) Then
            'Do nothing
        Else
            cmbAircraft.SelectedValue = AircraftId
        End If
        cmbAircraft.DataBind()
        Session("AircraftId") = cmbAircraft.SelectedValue

        cmbInstallOnMachine.DataSource = mMachineNameValueList
        cmbInstallOnMachine.DataBind()
        If (IsNothing(InstallOnId) Or InstallOnId = Guid.Empty.ToString) Then
            'Do nothing
        Else
            cmbInstallOnMachine.SelectedValue = InstallOnId
        End If
        Session("InstallOnId") = cmbInstallOnMachine.SelectedValue
        Session("mMachineNameValueList") = mMachineNameValueList

        'Added By Prashant 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtInstallationDate.Text.ToString, "(All)", True)
        cmbAircraftAssembly.DataSource = mAssemblylist

        If (Session("AssemblyId") = Guid.Empty.ToString Or IsNothing(Session("AssemblyId"))) Then
            'Do nothing
        Else
            cmbAircraftAssembly.SelectedValue = CType(Session("AssemblyId"), String)
        End If
        cmbAircraftAssembly.DataBind()
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue
        Session("mAssemblyList") = mAssemblylist
        '-----------------------------------------

        REM: Machine List without "All"
        'If mMachineNameValueList.Count > 1 Then
        mMachine = Machine.GetMachine(mMachineNameValueList(0).ID)
        Session("mMachine") = mMachine
        'End If
        mRemovedAssemblyStatusList = tmpRemovedAssemblyList.GetRemovedAssemblyList(InstallDate, cmbAircraft.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList
        dgRemovedList.DataSource = mRemovedAssemblyStatusList
        dgRemovedList.DataBind()
        ''***************
        ' ''REM:-Binds Installed AssemblyStatus List

        'Added and Commented By Rahul on 29-Apr-09
        ' mInstalledAssemblyStatusList = tmpInstalledAssemblyList.GetInstalledAssemblyList(Today.ToShortDateString, mMachineNameValueList(1).ID.ToString, "", "")
        mInstalledAssemblyStatusList = tmpInstalledAssemblyList.GetInstalledAssemblyList(InstallDate, cmbAircraft.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        Session("mInstalledAssemblyStatusList") = mInstalledAssemblyStatusList
        dgInstalledList.DataSource = mInstalledAssemblyStatusList
        dgInstalledList.DataBind()

        If cmbAdd.SelectedIndex < 0 Then 'Added by Saylee on 23-Oct-2009
            mAssemblyTypeListForUI = AssemblyTypeListForUI.GetAssemblyTypeListForUI
            cmbAdd.DataSource = mAssemblyTypeListForUI
            Session("mAssemblyTypeListForUI") = mAssemblyTypeListForUI
            cmbAdd.DataBind()
        End If

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = mMachineNameValueList(New Guid(cmbInstallOnMachine.SelectedValue)).IsReadOnly

        Session("IsReadOnly") = IsReadOnly
        Session("IsReadOnlyInstalledOn") = IsReadOnlyInstalledOn
        '***********************************
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            cmbInstallOnMachine.Focus()
            Session("MiddleFrame") = "wfRemovedAssemblyList_Ajax.aspx?"
            DataFieldBind()
            FindNow()
            ControlVisibility()
            SetPage()
            SetGrid()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not IsValid Then Exit Sub
        mMachine = Machine.GetMachine(New Guid(cmbInstallOnMachine.SelectedValue))
        Session("FromType") = 1 'NewInstall
        Session("IsExistingAssembly") = CType(False, Boolean)
        mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbInstallOnMachine.SelectedValue), txtInstallationDate.Text, mAssemblyTypeListForUI(CType(cmbAdd.SelectedIndex, Int32)).ID, False)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mRemovedAssemblyStatus") = Nothing

        'Added By Vikrant On 01-Dec-2014
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
        Session("mFileAttach") = mFileAttach
        'End

        Session("mMachineMaintenance") = mMachineMaintenance

        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        'If Not IsValid Then Exit Sub
        'Dim mRemovedAssemblyStatus As AssemblyStatus
        'mMachine = mMachine.GetMachine(New Guid(cmbInstallOnMachine.SelectedValue))
        'Session("FromType") = 1 'NewInstall
        'Session("IsExistingAssembly") = CType(False, Boolean)
        'mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbInstallOnMachine.SelectedValue), txtInstallationDate.Text, CType(cmbAdd.SelectedValue, Int32), False)
        'Session("mMachine") = mMachine
        'Session("mAssemblyStatus") = mAssemblyStatus
        'Session("mRemovedAssemblyStatus") = Nothing

        'Added by Vikrant on 28-July-2011
        MarkLog(Util.Action.[New], "AssemblyInstallation", cmbAdd.SelectedItem.Text, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfInstallAssembly_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged 'Added By Prahsnat 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtInstallationDate.Text.ToString, "(All)", True)
        cmbAircraftAssembly.DataSource = mAssemblylist
        cmbAircraftAssembly.DataBind()
        Session("mAssemblyList") = mAssemblylist
        upnlSearchCriteria.Update()
        btnFindNow_Click(sender, e)
        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly

        If IsReadOnly = True Then
            lblReadOnly.Visible = True
        Else
            lblReadOnly.Visible = False
        End If
        SetEnable()
        '*************************************************
    End Sub
    Private Sub cmbAircraftAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraftAssembly.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub txtInstallationDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtInstallationDate.TextChanged
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtInstallationDate.Text.ToString, "(All)", True)
        cmbAircraftAssembly.DataSource = mAssemblylist
        cmbAircraftAssembly.DataBind()
        Session("mAssemblyList") = mAssemblylist
        upnlSearchCriteria.Update()
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        ControlVisibility()
        SetGrid()

        upnlInstalledAssembly.Update()
        upnlRemovedAssemblyList.Update()
        upnlActionBtnBottom.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub dgRemovedList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "InstallSelected"
                Index = CInt(e.CommandArgument) + dgRemovedList.PageSize * dgRemovedList.PageIndex
                Dim Id As Guid = New Guid(dgRemovedList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                If (Not User.IsInRole("AssemblyInstallationNew")) Then
                    'Added by Vikrant on 28-July-2011
                    mRegNo = mRemovedAssemblyStatusList(Index).MachineInfo
                    mAssemblyType = mRemovedAssemblyStatusList(Index).AssemblyType
                    mAssemblyInfo = mRemovedAssemblyStatusList(Index).AssemblyInfo
                    mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                    MarkLog(Util.Action.Install, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to install " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mInstalledAssemblyStatusList = tmpInstalledAssemblyList.GetInstalledAssemblyList("1/1/2099", cmbInstallOnMachine.SelectedValue, "", "")
                If mInstalledAssemblyStatusList.Contains(mRemovedAssemblyStatusList.Item(Index).ModelID, mRemovedAssemblyStatusList.Item(Index).SerialNo) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.AssemblyAlreadyInstalled, MSGBox.Message_text.AssemblyAlreadyInstalled, "Selected " & mRemovedAssemblyStatusList.Item(Index).AssemblyType & " already installed. Can not be installed again.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mRemovedAssemblyStatus As AssemblyStatus
                mRemovedAssemblyStatus = AssemblyStatus.GetAssemblyStatus(Id)

                'Added by Saylee on 19-Mar-2013 for ALL14032013-1
                If CheckPeriodsForRemovedAssemblyStatus(mRemovedAssemblyStatus) = False Then
                    MSGBoxCtrl.show("Assembly Status Installation Alert!", "Periods for selected " & mRemovedAssemblyStatusList.Item(Index).AssemblyType & " are mismatching with selected Installed On " & cmbInstallOnMachine.SelectedItem.Text & " Aircraft.Can not be installed.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '******************************************

                Session("FromType") = 1 'NewInstall
                Session("IsExistingAssembly") = CType(True, Boolean)
                ''Installed Aseembly Status
                mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbInstallOnMachine.SelectedValue), txtInstallationDate.Text, mRemovedAssemblyStatus.AssemblyTypeID, True, Id.ToString)
                Session("mRemovedAssemblyStatus") = mRemovedAssemblyStatus
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mMachine") = mMachine

                'Added By Vikrant On 01-Dec-2014
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
                Session("mFileAttach") = mFileAttach
                'End
                ' '' NewMachineMaintenance() 'Added by Saylee on 6th-Oct-2009
                'Added by Vikrant on 28-July-2011
                mRegNo = mRemovedAssemblyStatusList(Index).MachineInfo
                mAssemblyType = mRemovedAssemblyStatusList(Index).AssemblyType
                mAssemblyInfo = mRemovedAssemblyStatusList(Index).AssemblyInfo
                mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                MarkLog(Util.Action.Install, "AssemblyInstallation", mAssemblyDetail, Util.ErrorType.NoError, mRemovedAssemblyStatusList(Index).AssemblyStatusID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfInstallAssembly_Ajax.aspx?BackPage=Index.aspx');", True)
        End Select
    End Sub
    Private Sub dgInstalledList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgInstalledList.PageIndexChanging
        dgInstalledList.PageIndex = e.NewPageIndex
        dgInstalledList.DataSource = mInstalledAssemblyStatusList
        Session("mInstalledAssemblyStatusList") = mInstalledAssemblyStatusList
        dgInstalledList.DataBind()
        SetGrid()
    End Sub
    Private Sub dgRemovedList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRemovedList.PageIndexChanging
        dgRemovedList.PageIndex = e.NewPageIndex
        dgRemovedList.DataSource = mRemovedAssemblyStatusList
        Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList
        dgRemovedList.DataBind()
    End Sub
    Private Sub dgInstalledList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstalledList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgInstalledList.PageSize * dgInstalledList.PageIndex
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInstallationEdit")) Then
                    'Added by Vikrant on 28-July-2011
                    mRegNo = mInstalledAssemblyStatusList(Index).MachineInfo
                    mAssemblyType = mInstalledAssemblyStatusList(Index).AssemblyType
                    mAssemblyInfo = mInstalledAssemblyStatusList(Index).AssemblyInfo
                    mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                    MarkLog(Util.Action.Edit, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to edit " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mInstalledAssemblyStatusList.Item(Index).AssemblyTypeID = 1 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.AirframeEdit, MSGBox.Message_text.AirframeEdit, "Airframe can not be edited", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf mInstalledAssemblyStatusList.Item(Index).IsMaster = True Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "This is a master record and can not be edited from here", MsgBoxStyle.OkOnly, "")
                    MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordEdit, MSGBox.Message_text.MasterRecordEdit, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mAssemblyStatus = AssemblyStatus.GetInstallAssemblyStatus(mInstalledAssemblyStatusList(Index).AssemblyStatusID, mInstalledAssemblyStatusList(Index).InstalledOnDBValue, True)
                    Session("mAssemblyStatus") = mAssemblyStatus
                    ' '' GetMachineMaintenance()  'Added by Saylee on 6th-Oct-2009
                    Session("mMachine") = mMachine
                    Session("IsExistingAssembly") = CType(True, Boolean)
                    Session("FromType") = 2 'EditInstall

                    'Added By Vikrant On 01-Dec-2014
                    If mAssemblyStatus.IsAttachmentAdded Then
                        mFileAttach = FileAttach.GetAttachment(mInstalledAssemblyStatusList(Index).AssemblyStatusID, 1) 'Sort = 1 - Installation
                        Session("mFileAttach") = mFileAttach
                    Else
                        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mInstalledAssemblyStatusList(Index).AssemblyStatusID, Sort:=1)
                        Session("mFileAttach") = mFileAttach
                    End If
                    'End

                    'Added by Vikrant on 28-July-2011
                    mRegNo = mInstalledAssemblyStatusList(Index).MachineInfo
                    mAssemblyType = mInstalledAssemblyStatusList(Index).AssemblyType
                    mAssemblyInfo = mInstalledAssemblyStatusList(Index).AssemblyInfo
                    mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                    MarkLog(Util.Action.Edit, "AssemblyInstallation", mAssemblyDetail, Util.ErrorType.NoError, mInstalledAssemblyStatusList(Index).AssemblyStatusID, EventLogID)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfInstallAssembly_Ajax.aspx?BackPage=Index.aspx');", True)
                End If
            Case "RevertInstallation"
                Index = CInt(e.CommandArgument) + dgInstalledList.PageSize * dgInstalledList.PageIndex
                If (Not User.IsInRole("AssemblyInstallationDelete")) Then
                    'Added by Vikrant on 28-July-2011
                    mRegNo = mInstalledAssemblyStatusList(Index).MachineInfo
                    mAssemblyType = mInstalledAssemblyStatusList(Index).AssemblyType
                    mAssemblyInfo = mInstalledAssemblyStatusList(Index).AssemblyInfo
                    mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                    MarkLog(Util.Action.RevertInstallation, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to revert installation " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mInstalledAssemblyStatusList.Item(Index).IsMaster = True Then
                    MSGBoxCtrl.show(MSGBox.Message_title.MasterRecordRevert, MSGBox.Message_text.MasterRecordRevert, "This is a master record and can not be reverted from here.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.ConfirmRevert, MSGBox.Message_text.ConfirmRevert, "You are trying to revert Installation.Confirm Revert?", MsgBoxStyle.YesNo, "revert")
                    mInstalledAssemblyStatusList.CurrentIndex = Index
                    Exit Sub
                End If

            Case "History"
                Index = CInt(e.CommandArgument) + dgInstalledList.PageSize * dgInstalledList.PageIndex
                If (Not User.IsInRole("AssemblyInstallationView") And Not User.IsInRole("AssemblyInstallationEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'If mInstalledAssemblyStatusList.Item(Index).AssemblyTypeID = 1 Then
                '    ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.AirframeEdit, SIMsgBox.Message_text.AirframeEdit, "Airframe can not be edited", MsgBoxStyle.OKOnly)
                '    Dim msg As New SIMsgBox(Page, "Airframe History!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
                '    'msg.ReplacePage = "wfRemovedAssemblyList_Ajax.aspx?BackPage="
                '    msg.ReplacePage = "wfRemovedAssemblyList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                '    Session("sender") = "Delete"
                '    msg.Show()
                '    Exit Sub
                'ElseIf mInstalledAssemblyStatusList.Item(Index).IsMaster = True Then
                '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
                '    'msg.ReplacePage = "wfRemovedAssemblyList_Ajax.aspx?BackPage="
                '    msg.ReplacePage = "wfRemovedAssemblyList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
                '    Session("sender") = "Delete"
                '    msg.Show()
                '    Exit Sub
                'Else

                'Added By Utkarsh On 30-Sep-2011 
                If IsDBNull(mInstalledAssemblyStatusList(Index).InstalledOnFormatted) Then
                    mAssemblyStatus = AssemblyStatus.GetInstallAssemblyStatus(mInstalledAssemblyStatusList(Index).AssemblyStatusID, Today.Date.ToString)
                Else
                    mAssemblyStatus = AssemblyStatus.GetInstallAssemblyStatus(mInstalledAssemblyStatusList(Index).AssemblyStatusID, mInstalledAssemblyStatusList(Index).InstalledOnFormatted)
                End If
                'End

                Session("mAssemblyStatus") = mAssemblyStatus
                ' '' GetMachineMaintenance()  'Added by Saylee on 6th-Oct-2009
                Session("mMachine") = mMachine
                Session("IsExistingAssembly") = CType(True, Boolean)

                'Added by Saylee on 14-Oct-2009
                Dim mUpdateHistoryAssemblyStausList As UpdateHistoryAssemblyStatusList
                Session("ModelName") = mAssemblyStatus.ModelName
                Session("RemoveDate") = txtInstallationDate.Text
                ''mUpdateHistoryAssemblyStausList = mUpdateHistoryAssemblyStausList.GetAssemblyStatusList(Guid.Empty, mAssemblyStatus.AssemblyID.ToString, , , , , , , , Today.Date.ToString, , , , , , , True)
                mUpdateHistoryAssemblyStausList = UpdateHistoryAssemblyStatusList.GetInstalledAssemblyList(txtInstallationDate.Text, mAssemblyStatus.AssemblyID)
                Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
                '======================================== 
                ''Session("FromType") = 2 'EditInstall

                'MarkLog(Util.Action.Edit, "Assembly Installation", mAssemblyDetail, Util.ErrorType.NoError, mInstalledAssemblyStatusList(Index).AssemblyStatusID, EventLogID)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfUpdateInstalledAssemblyHistory.aspx?BackPage=Index.aspx');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInstallationHistoryWindow", "OpenInstallationHistoryWindow()", True)
                'End If
            Case "View"
                Index = CInt(e.CommandArgument) + dgInstalledList.PageSize * dgInstalledList.PageIndex
                Dim mIsAttachemntAdded As Boolean = mInstalledAssemblyStatusList(Index).IsAttachmentAdded
                Dim mID As Guid = New Guid(mInstalledAssemblyStatusList(Index).AssemblyStatusID.ToString)
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "AssemblyInstallation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Vikrant on 28-July-2011
        Session("MiddleFrame") = ""
        RemoveSession()
        Session.Remove("InstallDate")
        Session.Remove("InstallOnId")
        Session.Remove("AircraftId")
        Session.Remove("AssemblyId")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgInstalledList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInstalledList.Sorting
        mInstalledAssemblyStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInstalledAssemblyStatusList") = mInstalledAssemblyStatusList
        dgInstalledList.DataSource = mInstalledAssemblyStatusList
        dgInstalledList.DataBind()
        SetGrid()
    End Sub
    'New addition by Rupali on 22-Jun-09 for Sorting Order
    Private Sub dgRemovedList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedList.Sorting
        mRemovedAssemblyStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList
        dgRemovedList.DataSource = mRemovedAssemblyStatusList
        dgRemovedList.DataBind()
    End Sub
    'Private Sub txtModel_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtModel.TextChanged
    '    Model = txtModel.Text
    'End Sub
    'Private Sub txtSerialNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSerialNo.TextChanged
    '    SerialNo = txtSerialNo.Text
    'End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub dgInstalledList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgInstalledList.Columns(i).HeaderText
            Next
        End If
    End Sub
    Protected Sub dgRemovedList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgRemovedList.Columns(i).HeaderText
            Next
        End If
    End Sub
#End Region

#Region " Report "

#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String = String.Empty
    Private SearchStr2 As String = String.Empty
    Private SearchStr3 As String = String.Empty
    Dim Model As String
    Dim SerialNo As String
#End Region

#Region " Event "
    Private Sub btnPrintRemovedAssembly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintRemovedAssembly.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstalledRemovedAssembly
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        SearchStr1 = "Aircraft :" + "  " + cmbAircraft.SelectedItem.Text
        SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        SearchStr3 = ""

        ReportDetails.Add(New rptStatus(, 0, "Installation Information", _
                          "Installation Date", txtInstallationDate.Text, , , , , , _
                              , , , , , , , , , , , , "Install On", cmbInstallOnMachine.SelectedItem.Text))
        'ReportDetails.Add(New rptStatus(, 1, , , , dgRemovedList.CaptionText))
        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgRemovedList.Columns.Item(1).HeaderText, , dgRemovedList.Columns.Item(2).HeaderText, dgRemovedList.Columns.Item(3).HeaderText, dgRemovedList.Columns.Item(4).HeaderText, _
               dgRemovedList.Columns.Item(5).HeaderText, dgRemovedList.Columns.Item(6).HeaderText, dgRemovedList.Columns.Item(7).HeaderText))
        Dim TotalCount As Integer
        TotalCount = Me.mRemovedAssemblyStatusList.Count
        Dim I As Integer
        Dim str(6) As String
        For I = 0 To TotalCount - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            If Me.dgRemovedList.Rows(I).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgRemovedList.Rows(I).Cells(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgRemovedList.Rows(I).Cells(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgRemovedList.Rows(I).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgRemovedList.Rows(I).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgRemovedList.Rows(I).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgRemovedList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgRemovedList.Rows(I).Cells(7).Text.Replace("<BR>", vbCrLf)
            ReportDetails.Add(New rptStatus(, 3, , , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Removed Assembly Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If mRemovedAssemblyStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnPrintInstalledAssembly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintInstalledAssembly.Click
        If (Not User.IsInRole("AssemblyInstallationPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim Rpt As New crListInstalledRemovedAssembly
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        SearchStr1 = "Aircraft :" + "  " + cmbAircraft.SelectedItem.Text
        SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        SearchStr3 = ""
        ReportDetails.Add(New rptStatus(, 0, "Installation Information", _
                          "Installation Date", txtInstallationDate.Text, , , , , , _
                          , , , , , , , , , , , , "Install On", cmbInstallOnMachine.SelectedItem.Text))
        'ReportDetails.Add(New rptStatus(, 1, , , , dgInstalledList.CaptionText))
        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgInstalledList.Columns.Item(0).HeaderText, , dgInstalledList.Columns.Item(1).HeaderText, dgInstalledList.Columns.Item(2).HeaderText, dgInstalledList.Columns.Item(3).HeaderText, _
                   dgInstalledList.Columns.Item(4).HeaderText, dgInstalledList.Columns.Item(5).HeaderText, dgInstalledList.Columns.Item(6).HeaderText))
        Dim TotalCount As Integer
        TotalCount = Me.mInstalledAssemblyStatusList.Count
        Dim I As Integer
        Dim str(6) As String
        For I = 0 To TotalCount - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            If Me.dgInstalledList.Rows(I).Cells(0).Text <> "&nbsp;" Then str(0) = Me.dgInstalledList.Rows(I).Cells(0).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells(1).Text <> "&nbsp;" Then str(1) = Me.dgInstalledList.Rows(I).Cells(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells(2).Text <> "&nbsp;" Then str(2) = Me.dgInstalledList.Rows(I).Cells(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells(3).Text <> "&nbsp;" Then str(3) = Me.dgInstalledList.Rows(I).Cells(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(4) = Me.dgInstalledList.Rows(I).Cells(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(5) = Me.dgInstalledList.Rows(I).Cells(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(6) = Me.dgInstalledList.Rows(I).Cells(6).Text.Replace("<BR>", vbCrLf)
            ReportDetails.Add(New rptStatus(, 3, , , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Installed Assembly Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mInstalledAssemblyStatusList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

#End Region

#End Region


    Private Sub cmbInstallOnMachine_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbInstallOnMachine.SelectedIndexChanged
        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnlyInstalledOn = mMachineNameValueList(New Guid(cmbInstallOnMachine.SelectedValue)).IsReadOnly 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnlyInstalledOn") = IsReadOnlyInstalledOn
        If cmbInstallOnMachine.Enabled = True Then
            cmbInstallOnMachine.Focus()
        End If
        Session("InstallOnId") = cmbInstallOnMachine.SelectedValue
        SetEnable()
        '*************************************************
    End Sub
    'Added By Vikrant On 27-Jul-2020 For ALL27072020
    Private Sub lnkSpareAssembly_Click(sender As Object, e As System.EventArgs) Handles lnkSpareAssembly.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSpareAssemblyInstallListWindow", "OpenSpareAssemblyInstallListWindow();", True)
    End Sub
    'End
End Class