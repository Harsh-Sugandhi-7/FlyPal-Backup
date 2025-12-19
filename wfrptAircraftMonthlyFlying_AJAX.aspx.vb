
'AJAX CREATED By : Saylee
'Dated           : 28-Feb-2014


Public Class wfrptAircraftMonthlyFlying_AJAX
    Inherits System.Web.UI.Page


#Region "Variable Declaration"
    Dim mrptAircraftMonthlyFlying As rptAircraftMonthlyFlying
    Dim mrptAircraftMonthlyFlyingList As rptAircraftMonthlyFlyingList
    Dim mAssemblyStatusList As AssemblyStatusList
    Dim mAircraftMonthlyPeriodList As AircraftMonthlyPeriodList
    Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
    Dim mMachineNameValueList As MachineNameValueList
    Public pLog As Log
    Dim ToDate As String
    Dim MachineID As String
    Dim Aircraft As String
    Dim Period As String
    Dim mPeriodListPerMachineOfAirframes As PeriodListPerMachineOfAirframes

    'Added by Abhishek on 27-SEP-2017
    Dim serchstr7 As String
    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim ds As New dsAircraftMonthlyFlying
    Dim mCompanyDetail As New CompanyDetail
    Dim ReportName As String = ""
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mrptAircraftMonthlyFlying = CType(Session("mrptAircraftMonthlyFlying"), rptAircraftMonthlyFlying)
        mrptAircraftMonthlyFlyingList = CType(Session("mrptAircraftMonthlyFlyingList"), rptAircraftMonthlyFlyingList)
        pLog = Session("pLog")
        mAircraftMonthlyPeriodList = Session("mAircraftMonthlyPeriodList")
        mAssemblyStatusList = Session("mMonthlyFlyingAssemblyStatusList")
        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
    Private Sub SetSession()
        Session("mrptAircraftMonthlyFlying") = mrptAircraftMonthlyFlying
        Session("mrptAircraftMonthlyFlyingList") = mrptAircraftMonthlyFlyingList
        Session("mAircraftMonthlyPeriodList") = mAircraftMonthlyPeriodList
        Session("pLog") = pLog
        Session("mMonthlyFlyingAssemblyStatusList") = mAssemblyStatusList
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptAircraftMonthlyFlying")
        Session.Remove("mrptAircraftMonthlyFlyingList")
        Session.Remove("mAircraftMonthlyPeriodList")
        Session.Remove("pLog")
        Session.Remove("mMonthlyFlyingAssemblyStatusList")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblyearselection.Visible = True
        lblPeriod1.Visible = True
    End Sub
    Private Sub SetValues()
        ToDate = cmbYear.SelectedItem.Text
        Dim i As Integer
        If Not mMachineNameValueList Is Nothing Then
            For i = 0 To mMachineNameValueList.Count - 1
                If mMachineNameValueList.Item(i).IsSelected = True Then
                    If Aircraft = "" Then
                        Aircraft = mMachineNameValueList.Item(i).RegNo
                    Else
                        Aircraft = Aircraft + "," + mMachineNameValueList.Item(i).RegNo
                    End If
                End If
            Next
        End If
        If Aircraft = "" Then
            Aircraft = cmbAircraft.SelectedItem.Text
        End If
        Period = IIf(chkBlockTime.Visible, IIf(chkBlockTime.Checked, "Block ", "Airborne "), "") + cmbPeriod.SelectedItem.Text
        MachineID = cmbAircraft.SelectedValue.ToString
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblyearselection.Text = "Year : " & IIf(ToDate <> "", ToDate, "")
        lblPeriod1.Text = "Period : " & IIf(cmbPeriod.Items.Count <> 0, cmbPeriod.SelectedItem.Text, "")
    End Sub
    Private Sub BindPeriod()
        Dim dat As String
        'dat = "12/31/" + cmbYear.SelectedItem.Text
        dat = DateSerial(CInt(cmbYear.SelectedItem.Text), 12, 31).ToString
        If cmbAircraft.SelectedIndex > 0 Then
            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(dat, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , _
                                 False, MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False, CompMonitoringInspRequired:=False, _
                                 CompMonitoringModRequired:=False, CompMonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
            Session("mMonthlyFlyingAssemblyStatusList") = mAssemblyStatusList
            AssemblyStatusPeriodList = mAssemblyStatusList(0).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            mAircraftMonthlyPeriodList = AircraftMonthlyPeriodList.GetAircraftMonthlyPeriodList(AssemblyStatusPeriodList)
            Session("mAircraftMonthlyPeriodList") = mAircraftMonthlyPeriodList
            cmbPeriod.DataSource = mAircraftMonthlyPeriodList
            cmbPeriod.DataBind()
        Else
            mPeriodListPerMachineOfAirframes = PeriodListPerMachineOfAirframes.GetPeriodListPerMachineOfAirframes(New Guid(cmbAircraft.SelectedValue.ToString))
            Session("mPeriodListPerMachineOfAirframes") = mPeriodListPerMachineOfAirframes
            mAircraftMonthlyPeriodList = AircraftMonthlyPeriodList.GetAircraftMonthlyPeriodList(mPeriodListPerMachineOfAirframes)
            Session("mAircraftMonthlyPeriodList") = mAircraftMonthlyPeriodList
            cmbPeriod.DataSource = mAircraftMonthlyPeriodList
            cmbPeriod.DataBind()
        End If
    End Sub
    Private Sub SetReport()
        Dim serchstr7 As String  'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsAircraftMonthlyFlying
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportName As String = ""

        myReport = New crAircraftMonthlyFlying
        ReportName = "Aircraft Monthly Flying"
        AddAircraft()
        SetValues()
        mrptAircraftMonthlyFlyingList = rptAircraftMonthlyFlyingList.GetrptAircraftMonthlyFlyingList(cmbYear.SelectedItem.Text, mMachineNameValueList, IIf(cmbPeriod.Items.Count <> 0, cmbPeriod.SelectedValue, 1), ToShowBlockTime:=chkBlockTime.Checked)
        'dgAircraftMonthlyList.DataSource = mrptAircraftMonthlyFlyingList
        'dgAircraftMonthlyList.DataBind()


        'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 

        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
            If cmbAircraft.SelectedIndex > 0 Or mrptAircraftMonthlyFlyingList.Count = 1 Then
                If mrptAircraftMonthlyFlyingList.Count = 1 Then
                    serchstr7 = MachineOperatorName.GetMachineOperatorName(mrptAircraftMonthlyFlyingList(0).ID).OperatorName
                Else
                    serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
                End If

            Else
                serchstr7 = ""
            End If
        Else
            serchstr7 = ""
        End If

        'End

        Dim EventLogDetail As String = "As On Date : " + ToDate + "Aircraft : " + Aircraft + " , Period: " + Period

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, ReportName, "", ToDate, Aircraft, Period, "", AppSettings("Product Version"), AppSettings("SINote"), "", serchstr7, "", "", AppSettings("Logo"))    'Changed By Utkarsh For Report Logo.

        If mrptAircraftMonthlyFlyingList.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.NoRecordFound, "Please select atleast one Aircraft.", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfrptAircraftMonthlyFlying.aspx?"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.NoRecordFound, "Please select atleast one Aircraft.", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1196)
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, mrptAircraftMonthlyFlyingList)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        mrptAircraftMonthlyFlyingList = Nothing
        ''Dim Str As String
        ''Str = "<script language=Javascript>openTranDetail();</script>"
        ''ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "AircraftFlying", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub
    Private Sub AddAircraft()
        Dim item As GridViewRow
        Dim chkBox As CheckBox
        Dim RegNo As String
        Dim RecordNo, PageItems As Integer
        Dim i As Integer
        'Added By Shweta On 12-March-2013 For  ALL12032013
        If cmbAircraft.SelectedIndex = 0 Then
            PageItems = dgAircraftMonthlyList.Rows.Count - 2
        Else
            PageItems = dgAircraftMonthlyList.Rows.Count - 1
        End If
        ''
        'For i = 0 To PageItems - 1 'Commented By Shweta On 12-March-2013 For  ALL12032013
        For i = 0 To PageItems  'Added By Shweta On 12-March-2013 For  ALL12032013
            RecordNo = i + dgAircraftMonthlyList.PageSize * dgAircraftMonthlyList.PageIndex
            item = dgAircraftMonthlyList.Rows(i)
            RegNo = item.Cells(2).Text
            chkBox = CType(item.FindControl("chkSelect"), CheckBox)
            mMachineNameValueList(RegNo).IsSelected = chkBox.Checked
        Next
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub ControlVisibility() 'Added by Saylee on 25-Jan-2013 for - ALL25012013
        Dim item As GridViewRow
        Dim chkBox As CheckBox
        Dim RegNo As String
        Dim RecordNo, PageItems As Integer
        Dim i As Integer
        PageItems = dgAircraftMonthlyList.Rows.Count - 1
        For i = 0 To PageItems

            RecordNo = i + dgAircraftMonthlyList.PageSize * dgAircraftMonthlyList.PageIndex
            item = dgAircraftMonthlyList.Rows(i)
            RegNo = item.Cells(2).Text
            chkBox = CType(item.FindControl("chkSelect"), CheckBox)
            If RegNo = "Total" Then
                chkBox.Visible = False
            End If
        Next
        chkBlockTime.Visible = IIf(mAircraftMonthlyPeriodList.Count > 0 AndAlso cmbPeriod.SelectedValue = "1", True, False) 'For Hours Period show block time check box
    End Sub
    Private Sub BindGrid()
        If cmbAircraft.SelectedIndex = 0 Then
            mrptAircraftMonthlyFlyingList = rptAircraftMonthlyFlyingList.GetrptAircraftMonthlyFlyingList(cmbYear.SelectedItem.Text, mMachineNameValueList, IIf(cmbPeriod.Items.Count <> 0, cmbPeriod.SelectedValue, 1), True, ToShowBlockTime:=chkBlockTime.Checked)
            dgAircraftMonthlyList.DataSource = mrptAircraftMonthlyFlyingList
            dgAircraftMonthlyList.DataBind()
        Else
            mrptAircraftMonthlyFlyingList = rptAircraftMonthlyFlyingList.GetrptAircraftMonthlyFlyingList(cmbYear.SelectedItem.Text, cmbAircraft.SelectedValue, IIf(cmbPeriod.Items.Count <> 0, cmbPeriod.SelectedValue, 1), ToShowBlockTime:=chkBlockTime.Checked)
            If mrptAircraftMonthlyFlyingList(0).RegNo = "" Then
                mrptAircraftMonthlyFlyingList(0).RegNo = cmbAircraft.SelectedItem.Text
            End If
            dgAircraftMonthlyList.DataSource = mrptAircraftMonthlyFlyingList
            dgAircraftMonthlyList.DataBind()
        End If

        For j As Integer = 0 To mMachineNameValueList.Count - 1
            mMachineNameValueList(j).IsSelected = False
        Next
        Session("mMachineNameValueList") = mMachineNameValueList
        lblgrid.Text = "List of Aircraft Monthly Flying  : " & mrptAircraftMonthlyFlyingList.Count & " Record(s) found."
    End Sub
