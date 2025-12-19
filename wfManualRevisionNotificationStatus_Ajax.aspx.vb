Imports System.IO
Imports OfficeOpenXml
Imports System.Data.SqlClient
Public Class wfManualRevisionNotificationStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mMCategoryList As MCategoryList
    Dim Fromdate, ToDate, mSearchingCriteria As String
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMCategoryList = Session("mMCategoryList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMCategoryList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub
    Private Sub ControlVisibility(ByVal Index As Int16)
        lblFromDate.Visible = IIf(Index <> 0, True, False)
        lblToDate.Visible = IIf(Index <> 0, True, False)
        If Index = 6 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        Else
            txtFromDate.Visible = False
            txtToDate.Visible = False
        End If
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblManualCategory.Visible = True
        lblManualName.Visible = True
        lblNotificationStatus.Visible = True
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 'All   
                txtFromDate.Text = CDate("01-01-1900")
                txtToDate.Text = CDate("01-01-2200")
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6))
                txtToDate.Text = Today.Date
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1))
                txtToDate.Text = Today.Date
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1)
                txtToDate.Text = Today.Date
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date
                txtToDate.Text = Today.Date
        End Select
        txtFromDate.Text = Format(CDate(txtFromDate.Text), AppSettings("DateFormat"))
        txtToDate.Text = Format(CDate(txtToDate.Text), AppSettings("DateFormat"))

    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            Fromdate = "1/1/1900"
            ToDate = "1/1/2200"
            lblDateRangeFrom.Text = "Date Range     : All"
        Else
            Fromdate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range     : " & New SmartDate(Fromdate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " (" & cmbDateRange.SelectedItem.Text & ")"
        End If

        lblManualName.Text = IIf(txtManual.Text.Trim <> "", "Manual : " & txtManual.Text.Trim, "Manual : All")
        lblManualCategory.Text = IIf(cmbCategory.SelectedIndex > 0, "Category : " & cmbCategory.SelectedItem.ToString, "Category : All")
        lblNotificationStatus.Text = IIf(cmbNotificationStatus.SelectedIndex > 0, "Status : " & cmbNotificationStatus.SelectedItem.ToString, "Status : All")

        Session("Fromdate") = Fromdate
        Session("ToDate") = ToDate

        mSearchingCriteria = lblDateRangeFrom.Text.ToString + ", " + lblManualName.Text + ", " + lblManualCategory.Text + ", " + lblNotificationStatus.Text 
    End Sub
    Public Sub SetReport()
        Dim mCompanyDetail As New Flypal.CompanyDetail
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim da As New CSLA10.Data.ObjectAdapter
        Dim ds As New dsManualAvailabilityStatus
        Dim Obj As RevisionNotificationStatusList
        Rpt = New crptRevisionNotificationStatus

        SetValues()
        Obj = RevisionNotificationStatusList.GetList(txtManual.Text.Trim, cmbCategory.SelectedValue.ToString, Fromdate, ToDate, CInt(cmbNotificationStatus.SelectedValue), IIf(cmbDateRange.SelectedIndex = 0, True, False))

        Dim Report As New Flypal.ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Revision Notification Status", IIf(cmbDateRange.SelectedIndex > 0, Fromdate, ""), ToDate, txtManual.Text.Trim, IIf(cmbCategory.SelectedIndex > 0, cmbCategory.SelectedItem.ToString, ""), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", cmbNotificationStatus.SelectedValue.ToString, AppSettings("Logo"))

        If Obj.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf Obj.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1411)
        End If
        ds.Clear()

        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, Obj)
        da.Fill(ds, mrptImage) '
        da.Fill(ds, Report)
        Rpt.SetDataSource(ds)

        Session("CrystalReport") = Rpt

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "RevisionNotificationStatus", mSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mMCategoryList = MCategoryList.GetMCategoryList(, "(All)")
        Session("mMCategoryList") = mMCategoryList
        cmbCategory.DataSource = mMCategoryList

        DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            ControlVisibility(2)
            setDatePeroid(2)
            cmbDateRange.SelectedIndex = 2
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            setFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        ControlVisibility2()
        SetValues()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class