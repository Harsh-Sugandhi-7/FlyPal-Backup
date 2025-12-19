'AJAX Created By: Saylee on 12-May-2015

Public Class wfPartMonitorServiceList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatus As AssemblyStatus
    Public mCompStatus As CompStatus
    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Public mPartMonitorServiceList As PartMonitorServiceList
    Public mPartMonitorService As PartMonitorService
    Public mCompMonitorServiceStatusList As tmpCompMonitorServiceStatusList
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
        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        mPartMonitorServiceList = CType(Session("mPartMonitorServiceList"), PartMonitorServiceList)
        mIssueDate = Session("mIssueDate")
        mCompMonitorServiceStatusList = CType(Session("mCompMonitorServiceStatusList"), tmpCompMonitorServiceStatusList)
        mIsSpareComp = Session("IsSpareComp") 'Added by Shital on 30-Sep-2020 for SpareComp
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
        Session("mIssueDate") = mIssueDate
        Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPartMonitorServiceList")
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
        Dim mPartMonitorService As PartMonitorService
        Dim ID As Guid = Guid.NewGuid 'Revise Activity
        ' mPartMonitorService = PartMonitorService.NewPartMonitorService(ID, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mMachine.HourType, ID)
        If mIsSpareComp = False Then
            mPartMonitorService = PartMonitorService.NewPartMonitorService(ID, mCompStatus.Comp.PartID, mAssemblyModelID, mHourType, ID)
        Else
            mPartMonitorService = PartMonitorService.NewPartMonitorService(ID, mCompStatus.Comp.PartID, mAssemblyModelID, mCompStatus.HourType, ID)
        End If
        ' RemoveSession()
        Session("mPartMonitorService") = mPartMonitorService

        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.[New], "Part Service", "", Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
        'End
        Dim GChildPage2, GChildPage4, GChildPage5, GChildPage6 As String '= Trim(Request.QueryString("GChildPage4"))
        GChildPage2 = Trim(Request.QueryString("GChildPage2"))
        GChildPage4 = Trim(Request.QueryString("GChildPage4"))
        GChildPage5 = Trim(Request.QueryString("GChildPage5"))
        GChildPage6 = Trim(Request.QueryString("GChildPage6"))


        'Response.Redirect("wfPartMonitorService_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=wfPartMonitorServiceList_AJAX.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow('" + GChildPage2 + "','" + GChildPage4 + "','" + GChildPage5 + "','" + GChildPage6 + "');", True)
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfPartMonitorServiceList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        'Session("sender") = "Delete"
        'msg1.Show()
        SetPage()
        SetGrid()
        upnldgGrid.Update()

        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPartMonitorServiceList.CurrentIndex = Index
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        Dim mPartMonitorService As PartMonitorService
        mPartMonitorService = PartMonitorService.GetPartMonitorService(mId, mMachine.HourType)
        Session("mPartMonitorService") = mPartMonitorService

        'Added By Utkarsh On 26-Jul-2011 For All19072011
        'MaintDetail = "Monitor Service Type : " + mPartMonitorService.PartMonitorServiceTypeName + " Description : " + mPartMonitorService.Description
        MaintDetail = "Part : " & mCompStatus.PartNameSerialNo & " Part Modification Type : " & mPartMonitorService.PartMonitorServiceTypeName & " Description : " & mPartMonitorService.Description
        MarkLog(Util.Action.Edit, "Part Service", MaintDetail, Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
        'End
        ' RemoveSession()
        SetGrid()
        upnldgGrid.Update()
        'Response.Redirect("wfPartMonitorService_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=wfPartMonitorServiceList_AJAX.aspx")
        Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow('" + GChildPageTmp + "');", True)
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
                            MaintDetail = "Service Type : " + mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).PartMonitorServiceTypeName + " Description : " + mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).Description
                            Session("sender") = ""
                            mId = mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).ID
                            If mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).IsAttachmentAdded Then
                                mFileAttach = FileAttach.GetAttachment(mPartMonitorServiceList(mPartMonitorServiceList.CurrentIndex).ID)
                            End If
                            PartMonitorService.DeletePartMonitorService(mPartMonitorServiceList.CurrentItem.id)
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
                            ' Response.Redirect("wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                        Catch ex As SqlException
                            If ex.Number = 8114 Or ex.Number = 8115 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Part Service", "Can't delete : " + MaintDetail + " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)

                                'Added by saylee on 1-Jun-2016
                                Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
                                mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).PartID, mPartMonitorServiceList.Item(mPartMonitorServiceList.CurrentIndex).ID.ToString)

                                If mPartMonitorServiceConfiguredList.Count > 0 Then
                                    Dim SerialNos As String = String.Empty

                                    For i As Integer = 0 To mPartMonitorServiceConfiguredList.Count - 1
                                        If i = mPartMonitorServiceConfiguredList.Count - 1 Then
                                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo
                                        Else
                                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo + ","
                                        End If
                                    Next

                                    MSGBoxCtrl.Show("Deletion Alert!", "Selected Service is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be deleted", "To delete master record please delete all configured status first", MsgBoxStyle.OkOnly, "")

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

                                MarkLog(Util.Action.Delete, "Part Service", MaintDetail, Util.ErrorType.NoError, mId, EventLogID)
                                'End
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    ' Response.Redirect("wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    'Response.Redirect("wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub DataFieldBind()
        ' mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(mCompStatus.Comp.PartID, mAssemblyModelID)
        mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(mCompStatus.Comp.PartID, Guid.Empty) 'Pass Blank ModelID as it was Commenetd in SP
        dgPartMonitorService.DataSource = mPartMonitorServiceList
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
        DataBind()
    End Sub
    Private Sub SetPage()
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD"
        Else
            ServiceMPDTitle = "Service"
        End If

        lblList.Text = "Part " + ServiceMPDTitle + " List of - [ " & "Part: " & mCompStatus.Comp.PartName & " ]"
        lbldgGridResult.Text = "List Of Part " + ServiceMPDTitle + ": " & mPartMonitorServiceList.Count & " Record(s)"
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = (mPartMonitorServiceList.Count > 0)
        btnPrintTop.Visible = (mPartMonitorServiceList.Count > 15)
        btnAddNewTop.Visible = (mPartMonitorServiceList.Count > 15)
        btnBackTop.Visible = (mPartMonitorServiceList.Count > 15)
    End Sub
    Private Sub SetGrid()

        Dim P As Boolean

        For j As Integer = 0 To dgPartMonitorService.Rows.Count - 1
            P = CType(Me.dgPartMonitorService.Rows.Item(j).Cells(14).Text, Boolean)

            If P = False Then
                Me.dgPartMonitorService.Rows.Item(j).Cells(13).Enabled = False
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
        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD(s)"
            dgPartMonitorService.HeaderRow.Cells(1).Text = "Task No."
            dgPartMonitorService.Columns(1).HeaderText = "Task No."
        Else
            ServiceMPDTitle = "Services"
            dgPartMonitorService.HeaderRow.Cells(1).Text = "Code/Form No."
            dgPartMonitorService.Columns(1).HeaderText = "Code/Form No."
        End If
    End Sub
    Private Sub dgPartMonitorService_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartMonitorService.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorService.PageSize * dgPartMonitorService.PageIndex
                Dim ID = mPartMonitorServiceList(Index).ID
                'Commneted By Utkarsh On 25-Mar-2011

                'If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                '    MarkLog(Util.Action.Edit, "PartMonitorService", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                '    msg.Show()
                '    Exit Sub
                'End If

                '************************************
                EditRecord(ID)
            Case "DeleteRec"
                '' If (Not User.IsInRole("MachineDelete")) Then
                ''If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                ''    MarkLog(Util.Action.Delete, "PartMonitorService", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorService.PageSize * dgPartMonitorService.PageIndex
                DeleteRecord(Index)
            Case "Select"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorService.PageSize * dgPartMonitorService.PageIndex
                Dim ID = mPartMonitorServiceList(Index).ID
                'Added By Vikrant on 03-Feb-2020 For Poonawalla Issue solve
                Dim mPartMonitorServiceTemp As PartMonitorService
                Dim IsPeriodPresentOnComp As Boolean
                mPartMonitorServiceTemp = PartMonitorService.GetPartMonitorService(mPartMonitorServiceList(Index).ID)
                For Each mPartMonitorServicePeriod As PartMonitorServicePeriod In mPartMonitorServiceTemp.PartMonitorServicePeriods
                    IsPeriodPresentOnComp = False
                    For Each mCompStatusPeriod As CompStatusPeriod In mCompStatus.CompStatusPeriods
                        If mCompStatusPeriod.PeriodID.Equals(mPartMonitorServicePeriod.PeriodID) Then
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
                    MSGBoxCtrl.Show("Alert!", "Selected Maintenace Activity Period(s) not present on Component", "Kindly select different maintenance activity", MsgBoxStyle.OkOnly, "DiffPeriodAlert")
                    Exit Sub
                End If
                'End
                If Session("NewPage") = "True" Then

                    'Saylee 29-Sep-2008
                    mIssueDate = Session("mIssueDate")
                    mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mAssemblyStatus.ID, mIssueDate, mCompStatus.Comp.PartID, mAssemblyStatus.Assembly.ModelID, mCompStatus.ID, mMachine.HourType, mCompStatus)
                    mCompMonitorServiceStatus.PartMonitorServiceID(True) = ID
                    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                    Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
                    RemoveSession()

                    Session("mPartMonitorService") = PartMonitorService.GetPartMonitorService(ID)  'Added By Saylee on 11-Jul-2018, REM:- Used for getting only record for change in DoneOnDate, then need to get Current values as per DoneOnDate
                    Session("mIssueDate") = mIssueDate
                    Response.Redirect("wfCompMonitorServiceStatusNew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
                    '--------------------------------
                Else
                    mCompMonitorServiceStatus.PartMonitorServiceID(False) = ID
                    Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                    Session("mCompMonitorServiceStatusList") = mCompMonitorServiceStatusList
                    If Session("URLForCompInst") Is Nothing Then 'dont remove session as Part Service Count Required on wfCompMonitorServiceStatus_AJAX btnBack.Click
                        RemoveSession()
                    Else
                        Session("StatusPageOpenFrom") = Request.QueryString("GChildPage2")
                        ''Dim URLForPartServiceList As New Stack
                        ''URLForPartServiceList.Push(Request.Url)
                        'Session("URLForPartServiceList") = URLForPartServiceList
                    End If


                    'Code added by Saylee on 14/2008
                    'Response.Redirect(Request.QueryString("GChildPage5") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    Response.Redirect("wfCompMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4"))
                    '--------------------------------
                End If
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartMonitorService.PageSize * dgPartMonitorService.PageIndex
                Dim ID = mPartMonitorServiceList(Index).ID
                '----------------------------------------------------------------------
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                '----------------------------------------------------------------------
                'mPartMonitorService = PartMonitorService.GetPartMonitorService(ID)
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
                    'msg1.ReplacePage = "wfPartMonitorServiceList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
                    'msg1.Show()
                End If

        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        NewRecord()
        '' If (Not User.IsInRole("MachineNew") And mPartMonitorService.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mPartMonitorService.IsNew) Then
        ''If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
        ''    MarkLog(Util.Action.[New], "PartMonitorService", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        ''    Session("sender") = "Authorization"
        ''    msg.Show()
        ''    Exit Sub
        ''End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnSeriviceMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSeriviceMaster.Click
        DataFieldBind()
        SetPage()
        SetGrid()
        ControlVisibility()
        upnlActionBtnTop.Update()
        upnldgGrid.Update()
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
            Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
        End If
    End Sub
    Private Sub dgPartMonitorService_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartMonitorService.Sorting
        mPartMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartMonitorServiceList") = mPartMonitorServiceList
        dgPartMonitorService.DataSource = mPartMonitorServiceList
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
        '    MarkLog(Util.Action.Print, "PartMonitorService", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
        '    msg.Show()
        '    Exit Sub
        'End If

        '**********************************
        Rpt = New crListPartMonitor
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        Dim Task_HeaderName As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            Task_HeaderName = "Task No"
        Else
            Task_HeaderName = "Code/ Form No"
        End If

        'Part Service List
        'ReportDetails.Add(New rptStatus(, 0, ,
        ', , , dgPartMonitorService.Columns.Item(1).HeaderText, , dgPartMonitorService.Columns.Item(2).HeaderText,
        'dgPartMonitorService.Columns.Item(3).HeaderText,
        'dgPartMonitorService.Columns.Item(4).HeaderText, dgPartMonitorService.Columns.Item(5).HeaderText,
        'dgPartMonitorService.Columns.Item(6).HeaderText, dgPartMonitorService.Columns.Item(7).HeaderText,
        'dgPartMonitorService.Columns.Item(8).HeaderText, dgPartMonitorService.Columns.Item(9).HeaderText))

        ReportDetails.Add(New rptStatus(, 0, ,
        , , , Task_HeaderName, , dgPartMonitorService.Columns.Item(2).HeaderText,
        dgPartMonitorService.Columns.Item(3).HeaderText,
        dgPartMonitorService.Columns.Item(4).HeaderText, dgPartMonitorService.Columns.Item(5).HeaderText,
        dgPartMonitorService.Columns.Item(9).HeaderText, dgPartMonitorService.Columns.Item(8).HeaderText))

        Dim TotalCount As Integer
        TotalCount = Me.mPartMonitorServiceList.Count
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

            If Me.dgPartMonitorService.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgPartMonitorService.Rows(I).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorService.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgPartMonitorService.Rows(I).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorService.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgPartMonitorService.Rows(I).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorService.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgPartMonitorService.Rows(I).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorService.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgPartMonitorService.Rows(I).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            '  If CType(Me.mPartMonitorServiceList.Item(I).ShowInCofA, String) <> "&nbsp;" Then str(5) = CType(Me.mPartMonitorServiceList.Item(I).ShowInCofA, String)
            '  If Me.dgPartMonitorService.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgPartMonitorService.Rows(I).Cells.Item(7).Text.Replace("<BR>", vbCrLf)

            If Me.dgPartMonitorService.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(6) = Me.dgPartMonitorService.Rows(I).Cells.Item(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgPartMonitorService.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgPartMonitorService.Rows(I).Cells.Item(8).Text.Replace("<BR>", vbCrLf)

            'ReportDetails.Add(New rptStatus(, 1, ,
            ' , , , str(0), , str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8)))
            ReportDetails.Add(New rptStatus(, 1, ,
             , , , str(0), , str(1), str(2), str(3), str(4), str(6), str(7)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
   mCompanyDetail.WebSite, "Part Service List Report", lblList.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If mPartMonitorServiceList.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfPartMonitorServiceList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5")
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
        '  MarkLog(Util.Action.Print, "PartMonitorService", "Part Service List Report", Util.ErrorType.HandledError, Guid.Empty)
        'End

        'Dim Str1 As String
        'Str1 = "<script language=Javascript>openTranDetail();</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

#End Region



End Class