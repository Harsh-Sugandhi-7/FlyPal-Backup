'Pallavi  - 28-07-2006
Partial Class wfSearchCriteriaForHistory
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents txtFromDate As SIControls.SICalendar
    'Protected WithEvents txtToDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variable Declaratioin"
    Dim mtmpHistoryList As tmpHistoryList
    Dim mAssemblyList As AssemblyList
    Dim mCompList As CompList
    Dim mRptAssemblyList As ReportAssemblyList
    Dim mEAssemblyList As AssemblyList

    Dim chkModel As Boolean = False
    Dim chkSerialNo As Boolean = False
    Dim chkAModel As Boolean = False
    Dim chkASerialNo As Boolean = False

    Dim ListID As Guid
    Dim MacID As String = ""
    Dim MacID1 As String = ""
    Dim AssemblyIndex As Integer = 0
    Dim I As Integer
    Dim chkFindModel As Boolean = False
    Dim chkFindNow As Boolean = False
    Dim MachineID As Guid = Guid.Empty
    Dim mMacList As AssemblyList
    Dim ListMacID1 As String
    Dim ListStartDate As String
    Dim ListEndDate As String
    Dim ListPartNo As String
    Dim ListCompSerialNo As String
    Dim ListModel As String
    Dim ListSerialNo As String
    Dim ListAModelNo As String
    Dim ListASerialNo As String
    Dim chkAss As Boolean = True
    Dim chkComp As Boolean = False
    Dim mAssType As Integer

    Dim mMachineList As MachineList
    Dim ObjMachineList As MachineList
    Dim ObjMachine As MachineInfo
    Dim ObjAssemblyStatus As AssemblyStatusInfo
    Dim ObjAssemblyStatusPeriod As AssemblyStatusPeriodInfo
    Dim ObjCompStatus As CompStatusInfo
    Dim ObjCompStatusPeriod As CompStatusPeriodInfo

    Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
    Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
    Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
    Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
    Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
    Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo
    Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
    Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
    Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
    Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo
    Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
    Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

    Dim Periodcount As Integer
    Dim Count As Integer
    Dim StartDate As String
    Dim EndDate As String
    Dim strWorkOrderNo As String
    Dim IsRemoved As Boolean = False
    Dim IsComplied As Boolean = False
    Dim IsInstalled As Boolean = False
    Dim AssemblyType As String
    Dim ReportLabel As String
    Private AssemblyTypeIndex As Integer
    Dim ReportType As Integer
    Private Type As String
    Private Date1 As String
    Private WorkOrderNo As String
    Private RegNo As String
    Private Model As String
    Private PartNo As String
    Private AssemblySerialNo As String
    Private [Of] As String
    Private Description As String
    Private SerialNo As String
    Private PeriodName As String
    Private ChildValue As String
    Private ParentValue As String
    Private Reason As String
    Private Remark As String
#End Region

