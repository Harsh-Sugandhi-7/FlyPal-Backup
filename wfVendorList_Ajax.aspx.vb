'Added by Yogita

Partial Class wfVendorList_Ajax
    Inherits Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents CheckBox1 As CheckBox


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(sender As System.Object, e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "

    Public mVendorList As VendorList
    Public mVendor As Vendor
    Public BackPage As String
    Public Type As Int16 = 0
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, EnquiryText, Name, No As String
    Dim EventLogID As Guid
    Public mName As String

#End Region

#Region " Helper Methods "

    Private Sub GetSession()
        Type = Val(Request.QueryString("Type"))
        mVendorList = Session("mVendorList")
        mVendor = Session("mVendor")
        Session("mCityList") = Nothing
        Session("mCity") = Nothing
        SearchIndex = Session("SearchIndex")
        Name = Session("Name")
    End Sub

    Private Sub SetSession()
        Session("mVendorList") = mVendorList
        Session("mVendor") = mVendor
        Session("Type") = Type
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mVendorList")
        Session.Remove("Type")
    End Sub

    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfVendorList_Ajax.aspx?Type=" & Request.QueryString("Type") And Val(Request.QueryString("Type")) = 1 Then
            Session.Remove("mVendorList")
            Session.Remove("mVendor")
            Session.Remove("mCitylist")
            Session.Remove("mCity")
            Session.Remove("SearchIndex")
            Session.Remove("Name")
        End If
    End Sub

    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub

    Private Sub BindControls()
        dgVendor1.DataSource = mVendorList
        dgVendor1.DataBind()
    End Sub

    Private Sub NewRecord()
        mVendor = Vendor.NewVendor
        Session("mVendor") = mVendor
    End Sub

    Private Sub EditRecord(mID As Guid)
        mVendor = Vendor.GetVendor(mID)
        Session("mVendor") = mVendor
    End Sub

    Private Sub DeleteRecord(mID As Guid)

        MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                        MSGBox.Message_text.Delete,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

        mVendor = Vendor.GetVendor(mID)
        Session("mVendor") = mVendor

    End Sub

    Private Sub ControlVisibility(Index As Int32)
        lblFor.Visible = IIf(Index <> 0, True, False)
        'txtSearch.Visible = IIf(Index <> 0, True, False)
    End Sub

    Private Sub FindNow(lookin As Integer,
                        Optional Name As String = "",
                        Optional City As String = "",
                        Optional State As String = "",
                        Optional Country As String = "",
                        Optional ContactPerson As String = "")

        'Get List From the Database as per Criteria  
        mVendorList = VendorList.GetVendortList(LookInType:=1, Name:=Trim(txtSearch.Text))

        'Set DataSource of the Grid
        dgVendor1.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        dgVendor1.DataBind()
        lblResult.Text = "As per criteria:" & mVendorList.Count & " Record(s) found."
        upnlGridView.Update()

    End Sub

    Private Sub MessageBoxResult()

        Dim Result As MsgBoxResult
        Dim msgCount As Integer = 0
        Result = MSGBoxCtrl.Result

        If Result > 0 Then

            Select Case Result
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Session("sender") = ""
                            mVendor = Session("mVendor")
                            Vendor.DeleteVendor(mVendor.ID)
                            SetControl()

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                MarkLog(Action.Delete,
                                        "Vendor", "Can't delete : " & mVendor.Name & " is Currently in use",
                                        ErrorType.NoError,
                                        mVendor.ID,
                                        EventLogID)

                                Dim stringInfo As String = ""

                                If ex.Message.Contains("tabCalloutCustomer") Then
                                    stringInfo = "Callout."
                                ElseIf ex.Message.Contains("tabEnqSupplier") Then
                                    stringInfo = "Enquiry."
                                ElseIf ex.Message.Contains("tabExportInvoice") Then
                                    stringInfo = "Export Invoice."
                                ElseIf ex.Message.Contains("tabInvoice") Then
                                    stringInfo = "Invoice."
                                ElseIf ex.Message.Contains("tabLineMaintInvoice") Then
                                    stringInfo = "Line Maint. Invoice."
                                ElseIf ex.Message.Contains("tabLineMaintOrder") Then
                                    stringInfo = "Line Maint. Order."
                                ElseIf ex.Message.Contains("tabMachinetabVendor") Then
                                    stringInfo = "Aircraft."
                                ElseIf ex.Message.Contains("tabMaintenanceInvoice") Then
                                    stringInfo = "Maintenance Invoice."
                                ElseIf ex.Message.Contains("tabnWO") Then
                                    stringInfo = "Work Order."
                                ElseIf ex.Message.Contains("tabOrder") Then
                                    stringInfo = "Order."
                                ElseIf ex.Message.Contains("tabOtherChargeDetails") Then
                                    stringInfo = "Other Charge."
                                ElseIf ex.Message.Contains("tabPayment") Then
                                    stringInfo = "Payment."
                                ElseIf ex.Message.Contains("tabProject") Then
                                    stringInfo = "Project."
                                ElseIf ex.Message.Contains("tabReceipt") Then
                                    stringInfo = "Receipt."
                                ElseIf ex.Message.Contains("tabIssue") Then
                                    stringInfo = "Issue."
                                ElseIf ex.Message.Contains("tabSalesInvoice") Then
                                    stringInfo = "Sales Invoice."
                                ElseIf ex.Message.Contains("tabSalesOrder") Then
                                    stringInfo = "Sales Order."
                                ElseIf ex.Message.Contains("FKtabStoretabVendor") Then
                                    stringInfo = "Store."
                                ElseIf ex.Message.Contains("tabVendorApproval") Then
                                    stringInfo = "Vendor Approval."
                                ElseIf ex.Message.Contains("FKtabItemtabVendor") Then
                                    stringInfo = "Item Master."
                                ElseIf ex.Message.Contains("tabCustomerContract") Then
                                    stringInfo = "Customer Contract."
                                ElseIf ex.Message.Contains("tabProject") Then
                                    stringInfo = "Project."
                                ElseIf ex.Message.Contains("tabnWO") Then
                                    stringInfo = "Work Order."
                                End If

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting,
                                                MSGBox.Message_text.ReferenceDeleting,
                                                stringInfo,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            BindControls()
                            msgCount = ex.Errors.Count

                        Finally

                            If msgCount = 0 Then

                                MarkLog(Action.Delete,
                                        "Vendor",
                                        mVendor.Name,
                                        ErrorType.NoError,
                                        mVendor.ID,
                                        EventLogID)

                            End If

                        End Try

                    End If

                Case MsgBoxResult.No
                    Session("sender") = ""
                    BindControls()
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    BindControls()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    BindControls()
            End Select

        ElseIf Result = -1 Then

            Session("sender") = ""
            DataFieldBind()

        ElseIf Result = 0 And Session("sender") = "Authorization" Then   'Code Added

            Session("sender") = ""
            DataFieldBind()

        End If

    End Sub

    Private Sub SetControl()
        Name = Session("Name")
        SearchIndex = Session("SearchIndex")
        txtSearch.Text = Name
        cmbLookIn.SelectedIndex = SearchIndex
        FindNow(SearchIndex, txtSearch.Text, txtSearch.Text, txtSearch.Text, txtSearch.Text, txtSearch.Text)
        ControlVisibility(SearchIndex)
        dgVendor1.DataBind()
    End Sub

    Private Sub ControlVisibility()
        dgVendor1.Columns(3).Visible = IIf(AppSettings("IsGSTApplicable") = "True", True, False) 'GSTIN
        If AppSettings("ClientCode") = "7AR" Then
            dgVendor1.Columns(2).HeaderText = "Cage Code"
            dgVendor1.Columns(2).Visible = False
        Else
            dgVendor1.Columns(4).Visible = False 'Vendors ID, ID coloumn
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        dgVendor1.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgVendor1.DataSource = mVendorList
        dgVendor1.DataBind()
        ControlVisibility(0)
        'SetVariables()
        SetControl()
        upnlGridView.Update()
    End Sub

#End Region

#Region " DataBinding "

    Public Sub DataFieldBind()
        'mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", , , )
        'dgVendor1.DataSource = mVendorList
        SearchIndex = IIf(IsNothing(SearchIndex), 0, SearchIndex)
        Name = Session("Name")
        Session("Name") = Name
        Session("SearchIndex") = SearchIndex
        Session("mVendorList") = mVendorList
        'dgVendor1.DataBind()
    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If cmbLookIn.Enabled = True Then
                setFocus(cmbLookIn)
            End If
            If Type = 1 Then
                Session("MiddleFrame") = "wfVendorList_Ajax.aspx?Type=" & Request.QueryString("Type")
            End If
            DataFieldBind()
            SetControl()
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Vendor") Then
                ScriptManager.RegisterStartupScript(Me, [GetType], "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, [GetType], "RemoveFav", "RemoveFav();", True)
            End If
        End If
        'lblResult.Text = "List of Vendor as per criteria :" & mVendorList.Count & " Record(s) found."
        ControlVisibility()
    End Sub

    Private Sub btnFindNow_Click(sender As System.Object, e As System.EventArgs) Handles btnFindNow.Click, txtSearch.TextChanged
        SearchIndex = IIf(cmbLookIn.SelectedIndex < 0, 0, cmbLookIn.SelectedIndex)
        Name = txtSearch.Text.Trim
        Session("SearchIndex") = SearchIndex
        Session("Name") = Name
        dgVendor1.PageIndex = 0
        FindNow(cmbLookIn.SelectedIndex, Trim(txtSearch.Text), Trim(txtSearch.Text), Trim(txtSearch.Text), Trim(txtSearch.Text), Trim(txtSearch.Text))
        dgVendor1.DataBind()
    End Sub

    Private Sub dgVendor1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgVendor1.RowCommand
        Dim Idx As Int32
        Dim mID As New Guid
        Select Case e.CommandName
            Case "EditRec"
                mID = New Guid(e.CommandArgument.ToString)
                mName = mVendorList(Idx).Name

                If (Not User.IsInRole("VendorView") And Not User.IsInRole("VendorEdit")) Then
                    SetSession()
                    MarkLog(Action.Edit, "Vendor", User.Identity.Name & " is not Authorized User to edit " & mName, ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecord(mID)
                MarkLog(Action.Edit, "Vendor", mVendor.Name, ErrorType.NoError, mVendor.ID, EventLogID)
                If Type = 1 Then
                    Dim str As String
                    str = "openledgersame('wfVendor_Ajax.aspx?BackPage2=index.aspx" & "&Type=" & Request.QueryString("Type") & "');"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
                Else
                    Response.Redirect("wfVendor_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Request.QueryString("Type") & "&BackPage2=wfVendorList_Ajax.aspx")
                End If
            Case "DeleteRec"
                mID = New Guid(e.CommandArgument.ToString)
                mName = mVendorList(Idx).Name

                If (Not User.IsInRole("VendorDelete")) Then
                    'setObject()
                    SetSession()
                    MarkLog(Action.Delete, "Vendor", User.Identity.Name & " is not Authorized User to delete " & mName, ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                If Request.QueryString("BackPage1") = "wfCommonVendorList_Ajax.aspx" Then 'Open From Enquiry Supplier
                    Dim mEnquiry As Enquiry
                    mEnquiry = Session("mEnquiry")
                    If mEnquiry.EnquirySuppliers.Contains(mID) Then
                        MSGBoxCtrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.DeleteAlert, "Vendor is already added as Supplier<BR><BR><b>Can not be deleted</b>", MsgBoxStyle.OkOnly, "")
                        Exit Sub
                    End If
                End If
                DeleteRecord(mID)
        End Select
        dgVendor1.DataSource = mVendorList
        dgVendor1.DataBind()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAddTop.Click
        NewRecord()
        'AJAX
        dgVendor1.DataSource = mVendorList
        dgVendor1.DataBind()
        'End
        If (Not User.IsInRole("VendorNew") And mVendor.IsNew) Or (Not User.IsInRole("VendorEdit") And Not mVendor.IsNew) Then
            SetSession()
            MarkLog(Action.Save, "Vendor", User.Identity.Name & " is not Authorized User to add " & mVendor.Name, ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        MarkLog(Action.[New], "Vendor", "", ErrorType.NoError, mVendor.ID, EventLogID)
        If Type = 1 Then
            Dim str As String
            str = "openledgersame('wfVendor_Ajax.aspx?BackPage2=index.aspx" & "&Type=" & Request.QueryString("Type") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        Else
            Response.Redirect("wfVendor_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&Type=" & Request.QueryString("Type") & "&BackPage2=wfVendorList_Ajax.aspx")
        End If
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        If Type = 1 Then
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Else
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type"))
        End If
    End Sub

    Private Sub cmblookin_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbLookIn.SelectedIndexChanged
        Dim Index As Int32 = cmbLookIn.SelectedIndex
        txtSearch.Text = ""
        lblFor.Visible = IIf(Index <> 0, True, False)
        'txtSearch.Visible = IIf(Index <> 0, True, False)
        If cmbLookIn.Enabled = True Then
            setFocus(cmbLookIn)
        End If
    End Sub

    Private Sub dgVendor1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgVendor1.PageIndexChanging
        dgVendor1.PageIndex = e.NewPageIndex
        dgVendor1.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        dgVendor1.DataBind()
    End Sub

    Private Sub dgVendor1_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgVendor1.Sorting
        mVendorList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mVendorList") = mVendorList
        dgVendor1.DataSource = mVendorList
        dgVendor1.DataBind()
    End Sub

    Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub hdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFav.Click
        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "Vendor")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

    Private Sub hdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFav.Click
        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "Vendor")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub

#End Region

End Class
