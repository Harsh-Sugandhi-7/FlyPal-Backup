Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfrptConsumablesAndExpendables_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mMachineNameValueList As MachineNameValueList
    Public mRequisitionListForCombo As RequisitionListForCombo
    Dim mDetailForEventLog As String = String.Empty
    Public PartNo As String = ""
    Public Description As String = ""
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mRequisitionListForCombo = Session("mRequisitionListForCombo")
        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
    Public Sub RemoveSession()
        Session.Remove("mRequisitionListForCombo")
        Session.Remove("mMachineNameValueList")
    End Sub
    Public Sub SetValues()
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If

        lblFrom.Text = "From Date : " & txtFromDate.Text
        lblTo.Text = "To Date : " & txtToDate.Text
        lblRegNo.Text = "Aircraft : " & IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, "")
        lblPart.Text = "Part : " & txtSearch.Text.Trim
        lblPRS.Text = "Requisition : " & IIf(cmbRequisitionText.SelectedIndex > 0, cmbRequisitionText.SelectedItem.Text, "")
    End Sub
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim IsDetailReport As Boolean = False
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rpt As ConsumableAndExpendableReport
        Dim mCompanyDetail As New CompanyDetail
        SetValues()

        If txtSearch.Text.Trim <> "" Then
            IsDetailReport = True
            myReport = New crptConsumableAndExpendablesPartWise
        ElseIf cmbRequisitionText.SelectedIndex > 0 Then
            myReport = New crptConsumableAndExpendablesReqWise
            IsDetailReport = True
        Else
            myReport = New crptConsumableAndExpendablesAircraftWise
            IsDetailReport = False
        End If


        rpt = ConsumableAndExpendableReport.GetList(FromDate:=txtFromDate.Text, ToDate:=txtToDate.Text, MachineID:=cmbAircraftList.SelectedValue.ToString _
                                          , ReqID:=cmbRequisitionText.SelectedValue.ToString, ItemName:=PartNo, ItemDesc:=Description, IsDetailReport:=IsDetailReport)
        If rpt.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1389)
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
               mCompanyDetail.WebSite, "Consumables & Expendables (C&E) Report", txtFromDate.Text, txtToDate.Text, IIf(cmbAircraftList.SelectedIndex > 0, cmbAircraftList.SelectedItem.Text, ""), IIf(cmbRequisitionText.SelectedIndex > 0, cmbRequisitionText.SelectedItem.Text, ""), _
               txtSearch.Text.Trim, AppSettings("Product Version"), AppSettings("SINote"), "", "", _
               rpt(rpt.Count - 1).TotalIssuedQty.ToString, rpt(rpt.Count - 1).TotalConsumedQty.ToString, AppSettings("Logo"))

        Dim ds As New dsConsumablesAndExpendables
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, rpt)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "ConsumablesAndExpendablesReport", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub ControlInVisible()
        lblFrom.Visible = False
        lblTo.Visible = False
        lblRegNo.Visible = False
        lblPart.Visible = False
        lblPRS.Visible = False
    End Sub
    Private Sub ControlVisible()
        lblFrom.Visible = True
        lblTo.Visible = True
        lblRegNo.Visible = True
        lblPart.Visible = True
        lblPRS.Visible = True
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , True, "(All)", ForInventory:=True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraftList.DataSource = mMachineNameValueList

        mRequisitionListForCombo = RequisitionListForCombo.GetRequisitionList("(All)", StartingDate:=AppSettings("StartingDateForCnEConsideration").ToString())
        Session("mRequisitionListForCombo") = mRequisitionListForCombo
        cmbRequisitionText.DataSource = mRequisitionListForCombo

        DataBind()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "DoneByAME" Then

                    End If '
            End Select
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            DataFieldBind()
            txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            ControlInVisible()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid() Then upnlValidationSummary.Update() : Exit Sub
        SetReport(False)
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not IsValid() Then upnlValidationSummary.Update() : Exit Sub
        SetReport(True)
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        SetValues()
        ControlVisible()
        upnlDisplaySearchCriteria.Update()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub txtSearch_TextChanged(sender As Object, e As System.EventArgs)
        If txtSearch.Text.Trim <> "" Then
            Dim mItem As Item
            cmbRequisitionText.ClearSelection()
            cmbRequisitionText.Enabled = False
            If (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
                PartNo = txtSearch.Text.Trim
                Description = txtSearch.Text.Trim
            ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
                PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
                Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
            End If
            mItem = Item.GetItemByName(PartNo)
            If mItem Is Nothing Then
                txtSearch.Text = ""
                cmbRequisitionText.Enabled = True
            Else
                If mItem.Name = "" Then
                    txtSearch.Text = ""
                    cmbRequisitionText.Enabled = True
                End If
            End If

        Else
            cmbRequisitionText.Enabled = True
        End If
    End Sub
    Private Sub cmbRequisitionText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbRequisitionText.SelectedIndexChanged
        If cmbRequisitionText.SelectedIndex > 0 Then
            txtSearch.Text = ""
            txtSearch.ReadOnly = True
            txtSearch.BackColor = Color.Gainsboro
        Else
            txtSearch.ReadOnly = False
            txtSearch.BackColor = Color.White
        End If
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetItemList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()


        Dim ItemList As New ItemListAutoComplete
        ItemList = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In ItemList
              Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In ItemList
                   Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

    
End Class