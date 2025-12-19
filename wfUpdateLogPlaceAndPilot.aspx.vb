Imports System.Linq

Public Class wfUpdateLogPlaceAndPilot
    Inherits System.Web.UI.Page


#Region " Variable Declarations "
    Public mLogList As LogList
    Public mLog As Log                 'Added Code
    ''Public mMachineNameValueList As tmpMachineList
    Public mMachineNameValueList As MachineNameValueList
    Public mMachine As Machine
    Public AircraftId As String
    Public StartDate As String
    Public EndDate As String
    Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
    Dim mLogDetail As String

    Dim mLogTypeList As LogTypeList
    Dim mFileAttach As FileAttach

    Public mCurrentSouPlace, mCurrentDesPlace, mCurrentPilot, mCurrentCoPilot As String
    Public mSearchListPilot As SearchList
    Public mSearchListPlace As SearchList

    Dim pilotlist As PilotListAutoComplete
    Dim placelist As PlaceListAutoComplete
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        'mMachineNameValueList = CType(Session("mMachineNameValueList"), tmpMachineList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mLogList = CType(Session("mLogList"), LogList)
        AircraftId = CType(Session("AircraftId"), String)
        StartDate = CType(Session("StartDate"), String)
        EndDate = CType(Session("EndDate"), String)
        mLogTypeList = Session("mLogTypeList")
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mLogList") = mLogList
        Session("StartDate") = StartDate
        Session("EndDate") = EndDate
        Session("mLogTypeList") = mLogTypeList
    End Sub
    Private Sub RemoveSession()
        mMachineNameValueList = Nothing
        mLogList = Nothing
        Session.Remove("mMachineNameValueList")
        Session.Remove("mLogList")
        Session.Remove("mLogTypeList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfUpdateLogPlaceAndPilot.aspx" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mLogList")
            Session.Remove("AircraftId")
            Session.Remove("StartDate")
            Session.Remove("EndDate")
            Session.Remove("mLogTypeList")
        End If
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TempLogID As Guid
                        Try
                            mLogList = CType(Session("mLogList"), LogList)
                            If mLogList.CurrentItem.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mLogList.CurrentItem.ID)
                            End If
                            '''commented By Deven 10-03-2008

                            ''Dim sDATE As String = ""
                            ''Dim dDATE As String = ""
                            ''If CDate(mLogList.CurrentItem.SouLocalDateTimeForDelete).ToString("yyyy") <> "0001" Then
                            ''    sDATE = mLogList.CurrentItem.SouLocalDateTimeForDelete.ToString
                            ''End If
                            ''If CDate(mLogList.CurrentItem.DesLocalDateTimeForDelete).ToString("yyyy") <> "0001" Then
                            ''    dDATE = mLogList.CurrentItem.DesLocalDateTimeForDelete.ToString
                            ''End If
                            '''Log.DeleteLog(mLogList.Item(CurrentRowIndex).ID, mMachineID, sDATE, dDATE)
                            TempLogID = mLogList.CurrentItem.ID
                            mLogDetail = mLogList.CurrentItem.LogTextNo.ToString + " Dated : " + mLogList.CurrentItem.DateFormatted.ToString

                            Log.DeleteLog(mLogList.CurrentItem.ID, mMachine.ID, mLogList.CurrentItem.SouLocalDateTimeForDelete.ToString, mLogList.CurrentItem.DesLocalDateTimeForDelete.ToString)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            MarkLog(Util.Action.Delete, "Flight Log", mLogDetail, Util.ErrorType.NoError, mLogList.Item(mLogList.CurrentIndex).ID, EventLogID)
                            'Added by Saylee on 27-July-2009
                            Session("mAircraftInformationBoardList") = Nothing
                            '*********************************
                            DataFieldBindForPageLoad()
                            SetPage()
                            ' '' ''Response.Redirect("wfLogList.aspx?MsgResult=0&BackPage=")
                        Catch ex As SqlException
                            Dim stringInfo As String = "Other transaction(s)."
                            If ex.Message.Contains("tabnWO") Then
                                stringInfo = "Work Order."
                            ElseIf ex.Message.Contains("tabFlightDelayAndCancellation") Then
                                stringInfo = "Flight Delay/Cancellation."
                            ElseIf ex.Message.Contains("tabDentBuckle") Then
                                stringInfo = "Dent & Buckle Chart."
                            ElseIf ex.Message.Contains("tabMELSnagCorrectiveAction") Then
                                stringInfo = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect.", "MEL/Snag.") 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
                            End If
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Util.Action.Delete, "Flight Log", "Can't delete : " & mLogDetail & " is Currently in use", Util.ErrorType.NoError, TempLogID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBindForPageLoad()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Log", mLogList.Item(mLogList.CurrentIndex).LogTextNo, Util.ErrorType.NoError, Guid.Empty) 'mLogList.Item(mLogList.CurrentIndex).ID)
                                'MarkLog(Util.Action.Delete, "Log", "Aircraft Name -> " + mLogList.Item(mLogList.CurrentIndex).RegNo + " -> Log No. -> " + mLogList.Item(mLogList.CurrentIndex).LogTextNo, Util.ErrorType.NoError, mLogList.Item(mLogList.CurrentIndex).ID)
                                MarkLog(Util.Action.Delete, "Flight Log", "Deleted SuccessFully : " & mLogDetail, Util.ErrorType.NoError, TempLogID, EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.DeletedSuccessFully, MSGBox.Message_text.DeletedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    ' '' ''Response.Redirect("wfLogList.aspx?MsgResult=0&BackPage=")
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    DataFieldBindForPageLoad() 'chagned by utkarsh on 24-sep-2013 for log_ajax changes
                    If MSGBoxCtrl.Sender = "NextTLP" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVoidLogWindow", "OpenVoidLogWindow()", True)
                    End If

                    ' '' ''Response.Redirect("wfLogList.aspx?MsgResult=0&BackPage=")
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
                    DataFieldBind()
                    ' '' ''Response.Redirect("wfLogList.aspx?MsgResult=0&BackPage=")
            End Select
        ElseIf Result1 = -1 Then
            ' '' ''Response.Redirect("wfLogList.aspx?MsgResult=0&BackPage=")
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            ' DataFieldBind()
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Show_100_Records As Boolean = False)
        Session("AircraftId") = cmbAircraft.SelectedValue
        Session("StartDate") = txtStartDate.Text
        Session("EndDate") = txtEndDate.Text

        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

        mMachine = Machine.GetMachine(mMachineID) 'Added by Saylee On 12-Feb-2014 For ALL12022014-1
        Session("mMachine") = mMachine

        mLogList = Nothing
        Session.Remove("mLogList")

        mLogList = LogList.GetLogList(mMachineID, txtStartDate.Text, txtEndDate.Text, Show_100_Records, txtLogPageNo.Text.Trim)
        Session("mLogList") = mLogList

        ControlVisibility()

        DataGridBind()
        SetGrid()
    End Sub

    Private Sub DataGridBind()
        gdvLogList.DataSource = mLogList
        gdvLogList.DataBind()

        upnlGrid.Update()

    End Sub
    Private Sub SetPage()
        If mLogList Is Nothing Then
            lblResult.Text = "List of flight logs of the Aircraft as per criteria : 0 Record(s) found."
        Else
            lblResult.Text = "List of flight logs of the Aircraft as per criteria : " & mLogList.Count & " Record(s) found."
        End If

        upnlGrid.Update()
    End Sub
    Private Sub ControlVisibility()

        'If (AppSettings("LogBookTimeEntry") = "UTC") Then
        If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
            gdvLogList.Columns(6).Visible = True
            gdvLogList.Columns(9).Visible = True
            gdvLogList.Columns(5).Visible = False
            gdvLogList.Columns(8).Visible = False
        Else
            gdvLogList.Columns(6).Visible = False
            gdvLogList.Columns(9).Visible = False
            gdvLogList.Columns(5).Visible = True
            gdvLogList.Columns(8).Visible = True
        End If


        'Added By Utkarsh ON 12-Apr-2012
        If mMachine.IsTLP = True Then   ' If mLog.IsTLP = True Then   -----------Changed by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
            gdvLogList.Columns(3).HeaderText = "TLP No."
            gdvLogList.Columns(5).Visible = False
            gdvLogList.Columns(6).Visible = False
            gdvLogList.Columns(8).Visible = False
            gdvLogList.Columns(9).Visible = False

            lblLogPageNo.Visible = True
            txtLogPageNo.Visible = True
        Else
            lblLogPageNo.Visible = False
            txtLogPageNo.Visible = False
        End If
        'End
        'If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Or mLogList.Count = 0 Then



        upnlGrid.Update()
    End Sub
    Private Function CHECK_isRequiredAssembliesInstalled(ByVal mLog As Log) As Boolean
        If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved Then
            MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "You are trying to create new log. Selected machine does not have required assemblies installed. ", MsgBoxStyle.OkOnly, "")
            Return False
            ' Exit Function
        End If
        ' Dim tmpAssemblyStatusList As tmpAssemblyStatusList = tmpAssemblyStatusList.GetAssemblyStatusList(Now.ToShortDateString, New Guid(cmbAircraft.SelectedValue),  True)
        Dim mLogAssemblyInstalledList As LogAssemblyInstalledList = LogAssemblyInstalledList.GetLogAssemblyInstalledList(MachineID:=New Guid(cmbAircraft.SelectedValue), CurrentDate:=Now.ToShortDateString)

        Dim IsAirFrameAvailable As Boolean = False
        Dim IsEngineAvailable As Boolean = False
        Dim AssembliesNotFound As String = ""
        ' Dim Obj As tmpAssemblyStatusList.tmpAssemblyStatusInfo
        Dim obj As LogAssemblyInstalledList.LogAssemblyInstalledListInfo

        For Each obj In mLogAssemblyInstalledList
            If obj.AssemblyTypeID = 1 Then IsAirFrameAvailable = True
            If obj.AssemblyTypeID = 2 Then IsEngineAvailable = True
        Next

        If (Not (IsAirFrameAvailable And IsEngineAvailable)) Then
            If IsEngineAvailable = False Then AssembliesNotFound = "Engine"
            If IsAirFrameAvailable = False Then AssembliesNotFound = AssembliesNotFound + IIf(AssembliesNotFound = "", "Machine", ",Machine").ToString

            MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, " ", MsgBoxStyle.OkOnly, "")
            Return False
            ' Exit Sub
        End If
        Return True
    End Function
    Private Sub SetGrid()
        'For j As Integer = 0 To gdvLogList.Rows.Count - 1

        '    Dim P As New Integer
        '    Dim mStr As String  'Label

        '    P = CType(Me.gdvLogList.Rows(j).Cells(16).Text, Boolean)
        '    If P = False Then
        '        gdvLogList.Rows.Item(j).Cells(15).Enabled = False
        '    End If

        '    If mLogList(j).LogTypeID = 1 Then
        '        mStr = Me.gdvLogList.Rows(j).Cells(18).Text
        '        If mStr = "True" Then
        '            Me.gdvLogList.Rows(j).Cells(17).BackColor = System.Drawing.ColorTranslator.FromHtml("#ff0000")
        '        End If
        '    Else
        '        Me.gdvLogList.Rows(j).Cells(17).BackColor = System.Drawing.ColorTranslator.FromHtml("#0000FF") 'Added by Saylee on 3-Dec-2014 for ALL03122014-1 : to show Blue for Zero valued Log
        '    End If

        'Next

    End Sub
    Private Sub BindValueForChangePlace()
        txtCurrentSouPlace.Text = mCurrentSouPlace
        txtCurrentDesPlace.Text = mCurrentDesPlace

        If cmbPlace1.Enabled = True Then
            SetFocus(cmbPlace1)
        End If
        upnlPlace.Update()
    End Sub
    Private Sub BindValueForChangePilot()
        txtCurrentPilot.Text = mCurrentPilot
        txtCurrentCoPilot.Text = mCurrentCoPilot

        If cmbPilot1.Enabled = True Then
            SetFocus(cmbPilot1)
        End If
        upnlPilot.Update()
    End Sub
    Private Sub ClearChangePlaceControls()
        txtCurrentSouPlace.Text = ""
        txtCurrentDesPlace.Text = ""
        cmbPlace1.SelectedIndex = 0
        cmbPlace2.SelectedIndex = 0
    End Sub
    Private Sub SetFromAutoComplete()

        Dim tempString As String = ""
        Dim tempString1 As String = ""
        Dim Place1Code As String = ""
        Dim Place2Code As String = ""

        Dim Place1ID As Guid = Guid.Empty
        Dim Place2ID As Guid = Guid.Empty

        mSearchListPlace = SearchList.GetSearchList("Place", "", "")
        Session("mSearchListPlace") = mSearchListPlace

        If cmbPlace1.SelectedIndex > 0 Then
            tempString = cmbPlace1.SelectedItem.Text.Trim
            If Not tempString = String.Empty Then
                If tempString.IndexOf("[") >= 0 Then
                    tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                End If
                If tempString.IndexOf("[") >= 0 And tempString.IndexOf("]") >= 0 Then
                    Place1Code = tempString.Substring(tempString.IndexOf("["), tempString.IndexOf("]") - tempString.IndexOf("[")).Trim
                End If
            End If
            Place1ID = mSearchListPlace.Item(tempString).GId

        End If

        If cmbPlace2.SelectedIndex > 0 Then
            tempString1 = cmbPlace2.SelectedItem.Text.Trim
            If Not tempString1 = String.Empty Then
                If tempString1.IndexOf("[") >= 0 Then
                    tempString1 = tempString1.Substring(0, tempString1.IndexOf("[")).Trim
                End If
                If tempString1.IndexOf("[") >= 0 And tempString1.IndexOf("]") >= 0 Then
                    Place2Code = tempString1.Substring(tempString1.IndexOf("["), tempString1.IndexOf("]") - tempString1.IndexOf("[")).Trim
                End If
            End If
            Place2ID = mSearchListPlace.Item(tempString1).GId

        End If


     

        mLog = Session("LogforPlacenPilot")
        Dim mUpdatePilotAndPlaceOfLog As New UpdatePilotAndPlaceOfLog

        Try
            Dim mDetailOld As String = "Old Departure Place : " + mLog.SourceName + "; Old  Arrival Place : " + mLog.DestinationName
            mUpdatePilotAndPlaceOfLog.UpdatePlace(mLog.ID, New Guid(Session("PrevLogID").ToString), New Guid(Session("NextLogID").ToString), Place1ID, Place2ID)

            Dim mDetailNew As String = "New Departure Place : " + cmbPlace1.SelectedItem.Text.Trim + "; New Arrival Place : " + cmbPlace2.SelectedItem.Text.Trim

            MarkLog(Util.Action.Save, "UpdateLogPlacePilot", "Log Details : " + mLog.LogTextNo + Environment.NewLine + mDetailOld + Environment.NewLine + mDetailNew, Util.ErrorType.NoError, mLog.ID, EventLogID)


        Catch ex As Exception
        Finally
            mLog = Nothing
        End Try


        'End
        'End If

        'If SetValue Then

        '    If (AppSettings("ClientCode") = "Heligo") Then
        '        mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
        '    Else
        '        mLog.PilotID1 = Pilot1ID
        '    End If
        '    mLog.PilotID2 = Pilot2ID
        '    mLog.SourceID = SourceID
        '    mLog.DestinationID = DestinationID
        'End If

    End Sub

    Private Sub SePilottFromAutoComplete()
        mSearchListPilot = SearchList.GetSearchList("Pilot", "", "")
        Session("mSearchListPilot") = mSearchListPilot

        mLog = Session("LogforPlacenPilot")
        Dim mUpdatePilotAndPlaceOfLog As New UpdatePilotAndPlaceOfLog

        Try
            Dim mDetailOld As String = "Old Pilot : " + mLog.Pilot1Name + "; Old Co-Pilot : " + mLog.Pilot2Name
            mUpdatePilotAndPlaceOfLog.UpdatePilot(mLog.ID, mSearchListPilot.Item(cmbPilot1.SelectedItem.Text.Trim).GId, mSearchListPilot.Item(cmbPilot2.SelectedItem.Text.Trim).GId)
            Dim mDetailNew As String = "New Pilot : " + cmbPilot1.SelectedItem.Text.Trim + "; New Co-Pilot : " + cmbPilot2.SelectedItem.Text.Trim

            MarkLog(Util.Action.Save, "UpdateLogPlacePilot", "Log Details : " + mLog.LogTextNo + Environment.NewLine + mDetailOld + Environment.NewLine + mDetailNew, Util.ErrorType.NoError, mLog.ID, EventLogID)

        Catch ex As Exception
        Finally

            mLog = Nothing
            Session("LogforPlacenPilot") = Nothing
        End Try

    End Sub
