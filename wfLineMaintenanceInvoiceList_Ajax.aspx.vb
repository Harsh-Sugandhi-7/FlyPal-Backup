'***********************************
'Modified by Harsh Sugandhi on 22nd April 2025 for FLYPAL 2334 => Facility to attach a file to Service Module. 
'***********************************


Public Class wfLineMaintenanceInvoiceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mLineMaintInvoice As LineMaintenanceInvoice
    Public mLineMaintInvoiceList As LineMaintenanceInvoiceList
    Public mDistinctTextListForLineMaintInvoice As DistinctTextListForOrder
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, InvoiceText, Name, No As String
    Public mTransTypeID As Trans
    Dim EventLogID As Guid
    Dim InvDetail As String
    Dim mModuleName As String = "LineMaintenanceInvoice"
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mLineMaintInvoice = Session("mLineMaintInvoice")
        mLineMaintInvoiceList = Session("mLineMaintInvoiceList")
        mDistinctTextListForLineMaintInvoice = Session("mDistinctTextListForLineMaintInvoice")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        InvoiceText = Session("InvoiceText")
        Name = Session("Name")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        mTransTypeID = Session("mTransTypeId")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mLineMaintInvoice")
        Session.Remove("mLineMaintInvoiceList")
        Session.Remove("mDistinctTextListForLineMaintInvoice")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("StatusId")
        Session.Remove("InvoiceText")
        Session.Remove("Name")
        Session.Remove("No")
        Session.Remove("mTransTypeId")
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgInvoiceList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
        If cmbInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(InvoiceText)) Then
            cmbInvoiceText.SelectedValue = IIf(InvoiceText = "", "(All)", InvoiceText)
        Else
            cmbInvoiceText.SelectedValue = "(All)"
        End If
        '-------------------------------------------------------------------------
        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)

    End Sub
    Private Sub ClearControl()
        txtName.Text = ""
        txtNo.Text = ""
    End Sub
    Private Sub NewRecord()
        mTransTypeID = Util.Trans.LineMaintenanceInvoice
        Session("mTransTypeId") = mTransTypeID
        mLineMaintInvoice = LineMaintenanceInvoice.NewLineMaintenanceInvoice(Guid.NewGuid)
        'mLineMaintInvoice.LineMaintenanceInvoiceDate = Today.Date
        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mLineMaintInvoice = LineMaintenanceInvoice.GetLineMaintenanceInvoice(mId)
        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.Show(MSGBox.Message_Title.Delete, MSGBox.Message_Text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mLineMaintInvoice = LineMaintenanceInvoice.GetLineMaintenanceInvoice(mId)
        Session("mLineMaintInvoice") = mLineMaintInvoice
    End Sub
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        InvoiceText = Session("InvoiceText")

        mDistinctTextListForLineMaintInvoice = DistinctTextListForOrder.GetDistinctTextList("18", , True, "(All)")
        cmbInvoiceText.DataSource = mDistinctTextListForLineMaintInvoice
        Session("mDistinctTextListForLineMaintInvoice") = mDistinctTextListForLineMaintInvoice

        Name = Session("Name")

        dgInvoiceList.DataSource = mLineMaintInvoiceList
        Session("mLineMaintInvoiceList") = mLineMaintInvoiceList
        DataBind()
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
                            Dim mLineMaintInvoice As LineMaintenanceInvoice
                            Session("sender") = ""
                            mLineMaintInvoice = CType(Session("mLineMaintInvoice"), LineMaintenanceInvoice)
                            mLineMaintInvoice.Delete()
                            mLineMaintInvoice.Save()
                            DataFieldBind()
                            SetControl()
                            SetTitle()
                            ControlVisibility()
                            upnlGrid.Update()
                            upnlActionBtn.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Changed By Utkarsh On 21-Jul-2011 For All19072011
                                mTransTypeID = mLineMaintInvoice.TransTypeID
                                SetTitle()
                                InvDetail = mLineMaintInvoice.LineMaintInvoiceNo + " Dated : " + mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted + " from " + mLineMaintInvoiceList(mLineMaintInvoice.ID).VendorName
                                MarkLog(Util.Action.Delete, mModuleName, "Can't delete : " & InvDetail & " is Currently in use", Util.ErrorType.NoError, mLineMaintInvoice.ID, EventLogID)
                                'End
                            End If
                            'DataFieldBind()
                            'SetControl()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 21-Jul-2011 For All19072011
                                mTransTypeID = mLineMaintInvoice.TransTypeID
                                'SetTitle()
                                InvDetail = mLineMaintInvoice.LineMaintInvoiceNo + " Dated : " + mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted + " from " + mLineMaintInvoiceList(mLineMaintInvoice.ID).VendorName
                                MarkLog(Util.Action.Delete, mModuleName, InvDetail, Util.ErrorType.NoError, mLineMaintInvoice.ID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub FindNow(Optional ByVal InvoiceText As String = "", Optional ByVal InvoiceNo As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal statusid As Integer = 0, Optional ByVal VendorName As String = "", Optional ByVal MachineName As String = "")
        mLineMaintInvoiceList = Nothing
        dgInvoiceList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mLineMaintInvoiceList = LineMaintenanceInvoiceList.GetLineMaintenanceInvoiceList(InvoiceText, InvoiceNo, FromDate, ToDate, statusid, VendorName, Util.Trans.LineMaintenanceInvoice, MachineName)
        'Set DataSource of the Grid
        Session("mLineMaintInvoiceList") = mLineMaintInvoiceList
        dgInvoiceList.DataSource = mLineMaintInvoiceList
        dgInvoiceList.DataBind()
        'Set Mapping Name 
        lblResult.Text = "As per criteria :" & mLineMaintInvoiceList.Count & " Record(s) found."
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        Select Case Index
            Case -1
                Call FindNow("", 0, FromDate, ToDate, 0, "", "")
            Case 0  'all
                Call FindNow("", 0, FromDate, ToDate, 0, "", "")
            Case 1  'Invoice date
                Call FindNow("", 0, txtFromDate.Text, txtToDate.Text, 0, "", "")
            Case 2  'Invoice Text 
                Call FindNow(InvoiceText, CInt(Val(No)), FromDate, ToDate, 0, "", "")
            Case 3 ' Vendor Name
                Call FindNow("", 0, FromDate, ToDate, 0, Name, "")
            Case 4 ' Machine Name
                Call FindNow("", 0, FromDate, ToDate, 0, "", Name)
            Case 5 ' Status Text 
                Call FindNow("", 0, FromDate, ToDate, CInt(StatusId), "", "")
        End Select
        dgInvoiceList.PageIndex = 0
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("1-1-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("1-1-2200").ToString(AppSettings("DateFormat"))
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
                'Dim Month As Integer
                'Month = Today.Month
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date) 'Changes by Prashant on 09-01-2008
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date) 'Changes by Prashant on 09-01-2008
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        cmbInvoiceText.Visible = IIf(SearchIndex = 2, True, False)
        txtNo.Visible = IIf(SearchIndex = 2 And cmbInvoiceText.SelectedIndex <> 0, True, False)
        lblNo.Visible = IIf(SearchIndex = 2 And cmbInvoiceText.SelectedIndex <> 0, True, False)
        txtName.Visible = IIf(SearchIndex = 3 Or SearchIndex = 4, True, False)
        cmbStatus.Visible = CBool(IIf(SearchIndex = 5, True, False))

        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        Else
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetTitle()
        lblLineMaintInvoiceList.Text = "List of Service Invoice"
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        InvoiceText = IIf(cmbInvoiceText.SelectedIndex <= 0, "", cmbInvoiceText.SelectedValue)
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("InvoiceText") = InvoiceText
        Session("No") = No
        Session("Name") = Name
    End Sub
    Private Sub GridBind()
        dgInvoiceList.DataSource = mLineMaintInvoiceList
        dgInvoiceList.DataBind()
    End Sub
    Private Sub ControlVisibility()
        btnPrintTop.Enabled = IIf(mLineMaintInvoiceList.Count = 0, False, True)
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack And Session("sender") = "" Then
            If cmbSearch.Enabled = True Then
                cmbSearch.Focus()
            End If
            mTransTypeID = Util.Trans.LineMaintenanceInvoice
            Session("mTransTypeId") = mTransTypeID
            Session("MiddleFrame") = "wfLineMaintenanceInvoiceList_Ajax.aspx?BackPage=index.aspx"
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "LineMaintenanceInvoice") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            DataFieldBind()
            SetControl()
            SetTitle()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgInvoiceList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInvoiceList.RowCommand
        Dim Index As Integer
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "LineMaintenanceInvoice") Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
                End If
                If (Not User.IsInRole("LineMaintenanceInvoiceView") And Not User.IsInRole("LineMaintenanceInvoiceEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"), True)
                    Exit Sub
                End If
                GridBind()
                Index = CInt(e.CommandArgument)
                mID = mLineMaintInvoiceList(Index).ID
                EditRecord(mID)
                InvDetail = mLineMaintInvoice.LineMaintInvoiceNo + " Dated : " + mLineMaintInvoice.LineMaintenanceInvoiceDateFormatted + " from " + mLineMaintInvoiceList(mLineMaintInvoice.ID).VendorName
                MarkLog(Util.Action.Edit, mModuleName, InvDetail, Util.ErrorType.NoError, mLineMaintInvoice.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfLineMaintenanceInvoice_Ajax.aspx?BackPage=index.aspx');", True)
            Case "DeleteRec"
                If (Not User.IsInRole("LineMaintenanceInvoiceDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"), True)
                    Exit Sub
                End If
                GridBind()
                Index = CInt(e.CommandArgument)
                mID = mLineMaintInvoiceList(Index).ID
                DeleteRecord(mID)

            Case "View"

                Index = CInt(e.CommandArgument) + dgInvoiceList.PageSize * dgInvoiceList.PageIndex
                mID = mLineMaintInvoiceList(Index).ID
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim mLineMaintenanceInvoice As LineMaintenanceInvoice
                Dim mFileAttach As FileAttach

                If (Not User.IsInRole("LineMaintenanceInvoiceView")) Then

                    GridBind()

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        MessageBox.Show("You are not authorized user", False),
                                                        True)

                    Exit Sub

                End If

                mLineMaintenanceInvoice = LineMaintenanceInvoice.GetLineMaintenanceInvoice(ID:=mID)

                DataFieldBind()
                SetControl()

                mFileAttach = FileAttach.GetAttachment(ReferenceID:=mLineMaintenanceInvoice.ID)
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
    Private Sub dgInvoiceList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgInvoiceList.PageIndexChanging
        dgInvoiceList.PageIndex = e.NewPageIndex
        dgInvoiceList.DataSource = mLineMaintInvoiceList
        Session("mLineMaintInvoiceList") = mLineMaintInvoiceList
        dgInvoiceList.DataBind()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        ClearControl()
        cmbDate.ClearSelection()
        cmbInvoiceText.ClearSelection()
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            cmbDate.Focus()
        End If
    End Sub
    Private Sub cmbInvoiceText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInvoiceText.SelectedIndexChanged
        ClearControl()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        If cmbInvoiceText.Enabled = True Then
            cmbInvoiceText.Focus()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "LineMaintenanceInvoice") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        setVariables()
        CallFindNow(SearchIndex)
        dgInvoiceList.DataBind()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "LineMaintenanceInvoice") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        'Added By vikrant On 16-July-2014
        If (Not User.IsInRole("LineMaintenanceInvoiceNew")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"), True)
            Exit Sub
        End If
        'End
        NewRecord()
        MarkLog(Util.Action.[New], "LineMaintenanceInvoice", "", Util.ErrorType.NoError, mLineMaintInvoice.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingLineMaintenanceOrderList_Ajax.aspx?BackPage=Index.aspx&ChildPage=index.aspx');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgInvoiceList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgInvoiceList.Sorting
        mLineMaintInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgInvoiceList.DataSource = mLineMaintInvoiceList
        Session("mLineMaintInvoiceList") = mLineMaintInvoiceList
        dgInvoiceList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region "Report "

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
        If Not User.IsInRole("LineMaintenanceInvoicePrint") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"), True)
            Exit Sub
        End If
        'For Invoice List
        Dim Rpt As New crLineMaintenanceInvoiceList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        If cmbSearch.SelectedIndex = 0 Then
            'All
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbSearch.SelectedIndex = 1 Then
            'Date
            SearchStr1 = "The report shows records filtered by the following criteria"
            If cmbDate.SelectedIndex = 0 Then
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
            ElseIf cmbDate.SelectedIndex = 6 Then
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
            Else
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
            End If
        ElseIf cmbSearch.SelectedIndex = 2 Then
            'Invoice 
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbInvoiceText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        ElseIf cmbSearch.SelectedIndex = 3 Then
            'Supplier
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 4 Then
            'Aircraft
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        ElseIf cmbSearch.SelectedIndex = 5 Then
            'Status
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        End If

        ReportDetails.Add(New rptStatus(, 0, ,
              dgInvoiceList.Columns.Item(1).HeaderText, dgInvoiceList.Columns.Item(2).HeaderText, dgInvoiceList.Columns.Item(3).HeaderText,
               dgInvoiceList.Columns.Item(4).HeaderText, dgInvoiceList.Columns.Item(6).HeaderText,
              dgInvoiceList.Columns.Item(7).HeaderText, dgInvoiceList.Columns.Item(8).HeaderText, dgInvoiceList.Columns.Item(9).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mLineMaintInvoiceList.Count

        Dim mCurrentPageindex As Integer = Me.dgInvoiceList.PageIndex 'Code Added				
        TotalCount = Me.dgInvoiceList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(9) As String

        For j = 0 To TotalCount - 1

            Me.dgInvoiceList.PageIndex = j
            Me.dgInvoiceList.DataSource = mLineMaintInvoiceList
            Session("mLineMaintInvoiceList") = mLineMaintInvoiceList
            dgInvoiceList.DataBind()

            For I = 0 To Me.dgInvoiceList.PageSize - 1
                If I <= Me.dgInvoiceList.Rows.Count - 1 Then

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

                    If Me.dgInvoiceList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgInvoiceList.Rows(I).Cells.Item(1).Text
                    If Me.dgInvoiceList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgInvoiceList.Rows(I).Cells.Item(2).Text
                    If Me.dgInvoiceList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgInvoiceList.Rows(I).Cells.Item(3).Text
                    If Me.dgInvoiceList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgInvoiceList.Rows(I).Cells.Item(4).Text
                    If Me.dgInvoiceList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(4) = Me.dgInvoiceList.Rows(I).Cells.Item(6).Text
                    If Me.dgInvoiceList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(5) = Me.dgInvoiceList.Rows(I).Cells.Item(7).Text
                    If Me.dgInvoiceList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(6) = Me.dgInvoiceList.Rows(I).Cells.Item(8).Text
                    If Me.dgInvoiceList.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(7) = Me.dgInvoiceList.Rows(I).Cells.Item(9).Text


                    ReportDetails.Add(New rptStatus(, 1, , str(0),
                                str(1), str(2), str(3), str(4), str(5), str(6), str(7)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Service Invoice List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Me.dgInvoiceList.PageIndex = mCurrentPageindex
        Me.dgInvoiceList.DataSource = mLineMaintInvoiceList
        Session("mLineMaintInvoiceList") = mLineMaintInvoiceList
        dgInvoiceList.DataBind()
    End Sub

#End Region


End Class