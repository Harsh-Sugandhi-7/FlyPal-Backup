Imports System.Web.UI.DataVisualization.Charting
Public Class wfHangarPlanningList
    Inherits System.Web.UI.Page


#Region "variable declaration"
    Public mhangarlist As HangarList
    Public mhanger As Hanger
    Dim Index As Int32
    Public shango As HangarList
    Dim mSearchIndex, mFromDate, mToDate, mAircraft, mHang, mText, mNo, mRemark As String
    Public mHangerMasterList As HangerMasterList
    Public mDistinctTextListForHangar As DistinctTextListForHangar
    Public mDistinctHangarListForHangar As DistinctHangarListForHangar
    Public mdistinctGood As DistinctGood
    Dim mFileAttach As FileAttach
    Dim mTransactionListCount As TransactionListCount
    Public mTransTypeID As Trans
#End Region
#Region "business properties"
    Private Sub GetSession()
        mhangarlist = CType(Session("mHangarList"), HangarList)
        mhanger = CType(Session("mHanger"), Hanger)
        Session("NewPage") = "False"
        mSearchIndex = Session("SearchIndex")
        mFromDate = Session("FromDate")
        mToDate = Session("ToDate")
        mAircraft = Session("mAircraft")
        mHang = Session("mHang")
        mText = Session("mText")
        mNo = Session("mNo")
        mRemark = Session("mRemark")
        mFileAttach = Session("mFileAttach")
        mTransactionListCount = Session("mTransactionListCount")
    End Sub
    Private Sub SetSession()
        Session("mhangarlist") = mhangarlist
        Session(" mhanger") = mhanger
        Session("SearchIndex") = mSearchIndex
        Session("mTransactionListCount") = mTransactionListCount
    End Sub
    Private Sub RemoveSession()
        Session.Remove("SearchIndex")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("mAircraft")
        Session.Remove("mHang")
        Session.Remove("mText")
        Session.Remove("mNo")
        Session.Remove("mFileAttach")
        Session.Remove("mTransactionListCount")
    End Sub
    Private Sub SetTitle()
        Dim mTransTypeList As TransactionList
        mTransTypeList = TransactionList.GetTransactionList()
        lblLedgerList.Text = "List of " + "Hangar Planning" + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"
        upnlTitle.Update()
    End Sub
   
    Private Sub ClearAll()
        'If Session("MiddleFrame") <> "wfhangarPlanningCalendarList.aspx?" Then
        '    RemoveSession()
        'End If
      
        If InStr(Session("MiddleFrame"), "wfhangarPlanningList.aspx?") <= 0 Then
            RemoveSession()

        End If
    End Sub

    Private Sub ControlVisibility(ByVal SearchIndex As Int32)
        txtFromDate.Visible = IIf(SearchIndex = 1, True, False)
        txtToDate.Visible = IIf(SearchIndex = 1, True, False)

        cmbHanger.Visible = IIf(SearchIndex = 2, True, False)
        'TxtAircraft.Visible = IIf(SearchIndex = 2, True, False)
        'TxtHangar.Visible = IIf(SearchIndex = 3, True, False)
        cmbText.Visible = IIf(SearchIndex = 3, True, False)
        '  txtText.Visible = IIf(SearchIndex = 4, True, False)
        ' txtNo.Visible = IIf(SearchIndex = 4, True, False)
        ' lblNo.Visible = IIf(SearchIndex = 2 And cmbIssueText.SelectedIndex <> 0 Or SearchIndex = 3 And cmbReceiptText.SelectedIndex <> 0 Or cmbIssueToType.SelectedIndex = 7 And cmbWoText.SelectedIndex <> 0 Or cmbIssueToType.SelectedIndex = 8 And cmbRequisitionText.SelectedIndex <> 0 Or SearchIndex = 8 And cmbOrderText.SelectedIndex <> 0, True, False)
        lblDateTimeFrom.Visible = IIf(SearchIndex = 1, True, False)
        lblDateTimeTo.Visible = IIf(SearchIndex = 1, True, False)
        txtNo.Visible = IIf(SearchIndex = 3 And cmbText.SelectedIndex <> 0, True, False)
        '' txtFromDate.Visible = False
        ' 'txtToDate.Visible = False
        If SearchIndex = 1 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            txtNo.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        'If SearchIndex = 2 Then
        '    txtNo.Visible = False
        'End If
        If cmbText.SelectedIndex = 0 Then
            txtNo.Visible = False
        End If
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        DatafieldBind()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mhanger = Hanger.GetHangar(mId)
        mhanger.MarkClean()
        Session("mhanger") = mhanger

    End Sub
    Private Sub setVariables()
        mSearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        mFromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
        mToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")
        mHang = IIf(cmbHanger.SelectedIndex < 0, "", cmbHanger.SelectedValue)
        ' mAircraft = TxtAircraft.Text.Trim
        ' mHangar = TxtHangar.Text.Trim
        mText = IIf(cmbText.SelectedIndex <= 0, "", cmbText.SelectedValue)

        mNo = txtNo.Text.Trim
        Session("FromDate") = mFromDate
        Session("ToDate") = mToDate
        Session("SearchIndex") = mSearchIndex
        Session("mAircraft") = mAircraft
        Session("mHangar") = mHang
        Session("mNo") = mNo
        Session("mText") = mText
    End Sub
    Private Sub SetControl()
        mFromDate = txtFromDate.Text
        mToDate = txtToDate.Text
        mHang = cmbHanger.SelectedValue.ToString()
        mSearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        ' mSearchIndex = IIf(IsNothing(mSearchIndex), 0, mSearchIndex)
        CallFindNow(mSearchIndex, 1)
        dgHangerList.DataBind()
        cmbSearch.SelectedIndex = mSearchIndex
        cmbHanger.SelectedValue = mHang
        'txtFromDate.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
        'txtToDate.Text = DateTime.Today.Date.ToString(AppSettings("DateFormat"))
        'txtFromDate.Text = mFromDate
        'txtToDate.Text = mToDate
        'TxtAircraft.Text = mAircraft
        'TxtHangar.Text = mHangar
        cmbText.SelectedValue = mText
        ' txtNo.Text = mNo
        'IIf(cmbText.SelectedIndex = mText, txtNo.Text = mNo, txtNo.Visible = False)
        'txtText.Text = mText
        txtNo.Text = mNo
        ControlVisibility(mSearchIndex)
        lblResults.Text = "List of Hangar Planning as per criteria :" & mhangarlist.Count & " Record(s) found."

    End Sub
    Private Sub ClearControls()
        txtNo.Text = ""
        txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        cmbHanger.SelectedIndex = 0
        cmbText.SelectedIndex = 0

    End Sub

    Private Sub FindNow(Optional ByVal caircraft As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal changar As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal cdatetimefrom As String = "1/1/1900", Optional ByVal cdatetimeto As String = "1/1/3300", Optional ByVal cRemark As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0)
        '  mhangarlist = Nothing
        mhangarlist = HangarList.GetHangarList(caircraft, changar, cdatetimefrom, cdatetimeto, cRemark, Text, No)
        'dgHangerList.DataSource = Nothing
        Session("mhangarlist") = mhangarlist
        dgHangerList.DataSource = mhangarlist
        dgHangerList.DataBind()
        lblResults.Text = "List of Hangar Planning as per criteria :" & mhangarlist.Count & " Record(s) found."
        upnlGrid.Update()
        '' lblResults.Text = "List of Hangar Planning as per criteria :" & mhangarlist.Count & " Record(s) found."
    End Sub

    Private Sub CallFindNow(ByVal Index As Integer, Optional ByVal IsForPrint As Boolean = False)
        Select Case Index
            Case -1 'all
                FindNow()
            Case 0   'all
                FindNow()
            Case 1  'Hangar date
                FindNow(, , mFromDate, mToDate)
            Case 2 'Hangar
                'FindNow(, cmbHanger.SelectedValue.ToString, , , , , )
                FindNow(, mHang, , , , , )
            Case 3  'Hangar Text
                FindNow(, , , , , mText, Val(txtNo.Text))



        End Select
        dgHangerList.PageIndex = 0   'Added Code on May,25,2007
    End Sub

