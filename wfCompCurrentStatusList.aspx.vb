Partial Class wfCompCurrentStatusList
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCurrentDate As String = ""
    Public mMachineList As MachineList
    Public mCompList As tmpCompCurrentStatusList
    Public mAssemblyStatusList As AssemblyStatusList
    Public AircraftId As String
    Protected WithEvents btnClose As System.Web.UI.WebControls.Button
    Public AssemblyId As String
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmbEnquiryText As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmbStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblResult As System.Web.UI.WebControls.Label
    Protected WithEvents txtToDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblFromDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblToDate As System.Web.UI.WebControls.Label
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents lblSearch As System.Web.UI.WebControls.Label
    Protected WithEvents lblPurchaseEnquiryList As System.Web.UI.WebControls.Label

    'On 28 may Prashant replaced this
    Protected WithEvents calDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mCompList = CType(Session("mCompList"), tmpCompCurrentStatusList)
        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mCompList") = mCompList
        Session("mAssemblyStatusList") = mAssemblyStatusList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineList")
        Session.Remove("mCompList")
        Session.Remove("mAssemblyStatusList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfCompCurrentStatusList.aspx" Then
            Session.Remove("mMachineList")
            Session.Remove("mCompList")
            Session.Remove("mAssemblyStatusList")
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            GetSession()
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "" Then
                        Session("sender") = ""
                        ' CompStatus.RevertRemovalCompStatus(mRemovedCompList.CurrentItem.CompStatusID, mRemovedCompList.CurrentItem.RemovedOnDBValue, mRemovedCompList.CurrentItem.IsExpired, mRemovedCompList.CurrentItem.AssemblyStatusID)
                        Response.Redirect("wfCompCurrentStatusList.aspx?MsgResult=0&BackPage=")
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfCompCurrentStatusList.aspx?MsgResult=0&BackPage=")
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfCompCurrentStatusList.aspx?MsgResult=0&BackPage=")
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfCompCurrentStatusList.aspx?MsgResult=0&BackPage=")
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfCompCurrentStatusList.aspx?MsgResult=0&BackPage=")
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub FindNow()
        Session("AircraftId") = cmbAircraft.SelectedValue
        Session("AssemblyId") = cmbAssembly.SelectedValue
        mCompList = tmpCompCurrentStatusList.GetCompCurrentStatusList(calDate.Value.ToString, cmbAircraft.SelectedValue, cmbAssembly.SelectedValue)
        dgCompList.DataSource = mCompList
        Session("mCompList") = mCompList
        dgCompList.DataBind()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMachineList = MachineList.GetMachineList()
        cmbAircraft.DataSource = mMachineList
        cmbAircraft.DataBind()
        FindNow()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' ClearAll()
        'GetSession()
        'calDate.Text = Today.Date.ToString
        If Not IsPostBack And Session("sender") = "" Then
            ' setFocus(cmbAircraft)
            Session("MiddleFrame") = "wfCompCurrentStatusList.aspx"
            DataFieldBind()
            Dim a As String = cmbAircraft.SelectedValue.ToString
            Dim mID As New Guid(a)
            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mCurrentDate, a, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            cmbAssembly.DataSource = mAssemblyStatusList
            cmbAssembly.DataBind()
        End If
        MessageBoxResult()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        Dim a As String = cmbAircraft.SelectedValue.ToString
        Dim mID As New Guid(a)
        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mCurrentDate, a, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
        cmbAssembly.DataSource = mAssemblyStatusList
        cmbAssembly.DataBind()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If IsValid Then
            FindNow()
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region "Reports"

#Region "Variable Declaration"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Private SearchStr1 As String
    Private SearchStr2 As String
    Private SearchStr3 As String
    Private SearchStr4 As String
#End Region

#Region "Events"
    Private Sub btnPrintTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click

        'If Not User.IsInRole("MachinePrint") Then mCustomMessages.Assert(CustomMessages.ErrorType.NotAuthorizedUser, "", "") : Exit Sub

        'For Component Current Status List
        Dim Rpt As New crListCompCurrentStatus
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsCommon
        Dim ReportDetails As New rptStatusList

        SearchStr1 = "The report shows records filtered by the following criteria till " + " " + calDate.Value.ToString
        SearchStr2 = "Aircraft :" + " " + cmbAircraft.SelectedItem.Text
        SearchStr3 = "Assembly :" + " " + cmbAssembly.SelectedItem.Text

        ReportDetails.Add(New rptStatus(, 0, , _
        , , , , , dgCompList.Columns.Item(1).HeaderText, dgCompList.Columns.Item(2).HeaderText, _
        dgCompList.Columns.Item(3).HeaderText, dgCompList.Columns.Item(4).HeaderText, dgCompList.Columns.Item(5).HeaderText, _
        dgCompList.Columns.Item(6).HeaderText, dgCompList.Columns.Item(7).HeaderText))

        Dim TotalCount As Integer
        FindNow()
        TotalCount = Me.mCompList.Count
        Dim I As Integer
        Dim str(6) As String

        For I = 0 To TotalCount - 1
            str(0) = ""
            str(1) = ""
            str(2) = ""
            str(3) = ""
            str(4) = ""
            str(5) = ""
            str(6) = ""

            If Me.dgCompList.Items(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgCompList.Items(I).Cells.Item(0).Text
            If Me.dgCompList.Items(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgCompList.Items(I).Cells.Item(1).Text
            If Me.dgCompList.Items(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgCompList.Items(I).Cells.Item(2).Text
            If Me.dgCompList.Items(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgCompList.Items(I).Cells.Item(3).Text
            If Me.dgCompList.Items(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgCompList.Items(I).Cells.Item(4).Text
            If Me.dgCompList.Items(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgCompList.Items(I).Cells.Item(5).Text
            If Me.dgCompList.Items(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgCompList.Items(I).Cells.Item(6).Text

            ReportDetails.Add(New rptStatus(, 1, , _
                        , , , , , , , , , , , _
                  , , , , , , , , str(0), str(1), str(2), str(3), str(4), str(5), str(6)))
        Next

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Component Current Status Report", SearchStr1, SearchStr2, SearchStr3, "", "", AppSettings("Product Version"), AppSettings("SINote"))

        If mCompList.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfCompCurrentStatusList.aspx?Backpage="
            msg1.Show()
        End If

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "<script language=Javascript>openTranDetail();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str1)

    End Sub

#End Region

#End Region

End Class


