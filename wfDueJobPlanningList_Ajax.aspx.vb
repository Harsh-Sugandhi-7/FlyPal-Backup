

'Created By: Prashant
'Dated By  : 8-Nov-2023



Public Class wfDueJobPlanningList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum
#End Region

#Region " Variable Declaration "
    Public mDueJobPlanningList As DueJobPlanningList
    Public mDueJobPlanning As DueJobPlanning
    Public mDistinctTextListForDueJobPlanning As DistinctTextListForDueJobPlanning
    Public mMachineNameValueList As MachineNameValueList
    Dim SearchIndex, DateIndex, FromDate, ToDate, DueJobPlanningText, No, SearchText As String
    Dim EventLogID As Guid
    Dim mDueJobPlanningDetail As String
    Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDueJobPlanning = Session("mDueJobPlanning")
        mDueJobPlanningList = Session("mDueJobPlanningList")
        mDistinctTextListForDueJobPlanning = Session("mDistinctTextListForDueJobPlanning")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        DueJobPlanningText = Session("DueJobPlanningText")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        SearchText = Session("SearchText")
    End Sub
    Private Sub SetSession()
        Session("mDueJobPlanning") = mDueJobPlanning
        Session("mDueJobPlanningList") = mDueJobPlanningList
        Session("mDistinctTextListForDueJobPlanning") = mDistinctTextListForDueJobPlanning
        SearchText = Session("SearchText")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDueJobPlanning")
        Session.Remove("mDueJobPlanningList")
        Session.Remove("mDistinctTextListForDueJobPlanning")
        Session.Remove("SearchText")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("DueJobPlanningText")
        Session.Remove("No")
        Session.Remove("BackPage")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDueJobPlanningList_Ajax.aspx?" Then
            Session.Remove("mDueJobPlanning")
            Session.Remove("mDueJobPlanningList")
            Session.Remove("mDistinctTextListForDueJobPlanning")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("DueJobPlanningText")
            Session.Remove("No")
            Session.Remove("BackPage")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgDueJobPlanningList.DataBind()
        cmbDate.SelectedIndex = DateIndex
        If cmbPlanNo.Items.Contains(New System.Web.UI.WebControls.ListItem(DueJobPlanningText)) Then
            cmbPlanNo.SelectedValue = DueJobPlanningText
        Else
            cmbPlanNo.SelectedValue = "(All)"
        End If
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "As per criteria :" & mDueJobPlanningList.Count & " Record(s) found."
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub NewRecord()
        mDueJobPlanning = DueJobPlanning.NewDueJobPlanning(New Guid)
        mDueJobPlanning.MachineID = New Guid(cmbAircraft.SelectedValue.ToString)
        mDueJobPlanning.RegNo = cmbAircraft.SelectedItem.Text
        mDueJobPlanning.DueJobPlanningDate = Today.Date
        Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mDueJobPlanning.DueJobPlanningDate.ToString, mDueJobPlanning.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
        AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

        mDueJobPlanning.DueJobPlanningPeriods.SetDueJobPlanningPeriods(mDueJobPlanning.ID, AssemblyStatusPeriodList, 1)

        Session("mDueJobPlanning") = mDueJobPlanning
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDueJobPlanning = DueJobPlanning.GetDueJobPlanning(mId)
        mDueJobPlanning.MarkClean()
        Session("mDueJobPlanning") = mDueJobPlanning
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDueJobPlanning = DueJobPlanning.GetDueJobPlanning(mId)
        Session("mDueJobPlanning") = mDueJobPlanning
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mDueJobPlanning As DueJobPlanning
                            Session("Sender") = ""
                            mDueJobPlanning = CType(Session("mDueJobPlanning"), DueJobPlanning)
                            mDueJobPlanning.Delete()
                            mDueJobPlanning.Save()
                            DataFieldBind()
                            SetControl()
                            ControlEnability()
                            upnlTitle.Update()
                            upnlGrid.Update()
                            ''upnlActionBtnBottom.Update()
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                If ex.Message.Contains("FKtabOrdertabDueJobPlanning") Then
                                    stringInfo = "Order."
                                ElseIf ex.Message.Contains("FKtabnWOtabDueJobPlanning") Then
                                    stringInfo = "Work Order."
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mDueJobPlanningDetail = "No: " + mDueJobPlanning.DueJobPlanningNo + " Dated: " + mDueJobPlanning.DueJobPlanningDateFormatted
                                MarkLog(Util.Action.Delete, "DueJobPlanning", mDueJobPlanningDetail, Util.ErrorType.NoError, mDueJobPlanning.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0,
                        Optional ByVal SearchText As String = "", Optional ByVal IsExpiredDueJobPlanning As Boolean = False)
        mDueJobPlanningList = Nothing
        dgDueJobPlanningList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mDueJobPlanningList = DueJobPlanningList.GetDueJobPlanningList(FromDate:=FromDate, ToDate:=ToDate, DueJobPlanningText:=Text, DueJobPlanningNo:=No, SearchText:=SearchText, IsExpiredDueJobPlanning:=IsExpiredDueJobPlanning)
        'Set DataSource of the Grid
        Session("mDueJobPlanningList") = mDueJobPlanningList
        dgDueJobPlanningList.DataSource = mDueJobPlanningList
        lblResult.Text = "As per criteria :" & mDueJobPlanningList.Count & " Record(s) found."
        dgDueJobPlanningList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        btnPrintTop.Enabled = (mDueJobPlanningList.Count > 0)
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow(FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, Text:=Trim(DueJobPlanningText), No:=CInt(Val(No)),
                SearchText:=txtSearchBox.Text.Trim)
        dgDueJobPlanningList.PageIndex = 0
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date)
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date)
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        lblFromDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
    End Sub
    Private Sub setVariables()
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        DueJobPlanningText = IIf(cmbPlanNo.SelectedIndex <= 0, "", cmbPlanNo.SelectedValue)
        No = txtNo.Text.Trim
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text)
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("DueJobPlanningText") = DueJobPlanningText
        Session("No") = No
        Session("SearchText") = SearchText
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub ControlEnability()
        'btnPrintTop.Enabled = IIf(dgDueJobPlanningList.Rows.Count = 0, False, True)
    End Sub
    Private Sub ControlVisibility()
        txtSearchBox.Visible = True
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights, Optional IsInRoleStr As String = "DueJobPlanning") As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        'Select Case OrderType
        ''   IsInRoleString = "DueJobPlanning"
        IsInRoleString = IsInRoleStr
        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
        End Select
    End Function
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " DatafieldBinding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        DueJobPlanningText = Session("DueJobPlanningText")
        mDistinctTextListForDueJobPlanning = DistinctTextListForDueJobPlanning.GetDistinctTextList("34", , True, "(All)")
        cmbPlanNo.DataSource = mDistinctTextListForDueJobPlanning
        mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "(SELECT)", , SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineNameValueList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
            cmbShowE.SelectedIndex = 4
            Session("MiddleFrame") = "wfDueJobPlanningList_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            ControlEnability()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgDueJobPlanningList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueJobPlanningList.RowCommand
        Dim mId As New Guid

        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim Idx As Int32
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex

                mId = New Guid(dgDueJobPlanningList.DataKeys(Idx).Value.ToString)
                EditRecord(mId)
                mDueJobPlanningDetail = "No: " + mDueJobPlanning.DueJobPlanningNo + " Dated: " + mDueJobPlanning.DueJobPlanningDateFormatted
                MarkLog(Util.Action.Edit, "DueJobPlanning", mDueJobPlanningDetail, Util.ErrorType.NoError, mId, EventLogID)

                Dim str As String
                str = "openledgersame('wfDueJobPlanning_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If


                Dim Idx As Int32
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex

                mId = New Guid(dgDueJobPlanningList.DataKeys(Idx).Value.ToString)
                If mDueJobPlanningList(mId).IsWOCreated Then
                    MSGBoxCtrl.Show("Alert..!!", "Record cannot be deleted.", "Work order already created for this Planning", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                DeleteRecord(mId)
            Case "CreateWORec"

                If (Not IsInRole(Rights.New, "CAMOWO")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Dim mnWO As nWO
                Dim tmpAssemblyStatusList As AssemblyStatusList
                Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
                Dim Index As Int32
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Index = gvr.RowIndex

                mId = New Guid(dgDueJobPlanningList.DataKeys(Index).Value.ToString)
                mDueJobPlanning = DueJobPlanning.GetDueJobPlanning(mId)
                Session("mDueJobPlanning") = mDueJobPlanning



                mnWO = nWO.NewWO(TransTypeID:=Trans.WOCAMO)
                mnWO.WODate = mDueJobPlanning.FromDateFormatted
                mnWO.WOPlanedDate = mDueJobPlanning.FromDateFormatted
                mnWO.MachineID = mDueJobPlanning.MachineID

                Dim mrptDueReport As rptDueReportForOnlyDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString, mDueJobPlanning.RegNo)


                If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan") Then
                    Dim TempRegNo As String = ""
                    TempRegNo = mDueJobPlanning.RegNo
                    mnWO.WOText = Replace(TempRegNo, "VT-", "")
                    If AppSettings("ClientCode") = "ADeccan" Then 'ADeccan Code Added by Saylee on 11-May-2018 for ADeccan11052018
                        mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yy")
                    End If
                ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                    mnWO.WOText = "MJO# " & CStr(CDate(mDueJobPlanning.FromDateFormatted).Date.Year) & " - " & mnWO.ModelName
                ElseIf AppSettings("ClientCode") = "TP" Then
                    mnWO.WOText = Replace(mDueJobPlanning.RegNo, "VT-", "") & "/" & CStr(CDate(mDueJobPlanning.FromDateFormatted).Date.Year)
                End If


                tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mDueJobPlanning.FromDateFormatted.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
                AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

                mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

                For i As Integer = 0 To mDueJobPlanning.DueJobPlanningItems.Count - 1
                    If mnWO.WOJobs.Contains(mDueJobPlanning.DueJobPlanningItems.Item(i).MaintenanceActivityID, "") = False And mrptDueReport.Contains(mDueJobPlanning.DueJobPlanningItems(i).MaintenanceActivityID, "") Then
                        Dim MaintenanceActivityID As Guid = mDueJobPlanning.DueJobPlanningItems.Item(i).MaintenanceActivityID
                        mnWO.WOJobs.Add(mnWO.ID, 2)
                        Dim Description As String = ""
                        mnWO.WOJobs.CurrentItem.PreviousTransID = MaintenanceActivityID

                        mnWO.WOJobs.CurrentItem.OnTypeID = mDueJobPlanning.DueJobPlanningItems.Item(i).OnTypeID
                        mnWO.WOJobs.CurrentItem.MonitorTypeID = mDueJobPlanning.DueJobPlanningItems.Item(i).MonitorTypeID

                        Description = mDueJobPlanning.DueJobPlanningItems.Item(i).Description

                        mnWO.WOJobs.CurrentItem.WOJobDescription = Description

                        mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = Description
                        mnWO.WOJobs.CurrentItem.Zone = mrptDueReport.Item(MaintenanceActivityID).Zone
                        mnWO.WOJobs.CurrentItem.AREA = mrptDueReport.Item(MaintenanceActivityID).Area
                        mnWO.WOJobs.CurrentItem.IsRII = mrptDueReport.Item(MaintenanceActivityID).IsRII
                        mnWO.WOJobs.CurrentItem.DueAsOf = mrptDueReport.Item(MaintenanceActivityID).DueAsof2
                        mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = mrptDueReport.Item(MaintenanceActivityID).EstimatedHours

                        If AppSettings("ShowCAMOOnlyForNewClients") = "True" And mDueJobPlanning.DueJobPlanningItems.Item(i).MonitorTypeID = 1 Then '"Servicing"
                            mnWO.WOJobs.CurrentItem.TaskCardNo = mrptDueReport.Item(MaintenanceActivityID).TaskNo
                            mnWO.WOJobs.CurrentItem.TaskSourceRef = mrptDueReport.Item(MaintenanceActivityID).SourceDoc
                            mnWO.WOJobs.CurrentItem.Publication = mrptDueReport.Item(MaintenanceActivityID).Reference
                            mnWO.WOJobs.CurrentItem.Skill = mrptDueReport.Item(MaintenanceActivityID).Skill
                            mnWO.WOJobs.CurrentItem.SkillID = mrptDueReport.Item(MaintenanceActivityID).SkillID
                        ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And mDueJobPlanning.DueJobPlanningItems.Item(i).MonitorTypeID = 3 Then '"Modification"
                            mnWO.WOJobs.CurrentItem.TaskCardNo = mrptDueReport.Item(MaintenanceActivityID).Number
                            mnWO.WOJobs.CurrentItem.InspCode = mrptDueReport.Item(MaintenanceActivityID).Code
                            mnWO.WOJobs.CurrentItem.TaskSourceRef = mrptDueReport.Item(MaintenanceActivityID).Reference
                        Else
                            mnWO.WOJobs.CurrentItem.InspCode = mrptDueReport.Item(MaintenanceActivityID).Code 'Added by Saylee on 18-Feb-2018 for ASH18022019 
                            mnWO.WOJobs.CurrentItem.TaskSourceRef = mrptDueReport.Item(MaintenanceActivityID).Reference
                        End If

                        If mrptDueReport.Item(MaintenanceActivityID).AssemblyTypeID = 1 Then
                            mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mrptDueReport.Item(MaintenanceActivityID).AssemblyTypeName
                        Else
                            mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mrptDueReport.Item(MaintenanceActivityID).AssemblyTypeName + IIf(mrptDueReport.Item(MaintenanceActivityID).Position = "", "", "(" + mrptDueReport.Item(MaintenanceActivityID).Position + ")")
                        End If



                        With mnWO.WOJobs.CurrentItem
                            'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
                            'TASK(s):
                            Dim mMaintenanceTask As MaintenanceTask
                            Dim mMaintenanceTaskDetail As MaintenanceTaskDetail

                            If .OnTypeID = 1 Then        'Assembly
                                mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, True)
                            ElseIf .OnTypeID = 2 Then    'Componant
                                mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, False)
                            End If

                            For Each mMaintenanceTaskDetail In mMaintenanceTask.MaintenanceTaskDetails
                                mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

                                With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem
                                    '.TaskAction = "No action taken." 'mMaintenanceTaskDetail.Task 'Commented By Prashant 12-Mar-2010
                                    .TaskAction = ""  'Added By Prashant 12-Mar-2010
                                    .ActualStartDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
                                    .ActualEndDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
                                    .IsDone = False
                                    .TaskCardID = mMaintenanceTaskDetail.TaskCardID  'Added By Prashant 29-Dec-2008

                                    'Added By Utkarsh On 27-Apr-2011

                                    Dim mTaskCard As TaskCard
                                    mTaskCard = TaskCard.GetTaskCard(mMaintenanceTaskDetail.TaskCardID)
                                    .TaskCardNo = mTaskCard.TaskCardNo
                                    .TaskDescription = mTaskCard.TaskDesc
                                    .RevNo = mTaskCard.RevNo
                                    .RevDate = mTaskCard.RevDate
                                    .IssueDate = mTaskCard.IssueDate

                                    ''Added by Saylee on 4-Feb-2013
                                    ''If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
                                    ''    .Reference = mSelectDueJobs.Item(i).Reference
                                    ''Else
                                    ''    .Reference = mTaskCard.Reference
                                    ''End If
                                    '***************************
                                    ''Commentedby Saylee on 15-Feb-2013
                                    .Reference = mTaskCard.Reference

                                    .Equipment = mTaskCard.Equipment
                                    .Material = mTaskCard.Material
                                    .EstimatedHours = mTaskCard.EstimatedHours
                                    .checks = mTaskCard.Check
                                    .RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo
                                    .ImageSize = mTaskCard.ImageSize
                                    .ImageFile = mTaskCard.ImageFile
                                    .FileExtension = mTaskCard.FileExtension

                                    'Added by Vikrant on 06-Sept-2013 For BA04092013
                                    Dim mTaskCardSpare As TaskCardSpare
                                    Dim mTaskCardStepsSpare As TaskCardSpare

                                    For Each mTaskCardSpare In mTaskCard.TaskCardSpares
                                        If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Contains(mTaskCardSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares(mTaskCardSpare.ItemID, "").RequiredQty += mTaskCardSpare.RequiredQty
                                        Else 'existing condition
                                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                            With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem
                                                .ItemID = mTaskCardSpare.ItemID
                                                .RequiredQty = mTaskCardSpare.RequiredQty
                                                .PartNo = mTaskCardSpare.PartNo
                                                .Description = mTaskCardSpare.Description
                                                .Remark = mTaskCardSpare.Remark
                                                .OnSerialNo = mTaskCardSpare.OnSerialNo
                                                .OffSerialNo = mTaskCardSpare.OffSerialNo
                                                .IsForSteps = False
                                            End With
                                        End If
                                    Next

                                    For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
                                        If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Contains(mTaskCardStepsSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares(mTaskCardStepsSpare.ItemID, "").RequiredQty += mTaskCardStepsSpare.RequiredQty
                                        Else 'existing condition
                                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                            With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem
                                                .ItemID = mTaskCardStepsSpare.ItemID
                                                .RequiredQty = mTaskCardStepsSpare.RequiredQty
                                                .PartNo = mTaskCardStepsSpare.PartNo
                                                .Description = mTaskCardStepsSpare.Description
                                                .Remark = mTaskCardStepsSpare.Remark
                                                .OnSerialNo = mTaskCardStepsSpare.OnSerialNo
                                                .OffSerialNo = mTaskCardStepsSpare.OffSerialNo
                                                .IsForSteps = True
                                            End With
                                        End If
                                    Next
                                    'End
                                    'Added By Vikrant on 03-Mar-2020 For ALL03032020
                                    For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals
                                        If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Contains(mTaskCardSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals(mTaskCardSpare.ItemID, "").RequiredQty += mTaskCardSpare.RequiredQty
                                        Else 'existing condition
                                            mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                            With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem
                                                .ItemID = mTaskCardSpare.ItemID
                                                .RequiredQty = mTaskCardSpare.RequiredQty
                                                .PartNo = mTaskCardSpare.PartNo
                                                .Description = mTaskCardSpare.Description
                                                .Remark = mTaskCardSpare.Remark
                                                .OnSerialNo = mTaskCardSpare.OnSerialNo
                                                .OffSerialNo = mTaskCardSpare.OffSerialNo
                                                .IsForSteps = False
                                                .IsPartRemoval = True
                                                .Position = mTaskCardSpare.Position
                                            End With
                                        End If
                                    Next
                                    'End
                                End With
                            Next

                            'KIT(s):
                            Dim mMaintenanceKit As MaintenanceKit

                            If .OnTypeID = 1 Then        'Assembly
                                mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True)
                            ElseIf .OnTypeID = 2 Then    'Componant
                                mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False)
                            End If
                            'Commented and Added by Saylee on 23-July-2013 for BA22072013 	
                            ''''For Each mMaintenanceKitDetail In mMaintenanceKit.MaintenanceKitDetails
                            ''''    mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

                            ''''    With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
                            ''''        .ItemID = mMaintenanceKitDetail.ItemID
                            ''''        .RequiredQty = mMaintenanceKitDetail.Qty
                            ''''        Dim mItem As Item = Item.GetItem(mMaintenanceKitDetail.ItemID)
                            ''''        .PartNo = mItem.Name
                            ''''        .Description = mItem.Description
                            ''''        mItem = Nothing
                            ''''    End With
                            ''''Next
                            '''''-----------------------------------------------------------------------
                            'Added by Saylee on 23-July-2013 for BA22072013 	
                            Dim mMaintenanceSpares As MaintenanceKit
                            Dim mMaintenanceSparesDetail As MaintenanceKitDetail

                            Dim mMaintenanceTools As MaintenanceKit
                            Dim mMaintenanceToolsDetail As MaintenanceKitDetail

                            If .OnTypeID = 1 Then        'Assembly
                                mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, False)
                                mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, True)
                            ElseIf .OnTypeID = 2 Then    'Componant
                                mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, False)
                                mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, True)
                            End If

                            For Each mMaintenanceSparesDetail In mMaintenanceSpares.MaintenanceKitDetails
                                If mnWO.WOJobs.CurrentItem.WOJobSpares.Contains(mMaintenanceSparesDetail.ItemID, "") Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                                    mnWO.WOJobs.CurrentItem.WOJobSpares(mMaintenanceSparesDetail.ItemID).RequiredQty += mMaintenanceSparesDetail.Qty
                                Else 'existing condition
                                    mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

                                    With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
                                        .ItemID = mMaintenanceSparesDetail.ItemID
                                        .RequiredQty = mMaintenanceSparesDetail.Qty
                                        Dim mItem As Item = Item.GetItem(mMaintenanceSparesDetail.ItemID)
                                        .PartNo = mItem.Name
                                        .Description = mItem.Description
                                        mItem = Nothing
                                        .Remark = mMaintenanceSparesDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                                    End With
                                End If

                            Next

                            For Each mMaintenanceToolsDetail In mMaintenanceTools.MaintenanceKitDetails
                                If Not mnWO.WOTools.Contains(mMaintenanceToolsDetail.ItemID) Then
                                    mnWO.WOTools.Add(mnWO.ID)

                                    With mnWO.WOTools.CurrentItem
                                        .ItemID = mMaintenanceToolsDetail.ItemID
                                        .RequiredQty = mMaintenanceToolsDetail.Qty
                                        Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
                                        .PartNo = mItem.Name
                                        .Description = mItem.Description
                                        mItem = Nothing
                                        .WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                                    End With
                                Else
                                    mnWO.WOTools.CurrentIndex = mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").SrNo - 1
                                    If mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty = 0 Then

                                    Else
                                        If (mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty <= mMaintenanceToolsDetail.Qty) Or (mMaintenanceToolsDetail.Qty = 0) Then
                                            With mnWO.WOTools.CurrentItem
                                                .ItemID = mMaintenanceToolsDetail.ItemID
                                                .RequiredQty = mMaintenanceToolsDetail.Qty
                                                Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
                                                .PartNo = mItem.Name
                                                .Description = mItem.Description
                                                mItem = Nothing
                                                .WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                                            End With
                                        End If
                                    End If
                                End If
                            Next
                            '-----------------------------------------------------------------------
                        End With

                    End If


                Next
                Session("mnWO") = mnWO

                'Dim URLFromDueReportPreview As Stack = New Stack
                'URLFromDueReportPreview.Push(Request.Url)
                'Session("wfDueJobPlanningList_Ajax") = "wfDueJobPlanningList_Ajax"
                'Session("URLFromDueReportPreview") = URLFromDueReportPreview
                'Response.Redirect("wfnWODetail_Ajax.aspx?BackPage=index.aspx")
                Dim str As String
                str = "openledgersame('wfnWODetail_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

        End Select
    End Sub
    Private Sub dgDueJobPlanningList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDueJobPlanningList.PageIndexChanging
        dgDueJobPlanningList.PageIndex = e.NewPageIndex
        dgDueJobPlanningList.DataSource = mDueJobPlanningList
        Session("mDueJobPlanningList") = mDueJobPlanningList
        dgDueJobPlanningList.DataBind()
        dgDueJobPlanningList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbPlanNo.SelectedIndexChanged
        If sender.id = "cmbDate" Then
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            setPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
        ElseIf sender.id = "cmbPlanNo" Then
            txtNo.Text = "0"
            If cmbPlanNo.Enabled = True Then
                cmbPlanNo.Focus()
            End If
        End If
    End Sub
    Private Sub imgFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgFindNow.Click  ''btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgDueJobPlanningList.DataBind()
        ControlEnability()
        lblResult.Text = "As per criteria :" & mDueJobPlanningList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlTitle.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click  '',btnAddNew.Click
        'If (Not IsInRole(Rights.New)) Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        If IsValid = False Then upnlValidationSummary.Update() : Exit Sub


        NewRecord()
        MarkLog(Util.Action.[New], "DueJobPlanning", "", Util.ErrorType.NoError, mDueJobPlanning.ID, EventLogID)
        Dim str As String
        str = "openledgersame('wfSelectDueJobList_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)



    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click ''btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgDueJobPlanningList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDueJobPlanningList.Sorting
        mDueJobPlanningList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mDueJobPlanningList") = mDueJobPlanningList
        dgDueJobPlanningList.DataSource = mDueJobPlanningList
        dgDueJobPlanningList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        dgDueJobPlanningList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgDueJobPlanningList.DataSource = mDueJobPlanningList
        dgDueJobPlanningList.DataBind()

        ControlVisibility(0)
        setVariables()
        SetControl()
        ControlEnability()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub txtSearchBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        dgDueJobPlanningList.DataBind()

        SetControl()
        ControlEnability()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
        'Dim da As New CSLA.Data.ObjectAdapter
        'Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        'Dim rpt As DueJobPlanningList
        'Dim ds As New dsDueJobPlanningList
        'myReport = New crptDueJobPlanningList
        'rpt = Session("mDueJobPlanningList")

        'Dim mCompanyDetail As New CompanyDetail
        'Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
        '                                  mCompanyDetail.Email, WebSite:="", ReportName:="", SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText, SearchStr2:=New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbPlanNo.SelectedIndex = 0, "", cmbPlanNo.SelectedItem.Text + IIf(txtNo.Text = "", "", "-" + txtNo.Text)),
        '                                  SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
        '                                  SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"),
        '                                  SearchStr11:="", SearchStr12:="", SearchStr13:="", SearchStr14:="", SearchStr15:="", SearchStr16:="")

        'If rpt.Count <= 0 Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'Else
        '    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1550)
        'End If
        'ds.Clear()
        'Dim mrptImage As rptImage = rptImage.GetImage(ds)
        'da.Fill(ds, rpt)
        'da.Fill(ds, mrptImage)
        'da.Fill(ds, mReport)
        'myReport.SetDataSource(ds)
        'Session("CrystalReport") = myReport
        'Dim Str As String
        'Str = "openTranDetail();"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
    '-----
#End Region

End Class