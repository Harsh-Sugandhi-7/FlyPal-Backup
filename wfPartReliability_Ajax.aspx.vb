Public Class wfPartReliability_Ajax
    Inherits System.Web.UI.Page

#Region "Variables"
    Public mComponentReliablity As ComponentReliablity
    Public PartNo As String
    Public Description As String
    Dim mPartReliabilitySearchingCriteria As String = String.Empty
#End Region

#Region "Methods"
    Private Sub GetSession()
        mComponentReliablity = Session("mComponentReliablity")
    End Sub
    Private Sub SetValues()
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        mPartReliabilitySearchingCriteria = "Part No. : " + PartNo + ", " + "Description : " + Description + ", " + "Serial No. : " + txtSerialNo.Text.Trim
    End Sub
#End Region


#Region "DataFieldBind"
    Private Sub DataFieldBind()
        mComponentReliablity = ComponentReliablity.GetComponentReliablity()
        dgGridView.DataSource = mComponentReliablity
        dgGridView.DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        custValidator.ControlToValidate = "txtsearch"
        If (txtSearch.Text = "") Then
            e.IsValid = False
        ElseIf ((txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0)) Then
            e.IsValid = False
        ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
            If ((PartNo = "" Or Description = "")) Then
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
        End If
    End Sub
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        If IsValid Then
            SetValues()
            mComponentReliablity = ComponentReliablity.GetComponentReliablity(txtSerialNo.Text.Trim, PartNo, Description)
            If mComponentReliablity.Count > 0 Then
                dgGridView.DataSource = mComponentReliablity
                Session("mComponentReliablity") = mComponentReliablity
                dgGridView.DataBind()
                upnlGrid.Update()
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsComponentReliablity
        Dim mCompanyDetail As New CompanyDetail
        myReport = New crptComponentReliablity
        SetValues()
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "", txtSerialNo.Text.Trim, PartNo, Description, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mComponentReliablity.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1288)
        End If

        ds.Clear()
        da.Fill(ds, mComponentReliablity)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "PartReliability", mPartReliabilitySearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
    End Sub
#End Region


End Class