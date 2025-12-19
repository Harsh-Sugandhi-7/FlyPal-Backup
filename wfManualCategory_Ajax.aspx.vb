Public Class wfManualCategory_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMCategory As MCategory
    Public mMCategoryList As MCategoryList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMCategory = CType(Session("mMCategory"), MCategory)
        mMCategoryList = CType(Session("mMCategoryList"), MCategoryList)
    End Sub
    Private Sub SetSession()
        Session("mMCategory") = mMCategory
        Session("mMCategoryList") = mMCategoryList
    End Sub
    Private Sub NewRecord()
        mMCategory = MCategory.NewMCategory()
        Session("mMCategory") = mMCategory
        'txtName.Text = ""
        lblTitle.Text = "MCategory [New]"
        upnlValidationSummary.Update()
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mMCategory = MCategory.GetMCategory(mId)
        Session("mMCategory") = mMCategory
        If Len(mMCategory.Name) > 15 Then
            lbltitle.Text = "MCategory [" & mMCategory.Name.Substring(0, 15) & "...]"
        Else
            lbltitle.Text = "MCategory [" & mMCategory.Name & "]"
        End If
        upnlValidationSummary.Update()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MCategoryGridBind()
        mMCategory = MCategory.GetMCategory(mId)
        Session("mMCategory") = mMCategory
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub SetObject()
        mMCategory.Name = Trim(txtName.Text)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            MCategory.DeleteMCategory(mMCategory.ID)
                            NewRecord()
                            DataFieldBind()
                            upnlCategoryDetails.Update()
                        Catch ex As Exception
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                        Finally
                            MarkLog(Util.Action.Delete, "MCategory", mMCategory.Name, Util.ErrorType.NoError, mMCategory.ID, EventLogID)
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
                        upnlCategoryDetails.Update()
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
        mMCategoryList = MCategoryList.GetMCategoryList()
        dgCategoryList.DataSource = mMCategoryList
        Session("mMCategoryList") = mMCategoryList
        txtName.DataBind()
        upnlCategoryDetails.Update()
        MCategoryGridBind()
    End Sub
    Private Sub MCategoryGridBind()
        dgCategoryList.DataSource = mMCategoryList
        dgCategoryList.DataBind()
        lblResult.Text = "MCategory List: " & mMCategoryList.Count & " Record(s) Found."
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
            SetObject()
            If mMCategory.IsSavable Then
                mMCategory.Save()
                MarkLog(Util.Action.Save, "MCategory", mMCategory.Name, Util.ErrorType.NoError, mMCategory.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
                txtName.Text = ""
            Else
                cvControlValidator.ErrorMessage = mMCategory.GetBrokenRulesString
                cvControlValidator.IsValid = mMCategory.IsSavable
                mMCategory.Name = ""
                'mMCategory = MCategory.NewMCategory()
                'Session("mMCategory") = mMCategory
                NewRecord()
                txtName.Text = ""
                DataFieldBind()
            End If
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "You can not add duplicate entry in Category.", MsgBoxStyle.OkOnly, "")
            DataFieldBind()
            Exit Sub
        End Try
    End Sub
    Private Sub dgCompanyList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCategoryList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                ' Dim index As Integer = CInt(e.CommandArgument) + dgCategoryList.PageIndex * dgCategoryList.PageSize
                'Dim mID As Guid = mMCategoryList(index).ID
                'Dim mName As String = mMCategoryList(index).Name
                Dim mId As Guid = mMCategoryList(CInt(e.CommandArgument)).ID
                'Dim mName As String = mMCategoryList(mId).Name
                EditRecord(mId)
                'EditRecord(mID)
                txtName.Focus()
                txtName.Text = mMCategory.Name
                txtName.DataBind()
                upnlCategoryDetails.Update()
                MCategoryGridBind()





                MarkLog(Util.Action.Edit, "MCategory", mMCategory.Name, Util.ErrorType.NoError, mMCategory.ID, EventLogID)
            Case "Remove"
                'Dim index As Integer = CInt(e.CommandArgument) + dgCategoryList.PageIndex * dgCategoryList.PageSize
                'Dim mID As Guid = mMCategoryList(index).ID
                'Dim mName As String = mMCategoryList(index).Name
                'DeleteRecord(mID)

                Dim mId As Guid = mMCategoryList(CInt(e.CommandArgument)).ID
                DeleteRecord(mID)

        End Select
    End Sub
    Private Sub dgCompanyList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCategoryList.PageIndexChanging
        dgCategoryList.PageIndex = e.NewPageIndex
        dgCategoryList.DataSource = mMCategoryList
        dgCategoryList.DataBind()
        Session("mMCategoryList") = mMCategoryList
        upnlGridView.Update()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Util.Action.[New], "MCategory", "", Util.ErrorType.NoError, mMCategory.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        txtName.Focus()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "MCategory", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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