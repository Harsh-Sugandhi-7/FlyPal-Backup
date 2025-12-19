Imports System.Linq
Imports System.Text
Public Class wfComponentReservationList_Ajax
    Inherits System.Web.UI.Page

#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        Authorized = 7
    End Enum
#End Region

#Region " Variable Declaration "
    Public mComponentReservationList As ComponentReservationList
    Public mComponentReservation As ComponentReservation
    Public mDistinctTextListForReceipt As DistinctTextListForReceipt
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptReceiptCumInvReg
    Dim SearchIndex, DateIndex, FromDate, ToDate, ReceiptText, Name, ReceiptNo As String
    Public mModuleName As String
    Public Tital As String
    Public mDocumentTypeForID As Integer
    Public mAttachToID As Guid
    Public mName As String
    Dim EventLogID As Guid
    Dim mRCIDetail As String
    Dim mTransactionListCount As TransactionListCount
    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 25
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCount As Integer = 0
    Private SerialNo As String = String.Empty
    Dim ComponentReservedList As Object = Nothing
    Dim ComponentUnscheduleUsedList As Object = Nothing
    'Dim OverReservedComponentList As Object = Nothing
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mComponentReservation = CType(Session("mComponentReservation"), ComponentReservation)
        mComponentReservationList = CType(Session("mComponentReservationList"), ComponentReservationList)
        ComponentUnscheduleUsedList = Session("mComponentUnscheduleUsedList")
        ComponentReservedList = Session("mComponentReservedList")
        'OverReservedComponentList = Session("mOverReservedComponentList")
        mDistinctTextListForReceipt = CType(Session("mDistinctTextListForReceipt"), DistinctTextListForReceipt)
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        ReceiptText = Session("ReceiptText")
        ReceiptNo = IIf(IsNothing(Session("ReceiptNo")), 0, Session("ReceiptNo"))
        mModuleName = Session("mModuleName")
        mTransactionListCount = Session("mTransactionListCount")
        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")
        totalCount = Session("totalCount")
        SerialNo = Session("SerialNo")
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mComponentReservation")
        Session.Remove("mComponentReservationList")
        Session.Remove("mDistinctTextListForReceipt")
        Session.Remove("mDistinctTextListForOrder")
        Session.Remove("mDistinctTextListForIssue")
        Session.Remove("mDistinctTextListforInvoice")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("OrderText")
        Session.Remove("ReceiptText")
        Session.Remove("IssueText")
        Session.Remove("InvoiceText")
        Session.Remove("Name")
        Session.Remove("OrderNo")
        Session.Remove("ReceiptNo")
        Session.Remove("IssueNo")
        Session.Remove("InvoiceNo")
        Session.Remove("mReceivedAs")
        Session.Remove("mTransactionListCount")
        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")
        Session.Remove("totalCount")
        Session.Remove("SerialNo")
        Session.Remove("mFileAttach")
        Session.Remove("WoNo")
        Session.Remove("WOText")
        Session.Remove("ReceivedFromType")
        Session.Remove("mComponentUnscheduleUsedList")
        Session.Remove("mID")
        'Session.Remove("mOverReservedComponentList")
        'Session.Remove("mOverReservedComponentList")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfComponentReservationList_Ajax.aspx?") <= 0 Then
            RemoveSessions()
        End If
    End Sub
    Private Sub ClearTextBoxs()
        txtReceiptNo.Text = ""
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        mComponentReservation = ComponentReservation.NewComponentReservation(Guid.NewGuid)
        Session("mComponentReservation") = mComponentReservation
    End Sub
    Private Sub EditRecord(ByVal mID As Guid)
        mComponentReservation = ComponentReservation.GetComponentReservation(mID)
        Session("mComponentReservation") = mComponentReservation
    End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        Session("mID") = mID
    End Sub
    Private Sub SetControl()
        'SetPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgComponentReservationList.DataBind()
        'cmbSearchCriteria.SelectedIndex = SearchIndex
        'cmbPeriod.SelectedIndex = DateIndex
        If cmbReceiptText.Items.Contains(New System.Web.UI.WebControls.ListItem(ReceiptText)) Then
            cmbReceiptText.SelectedValue = ReceiptText
        Else
            cmbReceiptText.SelectedValue = "(All)"
        End If
        txtSearchFor.Text = Name
        txtReceiptNo.Text = ReceiptNo
        ControlVisibility(SearchIndex, DateIndex, cmbReceiptText.SelectedIndex)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim ComponentReservationDetail As String
                        Dim mDetails As String = String.Empty
                        Try
                            Dim mId As New Guid
                            Session("sender") = ""
                            mId = Session("mID")
                            Dim IssueInfo As String = ""
                            mComponentReservation = ComponentReservation.GetComponentReservation(mId)
                            If mComponentReservationList.Count > 0 And mComponentReservationList(mComponentReservation.ID).IssueTo <> "" Then
                                IssueInfo = mComponentReservationList(mComponentReservation.ID).IssueDate + " " + mComponentReservationList(mComponentReservation.ID).IssueDateFormatted + " " + mComponentReservationList(mComponentReservation.ID).IssueTo
                            End If
                            ComponentReservationDetail = "Reserved Date:- " + mComponentReservation.ReserveForDateFormatted + " Reserved For Reg.No.:- " + mComponentReservation.RegNo + " Part No.:- " + mComponentReservation.PartNo + " Serial No.:- " + mComponentReservation.SerialNo + IIf(IssueInfo = "", "", " Issue Info:- " + IssueInfo)
                            mComponentReservation = ComponentReservation.GetComponentReservation(mId)
                            mComponentReservation.DeleteComponentReservation(mId)
                            Session("mComponentReservation") = mComponentReservation
                            DataFieldBind()
                            SetControl()
                            'UpdateItemGridView()
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Message.Contains("tabInvoiceItem") Then
                                stringInfo = "Invoice."
                            ElseIf ex.Message.Contains("tabIssueItem") Then
                                stringInfo = "Issue."
                            ElseIf ex.Message.Contains("tabOrderItem") Then
                                stringInfo = "Order."
                            ElseIf ex.Message.Contains("tabConditionCheckItem") Then
                                stringInfo = "Equipment Maintenance."
                            ElseIf ex.Message.Contains("tabCalibrationItem") Then
                                stringInfo = "Calibration."
                            ElseIf ex.Message.Contains("tabOtherChargeInvoices") Then
                                stringInfo = "Docket Charge."
                            ElseIf ex.Message.Contains("Can not delete record") Then
                                If User.Identity.Name.ToUpper = "BTPLAdmin".ToUpper Then
                                    stringInfo = ex.Message.Substring(ex.Message.IndexOf("use") + 3)
                                Else
                                    stringInfo = "Issue."
                                End If
                            End If
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            SetTitle()
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentReservation", ComponentReservationDetail, Util.ErrorType.NoError, mComponentReservation.ID, EventLogID)
                            End If
                            Session("ForEventLog") = "For Event Log"
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
    Private Sub FindNow(Optional ByVal Fromdate As String = "1/1/1900", _
    Optional ByVal ToDate As String = "1/1/2200", Optional ByVal Text As String = "", _
    Optional ByVal No As Integer = 0, Optional ByVal IntReceiptNo As String = "", _
    Optional ByVal VendorName As String = "", Optional ByVal AircraftName As String = "", _
    Optional ByVal StoreName As String = "", Optional ByVal DCNo As String = "", _
    Optional ByVal StatusID As Integer = 0, Optional ByVal ItemName As String = "", _
    Optional ByVal OrderText As String = "", Optional ByVal OrderNo As Integer = 0, _
    Optional ByVal IssueText As String = "", Optional ByVal IssueNo As Integer = 0, _
    Optional ByVal ReleaseNoteNo As String = "", Optional ByVal Type As Integer = 0, _
    Optional ByVal InvoiceText As String = "", Optional ByVal InvoiceNo As Integer = 0, _
    Optional ByVal CustomerName As String = "", Optional ByVal AWBNo As String = "", _
    Optional ByVal SerialNo As String = "", Optional ByVal Description As String = "", _
    Optional ByVal IsForPrint As Boolean = False, Optional ByVal ReceivedFromType As Integer = 0, _
    Optional ByVal WorkShopName As String = "", Optional ByVal WOText As String = "", Optional ByVal WONo As Integer = 0, _
    Optional ByVal BatchNo As String = "", Optional ByVal CodeNo As String = "")
        'clear the obj and grid
        mComponentReservationList = Nothing
        dgComponentReservationList.DataSource = Nothing
        'get the list

        mComponentReservationList = ComponentReservationList.GetComponentReservationList(Fromdate, ToDate, Text, No, ItemName, _
                                                                                         SerialNo:=SerialNo, ForWhat:=IIf(chkClosed.Checked = True, 2, -1))


        ComponentReservedList = (From c In mComponentReservationList
                                     Where c.IsReserve = True And c.IsUnscheduleUsed = False _
                                     Select c).ToList


        ComponentUnscheduleUsedList = (From c In mComponentReservationList
                                           Where c.IsUnscheduleUsed = True _
                                        Select c).ToList

        'OverReservedComponentList = (From c In mComponentReservationList
        '                             Where c.IsReserve = True And c.IsUnscheduleUsed = False And CDate(c.ReserveForDate) < CDate(ToDate) And c.IssueTo = "" _
        '                             Select c).ToList

        Session("mComponentReservationList") = mComponentReservationList

        dgComponentReservationList.DataSource = ComponentReservedList
        dgComponentReservationList.DataBind()
        Session("mComponentReservedList") = ComponentReservedList

        dgComponentUnscheduleUsedList.DataSource = ComponentUnscheduleUsedList
        dgComponentUnscheduleUsedList.DataBind()
        Session("mComponentUnscheduleUsedList") = ComponentUnscheduleUsedList

        'dgOverReservedComponentList.DataSource = OverReservedComponentList
        'dgOverReservedComponentList.DataBind()
        'Session("mOverReservedComponentList") = OverReservedComponentList

        lblResult.Text = "List of Reserved Component as per criteria: " & ComponentReservedList.Count.ToString & " Record(s) found."
        lblComponentUnscheduleUsedResult.Text = "List of Unscheduled Issued Component as per criteria: " & ComponentUnscheduleUsedList.Count.ToString & " Record(s) found."
        'lblResultOfOverReservedComponentList.Text = "List of Over Reserved Component as per criteria: " & OverReservedComponentList.Count.ToString & " Record(s) found."
        If chkClosed.Checked = True Then
            dgComponentReservationList.Columns(8).Visible = False 'Edit
            dgComponentReservationList.Columns(9).Visible = False 'Delete
        Else
            dgComponentReservationList.Columns(8).Visible = True  'Edit
            dgComponentReservationList.Columns(9).Visible = True 'Delete
        End If
        upnlGridView.Update()
        upnlComponentUnscheduleUsedList.Update()
        'Dim Cnt As Integer = ComponentReservedList.Count + ComponentUnscheduleUsedList.Count
        'lblList.Text = "List of Reserved Component " + " [Total No of Record(s):-" + Cnt.ToString() + "]"
        'upnlTitle.Update()
        'upnlOverReservedComponentList.Update()
    End Sub
    Private Sub CallFindNow(ByVal Indx As Int32)
        FindNow(txtFromDate.Text, txtToDate.Text, Trim(ReceiptText), CInt(Val(ReceiptNo)), "", "", "", "", "", 0, Trim(Name), _
                "", 0, "", 0, "", 0, "", 0, SerialNo:=txtSerailNo.Text.Trim, IsForPrint:=False)
        dgComponentReservationList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal PeriodIndex As Int32 = 0, _
                                    Optional ByVal OrdTxt As Int32 = 0, Optional ByVal RectTxt As Int32 = 0, _
                                    Optional ByVal IssTxt As Int32 = 0, Optional ByVal InvTxt As Int32 = 0)
        'cmbPeriod.Visible = CBool(IIf(SearchIndex = 1, True, False))
        'lblFromDate.Visible = CBool(IIf(SearchIndex = 1 And PeriodIndex <> 0, True, False))
        'lblToDate.Visible = CBool(IIf(SearchIndex = 1 And PeriodIndex <> 0, True, False))
        'cmbReceiptText.Visible = CBool(IIf(SearchIndex = 2, True, False))
        'txtReceiptNo.Visible = (SearchIndex = 2 And RectTxt > 0)

        'lblNo.Visible = (SearchIndex >= 2 And SearchIndex <= 5) And (OrdTxt > 0 Or RectTxt > 0 Or IssTxt > 0 Or InvTxt > 0)
        'If SearchIndex = 1 And PeriodIndex = 6 Then
        '    txtFromDate.Visible = True
        '    txtToDate.Visible = True
        '    txtFromDate.Enabled = True
        '    txtToDate.Enabled = True
        'ElseIf SearchIndex = 1 And (PeriodIndex = 1 Or PeriodIndex = 2 Or PeriodIndex = 3 Or PeriodIndex = 4 Or PeriodIndex = 5) Then
        '    txtFromDate.Visible = True
        '    txtToDate.Visible = True
        '    txtFromDate.Enabled = False
        '    txtToDate.Enabled = False
        'Else
        '    txtFromDate.Visible = False
        '    txtToDate.Visible = False
        'End If
        'txtSearchFor.Visible = IIf(SearchIndex = 3 Or SearchIndex = 4, True, False)
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
                txtToDate.Text = ToDate 'Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
    Private Sub setVariables()
        'SearchIndex = IIf(cmbSearchCriteria.SelectedIndex < 0, 0, cmbSearchCriteria.SelectedIndex)
        'DateIndex = IIf(cmbPeriod.SelectedIndex < 0, 0, cmbPeriod.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        ReceiptText = IIf(cmbReceiptText.SelectedIndex <= 0, "", cmbReceiptText.SelectedValue)
        Name = txtSearchFor.Text.Trim
        ReceiptNo = txtReceiptNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("ReceiptText") = ReceiptText
        Session("Name") = Name
        Session("ReceiptNo") = ReceiptNo
    End Sub
    Private Sub ClearControls()
        'cmbPeriod.SelectedIndex = 0
        cmbReceiptText.SelectedIndex = 0
        txtSearchFor.Text = ""
        txtReceiptNo.Text = ""
    End Sub
    Private Sub SetTitle()
        'Dim mTransTypeList As TransactionList
        'mTransTypeList = TransactionList.GetTransactionList()
        'lblList.Text = "List of Reserved Component"
        ''lblList.Text = "List of Goods Receipt " + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"
        'upnlTitle.Update()
    End Sub
    Private Sub addAttributes()
        txtReceiptNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtReceiptNo').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 4, DateIndex) 'Last one Year
        mDistinctTextListForReceipt = DistinctTextListForReceipt.GetDistinctTextList("13", , True, "(All)")
        cmbReceiptText.DataSource = mDistinctTextListForReceipt
        DataBind()
    End Sub

    Private Sub UpdateItemGridView()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            'If cmbSearchCriteria.Enabled = True Then
            '    setFocus(cmbSearchCriteria)
            'End If
            Session("MiddleFrame") = "wfComponentReservationList_Ajax.aspx?"
            txtFromDate.Text = CDate(Today.AddMonths(-3)).ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = CDate(Today.AddMonths(3)).ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
            SetControl()
            SetTitle()
        End If
    End Sub
    Private Sub dgComponentReservationList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgComponentReservationList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                'Dim index As Integer = CInt(e.CommandArgument) + dgComponentReservationList.PageIndex * dgComponentReservationList.PageSize
                'Dim mID As Guid = mComponentReservationList(index).ID
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ComponentReservationView") And Not User.IsInRole("ComponentReservationEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                EditRecord(mID)
                'UpdateItemGridView()
                SetTitle()
                'mRCIDetail = mComponentReservation.ReceiptNo + " Dated : " + mComponentReservation.RecCumInvDateFormatted + " from " + mComponentReservationList(mComponentReservation.ID).Name
                MarkLog(Util.Action.Edit, "ComponentReservation", mRCIDetail, Util.ErrorType.NoError, mComponentReservation.ID, EventLogID) 'End
                Dim str As String
                str = "openledgersame('wfComponentReservation_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                'Dim index As Integer = CInt(e.CommandArgument) + dgComponentReservationList.PageIndex * dgComponentReservationList.PageSize
                'Dim mID As Guid = mComponentReservationList(index).ID
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("ComponentReservationDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub dgComponentReservationList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgComponentReservationList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CDate(e.Row.Cells(1).Text) < Today.Date Then 'If Reserved Date N Today date i.e over due or over Reserved
                e.Row.Cells(1).BackColor = Color.Aqua
            End If
            upnlGridView.Update()
        End If
    End Sub
    Private Sub dgComponentReservationList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgComponentReservationList.PageIndexChanging
        dgComponentReservationList.PageIndex = e.NewPageIndex
        dgComponentReservationList.DataSource = ComponentReservedList
        dgComponentReservationList.DataBind()
        upnlGridView.Update()
        Session("mComponentReservedList") = ComponentReservedList
    End Sub
    'Private Sub dgComponentReservationList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgComponentReservationList.Sorting
    '    Session("mComponentReservedList").Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    '    Session("mComponentReservedList") = ComponentReservedList
    '    upnlGridView.Update()
    'End Sub
    Private Sub dgComponentUnscheduleUsedList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgComponentUnscheduleUsedList.RowCommand
        Select Case e.CommandName
            Case "Reallocate"
                If Not User.IsInRole("ComponentReservationNew") Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim str As String
                Dim mID As Guid = New Guid(dgComponentUnscheduleUsedList.DataKeys(CInt(e.CommandArgument)).Values(0).ToString)
                EditRecord(mID)
                Session("ItemNameComponentReservationList") = dgComponentUnscheduleUsedList.DataKeys(CInt(e.CommandArgument)).Values(1).ToString
                Session("ReallocateComponentReservation") = "Reallocate"
                SetTitle()
                MarkLog(Util.Action.[New], "ComponentReservation", "", Util.ErrorType.NoError, mComponentReservation.ID, EventLogID)
                str = "openledgersame('wfComponentReservationStockList_Ajax.aspx?BackPage=Index.aspx&ChildPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                'Dim index As Integer = CInt(e.CommandArgument) + dgComponentReservationList.PageIndex * dgComponentReservationList.PageSize
                'Dim mID As Guid = mComponentReservationList(index).ID
                Dim mID As Guid = New Guid(dgComponentUnscheduleUsedList.DataKeys(CInt(e.CommandArgument)).Values(0).ToString)
                If (Not User.IsInRole("ComponentReservationDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub dgComponentUnscheduleUsedList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgComponentUnscheduleUsedList.PageIndexChanging
        dgComponentUnscheduleUsedList.PageIndex = e.NewPageIndex
        dgComponentUnscheduleUsedList.DataSource = ComponentUnscheduleUsedList
        upnlComponentUnscheduleUsedList.Update()
        Session("mComponentUnscheduleUsedList") = ComponentUnscheduleUsedList
    End Sub
    'Private Sub dgComponentUnscheduleUsedList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgComponentUnscheduleUsedList.Sorting
    '    ComponentUnscheduleUsedList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
    '    upnlComponentUnscheduleUsedList.Update()
    '    Session("mComponentUnscheduleUsedList") = ComponentUnscheduleUsedList
    'End Sub
    'Private Sub dgOverReservedComponentList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgOverReservedComponentList.RowCommand
    '    Select Case e.CommandName
    '        Case "DeleteRecord"
    '            Dim mID As Guid = New Guid(e.CommandArgument.ToString)
    '            If (Not User.IsInRole("ComponentReservationDelete")) Then
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
    '                Exit Sub
    '            End If
    '            DeleteRecord(mID)
    '    End Select
    'End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        dgComponentReservationList.PageIndex = 0
        mpageindex = 0
        mCurrentpage = mpageindex + 1
        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        CallFindNow(SearchIndex)
        dgComponentReservationList.DataBind()
        btnBottomPrint.Enabled = IIf(mComponentReservationList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mComponentReservationList.Count = 0, False, True)
        upnTopButtons.Update()
        upnBottomButtons.Update()
    End Sub
    'Private Sub cmbSearchCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchCriteria.SelectedIndexChanged
    '    cmbPeriod.SelectedIndex = 0
    '    ClearControls()
    '    Dim PeriodIndex As Int32 = CInt(IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0))
    '    ControlVisibility(cmbSearchCriteria.SelectedIndex, 6, 0, 0, 0, 0)
    '    SetPeriod(PeriodIndex)
    '    If cmbSearchCriteria.Enabled = True Then
    '        setFocus(cmbSearchCriteria)
    '    End If
    'End Sub
    'Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged
    '    Dim PeriodIndex As Int32 = CInt(IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0))
    '    ControlVisibility(cmbSearchCriteria.SelectedIndex, PeriodIndex, 0, 0, 0, 0)
    '    SetPeriod(PeriodIndex)
    '    If cmbPeriod.Enabled = True Then
    '        setFocus(cmbPeriod)
    '    End If
    'End Sub
    'Private Sub cmbReceiptText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptText.SelectedIndexChanged
    '    ClearTextBoxs()
    '    ControlVisibility(cmbSearchCriteria.SelectedIndex, 0, 0, cmbReceiptText.SelectedIndex, 0, 0)
    '    If cmbReceiptText.Enabled = True Then
    '        setFocus(cmbReceiptText)
    '    End If
    'End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomAddNew.Click, btnAddNewTop.Click
        If Not User.IsInRole("ComponentReservationNew") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim str As String
        NewRecord()
        SetTitle()
        MarkLog(Util.Action.[New], "ComponentReservation", "", Util.ErrorType.NoError, mComponentReservation.ID, EventLogID)
        str = "openledgersame('wfComponentReservationStockList_Ajax.aspx?BackPage=Index.aspx&ChildPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        Session("mCount") = Nothing
        mDistinctTextListForReceipt = Nothing
        mComponentReservation = Nothing
        mModuleName = Nothing
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region


End Class