Public Class wfMailSettings_Ajax
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim cn As New SqlConnection
        cn.ConnectionString = AppSettings("DB:FlyPal")
        Dim cm As New SqlCommand
        Try
            cn.Open()

            cm.Parameters.Clear()

            Dim wrapper As New Simple3Des("FlyPal")
            Dim cipherText As String = wrapper.EncryptData(txtSmtpPassword.Text)

            cm.Connection = cn
            cm.CommandType = CommandType.StoredProcedure
            cm.CommandText = "UpdateMailSetting"
            cm.Parameters.AddWithValue("@SmtpHost", txtSmtpHost.Text.Trim)
            cm.Parameters.AddWithValue("@SmtpPort", txtSmtpPort.Text.Trim)
            cm.Parameters.AddWithValue("@SmtpUser", txtSmtpUser.Text.Trim)
            cm.Parameters.AddWithValue("@SmtpPassword", cipherText.ToString.Trim)
            cm.Parameters.AddWithValue("@TableName", cmbModuleType.SelectedValue.Trim)

            cm.ExecuteNonQuery()

            MSGBoxCtrl.show("Message", "", "Mail Settings updated successfully.", MsgBoxStyle.OkOnly, "")
            txtSmtpHost.Text = ""
            txtSmtpPort.Text = ""
            txtSmtpUser.Text = ""
            txtSmtpPassword.Text = ""
            cmbModuleType.SelectedIndex = 0
            upnlSendMailDetails.Update()
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
        End Try

    End Sub

    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
End Class