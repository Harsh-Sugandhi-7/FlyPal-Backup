
'CREATED By : Saylee
'Dated      : 19-Mar-2014

Public Class wfComponentLogBook_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim mMachineNameValueList As MachineNameValueList
    'Dim mAssemblylist As AssemblyList
    Public mAssemblyStatusList As AssemblyStatusList
    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName, AssemblyName As String
    Dim MachineID As String
    Dim AssemblyID, CompID As String
    Dim Aircraft As String
    Dim AssemblyType As String
    Dim AssemblyText, ComponentText As String
    Dim Model As String
    Dim SerialNo As String
    Dim mCompListForCompBook As CompListForComboBox

    Dim RegNo, SerialNoPosition As String
    Dim da As New CSLA.Data.ObjectAdapter
    Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
    Dim mCompanyDetail As New CompanyDetail

    Dim objCompLogRegister As New ComponentLogBook

    Dim dsComponentLogBook As New dsComponentLogBook
    Dim LogType As Integer

    Dim AssemblyTypeID As Integer

    Dim EventLogID As Guid
    Dim mLogBookSearchingCriteria As String = String.Empty
    Dim AOnDate, AOdate As String

    Dim mComponentDetails As ComponentDetails
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)
        AOnDate = Session("AOnDate")
        mCompListForCompBook = Session("mCompListForCompBook")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfComponentLogBook_AJAX.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblyStatusList")
            Session.Remove("mCompListForCompBook")
        End If
    End Sub
    Private Sub SetSession()

    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
        upnlDetails.Update()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblAssembly1.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblComponent1.Visible = True
    End Sub
    Private Sub ControlVisibility()

    End Sub
    Private Sub SetValues()
        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)
        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = txtFromDate.Text.ToString
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = txtToDate.Text.ToString
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If cmbAircraft.SelectedIndex > 0 Then
            AssemblyText = IIf(cmbAircraftAssembly.SelectedIndex > -1, cmbAircraftAssembly.SelectedItem.Text, "")
            MachineID = cmbAircraft.SelectedValue.ToString
            AssemblyID = cmbAircraftAssembly.SelectedValue.ToString
            AssemblyType = mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).AssemblyType
            SerialNo = mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).SerialNo
            Model = mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).Model
            'RegNo = mMachineList(cmbAircraft.SelectedIndex).RegNo  'Commented By Utkarsh On 19-Apr-2011
            RegNo = mMachineNameValueList(cmbAircraft.SelectedIndex).RegNo  'Added By Utkarsh On 19-Apr-2011
            AssemblyTypeID = mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).AssemblyTypeID

            If cmbComponent.SelectedIndex > -1 Then
                ComponentText = IIf(cmbComponent.SelectedIndex > -1, cmbComponent.SelectedItem.Text, "")
                CompID = cmbComponent.SelectedValue.ToString
            End If
        Else
            AssemblyText = ""
            ComponentText = ""
        End If
        lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
        lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
        lblAssembly1.Text = "Assembly : " & IIf(AssemblyText <> "", AssemblyText, "")

        lblComponent1.Text = "Component : " & IIf(ComponentText <> "", ComponentText, "")

        mLogBookSearchingCriteria = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + lblComponent1.Text


    End Sub
    Private Sub ResetValues()
        StartDate = txtFromDate.Text.ToString
        EndDate = txtToDate.Text.ToString
        MachineID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyID = "{00000000-0000-0000-0000-000000000000}"
        AssemblyType = ""
        Aircraft = ""
        AssemblyText = ""
        AssemblyTypeID = 1
        ComponentText = ""
    End Sub
    Private Sub SetReport()
        Dim serchstr7 As String  'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 

        Dim ReportName As String = ""
        SetValues()
        Dim str1 As String = ""

        mMachineList = MachineList.GetMachineListMonitoringStatus(Now.ToShortDateString, , , , , , , , , , , True, True, , cmbAircraftAssembly.SelectedValue, , , , , , , mCompListForCompBook(cmbComponent.SelectedIndex).CompID.ToString, , , , , , , , , , , , , , , , , , , , True, "(SELECT)", SkipIsForInventoryAircarft:=True)
        '***********************************
        If mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).Position <> "" Then
            SerialNoPosition = mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).SerialNo + "(" + mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).Position + ")"
        Else
            SerialNoPosition = mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).SerialNo
        End If

        mMachineList = MachineList.GetMachineListMonitoringStatus(txtFromDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , , True, , , "Airframe", , , , , , , , , , , , , , , , , , , , , , , , , , True, "(SELECT)", SkipIsForInventoryAircarft:=True)


        myReport = New crComponentLogBook

        Dim tmpDate As Date
        tmpDate = DateAdd(DateInterval.Day, -1, Date.Parse(txtFromDate.Text.ToString))


        mComponentDetails = ComponentDetails.GetComp(txtFromDate.Text.ToString, cmbAircraft.SelectedValue.ToString, cmbAircraftAssembly.SelectedValue.ToString, cmbComponent.SelectedValue.ToString)

        objCompLogRegister = ComponentLogBook.GetComponentLogBookRegister(StartDate, EndDate, New Guid(cmbAircraft.SelectedValue.ToString), mAssemblyStatusList(cmbAircraftAssembly.SelectedIndex).AssemblyID, New Guid(cmbComponent.SelectedValue.ToString), AssemblyTypeID, chkShowCompliance.Checked, chkShowPirepsMELSnag.Checked, chkShowMaintActivity.Checked, chkShowInstRem.Checked, chkWithAssembly.Checked, AppSettings("MELSnagNomenclature").ToString)



        If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
            If cmbAircraft.SelectedIndex > 0 Then
                serchstr7 = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue)).OperatorName
            Else
                serchstr7 = ""
            End If
        Else
            serchstr7 = ""
        End If

        'End
        Dim CurrentValue As String = ""

        If objCompLogRegister.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            CurrentValue = objCompLogRegister(0).CurrentValue
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 630)
            '*******************************
        End If

        ReportStatusList.Add(New rptStatus(, 0, New SmartDate(StartDate).FormattedText + " " + "To" + " " + New SmartDate(EndDate).FormattedText, AssemblyType + " " + "Details", , , _
            mMachineList(New Guid(cmbAircraft.SelectedValue)).RegNo, , mComponentDetails(0).PartName, mComponentDetails(0).CompSerialNo, mComponentDetails(0).Desc, CurrentValue, mComponentDetails(0).SinceOHFormatted))

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
            mCompanyDetail.WebSite, "Component Log Register", New SmartDate(StartDate).FormattedText, New SmartDate(EndDate).FormattedText, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", serchstr7, "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(dsComponentLogBook)
        '----------------------------------------------------------

        da.Fill(dsComponentLogBook, objCompLogRegister)
        da.Fill(dsComponentLogBook, Report)
        da.Fill(dsComponentLogBook, ReportStatusList)
        da.Fill(dsComponentLogBook, mrptImage) 'Added by Utkarsh for Report Logo)
        myReport.SetDataSource(dsComponentLogBook)
        Session("CrystalReport") = myReport
        str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str1, True)
        MarkLog(Util.Action.Print, "ComponentLogBook", mLogBookSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

        ResetValues()
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
                    Session("LogType") = LogType
                    Response.Redirect("wfSearchCriteriaForLogBook_Ajax.aspx?LogType=" + CStr(LogType))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfSearchCriteriaForLogBook_Ajax.aspx?LogType=" + CStr(LogType))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbAircraftAssembly" Then
            If cmbComponent.Enabled = False Then
                custValidator.ErrorMessage = "Please select the Component"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If

    End Sub
    Private Sub DataFieldBind()
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

            Session("MiddleFrame") = "wfComponentLogBook_AJAX.aspx?"
            ResetValues()
            lblAssembly.Enabled = False
            cmbAircraftAssembly.Enabled = False
            cmbComponent.Enabled = False
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            Session("AOnDate") = AOnDate
            SetComboOfMachine(AOnDate)
            DataFieldBind()
        End If
        ControlVisibility()

    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

        If IsValid = True Then

            If chkShowCompliance.Checked = False And chkShowInstRem.Checked = False And chkShowMaintActivity.Checked = False And chkShowPirepsMELSnag.Checked = False Then
                MSGBoxCtrl.show("Selection Alert!", "Select atleast one Maintenance Activity.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If


            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mMachineNameValueList = Nothing 'Added By Utkarsh On 19-Apr-2011
        mAssemblyStatusList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAircraftAssembly.Enabled = False
            lblComponent.Enabled = False
            cmbComponent.Enabled = False
        Else
            lblAssembly.Enabled = True
            cmbAircraftAssembly.Enabled = True
            MachineName = cmbAircraft.SelectedValue.ToString
            ''Dim mAssemblylist As AssemblyList
            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtFromDate.Text, cmbAircraft.SelectedValue, , , , , , , , , , True, SkipIsForInventoryAircarft:=True).Item(0), MachineInfo).AssemblyStatusList

            ''mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, , True)
            Session("mAssemblyStatusList") = mAssemblyStatusList
            cmbAircraftAssembly.DataSource = mAssemblyStatusList
            cmbAircraftAssembly.DataBind()



            AssemblyName = cmbAircraftAssembly.SelectedValue.ToString
            ''mCompListForCompBook = tmpInstalledCompList.GetInstalledCompList(txtFromDate.Text.ToString, cmbAircraft.SelectedValue.ToString, "", "", New Guid(cmbAircraftAssembly.SelectedValue.ToString))
            ''Session("mCompListForCompBook") = mCompListForCompBook
            ''cmbComponent.DataSource = mCompListForCompBook
            ''cmbComponent.DataBind()


            mCompListForCompBook = CompListForComboBox.GetCompList(txtFromDate.Text.ToString, cmbAircraft.SelectedValue.ToString, cmbAircraftAssembly.SelectedValue.ToString)
            cmbComponent.DataSource = mCompListForCompBook
            cmbComponent.DataBind()
            Session("mCompListForCompBook") = mCompListForCompBook

            If mCompListForCompBook.Count > 0 Then
                lblComponent.Enabled = True
                cmbComponent.Enabled = True
            Else
                lblComponent.Enabled = False
                cmbComponent.Enabled = False
            End If
        End If
        upnlDetails.Update()
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
    End Sub

    Private Sub cmbAircraftAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraftAssembly.SelectedIndexChanged


        AssemblyName = cmbAircraftAssembly.SelectedValue.ToString

        mCompListForCompBook = CompListForComboBox.GetCompList(txtFromDate.Text.ToString, cmbAircraft.SelectedValue.ToString, cmbAircraftAssembly.SelectedValue.ToString)
        cmbComponent.DataSource = mCompListForCompBook
        cmbComponent.DataBind()
        Session("mCompListForCompBook") = mCompListForCompBook

        If mCompListForCompBook.Count > 0 Then
            lblComponent.Enabled = True
            cmbComponent.Enabled = True
        Else
            lblComponent.Enabled = False
            cmbComponent.Enabled = False
        End If
        upnlDetails.Update()
        If cmbAircraftAssembly.Enabled = True Then
            setFocus(cmbAircraftAssembly)
        End If
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text.Trim


        If AOnDate = AOdate Then
        Else


            'If Date.TryParse(txtFromDate.Text.Trim, tmpdate) Then
            SetComboOfMachine(AOdate)
            lblAssembly.Enabled = False
            cmbAircraftAssembly.Enabled = False
            mAssemblyStatusList = Nothing
            Session("mAssemblyStatusList") = mAssemblyStatusList
            cmbAircraftAssembly.ClearSelection()
            cmbAircraftAssembly.DataSource = mAssemblyStatusList
            cmbAircraftAssembly.Controls.Clear()
            cmbAircraftAssembly.DataBind()

            cmbComponent.Enabled = False
            mCompListForCompBook = Nothing
            Session("mCompListForCompBook") = mCompListForCompBook
            cmbComponent.ClearSelection()
            cmbComponent.DataSource = mCompListForCompBook
            cmbComponent.Controls.Clear()
            cmbComponent.DataBind()
            upnlDetails.Update()
            DataFieldBind()
            'End If
        End If
        upnlDate.Update()
        upnlDetails.Update()
    End Sub

#End Region

End Class