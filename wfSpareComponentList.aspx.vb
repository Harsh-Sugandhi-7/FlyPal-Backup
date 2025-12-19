Public Class wfSpareComponentList
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Public mSpareCompList As SpareCompList
    Public mSearchSpareComplist As SpareCompList
    Public mCompStatus As CompStatus
    Dim mModelNo As String
    Dim EventLogID As Guid
    Public mSerialNo As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()

        mSpareCompList = CType(Session("mSpareCompList"), SpareCompList)

        mModelNo = Session("mModelNo")
        mSerialNo = Session("mSerialNo")

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSpareCompList")

        Session.Remove("mModelNo")
        Session.Remove("mSerialNo")

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSpareComponentList.aspx?" Then
            Session.Remove("mSpareCompList")
            Session.Remove("mModelNo")
            Session.Remove("mSerialNo")
        End If
    End Sub
    Private Sub FindNow()
        mSpareCompList = SpareCompList.GetSparedCompList(CompID:=cmbComponent.SelectedValue.ToString, IsPeriodValuesRequired:=True)
        Session("mSpareCompList") = mSpareCompList
        dgBuiltSpareList.DataSource = mSpareCompList
        dgBuiltSpareList.DataBind()
        lblBuiltSpareComponent.Text = "List of Built Comp " & " : " & mSpareCompList.Count & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mCompStatus = CType(Session("mCompStatus"), CompStatus)

                            CompStatus.DeleteSpareCompStatus(mCompStatus.ID, True)

                            DataFieldBind()
                            FindNow()
                            SetGrid()
                            upnlBuiltSpareComponent.Update()
                            upnlSearchCriteria.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            ElseIf ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
                        Finally
                            ' Dim mDetail As String = " Model : " & mCompStatus.ModelName & " Type : " & mCompStatus.CompTypeName & " Serial No. : " & mCompStatus.Comp.SerialNo
                            ' MarkLog(Util.Action.Delete, "Comp Status", "Can't delete : " & mDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        End Try
                    End If

                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '   DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()

        lblBuiltSpareComponent.Text = "List of Built Comp  " & " : " & mSpareCompList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()


    End Sub

    Private Sub SetGrid()

        Dim P As Integer
        Dim B As Boolean

        Dim B1 As Boolean

        For j As Integer = 0 To dgBuiltSpareList.Rows.Count - 1

            B1 = CType(Me.dgBuiltSpareList.Rows(j).Cells(8).Text, Boolean)
            If B1 = False Then
                dgBuiltSpareList.Rows(j).Cells(7).Enabled = False
            End If
        Next
    End Sub

    'Added By Vikrant On 01-Dec-2014
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID) 'Sort = 1 - Installation
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded)
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    'End
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()

        mSearchSpareComplist = SpareCompList.GetSparedCompList("(ALL)")
        cmbComponent.DataSource = mSearchSpareComplist

        If (Session("CompId") = Guid.Empty.ToString Or IsNothing(Session("CompId"))) Then
            'Do nothing
        Else
            cmbComponent.SelectedValue = CType(Session("CompId"), String)
        End If
        cmbComponent.DataBind()
        Session("CompId") = cmbComponent.SelectedValue
        Session("mSearchSpareComplist") = mSearchSpareComplist
        '-----------------------------------------

        'mSpareCompList = SpareCompList.GetSparedCompList(IsPeriodValuesRequired:=True)
        'Session("mSpareCompList") = mSpareCompList
        'dgBuiltSpareList.DataSource = mSpareCompList
        'dgBuiltSpareList.DataBind()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            Session("MiddleFrame") = "wfSpareComponentList.aspx?"
            DataFieldBind()
            FindNow()
            ControlVisibility()
            SetPage()
            SetGrid()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not IsValid Then Exit Sub


        mCompStatus = CompStatus.NewSpareCompStatus()
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompStatus.ID, Sort:=1) 'Sort = 1 : Installation
        Session("mFileAttach") = mFileAttach
        'End


        If (Not User.IsInRole("BuildSpareCompNew") And mCompStatus.IsNew) Or (Not User.IsInRole("BuildSpareCompEdit") And Not mCompStatus.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Session("mCompStatus") = mCompStatus
        'Added by Vikrant on 28-July-2011
        MarkLog(Util.Action.[New], "CompInstallation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfSpareCompStatus.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub cmbComponent_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbComponent.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub

    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        ControlVisibility()
        SetGrid()

        upnlBuiltSpareComponent.Update()
        upnlActionBtnBottom.Update()

    End Sub

    Private Sub dgBuiltSpareList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgBuiltSpareList.PageIndexChanging
        dgBuiltSpareList.PageIndex = e.NewPageIndex
        dgBuiltSpareList.DataSource = mSpareCompList
        Session("mSpareCompList") = mSpareCompList
        dgBuiltSpareList.DataBind()
        SetGrid()
    End Sub

    Private Sub dgBuiltSpareList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgBuiltSpareList.RowCommand
        Dim Index As Int32
        mSpareCompList = Session("mSpareCompList")
        Select Case e.CommandName
            Case "EditRec"
                ' Index = CInt(e.CommandArgument) + dgBuiltSpareList.PageSize * dgBuiltSpareList.PageIndex
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mCompStatus = CompStatus.GetSpareCompStatus(mID, True)
                Session("mCompStatus") = mCompStatus


                If mCompStatus.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachment(mID)
                    Session("mFileAttach") = mFileAttach
                Else
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mID)
                    Session("mFileAttach") = mFileAttach
                End If



                Dim mCompInfo As String = mSpareCompList(mID).ItemSerialNo

                MarkLog(Util.Action.Edit, "CompInstallation", mCompInfo, Util.ErrorType.NoError, mSpareCompList(mID).CompStatusID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfSpareCompStatus.aspx?BackPage=Index.aspx');", True)

            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mCompInfo, mCompDetail As String
   mCompStatus = CompStatus.GetSpareCompStatus(mID, True)
                Session("mCompStatus") = mCompStatus

                mCompInfo = mSpareCompList(mID).ItemSerialNo
                If (Not User.IsInRole("BuildSpareCompDelete")) Then
                    'Changed by Vikrant on 26-July-2011


                    mCompDetail = " Comp Info. : " & mCompInfo
                    MarkLog(Util.Action.Delete, "CompRemoval", User.Identity.Name & " is not Authorized User to delete " & mCompDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                If mCompStatus.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachment(mID)
                    Session("mFileAttach") = mFileAttach
                Else
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mID)
                    Session("mFileAttach") = mFileAttach
                End If




                MarkLog(Util.Action.Edit, "CompInstallation", mCompInfo, Util.ErrorType.NoError, mSpareCompList(mID).CompStatusID, EventLogID)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            Case "View"
                Index = CInt(e.CommandArgument) + dgBuiltSpareList.PageSize * dgBuiltSpareList.PageIndex
                ' Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mIsAttachemntAdded As Boolean = mSpareCompList(Index).IsAttachmentAdded
                Dim mID As Guid = New Guid(mSpareCompList(Index).CompStatusID.ToString)
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "CompInstallation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Vikrant on 28-July-2011
        Session("MiddleFrame") = ""
        RemoveSession()
        Session.Remove("InstallDate")
        Session.Remove("InstallOnId")
        Session.Remove("AircraftId")
        Session.Remove("CompId")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgBuiltSpareList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgBuiltSpareList.Sorting
        mSpareCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInstalledCompStatusList") = mSpareCompList
        dgBuiltSpareList.DataSource = mSpareCompList
        dgBuiltSpareList.DataBind()
        SetGrid()
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub dgBuiltSpareList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).ToolTip = dgBuiltSpareList.Columns(i).HeaderText
            Next
        End If
    End Sub

#End Region


End Class