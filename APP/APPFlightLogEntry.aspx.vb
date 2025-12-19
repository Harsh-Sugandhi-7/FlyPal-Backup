
'Created BY : Saylee
'Dated      : 27-Dec-2023

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web.Script.Serialization

Public Class APPFlightLogEntry
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    Public mLog As Log
    Public mMachine As Machine
    Public mFlightLogClassificationList As FlightLogClassificationList
    Private LogListCount As Integer = 0
    Dim EventLogID As Guid
    Private Flag As Int16
    Dim takeofftouchdown As Boolean
    Dim mLogDetail As String
    Dim Pilot1ID As Guid
    Dim Pilot2ID As Guid
    Dim SourceID As Guid
    Dim DestinationID As Guid
    Dim SetValue As Boolean = False
    Dim isvaluezero As Boolean = False
    Public Event TextChanged As EventHandler
    Dim mCompanyDetail As New CompanyDetail
    Public mSearchListPilot As SearchList
    Public mSearchListPlace As SearchList
#End Region

#Region "Methods"
    Private Sub GetSession()
        mUser = Session("User")
        mGBUser = Session("GBUser")
        mLog = CType(Session("mLog"), Log)
        mFlightLogClassificationList = CType(Session("mFlightLogClassificationList"), FlightLogClassificationList)
        mMachine = CType(Session("mMachine"), Machine)
        mSearchListPlace = Session("mSearchListPlace")
        mSearchListPilot = Session("mSearchListPilot")

    End Sub
    Private Sub SetSession()
        Session("mLog") = mLog
        Session("mMachine") = mMachine
        ''  Session("mLogList") = mLogList
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        Session("LogListCount") = LogListCount

        Session("mSearchListPlace") = mSearchListPlace
        Session("mSearchListPilot") = mSearchListPilot

        'Added By Utkarsh On 21-Sep-2011
        Session("Pilot1ID") = Pilot1ID
        Session("Pilot2ID") = Pilot2ID
        Session("SourceID") = SourceID
        Session("DestinationID") = DestinationID
        Session("SetValue") = SetValue
        'End

        Session("mCompanyDetail") = mCompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachine")
        ''Session.Remove("mLogList")
        Session.Remove("mLog")
        Session.Remove("LogListCount")
        Session.Remove("mSearchListPlace")
        Session.Remove("mSearchListPilot")
        'Added By Utkarsh On 12-Sep-2011
        Session.Remove("Pilot1ID")
        Session.Remove("Pilot2ID")
        Session.Remove("SourceID")
        Session.Remove("DestinationID")
        Session.Remove("SetValue")
        'End
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mLogListOnDate")
        Session.Remove("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
    End Sub
    Private Sub SetFromAutoComplete()
        Dim tempString As String
        Dim tempString1 As String
        Dim Place1Code As String
        Dim Place2Code As String

        tempString = Place1.Text.Trim
        If Not tempString = String.Empty Then
            If tempString.IndexOf("[") >= 0 Then
                tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
            End If
            If tempString.IndexOf("[") >= 0 And tempString.IndexOf("]") >= 0 Then
                Place1Code = tempString.Substring(tempString.IndexOf("["), tempString.IndexOf("]") - tempString.IndexOf("[")).Trim
            End If
        End If

        tempString1 = Place2.Text.Trim
        If Not tempString1 = String.Empty Then
            If tempString1.IndexOf("[") >= 0 Then
                tempString1 = tempString1.Substring(0, tempString1.IndexOf("[")).Trim
            End If
            If tempString1.IndexOf("[") >= 0 And tempString1.IndexOf("]") >= 0 Then
                Place2Code = tempString1.Substring(tempString1.IndexOf("["), tempString1.IndexOf("]") - tempString1.IndexOf("[")).Trim
            End If
        End If
        'End


        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
            mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
            mLog.Pilot1Name = "None"
        Else
            mLog.PilotID1 = mSearchListPilot.Item(Pilot1.Text.Trim).GId
            mLog.Pilot1Name = mSearchListPilot.Item(Pilot1.Text.Trim).Name
        End If

        mLog.PilotID2 = mSearchListPilot.Item(Pilot2.Text.Trim).GId
        mLog.Pilot2Name = mSearchListPilot.Item(Pilot2.Text.Trim).Name

        mLog.SourceID = mSearchListPlace.Item(tempString).GId
        mLog.DestinationID = mSearchListPlace.Item(tempString1).GId



    End Sub
    Private Sub ShowAlertMsg(ByVal Msg As String, ByVal MsgTitle As String, Optional ShowAgreebutton As Boolean = False, Optional AgreeString As String = "")

        Dim str As String
        If ShowAgreebutton = False Then
            str = "opennotificationpopup('" & Msg & "','" & MsgTitle & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
        Else
            str = "openAgreenotificationpopup('" & Msg & "','" & MsgTitle & "','" & AgreeString & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)
        End If

    End Sub
    Private Sub btnAgree_Click(sender As Object, e As EventArgs) Handles btnAgree.Click
        Select Case hdnDummyAgreeString.Value
            Case "SaveNew"
                mLog = Session("mLog")
                DataFieldBind()
                DataBind()

                If mLog.IsValid Then
                    If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    If Save() = True Then
                        'mLog = Log.GetLog(mLog.ID)
                        SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                        NewRecord(Today.Date.ToString, CType(Today.Date.ToString.Trim + " " + "0:00", DateTime).ToString)
                        Session.Remove("mFileAttach")
                        Session.Remove("IsAttachmentDeleted")
                        Session("mLog") = mLog

                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        'Added By Vikrant on 01-Dec-2021 for PBH
                        If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                            If Session("IsAircraftMadeNotInUse") = "True" Then
                                Session.Remove("AircraftId")
                                Session.Remove("IsAircraftMadeNotInUse")
                                ''MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                Exit Sub
                            End If
                        End If
                        'End
                        DataFieldBind()
                        EnableDisableButton()
                        DataBindGrid()

                        SetTitle()
                    End If
                Else
                    ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    upnlErrorList.Update()
                End If
            Case "Close"

                'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
                If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
                    Dim Title As String = "Save Alert !"
                    Dim Message As String = ""
                    If Not mLog.PilotID1.Equals(Guid.Empty) Then
                        Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
                        If mEmployeeStatus(0).Information <> "" Then
                            Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
                        End If
                    End If
                    If Not mLog.PilotID2.Equals(Guid.Empty) Then
                        Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
                        If mEmployeeStatus(0).Information <> "" Then
                            Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
                        End If
                    End If
                    If Message.Length > 0 Then
                        DataFieldBind()
                        Session("sender") = ""
                        ' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
                        'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg(Message, "Save Alert!")
                        Exit Sub
                    End If
                End If
                'End

                Dim mMaxLogOfAircraft As MaxLogOfAircraft
                mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

                If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                    'Added by Saylee on 18-May-2012 ALL17052012
                    ' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
                    'If (AppSettings("ClientCode") <> "Heligo") Then
                    If Not (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
                        'End
                        Dim MaxLogDateTime As String = ""

                        '  If (AppSettings("LogBookTimeEntry") = "UTC") Then
                        If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                            MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted
                        Else
                            MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted
                        End If

                        mLog = Session("mLog")
                        DataFieldBind()

                        If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
                            Session("SaveNClose") = "SaveNClose"
                            'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                            ShowAlertMsg("You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", "Save Alert!", True, "SaveLogFlexiLog")
                            Exit Sub
                        End If
                    Else

                        mLog = Session("mLog")
                        DataFieldBind()

                        If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
                            Session("SaveNClose") = "SaveNClose"
                            'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                            ShowAlertMsg("You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", "Save Alert!", True, "SaveLogFlexiLog")
                            Exit Sub
                        End If
                    End If
                End If

                'Added By Prashant 12-Apr-2010
                Dim IsMELCount As Boolean = False
                Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList

                mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)

                For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
                    'If (mTempMELSnagCorrectiveActionList(i).DueDate <= mLog.Date) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = True) Then
                    If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
                        If (CDate(mLog.Date) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
                            IsMELCount = True
                            Exit For
                        Else
                            IsMELCount = False
                        End If
                    End If
                Next
                mTempMELSnagCorrectiveActionList = Nothing
                '-----------------------------------

                If IsMELCount = True Then
                    ' MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue?", "", MsgBoxStyle.YesNo, "MELClose")
                    ShowAlertMsg("Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue?", "Minimum Equipment Level", True, "MELClose")
                    DataBind() 'Added By Utkarsh On 12-Sep-2011
                    Exit Sub
                Else

                    mLog = Session("mLog")
                    DataFieldBind()

                    ''DataBind()
                    If mLog.IsValid Then
                        If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                        Session("SaveNClose") = "SaveNClose"
                        If Save() = True Then
                            mLog = Log.GetLog(mLog.ID)
                            mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                            mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                            mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                            Session("mLog") = mLog
                            Session.Remove("mFileAttach")
                            Session.Remove("IsAttachmentDeleted")
                            SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                            'Added By Vikrant on 01-Dec-2021 for PBH
                            If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                If Session("IsAircraftMadeNotInUse") = "True" Then
                                    Session.Remove("AircraftId")
                                    Session.Remove("IsAircraftMadeNotInUse")
                                    '' MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                    ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                    Exit Sub
                                End If
                            End If
                            'End
                            Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Else
                            Exit Sub
                        End If
                    Else
                        ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                        upnlErrorList.Update()
                    End If
                End If
                ''Case "SaveLogAfterHrsSame"  'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

                ''    mLog = Session("mLog")
                ''    Session("isvaluezero") = "True"
                ''    DataFieldBind()

                ''    ''DataBind()
                ''    If mLog.IsValid Then
                ''        If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                ''        If SaveLogAfterHrsSame() = True Then
                ''            If Session("New") = "True" Then
                ''                Session("New") = ""
                ''                SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                ''                NewRecord()
                ''                Session.Remove("mFileAttach")
                ''                Session.Remove("IsAttachmentDeleted")
                ''                Session("mLog") = mLog

                ''                ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                ''                ' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

                ''                DataFieldBind()

                ''                EnableDisableButton()
                ''                ControlVisibility()
                ''                ControlVisibilityForAttachment()
                ''                DataBindGrid()

                ''                SetTitle()
                ''                mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
                ''                Session("mLogListOnDate") = mLogListOnDate
                ''                If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
                ''                    '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
                ''                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
                ''                    upnlLogInfo.Update()
                ''                End If
                ''                'Added By Vikrant on 01-Dec-2021 for PBH
                ''                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                ''                    If Session("IsAircraftMadeNotInUse") = "True" Then
                ''                        Session.Remove("AircraftId")
                ''                        Session.Remove("IsAircraftMadeNotInUse")
                ''                        MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                ''                    End If
                ''                End If
                ''                'End
                ''                upnlLogDetails.Update()
                ''                upnlFlightDetails.Update()
                ''                upnlFlightSummary.Update()
                ''                upnlTabs.Update()
                ''                upnlTabsNew.Update()
                ''            Else
                ''                mLog = Log.GetLog(mLog.ID)
                ''                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                ''                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                ''                mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                ''                Session("mLog") = mLog
                ''                SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                ''                SetTitle()
                ''                DataFieldBind()
                ''                EnableDisableButton()


                ''                'Added By Vikrant on 01-Dec-2021 for PBH
                ''                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                ''                    If Session("IsAircraftMadeNotInUse") = "True" Then
                ''                        Session.Remove("AircraftId")
                ''                        Session.Remove("IsAircraftMadeNotInUse")
                ''                        MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                ''                        Exit Sub
                ''                    End If
                ''                End If
                ''                'End
                ''                upnlTabsNew.Update()
                ''                If Session("SaveNClose") = "SaveNClose" Then
                ''                    Session("SaveNClose") = ""
                ''                    Session.Remove("SaveNClose")
                ''                    Session.Remove("mFileAttach")
                ''                    Session.Remove("IsAttachmentDeleted")
                ''                    Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                ''                End If
                ''            End If

                ''        End If
                ''    Else
                ''        ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                ''        upnlErrorList.Update()

                ''    End If
            Case "MELClose"
                mLog = Session("mLog")
                DataFieldBind()
                DataBind()

                If mLog.IsValid Then
                    If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    If Save() = True Then
                        mLog = Log.GetLog(mLog.ID)
                        mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                        mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                        mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                        Session("mLog") = mLog
                        Session.Remove("mFileAttach")
                        Session.Remove("IsAttachmentDeleted")
                        SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                        'Added By Vikrant on 01-Dec-2021 for PBH
                        If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                            If Session("IsAircraftMadeNotInUse") = "True" Then
                                Session.Remove("AircraftId")
                                Session.Remove("IsAircraftMadeNotInUse")
                                'MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                Exit Sub
                            End If
                        End If
                        'End
                        Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                    End If
                Else
                    ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    upnlErrorList.Update()
                End If
            Case "MEL"
                mLog = Session("mLog")
                DataFieldBind()
                DataBind()

                If mLog.IsValid Then
                    If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    If Save() = True Then
                        mLog = Log.GetLog(mLog.ID)
                        mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                        mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                        mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                        Session("mLog") = mLog
                        SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                        If Session("SaveNClose") = "SaveNClose" Then
                            Session("SaveNClose") = ""
                            Session.Remove("SaveNClose")
                            Session.Remove("mFileAttach")
                            Session.Remove("IsAttachmentDeleted")
                            SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                            'Added By Vikrant on 01-Dec-2021 for PBH
                            If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                If Session("IsAircraftMadeNotInUse") = "True" Then
                                    Session.Remove("AircraftId")
                                    Session.Remove("IsAircraftMadeNotInUse")
                                    'MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                    ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                    Exit Sub
                                End If
                            End If
                            'End
                            Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Else
                            ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                            ' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                            'Added By Vikrant on 01-Dec-2021 for PBH
                            If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                If Session("IsAircraftMadeNotInUse") = "True" Then
                                    Session.Remove("AircraftId")
                                    Session.Remove("IsAircraftMadeNotInUse")
                                    'MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                    ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                    Exit Sub
                                End If
                            End If
                            'End
                            DataFieldBind()
                            EnableDisableButton()
                            DataBindGrid()
                            SetTitle()


                        End If
                    End If
                Else
                    ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    upnlErrorList.Update()
                End If

            Case "SaveLogFlexiLog"  'Added by Saylee on 21-May-2012 ALL17052012 to save Flexi log

                mLog = Session("mLog")
                DataFieldBind()
                ''DataBind()

                If mLog.IsValid Then
                    If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    If SaveLogFlexiLog() = True Then
                        If Session("New") = "True" Then
                            Session("New") = ""
                            NewRecord(Today.Date.ToString, CType(Today.Date.ToString.Trim + " " + "0:00", DateTime).ToString)
                            Session.Remove("mFileAttach")
                            Session.Remove("IsAttachmentDeleted")
                            Session("mLog") = mLog
                            'Added By Vikrant on 01-Dec-2021 for PBH
                            If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                If Session("IsAircraftMadeNotInUse") = "True" Then
                                    Session.Remove("AircraftId")
                                    Session.Remove("IsAircraftMadeNotInUse")
                                    ' MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                    ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                End If
                            End If
                            'End
                            DataFieldBind()

                            EnableDisableButton()
                            DataBindGrid()

                            SetTitle()

                            SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                            ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                            ' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Else
                            mLog = Log.GetLog(mLog.ID)
                            mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                            mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                            mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                            Session("mLog") = mLog

                            SetTitle()
                            DataFieldBind()
                            EnableDisableButton()

                            If Session("SaveNClose") = "SaveNClose" Then
                                Session("SaveNClose") = ""
                                Session.Remove("SaveNClose")
                                Session.Remove("mFileAttach")
                                Session.Remove("IsAttachmentDeleted")
                                Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                            End If
                        End If

                    End If

                Else
                    ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    upnlErrorList.Update()
                End If
            Case "MELNew"
                mLog = Session("mLog")
                DataFieldBind()
                DataBind()

                If mLog.IsValid Then
                    If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    Session("New") = "True"
                    If Save() = True Then
                        'mLog = Log.GetLog(mLog.ID)
                        SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                        NewRecord(Today.Date.ToString, CType(Today.Date.ToString.Trim + " " + "0:00", DateTime).ToString)
                        Session.Remove("mFileAttach")
                        Session.Remove("IsAttachmentDeleted")
                        Session("mLog") = mLog

                        ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                        ' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        DataFieldBind()
                        EnableDisableButton()
                        DataBindGrid()
                        SetTitle()

                        'Added By Vikrant on 01-Dec-2021 for PBH
                        If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                            If Session("IsAircraftMadeNotInUse") = "True" Then
                                Session.Remove("AircraftId")
                                Session.Remove("IsAircraftMadeNotInUse")
                                'MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                Exit Sub
                            End If
                        End If
                        'End

                    End If
                Else
                    ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                    upnlErrorList.Update()
                End If
                'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
            Case "SaveLogAfterAvgFlightTimeDeviationWarning"
                mLog = Session("mLog")
                DataFieldBind()
                If mLog.IsValid Then
                    If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub
                    If SaveLogAfterAvgFlightTimeDeviationWarning() = True Then
                        If Session("New") = "True" Then
                            Session("New") = ""
                            NewRecord(Today.Date.ToString, CType(Today.Date.ToString.Trim + " " + "0:00", DateTime).ToString)
                            Session.Remove("mFileAttach")
                            Session.Remove("IsAttachmentDeleted")
                            Session("mLog") = mLog
                            'Added By Vikrant on 01-Dec-2021 for PBH
                            If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                                If Session("IsAircraftMadeNotInUse") = "True" Then
                                    Session.Remove("AircraftId")
                                    Session.Remove("IsAircraftMadeNotInUse")
                                    'MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                                    ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Save Alert!", True, "AircraftMadeNotInUse")
                                End If
                            End If
                            'End
                            DataFieldBind()

                            EnableDisableButton()

                            DataBindGrid()

                            SetTitle()


                            ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                            ' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Else
                            mLog = Log.GetLog(mLog.ID)
                            mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                            mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                            mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                            Session("mLog") = mLog

                            SetTitle()
                            DataFieldBind()
                            EnableDisableButton()

                            If Session("SaveNClose") = "SaveNClose" Then
                                Session("SaveNClose") = ""
                                Session.Remove("SaveNClose")
                                Session.Remove("mFileAttach")
                                Session.Remove("IsAttachmentDeleted")
                                Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                            End If
                        End If
                    Else
                        ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                        upnlErrorList.Update()
                    End If
                End If
        End Select
    End Sub
    Private Sub btnDisAgree_Click(sender As Object, e As EventArgs) Handles btnDisagree.Click

    End Sub
    Private Sub SetObject()
        With mLog
            'CNDC
            ''''''''''''If Not (calDateTime.IsDateValue) Then
            If Not IsDate(calDateTime.Text) Then
                .Date = System.DBNull.Value
            Else
                .Date = calDateTime.Text.ToString.Trim
            End If

            .LogText = Trim(txtLogText.Text)

            .LogNo = CInt(Val(Trim(txtLogNo.Text)))
            If .IsUTC = True Then
                'CNDC
                ''''''''''''If Not (CalUTCDateTime.IsDateValue) Then
                If Not IsDate(CalUTCDateTime.Text) Then
                    .SouUniverseDateTime = System.DBNull.Value
                Else
                    .SouUniverseDateTime = CType(CalUTCDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime)
                End If


            Else

                'CNDC ''''''''''''''If Not (calDeparture.IsDateValue) Then
                If Not IsDate(calDeparture.Text) Then
                    .SouLocalDateTime = System.DBNull.Value
                Else
                    .SouLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime)
                    ' .SouLocalDateTime = CType(calDeparture.Text.ToString + " " + calDepartureTime.Text.ToString, DateTime)
                End If

            End If


            If .IsUTC = True Then
                If Not IsDate(CalUTCArrival.Text) Then
                    .DesUniverseDateTime = System.DBNull.Value
                Else
                    .DesUniverseDateTime = CType(CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim, DateTime)
                End If

            Else
                If Not IsDate(calArrival.Text) Then
                    .DesLocalDateTime = System.DBNull.Value
                Else
                    .DesLocalDateTime = CType(calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim, DateTime)
                End If
            End If
            If .IsUTC Then
                If takeofftouchdown Then
                    If Not IsDate(calUTCTakeOffDateTime.Text) Then
                        .TakeOffUniverseDateTime = System.DBNull.Value
                    Else
                        .TakeOffUniverseDateTime = CType(calUTCTakeOffDateTime.Text.ToString.Trim + " " + txtUTCTakeOffTime.Text.ToString.Trim, DateTime)
                    End If

                    If Not IsDate(calUTCTouchDownDateTime.Text) Then
                        .TouchDownUniverseDateTime = System.DBNull.Value
                    Else
                        .TouchDownUniverseDateTime = CType(calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim, DateTime)
                    End If
                End If
                'End
            Else
                If takeofftouchdown Then
                    If Not IsDate(calTakeOffLocalDateTime.Text) Then
                        .TakeOffLocalDateTime = System.DBNull.Value
                    Else
                        .TakeOffLocalDateTime = CType(calTakeOffLocalDateTime.Text.ToString.Trim + " " + txtTakeOffLocalTime.Text.ToString.Trim, DateTime)
                    End If

                    If Not IsDate(calTouchDownLocalDateTime.Text) Then
                        .TouchDownLocalDateTime = System.DBNull.Value
                    Else
                        .TouchDownLocalDateTime = CType(calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim, DateTime)
                    End If
                End If
            End If

            If AppSettings("SetBlockTime") = "True" Then
                If Not .BlockTime.Equals(Trim(txtBlockTime.Text)) Then
                    .BlockTime = Trim(txtBlockTime.Text)
                    txtAirBorneTime.DataBind()
                    txtGroundRunTime.DataBind()
                End If

            End If

            If AppSettings("LogDetailPage") = "OldPage" Then
                .TimeInAir = Trim(txtAirBorneTime.Text)

                If Not AppSettings("Log") = "True" Then .TimeOnGround = Trim(txtGroundRunTime.Text)
            End If


            .PercentTimeOnGround = Val(Trim(txtPercentTimeOnGround.Text))

            'If mMachine.HourType = 2 Then
            '    .PrevHobbsValue = Trim(txtPrevHobbsValue.Text)
            '    .PrevHobbsOffsetValue = Trim(txtPrevHobbsOffset.Text)
            '    .CurrentHobbsOffsetValue = Trim(txtCurrentHobbsOffset.Text)
            '    .CurrentHobbsValue = Trim(txtCurrentHobbsValue.Text)
            '    .OffSet = Trim(txtCurrentHobbsOffset.Text)
            'End If

            .LogPageNo = txtLogPageNo.Text
            .FlightNo = txtFlightNo.Text.Trim
            .Remark = Trim(txtRemark.Text)
            .FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
            .FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text
            'Added by Shweta on 10-FEB-12
            If Session("isvaluezero") = "True" Then
                .IsValZero = True
            Else
                .IsValZero = False
            End If

        End With
        Session("mLog") = mLog
    End Sub

    Private Sub DisableButtons()
        Try

            hrefTimeline.Attributes("style") = "pointer-events: none"
            iTimeline.Attributes.Remove("style")

            hrefFlights.Attributes("style") = "pointer-events: none"
            iFlights.Attributes.Remove("style")

            hrefAvailability.Attributes("style") = "pointer-events: none"
            iAvailability.Attributes.Remove("style")

            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")

        Catch ex As Exception

        End Try

    End Sub
    Private Sub DataFieldBind()
        txtLogNo.Text = mLog.LogNo
        txtLogText.Text = mLog.LogText

        If Not mLog.Date Is System.DBNull.Value Then
            calDateTime.Text = Format(CDate(mLog.Date), AppSettings("DateFormat"))
        Else
            calDateTime.Text = ""
        End If

        '''''''''calDeparture.Value = mLog.SouLocalDateTime  DateFormat
        If Not mLog.SouLocalDateTime Is System.DBNull.Value Then
            calDeparture.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("DateFormat"))
            txtDepartureTime.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("TimeFormat"))
        Else
            calDeparture.Text = ""
            'calDepartureTime.Text = ""
        End If

        'calArrival.Text = mLog.DesLocalDateTime
        If Not mLog.DesLocalDateTime Is System.DBNull.Value Then
            calArrival.Text = Format(CDate(mLog.DesLocalDateTime), AppSettings("DateFormat"))
            txtArrivalTime.Text = Format(CDate(mLog.DesLocalDateTime), AppSettings("TimeFormat"))
        Else
            If Not mLog.SouLocalDateTime Is System.DBNull.Value Then
                calArrival.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("DateFormat"))
                txtArrivalTime.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("TimeFormat"))
            Else
                calArrival.Text = ""
            End If
        End If
        ''''''''''''''''CalUTCDateTime.Value = mLog.SouUniverseDateTime
        If Not mLog.SouUniverseDateTime Is System.DBNull.Value Then
            CalUTCDateTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("DateFormat"))
            txtUTCDepartureTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("TimeFormat"))
        Else
            CalUTCDateTime.Text = ""
        End If

        ''''''''''''''''''''CalUTCArrival.Value = mLog.DesUniverseDateTime
        If Not mLog.DesUniverseDateTime Is System.DBNull.Value Then
            CalUTCArrival.Text = Format(CDate(mLog.DesUniverseDateTime), AppSettings("DateFormat"))
            txtUTCArrivalTime.Text = Format(CDate(mLog.DesUniverseDateTime), AppSettings("TimeFormat"))
        Else
            If Not mLog.SouUniverseDateTime Is System.DBNull.Value Then
                CalUTCArrival.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("DateFormat"))
                txtUTCArrivalTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("TimeFormat"))
            Else
                CalUTCArrival.Text = "" 'Change by Vikrant on 20-Oct-2015 for Religare
            End If
        End If

        'Added By Utkarsh On 30-Aug-2011

        If takeofftouchdown Then
            If Not mLog.TakeOffLocalDateTime Is System.DBNull.Value Then
                calTakeOffLocalDateTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("DateFormat"))
                txtTakeOffLocalTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("TimeFormat"))
            Else
                calTakeOffLocalDateTime.Text = ""
            End If


            If Not mLog.TakeOffUniverseDateTime Is System.DBNull.Value Then
                calUTCTakeOffDateTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("DateFormat"))
                txtUTCTakeOffTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("TimeFormat"))
            Else
                calUTCTakeOffDateTime.Text = ""
            End If

            If Not mLog.TouchDownLocalDateTime Is System.DBNull.Value Then
                calTouchDownLocalDateTime.Text = Format(CDate(mLog.TouchDownLocalDateTime), AppSettings("DateFormat"))
                txtTouchDownLocalTime.Text = Format(CDate(mLog.TouchDownLocalDateTime), AppSettings("TimeFormat"))
            Else
                If Not mLog.TakeOffLocalDateTime Is System.DBNull.Value Then
                    calTouchDownLocalDateTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("DateFormat"))
                    txtTouchDownLocalTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("TimeFormat"))
                Else
                    calTouchDownLocalDateTime.Text = ""
                End If
            End If

            If Not mLog.TouchDownUniverseDateTime Is System.DBNull.Value Then
                calUTCTouchDownDateTime.Text = Format(CDate(mLog.TouchDownUniverseDateTime), AppSettings("DateFormat"))
                txtUTCTouchDownTime.Text = Format(CDate(mLog.TouchDownUniverseDateTime), AppSettings("TimeFormat"))
            Else
                If Not mLog.TakeOffUniverseDateTime Is System.DBNull.Value Then
                    calUTCTouchDownDateTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("DateFormat"))
                    txtUTCTouchDownTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("TimeFormat"))
                Else
                    calUTCTouchDownDateTime.Text = "" 'Change by Vikrant on 20-Oct-2015 for Religare
                End If
            End If

        End If

        'If takeofftouchdown Then
        '    txtBlockTime.Text = mLog.DiffTime
        '    txtGroundRunTime.Text = mLog.TimeOnGround
        'Else
        '    txtBlockTime.Text = mLog.DiffTime
        'End If
        DataBind()
        mSearchListPilot = SearchList.GetSearchList("Pilot", "", "")
        Session("mSearchListPilot") = mSearchListPilot
        mSearchListPlace = SearchList.GetSearchList("Place", "", "")
        Session("mSearchListPlace") = mSearchListPlace
        Pilot1.Text = mLog.Pilot1Name
        Pilot2.Text = mLog.Pilot2Name

    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        ' GridColumnHeadingSet()
        Dim tempString As String 'Added By Utkarsh On 24-Nov-2011 For ALL23112011

        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 500 char"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "calDeparture" Then
            'CNDC
            'If Not IsDate(calDeparture.Text) Then
            ''''''''''''''If Not (calDeparture.IsDateValue) Then
            If Not IsDate(calDeparture.Text) Then
                custValidator.ErrorMessage = "Departure date should be in valid date time format."
                e.IsValid = False
            Else
                Dim Date1, Time1 As String
                '''''''''Date1 = calDeparture.Value.ToString
                '''''''''Time1 = calDeparture.Value.ToString
                Date1 = calDeparture.Text.ToString
                Time1 = calDeparture.Text.ToString
                If Date1 = "1/1/0001" Then
                    custValidator.ErrorMessage = "Departure date should be in valid date time format."
                    e.IsValid = False

                    Exit Sub
                End If
                'CNDC
                'calDeparture.Text = Date1 + " " + Time1
                ''''''''''calDeparture.Value = Date1
                calDeparture.Text = Date1
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "calArrival" Then
            'CNDC
            'If Not IsDate(calArrival.Text) Then
            If Not IsDate(calArrival.Text) Then
                custValidator.ErrorMessage = "Arrival date should be in valid date time format."
                e.IsValid = False
            Else
                Dim Date1, Time1 As String
                'CNDC
                Date1 = calArrival.Text.ToString
                Time1 = calArrival.Text.ToString
                'Date1 = CDate(calArrival.Text).ToShortDateString()
                'Time1 = CDate(calArrival.Text).ToShortTimeString()
                If Date1 = "1/1/0001" Then
                    custValidator.ErrorMessage = "Arrival date should be in valid date time format."
                    e.IsValid = False

                    Exit Sub
                End If
                'CNDC
                calArrival.Text = Date1
                'calArrival.Text = Date1 + " " + Time1
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "Pilot1" Then

            If Not mSearchListPilot.Contains(Pilot1.Text.Trim) Then
                custValidator.ErrorMessage = "Enter correct Pilot1 name."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "Place1" Then
            'Added by Utkarsh On 24-Nov-2011 For ALL23112011
            tempString = Place1.Text.Trim
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
            If Not mSearchListPilot.Contains(Pilot2.Text.Trim) Then
                custValidator.ErrorMessage = "Enter correct Pilot2 name."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "Place2" Then
            'Added by Utkarsh On 24-Nov-2011 For ALL23112011
            tempString = Place2.Text.Trim
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
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'Code Added By Deven 21-03-2008 
        'BindClassification()
        '-------------------------------
        upnlBlockAirborneTime.DataBind()
        upnlBlockAirborneTime.Update()

        upnlGroundWithTotalTime.DataBind()
        upnlGroundWithTotalTime.Update()

        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)  'True added by Saylee 25-July-2012
        SetAPUGridObject(True)     'True added by Saylee 25-July-2012
        SetCGBGridObject(True)     'True added by Saylee 25-July-2012
        'GridColumnHeadingSet()
        Dim str As String = ""
        'Log
        If Not mLog.IsValid Then
            For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
                str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        'AirFrame
        For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1
            If Not mLog.LogAFAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'Engine
        For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
            If Not mLog.LogEngAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'APU
        For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
            If Not mLog.LogAPUAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False

        End If
        Flag = 1

    End Sub
    Public Function CustomValidate2() As Boolean    'For DgLog Fuel Oils
        Dim str As String = ""
        'AirFrame
        For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1
            If Not mLog.LogAFAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'Engine
        For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
            If Not mLog.LogEngAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        'APU
        For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
            If Not mLog.LogAPUAssemblies(i).IsValid Then
                Dim x As Integer
                For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next

        If Not mLog.PilotID1.Equals(mLog.PrevPilotID2) And Not mLog.PilotID2.Equals(mLog.PrevPilotID1) Then
            If Not mLog.PilotID1.Equals(mLog.PrevPilotID1) Then
                If mLog.LogCrews.Contains(mLog.PilotID1) Then
                    str = str + "Please Select Different Pilot as '" + "<b>" + mLog.Pilot1Name + "</b>" + "' is already entered in Flight Crew."
                    Pilot1.Text = mSearchListPilot(mLog.PrevPilotID1).Name
                End If
            End If
            If Not mLog.PilotID2.Equals(mLog.PrevPilotID2) Then
                If mLog.LogCrews.Contains(mLog.PilotID2) Then
                    str = str + "Please Select Different Co-Pilot as '" + "<b>" + mLog.Pilot2Name + "</b>" + "' is already entered in Flight Crew."
                    Pilot2.Text = mSearchListPilot(mLog.PrevPilotID2).Name
                End If
            End If
        End If
        'End

        If str <> "" Then
            cvRemark.ErrorMessage = str
            cvRemark.IsValid = False

            Return False
        End If

        Return True
    End Function
    Private Sub SetTitle()
        Dim Index As Integer
        Index = Session("Index")
        If mLog.IsNew Then
            If mLog.Date Is DBNull.Value Then
                lblTitle.InnerText = "Log Details of " & mMachine.RegNo & " as of - [New]"
            Else
                lblTitle.InnerText = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
            End If
        Else

            lblTitle.InnerText = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
            ''lblTitle.Text = "Status of " & mMachine.RegNo & " as of " & CStr(mLog.Date) & " [" & (mLogList(Index).LogTextNo) & "]"
        End If

        upnlTitle.Update()  ' '' ''AJAX- call "upnlTitle.Update" to show changes in title 
    End Sub
    Private Sub EnableDisableButton()


        calDateTime.Enabled = mLog.IsNew

        If Not mLog.IsNew Then
            'txtAirBorneTime.BackColor = Color.Gainsboro
            'txtGroundRunTime.BackColor = Color.Gainsboro
            'txtPercentTimeOnGround.BackColor = Color.Gainsboro
            'txtPrevHobbsValue.BackColor = Color.Gainsboro
            'txtPrevHobbsOffset.BackColor = Color.Gainsboro
            'txtCurrentHobbsOffset.BackColor = Color.Gainsboro
            'txtCurrentHobbsValue.BackColor = Color.Gainsboro
            'txtTotalTime.BackColor = Color.Gainsboro
            'txtBlockTime.BackColor = Color.Gainsboro
            'txtBlockTime.ReadOnly = True

            divAirBorneTime.Style.Add("background-color", "Gainsboro")
            divGroundRunTime.Style.Add("background-color", "Gainsboro")
            divPercentTimeOnGround.Style.Add("background-color", "Gainsboro")
            divBlockTime.Style.Add("background-color", "Gainsboro")

            divAirBorneTime.Disabled = True
            divGroundRunTime.Disabled = True
            divPercentTimeOnGround.Disabled = True

            tabFuel.Visible = True
            tabMEL.Visible = True
            tabParameter.Visible = True

        Else                                                        ' '' ''AJAX-Else case explicitly added bcaz after partial postback (Save&New) controls have to refresh.
            'txtAirBorneTime.BackColor = Color.White
            'txtGroundRunTime.BackColor = Color.White
            'txtPercentTimeOnGround.BackColor = Color.White
            'Detail Page code
            'If AppSettings("SetBlockTime") = "True" Then
            '    txtBlockTime.BackColor = Color.White
            '    txtBlockTime.ReadOnly = False
            'Else
            '    txtBlockTime.BackColor = Color.Gainsboro
            '    txtBlockTime.ReadOnly = True
            'End If
            '''''''''''''''''''''''''''
            '''
            divAirBorneTime.Style.Add("background-color", "White")
            divGroundRunTime.Style.Add("background-color", "White")
            divPercentTimeOnGround.Style.Add("background-color", "White")

            divAirBorneTime.Disabled = False
            divGroundRunTime.Disabled = False
            divPercentTimeOnGround.Disabled = False

            If AppSettings("SetBlockTime") = "True" Then
                divBlockTime.Style.Add("background-color", "White")
                divBlockTime.Disabled = False
            Else
                divBlockTime.Style.Add("background-color", "Gainsboro")
                divBlockTime.Disabled = True
            End If

            tabFuel.Visible = False
            tabMEL.Visible = False
            tabParameter.Visible = False
        End If

        'Place

        'Date 
        ''If mLogList.Count = 0 Then
        If LogListCount = 0 Then
            calDeparture.Enabled = True  ''And mMachine.HourType = 1
            calArrival.Enabled = True ''And mMachine.HourType = 1
            calDeparture.ReadOnly = Not (True) '' And mMachine.HourType = 1)
            calArrival.ReadOnly = Not (True) '' And mMachine.HourType = 1)

            txtDepartureTime.Enabled = True
            txtArrivalTime.Enabled = True
            txtDepartureTime.ReadOnly = Not (True)
            txtArrivalTime.ReadOnly = Not (True)

            ''Added By Utkarsh On 31-Aug-2011
            If takeofftouchdown Then
                calTakeOffLocalDateTime.Enabled = True
                calTouchDownLocalDateTime.Enabled = True
                calTakeOffLocalDateTime.ReadOnly = Not (True)
                calTouchDownLocalDateTime.ReadOnly = Not (True)

                txtTakeOffLocalTime.Enabled = True
                txtTouchDownLocalTime.Enabled = True
                txtTakeOffLocalTime.ReadOnly = Not (True)
                txtTouchDownLocalTime.ReadOnly = Not (True)


            End If
            'End

        End If
        If LogListCount > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") <> "9999" And mLog.IsNew = True And mLog.SouLocalDateTime.ToString <> "" Then
            calDeparture.Enabled = True  ''And mMachine.HourType = 1
            'calArrival.Enabled = False
            calArrival.Enabled = True
            calDeparture.ReadOnly = Not (True) '' And mMachine.HourType = 1)
            calArrival.ReadOnly = Not (True)

            txtDepartureTime.Enabled = True
            txtArrivalTime.Enabled = True
            txtDepartureTime.ReadOnly = Not (True)
            txtArrivalTime.ReadOnly = Not (True)
        End If


        'Added By Utkarsh On 31-Aug-2011

        If Not mLog.IsNew Then
            calDeparture.Enabled = False
            calArrival.Enabled = False
            CalUTCDateTime.Enabled = False
            CalUTCArrival.Enabled = False

            'Commented By Utkarsh On 19-Apr-2012 For ALL19042012

            'Pilot1.Enabled = False
            'Pilot2.Enabled = False

            'End

            '''Place1.Enabled = False
            '''Place2.Enabled = False
            divPlace1.Disabled = True
            divPlace2.Disabled = True

            'Commented By Utkarsh On 19-Apr-2012 For ALL19042012

            'Pilot1.BackColor = Color.Gainsboro
            'Pilot2.BackColor = Color.Gainsboro

            'End
            'Place1.BackColor = Color.Gainsboro
            'Place2.BackColor = Color.Gainsboro
            divPlace1.Style.Add("background-color", "Gainsboro")
            divPlace2.Style.Add("background-color", "Gainsboro")

            If takeofftouchdown Then

                calTakeOffLocalDateTime.Enabled = False
                calUTCTakeOffDateTime.Enabled = False
                calTouchDownLocalDateTime.Enabled = False
                calUTCTouchDownDateTime.Enabled = False
            End If
            txtDepartureTime.Enabled = False
            txtArrivalTime.Enabled = False
            txtUTCDepartureTime.Enabled = False
            txtUTCArrivalTime.Enabled = False
            chkArrival.Disabled = True
            chkTouchDown.Disabled = True
            chkTakeOff.Disabled = True
        Else                                                    ' '' ''AJAX-Else case explicitly added bcaz after partial postback (Save&New) controls have to refresh.
            divPlace1.Disabled = False
            divPlace2.Disabled = False
            'Place1.ReadOnly = False
            'Place2.ReadOnly = False


            chkArrival.Disabled = False
            chkTouchDown.Disabled = False
            chkTakeOff.Disabled = False
        End If

        'End
        'Hobbs-taken
        'btnHobbsOffset.Enabled = (mMachine.HourType = 2)
        'pnlHours.Visible = True '= Not (mMachine.HourType = 2) 'Added Code
        'pnlDecimal.Visible = (mMachine.HourType = 2)
        'plDecimal.Visible = (mMachine.HourType = 2)
        ''================Visibility for Hours and Decimal===================
        ''*pnlHours   


        'lblAirBorneTime.Visible = True
        'txtAirBorneTime.Visible = True
        'txtBlockTime.Visible = True
        'lblGroundRunTime.Visible = True
        'txtGroundRunTime.Visible = True
        'lblPercentTimeOnGround.Visible = True
        'txtPercentTimeOnGround.Visible = True

        ''pnlDecimal
        'lblHobbsPrevVal.Visible = (mMachine.HourType = 2)
        'txtPrevHobbsValue.Visible = (mMachine.HourType = 2)
        'lblOffsetPreVal.Visible = (mMachine.HourType = 2)
        'txtPrevHobbsOffset.Visible = (mMachine.HourType = 2)
        'lblOffsetCurrentVal.Visible = (mMachine.HourType = 2)
        'txtCurrentHobbsOffset.Visible = (mMachine.HourType = 2)
        'lblHobbsCurrentReading.Visible = (mMachine.HourType = 2)
        'txtCurrentHobbsValue.Visible = (mMachine.HourType = 2)

        '===========ReadOnly for Hours and Decimal=============
        'lblairfly.Visible = (mMachine.HourType = 1)
        'txtBlockTime.Visible = (mMachine.HourType = 1)
        'lblAirBorneTime.Visible = (mMachine.HourType = 1)
        'txtAirBorneTime.Visible = (mMachine.HourType = 1)
        'lblGroundRunTime.Visible = (mMachine.HourType = 1)
        'txtGroundRunTime.Visible = (mMachine.HourType = 1)
        'lblPercentTimeOnGround.Visible = (mMachine.HourType = 1)
        'txtPercentTimeOnGround.Visible = (mMachine.HourType = 1)


        'Added By Utkarsh On 31-Aug-2011

        If takeofftouchdown And mLog.IsLogAirborneEntry = False Then  'Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = False
            'txtAirBorneTime.BackColor = Color.Gainsboro
            'txtGroundRunTime.BackColor = Color.Gainsboro
            'txtAirBorneTime.ReadOnly = True
            'txtGroundRunTime.ReadOnly = True

            divAirBorneTime.Style.Add("background-color", "Gainsboro")
            divGroundRunTime.Style.Add("background-color", "Gainsboro")
            divAirBorneTime.Disabled = True
            divGroundRunTime.Disabled = True
        End If



        lblTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        lblUTCTakeOffDateTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)
        lblTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        lblUTCTouchDownDateTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)

        lblTakeOffTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        lblUTCTakeOffTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)
        lblTouchDownOffTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        lblUTCTouchDownOffTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)



        calTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        calUTCTouchDownDateTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)
        calTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        calUTCTakeOffDateTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)


        txtTakeOffLocalTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        txtUTCTakeOffTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)
        txtTouchDownLocalTime.Visible = (Not (mMachine.IsUTC) And takeofftouchdown)
        txtUTCTouchDownTime.Visible = ((mMachine.IsUTC) And takeofftouchdown)


        chkTakeOff.Visible = takeofftouchdown
        chkTouchDown.Visible = takeofftouchdown


        'End

        'pnlHours

        'Added By Utkarsh On 05-Sep-2011
        If Not takeofftouchdown Then
            '' txtAirBorneTime.ReadOnly = Not mLog.IsNew
            divAirBorneTime.Disabled = Not mLog.IsNew
        End If
        'End
        ' txtCurrentHobbsValue.ReadOnly = Not mLog.IsNew



        calDeparture.Visible = Not (mMachine.IsUTC)
        '

        txtDepartureTime.Visible = Not (mMachine.IsUTC)
        txtArrivalTime.Visible = Not (mMachine.IsUTC)

        calArrival.Visible = Not (mMachine.IsUTC)
        '
        CalUTCDateTime.Visible = (mMachine.IsUTC)
        '
        CalUTCArrival.Visible = (mMachine.IsUTC)
        '
        txtUTCDepartureTime.Visible = (mMachine.IsUTC)
        txtUTCArrivalTime.Visible = (mMachine.IsUTC)

        lblDepartureDate.Visible = Not (mMachine.IsUTC)
        lblDepartureTime.Visible = Not (mMachine.IsUTC)
        lblArrivalDate.Visible = Not (mMachine.IsUTC)
        lblArrivalTime.Visible = Not (mMachine.IsUTC)

        lblUTCDepartureDate.Visible = (mMachine.IsUTC)
        lblUTCDepartureTime.Visible = (mMachine.IsUTC)
        lblUTCArrivalDate.Visible = (mMachine.IsUTC)
        lblUTCArrivalTime.Visible = (mMachine.IsUTC)

        If mMachine.IsUTC Then
            If CalUTCDateTime.Enabled = False Then divDepartureDate.Style.Add("background-color", "Gainsboro")
            If CalUTCArrival.Enabled = False And chkArrival.Checked = False Then divArrivalDate.Style.Add("background-color", "Gainsboro")

            If calUTCTakeOffDateTime.Enabled = False And chkTakeOff.Checked = False Then divTakeOffDate.Style.Add("background-color", "Gainsboro")
            If calUTCTouchDownDateTime.Enabled = False And chkTouchDown.Checked = False Then divTouchDownDate.Style.Add("background-color", "Gainsboro")

        End If

    End Sub
    Private Sub BindClassification()
        mLog = CType(Session("mLog"), Log)

        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(SELECT)")
        cmbFlightLogClassification.DataSource = mFlightLogClassificationList
        cmbFlightLogClassification.DataBind()

        Session("mFlightLogClassificationList") = mFlightLogClassificationList

        If cmbFlightLogClassification.Items.Contains(New System.Web.UI.WebControls.ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
            cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
        Else
            cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
        End If

    End Sub
    Private Sub SetTakeoffTouchdownTitle()

        If takeofftouchdown Then
            lblDepartureDate.InnerText = "Chocks Off Date"
            lblUTCDepartureDate.InnerText = "UTC Chocks Off Date"
            lblArrivalDate.InnerText = "Chocks On Date"
            lblUTCArrivalDate.InnerText = "UTC Chocks On Date"

            If mMachine.IsUTC Then
                lblDepHeader.InnerText = "Departure (UTC)"
                lblArrHeader.InnerText = "Arrival (UTC)"
            End If


        End If

    End Sub
    Private Sub NewRecord(ByVal LogDate As String, Optional ByVal mSouLocalDateTime As String = "", Optional ByVal mSouUTCDateTime As String = "")
        mLog = Log.NewLog(mMachine, LogDate, mSouLocalDateTime, mSouUTCDateTime)
        ' mLog.BeginEdit()
        mMachine = Machine.GetMachine(mMachine.ID)
        DataBind()
        '''''CHECK_isRequiredAssembliesInstalled()
    End Sub
    Private Sub DataBindGrid()
        'If Not mLog Is Nothing Then
        '    'SetObject()

        '    SetAirFrameGridObject(True)
        '    SetEngineGridObject(True)
        '    SetAPUGridObject(True)
        '    SetCGBGridObject(True)

        '    dgAFPeriods.DataSource = mLog.LogAFAssemblies
        '    dgAFPeriods.DataBind()

        '    dgEnginePeriods.DataSource = mLog.LogEngAssemblies
        '    dgEnginePeriods.DataBind()

        '    dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
        '    dgAPUPeriods.DataBind()

        '    dgCGBPeriods.DataSource = mLog.LogCGBAssemblies
        '    dgCGBPeriods.DataBind()

        '    grdAllAssemblies.DataSource = mLog.ALL_LogAssemblies
        '    dgCGBPeriods.DataBind()


        '    GridColumnHeadingSet()

        '    ' '' ''AJAX- In DataFieldBind we binds object values to various controls. To reflect values we have call ".Update()" method of respective Panel
        '    upnlAirframeDetail.Update()
        '    upnlEngineDetail.Update()
        '    upnlAPUDetail.Update()
        '    upnlCGBDetail.Update()

        '    Session("mLog") = mLog
        'End If
    End Sub
    Private Sub CopyFromClone(ByVal ClonedLog As Log, Optional ByVal isFromLogDate As Boolean = False)
        mLog.PilotID1 = ClonedLog.PilotID1
        mLog.PilotID2 = ClonedLog.PilotID2
        mLog.Pilot1Name = ClonedLog.Pilot1Name
        mLog.Pilot2Name = ClonedLog.Pilot2Name

        mLog.IsUTC = ClonedLog.IsUTC
        mLog.SourceID = ClonedLog.SourceID


        If Not mLog.IsNew Then
            mLog.SouLocalDateTime = ClonedLog.SouLocalDateTime
            mLog.SouDayLightTime = ClonedLog.SouDayLightTime
            mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
            mLog.DesDayLightTime = ClonedLog.DesDayLightTime
        End If


        mLog.DestinationID = ClonedLog.DestinationID
        If mLog.IsUTC Then
            If Not isFromLogDate Then
                mLog.SouUniverseDateTime = ClonedLog.SouUniverseDateTime
            End If
        End If
        'Commeneted By Vikrant On 20-Oct-2015
        'If isFromLogDate Then
        '    mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
        '    mLog.DesDayLightTime = ClonedLog.DesDayLightTime
        '    If takeofftouchdown Then
        '        mLog.TouchDownLocalDateTime = mLog.DesLocalDateTime
        '    End If
        'End If
        'End
        'Added By Utkarsh On 05-Sep-2011
        'Commeneted by vikrant on 28-Oct-2015
        'If Not takeofftouchdown Then
        '    mLog.TimeOnGround = ClonedLog.TimeOnGround
        '    mLog.PercentTimeOnGround = ClonedLog.PercentTimeOnGround
        '    mLog.TimeInAir = ClonedLog.TimeInAir
        'End If
        'End

        mLog.Remark = ClonedLog.Remark

        mLog.LogPageNo = ClonedLog.LogPageNo
        mLog.FlightNo = ClonedLog.FlightNo
        mLog.FlightLogClassificationID = ClonedLog.FlightLogClassificationID
        mLog.FlightLogClassificationName = ClonedLog.FlightLogClassificationName

        'Hobbs - taken
        mLog.CurrentHobbsValue = ClonedLog.CurrentHobbsValue
        mLog.OffSet = ClonedLog.OffSet


        'Added by Yogita Ajax
        mLog.ImageFile = ClonedLog.ImageFile
        mLog.ImageSize = ClonedLog.ImageSize
        mLog.FileExtension = ClonedLog.FileExtension
        '--------------------------------------

        Session("mLog") = mLog
    End Sub
    Private Function IsValidTime(ByVal TimeValue As String) As Boolean
        Dim TimeRegulerExpression As String = ""
        If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
            'TimeRegulerExpression = "^(([01][\d]+)|(2[0-3]))\:[0-5][0-9]( )*(AM|am|PM|pm)$"   '12 Hour Format
            TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
        Else
            TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
        End If

        If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function CheckZeroDifferenceValue() As Boolean
        If mLog.IsHobbs Then
            If Val(mLog.TotalTime) <> 0 Then
                Return False
            End If
            If Val(mLog.TimeInAir) <> 0 Then
                Return False
            End If
        Else
            If mLog.TimeInAir = "0:00" OrElse mLog.TimeInAir = "" Then
            Else
                Return False
            End If
            If mLog.TotalTime = "0:00" OrElse mLog.TotalTime = "" Then
            Else
                Return False
            End If
            If mLog.BlockTime = "0:00" OrElse mLog.BlockTime = "" Then
            Else
                Return False
            End If
            If mLog.TimeOnGround = "0:00" OrElse mLog.TimeOnGround = "" Then
            Else
                Return False
            End If
        End If
        If Val(mLog.TotalLandings) <> 0 Then
            Return False
        End If

        Dim checkcol = mLog.LogAFAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        checkcol = mLog.LogAPUAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        checkcol = mLog.LogEngAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        checkcol = mLog.LogCGBAssemblies
        If Not callZeroDifferenceValue(checkcol) Then
            Return False
        End If
        Return True
    End Function

    Private Function callZeroDifferenceValue(ByVal obj) As Boolean
        For i As Integer = 0 To obj.Count - 1
            If mLog.IsHobbs Then
                If Val(obj(i).Hours) <> 0 Then
                    Return False
                End If
            Else
                If obj(i).Hours <> "0:00" Then
                    Return False
                End If
            End If
            If obj(i).ShowLandings Then
                If Val(obj(i).Landings) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowCycles Then
                If Val(obj(i).Cycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowStarts Then
                If Val(obj(i).Starts) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowNGCycles Then
                If Val(obj(i).NGCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowNFCycles Then
                If Val(obj(i).NFCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowRINS Then
                If Val(obj(i).RINS) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowBleeds Then
                If Val(obj(i).Bleeds) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowImpellerCycles Then
                If Val(obj(i).ImpellerCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowCTCycles Then
                If Val(obj(i).CTCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowPTCycles Then
                If Val(obj(i).PTCycles) <> 0 Then
                    Return False
                End If
            End If
            If obj(i).ShowGeneratorMods Then
                If Val(obj(i).GeneratorMods) <> 0 Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function

    Public Function AvgFlightTimeDeviation() As Boolean
        If AppSettings("NoOfLogsToConsiderForAvgFlightTime") <> "0" And AppSettings("DeviationInAvgFlightTimeInPercentage") <> "0" Then
            Dim mLastLogDetails As LastLogDetails = LastLogDetails.GetLastLogDetails(False, mLog.DateFormatted.ToString, CType(AppSettings("NoOfLogsToConsiderForAvgFlightTime"), Integer), mLog.SourceID.ToString, mLog.DestinationID.ToString, mMachine.AssemblyStatus.Assembly.ModelID.ToString)
            If mLastLogDetails.Count > 0 Then
                Dim CurrentLogTimeInAirInDec As Decimal = New Period(1, mLog.TimeInAir, 0, False, False).DbValueDec
                Dim AllowedDeviationInDec = (mLastLogDetails.AvgFlightTime * CType(AppSettings("DeviationInAvgFlightTimeInPercentage"), Integer) / 100)
                Dim ActualDeviationInDec As Decimal = Math.Abs(CurrentLogTimeInAirInDec - mLastLogDetails.AvgFlightTime)
                If ActualDeviationInDec > AllowedDeviationInDec Then
                    If CurrentLogTimeInAirInDec > mLastLogDetails.AvgFlightTime Then
                        Session("IsFlightTimeGreaterThanAvgFlightTime") = "True"
                    End If
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Private Sub SetPBHValues(ByVal TmpLog As Log, ByVal IsLogNew As Boolean)
        Try
            If mCompanyDetail.IsCombinedHours = False Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
                If IsLogNew Then

                    Dim mPBH As PBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")
                    If Not mPBH.MachineID.Equals(Guid.Empty) Then
                        If CDate(Today.Date) >= CDate(mPBH.StartDate) Then
                            mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec
                            If IsLogNew Then
                                mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
                            Else
                                mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec + New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec), 1, False, False).Value
                            End If

                            mPBH.RemainingHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.ElapsedHoursDec, 1, False, False).Value
                            mPBH.LastLogDetails = TmpLog.DateFormatted
                            'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
                            'Also mark Not InUse in tabMachine at same time 
                            If mPBH.RemainingHoursDec <= 0 Then
                                mPBH.IsNotActive = True
                                mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
                                mPBH.MachineNotInUse = True
                                Session("IsAircraftMadeNotInUse") = "True"
                            End If
                            mPBH.Save()
                        End If
                    End If
                End If
            ElseIf mCompanyDetail.IsCombinedHours = True Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
                Dim mPBH As PBH
                If IsLogNew Then
                    Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
                    mPBH = PBH.GetPBH(mPBHList(0).ID)
                    If CDate(Today.Date) >= CDate(mPBH.StartDate) Then
                        mPBH.RemainingHours = New Period(1, mPBH.RemainingHoursDec - New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec, 1, False, False).Value
                        If mPBH.CarryForwardHoursDec < 0 Then
                            mPBH.ElapsedHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.RemainingHoursDec, 1, False, False).Value
                        Else
                            mPBH.ElapsedHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.RemainingHoursDec, 1, False, False).Value
                        End If


                        mPBH.LastLogDetails = TmpLog.DateFormatted
                        If mPBH.RemainingHoursDec <= 0 Then
                            mPBH.IsNotActive = True
                            mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
                            mPBH.MachineNotInUse = True
                            Session("IsAircraftMadeNotInUse") = "True"
                        End If
                        mPBH.Save()
                    End If

                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Function Save() As Boolean
        'Authentication
        If Not mLog.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                'Dim strOutString As String = ReadXMLFile()
                'strOutString = strOutString.Split(CChar("$"))(1)
                'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                'CNDC
                If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < -10 _
                    Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < -10) _
                    Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < -10) Then

                    ' MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg(" Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" & maxAllowableDate.ToString(WebDateFormat), "Save alert..!!")
                    DataFieldBind()
                    Exit Function
                End If
            End If
        End If


        If mLog.IsLogAirborneEntry Then 'Added by Saylee on 1-Sep-2021 for ALL01092021 :
            mLog = Session("mLog")
        End If
        'Authentication
        Dim LogClone As Log
        LogClone = CType(mLog.Clone, Log)
        'Code Added By Deven 21-03-2008 
        'BindClassification()
        '-------------------------------

        SetObject()

        'SetAirFrameGridObject()  '''Commented by Saylee on 1-Sep-2021 for ALL01092021 :
        If mLog.IsLogAirborneEntry Then 'Added by Saylee on 1-Sep-2021 for ALL01092021 
            SetAirFrameGridObject(True)
        Else
            SetAirFrameGridObject()
        End If


        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)


        If mLog.IsValid = True Then
            Try
                If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013
                    ' If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
                    'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
                    If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
                    Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
                        'MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg("Assembly Required for Selected Aircraft.Required Assembly of the Aircraft is Not Installed on this Date of Log.", "Restriction!")
                        Return False
                        Exit Function
                    End If
                End If

                'If IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
                '    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "There is some information missing / not entered correctly.</br> </br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
                '    Exit Function
                'End If


                If AvgFlightTimeDeviation() = True And Not (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then
                    'Decision
                    ' MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterAvgFlightTimeDeviationWarning")
                    ShowAlertMsg("Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", "Save Alert..!!", True, AgreeString:="SaveLogAfterAvgFlightTimeDeviationWarning")
                    Session.Remove("IsFlightTimeGreaterThanAvgFlightTime")
                    Exit Function
                End If

                mLog.ApplyEdit()

                'Pilot 1
                'IF Pilot 1 deleted...
                If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID1
                        LogCrew.DutyAsID = 1
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
                        End If
                    End If
                End If

                'Pilot 2
                'IF Pilot 2 deleted...
                If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID2
                        LogCrew.DutyAsID = 2
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
                        End If
                    End If
                End If
                'End If
                'End 

                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim IsNewLog As Boolean
                IsNewLog = mLog.IsNew
                'End

                mLog = CType(mLog.Save(), Log)

                Dim mMaxLogOfAircraft As MaxLogOfAircraft
                mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

                If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                    If Not (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then
                        If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    Else
                        If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    End If
                End If
                'End
                Dim IsShowDateCntrl As String
                If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
                    IsShowDateCntrl = "False"
                    Session("IsSaveAndNew") = 0
                Else
                    IsShowDateCntrl = "True"
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)

                '-----------------------------------------------------------------------
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                MarkLog(Util.Action.Save, "Flight Log", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
                '-----------------------------------------------------------------------
                Session("mLog") = mLog

                Return True
            Catch ex As SqlException
                Session("LogClone") = LogClone
                If ex.Number = 8114 Or ex.Number = 8115 Then

                    'MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg(" Rate or Qty or Conversion Factor. ", "Numeric Overflow!")
                ElseIf ex.Number = 8145 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("<strong> Access Denied.User not authorized</strong> <p>" & " Please contact the Administrator.</p>", "Not Authorized !")
                ElseIf ex.Number = 2627 Then
                    If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then
                        'MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> Please enter the unique Log Page No. </strong> ", MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg("<strong> Please enter the unique Log Page No. </strong> ", "Save Alert ! ")
                    Else
                        'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg(ex.Procedure, "Database Error !")
                    End If
                ElseIf ex.Number = 547 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("This record cannot be deleted. It is used in other transaction(s) </p>", "Reference !")
                ElseIf ex.Number = 50000 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("Log already entered between current Date and Time span for this Aircraft.", "Alert..!!")
                End If
                'Added by utkash on 1-oct-2013 for log_ajax changes
                mLog = LogClone
                Session("mLog") = mLog
                'end
                Return False
            Finally
                LogClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Function SaveLogFlexiLog() As Boolean 'Added by Saylee on 21-May-2012 ALL17052012
        'Authentication
        If Not mLog.Date Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                'Dim strOutString As String = ReadXMLFile()
                'strOutString = strOutString.Split(CChar("$"))(1)
                'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

                'Changes by Kalpesh in 13-3-2013
                'These lines commented
                '
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")

                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                '---------------------------------

                'CNDC
                If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 _
                    Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) _
                    Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then

                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg(" Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" & maxAllowableDate.ToString(WebDateFormat), "Save alert..!!")

                    DataFieldBind()
                    Exit Function
                End If
            End If
        End If
        'Authentication
        Dim LogClone As Log
        LogClone = CType(mLog.Clone, Log)
        'Code Added By Deven 21-03-2008 
        'BindClassification()
        '-------------------------------

        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)

        If mLog.IsValid = True Then
            Try
                If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
                    'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
                    'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
                    If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
                    Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then

                        'MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg("Assembly Required for Selected Aircraft.Required Assembly of the Aircraft is Not Installed on this Date of Log.", "Restriction!")
                        Return False
                        Exit Function
                    End If
                End If

                'Added By Prashant 12-Apr-2010
                Dim IsMELCount As Boolean = False
                Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
                mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
                For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
                    'If (mTempMELSnagCorrectiveActionList(i).DueDate > calDateTime.Value) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = True) Then
                    If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
                        If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
                            IsMELCount = True
                            Exit For
                        Else
                            IsMELCount = False
                        End If
                    End If
                Next
                mTempMELSnagCorrectiveActionList = Nothing
                '-----------------------------------
                If IsMELCount = True Then
                    'MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, IIf(Session("New") = "True", "MELNew", "MEL"))
                    ShowAlertMsg("Installed Components does not fulfill Minimum Equipment Level to Fly", "Minimum Equipment Level", True, "MEL")
                    Exit Function
                    'ElseIf IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then  'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours


                    '    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
                    '    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "There is some information missing / not entered correctly.<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")

                    '    Exit Function
                ElseIf AvgFlightTimeDeviation() = True And Not (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then 'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterAvgFlightTimeDeviationWarning")
                    ShowAlertMsg("Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", "Save Alert..!!", True, AgreeString:="SaveLogAfterAvgFlightTimeDeviationWarning")
                    Session.Remove("IsFlightTimeGreaterThanAvgFlightTime")
                    Exit Function
                End If
                'End

                mLog.ApplyEdit()
                'Add Pilot and Co-pilot in Log Crew as Child...

                'IF Pilot 1 deleted...
                If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID1
                        LogCrew.DutyAsID = 1
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
                        End If
                    End If
                End If

                'Pilot 2
                'IF Pilot 2 deleted...
                If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID2
                        LogCrew.DutyAsID = 2
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
                        End If
                    End If
                End If

                'End If

                'End 

                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim IsNewLog As Boolean
                IsNewLog = mLog.IsNew
                'End

                mLog = CType(mLog.Save(), Log)

                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim mMaxLogOfAircraft As MaxLogOfAircraft
                mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

                If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                    If Not (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then
                        If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    Else
                        If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    End If
                End If
                'End
                Dim IsShowDateCntrl As String
                If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
                    IsShowDateCntrl = "False"
                    Session("IsSaveAndNew") = 0
                Else
                    IsShowDateCntrl = "True"
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)
                '-----------------------------------------------------------------------
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                MarkLog(Util.Action.Save, "Flight Log", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
                '-----------------------------------------------------------------------
                Session("mLog") = mLog

                Return True
            Catch ex As SqlException
                Session("LogClone") = LogClone
                If ex.Number = 8114 Or ex.Number = 8115 Then

                    'MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg(" Rate or Qty or Conversion Factor. ", "Numeric Overflow!")
                ElseIf ex.Number = 8145 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("<strong> Access Denied.User not authorized</strong> <p>" & " Please contact the Administrator.</p>", "Not Authorized !")
                ElseIf ex.Number = 2627 Then
                    If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then
                        'MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> Please enter the unique Log Page No. </strong> ", MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg("<strong> Please enter the unique Log Page No. </strong> ", "Save Alert ! ")
                    Else
                        'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg(ex.Procedure, "Database Error !")
                    End If
                ElseIf ex.Number = 547 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("This record cannot be deleted. It is used in other transaction(s) </p>", "Reference !")
                ElseIf ex.Number = 50000 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("Log already entered between current Date and Time span for this Aircraft.", "Alert..!!")
                End If
                'Added by utkash on 1-oct-2013 for log_ajax changes
                mLog = LogClone
                Session("mLog") = mLog
                'end
                Return False
            Finally
                LogClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Function SaveLogAfterAvgFlightTimeDeviationWarning() As Boolean
        Dim LogClone As Log
        LogClone = CType(mLog.Clone, Log)

        SetObject()
        SetAirFrameGridObject()
        SetEngineGridObject(True)
        SetAPUGridObject(True)
        SetCGBGridObject(True)

        If mLog.IsValid = True Then
            Try
                mLog.ApplyEdit()


                'IF Pilot 1 deleted...
                If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID1
                        LogCrew.DutyAsID = 1
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then
                    If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID1
                            LogCrew.DutyAsID = 1
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
                        End If
                    End If
                End If

                'Pilot 2
                'IF Pilot 2 deleted...
                If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                        mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                        Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                        LogCrew.CrewID = mLog.PilotID2
                        LogCrew.DutyAsID = 2
                        mLog.LogCrews.Add(LogCrew)
                    End If
                ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then
                    If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then
                        If Not mLog.LogCrews.Contains(mLog.PilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        End If
                    Else
                        If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
                            Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
                            LogCrew.CrewID = mLog.PilotID2
                            LogCrew.DutyAsID = 2
                            mLog.LogCrews.Add(LogCrew)
                        Else
                            mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
                        End If
                    End If
                End If

                'End If

                'End 
                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim IsNewLog As Boolean
                IsNewLog = mLog.IsNew
                'End
                mLog = CType(mLog.Save(), Log)

                'Added By Vikrant on 01-Dec-2021 for PBH
                Dim mMaxLogOfAircraft As MaxLogOfAircraft
                mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

                If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
                    If Not (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then
                        If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    Else
                        If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
                            SetPBHValues(mLog, IsNewLog)
                        End If
                    End If
                End If
                'End

                Dim IsShowDateCntrl As String
                If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
                    IsShowDateCntrl = "False"
                    Session("IsSaveAndNew") = 0
                Else
                    IsShowDateCntrl = "True"
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)

                '-----------------------------------------------------------------------
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
                MarkLog(Util.Action.Save, "Flight Log", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
                '-----------------------------------------------------------------------
                Session("mLog") = mLog

                Return True
            Catch ex As SqlException
                Session("LogClone") = LogClone
                If ex.Number = 8114 Or ex.Number = 8115 Then

                    'MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg(" Rate or Qty or Conversion Factor. ", "Numeric Overflow!")
                ElseIf ex.Number = 8145 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("<strong> Access Denied.User not authorized</strong> <p>" & " Please contact the Administrator.</p>", "Not Authorized !")
                ElseIf ex.Number = 2627 Then
                    If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then
                        'MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> Please enter the unique Log Page No. </strong> ", MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg("<strong> Please enter the unique Log Page No. </strong> ", "Save Alert ! ")
                    Else
                        'MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ShowAlertMsg(ex.Procedure, "Database Error !")
                    End If
                ElseIf ex.Number = 547 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("This record cannot be deleted. It is used in other transaction(s) </p>", "Reference !")
                ElseIf ex.Number = 50000 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg("Log already entered between current Date and Time span for this Aircraft.", "Alert..!!")
                End If
                'Added by utkash on 1-oct-2013 for log_ajax changes
                mLog = LogClone
                Session("mLog") = mLog
                Return False
            Finally
                'Added by utkash on 1-oct-2013 for log_ajax changes
                mLog = LogClone
                Session("mLog") = mLog
                'end
                LogClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Public Sub SendPUSHNotification(ByVal tmpLog As Log)

        Dim PreviousStepStatus As Boolean = False

        'Step # 1: Get User Devices

        Dim mUserDeviceList As APP_UserDeviceList = APP_UserDeviceList.GetUserDeviceList(1) '1:Flight Log

        If mUserDeviceList.Count = 0 Then
            PreviousStepStatus = False
        Else
            PreviousStepStatus = True
        End If

        If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------


        'Step # 2: Record PUSH Notification in the table



        Dim UserIDs(50) As Guid
        UserIDs = (From c As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList
                   Select (c.UserID)).Distinct().ToArray()

        Dim Notifications(UserIDs.Count - 1) As APP_UserNotification

        For i As Integer = 0 To UserIDs.Count - 1

            If UserIDs(i).Equals(Guid.Empty) Then Exit For

            Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)


            Try
                With mAPP_UserNotification
                    .UserID = UserIDs(i)
                    .SentOn = Now

                    'Flight Log Created for VT-WED as on 24-Feb-2021
                    .Message = "Flight Log Created for:- " + tmpLog.RegNo + " as on:- " + tmpLog.DateFormatted
                    .ModuleType = 1 'Flight Log
                    .ModuleID = tmpLog.ID
                End With



                mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)

                Notifications(i) = mAPP_UserNotification


                PreviousStepStatus = True
            Catch ex As Exception
                PreviousStepStatus = False
            End Try
        Next





        'Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)


        If PreviousStepStatus = False Then Exit Sub '----------------------------------------------------------------------------------------------------



        For Each Notification As APP_UserNotification In Notifications





            Dim errorcount As Integer = 0

StartStep3:

            'Step # 3: Trigger PUSH Notification

            errorcount = errorcount + 1

            System.Net.ServicePointManager.Expect100Continue = True
            System.Net.ServicePointManager.SecurityProtocol = 3072 'System.Net.SecurityProtocolType.Tls

            Dim request = TryCast(System.Net.WebRequest.Create("https://onesignal.com/api/v1/notifications"), System.Net.HttpWebRequest)

            request.KeepAlive = True
            request.Method = "POST"
            request.ContentType = "application/json; charset=utf-8"

            request.Headers.Add("authorization", "Basic YmE0YTUwZDgtMmJkYS00MjMzLWI5NjgtZTkxZmE5MzQ0NzMw")

            Dim serializer = New JavaScriptSerializer()

            'Forming Notification Detail URL
            '
            '
            Dim index As Integer = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("wfLogSOP_Ajax.aspx")
            Dim urlNotificationDetail As String = ""
            urlNotificationDetail = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, index) + "APP/Launcher.aspx?NotificationID=" + Notification.ID.ToString + "&ModuleID=" + tmpLog.ID.ToString + "&username=" + Notification.UserName + "&EventLogSessionID=" + Guid.NewGuid.ToString + "&ModuleTypeID=1"



            Dim filterObject As Object()
            ReDim filterObject(((mUserDeviceList.Count - 1) * 2) + 1)

            Dim idx As Integer = 0
            Dim Ridx As Integer = 0
            For Each info As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList

                If Notification.UserID.Equals(info.UserID) Then


                    If idx = 0 Then
                        filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(0).DeviceID.ToString}
                        idx = idx + 1
                    Else
                        Ridx = Ridx + 1

                        filterObject(idx) = New With {Key .[operator] = "OR"}
                        idx = idx + 1

                        filterObject(idx) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(Ridx).DeviceID.ToString}
                        idx = idx + 1
                    End If

                End If

            Next

            Dim obj = New With {Key .app_id = "f877b4d2-b6e5-4595-a381-87165f6e46a0", Key .contents = New With {Key .en = Notification.Message}, Key .headings = New With {Key .en = "FlyPal"}, Key .filters = filterObject, Key .data = New With {Key .url = urlNotificationDetail.ToString}}

            '---------------------

            Dim param = serializer.Serialize(obj)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(param)

            Dim responseContent As String = Nothing

            Try

                Using writer = request.GetRequestStream()
                    writer.Write(byteArray, 0, byteArray.Length)
                End Using

                Using response As System.Net.HttpWebResponse = request.GetResponse()

                    Using reader = New System.IO.StreamReader(response.GetResponseStream())

                        responseContent = reader.ReadToEnd()

                    End Using

                End Using

            Catch ex As System.Net.WebException
                System.Diagnostics.Debug.WriteLine(ex.Message)
                System.Diagnostics.Debug.WriteLine(New System.IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd())

                If errorcount <= 3 Then GoTo StartStep3

            End Try

            System.Diagnostics.Debug.WriteLine(responseContent)
        Next

    End Sub
