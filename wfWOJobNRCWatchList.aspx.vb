'***********************************
' Created by:   Harsh Sugandhi
' Created on:   25th Feb 2025
' Created for:  FLYPAL-2221 Provision to add JOB NRC to Watch-list.
'***********************************

<CLSCompliant(False)>
Public Class JobNRCWatchList
    Inherits Page

#Region " Variable Declaration "

    Public _WOJobNRCWatchList As WOJobNRCWatchList
    Public _DistinctWOText As nDistinctWOText

    Dim ModuleName As String = "Job NRC WatchList"
    Dim ReportName As String = "Pending NRC WatchList"
    Dim WOText As String = String.Empty
    Dim WONumber As Integer = 0
    Dim EventLogID As Guid

#End Region

#Region " Methods "

    Private Sub GetSession()

        Try

            _DistinctWOText = Session("DistinctWOText")
            _WOJobNRCWatchList = Session("WOJobNRCWatchList")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetSession()

        Try

            Session("DistinctWOText") = _DistinctWOText
            Session("WOJobNRCWatchList") = _WOJobNRCWatchList

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub RemoveSession()

        Try

            Session.Remove("_WOJobNRCWatchList")
            Session.Remove("DistinctWOText")
            Session.Remove("MiddleFrame")
            Session.Remove("sender")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub ClearSessions()

        Try

            If Session("MiddleFrame") <> "wfWOJobNRCWatchList.aspx?" Then

                Session.Remove("_WOJobNRCWatchList")
                Session.Remove("DistinctWOText")

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        Try

            If control.Enabled = False Or control.Visible = False Then Exit Sub

            Dim script As String
            script = "<script type='text/javascript'> 
                    document.getElementById('" + control.ClientID + "').focus();</script>"

            ClientScript.RegisterStartupScript([GetType],
                                               "FocusScript",
                                               script)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GridBind(Optional WOText As String = "",
                         Optional WONumber As Integer = 0)

        Try

            _WOJobNRCWatchList = WOJobNRCWatchList.GetWOJobNRCForWatchList(WOText:=WOText,
                                                                           WONumber:=WONumber)

            gvJobNRCWatchList.DataSource = _WOJobNRCWatchList
            Session("WOJobNRCWatchList") = _WOJobNRCWatchList
            gvJobNRCWatchList.DataBind()

            lblResult.Text = $"List of Records as per criteria : {_WOJobNRCWatchList.Count.ToString} 
                               Record(s) found."

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetSearchCriteria()

        Try

            WOText = IIf(ddlWOText.SelectedIndex <= 0,
                         "",
                         ddlWOText.SelectedValue)

            WONumber = IIf(txtWONO.Text.Trim = "",
                       0,
                       Val(txtWONO.Text.Trim))

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub PreserveStateOfFavIcon()

        Try

            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, ModuleName) Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "MarkAsFavorite",
                                                    "MarkAsFavorite();",
                                                    True)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "RemoveFromFavorite",
                                                    "RemoveFromFavorite();",
                                                    True)

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " DataFieldBind "

    Protected Sub DataFieldBind()

        Try

            _DistinctWOText = nDistinctWOText.GetDistinctWOText(AddTopItem:="(ALL)",
                                                                ForNRCWatchList:=True)
            ddlWOText.DataSource = _DistinctWOText
            Session("DistinctWOText") = _DistinctWOText

            DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        EventLogID = CType(Session("EventLogID"), Guid)
        Try

            ClearSessions()
            GetSession()

            If Not IsPostBack Then

                SetFocus(ddlWOText)

                DataFieldBind()
                GridBind()
                PreserveStateOfFavIcon()
                Session("MiddleFrame") = $"wfWOJobNRCWatchList.aspx?"

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Protected Sub FilterRecords(sender As Object, e As EventArgs) Handles btnFilterRecords.Click

        Try

            SetSearchCriteria()
            GridBind(WOText:=WOText,
                     WONumber:=WONumber)

            PreserveStateOfFavIcon()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Protected Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

        Try

            RemoveSession()
            PreserveStateOfFavIcon()
            Response.Redirect("Dashboard.aspx")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrintReport.Click

        Dim crystalReport As Engine.ReportClass = New crWOJobNRCWatchList
        Dim _CompanyDetail As New CompanyDetail
        Dim dataSet As New dsWOJobNRCWatchList
        Dim objectAdapter As New ObjectAdapter

        Try

            SetSearchCriteria()

            _WOJobNRCWatchList = WOJobNRCWatchList.GetWOJobNRCForWatchList(WOText:=WOText,
                                                                           WONumber:=WONumber)

            If _WOJobNRCWatchList.Count <= 0 Then

                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "No records available to Display Report.",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            End If

            Dim _ReportData As New ReportData(CompanyName:=_CompanyDetail.CompanyName,
                                              Address:=_CompanyDetail.Address,
                                              Tel1:=_CompanyDetail.Tel1,
                                              Tel2:=_CompanyDetail.Tel2,
                                              Fax:=_CompanyDetail.Fax,
                                              Email:=_CompanyDetail.Email,
                                              WebSite:=_CompanyDetail.WebSite,
                                              ReportName:=ReportName,
                                              ProductVersion:=AppSettings("Product Version"),
                                              SINote:=AppSettings("SINote"),
                                              SearchStr1:=WOText,
                                              SearchStr2:=(WONumber.ToString.Trim),
                                              SearchStr3:="",
                                              SearchStr4:="",
                                              SearchStr5:="",
                                              SearchStr6:="",
                                              SearchStr7:="",
                                              SearchStr8:="",
                                              SearchStr9:="",
                                              SearchStr10:="",
                                              SearchStr11:="",
                                              SearchStr12:="",
                                              SearchStr13:="",
                                              SearchStr14:=AppSettings("Logo"),
                                              SearchStr15:=AppSettings("ClientCode"))

            dataSet.Clear()

            Dim companyLogo As rptImage = rptImage.GetImage(dataSet)
            objectAdapter.Fill(dataSet, TableName:="rptImage", companyLogo)
            objectAdapter.Fill(dataSet, TableName:="WOJobNRCWatchList", _WOJobNRCWatchList)
            objectAdapter.Fill(dataSet, TableName:="ReportData", _ReportData)
            crystalReport.SetDataSource(dataSet)

            Session("CrystalReport") = crystalReport

            MarkLog(Action:=Action.Print,
                    ModuleName:=ModuleName,
                    Detail:="WO Job NRC WatchList Print.",
                    ErrorType:=ErrorType.NoError,
                    TransID:=Guid.Empty,
                    EventLogID:=EventLogID)

            PreserveStateOfFavIcon()

            ScriptManager.RegisterStartupScript(page:=Me,
                                                type:=[GetType],
                                                key:="Display Report",
                                                script:="displayReport()",
                                                addScriptTags:=True)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " GridView Events "

    Private Sub GVJobNRCWatchList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvJobNRCWatchList.RowCommand

        Dim mID As Guid
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        Dim mModelMonitorInsp As ModelMonitorInsp
        Dim mMachine As Machine
        Dim mAssemblyStatus As AssemblyStatus
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        Dim WODetails As nWO
        Dim AirframeCurrentValues As String
        Dim mAircraftCurrentValue As AircraftCurrentStatusList
        Dim openScript As String

        Try

            Select Case e.CommandName
                Case "AddToInspection"

                    mID = New Guid(e.CommandArgument.ToString)
                    WODetails = nWO.GetWO(ID:=_WOJobNRCWatchList(mID).WOID)

                    Session("NRCJobID") = mID
                    Session("WODetails") = WODetails

                    SetSession()

                    mMachine = Machine.GetMachine(WODetails.MachineID)
                    mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMachine.AssemblyStatus.ID)

                    Session("mMachine") = mMachine
                    Session("mAssemblyStatus") = mAssemblyStatus
                    Session("CloseDate") = _WOJobNRCWatchList(mID).JobCloseDate.ToString

                    mAssemblyMonitorInspStatus =
                        AssemblyMonitorInspStatus.
                            NewAssemblyMonitorInspStatus(Guid.NewGuid,
                                                         mAssemblyStatus.AssemblyID,
                                                         mAssemblyStatus.ID,
                                                         AsOnDate:=_WOJobNRCWatchList(mID).JobCloseDate.ToString,
                                                         mAssemblyStatus.Assembly.ModelID,
                                                         mMachine.HourType)

                    mModelMonitorInsp = ModelMonitorInsp.
                                            NewModelMonitorInsp(ID,
                                                                mAssemblyStatus.Assembly.ModelID,
                                                                mMachine.HourType, ID)

                    mModelMonitorInsp.Description = _WOJobNRCWatchList(mID).JobDescription + "<BR>" +
                                                    "WATCH-LIST INSTRUCTIONS => " +
                                                    _WOJobNRCWatchList(mID).WatchListInstructions

                    mModelMonitorInsp.ATAID = _WOJobNRCWatchList(mID).ATAChapterID


                    mModelMonitorInsp.Code = IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or
                                                 AppSettings("ShowAMOOnlyForNewClients") = "True",
                                                 _WOJobNRCWatchList(mID).TaskCardNo,
                                                 _WOJobNRCWatchList(mID).InspectionCode)

                    mModelMonitorInsp.WatchItemID = _WOJobNRCWatchList(mID).ID
                    mModelMonitorInsp.Zone = _WOJobNRCWatchList(mID).Zone
                    mModelMonitorInsp.Area = _WOJobNRCWatchList(mID).Area
                    mModelMonitorInsp.Note = _WOJobNRCWatchList(mID).JobRemark
                    mModelMonitorInsp.Reference = _WOJobNRCWatchList(mID).TaskSourceRef
                    mAssemblyMonitorInspStatus.DoneWONo = _WOJobNRCWatchList(mID).WONumber
                    mModelMonitorInsp.BeginEdit()

                    mAircraftCurrentValue =
                        AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(,
                                                                                    mMachine.RegNo, , , ,
                                                                                    Today.Date.ToString)
                    AirframeCurrentValues = mAircraftCurrentValue(0).ShowPeriods

                    Session("mModelMonitorInsp") = mModelMonitorInsp
                    Session("AirframeCurrentValues") = AirframeCurrentValues
                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                    Session("mIssueDate") = _WOJobNRCWatchList(mID).JobStartDateFormatted
                    Session("NewPage") = "True"
                    Session("IsOpenFromMPD") = "True"
                    Session("OpenFromDiscrepancyCorrectiveActionList") = "False"
                    Session("OpenFromJOBNRCList") = "True"
                    Session("FromEditThresholdInterval") = "False"
                    Session("WOJobNRCWatchList") = _WOJobNRCWatchList

                    MarkLog(Action.[New],
                            "Model Monitor Inspection",
                            $" Model : {mAssemblyStatus.Assembly.ModelName}",
                            ErrorType.NoError,
                            Guid.Empty,
                            EventLogID)

                    openScript = "openPageInSameWindow('wfInspectionForWatchItem.aspx?BackPage=index.aspx');"

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "Open Script",
                                                        openScript,
                                                        True)

                    PreserveStateOfFavIcon()

            End Select

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GVJobNRCWatchList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvJobNRCWatchList.PageIndexChanging

        Try

            gvJobNRCWatchList.PageIndex = e.NewPageIndex
            gvJobNRCWatchList.DataSource = _WOJobNRCWatchList
            Session("WOJobNRCWatchList") = _WOJobNRCWatchList
            gvJobNRCWatchList.DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub GVJobNRCWatchList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvJobNRCWatchList.Sorting

        Try

            gvJobNRCWatchList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
            gvJobNRCWatchList.DataSource = _WOJobNRCWatchList
            Session("WOJobNRCWatchList") = _WOJobNRCWatchList
            gvJobNRCWatchList.DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Favorite Icon Events "

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