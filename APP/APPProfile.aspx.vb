Imports System.Drawing
Public Class APPProfile
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Dim mUser As System.Security.Principal.IPrincipal
    Dim mGBUser As SI.UTILITY.User
    ''Dim mEventLogSession As EventLogSetSession

    ''Dim mCrew As Crew
    'Dim mCrewList As CrewList
    ''Dim mApp_CrewList As App_CrewList
    ''Dim mOperatorList As OperatorList
    Dim EventLogID As Guid

#End Region

#Region "Helper Method"

    Private Sub GetSession()

        mUser = Session("User")
        mGBUser = Session("GBUser")
        'mEventLogSession = Session("EventLogSession")

        'mApp_CrewList = Session("APP_APPProfile.CrewList")
        'mOperatorList = Session("APP_APPProfile.OperatorList")

    End Sub

    Private Sub LoadCombos()

        'If CBool(AppsSettings("OperatorManagement")) Then

        '    mOperatorList = OperatorList.GetOperatorList(mGBUser.UserID, , "(Select)")
        '    cmbOperatorList.DataSource = mOperatorList
        '    cmbOperatorList.DataBind()

        '    If mGBUser.UserTypeID <> 1 Then  '1 - Admin , 2- Crew , 3 - Operator
        '        cmbOperatorList.SelectedValue = mGBUser.UserOperators(0).OperatorID.ToString
        '        cmbOperatorList.Enabled = False

        '        cmbCrewList.Enabled = (mGBUser.UserTypeID <> 2)
        '    Else
        '        cmbOperatorList.Enabled = True
        '        cmbCrewList.Enabled = False
        '    End If

        'Else

        '    mOperatorList = OperatorList.GetOperatorList(mGBUser.UserID, , "")
        '    cmbOperatorList.DataSource = mOperatorList
        '    cmbOperatorList.DataBind()

        '    cmbOperatorList.SelectedValue = mGBUser.UserOperators(0).OperatorID.ToString
        '    DivOperator.Visible = False

        '    cmbCrewList.Enabled = (mGBUser.UserTypeID <> 2)

        'End If

        '' cmbCrewList.Enabled = (mGBUser.UserTypeID <> 2) 

        'Session("APP_APPProfile.OperatorList") = mOperatorList

        ''2-All , 1 - Left , 0 - In Service
        'mApp_CrewList = App_CrewList.GetApp_CrewList(0, "", "(Select)", IIf(cmbCrewList.SelectedIndex <= 0, Guid.Empty.ToString, cmbCrewList.SelectedValue), mGBUser.UserID.ToString, cmbOperatorList.SelectedValue, False, )
        ''mCrewList = CrewList.GetCrewList(0, , "(Select)", , , mGBUser.UserID.ToString, cmbOperatorList.SelectedValue.ToString)
        'cmbCrewList.DataSource = mApp_CrewList
        'cmbCrewList.DataBind()

        'Session("APP_APPProfile.CrewList") = mApp_CrewList


    End Sub

    Private Sub GetData()

        'If mGBUser.UserTypeID = 1 And cmbCrewList.SelectedIndex > 0 Then  'Admin

        '    mCrew = Crew.GetCrew(New Guid(cmbCrewList.SelectedValue))

        'ElseIf mGBUser.UserTypeID = 2 Then   'Crew

        '    mCrew = Crew.GetCrew(mGBUser.UserCrews(0).CrewID)
        '    BindData()

        'ElseIf mGBUser.UserTypeID = 3 Then   'Operator

        '    If cmbCrewList.SelectedIndex > 0 Then
        '        mCrew = Crew.GetCrew(New Guid(cmbCrewList.SelectedValue))
        '    End If

        'End If

        'If Not mCrew Is Nothing Then

        '    Dim mApp_CrewProfile_FDTL As App_CrewProfile_FDTL = App_CrewProfile_FDTL.GetApp_CrewProfile_FDTL(mCrew.ID.ToString, mGBUser.UserID.ToString, mCrew.OperatorID.ToString)
        '    'Dim Xaxis As String = "24hrs;7 days;14 days;28 days;90 days;365 days"
        '    'Dim Series As String
        '    'With mApp_CrewProfile_FDTL(0)
        '    '    Series = Format((.Last24hrs_FDT / 60), "#0.00") + ";" + Format((.Last7Days_FDT / 60), "#0.00") + ";" _
        '    '            + Format((.Last14Days_FDT / 60), "#0.00") + ";" + Format((.Last30Days_FDT / 60), "#0.00") + ";" _
        '    '            + Format((.Last90Days_FDT / 60), "#0.00") + ";" + Format((.Last365Days_FDT / 60), "#0.00")
        '    'End With
        '    'Dim str As String = "DrawChart('" & Xaxis & "','" & Series & "');"
        '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)

        '    '--------------------------------


        '    For i As Integer = 0 To 5

        '        If i = 0 Then

        '            Chart1.Series("FT").Points.AddXY("Last 24 Hrs", Format((mApp_CrewProfile_FDTL(0).Last24hrs_FT / 60), "#0.00"))
        '            Chart1.Series("FT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last24Hrs_FT_Formatted

        '            Chart1.Series("FDT").Points.AddXY("Last 24 Hrs", Format((mApp_CrewProfile_FDTL(0).Last24hrs_FDT / 60), "#0.00"))
        '            Chart1.Series("FDT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last24Hrs_FDT_Formatted

        '        ElseIf i = 1 Then

        '            Chart1.Series("FT").Points.AddXY("Last 7 days", Format((mApp_CrewProfile_FDTL(0).Last7Days_FT / 60), "#0.00"))
        '            Chart1.Series("FT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last7days_FT_Formatted

        '            Chart1.Series("FDT").Points.AddXY("Last 7 days", Format((mApp_CrewProfile_FDTL(0).Last7Days_FDT / 60), "#0.00"))
        '            Chart1.Series("FDT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last7Days_FDT_Formatted

        '        ElseIf i = 2 Then

        '            Chart1.Series("FT").Points.AddXY("Last 14 days", Format((mApp_CrewProfile_FDTL(0).Last14Days_FT / 60), "#0.00"))
        '            Chart1.Series("FT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last14days_FT_Formatted

        '            Chart1.Series("FDT").Points.AddXY("Last 14 days", Format((mApp_CrewProfile_FDTL(0).Last14Days_FDT / 60), "#0.00"))
        '            Chart1.Series("FDT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last14Days_FDT_Formatted

        '        ElseIf i = 3 Then

        '            Chart1.Series("FT").Points.AddXY("Last 28 days", Format((mApp_CrewProfile_FDTL(0).Last30Days_FT / 60), "#0.00"))
        '            Chart1.Series("FT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last30days_FT_Formatted

        '            Chart1.Series("FDT").Points.AddXY("Last 28 days", Format((mApp_CrewProfile_FDTL(0).Last30Days_FDT / 60), "#0.00"))
        '            Chart1.Series("FDT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last30Days_FDT_Formatted

        '        ElseIf i = 4 Then

        '            Chart1.Series("FT").Points.AddXY("Last 90 days", Format((mApp_CrewProfile_FDTL(0).Last90Days_FT / 60), "#0.00"))
        '            Chart1.Series("FT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last90days_FT_Formatted

        '            Chart1.Series("FDT").Points.AddXY("Last 90 days", Format((mApp_CrewProfile_FDTL(0).Last90Days_FDT / 60), "#0.00"))
        '            Chart1.Series("FDT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last90Days_FDT_Formatted

        '        ElseIf i = 5 Then

        '            Chart1.Series("FT").Points.AddXY("Last 365 days", Format((mApp_CrewProfile_FDTL(0).Last365Days_FT / 60), "#0.00"))
        '            Chart1.Series("FT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last365days_FT_Formatted

        '            Chart1.Series("FDT").Points.AddXY("Last 365 days", Format((mApp_CrewProfile_FDTL(0).Last365Days_FDT / 60), "#0.00"))
        '            Chart1.Series("FDT").Points(i).ToolTip = mApp_CrewProfile_FDTL(0).Last365Days_FDT_Formatted

        '        End If

        '        Chart1.ChartAreas("ChartArea1").AxisY.TitleFont = New System.Drawing.Font("Arial", 20, FontStyle.Bold)

        '        Chart1.Series("FT").SmartLabelStyle.Enabled = False
        '        Chart1.Series("FDT").SmartLabelStyle.Enabled = False

        '        Chart1.Series("FT").LabelAngle = -90
        '        Chart1.Series("FDT").LabelAngle = -90
        '    Next

        'End If



    End Sub

    Private Sub ApplyRights()
        Try

            'BottomMenu Rights
            '          
            If User.IsInRole("AircraftCurrentStatusView") = False Then '
                hrefTimeline.Attributes("style") = "pointer-events: none"
                iTimeline.Attributes.Remove("style")
            End If

            ''Flights
            If User.IsInRole("LogView") = False Then
                hrefFlights.Attributes("style") = "pointer-events: none"
                iFlights.Attributes.Remove("style")
            End If

            ''Availability
            ''
            ''
            'hrefAvailability.Attributes("style") = "pointer-events: none"
            'iAvailability.Attributes.Remove("style")

            ''Profile
            'If (mUser.IsInRole("CrewNew") Or mUser.IsInRole("CrewEdit") Or mUser.IsInRole("CrewDelete") Or mUser.IsInRole("CrewView") Or mUser.IsInRole("CrewPrint")) = False Then
            '    hrefProfile.Attributes("style") = "pointer-events: none"
            '    iProfile.Attributes.Remove("style")
            'End If

            ''-----
            'Profile
            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")


        Catch ex As Exception

        End Try

    End Sub

    Private Sub ShowAlertMsg(ByVal Msg As String, ByVal MsgTitle As String)


        Dim str As String
        str = "opennotificationpopup('" & Msg & "','" & MsgTitle & "');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str, True)

    End Sub

    Private Sub DisableButtons()
        Try

            hrefTimeline.Attributes("style") = "pointer-events: none"
            iTimeline.Attributes.Remove("style")

            hrefFlights.Attributes("style") = "pointer-events: none"
            iFlights.Attributes.Remove("style")

            hrefAvailability.Attributes("style") = "pointer-events: none"
            iAvailability.Attributes.Remove("style")

            hrefProfile.Attributes("style") = "pointer-events: none"
            iProfile.Attributes.Remove("style")


        Catch ex As Exception

        End Try



    End Sub

