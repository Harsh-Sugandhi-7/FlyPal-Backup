'Created by Vikrant

Partial Class wfUnitConverter_Ajax
    Inherits Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As Object

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mBaseUnitList As UnitList
    Public mConvertUnitList As UnitList
    Public mUnitConverter As UnitConverter
    Public mUnitListForConverter As UnitListForConverter
    'Added by Vikrant on 22-July-2011
    Dim EventLogID As Guid
    Dim mUnitDetail As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUnitConverter = CType(Session("mUnitConverter"), UnitConverter)
        mUnitListForConverter = CType(Session("mUnitListForConverter"), UnitListForConverter)
        mBaseUnitList = CType(Session("mBaseUnitList"), UnitList)
        mConvertUnitList = CType(Session("mConvertUnitList"), UnitList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUnitConverter")
        Session.Remove("mUnitListForConverter")
        Session.Remove("mBaseUnitList")
        Session.Remove("mConvertUnitList")
    End Sub
    Private Sub SetSession()
        Session("mUnitConverter") = mUnitConverter
        Session("mUnitListForConverter") = mUnitListForConverter
        Session("mBaseUnitList") = mBaseUnitList
        Session("mConvertUnitList") = mConvertUnitList
    End Sub
    Private Sub NewRecord()
        mUnitConverter = UnitConverter.NewUnitConverter(Guid.NewGuid)
        Session("mUnitConverter") = mUnitConverter
    End Sub
    Private Sub EditRecord(mID As Guid)
        mUnitConverter = UnitConverter.GetUnitConverter(mID)
        Session("mUnitConverter") = mUnitConverter
        setFocus(cmbBaseUnit)
    End Sub
    Private Sub DeleteRecord(mID As Guid)
        MSGBoxCntrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mUnitConverter = UnitConverter.GetUnitConverter(mID)
        Session("mUnitConverter") = mUnitConverter
    End Sub
    Private Sub setObject()
        mUnitConverter.PrimaryUnitID = New Guid(cmbBaseUnit.SelectedValue)
        mUnitConverter.ConvertUnitID = New Guid(cmbConvertUnit.SelectedValue)
        mUnitConverter.Factor = CDec(txtFactor.Text.Trim)
    End Sub
    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub setFocusOn()
        If cmbBaseUnit.Enabled = True Then
            setFocus(cmbBaseUnit)
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCntrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCntrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mUnitConverter = CType(Session("mUnitConverter"), UnitConverter)
                            UnitConverter.DeleteUnitConverter(mUnitConverter.ID, mUnitConverter.ConvertUnitID)

                            'Response.Redirect("wfUnitConverter_Ajax.aspx")
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, "", MsgBoxStyle.OKOnly)
                                'msg1.ReplacePage = "wfUnitConverter_Ajax.aspx?"
                                'msg1.Show()
                                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Unit Converter", MsgBoxStyle.OKOnly)
                                'msg1.ReplacePage = "wfUnitConverter_Ajax.aspx?"
                                'msg1.Show()
                                MSGBoxCntrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Unit Converter", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OKOnly)
                                'msg1.ReplacePage = "wfUnitConverter_Ajax.aspx?"
                                'MarkLog(Util.Action.Delete, "Unit Converter", "Can't delete :" & mUnitDetail & " is Currently in use", Util.ErrorType.NoError, mUnitConverter.ID, EventLogID)
                                'msg1.Show()
                                MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 50000 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DeleteAlert, SIMsgBox.Message_text.DeleteAlert, ex.Message, MsgBoxStyle.OKOnly)
                                'msg1.ReplacePage = "wfUnitConverter_Ajax.aspx?"
                                'msg1.Show()
                                MSGBoxCntrl.show(MSGBox.Message_title.DeleteAlert, MSGBox.Message_text.DeleteAlert, ex.Message, MsgBoxStyle.OkOnly, "")
                            End If

                            'upnlGridView.Update()CHK
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mUnitDetail = IIf(IsNothing(Session("mUnitDetail")), String.Empty, Session("mUnitDetail"))
                                MarkLog(Util.Action.Delete, "Unit Converter", mUnitDetail, Util.ErrorType.HandledError, mUnitConverter.ID, EventLogID)
                                Session.Remove("mUnitDetail")
                                NewRecord()
                                DataFieldBind()
                                SetTitle()
                                upnlUnitDetails.Update()
                                upnlGridView.Update()
                            Else
                                NewRecord()
                                DataFieldBind()
                                SetTitle()
                                upnlUnitDetails.Update()
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCntrl.Sender = "Delete" Then
                        Session("sender") = ""
                        NewRecord()
                        BindControls(False)
                        txtFactor.DataBind()
                        upnlUnitDetails.Update()
                    Else
                        Session("sender") = ""
                        BindControls()
                    End If

                    'NewRecord()CHK
                    'DataFieldBind()CHK
                    'SetTitle()CHK
                    'upnlUnitDetails.Update()CHK
                    'upnlGridView.Update()CHK
                    'Response.Redirect("wfUnitConverter_Ajax.aspx")
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    BindControls() 'CHK
                    'NewRecord() CHK
                    'DataFieldBind() CHK
                    'upnlUnitDetails.Update() CHK
                    'upnlGridView.Update() CHK
                    'Response.Redirect("wfUnitConverter_Ajax.aspx")
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
                    BindControls() 'CHK
                    'DataFieldBind() CHK
                    'upnlUnitDetails.Update() CHK
                    'upnlGridView.Update() CHK
                    'Response.Redirect("wfUnitConverter_Ajax.aspx")
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfUnitConverter_Ajax.aspx")
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            'Added by Vikrant on 22-July-2011
            'mUnitDetail = cmbBaseUnit.SelectedItem.ToString
            'MarkLog(Util.Action.Save, "Unit Converter", User.Identity.Name & " is not Authorized User to save " & mUnitDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            Session("sender") = ""
            DataFieldBind()
        End If

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub
    Private Sub addAttributes()
        txtFactor.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtFactor').value,event)")
    End Sub
    Private Sub SetTitle(Optional Index As Integer = 0)
        If mUnitConverter.IsNew Then
            lbltitle.Text = "Unit Converter [New]"
        Else
            lbltitle.Text = "Unit Converter [" & mUnitListForConverter.Item(Index).PrimaryUnitName & "]"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub BindControls(Optional SetSelectedValue As Boolean = True)
        cmbBaseUnit.DataSource = mBaseUnitList
        cmbBaseUnit.DataBind()
        cmbConvertUnit.DataSource = mConvertUnitList
        cmbConvertUnit.DataBind()
        dgUnitConverterList.DataSource = mUnitListForConverter
        dgUnitConverterList.DataBind()
        If SetSelectedValue Then
            If BaseUnitIDValue.Value <> "" Then
                cmbBaseUnit.SelectedValue = BaseUnitIDValue.Value
            End If
            If ConvertUnitIDValue.Value <> "" Then
                cmbConvertUnit.SelectedValue = ConvertUnitIDValue.Value
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mBaseUnitList = UnitList.GetUnitList(True)
        Session("mBaseUnitList") = mBaseUnitList
        cmbBaseUnit.DataSource = mBaseUnitList

        mConvertUnitList = UnitList.GetUnitList(True)
        cmbConvertUnit.DataSource = mConvertUnitList
        Session("mConvertUnitList") = mConvertUnitList

        mUnitListForConverter = UnitListForConverter.GetUnitListForConverter()
        dgUnitConverterList.DataSource = mUnitListForConverter
        Session("mUnitListForConverter") = mUnitListForConverter

        DataBind()
        lblResult.Text = "Unit Converter List : " & mUnitListForConverter.Count & " Record(s) Found."
        upnlGridView.Update()
    End Sub
    'Public Sub customvalidate( s As Object,  e As ServerValidateEventArgs)
    '    Dim CustValidator As CustomValidator
    '    CustValidator = CType(s, CustomValidator)
    '    If CustValidator.ControlToValidate = "cmbBaseUnit" Then
    '        If cmbBaseUnit.SelectedIndex = 0 Then
    '            CustValidator.ErrorMessage = "Select Base Unit"
    '            e.IsValid = False
    '        Else
    '            e.IsValid = True
    '        End If
    '    End If
    '    If CustValidator.ControlToValidate = "cmbConvertUnit" Then
    '        If cmbConvertUnit.SelectedIndex = 0 Then
    '            CustValidator.ErrorMessage = "Select Convert Unit"
    '            e.IsValid = False
    '        Else
    '            e.IsValid = True
    '        End If
    '    End If
    '    If CustValidator.ControlToValidate = "txtFactor" Then
    '        If Trim(txtFactor.Text) = "" Then
    '            CustValidator.ErrorMessage = "Enter Factor"
    '            e.IsValid = False
    '        ElseIf Val(txtFactor.Text) <= 0 Then
    '            CustValidator.ErrorMessage = "Code should be Numeric and Greater than Zero"
    '            e.IsValid = False
    '        Else
    '            e.IsValid = True
    '        End If
    '    End If
    'End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        addAttributes()

        EventLogID = CType(Session("EventLogID"), Guid)           'Added by Vikrant on 22-July-2011

        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            If Session("MiddleFrame") <> "wfUnitConverter_Ajax.aspx?" Then
                Session("MiddleFrame") = "wfUnitConverter_Ajax.aspx?"
            End If

            setFocusOn()
            NewRecord()
            DataFieldBind()
            SetTitle()

            'Added by Harsh on 15th July 2024 for FLYPAL 1757
            PreserveStateOfFavIcon()

        End If

    End Sub

    Private Sub Save(sender As Object, e As EventArgs) Handles btnSave.Click

        BindControls()

        If (Not User.IsInRole("UnitConverterNew") And mUnitConverter.IsNew) Or (Not User.IsInRole("UnitConverterEdit") And Not mUnitConverter.IsNew) Then
            setObject()
            SetSession()
            'Changed by Vikrant on 22-July-2011
            mUnitDetail = (cmbBaseUnit.SelectedItem.ToString + " : " + cmbConvertUnit.SelectedItem.ToString)
            MarkLog(Util.Action.Save, "Unit Converter", User.Identity.Name & " is not Authorized User to save " & mUnitDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                setObject()
                mUnitConverter.Save()
                setFocusOn()
                'Added by Vikrant on 22-July-2011
                mUnitDetail = (cmbBaseUnit.SelectedItem.ToString + " : " + cmbConvertUnit.SelectedItem.ToString)
                MarkLog(Util.Action.Save, "Unit Converter", mUnitDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
                SetTitle()
                upnlUnitDetails.Update()
                upnlGridView.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Unit Converter", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            'upnlValidationSummary.Update()
        End If

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub GridView_UnitConverterList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgUnitConverterList.RowCommand
        BindControls()

        Dim Index As Integer
        Dim mId As Guid
        Dim mBaseUnitID As Guid
        Dim mConvertUnitID As Guid
        'Added by Vikrant on 22-July-2011
        Dim mBaseUnitName As String
        Dim mConvertUnitName As String

        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgUnitConverterList.PageIndex * dgUnitConverterList.PageSize
                mId = mUnitListForConverter(Index).ID
                'Added by Vikrant on 22-July-2011
                mBaseUnitName = mUnitListForConverter(Index).PrimaryUnitName
                mConvertUnitName = mUnitListForConverter(Index).ConvertUnitName
                mUnitDetail = mBaseUnitName + " : " + mConvertUnitName
                'End
                If (Not User.IsInRole("UnitConverterNew") And Not User.IsInRole("UnitConverterEdit")) Then
                    setObject()
                    SetSession()
                    'Changed by Vikrant on 22-July-2011
                    MarkLog(Util.Action.Edit, "Unit Converter", User.Identity.Name & " is not Authorized User to edit " & mUnitDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfUnitConverter_Ajax.aspx?"
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                mBaseUnitID = mUnitListForConverter(Index).PrimaryUnitID
                mConvertUnitID = mUnitListForConverter(Index).ConvertUnitID
                If mBaseUnitID.Equals(mConvertUnitID) Then
                    'Added by Vikrant on 25-July-2011
                    'MarkLog(Util.Action.Edit, "Unit Converter", "Base entry " + mUnitDetail + " can not be edited", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.BaseUnitEntryEdit, SIMsgBox.Message_text.BaseUnitEntryEdit, "You can not edit this entry", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfUnitConverter_Ajax.aspx?"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.BaseUnitEntryEdit, MSGBox.Message_text.BaseUnitEntryEdit, "You can not edit this entry", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                EditRecord(mId)
                cmbBaseUnit.DataBind()
                cmbConvertUnit.DataBind()
                txtFactor.DataBind()
                'Changed by Vikrant on 22-July-2011
                MarkLog(Util.Action.Edit, "Unit Converter", mUnitDetail, Util.ErrorType.NoError, mUnitConverter.ID, EventLogID)
                SetTitle(Index)
                upnlUnitDetails.Update()
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgUnitConverterList.PageIndex * dgUnitConverterList.PageSize
                mId = mUnitListForConverter(Index).ID
                mBaseUnitID = mUnitListForConverter(Index).PrimaryUnitID
                mConvertUnitID = mUnitListForConverter(Index).ConvertUnitID
                'Added by Vikrant on 22-July-2011
                mBaseUnitName = mUnitListForConverter(Index).PrimaryUnitName
                mConvertUnitName = mUnitListForConverter(Index).ConvertUnitName
                mUnitDetail = mBaseUnitName + " : " + mConvertUnitName
                Session("mUnitDetail") = mUnitDetail
                'End
                If (Not User.IsInRole("UnitConverterDelete")) Then
                    setObject()
                    SetSession()
                    'Changed by Vikrant on 22-July-2011
                    MarkLog(Util.Action.Delete, "Unit Converter", User.Identity.Name & "is not Authorized User to delete" & mUnitDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                If mBaseUnitID.Equals(mConvertUnitID) Then
                    'Added by Vikrant on 25-July-2011
                    MarkLog(Util.Action.Delete, "Unit Converter", "Base entry " + mUnitDetail + " can not be deleted", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.BaseUnitEntry, SIMsgBox.Message_text.BaseUnitEntry, "You can not delete this entry", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfUnitConverter_Ajax.aspx?"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.BaseUnitEntry, MSGBox.Message_text.BaseUnitEntry, "You can not delete this entry", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    DeleteRecord(mId)
                    'Added by Vikrant on 22-July-2011
                    'MarkLog(Util.Action.Delete, "Unit Converter", mUnitDetail, Util.ErrorType.HandledError, mUnitConverter.ID, EventLogID)
                End If
        End Select

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub Add(sender As Object, e As EventArgs) Handles btnAdd.Click
        'Changed by Vikrant on 22-July-2011
        MarkLog(Util.Action.[New], "Unit Converter", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        setFocusOn()
        NewRecord()
        DataFieldBind()
        txtFactor.Text = 0
        SetTitle()

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click
        RemoveSession()
        'Changed by Vikrant on 22-July-2011
        MarkLog(Util.Action.Close, "Unit Converter", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub GridView_UnitConverterList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgUnitConverterList.Sorting
        mUnitListForConverter.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mUnitListForConverter") = mUnitListForConverter
        dgUnitConverterList.DataSource = mUnitListForConverter
        dgUnitConverterList.DataBind()

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub GridView_UnitConverterList_Pagination(source As Object, e As GridViewPageEventArgs) Handles dgUnitConverterList.PageIndexChanging
        dgUnitConverterList.PageIndex = e.NewPageIndex
        dgUnitConverterList.DataSource = mUnitListForConverter
        Session("mUnitListForConverter") = mUnitListForConverter
        dgUnitConverterList.DataBind()

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCntrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1757
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "Unit Converter")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "Unit Converter")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub PreserveStateOfFavIcon()

        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Unit Converter") Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Mark As Favourite",
                                                "MarkAsFavourite();",
                                                True)

        Else

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "Remove From Favourite",
                                                "RemoveFromFavourite();",
                                                True)

        End If

    End Sub
    'End

#End Region

End Class
