Public Class wfVendorApprovalList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mVendor As Vendor
    Public BackPage As String
    Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, EnquiryText, Name, No As String
    Dim EventLogID As Guid
    Public mName As String
    Public mVendorListForApprovalList As VendorList
    Public mVendorApprovals As VendorApprovals
    Public mVendorApproval As VendorApproval
    Public mRenewVendorApproval As VendorApproval
    Dim mFileAttach As FileAttach
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorApprovals = Session("mVendorApprovals")
        mVendorListForApprovalList = Session("mVendorListForApprovalList")
        Session("mCityList") = Nothing
        Session("mCity") = Nothing
        SearchIndex = Session("SearchIndex")
        Name = Session("Name")
    End Sub
    Private Sub SetSession()
        Session("mVendorApprovals") = mVendorApprovals
      End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorApprovals")
        Session.Remove("VendorName")
     End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfVendorApprovalList_Ajax.aspx?" Then
            Session.Remove("mVendorApprovals")
            Session.Remove("mCitylist")
            Session.Remove("mCity")
            Session.Remove("SearchIndex")
            Session.Remove("Name")
            Session.Remove("VendorName")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    'Private Sub BindControls()
    '    'dgApprovalList.DataSource = mVendorApprovals
    '    'dgApprovalList.DataBind()
    '    mVendorApprovals = VendorApprovals.GetVendorApprovalList(Guid.Empty, IsFromOtherLink:=1)
    '    dgApprovalList.DataSource = mVendorApprovals
    '    dgApprovalList.DataBind()
    '    lblResult.Text = "As per criteria :" & mVendorApprovals.Count & " Record(s) found."
    '    upnlGridView.Update()
    'End Sub
    Private Sub DeleteRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteVendorApproval")
        mVendorApproval = VendorApproval.GetVendorApproval(mID)
        Session("mVendorApproval") = mVendorApproval
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int32)
        If AppSettings("ClientCode") = "7AR" Then
            dgApprovalList.Columns(2).HeaderText = "Cage Code"
            dgApprovalList.Columns(2).Visible = False
        Else
            dgApprovalList.Columns(3).Visible = False 'Vendors ID, ID coloumn
        End If
    End Sub
    Private Sub FindNow(Optional ByVal VendorName As String = "")
        'Get List From the Database as per Criteria  
        mVendorApprovals = VendorApprovals.GetVendorApprovalList(Guid.Empty, IsFromOtherLink:=1, VendorName:=VendorName)
        'Set DataSource of the Grid
        dgApprovalList.DataSource = mVendorApprovals
        Session("mVendorApprovals") = mVendorApprovals
        ControlVisibility(SearchIndex)
        dgApprovalList.DataBind()
        lblResult.Text = "As per criteria :" & mVendorApprovals.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteVendorApproval" Then
                        Try
                            mVendorApproval = Session("mVendorApproval")
                            If mVendorApproval.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mVendorApproval.ID)
                            End If
                            VendorApproval.DeleteVendorApproval(mVendorApproval.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            'BindControls()
                            FindNow()
                        Catch ex As SqlException
                        Finally
                            MarkLog(Util.Action.Delete, "VendorApproval", "Approval No. : " & mVendorApproval.ApprovalNo & " Name : " & mVendorApproval.Name, Util.ErrorType.NoError, mVendorApproval.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'BindControls()
                    FindNow()
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'BindControls()
                    FindNow()
                Case MsgBoxResult.Ok And (Session("sender") = "Authorization" Or Session("sender") = "VendorApproval")  'Code Added
                    'BindControls()
                    FindNow()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'DataFieldBind()
            FindNow()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
            FindNow()
        End If
    End Sub
    Private Sub SetControl()
        Name = Session("Name")
        SearchIndex = Session("SearchIndex")
        txtSearch.Text = Name
        'cmbLookIn.SelectedIndex = SearchIndex
        FindNow(txtSearch.Text)
        ControlVisibility(SearchIndex)
        dgApprovalList.DataBind()
    End Sub
#End Region

#Region " DataBinding "
    Public Sub DataFieldBind()
        'mVendorApprovals = VendorApprovals.GetVendorApprovalList(Guid.Empty, IsFromOtherLink:=1)
        'dgApprovalList.DataSource = mVendorApprovals
        SearchIndex = IIf(IsNothing(SearchIndex), 0, SearchIndex)
        Name = Session("Name")
        Session("Name") = Name
        Session("SearchIndex") = SearchIndex
        'Session("mVendorApprovals") = mVendorApprovals
        'dgApprovalList.DataBind()
        mVendorListForApprovalList = VendorList.GetVendortList(0, , , , , , True, True, True, True)
        cmbVendorList.DataSource = mVendorListForApprovalList
        cmbVendorList.DataBind()
        Session("mVendorListForApprovalList") = mVendorListForApprovalList
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfVendorApprovalList_Ajax.aspx?"
            DataFieldBind()
            SetControl()
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "VendorApproval") Then
                ScriptManager.RegisterStartupScript(Me, [GetType], "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, [GetType], "RemoveFav", "RemoveFav();", True)
            End If
        End If
        End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        SearchIndex = 1 'IIf(cmbLookIn.SelectedIndex < 0, 0, cmbLookIn.SelectedIndex)
        Name = txtSearch.Text.Trim
        Session("SearchIndex") = SearchIndex
        Session("Name") = Name
        dgApprovalList.PageIndex = 0
        FindNow(Trim(txtSearch.Text))
    End Sub
    Private Sub dgApprovalList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgApprovalList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mVendorApproval = VendorApproval.GetVendorApproval(ID)
                Session("mVendorApproval") = mVendorApproval
                If mVendorApproval.IsAttachmentAdded = True Then
                    mFileAttach = FileAttach.GetAttachment(mVendorApproval.ID)
                Else
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mVendorApproval.ID)
                End If
                Session("mFileAttach") = mFileAttach
                Session("VendorName") = mVendorApprovals(ID).VendorName
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVendorApprovalWindow", "OpenVendorApprovalWindow()", True)
            Case "DeleteRec"
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("VendorApprovalDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(ID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mFileAttach = FileAttach.GetAttachment(ID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
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
                End If
            Case "RenewRec"
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mVendorApproval = VendorApproval.GetVendorApproval(ID)
                mRenewVendorApproval = VendorApproval.NewVendorApproval(Guid.NewGuid, mVendorApproval.VendorID, IsRenew:=True, _
                                                                        ApprovalNo:=mVendorApproval.ApprovalNo, Name:=mVendorApproval.Name, _
                                                                        FromDate:=mVendorApproval.FromDate.ToString, ToDate:=mVendorApproval.ToDate.ToString, _
                                                                        IsOneTime:=mVendorApproval.IsOneTime, IsApplicable:=mVendorApproval.IsApplicable, _
                                                                        SortNo:=mVendorApproval.SortNo + 1, ReferenceID:=mVendorApproval.ReferenceID.ToString, _
                                                                        Remark:=mVendorApproval.Remark)
                mVendorApproval.IsRenew = True
                Session("mVendorApproval") = mRenewVendorApproval
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mRenewVendorApproval.ID)
                Session("mFileAttach") = mFileAttach
                Session("VendorName") = mVendorApprovals(ID).VendorName
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVendorApprovalWindow", "OpenVendorApprovalWindow()", True)
            Case "HistoryRec"
                Dim index As Integer = CInt(e.CommandArgument)
                Dim ID As Guid = New Guid(Me.dgApprovalList.Rows.Item(index).Cells(0).Text)
                Dim VendorID As Guid = New Guid(dgApprovalList.DataKeys(index).Value.ToString)
                mVendorApprovals = VendorApprovals.GetVendorApprovalList(VendorID, HasHistory:=True, VendorApprovalID:=ID.ToString, IsFromOtherLink:=1)
                dgApprovalHistoryList.DataSource = mVendorApprovals
                dgApprovalHistoryList.DataBind()
                upnlApprovalHistory.Update()
                mdeApprovalHistory.Show()
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTop.Click
        If (Not User.IsInRole("VendorApprovalNew")) Or (Not User.IsInRole("VendorApprovalEdit")) Then
            SetSession()
            MarkLog(Util.Action.Save, "VendorApproval", User.Identity.Name & " is not Authorized User to add " & mVendor.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If cmbVendorList.SelectedIndex = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "To Add Document Approval Select Vendor From List ", MsgBoxStyle.OkOnly, "VendorApproval")
            Exit Sub
        End If
        mVendorApproval = VendorApproval.NewVendorApproval(Guid.NewGuid, New Guid(cmbVendorList.SelectedValue))
        Session("mVendorApproval") = mVendorApproval
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mVendorApproval.ID)
        Session("mFileAttach") = mFileAttach
        Session("VendorName") = cmbVendorList.SelectedItem.Text
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenVendorApprovalWindow", "OpenVendorApprovalWindow()", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgApprovalList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgApprovalList.PageIndexChanging
        dgApprovalList.PageIndex = e.NewPageIndex
        dgApprovalList.DataSource = mVendorApprovals
        Session("mVendorApprovals") = mVendorApprovals
        dgApprovalList.DataBind()
    End Sub
    Private Sub dgApprovalList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgApprovalList.Sorting
        mVendorApprovals.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mVendorApprovals") = mVendorApprovals
        dgApprovalList.DataSource = mVendorApprovals
        dgApprovalList.DataBind()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnApprovalHistoryClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprovalHistoryClose.Click
        mdeApprovalHistory.Hide()
    End Sub
    Private Sub hdnBtnVendorApproval_Click(sender As Object, e As System.EventArgs) Handles hdnBtnVendorApproval.Click
        mVendorApprovals = VendorApprovals.GetVendorApprovalList(Guid.Empty, IsFromOtherLink:=1, VendorName:=txtSearch.Text.Trim)
        dgApprovalList.DataSource = mVendorApprovals
        Session("mVendorApprovals") = mVendorApprovals
        dgApprovalList.DataBind()
        lblResult.Text = "As per criteria :" & mVendorApprovals.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub dgApprovalHistoryList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgApprovalHistoryList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim ID As Guid = New Guid(e.CommandArgument.ToString)
                mFileAttach = FileAttach.GetAttachment(ID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
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
                End If
        End Select
    End Sub
    Private Sub hdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFav.Click
        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "VendorApproval")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
    Private Sub hdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFav.Click
        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "VendorApproval")
        Catch ex As Exception
            ex.GetBaseException()
        End Try
    End Sub
#End Region

End Class