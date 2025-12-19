Public Class wfNRCList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Private mNRCList As NRCList
    Private mNRC As NRC
    Private mATAList As ATAList
    Public mModelList As ModelList
    Dim RegNo As String = ""
    Dim SerialNo As String = ""
    Dim StatusID As Integer = 0
    Dim IDForEventLog As Guid
    Dim EventLogID As Guid
    Dim DateIndex As String = ""
    Public FromDate As String = "1-1-1900"
    Public ToDate As String = "1-1-2200"
#End Region

#Region " Methods "
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfNRCList_Ajax.aspx?" Then
            Session.Remove("mNRCList")
            Session.Remove("RegNo")
            Session.Remove("SerialNo")
            Session.Remove("Status")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("DateIndex")
            Session.Remove("mModelList")
            RegNo = Nothing
            SerialNo = Nothing
            StatusID = Nothing
        End If
    End Sub
    Public Sub GetSession()
        mNRCList = Session("mNRCList")
        RegNo = Session("RegNo")
        SerialNo = Session("SerialNo")
        StatusID = Session("Status")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        DateIndex = Session("DateIndex")
        mModelList = CType(Session("mModelList"), ModelList)
    End Sub
    Public Sub RemoveSession()
        Session.Remove("mNRCList")
        Session.Remove("RegNo")
        Session.Remove("SerialNo")
        Session.Remove("Status")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("DateIndex")
        Session.Remove("mModelList")
        RegNo = Nothing
        SerialNo = Nothing
        StatusID = Nothing
    End Sub
    Private Sub SetPeriod(ByVal index As Int32)
        Select Case index
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
        Session("FromDate") = txtFromDate.Text
        Session("ToDate") = txtToDate.Text
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Public Sub DataFieldBind()
        DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex

        mATAList = ATAList.GetATAList("", "(All)")
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        'mModelList = ModelList.GetModelList(0, "", , , "(All)")
        'cmbModel.DataSource = mModelList
        'cmbModel.DataBind()
        'Session("mModelList") = mModelList

        If Not (RegNo Is Nothing Or SerialNo Is Nothing Or StatusID = 0) Then
            mNRCList = NRCList.GetNRCList(FromDate:=txtFromDate.Text.ToString, ToDate:=txtToDate.Text.ToString, RegNo:=RegNo, Text:="", No:=0, SerialNo:=SerialNo, Model:="", ATAID:=cmbATAChapter.SelectedValue.ToString)
        Else
            mNRCList = NRCList.GetNRCList(FromDate:=txtFromDate.Text.ToString, ToDate:=txtToDate.Text.ToString, RegNo:=txtRegNo.Text.Trim, SerialNo:=txtSerialNo.Text.Trim, Model:="", ATAID:=cmbATAChapter.SelectedValue.ToString)
        End If

        Session("mNRCList") = mNRCList
        dgNRCList.DataSource = mNRCList
        dgNRCList.DataBind()

        lblResult.Text = "List of NRC(s) as per criteria : " & mNRCList.Count.ToString & " Record(s) found. "
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        Dim NRCDetail As String = ""
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mNRC = Session("mNRC")
                            IDForEventLog = mNRC.ID
                            NRCDetail = "NRC " + mNRC.NRCNumber
                            NRC.DeleteNRC(mNRC.ID)
                            DataFieldBind()
                            upnlGrid.Update()
                            BottomActionButtonVisibility()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                Dim UseInstr As String = String.Empty
                                'If ex.Message.Contains("FKtabCWPtabNRC") Then
                                '    UseInstr = "CWP"
                                'ElseIf ex.Message.Contains("FKtabMROQuotationtabNRC") Then
                                '    UseInstr = "Quotation"
                                'End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, UseInstr, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "NRC", "Can't delete : " & NRCDetail & " is Currently in use " & UseInstr, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "NRC", NRCDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        DataFieldBind()
                        upnlGrid.Update()
                        BottomActionButtonVisibility()
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Public Sub DeletedRecord(ID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mNRC = NRC.GetNRC(ID)
        Session("mNRC") = mNRC
    End Sub
    Private Sub BottomActionButtonVisibility()
        btnBottomAdd.Visible = IIf(mNRCList.Count > 25, True, False)
        btnBottomClose.Visible = IIf(mNRCList.Count > 25, True, False)
        upnlBottomActionButton.Update()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfNRCList_Ajax.aspx?"
            txtRegNo.Text = RegNo
            txtSerialNo.Text = SerialNo
            ControlVisibility(1)
            SetPeriod(1)
            cmbDate.SelectedIndex = 1
            DataFieldBind()
            txtRegNo.Focus()
            BottomActionButtonVisibility()
        End If
    End Sub
    Private Sub txtRegNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtRegNo.TextChanged, txtSerialNo.TextChanged, txtFromDate.TextChanged, txtToDate.TextChanged, cmbATAChapter.SelectedIndexChanged
        RegNo = Trim(txtRegNo.Text)
        Session("RegNo") = RegNo
        Session("SerialNo") = txtSerialNo.Text.ToString
        'Session("Status") = cmbStatus.SelectedIndex
        mNRCList = NRCList.GetNRCList(FromDate:=txtFromDate.Text.ToString, ToDate:=txtToDate.Text.ToString, RegNo:=RegNo, SerialNo:=txtSerialNo.Text.Trim, Model:=txtRegNo.Text.Trim, ATAID:=cmbATAChapter.SelectedValue.ToString)
        Session("mNRCList") = mNRCList
        dgNRCList.DataSource = mNRCList
        DataBind()
        lblResult.Text = "List of NRC(s) as per criteria : " & mNRCList.Count.ToString & " Record(s) found. "
        BottomActionButtonVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub btnAddNewTop_Click(sender As Object, e As System.EventArgs) Handles btnAddNewTop.Click, btnBottomAdd.Click
        If (Not User.IsInRole("NRCNew")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        mNRC = NRC.NewNRC()
        mNRC.NRCDate = Today.Date
        Session("mNRC") = mNRC
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNRC_Ajax.aspx?BackPage=index.aspx');", True)
    End Sub
    Private Sub dgNRCList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgNRCList.Sorting
        mNRCList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mNRC") = mNRCList
        dgNRCList.DataSource = mNRCList
        dgNRCList.DataBind()
    End Sub
    Private Sub dgNRCList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgNRCList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("NRCView") And Not User.IsInRole("NRCEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mNRC = NRC.GetNRC(ID)
                Session("mNRC") = mNRC
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfNRC_Ajax.aspx?BackPage=index.aspx');", True)
            Case "Remove"
                If (Not User.IsInRole("NRCDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                DeletedRecord(ID)
        End Select
    End Sub
    Private Sub dgMachineList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgNRCList.PageIndexChanging
        dgNRCList.PageIndex = e.NewPageIndex
        dgNRCList.DataSource = mNRCList
        Session("mNRCList") = mNRCList
        dgNRCList.DataSource = mNRCList
        dgNRCList.DataBind()
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnBottomClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
        ControlVisibility(Index)
        SetPeriod(Index)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If
        mNRCList = NRCList.GetNRCList(FromDate:=txtFromDate.Text.ToString, ToDate:=txtToDate.Text.ToString, RegNo:=txtRegNo.Text.Trim, Text:="", No:=0, SerialNo:=txtSerialNo.Text.Trim, Model:="", ATAID:=cmbATAChapter.SelectedValue.ToString)
        Session("mNRCList") = mNRCList
        dgNRCList.DataSource = mNRCList
        DataBind()
        lblResult.Text = "List of NRC(s) as per criteria : " & mNRCList.Count.ToString & " Record(s) found. "
        BottomActionButtonVisibility()
        upnlGrid.Update()
    End Sub
#End Region
End Class