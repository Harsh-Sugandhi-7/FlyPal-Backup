'Created By Utkarsh On 06-Jan-2012 For Link Maintenance

Partial Class wfModelMonitorActivityList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variable Declaration"
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mModelMonitorService As ModelMonitorService
    Public mModelMonitorServiceList As ModelMonitorServiceList
    Public mModelMonitorInspList As ModelMonitorInspList
    Public mModelMonitorModList As ModelMonitorModList
    'Dim Type As Int32
    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance

    Dim FromType As Integer = 0
    Dim MaintenanceActivityID As Guid

    Dim mAllLinkMaintenanceList As LinkMaintenanceList
    Dim ModelID As Guid 'Added By Vikrant For MPD
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mAssemblyMonitorServiceStatus = CType(Session("mAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mModelMonitorService = CType(Session("mModelMonitorService"), ModelMonitorService)
        mModelMonitorServiceList = CType(Session("mModelMonitorServiceList"), ModelMonitorServiceList)
        MaintenanceActivityID = Session("MaintenanceActivityID")
        mModelMonitorInspList = Session("mModelMonitorInspList")
        mModelMonitorModList = Session("mModelMonitorModList")
        mLinkMaintenanceList = Session("mLinkMaintenanceList")
        mAllLinkMaintenanceList = Session("mAllLinkMaintenanceList")
        ModelID = Session("ModelIDForMPD") 'Added By Vikrant For MPD
    End Sub
    Private Sub RemoveSession()
        Session.Remove("MaintenanceActivityID")
        Session.Remove("mAllLinkMaintenanceList")
    End Sub
    Private Sub setLable()
        Select Case FromType
            Case 1
                Dim ServiceMPDTitle As String = ""
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD "
                Else
                    ServiceMPDTitle = "Service "
                End If


                lblResult.Text = "List Of Model " & ServiceMPDTitle & mModelMonitorServiceList.Count & " Record(s) found."
            Case 2
                lblResult.Text = "List Of Model Inspection : " & mModelMonitorInspList.Count & " Record(s) found."
            Case 3
                lblResult.Text = "List Of Model Directive : " & mModelMonitorModList.Count & " Record(s) found."
        End Select

    End Sub
    Private Sub ControlVisibility()

        Select Case FromType
            Case 1
                If mModelMonitorServiceList.Count > 25 Then
                    btnAddTop.Visible = True
                    btnBackTop.Visible = True
                Else
                    btnAddTop.Visible = False
                    btnBackTop.Visible = False
                End If
                dgMonitorActivityList.Columns(5).Visible = False
            Case 2
                If mModelMonitorInspList.Count > 25 Then
                    btnAddTop.Visible = True
                    btnBackTop.Visible = True
                Else
                    btnAddTop.Visible = False
                    btnBackTop.Visible = False
                End If
                dgMonitorActivityList.Columns(5).Visible = False
            Case 3
                If mModelMonitorModList.Count > 25 Then
                    btnAddTop.Visible = True
                    btnBackTop.Visible = True
                Else
                    btnAddTop.Visible = False
                    btnBackTop.Visible = False
                End If

        End Select
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.OK
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfModelMonitorActivityList.aspx?FromType=" & FromType)
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfModelMonitorActivityList.aspx?FromType=" & FromType)
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            '  DataFieldBind()
        End If
    End Sub

#End Region

#Region "DataBinding"
    Private Sub DataFieldBind()
        'mModelMonitorServiceList = Nothing
        'dgMonitorServiceList.DataSource = Nothing

        Select Case FromType
            Case 1
                'Commented & Added By Vikrant For MPD
                'mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(mAssemblyStatus.Assembly.ModelID)
                mModelMonitorServiceList = ModelMonitorServiceList.GetModelMonitorServiceList(ModelID)
                'End
                dgMonitorActivityList.DataSource = mModelMonitorServiceList
                Session("mModelMonitorServiceList") = mModelMonitorServiceList
            Case 2
                'Commented & Added By Vikrant For MPD
                'mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyStatus.Assembly.ModelID)
                mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(ModelID)
                'End
                dgMonitorActivityList.DataSource = mModelMonitorInspList
                Session("mModelMonitorInspList") = mModelMonitorInspList
            Case 3
                'Commented & Added By Vikrant For MPD
                'mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(mAssemblyStatus.Assembly.ModelID)
                mModelMonitorModList = ModelMonitorModList.GetModelMonitorModList(ModelID)
                'End
                dgMonitorActivityList.DataSource = mModelMonitorModList
                Session("mModelMonitorModList") = mModelMonitorModList
        End Select
        mAllLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList()
        Session("mAllLinkMaintenanceList") = mAllLinkMaintenanceList


        If FromType = 1 And AppSettings("ShowMaintenanceForNewClients") = "True" Then
            dgMonitorActivityList.Columns(2).HeaderText = "Task No."  ' .Cells(2).Text = "Task No."
        Else
            ' dgMonitorActivityList.HeaderRow.Cells(1).Text = "Code/Form No."
            dgMonitorActivityList.Columns(2).HeaderText = "Code/Form No."
        End If
        DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        FromType = Request.QueryString("FromType")
        If Not Page.IsPostBack Then
            DataFieldBind()
        End If
        setLable()
        ControlVisibility()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click, btnAddTop.Click
        Dim IsNotSelected As Boolean = True
        Dim IsDuplicate As Boolean = True
        Dim chkSelect As CheckBox
        For i As Integer = 0 To dgMonitorActivityList.Items.Count - 1
            chkSelect = CType(dgMonitorActivityList.Items(i).FindControl("chkSelect"), CheckBox)
            If chkSelect.Checked = True Then
                IsNotSelected = False
                Dim LinkID As Guid = New Guid(dgMonitorActivityList.Items(i).Cells(1).Text.ToString)
                'Checking if Current Maintenace Activite is linked with the Same activity which is already linked with this Maintenance activity
                'OR  Checking if trying to add same maintenance activity

                If Not ((mAllLinkMaintenanceList.Contains(MaintenanceActivityID, LinkID) Or mLinkMaintenanceList.Contains(MaintenanceActivityID, LinkID) Or MaintenanceActivityID.Equals(LinkID))) Then
                    mLinkMaintenanceList.add(LinkMaintenance.NewChildLinkedMaintenance(Guid.NewGuid, MaintenanceActivityID, LinkID, FromType))
                    IsDuplicate = False
                End If

            End If
        Next
        If IsNotSelected = True Then
            Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OKOnly)
            msg.ReplacePage = "wfModelMonitorActivityList.aspx?FromType=" & FromType
            msg.Show()
            Exit Sub
        End If

        If IsDuplicate = True Then
            Dim msg1 As New SIMsgBox(Page, "Duplicate Alert !", "<B>You are trying to add duplicate entry.</B> <BR><BR> Maintenance Acitivity is already linked.", "", MsgBoxStyle.OkOnly)
            msg1.ReplacePage = "wfModelMonitorActivityList.aspx?FromType=" & FromType
            msg1.Show()
            Exit Sub
        End If

        Session("mLinkMaintenanceList") = mLinkMaintenanceList
        RemoveSession()
        Dim URL As Stack = CType(Session("URL"), Stack) 'Getting Url of previous page from session,stored in stack
        Response.Redirect(URL.Peek.ToString)            'Returns topMost Object,Here URL of previous page.
    End Sub
    Private Sub dgMonitorActivityList_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgMonitorActivityList.SortCommand

        Select Case FromType
            Case 1
                mModelMonitorServiceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                dgMonitorActivityList.DataSource = mModelMonitorServiceList
                Session("mModelMonitorServiceList") = mModelMonitorServiceList
            Case 2
                mModelMonitorInspList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                dgMonitorActivityList.DataSource = mModelMonitorInspList
                Session("mModelMonitorInspList") = mModelMonitorInspList
            Case 3
                mModelMonitorModList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
                dgMonitorActivityList.DataSource = mModelMonitorModList
                Session("mModelMonitorModList") = mModelMonitorModList
        End Select

        DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        RemoveSession()
        Dim URL As Stack = CType(Session("URL"), Stack) 'Getting Url of previous page from session,stored in stack
        Response.Redirect(URL.Peek.ToString)            'Returns topMost Object,Here URL of previous page.
    End Sub
#End Region

End Class
