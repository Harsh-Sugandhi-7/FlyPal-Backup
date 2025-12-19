Public Class wfrptRemovedItemStoreBalance_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mStoreList As StoreList
    Public PartNo As String = ""
    Public Description As String = ""
    Public mStoreID As Guid
    Public FromDate As String
    Public ToDate As String
    Dim value As String = ""
    Dim ReportName As String = ""
    Dim mRemovedItemStoreBalanceSearchingCriteria As String = String.Empty
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mStoreList = CType(Session("mStoreList"), StoreList)
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("mStoreList") = mStoreList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mStoreList")
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility2()
        lblFrmDate.Visible = True
        lblToDates.Visible = True
        lblStoreName.Visible = True
        lblPartNo.Visible = True
        lblDesc.Visible = True
    End Sub
    Private Sub SetValues()
        If txtFromDate.Text = "" Then
            FromDate = "1/1/1900"
            lblFrmDate.Text = "From Date : " + FromDate
        Else
            FromDate = txtFromDate.Text
            lblFrmDate.Text = "From Date : " + FromDate
        End If

        If txtToDate.Text = "" Then
            ToDate = "1/1/3050"
            lblToDates.Text = "To Date : " + ToDate
        Else
            ToDate = txtToDate.Text
            lblToDates.Text = "To Date : " + ToDate
        End If

        mStoreID = New Guid(cmbStore.SelectedValue)
        lblStoreName.Text = "Store : " & IIf(cmbStore.SelectedIndex > 0, cmbStore.SelectedItem.Text, "All")

        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text.Trim)
            Description = Trim(txtSearch.Text.Trim)
        End If

        lblPartNo.Text = "Part No. : " & IIf(PartNo <> "", PartNo, "All")
        lblDesc.Text = "Description : " & IIf(Description <> "", Description, "All")
        If rdoBase.Checked = True Then
            value = "Base Value"
            ReportName = "Removed Item Store Balance Report (Base Value)"
        ElseIf rdoLanding.Checked = True Then
            value = "Landing Value"
            ReportName = "Removed Item Store Balance Report (Landing Value)"
        Else
            value = "Commercial Value"
            ReportName = "Removed Item Store Balance Report (Commercial Value)"
        End If
        mRemovedItemStoreBalanceSearchingCriteria = lblFrmDate.Text.Trim + ", " + lblToDates.Text.Trim + ", " + lblStoreName.Text + ", " + lblPartNo.Text.Trim + ", " + lblDesc.Text.Trim + ", " + value
    End Sub
    Private Sub SetReport(ByVal IsExcel As Boolean)
        Session("IsExcel") = IsExcel

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim objsearch As rptSearchingCriteria
        Dim rpt As rptRemovedItemStoreBalance

        SetValues()
        
        Dim ds As New dsRemovedItemStoreBalance
        myReport = New crptRemovedItemStoreBalance
        rpt = rptRemovedItemStoreBalance.GetRemovedItemStoreBalance(FromDate, ToDate, PartNo, Description, mStoreID, value, chkIsValued.Checked)

        objsearch = rptSearchingCriteria.GetSearchingCriteria(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate, ToDate, PartNo, "", "", "", "", cmbStore.SelectedItem.Text, "", "", Description, ReportName, , , "", value.Split(" ")(0).ToString & " Rate", AppSettings("Logo"))
        If rpt.Count <= 0 Then
           MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1270)
        End If
        ds.Clear()

        If IsExcel = False Then
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
        End If

        da.Fill(ds, rpt)
        da.Fill(ds, objsearch)

        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "RemovedItemStoreBalance", mRemovedItemStoreBalanceSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'Store
        mStoreList = StoreList.GetStoreList(3, "", "(All)", True)
        cmbStore.DataSource = mStoreList
        Session("mStoreList") = mStoreList
        lblStoreCount.Text = "You have " + (mStoreList.Count - 1).ToString + " Store(s) transactions rights out of total " + mStoreList.TotalStorelistCount.ToString + " Store(s)"

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack And Session("sender") = "" Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisibility2()
        upnlSerachCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        If Not IsDate(txtFromDate.Text.Trim) Then
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End If
    End Sub
    Private Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToDate.TextChanged
        If Not IsDate(txtToDate.Text.Trim) Then
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End If
    End Sub
#End Region
End Class