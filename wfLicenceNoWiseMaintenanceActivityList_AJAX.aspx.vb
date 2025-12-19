Imports Flypal.ModelListAutoComplete
Imports System.Linq
Imports System.Collections.Generic

Public Class wfLicenceNoWiseMaintenanceActivityList_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mLicenceNoWiseMaintenanceActivityList As LicenceNoWiseMaintenanceActivityList

    Public FromDate As String = String.Empty
    Public ToDate As String = String.Empty
    Public LicenceNo As String = String.Empty
    Dim mLicenceNoWiseMaintenanceActivitySearchingCriteria As String = String.Empty
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mLicenceNoWiseMaintenanceActivityList = Session("mLicenceNoWiseMaintenanceActivityList")
    End Sub
    Private Sub SetSession()
        Session("mLicenceNoWiseMaintenanceActivityList") = mLicenceNoWiseMaintenanceActivityList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mLicenceNoWiseMaintenanceActivityList")
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim ds As New dsLicenceNoWiseMaintenanceActivityList
        Dim rpt As New LicenceNoWiseMaintenanceActivityList
        If AppSettings("ClientCode") = "APFT" Or
           AppSettings("ClientCode") = "AAP" Then
            myReport = New crLicenceNoWiseMaintenanceActivityListAPFT  'Added By Prashant 29-Jul-2020 APFT29072020
        Else
            myReport = New crLicenceNoWiseMaintenanceActivityList
        End If

        Dim mCompanyDetail As New CompanyDetail

        rpt = LicenceNoWiseMaintenanceActivityList.GetLicenceNoWiseMaintenanceActivityList(FromDate, ToDate, LicenceNo, chkShowCompliance.Checked, chkShowPirepsMELSnag.Checked, chkInstallRemoval.Checked, txtRegNo.Text.Trim, txtModelList.Text.Trim)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
mCompanyDetail.WebSite, "", New SmartDate(txtFromDate.Text.ToString).FormattedText, New SmartDate(txtToDate.Text.ToString).FormattedText, txtLicenceNo.Text, "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", txtModelList.Text.Trim.ToString, txtRegNo.Text.Trim.ToString, AppSettings("Logo"))

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 710)
        End If

        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, rpt)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)

        txtLicenceNo.Text = ""
        txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))

        Session("CrystalReport") = myReport
        Dim str As String
        str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", str, True)
        MarkLog(Util.Action.Print, "LicenceNoWiseMaintenanceActivity", mLicenceNoWiseMaintenanceActivitySearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub ControlvisibilityForSearchingCriteria(ByVal showlabel As Boolean)
        lblDateRangeFrom.Visible = showlabel
        lblLicenceNo1.Visible = showlabel
        upnlSearchingCriteria.Update()
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text.Trim
        ToDate = txtToDate.Text.Trim
        LicenceNo = txtLicenceNo.Text.Trim

        lblDateRangeFrom.Text = "Date Range  : " & New SmartDate(txtFromDate.Text).FormattedText & " To Date : " & New SmartDate(txtToDate.Text.ToString).FormattedText
        lblLicenceNo1.Text = "Licence No. : " & IIf(txtLicenceNo.Text <> "", txtLicenceNo.Text, "All")
        mLicenceNoWiseMaintenanceActivitySearchingCriteria = lblDateRangeFrom.Text + ", " + lblLicenceNo1.Text
    End Sub
#End Region


#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            DataBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlvisibilityForSearchingCriteria(True)
        SetValues()
        upnlSearchingCriteria.Update()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region

#Region " Service "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim mModelList As ModelListAutoComplete
        Dim str As String = contextKey 'Holds the parameters to filter criteria..
        Dim AssemblyTypID As Integer = CInt(str)
        mModelList = ModelListAutoComplete.GetModelList(prefixText, 1)

        If count = 0 Then
            Return (From c As ModelListAutoCompleteInfo In mModelList
               Select c.Name).ToList
        Else
            Return (From c As ModelListAutoCompleteInfo In mModelList
                   Select c.Name).Take(count).ToList
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetRegTextList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim DistinctTextList As DistinctTextListAutoComplete
        DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 28)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function
#End Region
End Class