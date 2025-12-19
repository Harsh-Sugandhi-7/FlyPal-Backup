Public Class wfToGetBackDatedTransactions_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim DateIndex, FromDate, ToDate, mSearchingCriteria As String
    Dim mIsExcel As Boolean
    Dim EventLogID As Guid
#End Region

#Region "Business Methods"
    Private Sub GetSession()
    End Sub
    Private Sub SetSession()
    End Sub
    Public Sub RemoveSessions()
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfToGetBackDatedTransactions_Ajax.aspx?" Then
        End If
    End Sub
    Private Sub GenerateXLSXFile(tbl As DataTable)
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim dsReceipt As New dsRecCumInvReg
        Dim objSearch As rptSearchingCriteriaForReceipt

        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(tbl)

        dsNew.Tables("DT").TableName = "Back Dated Transactions"
		Session("ExcelFileName") = "Back Dated Transactions"
		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "GetBackDatedTransactions", "Export To Excel " + "Date Range " + txtFromDate.Text.Trim + ", " + txtToDate.Text.Trim + ", Type " + cmbForTransaction.SelectedItem.Text, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("DT")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ToGetBackDatedTransactions"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text.Trim)
        cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text.Trim)
        cmd.Parameters.AddWithValue("@For", CInt(cmbForTransaction.SelectedValue))
        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        'dataTable.Columns.Remove("Date")
        'dataTable.Columns.Remove("Text")
        'dataTable.Columns.Remove("No")
        Return dataTable
    End Function
#End Region

#Region " Data Bindings "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub
    'Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
    '    If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
    '    If IsValid = True Then
    '        SetReport()
    '    End If
    'End Sub
    'Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
    '    If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
    '    'If mDailyStatusLogReport.Count <= 0 Then
    '    '    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '    '    Exit Sub
    '    'Else
    '    '    SetReport(True)
    '    'End If
    'End Sub
    'Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
    '    Dim email As Thread
    '    Try
    '        email = New Thread(Sub() SetReport(True))
    '        'mIsPreview = False
    '        email.IsBackground = True
    '        email.Start()
    '    Catch ex As Exception
    '        Dim Day, Month, Year As String
    '        Day = Format(Today.Date.Day, "0#")
    '        Month = Format(Today.Date.Month, "0#")
    '        Year = Format(Today.Date.Year, "0#")
    '        Dim todaydate As String = Day & Month & Year
    '        Dim Path As String = AppSettings("DOCPath") & todaydate
    '        FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
    '        FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
    '        FileClose(1)
    '    End Try
    'End Sub
    Private Sub btnByExcel_Click(sender As Object, e As System.EventArgs) Handles btnByExcel.Click
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If IsValid = True Then
            GenerateXLSXFile(CreateDataTable())
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

End Class