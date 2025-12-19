Partial Class wfrptPurchaseConsumption_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mPurchaseConsumption As PurchaseConsumption
    Public FromDate As String
    Public ToDate As String
    Public mCategoryList As CategoryList
    Public strCategory As String
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    Dim email As Thread
    Public PartNo As String = ""
    Public Description As String = ""
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = Session("mCategoryList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mCategoryList") = mCategoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mCategoryList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblCategory1.Visible = True
        'lblReceiptType.Visible = True
        'lblModel1.Visible = True
        'lblStoreName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        'lblSupp.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText
        strCategory = String.Empty

        For i As Integer = 0 To ChklistCategory.Items.Count - 1
            If ChklistCategory.Items(i).Selected Then
                If strCategory.Length = 0 Then
                    strCategory = ChklistCategory.Items(i).Text
                Else
                    strCategory = strCategory + "," + ChklistCategory.Items(i).Text
                End If
            End If
        Next
        lblCategory1.Text = "Category Name : " & IIf(strCategory.Length > 0, strCategory, "All")
        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + ", " + lblCategory1.Text + ", " + PartNo + ", " + Description
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            'Session("IsExcel") = IsExcel
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

            SetValues()
            Dim Value As String = ""
            Dim ReportName As String = ""

            myReport = New crptPurchaseConsumption
            mPurchaseConsumption = PurchaseConsumption.GetPurchaseConsumption(FromDate:=FromDate, ToDate:=ToDate, PartName:=PartNo, Description:=Description, _
                                                                              CategoryList:=strCategory)
            If ByMail = False Then
                If mPurchaseConsumption.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1417)
                End If
            End If
            If (ByMail = True And mPurchaseConsumption.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                              SmtpHost:=mModuleList.Item("PurchaseConsumption").SmtpHost, SmtpPort:=mModuleList.Item("PurchaseConsumption").SmtpPort, _
                                              SmtpUser:=mModuleList.Item("PurchaseConsumption").SmtpUser, SmtpPassword:=mModuleList.Item("PurchaseConsumption").SmtpPassword)
                Exit Sub
            End If
            Dim mCompanyDetail As New CompanyDetail
            Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, _
                                          mCompanyDetail.Email, website:="", ReportName:="Purchase Consumption", SearchStr1:=FromDate, SearchStr2:=ToDate, SearchStr3:=strCategory, _
                                          SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
                                          SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"), _
                                          SearchStr11:="", SearchStr12:="", SearchStr13:="", SearchStr14:="", SearchStr15:="", searchstr16:="")
            Dim ds As New dsPurchaseConsumption
           
            If IsExcel = False Then
                ds.Clear()
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, mrptImage)
                da.Fill(ds, mPurchaseConsumption)
                da.Fill(ds, mReport)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport

                If ByMail = False Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
                    MarkLog(Util.Action.Print, "PurchaseConsumption", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                Else
                    SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Purchase Consumption", "Purchase Consumption", " For " + lblDateRangeFrom.Text, "", _
                                              Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                              ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                              SmtpHost:=mModuleList.Item("PurchaseConsumption").SmtpHost, SmtpPort:=mModuleList.Item("PurchaseConsumption").SmtpPort, _
                                              SmtpUser:=mModuleList.Item("PurchaseConsumption").SmtpUser, SmtpPassword:=mModuleList.Item("PurchaseConsumption").SmtpPassword)
                End If
            Else
                ds.Clear()
                da.Fill(ds, mReport)
                da.Fill(ds, "PurchaseConsumption", mPurchaseConsumption)

                Dim columnToRemove2 As String() = {"SearchStr4", "ReportName", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "ShortName", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr5", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                    End If
                Next

                Dim columnToRemove As String() = {"DisplayQty", "ItemID", "CategoryID", "InvoiceItemQty", "ReceiptText", "ReceiptNo", "ReceiptDate", "IssueText", "IssueNo", "IssueDate"}

                For i As Integer = 0 To columnToRemove.Length - 1
                    If ds.Tables("PurchaseConsumption").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("PurchaseConsumption").Columns.Remove(columnToRemove(i))
                    End If
                Next

                If ds.Tables("PurchaseConsumption").Columns.Contains("ItemName") Then
                    ds.Tables("PurchaseConsumption").Columns("ItemName").ColumnName = "Part No."
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("ReceiptNumber") Then
                    ds.Tables("PurchaseConsumption").Columns("ReceiptNumber").ColumnName = "Receipt Number"
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("ReceiptDateFormatted") Then
                    ds.Tables("PurchaseConsumption").Columns("ReceiptDateFormatted").ColumnName = "Receipt Date"
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("ReceiptItemQty") Then
                    ds.Tables("PurchaseConsumption").Columns("ReceiptItemQty").ColumnName = "In Qty."
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("IssueNumber") Then
                    ds.Tables("PurchaseConsumption").Columns("IssueNumber").ColumnName = "Issue Number"
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("IssueDateFormatted") Then
                    ds.Tables("PurchaseConsumption").Columns("IssueDateFormatted").ColumnName = "Issue Date"
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("IssueItemQty") Then
                    ds.Tables("PurchaseConsumption").Columns("IssueItemQty").ColumnName = "Out Qty."
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("EffRate") Then
                    ds.Tables("PurchaseConsumption").Columns("EffRate").ColumnName = "Rate"
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("IssuedAmount") Then
                    ds.Tables("PurchaseConsumption").Columns("IssuedAmount").ColumnName = "Issued Amount"
                End If
                If ds.Tables("PurchaseConsumption").Columns.Contains("CategoryName") Then
                    ds.Tables("PurchaseConsumption").Columns("CategoryName").ColumnName = "Category"
                End If

                If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                    ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                    ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                    ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Category"
                End If

                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("ReportData"))
                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Merge(ds.Tables("PurchaseConsumption"))
                dsNew.Tables("PurchaseConsumption").TableName = "Purchase Consumption"
				Session("ExcelFileName") = "Purchase Consumption"

				Session("dsNew") = dsNew
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                'Added by Prashant on 19-Jan-2021
                MarkLog(Util.Action.Print, "PurchaseConsumption", "Export To Excel " + mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
         Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (SetReport Sub Method): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList()
        ChklistCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList
       
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False, False)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(False, True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        If IsValid Then

            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
            Session("UserEmailID") = mModuleList.Item("PurchaseConsumption").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("PurchaseConsumption").SendCCMailID
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
    End Sub
#End Region


End Class