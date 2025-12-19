Public Class wfFuelType_Ajax
    Inherits System.Web.UI.Page


#Region "Variable Declaration"
    Public mFuelType As FuelType
    Public mFuelTypeList As FuelTypeList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFuelType = CType(Session("mFuelType"), FuelType)
        mFuelTypeList = CType(Session("mFuelTypeList"), FuelTypeList)
    End Sub
    Private Sub SetSession()
        Session("mFuelType") = mFuelType
        Session("mFuelTypeList") = mFuelTypeList
    End Sub
    Private Sub ClearAll()
        Session.Remove("mFuelType")
        Session.Remove("mFuelTypeList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub NewRecord()
        mFuelType = FuelType.NewFuelType(Guid.NewGuid)
        Session("mFuelType") = mFuelType
    End Sub
    Private Sub setObject()
        mFuelType.Name = Trim(txtFuelType.Text)
    End Sub
    Private Sub SetTitle()
        If Not mFuelType.IsNew Then
            If Len(mFuelType.Name) > 15 Then
                lbltitle.Text = "Fuel Type [" & mFuelType.Name.Substring(0, 15) & "...]"
            End If
        Else
            lbltitle.Text = "Fuel Type [New]"
        End If
        lblResult.Text = "List of Fuel Type(s) : " & mFuelTypeList.Count & " Record(s) Found."
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        mFuelType = FuelType.GetFuelType(ID)
        Session("mFuelType") = mFuelType
        setFocus(txtFuelType)
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfFuelType.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
        'Session("sender") = "Delete"
        'msg1.Show()
        'MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mFuelType = FuelType.GetFuelType(ID)
        Session("mFuelType") = mFuelType
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        'Dim FuelTypeInfo1 As String = ""
        'If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        '    Result1 = -1
        'Else
        '    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        'End If
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mFuelType = CType(Session("mFuelType"), FuelType)
                            FuelType.DeleteFuelType(mFuelType.ID)

                            ' Response.Redirect("wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            'If ex.Number = 8145 Then
                            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                            '    msg1.ReplacePage = "wfFuelType.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                            '    msg1.Show()
                            'ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                            '    msg1.ReplacePage = "wfFuelType.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                            '    msg1.Show()
                            'ElseIf ex.Number = 547 Then
                            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                            '    msg1.ReplacePage = "wfFuelType.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
                            '    FuelTypeInfo1 = "Name : " + mFuelType.Name
                            '    MarkLog(Util.Action.Delete, "FuelTypeMaster", "Can't delete :" & FuelTypeInfo1 & " is Currently in use", Util.ErrorType.HandledError, mFuelType.ID, EventLogID)
                            '    msg1.Show()
                            'End If
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                Dim FuelTypeInfo1 As String
                                FuelTypeInfo1 = "Name : " + mFuelType.Name
                                MarkLog(Util.Action.Delete, "FuelTypeMaster", "Can't delete :" & FuelTypeInfo1 & " is Currently in use", Util.ErrorType.HandledError, mFuelType.ID, EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            NewRecord()
                            DataFieldBind()
                            txtFuelType.Text = ""
                            upnlGrid.Update()
                            upnlDet.Update()
                            If msgCount = 0 Then
                                Dim FuelTypeInfo1 As String
                                FuelTypeInfo1 = "Name : " + mFuelType.Name
                                MarkLog(Util.Action.Delete, "FuelTypeMaster", FuelTypeInfo1, Util.ErrorType.NoError, mFuelType.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    '    Session("sender") = ""
                    '    Response.Redirect("wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    '  Response.Redirect("wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()

            ' Response.Redirect("wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mFuelTypeList = FuelTypeList.GetFuelTypeList("", "")
        dgFuelTypeList1.DataSource = mFuelTypeList
        Session("mFuelTypeList") = mFuelTypeList
        DataBind()
        lblResult.Text = "List of Fuel Type(s) : " & mFuelTypeList.Count & " Record(s) Found."
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtFuelType.Text) > 100 Then
                e.IsValid = False
                custValidator.ErrorMessage = "Fuel Type Too Long"
            End If
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And MSGBoxCtrl.Sender = "" Then
            ClearAll()
            NewRecord()
            DataFieldBind()
        End If
        'MessageBoxResult()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Dim FuelTypeInfo As String = ""
        Try
            setObject()
            mFuelType.Save()
            FuelTypeInfo = "Name : " + mFuelType.Name
            MarkLog(Util.Action.Save, "FuelTypeMaster", FuelTypeInfo, Util.ErrorType.HandledError, mFuelType.ID, EventLogID)
            NewRecord()
            DataFieldBind()
            SetSession()
            SetTitle()
            upnlGrid.Update()
            upnlResult.Update()
            upnlTitle.Update()
            upnlDet.Update()
        Catch ex As SqlException
            'If ex.Number = 8145 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
            '    msg1.ReplacePage = "wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            '    Session("sender") = "Delete"
            '    msg1.Show()
            'ElseIf ex.Number = 2601 Or ex.Number = 2627 Then '2627 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "Fuel Type Master", MsgBoxStyle.OkOnly)
            '    msg1.ReplacePage = "wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            '    Session("sender") = "Delete"
            '    msg1.Show()
            'ElseIf ex.Number = 547 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
            '    msg1.ReplacePage = "wfFuelType.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage")
            '    Session("sender") = "Delete"
            '    msg1.Show()
            'End If
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 50000 Then
                MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly, "Delete")
            End If

        End Try
    End Sub

    Private Sub dgFuelTypeList1_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFuelTypeList1.RowCommand
        'If e.Item.Cells(0).Text = "ID" Or e.Item.Cells(0).Text = "" Then Exit Sub
        Try
            'Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
            'Dim mFuelType As String = CStr(e.Item.Cells(1).Text)
            Dim FuelTypeInfo As String = ""
            Dim Index As Int32
            Dim ID As Guid
            Select Case e.CommandName
                Case "ViewRec"
                    ID = New Guid(e.CommandArgument.ToString)
                    EditRecord(ID)
                    txtFuelType.DataBind()

                    ' FuelTypeInfo = "Name : " + mFuelType
                    MarkLog(Util.Action.Edit, "FuelTypeMaster", FuelTypeInfo, Util.ErrorType.NoError, ID, EventLogID)
                    SetTitle()
                    upnlResult.Update()
                    upnlTitle.Update()
                    upnlDet.Update()
                Case "DeleteRec"

                    Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 25-04-2023
                    Index = gvr.RowIndex
                    ID = mFuelTypeList(Index).ID
                    DeleteRecord(ID)
            End Select
        Catch ex As Exception
            Throw ex.GetBaseException
        Finally
        End Try
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        MarkLog(Util.Action.Close, "FuelTypeMaster", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Session.Remove("mFuelType")


        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

        Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub

    Private Sub btnFindNow1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow1.Click
        Dim Search As String
        If txtsearchfueltype.Text <> "" Then
            Search = txtsearchfueltype.Text
        End If
        mFuelTypeList = FuelTypeList.GetFuelTypeList(txtsearchfueltype.Text)
        Session("mFuelTypeList") = mFuelTypeList
        dgFuelTypeList1.DataSource = mFuelTypeList
        dgFuelTypeList1.DataBind()
        lblResult.Text = "Fuel Type List: " & mFuelTypeList.Count & " Record(s) Found."
        upnlGrid.Update()
        upnlResult.Update()
    End Sub

    Private Sub dgFuelTypeList1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFuelTypeList1.PageIndexChanged
        'dgFuelTypeList1.CurrentPageIndex = e.NewPageIndex
        dgFuelTypeList1.DataSource = mFuelTypeList
        Session("mFuelTypeList") = mFuelTypeList
        dgFuelTypeList1.DataBind()
        upnlGrid.Update()
        upnlDet.Update()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        txtFuelType.Text = ""
        upnlGrid.Update()
        upnlDet.Update()
    End Sub
#End Region

   

   
  
End Class