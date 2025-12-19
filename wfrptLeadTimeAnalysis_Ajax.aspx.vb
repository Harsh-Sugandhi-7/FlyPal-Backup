'AJAX Conversion By Vikrant On 24-Jan-2014

Public Class wfrptLeadTimeAnalysis_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mVendor As Vendor
    Public mVendorList As VendorList
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String
    Public Description As String
    Public Supplier As String
    Dim EventLogDetail As String
    Dim mType As Integer = 0
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorlist"), VendorList)
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mVendorlist") = mVendorList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorlist")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
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
        lblVendor.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblOrderType.Visible = True 'Added By Vikrant On 24-Jan-2014 For ALL24012014
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub setDatePeroid(ByVal Index As Int32)
        Select Case Index
            Case 0 ' All   
                txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
                txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
            Case 1 'Last 1 Week
                txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 2 'Last 1 Month
                txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 3 'Last 1 Quater
                Select Case Today.Month
                    Case 1, 2, 3
                        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
                    Case 4, 5, 6
                        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 7, 8, 9
                        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                    Case 10, 11, 12
                        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
                End Select
            Case 4 'Last 1 Year
                txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 5 'Current Financial Year
                If Today.Month <= 3 Then  'Jan|Feb|Mar
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
                Else
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))   '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then
            FromDate = "1/1/1900"
            ToDate = "1/1/2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text
            ToDate = txtToDate.Text
            lblDateRangeFrom.Text = "Date Range : " & New SmartDate(FromDate).FormattedText & " To " & New SmartDate(ToDate).FormattedText & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        If cmbSupplier.SelectedIndex = 0 Then
            Supplier = ""
            lblVendor.Text = "Supplier : All"
        Else
            mVendor = Vendor.GetVendor(New Guid(cmbSupplier.SelectedValue))
            Supplier = mVendor.Name
            lblVendor.Text = "Supplier : " & Supplier
        End If
        'Added By Utkarsh ON 28-Nov-2012 FOR ALL28112012
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        'End
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        'Added By Vikrant On 24-Jan-2014 For ALL24012014
        lblOrderType.Text = "Order Type : " & IIf(cmbSearchOrderType.SelectedIndex > 0, cmbSearchOrderType.SelectedItem.ToString, "All")
        'End
        EventLogDetail = lblDateRangeFrom.Text + "," + lblVendor.Text + "," + lblPartNo.Text + "," + lblDesc.Text + "," + lblOrderType.Text
    End Sub
    'Added By Vikrant On 24-Jan-2014 For ALL24012014
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
    'End
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptLeadTimeAnalysis
        SetValues()
        Dim dsLeadTime As New dsLeadTimeAnalysis
        If mType = 1 Then
            myReport = New crptLeadTimeAnalysis
        Else
            myReport = New crptSupplierLeadTime
        End If

        rpt = rptLeadTimeAnalysis.GetLeadTimeAnalysis(FromDate, ToDate, Supplier, PartNo, Description, CInt(cmbSearchOrderType.SelectedValue))
        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, Supplier, "", "", "", "", "", "", Description, AppSettings("Logo"), , IIf(cmbSearchOrderType.SelectedIndex > 0, cmbSearchOrderType.SelectedItem, "").ToString)

        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            If mType = 1 Then
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 802)
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1287)
            End If
        End If
        dsLeadTime.Clear()
        da.Fill(dsLeadTime, rpt)
        da.Fill(dsLeadTime, objsearch)
        Dim mrptImage As rptImage = rptImage.GetImage(dsLeadTime)
        da.Fill(dsLeadTime, mrptImage)
        myReport.SetDataSource(dsLeadTime)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        If mType = 1 Then
            MarkLog(Util.Action.Print, "LeadTimeAnalysis", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            MarkLog(Util.Action.Print, "SupplierLeadTime", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'mVendorList = VendorList.GetVendortList(0, "", "", "", "", "", True, False, True)
        mVendorList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbSupplier.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        mType = Request.QueryString("Type")  '  1 For Component , 2 For Supplier Lead Time
        If Not IsPostBack Then
            RemoveSession()
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
        End If
        If mType = 1 Then
            lbltitle.Text = "Component Lead Time"
            lblPilotStar1.Visible = True
            rfvSelectPart.Visible = True
            btnClose.ToolTip = "Click to close Component Lead Time screen"
        Else
            lbltitle.Text = "Supplier Lead Time"
            lblPilotStar1.Visible = False
            rfvSelectPart.Visible = False
            btnClose.ToolTip = "Click to close Supplier Lead Time screen"
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
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
#End Region

End Class