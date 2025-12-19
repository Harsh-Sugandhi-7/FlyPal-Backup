Partial Class wfKitList_Ajax
    Inherits Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents valError As ValidationSummary
    Protected WithEvents txt As TextBox
    Protected WithEvents RequiredFieldValidator1 As RequiredFieldValidator
    Protected WithEvents reset As Button

    Protected WithEvents CustomValidator1 As CustomValidator
    Protected WithEvents print As ImageButton
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As Object

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.

        InitializeComponent()

    End Sub

#End Region

#Region " Enumaration "

    Public Enum UserRightsFor

        urfNew = 1
        urfEdit = 2
        urfDelete = 3
        urfView = 4
        urfPrint = 5
        urfSave = 6

    End Enum

#End Region

#Region " Variable Declaration "

    Dim mKit As Kit
    Dim mKitList As KitList
    Dim Index, Text As String
    'Added by Vikrant on 27-July-2011
    Dim EventLogID As Guid

#End Region

#Region " Business Methods "

    Private Overloads Sub SetFocus(cntrl As WebControl)

        Dim str As String

        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub

        Try

            str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
            ScriptManager.RegisterStartupScript(Me, [GetType], "focusscript", str, True)

        Catch ex As Exception
        End Try

    End Sub

    Private Sub GetSession()

        mKit = Session("mKit")
        mKitList = Session("mKitList")
        Index = Session("Index")
        Text = Session("Text")

    End Sub

    Private Sub SetSession()

        Session("mKit") = mKit
        Session("mKitList") = mKitList
        Session("Index") = Index
        Session("Text") = Text

    End Sub

    Private Sub RemoveSession()

        Session.Remove("Index")
        Session.Remove("Text")
        Session.Remove("mKit")
        Session.Remove("mKitList")

    End Sub

    Private Sub ClearAll()

        If Session("MiddleFrame") <> "wfKitList_Ajax.aspx" Then

            Session.Remove("mKit")
            Session.Remove("mKitList")
            Session.Remove("Text")
            Session.Remove("Index")

        End If

    End Sub

    Private Sub Setpage()

        If cmbLookIn.SelectedIndex = 0 Then ' Changed By Prashant on 28-12-2007

            txtFor.Text = ""
            txtFor.ReadOnly = True
            txtFor.BackColor = Color.Silver

        Else

            txtFor.ReadOnly = False
            txtFor.BackColor = Color.White

        End If

    End Sub

    Private Sub ClearControls()

        txtFor.Text = ""
        txtFor.ReadOnly = True
        txtFor.BackColor = Color.Silver
        cmbLookIn.SelectedIndex = 0

    End Sub

    Private Sub NewRecord()

        mKit = Kit.NewKit()
        mKit.Type = 1
        Session("mKit") = mKit

    End Sub

    Private Sub EditRecord(mId As Guid)

        mKit = Kit.Getkit(mId)
        Session("mKit") = mKit

    End Sub

    Private Sub DeleteRecord(mId As Guid)

        MSGBoxCtrl.show(MSGBox.Message_title.Delete,
                        MSGBox.Message_text.Delete,
                        "",
                        MsgBoxStyle.YesNo,
                        "Delete")

        mKit = Kit.Getkit(mId)
        Session("mKit") = mKit

    End Sub

    Private Function FormLevelRights(Type As UserRightsFor) As Boolean

        Select Case Type

        End Select

    End Function
    Private Sub EnableDisableButtons()

        'Enables Buttons as per User Rights
        btnAdd.Enabled = FormLevelRights(UserRightsFor.urfNew)
        dgKitList.Columns(2).Visible = FormLevelRights(UserRightsFor.urfEdit)
        dgKitList.Columns(3).Visible = FormLevelRights(UserRightsFor.urfDelete)

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult

        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then

            Select Case Result1

                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Delete" Then

                        Try

                            Dim mKit As Kit

                            Session("Sender") = ""
                            mKit = CType(Session("mKit"), Kit)
                            mKit.Delete()
                            mKit.Save()
                            DataFieldBind()
                            ClearControls()
                            upnlGrid.Update()
                            upnlGridViewTitle.Update()
                            upnlInspectionKitDetails.Update()
                            'Added by Vikrant
                            MarkLog(Action.Delete,
                                    "Inspection Kit",
                                    mKit.KitName,
                                    ErrorType.NoError,
                                    mKit.ID,
                                    EventLogID)

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                'Added by Vikrant
                                MarkLog(Action.Delete,
                                        "Currency",
                                        "Can't delete :" & mKit.KitName & " is Currently in use",
                                        ErrorType.NoError,
                                        mKit.ID,
                                        EventLogID)

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            DataFieldBind()

                        End Try

                    End If

                Case MsgBoxResult.No

                    Session("Sender") = ""

                Case MsgBoxResult.Ok And Session("sender") = ""        'Code Added

                    Session("sender") = ""
                    DataFieldBind()

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added

                    DataFieldBind()

            End Select

        ElseIf Result1 = -1 Then

            Session("sender") = ""

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added

            Session("sender") = ""
            DataFieldBind()

        End If

    End Sub

    '======Added By Prashant on 10-Dec-2007==========
    Private Sub FindNow(Optional LookInType As Int16 = 1,
                        Optional KitName As String = "",
                        Optional ItemName As String = "")

        If LookInType = -1 Then

            LookInType = 0   ' This step is IMP when details form  is opened directly.

        End If

        If LookInType = 0 Then

            mKitList = KitList.GetKitList(0, "", "")

        Else

            mKitList = KitList.GetKitList(LookInType, KitName, ItemName)

        End If

        dgKitList.DataSource = mKitList
        Session("mKitList") = mKitList
        dgKitList.DataBind()
        lblResult.Text = "List of Inspection Kit as per criteria : " + CType(mKitList.Count, String) + " Record(s) found."
        upnlGrid.Update()
        upnlGridViewTitle.Update()

    End Sub
    '================================================

    '======Added By Prashant on 10-Dec-2007==========
    Private Sub SetControl()

        Index = Session("Index")
        Text = Session("Text")
        FindNow(Index, Text, Text)
        txtFor.Text = Text
        cmbLookIn.SelectedIndex = Index
        dgKitList.DataBind()

    End Sub
    '=============================================

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mKitList = KitList.GetKitList(0, "", "")
        dgKitList.DataSource = mKitList

        Index = IIf(IsNothing(Index), 0, Index)
        Text = Session("Text")
        Session("Text") = Text
        Session("Index") = Index

        Session("mKitList") = mKitList
        DataBind()
        lblResult.Text = "List of Inspection Kit as per criteria : " + CType(mKitList.Count, String) + " Record(s) found."
        upnlGrid.Update()
        upnlGridViewTitle.Update()
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()

        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 27-July-2011

        If Not IsPostBack And Session("sender") = "" Then

            If cmbLookIn.Enabled = True Then
                SetFocus(cmbLookIn)
            End If

            Session("MiddleFrame") = "wfKitList_Ajax.aspx"
            DataFieldBind()
            SetControl()
            Setpage()

            'Added by Harsh on 15th July 2024 for FLYPAL 1745
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Kit") Then

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

        End If

    End Sub

    Private Sub GV_KitList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgKitList.PageIndexChanging

        dgKitList.PageIndex = e.NewPageIndex
        dgKitList.DataSource = mKitList
        Session("mKitList") = mKitList
        dgKitList.DataBind()

    End Sub

    Private Sub GV_KitList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgKitList.RowCommand

        Dim Index As Int32
        Dim mId As New Guid
        Dim mKitName As String

        Select Case e.CommandName

            Case "EditRec"

                Dim str As String
                Index = CInt(e.CommandArgument) + dgKitList.PageIndex * dgKitList.PageSize
                mId = mKitList(Index).ID
                mKitName = mKitList(Index).KitName

                If (Not User.IsInRole("KitView") And Not User.IsInRole("KitEdit")) Then

                    SetSession()
                    'Added by Vikrant on 27-July-2011
                    MarkLog(Action.Edit, "Inspection Kit",
                            User.Identity.Name & " is not Authorized User to edit " & mKitName,
                            ErrorType.NoError,
                            Guid.Empty,
                            EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")
                    Exit Sub

                End If

                EditRecord(mId)
                'Added by Vikrant on 27-July-2011
                MarkLog(Action.Edit,
                        "Inspection Kit",
                        mKit.KitName,
                        ErrorType.NoError,
                        mKit.ID,
                        EventLogID)

                Str = "openledgersame('wfKit_Ajax.aspx?BackPage=Index.aspx');"
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenScript",
                                                    str,
                                                    True)

            Case "DeleteRec"

                Index = CInt(e.CommandArgument) + dgKitList.PageIndex * dgKitList.PageSize
                mId = mKitList(Index).ID
                mKitName = mKitList(Index).KitName

                If (Not User.IsInRole("KitDelete")) Then

                    SetSession()
                    'Added by Vikrant on 27-July-2011
                    MarkLog(Action.Delete,
                            "Inspection Kit",
                            User.Identity.Name & " is not Authorized User to delete " & mKitName,
                            ErrorType.NoError,
                            Guid.Empty,
                            EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "Authorization")

                    Exit Sub

                End If

                DeleteRecord(mId)

        End Select

    End Sub

    Private Sub AddNewRecord(sender As Object, e As EventArgs) Handles btnAdd.Click

        Dim str As String

        NewRecord()
        SetSession()

        If (Not User.IsInRole("KitNew") And mKit.IsNew) Or
           (Not User.IsInRole("KitEdit") And Not mKit.IsNew) Then

            SetSession()
            'Added by Vikrant on 27-July-2011
            MarkLog(Action.[New],
                    "Inspection Kit",
                    User.Identity.Name & " is not Authorized User to add " & mKit.KitName,
                    ErrorType.NoError,
                    Guid.Empty,
                    EventLogID)

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "Authorization")

            Exit Sub

        End If


        'Added by Vikrant on 27-July-2011
        MarkLog(Action.[New],
                "Inspection Kit",
                "",
                ErrorType.NoError,
                mKit.ID,
                EventLogID)

        str = "openledgersame('wfKit_Ajax.aspx?BackPage=Index.aspx');"

        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "OpenScript",
                                            str,
                                            True)

    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click

        RemoveSession()

        Session("MiddleFrame") = ""
        'Added by Vikrant on 27-July-2011
        MarkLog(Action.Close,
                "Inspection Kit",
                "",
                ErrorType.NoError,
                Guid.Empty,
                EventLogID)

        Response.Redirect("Dashboard.aspx")

    End Sub

    Private Sub FindNow_Click(sender As Object, e As ImageClickEventArgs) Handles SearchButton.Click

        Index = cmbLookIn.SelectedIndex
        Text = txtFor.Text.Trim
        Session("Text") = Text
        Session("Index") = Index
        FindNow(cmbLookIn.SelectedIndex,
                txtFor.Text.Trim,
                txtFor.Text.Trim)

    End Sub

    Private Sub DD_LookIN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLookIn.SelectedIndexChanged

        Setpage()

        If cmbLookIn.Enabled = True Then

            SetFocus(cmbLookIn)

        End If

    End Sub

    'Added By Prashant 17-July-2009 for grid sorting
    Private Sub GV_KitList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgKitList.Sorting

        mKitList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mKitList") = mKitList
        dgKitList.DataSource = mKitList
        dgKitList.DataBind()

    End Sub

    Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

        MessageBoxResult()

    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1745
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "Kit")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "Kit")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    'End

#End Region

End Class
