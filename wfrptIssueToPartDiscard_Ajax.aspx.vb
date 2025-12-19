Public Class wfrptIssueToPartDiscard_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declarations"
    Dim Fromdate As String = ""
    Dim ToDate As String = ""
    Dim Supplier As String = ""
    Dim Store As String = ""
    Dim Status As String = ""
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim ReleaseNoteNo As String = ""
    Dim SerialNo As String = ""
    Dim mTransTypeID As Integer
    Public Shadows Title As String
    Public IssueType As String
    Dim mStoreList As StoreList 'Added By Prashant 30-Apr-2012 'ALL29042013
    Dim mVendorList As VendorList
    Dim mSearchCriteriaForEventLog As String = String.Empty
#End Region

#Region "Helper Methods"
    Private Sub RemoveSession()
        PartNo = Nothing
        Description = Nothing
    End Sub

    Private Sub ControlVisibility(ByVal index As Integer)
        If index = 6 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf index = 1 Or index = 2 Or index = 3 Or index = 4 Or index = 5 Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All'
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            Fromdate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If

        ''Commented,Changed and Added By Utkarsh On 22-Dec-2011  FOR ALL13122011

        Supplier = txtSupplier.Text.Trim
        lblVendor.Text = IIf(Supplier <> "", "Supplier : " & Supplier, "Supplier : All")

        Store = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text.Substring(0, cmbStore.SelectedItem.Text.Trim.IndexOf("(")).Trim, "") 'Added By Prashant 30-Apr-2012 'ALL29042013

        SerialNo = txtSerialNo.Text.Trim

        Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")

        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        'End
        ReleaseNoteNo = txtReleaseNoteNo.Text.Trim
        lblReleaseNoteNo.Text = "Release Note No. : " & IIf(ReleaseNoteNo <> "", ReleaseNoteNo, "All")
        lblSerialNo.Text = "Serial No. :" & IIf(SerialNo <> "", SerialNo, "All")
        lblStatus.Text = "Status : " & IIf(Status <> "", Status, "All")
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        lblToStore.Text = "From Store : " & IIf(Store <> "", Store, "All")
        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + ", " + lblVendor.Text + "," + lblToStore.Text + ", " + ", " + lblReleaseNoteNo.Text + ", " + lblSerialNo.Text + ", " + lblStatus.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Private Sub ControlVisibility4()
        optSerialized.Visible = IIf(optLandscape.Checked = True, True, False)
        optAllParts.Visible = IIf(optLandscape.Checked = True, True, False)
        If optPortrait.Checked = True Then
            optSerialized.Checked = False
            optAllParts.Checked = True
        End If
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor.Visible = False
        lblSerialNo.Visible = False
        lblReleaseNoteNo.Visible = False
        lblStatus.Visible = False
        lblToStore.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblVendor.Visible = True
        lblSerialNo.Visible = True
        lblReleaseNoteNo.Visible = True
        lblStatus.Visible = True
        lblToStore.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub SetReports()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim obj As rptIssueToPartDiscard
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsReceipt As New dsIssue
        Dim l As Integer
        SetValues()
        Title = GetTitle()

        If optPortrait.Checked Then
            myReport = New crptIssueToPartDiscard
        Else
            If AppSettings("ClientCode") = "Heligo" Then
                myReport = New crptIssueToPartDiscardLandscapeForHeligo
            Else
                myReport = New crptIssueToPartDiscardLandscape
            End If
        End If
        If optLandscape.Checked = True Then
            If optSerialized.Checked = True Then
                l = 1
            Else
                l = 0
            End If
        End If
        obj = rptIssueToPartDiscard.GetIssueToPartDiscard(l, Fromdate, ToDate, Store, Supplier, Val(cmbStatus.SelectedValue), ReleaseNoteNo, SerialNo, _
                                                          PartNo, Description, cmbStore.SelectedValue.ToString, Util.Trans.DisacrdPart, _
                                                          SupplierID:=cmbSupplier.SelectedValue.ToString)

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, "", _
                                                                                  ReleaseNoteNo, "", "", "", "", "", "", "", Supplier, Store, Status, _
                                                                                  IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), _
                                                                                  PartNo, Description, "", "", Store, Title, "", "", SerialNo, "", "", "", _
                                                                                  "", 19, "", "", AppSettings("Logo"))

        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf obj.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1209)
        End If
        dsReceipt.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt)
        da.Fill(dsReceipt, mrptImage)
        da.Fill(dsReceipt, obj)
        da.Fill(dsReceipt, objSearch)
        myReport.SetDataSource(dsReceipt)

        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "Issue To Part Discard", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Function GetTitle() As String
        Dim mTransTypeList As TransactionList
        Dim mTitle As String
        mTransTypeList = TransactionList.GetTransactionList()
        mTransTypeID = 19

        mTransTypeList = TransactionList.GetTransactionList("Issue")     'Added By Prashant 24/09/07

        mTitle = mTransTypeList.GetTransactionTypeName(Util.Trans.DisacrdPart).ToString + " Register"

        If mTitle = "" Then
            Return "Issue To Part Discard Register "
        Else
            Return mTitle '"Issue Register (Detail Report)"
        End If
        Return mTitle
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "rptIssueToPartDiscardFetchExportToExcel"
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("@FromDate", Fromdate)
        cmd.Parameters.AddWithValue("@ToDate", ToDate)
        cmd.Parameters.AddWithValue("@StoreName", Store)
        cmd.Parameters.AddWithValue("@VendorName", Supplier)
        cmd.Parameters.AddWithValue("@StatusID", Val(cmbStatus.SelectedValue))
        cmd.Parameters.AddWithValue("@SerialNo", SerialNo)
        cmd.Parameters.AddWithValue("@RelNoteNo", ReleaseNoteNo)
        cmd.Parameters.AddWithValue("@ItemName", PartNo)
        cmd.Parameters.AddWithValue("@Description", Description)
        cmd.Parameters.AddWithValue("@ToStoreID", New Guid(cmbStore.SelectedValue.ToString))
        cmd.Parameters.AddWithValue("@TransTypeID", Util.Trans.DisacrdPart)
        cmd.Parameters.AddWithValue("@IsSerializedOnly", IIf(optLandscape.Checked And optSerialized.Checked, True, False))
        cmd.Parameters.AddWithValue("@SupplierID", New Guid(cmbSupplier.SelectedValue.ToString))
        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        dataTable.Columns.Remove("Rem1")
        dataTable.Columns.Remove("Rem2")
        dataTable.Columns.Remove("Rem3")
        dataTable.Columns.Remove("Rem4")
        dataTable.Columns.Remove("Rem5")

        'dataTable.TableName = "MainReport"
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable)
        'SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsIssue

        Dim objSearch As rptSearchingCriteriaForReceipt
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), Fromdate, ToDate, "", _
                                                                                  ReleaseNoteNo, "", "", "", "", "", "", "", Supplier, Store, Status, IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, ""), _
                                                                                  PartNo, Description, "", "", Store, Title, "", "", SerialNo, "", "", "", _
                                                                                  "", 19, "", Today.Date.ToString(AppSettings("DateFormat")), _
                                                                                  AppSettings("Logo"))


        ds.Clear()
        da.Fill(ds, objSearch)

        Dim columnToRemove As String() = {"ID", "CompanyName", "InternalReceiptNo", "RecText", "IssText", "OrdText", "RecNo", "IssNo", "OrdNo", "Aircraft", "InvText", "InvNo", "FromStore", "Amend", "QuotationNo", "IntOrderNo", "Charge", "SuppInvNo", "FromInvDate", "ToInvDate", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "WorkShop", "WorkOrderNo", "WorkOrderText"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("rptSearchingCriteriaForReceipt").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("rptSearchingCriteriaForReceipt").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("rptSearchingCriteriaForReceipt"))
        dsNew.Merge(tbl)


        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("Supplier").ColumnName = "Vendor"
        dsNew.Tables("rptSearchingCriteriaForReceipt").Columns("DCNo").ColumnName = "Supplier"

        dsNew.Tables("rptSearchingCriteriaForReceipt").TableName = "Searching Criteria"
        dsNew.Tables("TMainReport").TableName = "Issue To Part Discard Register"
		Session("ExcelFileName") = "Issue To Part Discard Register"
		Session("dsNew") = dsNew
        'Session("DataTable") = tbl
        'Session("ReportName") = "RCI Register"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "Issue To Part Discard", "Export To excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()  'Added By Prashant 30-Apr-2012 'ALL29042013
        mStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList

        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        mVendorList = VendorList.GetVendorstList(0, , , , , , "(All)", , IsSupplier:=True)
        cmbSupplier.DataSource = mVendorList

        DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            DataFieldBind() 'Added By Prashant 30-Apr-2012 'ALL29042013
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
        ControlVisibility4()
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReports()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub optLandscape_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optLandscape.CheckedChanged
        ControlVisibility4()
    End Sub
    Private Sub optPortrait_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPortrait.CheckedChanged
        ControlVisibility4()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If Page.IsValid Then
            SetValues()
            GenerateXLSXFile(CreateDataTable())
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
#End Region


End Class