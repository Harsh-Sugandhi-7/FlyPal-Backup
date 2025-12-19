'AJAX Conversion by vikrant on 24-Jun-2015

Public Class wfManualSearch_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim mCategoryListForManualSearch As CategoryNameValueList
    Dim mFileAttach As FileAttach
    Dim FromDate, ToDate As String
    Dim mManualRevision As ManualRevision
    Dim mManualRevisionList As ManualRevisionList
    Dim EventLogID As Guid
#End Region

#Region "Methods"
    Private Sub GetSession()
        mManualRevisionList = Session("mManualRevisionList")
        mCategoryListForManualSearch = Session("mCategoryListForManualSearch")
        FromDate = Session("FromDate")
        ToDate = Session("ToDate")
    End Sub
    Private Sub SetSession()
        Session("mManualRevisionList") = mManualRevisionList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mManualRevisionList")
        Session.Remove("mFileAttach")
        Session.Remove("mCategoryListForManualSearch")
        Session.Remove("FromDate")
        Session.Remove("ToDate")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok

            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        If Index = 6 Then
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub ControlVisibility()
        'Added by Saylee on 16-Nov-2009
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            dgManualRevList.Columns(11).HeaderText = "Subscription No."
            dgManualRevList.Columns(13).HeaderText = "Expiry Date"
        Else
            dgManualRevList.Columns(11).HeaderText = "No."
            dgManualRevList.Columns(13).HeaderText = "Effective Date"
        End If
        '*********************************
        'dgManualRevList.DataBind()

        If mManualRevisionList.Count = 0 Then
            btnPrint.Enabled = False
            btnPrintTop.Enabled = False
            btnExport.Enabled = False
        Else
            btnPrint.Enabled = True
            btnPrintTop.Enabled = True
            btnExport.Enabled = True
        End If

        If mManualRevisionList.Count > 20 Then
            btnPrintTop.Visible = True
            btnCloseTop.Visible = True
        Else
            btnPrintTop.Visible = True
            btnCloseTop.Visible = True
        End If

    End Sub
    'Added by Archana on 29-July-09
    'Private Sub SetValues()
    '    'SearchStr2 = IIf(mManualSelection.ReportString <> "", mManualSelection.ReportString, "")
    '    'SearchStr3 = IIf(mCategorySelection.ReportString <> "", mCategorySelection.ReportString, "")
    '    'Added by archana on $-Aug-09
    '    SearchStr2 = IIf(mManualSelection.ReportString <> "", mManualSelection.Name, "")
    '    SearchStr3 = IIf(mCategorySelection.ReportString <> "", mCategorySelection.Name, "")

    '    If SearchStr2 = "" And SearchStr3 = "" And mDateSelection.StartDate.ToString = "01-Jan-1800" Then
    '        SearchStr1 = "The report shows all records till date."
    '        SearchStr4 = ""
    '        SearchStr5 = ""
    '    Else
    '        SearchStr1 = "The report shows record(s) filtered by following criteria :"
    '        If mDateSelection.StartDate.ToString = "01-Jan-1800" Then
    '            SearchStr4 = ""
    '            SearchStr5 = ""
    '        Else
    '            SearchStr4 = mDateSelection.StartDate
    '            SearchStr5 = mDateSelection.EndDate
    '        End If
    '    End If
    '    Session("SearchStr1") = SearchStr1
    '    Session("SearchStr2") = SearchStr2
    '    Session("SearchStr3") = SearchStr3
    '    Session("SearchStr4") = SearchStr4
    '    Session("SearchStr5") = SearchStr5

    'End Sub
    Private Sub SetGrid()
        Dim P As Integer
        For j As Integer = 0 To dgManualRevList.Rows.Count - 1
            P = CType(Me.dgManualRevList.Rows(j).Cells(18).Text, Boolean)
            If P = False Then
                dgManualRevList.Rows(j).Cells(17).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryListForManualSearch = CategoryNameValueList.GetCategoryNameValueList("(ALL)")
        Session("mCategoryListForManualSearch") = mCategoryListForManualSearch
        cmbCategory.DataSource = mCategoryListForManualSearch

        mManualRevisionList = ManualRevisionList.GetManualRevisionList("", Guid.Empty, "")
        Session("mManualRevisionList") = mManualRevisionList
        dgManualRevList.DataSource = mManualRevisionList

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblList.Text = "List of Manual & Subscription as per criteria : " & mManualRevisionList.Count & " Record(s) found."
        Else
            lblList.Text = "List of Manual & Revision as per criteria : " & mManualRevisionList.Count & " Record(s) found."
        End If
        DataBind()
    End Sub
    Private Sub SetPeriod(ByVal index As Int32)
        Select Case index
            Case 0 'All'
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End Select
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            'SetGrid()
            ControlVisibility()
            ControlVisibility(0)
            SetPeriod(0)
            cmbPeriod.SelectedIndex = 0
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearch.Click
        dgManualRevList.PageIndex = 0

        mManualRevisionList = ManualRevisionList.GetManualRevisionList(txtManualName.Text.Trim, New Guid(cmbCategory.SelectedValue.ToString), txtSearch.Text.Trim, txtFromDate.Text, txtToDate.Text)
        dgManualRevList.DataSource = mManualRevisionList
        dgManualRevList.DataBind()
        Session("mManualRevisionList") = mManualRevisionList

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            lblList.Text = "List of Manual & Subscription as per criteria : " & mManualRevisionList.Count & " Record(s) found."
        Else
            lblList.Text = "List of Manual & Revision as per criteria : " & mManualRevisionList.Count & " Record(s) found."
        End If

        'SetGrid()
        ControlVisibility()
        upnlGrid.Update()
        upnlActionBtn.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub dgManualRevList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManualRevList.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgManualRevList.PageIndex * dgManualRevList.PageSize
                Dim RevID As Guid = New Guid(dgManualRevList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                mManualRevision = ManualRevision.GetManualRevision(RevID)
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------

                mFileAttach = FileAttach.GetAttachment(mManualRevision.RevisionID)
                Session("mFileAttach") = mFileAttach
                If mManualRevision.IsAttachmentAdded Then
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    End If
                End If
        End Select
    End Sub
    Private Sub dgManualRevList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgManualRevList.PageIndexChanging
        dgManualRevList.PageIndex = e.NewPageIndex
        dgManualRevList.DataSource = mManualRevisionList
        dgManualRevList.DataBind()
        'SetGrid()
        Session("mManualRevisionList") = mManualRevisionList
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbPeriod.SelectedIndex <= 0, 0, cmbPeriod.SelectedIndex)
        ControlVisibility(Index)
        SetPeriod(Index)
    End Sub
