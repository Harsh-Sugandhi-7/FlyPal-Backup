Public Class wfrptExcessShortRejectedQtyRegister_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public FromDate As String
    Public ToDate As String
    Public PartNo As String
    Public Description As String = ""
    Dim EventLogDetail As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        PartNo = Session("PartNo")
        Description = Session("Description")
        PartNo = IIf(IsNothing(PartNo), "", PartNo)
        Description = IIf(IsNothing(Description), "", Description)
    End Sub
    Private Sub SetSession()
        Session("PartNo") = PartNo
        Session("Description") = Description
    End Sub
    Private Sub RemoveSession()
        Session.Remove("PartNo")
        Session.Remove("Description")
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblPartNo.Visible = True
    End Sub
    Private Sub SetValues()
        FromDate = txtFromDate.Text.ToString
        ToDate = txtToDate.Text.ToString
        lblDateRangeFrom.Text = "Date Range : " & New SmartDate(txtFromDate.Text.ToString).FormattedText & " To " & New SmartDate(txtToDate.Text.ToString).FormattedText

        If (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        PartNo = IIf(PartNo <> "" And Not IsNothing(PartNo), PartNo, "")
        Description = IIf(Description <> "" And Not IsNothing(Description), Description, "")
        Session("PartNo") = PartNo
        Session("Description") = Description
        lblPartNo.Text = "Part No./Description : " & IIf(txtSearch.Text <> "", txtSearch.Text.Trim, "")
        EventLogDetail = lblDateRangeFrom.Text + ", " + lblPartNo.Text
    End Sub
    Private Sub SetReport()
        Dim objSearch As rptSearchingCriteriaForReceipt
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mrptExcessShortRejectedQtyRegister As rptExcessShortRejectedQtyRegister
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New CSLA.Data.ObjectAdapter
        Dim SearchStrin3 As String = ""
        SetValues()
        Dim ds As New dsReceipt
        myReport = New crptExcessShortRejectedQtyRegister
        mrptExcessShortRejectedQtyRegister = rptExcessShortRejectedQtyRegister.GetExcessShortRejectedQty(FromDate, ToDate, PartNo, Description, IIf(rdoExcessQty.Checked, 1, IIf(rdoShortQty.Checked, 2, 3)))

        objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(companyID:=New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), FromDate:=FromDate, ToDate:=ToDate, InternalReceiptNo:="", ReleaseNoteNo:="", RecText:="", IssText:="", OrdText:="", RecNo:="", IssNo:="", OrdNo:="", Aircraft:="", Supplier:="", Store:="", Status:="", DCNo:="", PartNo:=PartNo, Description:=Description, InvText:="", InvNo:="", FromStore:="", Amend:="Excess/Short/Rejected Qty. Register", QuotationNo:="", IntOrderNo:="", SerialNo:="", Charge:="", SuppInvNo:="", FromInvDate:="", ToInvDate:="", WorkOrderNo:=AppSettings("Logo"), TransTypeID:=IIf(rdoExcessQty.Checked, 1, IIf(rdoShortQty.Checked, 2, 3)))

        If mrptExcessShortRejectedQtyRegister.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1339)
        End If



        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptExcessShortRejectedQtyRegister)
        da.Fill(ds, objSearch)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "FuelTypeRegister", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfrptExcessShortRejectedQtyRegister_Ajax.aspx?"
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCurrentSearchCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class