#End Region

#Region "DataBinding"
    Private Sub DatafieldBind()
        'mhangarlist = HangarList.GetHangarList(cdatetimefrom:=txtcdatefrom.Text, cdatetimeto:=txtcdateto.Text)
        dgHangerList.DataSource = mhangarlist
        Session("mhangarlist") = mhangarlist
        dgHangerList.DataBind()
        Titlecount()
    End Sub
    Private Sub Titlecount()
        mTransactionListCount = TransactionListCount.GetTransactionListCountt(86)
        Session("mTransactionListCount") = mTransactionListCount
    End Sub
    Private Sub Datafield()

        'mhangarlist = HangarList.GetHangarList(, changarID:=cmbHanger.SelectedValue.ToString, cdatetimefrom:=txtFromDate.Text, cdatetimeto:=txtToDate.Text, Text:=cmbText.SelectedValue, No:=Val(txtNo.Text))
        'dgHangerList.DataSource = mhangarlist
        'Session("mhangarlist") = mhangarlist
        'dgHangerList.DataBind()
        mdistinctGood = DistinctGood.GetDistinctText("3", 0, True, AddTopItem:="(ALL)")
        cmbHanger.DataSource = mdistinctGood
        cmbHanger.DataBind()
        mDistinctTextListForHangar = DistinctTextListForHangar.GetDistinctText("28", , True, "(All)")
        cmbText.DataSource = mDistinctTextListForHangar
        cmbText.DataBind()
        '  mDistinctHangarListForHangar = DistinctHangarListForHangar.GetDistinctText("1", , True, "(All)")
        'cmbHanger.DataSource = mhangarlist
        'cmbHanger.DataBind()
        'mHangerMasterList = HangerMasterList.GetHangarList()
        'cmbHanger.DataSource = mHangerMasterList
        'cmbHanger.DataBind()
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
                            mhanger = CType(Session("mhanger"), Hanger)
                            If mhanger.IsAttachmentAdded = True Then

                                mFileAttach = FileAttach.GetAttachment(mhanger.HID)
                            End If
                            mhanger.Delete()
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            mhanger.Save()
                            ' DatafieldBind()                        
                            SetControl()
                            Titlecount()
                            SetTitle()


                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Session("sender") = ""
                        DatafieldBind()
                        SetControl()

                    End If
            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtNo.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtNo').value,event)")
    End Sub
