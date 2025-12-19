

'Created by : Saylee
'Dated      : 1-Dec-2015

Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Public Class wfGroupTrainingRenewal
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTraining As Training
    Public mTrainingTypeList As TrainingTypeList
    Public mTrainingOrgList As TrainingOrgList
    Public mGroupEmployeeTrainingRenewalList As GroupEmployeeTrainingRenewalList
    Dim EventLogID As Guid
    Dim mTrainingID As Guid
    Private checkedIds As New List(Of String)()
    Dim mFileAttach As FileAttach
    Dim var As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTraining = CType(Session("mTraining"), Training)
        mTrainingTypeList = CType(Session("mTrainingTypeList"), TrainingTypeList)
        mTrainingOrgList = CType(Session("mTrainingOrgList"), TrainingOrgList)
        mGroupEmployeeTrainingRenewalList = CType(Session("mGroupEmployeeTrainingRenewalList"), GroupEmployeeTrainingRenewalList)
        mTrainingID = Session("mTrainingID")
    End Sub
    Private Sub SetSession()
        Session("mTraining") = mTraining
        Session("mTrainingTypeList") = mTrainingTypeList
        Session("mGroupEmployeeTrainingRenewalList") = mGroupEmployeeTrainingRenewalList
        Session("mTrainingID") = mTrainingID
        Session("mTrainingOrgList") = mTrainingOrgList
    End Sub
    Private Function SetObject(mEmployeeTraining As EmployeeTraining) As EmployeeTraining
        ''mEmployeeTraining.Date = CType(txtDate.Text, Object)
        If Not IsDate(txtDate.Text) Then
            mEmployeeTraining.Date = System.DBNull.Value
        Else
            mEmployeeTraining.Date = CType(txtDate.Text, Object)
        End If
        mEmployeeTraining.Duration = Val(txtDuration.Text)
        mEmployeeTraining.TrainingOrgID = New Guid(cmbTrainingOrgList.SelectedValue)
        mEmployeeTraining.Remark = Trim(txtRemark.Text)
        mEmployeeTraining.YearOfTraining = Year(CDate(mEmployeeTraining.Date))
        mEmployeeTraining.MonthOfTrainingID = Month(CDate(mEmployeeTraining.Date))

        If CType(Session("Size"), Integer) > 0 Then
            mEmployeeTraining.IsAttachmentAdded = True
        Else
            mEmployeeTraining.IsAttachmentAdded = False
        End If

        Return mEmployeeTraining
    End Function
    Private Sub SaveAttachment(ByVal mEmployeeRenewTraining As EmployeeTraining) '

        If CType(Session("Size"), Integer) > 0 Then
            Try
                mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mEmployeeRenewTraining.ID)
                mFileAttach.Size = Session("Size")
                mFileAttach.Extension = Session("Extension")
                mFileAttach.ImageFile = Session("ImageFile")
                mFileAttach.Save()
                mFileAttach = Nothing
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
            End Try

        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Try
                            Dim builder = New StringBuilder()
                            Dim succes As Boolean = False
                            builder.Append("You have selected the following checks :<br/>")
                            ' get the selected checkboxes from the form data
                            Dim checkString = Request.Form("chkSelect")
                            If checkString Is Nothing Then
                                MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            Else
                                ' we'll need a split to get the individual ids  
                                Try
                                    Dim values = checkString.Split(","c)
                                    Dim mEmployeeTraining As EmployeeTraining
                                    Dim mEmployeeRenewTraining As EmployeeTraining
                                    For Each value As String In values
                                        builder.Append("<br/>")
                                        builder.Append(value)
                                        checkedIds.Add(value)

                                        mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(New Guid(value))
                                        mEmployeeRenewTraining = EmployeeTraining.NewRenew(mEmployeeTraining, mTraining.FreqInMonths, True)
                                        mEmployeeRenewTraining = SetObject(mEmployeeRenewTraining)
                                        mEmployeeRenewTraining.Save()
                                        MarkLog(Flypal.Util.Action.Save, "EmployeeGroupTrainingRenewal", "Emp : " + mEmployeeTraining.EmployeeName + " Training : " + mTraining.Name + "Done At:" + txtDate.Text + " Renew From Group Training", Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                                        SaveAttachment(mEmployeeRenewTraining)
                                        mEmployeeRenewTraining = Nothing
                                        mEmployeeTraining = Nothing
                                      
                                        Session("mFileAttach") = mFileAttach
                                        succes = True

                                    Next
                                    values = ""
                                    checkString = Nothing

                                    If succes = True Then
                                        'Dim mopenas As String = Request.QueryString("Type")
                                        'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                        '    Exit Sub
                                        'End If
                                        Session.Remove("Size")
                                        Session.Remove("Extension")
                                        Session.Remove("ImageFile")
                                        mGroupEmployeeTrainingRenewalList = GroupEmployeeTrainingRenewalList.GetGroupEmployeeTrainingRenewalList(mTrainingID)
                                        dgEmpTrainingList.DataSource = mGroupEmployeeTrainingRenewalList
                                        dgEmpTrainingList.DataBind()
                                        Session("mGroupEmployeeTrainingRenewalList") = mGroupEmployeeTrainingRenewalList
                                        txtDate.Text = ""
                                        cmbTrainingOrgList.SelectedIndex = 0
                                        txtRemark.Text = ""
                                        txtDuration.Text = "0"
                                        ControlVisibilityForAttachment()
                                        SetGrid()
                                        upnlGrid.Update()

                                        upnlRenewalInfo.Update()
                                        upnlFileupload.Update()

                                        If mGroupEmployeeTrainingRenewalList.Count = 0 Then
                                            Session.Remove("Size")
                                            Dim mopenas As String = Request.QueryString("Type")
                                            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                                Exit Sub
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                Finally

                                End Try

                            End If
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                          
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""

                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If CType(Session("Size"), Integer) > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTraining = Training.GetTraining(mTrainingID)
        Session("mTraining") = mTraining

        mGroupEmployeeTrainingRenewalList = GroupEmployeeTrainingRenewalList.GetGroupEmployeeTrainingRenewalList(mTrainingID)
        dgEmpTrainingList.DataSource = mGroupEmployeeTrainingRenewalList
        Session("mGroupEmployeeTrainingRenewalList") = mGroupEmployeeTrainingRenewalList

        mTrainingTypeList = TrainingTypeList.GetTrainingTypeList()
        Session("mTrainingTypeList") = mTrainingTypeList
        cmbTrainingType.DataSource = mTrainingTypeList

        mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, , , "(SELECT)")
        Session("mTrainingOrgList") = mTrainingOrgList
        cmbTrainingOrgList.DataSource = mTrainingOrgList

        DataBind()

        lblSearch.Text = "List of " & mGroupEmployeeTrainingRenewalList.Count & " Employee(s) for Selection"
    End Sub
    Private Sub SetGrid()
        Dim TrainingHistoryCount As Boolean
        Dim IsAttachement As Boolean
        For n As Integer = 0 To dgEmpTrainingList.Rows.Count - 1
            TrainingHistoryCount = CType(Me.dgEmpTrainingList.Rows(n).Cells(14).Text, Boolean)
            IsAttachement = CType(Me.dgEmpTrainingList.Rows(n).Cells(16).Text, Boolean)

            If TrainingHistoryCount = False Then dgEmpTrainingList.Rows(n).Cells(13).Enabled = False

            If IsAttachement = False Then dgEmpTrainingList.Rows(n).Cells(15).Enabled = False
        Next
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibilityForAttachment()
            SetGrid()
            txtDuration.Text = "0"
        End If
        SetSession()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If IsPostBack Then
            If IsValid Then
                ' create a string builder to create the displayed string
                Dim builder = New StringBuilder()
                Dim succes As Boolean = False
                builder.Append("You have selected the following checks :<br/>")
                ' get the selected checkboxes from the form data
                Dim checkString = Request.Form("chkSelect")
                If checkString Is Nothing Then
                    MSGBoxCtrl.show("Selection Alert!", "Select atleast one Employee.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    ' we'll need a split to get the individual ids  
                    Try
                        MSGBoxCtrl.show("Save Alert", "You are about to Renew Training for selected employees. Do you want to continue? ", "", MsgBoxStyle.YesNo, "Save")
                        Exit Sub
                    Catch ex As Exception
                    Finally

                    End Try

                End If
            Else
                upnlValidationSummary.Update()
            End If

        End If

    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
        'SetGrid()
        'upnlGrid.Update()
        upnlFileupload.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If CType(Session("Size"), Integer) > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & CType(Session("Extension"), String)
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & CType(Session("Extension"), String))
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(CType(Session("ImageFile"), Byte()), 0, (CType(Session("ImageFile"), Byte())).Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        Session.Remove("Size")
        ControlVisibilityForAttachment()

    End Sub
    Private Sub dgEmpTrainingList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEmpTrainingList.PageIndexChanging
        dgEmpTrainingList.PageIndex = e.NewPageIndex
        dgEmpTrainingList.DataSource = mGroupEmployeeTrainingRenewalList
        Session("mGroupEmployeeTrainingRenewalList") = mGroupEmployeeTrainingRenewalList
        dgEmpTrainingList.DataBind()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        Session.Remove("mFileAttach")
        Dim checkString = Request.Form("chkSelect")
        Dim values = checkString.Split(","c)
        Dim builder = New StringBuilder()
        For Each value As String In values
            builder.Append("<br/>")
            builder.Append(value)
            checkedIds.Add(value)
        Next
    End Sub
    Private Sub dgEmpTrainingList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmpTrainingList.RowCommand
        Select Case e.CommandName
            Case "History"
                'Dim mID As Guid = mGroupEmployeeTrainingRenewalList(CType(e.CommandArgument.ToString, Integer)).ID

                Dim mID As Guid = mGroupEmployeeTrainingRenewalList(CType(e.CommandArgument.ToString, Integer) + dgEmpTrainingList.PageIndex * dgEmpTrainingList.PageSize).ID

                Dim mEmployeeTraining As EmployeeTraining
                mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
                Dim mEmployeeID As Guid = mGroupEmployeeTrainingRenewalList(CType(e.CommandArgument.ToString, Integer)).EmployeeID
                Session("mEmployeeID") = mEmployeeID.ToString
                Session("mEmployeeTraining") = mEmployeeTraining
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpTrainingHistoryWindow", "OpenEmpTrainingHistoryWindow()", True)
            Case "Attach"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString

                'Dim mID As Guid = mGroupEmployeeTrainingRenewalList(CType(e.CommandArgument.ToString, Integer)).ID

                Dim mID As Guid = mGroupEmployeeTrainingRenewalList(CType(e.CommandArgument.ToString, Integer) + dgEmpTrainingList.PageIndex * dgEmpTrainingList.PageSize).ID
                Dim mFileAttach As FileAttach
                mFileAttach = FileAttach.GetAttachment(Mid)
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
                        Dim Str As String
                        Str = "openFile();"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
                    End If
                End If
        End Select
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Session.Remove("Size")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("index.aspx")

    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region "Checked Selection" 'Added by Saylee on 11-Mar-2014 for ALL11032014
    Public Function NumeroChequeInclus(ByVal numero As String) As String
        If (checkedIds.Contains(numero)) Then
            Return "checked"
        Else
            Return String.Empty
        End If
    End Function
  
#End Region

   
   
    
End Class