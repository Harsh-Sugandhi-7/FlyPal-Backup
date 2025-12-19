Public Class wfrptHoursCorrection_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mMachineNameValueList As MachineNameValueList
    Dim StartDate As String
    Dim MachineName As String
    Dim MachineID As Guid
    Dim Aircraft As String
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptHoursCorrection_Ajax.aspx" Then
            Session.Remove("mMachineNameValueList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtDate.Text.Trim) Then
            StartDate = ""
        Else
            StartDate = txtDate.Text.Trim
        End If
        MachineID = New Guid(Request.Form("cmbAircraft").ToString)
        Aircraft = IIf(MachineID.Equals(Guid.Empty), "", mMachineNameValueList(MachineID).RegNo)
        lblDateRangeFrom.Text = "Date : " & IIf(StartDate <> "", StartDate, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblAircraft1.Text
    End Sub
    Private Sub ResetValues()
        StartDate = txtDate.Text.Trim
        MachineID = Guid.Empty
        Aircraft = ""
    End Sub
    Private Sub SetReport()
        Dim ReportName As String = ""
        SetValues()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim String3 As String = String.Empty
        Dim String4 As String = String.Empty
        Dim String5 As String = String.Empty
        Dim mHoursCorrection As HoursCorrection
        Dim dsHoursCorrection As New dsHoursCorrection
        myReport = New crptHoursCorrection

        mHoursCorrection = HoursCorrection.GetHoursCorrection(MachineID.ToString, StartDate)

        If mHoursCorrection.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'ElseIf mHoursCorrection(0).RecordsCount = 0 Then
            '    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no log added for this date", MsgBoxStyle.OkOnly, "")
            '    Exit Sub
        Else
            String3 = mHoursCorrection(0).CurrenctLogPageNo
            String4 = mHoursCorrection(0).PreviousLogDateFormatted
            String5 = mHoursCorrection(0).PreviousLogPageNo
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1289)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "", StartDate, Aircraft, String3, String4, String5, AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        Dim mrptImage As rptImage = rptImage.GetImage(dsHoursCorrection)
        da.Fill(dsHoursCorrection, mHoursCorrection)
        da.Fill(dsHoursCorrection, Report)
        da.Fill(dsHoursCorrection, mrptImage)
        myReport.SetDataSource(dsHoursCorrection)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "HoursCorrection", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
#End Region

#Region "events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptHoursCorrection_Ajax.aspx"
            ResetValues()
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineNameValueList = Nothing
        Session("MiddleFrame") = ""
        Session.Remove("mMachineNameValueList")
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

End Class