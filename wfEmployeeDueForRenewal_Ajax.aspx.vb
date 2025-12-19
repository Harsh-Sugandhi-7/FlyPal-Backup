'AJAX Conversion By Vikrant

Public Class wfEmployeeDueForRenewal_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mEmployee As Employee

    'EMPLOYEE DOCUMENT
    Public mEmployeeDocumentDueList As EmployeeDocumentDueList
    Public mEmployeeDocument As EmployeeDocument

    'EMPLOYEE TRAINING
    Public mEmployeeTrainningDueList As EmployeeTrainningDueList
    Public mEmployeeTraining As EmployeeTraining
    Public mTraining As Training
    Public mFreqInMonths As Integer = 0

    Dim Type As Int16

    Public IsFromRenewal As String = ""
    Public mEmployeeList As EmployeeList 'Added By Vikrant On 18-Apr-2013 For ALL18042013
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        'Added By Vikrant On 18-Apr-2013 For ALL18042013
        mEmployeeList = Session("mEmployeeList")
        'End
        mEmployeeDocumentDueList = Session("mEmployeeDocumentDueList")
        mEmployeeTrainningDueList = Session("mEmployeeTrainningDueList")
        mEmployee = CType(Session("mEmployee"), Employee)
    End Sub
    Private Sub ControlVisibility()
        lblNote.Visible = (Not chkExpiredEntries.Checked)
    End Sub
    Private Sub SetSession()
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfEmployeeDueForRenewal_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub RemoveSession()
        'Added By Vikrant On 11-Dec-2013 For AJAX Conversion
        Session.Remove("mEmployeeList")
        'End
        Session.Remove("mEmployeeDocumentDueList")
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
                    '
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'Response.Redirect("wfEmployeeDetails.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeDetails.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeDetails.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfEmployeeDetails.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()

        'DOCUMENT LIST
        mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(Date.Today.ToString)
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataBind()
        lblResult.Text = "The following Document(s) are due for renewal :" & mEmployeeDocumentDueList.Count & " Record(s) found."
        '----------

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

        'DataBind()
    End Sub
    Private Sub BindgGrids(Optional ByVal Name As String = "")
        If Name = "Document" Then
            dgDocumentList.DataSource = mEmployeeDocumentDueList
            dgDocumentList.DataBind()
        ElseIf Name = "Training" Then
            dgTrainingList.DataSource = mEmployeeTrainningDueList
            dgTrainingList.DataBind()
        Else
            dgDocumentList.DataSource = mEmployeeDocumentDueList
            dgDocumentList.DataBind()
            dgTrainingList.DataSource = mEmployeeTrainningDueList
            dgTrainingList.DataBind()
        End If
    End Sub
    'Added By Vikrant On 18-Apr-2013 For ALL18042013
    Private Sub FindNow()
        mEmployeeDocumentDueList = Nothing
        dgDocumentList.DataSource = Nothing

        mEmployeeTrainningDueList = Nothing
        dgTrainingList.DataSource = Nothing
        Dim ID As Guid
        If EmployeeIDValue.Value <> "" Then
            ID = New Guid(EmployeeIDValue.Value)
        Else
            ID = Guid.Empty
        End If

        mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(ID, Guid.Empty, "", Date.Today.ToString, 0, chkUsedInFlightLog.Checked, chkExpiredEntries.Checked)
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        dgDocumentList.DataBind()

        ControlVisibility()
        upnlDocumentGrid.Update()

        mEmployeeTrainningDueList = EmployeeTrainningDueList.GetEmployeeTrainningDueList(ID, Guid.Empty, Guid.Empty, Date.Today.ToString, 0, chkUsedInFlightLog.Checked, chkExpiredEntries.Checked)
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
                Session("MiddleFrame") = "wfEmployeeDueForRenewal_Ajax.aspx"
            End If
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnEmployeeMaster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmployeeMaster.Click
        Response.Redirect("wfEmployeeList_Ajax.aspx?BackPage=index.aspx")
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
        'Dim Index As Integer
        BindgGrids("Training")
        Select Case e.CommandName
            Case "Renew"
                'Index = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = CType(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("ID"), Guid)
                EmployeeID = CType(dgTrainingList.DataKeys(CInt(e.CommandArgument)).Values("EmployeeID"), Guid)
                mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
                mEmployee = Employee.GetEmployee(EmployeeID)

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
                'Dim str As String
                'str = "openledgersame('wfEmployeeTraining_Ajax.aspx?BackPage=index.aspx&IsFromRenewal=" & "True" & "');"
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", str, True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingWindow", "OpenEmpTrainingWindow()", True)
        End Select
    End Sub
    Private Sub dgDocumentList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDocumentList.PageIndexChanging
        dgDocumentList.PageIndex = e.NewPageIndex
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataBind()
    End Sub
    '-----END OF EMPLOYEE TRAINING

    'EMPLOYEE DOCUMENT
    Private Sub dgDocumentList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDocumentList.RowCommand
        Dim mID As Guid
        Dim EmployeeID As Guid
        'Dim Index As Integer
        BindgGrids("Document")
        Select Case e.CommandName
            Case "Renew"
                'Index = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
                mID = CType(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID"), Guid)
                EmployeeID = CType(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("EmployeeID"), Guid)

                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                mEmployee = Employee.GetEmployee(EmployeeID)

                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeDocumentsEdit") = False Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                '*******************************

                'Added By Prashant 18-May-2011
                'DOCUMENT LIST
                Dim mEmployeeDocumentList As EmployeeDocumentList
                mEmployeeDocumentList = EmployeeDocumentList.GetEmployeeDocumentList(EmployeeID)
                Session("mEmployeeDocumentList") = mEmployeeDocumentList
                '-----------------------------

                SetSession()

                mEmployeeDocument = EmployeeDocument.NewRenew(mEmployeeDocument, True)

                Session("IsRenew") = True
                Session("mEmployeeDocument") = mEmployeeDocument
                MarkLog(Flypal.Util.Action.Comply, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
                'Dim str As String
                'str = "openledgersame('wfEmployeeDocument_Ajax.aspx?BackPage=index.aspx');"
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow()", True)
        End Select
    End Sub
    '------END OF EMPLOYEE DOCUMENT
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Added By Vikrant On 18-Apr-2013 For ALL18042013
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()

        lblResult.Text = "The following Document(s) are due for renewal :" & mEmployeeDocumentDueList.Count & " Record(s) found."
        lblResult1.Text = "The following Training(s) are due for renewal :" & mEmployeeTrainningDueList.Count & " Record(s) found."
    End Sub
    'End
#End Region

#Region "Document & Training"
    Private Sub hdnBtnEmpDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpDocument.Click
        mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(Date.Today.ToString)
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataBind()

        dgTrainingList.DataSource = mEmployeeTrainningDueList
        dgTrainingList.DataBind()

        lblResult.Text = "The following Document(s) are due for renewal :" & mEmployeeDocumentDueList.Count & " Record(s) found."
        ControlVisibility()
        upnlDocumentGrid.Update()
        upnlTrainingGrid.Update()
    End Sub
    Private Sub hdnBtnEmpTraining_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpTraining.Click
        mEmployeeTrainningDueList = EmployeeTrainningDueList.GetEmployeeTrainningDueList(Date.Today.ToString)
        dgTrainingList.DataSource = mEmployeeTrainningDueList
        Session("mEmployeeTrainningDueList") = mEmployeeTrainningDueList
        dgTrainingList.DataBind()
        lblResult1.Text = "The following Training(s) are due for renewal :" & mEmployeeTrainningDueList.Count & " Record(s) found."

        mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(Date.Today.ToString)
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataBind()

        ControlVisibility()
        upnlTrainingGrid.Update()
        upnlDocumentGrid.Update()
    End Sub
#End Region
End Class