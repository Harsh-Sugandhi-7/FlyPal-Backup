'AJAX Created By: Saylee on 25-May-2015

Public Class wfPartMonitorModList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mCompMonitorModStatus As CompMonitorModStatus
    Public mPartMonitorModList As PartMonitorModList
    Public mPartMonitorMod As PartMonitorMod
    Public mCompMonitorModStatusList As tmpCompMonitorModStatusList
    ''Added By Saylee on 6th Oct 2008
    Public mIssueDate As String

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011

    Dim mFileAttach As FileAttach
    Public mIsSpareComp As Boolean = False          'Added By Prashant 1-Oct-2020 for SpareComp
    Public mAssemblyModelID As Guid = Guid.Empty    'Added By Prashant 1-Oct-2020 for SpareComp
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mPartMonitorModList = CType(Session("mPartMonitorModList"), PartMonitorModList)
        mIssueDate = Session("mIssueDate")
        mCompMonitorModStatusList = CType(Session("mCompMonitorModStatusList"), tmpCompMonitorModStatusList)
        mIsSpareComp = Session("IsSpareComp") 'Added By Prashant 1-Oct-2020 for SpareComp
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mPartMonitorModList") = mPartMonitorModList
        Session("mIssueDate") = mIssueDate
        Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartMonitorModList")
    End Sub
    Private Sub NewRecord()
        Dim mPartMonitorMod As PartMonitorMod
        Dim mHourType As Integer = 0
        If mIsSpareComp = False Then 'Added By Prashant 1-Oct-2020 for SpareComp
            If mAssemblyStatus.IsSpareAssembly = True Then
                mHourType = mAssemblyStatus.HourType
            Else
                mHourType = mMachine.HourType
            End If
            mPartMonitorMod = PartMonitorMod.NewPartMonitorMod(Guid.NewGuid, mCompStatus.Comp.PartID, mAssemblyModelID, mHourType)
        Else
            mPartMonitorMod = PartMonitorMod.NewPartMonitorMod(Guid.NewGuid, mCompStatus.Comp.PartID, mAssemblyModelID, mCompStatus.HourType)
        End If 'End of Added By Prashant 1-Oct-2020 for SpareComp

        ' RemoveSession()
        Session("mPartMonitorMod") = mPartMonitorMod

        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Part Mod", "", Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
        'End
        ' Response.Redirect("wfPartMonitorMod_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=wfPartMonitorModList_AJAX.aspx")
        '' ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow()", True)
        Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow('" + GChildPageTmp + "');", True)

    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfPartMonitorModList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        'Session("sender") = "Delete"
        'msg1.Show()

        SetPage()
        SetGrid()
        upnldgGrid.Update()

        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartMonitorModList.CurrentIndex = Index
        Session("mPartMonitorModList") = mPartMonitorModList
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mPartMonitorMod As PartMonitorMod
        If mIsSpareComp = True Or mAssemblyStatus.IsSpareAssembly = True Then
            mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mId, mCompStatus.HourType)
        Else
            mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(mId, mMachine.HourType)
        End If
        Session("mPartMonitorMod") = mPartMonitorMod
        'Added By Utkarsh On 26-Jul-2011 For All19072011
        'MaintDetail = "Monitor Mod Type : " + mPartMonitorMod.PartMonitorModTypeName + " Description : " + mPartMonitorMod.Description
        MaintDetail = "Part : " & mCompStatus.PartNameSerialNo & " Part Modification Type : " & mPartMonitorMod.PartMonitorModTypeName & " Description : " & mPartMonitorMod.Description
        MarkLog(Util.Action.Edit, "Part Mod", MaintDetail, Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
        'End

        'Response.Redirect("wfPartMonitorMod_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=wfPartMonitorModList_AJAX.aspx")
        ''ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow()", True)
        Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModMasterWindow", "OpenModMasterWindow('" + GChildPageTmp + "');", True)

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        Dim mId As Guid
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            MaintDetail = "Mod Type : " + mPartMonitorModList(mPartMonitorModList.CurrentIndex).PartMonitorModTypeName + " Description : " + mPartMonitorModList(mPartMonitorModList.CurrentIndex).Description
                            Session("sender") = ""
                            mId = mPartMonitorModList(mPartMonitorModList.CurrentIndex).ID
                            If mPartMonitorModList(mPartMonitorModList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorModList(mPartMonitorModList.CurrentIndex).ID)
                            End If
                            PartMonitorMod.DeletePartMonitorMod(mPartMonitorModList.CurrentItem.id)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            DataFieldBind()
                            SetGrid()
                            SetPage()
                            ControlVisibility()
                            upnlActionBtnTop.Update()
                            upnldgGrid.Update()
                            upnlButtons.Update()
                            ' Response.Redirect("wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                        Catch ex As SqlException
                            If ex.Number = 8114 Or ex.Number = 8115 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                ' MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Part Mod", "Can't delete : " + MaintDetail + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)

                                'Added by saylee on 1-Jun-2016
                                Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
                                mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).PartID, mPartMonitorModList.Item(mPartMonitorModList.CurrentIndex).ID.ToString)

                                If mPartMonitorModConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mPartMonitorModConfiguredList.Count - 1
                                        If i = mPartMonitorModConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected Modification is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

                                End If

                            End If
                            DataFieldBind()
                            SetPage()
                            SetGrid()
                            upnldgGrid.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Added By Utkarsh On 26-Jul-2011 For All19072011

                                MarkLog(Util.Action.Delete, "Part Mod", MaintDetail, Util.ErrorType.NoError, mId, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    ' Response.Redirect("wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub DataFieldBind()
        'Added By Prashant 1-Oct-2020 for SpareComp
        'mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID)
        mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(mCompStatus.Comp.PartID, mAssemblyModelID)
        'End of Added By Prashant 1-Oct-2020 for SpareComp
        dgPartMonitorMod.DataSource = mPartMonitorModList
        Session("mPartMonitorModList") = mPartMonitorModList
        DataBind()
    End Sub
    Private Sub SetPage()
        lblList.Text = "Part Modification List of - [ " & "Part: " & mCompStatus.Comp.PartName & "]"
        lbldgGridResult.Text = "List Of Part Modification: " & mPartMonitorModList.Count & " Record(s)"
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = (mPartMonitorModList.Count > 0)

        btnPrintTop.Visible = (mPartMonitorModList.Count > 15)
        btnAddNewTop.Visible = (mPartMonitorModList.Count > 15)
        btnBackTop.Visible = (mPartMonitorModList.Count > 15)
    End Sub
    Private Sub SetGrid()

        Dim P As Boolean

        For j As Integer = 0 To dgPartMonitorMod.Rows.Count - 1
            P = CType(Me.dgPartMonitorMod.Rows.Item(j).Cells(16).Text, Boolean)

            If P = False Then
                Me.dgPartMonitorMod.Rows.Item(j).Cells(15).Enabled = False
            End If
        Next
    End Sub
    Public Sub SetObject()
        mPartMonitorMod = CType(Session("mPartMonitorMod"), PartMonitorMod)
        With mCompMonitorModStatus
            .PartMonitorModID = mPartMonitorMod.ID
            .PartMonitorMod.Code = .PartMonitorMod.Code
            '.PartMonitorMod.ATAChapter = mPartMonitorMod.ATAChapter
            .PartMonitorMod.Reference = mPartMonitorMod.Reference
            .PartMonitorMod.Description = mPartMonitorMod.Description
            '.PartMonitorMod.MonitorTypeNam = mPartMonitorMod.MonitorTypeName
            .PartMonitorMod.ShowInCofA = mPartMonitorMod.ShowInCofA
            .PartMonitorMod.Note = mPartMonitorMod.Note

            'Added by Saylee on 13-July-2009
            If mPartMonitorMod.MonitorTypeID = 3 Then
                .IsApplicable = False
            End If
            '********************************
        End With
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 26-Jul-2011 For All19072011
        'Added By Prashant 1-Oct-2020 for SpareComp
        If mIsSpareComp = True Then
            Dim mModelList As ModelList
            mModelList = ModelList.GetModelList(1, , , , )
            mAssemblyModelID = mModelList.Item(0).ID
        Else
            mAssemblyModelID = mAssemblyStatus.Assembly.ModelID
        End If
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If btnAddNew.Enabled = True Then
                setFocus(btnAddNew)
            End If
            DataFieldBind()
            SetPage()
            SetGrid()
            ControlVisibility()
        End If

    End Sub
    Private Sub dgPartMonitorMod_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartMonitorMod.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorMod.PageSize * dgPartMonitorMod.PageIndex
                Dim ID = mPartMonitorModList(Index).ID
                'Commneted By Utkarsh On 25-Mar-2011

                'If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                '    MarkLog(Util.Action.Edit, "PartMonitorMod", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                '    msg.Show()
                '    Exit Sub
                'End If

                '************************************
                EditRecord(ID)
            Case "DeleteRec"
                '' If (Not User.IsInRole("MachineDelete")) Then
                ''If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                ''    MarkLog(Util.Action.Delete, "PartMonitorMod", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorMod.PageSize * dgPartMonitorMod.PageIndex
                DeleteRecord(Index)
            Case "Select"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorMod.PageSize * dgPartMonitorMod.PageIndex
                Dim ID = mPartMonitorModList(Index).ID
                'Added By Vikrant on 03-Feb-2020 For Poonawalla Issue solve
                Dim mPartMonitorModTemp As PartMonitorMod
                Dim IsPeriodPresentOnComp As Boolean
                mPartMonitorModTemp = PartMonitorMod.GetPartMonitorMod(mPartMonitorModList(Index).ID)
                For Each mPartMonitorModPeriod As PartMonitorModPeriod In mPartMonitorModTemp.PartMonitorModPeriods
                    IsPeriodPresentOnComp = False
                    For Each mCompStatusPeriod As CompStatusPeriod In mCompStatus.CompStatusPeriods
                        If mCompStatusPeriod.PeriodID.Equals(mPartMonitorModPeriod.PeriodID) Then
                            IsPeriodPresentOnComp = True
                            GoTo NextStatement
                        End If
                    Next
NextStatement:
                    If IsPeriodPresentOnComp = False Then
                        Exit For
                    End If
                Next
                If IsPeriodPresentOnComp = False Then
                    MSGBoxCtrl.show("Alert!", "Selected Maintenace Activity Period(s) not present on Component", "Kindly select different maintenance activity", MsgBoxStyle.OkOnly, "DiffPeriodAlert")
                    Exit Sub
                End If
                'End
                If Session("NewPage") = "True" Then

                    'Saylee 29-Sep-2008
                    mIssueDate = Session("mIssueDate")
                    'Added By Prashant 1-Oct-2020 for SpareComp
                    'mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mPartMonitorModList(Index).IssueDateFormatted.ToString, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
                    mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, _
                                                                                         mPartMonitorModList(Index).IssueDateFormatted.ToString, mCompStatus.Comp.PartID, mAssemblyModelID, mCompStatus.ID, mMachine.HourType)
                    'End of Added By Prashant 1-Oct-2020 for SpareComp
                    mCompMonitorModStatus.PartMonitorModID(True) = ID
                    Session("mPartMonitorMod") = PartMonitorMod.GetPartMonitorMod(ID)
                    Session("mCompMonitorModStatus") = mCompMonitorModStatus
                    Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                    RemoveSession()
                    Session("mIssueDate") = mIssueDate
                    Response.Redirect("wfCompMonitorModStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    '--------------------------------
                Else
                    'mCompMonitorModStatus.PartMonitorModID(False) = ID
                    'Session("mCompMonitorModStatus") = mCompMonitorModStatus
                    'Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                    'RemoveSession()
                    If mIsSpareComp = True Or mAssemblyStatus.IsSpareAssembly = True Then
                        mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(ID, mCompStatus.HourType)
                    Else
                        mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(ID, mMachine.HourType)
                    End If
                    Session("mPartMonitorMod") = mPartMonitorMod
                    SetObject()
                    SetSession()
                    Session("FromPartMonitorModList") = True
                    Session.Remove("Edit")
                    Session.Remove("mPartMonitorModList")
                    Session("mCompMonitorModStatusList") = mCompMonitorModStatusList
                    'Code added by Saylee on 14/2008
                    'Response.Redirect(Request.QueryString("GChildPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    Response.Redirect("wfCompMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    '--------------------------------
                End If
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorMod.PageSize * dgPartMonitorMod.PageIndex
                Dim ID = mPartMonitorModList(Index).ID
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                'mPartMonitorMod = PartMonitorMod.GetPartMonitorMod(ID)
                mFileAttach = FileAttach.GetAttachment(ID)
                Session("mFileAttach") = mFileAttach
                If mFileAttach.Size > 0 Then
                    'Dim path As String = AppSettings("DOCPath") & "\" & StrName & mManual.FileExtension
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
                Else
                    'Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfPartMonitorModList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                    'msg1.Show()
                End If

        End Select
    End Sub
    Private Sub hdnBtnModMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnModMaster.Click
        DataFieldBind()
        SetPage()
        SetGrid()
        ControlVisibility()
        upnlActionBtnTop.Update()
        upnldgGrid.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        NewRecord()
        '' If (Not User.IsInRole("MachineNew") And mPartMonitorMod.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mPartMonitorMod.IsNew) Then
        ''If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
        ''    MarkLog(Util.Action.[New], "PartMonitorMod", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        ''    Session("sender") = "Authorization"
        ''    msg.Show()
        ''    Exit Sub
        ''End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()


        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

        If Session("NewPage") = "True" Then  'Added By Saylee on 6th Oct-2008
            Session("NewPage") = "False"
            Response.Redirect(Request.QueryString("BackPage"))
        ElseIf Request.QueryString("GChildPage5") <> Nothing Then
            Response.Redirect(Request.QueryString("GChildPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
        Else
            '
            Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))

        End If
    End Sub
    Private Sub dgPartMonitorMod_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartMonitorMod.Sorting
        mPartMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorModList") = mPartMonitorModList
        dgPartMonitorMod.DataSource = mPartMonitorModList
        DataBind()
        SetGrid()
    End Sub
#End Region

#Region " Report "
    'Created By :- Jyoti
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        'Commneted By Utkarsh On 25-Mar-2011

        'If (Not User.IsInRole("MachinePrint")) Then
        '    MarkLog(Util.Action.Print, "PartMonitorMod", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        '    msg.Show()
        '    Exit Sub
        'End If

        '**********************************
        Rpt = New crListPartMonitor
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        'Part Monitor Mod List
        ReportDetails.Add(New rptStatus(, 0, , _
        , , , dgPartMonitorMod.Columns.Item(1).HeaderText, , dgPartMonitorMod.Columns.Item(2).HeaderText, _
        dgPartMonitorMod.Columns.Item(3).HeaderText, _
        dgPartMonitorMod.Columns.Item(4).HeaderText, dgPartMonitorMod.Columns.Item(5).HeaderText, _
        dgPartMonitorMod.Columns.Item(6).HeaderText, dgPartMonitorMod.Columns.Item(7).HeaderText, dgPartMonitorMod.Columns.Item(8).HeaderText, dgPartMonitorMod.Columns.Item(9).HeaderText, dgPartMonitorMod.Columns.Item(10).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mPartMonitorModList.Count
        Dim I As Integer

        Dim str(9) As String

        For I = 0 To TotalCount - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""
            str(7) = ""
            str(8) = ""
            str(9) = ""

            If Me.dgPartMonitorMod.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgPartMonitorMod.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgPartMonitorMod.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgPartMonitorMod.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgPartMonitorMod.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgPartMonitorMod.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgPartMonitorMod.Rows(I).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If CType(Me.mPartMonitorModList.Item(I).ShowInCofA, String) <> "&nbsp;" Then str(6) = CType(Me.mPartMonitorModList.Item(I).ShowInCofA, String)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgPartMonitorMod.Rows(I).Cells.Item(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgPartMonitorMod.Rows(I).Cells.Item(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorMod.Rows(I).Cells.Item(10).Text <> "&nbsp;" Then str(9) = Me.dgPartMonitorMod.Rows(I).Cells.Item(10).Text.Replace("<BR>", vbCrLf)


            ReportDetails.Add(New rptStatus(, 1, , _
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8), str(9)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "Part Mod List Report", lblList.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If mPartMonitorModList.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfPartMonitorModList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'Commented By Utkarsh On 27-Jul-2011 For All19072011
        '  MarkLog(Util.Action.Print, "PartMonitorMod", "Part Monitor Mod List Report", Util.ErrorType.HandledError, Guid.Empty)
        'End

        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

End Class