Public Class wfExportInvoiceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mExportInvoice As ExportInvoice
    Public mExportInvoiceList As ExportInvoiceList
    Public mDistinctTextListForExportInvoice As DistinctTextListForOrder
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptInvoiceRegister
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, tempExportInvoice, Name, No, IssueTextForExportInvoiceList, IssueNoForExportInvoiceList As String
    Public mName As String
    Dim EventLogID As Guid
    Dim InvDetail As String
    Dim mModuleName As String = "ExportInvoice"
    Dim totcnt As Integer
    Dim mMachineName As String
    Dim mTransactionListCount As TransactionListCount 'Added By Shweta On 19-August-2013 for ALL16082013-1
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Public mDistinctTextListForIssue As DistinctTextListForIssue
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mExportInvoice = Session("mExportInvoice")
        mExportInvoiceList = Session("mExportInvoiceList")
        mDistinctTextListForExportInvoice = Session("mDistinctTextListForExportInvoice")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        tempExportInvoice = Session("tempExportInvoice")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        mTransactionListCount = Session("mTransactionListCount")
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        IssueTextForExportInvoiceList = Session("IssueTextForExportInvoiceList")
        IssueNoForExportInvoiceList = Session("IssueNoForExportInvoiceList")
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mExportInvoice")
        Session.Remove("mExportInvoiceList")
        Session.Remove("mDistinctTextListForExportInvoice")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("tempExportInvoice")
        Session.Remove("IssueTextForExportInvoiceList")
        Session.Remove("IssueNoForExportInvoiceList")
        Session.Remove("Name")
        Session.Remove("No")
        Session.Remove("mTransactionListCount")
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        Session.Remove("SerialNo")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfExportInvoiceList_Ajax.aspx?BackPage=index.aspx") <= 0 Then
            RemoveSessions()
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        mExportInvoice = Flypal.ExportInvoice.NewExportInvoice()
        mExportInvoice.ExportInvoiceDate = Today.Date
        Session("mExportInvoice") = mExportInvoice
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mExportInvoice = ExportInvoice.GetExportInvoice(mID)
        Session("mExportInvoice") = mExportInvoice
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        GridBind()
        MSGBoxCtrl.Show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mExportInvoice = Flypal.ExportInvoice.GetExportInvoice(mID)
        Session("mExportInvoice") = mExportInvoice
    End Sub
    Private Sub SetControl()
        SetPeriod(DateIndex)

        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgExportInvoiceList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgExportInvoiceList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize

        CallFindNow(SearchIndex)
        dgExportInvoiceList.DataBind()
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId

        If cmbInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(tempExportInvoice)) Then
            cmbInvoiceText.SelectedValue = IIf(tempExportInvoice = "", "(All)", tempExportInvoice)
        Else
            cmbInvoiceText.SelectedValue = "(All)"
        End If
        '-------------------------------------------------------------------------
        If cmbIssueText.Items.Contains(New System.Web.UI.WebControls.ListItem(IssueTextForExportInvoiceList)) Then
            cmbIssueText.SelectedValue = IIf(IssueTextForExportInvoiceList = "", "(All)", IssueTextForExportInvoiceList)
        Else
            cmbIssueText.SelectedValue = "(All)"
        End If

        txtName.Text = Name
        txtNo.Text = No
        txtIssueNo.Text = IssueNoForExportInvoiceList
        ControlVisibility(SearchIndex, DateIndex)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim ReceiptCumInvoiceDetail As String = String.Empty
                        Dim mFrom As String = String.Empty
                        Try
                            mExportInvoice = CType(Session("mExportInvoice"), ExportInvoice)
                            mFrom = mExportInvoiceList(mExportInvoice.ID).Consignee
                            mExportInvoice.Delete()
                            mExportInvoice.Save()
                            DataFieldBind()
                            SetControl()
                            UpdateItemGridView()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            SetTitle()
                            upnlFindNow.Update()
                            If msgCount = 0 Then
                                ReceiptCumInvoiceDetail = mExportInvoice.ExportInvoiceTextNo + " Dated : " + mExportInvoice.ExportInvoiceDateFormatted + " from " + mFrom
                                MarkLog(Util.Action.Delete, mModuleName, ReceiptCumInvoiceDetail, Util.ErrorType.NoError, mExportInvoice.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ExportInvoiceText As String = "", Optional ByVal ExportInvoiceNo As Integer = 0,
                        Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200",
                        Optional ByVal StatusID As Integer = 0, Optional ByVal Consignee As String = "",
                        Optional ByVal IssueText As String = "", Optional ByVal IssueNo As Integer = 0)
        mExportInvoiceList = Nothing
        dgExportInvoiceList.DataSource = Nothing
        'Get List From the Database as per Criteria
        mExportInvoiceList = ExportInvoiceList.GetExportInvoiceList(ExportInvoiceText, ExportInvoiceNo, FromDate, ToDate, StatusID,
                                                                    Consignee, IsCustomPaging:=True, CurrentPage:=mpageindex, PageSize:=mpageSize,
                                                                    IssueText:=IssueText, IssueNo:=IssueNo)
        'bind the list to the datagrid
        totalCount = mExportInvoiceList.TotalRecords
        pagecount = Math.Ceiling(totalCount / mpageSize)

        Session("totalCount") = totalCount
        Session("pagecount") = pagecount
        dgExportInvoiceList.DataSource = mExportInvoiceList
        dgExportInvoiceList.DataBind()
        Session("mExportInvoiceList") = mExportInvoiceList
        UpdateItemGridView()
    End Sub
    Private Sub CallFindNow(ByVal Indx As Int32)
        FindNow(ExportInvoiceText:=tempExportInvoice, ExportInvoiceNo:=CInt(Val(No)), FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text,
                StatusID:=CInt(StatusId), Consignee:=Name, IssueText:=IssueTextForExportInvoiceList, IssueNo:=CInt(Val(IssueNoForExportInvoiceList)))
        dgExportInvoiceList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFD.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblTD.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub SetPeriod(ByVal index As Int32)
        Select Case index
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
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        tempExportInvoice = IIf(cmbInvoiceText.SelectedIndex <= 0, "", cmbInvoiceText.SelectedValue)

        Name = txtName.Text.Trim
        No = txtNo.Text.Trim

        IssueTextForExportInvoiceList = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedValue)
        IssueNoForExportInvoiceList = txtIssueNo.Text.Trim

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("tempExportInvoice") = tempExportInvoice
        Session("No") = No
        Session("Name") = Name
        Session("IssueTextForExportInvoiceList") = IssueTextForExportInvoiceList
        Session("IssueNoForExportInvoiceList") = IssueNoForExportInvoiceList
    End Sub
    Private Sub ClearControls()
        txtName.Text = ""
        txtNo.Text = ""
    End Sub
    Private Sub SetTitle()
        mModuleName = "ExportInvoice"
        Session("mModuleName") = mModuleName
        totcnt = Session("totcnt")
        lblExportInvoiceList.Text = "List of Export Invoice" + " [Total No of Record(s):-" + totcnt.ToString() + "]"
        upnlTitle.Update()
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
        txtIssueNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtIssueNo').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        Session("totcnt") = totcnt
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        tempExportInvoice = Session("tempExportInvoice")
        IssueTextForExportInvoiceList = Session("IssueTextForExportInvoiceList")
        IssueNoForExportInvoiceList = Session("IssueNoForExportInvoiceList")

        mDistinctTextListForExportInvoice = DistinctTextListForOrder.GetDistinctTextList("20", , True, "(All)")
        cmbInvoiceText.DataSource = mDistinctTextListForExportInvoice
        Name = Session("Name")
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(70)
        totcnt = mTransactionListCount(0).Count
        'End
        Session("totcnt") = totcnt
        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue
        DataBind()
    End Sub
    Private Sub GridBind()
        dgExportInvoiceList.DataSource = mExportInvoiceList
        dgExportInvoiceList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub UpdateItemGridView()
        Dim currentrow As Integer = mpageSize * (mpageindex)
        If totalCount = 0 Then
            lblResult.Text = "List of Export Invoice as per criteria :" & totalCount & " Record(s) found."
        Else
            lblResult.Text = "List of Export Invoice as per criteria :" & currentrow + 1 & " to " & currentrow + mExportInvoiceList.Count & " of " & totalCount & " Record(s) found."
        End If

        SliderExtender1.Minimum = 1
        SliderExtender1.Maximum = pagecount
        Slidercontrol.Text = mCurrentpage
        txtPageDisplay.Text = mCurrentpage
        lblpagecount.Text = pagecount
        If pagecount > 1 Then
            PnlPaging.Visible = True
        Else
            PnlPaging.Visible = False
        End If
        dgExportInvoiceList.DataBind()
        upnlGridView.Update()
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
                setFocus(cmbDate)
            End If
            Session("MiddleFrame") = "wfExportInvoiceList_Ajax.aspx?BackPage=index.aspx"
            DataFieldBind()
            SetControl()
            SetTitle()
        End If
    End Sub
    Private Sub dgExportInvoiceList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgExportInvoiceList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim mID As Guid = mExportInvoiceList(index).ID
                If (Not User.IsInRole("ExportInvoiceView") And Not User.IsInRole("ExportInvoiceEdit")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                EditRecord(mID)
                UpdateItemGridView()
                GridBind()
                SetTitle()
                InvDetail = mExportInvoiceList(mID).ExportInvoiceNumber + " Dated : " + mExportInvoiceList(mID).ExportInvoiceDateFormatted + " from " + mExportInvoiceList(mID).Consignee
                MarkLog(Util.Action.Edit, mModuleName, InvDetail, Util.ErrorType.NoError, mExportInvoice.ID, EventLogID)
                Dim str As String
                str = "openledgersame('wfExportInvoice_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim mID As Guid = mExportInvoiceList(index).ID
                If (Not User.IsInRole("ExportInvoiceDelete")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub dgExportInvoiceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgExportInvoiceList.PageIndexChanging
        dgExportInvoiceList.PageIndex = e.NewPageIndex
        mCurrentpage = e.NewPageIndex
        GridBind()
        UpdateItemGridView()
        Session("mExportInvoiceList") = mExportInvoiceList
    End Sub
    Private Sub dgExportInvoiceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgExportInvoiceList.Sorting
        mExportInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mExportInvoiceList") = mExportInvoiceList
        GridBind()
        UpdateItemGridView()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        dgExportInvoiceList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex)
        dgExportInvoiceList.DataBind()
        upnlGridView.Update()
        upnTopButtons.Update()
        upnBottomButtons.Update()
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbInvoiceText.SelectedIndexChanged
        If sender.ID = "cmbDate" Then
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            SetPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                setFocus(cmbDate)
            End If
        ElseIf sender.ID = "cmbInvoiceText" Then
            txtNo.Text = "0"
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            If cmbInvoiceText.Enabled = True Then
                setFocus(cmbInvoiceText)
            End If
        ElseIf sender.ID = "cmbIssueText" Then
            txtIssueNo.Text = "0"
            If cmbIssueText.Enabled = True Then
                cmbIssueText.Focus()
            End If
        End If
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomAddNew.Click, btnAddNewTop.Click
        NewRecord()
        If Not User.IsInRole("ExportInvoiceNew") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        SetTitle()
        MarkLog(Util.Action.[New], mModuleName, "", Util.ErrorType.NoError, mExportInvoice.ID, EventLogID)
        Dim str As String
        str = "openledgersame('wfExportInvoicePendingIssueList_Ajax.aspx?BackPage=Index.aspx&ChildPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        RemoveSessions()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnGridPaging_Click(sender As Object, e As System.EventArgs) Handles btnGridPaging.Click
        mCurrentpage = CInt(Slidercontrol.Text.Trim)
        mpageindex = mCurrentpage - 1
        dgExportInvoiceList.PageIndex = mpageindex
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(1)
        upnlFindNow.Update()
    End Sub
#End Region

End Class