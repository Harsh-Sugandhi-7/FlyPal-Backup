'************************************
'CREATED By : Harsh Sugandhi
'Dated      : 28th July 2025
'For        : FLYPAL-2572 Engine Power Derate option in Flight Log.
'************************************


Public Class SearchCriteriaForEngineDerateRegister
    Inherits Page

#Region " Variable Declaration "

    Dim StartDate As String
    Dim EndDate As String
    Dim MachineName As String
    Dim MachineID As String
    Dim RegNo As String
    Dim EnginePowerDerate As String
    Dim EventLogID As Guid
    Dim EngineDerateSearchingCriteria As String = String.Empty
    Public LogType As Integer
    Dim TodaysDate, ChangedFromDate As String
    Dim AMPNoStr As String = ""
    Public ModuleName As String = "Engine Derate Register"

    Dim ReportStatusList As New rptStatusList
    Dim MachineList As MachineList
    Dim MachineNameValueList As MachineNameValueList
    Dim dataAdapter As New ObjectAdapter
    Dim CrystalReport As Engine.ReportClass
    Dim CompanyDetail As New CompanyDetail
    Dim dsLogRegister As New dsLogRegister
    Dim LastMPDAMPRef As LastMPDAMPRef
    Public EngineDerate As EngineDerate

#End Region

