
Imports System.Collections.Generic
Imports Flypal.ModelListAutoComplete
Imports System.Linq
Imports System.Text

Public Class wfDDMeetingMinutes_Ajax
    Inherits System.Web.UI.Page


#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
    Private Enum OpenImportUtilityFrom
        CompMaster = 1
        Project = 2
    End Enum
#End Region

#Region " Variables and Declarations "
    Dim EventLogID As Guid
    Protected mMeeting As Meeting
    Dim DuplicateEntryMessage As String = String.Empty

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMeeting = Session("mMeeting")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMeeting")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "MeetingMinutes"
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
        End Select
    End Function
    Private Sub setObject()

        mMeeting.Title = Trim(txtTitle.Text)
        If txtMeetingDate.Text.Trim = String.Empty Then
            mMeeting.IDate = System.DBNull.Value
        Else
            mMeeting.IDate = txtMeetingDate.Text.Trim
        End If
        mMeeting.InfoToShow = chkToShow.Checked
        mMeeting = Session("mMeeting")
    End Sub
    Private Sub setObjectMeetingAgenda()
        Dim mMeetingClone As Meeting
        mMeetingClone = mMeeting.Clone
        Try
            Dim child As MeetingAgenda
            Dim txt As TextBox
            Dim txtPart As TextBox
            Dim ID As Guid
            For i As Integer = 0 To dgMeetingAgenda.Rows.Count - 1
                ID = New Guid(dgMeetingAgenda.DataKeys(i).Values("ID").ToString)
                Dim mFetchItemByName As FetchItemByName

                child = mMeeting.MeetingAgendas.Item(ID)

                txt = dgMeetingAgenda.Rows(i).FindControl("txtAgendaDetails")
                child.AgendaDetails = Trim(txt.Text)
            Next
        Catch ex As Exception
            mMeeting = mMeetingClone
            Session("mMeeting") = mMeeting
            mMeetingClone = Nothing
            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.GetBaseException.ToString, MsgBoxStyle.OkOnly, "")
        Finally
            Session("mMeeting") = mMeeting
        End Try
    End Sub
    Private Function Save(Optional ByVal CreatNewRecordAfterSave As Boolean = False, Optional ByVal ClosePageAfterSave As Boolean = False) As Boolean
        Try
            mMeeting.ApplyEdit()
            mMeeting.Save()
            MarkLog(Util.Action.Save, "Meeting", "Meeting : " + txtTitle.Text.ToString, Util.ErrorType.NoError, mMeeting.ID, EventLogID)

            Return True
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
                'ElseIf ex.Number = 2601 Then
                '    If InStr(ex.Message, "UKtabMeetingAgenda", CompareMethod.Text) Then
                '        DuplicateEntryMessage = ex.Message.Substring(ex.Message.IndexOf("SerialNo.:"))
                '        MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + DuplicateEntryMessage, MsgBoxStyle.OkOnly, "")
                '    End If
                '    'Return False
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "UKtabMeetingAgenda", CompareMethod.Text) Then
                DuplicateEntryMessage = ex1.Message.Substring(ex1.Message.IndexOf("SerialNo.:"))
                MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + DuplicateEntryMessage, MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then

                        If CustomValidatePart() = False Then upnlValidations.Update() : Exit Sub
                        If CustomValidate2() = False Then upnlValidations.Update() : Exit Sub
                        If Save() Then
                            SetPage()

                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Save" Then
                        RemoveSession()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If

                        Response.Redirect("index.aspx")
                    End If

                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
    Private Sub NewRecord()
        mMeeting = Meeting.NewMeeting()
        Session("mMeeting") = mMeeting
        DataFieldBind()
        SetPage()
        'txtTitle.Text = ""
        'txtMeetingDate.Text = ""
        upnlTitle.Update()
        upnlMROCompDetails.Update()
        upnlMeetingAgendaDetails.Update()
    End Sub
    Public Function CustomValidate2() As Boolean

        Dim strMsg As String = ""
        setObject()
        '  setObjectMeetingAgenda()
        If Not mMeeting.IsValid Then
            For i As Integer = 0 To mMeeting.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMeeting.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        If strMsg.Trim <> "" Then
            CustomValidator1.ErrorMessage = strMsg
            CustomValidator1.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()
        '  setObjectMeetingAgenda()
        If Not mMeeting.IsValid Then
            For i As Integer = 0 To mMeeting.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMeeting.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        'Dim mMeetingAgenda As MeetingAgenda
        'If Not mMeeting.MROMeetingAgenda.IsValid Then
        '    For Each mMeetingAgenda In mMeeting.MROMeetingAgenda
        '        For i As Integer = 0 To mMeetingAgenda.GetBrokenRulesCollection.Count - 1
        '            strMsg = strMsg + mMeetingAgenda.MROPartName + " : " + mMeetingAgenda.GetBrokenRulesCollection(i).Description + "<Br>"
        '        Next
        '    Next
        'End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
        e.IsValid = True
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)

    End Sub
    Private Function CustomValidatePart() As Boolean
        Dim strError As String = String.Empty
        Dim strSerialError As String = String.Empty
        Dim builder = New StringBuilder()
        Dim txtAgendaDetails As TextBox
        Dim cvValidator As RequiredFieldValidator
        Dim upnlAgendaDetailsValidate As UpdatePanel


        For j As Integer = 0 To dgMeetingAgenda.Rows.Count - 1
            cvValidator = CType(Me.dgMeetingAgenda.Rows(j).FindControl("rfvPart"), RequiredFieldValidator)

            upnlAgendaDetailsValidate = CType(Me.dgMeetingAgenda.Rows(j).FindControl("upnlAgendaDetailsValidate"), UpdatePanel)
            txtAgendaDetails = CType(Me.dgMeetingAgenda.Rows(j).FindControl("txtAgendaDetails"), TextBox)

            If txtAgendaDetails.Text = "" Then
                cvValidator.IsValid = False
                cvValidator.Text = "* Agenda Details Required"
                strError = "* Agenda Details Required"
                upnlAgendaDetailsValidate.Update()
            End If
        Next
        If strError <> "" Then
            Return False
        End If
        Return True
    End Function
    Private Sub addAttributes()

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        dgMeetingAgenda.DataSource = mMeeting.MeetingAgendas

        DataBind()
        txtMeetingDate.Text = mMeeting.IDateFormatted.ToString
    End Sub
    Private Sub SetPage()
        If mMeeting.IsNew = True Then
            lblTitle.Text = "Meeting [ NEW ]"
        Else
            lblTitle.Text = "Meeting For " + "[" + mMeeting.Title + "]"
        End If
        upnlTitle.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtTitle.Focus()
            DataFieldBind()
            SetPage()


        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If CustomValidatePart() = False Then upnlValidations.Update() : Exit Sub

        If IsValid Then
            If (Not IsInRole(Rights.[New]) And mMeeting.IsNew) Or (Not IsInRole(Rights.Edit) And Not mMeeting.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If mMeeting.MeetingAgendas.Count = 0 Then
                MSGBoxCtrl.show("Alert", "Meeting Agenda required", "Please add at least one Agenda and then click on Save", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            setObject()
            setObjectMeetingAgenda()

            If mMeeting.IsDirty Then
                If Save() Then
                    SetPage()
                End If
            End If
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click


        setObject()
        setObjectMeetingAgenda()
        If mMeeting.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Save")
            Exit Sub
        Else
            RemoveSession()
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If

            Response.Redirect("index.aspx")

        End If


    End Sub
    Private Sub btnAddMeetingAgenda_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddMeetingAgenda.Click
        If CustomValidatePart() = False Then upnlValidations.Update() : Exit Sub


        ' If IsValid Then
        setObjectMeetingAgenda()
        mMeeting.MeetingAgendas.Add(mMeeting.ID)
        dgMeetingAgenda.DataSource = mMeeting.MeetingAgendas
        dgMeetingAgenda.DataBind()
        upnlMeetingAgendaDetails.Update()
        'Else
        'upnlValidations.Update()
        'End If
    End Sub
    Private Sub dgMeetingAgenda_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMeetingAgenda.RowCommand
        Select Case e.CommandName
            Case "Del"
                setObjectMeetingAgenda()
                mMeeting.MeetingAgendas.Remove(CInt(e.CommandArgument) - 1)
                dgMeetingAgenda.DataSource = mMeeting.MeetingAgendas
                dgMeetingAgenda.DataBind()
                Session("mMeeting") = mMeeting
        End Select
    End Sub
    Private Sub dgMeetingAgenda_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMeetingAgenda.Sorting
        mMeeting.MeetingAgendas.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMeeting") = mMeeting
        dgMeetingAgenda.DataSource = mMeeting.MeetingAgendas
        dgMeetingAgenda.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnSaveNew_Click(sender As Object, e As System.EventArgs) Handles btnSaveNew.Click
        If CustomValidatePart() = False Then upnlValidations.Update() : Exit Sub

        If IsValid Then
            If (Not IsInRole(Rights.[New]) And mMeeting.IsNew) Or (Not IsInRole(Rights.Edit) And Not mMeeting.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            setObject()
            setObjectMeetingAgenda()

            If mMeeting.IsDirty Then
                If Save() Then
                    NewRecord()
                End If
            Else
                NewRecord()
            End If
        Else
            upnlValidations.Update()
        End If
    End Sub

#End Region
End Class