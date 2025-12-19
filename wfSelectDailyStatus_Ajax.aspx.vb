
'Created by : Saylee
'Date       : 21-Dec-2009


Partial Class wfSelectDailyStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mModelMonitorServiceList As ModelMonitorServiceList
    Public mModelMonitorInspList As ModelMonitorInspList
    Public mModelMonitorModList As ModelMonitorModList

    Public mPartMonitorServiceList As PartMonitorServiceList
    Public mPartMonitorInspList As PartMonitorInspList
    Public mPartMonitorModList As PartMonitorModList

    Public mRenewMachineCertificateList As MachineCertificateList

    Public mDailyStatus As DailyStatus
    Public mDailyStatusList As DailyStatusList
    Public mDailyStatusCertificateList As DailyStatusList

    Dim mBoardTypeID As Integer
    Dim MaintenanceActivityTypeID As Integer
    Dim ModelID As String
    Dim mtmpMachineID As String
    Dim mPartID As String
    Dim Aircraft As String
    Dim RegNo As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mBoardTypeID = Session("BoardTypeID")
        mDailyStatusList = Session("mDailyStatusList")
        MaintenanceActivityTypeID = Session("MaintenanceActivityTypeID")
        mDailyStatusCertificateList = Session("mDailyStatusCertificateList")

        Select Case MaintenanceActivityTypeID

            Case 1  'Model Service
                mModelMonitorServiceList = CType(Session("mModelMonitorServiceList"), ModelMonitorServiceList)
            Case 2  'Model Inspection
                mModelMonitorInspList = CType(Session("mModelMonitorInspList"), ModelMonitorInspList)
            Case 3  'Model Directive
                mModelMonitorModList = CType(Session("mModelMonitorModList"), ModelMonitorModList)
            Case 4 'Part Service
                mPartMonitorServiceList = CType(Session("mPartMonitorServiceList"), PartMonitorServiceList)
            Case 5  'Part Inspection
                mPartMonitorInspList = CType(Session("mPartMonitorInspList"), PartMonitorInspList)
            Case 6  'Part Directive
                mPartMonitorModList = CType(Session("mPartMonitorModList"), PartMonitorModList)
            Case 7 'Certificate 
                mRenewMachineCertificateList = Session("mRenewMachineCertificateList")
        End Select


        mPartID = Session("mPartID")

        MaintenanceActivityTypeID = Session("MaintenanceActivityTypeID")
        ModelID = Session("ModelID")
        mtmpMachineID = Session("mtmpMachineID")
        RegNo = Session("RegNo")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Session("sender") = ""
                    'Response.Redirect("wfSelectDailyStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo & "&MaintenanceActivityTypeID=" & MaintenanceActivityTypeID & "&ModelID=" & ModelID)
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'Response.Redirect("wfSelectDailyStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo & "&MaintenanceActivityTypeID=" & MaintenanceActivityTypeID & "&ModelID=" & ModelID)
                Case MsgBoxResult.OK And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfSelectDailyStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo & "&MaintenanceActivityTypeID=" & MaintenanceActivityTypeID & "&ModelID=" & ModelID)
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    'DataFieldBind()
                    'Response.Redirect("wfSelectDailyStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo & "&MaintenanceActivityTypeID=" & MaintenanceActivityTypeID & "&ModelID=" & ModelID)
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'Response.Redirect("wfSelectDailyStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo & "&MaintenanceActivityTypeID=" & MaintenanceActivityTypeID & "&ModelID=" & ModelID)
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SelectModelServiceRecord(ByVal Index As Int32)
        mDailyStatus = DailyStatus.NewChildDailyStatus(New Guid(), 1, MaintenanceActivityTypeID, mModelMonitorServiceList(Index).ID, ModelID.ToString)
        Session("mDailyStatus") = mDailyStatus
        If Not mDailyStatusList.Contains(mModelMonitorServiceList(Index).ID, "") Then
            mDailyStatusList.Add(mDailyStatus)
        End If
        Session("mDailyStatusList") = mDailyStatusList
        Session("FromSelectInfo") = "FromSelectInfo"
        ' Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        'Response.Redirect("index.aspx")
    End Sub
    Private Sub SelectModelInspRecord(ByVal Index As Int32)
        mDailyStatus = DailyStatus.NewChildDailyStatus(New Guid(mtmpMachineID), 2, MaintenanceActivityTypeID, mModelMonitorInspList(Index).ID, ModelID.ToString)
        Session("mDailyStatus") = mDailyStatus
        If Not mDailyStatusList.Contains(mModelMonitorInspList(Index).ID, "") Then
            mDailyStatusList.Add(mDailyStatus)
        End If
        Session("mDailyStatusList") = mDailyStatusList
        Session("FromSelectInfo") = "FromSelectInfo"
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        'Response.Redirect("index.aspx")
    End Sub

    Private Sub SelectModelModRecord(ByVal Index As Int32)
        mDailyStatus = DailyStatus.NewChildDailyStatus(New Guid(mtmpMachineID), 3, MaintenanceActivityTypeID, mModelMonitorModList(Index).ID, ModelID.ToString)
        Session("mDailyStatus") = mDailyStatus
        If Not mDailyStatusList.Contains(mModelMonitorModList(Index).ID, "") Then
            mDailyStatusList.Add(mDailyStatus)
        End If
        Session("mDailyStatusList") = mDailyStatusList
        Session("FromSelectInfo") = "FromSelectInfo"
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        'Response.Redirect("index.aspx")
    End Sub
    Private Sub SelectPartServiceRecord(ByVal Index As Int32)
        mDailyStatus = DailyStatus.NewChildDailyStatus(New Guid(mtmpMachineID), 1, MaintenanceActivityTypeID, mPartMonitorServiceList(Index).ID, ModelID.ToString)
        Session("mDailyStatus") = mDailyStatus
        If Not mDailyStatusList.Contains(mPartMonitorServiceList(Index).ID, "") Then
            mDailyStatusList.Add(mDailyStatus)
        End If
        Session("mDailyStatusList") = mDailyStatusList
        Session("FromSelectInfo") = "FromSelectInfo"

    End Sub
    Private Sub SelectPartInspRecord(ByVal Index As Int32)
        mDailyStatus = DailyStatus.NewChildDailyStatus(New Guid(mtmpMachineID), 2, MaintenanceActivityTypeID, mPartMonitorInspList(Index).ID, ModelID.ToString)
        Session("mDailyStatus") = mDailyStatus
        If Not mDailyStatusList.Contains(mPartMonitorInspList(Index).ID, "") Then
            mDailyStatusList.Add(mDailyStatus)
        End If
        Session("mDailyStatusList") = mDailyStatusList
        Session("FromSelectInfo") = "FromSelectInfo"
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ' Response.Redirect("index.aspx")
    End Sub

    Private Sub SelectPartModRecord(ByVal Index As Int32)
        mDailyStatus = DailyStatus.NewChildDailyStatus(New Guid(mtmpMachineID), 3, MaintenanceActivityTypeID, mPartMonitorModList(Index).ID, ModelID.ToString)
        Session("mDailyStatus") = mDailyStatus
        If Not mDailyStatusList.Contains(mPartMonitorModList(Index).ID, "") Then
            mDailyStatusList.Add(mDailyStatus)
        End If
        Session("mDailyStatusList") = mDailyStatusList
        Session("FromSelectInfo") = "FromSelectInfo"
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        'Response.Redirect("index.aspx")
    End Sub
    Private Sub SelectCertificateRecord(ByVal Index As Int32)
        mDailyStatus = DailyStatus.NewChildDailyStatus(New Guid(mtmpMachineID), 4, MaintenanceActivityTypeID, mRenewMachineCertificateList(Index).ID, Guid.Empty.ToString)
        Session("mDailyStatus") = mDailyStatus
        If Not mDailyStatusCertificateList.Contains(mRenewMachineCertificateList(Index).ID, "") Then
            mDailyStatusCertificateList.Add(mDailyStatus)
        End If
        Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
        Session("FromSelectInfo") = "FromSelectInfo"
    End Sub
