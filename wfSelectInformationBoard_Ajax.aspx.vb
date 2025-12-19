'AJAX Conversion By Vikrant On 30-Jun-2015

Public Class wfSelectInformationBoard_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mMachine As Machine
    Public mTmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
    Public mTmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
    Public mTmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList
    Public mRenewMachineCertificateList As MachineCertificateList

    Public mBoardInfo As AircraftInformationBoard.BoardInfo
    Public mBoardInfoList As AircraftInformationBoard.BoardInfoList

    Dim mBoardTypeID As Integer

#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mMachine = Session("mMachine")
        mBoardTypeID = Session("BoardType")
        mBoardInfoList = Session("mBoardInfoList")

        mTmpComplyAssemblyMonitorServiceStatusList = Session("mTmpComplyAssemblyMonitorServiceStatusList")
        mTmpComplyAssemblyMonitorInspStatusList = Session("mTmpComplyAssemblyMonitorInspStatusList")
        mTmpComplyAssemblyMonitorModStatusList = Session("mTmpComplyAssemblyMonitorModStatusList")
        mRenewMachineCertificateList = Session("mRenewMachineCertificateList")
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mTmpComplyAssemblyMonitorServiceStatusList")
        Session.Remove("mTmpComplyAssemblyMonitorInspStatusList")
        Session.Remove("mTmpComplyAssemblyMonitorModStatusList")
        Session.Remove("mRenewMachineCertificateList")
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Session("sender") = ""
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub

    Private Sub SelectServiceRecord(Index As Int32)

        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.
                                                                                    GetAssemblyMonitorServiceStatus(mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyMonitorServiceStatusID,
                                                                                                                    mTmpComplyAssemblyMonitorServiceStatusList.Item(Index).AssemblyStatusID,
                                                                                                                    mMachine.HourType)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorServiceStatusList(Index).AssemblyStatusID)

        mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.
                                            GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID,
                                                                                           mPrevAssemblyMonitorServiceStatus.AssemblyStatusID,
                                                                                           mPrevAssemblyMonitorServiceStatus.DoneOn.ToString,
                                                                                           mMachine.HourType)

        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        Session("FromSelectInfo") = "FromSelectInfo"
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        'Added by Saylee on 29-June-2009 to show DueOnValue blank for One time record
        If mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is DBNull.Value Then

            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID,
                                                                              mBoardTypeID,
                                                                              mAssemblyMonitorServiceStatus.ID,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo + "-" +
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType + "-" +
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).Description,
                                                                              "",
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelID.ToString,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceID.ToString,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneRemark)

        Else

            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID,
                                                                              mBoardTypeID,
                                                                              mAssemblyMonitorServiceStatus.ID,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceInfo + "-" +
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).MonitorType + "-" +
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).Description,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).DueOnValueFormatted,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelID.ToString,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).ModelMonitorServiceID.ToString,
                                                                              mTmpComplyAssemblyMonitorServiceStatusList(Index).DoneRemark)

        End If

        '================================
        Session("mBoardInfo") = mBoardInfo
        If Not mBoardInfoList.Contains(mBoardInfo.MonitorID, "") Then
            mBoardInfoList.Add(mBoardInfo)
        End If
        Session("mBoardInfoList") = mBoardInfoList

    End Sub

    Private Sub SelectInspRecord(Index As Int32)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).MachineID)
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyMonitorInspStatusID, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)

        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("FromSelectInfo") = "FromSelectInfo" 'Edit record
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorInspStatusList(Index).AssemblyStatusID)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        ' Commented and Added by Saylee on 29-June-2009 to show DueOnValue blank for One time record
        'mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mAssemblyMonitorInspStatus.ID, mTmpComplyAssemblyMonitorInspStatusList(Index).ModelMonitorInspInfo + "-" + mTmpComplyAssemblyMonitorInspStatusList(Index).MonitorType + "-" + mTmpComplyAssemblyMonitorInspStatusList(Index).Description, mTmpComplyAssemblyMonitorInspStatusList(Index).DueOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelID.ToString, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelMonitorInspID.ToString)
        If mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And Not mAssemblyMonitorInspStatus.DoneOn Is DBNull.Value Then
            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mAssemblyMonitorInspStatus.ID, mTmpComplyAssemblyMonitorInspStatusList(Index).ModelMonitorInspInfo + "-" + mTmpComplyAssemblyMonitorInspStatusList(Index).MonitorType + "-" + mTmpComplyAssemblyMonitorInspStatusList(Index).Description, "", mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelID.ToString, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelMonitorInspID.ToString, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).DoneRemark)
        Else
            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mAssemblyMonitorInspStatus.ID, mTmpComplyAssemblyMonitorInspStatusList(Index).ModelMonitorInspInfo + "-" + mTmpComplyAssemblyMonitorInspStatusList(Index).MonitorType + "-" + mTmpComplyAssemblyMonitorInspStatusList(Index).Description, mTmpComplyAssemblyMonitorInspStatusList(Index).DueOnValueFormatted, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelID.ToString, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).ModelMonitorInspID.ToString, mTmpComplyAssemblyMonitorInspStatusList.Item(Index).DoneRemark)
        End If
        '===============================

        Session("mBoardInfo") = mBoardInfo
        If Not mBoardInfoList.Contains(mBoardInfo.MonitorID, "") Then
            mBoardInfoList.Add(mBoardInfo)
        End If
        Session("mBoardInfoList") = mBoardInfoList

        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub

    Private Sub SelectModRecord(Index As Int32)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        mMachine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyMonitorModStatusID, mTmpComplyAssemblyMonitorModStatusList.Item(Index).AssemblyStatusID, mMachine.HourType)

        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
        Session("FromSelectInfo") = "FromSelectInfo" 'Edit record
        ''
        ' Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorModStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mTmpComplyAssemblyMonitorModStatusList(Index).AssemblyStatusID)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        'Commented and Added by Saylee on 29-June-2009 to show DueOnValue blank for One time record
        ' mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mAssemblyMonitorModStatus.ID, mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModInfo + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).MonitorType + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).Description, mTmpComplyAssemblyMonitorModStatusList(Index).DueOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(Index).ModelID.ToString, mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModID.ToString)
        If mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is DBNull.Value Then
            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mAssemblyMonitorModStatus.ID, mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModInfo + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).MonitorType + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).Description + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).ModNumber, "", mTmpComplyAssemblyMonitorModStatusList(Index).ModelID.ToString, mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModID.ToString, mTmpComplyAssemblyMonitorModStatusList(Index).DoneRemark)
        Else
            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mAssemblyMonitorModStatus.ID, mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModInfo + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).MonitorType + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).Description + "-" + mTmpComplyAssemblyMonitorModStatusList(Index).ModNumber, mTmpComplyAssemblyMonitorModStatusList(Index).DueOnValueFormatted, mTmpComplyAssemblyMonitorModStatusList(Index).ModelID.ToString, mTmpComplyAssemblyMonitorModStatusList(Index).ModelMonitorModID.ToString, mTmpComplyAssemblyMonitorModStatusList(Index).DoneRemark)
        End If
        '================================

        Session("mBoardInfo") = mBoardInfo
        If Not mBoardInfoList.Contains(mBoardInfo.MonitorID, "") Then
            mBoardInfoList.Add(mBoardInfo)
        End If


        Session("mBoardInfoList") = mBoardInfoList

        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

    End Sub

    Private Sub SelectCertificateRecord(Index As Int32)
        Dim mRenewMachineCertificate As MachineCertificate
        Dim mMachineCertificate As MachineCertificate
        mRenewMachineCertificate = MachineCertificate.GetRenewalMachineCertificate(mRenewMachineCertificateList.Item(Index).MachineID, mRenewMachineCertificateList.Item(Index).ID)

        If Not mRenewMachineCertificate.ReferenceID.Equals(Guid.Empty) Then
            mMachineCertificate = MachineCertificate.GetMachineCertificate(mRenewMachineCertificate.MachineID, mRenewMachineCertificate.ReferenceID)
        Else
            mMachineCertificate = MachineCertificate.GetMachineCertificate(mRenewMachineCertificate.MachineID, mRenewMachineCertificateList(Index).ID)
        End If

        Session("mRenewMachineCertificate") = mRenewMachineCertificate
        Session("mMachineCertificate") = mMachineCertificate
        Session("FromSelectInfo") = "FromSelectInfo" 'Edit record

        'mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mRenewMachineCertificateList(Index).ID, mRenewMachineCertificateList(Index).CertificateName + "-" + mRenewMachineCertificateList(Index).CertificateNo, IIf(IsDBNull(mRenewMachineCertificateList(Index).ExpiryDateFormatted), "", mRenewMachineCertificateList(Index).ExpiryDateFormatted), mMachine.AssemblyStatus.Assembly.ModelID.ToString, , mRenewMachineCertificateList(Index).Remark, mRenewMachineCertificateList(Index).CertificateName)

        If AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then
            'here Certificate No is removed while adding
            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mRenewMachineCertificateList(Index).ID, mRenewMachineCertificateList(Index).CertificateName, IIf(IsDBNull(mRenewMachineCertificateList(Index).ExpiryDateFormatted), "", mRenewMachineCertificateList(Index).ExpiryDateFormatted), mMachine.AssemblyStatus.Assembly.ModelID.ToString, , mRenewMachineCertificateList(Index).Remark, mRenewMachineCertificateList(Index).CertificateName)
        Else
            'Certificate with Certificate No
            mBoardInfo = AircraftInformationBoard.BoardInfo.NewChildBoardInfo(mMachine.ID, mBoardTypeID, mRenewMachineCertificateList(Index).ID, mRenewMachineCertificateList(Index).CertificateName + "-" + mRenewMachineCertificateList(Index).CertificateNo, IIf(IsDBNull(mRenewMachineCertificateList(Index).ExpiryDateFormatted), "", mRenewMachineCertificateList(Index).ExpiryDateFormatted), mMachine.AssemblyStatus.Assembly.ModelID.ToString, , mRenewMachineCertificateList(Index).Remark, mRenewMachineCertificateList(Index).CertificateName)
        End If
        Session("mBoardInfo") = mBoardInfo
        If Not mBoardInfoList.Contains(mBoardInfo.MonitorID, "") Then
            mBoardInfoList.Add(mBoardInfo)
        End If
        Session("mBoardInfoList") = mBoardInfoList
    End Sub

