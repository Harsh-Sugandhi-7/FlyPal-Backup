Public Class wfFuelInvoiceList_Ajax
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
    Public mFuelInvoiceList As FuelInvoiceList
    Dim mFileAttach As FileAttach
    Public mFuelInvoice As FuelInvoice
    Public mFuelInvoiceTextList As DistinctTextListForFuelInvoice
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, FuelInvoiceText, Name, No, ReportTitle As String
    Dim EventLogID As Guid
    Dim mTransactionListCount As TransactionListCount
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFuelInvoice = Session("mFuelInvoice")
        mFuelInvoiceList = Session("mFuelInvoiceList")
        mFuelInvoiceTextList = Session("mFuelInvoiceTextList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        FuelInvoiceText = Session("FuelInvoiceText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        mTransactionListCount = Session("mTransactionListCount")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub SetSession()
        Session("mFuelInvoice") = mFuelInvoice
        Session("mFuelInvoiceList") = mFuelInvoiceList
        Session("mFuelInvoiceTextList") = mFuelInvoiceTextList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mFuelInvoice")
        Session.Remove("mFuelInvoiceList")
        Session.Remove("mFuelInvoiceTextList")
        Session.Remove("mTransactionListCount")
        Session.Remove("mFileAttach")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfFuelInvoiceList_Ajax.aspx" Then
            Session.Remove("mFuelInvoice")
            Session.Remove("mFuelInvoiceList")
            Session.Remove("mFuelInvoiceTextList")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("FuelInvoiceText")
            Session.Remove("Name")
            Session.Remove("No")
            Session.Remove("mItemList")
            Session.Remove("mTransactionListCount")
        End If
    End Sub
    Private Sub NewRecord()
        mFuelInvoice = FuelInvoice.NewFuelInvoice()
        mFuelInvoice.Date = Today.Date
        mFuelInvoice.MarkClean()
        Session("mFuelInvoice") = mFuelInvoice
     End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mFuelInvoice = FuelInvoice.GetFuelInvoice(mId)
        mFuelInvoice.MarkClean()
        Session("mFuelInvoice") = mFuelInvoice
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mFuelInvoice = FuelInvoice.GetFuelInvoice(mId)
        Session("mFuelInvoice") = mFuelInvoice
        GridBind()
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgFuelInvoiceList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
          If cmbFuelInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(FuelInvoiceText)) Then
            cmbFuelInvoiceText.SelectedValue = FuelInvoiceText
        Else
            cmbFuelInvoiceText.SelectedValue = "(All)"
        End If

        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        SetTitle()  'Added by shweta on 22-12-11
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim ErrorsCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mVendorName As String
                        Dim FuelInvoiceDetail As String
                        Try
                            Session("sender") = ""
                            mFuelInvoice = CType(Session("mFuelInvoice"), FuelInvoice)
                            If mFuelInvoice.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mFuelInvoice.ID)
                            End If
                            mVendorName = mFuelInvoiceList(mFuelInvoice.ID).VendorName

                            mFuelInvoice.Delete()
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mFuelInvoice.Save()
                            DataFieldBind()
                            SetControl()
                            SetGrid()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                FuelInvoiceDetail = mFuelInvoice.FuelInvoiceNo + "," + " Dated : " + mFuelInvoice.DateFormatted + "," + " from : " + mVendorName
                                MarkLog(Util.Action.Delete, "FuelInvoice", "Can't delete : " & FuelInvoiceDetail & " is Currently in use", Util.ErrorType.HandledError, mFuelInvoice.ID, EventLogID, "FuelInvoice")
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            ErrorsCount = ex.Errors.Count
                        Finally
                            TotalCount()
                            If ErrorsCount = 0 Then
                                FuelInvoiceDetail = mFuelInvoice.FuelInvoiceNo + "," + " Dated : " + mFuelInvoice.DateFormatted + "," + " from : " + mVendorName
                                MarkLog(Util.Action.Delete, "FuelInvoice", FuelInvoiceDetail, Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID, "FuelInvoice")
                            End If
                            Session("ForEventLog") = "For Event Log"
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                        SetGrid()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()
                    SetGrid()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal StatusID As Integer = 0, Optional ByVal VendorName As String = "")
        mFuelInvoiceList = Nothing
        dgFuelInvoiceList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mFuelInvoiceList = FuelInvoiceList.GetFuelInvoiceList(Text:=Text, No:=No, FromDate:=FromDate, ToDate:=ToDate, StatusID:=StatusID, VendorName:=VendorName)
        'Set DataSource of the Grid
        Session("mFuelInvoiceList") = mFuelInvoiceList
        dgFuelInvoiceList.DataSource = mFuelInvoiceList
        dgFuelInvoiceList.DataBind()
        SetTitle() 'For lblResult
        upnlGridView.Update()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        Select Case Index
            Case -1
                Call FindNow(Text:="", No:=0, FromDate:=FromDate, ToDate:=ToDate, StatusID:=0, VendorName:="")   'for all records
            Case 0  'all
                Call FindNow(Text:="", No:=0, FromDate:=FromDate, ToDate:=ToDate, StatusID:=0, VendorName:="")   'for all records
            Case 1 'date
                Call FindNow(Text:="", No:=0, FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, StatusID:=0, VendorName:="")    'for all records
            Case 2  'Fuel Invoice Text ,No
                Call FindNow(Text:=FuelInvoiceText, No:=Val(No), FromDate:=FromDate, ToDate:=ToDate, StatusID:=0, VendorName:="")  'for all records
            Case 3 ' Vendor Name
                Call FindNow(Text:="", No:=0, FromDate:=FromDate, ToDate:=ToDate, StatusID:=0, VendorName:=Name)
            Case 4 ' Status
                Call FindNow(Text:="", No:=0, FromDate:=FromDate, ToDate:=ToDate, StatusID:=CInt(StatusId), VendorName:="")
        End Select
        dgFuelInvoiceList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5 Or DateIndex = 7) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        cmbFuelInvoiceText.Visible = IIf(SearchIndex = 2, True, False)
        lblNo.Visible = IIf(SearchIndex = 2, True, False)
        txtNo.Visible = IIf(SearchIndex = 2, True, False)
        txtName.Visible = IIf(SearchIndex = 3, True, False)
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
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        FuelInvoiceText = IIf(cmbFuelInvoiceText.SelectedIndex <= 0, "", cmbFuelInvoiceText.SelectedValue)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
         Session("FuelInvoiceText") = FuelInvoiceText
        Session("No") = No
        Session("Name") = Name
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetTitle()
        lblResult.Text = "List of Fuel Invoice as per criteria : " & mFuelInvoiceList.Count.ToString & " Record(s) found."
     End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        IsInRoleString = "FuelInvoice"
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
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        FuelInvoiceText = Session("FuelInvoiceText")
        Name = Session("Name")
        mFuelInvoiceTextList = DistinctTextListForFuelInvoice.GetDistinctTextList("25", 0, True, "(All)")
        cmbFuelInvoiceText.DataSource = mFuelInvoiceTextList
        mFuelInvoiceList = FuelInvoiceList.GetFuelInvoiceList(Text:="", No:=0, FromDate:="1/1/1900", ToDate:="1/1/2200", StatusID:=0, VendorName:="")
        dgFuelInvoiceList.DataSource = mFuelInvoiceList
        Session("mFuelInvoiceList") = mFuelInvoiceList
        DataBind()
    End Sub
    Public Sub TotalCount()
        lblFuelInvoiceList.Text = "Fuel Invoice List " & "[Total No of Record(s):-" & mFuelInvoiceList.Count.ToString & "]"
        upnlTitle.Update()
    End Sub
    Public Sub GridBind()
        dgFuelInvoiceList.DataSource = mFuelInvoiceList
        dgFuelInvoiceList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub SetGrid()
        Dim P As Integer
        For j As Integer = 0 To dgFuelInvoiceList.Rows.Count - 1
            P = CType(Me.dgFuelInvoiceList.Rows.Item(j).Cells(12).Text, Boolean)
            If P = False Then
                dgFuelInvoiceList.Rows.Item(j).Cells(11).Enabled = False
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
            If cmbSearch.Enabled = True Then
                setFocus(cmbSearch)
            End If
            Session("MiddleFrame") = "wfFuelInvoiceList_Ajax.aspx"
            DataFieldBind()
            TotalCount()
            SetControl()
        End If
        SetGrid()
        SetTitle()
    End Sub
    Private Sub dgFuelInvoiceList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFuelInvoiceList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument) + dgFuelInvoiceList.PageIndex * dgFuelInvoiceList.PageSize
                Dim mID As Guid = mFuelInvoiceList(index).ID
                EditRecord(mID)
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                GridBind()
                SetTitle()
                Dim FuelInvoiceDetail As String = mFuelInvoice.FuelInvoiceNo + " Dated : " + mFuelInvoice.DateFormatted + " to " + mFuelInvoiceList(mFuelInvoice.ID).VendorName & " Created By : " & mFuelInvoice.UserName
                MarkLog(Util.Action.Edit, "FuelInvoice", FuelInvoiceDetail, Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID, "FuelInvoice")
                Dim str As String
                str = "openledgersame('wfFuelInvoice_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgFuelInvoiceList.PageIndex * dgFuelInvoiceList.PageSize
                Dim mID As Guid = mFuelInvoiceList(index).ID
                If (Not IsInRole(Rights.Delete)) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                DeleteRecord(mID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim index As Integer = CInt(e.CommandArgument) + dgFuelInvoiceList.PageIndex * dgFuelInvoiceList.PageSize
                Dim mID As Guid = mFuelInvoiceList(index).ID
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
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDate.SelectedIndex = 0
        cmbFuelInvoiceText.SelectedIndex = 0
        ClearControls()
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
        End If
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            setFocus(cmbDate)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(cmbSearch.SelectedIndex)
        SetGrid()
        btnBottomPrint.Enabled = IIf(mFuelInvoiceList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mFuelInvoiceList.Count = 0, False, True)
    End Sub
    Private Sub cmbFuelInvoiceText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFuelInvoiceText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        If cmbFuelInvoiceText.Enabled = True Then
            setFocus(cmbFuelInvoiceText)
        End If
    End Sub

    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomAddNew.Click, btnAddNewTop.Click
        NewRecord()
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "FuelInvoice", "", Util.ErrorType.NoError, mFuelInvoice.ID, EventLogID)
        Dim str As String
        str = "openledgersame('wfFuelInvoice_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgFuelInvoiceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgFuelInvoiceList.PageIndexChanging
        dgFuelInvoiceList.PageIndex = e.NewPageIndex
        dgFuelInvoiceList.DataSource = mFuelInvoiceList
        Session("mFuelInvoiceList") = mFuelInvoiceList
        GridBind()
        SetGrid()
    End Sub
    Private Sub dgFuelInvoiceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgFuelInvoiceList.Sorting
        mFuelInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mFuelInvoiceList") = mFuelInvoiceList
        dgFuelInvoiceList.DataSource = mFuelInvoiceList
        GridBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

