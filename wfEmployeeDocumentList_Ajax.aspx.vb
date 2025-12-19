'Added by vikrant on 11-Nov-2019 For ALL08112019
Public Class wfEmployeeDocumentList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeDocument As EmployeeDocument
    Public mEmployeeDocumentList As EmployeeDocumentList
    Public mEmployeeDocumentHistoryList As EmployeeDocumentHistoryList
    Dim EventLogID As Guid
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
    Private Sub NewDocumentRecord()
        mEmployeeDocument = EmployeeDocument.NewEmployeeDocument
        Session("mEmployeeDocument") = mEmployeeDocument
    End Sub
    Private Sub EditDocumentRecord(ByVal mID As Guid)
        mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
        Session("mEmployeeDocument") = mEmployeeDocument
    End Sub
    Private Sub DeleteDocumentRecord(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDocument")
        mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
        Session("mEmployeeDocument") = mEmployeeDocument
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteDocument" Then
                        Try
                            Session("sender") = ""
                            mEmployeeDocument = Session("mEmployeeDocument")
                            EmployeeDocument.DeleteEmployeeDocument(mEmployeeDocument.ID)
                            DataFieldBind()
                            SetGrid()
                            ControlEnability()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Document", "Can't delete : " + "Emp : " + mEmployee.EmpNoName + " Document ; " + mEmployeeDocument.DocumentName + " is Currently in use", Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
        Dim s As Integer   'Document
        Dim lnkDocumentView As LinkButton 'ButtonColumn 
        Dim lnkDocumentHistory As LinkButton
        Dim DocumentHistoryCount As Boolean
        Dim IsDocumentApplicable As Boolean
        Dim OneTimeDocument As Boolean = False
        For m As Integer = 0 To dgDocumentList.Rows.Count - 1
            s = CType(Me.dgDocumentList.Rows.Item(m).Cells(16).Text, Integer)
            DocumentHistoryCount = CType(Me.dgDocumentList.Rows.Item(m).Cells(18).Text, Boolean)
            IsDocumentApplicable = CType(Me.dgDocumentList.Rows.Item(m).Cells(19).Text, Boolean)
            OneTimeDocument = CType(Me.dgDocumentList.Rows.Item(m).Cells(20).Text, Boolean) 'Added by Prashant 0n 24-Nov-2020 ALL24112020
            If s <= 0 Then
                lnkDocumentView = CType(dgDocumentList.Rows.Item(m).Cells(15).FindControl("lnkDocumentView"), LinkButton)
                lnkDocumentView.Enabled = False
            End If
            If DocumentHistoryCount = False Then
                lnkDocumentHistory = CType(dgDocumentList.Rows.Item(m).Cells(17).FindControl("lnkDocumentHistory"), LinkButton)
                lnkDocumentHistory.Enabled = False
            End If
            If IsDocumentApplicable = False Then
                dgDocumentList.Rows(m).Cells(12).Enabled = False
            End If
            If OneTimeDocument = True Then 'Added by Prashant 0n 24-Nov-2020 ALL24112020
                dgDocumentList.Rows(m).Cells(12).Enabled = False 'Renew link
            End If
        Next
    End Sub
    Private Sub ControlEnability()
        'If mEmployeeDocumentList.Count > 15 Then
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
        'DOCUMENT LIST
        mEmployeeDocumentList = EmployeeDocumentList.GetEmployeeDocumentList(mEmployee.ID)
        dgDocumentList.DataSource = mEmployeeDocumentList
        Session("mEmployeeDocumentList") = mEmployeeDocumentList
        lblDocs.Text = "List of Document : " & mEmployeeDocumentList.Count.ToString & " Record(s) found."
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
    Private Sub btnDocumentAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        If User.IsInRole("EmployeeDocumentsNew") = False Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        NewDocumentRecord()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow();", True)
    End Sub
    Private Sub dgDocumentList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDocumentList.RowCommand
        Dim Idx As Int32
        Dim mID As New Guid
        Select Case e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
                mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)

                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeDocumentsEdit") = False Then
                    MarkLog(Util.Action.Edit, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*******************************
                EditDocumentRecord(mID)
                Session("IsRenew") = False
                MarkLog(Flypal.Util.Action.Edit, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow();", True)
            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
                mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeDocumentsDelete") = False Then
                    MarkLog(Util.Action.Delete, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*******************************
                DeleteDocumentRecord(mID)
            Case "View"
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                Idx = rowIndex + dgDocumentList.PageIndex * dgDocumentList.PageSize
                mID = New Guid(dgDocumentList.DataKeys(rowIndex).Values("ID").ToString)

                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                If mEmployeeDocument.ImageSize > 0 Then
                    'Dim path As String = AppSettings("FilePath") & "\" & StrName & mCalibrationItemChild.FileExtension
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
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                Else
                    MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
                End If
                'New addition by Amrita for Document Renewal
            Case "Renew"
                'Added By Prashant On 17-July-2012
                If User.IsInRole("EmployeeDocumentsEdit") = False Then
                    MarkLog(Util.Action.Edit, "Employee Document", User.Identity.Name & " is not Authorized User to edit " + mEmployee.EmpNoName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                '*******************************
                Idx = CInt(e.CommandArgument) + dgDocumentList.PageIndex * dgDocumentList.PageSize
                mID = New Guid(dgDocumentList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                SetSession()
                'NewDocumentRecord()
                mEmployeeDocument = EmployeeDocument.NewRenew(mEmployeeDocument, True)
                Session("IsRenew") = True
                Session("mEmployeeDocument") = mEmployeeDocument
                Session.Remove("mFileAttach")
                MarkLog(Flypal.Util.Action.Comply, "Employee Document", "Emp : " + mEmployee.EmpNoName + " Document : " + mEmployeeDocument.DocumentName, Flypal.Util.ErrorType.NoError, mEmployeeDocument.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentWindow", "OpenEmpDocumentWindow();", True)
            Case "History"
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim rowIndex As Integer = gvr.RowIndex
                Idx = rowIndex + dgDocumentList.PageIndex * dgDocumentList.PageSize
                mID = New Guid(dgDocumentList.DataKeys(rowIndex).Values("ID").ToString)

                mEmployeeDocument = EmployeeDocument.GetEmployeeDocument(mID)
                Session("mEmployeeDocument") = mEmployeeDocument
                Dim mEmployeeID As Guid = New Guid(dgDocumentList.DataKeys(rowIndex).Values("EmployeeID").ToString)
                Session("mEmployeeID") = mEmployeeID.ToString
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpDocumentHistoryWindow", "OpenEmpDocumentHistoryWindow();", True)
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
    Private Sub hdnBtnEmpDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnEmpDocument.Click
        DataFieldBind()
        SetGrid()
        ControlEnability()
        upnlGrid.Update()
    End Sub
#End Region

End Class