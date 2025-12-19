
'AJAX Conversion By Saylee on 24-Jun-2015

Public Class wfMachineTankList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMachine As Machine
    Public mTankList As TankList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mTankList = CType(Session("mTankList"), TankList)
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mTankList") = mTankList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mTankList")
        Session.Remove("mTank")
    End Sub
    Private Sub NewRecord()
        Dim mTank As Tank
        mTank = Tank.NewTank(Guid.NewGuid)
        Session("mTank") = mTank
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0


        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mMachine.MachineTanks.Remove(mMachine.MachineTanks(mMachine.MachineTanks.CurrentIndex))
                            Session("mMachine") = mMachine
                            DataFieldBind()
                            SetPage()
                            upnlGridTankList.Update()
                            ' Response.Redirect("wfMachineTankList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8114 Or ex.Number = 8115 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'MarkLog(Util.Action.Delete, "Machine", "Aircraft Name ->" + mMachine.RegNo + " Aircraft Tank ->  " + mMachine.MachineTanks.Item(mTankList.CurrentIndex).TankName, Util.ErrorType.NoError, mMachine.MachineTanks.Item(mTankList.CurrentIndex).TankID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'Response.Redirect("wfMachineTankList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    ' Response.Redirect("wfMachineTankList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    '  Response.Redirect("wfMachineTankList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            'Response.Redirect("wfMachineTankList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '  DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        'If mMachine.IsNew Then
        '    lblTitle.Text = "Aircraft [New]"
        'Else
        '    lblTitle.Text = "Aircraft [" & mMachine.RegNo & "]"
        'End If
        lblResult.Text = "List of Tanks: " & mMachine.MachineTanks.Count & " Record(s)found"
    End Sub
    Private Sub ControlVisibility()
        'enabledisable buttons
        'btnAdd.Enabled = Not mMachine.AssemblyStatus.HasLogCount

        'Comment opened by Saylee on 7-Aug-2009
        dgTankList.Columns(2).Visible = Not mMachine.AssemblyStatus.HasLogCount

    End Sub
#End Region

#Region " Data Binding "
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbTankList" Then
            If cmbTankList.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Select Tanks form List."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        mTankList = TankList.GetTankList(, "(SELECT)")
        cmbTankList.DataSource = mTankList
        Session("mTankList") = mTankList
        dgTankList.DataSource = mMachine.MachineTanks
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the  page here
        GetSession()
        If Not IsPostBack Then
            If cmbTankList.Enabled = True Then
                setFocus(cmbTankList)
            End If
            DataFieldBind()
            SetPage()
            ControlVisibility()
        End If
       

    End Sub
    Private Sub hdnBtnTankMaster_Click(sender As Object, e As System.EventArgs) Handles hdnBtnTankMaster.Click
        DataFieldBind()
        upnlTank.Update()
        upnlGridTankList.Update()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            SetSession()
            'MarkLog(Util.Action.[New], "Machine", "", Util.ErrorType.NoError, mMachine.ID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            ''msg.ReplacePage = "wfMachineTankList.aspx?BackPage=" & Request.QueryString("BackPage")
            'msg.ReplacePage = "wfMachineTankList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        If Not IsValid Then upnlValidationSummary.Update() : ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True) : Exit Sub

        Dim TankID As New Guid(cmbTankList.SelectedValue.ToString)
        If mMachine.MachineTanks.Contains(TankID, "") = False Then
            'MarkLog(Util.Action.[New], "Machine", "Tank ->  " + cmbTankList.SelectedItem.Text, Util.ErrorType.NoError, TankID)
            mMachine.MachineTanks.Add(mMachine.ID, TankID)
            Session("mMachine") = mMachine
            DataFieldBind()
            SetPage()

            ControlVisibility()
            upnlGridTankList.Update()
            upnlTank.Update()
            'upnlTitle.Update()

            'Response.Redirect("wfMachineTankList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Else
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Tank already exists, can not be added.", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfMachineTankList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
            'Session("sender") = "Delete"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Tank already exists, can not be added.", MsgBoxStyle.OkOnly, "Delete")

        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
    End Sub
    Private Sub dgTankList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTankList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTankList.PageSize * dgTankList.PageIndex

                'If (Not User.IsInRole("MachineDelete")) Then
                If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
                    'MarkLog(Util.Action.Delete, "Machine", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfMachineTankList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")

                    Exit Sub
                End If
                DataFieldBind()
                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                'msg.ReplacePage = "wfMachineTankList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                'Session("sender") = "Delete"
                'msg.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

                mMachine.MachineTanks.CurrentIndex = Index
                Session("mMachine") = mMachine

        End Select
    End Sub
    Private Sub imgbtnTank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnTank.Click
        NewRecord()
        '  Response.Redirect("wfTank.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=wfMachineTankList.aspx")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentTankMasterFunction", "CallParentTankMasterFunction();", True)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTankMasterWindow", "OpenTankMasterWindow();", True)

    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        'MarkLog(Util.Action.Close, "Machine", "", Util.ErrorType.NoError, Guid.Empty)
        RemoveSession()

        '  Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
    End Sub
   
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
   
#End Region

#Region " Report "
    'Created By :- Jyoti
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

    '#Region "Event"
    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

    'If (Not User.IsInRole("MachinePrint")) Then
    '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
    '            msg.ReplacePage = "wfMachineTankList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
    '            msg.Show()
    '            Exit Sub
    '        End If




    '    Rpt = New crListAssemblyStatus
    '    Dim da As New CSLA.Data.ObjectAdapter
    '    Dim ds As New dsCommon
    '    Dim ReportDetails As New rptStatusList

    '    'For Detail Section
    '    ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Reg No.", _
    '       Me.mMachine.RegNo, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft" + "  " + Me.mMachine.AssemblyStatus.AsOnDate, _
    '       "Periods", "Value"))

    '    Dim TotalCount As Integer
    '    TotalCount = Me.mMachine.AssemblyStatus.AssemblyStatusPeriods.Count
    '    Dim I As Integer

    '    For I = 0 To TotalCount - 1
    '        If I = 0 Then
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Manufacturer", _
    '                   Me.mMachine.Owner, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                   CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        ElseIf I = 1 Then
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Model", _
    '                                      Me.mMachine.AssemblyStatus.Assembly.ModelName, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                                      CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        ElseIf I = 2 Then
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "Serial No", _
    '                                  Me.mMachine.AssemblyStatus.Assembly.SerialNo, , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                                  CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        Else
    '            ReportDetails.Add(New rptStatus(, 0, "Airframe Details", "", _
    '                                   "", , , , , , , , , , , , , , , , , "Times Since New Values of Aircraft", _
    '                                   CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).PeriodName, String), CType(Me.mMachine.AssemblyStatus.AssemblyStatusPeriods(I).AssemblyCurrentValue, String)))
    '        End If
    '    Next

    '    'For Assembly List Caption
    '    ReportDetails.Add(New rptStatus(, 1, , , , , , , , , , , , , , , lblAssemblyListInfo.Text))


    '    'For Assembly Status List
    '    ReportDetails.Add(New rptStatus(, 2, , , _
    '   , , dgAssemblyStatusList.Columns.Item(1).HeaderText, , dgAssemblyStatusList.Columns.Item(2).HeaderText, dgAssemblyStatusList.Columns.Item(3).HeaderText, _
    '   dgAssemblyStatusList.Columns.Item(4).HeaderText, dgAssemblyStatusList.Columns.Item(5).HeaderText, _
    '    dgAssemblyStatusList.Columns.Item(6).HeaderText, dgAssemblyStatusList.Columns.Item(7).HeaderText, dgAssemblyStatusList.Columns.Item(8).HeaderText, _
    '    dgAssemblyStatusList.Columns.Item(9).HeaderText))

    '    Dim TotalCount1 As Integer
    '    TotalCount1 = Me.mAssemblyStatusList.Count
    '    Dim m As Integer
    '    Dim str(8) As String
    '    For m = 0 To TotalCount1 - 1
    '        str(0) = ""
    '        str(1) = ""
    '        str(2) = ""
    '        str(3) = ""
    '        str(4) = ""
    '        str(5) = ""
    '        str(6) = ""
    '        str(7) = ""
    '        str(8) = ""
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgAssemblyStatusList.Items(m).Cells.Item(1).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgAssemblyStatusList.Items(m).Cells.Item(2).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgAssemblyStatusList.Items(m).Cells.Item(3).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgAssemblyStatusList.Items(m).Cells.Item(4).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgAssemblyStatusList.Items(m).Cells.Item(5).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgAssemblyStatusList.Items(m).Cells.Item(6).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgAssemblyStatusList.Items(m).Cells.Item(7).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgAssemblyStatusList.Items(m).Cells.Item(8).Text
    '        If Me.dgAssemblyStatusList.Items(m).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgAssemblyStatusList.Items(m).Cells.Item(9).Text

    '        ReportDetails.Add(New rptStatus(, 3, , , , , str(0), , _
    '                      str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8)))
    '    Next

    '    mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
    '    Dim Report As New ReportData(mCompanyDetail.CompanyName, _
    '    mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
    '    mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
    '    " Assembly List Report", "All the Assembly data is as on " & Me.mMachine.AssemblyStatus.AsOnDate, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))
    '    da.Fill(ds, ReportDetails)
    '    da.Fill(ds, Report)
    '    Rpt.SetDataSource(ds)
    '    Session("CrystalReport") = Rpt

    '    Dim Str1 As String
    '    Str1 = "<script language=Javascript>openTranDetail();</script>"
    '     ClientScript.RegisterStartupScript(Me.GetType(),"openTranDetail", Str1)
    'End Sub
    '#End Region



#End Region

   
   
  
End Class