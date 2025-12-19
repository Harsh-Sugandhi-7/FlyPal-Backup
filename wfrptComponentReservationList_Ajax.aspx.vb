Public Class wfrptComponentReservationList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList
    Dim mComponentReservationListForReport As ComponentReservationListForReport
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String = ""
    Public Description As String = ""
    Dim mSearchCriteriaForEventLog As String = String.Empty
#End Region

#Region " DataFieldBind "
    Public Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(ALL)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
        mComponentReservationListForReport = Session("mComponentReservationListForReport")
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mComponentReservationListForReport") = mComponentReservationListForReport
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mComponentReservationListForReport")
    End Sub
    Private Sub SetValues()

        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text

        lblFromDate.Text = "From Date: " + New SmartDate(txtFromDate.Text.Trim).FormattedText
        lblToDate.Text = "To Date: " + New SmartDate(txtToDate.Text.Trim).FormattedText

        lblRegNo.Text = IIf(cmbAircraft.SelectedIndex = 0, "Reg. No.: " + "All", "Reg. No.: " + cmbAircraft.SelectedItem.Text)

        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If

        lblPartNo.Text = "Part No.: " + PartNo
        lblDesc.Text = "Description: " + Description
        lblSerailNo.Text = "Serail No.: " + txtSerialNo.Text.Trim

        mSearchCriteriaForEventLog = FromDate + "To" + ToDate + "," + PartNo + ", " + Description
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean, Optional ByVal ByMail As Boolean = False)
        Try
            Dim da As New CSLA.Data.ObjectAdapter
            Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
            Dim rpt As ComponentReservationListForReport
            SetValues()
            Dim mCompanyDetail As New CompanyDetail
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Value As String = ""
            Dim ReportName As String = ""
            ReportName = " Component Reservation Report"

            rpt = ComponentReservationListForReport.GetComponentReservationList(FromDate:=FromDate, ToDate:=ToDate, Description:=Description, _
                                                                                ItemName:=PartNo, SerialNo:=txtSerialNo.Text, _
                                                                                RegNo:=IIf(cmbAircraft.SelectedIndex = 0, "", cmbAircraft.SelectedItem.Text), _
                                                                                ForWhat:=cmbCriteria.SelectedValue)
            If ByMail = False Then
                If rpt.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1522)
                End If
            End If
            If cmbCriteria.SelectedValue = 0 Then
                myReport = New crptComponentReservationList
            ElseIf cmbCriteria.SelectedValue = 1 Or cmbCriteria.SelectedValue = 2 Then
                myReport = New crptUnscheduleIssuedComponentList
            End If
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                     mCompanyDetail.WebSite, ReportName, SearchStr1:=New SmartDate(txtFromDate.Text.Trim).FormattedText, _
                     SearchStr2:=New SmartDate(txtToDate.Text.Trim).FormattedText, _
                     SearchStr3:=IIf(cmbAircraft.SelectedIndex = 0, "", cmbAircraft.SelectedItem.Text), SearchStr4:=IIf(PartNo = "", "", PartNo + " [" + Description + "]"), _
                     SearchStr5:=txtSerialNo.Text.Trim, ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), _
                     SearchStr6:=cmbCriteria.SelectedItem.Text, SearchStr7:=AppSettings("ClientCode"), SearchStr8:=AppSettings("Government Authority"), _
                     SearchStr9:="", SearchStr10:=AppSettings("Logo"))

            Dim ds As New dsPartPurchaseStatementList
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            If IsExcel = False Then
                da.Fill(ds, mrptImage)
            End If
            da.Fill(ds, rpt)
            da.Fill(ds, Report)
            myReport.SetDataSource(ds)
            Session("CrystalReport") = myReport

            MarkLog(Util.Action.Print, "PartPurchaseStatementList", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                'SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblDateRangeFrom.Text, "", _
                '                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                '                          ReportGenratedBy:=Session("ReportGenratedBy"), _
                '                          SmtpHost:=mModuleList.Item("PurchaseStatement").SmtpHost, SmtpPort:=mModuleList.Item("PurchaseStatement").SmtpPort, _
                '                          SmtpUser:=mModuleList.Item("PurchaseStatement").SmtpUser, SmtpPassword:=mModuleList.Item("PurchaseStatement").SmtpPassword)
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

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptComponentReservationList_Ajax.aspx"
            txtFromDate.Text = CDate(Today.AddMonths(-3)).ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = CDate(Today.AddMonths(3)).ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False, False)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnCloseTop_Click(sender As Object, e As System.EventArgs) Handles btnCloseTop.Click
        mComponentReservationListForReport = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(sender As Object, e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        lblSummary.Visible = True
        lblFromDate.Visible = True
        lblToDate.Visible = True
        lblRegNo.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblSerailNo.Visible = True
        upnlCriteria.Update()
    End Sub
#End Region

End Class