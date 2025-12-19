
'AJAX Conversion by Saylee On 08-Jul-2015

Public Class wfMachineList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mtmpMachineList As tmpMachineList
    Public mMachine As Machine
    Public Sortfield As String
    Public SortFlag As Boolean
    Public Idx As Integer
    Public SearchForText As String
    Dim Index As Int32

    Dim EventLogID As Guid 'Added by Saylee on 22-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mtmpMachineList = CType(Session("mtmpMachineList"), tmpMachineList)
        mMachine = CType(Session("mMachine"), Machine)
        SortFlag = CType(Session("SortFlag"), Boolean)
        Idx = CType(Session("Idx"), Integer)
        SearchForText = CType(Session("SearchForText"), String)
        Session("NewPage") = "False"
    End Sub
    Private Sub SetSession()
        Session("mtmpMachineList") = mtmpMachineList
        Session("mMachine") = mMachine
        Session("SortFlag") = SortFlag
        Session("Idx") = Idx
        Session("SearchForText") = SearchForText
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mtmpMachineList")
        Session.Remove("mMachine")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfMachineList_Ajax.aspx?" Then
            Session.Remove("mtmpMachineList")
            Session.Remove("mMachine")
            Session.Remove("mUnitList")
            Session.Remove("SortFlag")
            Session.Remove("Idx")
            Session.Remove("SearchForText")
            Session.Remove("mAssemblyStatusList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById ('" + cntrl.ClientID + "').focus(); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Function DecryptData(ByVal plaintext As String) As String
        Dim wrapper As New Simple3Des("FlyPal")
        Dim cipherText As String = wrapper.DecryptData(plaintext)
        Return cipherText
    End Function
    Private Sub SetGrid()
        Dim IsForInventory As Boolean = False
        Dim PlainText As String = String.Empty
        Dim P As Boolean
        For j As Integer = 0 To dgMachineList.Rows.Count - 1
            PlainText = DecryptData(Me.dgMachineList.Rows.Item(j).Cells(15).Text)
            IsForInventory = CBool(PlainText.Split("$$")(0))
            P = CType(Me.dgMachineList.Rows.Item(j).Cells(14).Text, Boolean)
            'If (P = True) Or (IsForInventory) Then
            '    dgMachineList.Rows.Item(j).Cells(12).Enabled = False
            'End If

            'Added by Saylee on 9-Aug-2018 for ALL09082018
            If P = True Then
                Me.dgMachineList.Rows.Item(j).BackColor = Color.OrangeRed
                Me.dgMachineList.Rows.Item(j).ToolTip = "ReadOnly Aircraft"
                Me.dgMachineList.Rows.Item(j).ForeColor = Color.White
            End If
            '********************************************************

            dgMachineList.Rows.Item(j).Cells(12).Enabled = IIf(P, False, IIf(IsForInventory = False And PlainText.Split("$$")(2).Equals(dgMachineList.Rows.Item(j).Cells(1).Text), True, False))
        Next
    End Sub
    '----Added by Vikrant on 16-12-2011  FOR ALL14122011-2------------
    Private Sub ControlVisibilityForGrid()
        'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
            dgMachineList.Columns.Item(10).HeaderText = "Landings/" & "</BR>" & "Flights/" & "</BR>" & "RINS "
        Else
            dgMachineList.Columns.Item(10).HeaderText = "Landings/" & "</BR>" & "Cycles/" & "</BR>" & "RINS/ " '& "</BR>" & "NGCycles/" & "</BR>" & "NFCycles "
        End If
        'dgMachineList.DataBind()
    End Sub
    '-------------------------------------------------------------------
    Private Sub FindNow(Optional ByVal mMachineCategoryName As String = "", _
                        Optional ByVal mRegNo As String = "", _
                        Optional ByVal mModelName As String = "", _
                        Optional ByVal mManufacturerName As String = "", _
                        Optional ByVal mOwnerName As String = "", _
                        Optional ByVal mAddTopItem As String = "")
        'clear the obj and grid for new search
        mMachine = Nothing
        dgMachineList.DataSource = Nothing
        'get the new list
        mtmpMachineList = tmpMachineList.GetMachineList(mMachineCategoryName, mRegNo, mModelName, mManufacturerName, mOwnerName, mAddTopItem)
        'bind the list to the grid
        dgMachineList.DataSource = mtmpMachineList
        ControlVisibilityForGrid()
        Session("mtmpMachineList") = mtmpMachineList
    End Sub
    'Private Sub CallFindNow(ByVal Index As Int32)
    '    Dim SearchText As String = Trim(txtFor.Text)
    '    Select Case Index
    '        Case 0  'All
    '            FindNow()
    '        Case 1  'RegNo.
    '            FindNow("", SearchText, "", "", "")
    '        Case 2  'Model Name
    '            FindNow("", "", SearchText, "", "", "")
    '        Case 3  'Manufacturer
    '            FindNow("", "", "", SearchText, "", "")
    '    End Select
    'End Sub
    Private Sub CallFindNow(ByVal Index As Int32)
        ' Dim SearchText As String = Trim(txtFor.Text)
        Select Case Index
            Case 0  'All
                FindNow()
            Case 1  'RegNo.
                FindNow("", txtFor.Text.Trim, "", "", "")
            Case 2  'Model Name
                FindNow("", "", txtFor.Text.Trim, "", "", "")
            Case 3  'Manufacturer
                FindNow("", "", "", txtFor.Text.Trim, "", "")
        End Select

    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim RegNo As String
                        Try
                            Dim mMachine As Machine
                            Session("sender") = ""
                            mMachine = CType(Session("mMachine"), Machine)
                            RegNo = mMachine.RegNo
                            Machine.DeleteMachine(mMachine.ID)
                            'mMachine.Delete()
                            'mMachine.Save()
                            DatafieldBind()
                            SetControl()
                            SetPage()
                            SetRights()
                            SetGrid()
                            btnAddNew.Enabled = AllowNewAircraft()
                            btnAddNewTop.Enabled = AllowNewAircraft()

                            upnlGrid.Update()
                            upnlResult.Update()
                            'Response.Redirect("wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                '   MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MSGBoxCtrl.show("Reference ! ", "Record cannot be deleted as its already used in Assembly/ Component Inspections,Services or Directives", "", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "Aircraft", "Can't delete : " + RegNo + " is Currently in use", Util.ErrorType.NoError, mMachine.ID, EventLogID)
                            End If
                            DatafieldBind()
                            SetControl()
                            SetPage()
                            SetRights()
                            SetGrid()
                            btnAddNew.Enabled = AllowNewAircraft()
                            btnAddNewTop.Enabled = AllowNewAircraft()

                            upnlGrid.Update()
                            upnlResult.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Aircraft", RegNo, Util.ErrorType.NoError, mMachine.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    ' Response.Redirect("wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    ' Response.Redirect("wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    '   Response.Redirect("wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetPage()
        lblResult.Text = "List of Aircraft as per criteria: " & mtmpMachineList.Count & " Record(s) found."
    End Sub
    Private Sub DisplayControls(ByVal Index As Integer)
        txtFor.Visible = IIf(Index <> 0, True, False)
        lblFor.Visible = IIf(Index <> 0, True, False)
    End Sub
    Private Sub SetControl()
        Index = Session("Idx")
        txtFor.Text = Session("SearchForText")
        'CallFindNow(Index)
        ControlVisibilityForGrid()
        dgMachineList.DataBind()
        DisplayControls(Idx)
    End Sub
    Private Function AllowNewAircraft() As Boolean
        Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
        'Commeneted By Saylee on 10-Feb-2014 for ALL10022014
        '''''Dim mtmpMachineList As MachineList = MachineList.GetMachineList()
        ''''If mtmpMachineList.Count >= mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
        ''''    ''MessageBox.Show("This version does not supports more than " & mCheck.Number("Aircraft").ToString & " Aircrafts", "Version 1.0", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ''''    Return False
        ''''Else
        ''''    Return True
        ''''End If
        'Added By Saylee on 10-Feb-2014 for ALL10022014
        Dim mAircraftCountForLicense As AircraftCountForLicense = AircraftCountForLicense.GetAircraftCountForLicense
        If mAircraftCountForLicense.Count >= mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
            ''MessageBox.Show("This version does not supports more than " & mCheck.Number("Aircraft").ToString & " Aircrafts", "Version 1.0", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        Else
            Return True
        End If
    End Function
    ''for Deccan Only...Now not needed
    ''Private Function AllowNewAircraft() As Boolean
    ''    Dim mtmpMachineList As MachineList = MachineList.GetMachineList()
    ''    
    ''    If (AppSettings("ClientCode") = "Deccan" or AppSettings("ClientCode") = "ADeccan") Then
    ''        If mtmpMachineList.Count >= 50 Then
    ''            Return False
    ''        Else
    ''            Return True
    ''        End If
    ''    Else
    ''        Dim mCheck As New Authenticate.CheckAuthentication(True)
    ''        If mtmpMachineList.Count >= mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
    ''            Return False
    ''        Else
    ''            Return True
    ''        End If
    ''    End If
    ''End Function
    'Added By Utkarsh On 11-Mar-2011
    Private Sub SetRights()
        If (Not User.IsInRole("MachinePrint")) Then
            btnPrint.Enabled = False
            btnPrint.ToolTip = "You are not authorized user"
            btnPrintTop.Enabled = False
            btnPrintTop.ToolTip = "You are not authorized user"
        End If
    End Sub
    '*******************************
#End Region

#Region " DataBinding "
    Private Sub DatafieldBind()
        mtmpMachineList = tmpMachineList.GetMachineList()
        dgMachineList.DataSource = mtmpMachineList
        ControlVisibilityForGrid()
        Session("mtmpMachineList") = mtmpMachineList

        DataBind()
        If IsNothing(Session("Idx")) Then cmbLookIn.SelectedIndex = 0 Else cmbLookIn.SelectedIndex = Idx
        If IsNothing(Session("SearchForText")) Then txtFor.Text = "" Else txtFor.Text = CType(Session("SearchForText"), String)
        Session("Idx") = cmbLookIn.SelectedIndex
        DisplayControls(cmbLookIn.SelectedIndex)

        Index = Session("Idx")
        txtFor.Text = Session("SearchForText")

        Session("Idx") = Index
        Session("SearchForText") = txtFor.Text
    End Sub
    Private Sub dgMachineList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMachineList.Sorting
        mtmpMachineList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgMachineList.DataSource = mtmpMachineList
        ControlVisibilityForGrid()
        Session("mtmpMachineList") = mtmpMachineList
        dgMachineList.DataBind()
        SetGrid()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        'Put user code to initialize the page here
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 22-July-2011
        If Not IsPostBack Then
            If Sortfield = "" Then  REM:this is to set the sort Command
                Sortfield = "RegNo"
            End If
            If cmbLookIn.Enabled = True Then
                setFocus(cmbLookIn)
            End If
            RemoveSession()
            Session("MiddleFrame") = "wfMachineList_Ajax.aspx?"
            DatafieldBind()
            SetControl()
            SetPage()
            SetRights()  'Added By Utkarsh On 11-Mar-2011
            btnAddNew.Enabled = AllowNewAircraft() ''Commented for Deccan Only
            btnAddNewTop.Enabled = AllowNewAircraft()

            If AppSettings("ClientCode") = "RED" And Not (UCase(User.Identity.Name) = UCase("btpladmin")) Then 'Added by Saylee on 13-Dec-2023, for RED client : Lock AddNew button for all users except btpladmin
                btnAddNew.Visible = False
                btnAddNewTop.Visible = False
            End If

            SetGrid()
        End If

    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgMachineList.PageIndex = 0
        Dim Index As Int32 = IIf(cmbLookIn.SelectedIndex > 0, cmbLookIn.SelectedIndex, 0)
        Session("Idx") = Index
        Session("SearchForText") = txtFor.Text
        CallFindNow(Index)
        ControlVisibilityForGrid()
        dgMachineList.DataBind()
        SetGrid()
        lblResult.Text = "List of Aircraft as per criteria: " & mtmpMachineList.Count & " Record(s) found."

        upnlGrid.Update()
        upnlTitle.Update()
        upnlResult.Update()
    End Sub
    Private Sub txtFor_TextChanged(sender As Object, e As System.EventArgs) Handles txtFor.TextChanged
        dgMachineList.PageIndex = 0
        Dim Index As Int32 = IIf(cmbLookIn.SelectedIndex > 0, cmbLookIn.SelectedIndex, 0)
        Session("Idx") = Index
        Session("SearchForText") = txtFor.Text
        CallFindNow(Index)
        ControlVisibilityForGrid()
        dgMachineList.DataBind()
        SetGrid()
        lblResult.Text = "List of Aircraft as per criteria: " & mtmpMachineList.Count & " Record(s) found."

        upnlGrid.Update()
        upnlTitle.Update()
        upnlResult.Update()
    End Sub
    Private Sub dgMachineList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMachineList.RowCommand
        ' Dim mID As New Guid(e.Item.Cells(0).Text) ''this line is added for each case seperately

        Select Case e.CommandName
            Case "EditRec"
                'Commented By Utkarsh On 11-Mar-2011

                ''If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                ''    ' setObject()
                ''    SetSession()
                ''    MarkLog(Util.Action.Edit, "Aircraft", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If

                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                'Dim mID As Guid = mtmpMachineList(Index).ID
                Dim mRegNo As String = mtmpMachineList(mID).RegNo
                'Added By Utkarsh On 11-Mar-2011
                If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Aircraft", User.Identity.Name & " is not Authorized User to edit " + mRegNo, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    Exit Sub
                End If
                '*******************************
                Session("ActiveTabIndex") = 0
                mMachine = Machine.GetMachine(mID)
                mMachine.BeginEdit()
                Session("mMachine") = mMachine
                Dim MachineDetail As String = "Reg No. : " + mMachine.RegNo + " with Model : " + mMachine.AssemblyStatus.Assembly.ModelName + " and Serial No : " + mMachine.AssemblyStatus.Assembly.SerialNo
                MarkLog(Util.Action.Edit, "Aircraft", MachineDetail, Util.ErrorType.NoError, mMachine.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMachine_Ajax.aspx?BackPage=Index.aspx');", True)

            Case "DeleteRec"
                'Commented By Utkarsh On 11-Mar-2011
                ''If (Not User.IsInRole("MachineDelete")) Then
                ''    'setObject()
                ''    SetSession()
                ''    MarkLog(Util.Action.Delete, "Aircraft", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                'Dim mID As Guid = mtmpMachineList(Index).ID
                Dim mRegNo As String = mtmpMachineList(mID).RegNo
                'Added By Utkarsh On 11-Mar-2011
                If (Not User.IsInRole("MachineDelete")) Then
                    SetSession()
                    MarkLog(Util.Action.Edit, "Aircraft", User.Identity.Name & " is not Authorized User to delete " + mRegNo, Util.ErrorType.HandledError, mID, EventLogID)
                    '  ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    Exit Sub
                    '************************************
                Else
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                    'msg.ReplacePage = "wfMachineList.aspx?BackPage=" & Request.QueryString("BackPage")
                    'Session("sender") = "Delete"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                    mMachine = Machine.GetMachine(mID)
                    Session("mMachine") = mMachine
                End If
        End Select
    End Sub
    Private Sub dgMachineList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgMachineList.PageIndexChanging
        dgMachineList.PageIndex = e.NewPageIndex
        dgMachineList.DataSource = mtmpMachineList
        Session("mtmpMachineList") = mtmpMachineList
        dgMachineList.DataBind()
        ControlVisibilityForGrid()
        SetGrid()
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        SetSession()
        mMachine = Machine.NewMachine(Guid.NewGuid)
        If (Not User.IsInRole("MachineNew") And mMachine.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mMachine.IsNew) Then
            '  setObject()
            SetSession()
            MarkLog(Util.Action.[New], "Aircraft", User.Identity.Name & " is not Authorized User to add ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfMachineList.aspx?BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        'mMachine = Machine.NewMachine(Guid.NewGuid)
        mMachine.BeginEdit()
        Session("mMachine") = mMachine
        MarkLog(Util.Action.[New], "Aircraft", "", Util.ErrorType.NoError, mMachine.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfMachine_Ajax.aspx?BackPage=Index.aspx');", True)

    End Sub
    Private Sub cmbLookIn_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLookIn.SelectedIndexChanged
        Dim Index As Int32 = IIf(cmbLookIn.SelectedIndex > 0, cmbLookIn.SelectedIndex, 0)
        Session.Remove("SearchForText")
        txtFor.Text = ""
        DisplayControls(Index)
        If cmbLookIn.Enabled = True Then
            setFocus(cmbLookIn)
        End If
        If Index = 0 Then
            dgMachineList.PageIndex = 0
            Session("Idx") = Index
            Session("SearchForText") = txtFor.Text
            CallFindNow(Index)
            ControlVisibilityForGrid()
            dgMachineList.DataBind()
            SetGrid()
            lblResult.Text = "List of Aircraft as per criteria: " & mtmpMachineList.Count & " Record(s) found."

            upnlGrid.Update()
            upnlTitle.Update()
            upnlResult.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Session.Remove("Idx")
        Session.Remove("SearchForText")
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

#Region " Report "
    'Created By:- Jyoti
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

    Private SearchStr1 As String
    Private SearchStr2 As String
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, btnPrintTop.Click
        'Commented By Utkarsh On 11-Mar-2011
        ''If (Not User.IsInRole("MachinePrint")) Then
        ''    MarkLog(Util.Action.Print, "Aircraft", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfMachineList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
        ''    msg.Show()
        ''    Exit Sub
        ''End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList
        Rpt = New crListMachine
        If cmbLookIn.SelectedIndex = 0 Then
            SearchStr1 = "The report shows all records till date."
            SearchStr2 = ""
        ElseIf cmbLookIn.SelectedIndex = 1 Or cmbLookIn.SelectedIndex = 2 Or cmbLookIn.SelectedIndex = 3 Then
            SearchStr1 = "The report shows records filtered by the following criteria"
            SearchStr2 = "By" + " " + cmbLookIn.SelectedItem.Text + " " + ":" + " " + txtFor.Text
        End If

        ReportDetails.Add(New rptStatus(, 0, , _
              , , , dgMachineList.Columns.Item(1).HeaderText, , dgMachineList.Columns.Item(2).HeaderText, dgMachineList.Columns.Item(3).HeaderText, _
              dgMachineList.Columns.Item(4).HeaderText, dgMachineList.Columns.Item(5).HeaderText, dgMachineList.Columns.Item(6).HeaderText, Replace(dgMachineList.Columns.Item(10).HeaderText, "</BR>", vbCrLf), _
                  dgMachineList.Columns.Item(11).HeaderText))
        Dim TotalCount As Integer
        ' TotalCount = Me.mtmpMachineList.Count
        'Dim I As Integer ''''
        Dim mCurrentPageindex As Integer = Me.dgMachineList.PageIndex   'Code Added
        TotalCount = Me.dgMachineList.PageCount                                'Code Changed

        Dim j As Integer                                                       'Code Added
        Dim I As Integer

        Dim str(12) As String
        For j = 0 To TotalCount - 1                                            'Code Changed

            Me.dgMachineList.PageIndex = j                              'Code Added 
            Me.dgMachineList.DataSource = mtmpMachineList                         'Code Added 
            Session("mtmpMachineList") = mtmpMachineList                             'Code Added 
            dgMachineList.DataBind()                                           'Code Added 
            '----Added by Vikrant on 16-12-2011  FOR ALL14122011-2------------
            ControlVisibilityForGrid()
            '----------------------------------------------------------------
            SetGrid()

            For I = 0 To Me.dgMachineList.PageSize - 1                         'Code Added 
                If I <= Me.dgMachineList.Rows.Count - 1 Then                  'Code Added




                    'Dim str(8) As String '''
                    'For I = 0 To TotalCount - 1
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
                    str(10) = ""


                    If Me.dgMachineList.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgMachineList.Rows(I).Cells.Item(1).Text
                    If Me.dgMachineList.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(1) = Me.dgMachineList.Rows(I).Cells.Item(4).Text
                    If Me.dgMachineList.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(2) = Me.dgMachineList.Rows(I).Cells.Item(2).Text
                    If Me.dgMachineList.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(3) = Me.dgMachineList.Rows(I).Cells.Item(5).Text
                    If Me.dgMachineList.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(4) = Me.dgMachineList.Rows(I).Cells.Item(3).Text
                    If Me.dgMachineList.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgMachineList.Rows(I).Cells.Item(6).Text
                    If Me.dgMachineList.Rows(I).Cells.Item(10).Text <> "&nbsp;" Then str(6) = Me.dgMachineList.Rows(I).Cells.Item(10).Text
                    If Me.dgMachineList.Rows(I).Cells.Item(11).Text <> "&nbsp;" Then str(7) = Me.dgMachineList.Rows(I).Cells.Item(11).Text

                    If Me.dgMachineList.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(8) = Me.dgMachineList.Rows(I).Cells.Item(7).Text + "(L)" 'Landings
                    If Me.dgMachineList.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(9) = Me.dgMachineList.Rows(I).Cells.Item(8).Text + "(C)" 'Cycles 
                    If Me.dgMachineList.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(10) = Me.dgMachineList.Rows(I).Cells.Item(9).Text + "(RI)" 'RINS

                    ReportDetails.Add(New rptStatus(, 1, , _
                      , , , , , , , , , , , _
               , , , , str(8), str(9), str(10), , str(0), str(1), str(2), str(3), str(4), str(5), str(6), str(7), , , ))
                End If
            Next
        Next
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
   mCompanyDetail.WebSite, "Aircraft List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If mtmpMachineList.Count = 0 Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
            'msg1.ReplacePage = "wfMachineList.aspx?Backpage="
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 29-Feb-2012
        da.Fill(ds, ReportDetails)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "Aircraft", "Machine List Report", Util.ErrorType.NoError, Guid.Empty)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        'Code Added 
        Me.dgMachineList.PageIndex = mCurrentPageindex
        Me.dgMachineList.DataSource = mtmpMachineList
        ControlVisibilityForGrid()
        Session("mtmpMachineList") = mtmpMachineList
        dgMachineList.DataBind()
        SetGrid()
        'Code Added 
    End Sub
#End Region

#End Region

  
  
   
End Class