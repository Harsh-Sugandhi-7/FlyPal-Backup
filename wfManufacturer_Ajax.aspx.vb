'Added by Bhushan

Public Class wfManufacturer_Ajax
    Inherits Web.UI.Page

#Region " Variable Declaration "
    Public mManufacturer As Manufacturer
    Public mManufacturerList As ManufacturerList

    Dim EventLogID As Guid 'Added By Utkarsh On 19-Jul-2011 For All19072011

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mManufacturer = CType(Session("mManufacturer"), Manufacturer)
        mManufacturerList = CType(Session("mManufacturerList"), ManufacturerList)
    End Sub
    Private Sub SetSession()
        Session("mManufacturer") = mManufacturer
        Session("mManufacturerList") = mManufacturerList
    End Sub
    Private Sub NewRecord()
        mManufacturer = Manufacturer.NewManufacturer(Guid.NewGuid)
        Session("mManufacturer") = mManufacturer
        txtName.Enabled = True
    End Sub
    Private Sub EditRecord(mId As Guid)
        mManufacturer = Manufacturer.GetManufacturer(mId)
        Session("mManufacturer") = mManufacturer
    End Sub
    Private Sub DeleteRecord(mId As Guid)
        '''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
        '''''msg1.ReplacePage = "wfManufacturer.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
        'End
        'Session("sender") = "Delete"
        '''''msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mManufacturer = Manufacturer.GetManufacturer(mId)
        Session("mManufacturer") = mManufacturer
    End Sub
    Private Sub setObject()
        mManufacturer.Name = Trim(txtName.Text)
    End Sub
    Private Overloads Sub setFocus(cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
        str = "document.getElementById('" + cntrl.ClientID + "').focus();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
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
                            mManufacturer = CType(Session("mManufacturer"), Manufacturer)
                            Manufacturer.DeleteManufacturer(mManufacturer.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                            'Response.Redirect("wfManufacturer.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                            'End
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                                ''Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                                'msg1.ReplacePage = "wfManufacturer.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                                ''End
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                                ''Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                                'msg1.ReplacePage = "wfManufacturer.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                                ''End
                                'msg1.Show()
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                                ''Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                                'msg1.ReplacePage = "wfManufacturer.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                                ''End
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011

                                MarkLog(Action.Delete, "Manufacturer", "Can't delete : " & mManufacturer.Name & " is Currently in use", ErrorType.NoError, mManufacturer.ID, EventLogID)
                                'End
                                ' msg1.Show()
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed By Utkarsh On 19-Jul-2011 For All19072011

                                MarkLog(Action.Delete, "Manufacturer", mManufacturer.Name, ErrorType.NoError, mManufacturer.ID, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    'Session("sender") = ""
                    'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                    ' Response.Redirect("wfManufacturer.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                    'End
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        SetTitle()
                    End If
                    Session("sender") = ""
                    SetTitle()
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    ' Session("sender") = ""
                    'DataFieldBind()
                    'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                    'Response.Redirect("wfManufacturer.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                    'End
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    'Session("sender") = ""
                    DataFieldBind()
                    'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
                    'Response.Redirect("wfManufacturer.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
                    'End
            End Select
        ElseIf Result1 = -1 Then
            'Session("sender") = ""
            DataFieldBind()
            'Changed By Utkarsh On 31-Jan-2013 For ALL30122013
            'Response.Redirect("wfManufacturer.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId"))
            'End
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            'Session("sender") = ""
            DataFieldBind()
        End If
        upnlManufacturer.Update()

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub
    Private Sub SetTitle()
        If mManufacturer.IsNew Then
            lbltitle.Text = "Manufacturer [New]"
        Else
            If Len(mManufacturer.Name) > 15 Then
                lbltitle.Text = "Manufacturer [" & mManufacturer.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Manufacturer [" & mManufacturer.Name & "]"
            End If
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        lblResult.Text = "Manufacturer List: " & mManufacturerList.Count & " Record(s) Found."
    End Sub
    Private Sub DisableName(mId As Guid) 'Added by : Saylee 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerManufacturer(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mManufacturerList = ManufacturerList.GetManufacturerList("", "")
        Session("mManufacturerList") = mManufacturerList
        dgManufacturer.DataSource = mManufacturerList
        dgManufacturer.DataBind() '''''DataBind()

        txtName.Text = mManufacturer.Name
        upnlManufacturer.Update()
    End Sub

    'Public Sub customvalidate( s As Object,  e As ServerValidateEventArgs)
    '    Dim custValidator As CustomValidator
    '    custValidator = CType(s, CustomValidator)
    '    If custValidator.ControlToValidate = "txtName" Then
    '        If txtName.Text.Trim.Length > 50 Then
    '            txtName.Text = txtName.Text.Trim.Substring(0, 46) + "..."
    '            e.IsValid = False
    '        End If
    '    End If
    'End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 19-Jul-2011 For All19072011

        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            If IsNothing(Request.QueryString("ChildPage2")) Or Request.QueryString("ChildPage2") = "" Then
                'Session("MiddleFrame") = "wfManufacturer_Ajax.aspx?"
            End If

            NewRecord()
            DataFieldBind()

            'Added by Harsh on 15th July 2024 for FLYPAL 1757
            PreserveStateOfFavIcon()

        End If

        SetTitle()

    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click

        'Changed By Utkarsh On 19-Jul-2011 For All19072011)
        MarkLog(Action.Close,
                "Manufacturer",
                "",
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)
        'End
        Session("sender") = ""
        'Added by utkarsh for Manufacturer Master as Popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then

            'Session.Remove("MiddleFrame")
            Session.Remove("mManufacturer")
            Session.Remove("mManufacturerList")
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "onclose",
                                                "CallParentCallback();",
                                                True)
            Exit Sub

        End If
        'End

        If IsNothing(Request.QueryString("ChildPage2")) Or
           Request.QueryString("ChildPage2") = "" Then

            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")

        Else

            Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" &
                              Request.QueryString("BackPage") & "&ChildPage=" &
                              Request.QueryString("ChildPage") & "&ChildPage2=" &
                              Request.QueryString("ChildPage2") & "&GChildPage=" &
                              Request.QueryString("GChildPage") & "&ChildPage1=" &
                              Request.QueryString("ChildPage1") & "&GChildPage1=" &
                              Request.QueryString("GChildPage1") & "&GChildPage2=" &
                              Request.QueryString("GChildPage2") & "&Type=" &
                              Request.QueryString("Type") & "&AssemblyTypeId=" &
                              Request.QueryString("AssemblyTypeId"))
            'end
        End If

    End Sub

    Private Sub Save(sender As Object, e As EventArgs) Handles btnSave.Click

        If (Not User.IsInRole("ManufacturerNew") And mManufacturer.IsNew) Or
           (Not User.IsInRole("ManufacturerEdit") And Not mManufacturer.IsNew) Then

            setObject()
            SetSession()
            'Changed By Utkarsh On 19-Jul-2011 For All19072011
            MarkLog(Action.Save,
                    "Manufacturer",
                    User.Identity.Name & " is not Authorized User to save " & mManufacturer.Name,
                    ErrorType.HandledError,
                    Guid.Empty,
                    EventLogID)

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "Authorization")
            Exit Sub

        End If

        If Not IsValid Then Exit Sub

        Try

            setObject()
            mManufacturer.Save()
            'Changed By Utkarsh On 19-Jul-2011 For All19072011
            MarkLog(Action.Save,
                    "Manufacturer",
                    mManufacturer.Name,
                    ErrorType.NoError,
                    mManufacturer.ID,
                    EventLogID)
            'End
            mManufacturer = Manufacturer.NewManufacturer(Guid.Empty)
            NewRecord()
            DataFieldBind()
            SetSession()
            SetTitle()

        Catch ex As SqlException

            If ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "Delete")

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "Delete")

            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                MSGBox.Message_text.ReferenceDelete,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "Delete")

            End If

            DataFieldBind()

        End Try

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub GridView_Manufacturer_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgManufacturer.RowCommand

        Dim Idx As Int32
        Dim mId As Guid

        Select Case e.CommandName

            Case "ViewRec"

                Idx = CInt(e.CommandArgument) + dgManufacturer.PageIndex * dgManufacturer.PageSize
                mId = mManufacturerList(Idx).ID
                Dim mName As String = mManufacturerList(Idx).Name

                If (Not User.IsInRole("ManufacturerView") And
                    Not User.IsInRole("ManufacturerEdit")) Then

                    setObject()
                    SetSession()
                    'Changed By Utkarsh On 19-Jul-2011 For All19072011
                    MarkLog(Action.Edit, "Manufacturer",
                            User.Identity.Name & " is not Authorized User to Edit " & mName,
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)
                    'End

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                EditRecord(mId)
                txtName.Text = mManufacturer.Name
                SetTitle()
                MarkLog(Action.Edit,
                        "Manufacturer",
                        mManufacturer.Name,
                        ErrorType.NoError,
                        mManufacturer.ID,
                        EventLogID)

                'End
                upnlManufacturer.Update()
                DisableName(mId)

            Case "DeleteRec"

                Idx = CInt(e.CommandArgument) + dgManufacturer.PageIndex * dgManufacturer.PageSize
                mId = mManufacturerList(Idx).ID
                Dim mName As String = mManufacturerList(Idx).Name

                If (Not User.IsInRole("ManufacturerDelete")) Then

                    setObject()
                    SetSession()
                    'Changed By Utkarsh On 19-Jul-2011 For All19072011
                    MarkLog(Action.Delete,
                            "Manufacturer",
                            User.Identity.Name & " is not Authorized User to Delete " & mName,
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)

                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                DeleteRecord(mId)

        End Select

        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub GridView_Manufacturer_Pagination(sender As Object, e As GridViewPageEventArgs) Handles dgManufacturer.PageIndexChanging

        dgManufacturer.PageIndex = e.NewPageIndex
        dgManufacturer.DataSource = mManufacturerList
        Session("mManufacturerList") = mManufacturerList
        dgManufacturer.DataBind()
        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub GridView_Manufacturer_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgManufacturer.Sorting

        mManufacturerList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mManufacturerList") = mManufacturerList
        dgManufacturer.DataSource = mManufacturerList
        dgManufacturer.DataBind()
        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub AddRecocrd(sender As Object, e As EventArgs) Handles btnAdd.Click

        NewRecord()
        'Added By Utkarsh On 19-Jul-2011 For All19072011
        MarkLog(Action.[New],
                "Manufacturer",
                "",
                ErrorType.NoError,
                mManufacturer.ID,
                EventLogID)
        'End
        DataFieldBind()
        SetTitle()
        'Added by Harsh on 15th July 2024 for FLYPAL 1757
        PreserveStateOfFavIcon()

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1757
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "Manufacturer")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "Manufacturer")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub PreserveStateOfFavIcon()

        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Manufacturer") Then

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