'************************************
'Created By Saylee on 5-Aug-2015
'Modified by Harsh Sugandhi on 06th Jun 2025 for FlyPaL-2476.
'************************************


Imports System.Linq


Public Class wfSearchCriteriaForEmployeeRegister
    Inherits Page

#Region " Variable Declaration "

    Dim EmployeeID As String
    Dim Employee As String
    Dim EmployeeName As String
    Dim Name As String
    Dim mDesignation As String = String.Empty
    Dim mDepartment As String = String.Empty
    Dim _Skills As String = String.Empty
    Dim mEmployeeList As EmployeeList
    Public EventLogDetails As String = String.Empty
    Dim mIsUseInLogRequired As Boolean
    Dim mIsTechnicalCrew As Boolean
    Dim mIsOthers As Boolean = False

#End Region

#Region " Helper Methods "

    Private Sub Display()

        Try

            lblSummary.Visible = True
            lblCrewSelection.Visible = True
            lblDesignationSelection.Visible = True
            lblDepartmantSelection.Visible = True
            lblIsEmployeeWorking.Visible = True
            lblIsContractedEmployee.Visible = True
            lblEmployeeSkillsSelection.Visible = True

            upnlCurrentCriteria.Update()
            SetValues()


        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub DataFieldBind()

        Try

            cmbDesignation.DataSource = DesignationList.GetDesignationList("", "(ALL)")
            cmbDesignation.DataBind()

            cmbDepartmentList.DataSource = EmployeeDepartmentList.GetEmployeeDepartmentList("(ALL)")
            cmbDepartmentList.DataBind()

            cmbEmployeeSkills.DataSource = MPDSkillList.GetSkillList(True, TagText:="(ALL)")
            cmbEmployeeSkills.DataBind()

            If Not (AppSettings("ShowAMOOnlyForNewClients") = "True") Then
                cmbCrewSelection.Items.RemoveAt(2)
            End If

            DataBind()

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetValues()

        Try

            mIsUseInLogRequired = False
            mIsTechnicalCrew = False
            EmployeeID = IIf(SelectedCrewID.Value.Length > 0, SelectedCrewID.Value, Guid.Empty.ToString)

            If txtSearch.Text.Trim = "" Then

                lblCrewSelection.Text = "All"
                EmployeeName = "All"
                Name = ""

            Else

                Employee = txtSearch.Text.Trim
                EmployeeName = Employee

                If Not New Guid(EmployeeID.ToString).Equals(Guid.Empty) Then

                    Dim mEmployee As Employee
                    mEmployee = Flypal.Employee.GetEmployee(New Guid(EmployeeID.ToString))
                    Name = mEmployee.Name

                Else

                    If txtSearch.Text.Trim = "" Then
                        Name = ""
                    Else

                        If txtSearch.Text.Contains("-") Then
                            Name = txtSearch.Text.Trim.Substring(txtSearch.Text.Trim.IndexOf("-") + 2)
                        Else
                            Name = txtSearch.Text
                        End If

                    End If

                End If

            End If

            lblCrewSelection.Text = "Employee : " & EmployeeName

            If cmbDesignation.SelectedIndex > 0 Then
                mDesignation = cmbDesignation.SelectedItem.Text
                lblDesignationSelection.Text = "Designation : " & mDesignation
            Else
                mDesignation = ""
                lblDesignationSelection.Text = "Designation : All"
            End If

            If cmbEmployeeIsWorking.SelectedIndex = 0 Then
                lblIsEmployeeWorking.Text = "Employee Is Working :"
            Else
                lblIsEmployeeWorking.Text = "Employee Is Working : " & cmbEmployeeIsWorking.SelectedItem.Text
            End If

            If cmbDepartmentList.SelectedIndex > 0 Then
                mDepartment = cmbDepartmentList.SelectedItem.Text
                lblDepartmantSelection.Text = "Department : " & mDepartment
            Else
                mDepartment = ""
                lblDepartmantSelection.Text = "Department : All"
            End If

            If cmbEmployeeSkills.SelectedIndex > 0 Then
                _Skills = cmbEmployeeSkills.SelectedItem.Text
                lblEmployeeSkillsSelection.Text = "Skills : " & _Skills
            Else
                mDepartment = ""
                lblEmployeeSkillsSelection.Text = "Skills : All"
            End If

            If cmbCrewSelection.SelectedValue = 0 Then

                lblFlyingOrTechnicalCrew.Text = cmbCrewSelection.SelectedItem.Text
                mIsUseInLogRequired = False
                mIsTechnicalCrew = False
                mIsOthers = False

            ElseIf cmbCrewSelection.SelectedValue = 1 Then

                lblFlyingOrTechnicalCrew.Text = cmbCrewSelection.SelectedItem.Text
                mIsUseInLogRequired = True
                mIsTechnicalCrew = False
                mIsOthers = False

            ElseIf cmbCrewSelection.SelectedValue = 2 Then

                lblFlyingOrTechnicalCrew.Text = cmbCrewSelection.SelectedItem.Text
                mIsUseInLogRequired = False
                mIsTechnicalCrew = True
                mIsOthers = False

            ElseIf cmbCrewSelection.SelectedValue = 3 Then

                lblFlyingOrTechnicalCrew.Text = cmbCrewSelection.SelectedItem.Text
                mIsUseInLogRequired = False
                mIsTechnicalCrew = False
                mIsOthers = True

            Else
                lblFlyingOrTechnicalCrew.Text = cmbCrewSelection.SelectedItem.Text
            End If

            If cmbContractedEmployee.SelectedIndex = 0 Then
                lblIsContractedEmployee.Text = "Contracted Employee :"
            Else
                lblIsContractedEmployee.Text = "Contracted Employee : " & cmbContractedEmployee.SelectedItem.Text
            End If

            EventLogDetails = "Employee: " & EmployeeName + ", " +
                               lblDesignationSelection.Text + ", " +
                               lblIsEmployeeWorking.Text + ", " +
                               lblDepartmantSelection.Text + ", " +
                               lblIsContractedEmployee.Text + ", " +
                               lblEmployeeSkillsSelection.Text

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SetReport(Optional IsExcel As Boolean = False)

        Try

            Dim da As New ObjectAdapter
            Dim myReport As Engine.ReportClass
            Dim mCompanyDetail As New CompanyDetail
            Dim ds As New dsEmployeeRegister

            SetValues()

            Dim mEmployeeList As EmployeeList = EmployeeList.GetEmployeeList(Name:=Name,
                                                                             Designation:=mDesignation, , , ,
                                                                             IsUseInLogRequired:=mIsUseInLogRequired, ,
                                                                             Department:=mDepartment,
                                                                             IsEmployeeWorking:=Val(cmbEmployeeIsWorking.SelectedValue),
                                                                             SkipNames:="None",
                                                                             IsContractedEmployee:=Val(cmbContractedEmployee.SelectedValue),
                                                                             IsTechnicalCrew:=mIsTechnicalCrew,
                                                                             IsOthers:=mIsOthers,
                                                                             SkillListRequired:=True,
                                                                             SkillID:=IIf(cmbEmployeeSkills.SelectedIndex = 0,
                                                                                          0,
                                                                                          CInt(cmbEmployeeSkills.SelectedValue)))
            If rdbSummary.Checked Then

                If CBool(AppSettings("ShowMaintenanceForNewClients")) Then
                    myReport = New crEmployeeRegisterNewClients 'Added to Show Employee Skills 
                ElseIf AppSettings("ClientCode") = "APFT" Or
                       AppSettings("ClientCode") = "AAP" Then
                    myReport = New crEmployeeRegisterAPFT
                Else
                    myReport = New crEmployeeRegister 'Only Employee List
                End If

            Else

                myReport = New crEmployeeReg 'Detail report

                Dim mEmployeeDocumentList As EmployeeDocumentList = EmployeeDocumentList.
                                                                        GetEmployeeDocumentListForRegisterReport(EmployeeID:=EmployeeID)
                Dim mEmployeeTrainingList As EmployeeTrainingList = EmployeeTrainingList.
                                                                        GetEmployeeTrainingListForRegisterReport(EmployeeID:=EmployeeID)
                Dim mEmployeePhotoList As EmployeeePhotoList = EmployeeePhotoList.
                                                                        GetImage(DataSet:=ds,
                                                                                 EmployeeID:=EmployeeID.ToString)

                da.Fill(ds, mEmployeeDocumentList)
                da.Fill(ds, mEmployeeTrainingList)
                da.Fill(ds, mEmployeePhotoList)

            End If

            Dim Report As New ReportData(CompanyName:=mCompanyDetail.CompanyName,
                                         Address:=mCompanyDetail.Address,
                                         Tel1:=mCompanyDetail.Tel1,
                                         Tel2:=mCompanyDetail.Tel2,
                                         Fax:=mCompanyDetail.Fax,
                                         Email:=mCompanyDetail.Email,
                                         WebSite:=mCompanyDetail.WebSite,
                                         ReportName:="Employee Register",
                                         ProductVersion:=AppSettings("Product Version"),
                                         SINote:=AppSettings("SINote"),
                                         SearchStr1:=EmployeeName,
                                         SearchStr2:=mDesignation,
                                         SearchStr3:=mDepartment,
                                         SearchStr4:=IIf(cmbEmployeeIsWorking.SelectedIndex = 0,
                                                         "",
                                                         cmbEmployeeIsWorking.SelectedItem.Text),
                                         SearchStr5:=IIf(cmbContractedEmployee.SelectedIndex = 0,
                                                         "",
                                                         cmbContractedEmployee.SelectedItem.Text),
                                         SearchStr6:=cmbCrewSelection.SelectedItem.Text,
                                         SearchStr7:=IIf(cmbEmployeeSkills.SelectedIndex = 0,
                                                         "",
                                                         cmbEmployeeSkills.SelectedItem.Text),
                                         SearchStr8:=AppSettings("Logo"),
                                         SearchStr9:=AppSettings("ShowMaintenanceForNewClients"))

            If mEmployeeList.Count = 0 Then

                MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                MSGBox.Message_text.NoRecordFound,
                                "There is no record for this search criteria",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            ElseIf mEmployeeList.Count > 0 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1140)
            End If

            If IsExcel = False Then

                Dim rptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, rptImage)
                da.Fill(ds, mEmployeeList)
                da.Fill(ds, Report)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "Open Report",
                                                    "openReport();",
                                                    True)

            ElseIf IsExcel = True Then  'Excel format

                ds.Clear()
                da.Fill(ds, "ReportData", Report)
                da.Fill(ds, "EmployeeList", mEmployeeList)

                Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote",
                                                   "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10",
                                                   "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15",
                                                   "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22",
                                                   "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100", "ShortName"}

                For i As Integer = 0 To columnToRemove2.Length - 1

                    If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                    End If

                Next

                Dim columnToRemove As String() = {"ID", "Address1", "Address2", "DesignationID", "CityID", "ExpatStatus", "ContractorID", "IsWorking", "IsShowWorkingDate",
                                                  "DateOfLeaving", "LastModifiedOn", "ImageFile", "ImageSize", "FileExtension", "GenderID", "IsLogTransfered",
                                                  "CurrCityID", "EmpName", "DateOfLeavingFormatted", "LastModifiedOnFormatted", "EmpNoName", "IsUseInFlightLog",
                                                  "EmployeeCountInFlightLog", "LicenceNoName", "IsContractedEmployee", "EmployeeDocumentCount",
                                                  "EmployeeDocumentCountForLink", "EmployeeTrainingCount", "EmployeeTrainingCountForLink", "LocationID", "CurrAddress1",
                                                  "CurrAddress2", "Day", "Month", "Year", "PointOfOrigin", "CurrPointOfOrigin", "IsSyncFromCRS", "IsTechnicalCrew", "IsOthers", "IsDocumentOrTrainingDue"}

                For i As Integer = 0 To columnToRemove.Length - 1

                    If ds.Tables("EmployeeList").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("EmployeeList").Columns.Remove(columnToRemove(i))
                    End If

                Next

                If ds.Tables("EmployeeList").Columns.Contains("EmpNo") Then
                    ds.Tables("EmployeeList").Columns("EmpNo").ColumnName = "Emp. No"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("Name") Then
                    ds.Tables("EmployeeList").Columns("Name").ColumnName = "Emp. Name"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("LicenseNo") Then
                    ds.Tables("EmployeeList").Columns("LicenseNo").ColumnName = "License No."
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CityName") Then
                    ds.Tables("EmployeeList").Columns("CityName").ColumnName = "City"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("StateName") Then
                    ds.Tables("EmployeeList").Columns("StateName").ColumnName = "State"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CountryName") Then
                    ds.Tables("EmployeeList").Columns("CountryName").ColumnName = "Country"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("PhoneNo") Then
                    ds.Tables("EmployeeList").Columns("PhoneNo").ColumnName = "Phone No."
                End If

                If ds.Tables("EmployeeList").Columns.Contains("GenderName") Then
                    ds.Tables("EmployeeList").Columns("GenderName").ColumnName = "Gender"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("DepartmentName") Then
                    ds.Tables("EmployeeList").Columns("DepartmentName").ColumnName = "Department"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("DesignationName") Then
                    ds.Tables("EmployeeList").Columns("DesignationName").ColumnName = "Designation"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CurrAddress") Then
                    ds.Tables("EmployeeList").Columns("CurrAddress").ColumnName = "Curr. Address"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CurrCityName") Then
                    ds.Tables("EmployeeList").Columns("CurrCityName").ColumnName = "Curr. City"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CurrStateName") Then
                    ds.Tables("EmployeeList").Columns("CurrStateName").ColumnName = "Curr. State"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CurrCountryName") Then
                    ds.Tables("EmployeeList").Columns("CurrCountryName").ColumnName = "Curr. Country"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CurrPhoneNo") Then
                    ds.Tables("EmployeeList").Columns("CurrPhoneNo").ColumnName = "Curr. Phone No."
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CurrMobile") Then
                    ds.Tables("EmployeeList").Columns("CurrMobile").ColumnName = "Curr. Mobile"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("CurrEmail") Then
                    ds.Tables("EmployeeList").Columns("CurrEmail").ColumnName = "Curr. Email"
                End If

                If ds.Tables("EmployeeList").Columns.Contains("LocationName") Then
                    ds.Tables("EmployeeList").Columns("LocationName").ColumnName = "Employee Base Station"
                End If

                If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                    ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Employee Name"
                End If

                If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                    ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Designation"
                End If

                If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                    ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Department"
                End If

                If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
                    ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Employee Working"
                End If

                If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
                    ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Contracted Employee"
                End If

                'Set Column Sequence
                ds.Tables("EmployeeList").Columns("Emp. No").SetOrdinal(1)
                ds.Tables("EmployeeList").Columns("Emp. Name").SetOrdinal(2)
                ds.Tables("EmployeeList").Columns("License No.").SetOrdinal(3)
                ds.Tables("EmployeeList").Columns("Department").SetOrdinal(4)
                ds.Tables("EmployeeList").Columns("Designation").SetOrdinal(5)
                ds.Tables("EmployeeList").Columns("Address").SetOrdinal(6)
                ds.Tables("EmployeeList").Columns("City").SetOrdinal(7)
                ds.Tables("EmployeeList").Columns("State").SetOrdinal(8)
                ds.Tables("EmployeeList").Columns("Country").SetOrdinal(9)
                ds.Tables("EmployeeList").Columns("Phone No.").SetOrdinal(10)
                ds.Tables("EmployeeList").Columns("Mobile").SetOrdinal(11)
                ds.Tables("EmployeeList").Columns("Email").SetOrdinal(12)
                ds.Tables("EmployeeList").Columns("Nationality").SetOrdinal(13)
                ds.Tables("EmployeeList").Columns("Curr. Address").SetOrdinal(14)
                ds.Tables("EmployeeList").Columns("Curr. City").SetOrdinal(15)
                ds.Tables("EmployeeList").Columns("Curr. State").SetOrdinal(16)
                ds.Tables("EmployeeList").Columns("Curr. Country").SetOrdinal(17)
                ds.Tables("EmployeeList").Columns("Curr. Phone No.").SetOrdinal(18)
                ds.Tables("EmployeeList").Columns("Curr. Mobile").SetOrdinal(19)
                ds.Tables("EmployeeList").Columns("Curr. Email").SetOrdinal(20)
                ds.Tables("EmployeeList").Columns("Gender").SetOrdinal(21)

                Dim dsNew As New DataSet
                dsNew.Clear()
                dsNew.Merge(ds.Tables("ReportData"))
                dsNew.Merge(ds.Tables("EmployeeList"))
                dsNew.Tables("ReportData").TableName = "Searching Criteria"
				dsNew.Tables("EmployeeList").TableName = "Employee Register"
				Session("ExcelFileName") = "Employee Register"
				Session("dsNew") = dsNew

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openFile",
                                                    "openFile();",
                                                    True)
                'Added by Prashant on 19-Jan-2021
                MarkLog(Action.Print,
                        "EmployeeRegister",
                        "Export To Excel " + EventLogDetails,
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID)

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

#End Region

#Region " Events"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        EventLogID = CType(Session("EventLogID"), Guid)
        Try

            If Not IsPostBack Then

                Session("MiddleFrame") = "wfSearchCriteriaForEmployeeRegister.aspx"
                DataFieldBind()

            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub SearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click, btnExport.Click

        Try

            If (sender.ID = "btnDisplay") Then
                SetReport(False)
            ElseIf (sender.ID = "btnExport") Then 'Added by Prashant on 19-Nov-2020 APFT19112020
                SetReport(True)
            End If

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

#End Region

#Region " Service Methods "

    <Services.WebMethod(), Script.Services.ScriptMethod()>
    Public Shared Function GetCrewListAutoComplete(prefixText As String,
                                                   count As Integer,
                                                   contextKey As String) As String()


        Dim mEmpNoNameList As EmpNoNameAutoComplete = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)

        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In mEmpNoNameList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In mEmpNoNameList
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If

    End Function

#End Region

End Class