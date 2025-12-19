Public Class wfrptDateSearchCriteria_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Dim mDateSearchingCriteria As String = String.Empty
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack And Session("sender") = "" Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            lblFromDate.Text = "From Date :" & New SmartDate(txtFromDate.Text).FormattedText
            lblToDate.Text = "To Date     :" & New SmartDate(txtToDate.Text).FormattedText
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
   Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsDayBook
        Dim da As New CSLA.Data.ObjectAdapter
        Dim obj As rptDayBookRegister
        Dim mCompanyDetail As New CompanyDetail

        FromDate = txtFromDate.Text
        ToDate = txtToDate.Text

        myReport = New crDayBookRegister

        obj = rptDayBookRegister.GetDayBookList(FromDate, ToDate)
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, _
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, _
        " ", New SmartDate(txtFromDate.Text).FormattedText, New SmartDate(txtToDate.Text).FormattedText, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        If obj.Count <= 0 Then
           MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 725)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, mrptImage)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        lblFromDate.Text = "From Date :" & New SmartDate(txtFromDate.Text).FormattedText
        lblToDate.Text = "To Date     :" & New SmartDate(txtToDate.Text).FormattedText
        upnlSerachCriteria.Update()
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        mDateSearchingCriteria = lblFromDate.Text.Trim + ", " + lblToDate.Text.Trim
        MarkLog(Util.Action.Print, "DayBook", mDateSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)

    End Sub
#End Region

End Class