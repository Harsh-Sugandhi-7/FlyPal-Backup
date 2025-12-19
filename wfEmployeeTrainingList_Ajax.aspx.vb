'Added by vikrant on 11-Nov-2019 For ALL08112019
Public Class wfEmployeeTrainingList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeTraining As EmployeeTraining
    Public mEmployeeTrainingList As EmployeeTrainingList
    Dim EventLogID As Guid
    Public mTraining As Training
    Public mFreqInMonths As Integer = 0
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mEmployee = CType(Session("mEmployee"), Employee)
    End Sub
    Private Sub SetSession()
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeList")
        Session.Remove("Text")
        Session.Remove("Index")
    End Sub
    Private Sub NewTrainingRecord()
        mEmployeeTraining = EmployeeTraining.NewEmployeeTraining
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub EditTrainingRecord(ByVal mID As Guid)
        mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub DeleteTrainingRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteTraining")
        mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteTraining" Then
                        Dim TrainingName As String
                        Try
                            Session("sender") = ""
                            mEmployeeTraining = Session("mEmployeeTraining")
                            TrainingName = mEmployeeTraining.TrainingName
                            EmployeeTraining.DeleteEmployeeTraining(mEmployeeTraining.ID)
                            DataFieldBind()
                            SetGrid()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Training", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Training : " + TrainingName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + TrainingName, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetGrid()
        Dim t As Boolean        'Training

        Dim TrainingHistoryCount As Boolean
        For n As Integer = 0 To dgTrainingList.Rows.Count - 1
            t = CType(Me.dgTrainingList.Rows(n).Cells(14).Text, Boolean)
            TrainingHistoryCount = CType(Me.dgTrainingList.Rows(n).Cells(16).Text, Boolean)
            If t = False Then
                dgTrainingList.Rows(n).Cells(13).Enabled = False
            End If
            If TrainingHistoryCount = False Then
                dgTrainingList.Rows(n).Cells(15).Enabled = False
            End If
            If mEmployeeTrainingList.Item(n).RecurringStatus = True Then
                dgTrainingList.Rows(n).Cells(10).Enabled = True
            Else
                dgTrainingList.Rows(n).Cells(10).Enabled = False
            End If
        Next
    End Sub
    Private Sub ControlEnability()
        'If mEmployeeTrainingList.Count > 15 Then
        '    btnAddTop.Visible = True
        '    btnBackTop.Visible = True
        'Else
        '    btnAddTop.Visible = False
        '    btnBackTop.Visible = False
        'End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mEmployeeTrainingList = EmployeeTrainingList.GetEmployeeTrainingList(mEmployee.ID)
        dgTrainingList.DataSource = mEmployeeTrainingList
        Session("mEmployeeTrainingList") = mEmployeeTrainingList
        lblTraining.Text = "List of Training : " & mEmployeeTrainingList.Count.ToString & " Record(s) found."
        DataBind() 'CHK Bind TextBox Individually
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 19-July-2011
        If Not IsPostBack Then
            DataFieldBind()
            SetGrid()  'Added By Utkarsh On 4-May-2011
            ControlEnability()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        If User.IsInRole("EmployeeTrainingNew") = False Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewTrainingRecord()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", "OpenEmpTrainingWindow();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTrainingGroupWindow", "OpenTrainingGroupWindow();", True)
    End Sub
    Private Sub dgDocumentList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingList.RowCommand
        Dim Idx As Int32
        Dim mID As New Guid
        'Dim mName As String
        Select Case e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeTrainingEdit") = False Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*******************************
                EditTrainingRecord(mID)
                Session("IsRenew") = False
                MarkLog(Flypal.Util.Action.Edit, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + mEmployeeTraining.TrainingName, Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", "OpenEmpTrainingWindow();", True)
                'Response.Redirect("wfEmployeeTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeTrainingDelete") = False Then
                    SetSession()
                    MarkLog(Util.Action.Delete, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*******************************
                DeleteTrainingRecord(mID)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------

                'Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                'Dim rowIndex As Integer = gvr.RowIndex
                'Idx = rowIndex + dgTrainingList.PageIndex * dgTrainingList.PageSize
                'mID = New Guid(dgTrainingList.DataKeys(rowIndex).Values("ID").ToString)

                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)


                'mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)

                Dim mFileAttach As FileAttach
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach

                If mFileAttach.Size > 0 Then
                    'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
            Case "Renew"
                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeTrainingEdit") = False Then
                    MarkLog(Action.Edit, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*******************************
                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

                mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)

                mTraining = Training.GetTraining(mEmployeeTraining.TrainingID)
                mFreqInMonths = mTraining.FreqInMonths

                SetSession()

                mEmployeeTraining = EmployeeTraining.NewRenew(mEmployeeTraining, mFreqInMonths, True)

                Session("mEmployeeTraining") = mEmployeeTraining
                Session("IsRenew") = True
                Session.Remove("mFileAttach")
                MarkLog(Flypal.Util.Action.Comply, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + mEmployeeTraining.TrainingName, Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", "OpenEmpTrainingWindow();", True)
                'Response.Redirect("wfEmployeeTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfEmployeeDetails_Ajax.aspx")
            Case "History"
                ' Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                'Dim rowIndex As Integer = gvr.RowIndex
                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = New Guid(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

                mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
                Dim mEmployeeID As Guid = CType(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("EmployeeID"), Guid)
                Session("mEmployeeID") = mEmployeeID.ToString
                Session("mEmployeeTraining") = mEmployeeTraining
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingHistoryWindow", "OpenEmpTrainingHistoryWindow();", True)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnEmpTraining_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpTraining.Click
        DataFieldBind()
        SetGrid()
        ControlEnability()
        upnlGrid.Update()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        SetReport()
    End Sub
    Public Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As EmployeeTrainningRegister
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsEmployeeTrainningRegister

        myReport = New crEmployeeTrainingList

        Dim SearchStr1 As String
        Dim SearchStr2 As String
        Dim SearchStr3 As String
        Dim SearchStr4 As String

        If txtName.Text <> "" Then
            SearchStr1 = txtName.Text
        Else
            SearchStr1 = ""
        End If


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                                     mCompanyDetail.WebSite, "Employee Training Register", SearchStr1, SearchStr2, SearchStr3,
                                     SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"),
                                     "", "", "", "", AppSettings("Logo"))

        obj = EmployeeTrainningRegister.GetEmployeeTrainningRegister("", mEmployee.ID.ToString, Guid.Empty.ToString(), Guid.Empty.ToString())

        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
    End Sub
#End Region

End Class