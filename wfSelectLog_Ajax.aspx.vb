'AJAX Created by :   Saylee
'Date            :   8-Dec-2014

Public Class wfSelectLog_Ajax
    Inherits System.Web.UI.Page

#Region " Enum "
    Public Enum FromType
        FromInstallation = 1
        FromRemoval = 2
        FromCompliance = 3
        FromCompRemovalInstall = 4
    End Enum
#End Region

#Region "Variable Declaration"
    Public mFromType As FromType
    Public mLogDate As String = ""
    Public mLogToDate As String = ""                    'Added Code By Girish 10,April,2007
    Public mMachineId As String = "{00000000-0000-0000-0000-000000000000}"
    Public mAssemblyStatusId As String = "{00000000-0000-0000-0000-000000000000}"
    Public mMachineNameValueList As MachineNameValueList
    Public mReportLogRegister As New ReportLogRegister
    Public mAssemblyID As String = "{00000000-0000-0000-0000-000000000000}"
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus    'AC
    'Public mAssemblyMonitorInspStatusList As AssemblyMonitorInspStatusList 'AC
    Public mDoneOn As String
    Public formType As String
    Public LogID As String
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mFromType = CType(Session("mFromType"), FromType)
        mLogDate = CType(Session("mLogDate"), String)
        mLogToDate = CType(Session("mLogToDate"), String)
        mAssemblyStatusId = Session("mAssemblyStatusId")
        mMachineId = Session("mMachineId") 'CType(Session("mMachineId"), Guid)
        mAssemblyID = Session("mAssemblyID")
        mReportLogRegister = CType(Session("mReportLogRegister"), ReportLogRegister)
        formType = Session("FormType")   'AC
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)  'AC
        'mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), AssemblyMonitorInspStatusList)  'AC
    End Sub
    Private Sub SetSession()
        Session("mFromType") = mFromType
        Session("mLogDate") = mLogDate
        Session("mLogToDate") = mLogToDate        'Added Code By Girish 10,April,2007
        Session("mAssemblyStatusId") = mAssemblyStatusId
        Session("mAssemblyID") = mAssemblyID
        Session("mReportLogRegister") = mReportLogRegister
        Session("mDoneOn") = mDoneOn
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    Private Sub RemoveSession()
        mFromType = Nothing
        ''mReportLogRegister = Nothing
        mLogDate = Nothing
        mLogToDate = Nothing                    'Added Code By Girish 10,April,2007
        mMachineId = Nothing
        mAssemblyStatusId = Nothing

        Session.Remove("mFromType")
        Session.Remove("mLogDate")
        Session.Remove("mLogToDate")            'Added Code By Girish 10,April,2007
        Session.Remove("mMachineId")
        Session.Remove("mAssemblyStatusId")

    End Sub
    Private Sub LoadPage()

        'mFromType = CType(Request.QueryString("FromType"), FromType)
        'mMachineId = CType(New Guid(Request.QueryString("MachineId")), Guid)
        'mAssemblyStatusId = CType(New Guid(Request.QueryString("AssemblyStatusID")), Guid)
        'mAssemblyID = Request.QueryString("AssemblyID").ToString
        mFromType = CType(Session("mFromType"), FromType)
        mMachineId = Session("mMachineId")
        mAssemblyStatusId = Session("mAssemblyStatusId")
        mAssemblyID = Session("mAssemblyID")

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(SELECT)", , True)

        cmbMachineList.DataSource = mMachineNameValueList
        cmbMachineList.DataBind()

        mDoneOn = Session("mDoneOn")
        Dim mDate As Date
        If mDoneOn = "" Then
            mDate = Today.Date
        Else
            mDate = CDate(mDoneOn)
        End If
        CalFromDate.Text = mDate.Date.AddDays(-1)
        CalToDate.Text = mDate.Date
        ''''''''''
        cmbMachineList.SelectedValue = mMachineId.ToString
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mFromType") = mFromType
        Session("mMachineId") = mMachineId
        Session("mAssemblyStatusId") = mAssemblyStatusId
        Session("mAssemblyID") = mAssemblyID
    End Sub
    Private Sub SetPage()
        cmbMachineList.SelectedValue = mMachineId.ToString
        If Not mReportLogRegister Is Nothing Then
            lblResult.Text = " List of Log : " & mReportLogRegister.Count & " record(s) found."
        End If
    End Sub
    Private Sub ControlVisibility()
        cmbMachineList.Enabled = (mFromType = FromType.FromInstallation)
        lblNote.Visible = (mFromType = FromType.FromCompliance)

        'Commented by Saylee on 6-Jul-2021, Work from Home
        ''''''Added By Utkarsh On 23-Apr-2012 For ALL23042012 'Log Page No...
        '''''dgLogList.Columns(3).Visible = IIf((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA"), True, False) 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
        ''''''End 

        For j As Integer = 0 To dgLogList.Rows.Count - 1
            Dim P As New Integer
            If mReportLogRegister(j).LogTypeID = 3 Then
                dgLogList.Rows(j).Cells(13).Enabled = False
            End If
        Next
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById ('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    Private Sub FindNow(ByVal MachineId As Guid, Optional ByVal fromLogDate As String = "", Optional ByVal toLogDate As String = "")
        If Not mFromType = FromType.FromCompRemovalInstall Then
            mReportLogRegister = ReportLogRegister.GetLogRegister(toLogDate, toLogDate, mAssemblyID, mMachineId.ToString, False, , 1)
        Else
            mReportLogRegister = ReportLogRegister.GetLogRegister(toLogDate, toLogDate, mAssemblyID, mMachineId.ToString, False, , 1)
        End If
        dgLogList.DataSource = mReportLogRegister
        Session("mReportLogRegister") = mReportLogRegister

        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
            dgLogList.Columns.Item(9).HeaderText = "Flights diff."
            If mReportLogRegister.Count > 0 Then
                If mReportLogRegister(0).Col3DiffPeriodID = 3 Then
                    dgLogList.Columns(10).HeaderText = "Flights final"
                Else
                    dgLogList.Columns(10).HeaderText = "NG Cycles final"
                End If
            End If
        Else
            dgLogList.Columns(9).HeaderText = "Cycles diff."
            If mReportLogRegister.Count > 0 Then
                If mReportLogRegister(0).Col3DiffPeriodID = 3 Then
                    dgLogList.Columns(10).HeaderText = "Cycles final"
                Else
                    dgLogList.Columns(10).HeaderText = "NG Cycles final"
                End If
            End If

        End If

        dgLogList.DataBind()
        'DeVeN 17-06-2009

        'upnlGrid.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "FirstLog" Then
                        Try
                            Session("sender") = ""
                            mAssemblyStatus = Session("mAssemblyStatus")
                            Session("ConsiderAssemblyInstValue") = True
                            Session.Remove("mFirstLogDetailAfterAssemblyInstallation")
                            MarkLog(Util.Action.Close, "Select Log From Comp Installation", User.Identity.Name & " opted for Assembly Installation Value(s) to consider ", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            Dim mopenas As String = Request.QueryString("Type")
                            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                Exit Sub
                            End If
                            'End
                            Response.Redirect(Request.QueryString("BackPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&LogId=" & Session("LogID") & "&LogDate=" & Session("mDoneOn") & "&Type=-1")

                        Catch ex As SqlException
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    If MSGBoxCtrl.Sender = "FirstLog" Then
                        Try
                            Session("sender") = ""
                            mAssemblyStatus = Session("mAssemblyStatus")
                            Session("ConsiderAssemblyInstValue") = False
                            Session.Remove("mFirstLogDetailAfterAssemblyInstallation")
                            MarkLog(Util.Action.Close, "Select Log From Comp Installation", User.Identity.Name & " NOT opted for Assembly Installation Value(s) to consider ", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            Dim mopenas As String = Request.QueryString("Type")
                            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                Exit Sub
                            End If
                            'End
                            Response.Redirect(Request.QueryString("BackPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&LogId=" & ID & "&LogDate=" & Session("mDoneOn") & "&Type=-1")
                        Catch ex As SqlException
                        End Try
                    End If
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                    ControlVisibility()
                    upnlGrid.Update()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            ControlVisibility()
            upnlGrid.Update()
        ElseIf Result1 = 0 Then

        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "cmbMachineList" Then
            If cmbMachineList.SelectedIndex = 0 Then
                CustValid.ErrorMessage = "Please select aircraft from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        'mMachineNameValueList = tmpMachineList.GetMachineList(, , , , , "<SELECT>")
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(SELECT)", , True)
        cmbMachineList.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        'FindNow(txtLogDate.Text)
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If cmbMachineList.Enabled = True Then
                setFocus(cmbMachineList)
            End If
            mDoneOn = Session("mDoneOn") 'Request.QueryString("DoneOn").ToString
            'Session("mDoneOn") = mDoneOn
            LoadPage()
            mDoneOn = Session("mDoneOn")
            Dim mDate As Date
            If mDoneOn = "" Then
                mDate = Today.Date
            Else
                mDate = CDate(mDoneOn)
            End If

            CalFromDate.Text = IIf(CalFromDate.Text = "", mDate.Date.AddDays(-1), CalFromDate.Text)
            IIf(CalToDate.Text = "", CalToDate.Text = mDate.Date, CalToDate.Text = CalToDate.Text)
            FindNow(New Guid(mMachineId), CalFromDate.Text, CalToDate.Text)
            CalFromDate.Enabled = False
            CalToDate.Enabled = False
            SetPage()
            ControlVisibility()
        End If

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            dgLogList.PageIndex = 0

            '' mLogDate = txtLogDate.Text        Commented Now

            mMachineId = cmbMachineList.SelectedValue.ToString
            Session("mMachineId") = mMachineId
            'Added By DeveN 
            mDoneOn = Session("mDoneOn")
            Dim mDate As Date
            If mDoneOn = "" Then
                mDate = Today.Date
            Else
                mDate = CDate(mDoneOn)
            End If

            IIf(CalFromDate.Text = "", CalFromDate.Text = mDate.Date.AddDays(-1).ToShortDateString, CalFromDate.Text = CalFromDate.Text)
            IIf(CalToDate.Text = "", CalToDate.Text = mDate.Date.ToShortDateString, CalToDate.Text = CalToDate.Text)
            FindNow(New Guid(mMachineId), CalFromDate.Text, CalToDate.Text)
            SetPage()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgLogList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLogList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Index As Integer = CInt(e.CommandArgument) + dgLogList.PageSize * dgLogList.PageIndex
                Dim mID As Guid = mReportLogRegister(Index).LogID
                RemoveSession()
                Session.Remove("mMachineNameValueList")
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                Session("LogID") = mID.ToString
                Session("FromLog") = True
                Session("mReportLogRegister") = mReportLogRegister
                Session("LogIdWO") = mID.ToString
                Session("LogDate") = Session("mDoneOn")
                Session("ConsiderAssemblyInstValue") = False
                'Added by Saylee on 16-Mar-2015 for ALL16032015
                'Comp Installation Date is same Assembly Inst. Date,
                'Also there is one Log entered on same date
                'so user may need Values before logs i.e Assembly Inst. values
                '''  If Session("ForCompInstall") = True Then '**Commented by Saylee as need to check for all 
                Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation
                mFirstLogDetailAfterAssemblyInstallation = Session("mFirstLogDetailAfterAssemblyInstallation")
                If Not mFirstLogDetailAfterAssemblyInstallation Is Nothing Then
                    If mReportLogRegister(Index).Col1DiffInDecimal <> 0 And mReportLogRegister(Index).LogID = mFirstLogDetailAfterAssemblyInstallation.LogID Then
                        MSGBoxCtrl.show("Alert!", "This is the first Log after Assembly Installation.", "Click <B>'Yes'</B> to consider Assembly Installation values.<BR>Click <b>'No'</b> to consider Final Values of selected log", MsgBoxStyle.YesNo, "FirstLog")
                        Exit Sub
                    End If

                End If
                ''   End If
                '*************************************
                MarkLog(Util.Action.Save, "Select Log From Comp Installation", User.Identity.Name & " selected log no. " & mReportLogRegister(Index).LogNo & " dated " + mReportLogRegister(Index).LogDateFormatted.ToString, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                'End

                Response.Redirect(Request.QueryString("BackPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&LogId=" & ID & "&LogDate=" & Session("mDoneOn") & "&Type=-1")
        End Select
    End Sub
    Private Sub dgLogList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgLogList.Sorting
        mReportLogRegister.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mReportLogRegister") = mReportLogRegister
        dgLogList.DataSource = mReportLogRegister
        dgLogList.DataBind()
        ControlVisibility()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RemoveSession()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session.Remove("LogID")
        Session("FromLog") = False
        Session("LogIdWO") = Guid.Empty.ToString
        mReportLogRegister = Nothing
        Session.Remove("mReportLogRegister")
        Session.Remove("mFirstLogDetailAfterAssemblyInstallation")
        Session.Remove("mMachineNameValueList")
        MarkLog(Util.Action.Close, "Select Log From Comp Installation", User.Identity.Name, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

        Response.Redirect(Request.QueryString("BackPage6") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
    End Sub

    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region


  
End Class