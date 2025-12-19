'Added by Prashant

Public Class wfWorkShop_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mWorkShop As WorkShop
    Public mWorkShopList As WorkShopList
    Public mLocationList As LocationList
    Public Type As Int16 = 0
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub GetSession()
        Type = Val(Request.QueryString("Type"))
        mWorkShop = Session("mWorkShop")
        mWorkShopList = Session("mWorkShopList")
        mLocationList = Session("mLocationList")
    End Sub
    Private Sub SetSession()
        Session("mWorkShop") = mWorkShop
        Session("mWorkShopList") = mWorkShopList
        Session("mLocationList") = mLocationList
    End Sub
    Private Sub NewRecord()
        mWorkShop = WorkShop.NewWorkShop()
        Session("mWorkShop") = mWorkShop
        SetWorkShopTitle()
        txtName.Enabled = True
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        mWorkShop = WorkShop.GetWorkShop(ID)
        Session("mWorkShop") = mWorkShop
        SetWorkShopTitle()
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        WorkShopGridBind()
        EditRecord(ID)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Public Function Save() As Boolean
        Try
            setObject()
            If mWorkShop.IsValid Then
                mWorkShop.Save()
                MarkLog(Util.Action.Save, "WorkShop", mWorkShop.Name, Util.ErrorType.HandledError, mWorkShop.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                upnlDetails.Update()
                Return True
            Else
                upnlTitle.Update()
                Return False
                Exit Function
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Function
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Function
            End If
            Return False
        End Try
    End Function
    Private Sub SetWorkShopTitle()
        If mWorkShop.IsNew = True Then
            lblTitle.Text = "Work Shop [New]"
        Else
            If Len(mWorkShop.Name) > 15 Then
                lblTitle.Text = "Work Shop [" & mWorkShop.Name.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Work Shop [" & mWorkShop.Name & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            WorkShop.DeleteWorkShop(mWorkShop.ID)
                            NewRecord()
                            DataFieldBind()
                            upnlDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                Exit Sub
                            End If
                        Finally
                            MarkLog(Util.Action.Delete, "WorkShop", mWorkShop.Name, Util.ErrorType.NoError, mWorkShop.ID, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        upnlDetails.Update()
                    End If
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub ClearAll()
        If InStr(Session("MiddleFrame"), "wfWorkShop_Ajax.aspx?") <= 0 And Val(Request.QueryString("Type")) = 2 Then
            Session.Remove("mWorkShop")
            Session.Remove("mWorkShopList")
            Session.Remove("mLocationList")
            Session.Remove("Type")
            Session.Remove("New")
        End If
    End Sub
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerWorkshop(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " DataBinding "
    Private Sub setObject()
        Dim Id As New Guid(cmbLocation.SelectedValue)
        mWorkShop.Name = Trim(txtName.Text)
        mWorkShop.LocationID = Id
        Session("mWorkShop") = mWorkShop
    End Sub
    Private Sub DataFieldBind()
        mLocationList = LocationList.GetLocationList(0, , , , , , True)
        Session("mLocationList") = mLocationList
        cmbLocation.DataSource = mLocationList
        cmbLocation.DataBind()
        txtName.DataBind()
        mWorkShopList = WorkShopList.GetWorkShopList(0, "", , False)
        Session("mWorkShopList") = mWorkShopList
        dgWorkShopList.DataSource = mWorkShopList
        WorkShopGridBind()
    End Sub
    Private Sub WorkShopGridBind()
        dgWorkShopList.DataSource = mWorkShopList
        dgWorkShopList.DataBind()
        lblResult.Text = "Work Shop List: " & mWorkShopList.Count & " Record(s) Found."
        Session("mWorkShop") = mWorkShop
        upnlGridView.Update()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        If CustValidator.ControlToValidate = "cmbLocation" Then
            If cmbLocation.SelectedIndex <= 0 Then
                CustValidator.ErrorMessage = "Select Location from the List."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ClearAll()
        GetSession()
        setFocus(txtName)

        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then

            If Session("sender") = "" And Session("New") <> "True" Then
                If Type = 2 Then
                    Session("MiddleFrame") = "wfWorkShop_Ajax.aspx?Type=" & Request.QueryString("Type")
                End If
                NewRecord()
            Else
                Session("New") = ""
            End If

            DataFieldBind()

            'Added by Harsh on 15th July 2024 for FLYPAL 1745
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "WorkShop") Then

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

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("WorkShopNew") And mWorkShop.IsNew) Or (Not User.IsInRole("WorkShopEdit") And Not mWorkShop.IsNew) Then
            'setObject()
            'SetSession()
            'MarkLog(Util.Action.Save, "WorkShop", User.Identity.Name & " is not Authorized User to Save " & mWorkShop.Name, Util.ErrorType.HandledError, mWorkShop.ID, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfWorkShop_Ajax.aspx?MsgResult=0&BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type")
            'Session("sender") = "Authorization"
            'msg.Show()
            'Exit Sub
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Page.Validate("a")
        If IsValid Then
            Save()
        Else
            WorkShopGridBind()
            upnlTitle.Update()
            Exit Sub
        End If
    End Sub
    Private Sub dgWorkShopList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWorkShopList.RowCommand
        Select Case e.CommandName
            Case "EditView"
                If (Not User.IsInRole("WorkShopView") And Not User.IsInRole("WorkShopEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgWorkShopList.PageIndex * dgWorkShopList.PageSize
                Dim mID As Guid = mWorkShopList(index).ID
                Dim mLocationID As Guid = mWorkShopList(index).locationID
                Dim mName As String = mWorkShopList(index).Name
                EditRecord(mID)
                setFocus(txtName)
                txtName.DataBind()

                cmbLocation.DataSource = mLocationList
                cmbLocation.DataBind()
                cmbLocation.SelectedValue = mLocationID.ToString
                Session("mLocationID") = mLocationID
                upnlDetails.Update()
                WorkShopGridBind()
                DisableName(mID) 'Added by : Shital 19-Jun-2020, ALL16062020
                MarkLog(Util.Action.Edit, "WorkShop", mWorkShop.Name, Util.ErrorType.NoError, mWorkShop.ID, EventLogID)
            Case "Remove"
                If (Not User.IsInRole("WorkShopDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Dim index As Integer = CInt(e.CommandArgument) + dgWorkShopList.PageIndex * dgWorkShopList.PageSize
                Dim mID As Guid = mWorkShopList(index).ID
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub dgWorkShopList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWorkShopList.PageIndexChanging
        dgWorkShopList.PageIndex = e.NewPageIndex
        dgWorkShopList.DataSource = mWorkShopList
        Session("mWorkShopList") = mWorkShopList
        WorkShopGridBind()
    End Sub
    Private Sub dgWorkShopList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWorkShopList.Sorting
        mWorkShopList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mWorkShopList") = mWorkShopList
        dgWorkShopList.DataSource = mWorkShopList
        dgWorkShopList.DataBind()
    End Sub
    Private Sub AddLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addLocation.Click
        Dim str As String
        setObject()
        Session("mWorkShop") = mWorkShop
        Session("New") = "True"
        'If Type = 2 Then
        '    str = "<script language='javascript'>OpenLocation('wfStoreLocation.aspx?BackPage1=Index.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "'); </script>"
        'Else
        '    str = "<script language='javascript'>OpenLocation('wfStoreLocation.aspx?BackPage1=wfWorkShop_Ajax.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "'); </script>"
        'End If
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        If Type = 2 Then
            str = "<script language='javascript'>OpenLocation('wfStoreLocation_Ajax.aspx?BackPage1=Index.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "'); </script>"
        Else
            str = "<script language='javascript'>OpenLocation('wfStoreLocation_Ajax.aspx?BackPage1=wfWorkShop_Ajax.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&Type=" & Request.QueryString("Type") & "'); </script>"
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScript", str, False)
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        setFocus(txtName)
        MarkLog(Util.Action.[New], "WorkShop", "", Util.ErrorType.NoError, mWorkShop.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        upnlDetails.Update()
        lblResult.Text = "Work Shop List: " & mWorkShopList.Count & " Record(s) Found."
    End Sub
    Private Sub btnBackBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackBottom.Click
        MarkLog(Util.Action.Close, "WorkShop", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session.Remove("mWorkShop")
        Session.Remove("New")
        If Type = 2 Then
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        Else
            Response.Redirect(Request.QueryString("GChildPage1") & "?BackPage1=&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&Type=" & Request.QueryString("Type"))
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    'Added by Harsh on 15th July 2024 for FLYPAL 1745
    Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

        Try
            MarkFavourite(HttpContext.Current.User.Identity.Name, "WorkShop")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

        Try
            RemoveFavourite(HttpContext.Current.User.Identity.Name, "WorkShop")
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    'End

#End Region

End Class