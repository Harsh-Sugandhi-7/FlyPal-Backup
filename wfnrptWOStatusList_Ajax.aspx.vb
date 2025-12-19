Public Class wfnrptWOStatusList
    Inherits System.Web.UI.Page



#Region "Data Binding"
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If

        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Try
            Dim da As New CSLA.Data.ObjectAdapter
            Dim mCompanyDetail As CompanyDetail
            Dim ReportName As String = String.Empty
            Dim ds As New dsWOMonthlyStatus
            ReportName = "WO Status Report"
            Dim TransType As Integer
            If cmbWOType.SelectedIndex = 0 Then
                TransType = 0
            ElseIf cmbWOType.SelectedIndex = 1 Then
                TransType = 89
            Else
                TransType = 88
            End If
            Dim mWOMonthlyStatusCountList As nWOMonthlyStatusCountList
            mWOMonthlyStatusCountList = nWOMonthlyStatusCountList.GetnWOMonthlyStatusCount(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), TransType)

            Dim CurrentMonthDate As SmartDate
            Dim LastMonthDate As SmartDate
            CurrentMonthDate = New SmartDate(CStr(DateAdd("d", -1, DateAdd("m", 1, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), CType(cmbMonth.SelectedIndex + 1, Integer), 1)))))

            LastMonthDate = New SmartDate(CStr(DateAdd("d", -1, DateAdd("m", 1, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), CType(cmbMonth.SelectedIndex + 1, Integer) - 1, 1)))))


            Dim myReport = New crptWOMonthlyStatusList

            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                                         mCompanyDetail.WebSite, "", AppSettings("ClientCode"), IIf(cmbWOType.SelectedIndex = 0, "All", cmbWOType.SelectedItem.ToString), _
                                          CurrentMonthDate.Date.ToString("MMM") + " - " + Year(CDate(CurrentMonthDate.ToString)).ToString, LastMonthDate.Date.ToString("MMM") + " - " + Year(CDate(LastMonthDate.ToString)).ToString, _
                                          "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
            'Dim Report As New ReportData(CompanyName:="", Address:="", Tel1:="", Tel2:="", Fax:="", Email:="", WebSite:="", ReportName:="", SearchStr1:="", SearchStr2:="", SearchStr3:="", SearchStr4:="", SearchStr5:="", ProductVersion:="", SINote:="", SearchStr6:="")

            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds, , "rptImage")
            da.Fill(ds, mrptImage)
            da.Fill(ds, "nWOMonthlyStatusCountList", mWOMonthlyStatusCountList)

            da.Fill(ds, Report)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)

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

#Region "Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            SetCombo()
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport(False)

        End If
    End Sub
#End Region


End Class