#End Region

#Region " Data Bindings "
    Private Sub DataFieldBind()

        Select Case MaintenanceActivityTypeID

            Case 1  'Model Service
                mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(New Guid(ModelID))
                dgSelectInformationList.DataSource = mModelMonitorServiceList
                Session("mModelMonitorServiceList") = mModelMonitorServiceList
                dgSelectInformationList.Columns(6).Visible = False
                lblSelectInformation.Text = "List of Services: " & mModelMonitorServiceList.Count & " Record(s) found "
            Case 2  'Model Inspection
                mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(New Guid(ModelID))
                dgSelectInformationList.DataSource = mModelMonitorInspList
                Session("mModelMonitorInspList") = mModelMonitorInspList
                dgSelectInformationList.Columns(6).Visible = False
                lblSelectInformation.Text = "List of Inspections: " & mModelMonitorInspList.Count & " Record(s) found "
            Case 3  'Model Directive
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(New Guid(ModelID))
                dgSelectInformationList.DataSource = mModelMonitorModList
                Session("mModelMonitorModList") = mModelMonitorModList
                lblSelectInformation.Text = "List of Directives: " & mModelMonitorModList.Count & " Record(s) found "
            Case 4 'Part Service
                mPartMonitorServiceList = PartMonitorServiceList.GetPartMonitorServiceList(New Guid(mPartID), New Guid(ModelID))
                dgSelectInformationList.DataSource = mPartMonitorServiceList
                Session("mPartMonitorServiceList") = mPartMonitorServiceList
                dgSelectInformationList.Columns(6).Visible = False
                lblSelectInformation.Text = "List of Services: " & mPartMonitorServiceList.Count & " Record(s) found "
            Case 5  'Part Inspection
                'Commented & Added By Vikrant For MPD
                ' mPartMonitorInspList = PartMonitorInspList.GetPartMonitorInspList(New Guid(mPartID), New Guid(ModelID))
                mPartMonitorInspList = PartMonitorInspList.GetPartMonitorInspList(New Guid(mPartID), Guid.Empty) 'Pass Blank ModelID as it was Commenetd in SP
                'End
                dgSelectInformationList.DataSource = mPartMonitorInspList
                Session("mPartMonitorInspList") = mPartMonitorInspList
                dgSelectInformationList.Columns(6).Visible = False
                lblSelectInformation.Text = "List of Inspections: " & mPartMonitorInspList.Count & " Record(s) found "
            Case 6  'Part Directive
                mPartMonitorModList = PartMonitorModList.GetPartMonitorModList(New Guid(mPartID), New Guid(ModelID))
                dgSelectInformationList.DataSource = mPartMonitorModList
                Session("mPartMonitorModList") = mPartMonitorModList
                lblSelectInformation.Text = "List of Modifications: " & mPartMonitorModList.Count & " Record(s) found "
            Case 7 'Certificate 
                dgCertificateList.Visible = True
                dgSelectInformationList.Visible = False
                mRenewMachineCertificateList = MachineCertificateList.GetMachineCertificateList(New Guid(mtmpMachineID.ToString), Today.Date.ToString)
                dgCertificateList.DataSource = mRenewMachineCertificateList
                Session("mRenewMachineCertificateList") = mRenewMachineCertificateList
                lblSelectInformation.Text = "List of Certificates: " & mRenewMachineCertificateList.Count & " Record(s) found "
                dgCertificateList.ShowHeaderWhenEmpty = True
        End Select
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack And CType(Session("Sender"), String) = "" Then
            MaintenanceActivityTypeID = CType(Request.QueryString("MaintenanceActivityTypeID"), Integer)
            ModelID = Request.QueryString("ModelID")
            mtmpMachineID = Request.QueryString("MachineID")
            RegNo = Request.QueryString("RegNo")
            Session("MaintenanceActivityTypeID") = MaintenanceActivityTypeID
            Session("ModelID") = ModelID
            Session("mtmpMachineID") = mtmpMachineID
            Session("RegNo") = RegNo
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        Dim mIsSelect As Boolean = False
        Select Case MaintenanceActivityTypeID
            Case 1  'Model Service
                For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If
                Next

                If mIsSelect = True Then
                    For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            Dim index As Integer = dgSelectInformationList.PageSize * dgSelectInformationList.PageIndex + i
                            SelectModelServiceRecord(index)
                        End If
                    Next
                    Session("mDailyStatusList") = mDailyStatusList
                    ''Response.Redirect("index.aspx")
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                End If

            Case 2  'Inspection
                For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If
                Next

                If mIsSelect = True Then
                    For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            Dim index As Integer = dgSelectInformationList.PageSize * dgSelectInformationList.PageIndex + i
                            SelectModelInspRecord(index)
                        End If
                    Next
                    Session("mDailyStatusList") = mDailyStatusList
                    'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    '' Response.Redirect("index.aspx")
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)

                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                End If
            Case 3  'Directive
                For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If
                Next

                If mIsSelect = True Then
                    For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            Dim index As Integer = dgSelectInformationList.PageSize * dgSelectInformationList.PageIndex + i
                            SelectModelModRecord(index)
                        End If
                    Next
                    Session("mDailyStatusList") = mDailyStatusList
                    'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    ''Response.Redirect("index.aspx")
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)

                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                End If

            Case 4  'Part Service
                For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If
                Next

                If mIsSelect = True Then
                    For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            Dim index As Integer = dgSelectInformationList.PageSize * dgSelectInformationList.PageIndex + i
                            SelectPartServiceRecord(index)
                        End If
                    Next
                    Session("mDailyStatusList") = mDailyStatusList
                    'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    ''Response.Redirect("index.aspx")
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)

                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                End If

            Case 5  'Part Inspection
                For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If
                Next

                If mIsSelect = True Then
                    For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            Dim index As Integer = dgSelectInformationList.PageSize * dgSelectInformationList.PageIndex + i
                            SelectPartInspRecord(index)
                        End If
                    Next
                    Session("mDailyStatusList") = mDailyStatusList
                    'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    ''Response.Redirect("index.aspx")
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)

                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                End If
            Case 6  'Part Directive
                For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                    If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If
                Next

                If mIsSelect = True Then
                    For i As Integer = 0 To dgSelectInformationList.Rows.Count - 1
                        If CType(dgSelectInformationList.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                            Dim index As Integer = dgSelectInformationList.PageSize * dgSelectInformationList.PageIndex + i
                            SelectPartModRecord(index)
                        End If
                    Next
                    Session("mDailyStatusList") = mDailyStatusList
                    'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    ''Response.Redirect("index.aspx")
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                End If
            Case 7 'Certificate

                For i As Integer = 0 To dgCertificateList.Rows.Count - 1
                    If CType(dgCertificateList.Rows(i).FindControl("chkSelectCertificate"), CheckBox).Checked = True Then
                        mIsSelect = True
                        Exit For
                    Else
                        mIsSelect = False
                    End If
                Next

                If mIsSelect = True Then
                    For i As Integer = 0 To dgCertificateList.Rows.Count - 1
                        If CType(dgCertificateList.Rows(i).FindControl("chkSelectCertificate"), CheckBox).Checked = True Then
                            Dim index As Integer = dgCertificateList.PageSize * dgCertificateList.PageIndex + i
                            SelectCertificateRecord(index)
                        End If
                    Next
                    Session("mDailyStatusList") = mDailyStatusList
                    Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
                    'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    ''Response.Redirect("index.aspx")
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)

                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
                End If
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("mDailyStatusList") = mDailyStatusList
        Session("mDailyStatusCertificateList") = mDailyStatusCertificateList
        ''Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ''Response.Redirect("Index.aspx")
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&MachineID=" & mtmpMachineID.ToString & "&RegNo=" & RegNo)

    End Sub
    Private Sub dgCertificateList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCertificateList.PageIndexChanging
        dgCertificateList.PageIndex = e.NewPageIndex
        dgCertificateList.DataSource = mRenewMachineCertificateList
        Session("mRenewMachineCertificateList") = mRenewMachineCertificateList
        dgCertificateList.DataBind()
    End Sub
    Private Sub dgCertificateList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCertificateList.RowCommand
        Dim Index As Int16 = CInt(e.CommandArgument) + dgCertificateList.PageSize * dgCertificateList.PageIndex
        SelectCertificateRecord(Index)
    End Sub
    Private Sub dgSelectInformationList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSelectInformationList.PageIndexChanging
        dgSelectInformationList.PageIndex = e.NewPageIndex
        Select Case MaintenanceActivityTypeID
            Case 1  'Assembly Service
                dgSelectInformationList.DataSource = mModelMonitorServiceList
                Session("mModelMonitorServiceList") = mModelMonitorServiceList
                dgSelectInformationList.DataBind()
            Case 2  'Assembly Inspection
                dgSelectInformationList.DataSource = mModelMonitorInspList
                Session("mModelMonitorInspList") = mModelMonitorInspList
                dgSelectInformationList.DataBind()
            Case 3  'Assembly Directive
                dgSelectInformationList.DataSource = mModelMonitorModList
                Session("mModelMonitorModList") = mModelMonitorModList
                dgSelectInformationList.DataBind()
        End Select
    End Sub
    Private Sub dgSelectInformationList_Sorting(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSelectInformationList.Sorting
        Select Case MaintenanceActivityTypeID
            Case 1  'Assembly Service
                mModelMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                mModelMonitorServiceList = Session("mModelMonitorServiceList")
                dgSelectInformationList.DataSource = mModelMonitorServiceList
                DataBind()
            Case 2  'Assembly Inspection
                mModelMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                mModelMonitorInspList = Session("mModelMonitorInspList")
                dgSelectInformationList.DataSource = mModelMonitorInspList
                DataBind()
            Case 3  'Assembly Directive
                mModelMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                mModelMonitorModList = Session("mModelMonitorModList")
                dgSelectInformationList.DataSource = mModelMonitorModList
                DataBind()
            Case 4  'Component Service
                mPartMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                mPartMonitorServiceList = Session("mPartMonitorServiceList")
                dgSelectInformationList.DataSource = mPartMonitorServiceList
                DataBind()
            Case 5  'Component Inspection
                mPartMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                mPartMonitorInspList = Session("mPartMonitorInspList")
                dgSelectInformationList.DataSource = mPartMonitorInspList
                DataBind()
            Case 6  'Component Directive
                mPartMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                mPartMonitorModList = Session("mPartMonitorModList")
                dgSelectInformationList.DataSource = mPartMonitorModList
                DataBind()
        End Select

    End Sub
    Private Sub dgCertificateList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCertificateList.Sorting
        mRenewMachineCertificateList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        mRenewMachineCertificateList = Session("mRenewMachineCertificateList")
        dgCertificateList.DataSource = mRenewMachineCertificateList
        DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

   
End Class
