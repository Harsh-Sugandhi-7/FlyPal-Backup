'Added By Yogita

Public Class wfSearchCriteriaForLifeHistoryAssembly_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mModel As Model
    Public mReportMonitoringParaList As ReportMonitoringParaList
    Public mMacList As AssemblyList
    Public mSelectPeriods As SelectPeriods = SelectPeriods.NewSelectPeriods
    Public mAssemblyStatus As AssemblyStatus

    Public mAssemblyStatusID As Guid              'Added Code
    Public FromDate As String = ""
    Public ToDate As String = ""
    Public GraphGroupType As String = ""
    Public AssemblyType As String = ""

    Dim I As Integer

    Dim mAssType As Integer
    Dim mModelName As String
    Dim mSerialNo As String
    Dim mPartNo As String
    Dim mCompSerialNo As String
    Dim MID As Guid
    Dim index As Int32

    Public mAssemblyList As AssemblyList 'Added by Saylee on 3rd-Aug-2009

    Dim mCompleteSearchingCriteria As String = String.Empty
    Dim EventLogID As Guid
#End Region

#Region " Property "
    Public WriteOnly Property AssType() As Integer
        Set(ByVal Value As Integer)
            mAssType = Value
        End Set
    End Property
    Public Property ModelProp() As String
        Get
            Return mModelName
        End Get
        Set(ByVal Value As String)
            mModelName = Value
        End Set
    End Property
    Public Property SerialNoProp() As String
        Get
            Return mSerialNo
        End Get
        Set(ByVal Value As String)
            mSerialNo = Value
        End Set
    End Property
    Public Property PartNoProp() As String
        Get
            Return mPartNo
        End Get
        Set(ByVal Value As String)
            mPartNo = Value
        End Set
    End Property
    Public Property CompSerialNoProp() As String
        Get
            Return mCompSerialNo
        End Get
        Set(ByVal Value As String)
            mCompSerialNo = Value
        End Set
    End Property
#End Region

