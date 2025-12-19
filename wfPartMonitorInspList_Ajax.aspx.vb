'AJAX Created By: Saylee on 21-May-2015

Public Class wfPartMonitorInspList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mCompMonitorInspStatus As CompMonitorInspStatus
    Public mPartMonitorInspList As PartMonitorInspList
    Public mPartMonitorInsp As PartMonitorInsp
    Public mCompMonitorInspStatusList As tmpCompMonitorInspStatusList
    ''Added By Saylee on 6th Oct 2008
    Public mIssueDate As String

    Dim EventLogID As Guid 'Added By Utkarsh On 26-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 26-Jul-2011 For All19072011

    Dim mFileAttach As FileAttach

    Public mIsSpareComp As Boolean = False 'Added by Shital on 30-Sep-2020 for SpareComp
    Public mAssemblyModelID As Guid 'Added by Shital on 30-Sep-2020 for SpareComp
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mCompMonitorInspStatus = CType(Session("mCompMonitorInspStatus"), CompMonitorInspStatus)
        mPartMonitorInspList = CType(Session("mPartMonitorInspList"), PartMonitorInspList)
        mIssueDate = Session("mIssueDate")
        mCompMonitorInspStatusList = CType(Session("mCompMonitorInspStatusList"), tmpCompMonitorInspStatusList)
        mIsSpareComp = Session("IsSpareComp") 'Added by Shital on 30-Sep-2020 for SpareComp
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mPartMonitorInspList") = mPartMonitorInspList
        Session("mIssueDate") = mIssueDate
        Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartMonitorInspList")
    End Sub
    Private Sub NewRecord()
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mIsSpareComp = False Then
            If mAssemblyStatus.IsSpareAssembly = True Then
                mHourType = mAssemblyStatus.HourType
            Else
                mHourType = mMachine.HourType
            End If
        End If

        '*********************

        Dim mPartMonitorInsp As PartMonitorInsp
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        'mPartMonitorInsp = PartMonitorInsp.NewPartMonitorInsp(ID, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mMachine.HourType, ID)
        If mIsSpareComp = False Then
            mPartMonitorInsp = PartMonitorInsp.NewPartMonitorInsp(ID, mCompStatus.Comp.PartID, mAssemblyModelID, mHourType, ID)
        Else
            mPartMonitorInsp = PartMonitorInsp.NewPartMonitorInsp(ID, mCompStatus.Comp.PartID, mAssemblyModelID, mCompStatus.HourType, ID)
        End If
        ' RemoveSession()
        Session("mPartMonitorInsp") = mPartMonitorInsp

        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Part Insp", "", Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)
        'End
        ' Response.Redirect("wfPartMonitorInsp_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=wfPartMonitorInspList_AJAX.aspx")
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow()", True)
        Dim GChildPage2, GChildPage4, GChildPage5, GChildPage6 As String 'Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow('" + GChildPageTmp + "');", True)
        GChildPage2 = Trim(Request.QueryString("GChildPage2"))
        GChildPage4 = Trim(Request.QueryString("GChildPage4"))
        GChildPage5 = Trim(Request.QueryString("GChildPage5"))
        GChildPage6 = Trim(Request.QueryString("GChildPage6"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow('" + GChildPage2 + "','" + GChildPage4 + "','" + GChildPage5 + "','" + GChildPage6 + "');", True)

    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfPartMonitorInspList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        'Session("sender") = "Delete"
        'msg1.Show()
        SetPage()
        SetGrid()
        upnldgGrid.Update()

        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartMonitorInspList.CurrentIndex = Index
        Session("mPartMonitorInspList") = mPartMonitorInspList
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mPartMonitorInsp As PartMonitorInsp
        If mIsSpareComp = True Then   'Added by Shital on 30-Sep-2020
            mPartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(mId, mCompStatus.HourType)
        Else
            mPartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(mId, mMachine.HourType)

        End If

        Session("mPartMonitorInsp") = mPartMonitorInsp

        'Added By Utkarsh On 26-Jul-2011 For All19072011
        'MaintDetail = "Monitor Insp Type : " + mPartMonitorInsp.PartMonitorInspTypeName + " Description : " + mPartMonitorInsp.Description
        MaintDetail = "Part : " & mCompStatus.PartNameSerialNo & " Part Inspection Type : " & mPartMonitorInsp.PartMonitorInspTypeName & " Description : " & mPartMonitorInsp.Description
        MarkLog(Util.Action.Edit, "Part Insp", MaintDetail, Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)
        'End
        ' RemoveSession()
        'Response.Redirect("wfPartMonitorInsp_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=wfPartMonitorInspList_AJAX.aspx")
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow()", True)
        Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow('" + GChildPageTmp + "');", True)

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
                            MaintDetail = "Insp Type : " + mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).PartMonitorInspTypeName + " Description : " + mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).Description
                            Session("sender") = ""
                            mId = mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).ID
                            If mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorInspList(mPartMonitorInspList.CurrentIndex).ID)
                            End If
                            PartMonitorInsp.DeletePartMonitorInsp(mPartMonitorInspList.CurrentItem.id)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            DataFieldBind()
                            SetPage()
                            SetGrid()
                            ControlVisibility()
                            upnlActionBtnTop.Update()
                            upnldgGrid.Update()
                            upnlButtons.Update()
                            ' Response.Redirect("wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                        Catch ex As SqlException
                            If ex.Number = 8114 Or ex.Number = 8115 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                '  MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Part Insp", "Can't delete : " + MaintDetail + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                                'Added by saylee on 1-Jun-2016
                                Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
                                mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInspList.Item(mPartMonitorInspList.CurrentIndex).PartID, mPartMonitorInspList.Item(mPartMonitorInspList.CurrentIndex).ID.ToString)

                                If mPartMonitorInspConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mPartMonitorInspConfiguredList.Count - 1
                                        If i = mPartMonitorInspConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.show("Deletion Alert!", "Selected Inspection is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

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

                                MarkLog(Util.Action.Delete, "Part Insp", MaintDetail, Util.ErrorType.NoError, mId, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    ' Response.Redirect("wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub DataFieldBind()
        'Commented & Added By Vikrant For MPD
        'mPartMonitorInspList = PartMonitorInspList.GetPartMonitorInspList(mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID)
        mPartMonitorInspList = PartMonitorInspList.GetPartMonitorInspList(mCompStatus.Comp.PartID, Guid.Empty) 'Pass Blank ModelID as it was Commenetd in SP
        'End
        dgPartMonitorInsp.DataSource = mPartMonitorInspList
        Session("mPartMonitorInspList") = mPartMonitorInspList
        DataBind()
    End Sub
    Private Sub SetPage()
        lblList.Text = "Part Inspection List of - [ " & "Part: " & mCompStatus.Comp.PartName & " ]"
        lbldgGridResult.Text = "List Of Part Inspection: " & mPartMonitorInspList.Count & " Record(s)"
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = (mPartMonitorInspList.Count > 0)
        btnPrintTop.Visible = (mPartMonitorInspList.Count > 15)
        btnAddNewTop.Visible = (mPartMonitorInspList.Count > 15)
        btnBackTop.Visible = (mPartMonitorInspList.Count > 15)
    End Sub
    Private Sub SetGrid()

        Dim P As Boolean

        For j As Integer = 0 To dgPartMonitorInsp.Rows.Count - 1
            P = CType(Me.dgPartMonitorInsp.Rows.Item(j).Cells(14).Text, Boolean)

            If P = False Then
                Me.dgPartMonitorInsp.Rows.Item(j).Cells(13).Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 26-Jul-2011 For All19072011
        'Added by Shital on 30-Sep-2020
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
    Private Sub dgPartMonitorInsp_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartMonitorInsp.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorInsp.PageSize * dgPartMonitorInsp.PageIndex
                Dim ID = mPartMonitorInspList(Index).ID
                'Commneted By Utkarsh On 25-Mar-2011

                'If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                '    MarkLog(Util.Action.Edit, "PartMonitorInsp", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                '    msg.Show()
                '    Exit Sub
                'End If

                '************************************
                EditRecord(ID)
            Case "DeleteRec"
                '' If (Not User.IsInRole("MachineDelete")) Then
                ''If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                ''    MarkLog(Util.Action.Delete, "PartMonitorInsp", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorInsp.PageSize * dgPartMonitorInsp.PageIndex
                DeleteRecord(Index)
            Case "Select"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorInsp.PageSize * dgPartMonitorInsp.PageIndex
                Dim ID = mPartMonitorInspList(Index).ID
                'Added By Vikrant on 03-Feb-2020 For Poonawalla Issue solve
                Dim mPartMonitorInspTemp As PartMonitorInsp
                Dim IsPeriodPresentOnComp As Boolean
                mPartMonitorInspTemp = PartMonitorInsp.GetPartMonitorInsp(mPartMonitorInspList(Index).ID)
                For Each mPartMonitorInspPeriod As PartMonitorInspPeriod In mPartMonitorInspTemp.PartMonitorInspPeriods
                    IsPeriodPresentOnComp = False
                    For Each mCompStatusPeriod As CompStatusPeriod In mCompStatus.CompStatusPeriods
                        If mCompStatusPeriod.PeriodID.Equals(mPartMonitorInspPeriod.PeriodID) Then
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
                    mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mIssueDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType)
                    mCompMonitorInspStatus.PartMonitorInspID(True) = ID
                    Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                    Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
                    RemoveSession()
                    Session("mPartMonitorInsp") = PartMonitorInsp.GetPartMonitorInsp(ID)
                    Session("mIssueDate") = mIssueDate
                    Response.Redirect("wfCompMonitorInspStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    '--------------------------------
                Else
                    mCompMonitorInspStatus.PartMonitorInspID(False) = ID
                    Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                    Session("mCompMonitorInspStatusList") = mCompMonitorInspStatusList
                    If Session("URLForCompInst") Is Nothing Then 'dont remove session as Part Service Count Required on wfCompMonitorServiceStatus_AJAX btnBack.Click
                        RemoveSession()
                    Else
                        'Dim URLForPartInspList As New Stack
                        'URLForPartInspList.Push(Request.Url)
                        'Session("URLForPartInspList") = URLForPartInspList
                        Session("StatusPageOpenFrom") = Request.QueryString("GChildPage2")
                    End If

                    'Code added by Saylee on 14/2008
                    'Response.Redirect(Request.QueryString("GChildPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    Response.Redirect("wfCompMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    '--------------------------------
                End If
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorInsp.PageSize * dgPartMonitorInsp.PageIndex
                Dim ID = mPartMonitorInspList(Index).ID
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                'mPartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(ID)
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
                    'msg1.ReplacePage = "wfPartMonitorInspList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                    'msg1.Show()
                End If

        End Select
    End Sub
    Private Sub hdnBtnInspMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnInspMaster.Click
        DataFieldBind()
        SetPage()
        SetGrid()
        ControlVisibility()
        upnldgGrid.Update()
        upnlActionBtnTop.Update()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        NewRecord()
        '' If (Not User.IsInRole("MachineNew") And mPartMonitorInsp.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mPartMonitorInsp.IsNew) Then
        ''If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
        ''    MarkLog(Util.Action.[New], "PartMonitorInsp", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
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

        If Not Session("URLForCompInst") Is Nothing Then
            Dim URLForCompInst As Stack = CType(Session("URLForCompInst"), Stack)
            Session.Remove("URLForCompInst")
            Response.Redirect(URLForCompInst.Peek.ToString)
        End If

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
    Private Sub dgPartMonitorInsp_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartMonitorInsp.Sorting
        mPartMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorInspList") = mPartMonitorInspList
        dgPartMonitorInsp.DataSource = mPartMonitorInspList
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
        '    MarkLog(Util.Action.Print, "PartMonitorInsp", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        '    msg.Show()
        '    Exit Sub
        'End If

        '**********************************
        Rpt = New crListPartMonitor
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        'Part Monitor Insp List
        ReportDetails.Add(New rptStatus(, 0, , _
        , , , dgPartMonitorInsp.Columns.Item(1).HeaderText, , dgPartMonitorInsp.Columns.Item(2).HeaderText, _
        dgPartMonitorInsp.Columns.Item(3).HeaderText, _
        dgPartMonitorInsp.Columns.Item(4).HeaderText, dgPartMonitorInsp.Columns.Item(5).HeaderText, _
        dgPartMonitorInsp.Columns.Item(6).HeaderText, dgPartMonitorInsp.Columns.Item(7).HeaderText, dgPartMonitorInsp.Columns.Item(8).HeaderText, dgPartMonitorInsp.Columns.Item(9).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mPartMonitorInspList.Count
        Dim I As Integer

        Dim str(8) As String

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

            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If CType(Me.mPartMonitorInspList.Item(I).ShowInCofA, String) <> "&nbsp;" Then str(5) = CType(Me.mPartMonitorInspList.Item(I).ShowInCofA, String)
            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorInsp.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgPartMonitorInsp.Rows(I).Cells.Item(9).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 1, , _
             , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "Part Insp List Report", lblList.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If mPartMonitorInspList.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfPartMonitorInspList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
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
        '  MarkLog(Util.Action.Print, "PartMonitorInsp", "Part Monitor Insp List Report", Util.ErrorType.HandledError, Guid.Empty)
        'End

        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region

End Class