Imports System.Text
Public Class wfExportInventoryDataToExcel
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("TMainReport")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = "ExportInventoryData"
        cmd.CommandType = CommandType.StoredProcedure

        Dim adaptor = New SqlDataAdapter

        adaptor.SelectCommand = cmd
        adaptor.Fill(dataTable)
        con.Close()
        Return dataTable
    End Function
    Private Sub GenerateXLSXFile(tbl As DataTable)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsIssue
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        ds.Clear()
        Dim dsNew As New DataSet
        dsNew.Clear()
        dsNew.Merge(tbl)
        dsNew.Tables("TMainReport").TableName = "Sheet1"
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        MarkLog(Util.Action.Print, "ExportInventoryDataToExcel", "Export To excel ", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
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
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        GenerateXLSXFile(CreateDataTable())
    End Sub
#End Region

End Class