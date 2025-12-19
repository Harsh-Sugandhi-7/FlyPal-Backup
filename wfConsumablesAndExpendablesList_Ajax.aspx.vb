Public Class wfConsumablesAndExpendablesList_Ajax
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
    Public mConsumableAndExpendableList As ConsumableAndExpendableList
    Public mConsumableAndExpendable As ConsumableAndExpendable
    Public mRequisitionListForCombo As RequisitionListForCombo
    Public mDistinctCnEText As DistinctCnEText
    Public mMachineNameValueList As MachineNameValueList
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, RequisitionText, ReqNo, PartNo, SerialNo, Reference, RegNo, TransText, TransNo As String
    Dim ReqID As Guid
    Dim EventLogID As Guid
    Dim mEventLogDetail As String
    Dim totcnt As Integer
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mConsumableAndExpendable = Session("mConsumableAndExpendable")
        mConsumableAndExpendableList = Session("mConsumableAndExpendableList")
        mRequisitionListForCombo = Session("mRequisitionListForCombo")
        mDistinctCnEText = Session("mDistinctCnEText")
        mMachineNameValueList = Session("mMachineNameValueList")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        StatusId = Session("StatusId")
        RequisitionText = Session("RequisitionText")
        ReqNo = IIf(IsNothing(Session("ReqNo")), 0, Session("ReqNo"))
        ReqID = Session("ReqID")
        TransText = Session("TransText")
        TransNo = IIf(IsNothing(Session("TransNo")), 0, Session("TransNo"))
        RegNo = Session("RegNo")
        Reference = Session("Reference")
        PartNo = Session("PartNo")
        SerialNo = Session("SerialNo")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfConsumablesAndExpendablesList_Ajax.aspx?" Then
            Session.Remove("mConsumableAndExpendable")
            Session.Remove("mConsumableAndExpendableList")
            Session.Remove("mRequisitionListForCombo")
            Session.Remove("mDistinctCnEText")
            Session.Remove("mMachineNameValueList")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("StatusId")
            Session.Remove("RequisitionText")
            Session.Remove("SerialNo")
            Session.Remove("ReqNo")
            Session.Remove("ReqID")
            Session.Remove("TransText")
            Session.Remove("TransNo")
            Session.Remove("RegNo")
            Session.Remove("Reference")
            Session.Remove("PartNo")
            Session.Remove("totcnt")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgCEList.DataBind()
        cmbSearch.SelectedIndex = SearchIndex
        cmbDate.SelectedIndex = DateIndex
        cmbStatus.SelectedValue = StatusId
        cmbCEText.SelectedValue = IIf(TransText = "", "All", TransText)
        cmbMachineList.SelectedValue = IIf(RegNo = "", Guid.Empty.ToString(), RegNo)
        If mRequisitionListForCombo.Contains(ReqID) Then 'Added By Rajnish On 01-01-2008
            cmbRequisitionText.SelectedValue = ReqID.ToString
        Else
            cmbRequisitionText.SelectedValue = "(All)"
        End If
        '' cmbRequisitionText.SelectedValue = IIf(RequisitionText = "", "(All)", RequisitionText)
        
        txtName.Text = PartNo
        txtNo.Text = ReqNo
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Consumables & Expendables(C&E) as per criteria :" & mConsumableAndExpendableList.Count & " Record(s) found."
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mConsumableAndExpendable = ConsumableAndExpendable.GetCE(mId)
        mConsumableAndExpendable.MarkClean()
        Session("mConsumableAndExpendable") = mConsumableAndExpendable
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mConsumableAndExpendable = ConsumableAndExpendable.GetCE(mId)
        Session("mConsumableAndExpendable") = mConsumableAndExpendable
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
                            Dim mConsumableAndExpendable As ConsumableAndExpendable
                            Session("Sender") = ""
                            mConsumableAndExpendable = CType(Session("mConsumableAndExpendable"), ConsumableAndExpendable)
                            mConsumableAndExpendable.Delete()
                            mConsumableAndExpendable.Save()
                            DataFieldBind()
                            SetControl()
                            upnlTitle.Update()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mEventLogDetail = mConsumableAndExpendable.CnETextNo + " Dated : " + mConsumableAndExpendable.TransDateFormatted.ToString + " Requested By : " + mConsumableAndExpendable.UserName + " Status : " + IIf(mConsumableAndExpendable.StatusID = 1, "Open", "Authorized")
                                MarkLog(Util.Action.Delete, "ConsumablesAndExpendables", mEventLogDetail, Util.ErrorType.NoError, mConsumableAndExpendable.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
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
    Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Integer = 0, Optional ByVal ReqID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/3300", Optional ByVal StatusID As Integer = 0, Optional ByVal PartNo As String = "", Optional ByVal SerialNo As String = "", Optional ByVal RegNo As String = "", Optional ByVal Reference As String = "")
        mConsumableAndExpendableList = Nothing
        dgCEList.DataSource = Nothing
        mConsumableAndExpendableList = ConsumableAndExpendableList.GetList(Text, No, ReqID, FromDate, ToDate, StatusID, PartNo, SerialNo, RegNo, Reference)
        Session("mConsumableAndExpendableList") = mConsumableAndExpendableList
        dgCEList.DataSource = mConsumableAndExpendableList
        lblResult.Text = "List of Consumables & Expendables(C&E) as per criteria :" & mConsumableAndExpendableList.Count & " Record(s) found."
        SetTitle()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        Select Case Index
            Case -1
                Call FindNow() 'for all records
            Case 0  'all
                Call FindNow() 'for all records
            Case 1 'Transaction date
                Call FindNow(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text)
            Case 2  'Transaction Text , No 
                Call FindNow(Text:=TransText, No:=CInt(Val(TransNo)))
            Case 3 'Req Text , No 
                Call FindNow(ReqID:=ReqID.ToString)
            Case 4 'Aircraft
                Call FindNow(RegNo:=RegNo)
            Case 5 'Part No
                Call FindNow(PartNo:=PartNo)
            Case 6 'serial No
                Call FindNow(SerialNo:=SerialNo)
            Case 7 'Reference
                Call FindNow(Reference:=Reference)
            Case 8 ' Status
                Call FindNow(StatusID:=StatusId)
        End Select
        dgCEList.PageIndex = 0
    End Sub
    Private Sub SetTitle()
        mConsumableAndExpendableList = Session("mConsumableAndExpendableList")
        totcnt = mConsumableAndExpendableList.TotalCount
        Session("totcnt") = totcnt
        LblTitle.Text = "List of Consumables & Expendables(C&E)"
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
        cmbDate.Visible = IIf(SearchIndex = 1, True, False)
        If SearchIndex = 1 And DateIndex = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
        cmbCEText.Visible = IIf(SearchIndex = 2, True, False)
        lblNo.Visible = IIf((SearchIndex = 2 And cmbCEText.SelectedIndex <> 0), True, False)
        txtNo.Visible = IIf((SearchIndex = 2 And cmbCEText.SelectedIndex <> 0), True, False)
        cmbRequisitionText.Visible = IIf(SearchIndex = 3, True, False)
        cmbStatus.Visible = IIf(SearchIndex = 8, True, False)
        txtName.Visible = IIf(SearchIndex = 4 Or SearchIndex = 5 Or SearchIndex = 6 Or SearchIndex = 7, True, False)
    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtName.Text = ""
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
        RequisitionText = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue)
        ReqID = IIf(cmbRequisitionText.SelectedIndex <= 0, Guid.Empty, New Guid(cmbRequisitionText.SelectedValue))
        TransText = IIf(cmbCEText.SelectedIndex <= 0, "All", cmbCEText.SelectedValue)
        RegNo = IIf(cmbMachineList.SelectedIndex > 0, cmbMachineList.SelectedItem.Text, "")
        If cmbSearch.SelectedIndex = 4 Then
            RegNo = txtName.Text.Trim
        ElseIf cmbSearch.SelectedIndex = 5 Then
            PartNo = txtName.Text.Trim
        ElseIf cmbSearch.SelectedIndex = 6 Then
            SerialNo = txtName.Text.Trim
        ElseIf cmbSearch.SelectedIndex = 7 Then
            Reference = txtName.Text.Trim
        End If
        If cmbSearch.SelectedIndex = 2 Then
            TransNo = txtNo.Text.Trim
        ElseIf cmbSearch.SelectedIndex = 3 Then
            ReqNo = txtNo.Text.Trim
        End If


        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("StatusId") = StatusId
        Session("RequisitionText") = RequisitionText
        Session("ReqNo") = ReqNo
        Session("ReqID") = ReqID
        Session("TransText") = TransText
        Session("TransNo") = TransNo
        Session("PartNo") = PartNo
        Session("SerialNo") = SerialNo
        Session("Reference") = Reference
        Session("RegNo") = RegNo
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "ConsumablesAndExpendables"


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
        Session("totcnt") = totcnt
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        StatusId = Session("StatusId")
        RequisitionText = Session("RequisitionText")
        ReqNo = Session("ReqNo")
        TransText = Session("TransText")
        TransNo = Session("TransNo")
        PartNo = Session("PartNo")
        SerialNo = Session("SerialNo")
        RegNo = Session("RegNo")
        Reference = Session("Reference")
        ReqID = Session("ReqID")


        mRequisitionListForCombo = RequisitionListForCombo.GetRequisitionList("(All)", StartingDate:=AppSettings("StartingDateForCnEConsideration").ToString())
        Session("mRequisitionListForCombo") = mRequisitionListForCombo
        cmbRequisitionText.DataSource = mRequisitionListForCombo

        mDistinctCnEText = DistinctCnEText.GetDistinctText("All")
        Session("mDistinctCnEText") = mDistinctCnEText
        cmbCEText.DataSource = mDistinctCnEText

        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(All)")
        cmbMachineList.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList

        DataBind()

        'mTransactionListCount = TransactionListCount.GetTransactionListCountt(TransTypeID)
        'LblTitle.Text = "List of " & mModuleName & "(s) [Total No of Record(s):-" & mTransactionListCount(0).Count & "]"
        'End
    End Sub
