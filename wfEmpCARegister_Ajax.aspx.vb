Imports System.Collections.Generic

Public Class wfEmpCARegister_Ajax
    Inherits Page

#Region "Varriable Decclaration"

    Dim mSearchCriteria As String = String.Empty
    Public mEmployeeList As EmployeeList
    Public mCAStatusList As CAStatusList

#End Region

#Region "Overloading Methods"

    Private Overloads Sub SetFocus(control As WebControl)

        Try
            If control.Enabled = False Or control.Visible = False Then Exit Sub
            control.Focus()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "Helper Method"

    Private Sub displayReport()

        Dim dApt As New ObjectAdapter
        Dim myReport As Engine.ReportClass
        Dim unused As New rptImage
        Dim mCompanyDetail As New CompanyDetail
        Dim dSet As New dsEmpCARegister
        Dim rpt As EmpCAAuthorizationRegisterList
        myReport = New crptEmpCARegister
        Try
            rpt = EmpCAAuthorizationRegisterList.GetEmpCAAuthorizationRegisterList(AsOnDate:=txtAsOnDate.Text,
                                                                                    EmployeeID:=IIf(ddlEmployees.SelectedIndex = 0, "{00000000-0000-0000-0000-000000000000}", ddlEmployees.SelectedValue),
                                                                                    StatusID:=IIf(ddlCAStatus.SelectedIndex = 0, 0, ddlCAStatus.SelectedValue))

            Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                          mCompanyDetail.Email, WebSite:="", ReportName:="", SearchStr1:=New SmartDate(txtAsOnDate.Text).FormattedText,
                                          SearchStr2:=IIf(ddlEmployees.SelectedIndex = 0, "(ALL)", ddlEmployees.SelectedItem.Text),
                                          SearchStr3:=IIf(ddlCAStatus.SelectedIndex = 0, "(ALL)", ddlCAStatus.SelectedItem.Text),
                                          SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                          SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"),
                                          SearchStr11:="", SearchStr12:="", SearchStr13:="", SearchStr14:="", SearchStr15:="", SearchStr16:="")

            If rpt.Count <= 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "No records found for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1550)
            End If

            dSet.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(dSet, DataTableName:="rptImage")
            dApt.Fill(dSet, rpt)
            dApt.Fill(dSet, "rptImage", mrptImage)
            dApt.Fill(dSet, mReport)
            myReport.SetDataSource(dSet)
            Session("CrystalReport") = myReport
            Dim strFunctionName As String = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, [GetType](), "openTranDetail", strFunctionName, True)
            MarkLog(Util.Action.Print, "EmpCARegister", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Data Binding"

    Private Sub DataFieldBind()
        Try
            'DropDownList DataBinding
            mEmployeeList = EmployeeList.GetEmployeeList(AddTopItem:="(ALL)", IsLicenceNoNamePropertyRequired:=True)
            mCAStatusList = CAStatusList.GetCWPStatusList("(ALL)")
            Session("mEmployeeList") = mEmployeeList
            Session("mCAStatusList") = mCAStatusList
            ddlEmployees.DataSource = mEmployeeList
            ddlCAStatus.DataSource = mCAStatusList
            DataBind()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "Message Box Event"

    Private Sub MessageBoxResult()
        Dim Result As MsgBoxResult
        Try
            Result = MSGBoxCtrl.Result
            If Result > 0 Then
                Select Case Result
                    Case MsgBoxResult.Ok
                        DataFieldBind()
                End Select
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region


#Region "Button Click Event"

    Private Sub btnDisplay_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click
        Try
            displayReport()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        Try
            MSGBoxCtrl.HideControl()
            MessageBoxResult()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Page Events"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            EventLogID = CType(Session("EventLogID"), Guid)
            If Not IsPostBack Then
                txtAsOnDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                DataFieldBind()
            End If
            MessageBoxResult()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

End Class