#End Region

#Region " Data Bindings "

    Private Sub DataFieldBind()

        Select Case mBoardTypeID

            Case 1  'Service

                mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceListForAircraftBoardInfo(Today.ToShortDateString, mMachine.ID.ToString, "", "", , , , , , , , 1)
                dgSelectInformationList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                dgSelectInformationList.DataBind()
                Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
                lblSelectInformation.Text = "List of Services: " & mTmpComplyAssemblyMonitorServiceStatusList.Count & " Record(s) found "

                btnDoneTop.Visible = (mTmpComplyAssemblyMonitorServiceStatusList.Count > 25)
                btnCloseTop.Visible = (mTmpComplyAssemblyMonitorServiceStatusList.Count > 25)

            Case 2  'Inspection

                mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspListForAircraftBoardInfo(Today.ToShortDateString, mMachine.ID.ToString, "", "", , , , , , , , 1)
                dgSelectInformationList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
                dgSelectInformationList.DataBind()
                Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
                lblSelectInformation.Text = "List of Inspections: " & mTmpComplyAssemblyMonitorInspStatusList.Count & " Record(s) found "

                btnDoneTop.Visible = (mTmpComplyAssemblyMonitorInspStatusList.Count > 25)
                btnCloseTop.Visible = (mTmpComplyAssemblyMonitorInspStatusList.Count > 25)

            Case 3  'Directive

                mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModListForAircraftBoardInfo(Today.ToShortDateString, mMachine.ID.ToString, "", "", , , , , , , , 1)
                dgSelectInformationList.DataSource = mTmpComplyAssemblyMonitorModStatusList
                dgSelectInformationList.DataBind()
                Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
                lblSelectInformation.Text = "List of Directives: " & mTmpComplyAssemblyMonitorModStatusList.Count & " Record(s) found "

                btnDoneTop.Visible = (mTmpComplyAssemblyMonitorModStatusList.Count > 25)
                btnCloseTop.Visible = (mTmpComplyAssemblyMonitorModStatusList.Count > 25)

            Case 4
                dgCertificateList.Visible = True
                dgSelectInformationList.Visible = False
                mRenewMachineCertificateList = MachineCertificateList.GetMachineCertificateListForAircraftBoardInfo(mMachine.ID, Today.Date.ToString)
                dgCertificateList.DataSource = mRenewMachineCertificateList
                dgCertificateList.DataBind()
                Session("mRenewMachineCertificateList") = mRenewMachineCertificateList
                lblSelectInformation.Text = "List of Certificates: " & mRenewMachineCertificateList.Count & " Record(s) found "

                btnDoneTop.Visible = (mRenewMachineCertificateList.Count > 25)
                btnCloseTop.Visible = (mRenewMachineCertificateList.Count > 25)

        End Select

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here

        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If

    End Sub

    Private Sub DoneSelecting(sender As Object, e As EventArgs) Handles btnDoneTop.Click, btnDoneBottom.Click

        Dim mIsSelect As Boolean = False

        Select Case mBoardTypeID
            Case 1  'Service

                For i As Integer = 0 To mTmpComplyAssemblyMonitorServiceStatusList.Count - 1

                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If

                Next

                If mIsSelect = True Then

                    For i As Integer = 0 To mTmpComplyAssemblyMonitorServiceStatusList.Count - 1

                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked Then
                            SelectServiceRecord(i)
                        End If

                    Next
                    Session("mBoardInfoList") = mBoardInfoList

                    Dim mOpenAs As String = Request.QueryString("Type")
                    If mOpenAs IsNot Nothing AndAlso mOpenAs = "pup" Then

                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "onclose",
                                                            "CallParentCallback();",
                                                            True)
                        Exit Sub

                    End If

                Else

                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne,
                                    MSGBox.Message_text.SelectAtleastOne,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Session("mIsNotSelect") = "NotSelect"

                End If

            Case 2  'Inspection

                For i As Integer = 0 To mTmpComplyAssemblyMonitorInspStatusList.Count - 1

                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If

                Next

                If mIsSelect = True Then

                    For i As Integer = 0 To mTmpComplyAssemblyMonitorInspStatusList.Count - 1

                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            SelectInspRecord(i)
                        End If

                    Next
                    Session("mBoardInfoList") = mBoardInfoList

                    Dim mOpenAs As String = Request.QueryString("Type")
                    If mOpenAs IsNot Nothing AndAlso mOpenAs = "pup" Then

                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "onclose",
                                                            "CallParentCallback();",
                                                            True)
                        Exit Sub

                    End If

                Else

                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne,
                                    MSGBox.Message_text.SelectAtleastOne,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Session("mIsNotSelect") = "NotSelect"

                End If
            Case 3  'Directive

                For i As Integer = 0 To mTmpComplyAssemblyMonitorModStatusList.Count - 1

                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If

                Next

                If mIsSelect = True Then

                    For i As Integer = 0 To mTmpComplyAssemblyMonitorModStatusList.Count - 1

                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            SelectModRecord(i)
                        End If

                    Next

                    Session("mBoardInfoList") = mBoardInfoList

                    Dim mOpenAs As String = Request.QueryString("Type")
                    If mOpenAs IsNot Nothing AndAlso mOpenAs = "pup" Then

                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "onclose",
                                                            "CallParentCallback();",
                                                            True)
                        Exit Sub

                    End If

                Else

                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne,
                                    MSGBox.Message_text.SelectAtleastOne,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Session("mIsNotSelect") = "NotSelect"

                End If

            Case 4 'Certificate

                For i As Integer = 0 To mRenewMachineCertificateList.Count - 1

                    If CType(dgCertificateList.Rows(i).FindControl("chkSelectCertificate"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If

                Next

                If mIsSelect = True Then

                    For i As Integer = 0 To mRenewMachineCertificateList.Count - 1
                        If CType(dgCertificateList.Rows(i).FindControl("chkSelectCertificate"), CheckBox).Checked Then
                            SelectCertificateRecord(i)
                        End If
                    Next

                    Session("mBoardInfoList") = mBoardInfoList
                    Dim mOpenAs As String = Request.QueryString("Type")

                    If mOpenAs IsNot Nothing AndAlso mOpenAs = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, [GetType], "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If

                Else

                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne,
                                    MSGBox.Message_text.SelectAtleastOne,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Session("mIsNotSelect") = "NotSelect"

                End If

        End Select

    End Sub

    Private Sub CloseModal(sender As Object, e As EventArgs) Handles btnCloseTop.Click, btnCloseBottom.Click

        Session("mBoardInfoList") = mBoardInfoList
        RemoveSession()
        Dim mOpenAs As String = Request.QueryString("Type")

        If mOpenAs IsNot Nothing AndAlso mOpenAs = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "onclose",
                                                "CallParentCallback();",
                                                True)
            Exit Sub

        End If

    End Sub

    Private Sub GV_SelectInformationList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgSelectInformationList.Sorting

        Select Case mBoardTypeID
            Case 1  'Service

                mTmpComplyAssemblyMonitorServiceStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
                dgSelectInformationList.DataSource = mTmpComplyAssemblyMonitorServiceStatusList
                dgSelectInformationList.DataBind()

            Case 2  'Inspection

                mTmpComplyAssemblyMonitorInspStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
                dgSelectInformationList.DataSource = mTmpComplyAssemblyMonitorInspStatusList
                dgSelectInformationList.DataBind()

            Case 3  'Directive

                mTmpComplyAssemblyMonitorModStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
                dgSelectInformationList.DataSource = mTmpComplyAssemblyMonitorModStatusList
                dgSelectInformationList.DataBind()
        End Select

    End Sub

    Private Sub GV_CertificateList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgCertificateList.Sorting

        mRenewMachineCertificateList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRenewMachineCertificateList") = mRenewMachineCertificateList
        dgCertificateList.DataSource = mRenewMachineCertificateList
        dgCertificateList.DataBind()

    End Sub

#End Region

End Class