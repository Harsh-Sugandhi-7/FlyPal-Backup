Public Class wfCustomExportToExcel
    Inherits System.Web.UI.Page

#Region "Business Methods"
    Private Sub GridBind(ByVal tbl As DataTable)
        dgRecordList.DataSource = tbl
        dgRecordList.DataBind()
        lblResult.Text = "List of Records : " + tbl.Rows.Count.ToString + " Record(s) found."
        upnlOtherChargeDetails.Update()
    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function SignOut() As String 'Added by Yogita for Explicit signout
        Web.Security.FormsAuthentication.SignOut()
        HttpContext.Current.Session.Abandon()

        MarkLog(Util.Action.Logoff)

        Thread.CurrentPrincipal = Nothing
        Return ""

    End Function
    Private Sub GenerateXLSXFile(ByVal tbl As DataTable)
        If (tbl.Rows.Count = 0) Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(tbl)
        dsNew.Tables(0).TableName = "Excel Report"

        Session("dsNew") = dsNew
        'Session("DataTable") = tbl
        'Session("ReportName") = "RCI Register"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
    Private Function CreateDataTable() As DataTable
        Dim dataTable As New DataTable("DT")
        Dim conString As String = AppSettings("DB:FlyPal")

        Dim con = New SqlConnection(conString)

        con.Open()

        Dim cmd As New SqlCommand()
        cmd.Connection = con
        cmd.CommandText = txtQuery.Text
        cmd.CommandType = CommandType.Text

        Dim adaptor = New SqlDataAdapter


        Try
            adaptor.SelectCommand = cmd
            adaptor.Fill(dataTable)
        Catch ex As Exception
            Throw
        End Try

        con.Close()
        Return dataTable
    End Function
#End Region

#Region "Events"
    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click, btnExportToExcelTop.Click
        Try
            GenerateXLSXFile(CreateDataTable())
        Catch ex As Exception
            MSGBoxCtrl.show("Error", ex.Message.ToString, "", MsgBoxStyle.OkOnly, "")
        End Try

    End Sub
    Private Sub btnExportToGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToGrid.Click, btnExportToGridTop.Click
        Try
            GridBind(CreateDataTable())
        Catch ex As Exception
            MSGBoxCtrl.show("Error", ex.Message.ToString, "", MsgBoxStyle.OkOnly, "")
        End Try
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        SignOut()

        '-----------------------------------------------
        Session.Remove("MenuID")
        Session.Remove("MiddleFrame")
        'Server.Transfer("Login.aspx")
        MarkLog(Util.Action.Logoff)
        'Drop all the references to the Principal.
        Thread.CurrentPrincipal = Nothing
        Dim str As String

        str = "window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenPageScript", str, True)
        Session.Remove("ReminderFired")
    End Sub
    Private Sub btnEncryptDecryptText_Click(sender As Object, e As System.EventArgs) Handles btnEncryptDecryptText.Click, btnEncryptDecryptTextTop.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenWindow", "OpenWindow();", True)
    End Sub
#End Region

   
End Class