#Region " Business Properties and Methods "
    Private Sub GetSession()
        mReportMonitoringParaList = CType(Session("mReportMonitoringParaList"), ReportMonitoringParaList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMacList = CType(Session("mMacList"), AssemblyList)
        mModelName = CType(Session("mModelName"), String)
        mSerialNo = CType(Session("mSerialNo"), String)
        mSelectPeriods = CType(Session("mSelectPeriods"), SelectPeriods)
        mModel = CType(Session("mModel"), Model)
        MID = CType(Session("MID"), Guid)

        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList) 'Added by Saylee on 3rd-Aug-2009
    End Sub
    Private Sub SetSession()
        Session("mReportMonitoringParaList") = mReportMonitoringParaList
        Session("mMacList") = mMacList
        Session("mModelName") = mModelName
        Session("mSerialNo") = mSerialNo
        Session("mSelectPeriods") = mSelectPeriods
        Session("mModel") = mModel
        Session("MID") = MID

        Session("mAssemblyList") = mAssemblyList 'Added by Saylee on 3rd-Aug-2009
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mReportMonitoringParaList")
        Session.Remove("mMacList")
        Session.Remove("mModel")
        Session.Remove("mAssemblyList")
    End Sub
    Private Sub SetObject()
        'mMachine.MachineCategoryID = CInt(cmbCategory.SelectedValue.ToString)
        'mMachine.AssemblyStatus.Assembly.ModelID = New Guid(cmbModel.SelectedValue)
        'mMachine.HourType = Val(cmbHourTypeList.SelectedValue)
        'mMachine.UnitID = Val(cmbUnit.SelectedValue.ToString)
        'Session("mMachine") = mMachine
        ' mAssemblyStatus.AsOnDate = cmbDateRange.SelectedValue
        'mAssemblyStatus.AssemblyStatusPeriods 
        'cmbDateRange()
        'cmbGraphGroupType()
        'cmbAssemblyType()
    End Sub

    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
            str = "document.getElementById('" + cntrl.ClientID + "').focus();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub

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
    Private Sub ControlVisibility(ByVal Index As Int32)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)

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
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        ' lblToDate.Visible = True
        lblGraphGroupType1.Visible = True
        lblAssemblyType1.Visible = True
        lblModel1.Visible = True
        lblSerialNo1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblGraphGroupType1.Visible = False
        lblAssemblyType1.Visible = False
        lblModel1.Visible = False
        lblSerialNo1.Visible = False
    End Sub
    Private Sub SetDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select

        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))

    End Sub
    Private Sub SetValues()

        ''If cmbDateRange.SelectedIndex = 0 Then                    'Date
        ''    FromDate = "1-1-1900"
        ''    ToDate = "1-1-2200"
        ''    lblDateRangeFrom.Text = "Date Range     : All"
        ''Else
        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        ''lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText
        ''End If

        If cmbGraphGroupType.SelectedIndex = 0 Then                'Graph Group Type
            GraphGroupType = "Year"
            lblGraphGroupType1.Text = "Graph Group Type  : " & GraphGroupType
        ElseIf cmbGraphGroupType.SelectedIndex = 1 Then
            GraphGroupType = "Month"
            lblGraphGroupType1.Text = "Graph Group Type  : " & GraphGroupType
        ElseIf cmbGraphGroupType.SelectedIndex = 2 Then
            GraphGroupType = "Day"
            lblGraphGroupType1.Text = "Graph Group Type  : " & GraphGroupType
        End If

        If cmbAssemblyType.SelectedIndex = 0 Then
            AssemblyType = "All"
            lblAssemblyType1.Text = "Assembly Type  : " & AssemblyType
        ElseIf cmbAssemblyType.SelectedIndex = 1 Then
            AssemblyType = "Airframe"
            lblAssemblyType1.Text = "Assembly Type  : " & AssemblyType
        ElseIf cmbAssemblyType.SelectedIndex = 2 Then
            AssemblyType = "Engine"
            lblAssemblyType1.Text = "Assembly Type  : " & AssemblyType
        ElseIf cmbAssemblyType.SelectedIndex = 3 Then
            AssemblyType = "Propeller"
            lblAssemblyType1.Text = "Assembly Type  : " & AssemblyType
        ElseIf cmbAssemblyType.SelectedIndex = 4 Then
            AssemblyType = "Auxillary Power Unit"
            lblAssemblyType1.Text = "Assembly Type  : " & AssemblyType
        ElseIf cmbAssemblyType.SelectedIndex = 5 Then
            AssemblyType = "Combined Gear Box"
            lblAssemblyType1.Text = "Assembly Type  : " & AssemblyType
        ElseIf cmbAssemblyType.SelectedIndex = 6 Then
            AssemblyType = "Main Gear Box"
            lblAssemblyType1.Text = "Assembly Type  : " & AssemblyType
        End If

        ''lblModel1.Text = "Model :" & txtModel.Text                      'Model
        ''lblSerialNo1.Text = "Serial No :" & txtSerialNo.Text            'Serial No
        If mAssemblyList.Count > 0 Then
            lblModel1.Text = "Model :" & mAssemblyList(New Guid(cmbModelList.SelectedValue)).ModelName          'Model
            lblSerialNo1.Text = "Serial No :" & mAssemblyList(New Guid(cmbModelList.SelectedValue)).SerialNo    'Serial No
        Else
            lblModel1.Text = "Model :"
            lblSerialNo1.Text = "Serial No :"
        End If

        mCompleteSearchingCriteria = lblDateRangeFrom.Text + ", " + lblGraphGroupType1.Text + ", " + lblAssemblyType1.Text + ", " + lblModel1.Text + ", " + lblSerialNo1.Text
    End Sub

    Public Sub SetReport()
        '==============================================================================
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim dsLifeHistory As New dsLifeHistory
        Dim da As New CSLA.Data.ObjectAdapter
        ' Dim Report As New crLifeHistory
        Dim mCompanyDetail As New Flypal.CompanyDetail
        Dim DateSearch As String
        Dim GraphGroupType As String
        Dim AssemblyType As String
        Dim ModelName As String
        Dim Period As String
        Dim GraphType As String
        Dim OperatorName As String = ""
        myReport = New crLifeHistory

        ''If cmbDateRange.SelectedIndex = 0 Then
        ''    FromDate = "1-1-1900"
        ''    ToDate = "1-1-2200"
        ''    ''string1 = "By" + " " + " " + ":" + " " + "(" + cmbDateRange.SelectedItem.Text + ")"
        ''    DateSearch = "(" + cmbDateRange.SelectedItem.Text + ")"
        ''Else
        ''string1 = "By" + " " + " " + ":" + " " + cmbDateRange.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Value.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Value.ToString).FormattedText
        FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, Today.Date.ToString)
        ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, Today.Date.ToString) ' txtToDate.Value.ToString
        ''DateSearch = cmbDateRange.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(FromDate.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(ToDate.ToString).FormattedText
        DateSearch = lblFromDate.Text + " " + New SmartDate(FromDate.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(ToDate.ToString).FormattedText
        '' End If

        If optLine.Checked = True Then
            ''string2 = "Graph Group Type :" + " " + cmbGraphGroupType.SelectedItem.Text + " " + "and" + " " + "Graph Type : Line"
            GraphGroupType = cmbGraphGroupType.SelectedItem.Text
            GraphType = "Line"
        End If

        ''string3 = "Assembly Type :" + " " + cmbAssemblyType.SelectedItem.Text
        AssemblyType = cmbAssemblyType.SelectedItem.Text
        ''string4 = "Model :" + " " + txtModel.Text + " " + "and" + " " + "Serial No. :" + " " + txtSerialNo.Text

        ModelName = mAssemblyList(New Guid(cmbModelList.SelectedValue)).ModelSerialNo

        If cmbPeriods.SelectedItem.Text <> "" Then
            ''string5 = " For Period :" + " " + cmbPeriods.SelectedItem.Text
            Period = cmbPeriods.SelectedItem.Text
        End If

        If mAssemblyList.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectRestriction, SIMsgBox.Message_text.SelectRestriction, "Please Another Assembly Type.", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfSearchCriteriaForLifeHistoryAssembly.aspx?Backpage="
            'msg1.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mReportMonitioringPara As ReportMonitoringPara
        mAssemblyStatusID = ReportFetchAssemblyStatusInfo.GetReportFetchAssemblyStatusInfo(Today.ToShortDateString, New Guid(cmbModelList.SelectedValue)).AssemblyStatusID
        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusID)

        mReportMonitioringPara = ReportMonitoringPara.GetReportMonitoringParaList(FromDate, ToDate, mAssemblyStatus.AssemblyID, Guid.Empty, CInt(cmbPeriods.SelectedValue.ToString), cmbGraphGroupType.SelectedItem.Text, mAssemblyStatus.HourType)

        'Added by Saylee on 11-Aug-2011
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(mAssemblyStatus.MachineID)
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        End If

        Dim mReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, _
        mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        "Graphical Representation of Life History", New SmartDate(FromDate).FormattedText, GraphGroupType, AssemblyType, ModelName, Period, AppSettings("Product Version"), AppSettings("SINote"), "", New SmartDate(ToDate).FormattedText, OperatorName, "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        If mReportMonitioringPara.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfSearchCriteriaForLifeHistoryAssembly.aspx?Backpage="
            'msg1.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 911)
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(dsLifeHistory)
        '----------------------------------------------------------
        da.Fill(dsLifeHistory, mReportMonitioringPara)
        da.Fill(dsLifeHistory, mReportData)
        da.Fill(dsLifeHistory, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(dsLifeHistory)
        Session("CrystalReport") = myReport

        'Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)

        MarkLog(Util.Action.Print, "LifeHistory", mCompleteSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForLifeHistoryAssembly.aspx" Then
            RemoveSession()
            Session.Remove("AssemblyTypeIndex")
            Session.Remove("GroupIndex")
            Session.Remove("SerialNo")
            mModelName = ""
            mSerialNo = ""
            Session("mModelName") = mModelName
            Session("mSerialNo") = mSerialNo
        End If

    End Sub
    'Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "cmbAircraft" Then
    '        If txtFromDate.Text.Equals(CDate(Today.AddDays(1).AddMonths(-1)) Then
    '            custValidator.ErrorMessage = "Please select the Aircraft"
    '            e.IsValid = False
    '        Else
    '            e.IsValid = True
    '        End If
    '    End If
    'End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Commented by Saylee on 3rd-Aug-2009
        ''cmbPeriods.DataSource = mSelectPeriods
        ''cmbPeriods.DataBind()

        mAssemblyList = AssemblyList.GetAssemblyList("", "", CInt(cmbAssemblyType.SelectedValue.ToString), "{00000000-0000-0000-0000-000000000000}", Now.Date.ToString)     ''mAssemblyList.Item(mAssemblyList.CurrentIndex).ModelName, mAssemblyList.Item(mAssemblyList.CurrentIndex).SerialNo, , mAssemblyList.Item(mAssemblyList.CurrentIndex).ID.ToString)


        If mAssemblyList.Count > 0 Then 'Added by Saylee on 3rd-Aug-2009
            cmbModelList.DataSource = mAssemblyList
            cmbModelList.DataBind()

            Session("mAssemblyList") = mAssemblyList

            mSelectPeriods = SelectPeriods.NewSelectPeriods
            mAssemblyStatusID = ReportFetchAssemblyStatusInfo.GetReportFetchAssemblyStatusInfo(Today.ToShortDateString, mAssemblyList(0).ID).AssemblyStatusID
            mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusID)
            Dim i As Integer = 0
            While i <= mAssemblyStatus.AssemblyStatusPeriods.Count - 1
                If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 Then
                    mSelectPeriods.Add(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID, mAssemblyStatus.AssemblyStatusPeriods(i).PeriodName)
                End If
                i = i + 1
            End While
            Session("mSelectPeriods") = mSelectPeriods
            cmbPeriods.DataSource = mSelectPeriods
            cmbPeriods.DataBind()
        Else
            cmbModelList.Enabled = False
            cmbPeriods.Enabled = False
        End If
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        SetSession()

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForLifeHistoryAssembly.aspx"
            RemoveSession()
            If cmbGraphGroupType.Enabled = True Then
                SetFocus(cmbGraphGroupType)
            End If
            If Session("sender") <> "SelectPeriods" Then
                If Session("AssemblyTypeIndex") Is Nothing Then
                    cmbAssemblyType.SelectedIndex = 1 'Added by Saylee on 3rd-Aug-2009
                Else
                    cmbAssemblyType.SelectedIndex = Session("AssemblyTypeIndex")
                End If
            End If
            DataFieldBind()
            ''ControlVisibility( 2)

            If Session("sender") <> "SelectPeriods" Then
                SetDatePeroid(4)
                ''cmbDateRange.SelectedIndex = 2
                'cmbAssemblyType.SelectedIndex = Session("AssemblyTypeIndex") 'Added by Saylee on 7th-Apr-2008
                cmbGraphGroupType.SelectedIndex = Session("GroupIndex")  ''Added by Saylee on 7th-Apr-2008
            Else
                GetSettings()
                Session("sender") = ""
            End If
            ''SetObject()
        End If

        txtModel.Text = mModelName
        txtSerialNo.Text = mSerialNo
        Session("MID") = MID
        Session("sender") = ""
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()

        upnlDislaySearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
        Session("AssemblyTypeIndex") = IIf(cmbAssemblyType.SelectedIndex <= 0, 0, cmbAssemblyType.SelectedIndex)
        If cmbAssemblyType.SelectedItem.Text = "(All)" Then
            lblAssemblyType1.Text = "Assembly Type :" & " " & "(All)"
        Else
            lblAssemblyType1.Text = "Assembly Type :" & " " & cmbAssemblyType.SelectedItem.Text
        End If
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        ' mAssemblyStatusID = Guid.Empty
        DataFieldBind()
        txtModel.Text = ""
        txtSerialNo.Text = ""
        If cmbAssemblyType.Enabled = True Then
            SetFocus(cmbAssemblyType)
        End If

        mAssemblyList = AssemblyList.GetAssemblyList("", "", CInt(cmbAssemblyType.SelectedValue.ToString), "{00000000-0000-0000-0000-000000000000}", Now.Date.ToString)
        If mAssemblyList.Count > 0 Then
            cmbModelList.Enabled = True
            cmbPeriods.Enabled = True
            cmbModelList.DataSource = mAssemblyList
            cmbModelList.DataBind()
            Session("mAssemblyList") = mAssemblyList
        Else
            cmbModelList.Enabled = False
            cmbPeriods.Enabled = False
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There are no Models for this Assembly", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfSearchCriteriaForLifeHistoryAssembly.aspx?Backpage="
            'msg1.Show()

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There are no Models for this Assembly", MsgBoxStyle.OkOnly, "")

            Exit Sub
        End If
    End Sub
    ''Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    ''    Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
    ''    ControlVisibility(Index)
    ''    setDatePeroid(Index)
    ''    If cmbDateRange.Enabled = True Then
    ''        SetFocus(cmbDateRange)
    ''    End If
    ''End Sub
    Private Sub cmbGraphGroupType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGraphGroupType.SelectedIndexChanged
        'Commented by Archana on 7-Aug-09
        'If cmbGraphGroupType.SelectedValue = "Year" Then
        '    lblGraphGroupType1.Text = "Graph Group Type :" & " " & cmbGraphGroupType.SelectedValue
        'ElseIf cmbGraphGroupType.SelectedValue = "Month" Then
        '    setDatePeroid(2)
        '    lblGraphGroupType1.Text = "Graph Group Type :" & " " & cmbGraphGroupType.SelectedValue
        'ElseIf cmbGraphGroupType.SelectedValue = "Day" Then
        '    lblGraphGroupType1.Text = "Graph Group Type :" & " " & cmbGraphGroupType.SelectedValue
        'End If

        'Added by Archana on 7-Aug-09
        If cmbGraphGroupType.SelectedValue = 0 Then         'Year
            SetDatePeroid(4)
            txtFromDate.ReadOnly = False
            txtFromDate.Enabled = True
            lblGraphGroupType1.Text = "Graph Group Type :" & " " & cmbGraphGroupType.SelectedValue
        ElseIf cmbGraphGroupType.SelectedValue = 1 Then     'Month
            SetDatePeroid(4)
            txtFromDate.ReadOnly = False
            txtFromDate.Enabled = True
            lblGraphGroupType1.Text = "Graph Group Type :" & " " & cmbGraphGroupType.SelectedValue
        ElseIf cmbGraphGroupType.SelectedValue = 2 Then     'Day
            SetDatePeroid(2)
            txtFromDate.ReadOnly = True
            txtFromDate.Enabled = False
            lblGraphGroupType1.Text = "Graph Group Type :" & " " & cmbGraphGroupType.SelectedValue
        End If
        If cmbGraphGroupType.Enabled = True Then
            SetFocus(cmbGraphGroupType)
        End If
    End Sub
    Private Sub txtModel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtModel.TextChanged
        If txtModel.Text = "" Then
            lblModel1.Text = ""
        Else
            lblModel1.Text = "Model : " & " " & txtModel.Text
        End If
    End Sub
    Private Sub txtSerialNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSerialNo.TextChanged
        If txtSerialNo.Text = "" Then
            lblSerialNo1.Text = ""
        Else
            lblSerialNo1.Text = "Serial No. : " & " " & txtSerialNo.Text
        End If
    End Sub
    Private Sub imgbtnModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnModels.Click
        SetSettings()
        mMacList = Nothing
        mMacList = AssemblyList.GetAssemblyList(txtModel.Text, txtSerialNo.Text, CInt(cmbAssemblyType.SelectedValue.ToString), "{00000000-0000-0000-0000-000000000000}", Now.Date.ToString)
        Session("mMacList") = mMacList
        Response.Redirect("wfSearchCriteriaForModel.aspx?BackPage=Index.aspx")
    End Sub
    Private Sub SetSettings()
        ''Session("DateIndex") = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        Session("FromDate") = txtFromDate.Text.ToString
        Session("ToDate") = txtToDate.Text.ToString
        Session("GroupIndex") = IIf(cmbGraphGroupType.SelectedIndex <= 0, 0, cmbGraphGroupType.SelectedIndex)
        Session("AssemblyTypeIndex") = IIf(cmbAssemblyType.SelectedIndex <= 0, 0, cmbAssemblyType.SelectedIndex)
        Session("SerialNo") = txtSerialNo.Text
    End Sub
    Private Sub GetSettings()
        ''cmbDateRange.SelectedIndex = Session("DateIndex")
        ''setDatePeroid(Session("DateIndex"))
        ''ControlVisibility(Session("DateIndex"))

        SetDatePeroid(4)
        txtFromDate.Text = Session("FromDate")
        txtToDate.Text = Session("ToDate")

        cmbGraphGroupType.SelectedIndex = Session("GroupIndex")
        cmbAssemblyType.SelectedIndex = Session("AssemblyTypeIndex")

        txtSerialNo.Text = Session("SerialNo")
    End Sub
    'Added by Saylee on 3rd-Aug-2009
    Private Sub cmbModelList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbModelList.SelectedIndexChanged
        mSelectPeriods = SelectPeriods.NewSelectPeriods
        mAssemblyStatusID = ReportFetchAssemblyStatusInfo.GetReportFetchAssemblyStatusInfo(Today.ToShortDateString, New Guid(cmbModelList.SelectedValue)).AssemblyStatusID
        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mAssemblyStatusID)
        Dim i As Integer = 0
        While i <= mAssemblyStatus.AssemblyStatusPeriods.Count - 1
            If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 Then
                mSelectPeriods.Add(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID, mAssemblyStatus.AssemblyStatusPeriods(i).PeriodName)
            End If
            i = i + 1
        End While
        Session("mSelectPeriods") = mSelectPeriods
        Session("sender") = "SelectPeriods"

        cmbPeriods.DataSource = mSelectPeriods
        cmbPeriods.DataBind()

        ' cmbAssemblyType.SelectedIndex = Session("AssemblyTypeIndex")
        'cmbGraphGroupType.SelectedIndex = Session("GroupIndex")
    End Sub
    'Added by Saylee on 3rd-Aug-2009
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    '    cmbGraphGroupType.Visible = Not CType(sender, Boolean)
    '    cmbAssemblyType.Visible = Not CType(sender, Boolean)
    '    cmbPeriods.Visible = Not CType(sender, Boolean)
    'End Sub
    'Private Sub txtToDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.CalendarVisibleChanged
    '    cmbModelList.Visible = Not CType(sender, Boolean)
    'End Sub
    'Added by Archana on 10-Aug-09
    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        If cmbGraphGroupType.SelectedValue = 2 Then     'Day
            txtFromDate.Text = CDate(txtToDate.Text).AddDays(1).AddMonths(-1)
        End If
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class