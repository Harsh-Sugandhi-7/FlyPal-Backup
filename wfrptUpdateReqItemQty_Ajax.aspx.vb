'Added By Vikrant On 28-Sep-2015 For All28092015

Imports System.Linq

Public Class wfrptUpdateReqItemQty_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPendingToIssueRequisitionItemList As PendingToIssueRequisitionItemList
    Public mDistinctTextListForRequisition As DistinctTextListForRequisition
    Dim FromDate, ToDate As String
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mPendingToIssueRequisitionItemList = CType(Session("mPendingToIssueRequisitionItemList"), PendingToIssueRequisitionItemList)
        mDistinctTextListForRequisition = Session("mDistinctTextListForRequisition")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPendingToIssueRequisitionItemList")
        Session.Remove("mDistinctTextListForRequisition")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfrptUpdateReqItemQty_Ajax.aspx" Then
            RemoveSession()
        End If
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32)
        If SearchIndex = 0 Then
            lblNo.Visible = False
            txtNo.Visible = False
        ElseIf SearchIndex > 0 Then
            lblNo.Visible = True
            txtNo.Visible = True
        End If
    End Sub
    Private Function AddItemsToList() As Integer
        Dim count As Integer = 0
        Dim chkBox As CheckBox
        For i As Integer = 0 To dgReqItemList.Rows.Count - 1
            chkBox = CType(dgReqItemList.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
            If chkBox.Checked Then
                count += 1
            End If
        Next
        Return count
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "UpdateQty" Then
                        Try
                            Dim chkBox As CheckBox
                            For i As Integer = 0 To dgReqItemList.Rows.Count - 1
                                chkBox = CType(dgReqItemList.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
                                If chkBox.Checked Then
                                    mPendingToIssueRequisitionItemList.UpdateReqItemIssueBalQty(New Guid(dgReqItemList.DataKeys(i).Values("ReqItemID").ToString))
                                    Dim mDetail As String = "Req. No. : " + dgReqItemList.Rows(i).Cells(3).Text + " Req. Date : " + dgReqItemList.Rows(i).Cells(4).Text + " Part No. : " + dgReqItemList.Rows(i).Cells(5).Text + " Requested Qty. : " + dgReqItemList.Rows(i).Cells(6).Text + " Enquiry Bal. Qty : " + mPendingToIssueRequisitionItemList(New Guid(dgReqItemList.DataKeys(i).Values("ReqItemID").ToString)).EnquiryBalQty.ToString + " Quotation Bal. Qty : " + mPendingToIssueRequisitionItemList(New Guid(dgReqItemList.DataKeys(i).Values("ReqItemID").ToString)).QuotationBalQty.ToString + " Order Bal. Qty : " + mPendingToIssueRequisitionItemList(New Guid(dgReqItemList.DataKeys(i).Values("ReqItemID").ToString)).OrderBalQty.ToString + " Issue Bal. Qty : " + mPendingToIssueRequisitionItemList(New Guid(dgReqItemList.DataKeys(i).Values("ReqItemID").ToString)).IssueBalQty.ToString
                                    MarkLog(Util.Action.Save, "RemovePendingRequisitionItem", mDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                                End If

                            Next
                            FindNow()
                            dgReqItemList.DataBind()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "UpdateQty" Then

                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "UpdateQty" Then
                    End If
            End Select
        End If
    End Sub
    Private Sub FindNow()
        dgReqItemList.DataSource = Nothing
        mPendingToIssueRequisitionItemList = Nothing
        mPendingToIssueRequisitionItemList = PendingToIssueRequisitionItemList.GetnPendingToIssueRequisitionItemList(txtSearchFor.Text.Trim, IIf(cmbRequisitionText.SelectedIndex > 0, cmbRequisitionText.SelectedItem.Text, ""), CInt(IIf(txtNo.Text <> "", txtNo.Text.Trim, 0)), txtFromDate.Text, txtToDate.Text)

        dgReqItemList.DataSource = mPendingToIssueRequisitionItemList
        Session("mPendingToIssueRequisitionItemList") = mPendingToIssueRequisitionItemList
        lblResult.Text = "List of Requisition Items as per criteria : " & mPendingToIssueRequisitionItemList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPendingToIssueRequisitionItemList = PendingToIssueRequisitionItemList.GetnPendingToIssueRequisitionItemList("", "", 0, FromDate, ToDate)
        Session("mPendingToIssueRequisitionItemList") = mPendingToIssueRequisitionItemList
        dgReqItemList.DataSource = mPendingToIssueRequisitionItemList

        mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)")
        cmbRequisitionText.DataSource = mDistinctTextListForRequisition
        Session("mDistinctTextListForRequisition") = mDistinctTextListForRequisition

        lblResult.Text = "List of Requisition Items as per criteria : " & mPendingToIssueRequisitionItemList.Count & " Record(s) found."

        DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            FromDate = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat")).ToString
            ToDate = Today.Date.ToString(AppSettings("DateFormat")).ToString
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
            Session("MiddleFrame") = "wfrptUpdateReqItemQty_Ajax.aspx"
            DataFieldBind()
            ControlVisibility(0)
        End If
        
    End Sub
    Private Sub btnFindNow_Click(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click
        dgReqItemList.PageIndex = 0
        FindNow()
        dgReqItemList.DataBind()
        upnlGrid.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "RemovePendingRequisitionItem", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgReqItemList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReqItemList.PageIndexChanging
        dgReqItemList.PageIndex = e.NewPageIndex
        dgReqItemList.DataSource = mPendingToIssueRequisitionItemList
        Session("mPendingToIssueRequisitionItemList") = mPendingToIssueRequisitionItemList
        dgReqItemList.DataBind()
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim NoOfRecordsSelected As Integer = AddItemsToList()
        If NoOfRecordsSelected >= 1 And NoOfRecordsSelected <= 25 Then
            MSGBoxCtrl.show("Alert!", "Selected Requisition Items will get removed from Enquiry/Quotation/Order/Issue Pending List.<BR><BR>Do you want to continue ?", "", MsgBoxStyle.YesNo, "UpdateQty")
        Else
            If NoOfRecordsSelected = 0 Then
                MSGBoxCtrl.show("Alert", "Please Select At least One Record(Max. Allowed 25)", "", MsgBoxStyle.OkOnly, "")
            ElseIf NoOfRecordsSelected > 25 Then
                MSGBoxCtrl.show("Alert", NoOfRecordsSelected & " records selected.<br>Can not Remove more than 25 records.", "", MsgBoxStyle.OkOnly, "")
            End If
        End If
    End Sub
    Private Sub cmbRequisitionText_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbRequisitionText.SelectedIndexChanged
        ControlVisibility(cmbRequisitionText.SelectedIndex)
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class