#Region " Report "
#Region "Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region "Event"
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomPrint.Click, btnPrintTop.Click
        'If Not IsInRole(Rights.Print) Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
        '    Exit Sub
        'End If
        ''For FuelInvoice List
        'Dim Rpt As New crFuelInvoiceList
        'Dim da As New CSLA.Data.ObjectAdapter
        'Dim ds As New dsCommon
        'Dim ReportDetails As New rptStatusList
        'SetTitle() 'For Report Titel
        'If cmbSearch.SelectedIndex = 0 Then
        '    'All
        '    SearchStr1 = "The report shows all records till date."
        '    SearchStr2 = ""
        'ElseIf cmbSearch.SelectedIndex = 1 Then
        '    'Date
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    If cmbDate.SelectedIndex = 0 Then
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
        '    ElseIf cmbDate.SelectedIndex = 6 Then
        '        'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
        '    Else
        '        'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
        '        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text).FormattedText
        '    End If
        'ElseIf cmbSearch.SelectedIndex = 2 And cmbFuelInvoiceText.SelectedIndex > 0 Then
        '    'FuelInvoice No.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbFuelInvoiceText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 2 Then
        '    'FuelInvoice No.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbFuelInvoiceText.SelectedItem.Text ''+ " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 3 Then
        '    'Part Number
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        'ElseIf cmbSearch.SelectedIndex = 4 Then
        '    'Vendor
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        'ElseIf cmbSearch.SelectedIndex = 5 And cmbEnquiryText.SelectedIndex > 0 Then
        '    'Enquiry No.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbEnquiryText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 5 Then
        '    'Enquiry No.
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbEnquiryText.SelectedItem.Text ''+ " " + lblNo.Text + " " + txtNo.Text
        'ElseIf cmbSearch.SelectedIndex = 6 Then
        '    'Status
        '    SearchStr1 = "The report shows records filtered by the following criteria"
        '    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        'End If

        'ReportDetails.Add(New rptStatus(, 0, , _
        '      dgFuelInvoiceList.Columns.Item(1).HeaderText, dgFuelInvoiceList.Columns.Item(2).HeaderText, dgFuelInvoiceList.Columns.Item(3).HeaderText, _
        '      dgFuelInvoiceList.Columns.Item(4).HeaderText, dgFuelInvoiceList.Columns.Item(5).HeaderText, dgFuelInvoiceList.Columns.Item(6).HeaderText, _
        '      dgFuelInvoiceList.Columns.Item(7).HeaderText, dgFuelInvoiceList.Columns.Item(8).HeaderText))

        ''Added by Saylee on 16-June 2007
        'Dim TotalCount As Integer
        'Dim mCurrentPageindex As Integer = Me.dgFuelInvoiceList.PageIndex 'Code Added
        'TotalCount = Me.dgFuelInvoiceList.PageCount
        'Dim j As Integer
        'Dim I As Integer
        'Dim str(7) As String

        'For j = 0 To TotalCount - 1

        '    Me.dgFuelInvoiceList.PageIndex = j
        '    Me.dgFuelInvoiceList.DataSource = mFuelInvoiceList
        '    Session("mFuelInvoiceList") = mFuelInvoiceList
        '    dgFuelInvoiceList.DataBind()
        '    For I = 0 To Me.dgFuelInvoiceList.PageSize - 1
        '        If I <= Me.dgFuelInvoiceList.Rows.Count - 1 Then
        '            str(0) = ""
        '            str(1) = ""
        '            str(2) = ""
        '            str(3) = ""
        '            str(4) = ""
        '            str(5) = ""
        '            str(6) = ""
        '            str(7) = ""

        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(1).Text
        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(2).Text
        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(3).Text
        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(4).Text
        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(5).Text
        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(6).Text
        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(7).Text
        '            If Me.dgFuelInvoiceList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgFuelInvoiceList.Rows(I).Cells.Item(8).Text


        '            ReportDetails.Add(New rptStatus(, 1, , str(0), _
        '                str(1), str(2), str(3), str(4), str(5), str(6), str(7)))
        '        End If
        '    Next
        'Next

        'mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, ReportTitle, SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        'If mFuelInvoiceList.Count = 0 Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        'Dim mrptImage As rptImage = rptImage.GetImage(ds)
        'da.Fill(ds, mrptImage)
        'da.Fill(ds, ReportDetails)
        'da.Fill(ds, Report)
        'Rpt.SetDataSource(ds)
        'Session("CrystalReport") = Rpt
        'Dim Str1 As String
        'Str1 = "openTranDetail();"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        'Me.dgFuelInvoiceList.DataSource = mFuelInvoiceList
        'Session("mFuelInvoiceList") = mFuelInvoiceList
        'dgFuelInvoiceList.DataBind()
        'SetGrid()
    End Sub
#End Region

#End Region

End Class