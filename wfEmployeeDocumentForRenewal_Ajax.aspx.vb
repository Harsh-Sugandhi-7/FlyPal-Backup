Public Class wfEmployeeDocumentForRenewal_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mEmployee As Employee
    'EMPLOYEE DOCUMENT
    Public mEmployeeDocumentDueList As EmployeeDocumentDueList
    Public mEmployeeDocument As EmployeeDocument
    Dim Type As Int16
    Public mEmployeeList As EmployeeList
    Dim var As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mEmployeeList = Session("mEmployeeList")
        mEmployeeDocumentDueList = Session("mEmployeeDocumentDueList")
        mEmployee = CType(Session("mEmployee"), Employee)
    End Sub
    Private Sub ControlVisibility()
        lblNote.Visible = (Not chkExpiredEntries.Checked)
        dgDocumentList.Columns(14).Visible = (chkNotApplicable.Checked)
        dgDocumentList.Columns(15).Visible = (Not chkNotApplicable.Checked)
    End Sub
    Private Sub SetSession()
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfEmployeeDocumentForRenewal_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmployeeList")
        Session.Remove("mEmployeeDocumentDueList")
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
        'DOCUMENT LIST
        mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(Date.Today.ToString, IIf(chkNotApplicable.checked, 2, 0))
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataBind()
        lblResult.Text = "The following Document(s) are due for renewal :" & mEmployeeDocumentDueList.Count & " Record(s) found."
        '----------
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(All)")
        cmbEmployeeList.DataSource = mEmployeeList
        cmbEmployeeList.DataBind()
        Session("mEmployeeList") = mEmployeeList
    End Sub
    Private Sub BindgGrids(Optional ByVal Name As String = "")
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        dgDocumentList.DataBind()
    End Sub
    Private Sub FindNow()
        mEmployeeDocumentDueList = Nothing
        dgDocumentList.DataSource = Nothing
        Dim ID As Guid
        If EmployeeIDValue.Value <> "" Then
            ID = New Guid(EmployeeIDValue.Value)
        Else
            ID = Guid.Empty
        End If

        mEmployeeDocumentDueList = EmployeeDocumentDueList.GetEmployeeDocumentDueList(ID, Guid.Empty, "", Date.Today.ToString, 1, _
                                                                                      chkUsedInFlightLog.Checked, chkExpiredEntries.Checked, IIf(chkNotApplicable.checked, 2, 0))
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        dgDocumentList.DataBind()

        ControlVisibility()
        upnlDocumentGrid.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'ClearAll()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            If Type <> 1 Then
                Session("MiddleFrame") = "wfEmployeeDocumentForRenewal_Ajax.aspx"
            End If
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub dgDocumentList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgDocumentList.PageIndexChanging
        dgDocumentList.PageIndex = e.NewPageIndex
        dgDocumentList.DataSource = mEmployeeDocumentDueList
        Session("mEmployeeDocumentDueList") = mEmployeeDocumentDueList
        dgDocumentList.DataBind()
    End Sub
    Private Sub dgDocumentList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDocumentList.RowCommand
        Dim mID As Guid
        Dim EmployeeID As Guid
        BindgGrids("Document")
        Select Case e.CommandName
            Case "Renew"
                mID = New Guid(e.CommandArgument.ToString)
                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                mEmployee = Employee.GetEmployee(mEmployeeDocument.EmployeeID)
                If User.IsInRole("EmployeeDocumentsEdit") = False Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim mEmployeeDocumentList As EmployeeDocumentList
                mEmployeeDocumentList = EmployeeDocumentList.GetEmployeeDocumentList(EmployeeID)
                Session("mEmployeeDocumentList") = mEmployeeDocumentList
                SetSession()
                mEmployeeDocument = EmployeeDocument.NewRenew(mEmployeeDocument, True)
                Session("IsRenew") = True
                Session("mEmployeeDocument") = mEmployeeDocument
                MarkLog(Flypal.Util.Action.Comply, "EmployeeDocumentForRenewal", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow()", True)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                mID = New Guid(e.CommandArgument.ToString)
                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                If mEmployeeDocument.ImageSize > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mEmployeeDocument.FileExtension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDocument.FileExtension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mEmployeeDocument.ImageFile, 0, mEmployeeDocument.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
            Case "History"
                mID = New Guid(e.CommandArgument.ToString)
                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                Session("mEmployeeDocument") = mEmployeeDocument
                Dim mEmployeeID As Guid = mEmployeeDocument.EmployeeID
                Session("mEmployeeID") = mEmployeeID.ToString
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentHistoryWindow", "OpenEmpDocumentHistoryWindow()", True)
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        lblResult.Text = "The following Document(s) are due for renewal :" & mEmployeeDocumentDueList.Count & " Record(s) found."
    End Sub
#End Region

#Region "Document"
    Private Sub hdnBtnEmpDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpDocument.Click
        FindNow()
        lblResult.Text = "The following Document(s) are due for renewal :" & mEmployeeDocumentDueList.Count & " Record(s) found."
    End Sub
#End Region
End Class