#End Region

#Region "Set Child Objects"

    Public Sub SetAirFrameGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)  ' For First Grid i.e AirFrame
        ' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgAFPeriods.Items" is replaced by "dgAFPeriods.Rows"
        For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1


            If isFromDataBindGrid Then If mLog.LogAFAssemblies.ShowHours Then mLog.LogAFAssemblies(i).Hours = Trim(txtAirBorneTime.Text)

            If mLog.LogAFAssemblies.ShowLandings Then mLog.LogAFAssemblies(i).Landings = Trim(txtLandings.Text)
            If mLog.LogAFAssemblies.ShowCycles Then mLog.LogAFAssemblies(i).Cycles = Trim(txtCycles.Text)
            'If mLog.LogAFAssemblies.ShowStarts Then mLog.LogAFAssemblies(i).Starts = Trim(txtAirFrameStarts.Text)
            'If mLog.LogAFAssemblies.ShowNGCycles Then mLog.LogAFAssemblies(i).NGCycles = Trim(txtAirFrameNGCycles.Text)
            'If mLog.LogAFAssemblies.ShowNFCycles Then mLog.LogAFAssemblies(i).NFCycles = Trim(txtAirFrameNFCycles.Text)
            'If mLog.LogAFAssemblies.ShowRINS Then mLog.LogAFAssemblies(i).RINS = Trim(txtAirFrameRins.Text)
            'If mLog.LogAFAssemblies.ShowBleeds Then mLog.LogAFAssemblies(i).Bleeds = Trim(txtAirFrameBleeds.Text)
            'If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.LogAFAssemblies(i).ImpellerCycles = Trim(txtAirFrameImpellerCycles.Text)
            'If mLog.LogAFAssemblies.ShowCTCycles Then mLog.LogAFAssemblies(i).CTCycles = Trim(txtAirFrameCTCycles.Text)
            'If mLog.LogAFAssemblies.ShowPTCycles Then mLog.LogAFAssemblies(i).PTCycles = Trim(txtAirFramePTCycles.Text)
            'If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.LogAFAssemblies(i).GeneratorMods = Trim(txtAirframeGeneratorMods.Text)
            If mLog.LogAFAssemblies.ShowCycles Then mLog.UpdateChildPeriods(3, "Cycles", mLog.LogAFAssemblies(i).Cycles)
            If mLog.LogAFAssemblies.ShowNGCycles Then mLog.UpdateChildPeriods(4, "NgCycles", mLog.LogAFAssemblies(i).NGCycles)
            If mLog.LogAFAssemblies.ShowNFCycles Then mLog.UpdateChildPeriods(5, "NfCycles", mLog.LogAFAssemblies(i).NFCycles)
            If mLog.LogAFAssemblies.ShowRINS Then mLog.UpdateChildPeriods(6, "RINS", mLog.LogAFAssemblies(i).RINS)
            If mLog.LogAFAssemblies.ShowLandings Then mLog.UpdateChildPeriods(7, "Landings", mLog.LogAFAssemblies(i).Landings)
            If mLog.LogAFAssemblies.ShowStarts Then mLog.UpdateChildPeriods(8, "Starts", mLog.LogAFAssemblies(i).Starts)

            If mLog.LogAFAssemblies.ShowBleeds Then mLog.UpdateChildPeriods(11, "Bleeds", mLog.LogAFAssemblies(i).Bleeds)
            If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.UpdateChildPeriods(12, "ImpellerCycles", mLog.LogAFAssemblies(i).ImpellerCycles)
            If mLog.LogAFAssemblies.ShowCTCycles Then mLog.UpdateChildPeriods(13, "CTCycles", mLog.LogAFAssemblies(i).CTCycles)
            If mLog.LogAFAssemblies.ShowPTCycles Then mLog.UpdateChildPeriods(14, "PTCycles", mLog.LogAFAssemblies(i).PTCycles)
            If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.UpdateChildPeriods(15, "GeneratorMods", mLog.LogAFAssemblies(i).GeneratorMods)
            '-----------------------------

        Next i
        Session("mLog") = mLog
    End Sub

    Public Sub SetEngineGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)        ' For Second Grid i.e ENGINE
        For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1

            If isFromDataBindGrid Then If mLog.LogEngAssemblies(i).ShowHours Then mLog.LogEngAssemblies(i).Hours = Trim(txtAirBorneTime.Text)
            If mLog.LogEngAssemblies(i).ShowLandings Then mLog.LogEngAssemblies(i).Landings = Trim(txtLandings.Text)
            If mLog.LogEngAssemblies(i).ShowCycles Then mLog.LogEngAssemblies(i).Cycles = Trim(txtCycles.Text)
            'If mLog.LogEngAssemblies(i).ShowStarts Then mLog.LogEngAssemblies(i).Starts = Trim(txtEngineStarts.Text)
            'If mLog.LogEngAssemblies(i).ShowNGCycles Then mLog.LogEngAssemblies(i).NGCycles = Trim(txtEngineNGCycles.Text)
            'If mLog.LogEngAssemblies(i).ShowNFCycles Then mLog.LogEngAssemblies(i).NFCycles = Trim(txtEngineNFCycles.Text)
            'If mLog.LogEngAssemblies(i).ShowRINS Then mLog.LogEngAssemblies(i).RINS = Trim(txtEngineRins.Text)
            'If mLog.LogEngAssemblies(i).ShowCFactors Then mLog.LogEngAssemblies(i).CFactor = Trim(txtEngineCFactors.Text)
            'If mLog.LogEngAssemblies(i).ShowBleeds Then mLog.LogEngAssemblies(i).Bleeds = Trim(txtEngineBleeds.Text)
            'If mLog.LogEngAssemblies(i).ShowImpellerCycles Then mLog.LogEngAssemblies(i).ImpellerCycles = Trim(txtEngineImpellerCycles.Text)
            'If mLog.LogEngAssemblies(i).ShowCTCycles Then mLog.LogEngAssemblies(i).CTCycles = Trim(txtEngineCTCycles.Text)
            'If mLog.LogEngAssemblies(i).ShowPTCycles Then mLog.LogEngAssemblies(i).PTCycles = Trim(txtEnginePTCycles.Text)
            '-----------------------------
            'If mLog.LogEngAssemblies(i).ShowGeneratorMods Then mLog.LogEngAssemblies(i).GeneratorMods = Trim(txtEngineGeneratorMods.Text) 'Added by Shweta on 7-May-2012  for ALL02052012
            'If mLog.LogEngAssemblies(i).ShowRapidTakeOffFactors Then mLog.LogEngAssemblies(i).RapidTakeOffFactor = Trim(txtEngineRapidTakeOffFactor.Text) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee

        Next i
        Session("mLog") = mLog
    End Sub
    'Change by Deven 21-03-2008
    'Public Sub SetAPUGridObject()        ' For Third Grid i.e APU
    Public Sub SetAPUGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)        ' For Third Grid i.e APU
        For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
            If isFromDataBindGrid Then If mLog.LogAPUAssemblies(i).ShowHours Then mLog.LogAPUAssemblies.Item(i).Hours = Trim(txtAirBorneTime.Text)
            If mLog.LogAPUAssemblies(i).ShowLandings Then mLog.LogAPUAssemblies.Item(i).Landings = Trim(txtLandings.Text)
            If mLog.LogAPUAssemblies(i).ShowCycles Then mLog.LogAPUAssemblies.Item(i).Cycles = Trim(txtCycles.Text)
            'If mLog.LogAPUAssemblies(i).ShowStarts Then mLog.LogAPUAssemblies.Item(i).Starts = Trim(txtAPUStarts.Text)
            'If mLog.LogAPUAssemblies(i).ShowNGCycles Then mLog.LogAPUAssemblies.Item(i).NGCycles = Trim(txtAPUNGCycles.Text)
            'If mLog.LogAPUAssemblies(i).ShowNFCycles Then mLog.LogAPUAssemblies.Item(i).NFCycles = Trim(txtAPUNFCycles.Text)
            'If mLog.LogAPUAssemblies(i).ShowRINS Then mLog.LogAPUAssemblies.Item(i).RINS = Trim(txtAPURins.Text)
            'If mLog.LogAPUAssemblies(i).ShowBleeds Then mLog.LogAPUAssemblies.Item(i).Bleeds = Trim(txtAPUBleeds.Text)
            'If mLog.LogAPUAssemblies(i).ShowImpellerCycles Then mLog.LogAPUAssemblies.Item(i).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
            'If mLog.LogAPUAssemblies(i).ShowCTCycles Then mLog.LogAPUAssemblies.Item(i).CTCycles = Trim(txtAPUCTCycles.Text)
            'If mLog.LogAPUAssemblies(i).ShowPTCycles Then mLog.LogAPUAssemblies.Item(i).PTCycles = Trim(txtAPUPTCycles.Text)
            'If mLog.LogAPUAssemblies(i).ShowGeneratorMods Then mLog.LogAPUAssemblies.Item(i).GeneratorMods = Trim(txtAPUGeneratorMods.Text) 'Added by Shweta on 7-May-2012
        Next i
        Session("mLog") = mLog
    End Sub
    'Added By Prashant 23-Oct-2009
    Public Sub SetCGBGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)         'For 4th Grid i.e CGB
        ' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgCGBPeriods.Items" is replaced by "dgCGBPeriods.Rows"
        For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1
            If isFromDataBindGrid Then If mLog.LogCGBAssemblies(i).ShowHours Then mLog.LogCGBAssemblies.Item(i).Hours = Trim(txtAirBorneTime.Text)
            If mLog.LogCGBAssemblies(i).ShowLandings Then mLog.LogCGBAssemblies.Item(i).Landings = Trim(txtLandings.Text)
            If mLog.LogCGBAssemblies(i).ShowCycles Then mLog.LogCGBAssemblies.Item(i).Cycles = Trim(txtCycles.Text)
            'If mLog.LogCGBAssemblies(i).ShowStarts Then mLog.LogCGBAssemblies.Item(i).Starts = Trim(txtCGBStarts.Text)
            'If mLog.LogCGBAssemblies(i).ShowNGCycles Then mLog.LogCGBAssemblies.Item(i).NGCycles = Trim(txtCGBNGCycles.Text)
            'If mLog.LogCGBAssemblies(i).ShowNFCycles Then mLog.LogCGBAssemblies.Item(i).NFCycles = Trim(txtCGBNFCycles.Text)
            'If mLog.LogCGBAssemblies(i).ShowRINS Then mLog.LogCGBAssemblies.Item(i).RINS = Trim(txtCGBRins.Text)
            'If mLog.LogCGBAssemblies(i).ShowBleeds Then mLog.LogCGBAssemblies.Item(i).Bleeds = Trim(txtCGBBleeds.Text)
            'If mLog.LogCGBAssemblies(i).ShowImpellerCycles Then mLog.LogCGBAssemblies.Item(i).ImpellerCycles = Trim(txtCGBImpellerCycles.Text)
            'If mLog.LogCGBAssemblies(i).ShowCTCycles Then mLog.LogCGBAssemblies.Item(i).CTCycles = Trim(txtCGBCTCycles.Text)
            'If mLog.LogCGBAssemblies(i).ShowPTCycles Then mLog.LogCGBAssemblies.Item(i).PTCycles = Trim(txtCGBPTCycles.Text)
            'If mLog.LogCGBAssemblies(i).ShowGeneratorMods Then mLog.LogCGBAssemblies.Item(i).GeneratorMods = Trim(txtCGBGeneratorMods.Text) 'Added by Shweta on 7-May-2012 for ALL02052012

        Next i
        Session("mLog") = mLog
    End Sub
    '--------------------------------


