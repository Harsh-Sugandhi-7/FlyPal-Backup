Public Class wfServiceInspectionName
    Inherits System.Web.UI.Page
#Region " Variable Declaration "
    Public mServiceInspectionName As ServiceInspectionName
    Public mServiceInspectionNameList As ServiceInspectionNameList
    Dim Index, Text As String
    Public EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Try
            Dim str As String
            str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
        Catch ex As Exception
            '
        End Try
    End Sub
    Private Sub GetSession()
        mServiceInspectionName = Session("mServiceInspectionName")
        mServiceInspectionNameList = Session("mServiceInspectionNameList")
        Index = Session("Index")
        Text = Session("Text")
    End Sub
    Private Sub SetSession()
        Session("mServiceInspectionName") = mServiceInspectionName
        Session("mServiceInspectionNameList") = mServiceInspectionNameList
        Session("Text") = Text
    End Sub
    Private Sub RemoveSession()
        Session.Remove("Index")
        Session.Remove("Text")
        Session.Remove("mServiceInspectionName")
        Session.Remove("mServiceInspectionNameList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfServiceInspectionName.aspx" Then
            Session.Remove("mServiceInspectionName")
            Session.Remove("mServiceInspectionNameList")
            Session.Remove("Text")
            Session.Remove("Index")
        End If
    End Sub
    Private Sub Setpage()
        txtserviceInspection.Text = ""
    End Sub
    Private Sub clearControls()
        txtserviceInspection.Text = ""

    End Sub
    Private Sub NewRecord()
        mServiceInspectionName = ServiceInspectionName.NewServiceInspectionName()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtserviceInspection" Then
            If txtserviceInspection.Text = "" Then
                custValidator.ErrorMessage = "Service Inspection Name Required."
                e.IsValid = False
            End If
        End If

    End Sub

    Private Sub EditRecord(ByVal mId As Guid)
        mServiceInspectionName = ServiceInspectionName.GetServiceInspectionName(mId)
        Session("mServiceInspectionName") = mServiceInspectionName
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mServiceInspectionName = ServiceInspectionName.GetServiceInspectionName(mId)
        Session("mServiceInspectionName") = mServiceInspectionName
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult

        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mServiceInspectionName As ServiceInspectionName
                            Session("Sender") = ""
                            mServiceInspectionName = CType(Session("mServiceInspectionName"), ServiceInspectionName)
                            mServiceInspectionName.Delete()
                            mServiceInspectionName.Save()
                            DataFieldBind()
                            clearControls()
                            upnlGrid.Update()
                            upnlGridViewTitle.Update()
                            upnlServiceInspectionNameDetails.Update()
                            MarkLog(Util.Action.Delete, "Service Inspection", mServiceInspectionName.ServiceInspectionName, Util.ErrorType.NoError, mServiceInspectionName.ID, EventLogID)
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Message.Contains("tabItemServiceInspections") Then
                                stringInfo = "Item Service Inspections."
                            End If

                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Util.Action.Delete, "Currency", "Can't delete :" & mServiceInspectionName.ServiceInspectionName & " is Currently in use", Util.ErrorType.NoError, mServiceInspectionName.ID, EventLogID)
                                'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, stringInfo, MsgBoxStyle.OkOnly, "")
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = ""
                    Session("sender") = ""
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub

    'Private Sub FindNow(Optional ByVal ServiceInspectionNameName As String = "")

    '    mServiceInspectionNameList = ServiceInspectionNameList.GetServiceInspectionList(ServiceInspectionNameName, "")

    '    dgServiceInspectionNameList.DataSource = mServiceInspectionNameList
    '    Session("mServiceInspectionNameList") = mServiceInspectionNameList
    '    dgServiceInspectionNameList.DataBind()
    '    lblResult.Text = "List of Service Inspection Name as per criteria :" + CType(mServiceInspectionNameList.Count, String) + " Record(s) found."
    '    upnlGrid.Update()
    '    upnlGridViewTitle.Update()
    'End Sub
    '================================================
    'Private Sub SetControl()
    '    txtserviceInspection.Text = Text
    '    dgServiceInspectionNameList.DataBind()

    'End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mServiceInspectionNameList = ServiceInspectionNameList.GetServiceInspectionList("")
        dgServiceInspectionNameList.DataSource = mServiceInspectionNameList

        Index = IIf(IsNothing(Index), 0, Index)
        Session("Index") = Index

        Session("mServiceInspectionNameList") = mServiceInspectionNameList
        DataBind()
        lblResult.Text = "List of Service Inspection Name as per criteria :" + CType(mServiceInspectionNameList.Count, String) + " Record(s) found."
        upnlGrid.Update()
        upnlGridViewTitle.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            Setpage()
        End If
    End Sub
    Private Sub dgServiceInspectionNameList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgServiceInspectionNameList.PageIndexChanging
        dgServiceInspectionNameList.PageIndex = e.NewPageIndex
        dgServiceInspectionNameList.DataSource = mServiceInspectionNameList
        Session("mServiceInspectionNameList") = mServiceInspectionNameList
        dgServiceInspectionNameList.DataBind()
    End Sub
    Private Sub dgServiceInspectionNameList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgServiceInspectionNameList.RowCommand
        Dim idx As Int32
        Dim mId As New Guid
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("PartView") And Not User.IsInRole("PartEdit")) Then
                    setObject()
                    SetSession()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                idx = CInt(e.CommandArgument) + dgServiceInspectionNameList.PageIndex * dgServiceInspectionNameList.PageSize
                Session("EditItem") = True
                mId = mServiceInspectionNameList(idx).ID
                Dim mName As String = mServiceInspectionNameList(idx).ServiceInspectionName
                EditRecord(mId)

                txtserviceInspection.Text = mServiceInspectionName.ServiceInspectionName
                upnlServiceInspectionNameDetails.Update()
            Case "DeleteRec"
                If (Not User.IsInRole("PartDelete")) Then
                    setObject()
                    SetSession()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                idx = CInt(e.CommandArgument) + dgServiceInspectionNameList.PageIndex * dgServiceInspectionNameList.PageSize
                mId = mServiceInspectionNameList(idx).ID
                DeleteRecord(mId)

        End Select
    End Sub
    Private Function setObject() As Boolean
        mServiceInspectionName.ApplyEdit()
        mServiceInspectionName.ServiceInspectionName = txtserviceInspection.Text
        mServiceInspectionName.ApplyEdit()
        Return True
    End Function
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click ', btnAddNewTop.Click
        If IsValid Then
            If Session("EditItem") = True Then
                If Not mServiceInspectionNameList.Contains(mServiceInspectionName.ID, mServiceInspectionName.ServiceInspectionName) Then
                    NewRecord()
                End If
            Else
                NewRecord()
            End If

            mServiceInspectionName.ServiceInspectionName = txtserviceInspection.Text
            Session("mServiceInspectionName") = mServiceInspectionName
            SetSession()
            If (Not User.IsInRole("PartNew") And mServiceInspectionName.IsNew) Or (Not User.IsInRole("PartEdit") And Not mServiceInspectionName.IsNew) Then
                SetSession()
                MarkLog(Util.Action.[New], "Inspection ServiceInspectionName", User.Identity.Name & " is not Authorized User to add " & mServiceInspectionName.ServiceInspectionName, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                Exit Sub
            End If
            Dim str As String
            MarkLog(Util.Action.[New], "Service Inspection Name", "", Util.ErrorType.NoError, mServiceInspectionName.ID, EventLogID)


            If mServiceInspectionNameList.Contains(mServiceInspectionName.ServiceInspectionName) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Service Inspection Name", MsgBoxStyle.Information, "")
                Exit Sub
            End If

            If IsValid Then
                mServiceInspectionName.Save()
                DataFieldBind()
                clearControls()
                upnlServiceInspectionNameDetails.Update()
            Else
                upnlValidationSummary.Update()
            End If
            Session.Remove("EditItem")

        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub dgServiceInspectionNameList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgServiceInspectionNameList.Sorting
        mServiceInspectionNameList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mServiceInspectionNameList") = mServiceInspectionNameList
        dgServiceInspectionNameList.DataSource = mServiceInspectionNameList
        dgServiceInspectionNameList.DataBind()
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
End Class