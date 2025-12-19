Public Class wfEmployeeTrainingForRenewal_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mEmployee As Employee
    'EMPLOYEE TRAINING
    Public mEmployeeTrainningDueList As EmployeeTrainningDueList
    Public mEmployeeTraining As EmployeeTraining
    Public mTraining As Training
    Public mFreqInMonths As Integer = 0
    Dim Type As Int16
    Public mEmployeeList As EmployeeList 'Added By Vikrant On 18-Apr-2013 For ALL18042013
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mEmployeeList = Session("mEmployeeList")
        mEmployeeTrainningDueList = Session("mEmployeeTrainningDueList")
        mEmployee = CType(Session("mEmployee"), Employee)
    End Sub
    Private Sub SetSession()
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfEmployeeTrainingForRenewal_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeList")
        Session.Remove("mEmployeeTrainningDueList")
        Session.Remove("mEmployee")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        'TRAINING LIST
        mEmployeeTrainningDueList = EmployeeTrainningDueList.GetEmployeeTrainningDueList(Date.Today.ToString)
        dgTrainingList.DataSource = mEmployeeTrainningDueList
        Session("mEmployeeTrainningDueList") = mEmployeeTrainningDueList
        dgTrainingList.DataBind()
        lblResult1.Text = "The following Training(s) are due for renewal :" & mEmployeeTrainningDueList.Count & " Record(s) found."
        '----------
        'Added By Vikrant On 18-Apr-2013 For ALL18042013
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)")
        cmbEmployeeList.DataSource = mEmployeeList
        cmbEmployeeList.DataBind()
        Session("mEmployeeList") = mEmployeeList
        'End
    End Sub
    Private Sub BindgGrids(Optional ByVal Name As String = "")
        dgTrainingList.DataSource = mEmployeeTrainningDueList
        dgTrainingList.DataBind()
    End Sub
    'Added By Vikrant On 18-Apr-2013 For ALL18042013
    Private Sub FindNow()
        mEmployeeTrainningDueList = Nothing
        dgTrainingList.DataSource = Nothing
        Dim ID As Guid
        If EmployeeIDValue.Value <> "" Then
            ID = New Guid(EmployeeIDValue.Value)
        Else
            ID = Guid.Empty
        End If
        mEmployeeTrainningDueList = EmployeeTrainningDueList.GetEmployeeTrainningDueList(ID, Guid.Empty, Guid.Empty, Date.Today.ToString, 1, _
                                                                                         chkUsedInFlightLog.Checked, chkExpiredEntries.Checked)
        Session("mEmployeeTrainningDueList") = mEmployeeTrainningDueList
        dgTrainingList.DataSource = mEmployeeTrainningDueList
        dgTrainingList.DataBind()
        upnlTrainingGrid.Update()
    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'ClearAll()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            If Type <> 1 Then
                Session("MiddleFrame") = "wfEmployeeTrainingForRenewal_Ajax.aspx"
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub dgTrainingList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingList.PageIndexChanging
        dgTrainingList.PageIndex = e.NewPageIndex
        dgTrainingList.DataSource = mEmployeeTrainningDueList
        Session("mEmployeeTrainningDueList") = mEmployeeTrainningDueList
        dgTrainingList.DataBind()
    End Sub
    'EMPLOYEE TRAINING
    Private Sub dgTrainingList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingList.RowCommand
        Dim mID As Guid
        Dim EmployeeID As Guid
        BindgGrids("Training")
        Select Case e.CommandName
            Case "Renew"
                mID = New Guid(e.CommandArgument.ToString)
                mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
                mEmployee = Employee.GetEmployee(mEmployeeTraining.EmployeeID)
                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeTrainingEdit") = False Then
                    SetSession()
                    MarkLog(Action.Edit, "Employee Training", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                '*******************************
                mTraining = Training.GetTraining(mEmployeeTraining.TrainingID)
                mFreqInMonths = mTraining.FreqInMonths
                'Added By Prashant 18-May-2011
                'TRAINING LIST
                Dim mEmployeeTrainingList As EmployeeTrainingList
                mEmployeeTrainingList = EmployeeTrainingList.GetEmployeeTrainingList(EmployeeID)
                Session("mEmployeeTrainingList") = mEmployeeTrainingList
                '-----------------------------
                SetSession()
                mEmployeeTraining = EmployeeTraining.NewRenew(mEmployeeTraining, mFreqInMonths, True)
                Session("mEmployeeTraining") = mEmployeeTraining
                Session("IsRenew") = True
                MarkLog(Flypal.Util.Action.Comply, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + mEmployeeTraining.TrainingName, Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", "OpenEmpTrainingWindow()", True)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mID = New Guid(e.CommandArgument.ToString)
                Dim mFileAttach As FileAttach
                mFileAttach = FileAttach.GetAttachment(mID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
            Case "History"
                mID = New Guid(e.CommandArgument.ToString)
                mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
                Dim mEmployeeID As Guid = mEmployeeTraining.EmployeeID 'CType(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("EmployeeID"), Guid)
                Session("mEmployeeID") = mEmployeeID.ToString
                Session("mEmployeeTraining") = mEmployeeTraining
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingHistoryWindow", "OpenEmpTrainingHistoryWindow()", True)
        End Select
    End Sub
    '-----END OF EMPLOYEE TRAINING
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Added By Vikrant On 18-Apr-2013 For ALL18042013
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        lblResult1.Text = "The following Training(s) are due for renewal :" & mEmployeeTrainningDueList.Count & " Record(s) found."
    End Sub
    'End
#End Region

#Region "Document & Training"
    Private Sub hdnBtnEmpTraining_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpTraining.Click
        FindNow()
        lblResult1.Text = "The following Training(s) are due for renewal :" & mEmployeeTrainningDueList.Count & " Record(s) found."
    End Sub
#End Region
End Class