#End Region

#Region "Data Bind"

    Private Sub BindData()

        'If Not mCrew Is Nothing Then

        '    With mCrew

        '        If CBool(AppsSettings("OperatorManagement")) Then
        '            cmbOperatorList.SelectedValue = mCrew.OperatorID.ToString
        '        Else
        '            cmbOperatorList.SelectedValue = mGBUser.UserOperators(0).OperatorID.ToString
        '        End If

        '        cmbCrewList.SelectedValue = IIf(.ID.Equals(Guid.Empty), "", .ID.ToString)
        '        txtCode.Text = .Code
        '        txtNationality.Text = .NationalityName
        '        txtCity.Text = .BaseCityName.ToString
        '        txtDesignation.Text = .DesignationName
        '        txtOnDutyAs.Text = .OnDutyAsName
        '        txtMobile.Text = .MobileNo
        '        txtEmail.Text = .Email
        '        txtDateOfBirth.Text = IIf(IsNothing(.DateOfBirth) Or IsDBNull(.DateOfBirth), "", .DateOfBirth)
        '        txtPassport.Text = .Passport
        '        txtDateOfJoining.Text = IIf(IsNothing(.DateOfJoining) Or IsDBNull(.DateOfJoining), "", .DateOfJoining)

        '        chkNotInSerVice.Checked = .IsLeft

        '        If .IsLeft Then
        '            txtDateOfNotInService.Text = IIf(IsNothing(.LeftDate) Or IsDBNull(.LeftDate), "", .LeftDate)
        '        End If

        '        rdbMale.Checked = IIf(.Gender.Equals(1), True, False)
        '        rdbFemale.Checked = IIf(.Gender.Equals(2), True, False)

        '        grdApplicableModels.DataSource = .CrewModels
        '        grdApplicableModels.DataBind()

        '        grdCriticalAirports.DataSource = .CrewCriticalAirports
        '        grdCriticalAirports.DataBind()


        '    End With

        'End If



        'upnlProfile.Update()

    End Sub

    Private Sub EmptyData()

        Try

            txtCode.Text = ""
            txtNationality.Text = ""
            txtCity.Text = ""
            txtDesignation.Text = ""
            txtOnDutyAs.Text = ""
            txtMobile.Text = ""
            txtEmail.Text = ""
            txtDateOfBirth.Text = ""
            txtPassport.Text = ""
            txtDateOfJoining.Text = ""

            chkNotInSerVice.Checked = False
            txtDateOfNotInService.Text = ""

            rdbMale.Checked = False
            rdbFemale.Checked = False

            grdApplicableModels.DataSource = Nothing
            grdApplicableModels.DataBind()

            grdCriticalAirports.DataSource = Nothing
            grdCriticalAirports.DataBind()



        Catch ex As Exception

        End Try
    End Sub
