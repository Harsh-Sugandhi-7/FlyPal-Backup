Public Class wfrptConsumptionReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "

    Public mCategoryList As CategoryList
    Public mAircraft As Machine
    Public mCategory As Category
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String
    Public Description As String
    Public StrAircraft As String
    Public StrCategory As String
    Public mMachineNameValueList As MachineNameValueList  'Added By Utkarsh ON 11-May-2012 FOR 11052012-4
    Dim mAircraftwiseConsumptionSearchingCriteria As String = String.Empty
    Dim aircraftlist As String = ""
    Dim mText As String = ""
    Dim email As Thread
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Public mStoreList As StoreList
    Public mnDistinctWOText As nDistinctWOText
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCategoryList = CType(Session("mCategoryList"), CategoryList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList) 'Added By Utkarsh ON 11-May-2012 FOR 11052012-4
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
        Session.Remove("mMachineNameValueList")  'Added By Utkarsh ON 11-May-2012 FOR 11052012-4
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub ControlvisibilityForDateCriteria(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
        lblDateRangeFrom.Visible = False
        upnlDateCriteria.Update()

    End Sub
    Private Sub ControlvisibilityForSearchingCriteria(ByVal showlabel As Boolean)
        lblDateRangeFrom.Visible = showlabel
        lblPartNo.Visible = showlabel
        lblDesc.Visible = showlabel
        lblAircraftName.Visible = showlabel
        lblCategoryName.Visible = showlabel
        upnlSearchingCriteria.Update()
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range  : All"
        Else
            FromDate = txtFromDate.Text.Trim
            ToDate = txtToDate.Text.Trim
            lblDateRangeFrom.Text = "Date Range  : " & New SmartDate(txtFromDate.Text).FormattedText & " To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If (txtSearch.Text.Trim.IndexOf("[") >= 0 AndAlso txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

        'Added By Utkarsh On 11-May-2012 FOR 11052012-4
        aircraftlist = hdnAircraftList.Value
        StrAircraft = IIf(aircraftlist = String.Empty, String.Empty, aircraftlist)
        lblAircraftName.Text = "Aircraft Name : " & IIf(StrAircraft.Length > 0, StrAircraft, "All")
        'End
        StrCategory = IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.Text, "")
        Session("mAircraft") = mAircraft
        Session("mCategory") = mCategory
        mAircraftwiseConsumptionSearchingCriteria = lblDateRangeFrom.Text + ", " + lblAircraftName.Text + ", " + lblCategoryName.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text + ", "
        If chkHighValue.Checked And txtCEffectiveRate.Text <> "" Then 'Added By Prashant 14-Aug-2014 For ALL14082014
            mText = "Report shows valued parts with landing rate greater than  " + txtCEffectiveRate.Text
        Else
            mText = ""
        End If
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            Session("IsExcel") = IsExcel
            SetValues()
          
            If StrAircraft.Split(",").Length > 7 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Select only 7 aircrafts from list ", False), True)
                Exit Sub
            End If
            
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim objSearch As rptSearchingCriteria
            Dim ds As New dsAircraftConsumption
            Dim rpt As rptAircraftwiseConsumption
            Dim mailText As String = ""

           If CmbSortBy.SelectedIndex = 0 Then
                myReport = New crptAircraftConsumptionAircraftWise
                mailText = "Aircraftwise Consumption Report"
            ElseIf CmbSortBy.SelectedIndex = 1 Then
                myReport = New crptAircraftConsumptionWorkOrderWise
                mailText = "Work Order wise Consumption Report"
            ElseIf CmbSortBy.SelectedIndex = 2 Then
                myReport = New crptAircraftConsumptionCategoryWise
                mailText = "Categorywise Consumption Report"
            ElseIf CmbSortBy.SelectedIndex = 3 Then
                myReport = New crptAircraftConsumptionStoreWise
                mailText = "Storewise Consumption Report"
            End If
         

            rpt = rptAircraftwiseConsumption.GetAircraftConsumption(FromDate, ToDate, StrAircraft, StrCategory, PartNo, Description, chkIsValued.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IIf(cmbWO.SelectedIndex = 0, "", cmbWO.SelectedItem.ToString), IIf(txtWONo.Text = "", 0, Val(txtWONo.Text)), IIf(cmbStore.SelectedIndex = 0, Guid.Empty.ToString, cmbStore.SelectedValue.ToString))  'Changed By Vikrant 3-Jan-2012 For ALL13122011-1
            objSearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, "", "", StrCategory, "", "", StrAircraft, "", Description, "", 0, IIf(cmbStore.SelectedIndex = 0, "", cmbStore.SelectedItem.Text), "", mText, AppSettings("Logo"))   'Changed By Utkarsh For Report Logo.
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 710)
                End If
            End If
            If (ByMail = True And rpt.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, mailText, mailText, "There is no record for this search criteria.", _
                    "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("GroupWiseConsumptionReport").SmtpHost, SmtpPort:=mModuleList.Item("GroupWiseConsumptionReport").SmtpPort, SmtpUser:=mModuleList.Item("GroupWiseConsumptionReport").SmtpUser, SmtpPassword:=mModuleList.Item("GroupWiseConsumptionReport").SmtpPassword)

                Exit Sub
            End If

            ds.Clear()
            '-----------Added by Utkarsh for Report Logo---------------
            If IsExcel = False Then
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, mrptImage)
            End If
            '----------------------------------------------------------
            da.Fill(ds, rpt)
            da.Fill(ds, objSearch)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport
            MarkLog(Util.Action.Print, "GroupWiseConsumptionReport", mAircraftwiseConsumptionSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, mailText, mailText, " For " + lblDateRangeFrom.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                    SmtpHost:=mModuleList.Item("GroupWiseConsumptionReport").SmtpHost, SmtpPort:=mModuleList.Item("GroupWiseConsumptionReport").SmtpPort, SmtpUser:=mModuleList.Item("GroupWiseConsumptionReport").SmtpUser, SmtpPassword:=mModuleList.Item("GroupWiseConsumptionReport").SmtpPassword)

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
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub addAttributes()
        txtCEffectiveRate.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtCEffectiveRate').value,event)")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCategoryList = CategoryList.GetCategoryList("(All)")
        cmbCategory.DataSource = mCategoryList
        Session("mCategoryList") = mCategoryList

        'Added By Utkarsh ON 11-May-2012 FOR 11052012-4
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString)
        Session("mMachineNameValueList") = mMachineNameValueList
        ChklistAircraft.DataSource = mMachineNameValueList
        'End

        'Added by Shital on 06-MAy-2021
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        mnDistinctWOText = nDistinctWOText.GetDistinctWOText("(SELECT)")
        cmbWO.DataSource = mnDistinctWOText
        Session("mnDistinctWOText") = mnDistinctWOText
        '   ------------
        DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            DataFieldBind()
            ControlvisibilityForDateCriteria(6)
            setDatePeroid(6)
            ControlvisibilityForSearchingCriteria(False)
            cmbDateRange.SelectedIndex = 6
            If CmbSortBy.SelectedIndex = 0 Then
                ChklistAircraft.Enabled = True
                chkSelectAllAircraft1.Enabled = True
            End If

        End If

    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlvisibilityForDateCriteria(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport(False, False)
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
        aircraftlist = hdnAircraftList.Value
        StrAircraft = IIf(aircraftlist = String.Empty, String.Empty, aircraftlist)

        If StrAircraft.Split(",").Length > 7 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("Select only 7 aircrafts from list ", False), True)
            Exit Sub
        End If



        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        Session("UserEmailID") = mModuleList.Item("AircraftConsumption").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AircraftConsumption").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsAircraftConsumption
        Dim rpt As rptAircraftwiseConsumption

        SetValues()
        rpt = rptAircraftwiseConsumption.GetAircraftConsumption(FromDate, ToDate, StrAircraft, StrCategory, PartNo, Description, chkIsValued.Checked, chkHighValue.Checked, CDec(Val(txtCEffectiveRate.Text)), IIf(cmbWO.SelectedIndex = 0, "", cmbWO.SelectedItem.ToString), IIf(txtWONo.Text = "", 0, Val(txtWONo.Text)), IIf(cmbStore.SelectedIndex = 0, Guid.Empty.ToString, cmbStore.SelectedValue.ToString))  'Changed By Vikrant 3-Jan-2012 For ALL13122011-1

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ds.Clear()
        da.Fill(ds, "ExcelrptAircraftwiseConsumption", rpt)

        Dim columnToRemove As String() = {"CategoryTotal", "ScheduleTotal", "UnScheduleTotal", "OthersTotal", "TotalAmount", "RequisitionItemTypeName", "RequisitionItemTypeID", "Owner", "BalanceQty", "Items"}
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("ExcelrptAircraftwiseConsumption").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("ExcelrptAircraftwiseConsumption").Columns.Remove(columnToRemove(i))
            End If
        Next

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ExcelrptAircraftwiseConsumption"))
        dsNew.Tables("ExcelrptAircraftwiseConsumption").TableName = "Aircraft Consumption"
		Session("ExcelFileName") = "Aircraft Consumption"
		Session("dsNew") = dsNew

		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "AircraftConsumption", "Export To excel " + mAircraftwiseConsumptionSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mCategoryList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlvisibilityForSearchingCriteria(True)
        SetValues()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub chkHighValue_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkHighValue.CheckedChanged 'Added By Prashant 14-Aug-2014 For ALL14082014
        If chkHighValue.Checked = True Then
            txtCEffectiveRate.Enabled = True
        Else
            txtCEffectiveRate.Enabled = False
            txtCEffectiveRate.Text = ""
        End If
        upnlHighValue.Update()
    End Sub
    Private Sub CmbSortBy_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles CmbSortBy.SelectedIndexChanged
        If CmbSortBy.SelectedIndex = 0 Then
            ChklistAircraft.Enabled = True
            chkSelectAllAircraft1.Enabled = True
            cmbWO.Enabled = False
            txtWONo.Enabled = False
            cmbCategory.Enabled = False
            ''cmbStore.Enabled = False
        ElseIf CmbSortBy.SelectedIndex = 1 Then
            cmbWO.Enabled = True
            txtWONo.Enabled = True
            cmbCategory.Enabled = False
            ''cmbStore.Enabled = False
            ChklistAircraft.Enabled = False
            chkSelectAllAircraft1.Enabled = False
            chkSelectAllAircraft1.Checked = False
        ElseIf CmbSortBy.SelectedIndex = 2 Then
            cmbCategory.Enabled = True
            cmbWO.Enabled = False
            txtWONo.Enabled = False
            ''cmbStore.Enabled = False
            ChklistAircraft.Enabled = False
            chkSelectAllAircraft1.Enabled = False
            chkSelectAllAircraft1.Checked = False
        ElseIf CmbSortBy.SelectedIndex = 3 Then
            ''cmbStore.Enabled = True
            cmbCategory.Enabled = False
            cmbWO.Enabled = False
            txtWONo.Enabled = False
            ChklistAircraft.Enabled = False
            chkSelectAllAircraft1.Enabled = False
            chkSelectAllAircraft1.Checked = False
        End If
        DataFieldBind()
        upnlCategory.Update()
        upnlWono.Update()
        unplAircraftlist.Update()
        upnlchkAllAircraft.Update()
    End Sub
#End Region



End Class
