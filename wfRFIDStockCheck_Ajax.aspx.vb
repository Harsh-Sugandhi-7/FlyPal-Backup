Imports System.Net
Imports CrystalDecisions.Shared.Json
Imports System.Web.Script.Serialization

Public Class wfRFIDStockCheck_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mrptStoresAcceptanceTag As rptStoresAcceptanceTag
    Public mrptStoresAcceptanceTagAPI As rptStoresAcceptanceTag
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfRFIDStockCheck_Ajax.aspx" Then
            Session.Remove("mPartListForSerialNoBatchNoChange")
        End If
    End Sub
    Private Sub GetSession()
        mrptStoresAcceptanceTag = CType(Session("mrptStoresAcceptanceTag"), rptStoresAcceptanceTag)
        mrptStoresAcceptanceTagAPI = CType(Session("mrptStoresAcceptanceTagAPI"), rptStoresAcceptanceTag)
    End Sub
    Private Sub DataFieldBind()
        'mrptStoresAcceptanceTag = rptStoresAcceptanceTag.GetStoresAcceptanceTag(Guid.Empty, True, Trim(txtRFIDNo.Text))
        'Session("mrptStoresAcceptanceTag") = mrptStoresAcceptanceTag
        'dgPartSearch.DataSource = mrptStoresAcceptanceTag
        'dgPartSearch.DataBind()
        'lblResult.Text = "List of Parts :" & mrptStoresAcceptanceTag.Count & " Record(s) found "

        Try
            Dim jsonStr As String

            'jsonStr = "http://bytzsoft.net:8280/FlyPal-ENT/v1.0.0/GetItemTagListJSON?CorporateID=" + AppSettings("CorporateID").ToString + "&UserName=BTPLAdmin&Password=bytzAdmin&AccessLog=ABC%3BABC%3BABC%3BABC&BarcodeNosString=" + txtRFIDNo.Text.ToString

            Dim service As New AzureFlyPalService.FlyPalServices

            'Dim output As  = service.GetItemTagListJSON(AppSettings("CorporateID").ToString, "BTPLAdmin", "bytzAdmin", "ABC;ABC;ABC", txtRFIDNo.Text.Trim)

            ''lblCountry.Text = "Country: " + output.CountryName
            Dim js As New JavaScriptSerializer()
            'service.GetItemTagListJSON(AppSettings("CorporateID").ToString, "BTPLAdmin", "bytzAdmin", "ABC;ABC;ABC;ABC", txtRFIDNo.Text.Trim)
            Dim output As String = service.GetItemTagListJSON(AppSettings("CorporateID").ToString, "BTPLAdmin", "bytzAdmin", "ABC;ABC;ABC;ABC", txtRFIDNo.Text.Trim)



            'Dim wclient As WebClient = New WebClient()
            'wclient.Headers.Add("Authorization", "Bearer 2f3280a8-bf15-39eb-9706-9b167b959c45")
            'Dim Result1 As String = wclient.DownloadString(jsonStr)
            If output.Equals("[]") Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                jsonStr = output.Replace("[", "").Replace("]", ",")
                jsonStr = "[" + jsonStr + "]"
                Dim jsonarray As New JsonArray(jsonStr)

                mrptStoresAcceptanceTagAPI = rptStoresAcceptanceTag.GetList(jsonarray)
                Session("mrptStoresAcceptanceTagAPI") = mrptStoresAcceptanceTagAPI
                dgPartSearch.DataSource = mrptStoresAcceptanceTagAPI
                dgPartSearch.DataBind()
                lblResult.Text = "List of Parts :" & mrptStoresAcceptanceTagAPI.Count & " Record(s) found "
            End If

        Catch ex As Exception
            txtRFIDNo.Text = ""
        End Try


    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                   
                Case MsgBoxResult.No
                   
                Case MsgBoxResult.Cancel
                   
                Case MsgBoxResult.Ok

                    
               
            End Select
        ElseIf Result1 = -1 Then
          
        ElseIf Result1 = 0 Then

        End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfRFIDStockCheck_Ajax.aspx"
            If txtRFIDNo.Enabled = True Then
                SetFocus(txtRFIDNo)
            End If
            mrptStoresAcceptanceTag = rptStoresAcceptanceTag.NewList
            dgPartSearch.DataSource = mrptStoresAcceptanceTag
            dgPartSearch.DataBind()
            'DataFieldBind()
        End If
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click
        If txtRFIDNo.Text.Trim = "" Then
            MSGBoxCtrl.Show("Alert", "Please Enter RFID No's", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        DataFieldBind()
        upnlGridView.Update()
    End Sub
    Private Sub dgPartSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartSearch.PageIndexChanging
        dgPartSearch.PageIndex = e.NewPageIndex
        dgPartSearch.DataSource = mrptStoresAcceptanceTagAPI
        Session("mrptStoresAcceptanceTagAPI") = mrptStoresAcceptanceTagAPI
        dgPartSearch.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "RFIDStockCheck", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mrptStoresAcceptanceTag = Nothing
        Session("MiddleFrame") = ""
        Session.Remove("mrptStoresAcceptanceTagAPI")
        Session.Remove("mrptStoresAcceptanceTag")
        Response.Redirect("DashBoard.aspx")
    End Sub
#End Region

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
End Class