#End Region

#Region "DataFieldBind"
    Private Sub SetCombo()
        Dim i As Integer
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, , , , , , , True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()
        Session("mMachineNameValueList") = mMachineNameValueList
        BindPeriod()
        mrptAircraftMonthlyFlyingList = rptAircraftMonthlyFlyingList.GetrptAircraftMonthlyFlyingList(cmbYear.SelectedItem.Text, mMachineNameValueList, IIf(cmbPeriod.Items.Count <> 0, cmbPeriod.SelectedValue, 1), True, ToShowBlockTime:=chkBlockTime.Checked)
        dgAircraftMonthlyList.DataSource = mrptAircraftMonthlyFlyingList
        dgAircraftMonthlyList.DataBind()
        Session("mrptAircraftMonthlyFlyingList") = mrptAircraftMonthlyFlyingList
        'mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString)
        'dgAircraft.DataSource = mMachineNameValueList
        'dgAircraft.DataBind()

    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not Page.IsPostBack Then
            SetCombo()
            DataFieldBind()
            lblgrid.Text = "List of Aircraft Monthly Flying  : " & mrptAircraftMonthlyFlyingList.Count & " Record(s) found."
            ControlVisibility()
        End If


    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        ControlVisibility() 'Added by Saylee on 25-Jan-2013 for - ALL25012013
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        BindPeriod()
        BindGrid() 'Added By Shweta On 12-March-2013 For  ALL12032013
        ControlVisibility() 'Added by Saylee on 25-Jan-2013 for - ALL25012013
        'chkBlockTime.Checked = False'commented by vikrant on 20-Dec-2021 for bug reported by Preeti
        upnlGrid.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPeriod.SelectedIndexChanged, cmbYear.SelectedIndexChanged
        BindGrid()
        ControlVisibility() 'Added by Saylee on 25-Jan-2013 for - ALL25012013
        chkBlockTime.Checked = False
        upnlGrid.Update()
    End Sub
    Private Sub chkBlockTime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBlockTime.CheckedChanged
        BindGrid()
        ControlVisibility() 'Added by Saylee on 25-Jan-2013 for - ALL25012013
        upnlGrid.Update()
    End Sub
