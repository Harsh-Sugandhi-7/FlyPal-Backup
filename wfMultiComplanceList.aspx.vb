'Created by :   Saylee
'Date       :   24-June-2009

Partial Class wfMultiComplanceList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtAsOnDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Enumeration"
    Enum Open
        CofAReport = 1
        RoutineInspectionReport = 2
        ModificationReport = 3
        DueReport = 4
    End Enum

    Enum StatusType
        AssemblyService = 1
        AssemblyInspection = 2
        AssemblyDirective = 3
        ComponentService = 4
        ComponentInspection = 5
        ComponentDirective = 6
    End Enum

#End Region

#Region " Variable Declaration "
    Dim mDueLimits As DueLimits

    Dim mPerDayLimits As PerDayLimits

    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList

    Dim mtmpMachineList As tmpMachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail

    Dim ObjMachineList As MachineList
    Dim ObjMachine As MachineInfo
    Dim ObjAssemblyStatus As AssemblyStatusInfo
    Dim ObjAssemblyStatusPeriod As AssemblyStatusPeriodInfo
    Dim ObjCompStatus As CompStatusInfo
    Dim ObjCompStatusPeriod As CompStatusPeriodInfo

    Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
    Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
    Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
    Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
    Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
    Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo
    Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
    Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
    Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
    Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo
    Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
    Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

    Private Flag As Int16
    Dim AOdate As String
    Dim AOnDate As String
    Dim Average As String
    Dim Aircraft As String
    Dim Periodcount As Integer
    Dim MachineName As String
    Dim AsonDate As String
    Dim Type As Integer = 1
    Dim AssemblyID As Guid
    Dim Count As Integer
    Dim mDueLimit As DueLimit
    Dim AvgMnths As Integer

    Private ATAChapter As String
    Private RegNo As String
    Private AssemblyType As String
    Private Model As String
    Private AssemblySerialNo As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private Note As String
    Private Description As String
    Private SerialNo As String
    Private EstimatedDate As String
    Private Freq1 As String
    Private Freq2 As String
    Private Freq3 As String

    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String

    Private SinceNew As String
    Private SinceNew1 As String
    Private SinceNew2 As String
    Private RemainingTime As String
    Private RemainingTime1 As String
    Private RemainingTime2 As String
    Private DueAsof As String
    Private DueAsof1 As String
    Private DueAsof2 As String
    Private DoneAt As String
    Private DoneAt1 As String
    Private DoneAt2 As String
    Private AssemblyModel As String
    Private MaintenanceEvent As String

    Private MinimumRemainingValue As Decimal
    Private AssemblyTypeID As Integer
    Private percent As String
    Private DueType As Integer

    Private mIsPreview As Boolean = False '11-Sep-2008

    'Added by Saylee on 12-Feb-2009
    Dim AircraftIndex As Integer
    Dim mAssemblyStatusList As AssemblyStatusList
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim TypeName As String
    Public mOpen As Open
    Dim mTypeListForCofA As TypeListForCofA
    Dim InspIndex As Integer
    Dim SerIndex As Integer
    Dim ModIndex As Integer
    Dim Extension As String
    Dim Extension1 As String
    Dim Extension2 As String
    Dim ExtensionDate As String
    Dim ApprovalRemark As String
    Dim RequiredManHours As String
    Dim Customer As String
    Dim Remark As String
    Dim Code As String
    Dim StatusMasterID As Guid
    Dim DocumentTypeForID As Integer
    Dim AssemblyDueAsof As String  'Added By DEVEN On 14/06/2008
    Dim AssemblyDueAsof1 As String 'Added By DEVEN On 14/06/2008
    Dim AssemblyDueAsof2 As String

    Public mStatusType As StatusType
    Private AssemblyStatusID As String
    Private ModelID As String
    Dim CompStatusID As Guid
    Dim StatusID As Guid
    Dim LogId As String
    Dim LogDate As String
    Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
    Dim tmpAssemblyStatusID As Guid
    Dim HourType As String

