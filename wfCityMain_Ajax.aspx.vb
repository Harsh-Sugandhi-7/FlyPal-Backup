'Created by Prashant 

Public Class wfCityMain_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCityMain As City
    Public mCityListMain As CityList
    Public Type As Int32 = 0
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        Type = CType(Session("Type"), Int32)
        mCityMain = CType(Session("mCityMain"), City)
        mCityListMain = CType(Session("mCityListMain"), CityList)
    End Sub
    Private Sub SetSession()
        Session("mCityMain") = mCityMain
        Session("mCityListMain") = mCityListMain
        Session("Type") = Type
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfCityMain_Ajax.aspx?" And Session("Type") <> "1" Then
            Session.Remove("mCityMain")
            Session.Remove("mCityListMain")
            Session.Remove("Type")
        End If
    End Sub
    Private Sub NewRecord()
        mCityMain = City.NewCity(Guid.NewGuid)
        cmbGMT.SelectedIndex = 0
        txtName.Text = ""
        txtName.Enabled = True
        Session("mCityMain") = mCityMain
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mCityMain = City.GetCity(mId)
        Session("mCityMain") = mCityMain
        setFocus(txtName)
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        GridBind()
        mCityMain = City.GetCity(mId)
        Session("mCityMain") = mCityMain
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        upnlTitle.Update()
    End Sub
    Private Sub setObject()
        mCityMain.Name = Trim(txtName.Text)
        mCityMain.GMT = cmbGMT.SelectedValue
        ''mCityMain.IsDayLight = chkIsDayLight.Checked
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim errcnt As Integer = 0
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mCityMain = Session("mCityMain")
                            City.DeleteCity(mCityMain.ID)
                            DataFieldBind()
                            SetGrid()
                            GridBind()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            Else
                                MSGBoxCtrl.show("Alert!", ex.Message, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                            errcnt = ex.Errors.Count
                        Finally
                            If errcnt = 0 Then
                                MarkLog(Util.Action.Delete, "City", mCityMain.Name, Util.ErrorType.NoError, mCityMain.ID, EventLogID)
                            End If
                            NewRecord()
                            upnlCityDetails.DataBind()
                            SetGrid()
                            upnlCityDetails.Update()
                            SetTitle()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                        SetGrid()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                       NewRecord()
                        SetTitle()
                        SetGrid()
                        upnlCityDetails.DataBind()
                        upnlCityDetails.Update()
                        upnlGrid.Update()
                        'DataFieldBind()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
                    SetGrid()
            End Select
        End If
    End Sub
    Private Sub SetTitle()
        If Not mCityMain.IsNew Then
            If Len(mCityMain.Name) > 15 Then
                lbltitle.Text = "City [" & mCityMain.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "City [" & mCityMain.Name & "]"
            End If
        Else
            lbltitle.Text = "City [New]"
        End If
        lblResult.Text = "As per criteria " & mCityListMain.Count & " Record(s) Found."
        upnlTitle.Update()
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerCityMain(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub GridBind()
        dgCityList.DataSource = mCityListMain
        dgCityList.DataBind()
        SetGrid()
        upnlGrid.Update()
    End Sub
    Private Sub DataFieldBind()
        mCityListMain = CityList.GetCityList("", "")
        dgCityList.DataSource = mCityListMain
        Session("mCityListMain") = mCityListMain
        DataBind()
        lblResult.Text = "As per criteria " & mCityListMain.Count & " Record(s) Found."
    End Sub
    Private Sub SetGrid()
        Dim IsSyncFromCRS As Boolean
        For j As Integer = 0 To dgCityList.Rows.Count - 1
            IsSyncFromCRS = CType(Me.dgCityList.Rows(j).Cells(4).Text, Boolean)

            If IsSyncFromCRS = True Then
                dgCityList.Rows(j).Cells(3).Enabled = False
                'dgCityList.Rows(j).Cells(3).Enabled = False
                'dgCityList.Rows(j).Cells(4).Enabled = False

            End If
        Next
    End Sub
    'Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "cmbGMT" Then
    '        If cmbGMT.SelectedIndex <= 0 Then
    '            e.IsValid = False
    '        End If
    '    End If
    'End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            Dim mopenas As String = Request.QueryString("Typepup")
            Type = CType(Val(Request.QueryString("Type")), Int32)
            Session("Type") = Type
            If Type <> 1 Then Session("MiddleFrame") = "wfCityMain_Ajax.aspx?"
            NewRecord()
            DataFieldBind()
            SetGrid()
        End If
        'MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "City", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Session.Remove("mCityListMain")


        Dim mopenas As String = Request.QueryString("Typepup")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        If Type = 1 Then
            Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
        Else
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("CityNew") And mCityMain.IsNew) Or (Not User.IsInRole("CityEdit") And Not mCityMain.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Page.Validate("a")
        If Not IsValid Then
            'GridBind()
            upnlTitle.Update()
            Exit Sub
        End If
        Try
            setObject()
            mCityMain.Save()
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            MarkLog(Util.Action.Save, "City", mCityMain.Name, Util.ErrorType.HandledError, mCityMain.ID, EventLogID)
            NewRecord()
            txtName.DataBind()
            cmbGMT.DataBind()
            '' chkIsDayLight.DataBind()
            DataFieldBind()
            GridBind()
            SetSession()
            SetTitle()
            SetGrid()
            upnlCityDetails.DataBind()
            upnlCityDetails.Update()
        Catch ex As SqlException
            GridBind()
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End Try
    End Sub
    'Private Sub dgCityList_ItemCommand1(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCityList.ItemCommand
    '    If e.Item.Cells(0).Text = "ID" Or e.Item.Cells(0).Text = "" Then Exit Sub
    '    Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
    '    Dim mName As String = CStr(e.Item.Cells(1).Text)

    '    Select Case e.CommandName
    '        Case "View"
    '            If (Not User.IsInRole("CityView") And Not User.IsInRole("CityEdit")) Then
    '                setObject()
    '                SetSession()
    '                MarkLog(Util.Action.Edit, "City", User.Identity.Name & " is not Authorized User to View " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
    '                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '                msg.ReplacePage = "wfCityMain_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Type
    '                Session("sender") = "Authorization"
    '                msg.Show()
    '                Exit Sub
    '            End If
    '            EditRecord(mId)
    '            txtName.DataBind()
    '            cmbGMT.SelectedValue = mCityMain.GMT
    '            cmbGMT.DataBind()
    '            chkIsDayLight.Checked = mCityMain.IsDayLight
    '            chkIsDayLight.DataBind()
    '            MarkLog(Util.Action.Edit, "City", mCityMain.Name, Util.ErrorType.NoError, mCityMain.ID, EventLogID)
    '            SetTitle()
    '        Case "Delete"
    '            If (Not User.IsInRole("CityDelete")) Then
    '                setObject()
    '                SetSession()
    '                MarkLog(Util.Action.Delete, "City", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
    '                Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '                msg.ReplacePage = "wfCityMain_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&Type=" & Type
    '                Session("sender") = "Authorization"
    '                msg.Show()
    '                Exit Sub
    '            End If
    '            DeleteRecord(mId)
    '    End Select
    'End Sub
    Private Sub dgCityList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCityList.PageIndexChanging
        dgCityList.PageIndex = e.NewPageIndex
        dgCityList.DataSource = mCityListMain
        Session("mCityListMain") = mCityListMain
        GridBind()
    End Sub
    Private Sub dgCityList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCityList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                If (Not User.IsInRole("CityView") And Not User.IsInRole("CityEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgCityList.PageIndex * dgCityList.PageSize
                Dim mID As Guid = mCityListMain(index).ID
                Dim mName As String = mCityListMain(index).Name
                EditRecord(mID)
                cmbGMT.SelectedValue = mCityMain.GMT
                upnlCityDetails.DataBind()
                upnlCityDetails.Update()
                GridBind()
                DisableName(mID) 'Added by : Shital 19-Jun-2020, ALL16062020
                MarkLog(Util.Action.Edit, "City", mCityMain.Name, Util.ErrorType.NoError, mCityMain.ID, EventLogID)
                SetTitle()
                SetGrid()
            Case "Remove"
                If (Not User.IsInRole("CityDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgCityList.PageIndex * dgCityList.PageSize
                Dim mID As Guid = mCityListMain(index).ID
                upnlTitle.Update()
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        'upnlGrid.Update()
        mCityListMain = CityList.GetCityList(Trim(txtSearch.Text))
        Session("mCityListMain") = mCityListMain
        GridBind()
        lblResult.Text = "As per criteria " & mCityListMain.Count & " Record(s) Found."
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        NewRecord()
        'txtName.Text = ""
        'cmbGMT.SelectedIndex = 0
        upnlCityDetails.DataBind()
        MarkLog(Util.Action.[New], "City", "", Util.ErrorType.NoError, mCityMain.ID, EventLogID)
        SetTitle()
        upnlCityDetails.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        'MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class
