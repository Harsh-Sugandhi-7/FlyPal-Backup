Public Class wfWorkInvoiceList_Ajax
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
    Public mWorkInvoiceList As WorkInvoiceList
    Public mWorkInvoice As WorkInvoice
    Public mWorkInvoiceTextNoList As WorkInvoiceTextNoList
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, WorkInvoiceText, Name, No As String
    Public mTransTypeID As Trans
    Dim EventLogID As Guid
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String
    Private SearchStr2 As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mWorkInvoice = Session("mWorkInvoice")
        mWorkInvoiceList = Session("mWorkInvoiceList")
        mWorkInvoiceTextNoList = Session("mWorkInvoiceTextNoList")
        SearchIndex = Session("SearchIndex")
        mTransTypeID = Session("mTransTypeId")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        WorkInvoiceText = Session("WorkInvoiceText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mWorkInvoice")
        Session.Remove("mWorkInvoiceList")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("WorkInvoiceText")
        Session.Remove("Name")
        Session.Remove("No")
        Session.Remove("mTransTypeId")
        Session.Remove("mFileAttach")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfWorkInvoiceList_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Sub NewRecord()
        mWorkInvoice = WorkInvoice.NewWorkInvoice(64)
        mWorkInvoice.Date = Today.Date
        Session("mWorkInvoice") = mWorkInvoice
        Session("mTransTypeId") = 64
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mWorkInvoice = WorkInvoice.GetWorkInvoice(mId)
        mWorkInvoice.MarkClean()
        Session("mFileAttach") = mFileAttach
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        GridBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mWorkInvoice = WorkInvoice.GetWorkInvoice(mId)
        Session("mWorkInvoice") = mWorkInvoice
        Session("mTransTypeId") = 64
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgWorkInvoiceList.DataBind()
        cmbSearchCriteria.SelectedIndex = SearchIndex
        cmbPeriod.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId

        If cmbWorkInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(WorkInvoiceText)) Then
            cmbWorkInvoiceText.SelectedValue = WorkInvoiceText
        Else
            cmbWorkInvoiceText.SelectedValue = "(All)"
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
                            mWorkInvoice = CType(Session("mWorkInvoice"), WorkInvoice)
                            If mWorkInvoice.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mWorkInvoice.ID)
                            End If
                            mWorkInvoice.Delete()
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mWorkInvoice.Save()
                            DataFieldBind()
                            SetControl()
                            SetGrid()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally
                            SetTitle()
                            upnlFindNow.Update()
                            Dim WorkInvoiceDetail As String = mWorkInvoice.Text + "-" + CStr(mWorkInvoice.No) + " Dated : " + mWorkInvoice.Date + " to " + mWorkInvoiceList(mWorkInvoice.ID).VendorName & " Created By : " & mWorkInvoice.UserName
                            MarkLog(Util.Action.Delete, "WorkInvoice", WorkInvoiceDetail, Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                        SetGrid()
                    End If
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal StatusID As Integer = 0, Optional ByVal VendorName As String = "")
        mWorkInvoiceList = Nothing
        dgWorkInvoiceList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mWorkInvoiceList = WorkInvoiceList.GetWorkInvoiceList(Text, No, FromDate, ToDate, StatusID, VendorName)
        'Set DataSource of the Grid
        dgWorkInvoiceList.DataSource = mWorkInvoiceList
        dgWorkInvoiceList.DataBind()
        Session("mWorkInvoiceList") = mWorkInvoiceList
        lblResult.Text = "List of Work Invoice as per criteria :" & mWorkInvoiceList.Count & " Record(s) found."
        BtnPrint.Enabled = IIf(mWorkInvoiceList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mWorkInvoiceList.Count = 0, False, True)
        SetGrid()
        upnlGridView.Update()
        upnTopButtons.Update()
        upnBottomButtons.Update()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        Dim tmpmTransTypeID As Trans = 0
        Select Case Index
            Case -1
                Call FindNow("", 0, FromDate, ToDate, 0, "")      'for all records
            Case 0  'all
                Call FindNow("", 0, FromDate, ToDate, 0, "")    'for all records
            Case 1 'date
                Call FindNow("", 0, txtFromDate.Text, txtToDate.Text, 0, "")      'for all records
            Case 2  'Quootation Teaxt ,No
                Call FindNow(WorkInvoiceText, Val(No), FromDate, ToDate, 0, "")   'for all records
            Case 3 ' Vendor Name
                Call FindNow(, 0, FromDate, ToDate, 0, Name)
            Case 4 ' Status
                Call FindNow(, 0, FromDate, ToDate, CInt(StatusId))
        End Select
        dgWorkInvoiceList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
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

        cmbWorkInvoiceText.Visible = IIf(SearchIndex = 2, True, False)
        lblNo.Visible = IIf(SearchIndex = 2 And cmbWorkInvoiceText.SelectedIndex <> 0, True, False)
        txtNo.Visible = IIf(SearchIndex = 2 And cmbWorkInvoiceText.SelectedIndex <> 0, True, False)
        txtName.Visible = IIf((SearchIndex = 3), True, False)
        cmbStatus.Visible = IIf(SearchIndex = 4, True, False)
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetTitle()
        mWorkInvoiceList = WorkInvoiceList.GetWorkInvoiceList()
        lblListOfWorkInvoice.Text = "List of Work Invoices" & "[Total No of Record(s):-" & mWorkInvoiceList.Count.ToString & "]"
        upnlTitle.Update()
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "WorkInvoice"
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

        mWorkInvoiceTextNoList = WorkInvoiceTextNoList.GetWorkInvoiceTextNoList(True, "(All)")
        Session("mWorkInvoiceTextNoList") = mWorkInvoiceTextNoList
        cmbWorkInvoiceText.DataSource = mWorkInvoiceTextNoList
        DataBind()
    End Sub
    Private Sub GridBind()
        dgWorkInvoiceList.DataSource = mWorkInvoiceList
        dgWorkInvoiceList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub SetGrid()
        Dim P As Boolean
        For j As Integer = 0 To dgWorkInvoiceList.Rows.Count - 1
            P = CType(Me.dgWorkInvoiceList.Rows.Item(j).Cells(12).Text, Boolean)
            If P = False Then
                dgWorkInvoiceList.Rows.Item(j).Cells(11).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbSearchCriteria.Enabled = True Then
                setFocus(cmbSearchCriteria)
            End If
            mTransTypeID = Request.QueryString("TransTypeId")
            Session("mTransTypeId") = mTransTypeID
            Session("MiddleFrame") = "wfWorkInvoiceList_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            SetTitle()
            SetGrid()
            BtnPrint.Enabled = IIf(dgWorkInvoiceList.Rows.Count = 0, False, True)
            btnPrintTop.Enabled = IIf(dgWorkInvoiceList.Rows.Count = 0, False, True)
        End If
    End Sub
    Private Sub dgWorkInvoiceList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWorkInvoiceList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgWorkInvoiceList.PageIndex * dgWorkInvoiceList.PageSize
                Dim mID As Guid = mWorkInvoiceList(index).ID
                Dim mWorkInvoiceNo As String = mWorkInvoiceList(index).WorkInvoiceNumber
                Dim mWorkInvoiceDate As String = mWorkInvoiceList(index).DateFormatted
                Dim mVendorName As String = mWorkInvoiceList(index).VendorName
                EditRecord(mId)
                Dim WorkInvoiceDetail As String = mWorkInvoiceNo + " Dated : " + New SmartDate(mWorkInvoiceDate).FormattedText + " from " + mVendorName
                MarkLog(Util.Action.Edit, "WorkInvoice", WorkInvoiceDetail, Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
                Dim str As String
                str = "openledgersame('wfWorkInvoice_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                If Not IsInRole(Rights.Delete) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgWorkInvoiceList.PageIndex * dgWorkInvoiceList.PageSize
                Dim mID As Guid = mWorkInvoiceList(index).ID
                DeleteRecord(mID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index As Integer = CInt(e.CommandArgument) + dgWorkInvoiceList.PageIndex * dgWorkInvoiceList.PageSize
                Dim mID As Guid = mWorkInvoiceList(index).ID
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach Is Nothing Then
                    'Do Nothing
                Else
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
                            Dim Str As String
                            Str = "openFile();"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                        End If
                        GridBind()
                    End If
                End If
        End Select
        SetGrid()
    End Sub
    Private Sub dgWorkInvoiceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWorkInvoiceList.PageIndexChanging
        dgWorkInvoiceList.PageIndex = e.NewPageIndex
        GridBind()
        Session("mWorkInvoiceList") = mWorkInvoiceList
        SetGrid()
    End Sub
    Private Sub cmbSearchCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchCriteria.SelectedIndexChanged
        cmbPeriod.SelectedIndex = 0
        cmbWorkInvoiceText.SelectedIndex = 0
        ClearControls()
        Dim DateIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0 And cmbPeriod.Visible, cmbPeriod.SelectedIndex, 0)
        ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbSearchCriteria.Enabled = True Then
            setFocus(cmbSearchCriteria)
        End If
    End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearchCriteria.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0)
        ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbPeriod.Enabled = True Then
            setFocus(cmbPeriod)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        SearchIndex = IIf(cmbSearchCriteria.SelectedIndex < 0, 0, cmbSearchCriteria.SelectedIndex)
        DateIndex = IIf(cmbPeriod.SelectedIndex < 0, 0, cmbPeriod.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)

        WorkInvoiceText = IIf(cmbWorkInvoiceText.SelectedIndex <= 0, "", cmbWorkInvoiceText.SelectedValue)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("WorkInvoiceText") = WorkInvoiceText
        Session("Name") = Name
        Session("No") = No
        CallFindNow(SearchIndex)
    End Sub
    Private Sub cmbWorkInvoiceText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWorkInvoiceText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearchCriteria.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedIndex, 0)
        ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)
        If cmbWorkInvoiceText.Enabled = True Then
            setFocus(cmbWorkInvoiceText)
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        If (Not IsInRole(Rights.New)) Then 'Added By vikrant On 16-July-2014
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If 'End
        NewRecord()
        'mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mWorkInvoice.ID)
        'Session("mFileAttach") = mFileAttach
        MarkLog(Util.Action.[New], "WorkInvoice", "", Util.ErrorType.NoError, mWorkInvoice.ID, EventLogID)
        Dim str As String
        str = "openledgersame('wfWorkInvoice_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgWorkInvoiceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWorkInvoiceList.Sorting
        mWorkInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mWorkInvoiceList") = mWorkInvoiceList
        GridBind()
        SetGrid()
    End Sub
    Private Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click, btnPrintTop.Click
        If Not IsInRole(Rights.Print) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim Rpt As New crWorkInvoiceList
        Dim da As New CSLA.Data.ObjectAdapter
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
                SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
            Else
                SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbPeriod.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
            End If
        ElseIf cmbSearchCriteria.SelectedIndex = 2 Then
            'WorkInvoice
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbWorkInvoiceText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        ElseIf cmbSearchCriteria.SelectedIndex = 3 Then
            'Supplier
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearchCriteria.SelectedIndex = 4 Then
            'Status
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        End If

        ReportDetails.Add(New rptStatus(, 0, , _
              dgWorkInvoiceList.Columns.Item(1).HeaderText, dgWorkInvoiceList.Columns.Item(2).HeaderText, dgWorkInvoiceList.Columns.Item(3).HeaderText, _
              dgWorkInvoiceList.Columns.Item(4).HeaderText, dgWorkInvoiceList.Columns.Item(5).HeaderText, dgWorkInvoiceList.Columns.Item(6).HeaderText, _
              dgWorkInvoiceList.Columns.Item(7).HeaderText, dgWorkInvoiceList.Columns.Item(8).HeaderText))
        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgWorkInvoiceList.PageIndex
        TotalCount = Me.dgWorkInvoiceList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(7) As String
        For j = 0 To TotalCount - 1
            Me.dgWorkInvoiceList.PageIndex = j
            Me.dgWorkInvoiceList.DataSource = mWorkInvoiceList
            Session("mWorkInvoiceList") = mWorkInvoiceList
            dgWorkInvoiceList.DataBind()

            For I = 0 To Me.dgWorkInvoiceList.PageSize - 1
                If I <= Me.dgWorkInvoiceList.Rows.Count - 1 Then

                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""
                    str(7) = ""

                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(1).Text
                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(2).Text
                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(3).Text
                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(4).Text
                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(5).Text
                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(6).Text
                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(7).Text
                    If Me.dgWorkInvoiceList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgWorkInvoiceList.Rows(I).Cells.Item(8).Text

                    ReportDetails.Add(New rptStatus(, 1, , str(0), _
                          str(1), str(2), str(3), str(4), str(5), str(6), str(7)))

                End If
            Next
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Work Invoice List", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
#End Region

End Class