#End Region
#Region "events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        If Not IsPostBack Then
            Session("mHangerList") = "wfHangarPlanningList.aspx?"
            mhangarlist = HangarList.GetHangarList()
            DatafieldBind()
            Datafield()
            txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            SetControl()
            SetTitle()
            If cmbSearch.Enabled = True Then
                cmbSearch.Focus()
            End If
        End If
       
    End Sub

    Protected Sub dgHangerList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgHangerList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                'mhangarlist = HangarList.GetHangarList(mID)
                mhanger = Hanger.GetHangar(mID)
                '  mhanger.BeginEdit()
                Session("mhanger") = mhanger
                ' Response.Redirect("~/wfHangar.aspx")
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfHangar.aspx?BackPage=Index.aspx');", True)

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHangerWindow", "OpenHangerWindow();", True)
            Case "DeleteRec"

                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                'mhanger = Hanger.GetHangar(mID)
                'MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                ''DeleteRecord(mID)
                ''DatafieldBind()
                'upnlGrid.Update()
                'Response.Redirect("wfHangarPlanningList.aspx")
                'Session("mhanger") = mhanger
                DeleteRecord(mID)
            Case "ViewRec"
                'Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                'mhanger = Hanger.GetHangar(mID)
                'Dim No As New Random
                'Dim StrName As String = "abc" & No.Next.ToString
                'If mhanger.IsAttachmentAdded = True Then
                '    Dim path As String = AppSettings("DOCPath") & "\" & StrName & mhanger.FileAttachments(0).Extension
                '    Dim fs As FileStream
                '    If File.Exists(AppSettings("DOCPath")) = False Then
                '        'Delete File if exist
                '        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mhanger.FileAttachments(0).Extension)
                '        ' Create the file.
                '        fs = File.Create(path)
                '        '' Add some information to the file.
                '        fs.Write(mhanger.FileAttachments(0).ImageFile, 0, mhanger.FileAttachments(0).ImageFile.Length)
                '        fs.Close()
                '        Session("DOCPath") = path
                '        Dim Str As String
                '        Str = "openFile();"
                '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                '    Else

                '    End If
                'End If
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                End If
                'End
        End Select
    End Sub

    Protected Sub dgHangerList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgHangerList.PageIndexChanging
        dgHangerList.PageIndex = e.NewPageIndex
        dgHangerList.DataSource = mhangarlist
        Session("mhangarlist") = mhangarlist
        dgHangerList.DataBind()
    End Sub

    'Protected Sub btnAddNewTop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNewTop.Click
    '    SetSession()
    '    mhanger = Hanger.NewHangar()
    '    If (Not User.IsInRole("mHangerNew") And mhanger.IsNew) Or (Not User.IsInRole("mhangerEdit") And Not mhanger.IsNew) Then
    '        setObject()
    '        SetSession()

    '        MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
    '        Exit Sub
    '    End If
    '    mMachine = Machine.NewMachine(Guid.NewGuid)
    '    mhanger.BeginEdit()
    '    Session("mhanger") = mhanger
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfhanger.aspx?BackPage=Index.aspx');", True)
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfhanger?BackPage=Index.aspx');", True)
    'End Sub




    'Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNew.Click
    '    If Not IsPostBack Then
    '        Chart1.Enabled = False
    '    Else
    '        If txtcdatefrom.Text = "" Or txtcdateto.Text = "" Then
    '            Chart1.Enabled = False
    '        Else
    '            shango = HangarList.GetHangarList()
    '            shango = HangarList.GetHangarList(cdatetimefrom:=txtcdatefrom.Text, cdatetimeto:=txtcdateto.Text)
    '            Chart1.DataSource = shango
    '            Chart1.DataBind()
    '        End If
    '    End If
    'End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
    'End Sub

    'Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFindNow.Click
    '    If Not IsPostBack Then
    '        Chart1.Enabled = False
    '    Else
    '        If txtcdatefrom.Text = "" Or txtcdateto.Text = "" Then
    '            Chart1.Enabled = False
    '        Else
    '            'shango = HangarList.GetHangarList()
    '            shango = HangarList.GetHangarList(cdatetimefrom:=txtcdatefrom.Text, cdatetimeto:=txtcdateto.Text)
    '            Chart1.DataSource = shango
    '            Chart1.DataBind()
    '        End If
    '    End If
    'End Sub


    'Protected Sub close_Click(ByVal sender As Object, ByVal e As EventArgs) Handles close.Click
    '    Response.Redirect("wfHangerPlanningList.aspx")
    'End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click, btnAddTop.Click
        SetSession()
        mhanger = Hanger.NewHangar()
        'mhanger.BeginEdit()
        Session("mhanger") = mhanger
        'Response.Redirect("~/wfHangar.aspx")
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfHangar.aspx?BackPage=Index.aspx');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenHangerWindow", "OpenHangerWindow();", True)
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Session.Remove("mTransactionListCount")
        ClearAll()
        Response.Redirect("~/Dashboard.aspx")
    End Sub

    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFindNow.Click

        setVariables()
        dgHangerList.PageIndex = 0
        CallFindNow(cmbSearch.SelectedIndex)
        'ClearControls()

        ControlVisibility(cmbSearch.SelectedIndex)

    End Sub
    Protected Sub cmbSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbSearch.SelectedIndexChanged
        ClearControls()

        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            cmbSearch.Focus()
        End If
    End Sub
    Protected Sub BtnCloseTop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCloseTop.Click
        Session("sender") = ""
        Session("MiddleFrame") = ""
        ClearAll()
        ' RemoveSession()
        Response.Redirect("~/Dashboard.aspx")
    End Sub
    'Protected Sub close_Click(ByVal sender As Object, ByVal e As EventArgs) Handles close.Click
    '    Response.Redirect("~/Dashboard.aspx")
    'End Sub   

    Protected Sub cmbText_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbText.SelectedIndexChanged
        If cmbText.Enabled = True Then
            txtNo.Visible = True
            txtNo.Enabled = True
        Else
            txtNo.Visible = False
        End If

    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        'MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
    '    'Response.Redirect("wfhangarplanningschedule.aspx")
    '    Response.Redirect("wfhangarPlanningCalendarList.aspx")
    '    ' Response.Redirect("wfhangarPlanningCalendarHangarGraph.aspx")

    'End Sub

    Private Sub hdnBtnHanger_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnHanger.Click
        mSearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
        txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        mFromDate = txtFromDate.Text
        mToDate = txtToDate.Text
        mHang = cmbHanger.SelectedValue.ToString()
        CallFindNow(mSearchIndex, 1)
        ControlVisibility(mSearchIndex)
        Titlecount()
        lblLedgerList.Text = "List of " + "Hangar Planning" + " [Total No of Record(s):-" + mTransactionListCount(0).Count.ToString() + "]"
        upnlTitle.Update()
        'FindNow()
        'mdistinctGood = DistinctGood.GetDistinctText("3", 0, True)
        'cmbHanger.DataSource = mdistinctGood
        'cmbHanger.DataBind()
        'mDistinctTextListForHangar = DistinctTextListForHangar.GetDistinctText("28", , True, "(All)")
        'cmbText.DataSource = mDistinctTextListForHangar
        'cmbText.DataBind()
        UpdatePanel1.Update()
        'UpdatePanel2.Update()

    End Sub

#End Region

    ' Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
    'Dim da As New CSLA.Data.ObjectAdapter
    'Dim rpt As New crptHangarPlanning
    'Dim mCompanyDetail As New CompanyDetail

    'Dim obj As HangarList
    'Dim ds As New dsHangarPlanning
    ''obj = rptWorkInvoice.GetWorkInvoice(mWorkInvoice.ID)
    'obj = HangarList.GetHangarList()

    'mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

    'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    '        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    '        mCompanyDetail.WebSite, "Hangar Planning Report", "", "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))


    'da.Fill(ds, obj)
    'Dim mrptImage As rptImage = rptImage.GetImage(ds)
    'da.Fill(ds, mrptImage)
    'da.Fill(ds, Report)
    'rpt.SetDataSource(ds)
    'Session("CrystalReport") = rpt

    'Dim Str1 As String
    'Str1 = "openTranDetail();"
    ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    '    Response.Redirect("~/wfHangarPlanningReport1.aspx")

    'End Sub

    Protected Sub dgHangerList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgHangerList.Sorting
        mhangarlist.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mhangarlist") = mhangarlist
        dgHangerList.DataSource = mhangarlist
        dgHangerList.DataBind()
    End Sub
End Class