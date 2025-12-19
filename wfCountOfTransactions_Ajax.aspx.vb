Imports InfoSoftGlobal
Imports System.Web.Script.Serialization

Partial Class wfCountOfTransactions_Ajax
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
    Public mCountOfTransactions As CountOfTransactions
#End Region

#Region "Business Methods"
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        Catch ex As SqlException

                        End Try
                    End If
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub SetCountOfTransactionsFunc()
        mCountOfTransactions = CountOfTransactions.GetCountOfTransactions(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)   'Serialize(Object)	Converts an object to a JSON string.
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CountOfTransactionsFunc", "CountOfTransactionsFunc();", True)
    End Sub
    Public Sub SetMonthwiseCountOfTransactionsGraphs()
        Dim mMonthwiseCountOfTransactions As MonthwiseCountOfTransactions
        mMonthwiseCountOfTransactions = MonthwiseCountOfTransactions.GetMonthwiseCountOfTransactions(IIf(cmbYear.SelectedIndex > -1, CInt(cmbYear.SelectedItem.Text), ""), cmbMonth.SelectedIndex + 1, cmbTransactions.SelectedValue.ToString)
        Dim MonthwiseCountOfTransactionsValues As String = New JavaScriptSerializer().Serialize(mMonthwiseCountOfTransactions)
        MonthwiseCountOfTransactionsValues = MonthwiseCountOfTransactionsValues.Replace("NameOfMonth", "label").Replace("CountOfTransactionsCount", "value")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthwiseCountOfTransactions", "MonthwiseCountOfTransactions('" + MonthwiseCountOfTransactionsValues.ToString + "');", True)
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If

        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'AddAttributes()
        If Not Page.IsPostBack Then
            SetCombo()
            SetCountOfTransactionsFunc()
            SetMonthwiseCountOfTransactionsGraphs()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub cmbMonth_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbMonth.SelectedIndexChanged, cmbYear.SelectedIndexChanged
        SetCountOfTransactionsFunc()
        SetMonthwiseCountOfTransactionsGraphs()
        upnlCountOFTransactions.Update()
        upnlMonthwiseCountOfTransactions.Update()
    End Sub
    Private Sub cmbTransactions_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbTransactions.SelectedIndexChanged
        SetCountOfTransactionsFunc()
        SetMonthwiseCountOfTransactionsGraphs()
        upnlCountOFTransactions.Update()
        upnlMonthwiseCountOfTransactions.Update()
    End Sub
#End Region

End Class
