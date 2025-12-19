'Added by Utkarsh On 24-Jan-2014

Public Class wfrptDestinationLogReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "

    Dim mPlaceList As PlaceList
    Dim mrptDestinationLogReport As rptDestinationLogReport
    Dim mCompanyDetail As New CompanyDetail
    Dim FromDate As String
    Dim ToDate As String
    Dim Departure As String
    Dim Arrival As String
    Dim EventLogDetails As String
    Dim DepartureID As Guid
    Dim ArrivalID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mPlaceList = Session("mPlaceList")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptDestinationLogReport_Ajax.aspx" Then
            Session.Remove("mrptDestinationLogReport")
            Session.Remove("mPlaceList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblDeparture.Visible = True
        lblArrival.Visible = True
        lblDateRangeFrom.Visible = True
        lblDateRangeTo.Visible = True
        lblSummary.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtFromDate.Text.Trim) Then
            FromDate = ""
        Else
            FromDate = txtFromDate.Text.Trim
        End If
        If Not IsDate(txtToDate.Text.Trim) Then
            ToDate = ""
        Else
            ToDate = txtToDate.Text.Trim
        End If
        DepartureID = New Guid(Request.Form("cmbDepartureFrom").ToString)
        ArrivalID = New Guid(Request.Form("cmbArrivalTo").ToString)

        Departure = mPlaceList(DepartureID).Name
        Arrival = mPlaceList(ArrivalID).Name


        lblDateRangeFrom.Text = "From Date : " & IIf(FromDate <> "", FromDate, "")
        lblDateRangeTo.Text = "To Date : " & IIf(ToDate <> "", ToDate, "")
        lblDeparture.Text = "Departure : " & IIf(Departure <> "", Departure, "")
        lblArrival.Text = "Arrival : " & IIf(Arrival <> "", Arrival, "")
        EventLogDetails = lblDateRangeFrom.Text + ", " + lblDateRangeTo.Text + ", " + lblDeparture.Text + ", " + lblArrival.Text
    End Sub
    Private Sub ResetValues()
        FromDate = txtFromDate.Text.Trim
        ToDate = txtToDate.Text.Trim
        Departure = ""
        Arrival = ""
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail

        Dim dsrptDestinationLogReport As New dsrptDestinationLogReport

        myReport = New crptDestinationLogReport

        SetValues()

        
        mrptDestinationLogReport = rptDestinationLogReport.GetrptDestinationLogReport(FromDate, ToDate, DepartureID, ArrivalID)

        Dim ReportData As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
             mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Destination Log Report", FromDate, ToDate, mPlaceList(DepartureID).Name, mPlaceList(ArrivalID).Name, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        If mrptDestinationLogReport.Count = 0 Then
            'ResetValues()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1132)
        End If

        da.Fill(dsrptDestinationLogReport, mrptDestinationLogReport)
        da.Fill(dsrptDestinationLogReport, ReportData)
        Dim mrptImage As rptImage = rptImage.GetImage(dsrptDestinationLogReport)
        da.Fill(dsrptDestinationLogReport, mrptImage)
        myReport.SetDataSource(dsrptDestinationLogReport)
        Session("CrystalReport") = myReport

      ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "DestinationLogReport", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'ResetValues()
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPlaceList = PlaceList.GetPlaceList(, , "(All)")
        cmbDepartureFrom.DataSource = mPlaceList
        cmbArrivalTo.DataSource = mPlaceList
        cmbArrivalTo.DataBind()
        cmbDepartureFrom.DataBind()
        Session("mPlaceList") = mPlaceList
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Utkarsh
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptDestinationLogReport_Ajax.aspx"
            ResetValues()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region


End Class