#End Region
    'Added by Abhishek on 27-SEP-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then

            ReportName = "Aircraft Monthly Flying"
            AddAircraft()
            SetValues()
            mrptAircraftMonthlyFlyingList = rptAircraftMonthlyFlyingList.GetrptAircraftMonthlyFlyingList(cmbYear.SelectedItem.Text, mMachineNameValueList, IIf(cmbPeriod.Items.Count <> 0, cmbPeriod.SelectedValue, 1), ToShowBlockTime:=chkBlockTime.Checked)

            'dgAircraftMonthlyList.DataSource = mrptAircraftMonthlyFlyingList
            'dgAircraftMonthlyList.DataBind()


            'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 

            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                If cmbAircraft.SelectedIndex > 0 Or mrptAircraftMonthlyFlyingList.Count = 1 Then
                    If mrptAircraftMonthlyFlyingList.Count = 1 Then
                        serchstr7 = MachineOperatorName.GetMachineOperatorName(mrptAircraftMonthlyFlyingList(0).ID).OperatorName
                    Else
                        serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
                    End If

                Else
                    serchstr7 = ""
                End If
            Else
                serchstr7 = ""
            End If

            'End

            Dim EventLogDetail As String = "As On Date : " + ToDate + "Aircraft : " + Aircraft + " , Period: " + Period

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
             mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
             mCompanyDetail.WebSite, ReportName, "", ToDate, Aircraft, Period, "", AppSettings("Product Version"), AppSettings("SINote"), "", serchstr7, "", "", AppSettings("Logo"))    'Changed By Utkarsh For Report Logo.

            If mrptAircraftMonthlyFlyingList.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.NoRecordFound, "Please select atleast one Aircraft.", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfrptAircraftMonthlyFlying.aspx?"
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.NoRecordFound, "Please select atleast one Aircraft.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else

                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1196)
            End If

            da.Fill(ds, "ExcelrptAircraftMonthlyFlyingList", mrptAircraftMonthlyFlyingList)
            da.Fill(ds, "ReportData", Report)
            Dim columnToRemove1 As String() = {"ID", "Month", "JanFlyingHrs", "FebFlyingHrs", "MarFlyingHrs", "AprFlyingHrs", "MayFlyingHrs", "JunFlyingHrs", "JulFlyingHrs", "AugFlyingHrs", "SepFlyingHrs", "OctFlyingHrs", "NovFlyingHrs", "DecFlyingHrs", "TotalFlyingHrs", "TotalAvgFlyingHrs", "IsSelected", "HasNoValue"}
            For i As Integer = 0 To columnToRemove1.Length - 1
                If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains(columnToRemove1(i)) Then
                    ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Remove(columnToRemove1(i))
                End If
            Next

            Dim columnToRemove2 As String() = {"SearchStr1", "SearchStr5", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "SearchStr6", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
            For i As Integer = 0 To columnToRemove2.Length - 1
                If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                    ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                End If
            Next



            If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Year"
            End If

            If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
            End If


            If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Period"
            End If

            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("JanFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("JanFlyingHrsInString").ColumnName = "Jan"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("FebFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("FebFlyingHrsInString").ColumnName = "Feb"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("MarFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("MarFlyingHrsInString").ColumnName = "Mar"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("AprFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("AprFlyingHrsInString").ColumnName = "Apr"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("MayFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("MayFlyingHrsInString").ColumnName = "May"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("JunFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("JunFlyingHrsInString").ColumnName = "Jun"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("JulFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("JulFlyingHrsInString").ColumnName = "Jul"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("AugFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("AugFlyingHrsInString").ColumnName = "Aug"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("SepFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("SepFlyingHrsInString").ColumnName = "Sep"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("OctFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("OctFlyingHrsInString").ColumnName = "Oct"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("NovFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("NovFlyingHrsInString").ColumnName = "Nov"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("DecFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("DecFlyingHrsInString").ColumnName = "Dec"
            End If
          
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("TotalFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("TotalFlyingHrsInString").ColumnName = "Total"
            End If
            If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("TotalAvgFlyingHrsInString") Then
                ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("TotalAvgFlyingHrsInString").ColumnName = "Average/Month"
            End If
            'If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
            '    ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Report Date"
            'End If
            'If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("SearchStr1") Then
            '    ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("SearchStr1").ColumnName = "Aircraft"
            'End If
            'If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("IssueDateFormatted") Then
            '    ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("IssueDateFormatted").ColumnName = "Issue Date"
            'End If
            'If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("DueOnValue") Then
            '    ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("DueOnValue").ColumnName = "Next Due"
            'End If
            'If ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns.Contains("LastCarriedOut") Then
            '    ds.Tables("ExcelrptAircraftMonthlyFlyingList").Columns("LastCarriedOut").ColumnName = "Last Carried"
            'End If
            Dim dsNew As New DataSet
            dsNew.Clear()

            dsNew.Merge(ds.Tables("ReportData"))
            dsNew.Merge(ds.Tables("ExcelrptAircraftMonthlyFlyingList"))

            dsNew.Tables("ReportData").TableName = "Searching Criteria"
            dsNew.Tables("ExcelrptAircraftMonthlyFlyingList").TableName = "Aircraft Monthly FlyingList"
			Session("ExcelFileName") = "Aircraft Monthly FlyingList"
			Session("dsNew") = dsNew
            Session("DataTableToBeFormattedForExportToExcel") = "Aircraft Monthly FlyingList"
            'PeriodColumnsForExportToExcel.AddRange(New String() {"OrderNo"})
            'Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            'Session("DataTable") = ds.Tables("ExcelrptAircraftwiseConsumption")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            MarkLog(Util.Action.Print, "AircraftFlying", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
        End If

    End Sub
End Class