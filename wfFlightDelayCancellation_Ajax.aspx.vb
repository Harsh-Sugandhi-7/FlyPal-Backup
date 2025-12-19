Imports System.Text
Public Class wfFlightDelayCancellation_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mFligthDelayAndCancellation As FligthDelayAndCancellation
    Dim EventLogID As Guid
    Public mEmployeeList As EmployeeList
    Dim ModuleName As String = "FlightDelayCancellation"
    Dim mEventLogDetail As String = String.Empty
    Public AircraftReadyAt As Boolean = False
    Public ATD As Boolean = False
    Public mFlightLogClassificationList As FlightLogClassificationList
    Public mReportLogRegister As New ReportLogRegister
    Public mATAList As ATAList
    Public mDCCauseAndEffectList As DCCauseAndEffectList
    Public mDCSecondaryCauseList As DCSecondaryCauseList
    Dim mTempAssemblyList As AssemblyList
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim hour As Decimal
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mATAList = Session("mATAList")
        mTempAssemblyList = Session("mTempAssemblyList")
        mReportLogRegister = Session("mReportLogRegister")
        mFligthDelayAndCancellation = CType(Session("mFligthDelayAndCancellation"), FligthDelayAndCancellation)
        mEmployeeList = CType(Session("mEmployeeList"), EmployeeList)
        mFlightLogClassificationList = Session("mFlightLogClassificationList")
        mDCSecondaryCauseList = Session("mDCSecondaryCauseList")
        mDCCauseAndEffectList = Session("mDCCauseAndEffectList")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mTempAssemblyList")
        Session.Remove("mReportLogRegister")
        Session.Remove("mFligthDelayAndCancellation")
        Session.Remove("mEmployeeList")
        Session.Remove("mFlightLogClassificationList")
        Session.Remove("mDCSecondaryCauseList")
        Session.Remove("mDCCauseAndEffectList")
        Session.Remove("FlightDCEdit")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mFligthDelayAndCancellation.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttch.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttch.Enabled = False
        End If
    End Sub
    Private Sub EnableDisableButton()

        If Not mFligthDelayAndCancellation.IsNew Or Session("FlightDCEdit") = True Then
            txtDate.BackColor = Color.Gainsboro
            txtATDTime.BackColor = Color.Gainsboro
            txtSTDTime.BackColor = Color.Gainsboro
            txtAircraftReadyAtTime.BackColor = Color.Gainsboro

            txtATDTime.ReadOnly = True
            txtSTDTime.ReadOnly = True
            txtAircraftReadyAtTime.ReadOnly = True

            'chkATD.Enabled = False
            'chkAircraftReadyAt.Enabled = False
            chkDelay.Enabled = False
            chkCancel.Enabled = False
            ChkReliability.Enabled = False

            txtTechDelay.ReadOnly = True
            txtTechDelay.BackColor = Color.Gainsboro

            txtOtherDelay.ReadOnly = True
            txtOtherDelay.BackColor = Color.Gainsboro
            cmbLogNo.Enabled = False
            txtDate.Enabled = False
            txtSTDDate.Enabled = False
            txtSTDTime.Enabled = False
            txtATDDate.Enabled = False
            txtATDTime.Enabled = False
            txtAircraftReadyAtDate.Enabled = False
            txtAircraftReadyAtTime.Enabled = False
        Else
            txtDate.BackColor = Color.White
            txtATDTime.BackColor = Color.White
            txtSTDTime.BackColor = Color.White
            txtAircraftReadyAtTime.BackColor = Color.White

            txtATDTime.ReadOnly = False
            txtSTDTime.ReadOnly = False
            txtAircraftReadyAtTime.ReadOnly = False


            txtTechDelay.ReadOnly = False
            txtTechDelay.BackColor = Color.White

            txtOtherDelay.ReadOnly = False
            txtOtherDelay.BackColor = Color.White

            'chkATD.Enabled = True
            'chkAircraftReadyAt.Enabled = True
            chkDelay.Enabled = True
            chkCancel.Enabled = True
            ChkReliability.Enabled = True
            cmbLogNo.Enabled = True
            txtDate.Enabled = True
            txtSTDDate.Enabled = True
            txtSTDTime.Enabled = True
            txtATDDate.Enabled = True
            txtATDTime.Enabled = True
            txtAircraftReadyAtDate.Enabled = True
            txtAircraftReadyAtTime.Enabled = True
        End If

        'Date 
        txtATDDate.ReadOnly = IIf(Not mFligthDelayAndCancellation.IsNew Or Session("FlightDCEdit") = "True", True, False)
        txtAircraftReadyAtDate.ReadOnly = IIf(Not mFligthDelayAndCancellation.IsNew Or Session("FlightDCEdit") = "True", True, False)
        txtSTDDate.ReadOnly = IIf(Not mFligthDelayAndCancellation.IsNew Or Session("FlightDCEdit") = "True", True, False)

        If Not mFligthDelayAndCancellation.IsNew Or Session("FlightDCEdit") = "True" Then
            txtATDDate.ReadOnly = True
            txtATDDate.BackColor = Color.Gainsboro
            txtAircraftReadyAtDate.ReadOnly = True
            txtAircraftReadyAtDate.BackColor = Color.Gainsboro
            txtSTDDate.ReadOnly = True
            txtSTDDate.BackColor = Color.Gainsboro
        Else
            txtATDDate.ReadOnly = False
            txtATDDate.BackColor = Color.White
            txtAircraftReadyAtDate.ReadOnly = False
            txtAircraftReadyAtDate.BackColor = Color.White
            txtSTDDate.ReadOnly = False
            txtSTDDate.BackColor = Color.White
        End If

    End Sub
    Private Sub ControlVisibility()
        If chkDelay.Checked Then
            lblLogStar.Visible = True
            lblATDStar.Visible = True
            lblSTDStar.Visible = True
            lblPICStar.Visible = True
            lblTechDelayStar.Visible = True
        Else
            lblLogStar.Visible = False
            lblATDStar.Visible = False
            lblSTDStar.Visible = False
            lblPICStar.Visible = False
            lblTechDelayStar.Visible = False
        End If
    End Sub
    Private Sub SetTitle()
        If mFligthDelayAndCancellation.IsNew Then
            lblTitle.Text = "Flight Delay/Cancellation Details [ New ]"
        Else
            lblTitle.Text = "Flight Delay/Cancellation Details " & "[ " & mFligthDelayAndCancellation.RegNo & " ]"
        End If

    End Sub
    Private Sub SetCheckBoxStatus()
        AircraftReadyAt = chkAircraftReadyAt.Checked
        Session("AircraftReadyAt") = AircraftReadyAt
        ATD = chkATD.Checked
        Session("ATD") = ATD
    End Sub
    Private Function IsValidTime(ByVal TimeValue As String) As Boolean
        Dim TimeRegulerExpression As String = ""
        If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
            'TimeRegulerExpression = "^(([01][\d]+)|(2[0-3]))\:[0-5][0-9]( )*(AM|am|PM|pm)$"   '12 Hour Format
            TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
        Else
            TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
        End If

        If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mFligthDelayAndCancellation.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mFligthDelayAndCancellation.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub SetObject()
        With mFligthDelayAndCancellation
            If Not IsDate(txtDate.Text.Trim) Then
                .Date = System.DBNull.Value
            Else
                .Date = txtDate.Text.Trim
            End If
            .IsDelay = chkDelay.Checked
            .IsCancel = chkCancel.Checked
            .ConsiderInReliability = ChkReliability.Checked
            .LogID = New Guid(cmbLogNo.SelectedValue)
            .FlightNo = txtFlightNo.Text.Trim
            .LogPageNo = txtLogPageNo.Text.Trim
            .Route = txtRoute.Text.Trim
            .FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue)
            .ALEID = New Guid(cmbALE.SelectedValue)
            .PICID = New Guid(cmbPIC.SelectedValue)
            .ATAID = New Guid(cmbATAChapter.SelectedValue)
            If Not IsDate(txtSTDDate.Text.Trim) Then
                .StandardTimeOfDeparture = System.DBNull.Value
            Else
                Dim DateTime As String = txtSTDDate.Text.ToString + " " + txtSTDTime.Text.ToString.Trim
                .StandardTimeOfDeparture = DateTime
            End If

            If Not IsDate(txtATDDate.Text.Trim) Then
                .ActualTimeOfDeparture = System.DBNull.Value
            Else
                Dim DateTime As String = txtATDDate.Text.ToString + " " + txtATDTime.Text.ToString.Trim
                .ActualTimeOfDeparture = DateTime
            End If


            If Not IsDate(txtAircraftReadyAtDate.Text.Trim) Then
                .AircraftReadyAt = System.DBNull.Value
            Else
                Dim DateTime As String = txtAircraftReadyAtDate.Text.ToString + " " + txtAircraftReadyAtTime.Text.ToString.Trim
                .AircraftReadyAt = DateTime
            End If

            .TechDelay = txtTechDelay.Text.Trim
            .OtherDelay = txtOtherDelay.Text.Trim
            .CauseOfOtherDC = txtCauseofOtherDC.Text.Trim
            .OtherCauseAndEffect = txtOthers.Text.Trim
            .PrimaryCause = txtPrimaryCause.Text.Trim
            .Investigation = txtInvestigation.Text.Trim
            .PreventiveMeasure = txtPreventiveMeasure.Text.Trim
            .Remarks = txtRemark.Text.Trim
            .InvestigatedByID = New Guid(cmbInvestigatedBy.SelectedValue)
            .ApprovedByID = New Guid(cmbApprovedBy.SelectedValue)

            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
            End If

            'DC Cause And Effects
            For i As Integer = 0 To ChklistCauseAndEffect.Items.Count - 1
                If ChklistCauseAndEffect.Items(i).Selected = True Then
                    If .FlightDCCauseAndEffects.Contains(New Guid(ChklistCauseAndEffect.Items(i).Value.ToString)) = False Then
                        .FlightDCCauseAndEffects.Add(New Guid(ChklistCauseAndEffect.Items(i).Value.ToString), .ID)
                        .FlightDCCauseAndEffects.CurrentItem.DCCauseAndEffectID = New Guid(ChklistCauseAndEffect.Items(i).Value.ToString)
                        .FlightDCCauseAndEffects.CurrentItem.FlightDelayAndCancellationID = .ID
                    End If
                Else
                    .FlightDCCauseAndEffects.Remove(New Guid(ChklistCauseAndEffect.Items(i).Value.ToString))
                End If
            Next
            'End

            'DC Secondary Cause
            For i As Integer = 0 To chkListSecondaryCause.Items.Count - 1
                If chkListSecondaryCause.Items(i).Selected = True Then
                    If .FlightDCSecondaryCauses.Contains(New Guid(chkListSecondaryCause.Items(i).Value.ToString)) = False Then
                        .FlightDCSecondaryCauses.Add(New Guid(chkListSecondaryCause.Items(i).Value.ToString), .ID)
                        .FlightDCSecondaryCauses.CurrentItem.DCSecondaryCauseID = New Guid(chkListSecondaryCause.Items(i).Value.ToString)
                        .FlightDCSecondaryCauses.CurrentItem.FlightDelayAndCancellationID = .ID

                    End If
                Else
                    .FlightDCSecondaryCauses.Remove(New Guid(chkListSecondaryCause.Items(i).Value.ToString))
                End If
            Next
            'End
            Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
        End With
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        DataFieldBinding()
                        If (Not User.IsInRole("FlightDelayCancellationNew") And Not User.IsInRole("FlightDelayCancellationEdit")) Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                            Exit Sub
                        End If
                        If Save() Then
                            MarkLog(Util.Action.Close, ModuleName, "", Util.ErrorType.NoError, mFligthDelayAndCancellation.ID, EventLogID)
                            RemoveSession()
                            Response.Redirect("Index.aspx")
                        Else
                            upnlValidationSummary.Update()
                            Exit Sub
                        End If

                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        RemoveSession()
                        Response.Redirect("Index.aspx")
                    End If
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "Save" Or MSGBoxCtrl.Sender = "SaveNew" Then
                        Session("sender") = ""
                    End If
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            If Session("New") = "True" Then Session("New") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
            If Session("New") = "True" Then Session("New") = ""
        End If
    End Sub
    Private Function Save() As Boolean
        SetObject()
        If CustomValidate1() Then
            Try
                If Not mFligthDelayAndCancellation.IsValid Then Return False

                'Added By Vikrant On 06-Aug-2013 For ALL01082013
                If Not mFligthDelayAndCancellation.PICID.Equals(Guid.Empty) Or Not mFligthDelayAndCancellation.ALEID.Equals(Guid.Empty) Or mFligthDelayAndCancellation.InvestigatedByID.Equals(Guid.Empty) Or mFligthDelayAndCancellation.ApprovedByID.Equals(Guid.Empty) Then
                    Dim title As String = "Save Alert !"
                    Dim message As New StringBuilder
                    Dim mEmployeeIDs As Guid() = {mFligthDelayAndCancellation.PICID, mFligthDelayAndCancellation.ALEID, mFligthDelayAndCancellation.InvestigatedByID, mFligthDelayAndCancellation.ApprovedByID}
                    For i As Integer = 0 To mEmployeeIDs.Length - 1
                        If Not mEmployeeIDs(i).Equals(Guid.Empty) Then
                            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeIDs(i).ToString, mFligthDelayAndCancellation.Date)
                            If (mEmployeeStatus(0).Information <> "") Then
                                Select Case i
                                    Case 0
                                        message.Append("<b>" + "PIC : " + "</b></br>")
                                        message.Append(mEmployeeStatus(0).Information)
                                        message.Append("</br>")
                                    Case 1
                                        message.Append("<b>" + "ALE : " + "</b></br>")
                                        message.Append(mEmployeeStatus(0).Information)
                                        message.Append("</br>")
                                    Case 2
                                        message.Append("<b>" + "Investigated By : " + "</b></br>")
                                        message.Append(mEmployeeStatus(0).Information)
                                        message.Append("</br>")
                                    Case 3
                                        message.Append("<b>" + "Approved By : " + "</b></br>")
                                        message.Append(mEmployeeStatus(0).Information)
                                        message.Append("</br>")
                                End Select
                            End If
                        End If
                    Next
                    If message.Length > 0 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message.ToString, , False), True)
                        Return False
                    End If
                End If
                'End

                mFligthDelayAndCancellation = CType(mFligthDelayAndCancellation.Save(), FligthDelayAndCancellation)
                Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
                SaveAttachment()
                Dim status As String = ""
                If mFligthDelayAndCancellation.IsDelay Then
                    status = "Delay"
                Else
                    status = "Cancel"
                End If
                mEventLogDetail = "Reg No : " & mFligthDelayAndCancellation.RegNo & ", Dated : " & mFligthDelayAndCancellation.DateFormatted & ", Status : " & status & ", Log No. : " & mReportLogRegister(mFligthDelayAndCancellation.LogID).LogNo
                MarkLog(Util.Action.Save, ModuleName, mEventLogDetail, Util.ErrorType.NoError, mFligthDelayAndCancellation.ID, EventLogID)
                Return True
            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                'FlightDCClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetValues(ByVal IsClear As Boolean)
        If Not IsClear Then
            mFligthDelayAndCancellation.FlightNo = mReportLogRegister(cmbLogNo.SelectedIndex).FlightNo
            mFligthDelayAndCancellation.LogPageNo = mReportLogRegister(cmbLogNo.SelectedIndex).LogPageNo
            mFligthDelayAndCancellation.LogID = mReportLogRegister(cmbLogNo.SelectedIndex).LogID
            mFligthDelayAndCancellation.Route = mReportLogRegister(cmbLogNo.SelectedIndex).DepartureFrom & " - " & mReportLogRegister(cmbLogNo.SelectedIndex).ArrivalTo
            'If AppSettings("LogBookTimeEntry") = "UTC" Then
            If mReportLogRegister(cmbLogNo.SelectedIndex).IsUTC Then
                If AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then
                    mFligthDelayAndCancellation.ActualTimeOfDeparture = mReportLogRegister(cmbLogNo.SelectedIndex).LogDate 'Added By Prashant  31-Jul-2018 APFT31082018
                Else
                    mFligthDelayAndCancellation.ActualTimeOfDeparture = mReportLogRegister(cmbLogNo.SelectedIndex).DepartureUTCTime
                End If
            Else
                If AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then
                    mFligthDelayAndCancellation.ActualTimeOfDeparture = mReportLogRegister(cmbLogNo.SelectedIndex).LogDate 'Added By Prashant  31-Jul-2018 APFT31082018
                Else
                    mFligthDelayAndCancellation.ActualTimeOfDeparture = mReportLogRegister(cmbLogNo.SelectedIndex).DepartureTime
                End If
            End If
            mFligthDelayAndCancellation.PICID = mEmployeeList(mReportLogRegister(cmbLogNo.SelectedIndex).PilotName).ID
            mFligthDelayAndCancellation.FlightLogClassificationID = mReportLogRegister(cmbLogNo.SelectedIndex).FlightLogClassificationID
        Else
            mFligthDelayAndCancellation.FlightNo = ""
            mFligthDelayAndCancellation.LogPageNo = ""
            mFligthDelayAndCancellation.LogID = Guid.Empty
            mFligthDelayAndCancellation.Route = ""
            mFligthDelayAndCancellation.ActualTimeOfDeparture = System.DBNull.Value
            mFligthDelayAndCancellation.AircraftReadyAt = System.DBNull.Value
            mFligthDelayAndCancellation.PICID = Guid.Empty
            mFligthDelayAndCancellation.FlightLogClassificationID = Guid.Empty

        End If
        DataBind()
        SetDateTime()
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBinding()
        mATAList = ATAList.GetATAList("", "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList

        mTempAssemblyList = AssemblyList.GetAssemblyList(1, mFligthDelayAndCancellation.MachineID.ToString)
        Session("mTempAssemblyList") = mTempAssemblyList

        mReportLogRegister = ReportLogRegister.GetRectifiedLog(mFligthDelayAndCancellation.Date.ToString, mFligthDelayAndCancellation.Date.ToString, mTempAssemblyList(0).ID.ToString, mFligthDelayAndCancellation.MachineID.ToString, False, , 2, , , , "(SELECT)", False, , True, True)
        cmbLogNo.DataSource = mReportLogRegister
        Session("mReportLogRegister") = mReportLogRegister

        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(SELECT)")

        cmbPIC.DataSource = mEmployeeList
        cmbALE.DataSource = mEmployeeList
        cmbApprovedBy.DataSource = mEmployeeList
        cmbInvestigatedBy.DataSource = mEmployeeList
        Session("mEmployeeList") = mEmployeeList

        mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(SELECT)")
        cmbFlightLogClassification.DataSource = mFlightLogClassificationList
        Session("mFlightLogClassificationList") = mFlightLogClassificationList
        SetDateTime()

        mDCCauseAndEffectList = DCCauseAndEffectList.GetCauseAndEffectList()
        ChklistCauseAndEffect.DataSource = mDCCauseAndEffectList
        Session("mDCCauseAndEffectList") = mDCCauseAndEffectList

        mDCSecondaryCauseList = DCSecondaryCauseList.GetSecondaryCauseList()
        chkListSecondaryCause.DataSource = mDCSecondaryCauseList
        Session("mDCSecondaryCauseList") = mDCSecondaryCauseList

        DataBind()

        'DC Cause And Effect
        For i As Integer = 0 To ChklistCauseAndEffect.Items.Count - 1
            If mFligthDelayAndCancellation.FlightDCCauseAndEffects.Contains(New Guid(ChklistCauseAndEffect.Items(i).Value.ToString)) = True Then
                ChklistCauseAndEffect.Items(i).Selected = True
            End If
        Next
        'End

        'DC Secondary Cause
        For i As Integer = 0 To chkListSecondaryCause.Items.Count - 1
            If mFligthDelayAndCancellation.FlightDCSecondaryCauses.Contains(New Guid(chkListSecondaryCause.Items(i).Value.ToString)) = True Then
                chkListSecondaryCause.Items(i).Selected = True
            End If
        Next
        'End
        If Not mFligthDelayAndCancellation.Date Is System.DBNull.Value Then
            txtDate.Text = Format(CDate(mFligthDelayAndCancellation.Date), AppSettings("DateFormat"))
        Else
            txtDate.Text = Format(Today.Date.ToString, AppSettings("DateFormat"))
        End If
        'SetDateTime()
    End Sub
    Private Sub SetDateTime()
        If Not mFligthDelayAndCancellation.StandardTimeOfDeparture Is System.DBNull.Value Then
            txtSTDDate.Text = Format(CDate(mFligthDelayAndCancellation.StandardTimeOfDeparture), AppSettings("DateFormat")) 'Previous DateTimeFormatLOG
            txtSTDTime.Text = Format(CDate(mFligthDelayAndCancellation.StandardTimeOfDeparture), AppSettings("TimeFormat"))
        Else
            txtSTDDate.Text = ""
        End If

        If Not mFligthDelayAndCancellation.ActualTimeOfDeparture Is System.DBNull.Value Then
            txtATDDate.Text = Format(CDate(mFligthDelayAndCancellation.ActualTimeOfDeparture), AppSettings("DateFormat")) 'Previous DateTimeFormatLOG
            txtATDTime.Text = Format(CDate(mFligthDelayAndCancellation.ActualTimeOfDeparture), AppSettings("TimeFormat"))
        Else
            txtATDDate.Text = ""
        End If

        If Not mFligthDelayAndCancellation.AircraftReadyAt Is System.DBNull.Value Then
            txtAircraftReadyAtDate.Text = Format(CDate(mFligthDelayAndCancellation.AircraftReadyAt), AppSettings("DateFormat")) 'Previous DateTimeFormatLOG
            txtAircraftReadyAtTime.Text = Format(CDate(mFligthDelayAndCancellation.AircraftReadyAt), AppSettings("TimeFormat"))
        Else
            txtAircraftReadyAtDate.Text = ""
        End If
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim str As String = ""
        If Not mFligthDelayAndCancellation.IsValid Then
            For i As Integer = 0 To mFligthDelayAndCancellation.GetBrokenRulesCollection.Count - 1
                str = str + mFligthDelayAndCancellation.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If

        For i As Integer = 0 To mFligthDelayAndCancellation.FlightDCCauseAndEffects.Count - 1
            If Not mFligthDelayAndCancellation.FlightDCCauseAndEffects(i).IsValid Then
                For j As Integer = 0 To mFligthDelayAndCancellation.FlightDCCauseAndEffects(i).GetBrokenRulesCollection.Count - 1
                    str = str + mFligthDelayAndCancellation.FlightDCCauseAndEffects.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If
        Next

        For i As Integer = 0 To mFligthDelayAndCancellation.FlightDCSecondaryCauses.Count - 1
            If Not mFligthDelayAndCancellation.FlightDCSecondaryCauses(i).IsValid Then
                For j As Integer = 0 To mFligthDelayAndCancellation.FlightDCSecondaryCauses(i).GetBrokenRulesCollection.Count - 1
                    str = str + mFligthDelayAndCancellation.FlightDCSecondaryCauses.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvCommon.ErrorMessage = str
            cvCommon.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBinding()
            EnableDisableButton()
            ControlVisibility()
            ControlVisibilityForAttachment()
            SetTitle()
        End If
    End Sub
    Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
        If IsPostBack Then
            '# Date Control Validation #
            'Try
            '    Dim tempdate As DateTime
            '    Dim Datestring As String = Format(CDate(txtDate.Text.Trim), AppSettings("DateFormat"))

            '    tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
            '    If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
            '        If Not ViewState("txtDate") Is Nothing Then
            '            txtDate.Text = Format(CDate(ViewState("txtDate")), AppSettings("DateFormat"))
            '        Else
            '            txtDate.Text = Format(Today.Date, AppSettings("DateFormat"))
            '        End If
            '    Else
            '        txtDate.Text = Format(tempdate, AppSettings("DateFormat"))
            '    End If
            '    ViewState("txtDate") = txtDate.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            'Catch ex As Exception
            '    If Not ViewState("txtDate") Is Nothing Then
            '        txtDate.Text = Format(CDate(ViewState("txtDate")), AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
            '    Else
            '        txtDate.Text = Format(Today.Date, AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
            '    End If
            '    RaiseEvent TextChanged(txtDate.Text, e)  'Raising textchange event for further calculation
            '    Exit Sub
            'End Try

            '# End
            If DateDiff(DateInterval.Day, CDate(mFligthDelayAndCancellation.Date.ToString), CDate(txtDate.Text.Trim)) <> 0 Then
                mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtDate.Text.Trim, txtDate.Text.Trim, mTempAssemblyList(0).ID.ToString, mFligthDelayAndCancellation.MachineID.ToString, False, , 2, , , , "(SELECT)", False, , True, True)
                cmbLogNo.DataSource = mReportLogRegister
                Session("mReportLogRegister") = mReportLogRegister
                mFligthDelayAndCancellation.Date = txtDate.Text.ToString
                mFligthDelayAndCancellation.StandardTimeOfDeparture = txtDate.Text.ToString

                If Not mFligthDelayAndCancellation.StandardTimeOfDeparture Is System.DBNull.Value Then
                    txtSTDDate.Text = Format(CDate(mFligthDelayAndCancellation.StandardTimeOfDeparture), AppSettings("DateFormat"))
                    txtSTDTime.Text = Format(CDate(mFligthDelayAndCancellation.StandardTimeOfDeparture), AppSettings("TimeFormat"))
                Else
                    txtSTDDate.Text = ""
                End If
                SetObject()
                SetValues(True)
                Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
                upnlFlightDetails.Update()
            End If
            'SetTitle()
        End If
    End Sub
    Private Sub cmbLogNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLogNo.SelectedIndexChanged
        If cmbLogNo.SelectedIndex > 0 Then
            SetObject()
            SetValues(False)
            txtRoute.Enabled = False
            txtLogPageNo.Enabled = False
        Else
            SetObject()
            SetValues(True)
            txtRoute.Enabled = True
            txtLogPageNo.Enabled = True
        End If
        Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
    End Sub
    Private Sub txtATDDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtATDDate.TextChanged
        If IsPostBack Then

            'If Trim(txtATDDate.Text) = "" Then
            '    ViewState("txtATDDate") = txtATDDate.Text.Trim
            '    Exit Sub
            'End If

            ''# Date Control Validation #

            'Try
            '    Dim tempdate As DateTime
            '    Dim Datestring As String = Format(CDate(txtATDDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '    tempdate = DateTime.ParseExact(Datestring, AppSettings("DateTimeFormatLOG"), System.Globalization.CultureInfo.InvariantCulture).ToString()
            '    If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
            '        If Not ViewState("txtATDDate") Is Nothing Then
            '            txtATDDate.Text = Format(CDate(ViewState("txtATDDate")), AppSettings("DateTimeFormatLOG"))
            '        Else
            '            txtATDDate.Text = Format(CDate(txtDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '        End If
            '    Else
            '        txtATDDate.Text = Format(tempdate, AppSettings("DateTimeFormatLOG"))
            '    End If
            '    ViewState("txtATDDate") = txtATDDate.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            'Catch ex As Exception
            '    If Not ViewState("calTakeOffLocalDateTime") Is Nothing Then
            '        txtATDDate.Text = Format(CDate(ViewState("txtATDDate")), AppSettings("DateTimeFormatLOG"))
            '    Else
            '        txtATDDate.Text = Format(CDate(txtDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '    End If
            'End Try
            If txtATDDate.Text = "" Then
                mFligthDelayAndCancellation.ActualTimeOfDeparture = System.DBNull.Value
            Else
                mFligthDelayAndCancellation.ActualTimeOfDeparture = txtATDDate.Text
            End If

            If Not mFligthDelayAndCancellation.ActualTimeOfDeparture Is System.DBNull.Value Then
                txtATDDate.Text = Format(CDate(mFligthDelayAndCancellation.ActualTimeOfDeparture), AppSettings("DateFormat"))
                txtATDTime.Text = Format(CDate(mFligthDelayAndCancellation.ActualTimeOfDeparture), AppSettings("TimeFormat"))
            Else
                txtATDDate.Text = ""
            End If
            Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
            EnableDisableButton()
            'upnlDelayDate.Update()
            'upnlDelayDetails.Update()
            '# End
        End If
    End Sub
    Private Sub txtAircraftReadyAtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAircraftReadyAtDate.TextChanged
        If IsPostBack Then

            'If Trim(txtAircraftReadyAtDate.Text) = "" Then
            '    ViewState("txtAircraftReadyAtDate") = txtAircraftReadyAtDate.Text.Trim
            '    Exit Sub
            'End If
            ''# Date Control Validation #
            'Try
            '    Dim tempdate As DateTime
            '    Dim Datestring As String = Format(CDate(txtAircraftReadyAtDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '    tempdate = DateTime.ParseExact(Datestring, AppSettings("DateTimeFormatLOG"), System.Globalization.CultureInfo.InvariantCulture).ToString()
            '    If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
            '        If Not ViewState("txtAircraftReadyAtDate") Is Nothing Then
            '            txtAircraftReadyAtDate.Text = Format(CDate(ViewState("txtAircraftReadyAtDate")), AppSettings("DateTimeFormatLOG"))
            '        Else
            '            txtAircraftReadyAtDate.Text = Format(CDate(txtDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '        End If
            '    Else
            '        txtAircraftReadyAtDate.Text = Format(tempdate, AppSettings("DateTimeFormatLOG"))
            '    End If
            '    ViewState("txtAircraftReadyAtDate") = txtAircraftReadyAtDate.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            'Catch ex As Exception
            '    If Not ViewState("txtAircraftReadyAtDate") Is Nothing Then
            '        txtAircraftReadyAtDate.Text = Format(CDate(ViewState("txtAircraftReadyAtDate")), AppSettings("DateTimeFormatLOG"))
            '    Else
            '        txtAircraftReadyAtDate.Text = Format(CDate(txtDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '    End If
            'End Try
            If txtAircraftReadyAtDate.Text = "" Then
                mFligthDelayAndCancellation.AircraftReadyAt = System.DBNull.Value
            Else
                mFligthDelayAndCancellation.AircraftReadyAt = txtAircraftReadyAtDate.Text
            End If
            If Not mFligthDelayAndCancellation.AircraftReadyAt Is System.DBNull.Value Then
                txtAircraftReadyAtDate.Text = Format(CDate(mFligthDelayAndCancellation.AircraftReadyAt), AppSettings("DateFormat"))
                txtAircraftReadyAtTime.Text = Format(CDate(mFligthDelayAndCancellation.AircraftReadyAt), AppSettings("TimeFormat"))
            Else
                txtAircraftReadyAtDate.Text = ""
            End If
            Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
            EnableDisableButton()
            'upnlDelayDate.Update()
            'upnlDelayDetails.Update()
            '# End
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mFligthDelayAndCancellation.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            MarkLog(Util.Action.Close, ModuleName, "", Util.ErrorType.NoError, mFligthDelayAndCancellation.ID, EventLogID)
            RemoveSession()
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub txtSTDTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSTDTime.TextChanged
        If IsValidTime(txtSTDTime.Text.ToString.Trim) = False Then
            txtSTDTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = txtSTDDate.Text.ToString + " " + txtSTDTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mFligthDelayAndCancellation.StandardTimeOfDeparture.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mFligthDelayAndCancellation.StandardTimeOfDeparture = DateTime

                'Added By Prashant  31-Jul-2018 APFT31082018
                hour = DateDiff(DateInterval.Minute, New SmartDate(txtSTDDate.Text.ToString + " " + txtSTDTime.Text.ToString.Trim).Date, SmartDate.StringToDate(mFligthDelayAndCancellation.ActualTimeOfDeparture.ToString))
                mFligthDelayAndCancellation.TechDelay = (New Period(1, hour, 0)).Value

                If mFligthDelayAndCancellation.TechDelay.Contains("-") Then
                    mFligthDelayAndCancellation.TechDelay = "0:00"
                    txtTechDelay.Text = mFligthDelayAndCancellation.TechDelay
                    txtTechDelay.DataBind()
                    upnlDelayDetails.Update()
                Else
                    txtTechDelay.Text = mFligthDelayAndCancellation.TechDelay
                    txtTechDelay.DataBind()
                    upnlDelayDetails.Update()
                End If
                'End of Added By Prashant  31-Jul-2018 APFT31082018
                Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
            End If
        End If
    End Sub
    Private Sub txtATDTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtATDTime.TextChanged
        If IsValidTime(txtATDTime.Text.ToString.Trim) = False Then
            txtATDTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = txtATDDate.Text.ToString + " " + txtATDTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mFligthDelayAndCancellation.ActualTimeOfDeparture.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mFligthDelayAndCancellation.ActualTimeOfDeparture = DateTime

                'Added By Prashant  31-Jul-2018 APFT31082018
                hour = DateDiff(DateInterval.Minute, New SmartDate(txtSTDDate.Text.ToString + " " + txtSTDTime.Text.ToString.Trim).Date, SmartDate.StringToDate(mFligthDelayAndCancellation.ActualTimeOfDeparture.ToString))
                mFligthDelayAndCancellation.TechDelay = (New Period(1, hour, 0)).Value

                If mFligthDelayAndCancellation.TechDelay.Contains("-") Then
                    mFligthDelayAndCancellation.TechDelay = "0:00"
                    txtTechDelay.Text = mFligthDelayAndCancellation.TechDelay
                    txtTechDelay.DataBind()
                    upnlDelayDetails.Update()
                Else
                    txtTechDelay.Text = mFligthDelayAndCancellation.TechDelay
                    txtTechDelay.DataBind()
                    upnlDelayDetails.Update()
                End If
                'End of Added By Prashant  31-Jul-2018 APFT31082018
                Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
            End If
        End If
    End Sub
    Private Sub chkDelay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDelay.CheckedChanged
        If chkDelay.Checked Then
            mFligthDelayAndCancellation.IsDelay = True
            chkCancel.Checked = False
            mFligthDelayAndCancellation.IsCancel = False
        Else
            mFligthDelayAndCancellation.IsDelay = False
            chkCancel.Checked = True
            mFligthDelayAndCancellation.IsCancel = True
        End If
        Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
        EnableDisableButton()
        ControlVisibility()
        upnlFlightDetails.Update()
        upnlDelayDetails.Update()
    End Sub
    Private Sub chkCancel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCancel.CheckedChanged
        If chkCancel.Checked Then
            mFligthDelayAndCancellation.IsDelay = False
            chkDelay.Checked = False
            mFligthDelayAndCancellation.IsCancel = True
        Else
            mFligthDelayAndCancellation.IsDelay = True
            chkDelay.Checked = True
            mFligthDelayAndCancellation.IsCancel = False
        End If
        Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
        EnableDisableButton()
        ControlVisibility()
        upnlFlightDetails.Update()
        upnlDelayDetails.Update()
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If (Not User.IsInRole("FlightDelayCancellationNew") And Not User.IsInRole("FlightDelayCancellationEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'Dim FlightDCClone As FligthDelayAndCancellation
        'FlightDCClone = CType(mFligthDelayAndCancellation.Clone, FligthDelayAndCancellation)
        If Save() Then
            DataFieldBinding()
            EnableDisableButton()
            ControlVisibility()
            ControlVisibilityForAttachment()
            SetTitle()
            upnlDelayDate.Update()
            upnlDelayDetails.Update()
            upnlFlightDetails.Update()
            upnlFileupload.Update()
            upnlTitle.Update()
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub chkATD_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkATD.CheckedChanged
        If chkATD.Checked Then
            txtATDDate.ReadOnly = False
            txtATDDate.BackColor = Color.White
        Else
            txtATDDate.ReadOnly = True
            txtATDDate.BackColor = Color.Gainsboro
        End If
        ATD = chkATD.Checked
        Session("ATD") = ATD
    End Sub
    Private Sub chkAircraftReadyAt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAircraftReadyAt.CheckedChanged
        If chkAircraftReadyAt.Checked Then
            txtAircraftReadyAtDate.ReadOnly = False
            txtAircraftReadyAtDate.BackColor = Color.White
        Else
            txtAircraftReadyAtDate.ReadOnly = True
            txtAircraftReadyAtDate.BackColor = Color.Gainsboro
        End If
        AircraftReadyAt = chkAircraftReadyAt.Checked
        Session("AircraftReadyAt") = AircraftReadyAt
    End Sub
    Private Sub btnCauseAndEffect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCauseAndEffect.Click
        SetObject()
        Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCauseAndEffectMasterWindow", "OpenCauseAndEffectMasterWindow()", True)
        'Response.Redirect("wfDCCauseAndEffect.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage1=wfFlightDelayCancellation.aspx")
    End Sub
    Private Sub btnSecondaryCause_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSecondaryCause.Click
        SetObject()
        Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSecCauseMasterWindow", "OpenSecCauseMasterWindow()", True)
        'Response.Redirect("wfDCSecondaryCause.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage1=wfFlightDelayCancellation.aspx")
    End Sub
    Private Sub txtSTDDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSTDDate.TextChanged
        If IsPostBack Then

            'If Trim(txtSTDDate.Text) = "" Then
            '    ViewState("txtSTDDate") = txtSTDDate.Text.Trim
            '    Exit Sub
            'End If

            ''# Date Control Validation #

            'Try
            '    Dim tempdate As DateTime
            '    Dim Datestring As String = Format(CDate(txtSTDDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '    tempdate = DateTime.ParseExact(Datestring, AppSettings("DateTimeFormatLOG"), System.Globalization.CultureInfo.InvariantCulture).ToString()
            '    If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
            '        If Not ViewState("txtAircraftReadyAtDate") Is Nothing Then
            '            txtSTDDate.Text = Format(CDate(ViewState("txtSTDDate")), AppSettings("DateTimeFormatLOG"))
            '        Else
            '            txtSTDDate.Text = Format(CDate(txtDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '        End If
            '    Else
            '        txtSTDDate.Text = Format(tempdate, AppSettings("DateTimeFormatLOG"))
            '    End If
            '    ViewState("txtSTDDate") = txtSTDDate.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            'Catch ex As Exception
            '    If Not ViewState("txtSTDDate") Is Nothing Then
            '        txtSTDDate.Text = Format(CDate(ViewState("txtSTDDate")), AppSettings("DateTimeFormatLOG"))
            '    Else
            '        txtSTDDate.Text = Format(CDate(txtDate.Text.Trim), AppSettings("DateTimeFormatLOG"))
            '    End If
            'End Try
            If txtSTDDate.Text = "" Then
                mFligthDelayAndCancellation.StandardTimeOfDeparture = System.DBNull.Value
            Else
                mFligthDelayAndCancellation.StandardTimeOfDeparture = txtSTDDate.Text
            End If

            If Not mFligthDelayAndCancellation.StandardTimeOfDeparture Is System.DBNull.Value Then
                txtSTDDate.Text = Format(CDate(mFligthDelayAndCancellation.StandardTimeOfDeparture), AppSettings("DateFormat"))
                txtSTDTime.Text = Format(CDate(mFligthDelayAndCancellation.StandardTimeOfDeparture), AppSettings("TimeFormat"))
            Else
                txtSTDDate.Text = ""
            End If
            Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
            EnableDisableButton()
            'upnlDelayDate.Update()
            'upnlDelayDetails.Update()
            '# End
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mFligthDelayAndCancellation.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttch.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mFligthDelayAndCancellation.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mFligthDelayAndCancellation.ID)
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttch.Enabled = False
        IsAttachmentDeleted = True
        mFligthDelayAndCancellation.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mFligthDelayAndCancellation.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mFligthDelayAndCancellation.ID)
        End If
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
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mFligthDelayAndCancellation.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mFligthDelayAndCancellation.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mFligthDelayAndCancellation.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub txtAircraftReadyAtTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAircraftReadyAtTime.TextChanged
        If IsValidTime(txtAircraftReadyAtTime.Text.ToString.Trim) = False Then
            txtAircraftReadyAtTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = txtAircraftReadyAtDate.Text.ToString + " " + txtAircraftReadyAtTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mFligthDelayAndCancellation.AircraftReadyAt.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mFligthDelayAndCancellation.AircraftReadyAt = DateTime
                Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
            End If
        End If
    End Sub
    Private Sub hdnBtnSecCauseMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSecCauseMaster.Click
        mDCSecondaryCauseList = DCSecondaryCauseList.GetSecondaryCauseList()
        chkListSecondaryCause.DataSource = mDCSecondaryCauseList
        Session("mDCSecondaryCauseList") = mDCSecondaryCauseList
        chkListSecondaryCause.DataBind()
        upnlSecCauseMaster.Update()
    End Sub
    Private Sub hdnBtnCauseAndEffectMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnCauseAndEffectMaster.Click
        mDCCauseAndEffectList = DCCauseAndEffectList.GetCauseAndEffectList()
        ChklistCauseAndEffect.DataSource = mDCCauseAndEffectList
        Session("mDCCauseAndEffectList") = mDCCauseAndEffectList
        ChklistCauseAndEffect.DataBind()
        upnlCausenEffectMaster.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class