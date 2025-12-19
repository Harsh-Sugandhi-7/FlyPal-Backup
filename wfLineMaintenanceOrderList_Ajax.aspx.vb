'***********************************
'Modified by Harsh Sugandhi on 22nd April 2025 for FLYPAL 2334 => Facility to attach a file to Line Maintenance Module. 
'***********************************


Public Class wfLineMaintenanceOrderList_Ajax
    Inherits Page

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

    Public mLineMaintenanceOrderList As LineMaintenanceOrderList
    Public mLineMaintenanceOrder As LineMaintenanceOrder
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public MachineID As Guid
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, OrderText, Name, No As String
    Dim EventLogID As Guid
    Public Flag As Integer = 0
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String
    Private SearchStr2 As String

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mLineMaintenanceOrder = Session("mLineMaintenanceOrder")
        mLineMaintenanceOrderList = Session("mLineMaintenanceOrderList")
        mDistinctTextListForOrder = Session("mDistinctTextListForOrder")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        OrderText = Session("OrderText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))

    End Sub

    Private Sub SetSession()

        Session("mLineMaintenanceOrder") = mLineMaintenanceOrder
        Session("mLineMaintenanceOrderList") = mLineMaintenanceOrderList
        Session("mDistinctTextListForOrder") = mDistinctTextListForOrder

    End Sub

    Private Sub RemoveSession()

        Session.Remove("mLineMaintenanceOrder")
        Session.Remove("mLineMaintenanceOrderList")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("OrderText")
        Session.Remove("Name")
        Session.Remove("No")

    End Sub

    Private Sub ClearAll()

        If InStr(Session("MiddleFrame"), "wfLineMaintenanceOrderList_Ajax.aspx") <= 0 Then
            RemoveSession()
        End If

    End Sub

    Private Sub NewRecord()

        mLineMaintenanceOrder = LineMaintenanceOrder.NewLineMaintenanceOrder()
        mLineMaintenanceOrder.OrderDate = Today.Date
        Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

    End Sub

    Private Sub EditRecord(mId As Guid)

        mLineMaintenanceOrder = LineMaintenanceOrder.GetLineMaintenanceOrder(mId)
        mLineMaintenanceOrder.MarkClean()
        Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

    End Sub

    Private Sub DeleteRecord(mId As Guid)

        GridBind()
        MSGBoxCtrl.Show(MSGBox.Message_Title.Delete, MSGBox.Message_Text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mLineMaintenanceOrder = LineMaintenanceOrder.GetLineMaintenanceOrder(mId)
        Session("mLineMaintenanceOrder") = mLineMaintenanceOrder

    End Sub

    Private Sub SetControl()

        SetPeriod(DateIndex)
        CallFindNow(SearchIndex)

        dgOrderList.DataBind()

        cmbSearchCriteria.SelectedIndex = SearchIndex
        cmbPeriod.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId

        If mDistinctTextListForOrder.Contains(OrderText) Then
            cmbOrderText.SelectedValue = IIf(OrderText = "", "(All)", OrderText)
        Else
            cmbOrderText.SelectedValue = "(All)"
        End If

        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then

            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Session("sender") = ""
                            mLineMaintenanceOrder = CType(Session("mLineMaintenanceOrder"), LineMaintenanceOrder)
                            mLineMaintenanceOrder.Delete()
                            mLineMaintenanceOrder.Save()
                            DataFieldBind()

                            SetControl()

                        Catch ex As SqlException

                            If ex.Number = 547 Then

                                MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
                                                MSGBox.Message_Text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                                Exit Sub

                            ElseIf ex.Number = 50000 Then

                                MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
                                                MSGBox.Message_Text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                                Exit Sub

                            End If

                        Finally

                            SetTitle()
                            upnlFindNow.Update()
                            Dim OrderDetail As String = mLineMaintenanceOrder.OrderNo +
                                                        " Dated : " + mLineMaintenanceOrder.OrderDateFormatted +
                                                        " To " + mLineMaintenanceOrderList(mLineMaintenanceOrder.ID).VendorName +
                                                        " Created By : " & mLineMaintenanceOrder.UserName

                            MarkLog(Action.Delete,
                                    "LineMaintenanceOrder",
                                    OrderDetail,
                                    ErrorType.NoError,
                                    mLineMaintenanceOrder.ID,
                                    EventLogID)

                        End Try

                    End If

                Case MsgBoxResult.No

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()

                    End If

            End Select

        End If

    End Sub

    Private Sub FindNow(Optional Text As String = "",
                        Optional No As Integer = 0,
                        Optional FromDate As String = "1/1/1900",
                        Optional ToDate As String = "1/1/2200",
                        Optional StatusID As Integer = 0,
                        Optional QuotationNo As String = "",
                        Optional VendorName As String = "",
                        Optional MachineName As String = "")

        mLineMaintenanceOrderList = Nothing
        dgOrderList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mLineMaintenanceOrderList = LineMaintenanceOrderList.GetOrderList(Text,
                                                                          No,
                                                                          FromDate,
                                                                          ToDate,
                                                                          StatusID,
                                                                          QuotationNo,
                                                                          VendorName,
                                                                          MachineName)
        'Set DataSource of the Grid
        dgOrderList.DataSource = mLineMaintenanceOrderList
        dgOrderList.DataBind()
        Session("mLineMaintenanceOrderList") = mLineMaintenanceOrderList
        lblResult.Text = "List of Service Order as per criteria : " & mLineMaintenanceOrderList.Count & " Record(s) found."
        btnPrint.Enabled = IIf(mLineMaintenanceOrderList.Count = 0, False, True)
        upnlGridView.Update()
        upnActionButtons.Update()

    End Sub

    Private Sub CallFindNow(Index As Integer)

        Dim tmpmTransTypeID As Trans = 0
        Select Case Index
            Case -1
                Call FindNow("", 0, FromDate, ToDate, 0, "", "", "")                    'for all records
            Case 0  'all
                Call FindNow("", 0, FromDate, ToDate, 0, "", "", "")                    'for all records
            Case 1 ' date
                Call FindNow("", 0, txtFromDate.Text, txtToDate.Text, 0, "", "", "")
            Case 2  'Order Text , No 
                Call FindNow(OrderText, CInt(Val(No)), FromDate, ToDate, 0, "", "", "")
            Case 3  'Machine
                Call FindNow("", 0, FromDate, ToDate, 0, "", "", Name)
            Case 4 ' Vendor Name
                Call FindNow("", 0, FromDate, ToDate, 0, "", Name, "")
            Case 5 ' QuotationNo
                Call FindNow("", 0, FromDate, ToDate, 0, Name, "", "")
            Case 6 ' Status
                Call FindNow("", 0, FromDate, ToDate, CInt(StatusId), "", "", "")
        End Select

        dgOrderList.PageIndex = 0

    End Sub

    Private Sub ControlVisibility(SearchIndex As Int32, Optional DateIndex As Int32 = 0)

        cmbPeriod.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)

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

        cmbOrderText.Visible = IIf(SearchIndex = 2, True, False)
        lblNo.Visible = IIf(SearchIndex = 2 And cmbOrderText.SelectedIndex <> 0, True, False)
        txtNo.Visible = IIf(SearchIndex = 2 And cmbOrderText.SelectedIndex <> 0, True, False)
        txtName.Visible = IIf((SearchIndex = 3 Or SearchIndex = 4 Or SearchIndex = 5), True, False)
        cmbStatus.Visible = IIf(SearchIndex = 6, True, False)

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

    Private Overloads Sub SetFocus(control As WebControl)
        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()
    End Sub

    Private Sub ClearControls()
        txtNo.Text = ""
        txtName.Text = ""
    End Sub

    Private Sub AddAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub

    Private Sub SetTitle()

        mLineMaintenanceOrderList = LineMaintenanceOrderList.GetOrderList("", 0, "1/1/1900", "1/1/2200", 0, "", "", "")
        lblLineMaintOrderList.Text = "List of Service Orders "
        upnlTitle.Update()

    End Sub

    Private Function IsInRole(CheckFor As Rights) As Boolean

        Dim IsInRoleString As String = "LineMaintenanceOrder"

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

#End Region

#Region " DataFieldBind "

    Private Sub DataFieldBind()

        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("17", , True, "(All)")
        Session("mDistinctTextListForOrder") = mDistinctTextListForOrder
        cmbOrderText.DataSource = mDistinctTextListForOrder
        DataBind()

    End Sub

    Private Sub GridBind()

        dgOrderList.DataSource = mLineMaintenanceOrderList
        dgOrderList.DataBind()
        upnlGridView.Update()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClearAll()
        AddAttributes()
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("sender") = "" Then

            If cmbSearchCriteria.Enabled = True Then
                SetFocus(cmbSearchCriteria)
            End If

            Session("MiddleFrame") = "wfLineMaintenanceOrderList_Ajax.aspx"
            DataFieldBind()
            SetControl()
            SetTitle()
            btnPrint.Enabled = IIf(dgOrderList.Rows.Count = 0, False, True)

        End If

    End Sub

    Private Sub GV_OrderList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgOrderList.RowCommand

        Select Case e.CommandName
            Case "EditView"

                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then

                    GridBind()

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        MessageBox.Show("You are not authorized user", False),
                                                        True)

                    Exit Sub

                End If

                Dim str As String
                Dim index As Integer = CInt(e.CommandArgument) + dgOrderList.PageIndex * dgOrderList.PageSize
                Dim mID As Guid = mLineMaintenanceOrderList(index).ID
                Dim mOrderNo As String = mLineMaintenanceOrderList(index).OrderNo
                Dim OrderDetail As String = String.Empty

                EditRecord(mID)

                OrderDetail = mLineMaintenanceOrder.OrderNo +
                              " Dated : " + mLineMaintenanceOrder.OrderDateFormatted +
                              " To " + mLineMaintenanceOrderList(mLineMaintenanceOrder.ID).VendorName &
                              " Created By : " & mLineMaintenanceOrder.UserName

                MarkLog(Action.Edit,
                        "LineMaintenanceOrder",
                        OrderDetail,
                        ErrorType.NoError,
                        mLineMaintenanceOrder.ID,
                        EventLogID)

                str = "openledgersame('wfLineMaintenanceOrder_Ajax.aspx?BackPage=index.aspx');"

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenScript",
                                                    str,
                                                    True)

            Case "DeleteRecord"

                If Not IsInRole(Rights.Delete) Then

                    GridBind()

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        MessageBox.Show("You are not authorized user", False),
                                                        True)

                    Exit Sub

                End If

                Dim index As Integer = CInt(e.CommandArgument) + dgOrderList.PageIndex * dgOrderList.PageSize
                Dim mID As Guid = mLineMaintenanceOrderList(index).ID

                DeleteRecord(mID)

            Case "View"

                Dim Index As Integer = CInt(e.CommandArgument) + dgOrderList.PageSize * dgOrderList.PageIndex
                Dim mId As Guid = mLineMaintenanceOrderList(Index).ID
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim mLineMaintenanceOrder As LineMaintenanceOrder
                Dim mFileAttach As FileAttach

                If Not IsInRole(Rights.View) Then

                    GridBind()

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        MessageBox.Show("You are not authorized user", False),
                                                        True)

                    Exit Sub

                End If

                mLineMaintenanceOrder = LineMaintenanceOrder.GetLineMaintenanceOrder(ID:=mId)

                DataFieldBind()
                SetControl()

                mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintenanceOrder.ID)
                Session("mFileAttach") = mFileAttach

                If mFileAttach.Size > 0 Then

                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                    Dim fs As FileStream

                    If File.Exists(AppSettings("DOCPath")) = False Then

                        'Delete file if exist
                        File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)

                        ' Create the file.
                        fs = File.Create(path)

                        ' Add some Information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()

                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "View Attachment",
                                                            "viewAttachment()",
                                                            True)

                    End If

                End If

        End Select

    End Sub

    Private Sub GV_OrderList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgOrderList.PageIndexChanging
        dgOrderList.PageIndex = e.NewPageIndex
        GridBind()
        Session("mLineMaintenanceOrderList") = mLineMaintenanceOrderList
    End Sub

    Private Sub GV_OrderList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgOrderList.Sorting

        mLineMaintenanceOrderList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLineMaintenanceOrderList") = mLineMaintenanceOrderList
        GridBind()

    End Sub

    Private Sub SearchCriteria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSearchCriteria.SelectedIndexChanged

        cmbPeriod.SelectedIndex = 0
        cmbOrderText.SelectedIndex = 0
        ClearControls()
        Dim DateIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0 And cmbPeriod.Visible, cmbPeriod.SelectedIndex, 0)
        ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)
        SetPeriod(DateIndex)
        If cmbSearchCriteria.Enabled = True Then
            SetFocus(cmbSearchCriteria)
        End If

    End Sub

    Private Sub Period_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPeriod.SelectedIndexChanged

        ClearControls()
        Dim SearchIndex As Int32 = cmbSearchCriteria.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0)
        ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)
        SetPeriod(DateIndex)
        If cmbPeriod.Enabled = True Then
            SetFocus(cmbPeriod)
        End If

    End Sub

    Private Sub SearchRecords(sender As Object, e As EventArgs) Handles btnSearch.Click

        Flag = 1
        SearchIndex = IIf(cmbSearchCriteria.SelectedIndex < 0, 0, cmbSearchCriteria.SelectedIndex)
        DateIndex = IIf(cmbPeriod.SelectedIndex < 0, 0, cmbPeriod.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("OrderText") = OrderText
        Session("Name") = Name
        Session("No") = No
        CallFindNow(SearchIndex)

    End Sub

    Private Sub OrderText_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOrderText.SelectedIndexChanged

        ClearControls()
        Dim SearchIndex As Int32 = cmbSearchCriteria.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0)

        ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)

        If cmbOrderText.Enabled = True Then
            SetFocus(cmbOrderText)
        End If

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    Private Sub AddNew(sender As Object, e As EventArgs) Handles btnAddNew.Click

        If (Not IsInRole(Rights.New)) Then 'Added By vikrant On 16-July-2014

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenScript",
                                                MessageBox.Show("You are not authorized user", False),
                                                True)

            Exit Sub

        End If

        NewRecord()
        MarkLog(Action.[New],
                "LineMaintenanceOrder",
                "",
                ErrorType.NoError,
                mLineMaintenanceOrder.ID,
                EventLogID)

        Dim str As String
        str = "openledgersame('wfLineMaintenanceOrder_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "OpenScript",
                                            str,
                                            True)

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")

    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click

        If Not IsInRole(Rights.Print) Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenScript",
                                                MessageBox.Show("You are not authorized user", False),
                                                True)

            Exit Sub

        End If

        Dim Rpt As New crLineMaintenanceOrderList
        Dim da As New ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        If cmbSearchCriteria.SelectedIndex = 0 Then

            'All
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""

        ElseIf cmbSearchCriteria.SelectedIndex = 1 Then

            'Date
            SearchStr1 = "The report shows records filtered by the following criteria"
            If cmbPeriod.SelectedIndex = 0 Then
                SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text
            ElseIf cmbPeriod.SelectedIndex = 6 Then

                SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text +
                             " " + lblFromDate.Text + " " +
                             New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " +
                             New SmartDate(txtToDate.Text).FormattedText

            Else

                SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text + " " +
                             lblFromDate.Text + " " +
                             New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " +
                             New SmartDate(txtToDate.Text).FormattedText

            End If

        ElseIf cmbSearchCriteria.SelectedIndex = 2 Then

            'Order
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbOrderText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text

        ElseIf cmbSearchCriteria.SelectedIndex = 3 Then

            'Aircraft
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtName.Text

        ElseIf cmbSearchCriteria.SelectedIndex = 4 Then

            'Supplier
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtName.Text

        ElseIf cmbSearchCriteria.SelectedIndex = 5 Then

            'Quotation No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtName.Text

        ElseIf cmbSearchCriteria.SelectedIndex = 6 Then

            'Status
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text

        End If

        ReportDetails.Add(New rptStatus(, 0, ,
                                        dgOrderList.Columns.Item(1).HeaderText,
                                        dgOrderList.Columns.Item(2).HeaderText,
                                        dgOrderList.Columns.Item(3).HeaderText,
                                        dgOrderList.Columns.Item(4).HeaderText,
                                        dgOrderList.Columns.Item(5).HeaderText,
                                        dgOrderList.Columns.Item(6).HeaderText,
                                        dgOrderList.Columns.Item(7).HeaderText,
                                        dgOrderList.Columns.Item(8).HeaderText,
                                        dgOrderList.Columns.Item(9).HeaderText,
                                        dgOrderList.Columns.Item(10).HeaderText,
                                        dgOrderList.Columns.Item(11).HeaderText,
                                        dgOrderList.Columns.Item(12).HeaderText,
                                        dgOrderList.Columns.Item(13).HeaderText))

        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgOrderList.PageIndex
        TotalCount = Me.dgOrderList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(12) As String

        For j = 0 To TotalCount - 1

            Me.dgOrderList.PageIndex = j
            Me.dgOrderList.DataSource = mLineMaintenanceOrderList
            Session("mOrderList") = mLineMaintenanceOrderList
            dgOrderList.DataBind()

            For I = 0 To Me.dgOrderList.PageSize - 1

                If I <= Me.dgOrderList.Rows.Count - 1 Then

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

                    If Me.dgOrderList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgOrderList.Rows(I).Cells.Item(1).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgOrderList.Rows(I).Cells.Item(2).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgOrderList.Rows(I).Cells.Item(3).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgOrderList.Rows(I).Cells.Item(4).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgOrderList.Rows(I).Cells.Item(5).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgOrderList.Rows(I).Cells.Item(6).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgOrderList.Rows(I).Cells.Item(7).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgOrderList.Rows(I).Cells.Item(8).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgOrderList.Rows(I).Cells.Item(9).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(10).Text <> "&nbsp;" Then str(9) = Me.dgOrderList.Rows(I).Cells.Item(10).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(11).Text <> "&nbsp;" Then str(10) = Me.dgOrderList.Rows(I).Cells.Item(11).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(12).Text <> "&nbsp;" Then str(11) = Me.dgOrderList.Rows(I).Cells.Item(12).Text
                    If Me.dgOrderList.Rows(I).Cells.Item(13).Text <> "&nbsp;" Then str(12) = Me.dgOrderList.Rows(I).Cells.Item(13).Text

                    ReportDetails.Add(New rptStatus(, 1, ,
                                                    str(0),
                                                    str(1),
                                                    str(2),
                                                    str(3),
                                                    str(4),
                                                    str(5),
                                                    str(6),
                                                    str(7),
                                                    str(8),
                                                    str(9),
                                                    str(10),
                                                    str(11),
                                                    str(12)))

                End If

            Next

        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                     mCompanyDetail.Address,
                                     mCompanyDetail.Tel1,
                                     mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax,
                                     mCompanyDetail.Email,
                                     mCompanyDetail.WebSite,
                                     "Service Order List Report",
                                     SearchStr1,
                                     SearchStr2,
                                     "",
                                     "",
                                     "",
                                     AppSettings("Product Version"),
                                     AppSettings("SINote"),
                                     "",
                                     "",
                                     "",
                                     "",
                                     AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "openTranDetail",
                                            Str1,
                                            True)

        Me.dgOrderList.PageIndex = mCurrentPageindex
        GridBind()

    End Sub

#End Region

End Class