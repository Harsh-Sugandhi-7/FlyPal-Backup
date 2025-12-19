Imports System.Text
Public Class wfFlightDelayCancellationList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mFligthDelayAndCancellationList As FligthDelayAndCancellationList
    Public mFligthDelayAndCancellation As FligthDelayAndCancellation
    Dim EventLogID As Guid
    Public mMachineNameValueList As MachineNameValueList
    Dim mEventLogDetail As String
    Public AircraftId As String
    Public StartDate As String
    Public EndDate As String
    Public IsCancel As Boolean
    Public IsDelay As Boolean
    Public Reliability As Boolean
    Dim ModuleName As String = "FlightDelayCancellation"
    Dim mFileAttach As FileAttach
    Dim IsReadOnly As Boolean
    Public mCompanyDetail As CompanyDetail
    Dim MachineIDs As New StringBuilder
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFligthDelayAndCancellationList = CType(Session("mFligthDelayAndCancellationList"), FligthDelayAndCancellationList)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        AircraftId = CType(Session("AircraftId"), String)
        StartDate = CType(Session("StartDate"), String)
        EndDate = CType(Session("EndDate"), String)
        IsCancel = CType(Session("IsCancel"), Boolean)
        IsDelay = CType(Session("IsDelay"), Boolean)
        Reliability = CType(Session("Reliability"), Boolean)
        IsReadOnly = Session("IsReadOnly")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mFligthDelayAndCancellationList")
        Session.Remove("AircraftId")
        Session.Remove("StartDate")
        Session.Remove("EndDate")
        Session.Remove("IsCancel")
        Session.Remove("IsDelay")
        Session.Remove("Reliability")
        Session.Remove("IsReadOnly")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfFlightDelayCancellationList_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mFligthDelayAndCancellationList")
            Session.Remove("AircraftId")
            Session.Remove("StartDate")
            Session.Remove("EndDate")
            Session.Remove("IsCancel")
            Session.Remove("IsDelay")
            Session.Remove("Reliability")
            Session.Remove("IsReadOnly")
        End If
    End Sub
    Private Sub EditRecord(ByVal Id As Guid)
        mFligthDelayAndCancellation = FligthDelayAndCancellation.GetFlightDelayCancellation(Id)
        mFligthDelayAndCancellation.RegNo = mFligthDelayAndCancellationList(Id).RegNo
        Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
        Session("mFligthDelayAndCancellationList") = Nothing
        Session("FlightDCEdit") = True
        mEventLogDetail = "Reg No : " & mFligthDelayAndCancellationList(Id).RegNo & ", Dated : " & mFligthDelayAndCancellationList(Id).Date & ", Status : " & mFligthDelayAndCancellationList(Id).Status & ", Log No. : " & mFligthDelayAndCancellationList(Id).LogTextNo
        MarkLog(Util.Action.Edit, "FlightDelayCancellation", mEventLogDetail, Util.ErrorType.HandledError, mFligthDelayAndCancellation.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfFlightDelayCancellation_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mFligthDelayAndCancellationList.CurrentIndex = Index
        Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TempID As Guid
                        Try
                            Session("sender") = ""
                            mFligthDelayAndCancellationList = CType(Session("mFligthDelayAndCancellationList"), FligthDelayAndCancellationList)
                            TempID = mFligthDelayAndCancellationList.CurrentItem.ID
                            If mFligthDelayAndCancellationList(TempID).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(TempID)
                            End If
                            mEventLogDetail = "Reg No : " & mFligthDelayAndCancellationList(TempID).RegNo & ", Dated : " & mFligthDelayAndCancellationList(TempID).Date & ", Status : " & mFligthDelayAndCancellationList(TempID).Status & ", Log No. : " & mFligthDelayAndCancellationList(TempID).LogTextNo
                            FligthDelayAndCancellation.DeleteFlightDelayCancellation(mFligthDelayAndCancellationList.CurrentItem.ID)
                            MarkLog(Util.Action.Delete, ModuleName, mEventLogDetail, Util.ErrorType.NoError, TempID, EventLogID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

                            If mCompanyDetail.IsSyncApplication Then
                                MachineIDs.Append("<MachineID>")
                                MachineIDs.Append("<id>")
                                MachineIDs.Append(AircraftId)
                                MachineIDs.Append("</id>")
                                MachineIDs.Append("</MachineID>")
                            End If
                            mFligthDelayAndCancellationList = FligthDelayAndCancellationList.GetFlightDCList(New Guid(AircraftId),
                                                                                                             txtFromDate.Text,
                                                                                                             txtToDate.Text,
                                                                                                             chkDelay.Checked,
                                                                                                             chkCancel.Checked,
                                                                                                             chkReliability.Checked,
                                                                                                             MachineIDStr:=MachineIDs.ToString,
                                                                                                            IsSyncApplication:=mCompanyDetail.IsSyncApplication)
                            Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
                            dgFlightDC.DataSource = mFligthDelayAndCancellationList
                            dgFlightDC.DataBind()

                            SetPage()
                            ControlVisibility()
                            SetGrid()
                            upnlGrid.Update()
                            upnlActionBtnTop.Update()
                            upnlActionBtnBottom.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, ModuleName, "Can't delete : " & mEventLogDetail & " is Currently in use", Util.ErrorType.NoError, TempID, EventLogID)
                            End If
                        Finally
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub FindNow()
        Session("AircraftId") = cmbAircraft.SelectedValue
        Session("StartDate") = txtFromDate.Text
        Session("EndDate") = txtToDate.Text
        Session("IsCancel") = chkCancel.Checked
        Session("IsDelay") = chkDelay.Checked
        Session("Reliability") = chkReliability.Checked

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        If mCompanyDetail.IsSyncApplication Then
            MachineIDs.Append("<MachineID>")
            MachineIDs.Append("<id>")
            MachineIDs.Append(AircraftId)
            MachineIDs.Append("</id>")
            MachineIDs.Append("</MachineID>")
        End If
        Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
        mFligthDelayAndCancellationList = Nothing
        Session.Remove("mFligthDelayAndCancellationList")
        mFligthDelayAndCancellationList = FligthDelayAndCancellationList.GetFlightDCList(mMachineID,
                                                                                         txtFromDate.Text,
                                                                                         txtToDate.Text,
                                                                                         chkDelay.Checked,
                                                                                         chkCancel.Checked,
                                                                                         MachineIDStr:=MachineIDs.ToString,
                                                                                         ConsiderInReliability:=chkReliability.Checked,
                                                                                         IsSyncApplication:=mCompanyDetail.IsSyncApplication)
        Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
        ControlVisibility()
        dgFlightDC.DataSource = mFligthDelayAndCancellationList
        dgFlightDC.DataBind()
    End Sub
    Private Sub SetPage()
        If mFligthDelayAndCancellationList Is Nothing Then
            lblResult.Text = "List of flight Delay/Cancellation of the Aircraft as per criteria : 0 Record(s) found."
        Else
            lblResult.Text = "List of flight Delay/Cancellation of the Aircraft as per criteria : " & mFligthDelayAndCancellationList.Count & " Record(s) found."
        End If
    End Sub
    Private Sub ControlVisibility()
        If ((Not mFligthDelayAndCancellationList Is Nothing) AndAlso mFligthDelayAndCancellationList.Count <= 0) Or mFligthDelayAndCancellationList Is Nothing Then
            btnPrint.Enabled = False
            btnPrintTop.Enabled = False
        Else
            btnPrint.Enabled = True
            btnPrintTop.Enabled = True
        End If
        If IsReadOnly Then
            btnAddNewTop.Enabled = False
            btnAdd.Enabled = False
            lblReadOnly.Visible = True
        Else
            btnAddNewTop.Enabled = True
            btnAdd.Enabled = True
            lblReadOnly.Visible = False
        End If
        chkReliability.Visible = Not mCompanyDetail.IsSyncApplication

    End Sub
    Private Sub SetGrid()
        Dim P As Integer
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        btnAddNewTop.Visible = Not mCompanyDetail.IsSyncApplication
        btnPrintTop.Visible = Not mCompanyDetail.IsSyncApplication
        For j As Integer = 0 To dgFlightDC.Rows.Count - 1

            Dim EditViewRecord As ImageButton = CType(dgFlightDC.Rows(j).FindControl("EditViewRecord"), ImageButton)
            Dim DeleteRecord As ImageButton = CType(dgFlightDC.Rows(j).FindControl("DeleteRecord"), ImageButton)

            'P = CType(Me.dgFlightDC.Rows.Item(j).Cells(13).Text, Boolean)
            'If P = False Then
            '    dgFlightDC.Rows.Item(j).Cells(12).Enabled = False
            'End If

            If IsReadOnly Then
                'dgFlightDC.Rows(j).Cells(10).Enabled = False
                'dgFlightDC.Rows(j).Cells(11).Enabled = False
                EditViewRecord.Enabled = False
                DeleteRecord.Enabled = False
            Else
                'dgFlightDC.Rows(j).Cells(10).Enabled = True
                'dgFlightDC.Rows(j).Cells(11).Enabled = True
                EditViewRecord.Enabled = True
                DeleteRecord.Enabled = True
            End If

            If mCompanyDetail.IsSyncApplication Then
                EditViewRecord.Visible = False
                DeleteRecord.Visible = False
            End If
        Next
    End Sub
#End Region

#Region "DataBind"
    Private Sub DataFieldBind()
        If Not IsDate(StartDate) Or Not IsDate(EndDate) Then
            'CNDC
            txtFromDate.Text = ""
            txtToDate.Text = ""
        Else
            txtFromDate.Text = StartDate
            txtToDate.Text = EndDate
        End If
        chkCancel.Checked = IsCancel
        chkDelay.Checked = IsDelay
        chkReliability.Checked = Reliability

        mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        If mMachineNameValueList.Count <> 0 Then
            If IsNothing(AircraftId) Then AircraftId = mMachineNameValueList(1).ID.ToString Else AircraftId = AircraftId
        Else
            AircraftId = "00000000-0000-0000-0000-000000000000"
        End If
        Session("AircraftId") = AircraftId
        cmbAircraft.DataBind()

        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly 'Added by Saylee on 06-Nov-2015 for ALL05112015 - Restrict User from using ReadOnly Aircraft
        Session("IsReadOnly") = IsReadOnly
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        If mCompanyDetail.IsSyncApplication Then
            MachineIDs.Append("<MachineID>")
            MachineIDs.Append("<id>")
            MachineIDs.Append(AircraftId)
            MachineIDs.Append("</id>")
            MachineIDs.Append("</MachineID>")
        End If
        mFligthDelayAndCancellationList = FligthDelayAndCancellationList.GetFlightDCList(New Guid(AircraftId),
                                                                                         txtFromDate.Text,
                                                                                         txtToDate.Text,
                                                                                         chkDelay.Checked,
                                                                                         chkCancel.Checked,
                                                                                         chkReliability.Checked,
                                                                                         MachineIDStr:=MachineIDs.ToString,
                                                                                         IsSyncApplication:=mCompanyDetail.IsSyncApplication)
        Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
        dgFlightDC.DataSource = mFligthDelayAndCancellationList
        dgFlightDC.DataBind()

        If mMachineNameValueList.Count > 1 And IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 1 Else cmbAircraft.SelectedValue = AircraftId
        AircraftId = cmbAircraft.SelectedValue
        Session("AircraftId") = AircraftId

    End Sub
    Private Sub SetControl()
        If Not IsDate(StartDate) Or Not IsDate(EndDate) Then
            txtFromDate.Text = ""
            txtToDate.Text = ""
        Else
            txtFromDate.Text = StartDate
            txtToDate.Text = EndDate
        End If

        txtFromDate.DataBind()
        txtToDate.DataBind()

        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text

        chkDelay.Checked = IsDelay
        chkCancel.Checked = IsCancel
        chkReliability.Checked = Reliability
        FindNow()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbAircraft.Enabled = True Then
                cmbAircraft.Focus()
            End If
            Session("MiddleFrame") = "wfFlightDelayCancellationList_Ajax.aspx"
            IsCancel = True
            IsDelay = True
            DataFieldBind()
            SetControl()
            SetPage()
            ControlVisibility()
            SetGrid()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            FindNow()
        Else
            mFligthDelayAndCancellationList = Nothing
            Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
            dgFlightDC.DataSource = Nothing
            dgFlightDC.DataBind()
        End If
        SetPage()
        SetGrid()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub dgFlightDC_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFlightDC.RowCommand
        Dim Index As Int32
        Dim ID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgFlightDC.PageIndex * dgFlightDC.PageSize
                Session("Index") = Index
                If (Not User.IsInRole("FlightDelayCancellationView") And Not User.IsInRole("FlightDelayCancellationEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                ID = mFligthDelayAndCancellationList(Index).ID
                EditRecord(ID)
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgFlightDC.PageIndex * dgFlightDC.PageSize
                Session("Index") = Index
                If Not User.IsInRole("FlightDelayCancellationDelete") Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(Index)
            Case "ViewRec"
                Index = CInt(e.CommandArgument) + dgFlightDC.PageIndex * dgFlightDC.PageSize
                Session("Index") = Index
                ID = mFligthDelayAndCancellationList(Index).ID

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                ID = mFligthDelayAndCancellationList(Index).ID
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgFlightDC_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgFlightDC.PageIndexChanging
        dgFlightDC.PageIndex = e.NewPageIndex
        dgFlightDC.DataSource = mFligthDelayAndCancellationList
        Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
        dgFlightDC.DataBind()
        SetGrid()
        ControlVisibility()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddNewTop.Click
        'Added By vikrant On 16-July-2014
        If (Not User.IsInRole("FlightDelayCancellationNew")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'End
        If Not IsValid Then Exit Sub
        Session("mFligthDelayAndCancellationList") = Nothing
        mFligthDelayAndCancellation = FligthDelayAndCancellation.NewFlightDelayCancellation(New Guid(cmbAircraft.SelectedValue))
        mFligthDelayAndCancellation.RegNo = (cmbAircraft.SelectedItem.ToString)
        Session("mFligthDelayAndCancellation") = mFligthDelayAndCancellation
        mEventLogDetail = "Reg No : " & mFligthDelayAndCancellation.RegNo & ", Dated : " & mFligthDelayAndCancellation.DateFormatted
        MarkLog(Util.Action.[New], ModuleName, mEventLogDetail, Util.ErrorType.NoError, mFligthDelayAndCancellation.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfFlightDelayCancellation_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        Page.Validate()
        IsReadOnly = mMachineNameValueList(New Guid(cmbAircraft.SelectedValue)).IsReadOnly
        Session("IsReadOnly") = IsReadOnly
        If IsValid Then
            FindNow()
            SetPage()
            SetGrid()
        Else
            mFligthDelayAndCancellationList = Nothing
            Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
            dgFlightDC.DataSource = Nothing
            dgFlightDC.DataBind()
            SetPage()
        End If

        ControlVisibility()
        If cmbAircraft.Enabled = True Then
            cmbAircraft.Focus()
        End If
        upnlGrid.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
    End Sub
    Private Sub dgFlightDC_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgFlightDC.Sorting
        mFligthDelayAndCancellationList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mFligthDelayAndCancellationList") = mFligthDelayAndCancellationList
        dgFlightDC.DataSource = mFligthDelayAndCancellationList
        dgFlightDC.DataBind()
        SetGrid()
        ControlVisibility()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region " Report "

#Region "Report Variable Declaration"
    'Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String = ""
    Private SearchStr2 As String = ""
    Private SearchStr3 As String = ""
    Private SearchStr4 As String = ""
    Private SearchStr5 As String = ""
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If Not User.IsInRole("FlightDelayCancellationPrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", IsTagRequired:=False), True)
            Exit Sub
        End If
        If mFligthDelayAndCancellationList Is Nothing OrElse mFligthDelayAndCancellationList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        If AppSettings("ClientCode") = "APFT" Or AppSettings("ClientCode") = "AAP" Then 'Added By Prashant  31-Jul-2018 APFT31082018
            Rpt = New crFlightDelayCancellationListForAPFT 'Added By Prashant  31-Jul-2018 APFT31082018
        Else
            Rpt = New crFlightDelayCancellationList
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "The report shows records filtered by the following criteria"
        SearchStr2 = "Aircraft :" + "  " + cmbAircraft.SelectedItem.Text
        If StartDate = "" Then
            SearchStr3 = ""
        Else
            SearchStr3 = "Start Date :" + "  " + txtFromDate.Text    'StartDate
        End If
        If EndDate = "" Then
            SearchStr4 = ""
        Else
            SearchStr4 = "End Date :" + "  " + txtToDate.Text    'EndDate
        End If

        'SearchStr5 = "Delay "
        ReportDetails.Add(New rptStatus(, 0, , , , , dgFlightDC.Columns.Item(1).HeaderText, ,
                              dgFlightDC.Columns.Item(2).HeaderText, dgFlightDC.Columns.Item(3).HeaderText, dgFlightDC.Columns.Item(4).HeaderText,
                             dgFlightDC.Columns.Item(5).HeaderText, dgFlightDC.Columns.Item(6).HeaderText,
                             IIf(AppSettings("ClientCode") = "APFT" Or AppSettings("ClientCode") = "AAP", "First Flight planned", dgFlightDC.Columns.Item(7).HeaderText), IIf(AppSettings("ClientCode") = "APFT", "First Chocks off Time", dgFlightDC.Columns.Item(8).HeaderText), IIf(AppSettings("ClientCode") = "APFT", "Delay", dgFlightDC.Columns.Item(9).HeaderText)))

        Dim TotalCount As Integer
        TotalCount = Me.mFligthDelayAndCancellationList.Count
        Dim m As Integer

        For m = 0 To TotalCount - 1
            Dim str(15) As String
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            str(9) = ""
            str(10) = ""
            str(11) = ""
            str(12) = ""
            str(13) = ""
            str(14) = ""
            str(15) = ""
            If Me.dgFlightDC.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgFlightDC.Rows(m).Cells(1).Text
            If Me.dgFlightDC.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgFlightDC.Rows(m).Cells(2).Text
            If Me.dgFlightDC.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgFlightDC.Rows(m).Cells(3).Text
            If Me.dgFlightDC.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgFlightDC.Rows(m).Cells(4).Text
            If Me.dgFlightDC.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgFlightDC.Rows(m).Cells(5).Text
            If Me.dgFlightDC.Rows(m).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgFlightDC.Rows(m).Cells(6).Text
            If Me.dgFlightDC.Rows(m).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgFlightDC.Rows(m).Cells(7).Text
            If Me.dgFlightDC.Rows(m).Cells(8).Text <> "&nbsp;" Then str(7) = Me.dgFlightDC.Rows(m).Cells(8).Text
            If Me.dgFlightDC.Rows(m).Cells(9).Text <> "&nbsp;" Then str(8) = Me.dgFlightDC.Rows(m).Cells(9).Text

            'If Me.dgLogList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(9) = Me.dgLogList.Rows(m).Cells(10).Text
            'If Me.dgLogList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(10) = Me.dgLogList.Rows(m).Cells(11).Text
            'If Me.dgLogList.Rows(m).Cells(12).Text <> "&nbsp;" Then str(11) = Me.dgLogList.Rows(m).Cells(12).Text
            'If Me.dgLogList.Rows(m).Cells(13).Text <> "&nbsp;" Then str(12) = Me.dgLogList.Rows(m).Cells(13).Text
            'If Me.dgLogList.Rows(m).Cells(14).Text <> "&nbsp;" Then str(13) = Me.dgLogList.Rows(m).Cells(14).Text
            'If Me.dgLogList.Rows(m).Cells(15).Text <> "&nbsp;" Then str(14) = Me.dgLogList.Rows(m).Cells(15).Text
            'If Me.dgLogList.Rows(m).Cells(16).Text <> "&nbsp;" Then str(15) = Me.dgLogList.Rows(m).Cells(16).Text

            'ReportDetails.Add(New rptStatus(, 1, , , , , str(0), , str(1), _
            'str(2), str(3), str(4), str(5), _
            'str(6), str(7), str(8), str(9), _
            'str(10), str(11), str(12), _
            'str(13), str(14), str(15)))

            ReportDetails.Add(New rptStatus(, 1, , , , , str(0), , str(1),
            str(2), str(3), str(4), str(5),
            str(6), str(7), str(8))) ', str(9)))


            'ReportDetails.Add(New rptStatus(, 1, , , , , str(0), , str(1), str(2), str(3), str(4), _
            'str(6), str(7), str(8), str(10), str(11), str(12), str(13), str(14), str(15)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
   mCompanyDetail.WebSite, "Flight Delay/Cancellation List Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        da.Fill(ds, ReportDetails)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "Log", "Log List Report", Util.ErrorType.NoError, Guid.Empty)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

End Class