#End Region

#Region " Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        takeofftouchdown = CType(AppSettings("TakeOffTouchDown"), Boolean)
        mLog.IsTakeoffTouchDown = takeofftouchdown
        EventLogID = CType(Session("EventLogID"), Guid)
        '  addAttributes()

        If mGBUser Is Nothing Then
            ShowAlertMsg("Session expired ! Please click on Home button on left side menu.", "Session expired..")
            DisableButtons()
            Exit Sub
        End If

        If Not IsPostBack Then

            If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
                mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
                mLog.Pilot1Name = "None"
            End If

            DataFieldBind()
            BindClassification()
        End If
        EnableDisableButton()
        SetTakeoffTouchdownTitle()
        SetFromAutoComplete()
        mLog.LogPageNo = txtLogPageNo.Text.Trim
        mLog.FlightNo = txtFlightNo.Text.Trim
    End Sub
    Private Sub ArrivalClick_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnArrivalClick.Click

        If (mMachine.IsUTC) Then
            If chkArrival.Checked Then
                CalUTCArrival.ReadOnly = False
                CalUTCArrival.BackColor = Color.White
                CalUTCArrival.Enabled = True
                divArrivalDate.Style.Add("background-color", "White")
            Else
                CalUTCArrival.ReadOnly = True
                CalUTCArrival.BackColor = Color.Gainsboro
                CalUTCArrival.Enabled = False
                divArrivalDate.Style.Add("background-color", "Gainsboro")
            End If
        Else
            If chkArrival.Checked Then
                calArrival.ReadOnly = False
                calArrival.BackColor = Color.White
                calArrival.Enabled = True
                divArrivalDate.Style.Add("background-color", "White")
            Else
                calArrival.ReadOnly = True
                calArrival.BackColor = Color.Gainsboro
                calArrival.Enabled = False
                divArrivalDate.Style.Add("background-color", "Gainsboro")
            End If
        End If
        upnlDepartureDet.Update()
        upnlArrivalDet.Update()
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub chkTakeOff_CheckedChanged(sender As Object, e As System.EventArgs) Handles hdnTakeOffClick.Click
        If (mMachine.IsUTC) Then
            If chkTakeOff.Checked Then
                calUTCTakeOffDateTime.ReadOnly = False
                calUTCTakeOffDateTime.BackColor = Color.White
                calUTCTakeOffDateTime.Enabled = True
                divTakeOffDate.Style.Add("background-color", "White")
            Else
                calUTCTakeOffDateTime.ReadOnly = True
                calUTCTakeOffDateTime.BackColor = Color.Gainsboro
                calUTCTakeOffDateTime.Enabled = False
                divTakeOffDate.Style.Add("background-color", "Gainsboro")
            End If
        Else
            If chkTakeOff.Checked Then
                calTakeOffLocalDateTime.ReadOnly = False
                calTakeOffLocalDateTime.BackColor = Color.White
                calTakeOffLocalDateTime.Enabled = True
                divTakeOffDate.Style.Add("background-color", "White")
            Else
                calTakeOffLocalDateTime.ReadOnly = True
                calTakeOffLocalDateTime.BackColor = Color.Gainsboro
                calTakeOffLocalDateTime.Enabled = False
                divTakeOffDate.Style.Add("background-color", "Gainsboro")
            End If
        End If
        upnlTakeOffDet.Update()
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub chkTouchDown_CheckedChanged(sender As Object, e As System.EventArgs) Handles hdnTouchDownClick.Click
        If (mMachine.IsUTC) Then
            If chkTouchDown.Checked Then
                calUTCTouchDownDateTime.ReadOnly = False
                calUTCTouchDownDateTime.BackColor = Color.White
                calUTCTouchDownDateTime.Enabled = True
                divTouchDownDate.Style.Add("background-color", "White")
            Else
                calUTCTouchDownDateTime.ReadOnly = True
                calUTCTouchDownDateTime.BackColor = Color.Gainsboro
                calUTCTouchDownDateTime.Enabled = False
                divTouchDownDate.Style.Add("background-color", "Gainsboro")
            End If
        Else
            If chkTouchDown.Checked Then
                calTouchDownLocalDateTime.ReadOnly = False
                calTouchDownLocalDateTime.BackColor = Color.White
                calTouchDownLocalDateTime.Enabled = True
                divTouchDownDate.Style.Add("background-color", "White")
            Else
                calTouchDownLocalDateTime.ReadOnly = True
                calTouchDownLocalDateTime.BackColor = Color.Gainsboro
                calTouchDownLocalDateTime.Enabled = True
                divTouchDownDate.Style.Add("background-color", "Gainsboro")
            End If
        End If
        upnlTouchDownDet.Update()
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtDepartureTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDepartureTime.TextChanged
        If IsValidTime(txtDepartureTime.Text.ToString.Trim) = False Then
            txtDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calDeparture.Text.ToString + " " + txtDepartureTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mLog.SouLocalDateTime = DateTime
                mLog.TakeOffLocalDateTime = DateTime
                mLog.DesLocalDateTime = DateTime
                mLog.TouchDownLocalDateTime = DateTime

                'Added on 30-Dec-2019
                Dim clnLog As Log
                clnLog = CType(mLog.Clone, Log)
                If mLog.IsNew Then

                    NewRecord(calDateTime.Text.ToString, CType(calDateTime.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime).ToString)
                    Session.Remove("mFileAttach")
                    Session.Remove("IsAttachmentDeleted")
                    CopyFromClone(clnLog, True)
                End If
                ' -------------

                DataFieldBind()
                'SetFocus after databind
                If takeofftouchdown Then
                    chkTakeOff.Focus()
                Else
                    If (Not (mMachine.IsUTC) And takeofftouchdown) Then
                        calTakeOffLocalDateTime.Focus()
                    Else
                        calUTCTakeOffDateTime.Focus()
                    End If
                End If
                'End
            End If
        End If
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtUTCDepartureTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUTCDepartureTime.TextChanged
        If IsValidTime(txtUTCDepartureTime.Text.ToString.Trim) = False Then
            txtUTCDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = CalUTCDateTime.Text.ToString + " " + txtUTCDepartureTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouUniverseDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then

                mLog.SouUniverseDateTime = DateTime
                mLog.TakeOffUniverseDateTime = DateTime

                mLog.DesUniverseDateTime = DateTime
                mLog.TouchDownUniverseDateTime = DateTime

                'Added on 30-Dec-2019
                Dim clnLog As Log
                clnLog = CType(mLog.Clone, Log)
                If mLog.IsNew Then
                    NewRecord(calDateTime.Text.ToString, , CType(calDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime).ToString)
                    Session.Remove("mFileAttach")
                    Session.Remove("IsAttachmentDeleted")
                    CopyFromClone(clnLog, True)
                End If
                ' -------------

                DataFieldBind()
                Place2.Focus() 'SetFocus after databind
            End If
        End If
        upnlDepartureDet.Update()
        upnlTakeOffDet.Update()
        upnlArrivalDet.Update()
        upnlTouchDownDet.Update()
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtTakeOffLocalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTakeOffLocalTime.TextChanged
        If IsValidTime(txtTakeOffLocalTime.Text.ToString.Trim) = False Then
            txtTakeOffLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calTakeOffLocalDateTime.Text.ToString.Trim + " " + txtTakeOffLocalTime.Text.ToString.Trim
            mLog.TakeOffLocalDateTime = DateTime
            DataFieldBind()
            Place2.Focus() 'SetFocus after databind
        End If
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtUTCTakeOffTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCTakeOffTime.TextChanged
        If IsValidTime(txtUTCTakeOffTime.Text.ToString.Trim) = False Then
            txtUTCTakeOffTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calUTCTakeOffDateTime.Text.ToString.Trim + " " + txtUTCTakeOffTime.Text.ToString.Trim
            mLog.TakeOffUniverseDateTime = DateTime
            DataFieldBind()
            Place2.Focus() 'SetFocus after databind
        End If
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtArrivalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtArrivalTime.TextChanged
        If IsValidTime(txtArrivalTime.Text.ToString.Trim) = False Then
            txtArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else

            'calTouchDownLocalDateTime.Text = calArrival.Text.Trim
            'txtTouchDownLocalTime.Text = txtArrivalTime.Text.Trim

            Dim DateTime As String = calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim

            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mLog.DesLocalDateTime = DateTime
                'mLog.TouchDownLocalDateTime = DateTime
                DataFieldBind()
                DataBindGrid()
                'SetFocus after databind
                If takeofftouchdown Then
                    chkTouchDown.Focus()
                Else
                    If (Not (mMachine.IsUTC) And takeofftouchdown) Then
                        calTouchDownLocalDateTime.Focus()
                    Else
                        calUTCTouchDownDateTime.Focus()
                    End If
                End If
                'End
            End If
        End If
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtUTCArrivalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCArrivalTime.TextChanged
        If IsValidTime(txtUTCArrivalTime.Text.ToString.Trim) = False Then
            txtUTCArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            'calUTCTouchDownDateTime.Text = CalUTCArrival.Text.Trim
            'txtUTCTouchDownTime.Text = txtUTCArrivalTime.Text.Trim
            Dim DateTime As String = CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesUniverseDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mLog.DesUniverseDateTime = DateTime
                'mLog.TouchDownUniverseDateTime = DateTime
                DataFieldBind()
                DataBindGrid()
            End If
        End If
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtTouchDownLocalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTouchDownLocalTime.TextChanged
        If IsValidTime(txtTouchDownLocalTime.Text.ToString.Trim) = False Then
            txtTouchDownLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim
            mLog.TouchDownLocalDateTime = DateTime
            DataFieldBind()
            DataBindGrid()
            txtAirBorneTime.Focus() 'SetFocus after databind
        End If
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub txtUTCTouchDownTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCTouchDownTime.TextChanged
        If IsValidTime(txtUTCTouchDownTime.Text.ToString.Trim) = False Then
            txtUTCTouchDownTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim
            mLog.TouchDownUniverseDateTime = DateTime
            DataFieldBind()
            DataBindGrid()
            txtAirBorneTime.Focus() 'SetFocus after databind

        End If
        upnlBlockAirborneTime.Update()
        upnlGroundWithTotalTime.Update()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
            'Code Added By Deven 21-03-2008 
            BindClassification()
            '-------------------------------
            SetObject()
            SetSession()
            mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
            MarkLog(Util.Action.Save, "Flight Log", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

            'MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            ShowAlertMsg("Access Denied.User not authorized", "Not Authorized !")
            Exit Sub
        End If

        If Not IsValid Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.

        'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
        If IsValid Then
            If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
                Dim Title As String = "Save Alert !"
                Dim Message As String = ""
                If Not mLog.PilotID1.Equals(Guid.Empty) Then
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
                    End If
                End If
                If Not mLog.PilotID2.Equals(Guid.Empty) Then
                    Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
                    If mEmployeeStatus(0).Information <> "" Then
                        Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
                    End If
                End If
                If Message.Length > 0 Then
                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
                    ShowAlertMsg(Message, "Save Alert!")
                    Exit Sub
                End If
            End If
        End If
        'End

        'Added by Saylee on 18-May-2012 ALL17052012
        Dim mMaxLogOfAircraft As MaxLogOfAircraft
        mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

        If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
            ' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
            'If (AppSettings("ClientCode") <> "Heligo") Then
            If Not (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL" Or AppSettings("ClientCode") = "APFT") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
                'End
                Dim MaxLogDateTime As String = ""
                'If (AppSettings("LogBookTimeEntry") = "UTC") Then
                If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                    MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted.ToString
                Else
                    MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted.ToString
                End If

                If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                    ShowAlertMsg("You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime, "Save Alert!", True, "SaveLogFlexiLog")
                    Exit Sub
                End If
            Else
                If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
                    'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
                    ShowAlertMsg("You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted, "Save Alert!", True, "SaveLogFlexiLog")
                    Exit Sub
                End If
            End If
        End If

        'Added By Prashant 12-Apr-2010
        Dim IsMELCount As Boolean = False
        Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
        mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
        For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
            'If (mTempMELSnagCorrectiveActionList(i).DueDate > calDateTime.Value) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = True) Then
            If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
                If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
                    IsMELCount = True
                    Exit For
                Else
                    IsMELCount = False
                End If
            End If
        Next
        mTempMELSnagCorrectiveActionList = Nothing
        '-----------------------------------
        If IsMELCount = True Then
            'MSGBoxCtrl.show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MEL")
            ShowAlertMsg("Installed Components does not fulfill Minimum Equipment Level to Fly", "Minimum Equipment Level", True, "MEL")
            If IsValid Then
                'BindClassification()
                SetObject()
                SetAirFrameGridObject()
                SetEngineGridObject(True)
                SetAPUGridObject(True)
                SetCGBGridObject(True)
            Else
                ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
                upnlErrorList.Update()
            End If
            Exit Sub
        End If
        '-------------------------------



        If IsValid Then
            If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
            If Save() = True Then
                mLog = Log.GetLog(mLog.ID)
                mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
                mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
                mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
                Session("mLog") = mLog

                'Added by Saylee on 14-July-2009
                Session("mAircraftInformationBoardList") = Nothing
                '*********************************

                ' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
                ' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                'Added By Vikrant on 01-Dec-2021 for PBH
                If Not Session("IsAircraftMadeNotInUse") Is Nothing Then
                    If Session("IsAircraftMadeNotInUse") = "True" Then
                        Session.Remove("AircraftId")
                        Session.Remove("IsAircraftMadeNotInUse")
                        'MSGBoxCtrl.show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
                        ShowAlertMsg("Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", "Alert!")
                        Exit Sub
                    End If
                End If
                'End
                DataFieldBind()

                EnableDisableButton()

                DataBindGrid()

                SetTitle()

                'upnlLogDetails.Update()
                'upnlFlightDetails.Update()
                'upnlFlightSummary.Update()
                'upnlTabs.Update()
                'upnlTabsNew.Update()
                SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
                ShowAlertMsg("Record Saved Successfully!!", "Save !")
            End If
        Else
            ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
            upnlErrorList.Update()
        End If
    End Sub
#End Region

#Region "Close"
    Protected Sub lnkHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHome.Click
        Try
            Response.Redirect("APPFlightLogList.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub
#End Region


#Region " Web Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim pilotlist As PilotListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        pilotlist = PilotListAutoComplete.GetPilotList(prefixText)
        If count = 0 Then
            Return (From c As PilotListAutoComplete.PilotListAutoCompleteInfo In pilotlist
                    Select c.Name).ToList
        Else
            Return (From c As PilotListAutoComplete.PilotListAutoCompleteInfo In pilotlist
                    Select c.Name).Take(count).ToList
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPlaceCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim placelist As PlaceListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        placelist = PlaceListAutoComplete.GetPlaceList(prefixText)
        If count = 0 Then
            Return (From c As PlaceListAutoComplete.PlaceListAutoCompleteInfo In placelist
                    Select c.Place).ToList
        Else
            Return (From c As PlaceListAutoComplete.PlaceListAutoCompleteInfo In placelist
                    Select c.Place).Take(count).ToList
        End If
    End Function




#End Region
End Class