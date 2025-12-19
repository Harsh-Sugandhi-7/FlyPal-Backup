

'AJAX Conversion by Saylee On 03-Jul-2015

Public Class wfAssemblyStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mAssemblyStatusList As tmpAssemblyStatusList
    Public mAssemblyStatus As AssemblyStatus
    Public mAssemblyTypeListForUI As AssemblyTypeListForUI
    Dim EventLogID As Guid 'Added By Utkarsh On 29-Jul-2011 For All19072011
    Dim MachineDetail As String 'Added By Utkarsh On 29-Jul-2011 For All19072011
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), tmpAssemblyStatusList)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyTypeListForUI = Session("mAssemblyTypeListForUI")
        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub SetSession()
        Session("mAssemblyStatusList") = mAssemblyStatusList
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mAssemblyTypeListForUI") = mAssemblyTypeListForUI

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyStatusList")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyTypeListForUI")
        Session.Remove("Edit")
        Session.Remove("mFileAttach")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
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
                                mMachine = Machine.GetMachine(mMachine.ID)
                                Session("mMachine") = mMachine
                                DataFieldBind()
                                SetGrid()
                                upnlGridView.Update()
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
                            MachineDetail = "Reg No. : " & mMachine.RegNo & " Model : " & mAssemblyStatus.ModelName & " Type : " & mAssemblyStatus.AssemblyTypeName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo
                            MarkLog(Util.Action.Delete, "Assembly Status", "Can't delete : " & MachineDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        ''DataFieldBind()
                        ''SetGrid()
                        ''upnlGridView.Update()
                    End If
            End Select
        End If
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean) 'Added By Vikrant On 01-Dec-2014
        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID, 1) 'Sort = 1 - Removal
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded) 'Sort = 2 - Removal
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
    Private Sub SetPage()
        lblAssemblyListInfo.Text = "List of all the Assemblies on " & mMachine.RegNo & " as of " & mMachine.AssemblyStatus.AsOnDateFormatted & ". The Time Since New values of all the assemblies will be as of " & mMachine.AssemblyStatus.AsOnDateFormatted & "."
        lblAssemblyStatusDetails.Text = "List of Assemblies: " & mAssemblyStatusList.Count & " Record(s)found"
    End Sub
    Private Sub ControlVisibility()
        btnAdd.Enabled = Not mMachine.AssemblyStatus.HasLogCount
    End Sub
    Private Sub SetRights() 'Added By Utkarsh On 14-Mar-2011
        If (Not User.IsInRole("MachineAssemblyPrint")) Then
            btnPrint.Enabled = False
            btnPrint.ToolTip = "You are not authorized user"
        End If
        If (User.IsInRole("MachineAssemblyNew")) = False Then
            btnAdd.Enabled = False
            btnAdd.ToolTip = "You are not authorized user"
        End If
    End Sub
    Private Sub SetGrid()
        Dim B As Boolean
        Dim img As New ImageButton
        For j As Integer = 0 To dgAssemblyStatusList.Rows.Count - 1
            B = CType(Me.dgAssemblyStatusList.Rows.Item(j).Cells(14).Text, Boolean)
            If B = False Then
                img = dgAssemblyStatusList.Rows.Item(j).Cells(13).FindControl("View")
                img.Visible = False
                dgAssemblyStatusList.Rows.Item(j).Cells(13).Enabled = False
            End If
        Next
    End Sub 'End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAssemblyStatusList = tmpAssemblyStatusList.GetAssemblyStatusList(mMachine.AssemblyStatus.AsOnDate.ToString, _
                                                                    mMachine.ID, mMachine.AssemblyStatus.IsMaster, MonitoringServiceRequired:=False, MonitoringInspRequired:=False, MonitoringModRequired:=False, CompMonitoringServiceRequired:=False, CompMonitoringInspRequired:=False, CompMonitoringModRequired:=False)
        dgAssemblyStatusList.DataSource = mAssemblyStatusList
        Session("mAssemblyStatusList") = mAssemblyStatusList

        If cmbAdd.SelectedIndex < 0 Then
            mAssemblyTypeListForUI = AssemblyTypeListForUI.GetAssemblyTypeListForUI()
            cmbAdd.DataSource = mAssemblyTypeListForUI
            Session("mAssemblyTypeListForUI") = mAssemblyTypeListForUI
        End If
        DataBind()
    End Sub
    Private Sub GridBind()
        dgAssemblyStatusList.DataSource = mAssemblyStatusList
        dgAssemblyStatusList.DataBind()
        SetGrid()
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 29-Jul-2011 For All19072011
        If Not IsPostBack Then
            If cmbAdd.Enabled = True Then
                setFocus(cmbAdd)
            End If
            DataFieldBind()
            SetPage()
            ControlVisibility()
            SetRights()  'Added By Utkarsh On 14-Mar-2011
            SetGrid()
        End If
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        mAssemblyStatus = AssemblyStatus.NewAssemblyStatus(Guid.NewGuid, mMachine.ID, mAssemblyTypeListForUI(CType(cmbAdd.SelectedIndex, Int32)).ID, CType(mMachine.AssemblyStatus.AsOnDate, String))
        MarkLog(Util.Action.[New], "Assembly Status", "", Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        SetGrid()
        upnlGridView.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)

        'Response.Redirect("wfAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfAssemblyStatusList_Ajax.aspx")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenOverFrame", "OpenOverFrame();", True)

    End Sub
    Private Sub dgAssemblyStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAssemblyStatusList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("MachineAssemblyView") And Not User.IsInRole("MachineAssemblyEdit")) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                'Dim index As Integer = CInt(e.CommandArgument) + dgAssemblyStatusList.PageIndex * dgAssemblyStatusList.PageSize
                'Dim mID As Guid = mAssemblyStatusList(index).ID
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mMachine") = mMachine
                'Changed By Utkarsh On 29-Jul-2011 For All19072011
                MachineDetail = "Reg No. : " & mMachine.RegNo & " Model : " & mAssemblyStatus.ModelName & " Type : " & mAssemblyStatus.AssemblyTypeName & " Serial No. : " & mAssemblyStatus.Assembly.SerialNo
                MarkLog(Util.Action.Edit, "Assembly Status", MachineDetail, Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
                'End
                SetGrid()
                Session("Edit") = True
                ' Response.Redirect("wfAssemblyStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfAssemblyStatusList_Ajax.aspx")
                'Response.Write("<script>window.open('wfAssemblyStatus.aspx','_parent');</script>")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenOverFrame", "OpenOverFrame();", True)

            Case "DeleteRec"
                If (Not User.IsInRole("MachineAssemblyNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineAssemblyDelete") And Not mMachine.IsNew) Then
                    GridBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                ' Dim index As Integer = CInt(e.CommandArgument) + dgAssemblyStatusList.PageIndex * dgAssemblyStatusList.PageSize
                'Dim mID As Guid = mAssemblyStatusList(index).ID
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                GridBind()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

                mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mID)
                Session("mAssemblyStatus") = mAssemblyStatus
            Case "ViewRec"
                'Dim Index As Integer = CInt(e.CommandArgument) + dgAssemblyStatusList.PageSize * dgAssemblyStatusList.PageIndex
                'Dim mID As Guid = mAssemblyStatusList(Index).ID
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                Dim mIsAttachemntAdded As Boolean = mAssemblyStatusList(mID).IsAttachmentAdded
                SetGrid()
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub
    Private Sub dgAssemblyStatusList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAssemblyStatusList.Sorting
        mAssemblyStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyStatusList") = mAssemblyStatusList
        GridBind()
        SetGrid()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "Assembly Status", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Changed By Utkarsh On 29-Jul-2011 For All19072011
        Session.Remove("Add")
        RemoveSession()
        'Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
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
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        GridBind()
        Rpt = New CrList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        'For Detail Section
        ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Reg No.", _
           Me.mMachine.RegNo, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft" + "  " + Me.mMachine.AssemblyStatus.AsOnDateFormatted, _
           "Periods", "Value"))

        Dim TotalCount As Integer
        TotalCount = Me.mMachine.AssemblyStatus.AssemblyStatusPeriods.Count
        Dim I As Integer

        For I = 0 To TotalCount - 1
            If I = 0 Then
                ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Owner",
                       Me.mMachine.Owner, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft",
                       CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
            ElseIf I = 1 Then
                'ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Model", _
                '                          Me.mMachine.AssemblyStatus.Assembly.ModelName, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
                '                          CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String)))

            ElseIf I = 2 Then
                ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Model",
                                          Me.mMachine.AssemblyStatus.Assembly.ModelName, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft",
                                          CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String)))

                ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Serial No",
                                      Me.mMachine.AssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft",
                                     "", ""))
            Else
                ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "",
                                       "", , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft",
                                       CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValueFormatted, String)))
            End If
        Next

        'For Assembly List Caption
        ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , , , lblAssemblyListInfo.Text))


        'For Assembly Status List
        ReportDetails.Add(New rptStatus(, 2, , , _
       , , dgAssemblyStatusList.Columns.Item(1).HeaderText, , dgAssemblyStatusList.Columns.Item(2).HeaderText, dgAssemblyStatusList.Columns.Item(3).HeaderText, _
       dgAssemblyStatusList.Columns.Item(4).HeaderText, dgAssemblyStatusList.Columns.Item(5).HeaderText, _
        dgAssemblyStatusList.Columns.Item(6).HeaderText, dgAssemblyStatusList.Columns.Item(7).HeaderText, dgAssemblyStatusList.Columns.Item(8).HeaderText, _
        dgAssemblyStatusList.Columns.Item(9).HeaderText, , dgAssemblyStatusList.Columns.Item(10).HeaderText))

        Dim TotalCount1 As Integer
        TotalCount1 = Me.mAssemblyStatusList.Count
        Dim m As Integer
        Dim str(9) As String
        For m = 0 To TotalCount1 - 1
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
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(1).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(2).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(3).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(4).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(5).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(6).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(7).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(8).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(9).Text.Replace("<BR>", vbCrLf)
            If Me.dgAssemblyStatusList.Rows(m).Cells.Item(10).Text <> "&nbsp;" Then str(9) = Me.dgAssemblyStatusList.Rows(m).Cells.Item(10).Text.Replace("<BR>", vbCrLf)

            ReportDetails.Add(New rptStatus(, 3, , , , , str(0), , _
                          str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8), , str(9)))
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        " Assembly List Report", "All the Assembly data is as on " & Me.mMachine.AssemblyStatus.AsOnDateFormatted, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        Dim mRptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mRptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
#End Region
#End Region

End Class