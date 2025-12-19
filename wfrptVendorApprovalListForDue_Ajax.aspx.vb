Public Class wfrptVendorApprovalListForDue_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mVendorApprovalListForDueCriteria As String = String.Empty
    Public mVendorList As VendorList
    Public mVendorTypeList As VendorTypeList
    Public ToDate As String
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mVendorList = CType(Session("mVendorList"), VendorList)
        mVendorTypeList = CType(Session("mVendorTypeList"), VendorTypeList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mVendorList")
        Session.Remove("mVendorTypeList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Controlvisibility(ByVal Index As Int16)
        lblVendorName.Visible = False
        lblDateRange.Visible = False
        lblVendorType.Visible = False
    End Sub
    Private Sub SetValues()
        ToDate = txtDate.Text.ToString
        lblDateRange.Text = "As On Date : " & New SmartDate(txtDate.Text.ToString).FormattedText

        lblVendorName.Text = "Vendor : " + IIf(cmbVendor.SelectedIndex > 0, cmbVendor.SelectedItem.Text, "ALL")
        lblVendorType.Text = "Vendor Type : " + IIf(cmbVendorType.SelectedIndex > 0, cmbVendorType.SelectedItem.Text, "ALL")
        mVendorApprovalListForDueCriteria = lblDateRange.Text.Trim + ", " + lblVendorName.Text.Trim + ", " + lblVendorType.Text.Trim
    End Sub
    Private Sub SetReport()
        SetValues()
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsVendorApprovalListForDue
        Dim mVendorApprovalListForDue As VendorApprovalListForDue

        myReport = New crptVendorApprovalListForDue
        mVendorApprovalListForDue = VendorApprovalListForDue.GetVendorApprovalListForDue(ToDate,
                                                                                         cmbVendor.SelectedValue.ToString,
                                                                                         cmbVendorType.SelectedValue.ToString,
                                                                                         IsWithAuditDue:=chkWithWithoutDocumentApproval.Checked)
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
               mCompanyDetail.WebSite,
               ReportName:=IIf(chkWithWithoutDocumentApproval.Checked = True, "Vendor Document Approval Due", "Approved Provider List"),
               New SmartDate(ToDate).FormattedText,
               cmbVendor.SelectedItem.Text, cmbVendorType.SelectedItem.Text,
               SearchStr4:=chkWithWithoutDocumentApproval.Checked, SearchStr5:="",
               AppSettings("Product Version"), AppSettings("SINote"),
               SearchStr6:="", "", "",
               "", AppSettings("Logo"))
        If mVendorApprovalListForDue.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1301)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mVendorApprovalListForDue)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "VendorApprovalListForDue", mVendorApprovalListForDueCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendorstList(0, , , , , , SelectTag:="(ALL)", IsCustomer:=True, IsSupplier:=True, IsServiceProvider:=True)
        cmbVendor.DataSource = mVendorList
        Session("mVendorList") = mVendorList

        mVendorTypeList = VendorTypeList.GetVendorTypeList("(ALL)")
        cmbVendorType.DataSource = mVendorTypeList
        Session("mVendorTypeList") = mVendorTypeList

        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 
        If Not IsPostBack Then
            txtDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            DataFieldBind()
            Controlvisibility(2)
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        lblDateRange.Visible = True
        lblVendorName.Visible = True
        lblVendorType.Visible = True
        SetValues()
        upnlSelection.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        SetReport()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mVendorList = Nothing
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class