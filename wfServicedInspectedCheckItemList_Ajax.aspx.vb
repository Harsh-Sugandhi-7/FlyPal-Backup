Public Class wfServicedInspectedCheckItemList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mItem As Item
    Protected mConditionCheckItem As ConditionCheckItem
    Protected mConditionCheckItemChildList As ConditionCheckItemChildList
    Protected mConditionCheckItemList As ConditionCheckItemList
    Protected mConditionCheckItemChild As ConditionCheckItemChild
    Dim EventLogID As Guid
    Dim SearchIndex, DateIndex, FromDate, ToDate, ItemName, Description, SerialNo As String
    Dim mFileAttach As FileAttach
    Private SearchStr1 As String
    Private SearchStr2 As String
    Dim mCompanyDetail As New CompanyDetail
    Dim mListOfItemServiceInspections As ServiceInspectionNameList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchIndex = Session("SearchIndex")
        ItemName = Session("ItemName")
        Description = Session("Description")
        SerialNo = Session("SerialNo")
        mConditionCheckItemChildList = Session("mConditionCheckItemChildList")
    End Sub
    Private Sub SetSession()
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mConditionCheckItemChildList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfServicedInspectedCheckItemList_Ajax.aspx" Then
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
            Session.Remove("mConditionCheckItemChildList")
            Session.Remove("ItemName")
            Session.Remove("Description")
            Session.Remove("SerialNo")
        End If
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid, ByVal mCalibrationItemID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mConditionCheckItem = ConditionCheckItem.GetConditionCheckItem(mCalibrationItemID)
        mConditionCheckItemChild = ConditionCheckItemChild.GetConditionCheckItemChild(mId)
        Session("mConditionCheckItem") = mConditionCheckItem
        Session("mConditionCheckItemChild") = mConditionCheckItemChild
    End Sub
    Private Sub SetControl()
        CallFindNow(SearchIndex)
        dgGridView.DataBind()
        txtItemName.Text = ItemName
        txtSerialNo.Text = SerialNo
        txtDescription.Text = Description
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Equipment Maintenance Item as per criteria :" & mConditionCheckItemChildList.Count & " Record(s) found."
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
                            mConditionCheckItemChild = CType(Session("mConditionCheckItemChild"), ConditionCheckItemChild)

                            If mConditionCheckItemChild.IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mConditionCheckItemChild.ID)
                            End If
                            If mConditionCheckItemChild.PreviousConditionCheckItemChildID.Equals(Guid.Empty) Then
                                mConditionCheckItem = CType(Session("mConditionCheckItem"), ConditionCheckItem)
                                Dim mtmpItemName As String = mConditionCheckItem.ItemName
                                Dim mtmpSerialNo As String = mConditionCheckItem.SerialNo
                                Dim mtmpId As Guid = mConditionCheckItem.ID

                                ConditionCheckItem.DeleteConditionCheckItem(mConditionCheckItem.ID)
                                If Not mFileAttach Is Nothing Then
                                    If mFileAttach.Size > 0 Then
                                        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                    End If
                                End If
                                mConditionCheckItem.Save()
                            End If
                            ConditionCheckItemChild.DeleteConditionCheckItemChild(mConditionCheckItemChild.ID)
                            mConditionCheckItemChild.Save()
                            DataFieldBind()
                            SetControl()
                            upnlGridView.Update()
                            upnTopButtons.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally
                            Dim mConditionCheckItemDetail As String = mConditionCheckItemChild.ConditionCheckNo + " Done On Date : " + mConditionCheckItemChild.DoneOnDateFormatted + " of " + "Part No. " + mConditionCheckItemChild.ItemName + " Serial No. " + mConditionCheckItemChild.SerialNo
                            MarkLog(Util.Action.Delete, "ConditionCheck", mConditionCheckItemDetail, Util.ErrorType.NoError, mConditionCheckItemChild.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DataFieldBind()
                        SetControl()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetControl()
            End Select
        End If
    End Sub
    Private Sub FindNow(Optional ByVal Fromdate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal ItemName As String = "", _
                        Optional ByVal Description As String = "", Optional ByVal SerialNo As String = "", _
                        Optional IsConditionCheckServicedInspected As Integer = 0, Optional ByVal ItemServiceInspectionsID As String = "{00000000-0000-0000-0000-000000000000}", _
                        Optional ServiceInspectionsID As String = "{00000000-0000-0000-0000-000000000000}")
        mConditionCheckItemChildList = Nothing
        dgGridView.DataSource = Nothing
        'Get List From the Database as per Criteria             
        mConditionCheckItemChildList = ConditionCheckItemChildList.GetConditionCheckItemChildList("1/1/1900", "1/1/2200", ItemName, Description, SerialNo, _
                                                                                                  IsConditionCheckServicedInspected:=IsConditionCheckServicedInspected, _
                                                                                                  ItemServiceInspectionsID:=Guid.Empty.ToString, _
                                                                                                  ServiceInspectionsID:=ServiceInspectionsID)
        'Set DataSource of the Grid
        Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
        dgGridView.DataSource = mConditionCheckItemChildList
        btnBottomPrint.Enabled = IIf(mConditionCheckItemChildList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mConditionCheckItemChildList.Count = 0, False, True)
        upnTopButtons.Update()
        upnBottomButtons.Update()
    End Sub
    Private Sub CallFindNow(ByVal Index As Integer)
        FindNow("1/1/1900", "1/1/2200", txtItemName.Text, txtDescription.Text, txtSerialNo.Text, IsConditionCheckServicedInspected:=2, _
                ServiceInspectionsID:=cmbListOfItemServiceInspections.SelectedValue.ToString)
        dgGridView.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ClearControls()
        txtItemName.Text = ""
        txtSerialNo.Text = ""
        txtDescription.Text = ""
    End Sub
    Private Sub addAttributes()
    End Sub
    Private Sub SetTitle()

    End Sub
    Private Function IsInRole() As Boolean
    End Function
#End Region

#Region " DataFieldBind "
    Private Sub DataFieldBind()
        mListOfItemServiceInspections = ServiceInspectionNameList.GetServiceInspectionList(AddTopItem:="ALL")
        cmbListOfItemServiceInspections.DataSource = mListOfItemServiceInspections
        cmbListOfItemServiceInspections.DataBind()
    End Sub
    Public Sub GridBind()
        dgGridView.DataBind()
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            If txtItemName.Enabled = True Then
                setFocus(txtItemName)
            End If
            'Session("MiddleFrame") = "wfConditionCheckItemList_Ajax.aspx"
            Session("MiddleFrame") = "wfServicedInspectedCheckItemList_Ajax.aspx"
            DataFieldBind()
            SetControl()
        End If
        SetTitle()
    End Sub
    Private Sub dgGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgGridView.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mTempConditionCheckItemID As Guid = mConditionCheckItemChildList(mID).ConditionCheckItemID

                Dim mOldConditionCheckItemChild As ConditionCheckItemChild
                Dim mConditionCheckItem As ConditionCheckItem
                Dim mConditionCheckItemChild As ConditionCheckItemChild

                mConditionCheckItem = ConditionCheckItem.GetConditionCheckItem(mTempConditionCheckItemID)
                mOldConditionCheckItemChild = ConditionCheckItemChild.GetConditionCheckItemChild(mID)
                mConditionCheckItemChild = ConditionCheckItemChild.NewComplyConditionCheckItemChild(mConditionCheckItem.ID, New SmartDate(Today.Date, False), mOldConditionCheckItemChild.ID)
                Session("mConditionCheckItem") = mConditionCheckItem
                If mOldConditionCheckItemChild.IsApplicable = True Then
                    mConditionCheckItemChild.ItemName = mOldConditionCheckItemChild.ItemName
                    mConditionCheckItemChild.Description = mOldConditionCheckItemChild.Description
                    mConditionCheckItemChild.SerialNo = mOldConditionCheckItemChild.SerialNo
                    mConditionCheckItemChild.Frequency = mOldConditionCheckItemChild.Frequency
                    mConditionCheckItemChild.ConditionCheckIntervalIn = mOldConditionCheckItemChild.ConditionCheckIntervalIn
                    mConditionCheckItemChild.DoneOnDate = Today.Date.ToShortDateString                             'Added  by Prashant 20-Apr-2012     For CPM No.: -ALL20042012 
                    mConditionCheckItemChild.Location = mOldConditionCheckItemChild.Location
                    mConditionCheckItemChild.IsConditionCheck = mOldConditionCheckItemChild.IsConditionCheck
                    mConditionCheckItemChild.IsServicedInspected = mOldConditionCheckItemChild.IsServicedInspected

                    mConditionCheckItemChild.ConditionCheckServicedInspected = mOldConditionCheckItemChild.ConditionCheckServicedInspected
                    mConditionCheckItemChild.ItemServiceInspectionsID = mOldConditionCheckItemChild.ItemServiceInspectionsID
                    mConditionCheckItemChild.ItemServiceInspectionsDescription = mOldConditionCheckItemChild.ItemServiceInspectionsDescription

                    If mConditionCheckItemChild.ConditionCheckIntervalIn = 1 Then
                        mConditionCheckItemChild.NextDueDate = Today.Date.AddDays(mOldConditionCheckItemChild.Frequency) '----------------------------------
                    ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 2 Then
                        mConditionCheckItemChild.NextDueDate = Today.Date.AddMonths(mOldConditionCheckItemChild.Frequency) '----------------------------------
                    ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 3 Then
                        mConditionCheckItemChild.NextDueDate = Today.Date.AddYears(mOldConditionCheckItemChild.Frequency) '----------------------------------
                    End If
                    Session("mConditionCheckItemChild") = mConditionCheckItemChild

                    Dim mCalibrationDetail As String = mOldConditionCheckItemChild.ConditionCheckNo + " Done On Date : " + mOldConditionCheckItemChild.DoneOnDateFormatted + " of " + "Part No. " + mConditionCheckItem.ItemName + " Serial No. " + mConditionCheckItem.SerialNo
                    MarkLog(Util.Action.Comply, "ConditionCheck", mCalibrationDetail, Util.ErrorType.NoError, mTempConditionCheckItemID, EventLogID)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenConditionCheckItemComplyWindow", "OpenConditionCheckItemComplyWindow();", True)
                Else
                    MSGBoxCtrl.show("Comply Alert!", "<strong>You are trying to comply the record.</strong>", "You can not comply this record as it is marked as Not Applicable.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            Case "DeleteRecord"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mTempConditionCheckItemID As Guid = mConditionCheckItemChildList(mID).ConditionCheckItemID
                DeleteRecord(mID, mTempConditionCheckItemID)

            Case "ViewRec"
                Dim mID As Guid
                mID = New Guid(e.CommandArgument.ToString)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mFileAttach = FileAttach.GetAttachment(mID)

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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
                'End
            Case "History"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mTempConditionCheckItemID As Guid = mConditionCheckItemChildList(mID).ConditionCheckItemID
                Dim mConditionCheckItemHistoryList As ConditionCheckItemHistoryList
                mConditionCheckItemHistoryList = ConditionCheckItemHistoryList.GetConditionCheckItemHistoryList(mTempConditionCheckItemID)
                Session("mConditionCheckItemHistoryList") = mConditionCheckItemHistoryList
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenConditionCheckItemHistoryWindow", "OpenConditionCheckItemHistoryWindow();", True)
        End Select
    End Sub
    Private Sub txtItemName_TextChanged(sender As Object, e As System.EventArgs) Handles txtItemName.TextChanged, txtDescription.TextChanged, txtSerialNo.TextChanged, cmbListOfItemServiceInspections.SelectedIndexChanged
        CallFindNow(0)
        dgGridView.DataBind()
        btnPrintTop.Enabled = IIf(mConditionCheckItemChildList.Count = 0, False, True)
        btnBottomPrint.Enabled = IIf(mConditionCheckItemChildList.Count = 0, False, True)
        lblResult.Text = "List of Equipment Maintenance Items as per criteria:" & mConditionCheckItemChildList.Count & " Record(s) found."
        upnlGridView.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomAddNew.Click, btnAddNewTop.Click
        Session("mConditionCheckItemChild") = Nothing
        Session("mConditionCheckItem") = Nothing
        Session("mConditionCheckItemChildList") = mConditionCheckItemChildList

        MarkLog(Util.Action.[New], "ConditionCheck", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenConditionCheckItemWindow", "OpenConditionCheckItemWindow();", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomClose.Click, btnCloseTop.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgGridView.PageIndexChanging
        dgGridView.PageIndex = e.NewPageIndex
        dgGridView.DataSource = mConditionCheckItemChildList
        Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
        GridBind()
    End Sub
    Private Sub dgGridView_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgGridView.Sorting
        mConditionCheckItemChildList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
        dgGridView.DataSource = mConditionCheckItemChildList
        GridBind()
    End Sub
    Private Sub hdnBtnCompMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnConditionCheckItem.Click, hdnBtnConditionCheckItemComply.Click
        mConditionCheckItemChildList = ConditionCheckItemChildList.GetConditionCheckItemChildList("1/1/1900", "1/1/2200", txtItemName.Text, _
                                                                                                  txtDescription.Text, txtSerialNo.Text, _
                                                                                                  IsConditionCheckServicedInspected:=2, _
                                                                                                  ItemServiceInspectionsID:=cmbListOfItemServiceInspections.SelectedValue.ToString)
        Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
        dgGridView.DataSource = mConditionCheckItemChildList
        dgGridView.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnBottomPrint.Click
        Dim Rpt As New crPendingConditionList
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim ds As New dsCommon
        Dim ds As New dsConditionCheckItemList


        If txtItemName.Text <> "" Then
            'ItemName 
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = IIf(txtItemName.Text.Trim <> "", "" + "ItemName" + " " + ":" + " " + txtItemName.Text.Trim, "")
        ElseIf txtDescription.Text <> "" Then
            'Description 
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = IIf(txtDescription.Text.Trim <> "", " " + "Description" + " " + ":" + " " + txtDescription.Text.Trim, "")
        ElseIf txtSerialNo.Text <> "" Then
            'Serial No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = IIf(txtSerialNo.Text.Trim <> "", " " + "Serial No." + " " + ":" + " " + txtSerialNo.Text.Trim, "")
        End If

        mConditionCheckItemChildList = Session("mConditionCheckItemChildList")
        'dgCalibrationItemList.DataSource = mConditionCheckItemChildList
        'dgCalibrationItemList.DataBind()
        If mConditionCheckItemChildList.Count = 0 Then
            MSGBoxCtrl.show(" Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly, "NoRecord")
            Exit Sub
        End If

        Dim ReportData As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Equipment Maintenance Item List Report", IIf(SearchStr2 <> "", SearchStr1, "").ToString, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, "ConditionCheckItemChildList", mConditionCheckItemChildList)
        da.Fill(ds, ReportData)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

End Class