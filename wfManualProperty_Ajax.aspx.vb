Public Class wfManualProperty_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mManualProperty As ManualProperty
    Public mManualPropertyList As ManualPropertyList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mManualProperty = CType(Session("mManualProperty"), ManualProperty)
        mManualPropertyList = CType(Session("mManualPropertyList"), ManualPropertyList)
    End Sub
    Private Sub SetSession()
        Session("mManualProperty") = mManualProperty
        Session("mManualPropertyList") = mManualPropertyList
    End Sub
    Private Sub NewRecord()
        mManualProperty = ManualProperty.NewManualProperty()
        Session("mManualProperty") = mManualProperty
        txtName.Text = ""
        lblTitle.Text = "Manual Property [New]"
        upnlValidationSummary.Update()
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mManualProperty = ManualProperty.GetManualProperty(mId)
        Session("mManualProperty") = mManualProperty
        If Len(mManualProperty.Name) > 15 Then
            lblTitle.Text = "Manual Property [" & mManualProperty.Name.Substring(0, 15) & "...]"
        Else
            lblTitle.Text = "Manual Property [" & mManualProperty.Name & "]"
        End If
        upnlValidationSummary.Update()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MCategoryGridBind()
        mManualProperty = ManualProperty.GetManualProperty(mId)
        Session("mManualProperty") = mManualProperty
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub SetObject()
        mManualProperty.Name = Trim(txtName.Text)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            ManualProperty.DeleteManualProperty(mManualProperty.ID)
                            NewRecord()
                            DataFieldBind()
                            upnlManualPropertyDetails.Update()
                        Catch ex As Exception
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                            NewRecord()
                        Finally
                            MarkLog(Util.Action.Delete, "ManualProperty", mManualProperty.Name, Util.ErrorType.NoError, mManualProperty.ID, EventLogID)
                            NewRecord()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        txtName.Text = ""
                        NewRecord()
                        DataFieldBind()
                        upnlManualPropertyDetails.Update()
                    End If
                    MCategoryGridBind()
                Case MsgBoxResult.Ok
                    MCategoryGridBind()
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mManualPropertyList = ManualPropertyList.GetManualPropertyList()
        dgManualPropertyList.DataSource = mManualPropertyList
        Session("mManualPropertyList") = mManualPropertyList
        txtName.DataBind()
        upnlManualPropertyDetails.Update()
        MCategoryGridBind()
    End Sub
    Private Sub MCategoryGridBind()
        dgManualPropertyList.DataSource = mManualPropertyList
        dgManualPropertyList.DataBind()
        lblResult.Text = "Manual Property List: " & mManualPropertyList.Count & " Record(s) Found."
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        txtName.Focus()
        If Not IsPostBack Then
            NewRecord()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If IsValid Then
                SetObject()
                mManualProperty.Save()
                MarkLog(Util.Action.Save, "ManualProperty", mManualProperty.Name, Util.ErrorType.NoError, mManualProperty.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
            Else
               upnlValidationSummary.Update()
            End If
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add duplicate entry in Category.", MsgBoxStyle.OkOnly, "")
            DataFieldBind()
            Exit Sub
        End Try
    End Sub
    Private Sub dgManualPropertyList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManualPropertyList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                'Dim index As Integer = CInt(e.CommandArgument) + dgManualPropertyList.PageIndex * dgManualPropertyList.PageSize
                'Dim mID As Guid = mManualPropertyList(index).ID
                'Dim mName As String = mManualProperty.Name
                Dim mId As Guid = mManualPropertyList(CInt(e.CommandArgument)).ID
                EditRecord(mID)
                txtName.Focus()
                txtName.Text = mManualProperty.Name
                txtName.DataBind()
                upnlManualPropertyDetails.Update()
                MCategoryGridBind()
                MarkLog(Util.Action.Edit, "ManualProperty", mManualProperty.Name, Util.ErrorType.NoError, mManualProperty.ID, EventLogID)
            Case "Remove"
                'Dim index As Integer = CInt(e.CommandArgument) + dgManualPropertyList.PageIndex * dgManualPropertyList.PageSize
                'Dim mID As Guid = mManualPropertyList(index).ID
                Dim mId As Guid = mManualPropertyList(CInt(e.CommandArgument)).ID
                Dim mName As String = mManualProperty.Name
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub dgCompanyList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgManualPropertyList.PageIndexChanging
        dgManualPropertyList.PageIndex = e.NewPageIndex
        dgManualPropertyList.DataSource = mManualPropertyList
        dgManualPropertyList.DataBind()
        Session("mManualPropertyList") = mManualPropertyList
        upnlGridView.Update()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Util.Action.[New], "ManualProperty", "", Util.ErrorType.NoError, mManualProperty.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        txtName.Focus()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "ManualProperty", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        MCategoryGridBind()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class