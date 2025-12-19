Public Class wfrptInventoryMovementStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim objsearch As rptSearchingCriteria
    Public mCategoryLists As CategoryList
    Dim EventLogID As Guid
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
#End Region

#Region " Business Methods "
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("Table")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim i As Integer = 3
        Dim mYear As Integer = 0
        Dim con = New SqlConnection(conString)
        While i > 0
            'For i As Integer = 3 To 1
            con.Open()

            '---------
            Dim currentdateqtrenddate As Date
            Dim Qtr As Integer = (CDate(txtDate.Text).Month - 1) \ 3 + 1

            If Qtr = 1 Then currentdateqtrenddate = DateSerial(Year(CDate(txtDate.Text)), 3, 31)
            If Qtr = 2 Then currentdateqtrenddate = DateSerial(Year(CDate(txtDate.Text)), 6, 30)
            If Qtr = 3 Then currentdateqtrenddate = DateSerial(Year(CDate(txtDate.Text)), 9, 30)
            If Qtr = 4 Then currentdateqtrenddate = DateSerial(Year(CDate(txtDate.Text)), 12, 31)

            '--------

            mYear = Year(CDate(txtDate.Text)) - i

            Dim mClosingDate As Date
            Dim mQuarter1LastDate As Date
            Dim mQuarter2LastDate As Date
            Dim mQuarter3LastDate As Date
            Dim mQuarter4LastDate As Date

            mClosingDate = DateSerial(mYear, 12, 31)
            mQuarter1LastDate = DateSerial(mYear + 1, 3, 31)
            mQuarter2LastDate = DateSerial(mYear + 1, 6, 30)
            mQuarter3LastDate = DateSerial(mYear + 1, 9, 30)
            mQuarter4LastDate = DateSerial(mYear + 1, 12, 31)

            Dim Q As Integer = (CDate(txtDate.Text).Month - 1) \ 3 + 1

            Dim Q1 As Integer = (mQuarter1LastDate.Month - 1) \ 3 + 1
            Dim Q2 As Integer = (mQuarter2LastDate.Month - 1) \ 3 + 1
            Dim Q3 As Integer = (mQuarter3LastDate.Month - 1) \ 3 + 1
            Dim Q4 As Integer = (mQuarter4LastDate.Month - 1) \ 3 + 1

            Dim cmd As New SqlCommand()
            cmd.Parameters.Clear()
            cmd.Connection = con
            cmd.CommandText = "InventoryMovementStatusByPivotFetch"
            cmd.Parameters.AddWithValue("@Year", mYear)
            cmd.Parameters.AddWithValue("@ClosingDate", mClosingDate)

            'If Month(mQuarter1LastDate) <= 3 Then
            '    cmd.Parameters.AddWithValue("@Quarter1LastDate", mQuarter1LastDate)
            'Else
            '    cmd.Parameters.AddWithValue("@Quarter1LastDate", DBNull.Value)
            'End If

            'If Month(mQuarter2LastDate) > 3 And Month(mQuarter2LastDate) <= 6 Then
            '    cmd.Parameters.AddWithValue("@Quarter2LastDate", mQuarter2LastDate)
            'Else
            '    cmd.Parameters.AddWithValue("@Quarter2LastDate", DBNull.Value)
            'End If

            'If Month(mQuarter3LastDate) > 6 And Month(mQuarter3LastDate) <= 9 Then
            '    cmd.Parameters.AddWithValue("@Quarter3LastDate", mQuarter3LastDate)
            'Else
            '    cmd.Parameters.AddWithValue("@Quarter3LastDate", DBNull.Value)
            'End If

            'If Month(mQuarter4LastDate) > 9 And Month(mQuarter4LastDate) <= 12 Then
            '    cmd.Parameters.AddWithValue("@Quarter4LastDate", mQuarter4LastDate)
            'Else
            '    cmd.Parameters.AddWithValue("@Quarter4LastDate", DBNull.Value)
            'End If

            If mQuarter1LastDate <= currentdateqtrenddate Then cmd.Parameters.AddWithValue("@Quarter1LastDate", mQuarter1LastDate) Else cmd.Parameters.AddWithValue("@Quarter1LastDate", "")
            If mQuarter2LastDate <= currentdateqtrenddate Then cmd.Parameters.AddWithValue("@Quarter2LastDate", mQuarter2LastDate) Else cmd.Parameters.AddWithValue("@Quarter2LastDate", "")
            If mQuarter3LastDate <= currentdateqtrenddate Then cmd.Parameters.AddWithValue("@Quarter3LastDate", mQuarter3LastDate) Else cmd.Parameters.AddWithValue("@Quarter3LastDate", "")
            If mQuarter4LastDate <= currentdateqtrenddate Then cmd.Parameters.AddWithValue("@Quarter4LastDate", mQuarter4LastDate) Else cmd.Parameters.AddWithValue("@Quarter4LastDate", "")


            cmd.Parameters.AddWithValue("@CategoryID", cmbCategory.SelectedValue.ToString)
            cmd.CommandType = CommandType.StoredProcedure

            Dim adaptor = New SqlDataAdapter

            adaptor.SelectCommand = cmd
            adaptor.Fill(dataTable)

            con.Close()
            i = i - 1
        End While
        'Next
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable)
        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(tbl)
        Dim mYear As Integer = 0
        Dim columnToRemove As String() = {"Type"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If dsNew.Tables("Table").Columns.Contains(columnToRemove(i)) Then
                dsNew.Tables("Table").Columns.Remove(columnToRemove(i))
            End If
        Next
        mYear = Year(CDate(txtDate.Text))
        dsNew.Tables("Table").Rows(0).Item(0) = "Closing FY " + Str(mYear - 3)
        dsNew.Tables("Table").Rows(1).Item(0) = "1st Qtr FY " + Str(mYear - 2)
        dsNew.Tables("Table").Rows(3).Item(0) = "2nd Qtr FY " + Str(mYear - 2)
        dsNew.Tables("Table").Rows(5).Item(0) = "3rd Qtr FY " + Str(mYear - 2)
        dsNew.Tables("Table").Rows(7).Item(0) = "4th Qtr FY " + Str(mYear - 2)

        dsNew.Tables("Table").Rows(10).Item(0) = "Fiscal Year " + Str(mYear - 2) + "/" + Str(mYear - 1)
        'dsNew.Tables("Table").Rows(10).Item(0).Style.Font.Bold = True
        dsNew.Tables("Table").Rows(11).Item(0) = "Closing FY " + Str(mYear - 2)
        dsNew.Tables("Table").Rows(12).Item(0) = "1st Qtr FY " + Str(mYear - 1)
        dsNew.Tables("Table").Rows(14).Item(0) = "2nd Qtr FY " + Str(mYear - 1)
        dsNew.Tables("Table").Rows(16).Item(0) = "3rd Qtr FY " + Str(mYear - 1)
        dsNew.Tables("Table").Rows(18).Item(0) = "4th Qtr FY " + Str(mYear - 1)

        dsNew.Tables("Table").Rows(21).Item(0) = "Fiscal Year " + Str(mYear - 1) + "/" + Str(mYear)
        'dsNew.Tables("Table").Rows(21).Item(0).Style.Font.Bold = True
        dsNew.Tables("Table").Rows(22).Item(0) = "Closing FY " + Str(mYear - 1)
        dsNew.Tables("Table").Rows(23).Item(0) = "1st Qtr FY " + Str(mYear)
        dsNew.Tables("Table").Rows(25).Item(0) = "2nd Qtr FY " + Str(mYear)
        dsNew.Tables("Table").Rows(27).Item(0) = "3rd Qtr FY " + Str(mYear)
        dsNew.Tables("Table").Rows(29).Item(0) = "4th Qtr FY " + Str(mYear)

		dsNew.Tables("Table").TableName = "INVENTORY MOVEMENT STATUS"
		Session("ExcelFileName") = "INVENTORY MOVEMENT STATUS"
		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        'If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
        '    For i As Integer = -10 To 10
        '        cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
        '    Next
        '    cmbYear.SelectedIndex = 10
        'End If
    End Sub
    Private Sub DataFieldBinding()
        mCategoryLists = CategoryList.GetCategoryList("(ALL)")
        cmbCategory.DataSource = mCategoryLists
        cmbCategory.DataBind()
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblCategory1.Visible = True
    End Sub
    Private Sub SetValues()
        lblyear1.Text = "As On Date : " & txtDate.Text
        lblCategory1.Text = "Category : " & IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "")
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Try
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim ds As New dsInventoryMovementStatus
            Dim mCompanyDetail As New CompanyDetail
            Dim mInventoryMovementStatus As InventoryMovementStatus
            myReport = New crptInventoryMovementStatus
            mInventoryMovementStatus = InventoryMovementStatus.GetInventoryMovementStatus(txtDate.Text, 0, cmbCategory.SelectedValue.ToString)
            If ByMail = False Then
                If mInventoryMovementStatus.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1365)
                End If
            End If
            If (ByMail = True And mInventoryMovementStatus.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, "Inventory Movement Status", "Inventory Movement Status", "There is no record for this search criteria.", _
                    "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                           SmtpHost:=mModuleList.Item("InventoryMovementStatus").SmtpHost, SmtpPort:=mModuleList.Item("InventoryMovementStatus").SmtpPort, SmtpUser:=mModuleList.Item("InventoryMovementStatus").SmtpUser, SmtpPassword:=mModuleList.Item("InventoryMovementStatus").SmtpPassword)

                Exit Sub
            End If
            objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", "", "", "", IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, ""), _
                                                                  "", "", "", Year(CDate(txtDate.Text)) - 1, "", Year(CDate(txtDate.Text)))
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, mInventoryMovementStatus)
            da.Fill(ds, objsearch)

            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            MarkLog(Util.Action.Print, "InventoryMovementStatus", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Inventory Movement Status", "Inventory Movement Status", _
                                          "Inventory Movement Status", "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, _
                                          Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                           SmtpHost:=mModuleList.Item("InventoryMovementStatus").SmtpHost, SmtpPort:=mModuleList.Item("InventoryMovementStatus").SmtpPort, SmtpUser:=mModuleList.Item("InventoryMovementStatus").SmtpUser, SmtpPassword:=mModuleList.Item("InventoryMovementStatus").SmtpPassword)

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

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtDate.Text = New SmartDate(Today.Date).FormattedText
            SetCombo()
            DataFieldBinding()
        End If
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("InventoryMovementStatus").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("InventoryMovementStatus").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            email = New Thread(Sub() SetReport(True))
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
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        GenerateXLSXFile(CreateDataTable())
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region


End Class