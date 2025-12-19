Public Class wfrptAircraftUtilizationGraph_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mModelList As ModelList
    Public mAircraftUtilizationByHoursCycles As AircraftUtilizationByHoursCycles
    Public mAircraftUtilizationByFlyingDays As AircraftUtilizationByFlyingDays
    Dim ToDate As String
    Dim AircraftName As String
    Dim AircraftIds As String
    Public mMachineNameValueList As MachineNameValueList

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid

    Dim mPeriodParameter As PeriodParamater
#End Region

#Region " Enumeration "
    Enum PeriodParamater
        TimeInAir = 1
        BlockTime = 2
        Cycles = 3
        Landings = 4
    End Enum
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub

    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub Display()

        If TabContainer1.ActiveTabIndex = 0 Then
            lblyear1.Visible = True
            lblAircraft1.Visible = True
            lblGraphType.Visible = True
        ElseIf TabContainer1.ActiveTabIndex = 1 Then
            lblAircraftGraphII1.Visible = True
            lblGraphTypeGraphII.Visible = True
            lblDates1.Visible = True
        ElseIf TabContainer1.ActiveTabIndex = 2 Then 'GEPL
            lblAircraftGraphIII.Visible = True
            lblDatesGIII.Visible = True
        End If

    End Sub
    Private Sub SetValues()
        ToDate = cmbYear.SelectedItem.Text
        AircraftName = String.Empty
        AircraftIds = String.Empty
        For i As Integer = 0 To ChklistAircraft.Items.Count - 1
            If ChklistAircraft.Items(i).Selected Then
                If AircraftName.Length = 0 Then
                    AircraftName = ChklistAircraft.Items(i).Text
                    AircraftIds = ChklistAircraft.Items(i).Value
                Else
                    AircraftName = AircraftName + "," + ChklistAircraft.Items(i).Text
                    AircraftIds = AircraftIds + "," + ChklistAircraft.Items(i).Value
                End If
            End If
        Next
        lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        lblAircraft1.Text = "Aircraft : " & IIf(IsNothing(AircraftName), "", AircraftName)
        lblGraphType.Text = "Graph Report : " & IIf(rdoByFlyingDay.Checked, "By Flying Days", "By Flying Hours")

        mCompleteSearchingCriteria = "Graph I " + lblyear1.Text + ", " + lblAircraft1.Text + ", " + lblGraphType.Text
    End Sub
    Private Sub SetValuesGraphII()

        AircraftName = String.Empty
        AircraftIds = String.Empty
        For i As Integer = 0 To ChklistAircraftGraphII.Items.Count - 1
            If ChklistAircraftGraphII.Items(i).Selected Then
                If AircraftName.Length = 0 Then
                    AircraftName = ChklistAircraftGraphII.Items(i).Text
                    AircraftIds = ChklistAircraftGraphII.Items(i).Value
                Else
                    AircraftName = AircraftName + "," + ChklistAircraftGraphII.Items(i).Text
                    AircraftIds = AircraftIds + "," + ChklistAircraftGraphII.Items(i).Value
                End If
            End If
        Next

        Select Case cmbPeriod.SelectedIndex
            Case 0
                If rdoTimeInair.Checked Then
                    mPeriodParameter = PeriodParamater.TimeInAir
                ElseIf rdoBlockTime.Checked Then
                    mPeriodParameter = PeriodParamater.BlockTime
                End If
            Case 1
                mPeriodParameter = PeriodParamater.Cycles
            Case 2
                mPeriodParameter = PeriodParamater.Landings
        End Select

        lblDates1.Text = "From Date : " & IIf((txtStartDate.Text <> "" And txtEndDate.Text <> ""), txtStartDate.Text + " To Date " + txtEndDate.Text, "")
        lblAircraftGraphII1.Text = "Aircraft : " & IIf(IsNothing(AircraftName), "", AircraftName)
        lblGraphTypeGraphII.Text = "Graph Parameter : " & IIf(cmbPeriod.SelectedIndex >= 0, cmbPeriod.SelectedItem.Text + IIf(cmbPeriod.SelectedIndex = 0, IIf(rdoTimeInair.Checked, " (By Airborne Time)", " (By Block Time)"), ""), "")

        mCompleteSearchingCriteria = "Graph II " + lblDates1.Text + ", " + lblAircraftGraphII1.Text + ", " + lblGraphTypeGraphII.Text
    End Sub
    Private Sub SetValuesGraphIII()
        ToDate = cmbFrmYear.SelectedItem.Text
        AircraftName = String.Empty
        AircraftIds = String.Empty
        For i As Integer = 0 To ChklistAircraftGraphIII.Items.Count - 1
            If ChklistAircraftGraphIII.Items(i).Selected Then
                If AircraftName.Length = 0 Then
                    AircraftName = ChklistAircraftGraphIII.Items(i).Text
                    AircraftIds = ChklistAircraftGraphIII.Items(i).Value
                Else
                    AircraftName = AircraftName + "," + ChklistAircraftGraphIII.Items(i).Text
                    AircraftIds = AircraftIds + "," + ChklistAircraftGraphIII.Items(i).Value
                End If
            End If
        Next
        lblDatesGIII.Text = "From Month and Year : " & IIf((cmbFrmYear.SelectedIndex >= 0 And cmbFrmMonth.SelectedIndex >= 0), cmbFrmMonth.SelectedItem.Text + " " + cmbFrmYear.SelectedItem.Text, "") + " Till " '+ Month( + " " + cmbFrmYear.SelectedItem.Text
        lblAircraftGraphIII.Text = "Aircraft : " & IIf(IsNothing(AircraftName), "", AircraftName)

        mCompleteSearchingCriteria = "Graph III " + lblDatesGIII.Text + ", " + lblAircraftGraphIII.Text
    End Sub
    Private Sub SetReportGraphII()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As DataSet

        ReportName = "Aircraft Utilization"
        SetValuesGraphII()
        myReport = New crAircraftUtilizationByPeriod
        ds = New dsAircraftUtilizationByHoursCycles
        mAircraftUtilizationByHoursCycles = AircraftUtilizationByHoursCycles.GetAircraftUtilizationGraphByPeriods(AircraftIds, New SmartDate(txtStartDate.Text), New SmartDate(txtEndDate.Text), mPeriodParameter)
        mAircraftUtilizationByHoursCycles.Sort("SortOrder", ComponentModel.ListSortDirection.Ascending)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, ReportName, txtStartDate.Text, AircraftName, txtEndDate.Text, "", CType(mPeriodParameter.ToString, String), AppSettings("Product Version"), AppSettings("SINote"), "", AppSettings("Logo"))

        If mAircraftUtilizationByHoursCycles.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1273)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mAircraftUtilizationByHoursCycles)

        da.Fill(ds, Report)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        txtStartDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
        txtEndDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "AircraftUtilizationGraph", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As DataSet

        ReportName = "Aircraft Utilization"
        SetValues()

        If rdoByFlyingHour.Checked Then
            myReport = New crptAircraftUtilizationByHoursCycles
            ds = New dsAircraftUtilizationByHoursCycles
            mAircraftUtilizationByHoursCycles = AircraftUtilizationByHoursCycles.GetAircraftUtilizationGraphByHoursCycles(AircraftIds, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1,, chkClassification.Checked)
            mAircraftUtilizationByHoursCycles.Sort("SortOrder", ComponentModel.ListSortDirection.Ascending)
        Else
            myReport = New crptAircraftUtilizationByFlyingDays
            ds = New dsAircraftUtilizationByFlyingDays
            mAircraftUtilizationByFlyingDays = AircraftUtilizationByFlyingDays.GetAircraftUtilizationGraphByFlyingDays(AircraftIds, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1)
            mAircraftUtilizationByFlyingDays.Sort("SortOrder", ComponentModel.ListSortDirection.Ascending)
        End If
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                 mCompanyDetail.WebSite, ReportName, ToDate, AircraftName, IIf(chkClassification.Checked, "True", "False"), "", "", AppSettings("Product Version"), AppSettings("SINote"), "", AppSettings("Logo"), "", "", cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


        If rdoByFlyingHour.Checked AndAlso mAircraftUtilizationByHoursCycles.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.NoRecordFound, "No record found for current searching criteria.", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAircraftUtilizationGraph.aspx?"
            'msg1.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

            Exit Sub
        ElseIf rdoByFlyingDay.Checked AndAlso mAircraftUtilizationByFlyingDays.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.NoRecordFound, "No record found for current searching criteria.", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAircraftUtilizationGraph.aspx?"
            'msg1.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1273)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)

        If rdoByFlyingHour.Checked Then
            da.Fill(ds, mAircraftUtilizationByHoursCycles)
        Else
            da.Fill(ds, mAircraftUtilizationByFlyingDays)
        End If

        da.Fill(ds, Report)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "AircraftUtilizationGraph", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetReportGraphIII() 'GEPL
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = String.Empty
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As DataSet

        ReportName = "Aircraft Utilization"
        SetValuesGraphIII()

        myReport = New crAircraftUtilizationForGEPL
        ds = New dsAircraftUtilizationByHoursCycles
        mAircraftUtilizationByHoursCycles = AircraftUtilizationByHoursCycles.GetAircraftUtilizationGraphByHoursCycles(AircraftIds, CInt(cmbFrmYear.SelectedItem.Text), cmbFrmMonth.SelectedIndex + 1, CInt(cmbMonths.SelectedValue))
        mAircraftUtilizationByHoursCycles.Sort("SortOrder", ComponentModel.ListSortDirection.Ascending)

        Dim StartDate As SmartDate = New SmartDate(DateSerial(CInt(cmbFrmYear.SelectedItem.ToString), cmbFrmMonth.SelectedIndex + 1, 1), False)
        Dim EndDate As SmartDate = New SmartDate(DateAdd(DateInterval.Month, CDec(cmbMonths.SelectedItem.ToString - 1), CDate(StartDate.FormattedText)))

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                 mCompanyDetail.WebSite, ReportName, ToDate, AircraftName, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", AppSettings("Logo"), "", MonthName(Month(CDate(StartDate.FormattedText)), True).ToUpper + " " + cmbFrmYear.SelectedItem.ToString + " TO " + MonthName(Month(CDate(EndDate.FormattedText)), True).ToUpper + " " + Year(CDate(EndDate.FormattedText)).ToString, "From " + cmbFrmMonth.SelectedItem.Text + " " + cmbFrmYear.SelectedItem.Text + " Till Next " + cmbMonths.SelectedValue.ToString + " months.")


        If mAircraftUtilizationByHoursCycles.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1273)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mAircraftUtilizationByHoursCycles)
        da.Fill(ds, Report)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)

        MarkLog(Util.Action.Print, "AircraftUtilizationGraph", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()

        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
                cmbFrmYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year) 'GEPL
            Next
            cmbYear.SelectedIndex = 10
            cmbFrmYear.SelectedIndex = 10 'GEPL
        End If
        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
            cmbFrmMonth.Items.Add(mon) 'GEPL
        Next


    End Sub
    Private Sub DataFieldBinding()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True)
        Session("mMachineNameValueList") = mMachineNameValueList
        ChklistAircraft.DataSource = mMachineNameValueList

        ChklistAircraftGraphII.DataSource = mMachineNameValueList
        ChklistAircraftGraphIII.DataSource = mMachineNameValueList 'GEPL
        DataBind()
    End Sub

    'Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidate As CustomValidator
    '    custValidate = CType(s, CustomValidator)
    '    If custValidate.ControlToValidate = "ChklistAircraft" Then
    '        For i As Integer = 0 To ChklistAircraft.Items.Count - 1
    '            If ChklistAircraft.Items(i).Selected Then
    '                e.IsValid = True
    '                Exit Sub
    '            End If
    '        Next
    '        custValidate.ErrorMessage = "Select atleast one Aircraft"
    '        e.IsValid = False
    '    End If
    'End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    '
            End Select
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not Page.IsPostBack Then
            SetCombo()
            DataFieldBinding()
            'txtStartDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtStartDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
            txtEndDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub

    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnCurrentSearchCriteriaGraphII_Click(sender As Object, e As System.EventArgs) Handles btnCurrentSearchCriteriaGraphII.Click
        Display()
        SetValuesGraphII()
        upnlCriteriaLabels.Update()
    End Sub
    Private Sub btnCurrentSearchCriteriaGraphIII_Click(sender As Object, e As System.EventArgs) Handles btnCurrentSearchCriteriaGraphIII.Click
        Display()
        SetValuesGraphIII()
        upnlCriteriaLabelsGIII.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport()
        Else
            upnlValidationsummary1.Update()
        End If
    End Sub
    Private Sub btnDisplayGraphII_Click(sender As Object, e As System.EventArgs) Handles btnDisplayGraphII.Click
        Page.Validate("b")
        If Page.IsValid Then
            SetReportGraphII()
        Else
            upnlValidationsummary2.Update()
        End If
    End Sub
    Private Sub btnDisplayGraphIII_Click(sender As Object, e As System.EventArgs) Handles btnDisplayGraphIII.Click
        Page.Validate("c")
        If Page.IsValid Then
            SetReportGraphIII()
        Else
            upnlValidationsummary3.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseGraphII.Click, btnCloseGraphIII.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region






End Class