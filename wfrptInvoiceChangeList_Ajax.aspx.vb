'Added by Utkarsh ON 24-Jan-2014

Public Class wfrptInvoiceChangeList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim FromDate As String
    Dim ToDate As String
    Private mChargeList As ChargeList
    Dim ChargeID As Guid
    Dim EventLogDetail As String
    Dim IsForOrder As Boolean
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mChargeList = Session("mChargeList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblFromDate.Visible = True
        lblToDate.Visible = True
        lblChageName.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
       
        FromDate = txtFromDate.Text.Trim
        ToDate = txtToDate.Text.Trim

        ChargeID = New Guid(Request.Form("cmbCharge"))
		'Sankalp 20-08-25
		If rdbOrder.Checked = True Then
			IsForOrder = True
		Else
			IsForOrder = False
        End If


        lblFromDate.Text = "From Date : " & FromDate
        lblToDate.Text = "To Date     : " & ToDate
        lblChageName.Text = "Charge : " & IIf(ChargeID.Equals(Guid.Empty), "", mChargeList(ChargeID).Name)
        EventLogDetail = lblFromDate.Text + ", " + lblToDate.Text + ", " + lblChageName.Text
    End Sub
    Private Sub SetReport()
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsInvoiceChange
        Dim da As New CSLA.Data.ObjectAdapter
        Dim obj As rptInvoiceChangeList
        Dim mCompanyDetail As New CompanyDetail

        SetValues()
        myReport = New crptInvoiceChangeList
        obj = rptInvoiceChangeList.GetrptInvoiceChangeList(ChargeID, FromDate, ToDate, IsForOrder)
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
        mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2,
        mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite,
         If(IsForOrder, "Order Charges", "Invoice Charges"), FromDate, ToDate, mChargeList(ChargeID).Name, SearchStr4:=IsForOrder.ToString(), "", AppSettings("Product Version"), AppSettings("SINote"), IsForOrder.ToString(), "", "", "", AppSettings("Logo"))
        If obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1207)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, obj)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "Invoice Charge", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptInvoiceChangeList_Ajax.aspx" Then
            Session.Remove("mChargeList")
        End If
    End Sub
#End Region
#Region " DataFieldBind "
    Public Sub DataFieldBind()
        mChargeList = ChargeList.GetChargeList("", -1, True)
        Session("mChargeList") = mChargeList
        cmbCharge.DataSource = mChargeList
        cmbCharge.DataBind()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptInvoiceChangeList_Ajax.aspx"
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport()
    End Sub
	'Sankalp 20-08-2025
	Private Sub rdbOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbOrder.CheckedChanged
		DataFieldBind()
	End Sub
	'Sankalp 20-08-2025
	Private Sub rdbInvoice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbInvoice.CheckedChanged
		DataFieldBind()
	End Sub
#End Region
End Class