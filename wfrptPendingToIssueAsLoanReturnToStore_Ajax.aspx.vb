
'Added by Utkarsh on 04-Feb-2014

Public Class wfrptPendingToIssueAsLoanReturnToStore_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'Public mItemList As ItemList
    Public mFromStoreList As StoreList
    Public mToStoreList As StoreList
    'Public mAircraftList As tmpMachineList
    'Public mVendorList As VendorList
    'Public mVendor As Vendor
    Public rpt As rptPendingToIssueAsLoanTakenFromStore

    Public StoreID As Guid
    Dim FromDate As String
    Dim ToDate As String
    Dim PartNo As String = ""
    Dim Description As String = ""
    Dim mPendingReceipt As Int16
    Dim NameOfToStore As String = ""  'Added by Prashant 5-Apr-2013 'ALL05042013
    Dim NameOfFromStore As String = ""  'Added by Prashant 5-Apr-2013 'ALL05042013
    Dim FromStore, ToStore As String
    Dim EventLogDetail As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mFromStoreList = CType(Session("mFromStoreList"), StoreList)
        mToStoreList = CType(Session("mToStoreList"), StoreList)
        'mAircraftList = CType(Session("mAircraftList"), tmpMachineList)
        'mVendorList = CType(Session("mVendorList"), VendorList)
        'mItemList = CType(Session("mItemList"), ItemList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mFromStoreList") = mFromStoreList
        Session("mToStoreList") = mToStoreList
        'Session("mAircraftList") = mAircraftList
        'Session("mVendorList") = mVendorList
        'Session("mItemList") = mItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mFromStoreList")
        Session.Remove("mToStoreList")
        'Session.Remove("mAircraftList")
        'Session.Remove("mVendorList")
        'Session.Remove("mItemList")
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
        ''txtFromDate.Visible = IIf(Index <> 0, True, False)
        ''txtToDate.Visible = IIf(Index <> 0, True, False)
        ''calFromDate.Visible = IIf(Index = 6, True, False)
        ''calToDate.Visible = IIf(Index = 6, True, False)
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
        upnlDateRange.Update()
    End Sub
    Private Sub ClearControls()
        'txtSearchFor.Text = ""
    End Sub
    Private Sub ControlVisibility2()
        lblDateRangeFrom.Visible = True
        lblVendor1.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
        lblFromStore1.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub ControlVisibility3()
        lblDateRangeFrom.Visible = False
        lblVendor1.Visible = False
        lblPartNo.Visible = False
        lblDesc.Visible = False
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
                    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
                End If
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            Case 6 'Between Dates
                txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
                txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End Select
    End Sub
    Private Sub SetValues()
        If cmbDateRange.SelectedIndex = 0 Then      'Date Range
            FromDate = "1-1-1900"
            ToDate = "1-1-2200"
            lblDateRangeFrom.Text = "Date Range : All"
        Else
            FromDate = txtFromDate.Text.ToString
            ToDate = txtToDate.Text.ToString
            lblDateRangeFrom.Text = "Date Range : " & FromDate & " To " & ToDate & " ( " & cmbDateRange.SelectedItem.Text & " ) "
        End If
        StoreID = New Guid(Request.Form("cmbFromStore").ToString)
        If StoreID.Equals(Guid.Empty) Then       'From Store
            FromStore = ""
            NameOfFromStore = ""   'Added by Prashant 5-Apr-2013 'ALL05042013
            lblFromStore1.Text = "From Store Name : All"
        Else
            FromStore = mFromStoreList(StoreID).Name
            NameOfFromStore = IIf(StoreID.Equals(Guid.Empty), "", mFromStoreList(StoreID).LocationStore)   'Added by Prashant 5-Apr-2013 'ALL05042013
            lblFromStore1.Text = "From Store Name : " & NameOfFromStore
        End If
        'Commented and Added by Prashant 5-Apr-2013 'ALL05042013
        'Store1 = IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "")
        Dim ToStoreID As Guid = New Guid(Request.Form("cmbStore").ToString)
        NameOfToStore = IIf(ToStoreID.Equals(Guid.Empty), "", mToStoreList(ToStoreID).LocationStore)
        ToStore = IIf(ToStoreID.Equals(Guid.Empty), "", mToStoreList(ToStoreID).Name)
        '-----------------------------

        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")

        lblVendor1.Text = "To Store Name : " & IIf(NameOfToStore <> "", NameOfToStore, "All")
        'Added By Shweta ON 06-Dec-2012 FOR ALL28112012
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        Session("PartNo") = PartNo
        Session("Description") = Description
        'eND
        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblFromStore1.Text + ", " + lblVendor1.Text + ", " + lblPartNo.Text + ", " + lblDesc.Text
    End Sub
    Private Sub ResetValues()
        FromDate = "1-1-1900"
        ToDate = "1-1-2200"
        PartNo = ""
        Description = ""
        Session("PartNo") = ""
        Session("Description") = ""
    End Sub
    Private Sub callFindNowReport()
        'Store
            FindNowReport("", "", FromDate, ToDate, ToStore, 8, 0, FromStore, "", PartNo, Description)
    End Sub
    Private Sub FindNowReport(Optional ByVal Text As String = "", Optional ByVal No As String = "", Optional ByVal FromDate As String = "1-1-1800", Optional ByVal ToDate As String = "1-1-3050", Optional ByVal ToStoreName As String = "", Optional ByVal ToTypeID As Integer = 0, Optional ByVal StatusID As Integer = 0, Optional ByVal FromStoreName As String = "", Optional ByVal SerialNo As String = "", Optional ByVal ItemName As String = "", Optional ByVal Description As String = "")
        rpt = rptPendingToIssueAsLoanTakenFromStore.GetPendingToIssueAsLoanTakenFromStore(FromDate, ToDate, FromStoreName, ToStoreName, ItemName, Description)
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteriaForReceipt
        'Dim rpt As rptIssueRegForReminder
        SetValues()
        Dim ds As New dsIssue
        myReport = New crptPendingToIssueToStoreAsLoanReturn
        callFindNowReport()
        objsearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{EB2E0504-72C0-46B5-A3BF-5F7E0893EB46}"), FromDate, ToDate, "", "", "", "", "", "", "", "", "", "", NameOfToStore, "", "", PartNo, Description, "", "", NameOfFromStore, "", "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
            'Added By Utkarsh On 7-Jun-2011 For All07062011
        ElseIf rpt.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 508)

            '*******************************
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, rpt)
        da.Fill(ds, mrptImage)
        da.Fill(ds, objsearch)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "PendingIssueLoanReturnToStore", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'ResetValues()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'From Store
        mFromStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbFromStore.DataSource = mFromStoreList
        Session("mFromStoreList") = mFromStoreList
        'To Store
        mToStoreList = StoreList.GetStoreList(0, "", "(All)", True)
        cmbStore.DataSource = mToStoreList
        Session("mToStoreList") = mToStoreList

        lblStoreCount.Text = "You have " + (mToStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mToStoreList.TotalStorelistCount.ToString + " Store(s)"

        'Aircraft
        'mAircraftList = tmpMachineList.GetMachineList(, , , , , "(All)")
        
        DataBind()
    End Sub
    
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            'RemoveSession()
            mPendingReceipt = CType(Request.QueryString("PendingToReceipt"), Int16)
            Session("mPendingReceipt") = mPendingReceipt
            'PendingToReceipt
            If cmbDateRange.Enabled = True Then
                setFocus(cmbDateRange)
            End If
            DataFieldBind()
            ControlVisibility(6)
            setDatePeroid(6)
            cmbDateRange.SelectedIndex = 6
            'SetTitle()
        End If
    End Sub
    Private Sub cmbDateRange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateRange.SelectedIndexChanged
        Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
        ControlVisibility(Index)
        setDatePeroid(Index)
        If cmbDateRange.Enabled = True Then
            SetFocus(cmbDateRange)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport() 'laktrum
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
End Class