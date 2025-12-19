'AJAX Conversion by vikrant on 04-Aug-2015

Public Class wfMaintenanceInvoiceList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMaintenanceInvoice As MaintenanceInvoice
    Public mMaintenanceInvoiceList As MaintenanceInvoiceList
    Public mVendorList As VendorList
    Public mChargesForList As ChargesForList
    Dim objSearch As rptSearchingCriteriaForReceipt
    Dim objReg As rptMainInvReg
    Dim SearchIndex, FromDate, ToDate, VendorId, ChargeForText, InvoiceText, No As String
    'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
    Public mDistinctTextListForMaintenanceInvoice As DistinctTextListForMaintenanceInvoice
    Dim EventLogID As Guid 'Added By Utkarsh On 21-Jul-2011 For All19072011
    Dim MIDetail As String 'Added By Utkarsh On 21-Jul-2011 For All19072011
    Dim totcnt As Integer 'Added by shweta on 23-12-11
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMaintenanceInvoice = Session("mMaintenanceInvoice")
        mMaintenanceInvoiceList = Session("mMaintenanceInvoiceList")
        mVendorList = Session("mVendorList")
        mChargesForList = Session("mChargesForList")
        SearchIndex = Session("SearchIndex")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        VendorId = IIf(Session("VendorText") = "", "", Session("VendorId"))
        ChargeForText = IIf(Session("IssueText") = "(All)", "", Session("ChargeForText"))
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        mDistinctTextListForMaintenanceInvoice = Session("mDistinctTextListForMaintenanceInvoice")
        InvoiceText = Session("InvoiceText")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMaintenanceInvoice")
        Session.Remove("mMaintenanceInvoiceList")
        Session.Remove("mVendorList")
        Session.Remove("mChargesForList")
        Session.Remove("SearchIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("VendorId")
        Session.Remove("ChargeForText")
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        Session.Remove("mDistinctTextListForMaintenanceInvoice")

        Session.Remove("InvoiceText")
        Session.Remove("No")
        Session.Remove("totcnt")
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfMaintenanceInvoiceList_Ajax.aspx") <= 0 Then
            RemoveSession()
        End If
    End Sub
    Private Sub SetControl()
        FromDate = IIf(FromDate = "1/1/1900" Or Not IsDate(FromDate), Today.Date, FromDate)
        ToDate = IIf(ToDate = "1/1/2200" Or Not IsDate(ToDate), Today.Date, ToDate)
        txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))   'Today.Date
        txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))

        CallFindNow(SearchIndex)
        dgMaintenanceInvoiceList.DataBind()

        lblResult.Text = "List of Maintenance Invoice as per criteria :" & mMaintenanceInvoiceList.Count & " Record(s) found."

        cmbSearch.SelectedIndex = SearchIndex
        cmbVendorText.SelectedValue = VendorId
        'cmbChargeForText.SelectedValue = IIf(ChargeForText <> "", ChargeForText, "(All)")

        If cmbChargeForText.Items.Contains(New System.Web.UI.WebControls.ListItem(ChargeForText)) Then 'Added By Rajnish On 26-03-2008
            cmbChargeForText.SelectedValue = ChargeForText
        Else
            cmbChargeForText.SelectedValue = "(All)"
        End If
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        ''cmbMaintenanceInvoiceText.SelectedValue = IIf(InvoiceText <> "", "(All)", InvoiceText)
        ''txtNo.Text = InvoiceNo

        If cmbMaintenanceInvoiceText.Items.Contains(New System.Web.UI.WebControls.ListItem(InvoiceText)) Then 'Added By Rajnish On 04-01-2008
            cmbMaintenanceInvoiceText.SelectedValue = InvoiceText
        Else
            cmbMaintenanceInvoiceText.SelectedValue = "(All)"
        End If
        txtNo.Text = No

        ControlVisibility(SearchIndex)
    End Sub
    Private Sub NewRecord()
        mMaintenanceInvoice = MaintenanceInvoice.NewMaintenanceInvoice
        'mMaintenanceInvoice.ChargeFor = "(All)"
        'mMaintenanceInvoice.MarkClean()
        Session("mMaintenanceInvoice") = mMaintenanceInvoice
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mMaintenanceInvoice = MaintenanceInvoice.GetMaintenanceInvoice(mId)
        'mMaintenanceInvoice.MarkClean()
        Session("mMaintenanceInvoice") = mMaintenanceInvoice
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        ' mInvoice.InvoiceItems.CurrentIndex = index
        mMaintenanceInvoice = MaintenanceInvoice.GetMaintenanceInvoice(mId)
        Session("mMaintenanceInvoice") = mMaintenanceInvoice
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
                            Dim mMaintenanceInvoice As MaintenanceInvoice
                            Session("Sender") = ""
                            mMaintenanceInvoice = CType(Session("mMaintenanceInvoice"), MaintenanceInvoice)
                            MIDetail = mMaintenanceInvoiceList(mMaintenanceInvoice.ID).InvoiceTextNo.Replace("/", "-") + " Dated : " + mMaintenanceInvoiceList(mMaintenanceInvoice.ID).Date1Formatted + " from " + mMaintenanceInvoiceList(mMaintenanceInvoice.ID).VendorName
                            'mInvoice.DeleteInvoice(mInvoice.ID)
                            mMaintenanceInvoice.Delete()
                            mMaintenanceInvoice.Save()
                            DataFieldBind()
                            SetControl()
                            SetTitle()
                            ControlVisibility()
                            upnlGrid.Update()
                            upnlActionBtn.Update()
                            upnlActionBtnTop.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Changed By Utkarsh On 22-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Maintenance Invoice", "Can't delete : " & MIDetail & " is Currently in use", Util.ErrorType.NoError, mMaintenanceInvoice.ID, EventLogID)
                                'End
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 22-Jul-2011 For All19072011
                                MarkLog(Util.Action.Delete, "Maintenance Invoice", MIDetail, Util.ErrorType.NoError, mMaintenanceInvoice.ID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    'Changed By Yogita on 19-Dec-2007 suggested by Kalpesh Sir
    Private Sub FindNow(Optional ByVal ChargeFor As String = "", Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal FromInvoiceDate As String = "1/1/1900", Optional ByVal ToInvoiceDate As String = "1/1/2200", Optional ByVal VendorID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal SearchFor As Integer = 0, Optional ByVal InvoiceText As String = "", Optional ByVal InvoiceNo As Integer = 0)
        'Private Sub FindNow(Optional ByVal ChargeFor As String = "", Optional ByVal FromDate As String = "1/1/1900",Optional ByVal ToDate As String = "1/1/2200", Optional ByVal FromInvoiceDate As String = "1/1/1900",Optional ByVal ToInvoiceDate As String = "1/1/2200", Optional ByVal VendorID As String = "{00000000-0000-0000-0000-000000000000}",Optional ByVal SearchFor As Integer = 0)
        mMaintenanceInvoiceList = Nothing
        dgMaintenanceInvoiceList.DataSource = Nothing
        mMaintenanceInvoiceList = MaintenanceInvoiceList.GetMaintenanceInvoiceList(ChargeFor, FromDate, ToDate, FromInvoiceDate, ToInvoiceDate, VendorID, SearchFor, InvoiceText, InvoiceNo)
        'Set DataSource of the Grid
        Session("mMaintenanceInvoiceList") = mMaintenanceInvoiceList
        dgMaintenanceInvoiceList.DataSource = mMaintenanceInvoiceList
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        'If txtNo.Text = "" Or IsNumeric(txtNo.Text) = False Then txtNo.Text = "0"
        Select Case Index
            'Changed By Yogita on 19-Dec-2007 suggested by Kalpesh Sir
            Case 0  'All
                FindNow()
            Case 1  'Entry Date
                FindNow("", FromDate, ToDate, "1/1/1900", "1/1/2200", , 1)
            Case 2  'Charge For
                FindNow(ChargeForText, , , , )
            Case 3 'Vendor Name
                FindNow(, , , , , VendorId, 1)
            Case 4 'Vendor Invoice Date
                FindNow(, , , FromDate, ToDate, , 2)
            Case 5
                FindNow(, , , "1/1/1900", "1/1/2200", , 1, InvoiceText, CInt(Val(No)))
        End Select
        dgMaintenanceInvoiceList.PageIndex = 0    'Added Code on May,25,2007
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = IIf(mMaintenanceInvoiceList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mMaintenanceInvoiceList.Count = 0, False, True)
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32)
        cmbChargeForText.Visible = IIf(SearchIndex = 2, True, False)
        cmbVendorText.Visible = IIf(SearchIndex = 3, True, False)
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        cmbMaintenanceInvoiceText.Visible = IIf(SearchIndex = 5, True, False)
        lblNo.Visible = IIf(SearchIndex = 5 And cmbMaintenanceInvoiceText.SelectedIndex <> 0, True, False)
        txtNo.Visible = IIf(SearchIndex = 5 And cmbMaintenanceInvoiceText.SelectedIndex <> 0, True, False)

        If SearchIndex = 1 Or SearchIndex = 4 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        Else
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub CallFindNowReport(ByVal Index As Integer)
        'If txtNo.Text = "" Or IsNumeric(txtNo.Text) = False Then txtNo.Text = "0"
        Dim VendorText As String = ""
        VendorText = IIf(cmbVendorText.SelectedIndex <= 0, "{00000000-0000-0000-0000-000000000000}", cmbVendorText.SelectedValue.ToString)
        Dim ChargeForText As String = ""
        ChargeForText = IIf(cmbChargeForText.SelectedIndex <= 0, "", cmbChargeForText.SelectedItem.Text)
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        InvoiceText = IIf(cmbMaintenanceInvoiceText.SelectedIndex <= 0, "", cmbMaintenanceInvoiceText.SelectedItem.Text)
        Select Case Index
            Case 0  'All
                objReg = rptMainInvReg.GetMainInvRegList("1/1/1900", "1/1/2200", , , , , "1/1/1900", "1/1/2200")
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1/1/1900", "1/1/2200")
            Case 1  ' 'Entry Date
                objReg = rptMainInvReg.GetMainInvRegList(txtFromDate.Text, txtToDate.Text, , , , , "1/1/1900", "1/1/2200")
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), txtFromDate.Text, txtToDate.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1/1/1900", "1/1/2200")
            Case 2  'Charge For
                objReg = rptMainInvReg.GetMainInvRegList("1/1/1900", "1/1/2200", , , , ChargeForText, "1/1/1900", "1/1/2200")
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ChargeForText, "", "1/1/1900", "1/1/2200")
            Case 3 'Vendor Name
                objReg = rptMainInvReg.GetMainInvRegList("1/1/1900", "1/1/2200", VendorText, , , , "1/1/1900", "1/1/2200")
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", VendorText, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "1/1/1900", "1/1/2200")
            Case 4  'Vendor Invoice Date
                objReg = rptMainInvReg.GetMainInvRegList("1/1/1900", "1/1/2200", , , , , txtFromDate.Text, txtToDate.Text)
                objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "1/1/1900", "1/1/2200", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", txtFromDate.Text, txtToDate.Text)
        End Select
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        VendorId = cmbVendorText.SelectedValue
        ChargeForText = IIf(cmbChargeForText.SelectedIndex <= 0, "", cmbChargeForText.SelectedValue)
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        InvoiceText = IIf(cmbMaintenanceInvoiceText.SelectedIndex <= 0, "", cmbMaintenanceInvoiceText.SelectedValue)
        No = txtNo.Text.Trim

        Session("SearchIndex") = SearchIndex
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("VendorId") = VendorId
        Session("ChargeForText") = ChargeForText
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        Session("InvoiceText") = InvoiceText
        Session("No") = No
    End Sub
    Private Sub ClearControls()
        '' cmbChargeForText.SelectedIndex = 0 ''Commented By Rajnish on 12-01-2008
        '' cmbVendorText.SelectedIndex = 0 ''Commented By Rajnish on 12-01-2008
        'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        ''cmbMaintenanceInvoiceText.SelectedIndex = 0   ''Commented By Rajnish on 12-01-2008
        txtNo.Text = ""
    End Sub
    Private Sub setPeriod(ByVal Index As Int32)
        Select Case Index
            Case 0, 2, 3, 4, 5 ' All   
                txtFromDate.Text = CDate("1-Jan-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("1-Jan-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                FromDate = Today.Date.ToString
                ToDate = Today.Date.ToString
                txtFromDate.Text = CDate(FromDate).ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate(ToDate).ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
    End Sub
    Private Sub SetTitle()
        totcnt = TransactionListCount.GetTransactionListCountt(30)(0).Count
        Session("totcnt") = totcnt
        lblList.Text = "List of Maintenance Invoice" + " [Total No of Record(s):-" + totcnt.ToString() + "]" 'Added by shweta on 23-12-11
        upnlTitle.Update()
    End Sub
#End Region

#Region " DataFielBinding "
    Private Sub DataFieldBind()
        Session("totcnt") = totcnt
        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        VendorId = Session("VendorId")
        ChargeForText = Session("ChargeForText")
        InvoiceText = Session("InvoiceText")
        No = Session("No")

        mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)")
        cmbVendorText.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        mChargesForList = ChargesForList.getChargeForList(True, "(All)")
        cmbChargeForText.DataSource = mChargesForList
        Session("mChargesForList") = mChargesForList

        'mMaintenanceInvoiceList = MaintenanceInvoiceList.GetMaintenanceInvoiceList("", "1/1/1900", "1/1/2200", "1/1/1900", "1/1/2200", "{00000000-0000-0000-0000-000000000000}", MaintenanceInvoiceList.SearchFor.All)
        'dgMaintenanceInvoiceList.DataSource = mMaintenanceInvoiceList
        'Session("mMaintenanceInvoiceList") = mMaintenanceInvoiceList

        mDistinctTextListForMaintenanceInvoice = DistinctTextListForMaintenanceInvoice.GetDistinctText("12", , True, "(All)")
        cmbMaintenanceInvoiceText.DataSource = mDistinctTextListForMaintenanceInvoice
        Session("mDistinctTextListForMaintenanceInvoice") = mDistinctTextListForMaintenanceInvoice

        ''New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
        'totcnt = mMaintenanceInvoiceList.Count 'Added by shweta on 23-12-11
        'Session("totcnt") = totcnt  'Added by shweta on 23-12-11

        DataBind()
    End Sub
    Private Sub GridBind()
        dgMaintenanceInvoiceList.DataSource = mMaintenanceInvoiceList
        dgMaintenanceInvoiceList.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 21-Jul-2011 For All19072011
        If Not IsPostBack Then
            If cmbSearch.Enabled = True Then
                cmbSearch.Focus()
            End If
            Session("MiddleFrame") = "wfMaintenanceInvoiceList_Ajax.aspx"
            DataFieldBind()
            SetControl()
            SetTitle()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgMaintenanceInvoiceList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMaintenanceInvoiceList.RowCommand
        Dim mId As Guid
        Dim Index As Int32
        Select Case e.CommandName
            Case "EditRec"
                GridBind()
                Index = CInt(e.CommandArgument) + dgMaintenanceInvoiceList.PageSize * dgMaintenanceInvoiceList.PageIndex
                mId = mMaintenanceInvoiceList(Index).ID
                If (Not User.IsInRole("MaintenanceInvoiceView") And Not User.IsInRole("MaintenanceInvoiceEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                Session("Edit") = True
                EditRecord(mId)

                'Changed By Utkarsh On 22-Jul-2011 For All19072011
                MIDetail = mMaintenanceInvoiceList(mMaintenanceInvoice.ID).InvoiceTextNo.Replace("/", "-") + " Dated : " + mMaintenanceInvoiceList(mMaintenanceInvoice.ID).Date1Formatted + " from " + mMaintenanceInvoiceList(mMaintenanceInvoice.ID).VendorName
                MarkLog(Util.Action.Edit, "Maintenance Invoice", MIDetail, Util.ErrorType.NoError, mMaintenanceInvoice.ID, EventLogID)
                'End
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMaintenanceInvoice_Ajax.aspx?BackPage=index.aspx');", True)
            Case "DeleteRec"
                GridBind()
                Index = CInt(e.CommandArgument) + dgMaintenanceInvoiceList.PageSize * dgMaintenanceInvoiceList.PageIndex
                mId = mMaintenanceInvoiceList(Index).ID
                If (Not User.IsInRole("MaintenanceInvoiceDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgMaintenanceInvoiceList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMaintenanceInvoiceList.PageIndexChanging
        dgMaintenanceInvoiceList.PageIndex = e.NewPageIndex
        dgMaintenanceInvoiceList.DataSource = mMaintenanceInvoiceList
        Session(" mMaintenanceInvoiceList") = mMaintenanceInvoiceList
        dgMaintenanceInvoiceList.DataBind()
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbChargeForText.SelectedIndex = 0
        cmbVendorText.SelectedIndex = 0
        cmbMaintenanceInvoiceText.SelectedIndex = 0
        ClearControls()
        ControlVisibility(cmbSearch.SelectedIndex)
        setPeriod(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If
    End Sub
    'New Addition By Yogita on 13-Dec-2007 to solve Bug No:-MIL4
    Private Sub cmbMaintenanceInvoiceText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMaintenanceInvoiceText.SelectedIndexChanged
        ClearControls() 'Added By Rajnish on 12-01-2008
        Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbMaintenanceInvoiceText.Enabled = True Then
            cmbMaintenanceInvoiceText.Focus()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgMaintenanceInvoiceList.DataBind()
        ControlVisibility()
        lblResult.Text = "List of Maintenance Invoice as per criteria :" & mMaintenanceInvoiceList.Count & " Record(s) found."
        upnlGrid.Update()
        upnlActionBtn.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        If (Not User.IsInRole("MaintenanceInvoiceNew") And Not User.IsInRole("MaintenanceInvoiceEdit")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewRecord()
        'Changed By Utkarsh On 22-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Maintenance Invoice", "", Util.ErrorType.NoError, mMaintenanceInvoice.ID, EventLogID)
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMaintenanceInvoice_Ajax.aspx?BackPage=index.aspx');", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgMaintenanceInvoiceList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMaintenanceInvoiceList.Sorting
        mMaintenanceInvoiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMaintenanceInvoiceList") = mMaintenanceInvoiceList
        dgMaintenanceInvoiceList.DataSource = mMaintenanceInvoiceList
        dgMaintenanceInvoiceList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region " Report "
    'Created By :- Jyoti
    'Dated On 11/5/2007

#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        If Not User.IsInRole("MaintenanceInvoicePrint") Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        'For Maintenance Invoice List
        Dim Rpt As New crMaintenanceList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        GridBind()

        If cmbSearch.SelectedIndex = 0 Then
            'All
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbSearch.SelectedIndex = 1 Then
            'Entry Date
            SearchStr1 = "The report shows records filtered by the following criteria"
            'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + lblFromDate.Text + " " + txtFromDate.Value.ToString + " " + lblToDate.Text + " " + txtToDate.Value.ToString
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
        ElseIf cmbSearch.SelectedIndex = 2 Then
            'Charge For
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbChargeForText.SelectedItem.Text
        ElseIf cmbSearch.SelectedIndex = 3 Then
            'Vendor
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbVendorText.SelectedItem.Text
        ElseIf cmbSearch.SelectedIndex = 4 Then
            'Vendor Invoice Date
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
        ElseIf cmbSearch.SelectedIndex = 5 And cmbMaintenanceInvoiceText.SelectedIndex > 0 Then
            'Enquiry No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbMaintenanceInvoiceText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text
        ElseIf cmbSearch.SelectedIndex = 5 Then
            'Enquiry No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbMaintenanceInvoiceText.SelectedItem.Text '' + " " + lblNo.Text + " " + txtNo.Text

        End If

        ReportDetails.Add(New rptStatus(, 0, , _
                    dgMaintenanceInvoiceList.Columns.Item(1).HeaderText, dgMaintenanceInvoiceList.Columns.Item(2).HeaderText, dgMaintenanceInvoiceList.Columns.Item(3).HeaderText, _
                    dgMaintenanceInvoiceList.Columns.Item(4).HeaderText, dgMaintenanceInvoiceList.Columns.Item(5).HeaderText, dgMaintenanceInvoiceList.Columns.Item(6).HeaderText, _
                    dgMaintenanceInvoiceList.Columns.Item(7).HeaderText, dgMaintenanceInvoiceList.Columns.Item(8).HeaderText, dgMaintenanceInvoiceList.Columns.Item(9).HeaderText, _
                    dgMaintenanceInvoiceList.Columns.Item(10).HeaderText, dgMaintenanceInvoiceList.Columns.Item(11).HeaderText, dgMaintenanceInvoiceList.Columns.Item(12).HeaderText, _
                    dgMaintenanceInvoiceList.Columns.Item(13).HeaderText, dgMaintenanceInvoiceList.Columns.Item(14).HeaderText, dgMaintenanceInvoiceList.Columns.Item(15).HeaderText, _
                    dgMaintenanceInvoiceList.Columns.Item(16).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mMaintenanceInvoiceList.Count

        Dim mCurrentPageindex As Integer = Me.dgMaintenanceInvoiceList.PageIndex 'Code Added				
        TotalCount = Me.dgMaintenanceInvoiceList.PageCount
        Dim j As Integer
        Dim I As Integer
        Dim str(15) As String

        For j = 0 To TotalCount - 1

            Me.dgMaintenanceInvoiceList.PageIndex = j
            Me.dgMaintenanceInvoiceList.DataSource = mMaintenanceInvoiceList
            Session("mMaintenanceInvoiceList") = mMaintenanceInvoiceList
            dgMaintenanceInvoiceList.DataBind()
            For I = 0 To Me.dgMaintenanceInvoiceList.PageSize - 1
                If I <= Me.dgMaintenanceInvoiceList.Rows.Count - 1 Then


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
                    str(13) = ""
                    str(14) = ""
                    str(15) = ""


                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(1).Text <> "&nbsp;" Then str(0) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(1).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(2).Text <> "&nbsp;" Then str(1) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(2).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(3).Text <> "&nbsp;" Then str(2) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(3).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(4).Text <> "&nbsp;" Then str(3) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(4).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(5).Text <> "&nbsp;" Then str(4) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(5).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(6).Text <> "&nbsp;" Then str(5) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(6).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(7).Text <> "&nbsp;" Then str(6) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(7).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(8).Text <> "&nbsp;" Then str(7) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(8).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(9).Text <> "&nbsp;" Then str(8) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(9).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(10).Text <> "&nbsp;" Then str(9) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(10).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(11).Text <> "&nbsp;" Then str(10) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(11).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(12).Text <> "&nbsp;" Then str(11) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(12).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(13).Text <> "&nbsp;" Then str(12) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(13).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(14).Text <> "&nbsp;" Then str(13) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(14).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(15).Text <> "&nbsp;" Then str(14) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(15).Text
                    If Me.dgMaintenanceInvoiceList.Rows(I).Cells(16).Text <> "&nbsp;" Then str(15) = Me.dgMaintenanceInvoiceList.Rows(I).Cells(16).Text
                    ReportDetails.Add(New rptStatus(, 1, , str(0), str(1), str(2), str(3), str(4), _
                            str(5), str(6), str(7), str(8), str(9), str(10), str(11), str(12), str(13), str(14), str(15)))
                End If
            Next
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Maintenance Invoice List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mMaintenanceInvoiceList.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Me.dgMaintenanceInvoiceList.PageIndex = mCurrentPageindex
        Me.dgMaintenanceInvoiceList.DataSource = mMaintenanceInvoiceList
        Session("mMaintenanceInvoiceList") = mMaintenanceInvoiceList
        dgMaintenanceInvoiceList.DataBind()
    End Sub
#End Region

#End Region

    
End Class