#End Region

#Region " Report "

#Region "Report Variable Declaration"
    Dim mCompanyDetail As New Flypal.CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region "Event"
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click, btnPrint.Click

        SetReport(False)

    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        ' Dim Rpt As New crManualRevision
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsManualRevisionList
        Dim Obj As ManualRevisionList
        Dim ReportName As String
        ' Dim mManualRevisionList As ManualRevisionList
        ''Rpt = New crManualRevisionList
        'Added by Saylee on 16-Nov-2009
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            Rpt = New crManualRevisionListForTAAL
            ReportName = "Manual Report"
        ElseIf AppSettings("ClientCode") = "GEP" Then
            Rpt = New crManualRevisionListForGEP
            ReportName = "Manual Revision List Report"
        Else
            Rpt = New crManualRevisionList
            ReportName = "Manual Revision List Report"
        End If

        mManualRevisionList = Session("mManualRevisionList")
        'dgManualRevList.DataSource = mManualRevisionList
        'dgManualRevList.DataBind()
        'SetGrid()



        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ReportName, SearchStr1:="", txtManualName.Text.Trim, IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.ToString, ""), "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        dgManualRevList.Visible = True

        Obj = mManualRevisionList
        ds.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 27-Feb-2012
        da.Fill(ds, Obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)

        Session("CrystalReport") = Rpt

        If IsExcel Then
            Dim columnToRemove As String() = {"ManualID", "MCategoryID", "IsInUse", "RevisionID", "HardCopy",
                                              "SoftCopy", "HardCopyString", "SoftCopyString", "RevImageFile", "RevImageSize", "RevFileExtension",
                                              "IsAttachmentAdded", "MRevisionSrNo", "EffectiveDateForSorting", "RevNote", "RemainingDays",
                                              "Frequency", "PropertyName", "PropertyValue", "DateOfIssue"}

            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ManualRevisionList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ManualRevisionList").Columns.Remove(columnToRemove(i))
                End If
            Next



            'Set Column Sequence
            ds.Tables("ManualRevisionList").Columns("SrNo").SetOrdinal(0)
            ds.Tables("ManualRevisionList").Columns("Name").SetOrdinal(1)
            ds.Tables("ManualRevisionList").Columns("ManualNo").SetOrdinal(2)
            ds.Tables("ManualRevisionList").Columns("ApplicableFor").SetOrdinal(3)
            ds.Tables("ManualRevisionList").Columns("MCategoryName").SetOrdinal(4)

            If AppSettings("ClientCode") = "7AR" Then
                ds.Tables("ManualRevisionList").Columns.Remove("ShortDesc")
                ds.Tables("ManualRevisionList").Columns("Note").SetOrdinal(5)
                ds.Tables("ManualRevisionList").Columns("IsInUseTag").SetOrdinal(6)

                ds.Tables("ManualRevisionList").Columns("No").SetOrdinal(7)
                ds.Tables("ManualRevisionList").Columns("RevNo").SetOrdinal(8)
                ds.Tables("ManualRevisionList").Columns("RevDate").SetOrdinal(9)
                ds.Tables("ManualRevisionList").Columns("EffectiveDate").SetOrdinal(10)
                ds.Tables("ManualRevisionList").Columns("Remark").SetOrdinal(11)
            Else
                ds.Tables("ManualRevisionList").Columns("ShortDesc").SetOrdinal(5)
                ds.Tables("ManualRevisionList").Columns("Note").SetOrdinal(6)
                ds.Tables("ManualRevisionList").Columns("IsInUseTag").SetOrdinal(7)

                ds.Tables("ManualRevisionList").Columns("No").SetOrdinal(8)
                ds.Tables("ManualRevisionList").Columns("RevNo").SetOrdinal(9)
                ds.Tables("ManualRevisionList").Columns("RevDate").SetOrdinal(10)
                ds.Tables("ManualRevisionList").Columns("EffectiveDate").SetOrdinal(11)
                ds.Tables("ManualRevisionList").Columns("Remark").SetOrdinal(12)
            End If


            '*************************************************************

            'Column Names****************************************
            If ds.Tables("ManualRevisionList").Columns.Contains("Name") Then
                ds.Tables("ManualRevisionList").Columns("Name").ColumnName = "Manual Name"
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("ManualNo") Then
                ds.Tables("ManualRevisionList").Columns("ManualNo").ColumnName = "	Manual No."
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("ApplicableFor") Then
                ds.Tables("ManualRevisionList").Columns("ApplicableFor").ColumnName = "Applicable For"
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("ShortDesc") Then
                ds.Tables("ManualRevisionList").Columns("ShortDesc").ColumnName = "Description"
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("No") Then
                ds.Tables("ManualRevisionList").Columns("No").ColumnName = "No."
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("IsInUseTag") Then
                ds.Tables("ManualRevisionList").Columns("IsInUseTag").ColumnName = "In Use"
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("RevNo") Then
                ds.Tables("ManualRevisionList").Columns("RevNo").ColumnName = "Revision No."
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("RevDate") Then
                ds.Tables("ManualRevisionList").Columns("RevDate").ColumnName = "Effective Date"
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("EffectiveDate") Then
                ds.Tables("ManualRevisionList").Columns("EffectiveDate").ColumnName = "Next Revision Date"
            End If
            If ds.Tables("ManualRevisionList").Columns.Contains("MCategoryName") Then
                ds.Tables("ManualRevisionList").Columns("MCategoryName").ColumnName = "Category"
            End If

            '***********************************************************


            Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite",
                                                           "ReportName", "SearchStr1", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "ProductVersion",
                                                           "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15",
                                                           "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23",
                                                           "SearchStr24", "SearchStr25", "ShortName", "SearchStr26", "SearchStr27", "SearchStr28",
                                                           "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34",
                                                           "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40",
                                                           "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46",
                                                           "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ApprovalNo"}

            For i As Integer = 0 To columnToRemoveCriteria.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
                End If
            Next

            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Manual Name"
            End If
            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Category"
            End If

            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ManualRevisionList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ManualRevisionList").TableName = "Manual Report"
			Session("ExcelFileName") = "Manual Report"
			Session("dsNew") = dsNew
			MarkLog(Util.Action.Print, "Manual Report", "Export to excel done by user", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFileExcel", "openFileExcel();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        SetReport(True)

    End Sub
#End Region

#End Region

End Class