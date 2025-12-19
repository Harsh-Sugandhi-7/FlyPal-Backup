Imports Flypal
Public Class wfMSPList_Ajax
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
    Public mMSPList As MSPList
    Public mMSP As MSP
    Public mDistinctTextListForMSP As DistinctTextListForMSP
    Dim SearchIndex, DateIndex, FromDate, ToDate, MSPText, No, SearchText As String
    Dim EventLogID As Guid
    Dim mMSPDetail As String


#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMSP = Session("mMSP")
        mMSPList = Session("mMSPList")
        mDistinctTextListForMSP = Session("mDistinctTextListForMSP")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        MSPText = Session("MSPText")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        SearchText = Session("SearchText")
    End Sub
    Private Sub SetSession()
        Session("mMSP") = mMSP
        Session("mMSPList") = mMSPList
        Session("mDistinctTextListForMSP") = mDistinctTextListForMSP
        SearchText = Session("SearchText")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMSP")
        Session.Remove("mMSPList")
        Session.Remove("mDistinctTextListForMSP")
        Session.Remove("SearchText")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("MSPText")
        Session.Remove("No")
        Session.Remove("BackPage")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfMSPList_Ajax.aspx?" Then
            Session.Remove("mMSP")
            Session.Remove("mMSPList")
            Session.Remove("mDistinctTextListForMSP")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("MSPText")
            Session.Remove("No")
            Session.Remove("BackPage")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgMSPList.DataBind()
        cmbDate.SelectedIndex = DateIndex
        If cmbMSPText.Items.Contains(New System.Web.UI.WebControls.ListItem(MSPText)) Then
            cmbMSPText.SelectedValue = MSPText
        Else
            cmbMSPText.SelectedValue = "(All)"
        End If
        txtNo.Text = No
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "As per criteria :" & mMSPList.Count & " Record(s) found."
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub NewRecord()
        mMSP = MSP.NewMSP(New Guid)
        mMSP.MSPDate = Today.Date
        Session("mMSP") = mMSP
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mMSP = MSP.GetMSP(mId)
        mMSP.MarkClean()
        Session("mMSP") = mMSP
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mMSP = MSP.GetMSP(mId)
        Session("mMSP") = mMSP
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
                            Dim mMSP As MSP
                            Session("Sender") = ""
                            mMSP = CType(Session("mMSP"), MSP)
                            mMSP.Delete()
                            mMSP.Save()
                            DataFieldBind()
                            SetControl()
                            ControlEnability()
                            upnlTitle.Update()
                            upnlGrid.Update()
                            ''upnlActionBtnBottom.Update()
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                If ex.Message.Contains("FKtabOrdertabMSP") Then
                                    stringInfo = "Order."
                                ElseIf ex.Message.Contains("FKtabnWOtabMSP") Then
                                    stringInfo = "Work Order."
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mMSPDetail = "No: " + mMSP.MSPNo + " Dated: " + mMSP.MSPDateFormatted + " Plan Name: " + mMSP.PlanName + " Contract No.: " + mMSP.ContractNo
                                MarkLog(Util.Action.Delete, "MSP", mMSPDetail, Util.ErrorType.NoError, mMSP.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub FindNow(Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0,
                        Optional ByVal SearchText As String = "", Optional ByVal IsExpiredMSP As Boolean = False)
        mMSPList = Nothing
        dgMSPList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mMSPList = MSPList.GetMSPList(FromDate:=FromDate, ToDate:=ToDate, MSPText:=Text, MSPNo:=No, SearchText:=SearchText, IsExpiredMSP:=IsExpiredMSP)
        'Set DataSource of the Grid
        Session("mMSPList") = mMSPList
        dgMSPList.DataSource = mMSPList
        lblResult.Text = "As per criteria :" & mMSPList.Count & " Record(s) found."
        dgMSPList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        btnPrintTop.Enabled = (mMSPList.Count > 0)
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow(FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, Text:=Trim(MSPText), No:=CInt(Val(No)),
                SearchText:=txtSearchBox.Text.Trim, IsExpiredMSP:=chkExpiredMSP.Checked)
        dgMSPList.PageIndex = 0
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date)
                ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date)
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
        lblFromDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        lblToDate.Visible = CBool(IIf(DateIndex <> 0, True, False))
        If DateIndex = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
    End Sub
    Private Sub setVariables()
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        MSPText = IIf(cmbMSPText.SelectedIndex <= 0, "", cmbMSPText.SelectedValue)
        No = txtNo.Text.Trim
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text)
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("MSPText") = MSPText
        Session("No") = No
        Session("SearchText") = SearchText
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub ControlEnability()
        'btnPrintTop.Enabled = IIf(dgMSPList.Rows.Count = 0, False, True)
    End Sub
    Private Sub ControlVisibility()
        txtSearchBox.Visible = True
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        'Select Case OrderType
        IsInRoleString = "MSP"
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
    'End
