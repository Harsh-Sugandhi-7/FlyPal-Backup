
'AJAX Conversion By: Saylee on 17-Mar-2015 : ModuleID:302

Public Class wfInstalledAssemblyList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'Private mMachineNameValueList As MachineList
    Private mMachineNameValueList As MachineNameValueList
    Private mInstalledAssemblyList As tmpInstalledAssemblyList
    Private mRemovedAssemblyList As tmpRemovedAssemblyList
    Private AircraftId As String
    Private RemoveDate As String
    Public mMachineMaintenance As MachineMaintenance      'Added by Saylee on 8th-Oct-2009
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
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mInstalledAssemblyList = CType(Session("mInstalledAssemblyList"), tmpInstalledAssemblyList)
        mRemovedAssemblyList = CType(Session("mRemovedAssemblyList"), tmpRemovedAssemblyList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        AircraftId = CType(Session("AircraftId"), String)
        RemoveDate = CType(Session("RemoveDate"), String)
        Model = CType(Session("Model"), String) 'Added by Rahul on 29-Apr-2009
        SerialNo = CType(Session("SerialNo"), String) 'Added by Rahul on 29-Apr-2009
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 8th-Oct-2009
        mFileAttach = Session("mFileAttach") 'Added By Vikrant On 01-Dec-2014
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AssemblyId = CType(Session("AssemblyId"), String)
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mInstalledAssemblyList")
        Session.Remove("mRemovedAssemblyList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mMachineMaintenance") 'Added by Saylee on 8th-Oct-2009
        Session.Remove("mFileAttach") 'Added By Vikrant On 01-Dec-2014
        Session.Remove("mAssemblylist")
        Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfInstalledAssemblyList_Ajax.aspx?" Then
            Session.Remove("mInstalledAssemblyList")
            Session.Remove("mRemovedAssemblyList")
            Session.Remove("mMachineNameValueList")
            Session.Remove("AircraftId")
            Session.Remove("RemoveDate")
            Session.Remove("Model") 'Added by Rahul on 29-Apr-2009
            Session.Remove("SerialNo") 'Added by Rahul on 29-Apr-2009
            Session.Remove("mMachineMaintenance") 'Added by Saylee on 8th-Oct-2009
            Session.Remove("mFileAttach") 'Added By Vikrant On 01-Dec-2014
            Session.Remove("mAssemblylist")
            Session.Remove("AssemblyId")
            Session.Remove("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GridBind()
        dgRemovedAssemblyList.DataSource = mRemovedAssemblyList
        dgRemovedAssemblyList.DataBind()
        SetGrid()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                    Else
                        Session("sender") = ""
                    End If
                Case MsgBoxResult.Yes
                    Dim AssemblyStatusID As Guid
                    Try
                        If MSGBoxCtrl.Sender = "RevertRemoval" Then
                            Session("sender") = ""
                            'Changed by vikrant on 26-July-2011
                            mRegNo = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).MachineInfo 'cmbMachine.SelectedItem.Text
                            mAssemblyType = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyType
                            mAssemblyInfo = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyInfo
                            AssemblyStatusID = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyStatusID
                            mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mRemovedAssemblyList.CurrentItem.AssemblyStatusID, 2) 'Added by Saylee on 8th-Oct-2009
                            'Added By Vikrant On 01-Dec-2014
                            If mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).IsAttachmentAdded Then
                                Dim mAssemblyStatusID As Guid = mRemovedAssemblyList.CurrentItem.AssemblyStatusID
                                mFileAttach = FileAttach.GetAttachment(mAssemblyStatusID, 2) 'Sort = 2: for Removal 
                            End If
                            AssemblyStatus.RevertRemovalAssemblyStatus(mRemovedAssemblyList.CurrentItem.AssemblyStatusID, mRemovedAssemblyList.CurrentItem.MachineID, mRemovedAssemblyList.CurrentItem.AssemblyTypeID, mRemovedAssemblyList.CurrentItem.RemovedOnDBValue)
                            'Added by Saylee on 14-July-2009
                            Session("mAircraftInformationBoardList") = Nothing
                            Try
                                MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                                'Added By Vikrant On 01-Dec-2014
                                If Not mFileAttach Is Nothing Then
                                    If mFileAttach.Size > 0 Then
                                        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID, 2) 'Sort = 2: for Removal 
                                    End If
                                End If
                                'End
                                Session("mMachineMaintenance") = mMachineMaintenance
                                'Added by Utkarsh on 11-Feb-2014 For Tech Direction Report
                                Dim mtechDirection As rptTechDirection = rptTechDirection.GetTechDirection(mRemovedAssemblyList.CurrentItem.AssemblyStatusID, 1) '2 for compoenent
                                If Not mtechDirection.IsNew Then 'there is no entry for current component.
                                    rptTechDirection.DeleteTechDirection(mtechDirection.ID)
                                End If
                                'end
                                MarkLog(Util.Action.Save, "AssemblyRemoval", "Revert: " & mAssemblyDetail, Util.ErrorType.NoError, AssemblyStatusID, EventLogID)
                                FindNow()
                                SetCaption()
                                SetGrid()
                                ControlVisibility()
                                upnlRemovedAssemblyHeader.Update()
                                upnlRemovedAssemblyList.Update()
                                upnlInstalledAssemblyHeader.Update()
                                UpnlInstalledAssemblyList.Update()
                            Catch ex As Exception
                                '
                            End Try
                        End If
                    Catch ex As SqlException
                        MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                        DataFieldBind()
                        FindNow()
                        upnlRemovedAssemblyHeader.Update()
                        upnlRemovedAssemblyList.Update()
                        msgCount = ex.Errors.Count
                    Finally
                        If msgCount = 0 Then
                            MarkLog(Util.Action.RevertRemoval, "AssemblyRemoval", mAssemblyDetail, Util.ErrorType.NoError, AssemblyStatusID, EventLogID)
                        End If
                    End Try
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetCaption()
        lblRemovedAssemblyList.Text = "List of Removed Assembly as of " & New SmartDate(calDate.Text).FormattedText & "  : " & mRemovedAssemblyList.Count & " Record(s) found."
        lblInstalledAssemblyList.Text = "List of Installed assembly as of " & New SmartDate(calDate.Text).FormattedText & " : " & mInstalledAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        btnPrintInstalledAssemblyList.Enabled = mInstalledAssemblyList.Count > 0
        btnPrintRemovedAssemblyList.Enabled = mRemovedAssemblyList.Count > 0
        SetEnable()
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean) 'Added By Vikrant On 01-Dec-2014
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 2) 'Sort = 2 - Removal
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded) 'Sort = 2 - Removal
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
    Private Sub SetEnable()
        IsReadOnly = Session("IsReadOnly") 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        For j As Integer = 0 To dgRemovedAssemblyList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgRemovedAssemblyList.Rows(j).Cells(9).Enabled = False
                dgRemovedAssemblyList.Rows(j).Cells(10).Enabled = False
            Else
                dgRemovedAssemblyList.Rows(j).Cells(9).Enabled = True
                dgRemovedAssemblyList.Rows(j).Cells(10).Enabled = True
            End If
            '*************************
        Next

        For j As Integer = 0 To dgInstalledAssemblyList.Rows.Count - 1
            'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
            'Disable Comply,Edit and Delete links if Aircraft is ReadOnly
            If IsReadOnly = True Then
                dgInstalledAssemblyList.Rows(j).Cells(10).Enabled = False
            Else
                dgInstalledAssemblyList.Rows(j).Cells(10).Enabled = True
            End If
            '*************************
        Next

        If IsReadOnly = True Then
            lblReadOnly.Visible = True
        Else
            lblReadOnly.Visible = False
        End If

        '21-Jul-2022:  Revert Removal Link disabled for all users as to avoid issues when reverting if already assembly was installed and removed after the date in this removal
        If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then
            dgRemovedAssemblyList.Columns(9).Visible = True
        Else
            dgRemovedAssemblyList.Columns(9).Visible = False
        End If
        '**************************************

        upnlRemovedAssemblyList.Update()
        UpnlInstalledAssemblyList.Update()

    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        For j As Integer = 0 To dgRemovedAssemblyList.Rows.Count - 1
            B = CType(Me.dgRemovedAssemblyList.Rows.Item(j).Cells(13).Text, Boolean)
            If B = False Then
                dgRemovedAssemblyList.Rows.Item(j).Cells(12).Enabled = False
            End If
        Next
        SetEnable()
    End Sub 'End
    Private Sub FindNow()
        mInstalledAssemblyList = tmpInstalledAssemblyList.GetInstalledAssemblyList(calDate.Text, cmbMachine.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        dgInstalledAssemblyList.DataSource = mInstalledAssemblyList
        Session("mInstalledAssemblyList") = mInstalledAssemblyList
        dgInstalledAssemblyList.DataBind()

        mRemovedAssemblyList = tmpRemovedAssemblyList.GetRemovedAssemblyList(calDate.Text, cmbMachine.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        dgRemovedAssemblyList.DataSource = mRemovedAssemblyList
        Session("mRemovedAssemblyList") = mRemovedAssemblyList
        dgRemovedAssemblyList.DataBind()
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()
        Dim TodayDate As String = Today.Date.ToString(AppSettings("DateFormat").ToString)
        If IsNothing(Session("RemoveDate")) Then
            calDate.Text = TodayDate
            RemoveDate = TodayDate 'Added By Rahul on 29-Apr-2009
        Else
            calDate.Text = RemoveDate
        End If
        Session("RemoveDate") = calDate.Text
        calDate.DataBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, SkipIsForInventoryAircarft:=True)
        cmbMachine.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        If (Session("AircraftId") = Guid.Empty.ToString Or IsNothing(Session("AircraftId"))) Then
            'Do nothing
        Else
            cmbMachine.SelectedValue = CType(Session("AircraftId"), String)
        End If
        cmbMachine.DataBind()

        'Added By Prashant 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbMachine.SelectedValue, calDate.Text.ToString, "(All)")
        cmbAircraftAssembly.DataSource = mAssemblylist
        Session("mAssemblyList") = mAssemblylist
        'If (Session("AssemblyId") = Guid.Empty.ToString Or IsNothing(Session("AssemblyId"))) Then
        '    'Do nothing
        'Else
        '    cmbAircraftAssembly.SelectedValue = CType(Session("AssemblyId"), String)
        'End If
        cmbAircraftAssembly.DataBind()
        '-----------------------------------------
        'Added by Rahul on 29-apr-09  --------------------
        mInstalledAssemblyList = tmpInstalledAssemblyList.GetInstalledAssemblyList(RemoveDate, cmbMachine.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        dgInstalledAssemblyList.DataSource = mInstalledAssemblyList
        Session("mInstalledAssemblyList") = mInstalledAssemblyList
        dgInstalledAssemblyList.DataBind()
        mRemovedAssemblyList = tmpRemovedAssemblyList.GetRemovedAssemblyList(RemoveDate, cmbMachine.SelectedValue.ToString, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
        dgRemovedAssemblyList.DataSource = mRemovedAssemblyList
        Session("mRemovedAssemblyList") = mRemovedAssemblyList
        dgRemovedAssemblyList.DataBind()

        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbMachine.SelectedValue)).IsReadOnly 'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft

        Session("IsReadOnly") = IsReadOnly
        '***********************************
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        REM:put here the code to initialize the page
        ClearAll()
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And MSGBoxCtrl.Sender = "" Then
            setFocus(cmbMachine)
            Session("MiddleFrame") = "wfInstalledAssemblyList_Ajax.aspx?"
            DataFieldBind()
            FindNow()
            ControlVisibility()
            SetCaption()
            SetGrid()
        End If
    End Sub
    Private Sub cmbMachine_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMachine.SelectedIndexChanged 'Added By Prahsnat 15-Jun-2015 
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbMachine.SelectedValue, calDate.Text.ToString, "(All)")
        cmbAircraftAssembly.DataSource = mAssemblylist
        cmbAircraftAssembly.DataBind()
        Session("mAssemblyList") = mAssemblylist
        upnlSearchCriteria.Update()
        If cmbMachine.Enabled = True Then
            setFocus(cmbMachine)
        End If
        btnFindNow_Click(sender, e)
        'Added by Saylee on 19-Jul-2018 for ALL19072018 - Restrict User from using ReadOnly Aircraft
        IsReadOnly = mMachineNameValueList(New Guid(cmbMachine.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
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
    Private Sub calDate_TextChanged(sender As Object, e As System.EventArgs) Handles calDate.TextChanged
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbMachine.SelectedValue, calDate.Text.ToString, "(All)")
        cmbAircraftAssembly.DataSource = mAssemblylist
        cmbAircraftAssembly.DataBind()
        Session("mAssemblyList") = mAssemblylist
        upnlSearchCriteria.Update()
        btnFindNow_Click(sender, e)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click 'Do not delete though button visible false
        Session("RemoveDate") = calDate.Text
        Session("AircraftId") = cmbMachine.SelectedValue
        Session("AssemblyId") = cmbAircraftAssembly.SelectedValue
        FindNow()
        SetCaption()
        ControlVisibility()
        SetGrid()
        UpnlInstalledAssemblyList.Update()
        upnlRemovedAssemblyList.Update()
        upnlInstalledAssemblyHeader.Update()
        upnlRemovedAssemblyHeader.Update()
        upnlPrintInstalledAssemblyList.Update()
        upnlButtons.Update()
    End Sub
    Private Sub dgInstalledAssemblyList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstalledAssemblyList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim InsatlledIndex As Integer = CInt(e.CommandArgument) + dgInstalledAssemblyList.PageSize * dgInstalledAssemblyList.PageIndex
                Dim mID As Guid = mInstalledAssemblyList.Item(InsatlledIndex).AssemblyStatusID
                If (Not User.IsInRole("AssemblyRemovalNew")) Then
                    'Changed by Vikrant on 26-July-2011
                    mRegNo = mInstalledAssemblyList.Item(InsatlledIndex).MachineInfo
                    mAssemblyInfo = mInstalledAssemblyList.Item(InsatlledIndex).AssemblyInfo
                    mAssemblyType = mInstalledAssemblyList.Item(InsatlledIndex).AssemblyType
                    mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                    MarkLog(Util.Action.Delete, "AssemblyRemoval", User.Identity.Name & " is not Authorized User to delete " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                   MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mInstalledAssemblyList.Item(InsatlledIndex).AssemblyTypeID = 1 Then
                    'Added by Vikrant on 26-July-2011
                    mRegNo = cmbMachine.SelectedItem.Text
                    mAssemblyType = mInstalledAssemblyList(InsatlledIndex).AssemblyType
                    mAssemblyInfo = mInstalledAssemblyList(InsatlledIndex).AssemblyInfo
                    mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                    MarkLog(Util.Action.Remove, "AssemblyRemoval", "Can't Remove : Airframe " & mAssemblyDetail & " can not be removed ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.AirframeDelete, MSGBox.Message_text.AirframeDelete, "You are trying to remove airframe.Airframe can not be removed", MsgBoxStyle.OkOnly, "Delete")
                    Exit Sub
                End If
                Dim checkRemovedAssemblyList As tmpRemovedAssemblyList
                checkRemovedAssemblyList = tmpRemovedAssemblyList.GetRemovedAssemblyList(Today.ToShortDateString, cmbMachine.SelectedValue, IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).ModelName, ""), IIf(cmbAircraftAssembly.SelectedIndex > 0, mAssemblylist(cmbAircraftAssembly.SelectedIndex).SerialNo, ""))
                If checkRemovedAssemblyList.Contains(mInstalledAssemblyList.Item(InsatlledIndex).AssemblyStatusID) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectRestriction, MSGBox.Message_text.SelectRestriction, "You are trying to remove assembly.Selected " & mInstalledAssemblyList.Item(InsatlledIndex).AssemblyType & ", Already removed, cannot remove again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mAssemblyStatus As AssemblyStatus
                mAssemblyStatus = AssemblyStatus.NewRemovalAssemblyStatus(mID, calDate.Text)
                Dim mPrevAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mID)
                Session("mPrevAssemblyStatus") = mPrevAssemblyStatus
                Session("From") = 1 'NewRemove 
                Session("mAssemblyStatus") = mAssemblyStatus
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyStatus.MachineID)
                Session("mMachine") = mMachine
                'Added By Vikrant On 01-Dec-2014
                Dim mFileAttach As FileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=2) 'Sort = 2 : Removal
                Session("mFileAttach") = mFileAttach
                'End
                'Added by Vikrant on 26-July-2011
                mRegNo = cmbMachine.SelectedItem.Text
                mAssemblyType = mInstalledAssemblyList(InsatlledIndex).AssemblyType
                mAssemblyInfo = mInstalledAssemblyList(InsatlledIndex).AssemblyInfo
                mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                MarkLog(Util.Action.Remove, "AssemblyRemoval", mAssemblyDetail, Util.ErrorType.NoError, mInstalledAssemblyList.Item(mInstalledAssemblyList.CurrentIndex).AssemblyStatusID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRemovedAssembly_Ajax.aspx?BackPage=Index.aspx');", True)
        End Select
    End Sub
    Private Sub dgRemovedAssemblyList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedAssemblyList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRemovedAssemblyList.PageSize * dgRemovedAssemblyList.PageIndex
                If (Not User.IsInRole("AssemblyRemovalView") And Not User.IsInRole("AssemblyRemovalEdit")) Then
                    'Changed by Vikrant on 26-July-2011
                    mRegNo = mRemovedAssemblyList(Index).MachineInfo
                    mAssemblyInfo = mRemovedAssemblyList(Index).AssemblyInfo
                    mAssemblyType = mRemovedAssemblyList(Index).AssemblyType
                    mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                    MarkLog(Util.Action.Edit, "AssemblyRemoval", User.Identity.Name & " is not Authorized User to edit " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mAssemblyStatus As AssemblyStatus
                mAssemblyStatus = AssemblyStatus.GetRemovalAssemblyStatus(mRemovedAssemblyList(Index).AssemblyStatusID, mRemovedAssemblyList(Index).RemovedOnDBValue, True)
                mAssemblyStatus.MarkClean()
                Dim mPrevAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mRemovedAssemblyList(Index).AssemblyStatusID)
                Session("From") = 2 'EditRemove
                Session("mPrevAssemblyStatus") = mPrevAssemblyStatus
                Session("mAssemblyStatus") = mAssemblyStatus
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyStatus.MachineID)
                Session("mMachine") = mMachine
                '' GetMachineMaintenance()  'Added by Saylee on 8th-Oct-2009
                'Added By Vikrant On 01-Dec-2014
                If mAssemblyStatus.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachment(mRemovedAssemblyList(Index).AssemblyStatusID, 2) 'Sort = 2 : Removal
                    Session("mFileAttach") = mFileAttach
                Else
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mRemovedAssemblyList(Index).AssemblyStatusID, Sort:=2)
                    Session("mFileAttach") = mFileAttach
                End If
                'End
                'Added by Vikrant on 26-July-2011
                mRegNo = cmbMachine.SelectedItem.Text
                mAssemblyType = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyType
                mAssemblyInfo = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyInfo
                mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                MarkLog(Util.Action.Edit, "AssemblyRemoval", mAssemblyDetail, Util.ErrorType.NoError, mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyStatusID, EventLogID)
                upnlRemovedAssemblyList.Update()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfRemovedAssembly_Ajax.aspx?BackPage=Index.aspx');", True)
            Case "RevertRemoval"
                GridBind()
                Dim Index As Integer = CInt(e.CommandArgument) + dgRemovedAssemblyList.PageSize * dgRemovedAssemblyList.PageIndex

                If (Not User.IsInRole("AssemblyRemovalDelete")) Then
                    'Changed by Vikrant on 26-July-2011
                    mRegNo = mRemovedAssemblyList(Index).MachineInfo
                    mAssemblyInfo = mRemovedAssemblyList(Index).AssemblyInfo
                    mAssemblyType = mRemovedAssemblyList(Index).AssemblyType
                    mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                    MarkLog(Util.Action.RevertRemoval, "AssemblyRemoval", mAssemblyDetail, Util.ErrorType.HandledError, mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyStatusID, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Added by Vikrant on 26-July-2011
                mRegNo = cmbMachine.SelectedItem.Text
                mAssemblyType = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyType
                mAssemblyInfo = mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyInfo
                mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                MarkLog(Util.Action.RevertRemoval, "AssemblyRemoval", mAssemblyDetail, Util.ErrorType.HandledError, mRemovedAssemblyList.Item(mRemovedAssemblyList.CurrentIndex).AssemblyStatusID, EventLogID)
                'End

                mRemovedAssemblyList.CurrentIndex = Index
                Session("mRemovedAssemblyList") = mRemovedAssemblyList
                 MSGBoxCtrl.show("Revert Confirmation!", "Confirm Revert Assembly Removal <BR> <BR> Do you want to Revert the current Removed Assembly?", "", MsgBoxStyle.YesNo, "RevertRemoval")
            Case "History"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRemovedAssemblyList.PageSize * dgRemovedAssemblyList.PageIndex
                If (Not User.IsInRole("AssemblyRemovalView") And Not User.IsInRole("AssemblyRemovalEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim mAssemblyStatus As AssemblyStatus
                mAssemblyStatus = AssemblyStatus.GetRemovalAssemblyStatus(mRemovedAssemblyList(Index).AssemblyStatusID, mRemovedAssemblyList(Index).RemovedOnDBValue)
                mAssemblyStatus.MarkClean()
                Dim mPrevAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mRemovedAssemblyList(Index).AssemblyStatusID)
                ''Session("From") = 2 'EditRemove
                Session("mPrevAssemblyStatus") = mPrevAssemblyStatus
                Session("mAssemblyStatus") = mAssemblyStatus
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyStatus.MachineID)
                Session("mMachine") = mMachine

                'Added by Saylee on 14-Oct-2009
                Dim mUpdateHistoryAssemblyStausList As UpdateHistoryAssemblyStatusList
                Session("ModelName") = mAssemblyStatus.ModelName
                Session("SerialNo") = mAssemblyStatus.Assembly.SerialNo
                mUpdateHistoryAssemblyStausList = UpdateHistoryAssemblyStatusList.GetRemovedAssemblyList(calDate.Text, mAssemblyStatus.AssemblyID)
                Session("mUpdateHistoryAssemblyStausList") = mUpdateHistoryAssemblyStausList
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemHistoryWindow", "OpenRemHistoryWindow()", True)
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRemovedAssemblyList.PageSize * dgRemovedAssemblyList.PageIndex
                Dim mID As Guid = mRemovedAssemblyList(Index).AssemblyStatusID
                Dim mIsAttachemntAdded As Boolean = mRemovedAssemblyList(mID).IsAttachmentAdded 'e.Item.Cells(14).Text
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Session.Remove("AircraftId")
        Session.Remove("RemoveDate")
        Session.Remove("AssemblyId")
        Response.Redirect("Dashboard.aspx?BackPage=")
    End Sub
    'Private Sub txtModel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtModel.TextChanged
    '    Model = txtModel.Text
    'End Sub
    'Private Sub txtSerialNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSerialNo.TextChanged
    '    SerialNo = txtSerialNo.Text
    'End Sub
    Private Sub dgInstalledAssemblyList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInstalledAssemblyList.Sorting
        mInstalledAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInstalledAssemblyList") = mInstalledAssemblyList
        dgInstalledAssemblyList.DataSource = mInstalledAssemblyList
        dgInstalledAssemblyList.DataBind()
    End Sub
    Private Sub dgRemovedAssemblyList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRemovedAssemblyList.Sorting
        mRemovedAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRemovedAssemblyList") = mRemovedAssemblyList
        dgRemovedAssemblyList.DataSource = mRemovedAssemblyList
        dgRemovedAssemblyList.DataBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub dgInstalledAssemblyList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgInstalledAssemblyList.Columns(i).HeaderText
            Next
        End If
    End Sub
    Protected Sub dgRemovedAssemblyList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgRemovedAssemblyList.Columns(i).HeaderText
            Next
        End If
    End Sub
#End Region

#Region " Report "

#Region " Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Dim Model As String
    Dim SerialNo As String
#End Region

#Region " Event "
    Private Sub btnPrintInstalledAssemblyList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintInstalledAssemblyList.Click
        If (Not User.IsInRole("AssemblyRemovalPrint")) Then
             MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim Rpt As New crListInstalledRemovedAssembly
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "Aircraft :" + "  " + cmbMachine.SelectedItem.Text
        SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        SearchStr3 = ""

        ReportDetails.Add(New rptStatus(, 0, "Search Criteria", _
                          lblRemovalDate.Text, New SmartDate(calDate.Text).FormattedText))
        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgInstalledAssemblyList.Columns.Item(1).HeaderText, , dgInstalledAssemblyList.Columns.Item(2).HeaderText, dgInstalledAssemblyList.Columns.Item(3).HeaderText, dgInstalledAssemblyList.Columns.Item(4).HeaderText, _
               dgInstalledAssemblyList.Columns.Item(5).HeaderText, dgInstalledAssemblyList.Columns.Item(6).HeaderText, dgInstalledAssemblyList.Columns.Item(7).HeaderText))
        Dim TotalCount As Integer
        TotalCount = Me.mInstalledAssemblyList.Count
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

            If Me.dgInstalledAssemblyList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgInstalledAssemblyList.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledAssemblyList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgInstalledAssemblyList.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledAssemblyList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgInstalledAssemblyList.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledAssemblyList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgInstalledAssemblyList.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledAssemblyList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgInstalledAssemblyList.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledAssemblyList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgInstalledAssemblyList.Rows(I).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgInstalledAssemblyList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgInstalledAssemblyList.Rows(I).Cells.Item(7).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, , _
             , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Installed Assembly Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mInstalledAssemblyList.Count = 0 Then
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
    Private Sub btnPrintRemovedAssemblyList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintRemovedAssemblyList.Click
        If (Not User.IsInRole("AssemblyRemovalPrint")) Then
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            msg.ReplacePage = "wfInstalledAssemblyList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            msg.Show()
            Exit Sub
        End If
        Dim Rpt As New crListInstalledRemovedAssembly
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "Aircraft :" + "  " + cmbMachine.SelectedItem.Text
        SearchStr2 = "Assembly :" + "  " + IIf(cmbAircraftAssembly.SelectedIndex > 0, cmbAircraftAssembly.SelectedItem.Text, "")
        SearchStr3 = ""

        ReportDetails.Add(New rptStatus(, 0, "Search Criteria", _
                          lblRemovalDate.Text, New SmartDate(calDate.Text).FormattedText))

        ReportDetails.Add(New rptStatus(, 2, , _
              , , , dgRemovedAssemblyList.Columns.Item(1).HeaderText, , dgRemovedAssemblyList.Columns.Item(2).HeaderText, dgRemovedAssemblyList.Columns.Item(3).HeaderText, dgRemovedAssemblyList.Columns.Item(4).HeaderText, _
               dgRemovedAssemblyList.Columns.Item(5).HeaderText, dgRemovedAssemblyList.Columns.Item(6).HeaderText, dgRemovedAssemblyList.Columns.Item(7).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mRemovedAssemblyList.Count
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

            If Me.dgRemovedAssemblyList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgRemovedAssemblyList.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedAssemblyList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgRemovedAssemblyList.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedAssemblyList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgRemovedAssemblyList.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedAssemblyList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgRemovedAssemblyList.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedAssemblyList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgRemovedAssemblyList.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedAssemblyList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgRemovedAssemblyList.Rows(I).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgRemovedAssemblyList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgRemovedAssemblyList.Rows(I).Cells.Item(7).Text.Replace("<BR>", vbCrLf)
            ReportDetails.Add(New rptStatus(, 3, , _
             , , , , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "List of Removed Assembly Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If mRemovedAssemblyList.Count = 0 Then
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
End Class