#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineListForCompliance"), MachineList)

        DueType = Session("DueType")
        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)

        AsonDate = Session("AsonDate")
        MachineName = Session("AircraftId")
        AssemblyName = Session("AssemblyId")
        AssemblyType = Session("AssemblyType")

        ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
        HourType = Session("HourType")
        AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")
        Aircraft = Session("Aircraft")
        LogId = Session("LogId")

    End Sub
    Private Sub SetSession()
        Session("mMachineListForCompliance") = mMachineList
        Session("mDueLimits") = mDueLimits

        Session("DueType") = DueType
        Session("AssemblyType") = AssemblyType
        'Added by Saylee on 12-Feb-2009
        Session("mAssemblyStatusList") = mAssemblyStatusList
        Session("HourType") = HourType
        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
        Session("Aircraft") = Aircraft

        Session("LogId") = LogId
        Session("DueType") = DueType
        Session("AsonDate") = AsonDate
        Session("AircraftId") = MachineName
        Session("HourType") = HourType
        Session("AssemblyId") = AssemblyName
    End Sub
    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfMultiComplanceList.aspx?" Then
            Session.Remove("mMachineListForCompliance")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Type")
            Session.Remove("AvgMnths")

            Session.Remove("mAssemblyStatusList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")
            Session.Remove("LogId")
            Session.Remove("OpenFindNowSelectLogForm")
            Session.Remove("AssemblyStatusPeriodList")
            Session.Remove("HourType")
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfMultiComplanceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfMultiComplanceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub Controltovisibility()

    End Sub
    'Comply Assembly Service
    Private Sub ComplyAssemblyService(ByVal ReportMaintenanceDetail As ReportMaintenanceDetail, ByVal Remark As String)

        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mBoardInfo As AircraftInformationBoard.BoardInfo

        Dim clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        ''clnAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(ReportMaintenanceDetail.StatusID, ReportMaintenanceDetail.AssemblyStatusID, CType(Session("HourType"), Integer))
        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then  'If CType(Session("OpenSelectLogForm"), Boolean) = True Then 'Val(Request.QueryString("Type")) = -1 Then

            'LogId = Session("LogId")
            LogId = Session("LogId")
            Session("LogId") = LogId

            LogDate = Session("LogDate")
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevAssemblyMonitorServiceStatus.ModelMonitorService.ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, New Guid(LogId), mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, CType(Session("HourType"), Integer))
            '  Session.Remove("FromLog")
        Else
            mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevAssemblyMonitorServiceStatus.ModelMonitorService.ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, CType(Session("HourType"), Integer))

        End If

        mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
        mAssemblyMonitorServiceStatus.DoneRemark = Remark
        mAssemblyMonitorServiceStatus.DoneWONo = Trim(txtWorkOrderNo.Text)

        Session("From") = 0 'New record

        clnAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
        Session("mBoardInfo") = mBoardInfo

        If mAssemblyMonitorServiceStatus.IsValid Then
            If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Service Status.Assembly Service Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If

            Try
                mAssemblyMonitorServiceStatus.ApplyEdit()
                mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
                Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod
                Dim DueOnValue As String
                If (mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorServiceStatus.IsApplicable = False) Then
                    DueOnValue = ""
                Else
                    For Each mAssemblyMonitorServiceStatusPeriod In mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                        If mAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                            DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                        Else
                            DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueTextFormatted
                        End If
                    Next

                End If

                mBoardInfo = Session("mBoardInfo")
                If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
                    mBoardInfo.MonitorID = mAssemblyMonitorServiceStatus.ID
                    mBoardInfo.DueOnValue = DueOnValue
                    mBoardInfo.ApplyEdit()
                    mBoardInfo.Save()
                    Session("mBoardInfo") = mBoardInfo
                    Session("mAircraftInformationBoardList") = Nothing
                End If

                Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                'MarkLog(Util.Action.Save, "ComplyAssemblyMonitorServiceStatus", "", Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
            Catch ex As SqlException
                Session("mAssemblyMonitorServiceStatus") = clnAssemblyMonitorServiceStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                End If
            Finally
                clnAssemblyMonitorServiceStatus = Nothing
            End Try
        End If
    End Sub
    'Comply Assembly Inspection
    Private Sub ComplyAssemblyInspection(ByVal ReportMaintenanceDetail As ReportMaintenanceDetail, ByVal Remark As String)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim mBoardInfo As AircraftInformationBoard.BoardInfo

        Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        ''clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)

        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(ReportMaintenanceDetail.StatusID, ReportMaintenanceDetail.AssemblyStatusID, CType(Session("HourType"), Integer))
        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then  ' If CType(Session("OpenSelectLogForm"), Boolean) = True Then 'Val(Request.QueryString("Type")) = -1 Then
            'LogId = Session("LogId")
            LogId = Session("LogId")
            Session("LogId") = LogId

            LogDate = Session("LogDate")
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, New Guid(LogId), mPrevAssemblyMonitorInspStatus.DoneOn.ToString, CType(Session("HourType"), Integer))

            ' Session.Remove("FromLog")

        Else
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, CType(Session("HourType"), Integer))
        End If
        mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
        mAssemblyMonitorInspStatus.DoneRemark = Remark
        mAssemblyMonitorInspStatus.DoneWONo = Trim(txtWorkOrderNo.Text)

        clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)

        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
        Session("mBoardInfo") = mBoardInfo

        If mAssemblyMonitorInspStatus.IsValid Then
            If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Insp Status.Assembly Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If

            Try
                mAssemblyMonitorInspStatus.ApplyEdit()
                mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
                Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod
                Dim DueOnValue As String

                If (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And Not mAssemblyMonitorInspStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorInspStatus.IsApplicable = False) Then
                    DueOnValue = ""
                Else
                    For Each mAssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                        If mAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                            DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                        Else
                            DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueTextFormatted
                        End If
                    Next
                End If

                mBoardInfo = Session("mBoardInfo")
                If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
                    mBoardInfo.MonitorID = mAssemblyMonitorInspStatus.ID
                    mBoardInfo.DueOnValue = DueOnValue
                    mBoardInfo.ApplyEdit()
                    mBoardInfo.Save()
                    Session("mBoardInfo") = mBoardInfo
                    Session("mAircraftInformationBoardList") = Nothing
                End If

                Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                'MarkLog(Util.Action.Save, "ComplyAssemblyMonitorInspStatus", "", Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID)
            Catch ex As SqlException
                Session("mAssemblyMonitorInspStatus") = clnAssemblyMonitorInspStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                End If
            Finally
                clnAssemblyMonitorInspStatus = Nothing
            End Try
        End If
    End Sub
    'Comply Assembly Directive
    Private Sub ComplyAssemblyDirective(ByVal ReportMaintenanceDetail As ReportMaintenanceDetail, ByVal Remark As String)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        Dim mBoardInfo As AircraftInformationBoard.BoardInfo

        Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus
        ''clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)

        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(ReportMaintenanceDetail.StatusID, ReportMaintenanceDetail.AssemblyStatusID, CType(Session("HourType"), Integer))
        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then  'If CType(Session("OpenSelectLogForm"), Boolean) = True Then 'Val(Request.QueryString("Type")) = -1 Then
            'LogId = Session("LogId")
            LogId = Session("LogId")
            Session("LogId") = LogId

            LogDate = Session("LogDate")
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDate, mPrevAssemblyMonitorModStatus.ModelMonitorMod.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, New Guid(LogId), mPrevAssemblyMonitorModStatus.DoneOn.ToString, CType(Session("HourType"), Integer))

            ' Session.Remove("FromLog")
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDate, mPrevAssemblyMonitorModStatus.ModelMonitorMod.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, CType(Session("HourType"), Integer))
        End If

        mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
        mAssemblyMonitorModStatus.DoneRemark = Remark
        mAssemblyMonitorModStatus.DoneWONo = Trim(txtWorkOrderNo.Text)

        clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)

        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
        Session("mBoardInfo") = mBoardInfo

        If mAssemblyMonitorModStatus.IsValid Then
            If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Mod Status.Assembly Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If

            Try
                mAssemblyMonitorModStatus.ApplyEdit()
                mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
                Dim mAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriod
                Dim DueOnValue As String

                If (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorModStatus.IsApplicable = False) Then
                    DueOnValue = ""
                Else
                    For Each mAssemblyMonitorModStatusPeriod In mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                        If mAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                            DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                        Else
                            DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueTextFormatted
                        End If
                    Next
                End If
                mBoardInfo = Session("mBoardInfo")
                If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
                    mBoardInfo.MonitorID = mAssemblyMonitorModStatus.ID
                    mBoardInfo.DueOnValue = DueOnValue
                    mBoardInfo.ApplyEdit()
                    mBoardInfo.Save()
                    Session("mBoardInfo") = mBoardInfo
                    Session("mAircraftInformationBoardList") = Nothing
                End If

                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                'MarkLog(Util.Action.Save, "ComplyAssemblyMonitorModStatus", "", Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID)
            Catch ex As SqlException
                Session("mAssemblyMonitorModStatus") = clnAssemblyMonitorModStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                End If
            Finally
                clnAssemblyMonitorModStatus = Nothing
            End Try
        End If
    End Sub

    'Comply Component Service
    Private Sub ComplyComponentService(ByVal ReportMaintenanceDetail As ReportMaintenanceDetail, ByVal Remark As String)

        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        
        Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus

        'Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ReportMaintenanceDetail.StatusMasterID, New Guid(AssemblyStatusID), mMachineList(New Guid(MachineName)).HourType)
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ReportMaintenanceDetail.StatusID, ReportMaintenanceDetail.AssemblyStatusID, ReportMaintenanceDetail.CompStatusID, CType(Session("HourType"), Integer))
        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then 'If CType(Session("OpenSelectLogForm"), Boolean) = True Then 'Val(Request.QueryString("Type")) = -1 Then
            'LogId = Session("LogId")
            LogId = Session("LogId")
            Session("LogId") = LogId

            LogDate = Session("LogDate")
            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, New Guid(LogId), mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString, CType(Session("HourType"), Integer))

            ' Session.Remove("FromLog")
        Else
            mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString, CType(Session("HourType"), Integer))
        End If
        mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
        mCompMonitorServiceStatus.DoneRemark = Remark
        mCompMonitorServiceStatus.DoneWONo = Trim(txtWorkOrderNo.Text)

        clnCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)

        If mCompMonitorServiceStatus.IsValid Then
            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Comp Service Status.Comp Service Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If

            Try
                mCompMonitorServiceStatus.ApplyEdit()
                mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
               
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                ' MarkLog(Util.Action.Save, "ComplyCompMonitorServiceStatus", "", Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
            Catch ex As SqlException
                Session("mCompMonitorServiceStatus") = clnCompMonitorServiceStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                End If
            Finally
                clnCompMonitorServiceStatus = Nothing
            End Try
        End If
    End Sub
    'Comply Component Inspection
    Private Sub ComplyComponentInspection(ByVal ReportMaintenanceDetail As ReportMaintenanceDetail, ByVal Remark As String)

        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        
        Dim clnCompMonitorInspStatus As CompMonitorInspStatus

        'Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(ReportMaintenanceDetail.StatusMasterID, New Guid(AssemblyStatusID), mMachineList(New Guid(MachineName)).HourType)
        Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(ReportMaintenanceDetail.StatusID, ReportMaintenanceDetail.AssemblyStatusID, ReportMaintenanceDetail.CompStatusID, CType(Session("HourType"), Integer))
        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then 'If CType(Session("OpenSelectLogForm"), Boolean) = True Then 'Val(Request.QueryString("Type")) = -1 Then
            'LogId = Session("LogId")
            LogId = Session("LogId")
            Session("LogId") = LogId

            LogDate = Session("LogDate")
            mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, New Guid(LogId), mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, CType(Session("HourType"), Integer))

            ' Session.Remove("FromLog")
        Else
            'mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, txtAsOnDate.Value.ToString, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mPrevCompMonitorInspStatus.ID.ToString)
            mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, CType(Session("HourType"), Integer))
        End If
        mCompMonitorInspStatus.RequiredManHours = mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours
        mCompMonitorInspStatus.DoneRemark = Remark
        mCompMonitorInspStatus.DoneWONo = Trim(txtWorkOrderNo.Text)

        clnCompMonitorInspStatus = CType(mCompMonitorInspStatus.Clone, CompMonitorInspStatus)

        If mCompMonitorInspStatus.IsValid Then
            If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Comp Insp Status.Comp Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If

            Try
                mCompMonitorInspStatus.ApplyEdit()
                mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
                
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                'MarkLog(Util.Action.Save, "ComplyCompMonitorInspStatus", "", Util.ErrorType.NoError, mCompMonitorInspStatus.ID)
            Catch ex As SqlException
                Session("mCompMonitorInspStatus") = clnCompMonitorInspStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                End If
            Finally
                clnCompMonitorInspStatus = Nothing
            End Try
        End If
    End Sub
    'Comply Component Directive
    Private Sub ComplyComponentDirective(ByVal ReportMaintenanceDetail As ReportMaintenanceDetail, ByVal Remark As String)

        Dim mCompMonitorModStatus As CompMonitorModStatus
       
        Dim clnCompMonitorModStatus As CompMonitorModStatus

        'Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(ReportMaintenanceDetail.StatusMasterID, New Guid(AssemblyStatusID), mMachineList(New Guid(MachineName)).HourType)
        Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(ReportMaintenanceDetail.StatusID, ReportMaintenanceDetail.AssemblyStatusID, ReportMaintenanceDetail.CompStatusID, CType(Session("HourType"), Integer))

        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then 'If CType(Session("OpenSelectLogForm"), Boolean) = True Then 'Val(Request.QueryString("Type")) = -1 Then
            'LogId = Session("LogId")
            LogId = Session("LogId")
            Session("LogId") = LogId

            LogDate = Session("LogDate")
            mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, New Guid(LogId), mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, CType(Session("HourType"), Integer))

            ' Session.Remove("FromLog")
        Else
            mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, CType(Session("HourType"), Integer))
        End If
        mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
        mCompMonitorModStatus.DoneRemark = Remark
        mCompMonitorModStatus.DoneWONo = Trim(txtWorkOrderNo.Text)

        clnCompMonitorModStatus = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)


        If mCompMonitorModStatus.IsValid Then
            If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Comp Mod Status.Comp Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly)
                msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If

            Try
                mCompMonitorModStatus.ApplyEdit()
                mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                'MarkLog(Util.Action.Save, "ComplyCompMonitorModStatus", "", Util.ErrorType.NoError, mCompMonitorModStatus.ID)
            Catch ex As SqlException
                Session("mCompMonitorModStatus") = clnCompMonitorModStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 8145 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                ElseIf ex.Number = 2627 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    msg1.Show()
                ElseIf ex.Number = 547 Then
                    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    msg1.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
                    msg1.Show()
                End If
            Finally
                clnCompMonitorModStatus = Nothing
            End Try
        End If
    End Sub
    ''Private Sub SetLog()
    ''    'If Val(Request.QueryString("Type")) = -1 Then
    ''    If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then
    ''        'LogId = New Guid(Request.QueryString("LogId"))
    ''        'LogDate = Request.QueryString("LogDate")

    ''        ''Session("LogId") = LogId
    ''        ''Session("LogDate") = LogDate

    ''        LogId = New Guid(CType(Session("LogId"), String))
    ''        Session("LogId") = CType(Session("LogId"), String)

    ''        Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId.ToString).Item(0), MachineInfo).AssemblyStatusList
    ''        AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
    ''        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
    ''        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
    ''        dgDoneOnValue.DataBind()

    ''        tmpAssemblyStatusList = Nothing

    ''    Else

    ''    End If
    ''End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)


    End Sub
    Public Sub DataFieldBind()
        If Not ReportMaintenanceDetails Is Nothing Then
            dgDueJob.DataSource = ReportMaintenanceDetails
            dgDueJob.DataBind()

            If Not Session("LogId") Is Nothing Then
                Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId.ToString).Item(0), MachineInfo).AssemblyStatusList
                AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
                Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                tmpAssemblyStatusList = Nothing
            End If

            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

            lblResult.Text = ReportMaintenanceDetails.Count & " Record(s) found."

            txtAircraft.Text = Aircraft
            txtAssembly.Text = AssemblyType

            txtAsOnDate.Enabled = False
            txtAsOnDate.Value = AsonDate

            btnSaveTop.Visible = ReportMaintenanceDetails.Count > 10
            btnCloseTop.Visible = ReportMaintenanceDetails.Count > 10
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '' ClearAll()
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            DueType = 1
            'DueType = Request.QueryString("DueType")
            If AsonDate Is Nothing Then AsonDate = Request.QueryString("DoneOn")
            If MachineName Is Nothing Then MachineName = Request.QueryString("MachineId")
            If HourType Is Nothing Then HourType = Request.QueryString("HourType")
            If AssemblyName Is Nothing Then AssemblyName = Request.QueryString("AssemblyID")
            SetFocus(txtWorkOrderNo)
            txtAsOnDate.Enabled = False
            txtAsOnDate.Value = AsonDate
            Session("mLogList") = Nothing
            DataFieldBind()
            Controltovisibility()
            ''SetLog()
        End If
        SetSession()
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        mMachineList = Nothing
        mAssemblyStatusList = Nothing
        '=============================
        ReportMaintenanceDetails = Nothing
        ''Session.Remove("LogId")
        'Session.Remove("OpenFindNowSelectLogForm")
        Response.Redirect(Request.QueryString("BackPage")) '' & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
        If (Not User.IsInRole("MultiComplianceNew") And Not User.IsInRole("MultiComplianceEdit")) Then
            'MarkLog(Util.Action.Edit, "MachineCertificateRenewList", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMultiComplanceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            msg.Show()
            Exit Sub
        End If

        Dim i As Integer
        Dim IsNotSelected As Boolean = True
        Dim chkSelect As CheckBox
        Dim txtRemark As TextBox
        ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
        For i = 0 To Me.dgDueJob.Items.Count - 1 'ReportMaintenanceDetails.Count - 1
            chkSelect = CType(Me.dgDueJob.Items(i).FindControl("chkSelect"), CheckBox)
            txtRemark = CType(Me.dgDueJob.Items(i).FindControl("txtRemark"), TextBox)
            If chkSelect.Checked = True Then
                IsNotSelected = False
                Select Case ReportMaintenanceDetails.Item(i).TypeID
                    Case StatusType.AssemblyService
                        ComplyAssemblyService(ReportMaintenanceDetails.Item(i), txtRemark.Text)

                    Case StatusType.AssemblyInspection
                        ComplyAssemblyInspection(ReportMaintenanceDetails.Item(i), txtRemark.Text)

                    Case StatusType.AssemblyDirective
                        ComplyAssemblyDirective(ReportMaintenanceDetails.Item(i), txtRemark.Text)

                    Case StatusType.ComponentService
                        ComplyComponentService(ReportMaintenanceDetails.Item(i), txtRemark.Text)

                    Case StatusType.ComponentInspection
                        ComplyComponentInspection(ReportMaintenanceDetails.Item(i), txtRemark.Text)

                    Case StatusType.ComponentDirective
                        ComplyComponentDirective(ReportMaintenanceDetails.Item(i), txtRemark.Text)
                End Select
            End If
        Next

        If IsNotSelected = True Then
            'give message box for selecting atleast one record
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Select atleast one record from List", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfMultiComplanceList.aspx?BackPage=" & Request.QueryString("BackPage")
            Session("sender") = "Authorization"
            msg.Show()
            Exit Sub
        Else
            Session.Remove("FromLog")
            ''  Session.Remove("LogId")
            '  Session.Remove("OpenFindNowSelectLogForm")
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub

    ''Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
    ''    If IsValid = True Then
    ''        SetSession()
    ''        Session("OpenSelectLogForm") = True

    ''        Dim mtmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList

    ''        Dim str As String
    ''        'str = "<script language='javascript'>openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=Index.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(txtAsOnDate.Value.ToString = "", Today.Date.ToShortDateString, txtAsOnDate.Value)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & AssemblyStatusID.ToString & "&AssemblyID=" & AssemblyName.ToString & "'); </script>"
    ''        str = "<script language='javascript'>openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=wfMultiComplanceList.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & mtmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & mtmpAssemblyStatusList(0).AssemblyID.ToString & "'); </script>"
    ''         ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", str)

    ''    End If
    ''End Sub
    ''New addition by Saylee on 06-July-09 for Sorting Order
    ''Private Sub dgDueJob_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgDueJob.SortCommand
    ''    ReportMaintenanceDetails.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    ''    ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
    ''    dgDueJob.DataSource = ReportMaintenanceDetails
    ''    DataBind()
    ''End Sub
#End Region

End Class
