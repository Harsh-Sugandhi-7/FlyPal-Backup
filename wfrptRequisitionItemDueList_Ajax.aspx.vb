Public Class wfrptRequisitionItemDueList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Dim PartNo As String
    Dim Description As String
#End Region

#Region " Methods "
    Private Sub SetValues()
        If (txtPartDescription.Text.Trim.IndexOf("[") > 0 And txtPartDescription.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtPartDescription.Text.Substring(0, txtPartDescription.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtPartDescription.Text.Trim, txtPartDescription.Text.Trim.IndexOf("[") + 2, txtPartDescription.Text.Trim.IndexOf("]") - txtPartDescription.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtPartDescription.Text)
            Description = Trim(txtPartDescription.Text)
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack And Session("sender") = "" Then
            'txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            'lblFromDate.Text = "From Date :" & New SmartDate(txtFromDate.Text).FormattedText
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsRequisitionItemDueList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim obj As RequisitionItemDueList
        Dim mCompanyDetail As New CompanyDetail
        SetValues()
        'FromDate = txtFromDate.Text
        ToDate = txtToDate.Text

        myReport = New crptRequisitionItemDueList

        obj = RequisitionItemDueList.GetRequisitionItemDueList(FromDate, ToDate, PartNo, Description, cmbRequisition.SelectedValue)
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, " ", cmbRequisition.SelectedItem.Text, New SmartDate(txtToDate.Text).FormattedText, _
        PartNo, Description, IIf(cmbRequisition.SelectedIndex = 0, "", cmbRequisition.SelectedItem.Text), AppSettings("Product Version"), _
        AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1445)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        'mDateSearchingCriteria = lblFromDate.Text.Trim + ", " + lblToDate.Text.Trim
        'MarkLog(Util.Action.Print, "DayBook", mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

End Class