#End Region


#Region "Events"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Try
            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)
            If mGBUser Is Nothing Then
                ShowAlertMsg("Session expired ! Please click on Home button on left side menu.", "Session expired..")
                DisableButtons()
                Exit Sub
            End If


            If Not IsPostBack Then

                LoadCombos()
                GetData()
                BindData()
                ''lblCorporateID.Text = " (" + mRegInformation.CorporateID + ")"

            End If

            ApplyRights()

            txtDateOfBirth.Enabled = False
            txtDateOfJoining.Enabled = False
            txtDateOfNotInService.Enabled = False
            txtPassport.Enabled = False
            txtMobile.Enabled = False


        Catch ex As Exception
            ShowAlertMsg(ex.Message, "Error")
        End Try


    End Sub

    Protected Sub cmbCrewList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbCrewList.SelectedIndexChanged
        Try

            GetData()
            BindData()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub cmbOperatorList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbOperatorList.SelectedIndexChanged
        Try

            'cmbCrewList.SelectedIndex = CInt(0)

            ''2-All , 1 - Left , 0 - In Service
            'mApp_CrewList = App_CrewList.GetApp_CrewList(0, "", "(Select)", IIf(cmbCrewList.SelectedIndex <= 0, Guid.Empty.ToString, cmbCrewList.SelectedValue), mGBUser.UserID.ToString, cmbOperatorList.SelectedValue, False, )

            'Session("APP_APPProfile.CrewList") = mApp_CrewList

            'If cmbOperatorList.SelectedIndex > 0 Then
            '    cmbCrewList.Enabled = True

            '    cmbCrewList.DataSource = mApp_CrewList
            '    cmbCrewList.DataBind()

            'Else

            '    cmbCrewList.Enabled = False
            '    cmbCrewList.SelectedIndex = CInt(0)

            '    EmptyData()

            'End If

        Catch ex As Exception

        End Try

    End Sub

    'Protected Sub lnkSearch_Click(sender As Object, e As System.EventArgs) Handles lnkSearch.Click
    '    Try
    '        GetData()
    '        BindData()

    '    Catch ex As Exception

    '    End Try
    'End Sub

    Protected Sub lnkHome_Click(sender As Object, e As System.EventArgs) Handles lnkHome.Click
        Try
            Response.Redirect("APPMenu.aspx?Username=" + mGBUser.Name + "&EventLogSessionID=" + EventLogID.ToString)
        Catch ex As Exception

        End Try

    End Sub


#End Region

End Class