'Added by vikrant For New Requisition
Partial Class wfRequisitionListNew
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents txtToDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mRequisitionListNew As RequisitionListNew
    ' Public mRequisitionListNew1 As RequisitionListNew 'Commented By Shweta On 19-August-2013 for ALL16082013-1
    Public mRequisitionNew As RequisitionNew
    Public mDistinctTextList As DistinctTextListForRequisition
    Public mLocationList As LocationList
    Dim OpeningFor, ReqTypeID As Integer
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, RequisitionText, Name, No, Location As String
    Dim EventLogID As Guid
    Dim mRequisitionDetail As String
    Dim mTransactionListCount As TransactionListCount 'Added By Shweta On 19-August-2013 for ALL16082013-1
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRequisitionNew = Session("mRequisitionNew")
        mRequisitionListNew = Session("mRequisitionListNew")
        mDistinctTextList = Session("mDistinctTextList")
        mLocationList = Session("mLocationList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        ReqTypeID = Session("ReqTypeID")
        RequisitionText = Session("RequisitionText")
        Name = Session("Name")
        Location = Session("Location")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        OpeningFor = CInt(Session("OpeningFor"))
    End Sub
    Private Sub SetSession()
        Session("mRequisitionNew") = mRequisitionNew
        Session("mRequisitionListNew") = mRequisitionListNew
        Session("mDistinctTextList") = mDistinctTextList
        Session("mLocationList") = mLocationList
        Session("OpeningFor") = OpeningFor
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRequisitionNew")
        Session.Remove("mRequisitionListNew")
        Session.Remove("mDistinctTextList")
        Session.Remove("mLocationList")
        Session.Remove("OpeningFor")
    End Sub
    Private Sub ClearAll()
        OpeningFor = Session("OpeningFor")
        If Session("MiddleFrame") <> "wfRequisitionListNew.aspx?OpeningFor=" & OpeningFor Then
            Session.Remove("mRequisitionNew")
            Session.Remove("mRequisitionListNew")
            Session.Remove("mDistinctTextList")
            Session.Remove("mLocationList")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("ReqTypeID")
            Session.Remove("RequisitionText")
            Session.Remove("Name")
            Session.Remove("No")
            Session.Remove("Location")
            Session.Remove("OpeningFor")
            Session.Remove("BackPage")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgRequisitionList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
        cmbRequisitionType.SelectedValue = ReqTypeID
        If cmbRequisitionText.Items.Contains(New System.Web.ui.WebControls.ListItem(RequisitionText)) Then 'Added By Rajnish On 01-01-2008
            cmbRequisitionText.SelectedValue = RequisitionText
        Else
            cmbRequisitionText.SelectedValue = "(All)"
        End If
        '' cmbRequisitionText.SelectedValue = IIf(RequisitionText = "", "(All)", RequisitionText)
        Try
            cmbRequisitionLocation.SelectedValue = IIf(Location = "", "<SELECT>", Location)
        Catch ex As Exception
            '
        End Try
        txtName.Text = Name
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Requisition as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
    End Sub
    Private Sub NewRecord()
        mRequisitionNew = RequisitionNew.NewRequisition
        mRequisitionNew.ReqDate = Today.Date
        Session("mRequisitionNew") = mRequisitionNew
        OpeningFor = Session("OpeningFor")
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mRequisitionNew = RequisitionNew.GetRequisition(mId)
        Dim child As RequisitionItemNew
        For Each child In mRequisitionNew.RequisitionItemsNew
            If child.ItemID.Equals(Guid.Empty) Then
                ' ''partno id .....
                ' ''child.ItemID = Guid.NewGuid
                ' ''child.Save()
            End If
        Next
        mRequisitionNew.MarkClean()
        Session("mRequisitionNew") = mRequisitionNew
        OpeningFor = Session("OpeningFor")
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfRequisitionListNew.aspx?BackPage=" & Request.QueryString("BackPage")
        Session("sender") = "Delete"
        msg1.Show()
        mRequisitionNew = RequisitionNew.GetRequisition(mId)
        Session("mRequisitionNew") = mRequisitionNew
        OpeningFor = Session("OpeningFor")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Dim mRequisitionNew As RequisitionNew
                            Session("Sender") = ""
                            mRequisitionNew = CType(Session("mRequisitionNew"), RequisitionNew)
                            'mRequisitionNew.DeleteRequisition(mRequisition.ID)
                            mRequisitionNew.Delete()
                            mRequisitionNew.Save()
                            Response.Redirect("wfRequisitionListNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRequisitionListNew.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRequisitionListNew.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfRequisitionListNew.aspx?BackPage=" & Request.QueryString("BackPage")
                                mRequisitionDetail = mRequisitionNew.RequisitionNo + " Dated : " + mRequisitionNew.ReqDateFormatted + " Requested By : " + mRequisitionNew.EmployeeName + " Status : " + IIf(mRequisitionNew.StatusID = 1, "Open", "Authorized")
                                MarkLog(Util.Action.Delete, "Requisition(N)", "Can't delete : " & mRequisitionDetail & " is Currently in use", Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                                msg1.Show()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mRequisitionDetail = mRequisitionNew.RequisitionNo + " Dated : " + mRequisitionNew.ReqDateFormatted + " Requested By : " + mRequisitionNew.EmployeeName + " Status : " + IIf(mRequisitionNew.StatusID = 1, "Open", "Authorized")
                                MarkLog(Util.Action.Delete, "Requisition(N) ", mRequisitionDetail, Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    Response.Redirect("wfRequisitionListNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    ' DataFieldBind()
                    Response.Redirect("wfRequisitionListNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfRequisitionListNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfRequisitionListNew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal StatusID As Integer = 0, Optional ByVal RequestingLocation As String = "", Optional ByVal Aircraft As String = "", Optional ByVal Employee As String = "", Optional ByVal LocationID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal ReqTypeID As Integer = 0)
        mRequisitionListNew = Nothing
        dgRequisitionList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mRequisitionListNew = RequisitionListNew.GetRequisitionList(ItemName, Text, No, FromDate, ToDate, StatusID, RequestingLocation, Employee, LocationID, Aircraft, ReqTypeID)
        'Set DataSource of the Grid
        Session("mRequisitionListNew") = mRequisitionListNew
        dgRequisitionList.DataSource = mRequisitionListNew
        lblResult.Text = "List of Requisition as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        Select Case Index
            Case -1
                Call FindNow() 'for all records
            Case 0  'all
                Call FindNow() 'for all records
            Case 1 'Requisition date
                Call FindNow("", "", 0, txtFromDate.Value.ToString, txtToDate.Value.ToString)
            Case 2  'Requisition Text , No 
                Call FindNow("", RequisitionText, CInt(Val(No)), FromDate, ToDate)
            Case 3 'Location
                Call FindNow("", "", 0, FromDate, ToDate, 0, Location)
            Case 4 ' Status
                Call FindNow("", "", 0, FromDate, ToDate, StatusId, "")
            Case 5 'Requisition Type
                Call FindNow("", "", 0, FromDate, ToDate, 0, Location, "", "", Guid.Empty.ToString, ReqTypeID)
            Case 6 'Part No
                Call FindNow(Name, "", 0, FromDate, ToDate, 0, Location, "", "", Guid.Empty.ToString, ReqTypeID)

        End Select
        dgRequisitionList.CurrentPageIndex = 0  'Added Code on May,25,2007
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Value = CDate("01-01-1900")
                txtToDate.Value = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Value = CDate(Today.AddDays(-6))
                txtToDate.Value = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Value = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Value = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Value = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Value = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Value = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Value = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Value = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Value = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Value = Today.AddDays(1).AddYears(-1)
                txtToDate.Value = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))
                End If
                txtToDate.Value = Today.Date
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date)
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date)
                txtFromDate.Value = FromDate
                txtToDate.Value = ToDate
        End Select
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
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        cmbRequisitionText.Visible = IIf(SearchIndex = 2, True, False)
        lblNo.Visible = IIf(SearchIndex = 2 And cmbRequisitionText.SelectedIndex <> 0, True, False)
        txtNo.Visible = IIf(SearchIndex = 2 And cmbRequisitionText.SelectedIndex <> 0, True, False)
        cmbRequisitionLocation.Visible = IIf(SearchIndex = 3, True, False)
        cmbStatus.Visible = IIf(SearchIndex = 4, True, False)
        cmbRequisitionType.Visible = IIf(SearchIndex = 5, True, False)
        txtName.Visible = IIf(SearchIndex = 6, True, False)
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Value.ToString <> "", txtFromDate.Value.ToString, "1/1/1900")
        ToDate = IIf(txtToDate.Value.ToString <> "", txtToDate.Value.ToString, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        ReqTypeID = IIf(cmbRequisitionType.SelectedIndex <= 0, 0, cmbRequisitionType.SelectedValue)
        RequisitionText = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue)
        Location = IIf(cmbRequisitionLocation.SelectedIndex > 0, cmbRequisitionLocation.SelectedItem.Text, "")
        Name = txtName.Text.Trim
        No = txtNo.Text.Trim
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("ReqTypeID") = ReqTypeID
        Session("RequisitionText") = RequisitionText
        Session("Location") = Location
        Session("No") = No
        Session("Name") = Name
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub ControlEnability()
        BtnPrint.Enabled = IIf(dgRequisitionList.Items.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(dgRequisitionList.Items.Count = 0, False, True)
    End Sub
#End Region

#Region " DatafieldBinding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        'Commented and added by Shweta on 19-August-2013 for ALL16082013-1
        'DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        'end
        StatusId = Session("StatusId")
        ReqTypeID = Session("ReqTypeID")
        RequisitionText = Session("RequisitionText")
        Name = Session("Name")
        No = Session("No")
        Location = Session("Location")
        mDistinctTextList = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)")
        cmbRequisitionText.DataSource = mDistinctTextList
        mLocationList = LocationList.GetLocationsList(0, , , , , , True, "(All)")
        cmbRequisitionLocation.DataSource = mLocationList
        Session("mLocationList") = mLocationList
        DataBind()
        'Commented and added By Shweta On 19-August-2013 for ALL16082013-1
        'mRequisitionListNew = RequisitionListNew.GetRequisitionList("", "", 0, "01/01/1900", "01/01/2050", 0, "", "", "{00000000-0000-0000-0000-000000000000}", "", ReqTypeID) 22-aug
        'dgRequisitionList.DataSource = mRequisitionListNew 22-aug
        'Session("mRequisitionListNew") = mRequisitionListNew 22-aug
        'lblResult.Text = "List of Requisition as per criteria :" & mRequisitionListNew.Count & " Record(s) found." 22-aug
        'mRequisitionListNew1 = RequisitionListNew.GetRequisitionList("", "") 
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(65)
        lblTotal.Text = "[Total No of Record(s):-" & mTransactionListCount(0).Count & "]"
        'End
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
            If Not IsPostBack And OpeningFor = 0 Then
                OpeningFor = CInt(Request.QueryString("OpeningFor"))
            End If
            Session("OpeningFor") = OpeningFor
            Session("MiddleFrame") = "wfRequisitionListNew.aspx?OpeningFor=" & OpeningFor
            DataFieldBind()
            SetControl()
        End If
        MessageBoxResult()
        ControlEnability()
    End Sub
    Private Sub dgRequisitionList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRequisitionList.ItemCommand
        Select Case e.CommandName
            Case "Edit"
                If (Not User.IsInRole("NewRequisitionView") And Not User.IsInRole("NewRequisitionEdit")) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                Dim mId As New Guid(e.Item.Cells(0).Text)
                EditRecord(mId)

                mRequisitionDetail = mRequisitionNew.RequisitionNo + " Dated : " + mRequisitionNew.ReqDateFormatted + " Requested By : " + mRequisitionNew.EmployeeName + " Status : " + IIf(mRequisitionNew.StatusID = 1, "Open", "Authorized")
                MarkLog(Util.Action.Edit, "Requisition(N)", mRequisitionDetail, Util.ErrorType.NoError, mId, EventLogID)

                Dim str As String
                str = "<script language='javascript'>  openledgersame('wfRequisitionNew.aspx?BackPage=index.aspx'); </script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
            Case "Delete"
                If (Not User.IsInRole("NewRequisitionDelete")) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                Dim mId As New Guid(e.Item.Cells(0).Text)
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgRequisitionList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRequisitionList.PageIndexChanged
        dgRequisitionList.CurrentPageIndex = e.NewPageIndex
        dgRequisitionList.DataSource = mRequisitionListNew
        Session("mRequisitionListNew") = mRequisitionListNew
        dgRequisitionList.DataBind()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDate.SelectedIndex = 0
        cmbRequisitionText.SelectedIndex = 0
        cmbRequisitionLocation.SelectedIndex = 0
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
        CallFindNow(SearchIndex)
        dgRequisitionList.DataBind()
        lblResult.Text = "List of Requisition as per criteria :" & mRequisitionListNew.Count & " Record(s) found."
    End Sub
    Private Sub cmbRequisitionText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRequisitionText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        If cmbRequisitionText.Enabled = True Then
            setFocus(cmbRequisitionText)
        End If
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        NewRecord()
        If (Not User.IsInRole("NewRequisitionNew")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        MarkLog(Util.Action.[New], "Requisition(N)", "", Util.ErrorType.NoError, mRequisitionNew.ID, EventLogID)
        Dim str As String
        str = "<script language='javascript'>  openledgersame('wfRequisitionNew.aspx?BackPage=index.aspx'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgRequisitionList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgRequisitionList.SortCommand
        mRequisitionListNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionListNew") = mRequisitionListNew
        dgRequisitionList.DataSource = mRequisitionListNew
        dgRequisitionList.DataBind()
    End Sub
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, BtnPrint.Click
        If (Not User.IsInRole("NewRequisitionPrint")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        Dim mCompanyDetail As New CompanyDetail
        Dim SearchStr1 As String = ""
        Dim SearchStr2 As String = ""
        Dim Rpt As New crRequisitionNewList
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
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Value.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Value.ToString).FormattedText
            Else
                SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Value.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Value.ToString).FormattedText
            End If
        ElseIf cmbSearch.SelectedIndex = 2 Then
            'Requisition No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbRequisitionText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        ElseIf cmbSearch.SelectedIndex = 3 Then
            'Requisition Location.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbRequisitionLocation.SelectedItem.Text
        ElseIf cmbSearch.SelectedIndex = 4 Then
            'Status
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
        ElseIf cmbSearch.SelectedIndex = 5 Then
            'Requisition Type
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbRequisitionType.SelectedItem.Text
        ElseIf cmbSearch.SelectedIndex = 6 Then
            'Part No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
        End If

        ReportDetails.Add(New rptStatus(, 0, , _
              dgRequisitionList.Columns.Item(1).HeaderText, dgRequisitionList.Columns.Item(2).HeaderText, dgRequisitionList.Columns.Item(3).HeaderText, _
              dgRequisitionList.Columns.Item(4).HeaderText, dgRequisitionList.Columns.Item(5).HeaderText, dgRequisitionList.Columns.Item(6).HeaderText, _
              dgRequisitionList.Columns.Item(7).HeaderText))

        Dim TotalCount As Integer
        Dim mCurrentPageindex As Integer = Me.dgRequisitionList.CurrentPageIndex
        TotalCount = Me.dgRequisitionList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(6) As String

        For j = 0 To TotalCount - 1

            Me.dgRequisitionList.CurrentPageIndex = j
            Me.dgRequisitionList.DataSource = mRequisitionListNew
            Session("mRequisitionListNew") = mRequisitionListNew
            dgRequisitionList.DataBind()
            For I = 0 To Me.dgRequisitionList.PageSize - 1
                If I <= Me.dgRequisitionList.Items.Count - 1 Then

                    str(0) = ""
                    str(1) = ""
                    str(2) = ""
                    str(3) = ""
                    str(4) = ""
                    str(5) = ""
                    str(6) = ""

                    If Me.dgRequisitionList.Items(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgRequisitionList.Items(I).Cells.Item(1).Text
                    If Me.dgRequisitionList.Items(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgRequisitionList.Items(I).Cells.Item(2).Text
                    If Me.dgRequisitionList.Items(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgRequisitionList.Items(I).Cells.Item(3).Text
                    If Me.dgRequisitionList.Items(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgRequisitionList.Items(I).Cells.Item(4).Text
                    If Me.dgRequisitionList.Items(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgRequisitionList.Items(I).Cells.Item(5).Text
                    If Me.dgRequisitionList.Items(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgRequisitionList.Items(I).Cells.Item(6).Text
                    If Me.dgRequisitionList.Items(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgRequisitionList.Items(I).Cells.Item(7).Text


                    ReportDetails.Add(New rptStatus(, 1, , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Requisition List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt

        Dim Str1 As String
        Str1 = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)

        Me.dgRequisitionList.CurrentPageIndex = mCurrentPageindex
        Me.dgRequisitionList.DataSource = mRequisitionListNew
        Session("mIssueList") = mRequisitionListNew
        dgRequisitionList.DataBind()
    End Sub
#End Region


End Class
