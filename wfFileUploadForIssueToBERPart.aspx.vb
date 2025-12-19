Public Class wfFileUploadForIssueToBERPart
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
#End Region

#Region " Business Methods "
     Private Sub addAttributes()
        txtDiscardAmt.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtDiscardAmt').value,event)")
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        addAttributes()
        If Not IsPostBack Then
            'Added By Vikrant On 04-Jun-2014 For All03062014-1
            If Session("ShowNotification") = True Then
                lblMessage.Visible = True
                txtDiscardAmt.Text = Session("EffRateOfPart")
                lblMaxEffectiveRateValue.Text = "Maximum Receipt Value Of This Part Is " + Session("MaxEffectiveRateValue")
                txtDiscardAmt.DataBind()
            Else
                lblMessage.Visible = False
            End If
            'ENd
            'ScriptManager.RegisterStartupScript(Me, Me.GetType, "pageloadscript", "OnPageLoad();", True)
            ClientScript.RegisterStartupScript(Me.GetType, "pageloadscript", "OnPageLoad();", True)
        End If
        If FileUpload1.HasFile Then
            Try
                Session("FileUpload.FileExtension") = Mid(FileUpload1.PostedFile.FileName, FileUpload1.PostedFile.FileName.LastIndexOf(".") + 1)
                Session("FileUpload.FileSize") = FileUpload1.PostedFile.ContentLength
                Session("FileUpload.FileContent") = FileUpload1.FileBytes
                Session("FileUpload.FileName") = Mid(FileUpload1.PostedFile.FileName, FileUpload1.PostedFile.FileName.LastIndexOf("\") + 2)
                Session("FileUpload.EffRateOfPart") = txtDiscardAmt.Text
                filepath.Text = FileUpload1.PostedFile.FileName
                'ScriptManager.RegisterStartupScript(Me.GetType, "onuploading", "onuploadcomplete(true);", True)
            Catch ex As Exception
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "alertscript", "alert(" + ex.Message + ");", True)
                'ClientScript.RegisterStartupScript(Me, Me.GetType, "alertscript", "alert(" + ex.Message + ");", True)
                ClientScript.RegisterStartupScript(Me.GetType, "alertscript", "alert(" + ex.Message + ");", True)
            End Try
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        'If Val(txtDiscardAmt.Text) > Val(Session("MaxEffectiveRateValue")) Then
        '    ClientScript.RegisterStartupScript(Me.GetType, "pageloadscript", "ReSetPageLayout();", True)
        '    ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", "alert(" + Chr(34) + "Discard amount is more than maximum receipt value " + Session("MaxEffectiveRateValue").ToString + Chr(34) + ");", True)
        '   Exit Sub
        'End If
        Session("FileUpload.EffRateOfPart") = txtDiscardAmt.Text
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "onuploading", "onuploadcomplete(false);", True)
        ClientScript.RegisterStartupScript(Me.GetType, "onuploading", "onuploadcomplete(false);", True)
    End Sub
    'Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
    ' End Sub
#End Region

  
End Class