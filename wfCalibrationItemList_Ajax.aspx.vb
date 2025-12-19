
Public Class wfCalibrationItemList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mCalibrationItem As CalibrationItem
    Protected mCalibrationItemChildList As CalibrationItemChildList
    Protected mCalibrationItemChild As CalibrationItemChild
    Dim EventLogID As Guid
    Dim SearchIndex, DateIndex, FromDate, ToDate, ItemName, Description, SerialNo As String
    Dim mFileAttach As FileAttach
    Public mOrder As Order
    Dim mBaseCurrency As Currency

#End Region

#Region " Helper Methods "
    Private Sub GetCalibrationItemChildList()
        mCalibrationItemChildList = CalibrationItemChildList.GetCalibrationChildList(FromDate, ToDate, ItemName, Description, SerialNo)
        dgCalibrationItemList.DataSource = mCalibrationItemChildList
        dgCalibrationItemList.DataBind()
        Session("mCalibrationItemChildList") = mCalibrationItemChildList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCalibrationItemChildList")
    End Sub
    Private Sub DeleteConfirmation(Optional ByRef mCalibrationItemChild As CalibrationItemChild = Nothing, Optional ByVal Str As String = "")
        Try
            If mCalibrationItemChild.IsAttachmentAdded Then
                mFileAttach = FileAttach.GetAttachment(mCalibrationItemChild.ID)
            End If
            If mCalibrationItemChild.PreviousCalibrationItemChildID.Equals(Guid.Empty) Then
                mCalibrationItem = CType(Session("mCalibrationItem"), CalibrationItem)
                Dim mtmpItemName As String = mCalibrationItem.ItemName
                Dim mtmpSerialNo As String = mCalibrationItem.SerialNo
                Dim mtmpId As Guid = mCalibrationItem.ID

                CalibrationItem.DeleteCalibrationItem(mCalibrationItem.ID)
                mCalibrationItem.Save()
                'MarkLog(Util.Action.Delete, "Calibration Calibration", "Calibration" + "-> For Part No. " + mtmpItemName + "Serial No. " + mtmpSerialNo, Util.ErrorType.NoError, mtmpId)
            End If


            CalibrationItemChild.DeleteCalibrationItemChild(mCalibrationItemChild.ID, Str:=Str)
            mCalibrationItemChild.Save()
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                End If
            End If
            Dim mCalibrationDetail As String
            If Str = "Yes" Then
                mCalibrationDetail = mCalibrationItemChild.CalibrationNo + " Done On Date : " + mCalibrationItemChild.DoneOnDateFormatted + " of " + "Part No. " + mCalibrationItemChild.ItemName + " Serial No. " + mCalibrationItemChild.SerialNo + " Interval " + mCalibrationItemChild.Frequency.ToString + " Priod " + mCalibrationItemChild.CalibrationPeriodInID.ToString
            Else
                mCalibrationDetail = mCalibrationItemChild.CalibrationNo + " Done On Date : " + mCalibrationItemChild.DoneOnDateFormatted + " of " + "Part No. " + mCalibrationItemChild.ItemName + " Serial No. " + mCalibrationItemChild.SerialNo
            End If
            MarkLog(Util.Action.Delete, "Calibration", mCalibrationDetail, Util.ErrorType.NoError, mCalibrationItemChild.ID, EventLogID)
            SetControl()
            SetGrid()
            upnlActionBtn.Update()
            upnlGridView.Update()
        Catch ex As Exception

        End Try
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
                            Dim mCalibrationItemChild As CalibrationItemChild
                            Session("sender") = ""
                            mCalibrationItemChild = CType(Session("mCalibrationItemChild"), CalibrationItemChild)
                            '-------------------
                            Dim mCalibrationItemHistoryList As CalibrationItemHistoryList
                            mCalibrationItemHistoryList = CalibrationItemHistoryList.GetCalibrationItemHistoryList(mCalibrationItemChild.CalibrationItemID)
                            If mCalibrationItemHistoryList.Count > 0 Then
                                If mCalibrationItemChild.CalibrationItemChildFrequency <> mCalibrationItemHistoryList(0).CalibrationItemChildFrequency Then
                                    MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, "Calibration Interval has Revised!. If Yes will applicable to previous history record also. If No previous history record will be with old interval.", MsgBoxStyle.YesNo, "DeleteConfirmation")
                                    Exit Sub
                                Else
                                    DeleteConfirmation(mCalibrationItemChild, "")
                                End If
                            Else
                                DeleteConfirmation(mCalibrationItemChild, "")
                            End If
                            '-------------------
                        Catch ex As Exception
                            'Dim msg1 As New SIMsgBox(Page, "Reference!<Br><Br><Br>You have clicked on the Delete link to Delete this entry. You can not delete this.", "<Br>Because this entry is used by someone.", "", MsgBoxStyle.OKOnly)
                            'msg1.ReplacePage = "wfCalibrationItemList.aspx?MsgResult=0"
                            'msg1.Show()
                        Finally
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "DeleteConfirmation" Then
                        Try
                            Dim mCalibrationItemChild As CalibrationItemChild
                            Session("sender") = ""
                            mCalibrationItemChild = CType(Session("mCalibrationItemChild"), CalibrationItemChild)

                            DeleteConfirmation(mCalibrationItemChild, "Yes")

                            'If mCalibrationItemChild.IsAttachmentAdded Then
                            '    mFileAttach = FileAttach.GetAttachment(mCalibrationItemChild.ID)
                            'End If
                            'If mCalibrationItemChild.PreviousCalibrationItemChildID.Equals(Guid.Empty) Then
                            '    mCalibrationItem = CType(Session("mCalibrationItem"), CalibrationItem)
                            '    Dim mtmpItemName As String = mCalibrationItem.ItemName
                            '    Dim mtmpSerialNo As String = mCalibrationItem.SerialNo
                            '    Dim mtmpId As Guid = mCalibrationItem.ID

                            '    CalibrationItem.DeleteCalibrationItem(mCalibrationItem.ID)
                            '    mCalibrationItem.Save()
                            '    'MarkLog(Util.Action.Delete, "Calibration Calibration", "Calibration" + "-> For Part No. " + mtmpItemName + "Serial No. " + mtmpSerialNo, Util.ErrorType.NoError, mtmpId)
                            'End If


                            'CalibrationItemChild.DeleteCalibrationItemChild(mCalibrationItemChild.ID)
                            'mCalibrationItemChild.Save()
                            'If Not mFileAttach Is Nothing Then
                            '    If mFileAttach.Size > 0 Then
                            '        FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                            '    End If
                            'End If
                            'Dim mCalibrationDetail As String = mCalibrationItemChild.CalibrationNo + " Done On Date : " + mCalibrationItemChild.DoneOnDateFormatted + " of " + "Part No. " + mCalibrationItemChild.ItemName + " Serial No. " + mCalibrationItemChild.SerialNo
                            'MarkLog(Util.Action.Delete, "Calibration", mCalibrationDetail, Util.ErrorType.NoError, mCalibrationItemChild.ID, EventLogID)
                            'SetControl()
                            'SetGrid()
                            'upnlActionBtn.Update()
                            'upnlGridView.Update()
                        Catch ex As Exception
                        Finally
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteConfirmation" Then
                        Try
                            Dim mCalibrationItemChild As CalibrationItemChild
                            Session("sender") = ""
                            mCalibrationItemChild = CType(Session("mCalibrationItemChild"), CalibrationItemChild)
                            DeleteConfirmation(mCalibrationItemChild, "No")
                        Catch ex As Exception
                        Finally
                        End Try
                    End If
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
    Private Sub setVariables()
        SearchIndex = IIf(cmbSearchCriteria.SelectedIndex < 0, 0, cmbSearchCriteria.SelectedIndex)
        'DateIndex = IIf(cmbPeriod.SelectedIndex < 0, 0, cmbPeriod.SelectedIndex)
        FromDate = "1/1/1900" 'IIf(txtFromDate.Value.ToString <> "", txtFromDate.Value.ToString, "1/1/1900")
        ToDate = "1/1/2200"

        ItemName = txtItemName.Text.Trim
        Description = txtDescription.Text.Trim
        SerialNo = txtSerialNo.Text.Trim

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("ItemName") = ItemName
        Session("Description") = Description
        Session("SerialNo") = SerialNo


    End Sub
    Private Sub GetSession()
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchIndex = Session("SearchIndex")
        ItemName = Session("ItemName")
        Description = Session("Description")
        SerialNo = Session("SerialNo")
        mCalibrationItemChildList = Session("mCalibrationItemChildList")
    End Sub
    Private Sub FindNow(Optional ByVal Fromdate As String = "1/1/1900", _
    Optional ByVal ToDate As String = "1/1/2200", Optional ByVal ItemName As String = "", _
  Optional ByVal Description As String = "", Optional ByVal SerialNo As String = "")
        'clear the obj and grid
        mCalibrationItemChildList = Nothing
        dgCalibrationItemList.DataSource = Nothing
        'get the list
        mCalibrationItemChildList = CalibrationItemChildList.GetCalibrationChildList(Fromdate, ToDate, ItemName, Description, SerialNo)
        'set the session
        Session("mCalibrationItemChildList") = mCalibrationItemChildList
        'bind the list to the datagrid
        dgCalibrationItemList.DataSource = mCalibrationItemChildList

    End Sub
    Private Sub CallFindNow(ByVal indx As Int32)
        Select Case indx
            Case 0  'All
                FindNow()
                'Case 1  'Date
                '    'FindNow(txtFromDate.Value.ToString, txtFromDate.Value.ToString, "", "", "")
            Case 1  'ItemName
                FindNow(FromDate, ToDate, ItemName)
            Case 2  'Description
                FindNow(FromDate, ToDate, "", Description, "")
            Case 3  'Serial No.
                FindNow(FromDate, ToDate, "", "", SerialNo)
        End Select
        dgCalibrationItemList.PageIndex = 0
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal PeriodIndex As Int32 = 0, Optional ByVal RectTxt As Int32 = 0, Optional ByVal Ordtxt As Int32 = 0)
        txtItemName.Visible = CBool(IIf(SearchIndex = 1, True, False))
        txtDescription.Visible = CBool(IIf(SearchIndex = 2, True, False))
        txtSerialNo.Visible = CBool(IIf(SearchIndex = 3, True, False))
    End Sub
    Private Sub ClearControl()
        txtItemName.Text = ""
        txtSerialNo.Text = ""
        txtDescription.Text = ""
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfCalibrationItemList_Ajax.aspx" Then
            Session.Remove("SearchIndex")
            Session.Remove("DateIndex")
            Session.Remove("FromDate")
            Session.Remove("ToDate")
        End If
    End Sub
    Private Sub SetControl()
        CallFindNow(SearchIndex)
        dgCalibrationItemList.DataBind()
        cmbSearchCriteria.SelectedIndex = SearchIndex
        txtItemName.Text = ItemName
        txtSerialNo.Text = SerialNo
        txtDescription.Text = Description
        ControlVisibility(SearchIndex, DateIndex)
        lblResult.Text = "List of Calibration Item as per criteria :" & mCalibrationItemChildList.Count & " Record(s) found."
    End Sub
    Private Sub SetGrid()
        btnPrint.Enabled = IIf(dgCalibrationItemList.Rows.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(dgCalibrationItemList.Rows.Count = 0, False, True)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            If cmbSearchCriteria.Enabled = True Then
                cmbSearchCriteria.Focus()
            End If
            Session("MiddleFrame") = "wfCalibrationItemList_Ajax.aspx"
            SetControl()
            SetGrid()
        End If
    End Sub
    Private Sub dgCalibrationItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCalibrationItemList.RowCommand
        Dim Index As Integer
        Dim ID As Guid
        Dim CalibrationItemID As Guid
        Dim mOldCalibrationItemChild As CalibrationItemChild
        Dim mCalibrationItem As CalibrationItem
        Dim mCalibrationItemChild As CalibrationItemChild

        Select Case e.CommandName
            Case "ComplyRecord"
                Index = CInt(e.CommandArgument)
                ID = mCalibrationItemChildList(Index).ID
                CalibrationItemID = mCalibrationItemChildList(Index).CalibrationItemID

                mCalibrationItem = CalibrationItem.GetCalibrationItem(CalibrationItemID)
                mOldCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(ID)
                mCalibrationItemChild = CalibrationItemChild.NewComplyCalibrationItemChild(mCalibrationItem.ID, Today.Date.ToString(AppSettings("DateFormat")), mOldCalibrationItemChild.ID)
                'mCalibrationItemChild = CalibrationItemChild.NewComplyCalibrationItemChild(mCalibrationItem.ID, Today.Date.ToString, mOldCalibrationItemChild.ID, Frequency:=mOldCalibrationItemChild.Frequency, PeriodID:=mOldCalibrationItemChild.CalibrationPeriodInID)
                Session("mCalibrationItem") = mCalibrationItem
                If mOldCalibrationItemChild.IsApplicable = True Then
                    mCalibrationItemChild.ItemName = mOldCalibrationItemChild.ItemName
                    mCalibrationItemChild.Description = mOldCalibrationItemChild.Description
                    mCalibrationItemChild.SerialNo = mOldCalibrationItemChild.SerialNo
                    '--Commneted and Added by Prashant on 3-Sep-2021 for ALL03092021
                    'mCalibrationItemChild.Frequency = mOldCalibrationItemChild.Frequency
                    'mCalibrationItemChild.CalibrationPeriodInID = mOldCalibrationItemChild.CalibrationPeriodInID
                    mCalibrationItemChild.CalibrationItemChildFrequency = mOldCalibrationItemChild.CalibrationItemChildFrequency
                    mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = mOldCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID
                    '--End of Added by Prashant on 3-Sep-2021 for ALL03092021
                    'mCalibrationItemChild.DoneOnDate = Today.Date.ToShortDateString                             'Commented  by Prashant 29-Sep-2009 
                    'mCalibrationItemChild.NextDueDate = Today.Date.AddMonths(mOldCalibrationItemChild.Frequency)'----------------------------------
                    'mCalibrationItemChild.DoneOnDate = mOldCalibrationItemChild.NextDueDate                      'Added  by Prashant 29-Sep-2009 again commented by Prashant 20-Apr-2012
                    'mCalibrationItemChild.NextDueDate = CDate(mOldCalibrationItemChild.NextDueDate).AddMonths(mOldCalibrationItemChild.Frequency) '----
                    mCalibrationItemChild.DoneOnDate = Today.Date.ToShortDateString                             'Added  by Prashant 20-Apr-2012     For CPM No.: -ALL20042012 
                    mCalibrationItemChild.Location = mOldCalibrationItemChild.Location
                    '--Commneted and Added by Prashant on 3-Sep-2021 for ALL03092021
                    'If mCalibrationItemChild.CalibrationPeriodInID = 1 Then
                    '    mCalibrationItemChild.NextDueDate = Today.Date.AddDays(mOldCalibrationItemChild.Frequency) '----------------------------------
                    'ElseIf mCalibrationItemChild.CalibrationPeriodInID = 2 Then
                    '    mCalibrationItemChild.NextDueDate = Today.Date.AddMonths(mOldCalibrationItemChild.Frequency) '----------------------------------
                    'ElseIf mCalibrationItemChild.CalibrationPeriodInID = 3 Then
                    '    mCalibrationItemChild.NextDueDate = Today.Date.AddYears(mOldCalibrationItemChild.Frequency) '----------------------------------
                    'End If
                    If mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 1 Then
                        mCalibrationItemChild.NextDueDate = Today.Date.AddDays(mOldCalibrationItemChild.CalibrationItemChildFrequency) '----------------------------------
                    ElseIf mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 2 Then
                        mCalibrationItemChild.NextDueDate = Today.Date.AddMonths(mOldCalibrationItemChild.CalibrationItemChildFrequency) '----------------------------------
                    ElseIf mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = 3 Then
                        mCalibrationItemChild.NextDueDate = Today.Date.AddYears(mOldCalibrationItemChild.CalibrationItemChildFrequency) '----------------------------------
                    End If
                    '--End of Added by Prashant on 3-Sep-2021 for ALL03092021
                    Session("mCalibrationItemChild") = mCalibrationItemChild

                    Dim mCalibrationDetail As String = mOldCalibrationItemChild.CalibrationNo + " Done On Date : " + mOldCalibrationItemChild.DoneOnDateFormatted + " of " + "Part No. " + mCalibrationItem.ItemName + " Serial No. " + mCalibrationItem.SerialNo
                    MarkLog(Util.Action.Comply, "Calibration", mCalibrationDetail, Util.ErrorType.NoError, CalibrationItemID, EventLogID)
                    Session.Remove("mFileAttach")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCalibrationItemWindow", "OpenCalibrationItemWindow();", True)
                Else
                    MSGBoxCtrl.show("Comply Alert!", "<strong>You are trying to comply the record.</strong></br>You can not comply this record as it is marked as Not Applicable.", "", MsgBoxStyle.OkOnly, "Alert")
                    Exit Sub
                End If
            Case "EditRecord"
                Index = CInt(e.CommandArgument)
                ID = mCalibrationItemChildList(Index).ID
                CalibrationItemID = mCalibrationItemChildList(Index).CalibrationItemID

                mCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(ID)
                mCalibrationItem = CalibrationItem.GetCalibrationItem(CalibrationItemID)
                Session("mCalibrationItemChild") = mCalibrationItemChild

                Dim mCalibrationDetail As String = mCalibrationItemChild.CalibrationNo + " Done On Date : " + mCalibrationItemChild.DoneOnDateFormatted + " of " + "Part No. " + mCalibrationItem.ItemName + " Serial No. " + mCalibrationItem.SerialNo
                MarkLog(Util.Action.Edit, "Calibration", mCalibrationDetail, Util.ErrorType.NoError, CalibrationItemID, EventLogID)
                Session.Remove("mFileAttach")
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCalibrationItemWindow", "OpenCalibrationItemWindow();", True)
            Case "DeleteRecord"
                Index = CInt(e.CommandArgument)
                ID = mCalibrationItemChildList(Index).ID
                CalibrationItemID = mCalibrationItemChildList(Index).CalibrationItemID

                mCalibrationItem = CalibrationItem.GetCalibrationItem(CalibrationItemID)
                mCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(ID)
                Session("mCalibrationItem") = mCalibrationItem
                Session("mCalibrationItemChild") = mCalibrationItemChild

                Try
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                End Try
            Case "ViewRec"
                Index = CInt(e.CommandArgument)
                ID = mCalibrationItemChildList(Index).ID
                CalibrationItemID = mCalibrationItemChildList(Index).CalibrationItemID
                mCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(ID)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
            Case "History"
                Dim mCalibrationItemHistoryList As CalibrationItemHistoryList
                mCalibrationItemHistoryList = CalibrationItemHistoryList.GetCalibrationItemHistoryList(New Guid(e.CommandArgument.ToString))
                Session("mCalibrationItemHistoryList") = mCalibrationItemHistoryList
                Session.Remove("mFileAttach")
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenCalibrationHistoryWindow", "OpenCalibrationHistoryWindow();", True)
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        'Session("mCalibrationItemChildList") = Nothing
        Session("mCalibrationItemChild") = Nothing
        Session("mCalibrationItem") = Nothing
        Session("mCalibrationItemChildList") = mCalibrationItemChildList

        MarkLog(Util.Action.[New], "Calibration", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session.Remove("mFileAttach")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenNewCalibrationItemWindow", "OpenNewCalibrationItemWindow();", True)
    End Sub
    Private Sub dgCalibrationItemList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCalibrationItemList.PageIndexChanging
        dgCalibrationItemList.PageIndex = e.NewPageIndex
        Session("mCalibrationItemChildList") = mCalibrationItemChildList
        dgCalibrationItemList.DataSource = mCalibrationItemChildList
        dgCalibrationItemList.DataBind()
        SetGrid()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        'Session.Remove("FromDate")
        ClearAll()
        RemoveSession()
        Session("MiddleFrame") = ""
        mCalibrationItemChildList = Nothing
        mCalibrationItem = Nothing
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbSearchCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchCriteria.SelectedIndexChanged
        'txtFromDate.Value = Today.Date
        ClearControl()
        ControlVisibility(cmbSearchCriteria.SelectedIndex, DateIndex)
        If cmbSearchCriteria.Enabled = True Then
            cmbSearchCriteria.Focus()
        End If
    End Sub

    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        setVariables()
        CallFindNow(SearchIndex)
        dgCalibrationItemList.DataBind()
        SetGrid()
        btnPrint.Enabled = IIf(mCalibrationItemChildList.Count = 0, False, True)
        btnPrintTop.Enabled = IIf(mCalibrationItemChildList.Count = 0, False, True)
        'set result label
        lblResult.Text = "List of Calibration Items as per criteria:" & mCalibrationItemChildList.Count & " Record(s) found."
        upnlGridView.Update()
        upnlActionBtn.Update()
    End Sub

    Private Sub dgCalibrationItemList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCalibrationItemList.Sorting
        mCalibrationItemChildList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCalibrationItemChildList") = mCalibrationItemChildList
        dgCalibrationItemList.DataSource = mCalibrationItemChildList
        dgCalibrationItemList.DataBind()
        SetGrid()
    End Sub

    Private Sub btnCreateOrder_Click(sender As Object, e As System.EventArgs) Handles btnCreateOrder.Click, btnCreateOrderTop.Click
        mBaseCurrency = Currency.GetBaseCurrency()
        Dim checkString = Request.Form("chkSelectList")
        Dim chkItemIDList = Request.Form("chkItemIDList")

        If checkString Is Nothing Then
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, " Record", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            Dim values = checkString.Split(","c)
            mOrder = Order.NewOrder(38)
            mOrder.AgainstTypeID = 5
            mOrder.IsOverhaul = False
            ' If AppSettings("ClientCode") = "IND" Then
            mOrder.IsCalibrationOrder = True
            'End If
            For Each variable As String In values
                mOrder.OrderItems.Add(mOrder.ID)
                With mOrder.OrderItems.CurrentItem
                    mOrder.OrderItems.CurrentItem.ItemID = mCalibrationItemChildList(New Guid(variable)).ItemID
                    mOrder.OrderItems.CurrentItem.SerialNo = mCalibrationItemChildList(New Guid(variable)).SerialNo
                    mOrder.OrderItems.CurrentItem.Qty = mCalibrationItemChildList(New Guid(variable)).StockBalanceQty
                    mOrder.OrderItems.CurrentItem.UnitID = mCalibrationItemChildList(New Guid(variable)).UnitID
                    mOrder.OrderItems.CurrentItem.ReceiptItemID = mCalibrationItemChildList(New Guid(variable)).ReceiptItemID
                End With
            Next
            Session("mOrder") = mOrder
            Dim str As String
            str = "openledgersame('wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
        End If
    End Sub

    'Private Sub dgCalibrationItemList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgCalibrationItemList.RowDataBound
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        If (e.Row.Cells(21).Text <= 0) Then
    '            'e.Row.Cells(0).Attributes("disabled") = "disabled"
    '            e.Row.Cells(0).Enabled = False
    '        End If
    '    End If

    'End Sub
#End Region

#Region " Reports "

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Private SearchStr1 As String
    Private SearchStr2 As String
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        Dim Rpt As New crPendingCalibration
        Dim da As New CSLA.Data.ObjectAdapter
        'Dim ds As New dsCommon
        Dim ds As New dsCalibration
        Dim ObjCal As rptDueCalibrationList

        If cmbSearchCriteria.SelectedIndex = 0 Then
            'All
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbSearchCriteria.SelectedIndex = 1 Then
            'ItemName 
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = IIf(txtItemName.Text.Trim <> "", "" + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtItemName.Text.Trim, "")
        ElseIf cmbSearchCriteria.SelectedIndex = 2 Then
            'Description 
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = IIf(txtDescription.Text.Trim <> "", " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtDescription.Text.Trim, "")
        ElseIf cmbSearchCriteria.SelectedIndex = 3 Then
            'Serial No.
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = IIf(txtSerialNo.Text.Trim <> "", " " + cmbSearchCriteria.SelectedItem.Text + " " + ":" + " " + txtSerialNo.Text.Trim, "")
        End If

        mCalibrationItemChildList = Session("mCalibrationItemChildList")
        'dgCalibrationItemList.DataSource = mCalibrationItemChildList
        'dgCalibrationItemList.DataBind()
        If mCalibrationItemChildList.Count = 0 Then
            MSGBoxCtrl.show(" Record Not Present!  ", "There is no record for the selected criteria.", "", MsgBoxStyle.OkOnly, "NoRecord")
            Exit Sub
        End If

        Dim ReportData As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Calibration List Report", IIf(SearchStr2 <> "", SearchStr1, "").ToString, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        setVariables()

        ObjCal = rptDueCalibrationList.GetrptDueCalibrationList(FromDate, ToDate, ItemName, Description, SerialNo)
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ObjCal)
        da.Fill(ds, ReportData)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnCalibrationItem_Click(sender As Object, e As System.EventArgs) Handles hdnBtnCalibrationItem.Click, hdnBtnNewCalibrationItem.Click
        SetControl()
        SetGrid()
        upnlActionBtn.Update()
        upnlGridView.Update()
    End Sub
#End Region



End Class