#Region " Helper Methods "
    Public Sub AssemblyStatus1()
        chkAssembly.Checked = True
        chkComponent.Checked = True
        chkAssembly.Enabled = True
        chkComponent.Enabled = True
        chkAss = True
        chkComp = True
        lblAModelNo.Enabled = True
        txtAModelNo.Enabled = True
        lblASerialNo.Enabled = True
        txtASerialNo.Enabled = True
        btnFindModel.Enabled = True

        lblCPartNo.Enabled = True
        txtCPartNo.Enabled = True
        lblCSerialNo.Enabled = True
        txtCSerialNo.Enabled = True
        btnFindPart.Enabled = True

        chkComponent_CheckedChanged(Nothing, Nothing)
    End Sub
    Public Sub AssemblyStatus2()
        chkAssembly.Checked = False
        chkComponent.Checked = True
        chkAssembly.Enabled = False
        chkAss = False
        chkComp = True
        lblAModelNo.Enabled = False
        txtAModelNo.Enabled = False
        lblASerialNo.Enabled = False
        txtASerialNo.Enabled = False
        btnFindModel.Enabled = False
        lblCPartNo.Enabled = True
        txtCPartNo.Enabled = True
        lblCSerialNo.Enabled = True
        txtCSerialNo.Enabled = True
        btnFindPart.Enabled = True
        pnlAModel.Visible = False
        pnlEModel.Visible = False
        pnlPart.Visible = True

        chkComponent_CheckedChanged(Nothing, Nothing)
    End Sub
    Private Sub FindNowModel(ByVal ListModel As String, ByVal ListSerialNo As String, ByVal mAssType As Integer)
        mAssemblyList = Nothing
        dgModel.DataSource = Nothing
        mAssemblyList = AssemblyList.GetAssemblyList(ListModel, ListSerialNo, mAssType, , Today.Date.ToString)
        dgModel.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList
        dgModel.DataBind()
        lblResult.Text = "List of Model & Serial No.s : " & mAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub FindNowEModel(ByVal ListAModelNo As String, ByVal ListASerialNo As String, ByVal mAssType As Integer)
        mEAssemblyList = Nothing
        dgEModel.DataSource = Nothing
        If ReportType = 2 Then
            mEAssemblyList = AssemblyList.GetAssemblyList(ListAModelNo, ListASerialNo, mAssType, , txtFromDate.Text.ToString)
            Session("mEAssemblyList") = mEAssemblyList
        Else
            mEAssemblyList = AssemblyList.GetAssemblyList(ListAModelNo, ListASerialNo, mAssType, , Today.Date.ToString)
            Session("mEAssemblyList") = mEAssemblyList
        End If
        dgEModel.DataSource = mEAssemblyList
        dgEModel.DataBind()
        lblResult3.Text = "List of Model & Serial No.s : " & mEAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub FindNowAModel(ByVal ListMacID1 As String, ByVal ListStartDate As String, ByVal ListEndDate As String, ByVal ListAModelNo As String, ByVal ListASerialNo As String, ByVal ReportType As Integer)
        mRptAssemblyList = Nothing
        dgAModel.DataSource = Nothing
        mRptAssemblyList = ReportAssemblyList.GetReportAssemblyList(ListMacID1, ListStartDate, ListEndDate, ListAModelNo, ListASerialNo, ReportType)
        dgAModel.DataSource = mRptAssemblyList
        Session("mAssemblyList") = mAssemblyList
        dgAModel.DataBind()
        Session("mRptAssemblyList") = mRptAssemblyList
        lblResult1.Text = "List of Model & Serial No.s : " & mRptAssemblyList.Count & " Record(s) found."
    End Sub
    Private Sub FindNowPart(ByVal ListPartNo As String, ByVal ListCompSerialNo As String, ByVal mAssType As Integer, ByVal ListEndDate As String)
        mCompList = Nothing
        dgPart.DataSource = Nothing
        mCompList = CompList.GetCompList(ListPartNo, ListCompSerialNo, ListEndDate, mAssType)
        dgPart.DataSource = mCompList
        dgPart.DataBind()
        Session("mCompList") = mCompList
        lblResult2.Text = "List of Part & Serial No.s : " & mCompList.Count & " Record(s) found."
    End Sub
    Private Sub ClearControlsofModel()
        txtModelNo.Text = ""
        txtSerialNo.Text = ""
    End Sub
    Private Sub ClearControlsofAModel()
        txtAModelNo.Text = ""
        txtASerialNo.Text = ""
    End Sub
    Private Sub ClearControlsofPart()
        txtCPartNo.Text = ""
        txtCSerialNo.Text = ""
    End Sub
    Public Sub SetComp()
        pnlPart.Visible = True
        pnlAModel.Visible = False
        pnlEModel.Visible = False
        lblAModelNo.Visible = False
        txtAModelNo.Visible = False
        lblASerialNo.Visible = False
        txtASerialNo.Visible = False
        btnFindModel.Visible = False
        lblCPartNo.Visible = True
        txtCPartNo.Visible = True
        lblCSerialNo.Visible = True
        txtCSerialNo.Visible = True
        btnFindPart.Visible = True
        dgPart.Visible = True
        dgAModel.Visible = False

        ''pnlPart.Visible = True
        ''pnlAModel.Visible = False
        pnlEModel.Visible = False
        lblAModelNo.Enabled = False
        txtAModelNo.Enabled = False
        lblASerialNo.Enabled = False
        txtASerialNo.Enabled = False
        btnFindModel.Enabled = False
        lblCPartNo.Visible = True
        txtCPartNo.Enabled = True
        lblCSerialNo.Enabled = True
        txtCSerialNo.Enabled = True
        btnFindPart.Enabled = True
    End Sub
    Public Sub SetAss()
        pnlEModel.Visible = True
        pnlPart.Visible = False
        lblAModelNo.Visible = True
        txtAModelNo.Visible = True
        lblASerialNo.Visible = True
        txtASerialNo.Visible = True
        btnFindModel.Visible = True
        lblCPartNo.Visible = False
        txtCPartNo.Visible = False
        lblCSerialNo.Visible = False
        txtCSerialNo.Visible = False
        btnFindPart.Visible = False
        dgEModel.Visible = True
        dgPart.Visible = False

        ''pnlEModel.Visible = True
        ''pnlPart.Visible = False
        lblAModelNo.Enabled = True
        txtAModelNo.Enabled = True
        lblASerialNo.Enabled = True
        txtASerialNo.Enabled = True
        btnFindModel.Enabled = True
        lblCPartNo.Enabled = False
        txtCPartNo.Enabled = False
        lblCSerialNo.Enabled = False
        txtCSerialNo.Enabled = False
        btnFindPart.Enabled = False

    End Sub
    Public Sub SetAssComp()

        pnlPart.Visible = True
        pnlEModel.Visible = True
        lblAModelNo.Visible = True
        txtAModelNo.Visible = True
        lblASerialNo.Visible = True
        txtASerialNo.Visible = True
        btnFindModel.Visible = True
        lblCPartNo.Visible = True
        txtCPartNo.Visible = True
        lblCSerialNo.Visible = True
        txtCSerialNo.Visible = True
        btnFindPart.Visible = True
        dgEModel.Visible = True
        dgPart.Visible = True

        ''pnlPart.Visible = True
        ''pnlEModel.Visible = True
        lblAModelNo.Enabled = True
        txtAModelNo.Enabled = True
        lblASerialNo.Enabled = True
        txtASerialNo.Enabled = True
        btnFindModel.Enabled = True
        lblCPartNo.Enabled = True
        txtCPartNo.Enabled = True
        lblCSerialNo.Enabled = True
        txtCSerialNo.Enabled = True
        btnFindPart.Enabled = True

    End Sub
    Public Sub SetMachineID()
        If ((chkFindNow = True) And (chkModel = True Or chkSerialNo = True)) Or ((chkFindNow = False)) Then
            mMacList = AssemblyList.GetAssemblyList(txtModelNo.Text, txtSerialNo.Text, 1, "{00000000-0000-0000-0000-000000000000}", Today.Date.ToString)
            If mMacList.Count = 0 Then
                MacID = "{00000000-0000-0000-0000-000000000000}"
                ReportDetail()
            Else
                For I = 0 To mMacList.Count - 1
                    If MacID = "" Then
                        MacID = MacID & "{" & mMacList(0).MachineID.ToString & "}"
                    Else
                        'MacID = MacID & "','{" & mMacList(I).MachineID.ToString & "}"
                        MacID = MacID & ",{" & mMacList(I).MachineID.ToString & "}"
                    End If
                Next
                ReportDetail()
            End If
        ElseIf chkFindNow = True Then
            mMacList = AssemblyList.GetAssemblyList("", "", , ListID.ToString, Today.Date.ToString)
            MacID = "{" & mMacList(0).MachineID.ToString & "}"
            ReportDetail()
        End If
    End Sub
    Public Sub SetModel()
        If ((chkFindNow = True And chkFindModel = True) And (chkModel = True Or chkSerialNo = True)) _
        Or (chkFindNow = False And chkFindModel = True) Or (chkFindNow = False And chkFindModel = False) Then
            mMacList = AssemblyList.GetAssemblyList(txtModelNo.Text, txtSerialNo.Text, , "{00000000-0000-0000-0000-000000000000}", Today.Date.ToString)

            'MacID1 = mMacList.GetDistinctMachineIDListString

            For I = 0 To mMacList.Count - 1
                If MacID1 = "" Then
                    MacID1 = MacID1 & "{" & mMacList(0).MachineID.ToString & "}"
                Else
                    MacID1 = MacID1 & "','{" & mMacList(I).MachineID.ToString & "}"
                End If
            Next I
        ElseIf ((chkFindNow = True And chkFindModel = True)) Or (chkFindNow = True) Then
            mMacList = AssemblyList.GetAssemblyList(txtModelNo.Text, txtSerialNo.Text, , ListID.ToString, Today.Date.ToString)
            MacID1 = "{" & mMacList(0).MachineID.ToString & "}"
        End If
    End Sub
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mEAssemblyList = CType(Session("mEAssemblyList"), AssemblyList)
        mRptAssemblyList = CType(Session("mRptAssemblyList"), ReportAssemblyList)
        mCompList = CType(Session("mCompList"), CompList)

        ListModel = CType(Session("ListModel"), String)
        ListSerialNo = CType(Session("ListSerialNo"), String)
        ListModel = IIf(IsNothing(ListModel), "", ListModel)
        ListSerialNo = IIf(IsNothing(ListSerialNo), "", ListSerialNo)

        ListPartNo = CType(Session("ListPartNo"), String)
        ListCompSerialNo = CType(Session("ListCompSerialNo"), String)
        ListPartNo = IIf(IsNothing(ListPartNo), "", ListPartNo)
        ListCompSerialNo = IIf(IsNothing(ListCompSerialNo), "", ListCompSerialNo)

        ListAModelNo = CType(Session("ListAModelNo"), String)
        ListASerialNo = CType(Session("ListASerialNo"), String)
        ListAModelNo = IIf(IsNothing(ListAModelNo), "", ListAModelNo)
        ListASerialNo = IIf(IsNothing(ListASerialNo), "", ListASerialNo)
        ListID = Session("ListID")

        AssemblyTypeIndex = Session("AssemblyTypeIndex")
        AssemblyIndex = Session("AssemblyIndex")
        chkFindModel = Session("chkFindModel")
        chkFindNow = Session("chkFindNow")
        chkAModel = Session("chkAModel")
        chkModel = Session("chkModel")
        chkASerialNo = Session("chkASerialNo")
        chkSerialNo = Session("chkSerialNo")
        ReportType = Session("ReportType")
        lblRemovalFrom.Text = Session("lblRemovalFrom.Text")
        lblRemovalof.Text = Session("lblRemovalof.Text")
    End Sub
    Public Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mAssemblyList") = mAssemblyList
        Session("mRptAssemblyList") = mRptAssemblyList
        Session("mCompList") = mCompList

        Session("ListModel") = ListModel
        Session("ListSerialNo") = ListSerialNo
        Session("ListPartNo") = ListPartNo
        Session("ListCompSerialNo") = ListCompSerialNo
        Session("ListAModelNo") = ListAModelNo
        Session("ListASerialNo") = ListASerialNo
        Session("ListID") = ListID

        Session("AssemblyTypeIndex") = AssemblyTypeIndex
        Session("AssemblyIndex") = AssemblyIndex
        Session("chkFindModel") = chkFindModel
        Session("chkFindNow") = chkFindNow
        Session("chkAModel") = chkAModel
        Session("chkModel") = chkModel
        Session("chkASerialNo") = chkASerialNo
        Session("chkSerialNo") = chkSerialNo
        Session("ReportType") = ReportType
        Session("lblRemovalFrom.Text") = lblRemovalFrom.Text
        Session("lblRemovalof.Text") = lblRemovalof.Text
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForHistory.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblyList")
            Session.Remove("mRptAssemblyList")
            Session.Remove("mCompList")

            Session.Remove("ListModel")
            Session.Remove("ListSerialNo")
            Session.Remove("ListPartNo")
            Session.Remove("ListCompSerialNo")
            Session.Remove("ListAModelNo")
            Session.Remove("ListASerialNo")
            Session.Remove("ListID")

            Session.Remove("AssemblyTypeIndex")
            Session.Remove("AssemblyIndex")
            Session.Remove("chkFindModel")
            Session.Remove("chkFindNow")
            Session.Remove("chkAModel")
            Session.Remove("chkModel")
            Session.Remove("chkASerialNo")
            Session.Remove("chkSerialNo")
            Session.Remove("ReportType")
            Session.Remove("lblRemovalFrom.Text")
            Session.Remove("lblRemovalof.Text")
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility(Optional ByVal IsAssembly As Boolean = False, Optional ByVal IsComponent As Boolean = False)
        ''pnlPart.Visible = False
        ''pnlEModel.Visible = False
        'lblAModelNo.Visible = False
        'txtAModelNo.Visible = False
        'lblASerialNo.Visible = False
        'txtASerialNo.Visible = False
        'btnFindModel.Visible = False
        'lblCPartNo.Visible = False
        'txtCPartNo.Visible = False
        'lblCSerialNo.Visible = False
        'txtCSerialNo.Visible = False
        'btnFindPart.Visible = False

        'dgPart.Visible = False
        'dgEModel.Visible = False
        'dgAModel.Visible = False
        'pnlPart.Visible = False
        'pnlEModel.Visible = False

        dgAModel.Visible = False
        If IsAssembly = True Then
            If chkAssembly.Checked Then
                pnlEModel.Visible = True
                dgEModel.Visible = True

                dgEModel.DataSource = mEAssemblyList
                dgEModel.DataBind()

                lblAModelNo.Visible = True
                txtAModelNo.Visible = True
                lblASerialNo.Visible = True
                txtASerialNo.Visible = True
                btnFindModel.Visible = True
                lblResult3.Visible = True
            Else
                pnlEModel.Visible = False
                dgEModel.Visible = False
                lblAModelNo.Visible = False
                txtAModelNo.Visible = False
                lblASerialNo.Visible = False
                txtASerialNo.Visible = False
                btnFindModel.Visible = False
                lblResult3.Visible = False
            End If
            upnlAssembly.Update()
        End If

        If IsComponent Then
            If chkComponent.Checked Then
                pnlPart.Visible = True
                dgPart.Visible = True

                dgPart.DataSource = mCompList
                dgPart.DataBind()

                lblCPartNo.Visible = True
                txtCPartNo.Visible = True
                lblCSerialNo.Visible = True
                txtCSerialNo.Visible = True
                btnFindPart.Visible = True
            Else
                pnlPart.Visible = False
                dgPart.Visible = False
                lblCPartNo.Visible = False
                txtCPartNo.Visible = False
                lblCSerialNo.Visible = False
                txtCSerialNo.Visible = False
                btnFindPart.Visible = False
            End If

            upnlComponent.Update()
        End If



    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblWorkOrderNo1.Visible = True
        lblAssemblyType1.Visible = True
        lblModelNo1.Visible = True
        lblSerialNo1.Visible = True
        lblAModelNo1.Visible = True
        lblASerialNo1.Visible = True
        lblCPartNo1.Visible = True
        lblCSerialNo1.Visible = True
        lblRemovalFrom.Visible = True
        lblRemovalof.Visible = True
    End Sub
    Private Sub SetValues()
        If cmbAssemblyType.SelectedItem.Text = "(All)" Then
            AssemblyType = ""
        Else
            AssemblyType = cmbAssemblyType.SelectedItem.Text
        End If
        If (chkAssembly.Checked) Then
            chkAss = True
        Else
            chkAss = False
        End If
        If (chkComponent.Checked) Then
            chkComp = True
        Else
            chkComp = False
        End If
        If ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkAss = True And chkComp = True)) Then
            ListModel = txtModelNo.Text
            ListSerialNo = txtSerialNo.Text
            ListAModelNo = txtAModelNo.Text
            ListASerialNo = txtASerialNo.Text
            ListPartNo = txtCPartNo.Text
            ListCompSerialNo = txtCSerialNo.Text
        ElseIf ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkAss = True And chkComp = False)) Then
            ListModel = txtModelNo.Text
            ListSerialNo = txtSerialNo.Text
            ListAModelNo = txtAModelNo.Text
            ListASerialNo = txtASerialNo.Text
            ListPartNo = ""
            ListCompSerialNo = ""
        ElseIf ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkComp = True And chkAss = False)) Then
            ListModel = txtModelNo.Text
            ListSerialNo = txtSerialNo.Text
            ListAModelNo = ""
            ListASerialNo = ""
            ListPartNo = txtCPartNo.Text
            ListCompSerialNo = txtCSerialNo.Text
        ElseIf ((AssemblyTypeIndex = 2 Or AssemblyTypeIndex = 3 Or AssemblyTypeIndex = 4 Or AssemblyTypeIndex = 5 Or AssemblyTypeIndex = 6) And (chkComp = True Or chkComp = False) And (chkAss = False)) Then
            ListModel = txtModelNo.Text
            ListSerialNo = txtSerialNo.Text
            ListAModelNo = ""
            ListASerialNo = ""
            ListPartNo = txtCPartNo.Text
            ListCompSerialNo = txtCSerialNo.Text
        End If
        MacID = ""
        strWorkOrderNo = txtWorkOrderNo.Text
        'If Not (txtFromDate.IsDateValue) Then
        '    StartDate = ""
        'Else
        '    StartDate = txtFromDate.Value.ToString
        'End If
        'If Not (txtToDate.IsDateValue) Then
        '    EndDate = ""
        'Else
        '    EndDate = txtToDate.Value.ToString
        'End If

        If Not IsDate(txtFromDate.Text) Then
            StartDate = ""
        Else
            StartDate = CDate(txtFromDate.Text).ToString(AppSettings("DateFormat"))
        End If
        If Not IsDate(txtToDate.Text) Then
            EndDate = ""
        Else
            EndDate = CDate(txtToDate.Text).ToString(AppSettings("DateFormat"))
        End If

        'lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", StartDate, "")
        'lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", EndDate, "")
        If (StartDate <> "") Then
            lblDateRangeFrom.Text = "From Date : " & New SmartDate(txtFromDate.Text.ToString).FormattedText
        Else
            lblDateRangeFrom.Text = "From Date : "
        End If

        If (EndDate <> "") Then
            lblDateRangeTo.Text = "To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText
        Else
            lblDateRangeTo.Text = "To Date : "
        End If
        lblWorkOrderNo1.Text = "Work Order No. : " & IIf(strWorkOrderNo <> "", strWorkOrderNo, "")
        lblAssemblyType1.Text = "Assembly : " & IIf(AssemblyType <> "", AssemblyType, "")
        lblModelNo1.Text = "Model : " & IIf(ListModel <> "", ListModel, "")
        lblSerialNo1.Text = "Serial No. : " & IIf(ListSerialNo <> "", ListSerialNo, "")
        lblAModelNo1.Text = "Model : " & IIf(ListAModelNo <> "", ListAModelNo, "")
        lblASerialNo1.Text = "Assembly Serial No. : " & IIf(ListASerialNo <> "", ListASerialNo, "")
        lblCPartNo1.Text = "Part No. : " & IIf(ListPartNo <> "", ListPartNo, "")
        lblCSerialNo1.Text = "Component Serial No. : " & IIf(ListCompSerialNo <> "", ListCompSerialNo, "")
    End Sub
    Private Sub ResetValues()
        MachineID = Guid.Empty
        StartDate = txtFromDate.Text.ToString
        EndDate = txtToDate.Text.ToString
        txtModelNo.Text = ""
        txtSerialNo.Text = ""
        txtAModelNo.Text = ""
        txtASerialNo.Text = ""
        txtCPartNo.Text = ""
        txtCSerialNo.Text = ""
        ListStartDate = ""
        ListEndDate = ""
        ListPartNo = ""
        ListCompSerialNo = ""
        ListModel = ""
        ListSerialNo = ""
        ListAModelNo = ""
        ListASerialNo = ""
        AssemblyType = ""
        strWorkOrderNo = ""
        MacID = ""
        MacID1 = ""
        ListMacID1 = ""
        AssemblyTypeIndex = 0
        AssemblyIndex = 0
        IsInstalled = False
        IsRemoved = False
        IsComplied = False
        chkAss = True
        chkComp = True
        chkFindNow = False
        chkFindModel = False
    End Sub
    Public Sub ReportDetail()
        mtmpHistoryList = tmpHistoryList.GetHistoryList(StartDate, EndDate, strWorkOrderNo, AssemblyType, ListModel, _
            ListSerialNo, ListAModelNo, ListASerialNo, ListPartNo, ListCompSerialNo, MacID, chkAss, chkComp, _
            IsRemoved, IsInstalled, IsComplied)
    End Sub
    Private Sub SetReport(ByVal ReportType As Integer)
        Dim RptIns As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim RptRem As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim RptComp As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        mtmpHistoryList = New tmpHistoryList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        SetValues()
        RptIns = New crInstallationTo
        RptRem = New crRemovalFrom
        RptComp = New crCompliance
        RptCommonHistory = New crCommonHistory

        Select Case ReportType
            Case 1 'Installation To
                IsInstalled = True
                SetMachineID()
                If AssemblyTypeIndex = 1 Or AssemblyTypeIndex = 2 Or AssemblyTypeIndex = 3 Or AssemblyTypeIndex = 4 Or AssemblyTypeIndex = 5 Or AssemblyTypeIndex = 6 Then
                    ReportLabel = "Installation To" + " " + AssemblyType
                Else
                    ReportLabel = "Installation To All"
                End If
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                    mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text.ToString).FormattedText, New SmartDate(txtToDate.Text.ToString).FormattedText, AssemblyType, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
                If mtmpHistoryList.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfSearchCriteriaForHistory.aspx?Backpage=&ReportType=" & Request.QueryString("ReportType")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    'Added By Utkarsh On 7-Jun-2011 For All07062011

                ElseIf mtmpHistoryList.Count > 0 Then
                    
                   RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 627)

                    '*******************************
                End If
                ds.Clear()
                '-----------Added by Utkarsh for Report Logo---------------
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                '----------------------------------------------------------
                da.Fill(ds, mtmpHistoryList)
                da.Fill(ds, Report)
                da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
                RptIns.SetDataSource(ds)
                Session("CrystalReport") = RptIns
            Case 2   'Removal From
                IsRemoved = True
                SetMachineID()
                If AssemblyTypeIndex = 1 Or AssemblyTypeIndex = 2 Or AssemblyTypeIndex = 3 Or AssemblyTypeIndex = 4 Or AssemblyTypeIndex = 5 Or AssemblyTypeIndex = 6 Then
                    ReportLabel = "Removal From" + " " + AssemblyType
                Else
                    ReportLabel = "Removal From All"
                End If
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                    mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text.ToString).FormattedText, New SmartDate(txtToDate.Text.ToString).FormattedText, AssemblyType, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
                If mtmpHistoryList.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfSearchCriteriaForHistory.aspx?Backpage=&ReportType=" & Request.QueryString("ReportType")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    'Added By Utkarsh On 7-Jun-2011 For All07062011

                ElseIf mtmpHistoryList.Count > 0 Then
                    
                   RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 626)

                    '*******************************
                End If
                ds.Clear()
                '-----------Added by Utkarsh for Report Logo---------------
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                '----------------------------------------------------------
                da.Fill(ds, mtmpHistoryList)
                da.Fill(ds, Report)
                da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
                RptRem.SetDataSource(ds)
                Session("CrystalReport") = RptRem
            Case 3  'Compliance On
                IsComplied = True
                SetMachineID()
                If AssemblyTypeIndex = 1 Or AssemblyTypeIndex = 2 Or AssemblyTypeIndex = 3 Or AssemblyTypeIndex = 4 Or AssemblyTypeIndex = 5 Or AssemblyTypeIndex = 6 Then
                    ReportLabel = "Compliance On" + " " + AssemblyType
                Else
                    ReportLabel = "Compliance On All"
                End If
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                    mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text.ToString).FormattedText, New SmartDate(txtToDate.Text.ToString).FormattedText, AssemblyType, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
                If mtmpHistoryList.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfSearchCriteriaForHistory.aspx?Backpage=&ReportType=" & Request.QueryString("ReportType")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    'Added By Utkarsh On 7-Jun-2011 For All07062011

                ElseIf mtmpHistoryList.Count > 0 Then
                    
                   RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 628)

                    '*******************************
                End If
                ds.Clear()
                '-----------Added by Utkarsh for Report Logo---------------
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                '----------------------------------------------------------
                da.Fill(ds, mtmpHistoryList)
                da.Fill(ds, Report)
                da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
                RptComp.SetDataSource(ds)
                Session("CrystalReport") = RptComp
            Case 4  'Common History
                IsRemoved = True
                IsInstalled = True
                IsComplied = True
                SetMachineID()
                If AssemblyTypeIndex = 1 Or AssemblyTypeIndex = 2 Or AssemblyTypeIndex = 3 Or AssemblyTypeIndex = 4 Or AssemblyTypeIndex = 5 Or AssemblyTypeIndex = 6 Then
                    ReportLabel = "Common History For" + " " + AssemblyType
                Else
                    ReportLabel = "Common History For All"
                End If
                Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                    mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text.ToString).FormattedText, New SmartDate(txtToDate.Text.ToString).FormattedText, AssemblyType, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.
                If mtmpHistoryList.Count = 0 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfSearchCriteriaForHistory.aspx?Backpage=&ReportType=" & Request.QueryString("ReportType")
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                    'Added By Utkarsh On 7-Jun-2011 For All07062011

                ElseIf mtmpHistoryList.Count > 0 Then
                    
                   RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 629)

                    '*******************************
                End If
                ds.Clear()
                '-----------Added by Utkarsh for Report Logo---------------
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                '----------------------------------------------------------
                da.Fill(ds, mtmpHistoryList)
                da.Fill(ds, Report)
                da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
                RptCommonHistory.SetDataSource(ds)
                Session("CrystalReport") = RptCommonHistory
        End Select
        Dim Str As String
        'Str = "<script language=Javascript>openTranDetail();</script>"
        '  ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)
        ResetValues()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    Response.Redirect("wfSearchCriteriaForHistory.aspx?MsgResult=0&Backpage=&ReportType=" & Request.QueryString("ReportType"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfSearchCriteriaForHistory.aspx?MsgResult=0&Backpage=&ReportType=" & Request.QueryString("ReportType"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAssemblyList = AssemblyList.GetAssemblyList(ListModel, ListSerialNo, mAssType, , Today.Date.ToString)
        dgModel.DataSource = mAssemblyList
        Session("mAssemblyList") = mAssemblyList

        mEAssemblyList = AssemblyList.GetAssemblyList(ListAModelNo, ListASerialNo, mAssType, , Today.Date.ToString)
        dgEModel.DataSource = mEAssemblyList
        Session("mEAssemblyList") = mEAssemblyList

        mCompList = CompList.GetCompList(ListPartNo, ListCompSerialNo, Today.Date.ToShortDateString, mAssType)
        dgPart.DataSource = mCompList
        Session("mCompList") = mCompList
        DataBind()
    End Sub
    Public Sub Title()
        'If ReportType = 1 Then
        If Session("Title").ToString() = 1 Then
            'lbltitle.Text = "Search criteria for Installation"
            lbltitle.Text = "Installation History Register"
            lblType.Text = "Installation To : "
            lblRemovalFrom.Text = "Installation To : "
            lblRemovalof.Text = "Installation On : "
            lblStep3.Text = "Step III. Selection of Installation To"
            lblStep4.Text = "Step IV. Selection of Installation On"
            'ElseIf ReportType = 2 Then
        ElseIf Session("Title").ToString() = 2 Then
            'lbltitle.Text = "Search criteria for Removal"
            lbltitle.Text = "Removal History Register"
            lblType.Text = "Removal From : "
            lblRemovalFrom.Text = "Removal From : "
            lblRemovalof.Text = "Removal of : "
            lblStep3.Text = "Step III. Selection of Removal From"
            lblStep4.Text = "Step IV. Selection of Removal of"
            'ElseIf ReportType = 3 Then
        ElseIf Session("Title").ToString() = 3 Then
            'lbltitle.Text = "Search criteria for Compliance"
            lbltitle.Text = "Compliance History Register"
            lblType.Text = "Compliance On : "
            lblRemovalFrom.Text = "Compliance On : "
            lblRemovalof.Text = "Compliance of : "
            lblStep3.Text = "Step III. Selection of Compliance On"
            lblStep4.Text = "Step IV. Selection of Compliance of"
            'ElseIf ReportType = 4 Then
        ElseIf Session("Title").ToString() = 4 Then
            'lbltitle.Text = "Search criteria for Common History"
            lbltitle.Text = "Common History Register"
            lblType.Text = "To/From/of : "
            lblRemovalFrom.Text = "To/From/On : "
            lblRemovalof.Text = "On/of : "
            lblStep3.Text = "Step III. Selection of Installation To/Removal From/Compliance On"
            lblStep4.Text = "Step IV. Selection of Installation/Removal/Compliance On/of"
        End If
    End Sub
    Public Sub NewPageofModel(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Session("mAssemblyList") = Nothing
        dgModel.PageIndex = e.NewPageIndex
        'mAssemblyList = AssemblyList.GetAssemblyList("", "", cmbAssemblyType.SelectedIndex)
        mAssemblyList = AssemblyList.GetAssemblyList("", "", cmbAssemblyType.SelectedIndex, , Today.Date.ToString)
        Session("mAssemblyList") = mAssemblyList
        dgModel.DataSource = mAssemblyList
        dgModel.DataBind()
        'lblResult.Text = "List of Model & Serial Nos.: " & mAssemblyList.Count & " Record(s) found."
    End Sub
    Public Sub NewPageofEModel(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        'Commented by Saylee on 5-May-2011
        ''If AssemblyTypeIndex = 0 Then
        ''    Session("mAssemblyList") = Nothing
        ''    dgEModel.CurrentPageIndex = e.NewPageIndex
        ''    mAssemblyList = AssemblyList.GetAssemblyList("", "", 0, , Today.Date.ToString)
        ''    dgEModel.DataSource = mAssemblyList
        ''    Session("mAssemblyList") = mAssemblyList
        ''    dgEModel.DataBind()
        ''    ' lblResult3.Text = "List of Model & Serial Nos.: " & mAssemblyList.Count & " Record(s) found."
        ''ElseIf AssemblyTypeIndex = 1 Then
        ''    dgAModel.CurrentPageIndex = e.NewPageIndex
        ''    ' mRptAssemblyList = ReportAssemblyList.GetReportAssemblyList("", Trim(txtFromDate.Text), Trim(txtToDate.Text), txtAModelNo.Text.Trim, txtASerialNo.Text.Trim, 1)
        ''    dgAModel.DataSource = mRptAssemblyList
        ''    Session("mRptAssemblyList") = mRptAssemblyList
        ''    dgAModel.DataBind()
        ''    '  lblResult1.Text = "List of Model & Serial Nos.: " & mRptAssemblyList.Count & " Record(s) found."
        ''End If

        'Added by Saylee on 5-May-2011
        Session("mEAssemblyList") = Nothing
        dgEModel.PageIndex = e.NewPageIndex
        mEAssemblyList = AssemblyList.GetAssemblyList("", "", 0, , Today.Date.ToString)
        dgEModel.DataSource = mEAssemblyList
        Session("mEAssemblyList") = mEAssemblyList
        dgEModel.DataBind()

    End Sub
    Public Sub NewPageofPart(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgPart.PageIndex = e.NewPageIndex
        ' mCompList = CompList.GetCompList("", "", "", cmbAssemblyType.SelectedIndex)
        dgPart.DataSource = mCompList
        Session("mCompList") = mCompList
        dgPart.DataBind()
        ' lblResult2.Text = "List of Part & Serial Nos.: " & mCompList.Count & " Record(s) found."
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForHistory.aspx?"
            DataFieldBind()
            pnlAModel.Visible = False
            'Commented by Saylee on 28-July-2009
            ''chkAssembly.Checked = True
            ''chkComponent.Checked = True

            'Added by Saylee on 28-July-2009
            chkAssembly.Checked = False
            chkComponent.Checked = False
            'Session("ReportType") = 1
            ReportType = CType(Request.QueryString("ReportType"), Integer)

            If ReportType <> 0 Then
                Session("Title") = ReportType
            End If


            Session("lblRemovalFrom.Text") = lblRemovalFrom.Text
            Session("lblRemovalof.Text") = lblRemovalof.Text
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            setFocus(txtWorkOrderNo)
            ResetValues()
            lblResult.Text = "List of Model & Serial No.s : " & mAssemblyList.Count & " Record(s) found."
            lblResult3.Text = "List of Model & Serial No.s : " & mAssemblyList.Count & " Record(s) found."
            lblResult2.Text = "List of Part & Serial No.s : " & mCompList.Count & " Record(s) found."
            ControlVisibility(True, True)
        End If
        Title()
        SetSession()
        MessageBoxResult()
    End Sub

    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If (chkAssembly.Checked = False And chkComponent.Checked = False) Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select either Assembly or Component", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfSearchCriteriaForHistory.aspx?Backpage=&ReportType=" & Request.QueryString("ReportType")
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select either Assembly or Component", MsgBoxStyle.OkOnly, "")

            Exit Sub
        End If
        If IsValid = True Then
            SetReport(ReportType)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    'Private Sub cmbAssemblyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssemblyType.SelectedIndexChanged
    '    If (cmbAssemblyType.SelectedIndex = 2 Or cmbAssemblyType.SelectedIndex = 3 Or cmbAssemblyType.SelectedIndex = 4 Or cmbAssemblyType.SelectedIndex = 5 Or cmbAssemblyType.SelectedIndex = 6) Then
    '        AssemblyStatus2()
    '    ElseIf (cmbAssemblyType.SelectedIndex = 0) Then
    '        AssemblyStatus1()
    '        pnlEModel.Visible = True
    '        pnlAModel.Visible = False
    '        dgEModel.CurrentPageIndex = 0
    '        ListAModelNo = IIf(txtAModelNo.Text <> "", Trim(txtAModelNo.Text), "")
    '        ListASerialNo = IIf(txtASerialNo.Text <> "", Trim(txtASerialNo.Text), "")
    '        mAssType = AssemblyTypeIndex
    '        Session("ListAModelNo") = ListAModelNo
    '        Session("ListASerialNo") = ListASerialNo
    '        Session("mAssType") = mAssType
    '        FindNowEModel(ListAModelNo, ListASerialNo, mAssType)
    '    ElseIf (cmbAssemblyType.SelectedIndex = 1) Then
    '        AssemblyStatus1()
    '        ''pnlEModel.Visible = False
    '        pnlEModel.Visible = True
    '        pnlAModel.Visible = True
    '        MacID1 = ""
    '        ListMacID1 = ""
    '        'SetModel()
    '        dgAModel.CurrentPageIndex = 0
    '        ListAModelNo = IIf(txtAModelNo.Text <> "", Trim(txtAModelNo.Text), "")
    '        ListASerialNo = IIf(txtASerialNo.Text <> "", Trim(txtASerialNo.Text), "")
    '        ReportType = ReportType
    '        ListStartDate = IIf(txtFromDate.Value.ToString <> "", (txtFromDate.Value.ToString), "")
    '        ListEndDate = IIf(txtToDate.Value.ToString <> "", (txtToDate.Value.ToString), "")
    '        ListMacID1 = MacID1
    '        Session("ListAModelNo") = ListAModelNo
    '        Session("ListASerialNo") = ListASerialNo
    '        Session("ReportType") = ReportType
    '        Session("ListStartDate") = ListStartDate
    '        Session("ListEndDate") = ListEndDate
    '        Session("ListMacID1") = ListMacID1
    '        FindNowAModel("{00000000-0000-0000-0000-000000000000}", ListStartDate, ListEndDate, ListAModelNo, ListASerialNo, ReportType)
    '    End If
    '    If Not AssemblyIndex.Equals(cmbAssemblyType.SelectedIndex) Then
    '        txtModelNo.Text = ""
    '        txtSerialNo.Text = ""
    '        txtAModelNo.Text = ""
    '        txtASerialNo.Text = ""
    '        txtCPartNo.Text = ""
    '        txtCSerialNo.Text = ""
    '    End If
    '    If cmbAssemblyType.SelectedIndex = 0 Then
    '        AssemblyTypeIndex = 0
    '        AssemblyIndex = 0
    '    Else
    '        AssemblyTypeIndex = cmbAssemblyType.SelectedIndex
    '        AssemblyIndex = cmbAssemblyType.SelectedIndex
    '    End If
    '    Session("AssemblyTypeIndex") = AssemblyTypeIndex
    '    Session("AssemblyIndex") = AssemblyIndex

    '    If cmbAssemblyType.Enabled = True Then
    '        SetFocus(cmbAssemblyType)
    '    End If
    'End Sub
    Private Sub chkAssembly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAssembly.CheckedChanged
        If ((chkAssembly.Checked) And (chkComponent.Checked)) Then
            chkAss = True
            chkComp = True
        ElseIf ((chkAssembly.Checked) And (chkComponent.Checked = False)) Then
            chkAss = True
            chkComp = False
        ElseIf ((chkAssembly.Checked = False) And (chkComponent.Checked)) Then
            chkAss = False
            chkComp = True
        ElseIf ((chkAssembly.Checked = False) And (chkComponent.Checked = False)) Then
            chkAss = False
            chkComp = False
        End If
        If ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkAss = True And chkComp = True)) Then
            SetAssComp()
        ElseIf ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkAss = True And chkComp = False)) Then
            SetAss()
        ElseIf ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkComp = True And chkAss = False)) Then
            SetComp()
        ElseIf ((AssemblyTypeIndex = 2 Or AssemblyTypeIndex = 3 Or AssemblyTypeIndex = 4 Or AssemblyTypeIndex = 5 Or AssemblyTypeIndex = 6) And (chkComp = True And chkAss = False)) Then
            SetComp()
        End If
        'If (chkAss = False And chkComp = False) Then
        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select either Assembly or Component", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfSearchCriteriaForHistory.aspx?Backpage=&ReportType=" & Request.QueryString("ReportType")
        '    msg1.Show()
        '    Exit Sub
        'End If

        ControlVisibility(True, False)

    End Sub
    Private Sub chkComponent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkComponent.CheckedChanged
        If ((chkAssembly.Checked) And (chkComponent.Checked)) Then
            chkAss = True
            chkComp = True
        ElseIf ((chkAssembly.Checked) And (chkComponent.Checked = False)) Then
            chkAss = True
            chkComp = False
        ElseIf ((chkAssembly.Checked = False) And (chkComponent.Checked)) Then
            chkAss = False
            chkComp = True
        ElseIf ((chkAssembly.Checked = False) And (chkComponent.Checked = False)) Then
            chkAss = False
            chkComp = False
        End If
        If ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkAss = True And chkComp = True)) Then
            SetAssComp()
        ElseIf ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkAss = True And chkComp = False)) Then
            SetAss()
        ElseIf ((AssemblyTypeIndex = 0 Or AssemblyTypeIndex = 1) And (chkComp = True And chkAss = False)) Then
            SetComp()
        ElseIf ((AssemblyTypeIndex = 2 Or AssemblyTypeIndex = 3 Or AssemblyTypeIndex = 4 Or AssemblyTypeIndex = 5 Or AssemblyTypeIndex = 6) And (chkComp = True And chkAss = False)) Then
            SetComp()
        End If
        'If (chkAss = False And chkComp = False) Then
        '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select either Assembly or Component", MsgBoxStyle.OKOnly)
        '    msg1.ReplacePage = "wfSearchCriteriaForHistory.aspx?Backpage=&ReportType=" & Request.QueryString("ReportType")
        '    msg1.Show()
        '    Exit Sub
        'End If
        ControlVisibility(False, True)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        chkFindNow = True
        Session("chkFindNow") = chkFindNow
        dgModel.PageIndex = 0
        ListModel = IIf(txtModelNo.Text <> "", Trim(txtModelNo.Text), "")
        ListSerialNo = IIf(txtSerialNo.Text <> "", Trim(txtSerialNo.Text), "")
        mAssType = AssemblyTypeIndex
        Session("ListModel") = ListModel
        Session("ListSerialNo") = ListSerialNo
        Session("mAssType") = mAssType
        FindNowModel(ListModel, ListSerialNo, mAssType)
    End Sub
    'Private Sub dgModel_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgModel.ItemCommand
    Private Sub dgModel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgModel.RowCommand
        'Dim Index As Int16 = e.Item.ItemIndex + dgModel.CurrentPageIndex * dgModel.PageSize


        Select Case e.CommandName
            Case "Select"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgModel.PageIndex * dgModel.PageSize
                chkFindNow = True
                Session("chkFindNow") = chkFindNow
                ClearControlsofModel()
                ListModel = mAssemblyList(Index).ModelName
                ListSerialNo = mAssemblyList(Index).SerialNo
                ListID = mAssemblyList(Index).ID
                txtModelNo.Text = ListModel
                txtSerialNo.Text = ListSerialNo
                Session("ListModel") = ListModel
                Session("ListSerialNo") = ListSerialNo
                Session("ListID") = ListID
        End Select
    End Sub

    Private Sub dgModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgModel.PageIndexChanging
        dgModel.PageIndex = e.NewPageIndex
        Session("mAssemblyList") = mAssemblyList
        dgModel.DataSource = mAssemblyList
        dgModel.DataBind()
    End Sub

    Private Sub btnFindModel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindModel.Click
        chkFindModel = True
        Session("chkFindModel") = chkFindModel
        If AssemblyTypeIndex = 1 Then
            pnlEModel.Visible = False
            pnlAModel.Visible = True
            MacID1 = ""
            ListMacID1 = ""
            SetModel()
            dgAModel.PageIndex = 0
            ListAModelNo = IIf(txtAModelNo.Text <> "", Trim(txtAModelNo.Text), "")
            ListASerialNo = IIf(txtASerialNo.Text <> "", Trim(txtASerialNo.Text), "")
            ReportType = ReportType
            ListStartDate = IIf(txtFromDate.Text.ToString <> "", (txtFromDate.Text.ToString), "")
            ListEndDate = IIf(txtToDate.Text.ToString <> "", (txtToDate.Text.ToString), "")
            ListMacID1 = IIf(MacID1 = "", "{00000000-0000-0000-0000-000000000000}", MacID1)
            Session("ListAModelNo") = ListAModelNo
            Session("ListASerialNo") = ListASerialNo
            Session("ReportType") = ReportType
            Session("ListStartDate") = ListStartDate
            Session("ListEndDate") = ListEndDate
            Session("ListMacID1") = ListMacID1
            FindNowAModel(ListMacID1, ListStartDate, ListEndDate, ListAModelNo, ListASerialNo, ReportType)
        ElseIf AssemblyTypeIndex = 0 Then
            pnlEModel.Visible = True
            pnlAModel.Visible = False
            dgEModel.PageIndex = 0
            ListAModelNo = IIf(txtAModelNo.Text <> "", Trim(txtAModelNo.Text), "")
            ListASerialNo = IIf(txtASerialNo.Text <> "", Trim(txtASerialNo.Text), "")
            mAssType = AssemblyTypeIndex
            Session("ListAModelNo") = ListAModelNo
            Session("ListASerialNo") = ListASerialNo
            Session("mAssType") = mAssType
            FindNowEModel(ListAModelNo, ListASerialNo, mAssType)
        End If
    End Sub
    'Private Sub dgAModel_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAModel.ItemCommand
    Private Sub dgAModel_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAModel.RowCommand
        ' Dim Index As Int16 = e.Item.ItemIndex + dgAModel.CurrentPageIndex * dgAModel.PageSize

        Select Case e.CommandName
            Case "Select"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgAModel.PageIndex * dgAModel.PageSize
                chkFindModel = True
                Session("chkFindModel") = chkFindModel
                ClearControlsofAModel()
                ListAModelNo = mRptAssemblyList(Index).ModelName
                ListASerialNo = mRptAssemblyList(Index).SerialNo
                txtAModelNo.Text = ListAModelNo
                txtASerialNo.Text = ListASerialNo
                Session("ListAModelNo") = ListAModelNo
                Session("ListASerialNo") = ListASerialNo
                'pnlAModel.Visible = False
        End Select
    End Sub
    'Private Sub dgEModel_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEModel.ItemCommand
    Private Sub dgEModel_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEModel.RowCommand
        'Dim Index As Int16 = e.Item.ItemIndex + dgEModel.CurrentPageIndex * dgEModel.PageSize


        Select Case e.CommandName
            Case "Select"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgEModel.PageIndex * dgEModel.PageSize
                chkFindModel = True
                Session("chkFindModel") = chkFindModel
                ' ClearControlsofModel()
                ClearControlsofAModel()
                mEAssemblyList = CType(Session("mEAssemblyList"), AssemblyList)
                ListAModelNo = mEAssemblyList(Index).ModelName
                ListASerialNo = mEAssemblyList(Index).SerialNo
                txtAModelNo.Text = ListAModelNo
                txtASerialNo.Text = ListASerialNo
                Session("ListAModelNo") = ListAModelNo
                Session("ListASerialNo") = ListASerialNo
                'pnlEModel.Visible = False
        End Select
    End Sub

    Private Sub dgEModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEModel.PageIndexChanging
        dgEModel.PageIndex = e.NewPageIndex
        dgEModel.DataSource = mEAssemblyList
        Session("mEAssemblyList") = mEAssemblyList
        dgEModel.DataBind()
        ControlVisibility(True, False)
    End Sub

    Private Sub btnFindPart_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindPart.Click
        dgPart.PageIndex = 0
        ListPartNo = IIf(txtCPartNo.Text <> "", Trim(txtCPartNo.Text), "")
        ListCompSerialNo = IIf(txtCSerialNo.Text <> "", Trim(txtCSerialNo.Text), "")
        mAssType = AssemblyTypeIndex
        ListEndDate = IIf(txtToDate.Text.ToString <> "", (txtToDate.Text.ToString), "")
        Session("ListPartNo") = ListPartNo
        Session("ListCompSerialNo") = ListCompSerialNo
        Session("mAssType") = mAssType
        Session("ListEndDate") = ListEndDate
        FindNowPart(ListPartNo, ListCompSerialNo, mAssType, ListEndDate)
    End Sub
    'Private Sub dgPart_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPart.ItemCommand
    Private Sub dgPart_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPart.RowCommand
        'Dim Index As Int16 = e.Item.ItemIndex + dgPart.CurrentPageIndex * dgPart.PageSize



        Select Case e.CommandName
            Case "Select"
                Dim Index As Int16 = CInt(e.CommandArgument) + dgPart.PageIndex * dgPart.PageSize
                ClearControlsofPart()
                ListPartNo = mCompList(Index).PartName
                ListCompSerialNo = mCompList(Index).SerialNo
                txtCPartNo.Text = ListPartNo
                txtCSerialNo.Text = ListCompSerialNo
                Session("ListPartNo") = ListPartNo
                Session("ListCompSerialNo") = ListCompSerialNo
        End Select
    End Sub

    Private Sub dgPart_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPart.PageIndexChanging
        dgPart.PageIndex = e.NewPageIndex
        dgPart.DataSource = mCompList
        Session("mCompList") = mCompList
        dgPart.DataBind()
        ControlVisibility(False, True)
    End Sub

    Private Sub txtModelNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtModelNo.TextChanged
        chkModel = True
        Session("chkModel") = chkModel
    End Sub
    Private Sub txtSerialNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSerialNo.TextChanged
        chkSerialNo = True
        Session("chkSerialNo") = chkSerialNo
    End Sub
    Private Sub txtAModelNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAModelNo.TextChanged
        chkAModel = True
        Session("chkAModel") = chkAModel
    End Sub
    Private Sub txtASerialNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtASerialNo.TextChanged
        chkASerialNo = True
        Session("chkASerialNo") = chkASerialNo
    End Sub
    'Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
    '    Me.cmbAssemblyType.Visible = Not CType(sender, Boolean)
    'End Sub
    'Added By Rahul 18-June-2009 for grid sorting
    'Private Sub dgAModel_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgAModel.SortCommand
    Private Sub dgAModel_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAModel.Sorting
        mRptAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRptAssemblyList") = mRptAssemblyList
        dgAModel.DataSource = mRptAssemblyList
        dgAModel.DataBind()
    End Sub
    'Private Sub dgEModel_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgEModel.SortCommand
    Private Sub dgEModel_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgEModel.Sorting
        mEAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mEAssemblyList") = mEAssemblyList
        dgEModel.DataSource = mEAssemblyList
        dgEModel.DataBind()
    End Sub
    'Private Sub dgPart_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgPart.SortCommand
    Private Sub dgPart_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPart.Sorting
        mCompList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompList") = mCompList
        dgPart.DataSource = mCompList
        dgPart.DataBind()
    End Sub
    'Private Sub dgModel_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgModel.SortCommand
    Private Sub dgModel_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgModel.Sorting
        mAssemblyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAssemblyList") = mAssemblyList
        dgModel.DataSource = mAssemblyList
        dgModel.DataBind()
    End Sub
    '---------------------------------------------------
#End Region


End Class