#Region " Helper Methods "

    Private Sub GetSession()

        Try

            MachineNameValueList = CType(Session("MachineNameValueList"), MachineNameValueList)
            LogType = Session("LogType")
            EngineDerate = CType(Session("EngineDerate"), EngineDerate)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ClearAll()

        Try

            LogType = Session("LogType")

            If Session("MiddleFrame") <> "wfSearchCriteriaForEngineDerateRegister.aspx" Then
                Session.Remove("MachineNameValueList")
                Session.Remove("AssemblyList")
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetSession()

        Try

            Session("EngineDerate") = EngineDerate

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        Try

            If control.Enabled = False Or control.Visible = False Then Exit Sub
            Dim str As String
            str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
            ClientScript.RegisterStartupScript([GetType], "Focus On Control", str)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ControlVisibility()

        Try

            If LogType = 1 Then

            Else

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetValues()

        Try

            If Not IsDate(txtFromDate.Text) Then
                StartDate = New SmartDate(Today.ToString()).FormattedText
                lblSearchCriteriaDateRangeFrom.Text = $"From Date : "
            Else
                StartDate = txtFromDate.Text.ToString
                lblSearchCriteriaDateRangeFrom.Text = $"From Date :  {New SmartDate(txtFromDate.Text.ToString()).FormattedText}"
            End If

            If Not IsDate(txtToDate.Text) Then
                EndDate = New SmartDate(Today.ToString()).FormattedText
                lblSearchCriteriaDateRangeTo.Text = $"To Date : "
            Else
                EndDate = txtToDate.Text.ToString
                lblSearchCriteriaDateRangeTo.Text = $"To Date : {New SmartDate(txtToDate.Text.ToString()).FormattedText}"
            End If

            If ddlAircraft.SelectedIndex > 0 Then
                MachineID = ddlAircraft.SelectedValue.ToString
                RegNo = MachineNameValueList(ddlAircraft.SelectedIndex).RegNo
                lblSearchCriteriaAircraft.Text = $"Aircraft : {ddlAircraft.SelectedItem.Text}"
            Else
                MachineID = ""
                RegNo = ""
                lblSearchCriteriaAircraft.Text = $"Aircraft : ALL"
            End If

            If ddlEngineDerate.SelectedIndex > 0 Then
                EnginePowerDerate = ddlEngineDerate.SelectedValue.ToString
                lblSearchCriteriaEngineDerate.Text = $"Engine Derate : {ddlEngineDerate.SelectedItem.Text}"
            Else
                EnginePowerDerate = ""
                lblSearchCriteriaEngineDerate.Text = $"Engine Derate : ALl"
            End If

            EngineDerateSearchingCriteria = lblSearchCriteriaDateRangeFrom.Text.Trim() +
                                             ", " + lblSearchCriteriaDateRangeTo.Text.Trim() +
                                             ", " + lblSearchCriteriaAircraft.Text.Trim() +
                                             ", " + lblSearchCriteriaEngineDerate.Text.Trim()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ResetValues()

        Try

            StartDate = txtFromDate.Text.ToString
            EndDate = txtToDate.Text.ToString
            MachineID = "{00000000-0000-0000-0000-000000000000}"

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DisplayReportInPDF()

        Dim ReportName, OperatorName As String
        ReportName = "Engine Derate Register"
        Dim AssemblyList As AssemblyList
        Try

            SetValues()

            CrystalReport = New crEngineDerateRegister

            If AppSettings("ShowMaintenanceForNewClients") = "True" Then

                LastMPDAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(MachineID:=New Guid(ddlAircraft.SelectedValue.ToString))

                If (LastMPDAMPRef.AMPNo <> "") Then
                    AMPNoStr = "AMP No.: " + LastMPDAMPRef.AMPNo + ", Rev No.: " + LastMPDAMPRef.RevNo + ", Dated: " + LastMPDAMPRef.FromDateFormatted
                Else
                    AMPNoStr = ""
                End If

            Else
                AMPNoStr = ""
            End If

            If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then

                If ddlAircraft.SelectedIndex > 0 Then
                    OperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(ddlAircraft.SelectedValue)).OperatorName
                Else
                    OperatorName = ""
                End If

            Else
                OperatorName = ""
            End If


            AssemblyList = AssemblyList.GetAssemblyListForComboBox(1,
                                                                   ddlAircraft.SelectedValue,
                                                                   txtFromDate.Text.ToString, ,
                                                                   True)
            Session("AssemblyList") = AssemblyList

            Dim ReportData As New ReportData(CompanyName:=CompanyDetail.CompanyName,
                                             Address:=CompanyDetail.Address,
                                             Tel1:=CompanyDetail.Tel1,
                                             Tel2:=CompanyDetail.Tel2,
                                             Fax:=CompanyDetail.Fax,
                                             Email:=CompanyDetail.Email,
                                             WebSite:=CompanyDetail.WebSite,
                                             ReportName:=ReportName,
                                             ProductVersion:=AppSettings("Product Version"),
                                             SINote:=AppSettings("SINote"),
                                             SearchStr1:=$"{New SmartDate(StartDate).FormattedText}  To  {New SmartDate(EndDate).FormattedText}",
                                             SearchStr2:=False,
                                             SearchStr3:=(ddlAircraft.SelectedItem.Text.ToString),
                                             SearchStr4:="False",
                                             SearchStr5:="",
                                             SearchStr6:="",
                                             SearchStr7:=OperatorName,
                                             SearchStr8:="",
                                             SearchStr9:=rdbLocal.Checked.ToString,
                                             SearchStr10:=AppSettings("Logo"),
                                             SearchStr11:=AMPNoStr)

            Dim ReportLogRegister As ReportLogRegister = ReportLogRegister.
                                                            GetLogRegister(StartDate:=StartDate,
                                                                           EndDate:=EndDate,
                                                                           AssemblyID:=AssemblyList(0).ID.ToString,
                                                                           MachineID:=MachineID,
                                                                           CalculateTotal:=True,
                                                                           FlightLogClassificationName:="",
                                                                           StatusSelectLog:=0,
                                                                           IsLogNo:=False,
                                                                           IsLogPageNo:=True,
                                                                           IsFlightNo:=False,
                                                                           SkipVoidLog:=True,
                                                                           SkipMaintLog:=True,
                                                                           IsFlightLogClassification:=False,
                                                                           GetLogPeriodsDayWise:=False,
                                                                           ShowSinceTSO:=False,
                                                                           IsUTC:=IIf(rdbUTC.Checked,
                                                                                      True,
                                                                                      False),
                                                                           IsForLogBook:=True,
                                                                           EngineDerateID:=CInt(ddlEngineDerate.SelectedValue))

            If ReportLogRegister.Count = 0 Then

                MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "There are no records for this search criteria",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            End If

            Dim DerateInfo As EngineDerateRegisterReport = EngineDerateRegisterReport.
                                                            GetDerateWiseTotalLogsCount(LogRegister:=ReportLogRegister)

            Dim companyLogo As rptImage = rptImage.GetImage(dsLogRegister)
            dataAdapter.Fill(dsLogRegister, "ReportLogRegister", ReportLogRegister)
            dataAdapter.Fill(dsLogRegister, "EngineDerateRegister", DerateInfo)
            dataAdapter.Fill(dsLogRegister, "ReportData", ReportData)

            CrystalReport.SetDataSource(dsLogRegister)
            Session("CrystalReport") = CrystalReport

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Display Report In PDF",
                                                "displayReportInPDF();",
                                                True)

            MarkLog(Action.Print,
                    "Engine Derate Register",
                    EngineDerateSearchingCriteria,
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

            ResetValues()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DisplaySearchCriteria()

        Try

            lblSearchCriteriaDateRangeFrom.Visible = True
            lblSearchCriteriaDateRangeTo.Visible = True
            lblSearchCriteriaAircraft.Visible = True
            lblSearchCriteriaEngineDerate.Visible = True

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MessageBoxResult()

        Try

            Dim MsgBoxResult As MsgBoxResult
            MsgBoxResult = CType(Request.QueryString("MsgResult"), MsgBoxResult)

            If MsgBoxResult > 0 Then

                Select Case MsgBoxResult
                    Case MsgBoxResult.Yes
                    '
                    Case MsgBoxResult.No
                    '
                    Case MsgBoxResult.Ok
                        Session("Sender") = ""
                        Session("LogType") = LogType
                        Response.Redirect("wfSearchCriteriaForEngineDerateRegister.aspx?LogType=" + CStr(LogType))
                    Case Else
                        '
                End Select

            ElseIf MsgBoxResult = -1 Then
                Session("Sender") = ""
                Response.Redirect("wfSearchCriteriaForEngineDerateRegister.aspx?LogType=" + CStr(LogType))
            End If

            PreserveStateOfFavIcon()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Public Sub SetAircraftValues(CurrentDate As String)

        Try

            MachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=CurrentDate, , , , , , ,
                                                                       IsTagRequired:=True,
                                                                       TagText:="(SELECT)", ,
                                                                       SkipIsForInventoryAircarft:=True)
            ddlAircraft.DataSource = MachineNameValueList
            Session("MachineNameValueList") = MachineNameValueList
            ddlAircraft.DataBind()
            upnlDetails.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Data Binding "

    Public Sub CustomValidations(s As Object, e As ServerValidateEventArgs)

        Try

            Dim CustomValidator As CustomValidator
            CustomValidator = CType(s, CustomValidator)

            If CustomValidator.ControlToValidate = "ddlAircraft" Then

                If ddlAircraft.SelectedIndex = 0 Then
                    CustomValidator.ErrorMessage = "Please select the Aircraft"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DataFieldBind()

        Try

            EngineDerate = EngineDerate.GetDerateList("", "(SELECT)")
            ddlEngineDerate.DataSource = EngineDerate
            Session("EngineDerate") = EngineDerate

            DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            ClearAll()
            GetSession()

            EventLogID = CType(Session("EventLogID"), Guid)

            If Not IsPostBack Then

                LogType = Request.QueryString("LogType")
                Session("LogType") = LogType
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

                rdbLocal.Visible = IIf(AppSettings("ClientCode") = "GEP" Or
                                                AppSettings("ClientCode") = "SHN",
                                       True,
                                       False)
                rdbUTC.Visible = IIf(AppSettings("ClientCode") = "GEP" Or
                                              AppSettings("ClientCode") = "SHN",
                                     True,
                                     False)

                TodaysDate = Now.Date.ToString(AppSettings("DateFormat"))
                Session("TodaysDate") = TodaysDate
                SetAircraftValues(CurrentDate:=TodaysDate)

                PreserveStateOfFavIcon()

                Session("MiddleFrame") = "wfSearchCriteriaForEngineDerateRegister.aspx"

                ResetValues()
                DataFieldBind()

            End If

            ControlVisibility()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ShowCurrentSearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click

        Try

            DisplaySearchCriteria()
            SetValues()
            upnlSearchCriteria.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

        Try

            If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

            If IsValid = True Then
                DisplayReportInPDF()
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            MachineNameValueList = Nothing
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub AircraftChanged(sender As Object, e As EventArgs) Handles ddlAircraft.SelectedIndexChanged

        Try

            rdbLocal.Checked = IIf(MachineNameValueList(New Guid(ddlAircraft.SelectedValue)).IsUTC = False, True, False)
            rdbUTC.Checked = IIf(MachineNameValueList(New Guid(ddlAircraft.SelectedValue)).IsUTC = True, True, False)

            upnlLocalUTC.Update()
            upnlDetails.Update()

            If ddlAircraft.Enabled = True Then
                SetFocus(ddlAircraft)
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub FromDateChanged(sender As Object, e As EventArgs) Handles txtFromDate.TextChanged

        Try

            ChangedFromDate = txtFromDate.Text.Trim

            If TodaysDate <> ChangedFromDate Then

                SetAircraftValues(CurrentDate:=ChangedFromDate)
                DataFieldBind()

            End If

            upnlDateSelection.Update()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub PreserveStateOfFavIcon()

        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, ModuleName) Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Mark As Favorite",
                                                "MarkAsFavorite();",
                                                True)

        Else

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Remove From Favorite",
                                                "RemoveFromFavorite();",
                                                True)

        End If

    End Sub

    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavorite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, ModuleName)
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavorite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, ModuleName)
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

End Class