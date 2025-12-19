'***********************************
'Created By  SAYLEE
'Dated:     : 10-Jul-2023
'***********************************

Imports System.Linq

Public Class wfnResourceAllocationAMO
    Inherits Page

#Region " Variable Declaration "
    Public mWOJobStatusList As nWOJobStatusList

    Public mWOJobTypeList As nWOJobTypeList
    Public mnWOJob As nWOJob
    Public mnWO As nWO
    Dim mDistinctWOText As nDistinctWOText
    Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, WOJobTypeID, No, WOJobStatusID, RegNo As String
    Dim ShowUnAllocatedJobs As Boolean
    Dim WOID As String
    Dim EventLogID As Guid
    Dim mWODetail As String
    Dim totcnt As Integer
    Public mpageSize As Integer = 25
    Public mCurrentpage As Integer = 1
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0

    Dim mMachineNameValueList As MachineNameValueList
    Public mWOJobList As nWOJobList
    Dim WOJobTypeIDList As Object = Nothing
#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mWOJobList = Session("mWOJobList")
        mWOJobStatusList = Session("mWOJobStatusList")
        mDistinctWOText = Session("mDistinctWOText")
        WOText = Session("WOText")
        No = IIf(IsNothing(Session("No")), 0, Session("No"))
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
        SearchIndex = Session("SearchIndex")
        DateIndex = Session("DateIndex")
        WOJobTypeID = Session("SearchWOJobTypeIDOnResourceAllocationAMO")
        WOJobStatusID = Session("WOJobStatusID")
        WOID = Session("WOID")

        mCurrentpage = Session("mCurrentpage")
        mpageSize = Session("mpageSize")
        mpageindex = Session("mpageindex")
        pagecount = Session("pagecount")

        mnWOJob = Session("mnWOJob")
        mnWO = Session("mnWO")
        totcnt = Session("totcnt")
        mMachineNameValueList = Session("mMachineNameValueList")
        RegNo = Session("RegNo")
        ShowUnAllocatedJobs = Session("ShowUnAllocatedJobs")
    End Sub

    Private Sub SetSession()
        Session("mWOJobList") = mWOJobList
        Session("mWOJobStatusList") = mWOJobStatusList

        Session("mDistinctWOText") = mDistinctWOText
        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("SearchWOJobTypeIDOnResourceAllocationAMO") = WOJobTypeID
        Session("WOJobStatusID") = WOJobStatusID
        Session("No") = No
        Session("WOText") = WOText
        Session("mWOJobList") = mWOJobList
        Session("mnWOJob") = mnWOJob
        Session("mnWO") = mnWO
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("RegNo") = RegNo
        Session("ShowUnAllocatedJobs") = ShowUnAllocatedJobs
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mWOJobList")
        Session.Remove("mWOJobStatusList")
        Session.Remove("mWOJobList")
        Session.Remove("mDistinctWOText")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
        Session.Remove("SearchIndex")
        Session.Remove("DateIndex")
        Session.Remove("SearchWOJobTypeIDOnResourceAllocationAMO")
        Session.Remove("WOJobStatusID")
        Session.Remove("No")
        Session.Remove("WOText")
        Session.Remove("mWOJobTypeList")
        Session.Remove("mWOJobList")
        Session.Remove("mnWOJob")
        Session.Remove("mnWO")

        Session.Remove("mCurrentpage")
        Session.Remove("mpageSize")
        Session.Remove("mpageindex")
        Session.Remove("pagecount")

        Session.Remove("totcnt")
        Session.Remove("mMachineNameValueList")
        Session.Remove("RegNo")
        Session.Remove("ShowUnAllocatedJobs")
        Session.Remove("WOID")
    End Sub

    Private Sub ClearAll()

        If InStr(Session("MiddleFrame"), "wfnResourceAllocationAMO.aspx") <= 0 Then
            RemoveSession()
            Session.Remove("mWOJobList")
        End If

    End Sub

    Private Sub EditRecord(mId As Guid, mWOID As Guid)

        mnWO = nWO.GetWO(mWOID, False)

        If mnWO.WOJobs.Count <> 0 Then
            mnWOJob = mnWO.WOJobs.Item(mId)

            If Not mnWOJob Is Nothing Then
                mnWO.WOJobs.CurrentIndex = mnWO.WOJobs.IndexOfItem(mId)
                Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
            Else

                'NRC jobs
                mnWOJob = nWOJob.GetWOJobNRC(mId)

            End If

        ElseIf mnWO.WONRCJobs.Count <> 0 And mnWOJob Is Nothing Then

            mnWOJob = mnWO.WONRCJobs.Item(mId)
            mnWO.WONRCJobs.CurrentIndex = mnWO.WONRCJobs.IndexOfItem(mId)
            Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID
            Session("mnWO") = mnWO

        End If

        Session("mnWOJob") = mnWOJob
        Session("mnWO") = mnWO

    End Sub

    Private Overloads Sub SetFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript([GetType](), "focusscript", str)
    End Sub

    Private Sub SetPeriod(Index As Int32)

        If FromDate = "1/1/1900" Then
            txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
        Else
            txtFromDate.Text = FromDate
        End If

        If ToDate = "1/1/2200" Then
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        Else
            txtToDate.Text = ToDate
        End If

    End Sub

    Private Sub SetVariables()
        If Not WOID Is Nothing Then
            WOID = IIf(WOID.ToString.Length > 0, WOID.ToString, Guid.Empty.ToString)
        Else
            WOID = Guid.Empty.ToString
        End If


        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
        ''  WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
        WOJobTypeID = IIf(cmbWOJobType.SelectedIndex <= 0, 0, cmbWOJobType.SelectedValue)
        ''  RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedValue)
        ''  No = txtNo.Text.Trim

        ShowUnAllocatedJobs = IIf(chkUnallocatedJobs.Checked, chkUnallocatedJobs.Checked, False)

        Session("FromDate") = FromDate
        Session("ToDate") = ToDate
        Session("SearchIndex") = SearchIndex
        Session("DateIndex") = DateIndex
        Session("SearchWOJobTypeIDOnResourceAllocationAMO") = WOJobTypeID
        Session("WOJobStatusID") = WOJobStatusID
        Session("No") = No
        Session("WOText") = WOText
        Session("RegNo") = RegNo
        Session("WOID") = WOID
        Session("ShowUnAllocatedJobs") = ShowUnAllocatedJobs
    End Sub

    Private Sub SetControl()
        SetPeriod(DateIndex)
        CallFindNow(SearchIndex)
        dgWOJobList.DataBind()



        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgWOJobList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = CInt(Session("mpageindex"))
        pagecount = CInt(Session("pagecount"))

        mpageindex = dgWOJobList.PageIndex
        mCurrentpage = mpageindex + 1

        Session("mpageindex") = mpageindex
        Session("mCurrentpage") = mCurrentpage
        Session("mpageSize") = mpageSize




        ' cmbWO.SelectedValue = IIf(WOText = "", "(All)", WOText)
        cmbWOJobType.SelectedIndex = WOJobTypeID

        '  txtNo.Text = No
        '  cmbAircraft.SelectedValue = IIf(RegNo = "", "(ALL)", RegNo)
        chkUnallocatedJobs.Checked = ShowUnAllocatedJobs

        ControlVisibility(SearchIndex, DateIndex)

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            ' dgWOJobList.Columns(3).HeaderText = "E.O. No."
            dgWOJobList.DataBind()
            lblResult.Text = "List of Engineering Order Jobs as per criteria : " & mWOJobList.Count & " Record(s) found."
        Else
            ' dgWOJobList.Columns(3).HeaderText = "W.O. No."

            dgWOJobList.DataBind()
            lblResult.Text = "List of Work Order Jobs as per criteria : " & mWOJobList.Count & " Record(s) found."
        End If
    End Sub

    Private Sub SetTitle()

        If (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblTitle.Text = "List of Engineering Order Jobs "  'shweta [Total No of Record(s):-" + totcnt.ToString() + "]
        Else
            lblTitle.Text = "List of Work Order Jobs "  'shweta  [Total No of Record(s):-" + totcnt.ToString() + "]
        End If

    End Sub

    Private Sub AddAttributes()

    End Sub

    Private Sub FindNow(Optional Text As String = "",
                        Optional No As Int32 = 0,
                        Optional FromDate As String = "1/1/1900",
                        Optional ToDate As String = "1/1/2200",
                        Optional WOJobStatusID As Integer = 0,
                        Optional WOJobTypeID As Integer = 0)

        mWOJobList = Nothing
        dgWOJobList.DataSource = Nothing

        mWOJobList = nWOJobList.GetWOJobList(Text,
                                             No,
                                             FromDate,
                                             ToDate,
                                             WOJobStatusID,
                                             WOJobTypeID,
                                             WOID:=IIf(WOID Is Nothing, "{00000000-0000-0000-0000-000000000000}", WOID),
                                             ShowUnAllocatedJobs:=ShowUnAllocatedJobs)
        dgWOJobList.DataSource = mWOJobList
        Session("mWOJobList") = mWOJobList

    End Sub

    Private Sub CallFindNow(Index As Integer)

        FindNow(WOText, CInt(Val(No)), txtFromDate.Text.ToString, txtToDate.Text.ToString, , WOJobTypeID)
        dgWOJobList.PageIndex = 0

    End Sub

    Private Sub ControlVisibility(SearchIndex As Int32,
                                  Optional DateIndex As Int32 = 0)

    End Sub

    Private Sub ClearControls()
    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Dim TempWOID As Guid
        Dim msgCount As Integer = 0

        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Dim mnWO As nWO
                            Session("sender") = ""
                            mnWO = CType(Session("mnWO"), nWO)
                            TempWOID = mnWO.ID

                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then

                                If (mnWO.IsSync = 1 Or mnWO.IsSync = 2) Then
                                    MSGBoxCtrl.Show("Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly, "")
                                    Exit Sub
                                Else

                                    mnWO.Delete()
                                    mnWO.Save()
                                    DataFieldBind()
                                    SetControl()
                                    SetTitle()
                                    SetGrid()
                                    upnlGridView.Update()
                                    upnlActionBtnTop.Update()
                                    upnlActionBtnBottom.Update()
                                    upnlResult.Update()

                                End If

                            Else

                                mnWO.Delete()
                                mnWO.Save()
                                DataFieldBind()
                                SetControl()
                                SetTitle()
                                SetGrid()
                                upnlGridView.Update()
                                upnlActionBtnTop.Update()
                                upnlActionBtnBottom.Update()
                                upnlResult.Update()

                            End If

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
                                        "Work Order",
                                        "Can't delete : " & mWODetail &
                                        " is Currently in use",
                                        ErrorType.NoError,
                                        TempWOID,
                                        EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            DataFieldBind()
                            SetControl()
                            SetGrid()
                            upnlResult.Update()
                            msgCount = ex.Errors.Count

                        Finally

                            If msgCount = 0 Then
                            End If

                        End Try

                    End If

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
            End Select

        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If

    End Sub

    Public Sub SetToolTip()

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso
           (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

            lblTitle.Text = "List of Engineering Order Jobs"
            dgWOJobList.ToolTip = "List of Engineering Order Jobs"
            btnClose.ToolTip = "Click to close List of Engineering Order Job screen"
            btnCloseTop.ToolTip = "Click to close List of Engineering Job Order screen"
            btnFindNow.ToolTip = "Click to find list of Engineering Order Jobs as per searching criteria"

        Else

            lblTitle.Text = "List of Work Order Jobs"
            dgWOJobList.ToolTip = "List of Work Order Job"
            btnClose.ToolTip = "Click to close List of Work Order Job screen"
            btnCloseTop.ToolTip = "Click to close List of Work Order Job screen"
            btnFindNow.ToolTip = "Click to find list of Work Order Jobs as per searching criteria"

        End If

    End Sub

#End Region

#Region " DataFieldBind "

    Private Sub DataFieldBind()

        FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
        ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
        SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
        DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
        WOJobTypeID = Session("SearchWOJobTypeIDOnResourceAllocationAMO")
        WOJobStatusID = Session("WOJobStatusID")
        WOText = Session("WOText")
        RegNo = Session("RegNo")
        ShowUnAllocatedJobs = Session("ShowUnAllocatedJobs")
        mWOJobList = nWOJobList.GetWOJobList()
        totcnt = mWOJobList.Count 'Added by shweta on 11-1-12
        Session("totcnt") = totcnt 'Added by shweta on 11-1-12
        dgWOJobList.DataSource = mWOJobList
        Session("mWOJobList") = mWOJobList
        mWOJobTypeList = nWOJobTypeList.GetWOJobTypeList("(All)")
        WOJobTypeIDList = (From c As nWOJobTypeList.nWOJobTypeListInfo In mWOJobTypeList
                           Where {0, 1, 2, 5}.Contains(c.ID)
                           Select c).ToList
        cmbWOJobType.DataSource = WOJobTypeIDList

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblResult.Text = "List of Engineering Order Jobs as per criteria : " & mWOJobList.Count & " Record(s) found."
        Else
            lblResult.Text = "List of Work Order Jobs as per criteria : " & mWOJobList.Count & " Record(s) found."
        End If

        DataBind()

    End Sub

    Private Sub SetGrid()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here
        ClearAll()
        AddAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            Session("MiddleFrame") = "wfnResourceAllocationAMO.aspx"
            cmbShowE.SelectedValue = 4
            DataFieldBind()
            SetControl()

        End If

        SetToolTip()
        SetTitle()
        SetGrid()

    End Sub

    Private Sub GV_WOJobList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgWOJobList.PageIndexChanging

        dgWOJobList.PageIndex = e.NewPageIndex
        dgWOJobList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        dgWOJobList.DataSource = mWOJobList
        Session("mWOJobList") = mWOJobList
        dgWOJobList.DataBind()

        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgWOJobList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        mpageindex = e.NewPageIndex
        pagecount = CInt(Session("pagecount"))

    End Sub

    Private Sub SearchRecord(sender As Object, e As EventArgs) Handles btnFindNow.Click

        SetVariables()
        CallFindNow(SearchIndex)
        dgWOJobList.DataBind()

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
        Else
            lblResult.Text = "List of Work Order Jobs as per criteria : " & mWOJobList.Count & " Record(s) found."
        End If

        upnlGridView.Update()
        upnlActionBtnTop.Update()
        upnlActionBtnBottom.Update()
        upnlResult.Update()

    End Sub

    Private Sub CloseTop(sender As Object, e As EventArgs) Handles btnCloseTop.Click, btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub GV_WOJobList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgWOJobList.RowCommand

        Select Case e.CommandName
            Case "Allocate"

                Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobList.PageSize * dgWOJobList.PageIndex
                Dim mId As Guid = mWOJobList(Index).ID
                Dim mWOID As Guid = mWOJobList(Index).WOID
                Dim mDate As String = mWOJobList(Index).WODateFormatted
                Dim mWorkOrderNo As String = mWOJobList(Index).WONumber
                Dim mDescription As String = mWOJobList(Index).WOJobDescription

                Dim mJobType As String = mWOJobList(Index).WOJobType

                mWODetail = mWorkOrderNo + " Dated : " +
                            mDate + " Description : " +
                            mDescription + " Job Type : " + mJobType

                MarkLog(Action.Edit,
                        "Work Order Job",
                        mWODetail,
                        ErrorType.NoError,
                        mId,
                        EventLogID)

                EditRecord(mId, mWOID)
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenResourceAllocationWindow",
                                                    "OpenResourceAllocationWindow()",
                                                    True)
        End Select

    End Sub

    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)

        SetVariables()
        dgWOJobList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
        Session("mpageSize") = cmbShowE.SelectedItem.ToString
        mpageSize = IIf(CInt(Session("mpageSize")) = 0, dgWOJobList.PageSize, CInt(Session("mpageSize")))
        mCurrentpage = CInt(Session("mCurrentpage"))
        pagecount = CInt(Session("pagecount"))
        SetControl()
        upnlGridView.Update()
        upnlResult.Update()

    End Sub

    Private Sub GV_WOJobList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgWOJobList.Sorting
        mWOJobList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgWOJobList.DataSource = mWOJobList
        Session("mWOJobList") = mWOJobList
        dgWOJobList.DataBind()
    End Sub

    Protected Sub WO_TextChanged(sender As Object, e As EventArgs)

        If SelectedWOID.Value <> "" Then
            WOID = IIf(SelectedWOID.Value.Length > 0, SelectedWOID.Value, Guid.Empty.ToString)
            Session("WOID") = WOID
        ElseIf txtWO.Text <> "" Then
            Dim mWOListForCombo As nWOListForCombo = nWOListForCombo.GetnWOListForCombo(WONumber:=txtWO.Text, TranstypeID:=0)
            WOID = IIf(txtWO.Text.Length > 0, mWOListForCombo(0).ID.ToString, Guid.Empty.ToString)
            Session("WOID") = WOID
        Else
            WOID = Guid.Empty.ToString
            Session("WOID") = WOID
        End If

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnBottomPrint.Click, btnPrintTop.Click

        Dim mCompanyDetail As New CompanyDetail
        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim Rpt As New crnWOJobList
        Dim da As New ObjectAdapter
        Dim ds As New dsWOJobList
        Dim ReportDetails As New rptStatusList

        SetVariables()
        CallFindNow(SearchIndex)

        SearchStr1 = "The report shows records filtered by the following criteria"

        SearchStr2 = "By" + " " + "Date Range " + FromDate + " " +
                     ToDate + " " + IIf(txtWO.Text = "", "", "WO. No. " + txtWO.Text.Trim) +
                     IIf(cmbWOJobType.SelectedIndex <= 0, "", " Job Type " + cmbWOJobType.SelectedItem.Text) +
                     IIf(RegNo = "", "", " Reg No. " + RegNo)

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                     mCompanyDetail.Address,
                                     mCompanyDetail.Tel1,
                                     mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax,
                                     mCompanyDetail.Email,
                                     mCompanyDetail.WebSite,
                                     "",
                                     SearchStr1,
                                     SearchStr2,
                                     "",
                                     "",
                                     "",
                                     AppSettings("Product Version"),
                                     AppSettings("SINote"),
                                     "",
                                     "",
                                     "",
                                     "",
                                     AppSettings("Logo"))

        If mWOJobList.Count = 0 Then

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                            MSGBox.Message_text.NoRecordFound,
                            "There is no record for this search criteria",
                            MsgBoxStyle.OkOnly,
                            "")

            Exit Sub

        End If

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mWOJobList)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "openTranDetail",
                                            "openTranDetail();",
                                            True)

    End Sub

    Private Sub HdnBtnResourceAllocation_Click(sender As Object, e As EventArgs) Handles hdnBtnResourceAllocation.Click
        DataFieldBind()
        SetControl()
        upnlGridView.Update()
    End Sub

#End Region

#Region "Service Methods"

    <Services.WebMethod(), Script.Services.ScriptMethod()>
    Public Shared Function GetWOListAutoComplete(prefixText As String,
                                                 count As Integer,
                                                 contextKey As String) As String()

        Dim mWOListForCombo As nWOListForCombo = nWOListForCombo.GetnWOListForCombo(WONumber:=prefixText, TranstypeID:=0)

        If count = 0 Then

            Return (From c As nWOListForCombo.nWOListForComboInfo In mWOListForCombo
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.WONumber, c.ID.ToString())).Take(count).ToArray
        Else

            Return (From c As nWOListForCombo.nWOListForComboInfo In mWOListForCombo
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.WONumber, c.ID.ToString())).Take(count).ToArray

        End If

    End Function

#End Region

End Class