Imports javax.transaction

Public Class wfEmpCAAuthorizationList_Ajax
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
    Public mEmpCAAuthorization As EmpCAAuthorization
    Public mEmpCAAuthorizationList As EmpCAAuthorizationList
    Public mDistinctTextListForMSP As DistinctTextListForMSP
    Dim SearchIndex, DateIndex, FromDate, ToDate, EmpCAAuthorizationText, EmpCAAuthorizationNo, SearchText As String
    Dim EventLogID As Guid
    Dim mEmpCAAuthorizationDetail As String
    Dim mFileAttach As FileAttach
    Dim CAType As Integer = 0
    Dim EmployeeID As String = Guid.Empty.ToString
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEmpCAAuthorization = Session("mEmpCAAuthorization")
        mEmpCAAuthorizationList = Session("mEmpCAAuthorizationList")
        mDistinctTextListForMSP = Session("mDistinctTextListForEmpCAAuthorization")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        EmpCAAuthorizationText = Session("EmpCAAuthorizationText")
        EmpCAAuthorizationNo = IIf(IsNothing(Session("EmpCAAuthorizationNo")), 0, Session("EmpCAAuthorizationNo"))
        SearchText = Session("SearchText")
        CAType = Session("CAType")
    End Sub
    Private Sub SetSession()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Session("mEmpCAAuthorizationList") = mEmpCAAuthorizationList
        Session("mDistinctTextListForEmpCAAuthorization") = mDistinctTextListForMSP
        SearchText = Session("SearchText")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmpCAAuthorization")
        Session.Remove("mEmpCAAuthorizationList")
        Session.Remove("mDistinctTextListForEmpCAAuthorization")
        Session.Remove("SearchText")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("EmpCAAuthorizationText")
        Session.Remove("EmpCAAuthorizationNo")
        Session.Remove("BackPage")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfEmpCAAuthorizationList_Ajax.aspx?CAType=" & Val(Request.QueryString("CAType")).ToString Then
            Session.Remove("mEmpCAAuthorization")
            Session.Remove("mEmpCAAuthorizationList")
            Session.Remove("mDistinctTextListForEmpCAAuthorization")
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("EmpCAAuthorizationText")
            Session.Remove("EmpCAAuthorizationNo")
            Session.Remove("BackPage")
            Session.Remove("IsEmpCAAuthorizationForRenew")
        End If
    End Sub
    Private Sub SetControl()
        setPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgEmpCAAuthorizationList.DataBind()
        cmbDate.SelectedIndex = DateIndex
        If cmbEmpCAAuthorizationText.Items.Contains(New System.Web.UI.WebControls.ListItem(EmpCAAuthorizationText)) Then
            cmbEmpCAAuthorizationText.SelectedValue = EmpCAAuthorizationText
        Else
            cmbEmpCAAuthorizationText.SelectedValue = "(All)"
        End If
        txtNo.Text = EmpCAAuthorizationNo
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "As per criteria :" & mEmpCAAuthorizationList.Count & " Record(s) found."
        If Not SearchText Is Nothing Then
            SearchText = IIf(SearchText = "", "", SearchText)
        Else
            SearchText = ""
        End If
    End Sub
    Private Sub NewRecord()
        mEmpCAAuthorization = EmpCAAuthorization.NewEmpCAAuthorization(New Guid)
        mEmpCAAuthorization.EmpCAAuthorizationDate = Today.Date

        If CAType = 1 Then
            mEmpCAAuthorization.EmployeeID = SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeID

            'mEmpCAAuthorization.EmployeeCode = mEmployeeList(New Guid(cmbEmployee.SelectedValue)).EmpNo

            mEmpCAAuthorization.EmployeeCode = Employee.GetEmployee(mEmpCAAuthorization.EmployeeID).EmpNo

            'mEmpCAAuthorization.AMELNo = mEmployeeList(New Guid(cmbEmployee.SelectedValue)).LicenseNo

            mEmpCAAuthorization.AMELNo = Employee.GetEmployee(mEmpCAAuthorization.EmployeeID).LicenseNo

            'mEmpCAAuthorization.AMELCat = Employee.GetEmployee(New Guid(cmbEmployee.SelectedValue)).CAT 'mEmployeeList(New Guid(cmbEmployee.SelectedValue)).ca

            mEmpCAAuthorization.AMELCat = Employee.GetEmployee(mEmpCAAuthorization.EmployeeID).CAT
        End If
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mId)
        If mEmpCAAuthorization.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mId)
            Session("mFileAttachEmpCAAuthorization") = mFileAttach
        End If
        mEmpCAAuthorization.MarkClean()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mId)
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
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
                            Dim mEmpCAAuthorization As EmpCAAuthorization
                            Session("Sender") = ""
                            mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
                            mEmpCAAuthorization.Delete()
                            mEmpCAAuthorization.Save()
                            DataFieldBind()
                            SetControl()
                            ControlEnability()
                            upnlTitle.Update()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'If ex.Message.Contains("FKtabOrdertabMSP") Then
                                '    stringInfo = "Order."
                                'ElseIf ex.Message.Contains("FKtabnWOtabMSP") Then
                                '    stringInfo = "Work Order."
                                'End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mEmpCAAuthorizationDetail = "Authorization No.: " + mEmpCAAuthorization.CANumber + " Dated: " + mEmpCAAuthorization.EmpCAAuthorizationDateFormatted + " Employee: " + mEmpCAAuthorization.EmployeeName + " Code: " + mEmpCAAuthorization.EmployeeCode
                                MarkLog(Util.Action.Delete, "EmpCAAuthorization", mEmpCAAuthorizationDetail, Util.ErrorType.NoError, mEmpCAAuthorization.ID, EventLogID)
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
                        Optional ByVal SearchText As String = "", Optional ByVal IsExpiredMSP As Boolean = False, Optional ByVal CAOpenType As Integer = 0, Optional ByVal EmployeeIDforCA As String = "{00000000-0000-0000-0000-000000000000}")
        mEmpCAAuthorizationList = Nothing
        dgEmpCAAuthorizationList.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mEmpCAAuthorizationList = EmpCAAuthorizationList.GetEmpCAAuthorizationList(FromDate:=FromDate, ToDate:=ToDate, EmpCAAuthorizationText:=Text, EmpCAAuthorizationNo:=No, SearchText:=SearchText, IsExpiredEmpCAAuthorization:=IsExpiredMSP, CAtype:=CAType, EmployeeIDforCA:=EmployeeIDforCA)
        'Set DataSource of the Grid
        Session("mEmpCAAuthorizationList") = mEmpCAAuthorizationList
        dgEmpCAAuthorizationList.DataSource = mEmpCAAuthorizationList
        lblResult.Text = "As per criteria :" & mEmpCAAuthorizationList.Count & " Record(s) found."
        dgEmpCAAuthorizationList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        btnPrintTop.Enabled = (mEmpCAAuthorizationList.Count > 0)
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        EmployeeID = SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeID.ToString()
        FindNow(FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, Text:=Trim(EmpCAAuthorizationText), No:=CInt(Val(EmpCAAuthorizationNo)),
                SearchText:=txtSearchBox.Text.Trim, IsExpiredMSP:=chkExpiredMSP.Checked, CAOpenType:=CAType, EmployeeIDforCA:=EmployeeID)
        dgEmpCAAuthorizationList.PageIndex = 0
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

        btnAddNewTop.Visible = IIf(CAType > 1, False, True)
        If CAType = 1 Then
            If dgEmpCAAuthorizationList.Rows.Count >= 1 Then
                btnAddNewTop.Visible = False
            Else
                btnAddNewTop.Visible = True
            End If

        End If

    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
    End Sub
    Private Sub setVariables()
        DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        EmpCAAuthorizationText = IIf(cmbEmpCAAuthorizationText.SelectedIndex <= 0, "", cmbEmpCAAuthorizationText.SelectedValue)
        EmpCAAuthorizationNo = txtNo.Text.Trim
        SearchText = IIf(txtSearchBox.Text = "", "", txtSearchBox.Text)
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("EmpCAAuthorizationText") = EmpCAAuthorizationText
        Session("EmpCAAuthorizationNo") = EmpCAAuthorizationNo
        Session("SearchText") = SearchText
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub ControlEnability()
        'btnPrintTop.Enabled = IIf(dgEmpCAAuthorizationList.Rows.Count = 0, False, True)
    End Sub
    Private Sub ControlVisibility()
        txtSearchBox.Visible = True
        btnAddNewTop.Visible = IIf(CAType > 1, False, True)
        If CAType = 1 Then
            If dgEmpCAAuthorizationList.Rows.Count >= 1 Then
                btnAddNewTop.Visible = False
            Else
                btnAddNewTop.Visible = True
            End If

        End If

    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights
        'Select Case OrderType
        If CAType = 1 Then
            IsInRoleString = "EmpCAApplyForCA"
        ElseIf CAType = 2 Then
            IsInRoleString = "EmpCAValidateForCA"
        ElseIf CAType = 3 Then
            IsInRoleString = "EmpCAApproveForCA"
        Else
            IsInRoleString = "EmpCAAuthorization"
        End If
        'IsInRoleString = "EmpCAAuthorization"
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
        DateIndex = IIf(IsNothing(DateIndex), 0, DateIndex)
        EmpCAAuthorizationText = Session("EmpCAAuthorizationText")
        mDistinctTextListForMSP = DistinctTextListForMSP.GetDistinctTextList("33", , True, "(All)") '33 is for Emp CA Authorization Text
        cmbEmpCAAuthorizationText.DataSource = mDistinctTextListForMSP
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
            CAType = Val(Request.QueryString("CAType"))
            Session("MiddleFrame") = "wfEmpCAAuthorizationList_Ajax.aspx?CAType=" & CAType.ToString
            Session("CAType") = CAType
            DataFieldBind()
            SetControl()
            ControlEnability()
            ControlVisibility()
        End If
        upnlSearchCriteria.Visible = IIf(CAType > 1, True, False)
        If (CAType > 2) Then
            chkExpiredMSP.Visible = True
        Else
            chkExpiredMSP.Visible = False
        End If

    End Sub
    Private Sub dgEmpCAAuthorizationList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmpCAAuthorizationList.RowCommand
        Dim mId As New Guid
        Dim Idx As Int32
        Select Case e.CommandName
            Case "EditRec"
                If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex
                'If mEmpCAAuthorizationList(Idx).OrderNumber <> "" Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in order " + mEmpCAAuthorizationList(Idx).OrderNumber, MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                'If mEmpCAAuthorizationList(Idx).WorkOrderNumber <> "" Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in work order " + mEmpCAAuthorizationList(Idx).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                mId = New Guid(dgEmpCAAuthorizationList.DataKeys(Idx).Value.ToString)
                EditRecord(mId)
                'mEmpCAAuthorizationDetail = "Authorization No.: " + mEmpCAAuthorization.CANumber + " Dated: " + mEmpCAAuthorization.EmpCAAuthorizationDateFormatted + " Plan Name: " + mEmpCAAuthorization.EmployeeName + " Contract No.: " + mEmpCAAuthorization.EmployeeCode
                mEmpCAAuthorizationDetail = " Dated: " + mEmpCAAuthorization.EmpCAAuthorizationDateFormatted + " Plan Name: " + mEmpCAAuthorization.EmployeeName + " Contract No.: " + mEmpCAAuthorization.EmployeeCode
                MarkLog(Util.Action.Edit, "EmpCAAuthorization", mEmpCAAuthorizationDetail, Util.ErrorType.NoError, mId, EventLogID)

                Dim str As String

                If CAType = 1 Then
                    str = "openledgersame('wfEmpCAApplication_Ajax.aspx?BackPage=index.aspx');"
                ElseIf CAType = 2 Then
                    str = "openledgersame('wfEmpCAApplication_Ajax.aspx?BackPage=index.aspx');"
                ElseIf CAType = 3 Then
                    str = "openledgersame('wfEmpCAApplication_Ajax.aspx?BackPage=index.aspx');"
                Else
                    str = "openledgersame('wfEmpCAAuthorization_Ajax.aspx?BackPage=index.aspx');"
                End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "DeleteRec"
                If (Not IsInRole(Rights.Delete)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex

                mId = New Guid(dgEmpCAAuthorizationList.DataKeys(Idx).Value.ToString)
                DeleteRecord(mId)
            Case "Renew"
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex
                mId = New Guid(dgEmpCAAuthorizationList.DataKeys(Idx).Value.ToString)
                If (Not IsInRole(Rights.Edit)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                Dim mEmpCAAuthorizationOLD As EmpCAAuthorization

                mEmpCAAuthorizationOLD = EmpCAAuthorization.GetEmpCAAuthorization(mId)
                mEmpCAAuthorization = EmpCAAuthorization.NewEmpCAAuthorizationRenew(ID:=Guid.NewGuid, mEmpCAAuthorizationOLD)

                If mEmpCAAuthorizationOLD.IsAttachmentAdded = True Then
                    mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorizationOLD.ID)
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mEmpCAAuthorization.ID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension)
                    Session("mFileAttachEmpCAAuthorization") = mFileAttach
                End If

                mEmpCAAuthorization.MarkClean()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session("IsEmpCAAuthorizationForRenew") = "True"
                MarkLog(Flypal.Util.Action.Comply, "EmpCAAuthorization", "", Util.ErrorType.NoError, mEmpCAAuthorization.ID, EventLogID)
                Dim str As String
                str = "openledgersame('wfEmpCAAuthorization_Ajax.aspx?BackPage=index.aspx');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
            Case "History"
                mId = New Guid(e.CommandArgument.ToString)
                mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mId)
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpCAAuthorizationHistoryWindow", "OpenEmpCAAuthorizationHistoryWindow()", True)
        End Select
    End Sub
    Private Sub dgEmpCAAuthorizationList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEmpCAAuthorizationList.PageIndexChanging
        dgEmpCAAuthorizationList.PageIndex = e.NewPageIndex
        dgEmpCAAuthorizationList.DataSource = mEmpCAAuthorizationList
        Session("mEmpCAAuthorizationList") = mEmpCAAuthorizationList
        dgEmpCAAuthorizationList.DataBind()
        dgEmpCAAuthorizationList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
    End Sub
    Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbEmpCAAuthorizationText.SelectedIndexChanged
        If sender.id = "cmbDate" Then
            Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
            ControlVisibility(1, DateIndex)
            setPeriod(DateIndex)
            If cmbDate.Enabled = True Then
                cmbDate.Focus()
            End If
        ElseIf sender.id = "cmbEmpCAAuthorizationText" Then
            txtNo.Text = "0"
            If cmbEmpCAAuthorizationText.Enabled = True Then
                cmbEmpCAAuthorizationText.Focus()
            End If
        End If
    End Sub
    Private Sub imgFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgEmpCAAuthorizationList.DataBind()
        ControlEnability()
        lblResult.Text = "As per criteria :" & mEmpCAAuthorizationList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlTitle.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click
        If (Not IsInRole(Rights.New)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewRecord()
        Session("IsEmpCAAuthorizationForRenew") = "False"
        MarkLog(Util.Action.[New], "EmpCAAuthorization", "", Util.ErrorType.NoError, mEmpCAAuthorization.ID, EventLogID)
        Dim str As String




        If CAType = 1 Then
            str = "openledgersame('wfEmpCAApplication_Ajax.aspx?BackPage=index.aspx');"
        ElseIf CAType = 2 Then
            str = "openledgersame('wfEmpCAApplication_Ajax.aspx?BackPage=index.aspx');"
        Else
            str = "openledgersame('wfEmpCAAuthorization_Ajax.aspx?BackPage=index.aspx');"
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgEmpCAAuthorizationList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEmpCAAuthorizationList.Sorting
        mEmpCAAuthorizationList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mEmpCAAuthorizationList") = mEmpCAAuthorizationList
        dgEmpCAAuthorizationList.DataSource = mEmpCAAuthorizationList
        dgEmpCAAuthorizationList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        dgEmpCAAuthorizationList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgEmpCAAuthorizationList.DataSource = mEmpCAAuthorizationList
        dgEmpCAAuthorizationList.DataBind()

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
        dgEmpCAAuthorizationList.DataBind()

        SetControl()
        ControlEnability()
        ControlVisibility()
        upnlGrid.Update()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As EmpCAAuthorizationList
        Dim ds As New dsEmpCAAuthorizationList
        myReport = New crptEmpCAAuthorizationList
        rpt = Session("mEmpCAAuthorizationList")

        Dim mCompanyDetail As New CompanyDetail
        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                    mCompanyDetail.Email, WebSite:="List Of Company Authorization", ReportName:="", SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText,
                                    SearchStr2:=New SmartDate(txtToDate.Text).FormattedText,
                                    SearchStr3:=IIf(cmbEmpCAAuthorizationText.SelectedIndex = 0, "", cmbEmpCAAuthorizationText.SelectedItem.Text + IIf(txtNo.Text = "", "", "-" + txtNo.Text)),
                                    SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                    SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"),
                                    SearchStr11:="", SearchStr12:="", SearchStr13:="", SearchStr14:="", SearchStr15:="", SearchStr16:="",
                                    SearchStr17:="", SearchStr18:="", SearchStr19:="", SearchStr20:="", SearchStr21:="", SearchStr22:="",
                                    SearchStr23:="", SearchStr24:="", SearchStr25:="", SearchStr26:="", SearchStr27:="", SearchStr28:="",
                                    SearchStr29:="", SearchStr30:="", SearchStr31:="", SearchStr32:="", SearchStr33:="", SearchStr34:="",
                                    SearchStr35:="", SearchStr36:="", SearchStr37:="", SearchStr38:="", SearchStr39:="", SearchStr40:="",
                                    SearchStr41:="", SearchStr42:="", SearchStr43:="", SearchStr44:="", SearchStr45:="", SearchStr46:="",
                                    SearchStr47:="", SearchStr48:="", SearchStr49:="", SearchStr50:="")

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
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

    Private Sub chkExpiredMSP_CheckedChanged(sender As Object, e As EventArgs) Handles chkExpiredMSP.CheckedChanged
        setVariables()
        CallFindNow(SearchIndex)
        dgEmpCAAuthorizationList.DataBind()
        ControlEnability()
        lblResult.Text = "As per criteria :" & mEmpCAAuthorizationList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlTitle.Update()
    End Sub

#End Region

End Class