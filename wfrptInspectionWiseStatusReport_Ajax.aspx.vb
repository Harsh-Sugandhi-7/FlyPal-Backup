Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Public Class wfrptInspectionWiseStatusReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private checkedIds As String()
    Public mModelMonitorInspList As ModelMonitorInspList
    Public ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Public mInspectionTypeList As ModelMonitorInspTypeList
    Public mMachineList As MachineList
    Dim AssemblyID As Guid
    Private ATAChapter As String
    Private AssemblyType As String
    Private AssemblySerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private MonitorType As String
    Private Note As String
    Private Description As String
    Private Freq1 As String
    Private Freq2 As String
    Private Freq3 As String
    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String
    Private RemainingTime As String
    Private RemainingTime1 As String
    Private RemainingTime2 As String
    Private DueAsof As String
    Private DueAsof1 As String
    Private DueAsof2 As String
    Private AssemblyModel As String
    Private Reference As String
    Private DoneOnValue As String
    Private DoneOnDate As String
    Private Remark As String
    Private Extension As String
    Private Extension1 As String
    Private Extension2 As String
    Private ExtensionDate As String
    Private ApprovalRemark As String
    Dim AssemblyDueAsof2 As String

    Dim mSearchCriteriaForEventLog As String = ""


    Dim EventLogID As Guid
    'Added By Saylee On 10-Nov-2014
    Dim AirframeDueAsof As String
    Dim AirframeDueAsof1 As String
    Dim AirframeDueAsof2 As String
    'End
    Private EstimatedDate As String
    Private RegNo As String

    Private SinceNew As String
    Private SinceNew1 As String
    Private SinceNew2 As String

    Private DoneAt As String
    Private DoneAt1 As String
    Private DoneAt2 As String
    Private MaintenanceEvent As String
    Private MinimumRemainingValue As Decimal
    Private AssemblyTypeID As Integer
    'Added by Saylee on 12-Feb-2009
    Dim RequiredManHours As String
    Dim Customer As String
    Dim Code As String
    Dim StatusMasterID As Guid
    Dim DocumentTypeForID As Integer
    Dim AssemblyDueAsof As String  'Added By DEVEN On 14/06/2008
    Dim AssemblyDueAsof1 As String 'Added By DEVEN On 14/06/2008
    Dim DueStatus As Integer
    Dim StatusID As Guid
    Public MonitorInspIDs As New StringBuilder
    Public mMachineNameValueList As MachineNameValueList
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mModelMonitorInspList = Session("mModelMonitorInspList")
        mMachineNameValueList = Session("mMachineNameValueList")
        mInspectionTypeList = Session("mInspectionTypeList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelMonitorInspList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mInspectionTypeList")
    End Sub
    Private Sub setLable()
        lblResult.Text = "List Of Model Inspsection : " & mModelMonitorInspList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()
        If mModelMonitorInspList Is Nothing Then
            btnPrint.Visible = False
            btnClose.Visible = False
        Else
            btnPrint.Visible = IIf(mModelMonitorInspList.Count > 25, True, False)
            btnClose.Visible = IIf(mModelMonitorInspList.Count > 25, True, False)
        End If
    End Sub
    Private Sub SetMonitorInspIDs()
        checkedIds = Request.Form("chkSelect").Split(",")
        MonitorInspIDs.Append("<MonitorInspID>")
        For i As Integer = 0 To checkedIds.Count - 1
            MonitorInspIDs.Append("<id>")
            MonitorInspIDs.Append(checkedIds(i))
            MonitorInspIDs.Append("</id>")
        Next
        MonitorInspIDs.Append("</MonitorInspID>")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '  DataFieldBind()
        End If
    End Sub
    Public Function ReportDetail(Optional ByVal IsExcel As Boolean = False) As ReportMaintenanceDetailList
        Try
            Dim ObjMachine As MachineInfo
            Dim ObjAssemblyStatus As AssemblyStatusInfo
            Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
            Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo


            mMachineList = MachineList.GetMachineListComplianceAssembliesMonitoringStatus(Today.Date.ToString, cmbAircraft.SelectedValue.ToString, "", "", False, True, False, MonitorInspIDs:=MonitorInspIDs.ToString)
            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""


            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                        If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                            If (ObjAssemblyMonitorInspStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True)) Then
                                ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                Description = IIf(ObjAssemblyMonitorInspStatus.ModelMonitorInspCode <> "", "Code/Form No.:" + ObjAssemblyMonitorInspStatus.ModelMonitorInspCode + vbCrLf, "") + IIf(ObjAssemblyMonitorInspStatus.Description <> "", "Description:" + ObjAssemblyMonitorInspStatus.Description, "")
                                AssemblyModel = ObjAssemblyStatus.Model
                                AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                Position = ObjAssemblyStatus.Position
                                MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                MonitorType = ObjAssemblyMonitorInspStatus.Type
                                EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted
                                MinimumRemainingValue = ObjAssemblyMonitorInspStatus.MinimumRemainingValue
                                AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                StatusMasterID = ObjAssemblyMonitorInspStatus.ModelMonitorInspID  '11-Sep-2008
                                DueStatus = ObjAssemblyMonitorInspStatus.DueStatus
                                DocumentTypeForID = 9
                                DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOnFormatted.ToString   'Added By Saylee 2-Aug-2012
                                Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                                Remark = ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                'Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008

                                Freq1 = ""
                                Freq2 = ""
                                Freq3 = ""

                                ElapsedTime = ""
                                ElapsedTime1 = ""
                                ElapsedTime2 = ""

                                RemainingTime = ""
                                RemainingTime1 = ""
                                RemainingTime2 = ""

                                DueAsof = ""
                                DueAsof1 = ""
                                DueAsof2 = ""

                                AssemblyDueAsof = ""
                                AssemblyDueAsof1 = ""
                                AssemblyDueAsof2 = ""

                                SinceNew = ""
                                SinceNew1 = ""
                                SinceNew2 = ""

                                AirframeDueAsof = ""
                                AirframeDueAsof1 = ""
                                AirframeDueAsof2 = ""

                                DoneAt = ""
                                DoneAt1 = ""
                                DoneAt2 = ""

                                Extension = ""
                                Extension1 = ""
                                Extension2 = ""

                                MaintenanceEvent = ""
                                DoneOnValue = ""

                                For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                    If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""  'Added By Saylee On 10-Nov-2017 
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AirframeDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame 'Added By Saylee On 10-Nov-2017 
                                            End If
                                            Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                            ''DoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                        Else
                                            Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = "" 'Added By Saylee On 10-Nov-2017 
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame 'Added By Saylee On 10-Nov-2017
                                            End If
                                            Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                            ''DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                        End If
                                    Else
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = "" 'Added By Saylee On 10-Nov-2017 
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame 'Added By Saylee On 10-Nov-2017
                                            End If
                                            Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                            DoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                        Else
                                            Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame 'Added By Saylee On 10-Nov-2017
                                            End If
                                            Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                            DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                        End If
                                    End If
                                Next
                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                Reference = ObjAssemblyMonitorInspStatus.Reference
                                AssemblyType = ObjAssemblyStatus.AssemblyType
                                RegNo = ObjMachine.RegNo
                                RequiredManHours = ObjAssemblyMonitorInspStatus.RequiredManHours
                                Customer = ObjMachine.Customer
                                Note = ObjAssemblyMonitorInspStatus.Notes
                                'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                'MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type
                                If ObjAssemblyMonitorInspStatus.Reference <> "" Then
                                    MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatus.Reference
                                Else
                                    MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")"
                                End If


                                'Added by Saylee 04-08-2008
                                ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark

                                StatusID = ObjAssemblyMonitorInspStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1

                                'If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                   , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                   SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                   AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , Reference, DoneOnValue, DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorInspStatus.IsApplicable, StatusID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:="", MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=6, IsMaster:=ObjAssemblyMonitorInspStatus.IsMaster, RecordID:=StatusID.ToString))
                            End If
                        End If

                    Next
                Next
            Next
            CType(ReportMaintenanceDetails, ReportMaintenanceDetailList).Sort("StatusMasterID", ComponentModel.ListSortDirection.Ascending)
        Catch ex As Exception

        End Try
        Return ReportMaintenanceDetails
    End Function
    Private Sub SetReport()
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim RptInspectionStatusList As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim OperatorName As String = String.Empty
        Dim mTaskCardListByMaintenanceActivity As TaskCardListByMaintenanceActivity

        RptInspectionStatusList = New crptInspectionWiseStatusReport

        mTaskCardListByMaintenanceActivity = TaskCardListByMaintenanceActivity.GetTaskCardList(Guid.Empty.ToString)

        ReportDetail()

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Inspection wise Status Report", New SmartDate(Today.Date).FormattedText, txtCodeFormNo.Text, txtReference.Text, txtDescription.Text, IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.ToString, ""), AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", chkTaskCard.Checked, AppSettings("Logo"), , , ) 'Changed By Utkarsh For Report Logo.

        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1403)
        End If
        ds.Clear()
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportMaintenanceDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        da.Fill(ds, mTaskCardListByMaintenanceActivity)

        RptInspectionStatusList.SetDataSource(ds)
        Session("CrystalReport") = RptInspectionStatusList
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "InspectionWiseStatusReport", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region "DataBinding"
    Private Sub DataFieldBind()
        mModelMonitorInspList = Nothing
        dgMonitorActivityList.DataSource = mModelMonitorInspList
        Session("mModelMonitorInspList") = mModelMonitorInspList

        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(All)")
        cmbInspType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(All)", , True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataSource = mMachineNameValueList

        DataBind()
    End Sub
    Private Sub FindNow()
        dgMonitorActivityList.PageIndex = 0
        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(Guid.Empty, CInt(cmbInspType.SelectedValue), 0, "", Trim(txtDescription.Text), Trim(txtReference.Text), MPDNO:=Trim(txtCodeFormNo.Text))
        dgMonitorActivityList.DataSource = mModelMonitorInspList
        Session("mModelMonitorInspList") = mModelMonitorInspList
        dgMonitorActivityList.DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            Session("MiddleFrame") = "wfrptInspectionWiseStatusReport_Ajax.aspx?"
            DataFieldBind()
            'setLable()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgMonitorActivityList_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorActivityList.Sorting
        mModelMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgMonitorActivityList.DataSource = mModelMonitorInspList
        Session("mModelMonitorInspList") = mModelMonitorInspList
        dgMonitorActivityList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If Not Request.Form("chkSelect") Is Nothing Then
            SetMonitorInspIDs()
            checkedIds = Request.Form("chkSelect").Split(",")
            If checkedIds.Length >= 1 And checkedIds.Length <= 25 Then
                SetReport()
            Else
                MSGBoxCtrl.show("Alert", checkedIds.Length.ToString & " records selected.<br>Can not print more than 25 records.", "", MsgBoxStyle.OkOnly, "")
            End If
        Else
            MSGBoxCtrl.show("Alert", "Please Select At least One Record(Max. Allowed 25)", "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        setLable()
        ControlVisibility()
        upnlGrid.Update()
        upnlAddBottom.Update()
        upnlAddTop.Update()
    End Sub
#End Region



  
End Class