#End Region

#Region " Events "
    Private Sub wfConsumablesAndExpendablesList_Ajax_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            cmbDate.Focus()
            Session("MiddleFrame") = "wfConsumablesAndExpendablesList_Ajax.aspx?"
            DataFieldBind()
            SetControl()
        End If
    End Sub
    'Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load


    '    EventLogID = CType(Session("EventLogID"), Guid)
    '    If Not IsPostBack And Session("sender") = "" Then
    '        If cmbSearch.Enabled = True Then
    '            cmbSearch.Focus()
    '        End If
    '        TransTypeID = CInt(Request.QueryString("TransTypeID"))
    '        Session("TransTypeID") = TransTypeID
    '        Session("MiddleFrame") = "wfRequisitionList_Ajax.aspx?TransTypeID=" & TransTypeID
    '        SetValuesByRequisitionType()
    '        DataFieldBind()
    '        SetControl()
    '        ControlEnability()
    '        ControlVisibility()
    '    End If
    'End Sub
    Private Sub dgCEList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCEList.RowCommand
        Dim mId As New Guid
        Dim Idx As Int32
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Idx = CInt(e.CommandArgument)
                mId = New Guid(e.CommandArgument.ToString())
                EditRecord(mId)

                mEventLogDetail = mConsumableAndExpendable.CnETextNo + " Dated : " + mConsumableAndExpendable.TransDateFormatted.ToString + " Requested By : " + mConsumableAndExpendable.UserName + " Status : " + IIf(mConsumableAndExpendable.StatusID = 1, "Open", "Authorized")
                MarkLog(Util.Action.Edit, "ConsumablesAndExpendables", mEventLogDetail, Util.ErrorType.NoError, mId, EventLogID)
                Session("EditCE") = "True"
                Dim str As String
                str = "openledgersame('wfConsumablesAndExpendables_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Idx = CInt(e.CommandArgument)
                mId = New Guid(e.CommandArgument.ToString)
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgCEList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCEList.PageIndexChanging
        dgCEList.PageIndex = e.NewPageIndex
        dgCEList.DataSource = mConsumableAndExpendableList
        Session("mConsumableAndExpendableList") = mConsumableAndExpendableList
        dgCEList.DataBind()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbDate.SelectedIndex = 0
        cmbRequisitionText.SelectedIndex = 0
        cmbMachineList.SelectedIndex = 0
        ClearControls()
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        setPeriod(DateIndex)
        If cmbDate.Enabled = True Then
            cmbDate.Focus()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgCEList.DataBind()
        lblResult.Text = "List of Consumables & Expendables(C&E) as per criteria :" & mConsumableAndExpendableList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlActionBtnBottom.Update()
    End Sub
    'Private Sub cmbRequisitionText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRequisitionText.SelectedIndexChanged
    '    ClearControls()
    '    Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
    '    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
    '    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
    '    If cmbRequisitionText.Enabled = True Then
    '        cmbRequisitionText.Focus()
    '    End If
    'End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        If (Not IsInRole(Rights.New)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mConsumableAndExpendable As ConsumableAndExpendable
        Dim str As String
        mConsumableAndExpendable = ConsumableAndExpendable.NewCE(Guid.NewGuid)
        Session("mConsumableAndExpendable") = mConsumableAndExpendable

        str = "openledgersame('wfConsumablesAndExpendables_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgCEList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCEList.Sorting
        mConsumableAndExpendableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mConsumableAndExpendableList") = mConsumableAndExpendableList
        dgCEList.DataSource = mConsumableAndExpendableList
        dgCEList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbCEText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbCEText.SelectedIndexChanged
        ClearControls()
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
        ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
        If cmbCEText.Enabled = True Then
            cmbCEText.Focus()
        End If
    End Sub
#End Region

    
 

   
    
End Class