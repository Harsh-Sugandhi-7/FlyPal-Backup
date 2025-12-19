'****************************
'AJAX Conversion By Vikrant
'****************************

Public Class wfEmployeeList_Ajax
    Inherits Page

    'Added by Vikrant on 11-Nov-2019 For ALL08112019
#Region " Enum "

    Private Enum Rights

        [New] = 0
        Edit = 1
        Delete = 2
        View = 3
        Print = 4

    End Enum

#End Region
    'End

#Region " Variable Declaration "

    Public mEmployee As Employee
    Public mEmployeeList As EmployeeList
    Public BackPage As String
    Dim Type As Int16
    Public Text, Index, ShowNoE As String

    Dim EventLogID As Guid 'Added by Saylee on 22-July-2011
    Dim mEmployeeWorking As Integer = -1

#End Region

#Region " Helper Methods "

    Private Sub GetSession()

        mEmployeeList = Session("mEmployeeList")
        mEmployee = Session("mEmployee")
        Index = Session("Index")
        Text = Session("Text")
        ShowNoE = Session("ShowNoE")

    End Sub

    Private Sub SetSession()

        Session("mEmployeeList") = mEmployeeList
        Session("mEmployee") = mEmployee
        Session("Index") = Index
        Session("Text") = Text
        Session("ShowNoE") = ShowNoE

    End Sub

    Private Sub RemoveSession()

        Session.Remove("mEmployeeList")
        Session.Remove("Type")
        Session.Remove("Text")
        Session.Remove("Index")
        Session.Remove("ShowNotWorkingEmployee")

    End Sub

    Private Overloads Sub SetFocus(cntrl As WebControl)

        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()

    End Sub

    Private Sub NewRecord()

        mEmployee = Employee.NewEmployee()

        Session("mEmployee") = mEmployee

    End Sub

    Private Sub ClearAll()

        If Session("MiddleFrame") <> "wfEmployeeList_Ajax.aspx" Then

            Session.Remove("mEmployee")
            Session.Remove("mEmployeeList")
            Session.Remove("Text")
            Session.Remove("Index")
            Session.Remove("ShowNotWorkingEmployee")

        End If

    End Sub

    Private Sub EditRecord(mID As Guid)

        mEmployee = Employee.GetEmployee(mID)

        Session("mEmployee") = mEmployee

    End Sub

    Private Sub DeleteRecord(mID As Guid)

        MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                        MSGBox.Message_text.Delete,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

        mEmployee = Employee.GetEmployee(mID)
        Session("mEmployee") = mEmployee

    End Sub

    'Modified by Harsh Sugandhi on 15th July 2024 for FLYPAL-1728
    'Modified by Harsh Sugandhi on 16th October 2024 for FLYPAL-1971 Employee Master Search Issue.
    Private Sub FindNow(LookInType As Integer,
                        Optional Name As String = "",
                        Optional Designation As String = "",
                        Optional EmpNo As String = "",
                        Optional Contractor As String = "")

        If LookInType = -1 Then

            LookInType = 0   ' This step is IMP when details form  is opened directly.

        End If


        If chkShownotworkingemployee.Checked = True Then  'Added by Prashant 13-Aug-2020 All13082020

            mEmployeeWorking = 0  ' Not working employee

        Else

            mEmployeeWorking = 1  'Working employee

        End If


        If LookInType = 0 Then 'ALL

            FetchEmployeeListAsPerRights(IsEmployeeWorking:=mEmployeeWorking)

        ElseIf LookInType = 1 Then 'Emp No.

            If EmpNo <> "" Then

                FetchEmployeeListAsPerRights(EmpNo:=EmpNo,
                                             IsEmployeeWorking:=mEmployeeWorking)

            End If

        ElseIf LookInType = 2 Then 'Employee

            If Name <> "" Then

                FetchEmployeeListAsPerRights(Name:=Name,
                                             IsEmployeeWorking:=mEmployeeWorking)

            End If

        ElseIf LookInType = 3 Then 'Designation

            If Designation <> "" Then

                FetchEmployeeListAsPerRights(Designation:=Designation,
                                             IsEmployeeWorking:=mEmployeeWorking)

            End If

        ElseIf LookInType = 4 Then 'Contractor

            If Contractor <> "" Then

                FetchEmployeeListAsPerRights(Contractor:=Contractor,
                                             IsEmployeeWorking:=mEmployeeWorking)

            End If

        ElseIf LookInType = 5 Then 'Flying Crew

            mEmployeeList = EmployeeList.GetEmployeeList(, , , ,
                                                         Contractor,
                                                          IsEmployeeWorking:=mEmployeeWorking,
                                                         IsUseInLogRequired:=1)

        ElseIf LookInType = 6 Then 'Technical Crew

            mEmployeeList = EmployeeList.GetEmployeeList(, , , ,
                                                         Contractor,
                                                          IsEmployeeWorking:=mEmployeeWorking,
                                                         IsTechnicalCrew:=1)

        End If

        dgEmployeeList.DataSource = mEmployeeList
        Session("mEmployeeList") = mEmployeeList
        lblResult.Text = "List of Employee as per criteria : " & mEmployeeList.Count & " Record(s) found."

    End Sub

    Private Sub SetControl()

        '======Added By Saylee on 12-Oct-2007==========
        Index = Session("Index")
        Text = Session("Text")
        chkShownotworkingemployee.Checked = CType(Session("ShowNotWorkingEmployee"), Boolean)

        FindNow(Index,
                Text,
                Text,
                Text,
                Text)

        txtSearch.Text = Text
        cmbLookIn.SelectedValue = Index

        If ShowNoE Is Nothing Then

            cmbShowE.SelectedValue = "4"

        Else

            cmbShowE.SelectedValue = ShowNoE 'Ajay 17-08-2023

        End If

        ControlVisibility(Index)
        dgEmployeeList.DataBind()
        '=============================================

    End Sub

    'Modified by Harsh Sugandhi on 15th July 2024 for FLYPAL-1728
    Private Sub ControlVisibility(Index As Int32)

        '======Added By Saylee on 12-Oct-2007==========
        lblFor.Visible = IIf(Index = 0 Or Index = 5 Or Index = 6, False, True)
        txtSearch.Visible = IIf(Index = 0 Or Index = 5 Or Index = 6, False, True)
        '==============================================

        'Added by Harsh Sugandhi on 15th July 2024 for FLYPAL-1728
        dgEmployeeList.Columns(8).Visible = IIf(AppSettings("ShowAMOOnlyForNewClients").ToLower = "true",
                                                      True,
                                                      False)

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then

            Select Case Result1

                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Dim EmpNoName As String

                        Try

                            Session("sender") = ""
                            mEmployee = Session("mEmployee")
                            EmpNoName = mEmployee.EmpNoName
                            Employee.DeleteEmployee(mEmployee.ID)

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                MarkLog(Action.Delete,
                                        "Employee",
                                        "Can't delete : " + EmpNoName + " is Currently in use",
                                        ErrorType.NoError,
                                        mEmployee.ID,
                                        EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            msgCount = ex.Errors.Count

                        Finally

                            If msgCount = 0 Then

                                MarkLog(Action.Delete,
                                        "Employee",
                                        EmpNoName,
                                        ErrorType.NoError,
                                        mEmployee.ID,
                                        EventLogID)

                            End If

                            FindNow(CInt(cmbLookIn.SelectedValue),
                                    txtSearch.Text,
                                    txtSearch.Text,
                                    txtSearch.Text,
                                    txtSearch.Text)

                            dgEmployeeList.DataBind()
                            SetGrid()
                            upnlGridTitle.Update()
                            upnlGrid.Update()

                        End Try

                    End If

                Case MsgBoxResult.No

                    Session("sender") = ""

                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added

                    Session("sender") = ""

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added

                    Session("sender") = ""

            End Select

        ElseIf Result1 = -1 Then

            Session("sender") = ""

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added

            Session("sender") = ""

        End If

    End Sub

    Private Sub SetGrid()

        Dim P As Integer
        Dim lb As LinkButton
        Dim IsSyncFromCRS As Boolean

        For j As Integer = 0 To dgEmployeeList.Rows.Count - 1

            P = CType(Me.dgEmployeeList.Rows(j).Cells(15).Text, Integer) 'Image Size 
            IsSyncFromCRS = CType(Me.dgEmployeeList.Rows(j).Cells(16).Text, Boolean)

            If P <= 0 Then

                lb = CType(dgEmployeeList.Rows(j).Cells(13).FindControl("LinkButton1"), LinkButton) 'View  Link 
                lb.Enabled = False

            End If

            If IsSyncFromCRS = True Then

            End If

        Next

    End Sub

    'Added by Vikrant on 11-Nov-2019 For ALL08112019
    Private Function IsInRole(CheckFor As Rights, IsInRoleString As String) As Boolean

        Select Case CheckFor

            Case Rights.View

                Return User.IsInRole(IsInRoleString + "View")

            Case Rights.[New]

                Return User.IsInRole(IsInRoleString + "New")

            Case Rights.Edit

                Return User.IsInRole(IsInRoleString + "Edit")

            Case Rights.Delete

                Return User.IsInRole(IsInRoleString + "Delete")

            Case Rights.Print

                Return User.IsInRole(IsInRoleString + "Print")

        End Select

    End Function

    Private Sub SetVariables()

        ShowNoE = IIf(cmbShowE.SelectedIndex <= 0, 0, cmbShowE.SelectedValue)
        Session("ShowNoE") = ShowNoE

    End Sub
    'End

    'Added by Harsh Sugandhi on 5th July 2024 for FLYPAL-1728
    Private Sub FillLookInDD()

        Try

            If IsInRole(Rights.View, "FlyingCrew") Then

                cmbLookIn.Items.Add(New ListItem("Flying Crew", "5"))

            End If

            If AppSettings("ShowAMOOnlyForNewClients").ToLower = "true" AndAlso
               IsInRole(Rights.View, "TechnicalCrew") Then

                cmbLookIn.Items.Add(New ListItem("Technical Crew", "6"))

            End If

        Catch ex As Exception

            ex.GetBaseException()

        End Try

    End Sub

    'Modified by Harsh Sugandhi on 16th October 2024 for FLYPAL-1971 Employee Master Search Issue.
    Private Sub FetchEmployeeListAsPerRights(Optional Name As String = "",
                                             Optional Designation As String = "",
                                             Optional EmpNo As String = "",
                                             Optional Contractor As String = "",
                                             Optional IsEmployeeWorking As Integer = 2,
                                             Optional IsTechnicalCrew As Boolean = False,
                                             Optional IsUseInLogRequired As Boolean = False,
                                             Optional SkipTechnicalCrewAndFlyingCrew As Boolean = False,
                                             Optional ShowAllTechnicalCrewAndUnassigned As Boolean = False,
                                             Optional ShowAllFlyingCrewAndUnassigned As Boolean = False)

        If AppSettings("ShowAMOOnlyForNewClients").ToLower = "true" Then

            If Not IsInRole(Rights.View, "FlyingCrew") AndAlso
                   IsInRole(Rights.View, "TechnicalCrew") Then

                'All Technical Crew records along with records that are Unassigned [neither of Flying Crew nor Technical Crew]
                mEmployeeList = EmployeeList.GetEmployeeList(Name:=Name,
                                                             Designation:=Designation,
                                                             EmpNo:=EmpNo,
                                                             Contractor:=Contractor,
                                                             IsEmployeeWorking:=IsEmployeeWorking,
                                                             IsTechnicalCrew:=1,
                                                             ShowAllTechnicalCrewAndUnassigned:=1)

            ElseIf Not IsInRole(Rights.View, "TechnicalCrew") AndAlso
                       IsInRole(Rights.View, "FlyingCrew") Then

                'All Flying Crew records along with records that are Unassigned [neither of Flying Crew nor Technical Crew]
                mEmployeeList = EmployeeList.GetEmployeeList(Name:=Name,
                                                             Designation:=Designation,
                                                             EmpNo:=EmpNo,
                                                             Contractor:=Contractor,
                                                             IsEmployeeWorking:=IsEmployeeWorking,
                                                             IsTechnicalCrew:=IsTechnicalCrew,
                                                             IsUseInLogRequired:=1,
                                                             ShowAllFlyingCrewAndUnassigned:=1)

            ElseIf Not IsInRole(Rights.View, "FlyingCrew") AndAlso
                   Not IsInRole(Rights.View, "TechnicalCrew") Then

                'Only those records that are Unassigned [neither of Flying Crew nor Technical Crew]
                mEmployeeList = EmployeeList.GetEmployeeList(Name:=Name,
                                                             Designation:=Designation,
                                                             EmpNo:=EmpNo,
                                                             Contractor:=Contractor,
                                                             IsEmployeeWorking:=IsEmployeeWorking,
                                                             SkipTechnicalCrewAndFlyingCrew:=1)

            ElseIf IsInRole(Rights.View, "FlyingCrew") AndAlso
                   IsInRole(Rights.View, "TechnicalCrew") Then

                'All records   [ Flying Crew, Technical Crew, Unassigned ]
                mEmployeeList = EmployeeList.GetEmployeeList(Name:=Name,
                                                             Designation:=Designation,
                                                             EmpNo:=EmpNo,
                                                             Contractor:=Contractor,
                                                             IsEmployeeWorking:=IsEmployeeWorking)

            End If

        ElseIf IsInRole(Rights.View, "FlyingCrew") Then

            'All Flying Crew records along with records that are Unassigned	[neither of Flying Crew nor Technical Crew]
            mEmployeeList = EmployeeList.GetEmployeeList(Name:=Name,
                                                         Designation:=Designation,
                                                         EmpNo:=EmpNo,
                                                         Contractor:=Contractor,
                                                         IsUseInLogRequired:=1,
                                                         IsEmployeeWorking:=IsEmployeeWorking,
                                                         IsTechnicalCrew:=IsTechnicalCrew,
                                                         ShowAllFlyingCrewAndUnassigned:=1)

        ElseIf Not IsInRole(Rights.View, "FlyingCrew") AndAlso
                   AppSettings("ShowAMOOnlyForNewClients").ToLower = "false" Then

            'Only those records that are Unassigned [neither of Flying Crew nor Technical Crew]
            mEmployeeList = EmployeeList.GetEmployeeList(Name:=Name,
                                                         Designation:=Designation,
                                                         EmpNo:=EmpNo,
                                                         Contractor:=Contractor,
                                                         IsEmployeeWorking:=IsEmployeeWorking,
                                                         SkipTechnicalCrewAndFlyingCrew:=1)

        End If

    End Sub
    'End

#End Region

#Region " DataBinding "

    Public Sub DataFieldBind()

        mEmployeeList = EmployeeList.GetEmployeeList("", "", "")
        dgEmployeeList.DataSource = mEmployeeList
        '======Added By Saylee on 12-Oct-2007==========
        Index = IIf(IsNothing(Index), 0, Index)
        Text = Session("Text")
        Session("Text") = Text
        Session("Index") = Index
        '============================================
        Session("mEmployeeList") = mEmployeeList
        dgEmployeeList.DataBind()
        lblResult.Text = "List of Employee as per criteria : " & mEmployeeList.Count & " Record(s) found."

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011

        If Not IsPostBack And Session("sender") = "" Then

            If Type <> 1 Then
                Session("MiddleFrame") = "wfEmployeeList_Ajax.aspx"
            End If

            If cmbLookIn.Enabled = True Then
                SetFocus(cmbLookIn)
            End If

            If Session("ShowNoE") Is Nothing Then

                cmbShowE.SelectedValue = "4"
                Session("ShowNoE") = cmbShowE.SelectedValue
                ShowNoE = cmbShowE.SelectedValue

            End If

            ' DataFieldBind()
            SetControl() 'Added By Saylee on 12-Oct-2007
            SetGrid()
            FillLookInDD()

            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Employee") Then
                ScriptManager.RegisterStartupScript(Me, [GetType], "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, [GetType], "RemoveFav", "RemoveFav();", True)
            End If

        Else 'Added To Fire RowCommand on Click Of View

        End If

    End Sub

    Private Sub SearchRecords(sender As Object, e As EventArgs) Handles btnFindNow.Click

        dgEmployeeList.PageIndex = 0
        Index = IIf(CInt(cmbLookIn.SelectedValue) < 0, 0, CInt(cmbLookIn.SelectedValue))
        Text = txtSearch.Text.Trim
        Session("Text") = Text
        Session("Index") = Index
        Session("ShowNotWorkingEmployee") = chkShownotworkingemployee.Checked

        FindNow(CInt(cmbLookIn.SelectedValue),
                Text,
                Text,
                Text,
                Text)

        SetVariables()
        dgEmployeeList.DataBind()
        SetGrid()

    End Sub

    Private Sub Add(sender As Object, e As EventArgs) Handles btnAdd.Click, btnAddTop.Click

        SetSession()
        Session("mEmployeeList") = Nothing
        mEmployeeList = Nothing
        NewRecord()

        If (Not User.IsInRole("EmployeeNew") And mEmployee.IsNew) Or
           (Not User.IsInRole("EmployeeEdit") And Not mEmployee.IsNew) Then

            SetSession()

            MarkLog(Action.Save, "Employee",
                    User.Identity.Name & " is not Authorized User to add",
                    ErrorType.HandledError,
                    Guid.Empty,
                    EventLogID)

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "Authorization")
            Exit Sub

        End If

        MarkLog(Action.[New],
                "Employee",
                "",
                ErrorType.NoError,
                mEmployee.ID,
                EventLogID)

        If Type = 1 Then

            Dim str As String

            str = "openledgersame('wfEmployee_Ajax.aspx?MainPage=wfEmployeeList_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "');"
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenScript",
                                                str,
                                                True)

        Else

            Dim str As String

            str = "openledgersame('wfEmployee_Ajax.aspx?MainPage=wfEmployeeList_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "');"
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenScript",
                                                str,
                                                True)

        End If

    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click, btnCloseTop.Click

        RemoveSession()
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")

    End Sub

    Private Sub DD_Lookin_Changed(sender As Object, e As EventArgs) Handles cmbLookIn.SelectedIndexChanged

        Dim Index As Int32 = CInt(cmbLookIn.SelectedValue)
        txtSearch.Text = ""
        lblFor.Visible = IIf(Index = 0 Or Index = 5 Or Index = 6, False, True)
        txtSearch.Visible = IIf(Index = 0 Or Index = 5 Or Index = 6, False, True)

        If cmbLookIn.Enabled = True Then

            SetFocus(cmbLookIn)

        End If

    End Sub

    Private Sub GV_EmployeeList_PageIndexChanged(source As Object, e As GridViewPageEventArgs) Handles dgEmployeeList.PageIndexChanging

        dgEmployeeList.PageIndex = e.NewPageIndex
        dgEmployeeList.DataSource = mEmployeeList
        Session("mEmployeeList") = mEmployeeList

        dgEmployeeList.DataBind()
        SetGrid()

    End Sub

    Protected Sub GV_EmployeeList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgEmployeeList.RowCommand

        Dim Index As Integer
        Dim mID As Guid

        Select Case e.CommandName

            Case "EditRec"

                Index = CInt(e.CommandArgument) + dgEmployeeList.PageSize * dgEmployeeList.PageIndex
                mID = mEmployeeList(Index).ID
                If (Not User.IsInRole("EmployeeView") And Not User.IsInRole("EmployeeEdit")) Then

                    SetSession()
                    MarkLog(Action.Edit,
                            "Employee",
                            User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName,
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                Session("mEmployeeList") = Nothing
                mEmployeeList = Nothing
                EditRecord(mID)

                MarkLog(Action.Edit,
                        "Employee",
                        mEmployee.EmpNoName,
                        ErrorType.NoError,
                        mEmployee.ID,
                        EventLogID)

                If Type = 1 Then

                    Dim str As String
                    str = "openledgersame('wfEmployee_Ajax.aspx?BackPage2=index.aspx&Type=" & Request.QueryString("Type") & "');"
                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        str,
                                                        True)

                Else

                    Dim str As String
                    str = "openledgersame('wfEmployee_Ajax.aspx?MainPage=wfEmployeeList_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "');"
                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "OpenScript",
                                                        str,
                                                        True)

                End If

            Case "DeleteRec"

                Index = CInt(e.CommandArgument) + dgEmployeeList.PageSize * dgEmployeeList.PageIndex
                mID = mEmployeeList(Index).ID

                If (Not User.IsInRole("EmployeeDelete")) Then

                    SetSession()
                    MarkLog(Action.Delete,
                            "Employee",
                            User.Identity.Name & " is not Authorized User to delete " + mEmployee.EmpNoName,
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                DeleteRecord(mID)

            Case "View"

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------

                Index = rowIndex
                mID = New Guid(dgEmployeeList.DataKeys(Index).Value.ToString)

                mEmployee = Employee.GetEmployee(mID)

                If mEmployee.ImageSize > 0 Then

                    Dim path As String = AppSettings("DOCPath") & StrName & mEmployee.FileExtension
                    Dim fs As FileStream

                    If File.Exists(AppSettings("DOCPath")) = False Then

                        'Delete File if exist
                        IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployee.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mEmployee.ImageFile, 0, mEmployee.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFilel();", True)

                    End If

                Else

                End If

            Case "PrintRec"

                If (Not User.IsInRole("EmployeePrint")) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Exit Sub

                End If

                Index = CInt(e.CommandArgument) + dgEmployeeList.PageSize * dgEmployeeList.PageIndex
                mID = mEmployeeList(Index).ID

                mEmployee = Employee.GetEmployee(mID)

                Dim Rpt As Engine.ReportClass
                Dim ds As New dsEmployeeDetails
                Dim da As New ObjectAdapter
                Dim mCompanyDetail As New CompanyDetail

                Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "PRELIMINARY DEFECT REPORT",
                                             "",
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

                Rpt = New crEmployeeTag

                '-----------Added by Utkarsh for Report Logo---------------
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                '----------------------------------------------------------
                Dim mEmployeePhoto As EmployeePhoto

                mEmployeePhoto = EmployeePhoto.GetImage(ds,
                                                        mEmployee.ID.ToString,
                                                        AppSettings("DefaultImagePath") & "\photo-not-available-1.png")
                da.Fill(ds, mEmployee)
                da.Fill(ds, Report)
                da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
                da.Fill(ds, mEmployeePhoto)
                Rpt.SetDataSource(ds)
                Session("CrystalReport") = Rpt

                DataFieldBind()
                SetControl()
                SetGrid()

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openTranDetail1",
                                                    "openTranDetail();",
                                                    True)
                'Added by Vikrant on 11-Nov-2019 For ALL08112019

            Case "DocsAddRemove"

                'Added by Saylee on 7-Mar-2014 for ALL07032014
                If (Not IsInRole(Rights.View, "EmployeeDocuments") And Not IsInRole(Rights.Edit, "EmployeeDocuments")) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                mEmployee = Employee.GetEmployee(New Guid(dgEmployeeList.DataKeys(CInt(e.CommandArgument)).Value.ToString))

                Session("mEmployee") = mEmployee
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenToAddDocDetail",
                                                    "OpenToAddDocDetail();",
                                                    True)

            Case "TrainingAddRemove"

                'Added by Saylee on 7-Mar-2014 for ALL07032014
                If (Not IsInRole(Rights.View, "EmployeeTraining") And
                    Not IsInRole(Rights.Edit, "EmployeeTraining")) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                mEmployee = Employee.GetEmployee(New Guid(dgEmployeeList.DataKeys(CInt(e.CommandArgument)).Value.ToString))

                Session("mEmployee") = mEmployee
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenToAddTrainingDetail",
                                                    "OpenToAddTrainingDetail();",
                                                    True)
                'End

        End Select

    End Sub

    'Added By Prashant 23-June-2009 for grid sorting
    Private Sub GV_EmployeeList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgEmployeeList.Sorting

        mEmployeeList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mEmployeeList") = mEmployeeList
        dgEmployeeList.DataSource = mEmployeeList
        dgEmployeeList.DataBind()
        SetGrid()

    End Sub

    '-----------------------------------------------
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MessageBoxResult()

    End Sub

    'Added by Vikrant on 11-Nov-2019 For ALL08112019
    Private Sub HdnBtnAddDocDetail_Click(sender As Object, e As EventArgs) Handles hdnBtnAddDocDetail.Click, hdnBtnAddTrainingDetail.Click

		SetControl()
		SetGrid()
		upnlGrid.Update()

	End Sub

	Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFav.Click

		Try
			MarkFavourite(HttpContext.Current.User.Identity.Name, "Employee")
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFav.Click

		Try
			RemoveFavourite(HttpContext.Current.User.Identity.Name, "Employee")
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)

		dgEmployeeList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
		dgEmployeeList.DataSource = mEmployeeList
		dgEmployeeList.DataBind()

		ControlVisibility(0)
		SetVariables()
		SetControl()
		upnlGrid.Update()

	End Sub
	'END

#End Region

End Class