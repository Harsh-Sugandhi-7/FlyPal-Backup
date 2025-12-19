'Created By Utkarsh ON 09-Aug-2012

Partial Class wfrptWSEventLog
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Validationsummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cvFromdate As System.Web.UI.WebControls.CustomValidator
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

#Region " Variable Declarations "
    Public FromDate As String
    Public ToDate As String
    Public Action As String = ""
    Public ModuleName As String
    Public mEventlogList As WS_EventLogList
    Public mModuleNameList As WS_ModuleNameList
#End Region

#Region " Business Properties and Methods "
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True

            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblModuleName1.Visible = True
        lblAction1.Visible = True
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblToDate.Visible = False
        lblModuleName1.Visible = False
        lblAction1.Visible = False
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.text = Today.Date
                txtToDate.Text = Today.Date
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " )"
        End If

        If cmbModuleName.SelectedIndex > 0 Then
            ModuleName = cmbModuleName.SelectedItem.ToString
            lblModuleName1.Text = "Module Name : " & ModuleName
        Else
            ModuleName = ""
            lblModuleName1.Text = "Module Name : All"
        End If
        If cmbAction.SelectedIndex > 0 Then
            Action = cmbAction.SelectedItem.ToString
            lblAction1.Text = "Action : " & cmbAction.SelectedItem.Text
        Else
            Action = ""
            lblAction1.Text = "Action : All"
        End If
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
    End Sub
#End Region

#Region "Data Binding"
    Private Sub Datafieldbind()
        mEventlogList = WS_EventLogList.GetEventLogList("", "", txtFromDate.Text.ToString, txtToDate.Text.ToString)
        dgEventLogList.DataSource = mEventlogList
        dgEventLogList.DataBind()

        mModuleNameList = WS_ModuleNameList.GetModuleName()
        cmbModuleName.DataSource = mModuleNameList
        cmbModuleName.DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Page.IsPostBack Then
            If cmbDateRange.Enabled = True Then
                SetFocus(cmbDateRange)
                setDatePeroid(2)
                cmbDateRange.SelectedIndex = 2
            End If
            ControlVisibility(2)
            Datafieldbind()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetValues()
        mEventlogList = WS_EventLogList.GetEventLogList(ModuleName, Action, FromDate, ToDate)
        dgEventLogList.DataSource = mEventlogList
        dgEventLogList.DataBind()

    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region
End Class