#End Region

#Region " DatafieldBinding "
    Private Sub DataFieldBind()
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        MSPText = Session("MSPText")
        mDistinctTextListForMSP = DistinctTextListForMSP.GetDistinctTextList("32", , True, "(All)")
        cmbMSPText.DataSource = mDistinctTextListForMSP
        DataBind()
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
                cmbDate.Focus()
            End If
            cmbShowE.SelectedIndex = 4
            Session("MiddleFrame") = "wfMSPList_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            ControlEnability()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgMSPList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMSPList.RowCommand
        Dim mId As New Guid
        Dim Idx As Int32
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '' Idx = CInt(e.CommandArgument) 'Commented by Ajay on 13-Jan-2023
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex
                If mMSPList(Idx).OrderNumber <> "" Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in order " + mMSPList(Idx).OrderNumber, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mMSPList(Idx).WorkOrderNumber <> "" Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in work order " + mMSPList(Idx).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                mId = New Guid(dgMSPList.DataKeys(Idx).Value.ToString)
                EditRecord(mId)
                mMSPDetail = "No: " + mMSP.MSPNo + " Dated: " + mMSP.MSPDateFormatted + " Plan Name: " + mMSP.PlanName + " Contract No.: " + mMSP.ContractNo
                MarkLog(Util.Action.Edit, "MSP", mMSPDetail, Util.ErrorType.NoError, mId, EventLogID)

                Dim str As String
                str = "openledgersame('wfMSP_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex

                mId = New Guid(dgMSPList.DataKeys(Idx).Value.ToString)
                DeleteRecord(mId)
            Case "Renew"
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex
                mId = New Guid(dgMSPList.DataKeys(Idx).Value.ToString)
                If (Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                mMSP = MSP.NewMSPRenew(ID:=Guid.NewGuid, ReferenceID:=mId)
                mMSP.MarkClean()
                Session("mMSP") = mMSP

                MarkLog(Flypal.Util.Action.Comply, "MSP", "", Util.ErrorType.NoError, mMSP.ID, EventLogID)
                Dim str As String
                str = "openledgersame('wfMSP_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End Select
    End Sub
    Private Sub dgMSPList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMSPList.PageIndexChanging
        dgMSPList.PageIndex = e.NewPageIndex
        dgMSPList.DataSource = mMSPList
        Session("mMSPList") = mMSPList
        dgMSPList.DataBind()
        dgMSPList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbMSPText.SelectedIndexChanged
        If sender.id = "cmbDate" Then
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            setPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
        ElseIf sender.id = "cmbMSPText" Then
            txtNo.Text = "0"
            If cmbMSPText.Enabled = True Then
                cmbMSPText.Focus()
            End If
        End If
    End Sub
    Private Sub imgFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgFindNow.Click  ''btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgMSPList.DataBind()
        ControlEnability()
        lblResult.Text = "As per criteria :" & mMSPList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlTitle.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click  '',btnAddNew.Click
        If (Not IsInRole(Rights.New)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewRecord()
        MarkLog(Util.Action.[New], "MSP", "", Util.ErrorType.NoError, mMSP.ID, EventLogID)
        Dim str As String
        str = "openledgersame('wfMSP_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click ''btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgMSPList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMSPList.Sorting
        mMSPList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMSPList") = mMSPList
        dgMSPList.DataSource = mMSPList
        dgMSPList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        dgMSPList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgMSPList.DataSource = mMSPList
        dgMSPList.DataBind()

        ControlVisibility(0)
        setVariables()
        SetControl()
        ControlEnability()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub txtSearchBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchBox.TextChanged
        ControlVisibility(0)
        setVariables()
        CallFindNow(SearchIndex)
        dgMSPList.DataBind()

        SetControl()
        ControlEnability()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As MSPList
        Dim ds As New dsMSPList
        myReport = New crptMSPList
        rpt = Session("mMSPList")

        Dim mCompanyDetail As New CompanyDetail
        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                          mCompanyDetail.Email, WebSite:="", ReportName:="", SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText, SearchStr2:=New SmartDate(txtToDate.Text).FormattedText, SearchStr3:=IIf(cmbMSPText.SelectedIndex = 0, "", cmbMSPText.SelectedItem.Text + IIf(txtNo.Text = "", "", "-" + txtNo.Text)),
                                          SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                          SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"),
                                          SearchStr11:="", SearchStr12:="", SearchStr13:="", SearchStr14:="", SearchStr15:="", SearchStr16:="")

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1550)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, rpt)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mReport)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
    '-----
#End Region

End Class