Public Class wfSpareAssemblyList
    Inherits System.Web.UI.Page


#Region " Variable Declaration "

    Public mSpareAssemblyList As SpareAssemblyList
    Public mSearchSpareAssemblylist As SpareAssemblyList
    Public mAssemblyStatus As AssemblyStatus

    Dim mModelNo As String
    Dim EventLogID As Guid
    Public mSerialNo As String

    Public mAssemblyType As String
    Dim mFileAttach As FileAttach



#End Region

#Region " Business Methods "
    Private Sub GetSession()

        mSpareAssemblyList = CType(Session("mSpareAssemblyList"), SpareAssemblyList)

        mModelNo = Session("mModelNo")
        mSerialNo = Session("mSerialNo")

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSpareAssemblyList")

        Session.Remove("mModelNo")
        Session.Remove("mSerialNo")

    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSpareAssemblyList.aspx?" Then
            Session.Remove("mSpareAssemblyList")
            Session.Remove("mModelNo")
            Session.Remove("mSerialNo")
        End If
    End Sub
    Private Sub FindNow()
        mSpareAssemblyList = SpareAssemblyList.GetSparedAssemblyList(AssemblyID:=cmbAssembly.SelectedValue.ToString, IsPeriodValuesRequired:=True)
        Session("mSpareAssemblyList") = mSpareAssemblyList
        dgBuiltSpareList.DataSource = mSpareAssemblyList
        dgBuiltSpareList.DataBind()
        lblBuiltSpareAssembly.Text = "List of Built assembly " & " : " & mSpareAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
                            If mAssemblyStatus.AssemblyTypeID <> 1 Then
                                AssemblyStatus.DeleteAssemblyStatus(mAssemblyStatus.ID)
                               
                                DataFieldBind()
                                FindNow()
                                SetGrid()
                                upnlBuiltSpareAssembly.Update()
                                upnlSearchCriteria.Update()
                            Else
                                MSGBoxCtrl.show(MSGBox.Message_title.AirframeDelete, MSGBox.Message_text.AirframeDelete, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If
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
                            Dim mDetail As String = " Model : " & mAssemblyStatus.ModelName & " Type : " & mAssemblyStatus.AssemblyTypeName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo
                            MarkLog(Util.Action.Delete, "Assembly Status", "Can't delete : " & mDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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

        lblBuiltSpareAssembly.Text = "List of Built assembly  " & " : " & mSpareAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub ControlVisibility()


    End Sub

    Private Sub SetGrid()

        Dim P As Integer
        Dim B As Boolean

        Dim B1 As Boolean

        For j As Integer = 0 To dgBuiltSpareList.Rows.Count - 1

            B1 = CType(Me.dgBuiltSpareList.Rows(j).Cells(10).Text, Boolean)
            If B1 = False Then
                dgBuiltSpareList.Rows(j).Cells(9).Enabled = False
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

        mSearchSpareAssemblylist = SpareAssemblyList.GetSparedAssemblyList("(ALL)")
        cmbAssembly.DataSource = mSearchSpareAssemblylist

        If (Session("AssemblyId") = Guid.Empty.ToString Or IsNothing(Session("AssemblyId"))) Then
            'Do nothing
        Else
            cmbAssembly.SelectedValue = CType(Session("AssemblyId"), String)
        End If
        cmbAssembly.DataBind()
        Session("AssemblyId") = cmbAssembly.SelectedValue
        Session("mSearchSpareAssemblylist") = mSearchSpareAssemblylist
        '-----------------------------------------

        'mSpareAssemblyList = SpareAssemblyList.GetSparedAssemblyList(IsPeriodValuesRequired:=True)
        'Session("mSpareAssemblyList") = mSpareAssemblyList
        'dgBuiltSpareList.DataSource = mSpareAssemblyList
        'dgBuiltSpareList.DataBind()

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Vikrant on 26-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            Session("MiddleFrame") = "wfSpareAssemblyList.aspx?"
            DataFieldBind()
            FindNow()
            ControlVisibility()
            SetPage()
            SetGrid()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not IsValid Then Exit Sub


        mAssemblyStatus = AssemblyStatus.NewSpareAssemblyStatus(2)
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
        Session("mFileAttach") = mFileAttach
        'End


        If (Not User.IsInRole("BuildSpareAssemblyNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("BuildSpareAssemblyEdit") And Not mAssemblyStatus.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Session("mAssemblyStatus") = mAssemblyStatus
        'Added by Vikrant on 28-July-2011
        MarkLog(Util.Action.[New], "AssemblyInstallation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfSpareAssemblyStatus.aspx?BackPage=Index.aspx');", True)
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        btnFindNow_Click(sender, e)
    End Sub

    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
        ControlVisibility()
        SetGrid()

        upnlBuiltSpareAssembly.Update()
        upnlActionBtnBottom.Update()

    End Sub

    Private Sub dgBuiltSpareList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgBuiltSpareList.PageIndexChanging
        dgBuiltSpareList.PageIndex = e.NewPageIndex
        dgBuiltSpareList.DataSource = mSpareAssemblyList
        Session("mSpareAssemblyList") = mSpareAssemblyList
        dgBuiltSpareList.DataBind()
        SetGrid()
    End Sub

    Private Sub dgBuiltSpareList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgBuiltSpareList.RowCommand
        Dim Index As Int32
        mSpareAssemblyList = Session("mSpareAssemblyList")
        Select Case e.CommandName
            Case "EditRec"
                ' Index = CInt(e.CommandArgument) + dgBuiltSpareList.PageSize * dgBuiltSpareList.PageIndex
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mID, True)
                Session("mAssemblyStatus") = mAssemblyStatus


                If mAssemblyStatus.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachment(mID)
                    Session("mFileAttach") = mFileAttach
                Else
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mID)
                    Session("mFileAttach") = mFileAttach
                End If


                mAssemblyType = mSpareAssemblyList(mID).AssemblyType
                Dim mAssemblyInfo As String = mSpareAssemblyList(mID).ModelSerialNo

                MarkLog(Util.Action.Edit, "AssemblyInstallation", mAssemblyInfo, Util.ErrorType.NoError, mSpareAssemblyList(mID).AssemblyStatusID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfSpareAssemblyStatus.aspx?BackPage=Index.aspx');", True)

            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mAssemblyInfo, mAssemblyDetail As String
                Dim mAssemblyType As String
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mID, True)
                Session("mAssemblyStatus") = mAssemblyStatus
                mAssemblyType = mSpareAssemblyList(mID).AssemblyType
                mAssemblyInfo = mSpareAssemblyList(mID).ModelSerialNo
                If (Not User.IsInRole("BuildSpareAssemblyDelete")) Then
                    'Changed by Vikrant on 26-July-2011
                    
                   
                    mAssemblyDetail = " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                    MarkLog(Util.Action.Delete, "AssemblyRemoval", User.Identity.Name & " is not Authorized User to delete " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                If mAssemblyStatus.IsAttachmentAdded Then
                    mFileAttach = FileAttach.GetAttachment(mID)
                    Session("mFileAttach") = mFileAttach
                Else
                    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mID)
                    Session("mFileAttach") = mFileAttach
                End If


               

                MarkLog(Util.Action.Edit, "AssemblyInstallation", mAssemblyInfo, Util.ErrorType.NoError, mSpareAssemblyList(mID).AssemblyStatusID, EventLogID)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
            Case "View"
                Index = CInt(e.CommandArgument) + dgBuiltSpareList.PageSize * dgBuiltSpareList.PageIndex
                ' Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mIsAttachemntAdded As Boolean = mSpareAssemblyList(Index).IsAttachmentAdded
                Dim mID As Guid = New Guid(mSpareAssemblyList(Index).AssemblyStatusID.ToString)
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "AssemblyInstallation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Vikrant on 28-July-2011
        Session("MiddleFrame") = ""
        RemoveSession()
        Session.Remove("InstallDate")
        Session.Remove("InstallOnId")
        Session.Remove("AircraftId")
        Session.Remove("AssemblyId")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub dgBuiltSpareList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgBuiltSpareList.Sorting
        mSpareAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mInstalledAssemblyStatusList") = mSpareAssemblyList
        dgBuiltSpareList.DataSource = mSpareAssemblyList
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