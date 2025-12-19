Public Class wfSearchCriteriaForCoreUnitDueReport_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCoreUnitDueList As CoreUnitDueList
    Public mSupplierList As VendorList
    Dim AsOnDate, Supplier As String
    Dim EventLogID As Guid 'Added by Prashant
    Dim mCoreUnitDueReportSearchingCriteria As String = String.Empty
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mSupplierList = CType(Session("mSupplierList"), VendorList)
        mCoreUnitDueList = Session("mCoreUnitDueList")
    End Sub
    Private Sub SetSession()
        Session("mSupplierList") = mSupplierList
        Session("mCoreUnitDueList") = mCoreUnitDueList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mSupplierList")
        Session.Remove("mCoreUnitDueList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
#End Region

#Region " Helper Methods "
    Private Sub Display()
        lblSupplier1.Visible = True
        lblDateRangeFrom.Visible = True
    End Sub
    Private Sub SetValues()
        If Not IsDate(txtAsOnDate.Text.Trim) Then
            AsOnDate = ""
        Else
            AsOnDate = txtAsOnDate.Text
        End If
        lblDateRangeFrom.Text = "As On Date : " & IIf(AsOnDate <> "", New SmartDate(AsOnDate).FormattedText, "")
        Supplier = IIf(cmbSupplier.SelectedIndex > 0, cmbSupplier.SelectedItem.Text, "All")
        lblSupplier1.Text = "Supplier : " & Supplier
        mCoreUnitDueReportSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblSupplier1.Text
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsRecCumInvReg
        Dim mCompanyDetail As New CompanyDetail
        Dim objSearch As rptSearchingCriteriaForReceipt
        myReport = New crptCoreUnitDueReport

        mCoreUnitDueList = CoreUnitDueList.GetCoreUnitDueList(txtAsOnDate.Text, cmbSupplier.SelectedValue.ToString)

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), AsOnDate, "", "", "", "", "", "", "", "", "", "", Supplier, "", "", "", "", "", "", "", "", AppSettings("Logo"), "", "", "", "", "", "", "") 'Changed By Utkarsh For Report Logo.)

        If mCoreUnitDueList.Count = 0 Then
             MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1186)
        End If
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, mCoreUnitDueList)
        da.Fill(ds, objSearch)
        myReport.SetDataSource(ds)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "Core Unit Due Report", mCoreUnitDueReportSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mSupplierList = VendorList.GetVendorstList(0, "", "", "", "", "", "(All)", False, True)
        cmbSupplier.DataSource = mSupplierList
        Session("mSupplierList") = mSupplierList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            txtAsOnDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            DataFieldBind()
            If cmbSupplier.Enabled = True Then
                setFocus(cmbSupplier)
            End If
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtAsOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAsOnDate.TextChanged
        If Not IsDate(txtAsOnDate.Text.Trim) Then
            txtAsOnDate.Text = New SmartDate(Now.Date.ToString).FormattedText
        End If
    End Sub
#End Region

End Class