'Created by : Saylee
'Dated      : 29-Jul-2022

Public Class wfnWOInvoiceList_Ajax
    Inherits Page

#Region " Variable Declaration "
    Private mWOInvoice As WOInvoice
    Private mWOInvoiceList As WOInvoiceList
    Private mDistinctWOInvoiceText As DistinctTextListForWOInvoice
    Dim mDistinctWOText As nDistinctWOText

    Dim DateIndex, FromDate, ToDate, Text, StatusID, No, IsDateChecked, WOText, WONo As String
    Dim EventLogID As Guid
    Dim ttlCnt As Integer
    Dim mFileAttach As FileAttach
    Dim mTransactionListCount As TransactionListCount
    Dim mStatusList As StatusList
    Protected nWOList As nWOList
    Dim WOID As String = Guid.Empty.ToString

    Public mEventLog As EventLog
    Public mUser As User

#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mWOInvoice = Session("mWOInvoice")
        mWOInvoiceList = Session("mWOInvoiceList")
        mDistinctWOInvoiceText = Session("mDistinctWOInvoiceText")
        Text = Session("Text")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        DateIndex = Session("DateIndex")
        StatusID = Session("StatusID")
        mTransactionListCount = Session("mTransactionListCount")
        IsDateChecked = Session("IsDateChecked")
        WOID = Session("WOID")
        nWOList = Session("nWOList")
        mUser = Session("mUser")
        mDistinctWOText = Session("mDistinctWOText")
        WOText = Session("WOText")
        WONo = IIf(IsNothing(Session("WONo")), 0, Session("WONo"))
    End Sub

    Private Sub SetSession()
        Session("mWOInvoice") = mWOInvoice
        Session("mWOInvoiceList") = mWOInvoiceList
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID
        Session("No") = No
        Session("mDistinctWOText") = mDistinctWOText
        Session("Text") = Text
        Session("IsDateChecked") = IsDateChecked
        Session("WOText") = WOText
        Session("WONo") = WONo
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mWOInvoice")
        Session.Remove("mWOInvoiceList")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("DateIndex")
        Session.Remove("StatusID")
        Session.Remove("No")
        Session.Remove("RegNo")
        Session.Remove("Text")
        Session.Remove("mTransactionListCount")
        Session.Remove("IsDateChecked")
        Session.Remove("WOID")
        Session.Remove("nWOList")
        Session.Remove("mUser")
        Session.Remove("mDistinctWOText")
        Session.Remove("WOText")
        Session.Remove("WONo")
    End Sub

    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfnWOInvoiceList_Ajax.aspx") <= 0 Then
            RemoveSession()
            Session.Remove("mWOInvoiceList")
            Session.Remove("IsPageLoadedForFirstTime")
        End If
    End Sub

    Private Sub AddAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value)")
        txtWONo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtWONo').value)")
    End Sub

    Private Sub SetGrid()
        btnAddNew.Visible = IIf(AppSettings("ClientCode") <> "A3S", True, False)
    End Sub

    Private Sub SetPeriod(Index As Int32)
        Select Case Index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate
        End Select
    End Sub

    Private Sub SetVariables()

        DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        Text = IIf(cmbInvoice.SelectedIndex <= 0, "", cmbInvoice.SelectedValue)
        StatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        No = txtNo.Text.Trim
        IsDateChecked = chkDate.Checked
        WOID = IIf(cmbWorkOrder.SelectedIndex <= 0, Guid.Empty.ToString, cmbWorkOrder.SelectedValue)
        WOText = IIf(cmbWorkOrder.SelectedIndex <= 0, "", cmbWorkOrder.SelectedValue)
        WONo = txtWONo.Text.Trim

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID
        Session("WOID") = WOID
        Session("Text") = Text
        Session("No") = No
        Session("WOText") = WOText
        Session("WONO") = WONo
        Session("IsDateChecked") = IsDateChecked

    End Sub

    Private Sub FindNow(Optional Text As String = "",
                        Optional No As Integer = 0,
                        Optional FromDate As String = "",
                        Optional ToDate As String = "",
                        Optional StatusID As Integer = 0,
                        Optional AddTopItem As String = "",
                        Optional IsDateChecked As String = "",
                        Optional WOID As String = "{00000000-0000-0000-0000-000000000000}",
                        Optional WOText As String = "",
                        Optional WoNo As Integer = 0)
        mWOInvoiceList = Nothing
        dgInvoiceList.DataSource = Nothing
        mWOInvoiceList = WOInvoiceList.GetWOInvoiceList(FromDate, ToDate, Text, No, StatusID, WOText:=WOText, WONo:=WoNo)
        dgInvoiceList.DataSource = mWOInvoiceList
        Session("mWOInvoiceList") = mWOInvoiceList
    End Sub

    Private Sub SetControl()
        SetPeriod(DateIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        No = IIf(No Is Nothing, txtNo.Text.Trim, No)
        Text = IIf(Text Is Nothing, IIf(cmbInvoice.SelectedIndex <= 0, "", cmbInvoice.SelectedValue), Text)
        WOID = IIf(cmbWorkOrder.SelectedIndex <= 0, Guid.Empty.ToString, cmbWorkOrder.SelectedValue)
        WOText = IIf(cmbWorkOrder.SelectedIndex <= 0, "", cmbWorkOrder.SelectedValue)
        WONo = IIf(WONo Is Nothing, txtWONo.Text.Trim, WONo)
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("DateIndex") = DateIndex
        Session("StatusID") = StatusID
        Session("StatusId") = StatusID
        Session("No") = No
        Session("Text") = Text
        Session("IsDateChecked") = IsDateChecked
        Session("WOID") = WOID
        Session("WONo") = WONo
        Session("WOText") = WOText
        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
        txtFromDate.Text = FromDate
        txtToDate.Text = ToDate
        txtNo.Text = No
        cmbWorkOrder.SelectedValue = IIf(WOID = Guid.Empty.ToString, "(ALL)", WOID)
        cmbStatus.SelectedValue = StatusID

        If mDistinctWOInvoiceText.Contains(Text) Then
            cmbInvoice.SelectedValue = IIf(Text = "", "(ALL)", Text)
        Else
            cmbInvoice.SelectedValue = "(ALL)"
        End If

        chkDate.Checked = IIf(IsDateChecked Is Nothing, True, IsDateChecked)
        txtNo.Text = No
        mUser = CType(Session("mUser"), User)
        mEventLog = Session("mEventLog")

        If mUser Is Nothing Then mUser = SI.UTILITY.User.GetUser(mEventLog.UserID)
        Session("mUser") = mUser

        If mDistinctWOText.Contains(WOText) Then
            cmbWorkOrder.SelectedValue = IIf(WOText = "", "(ALL)", WOText)
        Else
            cmbWorkOrder.SelectedValue = "(ALL)"
        End If
        txtWONo.Text = WONo

        FindNow(Text,
                Val(No),
                FromDate,
                ToDate,
                Val(StatusID),
                "",
                IIf(IsDateChecked Is Nothing, True, IsDateChecked),
                WOText:=WOText,
                WoNo:=WONo)
        dgInvoiceList.DataBind()
        cmbDate.SelectedIndex = DateIndex
        cmbInvoice.SelectedValue = IIf(Text = "", "(ALL)", Text)
        txtNo.Text = No

        ControlVisibility(DateIndex)
        dgInvoiceList.DataBind()

        If mWOInvoiceList.Count > 0 And mWOInvoiceList.Count <> mWOInvoiceList.TotalRecords Then
            lblResult.Text = "List of Invoice(s) as per criteria : Recent " & mWOInvoiceList.Count & " of " & mWOInvoiceList.TotalRecords.ToString & " Record(s)."
        Else
            lblResult.Text = "List of Invoice(s) as per criteria : " & mWOInvoiceList.Count & " Record(s)."
        End If
    End Sub

    Private Sub ControlVisibility(Optional ByVal DateIndex As Int32 = 0)
        If DateIndex = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub

    Private Sub SetTitle()
        lbltitle.InnerText = "List of Invoice(s) " & "  [ Total No of Record(s) :- " + mWOInvoiceList.TotalRecords.ToString() + " ]"
        upnltitle.Update()
    End Sub

    Private Sub GetAttachment(ID As Guid, mIsAttachmentAdded As Boolean) 'Added By Vikrant On 01-Dec-2014
        If mIsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 1)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim TextNo As String
                        Try
                            Dim mWOInvoice As WOInvoice
                            Session("sender") = ""
                            mWOInvoice = CType(Session("mWOInvoice"), WOInvoice)
                            TextNo = mWOInvoice.WOInvoiceNumber + " Dated : " + mWOInvoice.WOInvoiceDateFormatted.ToString
                            WOInvoice.DeleteWOInvoice(mWOInvoice.ID)
                            DataFieldBind()
                            SetControl()
                            SetGrid()
                            upnlGrid.Update()
                            upnlResult.Update()
                        Catch ex As SqlException
                            Dim UseInstr As String = String.Empty
                            If ex.Message.Contains("FKtabReqtabCWP") Then
                                UseInstr = "Requisition"
                            ElseIf ex.Message.Contains("FKtabMROInvoicetabCWP") Then
                                UseInstr = "Invoice"
                            End If
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, UseInstr, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "nWOInvoice", "Can't delete : " + TextNo + " is Currently in use", Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)
                            End If
                            DataFieldBind()
                            SetControl()
                            SetGrid()
                            upnlGrid.Update()
                            upnlResult.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DeletedSuccessFully, MSGBox.Message_text.DeletedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "nWOInvoice", TextNo, Util.ErrorType.NoError, mWOInvoice.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok  'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub

#End Region

#Region " Data Bind "

    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        mDistinctWOInvoiceText = DistinctTextListForWOInvoice.GetDistinctTextList(IsSelectTagRequired:=True, Tag:="(ALL)")
        cmbInvoice.DataSource = mDistinctWOInvoiceText
        Session("mDistinctWOInvoiceText") = mDistinctWOInvoiceText
        mStatusList = StatusList.GetStatusList(0, IsSelectTagRequired:=True)
        cmbStatus.DataSource = mStatusList
        Session("mStatusList") = mStatusList
        mDistinctWOText = nDistinctWOText.GetDistinctWOText("(ALL)")
        cmbWorkOrder.DataSource = mDistinctWOText
        Session("mDistinctWOText") = mDistinctWOText
        DataBind()
    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearAll()
        AddAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfnWOInvoiceList_Ajax.aspx"
            chkDate.Checked = True
            mEventLog = EventLog.GetEventLog(CType(Session("EventLogID"), Guid))
            Session("mEventLog") = mEventLog
            DataFieldBind()
            SetControl()

            'Added by Harsh on 29th March 2024
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "nWOInvoice") Then
                ScriptManager.RegisterStartupScript(Me, [GetType], "MarkAsFavourite", "MarkAsFavourite();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, [GetType], "RemoveFromFavourite", "RemoveFromFavourite();", True)
            End If

        End If

        SetGrid()
        SetTitle()
    End Sub

    Private Sub CmbDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        SetPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            SetFocus(cmbDate)
        End If

        SetVariables()
        SetGrid()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlResult.Update()
        upnlInvoice.Update()
        upnlInvoiceNo.Update()
        upnlInvoicelblNo.Update()
    End Sub

    Protected Sub SearchBtn(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click

        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(DateIndex)
        SetVariables()
        FindNow(Text, Val(No), FromDate, ToDate, Val(StatusID), "", IsDateChecked, WOID, WOText:=WOText, WoNo:=WONo)
        dgInvoiceList.DataBind()
        SetGrid()
        ControlVisibility()
        lblResult.Text = "List of Invoice(s) as per criteria : " & mWOInvoiceList.Count & " Record(s)."
        upnlGrid.Update()
        upnlResult.Update()
        upnlInvoice.Update()
        upnlInvoiceNo.Update()
        upnlInvoicelblNo.Update()
        If mWOInvoiceList.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
    End Sub

    Private Sub GridViewSorting(sender As Object, e As GridViewSortEventArgs) Handles dgInvoiceList.Sorting
        mWOInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgInvoiceList.DataSource = mWOInvoiceList
        Session("mWOInvoiceList") = mWOInvoiceList
        dgInvoiceList.DataBind()
        SetGrid()
    End Sub

    Private Sub GridViewPageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgInvoiceList.PageIndexChanging
        dgInvoiceList.PageIndex = e.NewPageIndex
        dgInvoiceList.DataSource = mWOInvoiceList
        Session("mCWPList") = mWOInvoiceList
        dgInvoiceList.DataBind()
        SetGrid()
    End Sub

    Private Sub GridViewRowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgInvoiceList.RowCommand

        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgInvoiceList.PageSize * dgInvoiceList.PageIndex
                Dim mID As Guid = mWOInvoiceList(Index).ID
                mWOInvoice = WOInvoice.GetWOInvoice(mID)

                If (Not User.IsInRole("WOInvoiceView") And Not User.IsInRole("WOInvoiceEdit")) Then
                    SetSession()
                    MarkLog(Action.Edit, "nWOInvoice", User.Identity.Name & " is not Authorized User to edit " + mWOInvoice.WOInvoiceNumber, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Session("mWOInvoice") = mWOInvoice
                Dim mWOInvoiceDetail As String = "Contract : " + mWOInvoice.WOInvoiceNumber + " dated : " + mWOInvoice.WOInvoiceDateFormatted
                MarkLog(Action.Edit, "nWOInvoice", mWOInvoiceDetail, ErrorType.NoError, mWOInvoice.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, [GetType](), "OpenScript", "openLedgerSame('wfnWOInvoice_Ajax.aspx?BackPage=Index.aspx');", True)

            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgInvoiceList.PageSize * dgInvoiceList.PageIndex
                Dim mID As Guid = mWOInvoiceList(Index).ID
                mWOInvoice = WOInvoice.GetWOInvoice(mID)

                If (Not User.IsInRole("WOInvoiceDelete")) Then
                    SetSession()
                    MarkLog(Action.Delete, "nWOInvoice", User.Identity.Name & " is not Authorized User to delete " + mWOInvoice.WOInvoiceNumber, Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                    Session("mWOInvoice") = mWOInvoice
                End If

        End Select

    End Sub

    Private Sub AddNew(sender As Object, e As EventArgs) Handles btnAddNew.Click
        mWOInvoice = WOInvoice.NewWOInvoice(Guid.Empty)
        MarkLog(Action.[New], "nWOInvoice", "", ErrorType.NoError, mWOInvoice.ID, EventLogID)
        Session("mWOInvoice") = mWOInvoice
        SetGrid()
        upnlGridView.Update()
        ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", "openLedgerSame('wfnPendingWOListForInvoice_Ajax.aspx?BackPage=Index.aspx');", True)
    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()

        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    'Added by Harsh on 29th March 2024
    Private Sub HdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click
        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "nWOInvoice")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

    Private Sub HdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click
        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "nWOInvoice")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

#End Region

End Class