#End Region

#Region " Data Bindings "
    Private Sub LogListBind(Optional ByVal Show_100_Records As Boolean = False)


        ''mMachineNameValueList = tmpMachineList.GetMachineList("", "", "", "", "", "<SELECT>")
        mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "<SELECT>", , SkipIsForInventoryAircarft:=True)
        Session("mMachineNameValueList") = mMachineNameValueList

        cmbAircraft.DataSource = mMachineNameValueList
        'cmbAircraft.DataBind() 'Commented By Vikrant On 10-Dec-2013 For ALL09122013-2

        If mMachineNameValueList.Count <> 0 Then
            If IsNothing(AircraftId) Then AircraftId = mMachineNameValueList(1).ID.ToString Else AircraftId = AircraftId
        Else
            AircraftId = "00000000-0000-0000-0000-000000000000"
        End If

        Session("AircraftId") = AircraftId


        placelist = PlaceListAutoComplete.GetPlaceList(AddTopItem:="(SELECT)")

        pilotlist = PilotListAutoComplete.GetPilotList(AddTopItem:="(SELECT)")

        cmbPlace1.DataSource = placelist
        cmbPlace2.DataSource = placelist
        cmbPlace1.DataBind()
        cmbPlace2.DataBind()

        cmbPilot1.DataSource = pilotlist
        cmbPilot2.DataSource = pilotlist
        cmbPilot1.DataBind()
        cmbPilot2.DataBind()


        mLogList = LogList.GetLogList(New Guid(AircraftId), txtStartDate.Text, txtEndDate.Text, Show_100_Records, txtLogPageNo.Text.Trim, SkipMaintLogAndVoidLog:=True)
        Session("mLogList") = mLogList
        DataGridBind()

        SetGrid()

        If mMachineNameValueList.Count > 1 And IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 1 Else cmbAircraft.SelectedValue = AircraftId

        mMachine = Machine.GetMachine(New Guid(AircraftId)) 'Added by Saylee On 12-Feb-2014 For ALL12022014-1
        Session("mMachine") = mMachine
        ControlVisibility()

        upnlSearchCriteria.DataBind() 'Added By Vikrant On 10-Dec-2013 For ALL09122013-2
        AircraftId = cmbAircraft.SelectedValue
        Session("AircraftId") = AircraftId



        upnlSearchCriteria.Update()
        upnlGrid.Update()
    End Sub
    Private Sub DataFieldBindForPageLoad()
        If Not IsDate(StartDate) Or Not IsDate(EndDate) Then
            'CNDC
            txtStartDate.Text = ""
            txtEndDate.Text = ""
        Else
            'CNDC
            txtStartDate.Text = StartDate
            txtEndDate.Text = EndDate
        End If

        StartDate = txtStartDate.Text
        EndDate = txtEndDate.Text

        Session("StartDate") = StartDate
        Session("EndDate") = EndDate

        mLogTypeList = LogTypeList.GetLogTypeList()


        LogListBind(True)
    End Sub
    Private Sub DataFieldBind()
        If Not IsDate(StartDate) Or Not IsDate(EndDate) Then
            'CNDC
            txtStartDate.Text = ""
            txtEndDate.Text = ""

            ''txtStartDate.Text = Today.AddMonths(-1).ToShortDateString
            ''calEndDate.Value = Today.ToShortDateString
        Else
            'CNDC
            txtStartDate.Text = StartDate
            txtEndDate.Text = EndDate
            'calStartDate.Text = StartDate
            'calEndDate.Text = EndDate
        End If

        'Commented By Vikrant On 10Dec-2013 For ALL09122013-2
        'calStartDate.DataBind()
        'calEndDate.DataBind()
        'End

        StartDate = txtStartDate.Text
        EndDate = txtEndDate.Text

        Session("StartDate") = StartDate
        Session("EndDate") = EndDate

        mLogTypeList = LogTypeList.GetLogTypeList()

        LogListBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        Dim tempString As String
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "Pilot1" Then

            If Not mSearchListPilot.Contains(cmbPilot1.SelectedItem.Text.Trim) Then
                custValidator.ErrorMessage = "Enter correct Pilot1 name."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "Place1" Then
            'Added by Utkarsh On 24-Nov-2011 For ALL23112011
            tempString = cmbPlace1.SelectedItem.Text.Trim
            If Not tempString = String.Empty Then
                'tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                If tempString.IndexOf("[") < 0 Then
                    custValidator.ErrorMessage = "Enter correct Source name."
                    e.IsValid = False
                Else
                    tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                    If Not mSearchListPlace.Contains(tempString) Then
                        custValidator.ErrorMessage = "Enter correct Source name."
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                End If
            End If
            'End
        ElseIf custValidator.ControlToValidate = "Pilot2" Then
            If Not mSearchListPilot.Contains(cmbPilot2.SelectedItem.Text.Trim) Then
                custValidator.ErrorMessage = "Enter correct Pilot2 name."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "Place2" Then
            'Added by Utkarsh On 24-Nov-2011 For ALL23112011
            tempString = cmbPlace2.SelectedItem.Text.Trim
            If Not tempString = String.Empty Then
                '  tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                If tempString.IndexOf("[") < 0 Then
                    custValidator.ErrorMessage = "Enter correct Destination name."
                    e.IsValid = False
                Else
                    tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
                    If Not mSearchListPlace.Contains(tempString) Then
                        custValidator.ErrorMessage = "Enter correct Destination name."
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                End If
            End If
            'End
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)     'Added by Prashant on 20-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If cmbAircraft.Enabled = True Then
                cmbAircraft.Focus()
            End If
            Session("MiddleFrame") = "wfUpdateLogPlaceAndPilot.aspx"
            DataFieldBindForPageLoad()
            SetGrid()
            SetPage()
            ControlVisibility()
        End If

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            If Trim(txtLogPageNo.Text) <> "" Then 'Added By Vikrant On 12-Dec-2013 For ALL09122013-2
                FindNow()
            Else 'End
                If chkShowAll.Checked = True Then
                    FindNow()
                Else
                    FindNow(True)
                End If
            End If


            SetPage()
        Else
            upnlError.Update()
            mLogList = Nothing
            Session("mLogList") = mLogList

            gdvLogList.DataSource = Nothing
            gdvLogList.DataBind()

            upnlGrid.Update()

            SetGrid()

            SetPage()
        End If
    End Sub
    Private Sub chkShowAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Trim(txtLogPageNo.Text) <> "" Then 'Added By Vikrant On 12-Dec-2013 For ALL09122013-2
            FindNow()
        Else 'End
            If chkShowAll.Checked = True Then
                FindNow()
            Else
                FindNow(True)
            End If
        End If


        SetPage()
        ControlVisibility()
        If cmbAircraft.Enabled = True Then
            cmbAircraft.Focus()
        End If
    End Sub

    Private Sub dgLogList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gdvLogList.RowCommand
        Dim Index As Int32
        Dim ID As Guid

        Select Case e.CommandName
            Case "ChangePlace"
                Index = CInt(e.CommandArgument) + gdvLogList.PageIndex * gdvLogList.PageSize
                Session("Index") = Index
                mCurrentSouPlace = mLogList(Index).SouPlaceName
                If mCurrentSouPlace = "&nbsp;" Then mCurrentSouPlace = ""

                mCurrentDesPlace = mLogList(Index).DesPlaceName
                If mCurrentDesPlace = "&nbsp;" Then mCurrentDesPlace = ""

                If Index = 0 Then
                    Session("PrevLogID") = mLogList(Index + 1).ID
                    Session("NextLogID") = Guid.Empty.ToString
                Else
                    Session("PrevLogID") = mLogList(Index + 1).ID
                    Session("NextLogID") = mLogList(Index - 1).ID
                End If

                Session.Remove("isvaluezero")
                Session.Remove("mFileAttach")
                ID = mLogList(Index).ID
                mLogDetail = mLogList(Index).LogTextNo + " Dated : " + mLogList(Index).DateFormatted

                If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
                    MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                mLog = Log.GetLog(ID)
                Session("LogforPlacenPilot") = mLog
                BindValueForChangePlace()
                mdlPopUpChangePlace.Show()
                gdvLogList.DataSource = mLogList


            Case "ChangePilot"
                Index = CInt(e.CommandArgument) + gdvLogList.PageIndex * gdvLogList.PageSize
                Session("Index") = Index
                mCurrentPilot = mLogList(Index).Pilot1Name
                If mCurrentPilot = "&nbsp;" Then mCurrentPilot = ""

                mCurrentCoPilot = mLogList(Index).Pilot2Name
                If mCurrentCoPilot = "&nbsp;" Then mCurrentCoPilot = ""



                Session.Remove("isvaluezero")
                Session.Remove("mFileAttach")
                ID = mLogList(Index).ID
                mLogDetail = mLogList(Index).LogTextNo + " Dated : " + mLogList(Index).DateFormatted

                If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
                    MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                mLog = Log.GetLog(ID)
                Session("LogforPlacenPilot") = mLog
                BindValueForChangePilot()
                mdlPopUpChangePilot.Show()
                gdvLogList.DataSource = mLogList


            Case "ViewRec"    'Added By Prashant 28-July-2009
                Index = CInt(e.CommandArgument) + gdvLogList.PageIndex * gdvLogList.PageSize
                Session("Index") = Index
                If (Not User.IsInRole("LogView")) Then
                    MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                ID = mLogList(Index).ID
                mFileAttach = FileAttach.GetAttachment(ID)
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                Else
                    'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfLogList.aspx?BackPage="
                    'msg1.Show()
                End If
                '-----------------------------------------
        End Select
    End Sub
    Private Sub dgLogList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gdvLogList.PageIndexChanging
        gdvLogList.PageIndex = e.NewPageIndex

        DataGridBind()

        Session("mLogList") = mLogList

        SetGrid()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Session.Remove("AircraftId")
        Session.Remove("StartDate")
        Session.Remove("EndDate")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        Page.Validate()
        upnlError.Update()
        '' btnFindNow_Click(sender, e) 'Commented by saylee on 21-Apr-2011
        'Added by saylee on 21-Apr-2011
        If IsValid Then
            If Trim(txtLogPageNo.Text) <> "" Then 'Added By Vikrant On 12-Dec-2013 For ALL09122013-2
                FindNow()
            Else 'End
                If chkShowAll.Checked = True Then
                    FindNow(False) 'Show all records irrespective of 100
                Else
                    FindNow(True)
                End If
            End If
            SetPage()

        Else



            mLogList = Nothing
            Session("mLogList") = mLogList
            gdvLogList.DataSource = Nothing
            gdvLogList.DataBind()
            SetPage()
        End If
        '=======================================
        ControlVisibility()
        If cmbAircraft.Enabled = True Then
            cmbAircraft.Focus()
        End If
    End Sub
    'Added By Prashant 22-June-2009 for grid sorting
    Private Sub dgLogList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvLogList.Sorting
        mLogList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLogList") = mLogList

        DataGridBind()

        SetGrid()
    End Sub
    '---------------------------------------------
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnVoidLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnVoidLog.Click
        mLogList = LogList.GetLogList(New Guid(AircraftId), txtStartDate.Text, txtEndDate.Text, chkShowAll.Checked, txtLogPageNo.Text.Trim)
        Session("mLogList") = mLogList
        DataGridBind()
        SetGrid()
        'upnlGrid.Update()
    End Sub
    Private Sub btnPlaceClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlaceClose.Click, btnDummyPlace.Click, btnPilotClose.Click
        MarkLog(Util.Action.Close, "UpdateLogPlacePilot", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mLogList = Nothing
        RemoveSession()

        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub btnPlaceOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlaceOk.Click
        If cmbPlace1.SelectedIndex = 0 And cmbPlace2.SelectedIndex = 0 Then
            MSGBoxCtrl.show("Alert..!!!", "Please enter atleast Arrival or Departure Place", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf cmbPlace1.SelectedItem.Text = cmbPlace2.SelectedItem.Text Then
            MSGBoxCtrl.show("Alert..!!!", "Arrival or Departure Place should not be same", "", MsgBoxStyle.OkOnly, "")

            Exit Sub
        End If

        SetFromAutoComplete()

        mdlPopUpChangePlace.Hide()
        Session.Remove("mCurrentSouPlace")
        Session.Remove("mCurrentDesPlace")
        ClearChangePlaceControls()
        LogListBind()
    End Sub
    Private Sub btnPilotOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPilotOk.Click
        If cmbPilot1.SelectedIndex = 0 And cmbPilot2.SelectedIndex = 0 Then
            MSGBoxCtrl.show("Alert..!!!", "Please select atleast Pilot or Co-Pilot name", "", MsgBoxStyle.OkOnly, "")

            Exit Sub
        ElseIf cmbPilot1.SelectedItem.Text = cmbPilot2.SelectedItem.Text Then
            MSGBoxCtrl.show("Alert..!!!", "Pilot or Co-Pilot name should not be same", "", MsgBoxStyle.OkOnly, "")

            Exit Sub

        End If


        SePilottFromAutoComplete()

        mdlPopUpChangePilot.Hide()
        Session.Remove("mCurrentPilot")
        Session.Remove("mCurrentCoPilot")
        ClearChangePlaceControls()
        LogListBind()
    End Sub
#End Region

End Class